'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports System.Threading
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Reflection

Imports DebugWindow
Imports HelperFunctions
Imports SerialManager
Imports SettingsManager
Imports MechInterface

Public Class fLockMech

    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private mechInterface As fMechInterface
    Private mechInterfaceFactory As cMechInterfaceFactory = New cMechInterfaceFactory

    Private doorOpenState As Boolean
    Private productTriggerReference As Integer
    Private userList As cUserList
    Private userIdList() As cUser
    Private accessLevel As Integer = 0

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' Initialise
    ' record door openings and closings
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the gui invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle
        Dim allUsers As New List(Of cUser)
        Dim userTotal As Integer = 0

        ' get the managers
        debugInformation = debugInformationFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()
        mechInterface = mechInterfaceFactory.GetManager()

        userList = New cUserList

        Try
            allUsers = userList.GetAllUsers()
        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 9999, "Unable to recover userlist" & ex.Message.ToString, True)
        End Try

        If settingsManager.GetValue("DoorLockEnabled") Then

            debugInformation.Progress(fDebugWindow.Level.INF, 1500, "Lock Mechanism Enabled", True)

            mechInterface.SetGpioBit(2, False)

            lockTimer.Enabled = True
            LockButton.Enabled = True
            ConfigureButton.Enabled = True
            doorOpenState = mechInterface.GetGpioBit(6)

        End If

        If Not allUsers Is Nothing Then

            For Each singleUser As cUser In allUsers


                UserListBox.Items.Add(singleUser.userName)
                ReDim Preserve userIdList(userTotal)
                userIdList(userTotal) = singleUser

                userTotal = userTotal + 1

            Next
        End If

    End Sub

    ' OpenKeypad & CloseKeypad
    ' show and hide the window.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub OpenKeypad()

        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf OpenKeypad))

        Else
            '   Width = 204
            Text = "Enter Passcode"
            helperFunctions.SetTextBoxText(PasscodeText, "")

            Show()
            BringToFront()
            TopMost = True

            helperFunctions.SetButtonEnable(ConfigureButton, False)
            helperFunctions.SetButtonEnable(LayoutButton, False)
            helperFunctions.SetButtonEnable(LockButton, False)

            debugInformation.Progress(fDebugWindow.Level.INF, 1503, "LockMech Form Visible", True)
        End If

    End Sub

    Private Sub CloseKeypad()

        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf CloseKeypad))
        Else
            Hide()
            debugInformation.Progress(fDebugWindow.Level.INF, 1504, "LockMech Form Hidden", True)
        End If
    End Sub

    Private Sub numeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn0.Click, btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn8.Click, btn9.Click

        Dim btn As Button = DirectCast(sender, Button)
        Dim myNum As String = btn.Tag

        Select Case PasscodeText.Text.Length

            Case 0
                helperFunctions.SetTextBoxText(PasscodeText, myNum)
                Text = "Press Enter or Clear"

            Case 1 To 6
                helperFunctions.SetTextBoxText(PasscodeText, PasscodeText.Text & myNum)

            Case 6
                If LockButton.Enabled = True Then

                Else
                    Text = "Press Enter or Clear"
                End If

        End Select

        'If PasscodeText.Text.Length = 6 Then

        '    If PasscodeText.Text <> settingsManager.GetValue("AccessPasscode") Then

        '        Text = "Passcode Incorrect"
        '        helperFunctions.SetTextBoxText(PasscodeText, "")
        '    Else

        '        '    Width = 308
        '        Text = "Unlocked.."
        '    End If

        'End If

    End Sub


    ' LockTimerElapsed
    ' record door openings and closings
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub LockTimerElapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles lockTimer.Elapsed

        Dim doorOpen As Boolean

        If settingsManager.GetValue("DoorLockEnabled") Then
            doorOpen = mechInterface.GetGpioBit(6)
            If doorOpenState <> doorOpen Then

                If doorOpen Then
                    debugInformation.Progress(fDebugWindow.Level.INF, 1501, "Door Open", True)
                Else
                    debugInformation.Progress(fDebugWindow.Level.INF, 1502, "Door Close", True)
                End If
                doorOpenState = doorOpen
            End If

        End If
    End Sub


    ' SerialManagerForm_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ClearCodeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearCodeButton.Click

        helperFunctions.SetTextBoxText(PasscodeText, "")

    End Sub

    ' LockButtonClicked & FireLocks
    ' fire the lock pins for a given time.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub LockButtonClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LockButton.Click
        Dim LockThread = New Thread(AddressOf FireLocks)
        LockThread.Name = "Fire Door Locks"
        LockThread.Start()
    End Sub

    Private Sub FireLocks()
        mechInterface.SetGpioBit(2, True)
        Thread.Sleep(settingsManager.GetValue("DoorLockDuration"))
        mechInterface.SetGpioBit(2, False)
    End Sub

    ' ExitButtonClicked
    ' hide this form
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ExitButtonClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Hide()
    End Sub

    ' ConfigureButtonClick
    ' fire up the configure app and close the vend application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ConfigureButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureButton.Click

        Dim processStart As ProcessStartInfo = New ProcessStartInfo
        Dim applicationFolder As String = My.Application.Info.DirectoryPath

        processStart.UseShellExecute = False
        processStart.FileName = applicationFolder & "\Configure.exe"
        processStart.WorkingDirectory = applicationFolder
        processStart.RedirectStandardOutput = False

        serialManager.Disconnect()
        System.Diagnostics.Process.Start(processStart)
        fMain.StopProgram()

    End Sub

    Private Sub LayoutButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LayoutButton.Click

        Dim processStart As ProcessStartInfo = New ProcessStartInfo
        Dim applicationFolder As String = My.Application.Info.DirectoryPath

        processStart.UseShellExecute = False
        processStart.FileName = applicationFolder & "\Layout.exe"
        processStart.Arguments = IIf(accessLevel = 4, "-NS", "")
        processStart.WorkingDirectory = applicationFolder
        processStart.RedirectStandardOutput = False

        serialManager.Disconnect()
        System.Diagnostics.Process.Start(processStart)
        fMain.StopProgram()

    End Sub

    Private Sub EnterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnterButton.Click

        Dim selectedUser As cUser

        If UserListBox.SelectedIndex <> -1 Then

            selectedUser = userIdList(UserListBox.SelectedIndex)

            Try
                accessLevel = selectedUser.CheckUserCredentials(PasscodeText.Text)
            Catch ex As Exception
                debugInformation.Progress(fDebugWindow.Level.ERR, 9999, "Unable to check user credentials : " & ex.Message.ToString, True)
            End Try


            ' default all the buttons to off
            helperFunctions.SetButtonEnable(ConfigureButton, False)
            helperFunctions.SetButtonEnable(LayoutButton, False)
            helperFunctions.SetButtonEnable(LockButton, False)

            If accessLevel = 1 Or accessLevel = 3 Then helperFunctions.SetButtonEnable(ConfigureButton, True)
            If accessLevel = 1 Or accessLevel = 2 Or accessLevel = 4 Then helperFunctions.SetButtonEnable(LayoutButton, True)

        End If

        '    MsgBox(accessLevel.ToString)

    End Sub

End Class

