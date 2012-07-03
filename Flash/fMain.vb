'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

' Imports
' ----------------------------------------------------------------------------------------------------------------------------------------------------
#Region "Imports"

Imports System.IO
Imports System.Threading

Imports AccessPayment
Imports AttractManager
Imports DebugWindow
Imports Eport
Imports MechInterface
Imports MdbManager
Imports HelperFunctions
Imports SerialManager
Imports SettingsManager
Imports VideoManager
Imports YesPay
Imports System.Net.NetworkInformation

#End Region

Public Class fMain

#Region "Variables"

    ' variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private databaseManager As cDatabaseManager
    Private databaseManagerFactory As cDatabaseManagerFactory = New cDatabaseManagerFactory
    Private yespayManager As fYespayManager
    Private yespayManagerFactory As cYespayManagerFactory = New cYespayManagerFactory
    Private ePortManager As fEportManager
    Private ePortManagerFactory As cEportManagerFactory = New cEportManagerFactory
    Private mdbManager As fMdbManager
    Private mdbManagerFactory As cMdbManagerFactory = New cMdbManagerFactory
    Private videoManager As fVideoManager
    Private videoManagerFactory As cVideoManagerFactory = New cVideoManagerFactory
    Private mechInterface As fMechInterface
    Private mechInterfaceFactory As cMechInterfaceFactory = New cMechInterfaceFactory
    Private attractManager As fAttractManager
    Private attractManagerFactory As cAttractManagerFactory = New cAttractManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private accessPaymentManager As fAccessPayment
    Private accessPaymentManagerFactory As cAccessPaymentFactory = New cAccessPaymentFactory
    '  Private dinkeyDongle As cDinkeyDongle = New cDinkeyDongle

    Private flashProxy As Flash.External.ExternalInterfaceProxy
    Private recieptPrinter As cReceiptPrinter = New cReceiptPrinter
    Private printerStatus As cPrinterStatus = New cPrinterStatus
    Private flashMovieFilePath As String
    Private mLockMech As fLockMech
    Private vendApplicationStatus As ApplicationStatus
    Private oldApplicationStatus As ApplicationStatus
    Private swipeTimeoutCounter As Integer
    Private vendingTimeoutCounter As Integer
    Private eportTimeoutCounter As Integer
    Private activeProduct As cDatabaseManager.ProductStructure
    Private activePosition As cDatabaseManager.PositionStructure
    Private managementThread As Thread
    Private continueThread As Boolean = True
    Private startLoopDelay As Integer = 0
    Private endLoopDelay As Integer = 1000
    Private successDetermined As Boolean = False
    Private mdbTransactionSucessful As Boolean
    Private paymentType As String = ""
    Private printTimeout As Integer = 0
    Private errorMessageCountdown As Integer = 0
    Private dueWatchDogKick As Date = Now.AddSeconds(1)
    Private currentSaleId As Integer

    'Private WithEvents fRemote As RemoteControl

    'Public Shared Event DebugData(strData As String)

#End Region

#Region "Enumerations"

    ' the vend app status    
    Public Enum ApplicationStatus As Integer

        InitialiseSetCursor
        Reset
        TransferData
        Idle
        AskForSwipe
        WaitSwipe
        WaitCardPayment
        Approved
        FreeVend
        VendVMC
        WaitFridgeDoorOpen
        WaitFridgeDoorClose
        FltTransfer
        BusyVending
        ErrorMinor
        ShowErrorMessage
        Exiting

    End Enum

#End Region

