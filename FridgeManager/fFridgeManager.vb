'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
' Imported Libraries.
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports System.IO
Imports System.Windows.Forms
Imports System.Threading
Imports DebugWindow
Imports HelperFunctions
Imports SettingsManager

Public Class fFridgeManager

    ' delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub FridgeEventCallback(ByVal eventCode As Message, ByVal doorIndex As Integer)

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Enum FridgeManagerState
        MS_NULL
        MS_SET_TIMEOUT
        MS_POLL
        MS_OPEN
        MS_CLOSE
        MS_WORKING
        MS_EXIT
    End Enum

    Private Enum FridgeLogEvent
        LOG_TEMPERATURE
        LOG_DEFROST_START
        LOG_DEFROST_END
        LOG_MINIMUM_TEMP_REACHED
        LOG_MAXIMUM_TEMP_REACHED
        LOG_STARTING_COMPRESSOR
        LOG_STOPPING_COMPRESSOR
        LOG_MANAGER_STARTS
    End Enum

    Public Enum Message
        FRD_COMMS_OK
        FRD_COMMS_FAIL
        FRD_OPEN_OK
        FRD_OPEN_FAIL
        FRD_CLOSE_OK
        FRD_CLOSE_FAIL
        FRD_TURN_FAN_ON
        FRD_TURN_FAN_OFF
        FRD_TURN_COMPRESSOR_ON
        FRD_TURN_COMPRESSOR_OFF
        FRD_DOORS_SAFE
        FRD_DOORS_NOT_SAFE
    End Enum

    Public Enum State
        ST_OPEN
        ST_CLOSE
    End Enum

 
    ' Constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Const NO_TEMPERATURE_TO_RECORD = -999
    Private Const NO_DOOR = -1
    Private Const NOT_TRANSMITED = -1
    Private Const DOOR_MOVE_TIMEOUT = 10
    Private Const DOOR_RETRY_TIMEOUT = 5
    Private Const AUTOCLOSE_INTERVAL = DOOR_MOVE_TIMEOUT + 1
   Private Const LONG_SLEEP_TIME = 1000
    Private Const NORMAL_SLEEP_TIME = 200

    Public Const LAST_DOOR_OPERATED = 0

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private motorTimeoutInterval As Integer
    Private continueThread As Boolean = True
    Private temperaturePanelToggle As Boolean = False
    Private currentDoor As Integer = 0
    Private managementThread As Thread
    Private managerState As FridgeManagerState = FridgeManagerState.MS_SET_TIMEOUT
    Private messageBuffer As String = ""
    Private commsTimeoutDue As Date
    Private retryMovementDue As Date
    Private eventCallbackDelegate() As FridgeEventCallback
    Private lastOpenSwitchState As Integer = -1
    Private lastClosedSwitchState As Integer = -1
    Private doorClosed(7) As Boolean
    Private doorShouldBeClosed(7) As Boolean
    Private dueAutoCloseOpenDoors As Date
    Private displayedManagerState As FridgeManagerState = FridgeManagerState.MS_NULL
    Private transmittedSafeState As Integer = NOT_TRANSMITED
    Private retryMessage As String = ""
    Private retryMessageAttempts As Integer = 0
    Private loopEndDelay As Integer = NORMAL_SLEEP_TIME

    Private fridgeJustStarted As Boolean = True
    Private tempReadsTillLogDue As Integer
    Private dueTemperatureRead As Date = Now.AddSeconds(5)
    Private dueTemperatureLog As Integer
    Private compressorRunningTime As Integer = 0
    Private compressorStoppedTime As Integer = 0
    Private compressorRunning As Boolean = False
    Private tempAtCompressorStart As Double = 0
    Private lastCompressorRunning As Boolean = False
    Private defrostCycleCountdown As Integer = 0
    Private lastReportedSafteyStatus As Boolean = True

    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

#Region "Window Events"

    ' FridgeManagerFormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub FridgeManagerFormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

