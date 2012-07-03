'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing

Imports DebugWindow
Imports HelperFunctions
Imports FridgeManager
Imports SerialManager
Imports SettingsManager

' the serial manager form
' event driven serial IO
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fMechInterface

    ' Dll declarations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Declare Sub WdtEnable Lib "wdtdll.dll" Alias "WdtEnable" (ByVal type As Integer, ByVal value As Integer)

    Public Declare Function IoReadByte Lib "wdtdll.dll" Alias "IoReadByte" (ByVal portAddress As Short) As Byte
    Public Declare Sub IoWriteByte Lib "wdtdll.dll" Alias "IoWriteByte" (ByVal portAddress As Short, ByVal portValue As Byte)

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Message

        MECH_DELIVERY_STARTS
        MECH_FRIDGE_OPEN
        MECH_FRIDGE_CLOSE
        MECH_VEND_ACTIVITY
        MECH_PRODUCT_OFFERED
        MECH_DELIVERY_FAILS
        MECH_DELIVERY_COMPLETES
        MECH_INITIALISING
        MECH_READY
        MECH_PC_SERVICE_REQUEST
        MECH_VMC_SERVICE_ABORT
        MECH_DOOR_CLOSE_DUE
        MECH_POSITION
        MECH_VEND_INFO
        MECH_VMC_DOOR_FORCED

    End Enum

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Structure VendInfoStruct

        Dim horizontalPosition As Integer
        Dim verticalPosition As Integer
        Dim verticalDeliveryOffset As Integer
        Dim pickAttempts As Integer
        Dim fridgeDoor As Integer

    End Structure

    ' Constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Const NO_DOOR = -1
    Const MINUTES = 0
    Const SECONDS = 1

    Const GPIO_PORT = &H801

    ' Delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub MechanismEventCallback(ByVal eventCode As Message, ByVal integerValue1 As Integer, ByVal integerValue2 As Integer)

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private eventCallbackDelegate() As MechanismEventCallback

    Private nextVendInfo As VendInfoStruct
    '    Private currentMechState As MechState = MechState.MS_IDLE
    Private vmcVersion As String
    Private moduleRunning As Boolean = True
    Private allowSmallVerticalMove As Boolean = False
    Private currentPosition As Point
    Private targetMovePosition As Point

    ' Managers
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private fridgeManager As fFridgeManager
    Private fridgeManagerFactory As cFridgeManagerFactory = New cFridgeManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' Initialise
    ' connect to the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        Dim vmcSerialPort As String

        ' get the managers
        debugInformation = debugInformationFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        fridgeManager = fridgeManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

        ' populate the next vend info structure
        nextVendInfo.horizontalPosition = 0
        nextVendInfo.verticalPosition = 0
        nextVendInfo.verticalDeliveryOffset = 0
        nextVendInfo.pickAttempts = 0
        nextVendInfo.fridgeDoor = 0

        ' fire up the the serial port 
        vmcSerialPort = settingsManager.GetValue("VmcSerialPort")

        If serialManager.Connect(vmcSerialPort) Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1020, "Communication on COM Port " & vmcSerialPort & " Established", True)
        Else
            debugInformation.Progress(fDebugWindow.Level.ERR, 1021, "Communication on COM Port Failed", True)
        End If

        serialManager.AddCallback(AddressOf SerialPortEvent)

        If settingsManager.GetValue("SerialDebug") Then
            serialManager.Show()
        End If

        ' fire up the fridge management
        fridgeManager.Initialise()
        fridgeManager.AddCallback(AddressOf FridgeEvent)

        If settingsManager.GetValue("FridgeDebug") Then
            fridgeManager.Show()
        End If

        ' do we want to show this window ??
        If settingsManager.GetValue("MechanismDebug") Then
            Show()
        End If

        ' initialise custom components..
        VmcSettings.Initialise()
        AutoHome.Initialise()
        ServiceState.Initialise()
        MechTest.Initialise()
        PowerMonitor.Initialise()
        SerialLoader.Initialise()

        ' enquire about the vmc version
        serialManager.SendMessage("GETVERSION")

    End Sub


    ' RunFan & RunCompressor
    ' issue the FAN or COMPRESSOR commands
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function RunFan(ByVal newState As Boolean) As Boolean
        serialManager.SendMessage(IIf(newState, "FAN 1", "FAN 0"))
    End Function

    Public Function RunCompressor(ByVal newState As Boolean) As Boolean
        serialManager.SendMessage(IIf(newState, "COMPRESSOR 1", "COMPRESSOR 0"))
    End Function

    ' MechTest_Enter & MechTest_Leave
    ' start and stop the switch test..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub MechTest_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MechTestTab.Enter
        MechTest.StartTest()
    End Sub

    Private Sub MechTest_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MechTestTab.Leave
        MechTest.StopTest()
    End Sub

    ' FridgeEvent
    ' things are happenning on the fridge
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub FridgeEvent(ByVal eventCode As fFridgeManager.Message, ByVal doorParameter As Integer)

        Select Case eventCode

            Case fFridgeManager.Message.FRD_COMMS_OK

            Case fFridgeManager.Message.FRD_COMMS_FAIL

            Case fFridgeManager.Message.FRD_CLOSE_FAIL
                serialManager.SendMessage("WAYPOINT NO")

                SendApplicationMessage(Message.MECH_FRIDGE_CLOSE, doorParameter, 0)

            Case fFridgeManager.Message.FRD_CLOSE_OK
                serialManager.SendMessage("WAYPOINT YES")

                SendApplicationMessage(Message.MECH_FRIDGE_CLOSE, doorParameter, 1)

            Case fFridgeManager.Message.FRD_OPEN_FAIL
                serialManager.SendMessage("WAYPOINT NO")

                SendApplicationMessage(Message.MECH_FRIDGE_OPEN, doorParameter, 0)

            Case fFridgeManager.Message.FRD_OPEN_OK
                serialManager.SendMessage("WAYPOINT YES")

                SendApplicationMessage(Message.MECH_FRIDGE_OPEN, doorParameter, 1)

            Case fFridgeManager.Message.FRD_TURN_FAN_ON
                serialManager.SendMessage("FAN 1")

            Case fFridgeManager.Message.FRD_TURN_FAN_OFF
                serialManager.SendMessage("FAN 0")

            Case fFridgeManager.Message.FRD_TURN_COMPRESSOR_ON
                serialManager.SendMessage("COMPRESSOR 1")

            Case fFridgeManager.Message.FRD_TURN_COMPRESSOR_OFF
                serialManager.SendMessage("COMPRESSOR 0")

        End Select

    End Sub

    ' VendProduct
    ' send a message to the vmc to start vending
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub VendProduct(ByVal horizontalPosition As Integer, ByVal verticalPosition As Integer, ByVal verticalDeliveryOffset As Integer, ByVal pickAttempts As Integer, ByVal fridgeDoor As Integer)

        ' record the information that we will need to complete the vend
        nextVendInfo.verticalPosition = verticalPosition
        nextVendInfo.horizontalPosition = horizontalPosition
        nextVendInfo.verticalDeliveryOffset = verticalDeliveryOffset
        nextVendInfo.pickAttempts = pickAttempts
        nextVendInfo.fridgeDoor = fridgeDoor

        SendVendCommand()

    End Sub

    ' SendVendCommand
    ' issue the command to vend to the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SendVendCommand()

        Dim command As String
        Dim horizontalPosition As String = Format(nextVendInfo.horizontalPosition, "####0000")
        Dim verticalPosition As String = Format(nextVendInfo.verticalPosition, "####0000")
        Dim verticalDeliveryOffset As String = Format(nextVendInfo.verticalDeliveryOffset, "##00")
        Dim pickAttempts As String = Format(nextVendInfo.pickAttempts, "##00")

        ' issue the command to vend to the VMC
        command = "VEND " & horizontalPosition & "," & verticalPosition & "," & verticalDeliveryOffset & "," & pickAttempts
        serialManager.SendMessage(command)
        '    currentMechState = MechState.MS_VENDING
        debugInformation.Progress(fDebugWindow.Level.INF, 1190, "Vend command sent to VMC: " & command, True)

    End Sub

    ' TestPosition
    ' send a message to the vmc to do a move
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub TestPosition(ByVal horizontalPosition As Integer, ByVal verticalPosition As Integer, ByVal homeFirst As Integer, ByVal fridgeDoor As Integer)

        Dim command As String

        nextVendInfo.verticalPosition = verticalPosition
        nextVendInfo.horizontalPosition = horizontalPosition
        nextVendInfo.fridgeDoor = fridgeDoor

        ' issue the command to vend to the VMC
        command = "TESTPOS " & Format(horizontalPosition, "####0000") & "," & Format(verticalPosition, "####0000") & "," & Format(homeFirst, "#0")

        serialManager.SendMessage(command)

        '  currentMechState = MechState.MS_MOVING
        debugInformation.Progress(fDebugWindow.Level.INF, 1192, "Test position command sent to the VMC: " & command, True)

    End Sub

    Public Sub MoveHead(ByVal xPosition As Integer, ByVal yPosition As Integer)

        targetMovePosition.X = xPosition
        targetMovePosition.Y = yPosition

        serialManager.SendMessage("MOVEHEAD " & Format(xPosition, "####0000") & "," & Format(yPosition, "####0000"))

    End Sub

    ' SendMessageToVMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendMessageToVMC(ByVal vmcMessage As String)

        serialManager.SendMessage(vmcMessage)

    End Sub

    ' SendDoorCloseCommand
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendDoorCloseCommand()

        serialManager.SendMessage("MOVEDOOR 0")

    End Sub

    ' MoveHeadSafe
    ' get clear of the fridgedoor - move horizontally only till you get to the horizontal home, then close the fridge door.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub MoveHeadSafe()
        serialManager.SendMessage("MOVEHEADSAFE")
    End Sub

    ' SelectionChanges
    ' when the user clicks on a setting we may need to give a line or two of explaination
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SelectionChanges(ByVal explainationTest As String) Handles VmcSettings.SelectionChanges

        helperFunctions.SetLabelText(ExplainationLabel, explainationTest)

    End Sub

    ' Shutdown
    ' stop all the processes held by the mech interface
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Shutdown()
        serialManager.Disconnect()
        fridgeManager.Shutdown()
        MechTest.Shutdown()
    End Sub

    ' isOkayToVend
    ' are we okay to vvend ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function IsOkayToVend() As Boolean

        Return ServiceState.IsOkayToVend()

    End Function


    ' SerialPortEvent
    ' event driven incomming serial messages.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = messageContent.Split("=")
            Dim parameterCount As Integer = UBound(parameterList)
            Dim doorCountDown As Integer
            Dim infoCode As Integer

            Select Case parameterList(0)

                   Case "INFO"

                    ' send the info code
                    infoCode = Val(parameterList(1))

                    SendApplicationMessage(Message.MECH_VEND_INFO, infoCode, 0)

                    ' then log general activity
                    SendApplicationMessage(Message.MECH_VEND_ACTIVITY, 0, 0)

                Case "VENDFAILED"

                    debugInformation.Progress(fDebugWindow.Level.WRN, 1070, "VMC: VENDFAILED", True)
                    SendApplicationMessage(Message.MECH_DELIVERY_FAILS, 0, 0)

                Case "VENDOK"

                    debugInformation.Progress(fDebugWindow.Level.INF, 1071, "VMC: VENDOK", True)
                    SendApplicationMessage(Message.MECH_DELIVERY_COMPLETES, 0, 0)

                Case "VENDING"
                    debugInformation.Progress(fDebugWindow.Level.INF, 1191, "VMC: Vend phase  --" & parameterList(1), True)
                    If parameterList(1) = "6" Then _
                        SendApplicationMessage(Message.MECH_PRODUCT_OFFERED, 0, 0)

                Case "SAFEVERTICAL"

                    If settingsManager.GetValue("HasFridge") Then


                        If fridgeManager.DoorsSafe() Then
                            serialManager.SendMessage("SAFEVERTICAL YES")

                        ElseIf allowSmallVerticalMove Then

                            allowSmallVerticalMove = False

                            If System.Math.Abs(currentPosition.Y - targetMovePosition.Y) < 4 Then
                                serialManager.SendMessage("SAFEVERTICAL YES")

                            Else

                                serialManager.SendMessage("SAFEVERTICAL NO")
                            End If

                        Else

                            serialManager.SendMessage("SAFEVERTICAL NO")
                        End If

                    Else

                        serialManager.SendMessage("SAFEVERTICAL YES")
                    End If

                Case "-CHK"
                    debugInformation.Progress(fDebugWindow.Level.WRN, 1072, "VMC: CHK", True)

                Case "-SVC"
                    debugInformation.Progress(fDebugWindow.Level.INF, 1073, "VMC: -SVC", True)

                Case "VERSION"
                        vmcVersion = parameterList(1)
                        helperFunctions.SetFormText(Me, "Mechanism Interface, VMC:" & vmcVersion)

                Case "WAYPOINT"
                        If parameterList(1) = "31" Then

                            If fridgeManager.LastDoorOperated() = 0 Then
                                serialManager.SendMessage("WAYPOINT YES")

                            Else
                                fridgeManager.CloseDoor(fFridgeManager.LAST_DOOR_OPERATED)
                            End If


                            ' do we need to open or close a door ?
                        ElseIf settingsManager.GetValue("HasFridge") AndAlso nextVendInfo.fridgeDoor <> NO_DOOR Then

                            ' open / close the correct door.
                            Select Case parameterList(1)

                                Case "1", "12"
                                    fridgeManager.OpenDoor(nextVendInfo.fridgeDoor)

                                Case "2", "3", "11", "21"
                                    fridgeManager.CloseDoor(nextVendInfo.fridgeDoor)

                                Case "31"
                                    fridgeManager.CloseDoor(fFridgeManager.LAST_DOOR_OPERATED)

                            End Select

                        Else

                            serialManager.SendMessage("WAYPOINT YES")                        ' no, then tell the VMC to keep going..

                        End If

                        ' the red button has been pressed
                Case "SERVICE"
                        SendApplicationMessage(Message.MECH_PC_SERVICE_REQUEST, 0, 0)

                        ' thye vmc has cancelled its config menu, because of incomming comms
                Case "SERVICEABORT"
                        SendApplicationMessage(Message.MECH_VMC_SERVICE_ABORT, 0, 0)

                Case "DOORFORCED"
                        SendApplicationMessage(Message.MECH_VMC_DOOR_FORCED, 0, 0)

                Case "DOORCLOSEDUE"

                        If (helperFunctions.StringToInteger(parameterList(1), doorCountDown)) Then
                            SendApplicationMessage(Message.MECH_DOOR_CLOSE_DUE, doorCountDown, 0)
                        End If

                Case "RACK"
                        Dim axisList() As String = Split(parameterList(1), ",")

                        If (axisList(0) = "?" Or axisList(1) = "?") Then

                            currentPosition.X = -1
                            currentPosition.Y = -1
                        Else

                            helperFunctions.StringToInteger(axisList(0), currentPosition.X)
                            helperFunctions.StringToInteger(axisList(1), currentPosition.Y)
                        End If

                        SendApplicationMessage(Message.MECH_POSITION, currentPosition.X, currentPosition.Y)

                Case "INITIALIZING"
                        SendApplicationMessage(Message.MECH_INITIALISING, 0, 0)

                Case "READY"
                    SendApplicationMessage(Message.MECH_READY, 0, 0)

                    'we can ignore these
                Case Else

            End Select

        End If

    End Sub

    ' AllowASmallVerticalMove
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub AllowASmallVerticalMove()

        allowSmallVerticalMove = True

    End Sub

    ' AddCallback
    ' record which function needs to know about incomming data..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub AddCallback(ByVal dataCallbackFunction As MechanismEventCallback)

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
    Private Sub SendApplicationMessage(ByVal eventCode As Message, ByVal parameter1 As Integer, ByVal parameter2 As Integer)

        If Not IsNothing(eventCallbackDelegate) Then

            For Each callbackDelegate As MechanismEventCallback In eventCallbackDelegate

                If Not IsNothing(callbackDelegate) Then
                    callbackDelegate.Invoke(eventCode, parameter1, parameter2)
                End If

            Next
        End If

    End Sub

    ' SerialManagerForm_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialManagerForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub


    ' AutoHome_HomePositionConfirmed
    ' the user has confirmed that the home position is valid
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AutoHome_HomePositionConfirmed(ByVal horizontalPosition As Integer, ByVal verticalPosition As Integer) Handles AutoHome.HomePositionConfirmed

        VmcSettings.UpdateValueOnGrid("HORZ_HP", horizontalPosition, uVmcSettings.GridColumn.CLM_WRITE)
        VmcSettings.UpdateValueOnGrid("VERT_HP", verticalPosition, uVmcSettings.GridColumn.CLM_WRITE)

    End Sub

    ' fMechInterface_VisibleChanged
    ' the switch test needs to be disabled on hide and reenabled if visible
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub fMechInterface_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged

        If Visible Then

            Select Case GeneralTab.SelectedIndex

                Case 3
                    MechTest.StartTest()
            End Select

        Else
            MechTest.StopTest()
        End If

    End Sub

    ' StartWatchDog & StopWatchDog
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StartWatchDog(ByVal delayInSeconds As Integer)

        If settingsManager.GetValue("WatchdogPcEnable") Then

            If moduleRunning Then

                If settingsManager.GetValue("Has8712") Then
                    WdtEnable(SECONDS, delayInSeconds)
                    debugInformation.Progress(fDebugWindow.Level.INF, 1074, "Kicked Watchdog: " & delayInSeconds.ToString, False)

                End If

            Else
                If settingsManager.GetValue("Has8712") Then
                    WdtEnable(0, 0)
                    debugInformation.Progress(fDebugWindow.Level.INF, 1075, "Kicked Watchdog (module not running): Stopped", True)

                End If
            End If

        End If

    End Sub

    Public Sub StopWatchDog()

        If settingsManager.GetValue("WatchdogPcEnable") Then

            moduleRunning = False
            If settingsManager.GetValue("Has8712") Then
                WdtEnable(0, 0)
                debugInformation.Progress(fDebugWindow.Level.INF, 1076, "Stopped Watchdog", True)
            End If

        End If

    End Sub

    ' EnableWatchdog and WatchdogIsEnabled
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub EnableWatchdog(ByVal newState As Boolean)

        moduleRunning = newState

    End Sub

    Public Function WatchdogIsEnabled()

        Return moduleRunning

    End Function

    Public Function GetGpioBit(ByVal bitIndex As Integer) As Boolean

        Dim portValue As Byte = 0

        If settingsManager.GetValue("WatchdogPcEnable") Then

            If settingsManager.GetValue("Has8712") Then
                portValue = IoReadByte(GPIO_PORT)
            End If

            Return (portValue And (2 ^ bitIndex)) <> 0

        End If

    End Function

    Public Sub SetGpioBit(ByVal bitIndex As Integer, ByVal newState As Boolean)

        Dim portValue As Byte
        Dim newValue

        If settingsManager.GetValue("WatchdogPcEnable") Then

            If settingsManager.GetValue("Has8712") Then

                portValue = IoReadByte(GPIO_PORT)

                If newState Then

                    newValue = portValue Or (2 ^ bitIndex)
                    IoWriteByte(GPIO_PORT, newValue)

                Else

                    newValue = portValue And (&HFF - (2 ^ bitIndex))
                    IoWriteByte(GPIO_PORT, newValue)

                End If

            End If

        End If
    End Sub

End Class

' class cMechInterfaceFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cMechInterfaceFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared mechInterface As fMechInterface = Nothing

    Public Function GetManager() As fMechInterface

        If IsNothing(mechInterface) Then

            mechInterface = New fMechInterface

        End If

        Return mechInterface

    End Function

End Class