#Region "Form Load & Reset Events"

    ' MainForm_Load 
    ' get the VMC to deliver a product.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
        Dim videoFrameRate As Integer
        Dim videoFrameSize As Size
        Dim paymentTypeObject As Object

        setShellReady()
        'If dinkeyDongle.ProtCheck() Then
        '    End             'terminate
        '    Return
        'End If

        ' load up the settings manager
        settingsManager = settingsManagerFactory.GetManager()
        settingsManager.Initialise(My.Settings.DatabaseConnectionString)

        ' useful functionality
        helperFunctions = helperFunctionsFactory.GetManager()

        ' add a windows hook to catch keystrokes.
        mHook.HookKeyboard(AddressOf KeyboardEvents)

        ' fire up the debug window.
        debugInformation = debugInformationFactory.GetManager()
        If settingsManager.GetValue("RemoteDebugAllowed") Then
            debugInformation.AllowDebugConnections()
        End If

        debugInformation.Progress(fDebugWindow.Level.INF, 1010, "Vend Application Started", True)

        If settingsManager.GetValue("GeneralDebug") Then
            debugInformation.Show()
        End If

        ' output the version number
        debugInformation.Progress(fDebugWindow.Level.INF, 1011, "Version: " & My.Application.Info.Version.ToString, True)

        ' create a connection to the database
        databaseManager = databaseManagerFactory.GetManager()
        databaseManager.Initialise(My.Settings.DatabaseConnectionString)

        ' create a mechanism interface
        mechInterface = mechInterfaceFactory.GetManager()
        mechInterface.Initialise()
        mechInterface.AddCallback(AddressOf MechanismEvent)

        ' locate movie in exe directory
        flashMovieFilePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + settingsManager.GetValue("MovieFileName")
        debugInformation.Progress(fDebugWindow.Level.INF, 1030, "Loading Flash :" & flashMovieFilePath, True)

        'load movie, frame 0

        ' ensure that the video path exists
        If Not File.Exists(flashMovieFilePath) Then
            debugInformation.Progress(fDebugWindow.Level.ERR, 1034, "Movie: '" & flashMovieFilePath & "' does not exist", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.INF, 1031, "Loading flash movie", True)
        End If

        AxFlash.LoadMovie(0, flashMovieFilePath)

        'create new ExternalInterface proxy
        flashProxy = New Flash.External.ExternalInterfaceProxy(AxFlash)
        debugInformation.Progress(fDebugWindow.Level.INF, 1032, "Flash proxy created", True)




        'create an addhandler delegate for incoming flash app messages
        AddHandler flashProxy.ExternalFSCommandCall, AddressOf proxy_ExternalFSCommandCall
        debugInformation.Progress(fDebugWindow.Level.INF, 1033, "Flash handler set", True)

        ' wait a second for flash to get into gear
        Do Until AxFlash.IsPlaying
            Application.DoEvents()
            Thread.Sleep(100)
        Loop

        debugInformation.Progress(fDebugWindow.Level.INF, 1035, "Flash loaded", True)

        ' sort out the video recorder
        If settingsManager.GetValue("VideoEnabled") Then

            videoFrameRate = settingsManager.GetValue("VideoFrameRate")
            videoFrameSize = settingsManager.GetValue("VideoFrameSize")
            videoManager = videoManagerFactory.GetManager()

            videoManager.Initialise()

            videoManager.AddCamera(fVideoManager.CameraListItem.PICK_HEAD_VIEW, settingsManager.GetValue("VideoDeviceNamePickHead"), settingsManager.GetValue("VideoDeviceInstancePickHead"), settingsManager.GetValue("VideoCompressorPickHead"), "Pick Head")
            videoManager.AddCamera(fVideoManager.CameraListItem.CUSTOMER_VIEW, settingsManager.GetValue("VideoDeviceNameCustomer"), settingsManager.GetValue("VideoDeviceInstanceCustomer"), settingsManager.GetValue("VideoCompressorCustomer"), "Customer")
            videoManager.AddCamera(fVideoManager.CameraListItem.MECH_VIEW, settingsManager.GetValue("VideoDeviceNameMech"), settingsManager.GetValue("VideoDeviceInstanceMech"), settingsManager.GetValue("VideoCompressorMech"), "Mechanism")

            videoManager.SetView(fVideoManager.CameraListItem.PICK_HEAD_VIEW, settingsManager.GetValue("VideoLocationPickHead"), settingsManager.GetValue("VideoSizePickHead"), videoFrameSize, videoFrameRate)
            videoManager.SetView(fVideoManager.CameraListItem.CUSTOMER_VIEW, settingsManager.GetValue("VideoLocationCustomer"), settingsManager.GetValue("VideoSizeCustomer"), videoFrameSize, videoFrameRate)
            videoManager.SetView(fVideoManager.CameraListItem.MECH_VIEW, settingsManager.GetValue("VideoLocationMech"), settingsManager.GetValue("VideoSizeMech"), videoFrameSize, videoFrameRate)

            If settingsManager.GetValue("VideoDebug") Then

                videoManager.Show()

            End If

        End If

        ' lock mech/service menu
        mLockMech = New fLockMech
        mLockMech.Initialise()


        ' windowed or full screen ?
        If settingsManager.GetValue("RunWindowed") Then
            FormBorderStyle = FormBorderStyle.FixedSingle

        Else
            Me.Location = New System.Drawing.Point(0, 0)

            'Me.Size = New System.Drawing.Size(768, 1280)
            'Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            'Me.TopMost = True

            Me.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

        End If

        ' set up the payment manager
        paymentTypeObject = settingsManager.GetValue("PaymentType")

        If Not paymentTypeObject Is Nothing Then

            paymentType = paymentTypeObject.ToString.ToUpper

            Select Case paymentType

                Case "EPORT"

                    ' set up for eport
                    ePortManager = ePortManagerFactory.GetManager()

                    '      ePortManager.Initialise(settingsManager.GetValue("EportBatchRetryInterval"), settingsManager.GetValue("ReaderHidBufferLength"), settingsManager.GetValue("EportCommMethod"))
                    ePortManager.Initialise(My.Settings.DatabaseConnectionString, settingsManager.GetValue("EportBatchRetryInterval"), settingsManager.GetValue("ReaderHidBufferLength"), settingsManager.GetValue("EportCommMethod"))
                    ePortManager.SetCallback(AddressOf EportEventCallback)

                    If settingsManager.GetValue("PaymentDebug") Then
                        ePortManager.Show()
                        debugInformation.Progress(fDebugWindow.Level.WRN, 1276, "Displaying ePortManager dialog", True)
                    End If

                    '      recieptPrinter.Initialise("HelveticaNeueLt Std", 9, 29)
                    recieptPrinter.Initialise("Courier", 8, 42)

                Case "YESPAY"

                    ' set up for yes pay
                    yespayManager = yespayManagerFactory.GetManager()

                    yespayManager.Initialise(My.Settings.DatabaseConnectionString)
                    yespayManager.SetCallback(AddressOf YesPayEventCallback)

                    If settingsManager.GetValue("PaymentDebug") Then
                        yespayManager.Show()
                        debugInformation.Progress(fDebugWindow.Level.WRN, 1522, "Displaying yespayManager dialog", True)
                    End If

                    '  Reciept.Initialise("HelveticaNeueLt Std", 8, 42)
                    recieptPrinter.Initialise("Tahoma", 10, 42)

                Case "MDB"

                    ' set up for mdb payment
                    mdbManager = mdbManagerFactory.GetManager()
                    mdbManager.Initialise()

                    mdbManager.SetCallback(AddressOf MdbPaymentEventCallback)

                    If settingsManager.GetValue("PaymentDebug") Then
                        mdbManager.Show()
                        debugInformation.Progress(fDebugWindow.Level.WRN, 1526, "Displaying mdbManager dialog", True)
                    End If

                    recieptPrinter.Initialise("Courier", 8, 42)

                Case "FREE"

                    recieptPrinter.Initialise("Tahoma", 10, 42)

                Case Else

                    debugInformation.Progress(fDebugWindow.Level.ERR, 1523, "Unknown payment type", True)

            End Select

        End If

        ' fire up the attract Manager
        If settingsManager.GetValue("AttractActive") Then
            attractManager = attractManagerFactory.GetManager()
        End If

        ' start the application with initialise.
        '  vendApplicationStatus = ApplicationStatus.Reset
        vendApplicationStatus = ApplicationStatus.InitialiseSetCursor

        ' fire up the management thread
        managementThread = New Thread(AddressOf ApplicationStatusThread)
        managementThread.Name = "Application Status"
        managementThread.Priority = ThreadPriority.BelowNormal
        managementThread.Start()

        '     accessPaymentManager = accessPaymentManagerFactory.GetManager
        '   accessPaymentManager.Show()
        '     accessPaymentManager.Initialise()
    End Sub

