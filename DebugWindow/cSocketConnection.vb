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

Public Class cSocketConnection

    Public Enum Message
        SKT_CONNECT
        SKT_DISCONNECT
        SKT_READ
        SKT_SENT
    End Enum

    Public Delegate Sub SocketEventCallback(ByVal eventCode As Message, ByVal messageToSend As String)

    Public Const BufferSize As Integer = 1024               ' size of receive buffer.

    Private workSocket As Socket
    Private eventCallback As SocketEventCallback = Nothing
    Private byteBuffer(BufferSize) As Byte                       ' receive buffer.
    Private messageBuffer As String

    Public Sub New()

        ' intializes a TCP/IP socket.
        workSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

    End Sub

    Public Sub Connect(ByVal ipAddressToConnect As String, ByVal portNumber As Integer, ByVal eventCallback As SocketEventCallback)

        If Not ipAddressToConnect Is Nothing Then

            Dim host As IPAddress = IPAddress.Parse(ipAddressToConnect)
            Dim ipEndpoint As IPEndPoint = New IPEndPoint(host, portNumber) ' Assign port and host

            Me.eventCallback = eventCallback

            Try
                workSocket.BeginConnect(ipEndpoint, New AsyncCallback(AddressOf ConnectCallback), Nothing)

            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try

        End If

    End Sub

    Public Sub Disconnect()
        Try
            If workSocket.Connected Then
                workSocket.Shutdown(SocketShutdown.Both)
                '      workSocket.Disconnect(True)
            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub ConnectCallback(ByVal ar As IAsyncResult)

        SendApplicationMessage(Message.SKT_CONNECT, "")
        messageBuffer = ""

        Try
            workSocket.BeginReceive(byteBuffer, 0, BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), Nothing)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub ReadCallback(ByVal ar As IAsyncResult)

        Dim terminationPosition As Integer
        Dim completeMessage As String
        Dim messageRemnant As String

        ' read data from client socket. 
        Try
            Dim bytesRead As Integer = workSocket.EndReceive(ar)

            If bytesRead = 0 Then

                SendApplicationMessage(Message.SKT_DISCONNECT, "")

            Else

                ' append the incomming data to the message buffer
                messageBuffer = messageBuffer & Encoding.ASCII.GetString(byteBuffer, 0, bytesRead)

                ' check for end-of-line tag. If it is not there, read more data.
                terminationPosition = messageBuffer.IndexOf(Chr(13))

                While terminationPosition > -1

                    completeMessage = messageBuffer.Substring(0, terminationPosition)
                    messageRemnant = messageBuffer.Substring(terminationPosition + 1)
                    messageBuffer = messageRemnant

                    SendApplicationMessage(Message.SKT_READ, completeMessage)

                    terminationPosition = messageBuffer.IndexOf(Chr(13))

                End While

                ' look for more data
                workSocket.BeginReceive(byteBuffer, 0, BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), Nothing)
            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Function Send(ByVal messageToSend As String) As Boolean

        ' convert the string data to byte data using ASCII encoding.
        Dim byteData As Byte() = Encoding.ASCII.GetBytes(messageToSend)

        ' Begin sending the data to the remote device.
        Try
            workSocket.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), Nothing)
        Catch ex As Exception
        End Try

    End Function

    Private Sub SendCallback(ByVal ar As IAsyncResult)

        Try

            Dim bytesSent As Integer = workSocket.EndSend(ar)                     ' complete sending the data to the remote device.

            '    Console.WriteLine("Sent {0} bytes to client.", bytesSent)

            workSocket.BeginReceive(byteBuffer, 0, BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), Nothing)
            SendApplicationMessage(Message.SKT_SENT, "")

        Catch e As Exception
            Console.WriteLine(e.ToString())
        End Try

    End Sub



    ' SendApplicationMessage
    ' notify the application that something has happenned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendApplicationMessage(ByVal eventCode As Message, ByVal messageToSend As String)

        If Not eventCallback Is Nothing Then

            eventCallback.Invoke(eventCode, messageToSend)
        End If

    End Sub

End Class
