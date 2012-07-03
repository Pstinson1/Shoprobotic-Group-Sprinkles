'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Timers
Imports System.Threading
Imports System.Windows.Forms

Imports DebugWindow
Imports HelperFunctions
Imports SerialManager
Imports SettingsManager

Public Class uServiceState

    Enum ServiceState

        VMC_INITIALISING
        VMC_READY
        VMC_SERVICE_MODE
        VMC_SERVICE_REQUIRED
        VMC_BUSY
        VMC_DEAD
        VMC_NOT_USING_HEARTBEAT

    End Enum

    ' constants
    Private Const INITIALISATION_INTERVAL = 4000

    ' managers
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory

    ' varaibles
    Private currentVmcStatus As ServiceState = ServiceState.VMC_DEAD
    Private outstandingHeatbeats As Integer
    Private displayedServiceFlags As Integer = -1

    Private heartbeatTimer As System.Timers.Timer

    Private heartbeatLiveInterval As Integer
    Private heartbeatLiveAllowedOutstanding As Integer
    Private heartbeatDeadInterval As Integer


    ' Initialise
    ' set up the control
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' grab the managers
        debugInformation = debugInformationFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

        m8712.Initialise()

        ' pull the reset pin low
        If settingsManager.GetValue("WatchdogVmcEnable") Then

            m8712.SetGpioBit(1, 0)
        End If

        heartbeatLiveInterval = settingsManager.GetValue("HeartbeatLiveInterval")
        heartbeatLiveAllowedOutstanding = settingsManager.GetValue("HeartbeatAllowedOutstanding")
        heartbeatDeadInterval = settingsManager.GetValue("HeartbeatDeadInterval")

        ' sort out the serial connection
        serialManager.AddCallback(AddressOf SerialPortEvent)

        ' create and start up the timer object
        heartbeatTimer = New System.Timers.Timer()
        AddHandler heartbeatTimer.Elapsed, AddressOf TimerEvent

        ' just in case heartbeat live interval is invalid go for 1/2 second.
        If heartbeatLiveInterval = 0 Then _
             heartbeatLiveInterval = 2000

        If heartbeatLiveInterval = 0 Then _
              heartbeatLiveInterval = 60000

        heartbeatTimer.Interval = heartbeatLiveInterval
        heartbeatTimer.Enabled = True

        ' are we going to bother with a heartbeat ?
        If settingsManager.GetValue("HeartbeatEnable") Then

            SetVmcStatus(ServiceState.VMC_READY)

        Else

            ' no heartbeat, so don't intercept the serial messages or start the timer.
            SetVmcStatus(ServiceState.VMC_NOT_USING_HEARTBEAT)

        End If

    End Sub

    ' IsOkayToVend
    ' are we okay to vvend ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function IsOkayToVend() As Boolean

        If (currentVmcStatus = ServiceState.VMC_NOT_USING_HEARTBEAT) OrElse (currentVmcStatus = ServiceState.VMC_READY) Then
            Return True
        Else
            Return False
        End If

    End Function

    ' SerialPortEvent
    ' process messages from the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        Dim statusCode As Integer
        Dim informationCode As Integer

        ' is the heartbeat enabled ?
        If settingsManager.GetValue("HeartbeatEnable") Then

            If eventCode = fSerialManager.Message.COM_RECV Then

                Dim parameterList As String() = Split(messageContent, "=")
                Dim parameterCount As Integer = UBound(parameterList)

                Select Case parameterList(0)

                    Case "INITIALIZING"

                        SetVmcStatus(ServiceState.VMC_INITIALISING)
                        heartbeatTimer.Interval = INITIALISATION_INTERVAL
                        outstandingHeatbeats = 0

                    Case "READY"

                        SetVmcStatus(ServiceState.VMC_READY)
                        heartbeatTimer.Interval = heartbeatLiveInterval
                        outstandingHeatbeats = 0

                    Case "VENDOK", "VENDFAILED"

                        SetVmcStatus(ServiceState.VMC_READY)

                    Case "-SVC"

                        SetVmcStatus(ServiceState.VMC_SERVICE_REQUIRED)
                        outstandingHeatbeats = 0
                        heartbeatTimer.Interval = 2000

                    Case "FITTOVEND", "HEALTH"

                        helperFunctions.StringToInteger(parameterList(1), statusCode)
                        outstandingHeatbeats = 0
                        heartbeatTimer.Interval = heartbeatLiveInterval

                        If statusCode = 0 Then

                            SetVmcStatus(ServiceState.VMC_READY)
                        Else

                            SetVmcStatus(ServiceState.VMC_SERVICE_REQUIRED)
                        End If

                        UpdateServiceFlags(statusCode)

                    Case "INFO"

                        helperFunctions.StringToInteger(parameterList(1), informationCode)

                        Select Case informationCode

                            Case 7
                                SetVmcStatus(ServiceState.VMC_SERVICE_MODE)

                            Case 8
                                SetVmcStatus(ServiceState.VMC_READY)

                            Case 185
                                SetVmcStatus(ServiceState.VMC_READY)

                            Case 186
                                SetVmcStatus(ServiceState.VMC_BUSY)

                        End Select

                End Select

            End If

        End If


    End Sub

    ' TimerEvent
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub TimerEvent(ByVal obj As Object, ByVal e As ElapsedEventArgs)

        ' is the heartbeat enabled ?
        If settingsManager.GetValue("HeartbeatEnable") Then

            Select Case currentVmcStatus

                Case ServiceState.VMC_SERVICE_REQUIRED, ServiceState.VMC_DEAD, ServiceState.VMC_READY

                    ' have we missed any heartbeat responses
                    If outstandingHeatbeats > 0 Then _
                        debugInformation.Progress(fDebugWindow.Level.INF, 1400, "Heatbeat status request: " & outstandingHeatbeats & " outstanding", True)

                    ' request the status or the VMC
                    serialManager.SendMessage("GETHEALTH")

                    outstandingHeatbeats = outstandingHeatbeats + 1

                    ' have we not got a response for a while, better reset the VMC
                    If outstandingHeatbeats > heartbeatLiveAllowedOutstanding Then

                        SetVmcStatus(ServiceState.VMC_DEAD)

                        heartbeatTimer.Interval = heartbeatDeadInterval

                        outstandingHeatbeats = 0
                        HardwareReset()

                    End If

                Case ServiceState.VMC_NOT_USING_HEARTBEAT

                    SetVmcStatus(ServiceState.VMC_DEAD)

                Case ServiceState.VMC_INITIALISING
                Case ServiceState.VMC_SERVICE_MODE

            End Select

        Else

            SetVmcStatus(ServiceState.VMC_NOT_USING_HEARTBEAT)

        End If


    End Sub

    ' UpdateServiceFlags
    ' tell the user which service flags are set.
    ' #define 	    SVC_ARM		    	  		    	0b0000000000000001		/* peripheral */
    ' #define 		SVC_RACK				            0b0000000000000010
    ' #define 		SVC_DOOR				            0b0000000000000100	
    ' #define 		SVC_PC					            0b0000000000001000	
    ' #define 		SVC_VACUUM				        0b0000000000010000	
    ' #define 		SVC_PRODUCTSENSOR		0b0000000000100000	

    ' #define 		SVC_OBSTRUCTION		    0b0000000100000000		/* rationale */
    ' #define 		SVC_HOMINGFAILED		    0b0000001000000000	
    ' #define 		SVC_MOVEFAILED			    0b0000010000000000
    ' #define		SVC_MISSINGCOMMS		    0b0000100000000000
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateServiceFlags(ByVal serviceFlags As Integer)

        If displayedServiceFlags <> serviceFlags Then

            helperFunctions.ClearListBox(ServiceFlagsListBox)

            ' mechanism
            If (serviceFlags And &H1) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "PICKING ARM")
            If (serviceFlags And &H2) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "RACK MECHANISM")
            If (serviceFlags And &H4) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "SECURE DOOR")
            If (serviceFlags And &H8) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "PC COMMS INTERFACE")
            If (serviceFlags And &H10) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "VACUUM")
            If (serviceFlags And &H20) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "PRODUCT SENSOR")

            ' rationale
            If (serviceFlags And &H100) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "POSSIBLE PRODUCT OBSTRUCTION")
            If (serviceFlags And &H200) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "HOME FAILED")
            If (serviceFlags And &H400) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "MOVE FAILED")
            If (serviceFlags And &H800) Then helperFunctions.AddToListBox(ServiceFlagsListBox, "EXPECTED COMMS DID NOT ARRIVE")

            displayedServiceFlags = serviceFlags

        End If

    End Sub

    ' SetVmcStatus
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetVmcStatus(ByVal newVmcStatus As ServiceState)

        helperFunctions.SetListBoxSelection(StateListBox, newVmcStatus)

        If newVmcStatus <> currentVmcStatus Then

            If newVmcStatus = ServiceState.VMC_SERVICE_REQUIRED Then
                helperFunctions.SetLabelText(StatusLabel, "Unable to vend, service required")
            Else
                helperFunctions.SetLabelText(StatusLabel, "Okay to vend")
            End If

            currentVmcStatus = newVmcStatus

        End If

    End Sub

    ' GetVmcStatus
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetVmcStatus() As ServiceState

        Return currentVmcStatus

    End Function

    ' HardwareReset
    ' fire the reset pin high for a short while - GP20
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub HardwareReset()

        If settingsManager.GetValue("WatchdogVmcEnable") Then

            debugInformation.Progress(fDebugWindow.Level.ERR, 1401, "Forcing VMC Reset", True)

            m8712.SetGpioBit(1, True)
            Thread.Sleep(50)
            m8712.SetGpioBit(1, False)

            debugInformation.Progress(fDebugWindow.Level.ERR, 1402, "VMC Reset Complete", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.ERR, 1403, "Not forcing VMC Reset", True)
        End If


    End Sub

    ' SoftwareReset
    ' fire the reset pin high for a short while - GP20
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SoftwareReset()
        serialManager.SendMessage("RESET")
    End Sub

    ' ResetServiceRequired
    ' tell the VMC to remove the service required flag
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ResetServiceRequired()
        serialManager.SendMessage("SETPRM VEND_SR 0")
    End Sub

    ' ServiceButton_Click, SoftwareResetButton_Click, HardwareResetButton_Click
    ' send a reset message to the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SoftwareResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoftwareResetButton.Click
        SoftwareReset()
    End Sub

    Private Sub HardwareResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HardwareResetButton.Click
        HardwareReset()
    End Sub

    Private Sub ServiceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServiceButton.Click
        ResetServiceRequired()
    End Sub

    Private Sub StateListBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles StateListBox.Resize

    End Sub

    Private Sub StateListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StateListBox.SelectedIndexChanged

    End Sub
End Class