#End Region

    Private Sub LogContact(ByVal logData As String)

        Dim logFileName As String
        Dim fileStreamObject As FileStream
        Dim fileStreamWriterObject As StreamWriter

        ' do we need to add this message to the log ?
        Try
            ' open and close when writing logs kaseya works a lot better if the files arn't held open
            logFileName = "Contacts_" & Format(Now.Month, "##00") & "_" & Format(Now.Day, "##00") & "_" & Format(Now.Year, "####0000") & ".txt"

            fileStreamObject = New FileStream("C:\Control\Contacts\" & logFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)
            fileStreamWriterObject = New StreamWriter(fileStreamObject)

            fileStreamWriterObject.BaseStream.Seek(0, SeekOrigin.End)
            fileStreamWriterObject.WriteLine(logData)
            fileStreamWriterObject.Flush()
            fileStreamWriterObject.Close()
            fileStreamObject.Close()
        Catch ex As Exception
        End Try

    End Sub
    Public Function checkMac() As Boolean
        Dim networkcard() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()

        Dim ret As Boolean = False


        For Each add As NetworkInterface In networkcard

            If add.GetPhysicalAddress.ToString = "0010F31E9FF0" Then
                ret = True
            Else
                ret = False

            End If

        Next
        Return ret
    End Function

    ' MechanismEvent
    ' things are happenning on the vend process
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub MechanismEvent(ByVal eventCode As fMechInterface.Message, ByVal integerValue1 As Integer, ByVal integerValue2 As Integer)

        Dim eventDescription As String = ""
        Dim eventLevel As fDebugWindow.Level

        Select Case eventCode

            Case fMechInterface.Message.MECH_DELIVERY_STARTS
            Case fMechInterface.Message.MECH_FRIDGE_OPEN
            Case fMechInterface.Message.MECH_FRIDGE_CLOSE

            Case fMechInterface.Message.MECH_VEND_ACTIVITY
                vendingTimeoutCounter = 30

            Case fMechInterface.Message.MECH_PRODUCT_OFFERED
                MessageToFlash("purchase", "2")
                CompleteTheTransactionOK()

            Case fMechInterface.Message.MECH_DELIVERY_COMPLETES

                ' the product has been delivered
                MessageToFlash("purchase", "2")
                startLoopDelay = 1000
                endLoopDelay = 1000
                vendApplicationStatus = ApplicationStatus.Reset

            Case fMechInterface.Message.MECH_DELIVERY_FAILS
                debugInformation.Progress(fDebugWindow.Level.WRN, 1555, "Mech delivery failed", True)
                VendResultFailed()

            Case fMechInterface.Message.MECH_VMC_DOOR_FORCED
                If vendApplicationStatus = ApplicationStatus.Idle Then
                    mechInterface.SendDoorCloseCommand()
                End If

            Case fMechInterface.Message.MECH_PC_SERVICE_REQUEST
                mLockMech.OpenKeypad()

            Case fMechInterface.Message.MECH_VEND_INFO

                If debugInformation.VMCEventDetails(eventLevel, integerValue1, eventDescription) Then

                    debugInformation.Progress(eventLevel, integerValue1, "VMC: " & eventDescription, True)
                Else

                    debugInformation.Progress(eventLevel, integerValue1, "VMC: unrecognised code=" & integerValue1.ToString, True)
                End If

            Case fMechInterface.Message.MECH_INITIALISING

                Select Case paymentType

                    Case "EPORT"
                    Case "YESPAY"
                    Case "MDB"
                        mdbManager.Cancel()
                    Case "FREE"

                End Select

                DisplayErrorMessage("Communications problem", 3)

            Case fMechInterface.Message.MECH_READY

        End Select

    End Sub

#Region "Yespay"

    ' YesPayEventCallback
    ' callback function for yespay interface
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub YesPayEventCallback(ByVal eventCode As Integer, ByVal integerValue As Integer, ByVal stringValue As String)

        Select Case eventCode

            Case fYespayManager.Message.YP_AUTHORISATION_OK
                ccApproved()

            Case fYespayManager.Message.YP_AUTHORISATION_FAIL
                ccDeclined()

            Case Else

        End Select

    End Sub

#End Region

#Region "Mdb"

    Private Sub MdbPaymentEventCallback(ByVal eventCode As Integer, ByVal integerValue As Integer, ByVal stringValue As String)

        Select Case eventCode

            Case fMdbManager.Message.MDB_FTL_TRANSFER_START
                MessageToFlash("displayMessage", "Preparing to print your receipt, please wait.")
                startLoopDelay = 500
                endLoopDelay = 500
                printTimeout = 10
                vendApplicationStatus = ApplicationStatus.FltTransfer

            Case fMdbManager.Message.MDB_FTL_TRANSFER
                printTimeout = 10
                vendApplicationStatus = ApplicationStatus.FltTransfer

            Case fMdbManager.Message.MDB_INSERT_CARD
                MessageToFlash("displayMessage", "Please insert your card into the card reader")

            Case fMdbManager.Message.MDB_CARD_ACTIVITY
                swipeTimeoutCounter = 60

            Case fMdbManager.Message.MDB_TRANSACTION_START_OK
                ccAskForSwipe()

            Case fMdbManager.Message.MDB_TRANSACTION_START_FAIL
                ccCommunicationsFailure()

            Case fMdbManager.Message.MDB_CANCELLED

                mdbManager.Cancel()
                debugInformation.Progress(fDebugWindow.Level.WRN, 1183, "Transaction Cancelled", True)

                MessageToFlash("displayMessage", "Transaction cancelled")

                startLoopDelay = 2000
                endLoopDelay = 2000
                vendApplicationStatus = ApplicationStatus.Reset

            Case fMdbManager.Message.MDB_AUTHORISATION_OK
                ccApproved()

            Case fMdbManager.Message.MDB_AUTHORISATION_FAIL
                ccDeclined()

            Case fMdbManager.Message.MDB_RECIEPT_READY
                PrintMdbReceipt()

            Case fMdbManager.Message.MDB_REPORT_READY
                PrintMdbReport()

            Case fMdbManager.Message.MDB_FLT_AUDIT_FAILURE

            Case fMdbManager.Message.MDB_FLT_RECEIPT_FAILURE
                ManageFailedMdbReciept()

            Case fMdbManager.Message.MDB_DISPLAY_REQUEST
                MessageToFlash("displayMessage", stringValue)

            Case Else

        End Select

    End Sub


#End Region

    Private Sub DisplayErrorMessage(ByVal errorMessage As String, ByVal errorDurationSeconds As Integer)

        errorMessageCountdown = errorDurationSeconds
        startLoopDelay = 500
        endLoopDelay = 500
        vendApplicationStatus = ApplicationStatus.ShowErrorMessage

        MessageToFlash("displayMessage", errorMessage)

    End Sub

#Region "Eport"

    ' EportEventCallback
    ' callback function for yespay interface
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub EportEventCallback(ByVal eventCode As Integer, ByVal integerValue As Integer, ByVal stringValue As String)

        Select Case (eventCode)

            Case fEportManager.Message.EP_APPROVAL_SWIPE_CARD
                ccAskForSwipe()

            Case fEportManager.Message.EP_APPROVAL_SWIPED_OK
                ccSwipedOkay()

            Case fEportManager.Message.EP_APPROVAL_RESWIPE_CARD
                ccAskForReswipe()

            Case fEportManager.Message.EP_APPROVAL_FAIL
                ccApprovalFailure()

            Case fEportManager.Message.EP_APPROVAL_DECLINED
                ccDeclined()

            Case fEportManager.Message.EP_APPROVAL_AUTHORISED
                ccApproved()

            Case fEportManager.Message.EP_APPROVAL_COMMUNICATIONS_FAIL
                ccCommunicationsFailure()

        End Select

    End Sub
#End Region

#Region "VB <-> Flash Communications"

    ' proxy_ExternalFSCommandCall
    ' This is the area that incoming messages from the Flash app are processed
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub proxy_ExternalFSCommandCall(ByVal sender As System.Object, ByVal e As AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEvent)

        Dim incomingMessage As String = e.command.ToString & "=" & e.args.ToString
        Dim flashMessage As String() = incomingMessage.Split("=")
        Dim productId As Integer
        Dim redElement As Integer
        Dim greenElement As Integer
        Dim blueElement As Integer

        Select Case flashMessage(0)

            Case "purchaseItem"
             
                productId = Convert.ToInt32(flashMessage(1))

                If productId = settingsManager.GetValue("DoorLockProdTriggerId") Then
                    mLockMech.OpenKeypad()
                    startLoopDelay = 2000
                    endLoopDelay = 2000
                    vendApplicationStatus = ApplicationStatus.Reset
                Else

                    If Not attractManager Is Nothing Then _
                        attractManager.EnableAttract(False)

                    Select Case paymentType

                        Case "EPORT"
                            ccStartTransaction(productId)
                        Case "YESPAY"
                            ccStartTransaction(productId)
                        Case "MDB"
                            ccStartTransaction(productId)
                        Case "FREE"
                            ccFreeVend(productId)

                        Case Else
                            startLoopDelay = 2000
                            endLoopDelay = 2000
                            vendApplicationStatus = ApplicationStatus.Reset


                    End Select

                    If Not attractManager Is Nothing Then _
                        attractManager.WaitForAttractToStop()

                End If
             
            Case "statusCode"                          ' statusCode is simply a method for knowing where the flash app is
            Case "startOver"                             ' Flash app started over - could pass XML
            Case "allProducts"                           ' ignore this the app will post data to the flash when nessacary

            Case "colourSet", "colorSet"

                If Not attractManager Is Nothing AndAlso flashMessage.Length >= 2 AndAlso flashMessage(1).Length >= 6 Then

                    Dim upperCaseColourSpec = flashMessage(1).ToUpper

                    helperFunctions.HexStringToInteger(upperCaseColourSpec.Substring(0, 2), redElement)
                    helperFunctions.HexStringToInteger(upperCaseColourSpec.Substring(2, 2), greenElement)
                    helperFunctions.HexStringToInteger(upperCaseColourSpec.Substring(4, 2), blueElement)

                    attractManager.SetBaseColour(redElement, greenElement, blueElement)
                    attractManager.RunLightingScript("COLOURED IDLING", True)

                End If

            Case "logContact"

                If flashMessage.Length = 2 Then
                    LogContact(flashMessage(1))
                End If

            Case Else

        End Select

        debugInformation.Progress(fDebugWindow.Level.INF, 1491, "Flash message recieved" & incomingMessage, True)

    End Sub

    ' MessageToFlash, FlashPlay & FlashGotoFrame
    ' send a message to the flash application, and navigate the flash movie..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub MessageToFlashDelegate(ByVal functionName As String, ByVal message As String)

    Private Sub MessageToFlash(ByVal functionName As String, ByVal message As String)

        Dim newDelagate As MessageToFlashDelegate
        Dim messageSuccess As Boolean = False
        Dim retryCount As Integer = 0

        '   Application.DoEvents()

        If AxFlash.InvokeRequired Then

            newDelagate = New MessageToFlashDelegate(AddressOf MessageToFlash)
            AxFlash.Invoke(newDelagate, New Object() {functionName, message})

        Else

            While messageSuccess = False AndAlso retryCount < 4

                Try
                    flashProxy.CallMe(functionName, message)
                    messageSuccess = True

                Catch ex As Exception
                    Thread.Sleep(75)
                    retryCount = retryCount + 1
                    debugInformation.Progress(fDebugWindow.Level.WRN, 1490, "MessageToFlash failed , retrying:" & functionName & " " & message, True)
                End Try

            End While

        End If
    End Sub

#End Region

#Region "Management loop"

    ' ApplicationStatusThread
    ' drive the application through the logic..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ApplicationStatusThread()

        Dim productList As String
        Dim timeNow As Date

        While continueThread

            timeNow = Now()

            ' kick the watchdog.
            mechInterface.StartWatchDog(settingsManager.GetValue("WatchdogPcTimeout"))

            ' log the application status changes
            If oldApplicationStatus <> vendApplicationStatus Then

                oldApplicationStatus = vendApplicationStatus
                debugInformation.Progress(fDebugWindow.Level.INF, 1080, "Changed App State to " & vendApplicationStatus.ToString, True)
            End If

            ' is a delay required at the start
            If startLoopDelay <> 0 Then
                Thread.Sleep(startLoopDelay)
                startLoopDelay = 0
            End If


            Select Case vendApplicationStatus

                Case ApplicationStatus.InitialiseSetCursor

                    If settingsManager.GetValue("ShowCursor") Then
                        MessageToFlash("mouseOn", "")

                    Else
                        MessageToFlash("mouseOff", "")
                    End If

                    vendApplicationStatus = ApplicationStatus.Reset

                Case ApplicationStatus.Reset
                
                    debugInformation.Progress(fDebugWindow.Level.INF, 1091, "Vend Application Reset", True)
                    MessageToFlash("home", "")

                    Select Case paymentType

                        Case "EPORT"
                            ePortManager.ActivateReader(False)
                            ePortManager.ResetTransaction()

                        Case "YESPAY"
                            yespayManager.ResetTransaction()

                        Case "MDB"
                            mdbManager.ResetTransaction()

                    End Select

                    If Not attractManager Is Nothing Then _
                        attractManager.EnableAttract(True)

                    activeProduct.productId = 0

                    ' DEBUG
                    '         currentSaleId = -1

                    vendApplicationStatus = ApplicationStatus.TransferData
                    endLoopDelay = 500
                    startLoopDelay = 500
                    successDetermined = False

                    If settingsManager.GetValue("VideoEnabled") Then
                        videoManager.StopRecording(fVideoManager.CameraListItem.PICK_HEAD_VIEW)
                        videoManager.StopRecording(fVideoManager.CameraListItem.CUSTOMER_VIEW)
                        videoManager.StopRecording(fVideoManager.CameraListItem.MECH_VIEW)

                        videoManager.ShowView(fVideoManager.CameraListItem.PICK_HEAD_VIEW, False)

                    End If

                Case ApplicationStatus.TransferData

                    productList = databaseManager.GetFullProductList()
                    MessageToFlash("qryAllProd", productList)
                    endLoopDelay = 500
                    vendApplicationStatus = ApplicationStatus.Idle

                Case ApplicationStatus.Idle

                    ' holding state
                    If paymentType = "YESPAY" Then
                        yespayManager.Manage()
                    End If

                Case ApplicationStatus.AskForSwipe

                    If Not attractManager Is Nothing Then   ' illuminate the card reader..
                        attractManager.RunLightingScript("WAIT CARD", True)
                    End If

                    Select Case paymentType

                        Case "MDB"
                            MessageToFlash("displayMessage", "Please follow the instructions on the card reader")


                        Case Else
                            MessageToFlash("cardAuth", "0")

                    End Select

                    vendApplicationStatus = ApplicationStatus.WaitSwipe
                    endLoopDelay = 500

                    swipeTimeoutCounter = 60
                    eportTimeoutCounter = 120

                    If Not attractManager Is Nothing Then   ' illuminate the card reader..
                        attractManager.RunLightingScript("WAIT CARD", True)
                    End If


                Case ApplicationStatus.WaitSwipe

                    Select Case paymentType

                        Case "MDB"

                        Case Else

                            ' check that we dont timeout
                            If swipeTimeoutCounter > 0 Then
                                swipeTimeoutCounter -= 1

                            Else
                                ccCommunicationsFailure()

                            End If

                    End Select

                Case ApplicationStatus.WaitCardPayment

                    ' wait here till the eport drives the state elsewhere
                    ' check that we dont timeout
                    If eportTimeoutCounter > 0 Then
                        eportTimeoutCounter -= 1

                    Else
                        ccCommunicationsFailure()


                    End If

                Case ApplicationStatus.FreeVend

                    MessageToFlash("purchase", "1")
                    vendApplicationStatus = ApplicationStatus.VendVMC
                    endLoopDelay = 100

                Case ApplicationStatus.Approved


                    Select Case paymentType

                        Case "MDB"
                            MessageToFlash("displayMessage", "Payment approved.")
                        Case Else

                            MessageToFlash("cardAuth", "2")
                    End Select


                    vendApplicationStatus = ApplicationStatus.VendVMC
                    endLoopDelay = 100

                Case ApplicationStatus.VendVMC

                    If settingsManager.GetValue("VideoEnabled") Then
                        videoManager.StartRecording(fVideoManager.CameraListItem.PICK_HEAD_VIEW)
                        videoManager.StartRecording(fVideoManager.CameraListItem.CUSTOMER_VIEW)
                        videoManager.StartRecording(fVideoManager.CameraListItem.MECH_VIEW)

                        videoManager.ShowView(fVideoManager.CameraListItem.PICK_HEAD_VIEW, True)

                    End If

                    Select Case paymentType
                        Case "MDB"
                            MessageToFlash("displayMessage", "your product will be delivered.")
                        Case Else
                            MessageToFlash("purchase", "1")
                    End Select

                    MessageToFlash("purchase", "1")
                    endLoopDelay = 1000
                    vendingTimeoutCounter = 20
                    vendApplicationStatus = ApplicationStatus.BusyVending

                    mechInterface.VendProduct(activePosition.xPos, activePosition.yPos, activePosition.vdo, activePosition.pickAttempts, activePosition.fridgeDoor)

                Case ApplicationStatus.BusyVending

                    ' timed out ?
                    If vendingTimeoutCounter = 0 Then
                        debugInformation.Progress(fDebugWindow.Level.WRN, 1092, "Timed out on Busy Vending", True)
                        VendResultFailed()

                    Else
                        vendingTimeoutCounter -= 1
                    End If

                Case ApplicationStatus.FltTransfer

                    If printTimeout = 0 Then

                        debugInformation.Progress(fDebugWindow.Level.WRN, 1094, "Flt transfer timed out..", True)
                        MessageToFlash("purchase", "4")
                        endLoopDelay = 1000
                        vendApplicationStatus = ApplicationStatus.Reset

                        ' JON DEBUG
                        If Not mdbManager Is Nothing Then
                            mdbManager.Cancel()
                        End If


                    Else
                        printTimeout = printTimeout - 1

                    End If

                Case ApplicationStatus.ErrorMinor

                    debugInformation.Progress(fDebugWindow.Level.WRN, 1090, "User Prompted: Vend Failed", True)
                    endLoopDelay = 4000
                    vendApplicationStatus = ApplicationStatus.Reset

                    Select Case paymentType

                        Case "EPORT"
                            MessageToFlash("purchase", "3")

                        Case "YESPAY"
                            MessageToFlash("purchase", "3")

                        Case "FREE"
                            MessageToFlash("purchase", "3")

                        Case "MDB"
                            MessageToFlash("purchase", "3")
                            '      MessageToFlash("displayMessage", "Sorry, unable to deliver your product.")

                    End Select

                Case ApplicationStatus.ShowErrorMessage

                    If errorMessageCountdown = 0 Then

                        errorMessageCountdown = errorMessageCountdown - 1
                    Else
                        endLoopDelay = 500
                        endLoopDelay = 500
                        vendApplicationStatus = ApplicationStatus.Reset

                    End If



                Case ApplicationStatus.Exiting
                    StopProgram()

            End Select

            If continueThread Then

                If endLoopDelay <> lastEndLoopDelay Then

                    debugInformation.Progress(fDebugWindow.Level.INF, 1093, "endLoopDelay=" & endLoopDelay.ToString, True)
                    lastEndLoopDelay = endLoopDelay
                End If
                Thread.Sleep(endLoopDelay)
            End If

        End While

    End Sub

#End Region

    Private lastEndLoopDelay As Integer = -1

#Region "Payment"

    ' DisplayDebugProductInfo
    ' output a products details and its position 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub DisplayDebugProductInfo(ByVal productInfo As Vend.cDatabaseManager.ProductStructure, ByVal positionInfo As Vend.cDatabaseManager.PositionStructure)

        debugInformation.Progress(fDebugWindow.Level.INF, 1110, "All data for product", True)

        debugInformation.Progress(fDebugWindow.Level.INF, 1111, "productId " & productInfo.productId, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1113, "authourisationPrice " & productInfo.authourisationPrice, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1114, "displayPrice " & productInfo.displayPrice, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1115, "productName " & productInfo.productName, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1116, "categoryName " & productInfo.categoryName, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1117, "shortDescription " & productInfo.shortDescription, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1118, "namePrefix " & productInfo.namePrefix, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1119, "keyVend " & productInfo.keyVend, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1120, "imageUrl1" & productInfo.imageUrl1, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1121, "imageUrl2" & productInfo.imageUrl2, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1122, "imageUrl3 " & productInfo.imageUrl3, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1123, "imageUrl4 " & productInfo.imageUrl4, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1124, "Position Id " & positionInfo.posId, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1125, "stock " & positionInfo.itemsInStock, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1125, "xPos, yPos " & positionInfo.xPos & ", " & positionInfo.yPos, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1126, "vdo " & positionInfo.vdo, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1127, "Pick Attempts " & positionInfo.pickAttempts, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1128, "Fridge door " & positionInfo.fridgeDoor, True)

    End Sub

    ' ccFreeVend
    ' start a free vend
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub ccFreeVend(ByVal productId As Integer)

        debugInformation.Progress(fDebugWindow.Level.INF, 1100, "User Pressed Buy Button Free Vend: Request for ProdId = " & productId, True)

        If mechInterface.IsOkayToVend() Then

            If databaseManager.RecoverProductData(productId, activeProduct) And _
                databaseManager.RecoverPositionData(productId, activePosition) Then

                DisplayDebugProductInfo(activeProduct, activePosition)

                vendApplicationStatus = ApplicationStatus.FreeVend

            Else
                debugInformation.Progress(fDebugWindow.Level.ERR, 1134, "Product not found", False)
                vendApplicationStatus = ApplicationStatus.Reset
            End If


        Else
            debugInformation.Progress(fDebugWindow.Level.ERR, 1136, "VMC is Offline - Cannot Proceed", False)
            ccCommunicationsFailure()
        End If

    End Sub

    Sub ccStartTransaction(ByVal productId As Integer)

        Dim authorisationStartedOkay As Boolean = False
        Dim printerReady As cPrinterStatus.STATUS
        Dim extendedDetectedErrorState As Integer
        Dim extendedPrinterStatus As Integer
        Dim currentTaxRate As Integer
        Dim AuthPrice As Integer
        debugInformation.Progress(fDebugWindow.Level.INF, 1101, "User Pressed Buy Button Start Credit Card Transaction: Request for ProdId = " & productId, True)

        ' tax rate in 100th of a percent ie a 6% tax rate would be 600


        '   DEBUG
        currentSaleId = -1

        ' are we okay to print a reciept
        printerStatus.CheckPrinterStatus(printerReady, extendedDetectedErrorState, extendedPrinterStatus)

        If Not mechInterface.IsOkayToVend() Then

            debugInformation.Progress(fDebugWindow.Level.ERR, 1131, "VMC is Offline - Cannot Proceed", True)
            ccVMCOffline()

        ElseIf printerReady <> cPrinterStatus.STATUS.PRT_READY AndAlso settingsManager.GetValue("RecieptRequired") Then

            debugInformation.Progress(fDebugWindow.Level.WRN, 1140, "Printer is not ready - cannot proceed", True)
            DisplayErrorMessage("The receipt printer is not ready - cannot proceed", 3)
            '      ccCommunicationsFailure()

        ElseIf Not databaseManager.RecoverProductData(productId, activeProduct) Then

            debugInformation.Progress(fDebugWindow.Level.ERR, 1130, "Product not found", True)
            ccVMCOffline()

        Else

            databaseManager.RecoverPositionData(productId, activePosition)
            DisplayDebugProductInfo(activeProduct, activePosition)

            Update()

            debugInformation.Progress(fDebugWindow.Level.INF, 1200, "Starting Credit card sequence", True)
            currentTaxRate = settingsManager.GetValue("TaxRate")
            debugInformation.Progress(fDebugWindow.Level.INF, 1102, "Tax rate = " & currentTaxRate.ToString & " (x 1/100th %)", True)

            AuthPrice = activeProduct.authourisationPrice
            If currentTaxRate > 0 And activeProduct.taxable = 1 Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1102, "Old Price = " & AuthPrice, True)
                AuthPrice = AuthPrice + (AuthPrice * (currentTaxRate / 10000))
                debugInformation.Progress(fDebugWindow.Level.INF, 1102, "new Price = " & AuthPrice, True)

            End If

            debugInformation.Progress(fDebugWindow.Level.INF, 1102, "Full Including Tax Price:" & AuthPrice.ToString, True)


            ' fill in what we can for sales reporting.
            Select Case paymentType

                Case "EPORT"
                    authorisationStartedOkay = ePortManager.Authorise(AuthPrice, activeProduct.productName, activeProduct.productId)

                Case "YESPAY"
                    authorisationStartedOkay = yespayManager.Authorise(AuthPrice, activeProduct.productName, activeProduct.productId)

                Case "MDB"
                    authorisationStartedOkay = mdbManager.Authorise(AuthPrice, activeProduct.productName, activeProduct.productId)

            End Select

            If authorisationStartedOkay Then

                endLoopDelay = 500
                swipeTimeoutCounter = 60
                eportTimeoutCounter = 90
                vendApplicationStatus = ApplicationStatus.WaitCardPayment

            Else

                ccCommunicationsFailure()
            End If

        End If

    End Sub

    Sub ccAskForSwipe()

        vendApplicationStatus = ApplicationStatus.AskForSwipe

    End Sub

    Sub ccAskForReswipe()

        MessageToFlash("cardAuth", "7")
        startLoopDelay = 2000
        endLoopDelay = 2000

        vendApplicationStatus = ApplicationStatus.AskForSwipe

    End Sub

    Sub ccSwipedOkay()

        startLoopDelay = 500
        MessageToFlash("cardAuth", "1")               ' dont do anything else the eport drives this bit of code

    End Sub


    Sub ccInvalidCard()

        MessageToFlash("cardAuth", "6")
        startLoopDelay = 2000
        endLoopDelay = 2000

        vendApplicationStatus = ApplicationStatus.Reset

    End Sub

    Sub ccApproved()

        debugInformation.Progress(fDebugWindow.Level.INF, 1180, "Credit Card Authorized OK", True)

        startLoopDelay = 500
        endLoopDelay = 500
        vendApplicationStatus = ApplicationStatus.Approved

    End Sub

    Sub ccDeclined()

        debugInformation.Progress(fDebugWindow.Level.INF, 1181, "Credit Card Declined", True)

        MessageToFlash("cardAuth", "3")
        startLoopDelay = 2000
        endLoopDelay = 2000
        vendApplicationStatus = ApplicationStatus.Reset

    End Sub

    Sub ccApprovalFailure()

        debugInformation.Progress(fDebugWindow.Level.WRN, 1182, "Unable to approve card", True)

        MessageToFlash("cardAuth", "4")
        startLoopDelay = 2000
        endLoopDelay = 2000
        vendApplicationStatus = ApplicationStatus.Reset

    End Sub

    Sub ccCommunicationsFailure()


        If Not attractManager Is Nothing Then   ' illuminate the card reader..
            attractManager.RunLightingScript("IDLING", True)
        End If

        Select Case paymentType

            Case "EPORT"
                ePortManager.ResetTransaction()

            Case "YESPAY"
            Case "FREE"

            Case "MDB"
                mdbManager.Cancel()


        End Select

        debugInformation.Progress(fDebugWindow.Level.WRN, 1183, "Communications Failure", True)

        MessageToFlash("cardAuth", "6")
        startLoopDelay = 2000
        endLoopDelay = 2000
        vendApplicationStatus = ApplicationStatus.Reset

    End Sub


    Sub ccVMCOffline()


        If Not attractManager Is Nothing Then   ' illuminate the card reader..
            attractManager.RunLightingScript("IDLING", True)
        End If

        Select Case paymentType

            Case "EPORT"
                ePortManager.ResetTransaction()

            Case "YESPAY"
            Case "FREE"

            Case "MDB"
                mdbManager.Cancel()


        End Select

        debugInformation.Progress(fDebugWindow.Level.WRN, 1183, "Communications Failure", True)

        MessageToFlash("purchase", "6")
        startLoopDelay = 4000
        endLoopDelay = 4000
        vendApplicationStatus = ApplicationStatus.Reset

    End Sub