#End Region

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Initialise() As Boolean

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the gui invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle
        Dim createTableCommand As String = "CREATE TABLE [dbo].[FridgeTemperature](" & _
                                                                     "[Id] [int] IDENTITY(1,1) NOT NULL," & _
                                                                     "[DateTime] [datetime] NULL," & _
                                                                     "[Temperature] [decimal](18, 1) NULL," & _
                                                                     "[Event] [int] NULL" & _
                                                                    ") ON [PRIMARY]"

        ' get the managers
        helperFunctions = helperFunctionsFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()

        ' schedule autoclose
        dueAutoCloseOpenDoors = Now.AddSeconds(AUTOCLOSE_INTERVAL)

        ' assume unsafe to move
        doorClosed(0) = False
        doorClosed(1) = False
        doorClosed(2) = False
        doorClosed(3) = False
        doorClosed(4) = False
        doorClosed(5) = False
        doorClosed(6) = False

        ' at the start all door should be closed
        doorShouldBeClosed(0) = True
        doorShouldBeClosed(1) = True
        doorShouldBeClosed(2) = True
        doorShouldBeClosed(3) = True
        doorShouldBeClosed(4) = True
        doorShouldBeClosed(5) = True
        doorShouldBeClosed(6) = True

        ' what should we overwrite the fridge motor timeout as ?
        motorTimeoutInterval = settingsManager.GetValue("FridgeMotorTimeout")

        ' recover which doors are in use
        RequiredCloseCheck1.Checked = settingsManager.GetValue("UseDoor1")
        RequiredCloseCheck2.Checked = settingsManager.GetValue("UseDoor2")
        RequiredCloseCheck3.Checked = settingsManager.GetValue("UseDoor3")
        RequiredCloseCheck4.Checked = settingsManager.GetValue("UseDoor4")
        RequiredCloseCheck5.Checked = settingsManager.GetValue("UseDoor5")
        RequiredCloseCheck6.Checked = settingsManager.GetValue("UseDoor6")
        RequiredCloseCheck7.Checked = settingsManager.GetValue("UseDoor7")

        ' try to open the serial port
        Try
            SerialPort.PortName = settingsManager.GetValue("FridgeSerialPort")
            SerialPort.Open()
            helperFunctions.AddToListBox(ProgressList, "Fridge serial port opened on " & SerialPort.PortName)
            debugInformation.Progress(fDebugWindow.Level.INF, 1700, "Fridge serial port opened on " & SerialPort.PortName, True)

        Catch ex As Exception
            SendApplicationMessage(Message.FRD_COMMS_FAIL, 0)
            helperFunctions.AddToListBox(ProgressList, "Unable to open fridge serial port " & SerialPort.PortName)
            debugInformation.Progress(fDebugWindow.Level.WRN, 1701, "Unable to open fridge serial port " & SerialPort.PortName, True)

            Return False

        End Try

        ' advise main application that comms have opened
        SendApplicationMessage(Message.FRD_COMMS_OK, 0)

        ' ensure that the temperature table exists
        If settingsManager.ConnectToDatabase() Then

            If Not settingsManager.TableExists("FridgeTemperature") Then
                settingsManager.RunDatabaseNonQuery(createTableCommand)
            End If

            settingsManager.DisconnectFromDatabase()
        End If

        'get the temp log interval
        tempReadsTillLogDue = 1

        ' fire up the  management thread
        managementThread = New Thread(AddressOf BackgroundThreadProcess)
        managementThread.Priority = ThreadPriority.Lowest
        managementThread.Name = "Fridge"
        managementThread.Start()

        LogEvent(FridgeLogEvent.LOG_MANAGER_STARTS, 0)

        Return True

    End Function

    ' OpenDoor      
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function OpenDoor(ByVal doorIndex As Integer) As Boolean

        Dim requestResult As Boolean = False

        If doorIndex >= 1 And doorIndex <= 7 Then

            helperFunctions.AddToListBox(ProgressList, "Request door open " & doorIndex.ToString)

            debugInformation.Progress(fDebugWindow.Level.INF, 1702, "Door open requested " & doorIndex, True)

            If managerState = FridgeManagerState.MS_POLL Then

                doorShouldBeClosed(doorIndex - 1) = False
                currentDoor = doorIndex
                managerState = FridgeManagerState.MS_OPEN
                requestResult = True

            End If

        End If


        Return requestResult

    End Function

    ' CloseDoor
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function CloseDoor(ByVal doorIndex As Integer) As Boolean

        Dim requestResult As Boolean = False

        If doorIndex = LAST_DOOR_OPERATED Then
            doorIndex = currentDoor
        End If

        If doorIndex >= 1 And doorIndex <= 7 Then

            helperFunctions.AddToListBox(ProgressList, "Request door close " & doorIndex.ToString)
            debugInformation.Progress(fDebugWindow.Level.INF, 1703, "Door close requested " & doorIndex, True)

            If managerState = FridgeManagerState.MS_POLL Then

                doorShouldBeClosed(doorIndex - 1) = True
                currentDoor = doorIndex
                managerState = FridgeManagerState.MS_CLOSE
                requestResult = True

            End If

        End If

        Return requestResult

    End Function


    ' LastDoorOperated
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function LastDoorOperated() As Integer
        Return currentDoor
    End Function


    ' DoorsSafe
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function DoorsSafe() As Boolean

        Dim requestResult As Boolean = True

        If (RequiredCloseCheck1.Checked And doorClosed(0) = False) Or _
             (RequiredCloseCheck2.Checked And doorClosed(1) = False) Or _
             (RequiredCloseCheck3.Checked And doorClosed(2) = False) Or _
             (RequiredCloseCheck4.Checked And doorClosed(3) = False) Or _
             (RequiredCloseCheck5.Checked And doorClosed(4) = False) Or _
             (RequiredCloseCheck6.Checked And doorClosed(5) = False) Or _
             (RequiredCloseCheck7.Checked And doorClosed(6) = False) Then

            requestResult = False

        End If

        Return requestResult

    End Function

    ' Shutdown
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Shutdown()

        ' allow the management thread to drop out
        managerState = FridgeManagerState.MS_EXIT

        continueThread = False
        Thread.Sleep(100)

    End Sub

    ' BackgroundThreadProcess
    ' The management thread
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub BackgroundThreadProcess()

        Dim timeNow As Date
        Dim autoCloseDoor As Integer
        Dim doorsAreSafe As Boolean
        Dim firstPass As Boolean = True

        Do While continueThread

            ' display the managers state
            If displayedManagerState <> managerState Then

                helperFunctions.StatusStripMessage(StatusStrip1, 1, managerState.ToString)
                displayedManagerState = managerState
            End If

            ' get the time once per loop 
            timeNow = Now

            ' process tasks to be done, by the manager, initialisation, polling etc..
            Select Case managerState

                Case FridgeManagerState.MS_SET_TIMEOUT

                    SendMessage("timeout " & motorTimeoutInterval.ToString, True)
                    loopEndDelay = LONG_SLEEP_TIME

                Case FridgeManagerState.MS_POLL

                    ' any doors open that shouldn't be?
                    If AutoCloseDoorsCheck.Checked And timeNow > dueAutoCloseOpenDoors Then

                        autoCloseDoor = DetermineDoorToAutoClose()

                        If autoCloseDoor <> 0 Then
                            helperFunctions.AddToListBox(ProgressList, "Auto close door " & autoCloseDoor.ToString)
                            CloseDoor(autoCloseDoor)
                        End If

                        dueAutoCloseOpenDoors = timeNow.AddSeconds(AUTOCLOSE_INTERVAL)

                        ' due to read the temperature ?
                    ElseIf timeNow > dueTemperatureRead Then

                        dueTemperatureRead = Now.AddSeconds(60)
                        SendMessage("temperature", False)

                        ' just read the switch state
                    Else
                        SendMessage("state", False)
                    End If

                Case FridgeManagerState.MS_OPEN

                    retryMessageAttempts = 3
                    retryMessage = "open " & currentDoor.ToString
                    retryMovementDue = timeNow.AddSeconds(DOOR_RETRY_TIMEOUT)

                    dueAutoCloseOpenDoors = timeNow.AddSeconds(AUTOCLOSE_INTERVAL)
                    SendMessage(retryMessage, True)
                    managerState = FridgeManagerState.MS_WORKING

                Case FridgeManagerState.MS_CLOSE

                    retryMessageAttempts = 3
                    retryMovementDue = timeNow.AddSeconds(DOOR_RETRY_TIMEOUT)

                    retryMessage = "close " & currentDoor.ToString

                    dueAutoCloseOpenDoors = timeNow.AddSeconds(AUTOCLOSE_INTERVAL)
                    SendMessage(retryMessage, True)
                    managerState = FridgeManagerState.MS_WORKING

                Case FridgeManagerState.MS_WORKING

                    If timeNow > retryMovementDue Then

                        If retryMessageAttempts <> 0 Then

                            retryMovementDue = timeNow.AddSeconds(DOOR_RETRY_TIMEOUT)
                            dueAutoCloseOpenDoors = timeNow.AddSeconds(AUTOCLOSE_INTERVAL)
                            SendMessage(retryMessage, True)
                            retryMessageAttempts = retryMessageAttempts - 1

                        Else

                            helperFunctions.AddToListBox(ProgressList, "Fridge movement timed out (PC)")
                            debugInformation.Progress(fDebugWindow.Level.ERR, 1704, "Fridge movement timed out (PC)", True)
                            managerState = FridgeManagerState.MS_POLL

                        End If

                    End If

            End Select


            doorsAreSafe = DoorsSafe()

            If doorsAreSafe <> lastReportedSafteyStatus Or firstPass Then

                If doorsAreSafe Then

                    helperFunctions.StatusStripMessage(StatusStrip1, 0, "Doors safe to vend")
                    SendApplicationMessage(Message.FRD_DOORS_SAFE, NO_DOOR)
                Else
                    helperFunctions.StatusStripMessage(StatusStrip1, 0, "Doors are a hazzard to vertical movement")
                    SendApplicationMessage(Message.FRD_DOORS_NOT_SAFE, NO_DOOR)
                End If

                firstPass = False
                lastReportedSafteyStatus = doorsAreSafe

            End If

            If continueThread Then

                Thread.Sleep(loopEndDelay)
            End If

        Loop

    End Sub

    ' DetermineDoorToAutoClose
    ' which door to autoclose ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Dim autoDoorCloseIndex As Integer = 0

    Private Function DetermineDoorToAutoClose()

        Dim autoCloseDoor As Integer = 0
        Dim requiredClosedCheck() As CheckBox = {RequiredCloseCheck1, RequiredCloseCheck2, RequiredCloseCheck3, RequiredCloseCheck4, RequiredCloseCheck5, RequiredCloseCheck6, RequiredCloseCheck7}
        Dim doorCount As Integer = 0
        Dim doorIndex As Integer = autoDoorCloseIndex

        While (autoCloseDoor = 0 And doorCount < 7)

            If requiredClosedCheck(doorIndex).Checked And doorShouldBeClosed(doorIndex) And Not doorClosed(doorIndex) Then autoCloseDoor = doorIndex + 1

            doorIndex = (doorIndex + 1) Mod 7

            doorCount += 1

        End While

        autoDoorCloseIndex = (autoDoorCloseIndex + 1) Mod 7

        Return autoCloseDoor

    End Function

    ' ProcessMessage
    ' process incomming messages
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProcessMessage(ByVal messageString As String)

        Dim wordList As String() = Split(messageString, " ")

        If wordList.Length > 0 Then

            Select Case wordList(0)

                Case "+welcome"

                    helperFunctions.AddToListBox(ProgressList, messageString.ToString)
                    helperFunctions.AddToListBox(ProgressList, "Fridge control board has reset")
                    managerState = FridgeManagerState.MS_SET_TIMEOUT

                Case "+complete"

                    helperFunctions.AddToListBox(ProgressList, messageString.ToString)
                    ProcessCompleteMovement(wordList)
                    helperFunctions.AddToListBox(ProgressList, "Movement complete")
                    debugInformation.Progress(fDebugWindow.Level.INF, 1705, "Door movement complete ", True)
                    managerState = FridgeManagerState.MS_POLL



                Case "-timeout"

                    helperFunctions.AddToListBox(ProgressList, messageString.ToString)
                    ProcessFailedMovement(wordList)
                    helperFunctions.AddToListBox(ProgressList, "-timeout")
                    debugInformation.Progress(fDebugWindow.Level.ERR, 1706, "Door movement timeout ", True)
                    managerState = FridgeManagerState.MS_POLL

                Case "+state"

                    AssessSwitchState(wordList)

                Case "+temperature"

                    ProcessTemperature(wordList)

                Case "+timeout"

                    helperFunctions.AddToListBox(ProgressList, messageString.ToString)
                    helperFunctions.AddToListBox(ProgressList, "Timeout set okay")
                    managerState = FridgeManagerState.MS_POLL
                    loopEndDelay = NORMAL_SLEEP_TIME

                Case Else

                    helperFunctions.AddToListBox(ProgressList, "Unknown: " & messageString.ToString)

            End Select

        End If

    End Sub



    ' AssessSwitchState
    ' validate and process the switch states
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AssessSwitchState(ByVal parameterList() As String)

        Dim openSwitchValue As Integer
        Dim closedSwitchValue As Integer

        If parameterList.Length >= 3 Then

            If helperFunctions.StringToInteger(parameterList(1), openSwitchValue) AndAlso helperFunctions.StringToInteger(parameterList(2), closedSwitchValue) Then
                If openSwitchValue >= 0 AndAlso openSwitchValue <= 255 AndAlso closedSwitchValue >= 0 AndAlso closedSwitchValue <= 255 Then
                    SetSwitches(openSwitchValue, closedSwitchValue)
                End If
            End If

        End If

    End Sub

    ' ProcessTemperature
    ' store and process the temperature record
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProcessTemperature(ByVal wordList() As String)

        Dim formattedTemperature As String
        Dim temperatureValue As Double
        Dim maximumValidTemperature As Double = settingsManager.GetValue("TargetTemperature") + settingsManager.GetValue("AllowedTemperatureVariance")
        Dim minimumValidTemperature As Double = settingsManager.GetValue("TargetTemperature") - settingsManager.GetValue("AllowedTemperatureVariance")

        ' was this read sucessfull ?
        If wordList.Length >= 2 AndAlso helperFunctions.StringToDouble(wordList(1), temperatureValue) AndAlso temperatureValue > -200 And temperatureValue < 600 Then

            ' toggle the temp panel to indicate we have recevied a temperature
            helperFunctions.SetPanelColour(TemperaturePanel, IIf(temperaturePanelToggle, Drawing.Color.Maroon, Drawing.Color.Red))
            temperaturePanelToggle = Not temperaturePanelToggle

            ' calculate the temperature write it to the label
            helperFunctions.StringToInteger(wordList(1), temperatureValue)
            temperatureValue = temperatureValue / 10

            formattedTemperature = temperatureValue.ToString("N1")
            helperFunctions.SetLabelText(TemperatureLabel, formattedTemperature)

            ' log the temperature
            tempReadsTillLogDue = tempReadsTillLogDue - 1

            If tempReadsTillLogDue = 0 Then
                tempReadsTillLogDue = settingsManager.GetValue("TemperatureLogInterval")
                LogEvent(FridgeLogEvent.LOG_TEMPERATURE, temperatureValue)
            End If

        End If

    End Sub

    Private Sub LogEvent(ByVal eventCode As FridgeLogEvent, ByVal temperatureValue As Double)

        If settingsManager.ConnectToDatabase() Then
            settingsManager.RunDatabaseNonQuery("insert fridgetemperature (datetime, temperature, event) values (getdate(), " & temperatureValue & ", " & eventCode & ")")
            settingsManager.DisconnectFromDatabase()
        End If


    End Sub

    ' NowDatabaseTime
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    'Private Function NowDatabaseTime() As String

    '    Dim dateNow As Date = Now
    '    Dim databaseDate As String

    '    databaseDate = dateNow.Year & "-" & dateNow.Month & "-" & dateNow.Day & " " & dateNow.Hour & ":" & dateNow.Minute & ":" & dateNow.Second

    '    Return databaseDate

    'End Function

    ' ProcessFailedMovement
    ' parse the door index and return notification to the application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProcessFailedMovement(ByVal wordList() As String)

        Dim doorIndex As Integer

        If wordList.Length >= 4 Then

            helperFunctions.StringToInteger(wordList(3), doorIndex)

            If wordList(2) = "open" Then
                SendApplicationMessage(Message.FRD_OPEN_FAIL, doorIndex - 1)

            ElseIf wordList(2) = "close" Then
                SendApplicationMessage(Message.FRD_CLOSE_FAIL, doorIndex - 1)

            End If

        End If

    End Sub

    ' ProcessCompleteMovement
    ' parse the door index and return notification to the application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProcessCompleteMovement(ByVal wordList() As String)

        Dim doorIndexOffset As Integer
        Dim doorIndex As Integer

        If wordList.Length >= 4 Then

            helperFunctions.StringToInteger(wordList(3), doorIndexOffset)
            doorIndex = doorIndexOffset - 1

            If doorIndex < 0 Or doorIndex > 6 Then

                debugInformation.Progress(fDebugWindow.Level.ERR, 1707, "Invalid door Index returned on movement complete message", True)

            Else

                If wordList(2) = "open" Then

                    SetPanelSwitch(State.ST_OPEN, doorIndex, Drawing.Color.Red)
                    SendApplicationMessage(Message.FRD_OPEN_OK, doorIndex)

                ElseIf wordList(2) = "close" Then

                    doorClosed(doorIndex) = True
                    SetPanelSwitch(State.ST_CLOSE, doorIndex, Drawing.Color.Red)
                    SendApplicationMessage(Message.FRD_CLOSE_OK, doorIndex)

                End If

            End If
        End If

    End Sub

