'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.Threading
Imports System.IO
Imports System.Net.Sockets
Imports System.Xml
Imports System.Data.SqlClient

Imports DebugWindow
Imports HelperFunctions
Imports SerialManager
Imports SettingsManager

' the serial manager form
' event driven serial IO
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fAttractManager

    ' enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Enum WanderVmcStatusEnum
        WS_RUNNING
        WS_STOPPING
        WS_STOPPED

        WS_UNKNOWN

    End Enum

    Enum WanderStatusEnum

        PC_WANDER_DISABLED
        PC_WANDER_IDLE
        PC_WANDER_STOP
        PC_WANDER_STOPPING
        PC_WANDER_START
        PC_WANDER_STARTING
        PC_WANDER_ACTIVE

        PC_UNKNOWN

    End Enum

    ' structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Structure Time

        Dim Hour As Integer
        Dim Minute As Integer
        Dim Second As Integer

    End Structure

    ' variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugManager As fDebugWindow
    Private debugManagerFactory As cDebugWindowFactory = New cDebugWindowFactory

    Private latestIncommingData As String
    Private attractThread As Thread
    Private continueProcessing As Boolean = True
    Private wanderPosition As AvailablePositionStruct = New AvailablePositionStruct
    Private attractActive As Boolean = False
    Private nextMoveDue As Date
    Private timeNow As Date
    Private editMode As Boolean = False
    Private initialProductLoading As Boolean = False

    Private vmcWanderStatus As WanderVmcStatusEnum = WanderVmcStatusEnum.WS_STOPPED
    Private wanderStatus As WanderStatusEnum = WanderStatusEnum.PC_WANDER_IDLE
    Private displayedVmcWanderStatus As WanderVmcStatusEnum = WanderVmcStatusEnum.WS_UNKNOWN
    Private displayedWanderStatus As WanderStatusEnum = WanderStatusEnum.PC_UNKNOWN

    ' constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' Initialise & Shutdown
    ' connect to the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the gui invoke to work..
        Dim temporaryHandle1 As System.IntPtr = Me.Handle
        Dim temporaryHandle2 As System.IntPtr = CueListView.Handle
        Dim temporaryHandle3 As System.IntPtr = ScriptListView.Handle
        Dim temporaryHandle4 As System.IntPtr = ProductPositionListView.Handle

        ' get the managers
        helperFunctions = helperFunctionsFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        debugManager = debugManagerFactory.GetManager()

        ' get serial comms events
        serialManager.AddCallback(AddressOf SerialPortEvent)

        debugManager.Progress(fDebugWindow.Level.INF, 2200, "Starting attract Manager", True)

        InitialiseLighting()
        InitialisePositions()
        debugManager.Progress(fDebugWindow.Level.INF, 2200, "Step 1 Manager", True)
        ' start the management
        attractThread = New Thread(AddressOf AttractThreadProcess)
        attractThread.Priority = ThreadPriority.BelowNormal
        attractThread.Name = "Attract Wander"
        attractThread.Start()

        DrawTrackbarControls(0)
        debugManager.Progress(fDebugWindow.Level.INF, 2200, "Step 2 Manager", True)
        If settingsManager.GetValue("ShowAttractDebug") Then
            Show()
        End If
        debugManager.Progress(fDebugWindow.Level.INF, 2200, "Step 3 Manager", True)
    End Sub


    Private isNightTime As Boolean = False
    Private dueNightDayAssessment As Date = Now

    Private Sub AssessTimeOfDay()

        Dim dayStartTime As Integer
        Dim nightStartTime As Integer
        Dim assessmentTime As Integer
        Dim timeNow As Date = Now

        ' as we count time in minute intervals in this context only do this every 60 seconds
        If timeNow > dueNightDayAssessment Then

            dueNightDayAssessment = dueNightDayAssessment.AddMinutes(1)

            dayStartTime = GetAssessmentTime(settingsManager.GetValue("StartDayTime"))
            nightStartTime = GetAssessmentTime(settingsManager.GetValue("StartNightTime"))
            assessmentTime = GetAssessmentTime(timeNow.ToString("HH:mm"))

            isNightTime = False

            If dayStartTime = nightStartTime Then
                ' error return daytime

            ElseIf dayStartTime < nightStartTime Then

                ' how yoy would hope day night is set up day start 07:00, night start 20:00 etc..
                If assessmentTime < dayStartTime Or assessmentTime >= nightStartTime Then
                    isNightTime = True
                End If

                ' if they define the day as in the evening 20:00 and night after 01:00 
            ElseIf assessmentTime < dayStartTime And assessmentTime >= nightStartTime Then
                isNightTime = True
            End If

            ' throw sometine up for debug
            Progress("Assesing the time of day")
            Progress("day starts at: " & dayStartTime.ToString)
            Progress("night starts at: " & nightStartTime.ToString)
            Progress("time now: " & assessmentTime.ToString)
            Progress("we are in " & IIf(isNightTime, "NIGHTTIME", "DAYTIME"))


        End If

        ' if we are idling and we move from day to night of vice versa we need to change scripts
        If isNightTime AndAlso Not scriptList Is Nothing AndAlso scriptList(currentScriptIndex).name = "IDLING" Then
            RunLightingScript("IDLINGNIGHT", True)

        ElseIf Not isNightTime AndAlso Not scriptList Is Nothing AndAlso scriptList(currentScriptIndex).name = "IDLINGNIGHT" Then
            RunLightingScript("IDLING", True)
        End If

    End Sub

    Private Function GetAssessmentTime(ByVal timeString As String) As Integer

        If timeString Is Nothing Then
            Return -1
        End If


        Dim timePart() As String = timeString.Split(":")
        Dim assessedTime As Integer = -1
        Dim hourCount As Integer
        Dim minuteCount As Integer

        ' enough numbers in the string, are they actually numbers, and are they valid ?
        If timePart.Length = 2 AndAlso _
            helperFunctions.StringToInteger(timePart(0), hourCount) AndAlso helperFunctions.StringToInteger(timePart(1), minuteCount) AndAlso _
             hourCount >= 0 AndAlso hourCount <= 23 AndAlso minuteCount >= 0 AndAlso minuteCount <= 59 Then
            assessedTime = (hourCount * 100) + minuteCount
        End If

        Return assessedTime

    End Function


    Public Sub Shutdown()
        LightingPort.Close()
        continueProcessing = False
    End Sub

    ' AttractThreadProcess
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AttractThreadProcess()

        While continueProcessing

            If Not editMode Then
                timeNow = Now()

                AssessTimeOfDay()

                Select Case wanderStatus

                    Case WanderStatusEnum.PC_WANDER_IDLE

                        If attractActive And timeNow > nextMoveDue Then

                            ' issue the command to vend to the VMC
                            If NextAttractPosition(wanderPosition) Then
                                wanderStatus = WanderStatusEnum.PC_WANDER_START
                            End If

                        End If

                    Case WanderStatusEnum.PC_WANDER_START

                        Progress("Next Position Is " & wanderPosition.name & "-" & wanderPosition.preName & " (" & wanderPosition.xPosition & "," & wanderPosition.yPosition & ")")
                        serialManager.SendMessage("WANDER " & Format(wanderPosition.xPosition, "####0000") & "," & Format(wanderPosition.yPosition, "####0000"))
                        serialManager.SendMessage("GETWANDER")
                        wanderStatus = WanderStatusEnum.PC_WANDER_STARTING

                    Case WanderStatusEnum.PC_WANDER_STARTING

                        If vmcWanderStatus = WanderVmcStatusEnum.WS_STOPPED Then
                            wanderStatus = WanderStatusEnum.PC_WANDER_IDLE
                            ScheduleNextMove(settingsManager.GetValue("IdlingWaitAtLocation"))

                        ElseIf vmcWanderStatus = WanderVmcStatusEnum.WS_RUNNING Then
                            wanderStatus = WanderStatusEnum.PC_WANDER_ACTIVE
                        Else
                            serialManager.SendMessage("GETWANDER")
                        End If

                    Case WanderStatusEnum.PC_WANDER_STOP

                        serialManager.SendMessage("WANDERSTOP")
                        serialManager.SendMessage("GETWANDER")
                        wanderStatus = WanderStatusEnum.PC_WANDER_STOPPING

                    Case WanderStatusEnum.PC_WANDER_ACTIVE

                        If vmcWanderStatus = WanderVmcStatusEnum.WS_STOPPED Then

                            Progress("Wander has stopped.")
                            wanderStatus = WanderStatusEnum.PC_WANDER_IDLE
                            ScheduleNextMove(settingsManager.GetValue("IdlingWaitAtLocation"))

                        Else
                            serialManager.SendMessage("GETWANDER")
                        End If


                    Case WanderStatusEnum.PC_WANDER_STOPPING

                        If vmcWanderStatus = WanderVmcStatusEnum.WS_STOPPED Then

                            Progress("Wander has been stopped.")
                            wanderStatus = WanderStatusEnum.PC_WANDER_IDLE

                        Else
                            serialManager.SendMessage("GETWANDER")
                        End If

                End Select
            End If


            ' update the status strip
            If vmcWanderStatus <> displayedVmcWanderStatus Then
                helperFunctions.StatusStripMessage(StatusStrip, 1, vmcWanderStatus.ToString)
                displayedVmcWanderStatus = vmcWanderStatus
            End If

            If wanderStatus <> displayedWanderStatus Then

                helperFunctions.StatusStripMessage(StatusStrip, 0, wanderStatus.ToString)
                displayedWanderStatus = wanderStatus
            End If

            If continueProcessing Then
                Thread.Sleep(100)
            End If

        End While

    End Sub

    ' ScheduleNextMove
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ScheduleNextMove(ByVal secondsToWait As Integer)
        nextMoveDue = timeNow.AddSeconds(secondsToWait)
        Progress("Next move scheduled for: " & nextMoveDue.ToString("T"))
    End Sub

    ' SerialPortEvent
    ' event driven incomming serial messages.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = messageContent.Split("=")

            If parameterList.Length >= 1 Then

                Select Case parameterList(0)

                    Case "WANDER"

                        If parameterList.Length >= 2 Then
                            helperFunctions.StringToInteger(parameterList(1), vmcWanderStatus)
                        End If

                    Case "LIGHT"

                        If parameterList.Length >= 2 Then

                            Select Case parameterList(1)

                                ' vending cues
                                Case "0"
                                    RunLightingScript("VEND START", True)

                                Case "1"
                                    RunLightingScript("VEND GET PRODUCT", True)

                                Case "2"
                                    RunLightingScript("VEND TO DRUM", True)

                                Case "3"
                                    RunLightingScript("VEND DELIVER PRODUCT", True)

                                Case "4"
                                    RunLightingScript("VEND PRESENT", True)

                                Case "5"
                                    RunLightingScript("VEND COMPLETE", True)

                                    ' wander cues
                                Case "10"
                                    RunLightingScript("WANDER BEGIN", True)

                                Case "11"
                                    RunLightingScript("WANDER INTERRUPT", True)

                                Case "12"
                                    RunLightingScript("WANDER END", True)

                                Case "20"
                                    RunLightingScript(IIf(isNightTime, "IDLINGNIGHT", "IDLING"), True)

                            End Select

                        End If

                    Case Else

                End Select

            End If

        End If

    End Sub

    Private Sub Progress(ByVal messageContent)

        Dim timeNow = Date.Now()
        Dim timeToOutput As String = Format(timeNow.Hour, "##00") & ":" & Format(timeNow.Minute, "##00") & ":" & Format(timeNow.Second, "##00") & ":" & Format(timeNow.Millisecond, "###000")

        helperFunctions.AddToListBox(ProgressList, timeToOutput & ControlChars.Tab & messageContent)
    End Sub

    ' TestButton_Click, HideButton_Click, StartButton_Click, StopButton_Click
    ' manage the buttons
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

    Public Sub EnableAttract(ByVal newState)

        If newState <> attractActive Then

            attractActive = newState

            Progress("------------------------------------------------------------------------")
            Progress("Attract active changed to " & attractActive.ToString)

            If attractActive Then
                ScheduleNextMove(settingsManager.GetValue("IdlingTimeout"))

            Else

                wanderStatus = WanderStatusEnum.PC_WANDER_STOP
            End If

        End If

    End Sub

    Public Sub WaitForAttractToStop()

        While wanderStatus <> fAttractManager.WanderStatusEnum.PC_WANDER_IDLE
            Thread.Sleep(100)
            Application.DoEvents()
        End While

    End Sub


    Private Sub EditPlayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditPlayButton.Click

        editMode = Not editMode

        helperFunctions.SetButtonText(EditPlayButton, IIf(editMode, "Play", "Edit"))
        helperFunctions.SetButtonVisible(SetCueButton, editMode)
        helperFunctions.SetButtonVisible(SaveScriptButton, editMode)

    End Sub

    Private Sub ColourSetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColourSetButton.Click

        Dim redElement As Integer = MasterRedTrackbar.Value
        Dim greenElement As Integer = MasterGreenTrackbar.Value
        Dim blueElement As Integer = MasterBlueTrackbar.Value

        SetBaseColour(redElement, greenElement, blueElement)
        RunLightingScript("COLOURED IDLING", True)

    End Sub

    ' ChannelIndexText_Leave
    ' update channel indexing
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ChannelIndexText_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChannelIndex1Text.Leave, ChannelIndex2Text.Leave, ChannelIndex3Text.Leave, ChannelIndex4Text.Leave, ChannelIndex5Text.Leave, ChannelIndex6Text.Leave, ChannelIndex7Text.Leave, ChannelIndex8Text.Leave, ChannelIndex9Text.Leave, ChannelIndex10Text.Leave, ChannelIndex11Text.Leave, ChannelIndex12Text.Leave, ChannelIndex13Text.Leave, ChannelIndex14Text.Leave, ChannelIndex15Text.Leave, ChannelIndex16Text.Leave, ChannelIndex17Text.Leave, ChannelIndex18Text.Leave, ChannelIndex19Text.Leave, ChannelIndex20Text.Leave, ChannelIndex21Text.Leave, ChannelIndex22Text.Leave, ChannelIndex23Text.Leave, ChannelIndex24Text.Leave


        Dim lineOffest As Integer
        Dim newChannelIndex As Integer
        Dim channelToChange As Integer


        If helperFunctions.StringToInteger(sender.Tag, lineOffest) Then

            channelToChange = baseSelectedChannel + lineOffest + 1

            If helperFunctions.StringToInteger(sender.Text, newChannelIndex) Then

                If channelIndexing(channelToChange) <> newChannelIndex Then

                    channelIndexing(channelToChange) = newChannelIndex

                    If settingsManager.ConnectToDatabase() Then

                        settingsManager.RunDatabaseQuery("update lightingchannelindex set  channel = " & newChannelIndex & " where id=" & channelToChange)
                        settingsManager.DisconnectFromDatabase()
                    End If

                End If

            Else
                MsgBox("All channel indexes must be a integer numeric")
                helperFunctions.SetTextBoxText(sender, "*")

            End If

        Else
            MsgBox("An Internal error has occured")

        End If









    End Sub

End Class

' class cAttractManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cAttractManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared attractManager As fAttractManager = Nothing

    Public Function GetManager() As fAttractManager

        If IsNothing(attractManager) Then

            attractManager = New fAttractManager
            attractManager.Initialise()

        End If

        Return attractManager

    End Function

End Class