#End Region

#Region "Vend Process"

    ' VendResultFailed
    ' failed
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub VendResultFailed()

        vendApplicationStatus = ApplicationStatus.ErrorMinor

        ' close down the video recordings
        If settingsManager.GetValue("VideoEnabled") Then

            videoManager.ShowView(fVideoManager.CameraListItem.PICK_HEAD_VIEW, False)

            videoManager.StopRecording(fVideoManager.CameraListItem.MECH_VIEW)
            videoManager.StopRecording(fVideoManager.CameraListItem.PICK_HEAD_VIEW)
            videoManager.StopRecording(fVideoManager.CameraListItem.CUSTOMER_VIEW)


        End If

        If Not successDetermined Then

            successDetermined = True

            'print a ticket for the customer giving trancation references
            If settingsManager.GetValue("HasPrinter") Then

                Select Case paymentType

                    Case "EPORT"
                        EportReceipt(False)

                    Case "YESPAY"
                        YespayReceipt(False)

                    Case "MDB"
                        MdbReceipt(False)

                    Case "FREE"
                        FreeReceipt(False)

                End Select

            End If

        End If



        ' advise the payment manager of the  failure
        Select Case paymentType

            Case "EPORT"
                ePortManager.StartBatch(False)

            Case "YESPAY"
                yespayManager.StartBatch(False)

            Case "MDB"
                mdbManager.StartBatch(False)


        End Select

        Dim newstock As Integer
        Try
            If activePosition.itemsInStock > 1 Then
                newstock = activePosition.itemsInStock - 1
            Else
                newstock = 0

            End If
            helperFunctions.SetStockLevel(activePosition.posId, newstock, My.Settings.DatabaseConnectionString)
            debugInformation.Progress(fDebugWindow.Level.WRN, 9901, "Sprinkles = Failed the vend so decrimenting the stock available to " & newstock, True)

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 9901, "Sprinkles = Failed the vend so decrimenting the stock available to " & newstock & ex.ToString, True)


        End Try

    End Sub

    Private Sub CompleteTheTransactionOK()

        Dim successRecordingSale As Boolean = False
        Dim successAddingTransactionId As Boolean = False

        debugInformation.Progress(fDebugWindow.Level.INF, 1184, "Completing Transaction ", True)

        Try

            successRecordingSale = helperFunctions.RecordaSale(activePosition.posId, currentSaleId, My.Settings.DatabaseConnectionString)

        Catch ex As Exception
        End Try

        If successRecordingSale Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1185, "Printing Receipt", True)

            If Not successDetermined Then

                successDetermined = True

                If settingsManager.GetValue("HasPrinter") Then

                    Select Case paymentType

                        Case "EPORT"
                            EportReceipt(True)
                            successAddingTransactionId = helperFunctions.AddTransactionId(currentSaleId, ePortManager.GetTransactionID(), My.Settings.DatabaseConnectionString)

                        Case "YESPAY"
                            YespayReceipt(True)

                        Case "MDB"
                            MdbReceipt(True)

                        Case "FREE"
                            FreeReceipt(True)


                    End Select

                End If

            Else
                debugInformation.Progress(fDebugWindow.Level.ERR, 9999, "unable to record sale on position id=" & activePosition.posId & "(" & activeProduct.productName & ")", True)

            End If

            If settingsManager.GetValue("VideoEnabled") Then

                videoManager.ShowView(fVideoManager.CameraListItem.PICK_HEAD_VIEW, False)

                videoManager.StopRecording(fVideoManager.CameraListItem.PICK_HEAD_VIEW)
                videoManager.StopRecording(fVideoManager.CameraListItem.CUSTOMER_VIEW)
                videoManager.StopRecording(fVideoManager.CameraListItem.MECH_VIEW)

            End If


            'If databaseManager.MarkNextToVend(activePosition.posId, activeProduct.productId) = True Then
            '    debugInformation.Progress(fDebugWindow.Level.INF, 1186, "Success Setting Next Position To Vend", True)
            'Else
            '    debugInformation.Progress(fDebugWindow.Level.WRN, 1187, "Failed Setting Next Position To Vend", True)
            'End If

        End If

        Select Case paymentType

            Case "EPORT"
                ePortManager.StartBatch(True)
                debugInformation.Progress(fDebugWindow.Level.INF, 1201, "Batching ePort transaction", True)

            Case "YESPAY"
                yespayManager.StartBatch(True)
                debugInformation.Progress(fDebugWindow.Level.INF, 1524, "Batching Yespay transaction", True)

            Case "MDB"
                mdbManager.StartBatch(True)
                debugInformation.Progress(fDebugWindow.Level.INF, 999, "Batching MDB transaction", True)

            Case "FREE"

        End Select


    End Sub