#Region "Switch Information"

    ' SetSwitches 
    ' bytes have been recieved on the serial port
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetSwitches(ByVal openSwitchState As Integer, ByVal closeSwitchState As Integer)

        helperFunctions.SetLabelText(OpenPollLabel, openSwitchState.ToString)
        helperFunctions.SetLabelText(ClosePollLabel, closeSwitchState.ToString)

        If openSwitchState <> lastOpenSwitchState Then

            SetPanelSwitch(State.ST_OPEN, 0, IIf((openSwitchState And 1) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))
            SetPanelSwitch(State.ST_OPEN, 1, IIf((openSwitchState And 2) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))
            SetPanelSwitch(State.ST_OPEN, 2, IIf((openSwitchState And 4) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))
            SetPanelSwitch(State.ST_OPEN, 3, IIf((openSwitchState And 8) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))
            SetPanelSwitch(State.ST_OPEN, 4, IIf((openSwitchState And 16) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))
            SetPanelSwitch(State.ST_OPEN, 5, IIf((openSwitchState And 32) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))
            SetPanelSwitch(State.ST_OPEN, 6, IIf((openSwitchState And 64) <> 0, Drawing.Color.Maroon, Drawing.Color.Red))

            lastOpenSwitchState = openSwitchState

        End If

        If closeSwitchState <> lastClosedSwitchState Then

            doorClosed(0) = IIf(closeSwitchState And 1, False, True)
            doorClosed(1) = IIf(closeSwitchState And 2, False, True)
            doorClosed(2) = IIf(closeSwitchState And 4, False, True)
            doorClosed(3) = IIf(closeSwitchState And 8, False, True)
            doorClosed(4) = IIf(closeSwitchState And 16, False, True)
            doorClosed(5) = IIf(closeSwitchState And 32, False, True)
            doorClosed(6) = IIf(closeSwitchState And 64, False, True)

            SetPanelSwitch(State.ST_CLOSE, 0, IIf(doorClosed(0), Drawing.Color.Red, Drawing.Color.Maroon))
            SetPanelSwitch(State.ST_CLOSE, 1, IIf(doorClosed(1), Drawing.Color.Red, Drawing.Color.Maroon))
            SetPanelSwitch(State.ST_CLOSE, 2, IIf(doorClosed(2), Drawing.Color.Red, Drawing.Color.Maroon))
            SetPanelSwitch(State.ST_CLOSE, 3, IIf(doorClosed(3), Drawing.Color.Red, Drawing.Color.Maroon))
            SetPanelSwitch(State.ST_CLOSE, 4, IIf(doorClosed(4), Drawing.Color.Red, Drawing.Color.Maroon))
            SetPanelSwitch(State.ST_CLOSE, 5, IIf(doorClosed(5), Drawing.Color.Red, Drawing.Color.Maroon))
            SetPanelSwitch(State.ST_CLOSE, 6, IIf(doorClosed(6), Drawing.Color.Red, Drawing.Color.Maroon))

            lastClosedSwitchState = closeSwitchState

        End If

    End Sub

