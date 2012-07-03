'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Drawing
Imports System
Imports System.Windows.Forms

Imports SerialManager
Imports HelperFunctions

Public Class uMechTest

    ' managers
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    Private trackingArm As Boolean = False
    Private offsetArm As Point
    Private armDrive As Point
    Private armDriveLast As Point

    Private trackingMech As Boolean = False
    Private offsetMech As Point
    Private mechDrive As Point
    Private mechDriveLast As Point
    Private sizeOffset As Point
    Private counterQueryScalar As Integer = 0

    Private horizontalCounter As Integer = -1
    Private verticalCounter As Integer = -1
    Private pickArmCounter As Integer = -1


    Private rackPickingArmProgress As VerticalProgressBar
    Private rackVerticalProgress As VerticalProgressBar
    Private rackHorizontalProgress As ProgressBar

    ' constants
    Private MECH_CENTRE_X = 113
    Private MECH_CENTRE_Y = 104
    Private ARM_CENTRE_X = 57
    Private ARM_CENTRE_Y = 104
    Private HORZ_MAX_COUNT = 50
    Private VERT_MAX_COUNT = 50
    Private ARM_MAX_COUNT = 50

    Public Sub Initialise()

        serialManager = serialManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

        serialManager.AddCallback(AddressOf SerialPortEvent)


        sizeOffset = New Point(RackKnobPanel.Width / 2, RackKnobPanel.Height / 2)
        RackKnobPanel.Location = New Point(MECH_CENTRE_X - sizeOffset.X, MECH_CENTRE_Y - sizeOffset.Y)
        ArmKnobPanel.Location = New Point(ARM_CENTRE_X - sizeOffset.X, ARM_CENTRE_Y - sizeOffset.Y)
        mechDriveLast = New Point(0, 0)
        mechDrive = New Point(0, 0)
        armDriveLast = New Point(0, 0)
        armDrive = New Point(0, 0)

        rackPickingArmProgress = New VerticalProgressBar
        rackPickingArmProgress.Width = 17
        rackPickingArmProgress.Height = 140
        rackPickingArmProgress.Text = ""
        rackPickingArmProgress.Name = "pickingArmProgress"
        rackPickingArmProgress.Location = New Point(13, 37)
        rackPickingArmProgress.Maximum = ARM_MAX_COUNT
        rackPickingArmProgress.Minimum = 0
        '    rackPickingArmProgress.Step = 1
        '    rackPickingArmProgress.Style = ProgressBarStyle.Continuous
        ArmGroup.Controls.Add(rackPickingArmProgress)

        rackHorizontalProgress = New ProgressBar
        rackHorizontalProgress.Width = 140
        rackHorizontalProgress.Height = 17
        rackHorizontalProgress.Text = ""
        rackHorizontalProgress.Name = "horizontalProgress"
        rackHorizontalProgress.Location = New Point(42, 189)
        rackHorizontalProgress.Maximum = HORZ_MAX_COUNT
        rackHorizontalProgress.Minimum = 0
        '  rackHorizontalProgress.Step = 1
        '   rackHorizontalProgress.Style = ProgressBarStyle.Continuous
        MechGroup.Controls.Add(rackHorizontalProgress)

        rackVerticalProgress = New VerticalProgressBar
        rackVerticalProgress.Width = 17
        rackVerticalProgress.Height = 140
        rackVerticalProgress.Text = ""
        rackVerticalProgress.Name = "verticalProgress"
        rackVerticalProgress.Location = New Point(13, 37)
        rackVerticalProgress.Maximum = VERT_MAX_COUNT
        rackVerticalProgress.Minimum = 0
        '       rackVerticalProgress.Step = 1
        '    rackVerticalProgress.Style = ProgressBarStyle.Continuous
        MechGroup.Controls.Add(rackVerticalProgress)

    End Sub

    ' StartTest & StopTest
    ' start and stop the testing
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StartTest()

        serialManager.SendMessage("CLEARCOUNTERS")
        MechTestTimer.Enabled = True

    End Sub

    Public Sub StopTest()
        MechTestTimer.Enabled = False
        serialManager.SendMessage("MECHDRV 0 0000 0 0000")
        serialManager.SendMessage("ARMDRV 0 0000")
    End Sub

    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = messageContent.Split("=")

            Select Case parameterList(0)

                Case "SWITCHES"

                    parameterList = parameterList(1).Split(" ")
                    UpdateSwitchScreen(parameterList)

                Case "COUNTERS"

                    Dim axisList() As String = Split(parameterList(1), ",")

                    ProcessCounters(axisList)

            End Select

        End If

    End Sub

    ' UpdateSwitchScreen
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub UpdateSwitchScreen(ByVal blockData() As String)

        If blockData.Length >= 1 Then
            ShowSwitch(HorzHomePanel, blockData(0), 0)
            ShowSwitch(HorzLimitPanel, blockData(0), 1)
        End If

        If blockData.Length >= 2 Then
            ShowSwitch(VertHomePanel, blockData(1), 0)
            ShowSwitch(VertLimitPanel, blockData(1), 1)
        End If

        If blockData.Length >= 3 Then
            ShowSwitch(ArmHomePanel, blockData(2), 0)
        End If

        If blockData.Length >= 4 Then
            ShowSwitch(DoorOpenPanel, blockData(3), 0)
            ShowSwitch(DoorClosePanel, blockData(3), 1)
        End If

        If blockData.Length >= 5 Then
            ShowSwitch(ShelfDetectionPanel, blockData(4), 0)
        End If

        If blockData.Length >= 6 Then
            ShowSwitch(FrontSensorPanel, blockData(5), 0)
            ShowSwitch(RearSensorPanel, blockData(5), 1)
        End If

        If blockData.Length >= 7 Then
            ShowSwitch(ProductHeldPanel, blockData(6), 0)
        End If

    End Sub

    Private Sub ShowSwitch(ByVal panelToColour As Panel, ByVal blockString As String, ByVal bitIndex As Integer)

        If blockString.Length > bitIndex Then

            If blockString.Substring(bitIndex, 1) = "1" Then
                helperFunctions.SetPanelColour(panelToColour, System.Drawing.Color.Red)
            Else
                helperFunctions.SetPanelColour(panelToColour, System.Drawing.Color.Maroon)
            End If

        End If

    End Sub

    ' MechTestTimer_Tick
    ' periodically check to see how much juice we should send to the arm and the mech.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub MechTestTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MechTestTimer.Tick

        Dim messageToSend As String
        Dim reductionToMake As Integer

        ' return the mech joystick to centre
        If trackingMech = False Then

            reductionToMake = IIf(System.Math.Abs(mechDrive.X) > 5, 5, System.Math.Abs(mechDrive.X))
            mechDrive.X = mechDrive.X + IIf(mechDrive.X > 0, -reductionToMake, reductionToMake)

            reductionToMake = IIf(System.Math.Abs(mechDrive.Y) > 5, 5, System.Math.Abs(mechDrive.Y))
            mechDrive.Y = mechDrive.Y + IIf(mechDrive.Y > 0, -reductionToMake, reductionToMake)

            RackKnobPanel.Location = New Point(mechDrive.X + MECH_CENTRE_X - sizeOffset.X, mechDrive.Y + MECH_CENTRE_Y - sizeOffset.Y)
            MechVectorLabel.Text = mechDrive.X.ToString & "," & mechDrive.Y.ToString

        End If

        ' return the arm joystick to centre
        If trackingArm = False Then
            reductionToMake = IIf(System.Math.Abs(armDrive.Y) > 5, 5, System.Math.Abs(armDrive.Y))
            armDrive.Y = armDrive.Y + IIf(armDrive.Y > 0, -reductionToMake, reductionToMake)

            ArmVectorLabel.Text = armDrive.Y.ToString
            ArmKnobPanel.Location = New Point(armDrive.X + ARM_CENTRE_X - sizeOffset.X, armDrive.Y + ARM_CENTRE_Y - sizeOffset.Y)

        End If


        ' send the messages 
        If mechDriveLast <> mechDrive Then

            messageToSend = IIf(mechDrive.X >= 0, "MECHDRV 1 ", "MECHDRV 0 ")
            messageToSend = messageToSend & System.Math.Abs(mechDrive.X * 1023 / 60).ToString("0000")
            messageToSend = messageToSend & IIf(mechDrive.Y >= 0, " 1 ", " 0 ")
            messageToSend = messageToSend & System.Math.Abs(mechDrive.Y * 1023 / 60).ToString("0000")

            serialManager.SendMessage(messageToSend)
            mechDriveLast = mechDrive
        End If

        If armDriveLast <> armDrive Then

            messageToSend = IIf(armDrive.Y >= 0, "ARMDRV 1 ", "ARMDRV 0 ")
            messageToSend = messageToSend & System.Math.Abs(armDrive.Y * 1023 / 60).ToString("0000")

            serialManager.SendMessage(messageToSend)
            armDriveLast = armDrive
        End If

        ' alternate counters and switches
        counterQueryScalar = counterQueryScalar + 1
        Select Case counterQueryScalar

            Case 1
                serialManager.SendMessage("GETCOUNTERS")

            Case 2
                serialManager.SendMessage("GETSWITCHES ")
                counterQueryScalar = 0

        End Select

    End Sub

    ' Shutdown
    ' kill the timer
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Shutdown()

        MechTestTimer.Enabled = False
    End Sub

    ' ProcessCounters
    ' drive counter values have returned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ProcessCounters(ByVal axisList() As String)

        Dim newHorizontalCounter As Integer
        Dim newVerticalCounter As Integer
        Dim newPickArmCounter As Integer

        If Not axisList Is Nothing AndAlso axisList.Length = 3 Then

            helperFunctions.StringToInteger(axisList(0), newHorizontalCounter)
            helperFunctions.StringToInteger(axisList(1), newVerticalCounter)
            helperFunctions.StringToInteger(axisList(2), newPickArmCounter)

            If newHorizontalCounter <> horizontalCounter Then
                horizontalCounter = newHorizontalCounter
                helperFunctions.SetProgressBarValue(rackHorizontalProgress, Math.Min(horizontalCounter, HORZ_MAX_COUNT))
            End If

            If newVerticalCounter <> verticalCounter Then
                verticalCounter = newVerticalCounter
                helperFunctions.SetVerticalProgressBarValue(rackVerticalProgress, Math.Min(verticalCounter, VERT_MAX_COUNT))
            End If

            If newPickArmCounter <> pickArmCounter Then
                pickArmCounter = newPickArmCounter
                helperFunctions.SetVerticalProgressBarValue(rackPickingArmProgress, Math.Min(pickArmCounter, ARM_MAX_COUNT))
            End If

        End If

    End Sub

    ' ArmKnobPanel_MouseMove
    ' change the drive required by the arm
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ArmKnobPanel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ArmKnobPanel.MouseMove

        If trackingArm Then

            armDrive = ArmGroup.PointToClient(New Point(MousePosition.X, MousePosition.Y))

            armDrive.X = 0
            armDrive.Y = armDrive.Y - (offsetArm.Y + ARM_CENTRE_Y) + sizeOffset.Y

            If armDrive.Y < -60 Then armDrive.Y = -60
            If armDrive.Y > 60 Then armDrive.Y = 60

            ArmVectorLabel.Text = armDrive.Y.ToString

            ArmKnobPanel.Location = New Point(armDrive.X + ARM_CENTRE_X - sizeOffset.X, armDrive.Y + ARM_CENTRE_Y - sizeOffset.Y)

        End If

    End Sub

    ' RackKnob_MouseMove
    ' change the drive required by the mech
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub RackKnob_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RackKnobPanel.MouseMove

        If trackingMech Then

            mechDrive = MechGroup.PointToClient(New Point(MousePosition.X, MousePosition.Y))

            mechDrive.X = mechDrive.X - (offsetMech.X + MECH_CENTRE_X) + sizeOffset.X
            mechDrive.Y = mechDrive.Y - (offsetMech.Y + MECH_CENTRE_Y) + sizeOffset.Y

            If mechDrive.X < -60 Then mechDrive.X = -60
            If mechDrive.X > 60 Then mechDrive.X = 60
            If mechDrive.Y < -60 Then mechDrive.Y = -60
            If mechDrive.Y > 60 Then mechDrive.Y = 60

            MechVectorLabel.Text = mechDrive.X.ToString & "," & mechDrive.Y.ToString

            RackKnobPanel.Location = New Point(mechDrive.X + MECH_CENTRE_X - sizeOffset.X, mechDrive.Y + MECH_CENTRE_Y - sizeOffset.Y)

        End If

    End Sub


    ' ArmKnobPanel_MouseUp, RackKnob_MouseUp
    ' start and stop tracking the mouse moovement on the arm and mech lever
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ArmKnobPanel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ArmKnobPanel.MouseUp
        trackingArm = False
    End Sub

    Private Sub RackKnob_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RackKnobPanel.MouseUp
        trackingMech = False
    End Sub

    Private Sub ArmKnobPanel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ArmKnobPanel.MouseDown
        trackingArm = True
        offsetArm = New Point(e.X, e.Y)
    End Sub

    Private Sub RackKnob_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RackKnobPanel.MouseDown
        trackingMech = True
        offsetMech = New Point(e.X, e.Y)
    End Sub



    ' DoorOpenButton_Click, DoorCloseButton_Click, VacuumOffButton_Click, VacuumOn_Click, FanOffButton_Click, FanOnButton_Click, CompressorOffButton_Click, CompressorOnButton_Click, 
    ' send messages for the VMC to turn on various peripherals
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DoorOpenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DoorOpenButton.Click
        serialManager.SendMessage("MOVEDOOR 1")
    End Sub

    Private Sub DoorCloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DoorCloseButton.Click
        serialManager.SendMessage("MOVEDOOR 0")
    End Sub

    Private Sub VacuumOffButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VacuumOffButton.Click
        serialManager.SendMessage("VACUUM 0")
    End Sub

    Private Sub VacuumOn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VacuumOnButton.Click
        serialManager.SendMessage("VACUUM 1")
    End Sub

    Private Sub FanOnButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FanOnButton.Click
        serialManager.SendMessage("FAN 1")
    End Sub

    Private Sub FanOffButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FanOffButton.Click
        serialManager.SendMessage("FAN 0")
    End Sub

    Private Sub CompressorOnButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompressorOnButton.Click
        serialManager.SendMessage("COMPRESSOR 1")
    End Sub

    Private Sub CompressorOffButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompressorOffButton.Click
        serialManager.SendMessage("COMPRESSOR 0")
    End Sub

    Private Sub HomeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomeButton.Click
        serialManager.SendMessage("MOVEHEADHOME")
    End Sub

    Private Sub LimitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LimitButton.Click
        serialManager.SendMessage("MOVEHEADMIN")
    End Sub

End Class