#End Region

#Region "Receipts"



    Sub EportReceipt(ByVal vendSuccessful As Boolean)
        debugInformation.Progress(fDebugWindow.Level.INF, 1209, "Starting Receipt Generation", True)

        Dim Price As Decimal
        Dim TaxRate As Decimal = settingsManager.GetValue("TaxRate")
        Dim TaxAmount As Decimal
        Dim TotalPrice As Decimal

        price = activeProduct.displayPrice
        TaxRate = TaxRate / 100
        TaxAmount = Price / 100 * TaxRate
        TotalPrice = Price + TaxAmount

         Dim currencySymbol As String = settingsManager.GetValue("CurrencySymbol")
        If currencySymbol Is Nothing Then
            currencySymbol = ""
        End If


        recieptPrinter.Reset()
        recieptPrinter.AddText("RecieptHeader")
        recieptPrinter.AddLine("")

        If vendSuccessful = True Then
            recieptPrinter.AddLine(activeProduct.keyVend.ToString)
            'recieptPrinter.AddLine(activeProduct.namePrefix.ToString)
            recieptPrinter.AddLine(activeProduct.productName.ToString)
            recieptPrinter.AddLine("")
            recieptPrinter.AddLine("SALE: " & Format(Price, currencySymbol & "#,##0.00"))

            If TaxRate <> 0 And activeProduct.taxable = 1 Then
                recieptPrinter.AddLine("Tax:" & Format(TaxAmount, currencySymbol & "#,##0.00").ToString)
                recieptPrinter.AddLine("TOTAL: " & Format(TotalPrice, currencySymbol & "#,##0.00").ToString)
            Else
                recieptPrinter.AddLine("TOTAL: " & Format(Price, currencySymbol & "#,##0.00").ToString)
            End If


        Else
            recieptPrinter.AddLine("We are sorry the transaction")
            recieptPrinter.AddLine("failed, your card has ")
            recieptPrinter.AddLine("not been charged.")
        End If

        recieptPrinter.AddLine("")


        recieptPrinter.AddLine("Terminal:" & ePortManager.GetTerminalSerialNo())
        recieptPrinter.AddLine("TransID: " & ePortManager.GetTransactionID())
        recieptPrinter.AddLine("Date:    " & ePortManager.GetTransactionDateTime())
        recieptPrinter.AddLine("Card No:  ************" & ePortManager.GetLast4Digits())

        recieptPrinter.AddText("RecieptFooter")

        debugInformation.Progress(fDebugWindow.Level.INF, 1209, recieptPrinter.ToString(), True)

        recieptPrinter.Print()

    End Sub

    Sub PrintMdbReport()

        Dim reportLines() As String = Nothing

        recieptPrinter.Reset()

        ' tag on what the card reader wants to issue as a reciept
        mdbManager.GetPrinterOutput(reportLines)

        For Each singleLine As String In reportLines
            recieptPrinter.AddLine(singleLine)
        Next

        recieptPrinter.Print()

        ' scrub the reciept from memory
        mdbManager.ClearIncommingFlt()

    End Sub

    Sub ManageFailedMdbReciept()


        Dim success As Boolean
        Dim receiptLines() As String = Nothing


        recieptPrinter.Reset()
        recieptPrinter.AddText("RecieptHeader")
        recieptPrinter.AddLine("")

        ' then tag on our stuff
        If mdbTransactionSucessful = True Then

            Select Case settingsManager.GetValue("DatabaseXmlFormat")

                Case cDatabaseManager.XmlRequirementStruct.XML_HARVEYNICHOLS
                    recieptPrinter.AddLine(activeProduct.productName.ToString & " " & activeProduct.namePrefix)
                    recieptPrinter.AddLine(activeProduct.productColour & " " & activeProduct.productSize)

                Case Else
                    recieptPrinter.AddLine(activeProduct.productName.ToString)
                    recieptPrinter.AddLine(activeProduct.displayPrice.ToString)

                    recieptPrinter.AddLine("")
                    recieptPrinter.AddLine("FLTTO issue")
            End Select

            recieptPrinter.AddLine("")
        End If

        If mdbTransactionSucessful = False Then
            recieptPrinter.AddLine("We are sorry the transaction failed, your ")
            recieptPrinter.AddLine("card has not been charged.")
        End If

        recieptPrinter.AddLine("")
        recieptPrinter.AddText("RecieptFooter")
        recieptPrinter.Print()

        ' scrub the reciept from memory
        mdbManager.ClearIncommingFlt()

        Try
            ' sales reporting.
            success = helperFunctions.AddTransactionId(currentSaleId, 0, My.Settings.DatabaseConnectionString)

            If Not success Then
                debugInformation.Progress(fDebugWindow.Level.ERR, 9999, "unable to add transaction number to sales record", True)
            End If


        Catch ex As Exception
        End Try

    End Sub

    Sub PrintMdbReceipt()

        Dim success As Boolean
        Dim receiptLines() As String = Nothing
        Dim txnNumberDescription As String = ""

        recieptPrinter.Reset()

        recieptPrinter.AddText("RecieptHeader")

        ' tag on what the card reader wants to issue as a reciept
        mdbManager.GetPrinterOutput(receiptLines)

        For Each singleLine As String In receiptLines
            recieptPrinter.AddLine(singleLine)
        Next
        recieptPrinter.AddLine("")

        ' then tag on our stuff
        If mdbTransactionSucessful = True Then


            Select Case settingsManager.GetValue("DatabaseXmlFormat")

                Case cDatabaseManager.XmlRequirementStruct.XML_HARVEYNICHOLS
                    recieptPrinter.AddLine(activeProduct.productName.ToString & " " & activeProduct.namePrefix)
                    recieptPrinter.AddLine(activeProduct.productColour & " " & activeProduct.productSize)

                Case Else
                    recieptPrinter.AddLine(activeProduct.productName.ToString)

            End Select

            recieptPrinter.AddLine("")
        End If

        ' add the transaction number
        txnNumberDescription = settingsManager.GetValue("TransactionNumberDescription")

        If Not txnNumberDescription Is Nothing Then
            recieptPrinter.AddLine(txnNumberDescription & mdbManager.GetTransactionNumber())
        End If

        If mdbTransactionSucessful = False Then
            recieptPrinter.AddLine("We are sorry the transaction failed, your ")
            recieptPrinter.AddLine("card has not been charged.")

        End If

        recieptPrinter.AddLine("")
        recieptPrinter.AddText("RecieptFooter")

        recieptPrinter.Print()

        ' scrub the reciept from memory
        mdbManager.ClearIncommingFlt()

        Try
            ' sales reporting.
            success = helperFunctions.AddTransactionId(currentSaleId, mdbManager.GetTransactionNumber(), My.Settings.DatabaseConnectionString)

            If Not success Then
                debugInformation.Progress(fDebugWindow.Level.ERR, 9999, "unable to add transaction number to sales record", True)
            End If


        Catch ex As Exception

        End Try

    End Sub

    Sub MdbReceipt(ByVal vendSuccessful As Boolean)

        mdbTransactionSucessful = vendSuccessful

    End Sub

    Sub FreeReceipt(ByVal vendSuccessful As Boolean)

        Dim receiptLines() As String = Nothing

        recieptPrinter.Reset()
        recieptPrinter.AddText("RecieptHeader")

        recieptPrinter.AddLine("")

        ' then tag on our stuff
        If vendSuccessful = True Then

            Select Case settingsManager.GetValue("DatabaseXmlFormat")

                Case cDatabaseManager.XmlRequirementStruct.XML_HARVEYNICHOLS
                    recieptPrinter.AddLine(activeProduct.productName.ToString & " " & activeProduct.namePrefix)
                    recieptPrinter.AddLine(activeProduct.productColour & " " & activeProduct.productSize)

                Case cDatabaseManager.XmlRequirementStruct.XML_COORD
                    recieptPrinter.AddLine(activeProduct.productName.ToString)
                    recieptPrinter.AddLine(activeProduct.namePrefix.ToString)

                Case Else
                    recieptPrinter.AddLine(activeProduct.productName.ToString)

            End Select

            recieptPrinter.AddLine("** FREE VEND **")

        Else
            recieptPrinter.AddLine("We are sorry the transaction failed..")
        End If

        recieptPrinter.AddLine("")
        recieptPrinter.AddText("RecieptFooter")

        recieptPrinter.Print()

    End Sub

    Sub YespayReceipt(ByVal vendSuccessful As Boolean)

        Dim price As Decimal = activeProduct.displayPrice
        Dim myPrice As String = Format(price, "£#,##0.00")
        Dim recieptDetails As fYespayManager.Reciept = New fYespayManager.Reciept

        yespayManager.GetReciept(recieptDetails)

        recieptPrinter.Reset()
        recieptPrinter.AddLine("[LogoHeader]")

        recieptPrinter.AddLine("CUSTOMER COPY")
        recieptPrinter.AddLine("")

        recieptPrinter.AddLine(recieptDetails.merchantAddress)
        recieptPrinter.AddLine("Merchant Id: " & recieptDetails.merchantId)
        recieptPrinter.AddLine("Terminal: " & recieptDetails.terminalId)
        recieptPrinter.AddLine("Date: " & recieptDetails.timeDate)
        recieptPrinter.AddLine("Card No: " & recieptDetails.last4Digits)
        recieptPrinter.AddLine("Expiry Date: " & recieptDetails.expiryDate)
        recieptPrinter.AddLine("Ref No: " & recieptDetails.pgtrCode)
        recieptPrinter.AddLine("")

        If vendSuccessful = True Then

            recieptPrinter.AddLine("Trans Id: " & recieptDetails.recieptNumber)
            recieptPrinter.AddLine("Trans Ref: " & recieptDetails.transactionRef)
            recieptPrinter.AddLine("Authorisation: " & recieptDetails.authorisationCode)
            recieptPrinter.AddLine("")
            recieptPrinter.AddLine(activeProduct.keyVend)
            recieptPrinter.AddLine(activeProduct.namePrefix)
            recieptPrinter.AddLine(activeProduct.productName)
            recieptPrinter.AddLine("")
            recieptPrinter.AddLine("SALE: " & myPrice.ToString)
            recieptPrinter.AddLine("TOTAL: " & myPrice.ToString)

        Else
            recieptPrinter.AddLine("We are sorry the transaction failed, your")
            recieptPrinter.AddLine("card has not been charged.")
        End If

        recieptPrinter.AddLine("")

        If vendSuccessful = True Then

            recieptPrinter.AddLine(recieptDetails.customerDeclaration)

        End If

        recieptPrinter.AddLine(recieptDetails.retentionReminder)
        recieptPrinter.AddLine("")

        recieptPrinter.AddLine("Thanks for shopping with us.")
        recieptPrinter.AddLine("powered by teknovation")
        recieptPrinter.AddLine("www.teknovation.co.uk")

        recieptPrinter.Print()
    End Sub

