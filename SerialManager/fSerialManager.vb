'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.IO

Imports HelperFunctions
Imports DebugWindow

' the serial manager form
' event driven serial IO
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fSerialManager

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Message
        COM_RECV
        COM_RECV_ERROR
        COM_SEND
        COM_SEND_ERROR
        COM_OPEN_OK
        COM_OPEN_FAIL
        COM_CLOSE
    End Enum

    ' Delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub SerialEventCallback(ByVal eventCode As Integer, ByVal messageContent As String)

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    Private eventCallbackDelegate() As SerialEventCallback
    Private invalidVmcNameCharactors As String = "#?/'!£$%^&*()"
    Private messageBuffer As String = ""
    Private portConnected As Boolean = False
    Private suspendTransmission As Boolean = False
    Private removeControlCharsRequired As Boolean = True
    Private binaryOnly As Boolean = False

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        helperFunctions = helperFunctionsFactory.GetManager
        debugInformation = debugInformationFactory.GetManager

    End Sub

    Public Function SendMessageBin(ByVal binaryBuffer() As Byte, ByVal bufferLength As Integer) As Boolean

        SerialPort.Write(binaryBuffer, 0, bufferLength)

    End Function

    ' SendMessage
    ' queue a message to be sent
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function SendMessage(ByVal rawMessage As String) As Boolean

        Dim message As String = RemoveControlChars(rawMessage)
        Dim displayMessage As Boolean = True
        Dim checkSum As String
        Dim result As Boolean = False

        If binaryOnly Then
            Return False
        End If

        ' should we show this in the progress box
        If rawMessage.Length >= 9 AndAlso rawMessage.Substring(0, 9) = "GETHEALTH" Then
            displayMessage = GetHealthShowRadio.Checked
        End If

        If rawMessage.Length >= 9 AndAlso rawMessage.Substring(0, 9) = "GETWANDER" Then
            displayMessage = WanderShowRadio.Checked
        End If

        If rawMessage.Length >= 11 AndAlso rawMessage.Substring(0, 11) = "GETSWITCHES" Then
            displayMessage = GetSwitchesShowRadio.Checked
        End If

        If rawMessage.Length >= 11 AndAlso rawMessage.Substring(0, 11) = "GETCOUNTERS" Then
            displayMessage = GetCountersShowRadio.Checked
        End If

        If rawMessage.Length >= 7 AndAlso rawMessage.Substring(0, 7) = "ARMDRV " Then
            displayMessage = ArmdrvShowRadio.Checked
        End If

        If rawMessage.Length >= 8 AndAlso rawMessage.Substring(0, 8) = "MECHDRV " Then
            displayMessage = MechdrvShowRadio.Checked
        End If

        If rawMessage.Length >= 4 AndAlso rawMessage.Substring(0, 4) = "MDB " Then

            If MdbIgnoreNoneRadio.Checked Then
                displayMessage = True

            ElseIf MdbIgnoreAllRadio.Checked Then
                displayMessage = False

            ElseIf MdbIgnorePollRadio.Checked AndAlso rawMessage = "MDB 12" Then
                displayMessage = False

            End If

        End If

        If SerialPort.IsOpen Then

            If suspendTransmission Then

                helperFunctions.AddToListBox(ProgressListBox, ">" & ControlChars.Tab & message & "-(not sent)")

            Else

                checkSum = GenerateHexChecksum(message)

                If displayMessage Then
                    helperFunctions.AddToListBox(ProgressListBox, ">" & ControlChars.Tab & message & "-" & checkSum)
                    debugInformation.Progress(fDebugWindow.Level.INF, 2300, rawMessage, True)
                End If

                Try

                    SerialPort.Write(message & GenerateHexChecksum(message) & Chr(10) & Chr(13))
                    SendApplicationMessage(fSerialManager.Message.COM_SEND, message & "-" & checkSum)

                    result = True

                Catch ex As Exception
                    helperFunctions.AddToListBox(ProgressListBox, "   " & ex.ToString)
                End Try

            End If

        Else
            SendApplicationMessage(fSerialManager.Message.COM_SEND_ERROR, rawMessage)
        End If
        '      End If

        Return result

    End Function

    Public Sub SetBinaryOnly(ByVal newState As Boolean)
        binaryOnly = newState
    End Sub

    Public Sub RemoveSuspension()
        suspendTransmission = False
    End Sub

    Public Sub CleanupInput(ByVal newState As Boolean)
        removeControlCharsRequired = newState
    End Sub

    ' IsConnected
    ' is the VMC serial port connected ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function IsConnected() As Boolean

        Return SerialPort.IsOpen

    End Function

    ' GenerateHexChecksum
    ' create a 2 charactor checksum from a given text string
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function GenerateHexChecksum(ByVal message) As String

        Dim Index As Integer
        Dim checksum As Integer = 0

        ' build up a checksum
        checksum = 0

        For Index = 0 To message.Length - 1
            checksum = (Asc(Mid(message, Index + 1, 1)) + checksum) Mod 256
        Next

        Return Hex$(Int(checksum / 16)) & Hex$(checksum Mod 16)

    End Function

    ' RemoveControlChars
    ' strip any low end control charactors
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function RemoveControlChars(ByVal inString As String) As String

        Dim outString As String = ""
        Dim index As Integer
        Dim charBlock As String

        For index = 0 To inString.Length - 1
            charBlock = inString.Substring(index, 1)
            If Asc(charBlock) >= 32 Then
                outString = outString & charBlock
            End If
        Next

        Return outString
    End Function

    ' AddCallback
    ' record which function needs to know about incomming data..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub AddCallback(ByVal dataCallbackFunction As SerialEventCallback)

        Dim currentNumberOfCallbacks As Integer

        If IsNothing(eventCallbackDelegate) Then
            Array.Resize(eventCallbackDelegate, 1)
            eventCallbackDelegate(0) = dataCallbackFunction
        Else
            currentNumberOfCallbacks = eventCallbackDelegate.Length
            Array.Resize(eventCallbackDelegate, currentNumberOfCallbacks + 1)
            eventCallbackDelegate(currentNumberOfCallbacks) = dataCallbackFunction
        End If

    End Sub

    ' SendApplicationMessage
    ' notify the application that something has happenned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendApplicationMessage(ByVal eventCode As Integer, ByVal stringValue As String)

        If Not IsNothing(eventCallbackDelegate) Then
            For Each callbackDelegate As SerialEventCallback In eventCallbackDelegate
                If Not IsNothing(callbackDelegate) Then
                    callbackDelegate.Invoke(eventCode, stringValue)
                End If
            Next
        End If

    End Sub

    ' DataRecieved
    ' bytes have been recieved on the serial port
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DataRecieved(ByVal sender As Object, ByVal e As IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort.DataReceived

        Dim messageBlock As String = ""
        Dim completeMessage As String
        Dim messageRemnant As String
        Dim terminationPosition As Integer
        Dim messageChecksum As String = ""
        Dim messageContent As String = ""
        Dim displayProgress As Boolean

        Try

            messageBlock = SerialPort.ReadExisting()

        Catch ex As Exception
            '   helperFunctions.AddToListBox(ProgressListBox, "   " & ex.ToString)
        End Try

        messageBuffer = messageBuffer & messageBlock

        terminationPosition = messageBuffer.IndexOf(Chr(13))

        While terminationPosition <> -1 And portConnected

            ' shuffle the queue
            If removeControlCharsRequired Then
                completeMessage = RemoveControlChars(Trim(messageBuffer.Substring(0, terminationPosition)))

            Else
                completeMessage = messageBuffer.Substring(0, terminationPosition)
            End If

            completeMessage = RemoveControlChars(Trim(messageBuffer.Substring(0, terminationPosition)))
            messageRemnant = messageBuffer.Substring(terminationPosition + 1)
            messageBuffer = messageRemnant

            ' should we display this in our progress list, some messages are excluded from progress due to high traffic
            displayProgress = True

            If completeMessage.Length >= 4 AndAlso completeMessage.Substring(0, 4) = "ADC=" Then
                displayProgress = AdcShowRadio.Checked
            End If

            If completeMessage.Length >= 9 AndAlso completeMessage.Substring(0, 9) = "SWITCHES=" Then
                displayProgress = GetSwitchesShowRadio.Checked
            End If

            If completeMessage.Length >= 7 AndAlso completeMessage.Substring(0, 7) = "WANDER=" Then
                displayProgress = WanderShowRadio.Checked
            End If

            If completeMessage.Length >= 7 AndAlso completeMessage.Substring(0, 7) = "HEALTH=" Then
                displayProgress = GetHealthShowRadio.Checked
            End If

            If completeMessage.Length >= 9 AndAlso completeMessage.Substring(0, 9) = "SWITCHES=" Then
                displayProgress = GetSwitchesShowRadio.Checked
            End If

            If completeMessage.Length >= 9 AndAlso completeMessage.Substring(0, 9) = "COUNTERS=" Then
                displayProgress = GetCountersShowRadio.Checked
            End If

            If completeMessage.Length >= 4 AndAlso completeMessage.Substring(0, 4) = "MDB=" Then

                If MdbIgnoreAllRadio.Checked Then
                    displayProgress = False
                ElseIf MdbIgnoreNoneRadio.Checked Then
                    displayProgress = True
                ElseIf MdbIgnorePollRadio.Checked AndAlso completeMessage = "MDB=" Then
                    displayProgress = False
                Else
                End If

            End If

            ' if required, add to the progress box.
            If displayProgress Then
                debugInformation.Progress(fDebugWindow.Level.INF, 2301, completeMessage, True)
                helperFunctions.AddToListBox(ProgressListBox, "!" & ControlChars.Tab & completeMessage)

            End If

            ' suspend the transmission on start up sequence
            If completeMessage = "INITIALIZING" Then
                suspendTransmission = True

            ElseIf completeMessage = "READY" Then
                suspendTransmission = False

            End If



            ' tell the application what is going on.
            SendApplicationMessage(fSerialManager.Message.COM_RECV, completeMessage)

            ' look for another message
            terminationPosition = messageBuffer.IndexOf(Chr(13))

            ' release the thread, good manners. 
            ' Threading.Thread.Sleep(20)

        End While

    End Sub

    ' WindowIsVisible
    ' return the show state
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function WindowIsVisible() As Boolean
        Return Visible
    End Function

    ' Connect, Disconnect
    ' connect/disconnect to the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Connect(ByVal portName As String) As Boolean

        Dim connectionResult As Boolean = False

        Debug.WriteLine("Connecting port:" & portName)
        Try

            SerialPort.Handshake = IO.Ports.Handshake.None

            SerialPort.PortName = portName
            SerialPort.StopBits = IO.Ports.StopBits.One
            SerialPort.Open()
            portConnected = True
            connectionResult = True

            SendApplicationMessage(fSerialManager.Message.COM_OPEN_OK, "")

        Catch ex As Exception

            SendApplicationMessage(fSerialManager.Message.COM_OPEN_FAIL, "")
            connectionResult = False
        End Try

        Return connectionResult

    End Function

    Public Function SetBaud(ByVal newSpeed As Integer) As Boolean

        Dim setBaudResult As Boolean = False

        Try
            SerialPort.BaudRate = newSpeed
            setBaudResult = True
        Catch ex As Exception
        End Try

        Return setBaudResult

    End Function

    Public Function Disconnect() As Boolean

        Dim connectionResult As Boolean = False

        Try

            If SerialPort.IsOpen Then

                SerialPort.Close()

                connectionResult = True
                portConnected = False

                SendApplicationMessage(fSerialManager.Message.COM_CLOSE, "")

            End If

        Catch ex As Exception
        End Try

        Return connectionResult

    End Function

    ' SerialManagerForm_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialManagerForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

    ' CommandLineTextChanged
    ' trap the enter key being pressed
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub CommandLineTextChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CommandLineTextBox.KeyPress

        If Asc(e.KeyChar) = Keys.Enter Then

            SendMessage(CommandLineTextBox.Text)
            e.Handled = True
            CommandLineTextBox.Text = ""
        End If

    End Sub


    ' HideButton_Click & ClearButton_Click
    ' deal with the button presses
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

    Private Sub ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearButton.Click
        helperFunctions.ClearListBox(ProgressListBox)
    End Sub

    ' ShowAllButton_Click & DefaultButton_Click
    ' messages to display or hide in the serial manager
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ShowAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowAllButton.Click
        helperFunctions.SetRadioChecked(AdcShowRadio)
        helperFunctions.SetRadioChecked(GetSwitchesShowRadio)
        helperFunctions.SetRadioChecked(GetHealthShowRadio)
        helperFunctions.SetRadioChecked(ArmdrvShowRadio)
        helperFunctions.SetRadioChecked(MechdrvShowRadio)
        helperFunctions.SetRadioChecked(MdbIgnoreNoneRadio)
        helperFunctions.SetRadioChecked(WanderShowRadio)
        helperFunctions.SetRadioChecked(GetCountersShowRadio)

    End Sub

    Private Sub DefaultButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefaultButton.Click
        helperFunctions.SetRadioChecked(AdcHideRadio)
        helperFunctions.SetRadioChecked(GetSwitchesHideRadio)
        helperFunctions.SetRadioChecked(GetHealthHideRadio)
        helperFunctions.SetRadioChecked(ArmdrvHideRadio)
        helperFunctions.SetRadioChecked(MechdrvHideRadio)
        helperFunctions.SetRadioChecked(MdbIgnorePollRadio)
        helperFunctions.SetRadioChecked(WanderHideRadio)
        helperFunctions.SetRadioChecked(GetCountersHideRadio)

    End Sub

End Class

' class cSerialManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cSerialManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared serialManager As fSerialManager = Nothing

    Public Function GetManager() As fSerialManager

        If IsNothing(serialManager) Then

            serialManager = New fSerialManager
            serialManager.Initialise()

        End If

        Return serialManager

    End Function

End Class

