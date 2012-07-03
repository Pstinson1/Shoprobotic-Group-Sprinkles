'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports System
Imports System.Net

Public Class cSocketListener

    ' enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Message
        SKT_CONNECT
        SKT_REFUSED
        SKT_DISCONNECT
        SKT_READ
    End Enum

    ' delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub SocketEventCallback(ByVal eventCode As Message, ByVal socketReporting As StateObject, ByVal messageToSend As String)

    ' constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Const MAXIMUM_CONNECTIONS = 50

    ' variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private data As String = Nothing
    Private continueListening As Boolean = True
    Private eventCallback As SocketEventCallback = Nothing
    Private managementThread As Thread
    Private listeningSocket As Socket
    Private connectionObject(MAXIMUM_CONNECTIONS) As StateObject
    Private allDone As New ManualResetEvent(False)                                           ' thread signal.
    Private availableAddressList() As IPAddress = Nothing

    ' AvailableAddresses
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function AvailableAddresses() As IPAddress()

        Dim hostName As String = System.Net.Dns.GetHostName
        Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(hostName)
        Dim addressIndex As Integer = 0

        ReDim availableAddressList(ipHostInfo.AddressList.Length - 1)

        For Each singleAddress As IPAddress In ipHostInfo.AddressList
            availableAddressList(addressIndex) = ipHostInfo.AddressList(addressIndex)
            addressIndex = addressIndex + 1
        Next

        Return availableAddressList

    End Function

    ' Start
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Start(ByVal listeningPort As Integer, ByVal eventCallback As SocketEventCallback) As Boolean

        Dim connectionIndex As Integer
        Dim listenerStartResult As Boolean = False

        ' establish the local endpoint for the socket.
        Dim hostName As String = System.Net.Dns.GetHostName
        Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(hostName)
        Dim ipAddress As IPAddress = ipHostInfo.AddressList(0)
        Dim localEndPoint As New IPEndPoint(ipAddress, listeningPort)

        ' remember where to post messages to
        Me.eventCallback = eventCallback

        ' create containers for MAX_CONNECTIONS
        For connectionIndex = 0 To MAXIMUM_CONNECTIONS - 1

            connectionObject(connectionIndex) = New StateObject
            connectionObject(connectionIndex).identifier = connectionIndex
            connectionObject(connectionIndex).inUse = False
            connectionObject(connectionIndex).messageBuffer = ""
        Next

        ' intializes a TCP/IP socket.
        listeningSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        Try

            ' bind it to the local address and start listening
            listeningSocket.Bind(localEndPoint)
            listeningSocket.Listen(100)

            Console.WriteLine("Bound and listening")

            ' success
            listenerStartResult = True

        Catch e As Exception
            Console.WriteLine(e.ToString())
        End Try

        continueListening = True

        ' monitor the listener for incomming connections in a managing thread
        managementThread = New Thread(AddressOf SocketManagementThread)
        managementThread.Name = "Socket Listener"
        managementThread.Start()

        Return listenerStartResult

    End Function

    Public Sub Shutdown()

        Dim connectionIndex As Integer

        continueListening = False
        allDone.Set()

        For connectionIndex = 0 To MAXIMUM_CONNECTIONS - 1

            If Not connectionObject(connectionIndex) Is Nothing AndAlso connectionObject(connectionIndex).inUse Then
                connectionObject(connectionIndex).workSocket.BeginDisconnect(True, AddressOf DisconnectCallback, connectionObject(connectionIndex))
            End If
        Next

        If Not listeningSocket Is Nothing Then
            listeningSocket.Close()
        End If

    End Sub


    Public Sub Broadcast(ByVal messageText As String)

        Dim connectionIndex As Integer

        For connectionIndex = 0 To MAXIMUM_CONNECTIONS - 1

            If Not connectionObject(connectionIndex) Is Nothing AndAlso connectionObject(connectionIndex).inUse Then

                Send(connectionObject(connectionIndex), messageText)

            End If

        Next


    End Sub

    Public Sub SocketManagementThread()

        ' Data buffer for incoming data.
        Dim bytes() As Byte = New [Byte](1024) {}

        While continueListening

            Try
                ' set the event to nonsignaled state.
                allDone.Reset()

                ' start an asynchronous socket to listen for connections.
                listeningSocket.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), listeningSocket)

                ' Wait until a connection is made before continuing.
                allDone.WaitOne()

            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try

        End While

        Try
            Console.WriteLine("Listener closed")
            Console.Read()
        Catch ex As Exception
        End Try

    End Sub

    Private Sub DisconnectCallback(ByVal ar As IAsyncResult)

        Dim connectionObject As StateObject = CType(ar.AsyncState, StateObject)

        If connectionObject Is Nothing Then

            Console.WriteLine("Non- reference socket has been closed.")
        Else

            connectionObject.inUse = False
            Console.WriteLine(connectionObject.identifier & " marked as not in use")
        End If

    End Sub

    ' SendApplicationMessage
    ' notify the application that something has happenned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendApplicationMessage(ByVal eventCode As Message, ByVal socketReporting As StateObject, ByVal messageToSend As String)

        If Not eventCallback Is Nothing Then

            eventCallback.Invoke(eventCode, socketReporting, messageToSend)
        End If


    End Sub

    Private Sub AcceptCallback(ByVal ar As IAsyncResult)

        ' Get the socket that handles the client request.
        Dim listener As Socket = CType(ar.AsyncState, Socket)

        If continueListening Then

            Dim incommingSocket As Socket = listener.EndAccept(ar)

            incommingSocket.ReceiveBufferSize = 1024
            incommingSocket.Blocking = False

            ' find an available connection object
            Dim state As StateObject = FreeConnectionObject()

            ' Signal the main thread to continue.
            allDone.Set()

            If state Is Nothing Then
                SendApplicationMessage(Message.SKT_REFUSED, Nothing, "")
                incommingSocket.BeginDisconnect(True, AddressOf DisconnectCallback, Nothing)
            Else

                '  Dim state As New StateObject()

                state.workSocket = incommingSocket
                state.remoteAddress = incommingSocket.RemoteEndPoint
                state.inUse = True
                state.messageBuffer = ""

                '   Send(state.workSocket, "Connection Accepted you will recieve all debug messages." & ControlChars.CrLf)
                Send(state, "Connection Accepted you will recieve all debug messages." & ControlChars.CrLf)
                If continueListening Then
                    incommingSocket.BeginReceive(state.byteBuffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
                End If
                SendApplicationMessage(Message.SKT_CONNECT, state, "")

                End If

        End If


    End Sub

    Private Function FreeConnectionObject() As StateObject

        Dim connectionIndex As Integer = 0
        Dim returnedStateObject As StateObject = Nothing

        While returnedStateObject Is Nothing And connectionIndex < MAXIMUM_CONNECTIONS

            If Not connectionObject(connectionIndex).inUse Then
                returnedStateObject = connectionObject(connectionIndex)

            End If

            connectionIndex = connectionIndex + 1

        End While

        Return returnedStateObject

    End Function

    Private Sub ReadCallback(ByVal ar As IAsyncResult)

        Dim terminationPosition As Integer
        Dim completeMessage As String
        Dim messageRemnant As String

        ' retrieve the state object and the handler socket from the asynchronous state object.
        Dim state As StateObject = CType(ar.AsyncState, StateObject)
        Dim handler As Socket = state.workSocket

        Try
            ' read data from client socket. 
            Dim bytesRead As Integer = handler.EndReceive(ar)

            If bytesRead = 0 Then

                If state.inUse Then
                    SendApplicationMessage(Message.SKT_DISCONNECT, state, "")
                    state.inUse = False
                End If

            Else

                ' append the incomming data to the message buffer
                state.messageBuffer = state.messageBuffer & Encoding.ASCII.GetString(state.byteBuffer, 0, bytesRead)

                ' check for end-of-line tag. If it is not there, read more data.
                terminationPosition = state.messageBuffer.IndexOf(Chr(13))

                While terminationPosition > -1

                    completeMessage = state.messageBuffer.Substring(0, terminationPosition)
                    messageRemnant = state.messageBuffer.Substring(terminationPosition + 1)
                    state.messageBuffer = messageRemnant

                    SendApplicationMessage(Message.SKT_READ, state, completeMessage)

                    terminationPosition = state.messageBuffer.IndexOf(Chr(13))

                End While

                ' look for more data
                handler.BeginReceive(state.byteBuffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
            End If

        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

    End Sub

    Public Sub Send(ByVal handler As StateObject, ByVal data As [String])

        If continueListening = False Then
            Return
        End If

        ' convert the string data to byte data using ASCII encoding.
        Dim byteData As Byte() = Encoding.ASCII.GetBytes(data)

        ' Begin sending the data to the remote device.
        If continueListening Then
            handler.workSocket.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), handler)
        End If

    End Sub

    Private Sub SendCallback(ByVal ar As IAsyncResult)

        Dim handler As StateObject
        Dim bytesSent As Integer

        handler = CType(ar.AsyncState, StateObject)             ' retrieve the state object

        Try
            bytesSent = handler.workSocket.EndSend(ar)                     ' complete sending the data to the remote device.

        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try




        Try

            If continueListening Then
                handler.workSocket.BeginReceive(handler.byteBuffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), handler)
            End If

        Catch ex As Exception
            Console.WriteLine("workSocket.ReceiveBufferSize" & handler.workSocket.ReceiveBufferSize.ToString)
            Console.WriteLine(ex.ToString())
        End Try

    End Sub

End Class

' State object for reading client data asynchronously
Public Class StateObject

    Public Const BufferSize As Integer = 1024               ' size of receive buffer.

    Public workSocket As Socket = Nothing                     ' client  socket.
    Public byteBuffer(BufferSize) As Byte                       ' receive buffer.
    Public messageBuffer As String
    Public remoteAddress As EndPoint
    Public inUse As Boolean
    Public identifier As Integer

End Class
