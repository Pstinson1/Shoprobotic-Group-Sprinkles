'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.IO
Imports System.Threading

Imports SerialManager
Imports HelperFunctions
Imports SettingsManager

Public Class uSerialLoader

    Private defaultFirmwareFolder As String
    Private selectedFirmwarePath As String = ""
    Private inBootloaderSerialMode As Boolean = False
    Private mc_sendbuf() As Byte = Nothing
    Private mc_sendbuf_pos As Integer = 0
    Private mc_sendbuf_checksum As Integer = 0
    Private Last_Data_Sent() As Byte = Nothing
    Private captured = False

    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private managementThread As Thread
    Private continueRunning As Boolean = True

    ' control commands
    Const CP_Transparent = 1                           ' set Transparent
    Const CP_Invalidate = 2                               ' Invalidate User Program Area
    Const CP_LDR_Mode = 3                              ' Put Target into Loader State
    Const CP_New_Code = 4                              ' New code to store
    Const CP_Dump = 5                                      ' Dump user memory and EEPROM
    Const CP_Erase = 6                                      ' Erase Program and EEPROM memory
    Const CP_Erase_PGM = 7                             ' Erase Program and EEPROM memory
    Const CP_Erase_EE = 8                                ' Erase Program and EEPROM memory
    Const CP_Reset = 9                                      ' Reboot

    Const DEFAULT_BUFFER_LENGTH = 256

    Public Sub Initialise()

        ' grab the managers
        helperFunctions = helperFunctionsFactory.GetManager
        settingsManager = settingsManagerFactory.GetManager
        serialManager = serialManagerFactory.GetManager

        ' direct serial io to this class
        serialManager.AddCallback(AddressOf SerialPortEvent)

        ' determime where we are going to store the firmware files
        defaultFirmwareFolder = System.Windows.Forms.Application.StartupPath & "\VMC Firmware\"
        selectedFirmwarePath = settingsManager.GetValue("LastVmcFileSelected")

        helperFunctions.SetLabelText(FilePathLabel, selectedFirmwarePath)

        ' ensure that the logging folder exists
        If Not Directory.Exists(defaultFirmwareFolder) Then
            Directory.CreateDirectory(defaultFirmwareFolder)
        End If

        ReDim mc_sendbuf(DEFAULT_BUFFER_LENGTH)
        ReDim Last_Data_Sent(DEFAULT_BUFFER_LENGTH)

    End Sub

    Private Sub FileSelectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileSelectButton.Click

        OpenHexFileDialog.InitialDirectory = defaultFirmwareFolder
        OpenHexFileDialog.FileName = settingsManager.GetValue("LastVmcFileSelected")

        If OpenHexFileDialog.ShowDialog() Then
            selectedFirmwarePath = OpenHexFileDialog.FileName
            helperFunctions.SetLabelText(FilePathLabel, selectedFirmwarePath)
            settingsManager.SetValue("LastVmcFileSelected", selectedFirmwarePath)
        End If

    End Sub


    Private Sub StartCommsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartCommsButton.Click

        inBootloaderSerialMode = True
        captured = False

        ' up the speed on the serial port
        serialManager.Disconnect()
        serialManager.SetBaud(115200)
        serialManager.CleanupInput(False)
        serialManager.Connect(settingsManager.GetValue("VmcSerialPort"))

        serialManager.SetBinaryOnly(True)

        continueRunning = True
        managementThread = New Thread(AddressOf ManagementFunction)
        managementThread.Name = ""
        managementThread.Start()

    End Sub

    Private Sub StopCommsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopCommsButton.Click

        serialManager.Disconnect()
        serialManager.SetBaud(9600)
        serialManager.CleanupInput(True)
        serialManager.Connect(settingsManager.GetValue("VmcSerialPort"))
        continueRunning = False
        inBootloaderSerialMode = False
    End Sub

    Dim inputBuffer As String = ""

    Public Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        Dim payloadType As String
        Dim deviceId As String
        Dim command As String
        Dim status As String
        Dim count As String
        Dim checksum As String

        Dim commandId As Integer

        If Not inBootloaderSerialMode Then
            Return
        End If

        Select Case eventCode

            Case fSerialManager.Message.COM_RECV

                helperFunctions.AddToListBox(ProgressList, messageContent)

                payloadType = messageContent.Substring(0, 1)

                Select Case payloadType

                    Case "!"

                        deviceId = messageContent.Substring(1, 4)
                        command = messageContent.Substring(5, 2)
                        status = messageContent.Substring(7, 4)
                        count = messageContent.Substring(11, 4)
                        checksum = messageContent.Substring(15, 2)

                        helperFunctions.SetLabelText(DeviceIdLabel, "Device Id: " & deviceId)
                        helperFunctions.SetLabelText(DeviceStatusLabel, "Status: " & status)

                    Case Else





                End Select






                helperFunctions.HexStringToInteger(messageContent.Substring(0, 2), commandId)

                If messageContent.Length < 17 Then
                    helperFunctions.AddToListBox(ProgressList, "Response record too short -->" + messageContent)
                End If

        End Select

    End Sub

    Private Sub Send_Command(ByVal target As Integer, ByVal cmd As Integer)

        Dim byteIndex As Integer
        Dim byteOutput As String = "> "

        BuildPrototype(target)
        mc_sendbuf_insert_len(1)
        mc_sendbuf_add_byte(9)                   '  type 9 Commands
        mc_sendbuf_add_byte(cmd)
        mc_sendbuf_add_chk()
        mc_sendbuf_add_crlf()

        Buffer.BlockCopy(mc_sendbuf, 0, Last_Data_Sent, 0, 13)

        For byteIndex = 0 To mc_sendbuf.Length - 1

            byteOutput = byteOutput & mc_sendbuf(byteIndex).ToString("X2") & " "

        Next

        helperFunctions.AddToListBox(ProgressList, byteOutput)
        serialManager.SendMessageBin(mc_sendbuf, mc_sendbuf_pos)

    End Sub

    Private Sub BuildPrototype(ByVal target As Integer)

        mc_sendbuf_pos = 0
        mc_sendbuf_checksum = 0

        ' initialise the field
        mc_sendbuf(0) = Asc(":")
        mc_sendbuf_pos = mc_sendbuf_pos + 1

        ' null the payload length field
        mc_sendbuf_add_byte(0)

        mc_sendbuf_add_byte(target / &H100)
        mc_sendbuf_add_byte(target Mod &H100)

    End Sub

    Private Sub mc_sendbuf_insert_len(ByVal data_length As Integer)

        Dim str As String = Convert.ToInt32(data_length).ToString("X2")

        ' get the sequence number for this PDU
        mc_sendbuf_checksum = mc_sendbuf_checksum + data_length

        mc_sendbuf(1) = str.Substring(0, 1)
        mc_sendbuf(2) = str.Substring(1, 1)

    End Sub

    Private Sub mc_sendbuf_add_byte(ByVal next_byte As Byte)

        Dim str As String = next_byte.ToString("X2")

        mc_sendbuf_checksum = mc_sendbuf_checksum + next_byte

        mc_sendbuf(mc_sendbuf_pos) = str.Substring(0, 1)
        mc_sendbuf_pos = mc_sendbuf_pos + 1
        mc_sendbuf(mc_sendbuf_pos) = str.Substring(1, 1)
        mc_sendbuf_pos = mc_sendbuf_pos + 1

    End Sub

    Private Sub mc_sendbuf_add_chk()

        Dim twosComplement As Char = Chr((&H100 - mc_sendbuf_checksum) + 1)
        Dim twosComplementInt As Integer = Convert.ToInt32(twosComplement)
        Dim str As String = twosComplementInt.ToString("X2")

        mc_sendbuf(mc_sendbuf_pos) = Asc(str.Substring(0, 1))
        mc_sendbuf_pos = mc_sendbuf_pos + 1
        mc_sendbuf(mc_sendbuf_pos) = Asc(str.Substring(1, 1))
        mc_sendbuf_pos = mc_sendbuf_pos + 1

    End Sub


    Private Sub mc_sendbuf_add_crlf()
        mc_sendbuf(mc_sendbuf_pos) = Convert.ToByte(ControlChars.Cr)
        mc_sendbuf_pos = mc_sendbuf_pos + 1
        mc_sendbuf(mc_sendbuf_pos) = Convert.ToByte(ControlChars.Lf)
        mc_sendbuf_pos = mc_sendbuf_pos + 1
    End Sub

    ' buttons
    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click

        commandMode = Mode.Reset

    End Sub

    Enum Mode

        Idle
        Reset
        Capture
        Captured

    End Enum
    Private commandMode As Mode

    Private Sub ManagementFunction()

        Dim captureCommandBuffer() As Byte = {&H3A, &H30, &H31, &H30, &H30, &H30, &H30, &H30, &H39, &H30, &H33, &H46, &H33, &HD, &HA}
        Dim resetCommandBuffer() As Byte = {&H3A, &H30, &H31, &H30, &H30, &H30, &H30, &H30, &H39, &H30, &H39, &H45, &H44, &HD, &HA}

        While continueRunning

            Select Case commandMode

                Case Mode.Idle

                Case Mode.Reset
                    serialManager.SendMessageBin(resetCommandBuffer, 15)
                    commandMode = Mode.Idle

                Case Mode.Capture
                    serialManager.SendMessageBin(captureCommandBuffer, 15)

                Case Mode.Captured

            End Select

            Thread.Sleep(500)

        End While

    End Sub

    Private Sub CaptureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaptureButton.Click
        commandMode = Mode.Capture

    End Sub

End Class