#End Region

#Region "Close Application"

    Public Sub StopProgram()

        debugInformation.Progress(fDebugWindow.Level.INF, 1012, "Closing vend application", True)
        debugInformation.Shutdown()

        continueThread = False

        If Not mechInterface Is Nothing Then mechInterface.Shutdown()
        If Not printerStatus Is Nothing Then printerStatus.Shutdown()
        If Not attractManager Is Nothing Then attractManager.Shutdown()

        If settingsManager.GetValue("WatchdogPcEnable") Then

            debugInformation.Progress(fDebugWindow.Level.INF, 1410, "Shutting down Pc Watchdog", True)
            mechInterface.EnableWatchdog(False)
            mechInterface.StopWatchDog()
        End If


        Select Case paymentType

            Case "EPORT"
                ePortManager.Shutdown()

            Case "YESPAY"
                yespayManager.Shutdown()

        End Select

        mHook.UnhookKeyboard()

        End

    End Sub

    Private Sub MainForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        StopProgram()
    End Sub

    ' KeyboardEvents
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub KeyboardEvents(ByVal lParam As KBDLLHOOKSTRUCT)

        If lParam.vkCode = 83 And CBool(lParam.flags And mHook.LLKHF_ALTDOWN) And CBool(lParam.flags And mHook.LLKHF_UP) Then
            settingsManager.Show()
        End If

    End Sub

#End Region

    'Private Sub fRemote_ButtonClicked(strButton As String) Handles fRemote.ButtonClicked
    '    Select Case strButton
    '        Case "RestockScreen"
    '            MessageToFlash("custom", "1")
    '            endLoopDelay = 500
    '            vendApplicationStatus = ApplicationStatus.Idle
    '        Case "Reset"
    '            endLoopDelay = 4000
    '            vendApplicationStatus = ApplicationStatus.Reset


    '    End Select
    'End Sub
End Class