#End Region

    ' DataRecieved
    ' bytes have been recieved on the serial port
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DataRecieved(ByVal sender As Object, ByVal e As IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort.DataReceived

        Dim messageBlock As String = ""
        Dim completeMessage As String
        Dim terminationPosition As Integer
        Dim messageChecksum As String = ""
        Dim messageContent As String = ""

        Try
            messageBlock = SerialPort.ReadExisting()

        Catch ex As Exception
            helperFunctions.AddToListBox(ProgressList, "   " & ex.ToString)
        End Try

        messageBuffer = messageBuffer & messageBlock
        terminationPosition = messageBuffer.IndexOf(Chr(13))

        While terminationPosition <> -1

            ' shuffle the queue
            completeMessage = RemoveControlChars(messageBuffer.Substring(0, terminationPosition).Trim()) 'RemoveControlChars(messageBuffer.Substring(0, terminationPosition).Trim())
            messageBuffer = messageBuffer.Substring(terminationPosition + 1)

            terminationPosition = messageBuffer.IndexOf(Chr(13))

            ProcessMessage(completeMessage.Trim)
        End While

    End Sub

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
    Public Sub AddCallback(ByVal dataCallbackFunction As FridgeEventCallback)

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


    ' SendMessage
    ' send a message to the control board
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function SendMessage(ByVal message As String, ByVal addToProgress As Boolean) As Boolean

        '  Dim message As String
        Dim result As Boolean = False

        Try
            '     message = RemoveControlChars(rawMessage)

            If SerialPort.IsOpen Then

                SerialPort.WriteLine(message)
                result = True

                If addToProgress Then
                    helperFunctions.AddToListBox(ProgressList, ">  " & message)

                End If

            End If

        Catch ex As Exception
            helperFunctions.AddToListBox(ProgressList, "   " & ex.ToString)
        End Try

        Return result

    End Function

    ' SetPanelSwitch
    ' set the switch indicator
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetPanelSwitch(ByVal switchBlock As State, ByVal switchIndex As Integer, ByVal newColour As System.Drawing.Color)

        Select Case switchBlock

            Case State.ST_OPEN

                If (switchIndex = 0) Then
                    helperFunctions.SetPanelColour(OpenPanel1, newColour)
                ElseIf (switchIndex = 1) Then
                    helperFunctions.SetPanelColour(OpenPanel2, newColour)
                ElseIf (switchIndex = 2) Then
                    helperFunctions.SetPanelColour(OpenPanel3, newColour)
                ElseIf (switchIndex = 3) Then
                    helperFunctions.SetPanelColour(OpenPanel4, newColour)
                ElseIf (switchIndex = 4) Then
                    helperFunctions.SetPanelColour(OpenPanel5, newColour)
                ElseIf (switchIndex = 5) Then
                    helperFunctions.SetPanelColour(OpenPanel6, newColour)
                ElseIf (switchIndex = 6) Then
                    helperFunctions.SetPanelColour(OpenPanel7, newColour)
                End If

            Case State.ST_CLOSE

                If (switchIndex = 0) Then
                    helperFunctions.SetPanelColour(ClosedPanel1, newColour)
                ElseIf (switchIndex = 1) Then
                    helperFunctions.SetPanelColour(ClosedPanel2, newColour)
                ElseIf (switchIndex = 2) Then
                    helperFunctions.SetPanelColour(ClosedPanel3, newColour)
                ElseIf (switchIndex = 3) Then
                    helperFunctions.SetPanelColour(ClosedPanel4, newColour)
                ElseIf (switchIndex = 4) Then
                    helperFunctions.SetPanelColour(ClosedPanel5, newColour)
                ElseIf (switchIndex = 5) Then
                    helperFunctions.SetPanelColour(ClosedPanel6, newColour)
                ElseIf (switchIndex = 6) Then
                    helperFunctions.SetPanelColour(ClosedPanel7, newColour)
                End If

        End Select

    End Sub


    ' SendApplicationMessage
    ' notify the application that something has happenned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendApplicationMessage(ByVal eventCode As Message, ByVal doorIndex As Integer)

        If Not IsNothing(eventCallbackDelegate) Then

            For Each callbackDelegate As FridgeEventCallback In eventCallbackDelegate

                If Not IsNothing(callbackDelegate) Then
                    callbackDelegate.Invoke(eventCode, doorIndex)
                End If

            Next
        End If

    End Sub

    ' RequiredCloseCheck_CheckedChanged
    ' activate/deactivate a fridge in the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub RequiredCloseCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RequiredCloseCheck1.CheckedChanged, RequiredCloseCheck2.CheckedChanged, RequiredCloseCheck3.CheckedChanged, RequiredCloseCheck4.CheckedChanged, RequiredCloseCheck5.CheckedChanged, RequiredCloseCheck6.CheckedChanged, RequiredCloseCheck7.CheckedChanged
        settingsManager.SetValue("UseDoor" & sender.Tag, sender.Checked)
    End Sub

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

End Class

' class cSerialManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cFridgeManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared fridgeManager As fFridgeManager = Nothing

    Public Function GetManager() As fFridgeManager

        If IsNothing(fridgeManager) Then

            fridgeManager = New fFridgeManager

        End If

        Return fridgeManager

    End Function

End Class

