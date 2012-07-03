'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

#Region "Imports Statements"

Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Runtime.InteropServices

Imports DebugWindow
Imports Eport
Imports FridgeManager
Imports MechInterface
Imports MdbManager
Imports AttractManager
Imports SerialManager
Imports SettingsManager
Imports VideoManager
Imports YesPay
Imports System.Net.NetworkInformation

#End Region

Public Class fMain
    Inherits System.Windows.Forms.Form

#Region "Declarations"

    ' managers
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private fridgeManager As fFridgeManager
    Private fridgeManagerFactory As cFridgeManagerFactory = New cFridgeManagerFactory
    Private videoManager As fVideoManager
    Private videoManagerFactory As cVideoManagerFactory = New cVideoManagerFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private mechManager As fMechInterface
    Private mechManagerFactory As cMechInterfaceFactory = New cMechInterfaceFactory
    Private attractManager As fAttractManager
    Private attractManagerFactory As cAttractManagerFactory = New cAttractManagerFactory


    Private mdbManager As fMdbManager = Nothing
    Private mdbManagerFactory As cMdbManagerFactory = New cMdbManagerFactory
    Private yespayManager As fYespayManager
    Private yespayManagerFactory As cYespayManagerFactory = New cYespayManagerFactory
    Private ePortManager As fEportManager
    Private ePortManagerFactory As cEportManagerFactory = New cEportManagerFactory

    ' variables
    Private recieptPrinter As cReceiptPrinter = New cReceiptPrinter
    '   Private dinkeyDongle As cDinkeyDongle = New cDinkeyDongle


#End Region

#Region "Load and Unload Events"
    Public Function checkMac() As Boolean
        'Dim networkcard() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()

        Dim ret As Boolean = True


        'For Each add As NetworkInterface In networkcard
        '    Dim s As String

        '    If add.GetPhysicalAddress.ToString = "0010F31E9FF0" Then
        '        ret = True
        '    Else
        '        ret = False

        '    End If

        'Next
        Return ret
    End Function
    Private Sub FormInitialise(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
     
        Dim videoFrameRate As Integer
        Dim videoFrameSize As Size
        Dim flashMovieFilePath As String
        'If dinkeyDongle.ProtCheck() Then
        '    End             'terminate
        '    Return
        'End If


        settingsManager = settingsManagerFactory.GetManager()
        settingsManager.Initialise(My.Settings.DatabaseConnection)

        ' fire up the debug window.
        debugInformation = debugInformationFactory.GetManager()
        If settingsManager.GetValue("RemoteDebugAllowed") Then
            debugInformation.AllowDebugConnections()
        End If
        debugInformation.Progress(fDebugWindow.Level.INF, 2010, "Configuration application started", True)

        ' sort out the video recorder
        If settingsManager.GetValue("VideoEnabled") = True Then

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

            '    VideoButton.Enabled = True

        End If


        ' is the application windowed ?
        'If Not settingsManager.GetValue("RunWindowed") Then
        '    FormBorderStyle = FormBorderStyle.None
        '    WindowState = FormWindowState.Maximized
        '    MainPanel.Location = New Point((Screen.PrimaryScreen.Bounds.Width - MainPanel.Size.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - MainPanel.Size.Height) / 2)
        'End If
        
        ' sort out the serial connection
        mechManager = mechManagerFactory.GetManager
        mechManager.Initialise()
        mechManager.AddCallback(AddressOf MechanismEvent)

        ' sort out the serial connection
        serialManager = serialManagerFactory.GetManager()

        ' grab up the fridge manager
        fridgeManager = fridgeManagerFactory.GetManager

        ' sort out the user controls.
        PositionProperty.Initialise()
        ProductProperty.Initialise()
        ProductExplorer.Initialise()
        SplashScreen.Initialise()
        ProductExplorer.Populate()

        ' say what database you are working with
        StatusStrip1.Items(1).Text = "Working with database: " & settingsManager.GetDatabaseDescription()

        ' get the MDB manager and show it
        Select Case settingsManager.GetValue("PaymentType")

            Case "MDB"
                recieptPrinter.Initialise("Courier", 8, 42)

                mdbManager = mdbManagerFactory.GetManager()
                mdbManager.Initialise()
                mdbManager.SetCallback(AddressOf MdbPaymentEventCallback)

                If settingsManager.GetValue("PaymentDebug") Then
                    mdbManager.Show()
                    debugInformation.Progress(fDebugWindow.Level.WRN, 1522, "Displaying mdbManager dialog", True)
                End If

            Case "YESPAY"

                ' set up for yes pay
                yespayManager = yespayManagerFactory.GetManager()

                yespayManager.Initialise(My.Settings.DatabaseConnection)

                If settingsManager.GetValue("PaymentDebug") Then
                    yespayManager.Show()
                    debugInformation.Progress(fDebugWindow.Level.WRN, 1527, "Displaying yespayManager dialog", True)
                End If

                '  Reciept.Initialise("HelveticaNeueLt Std", 8, 42)
                recieptPrinter.Initialise("Tahoma", 10, 42)



            Case "EPORT"

                ' set up for eport
                ePortManager = ePortManagerFactory.GetManager()

                '   ePortManager.Initialise(settingsManager.GetValue("EportBatchRetryInterval"), settingsManager.GetValue("ReaderHidBufferLength"), settingsManager.GetValue("EportCommMethod"))
                ePortManager.Initialise(My.Settings.DatabaseConnection, 1800, settingsManager.GetValue("ReaderHidBufferLength"), settingsManager.GetValue("EportCommMethod"))

                If settingsManager.GetValue("PaymentDebug") Then
                    ePortManager.Show()
                    debugInformation.Progress(fDebugWindow.Level.WRN, 1276, "Displaying ePortManager dialog", True)
                End If

                recieptPrinter.Initialise("HelveticaNeueLt Std", 9, 29)

            Case "FREE"

            Case Else

                debugInformation.Progress(fDebugWindow.Level.ERR, 1281, "Unknown payment type", True)


        End Select



        ' locate movie in exe directory
        flashMovieFilePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + settingsManager.GetValue("MovieFileName")

        ' ensure that the video path exists
        If Not File.Exists(flashMovieFilePath) Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1034, "Movie: '" & flashMovieFilePath & "' does not exist", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.INF, 1031, "Flash Movie Exists", True)
        End If

        ' fire up the attract Manager
        If settingsManager.GetValue("AttractActive") Then
            attractManager = attractManagerFactory.GetManager()
        End If
        debugInformation.Progress(fDebugWindow.Level.INF, 1031, "AttractActive2", True)
    End Sub

    Private Sub MechanismEvent(ByVal eventCode As fMechInterface.Message, ByVal integerValue1 As Integer, ByVal integerValue2 As Integer)

        Select Case eventCode

            Case fMechInterface.Message.MECH_VMC_SERVICE_ABORT

            Case fMechInterface.Message.MECH_DELIVERY_COMPLETES

                ' get the MDB manager and show it
                If Not mdbManager Is Nothing Then
                    mdbManager.StartBatch(True)
                End If

            Case fMechInterface.Message.MECH_DELIVERY_FAILS

                If Not mdbManager Is Nothing Then
                    mdbManager.StartBatch(False)
                End If

        End Select

    End Sub

    Private Sub FormShutdown(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        debugInformation.Progress(fDebugWindow.Level.INF, 2011, "Closing configuration application", True)

        If Not mechManager Is Nothing Then mechManager.Shutdown()
        If Not serialManager Is Nothing Then serialManager.Disconnect()
        '  If Not fridgeManager Is Nothing Then fridgeManager.Shutdown()
        If Not mdbManager Is Nothing Then mdbManager.Shutdown()
        If Not attractManager Is Nothing Then attractManager.Shutdown()
        If Not ePortManager Is Nothing Then ePortManager.Shutdown()


        debugInformation.Shutdown()
        SplashScreen.Shutdown()

    End Sub

#End Region

    Private Sub MdbPaymentEventCallback(ByVal eventCode As Integer, ByVal integerValue As Integer, ByVal stringValue As String)

        Select Case eventCode

            Case fMdbManager.Message.MDB_INSERT_CARD
            Case fMdbManager.Message.MDB_CARD_ACTIVITY
            Case fMdbManager.Message.MDB_TRANSACTION_START_OK
            Case fMdbManager.Message.MDB_TRANSACTION_START_FAIL
            Case fMdbManager.Message.MDB_AUTHORISATION_OK
            Case fMdbManager.Message.MDB_AUTHORISATION_FAIL
            Case fMdbManager.Message.MDB_RECIEPT_READY
                PrintMdbReceipt()

            Case fMdbManager.Message.MDB_REPORT_READY
                PrintMdbReport()

            Case Else

        End Select

    End Sub

    ' WatchdogTimerElapsed
    ' kick the watchdog
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub WatchdogTimerElapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles WatchdogTimer.Elapsed

        If Not mechManager Is Nothing Then
            mechManager.StartWatchDog(settingsManager.GetValue("WatchdogPcTimeout"))
        End If

    End Sub

    Private Sub ProductExplorer_SelectionChanges(ByVal selectedTag As String) Handles ProductExplorer.SelectionChanges

        Dim nodeParameters() As String = selectedTag.Split(",")

        ' select the correct tab on the properties control
        Select Case nodeParameters(0)

            Case "C"
                ProductProperty.Visible = False
                PositionProperty.Visible = False

            Case "P"
                ProductProperty.Visible = True
                PositionProperty.Visible = False

                ProductProperty.LoadItem(Convert.ToInt32(nodeParameters(1)))

            Case "R"
                ProductProperty.Visible = False
                PositionProperty.Visible = True

                PositionProperty.LoadItem(Convert.ToInt32(nodeParameters(1)))

        End Select

        StatusStrip1.Items(0).Text = "CurrentTag = " & selectedTag


    End Sub

    Sub PrintMdbReceipt()

        Dim receiptLines() As String = Nothing

        recieptPrinter.Reset()

        ' tag on what the card reader wants to issue as a reciept
        mdbManager.GetPrinterOutput(receiptLines)

        For Each singleLine As String In receiptLines
            recieptPrinter.AddLine(singleLine)
        Next

        recieptPrinter.Print()

        ' scrub the reciept from memory
        '  mdbManager.ClearIncommingFlt()

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
        '  mdbManager.ClearIncommingFlt()

    End Sub

    ' NameIdentifierChangesOccur
    ' we need to ammend a name on the product explorer
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub NameIdentifierChangesOccur(ByVal changedName As String) Handles PositionProperty.NameIdentifierChanges, ProductProperty.NameIdentifierChanges

        ProductExplorer.RenameCurrentSelection(changedName)

    End Sub

    Private Sub ActiveStatusChangesOccur(ByVal activeState As Boolean) Handles PositionProperty.ActiveStatusChanges, ProductProperty.ActiveStatusChanges

        ProductExplorer.SetActiveCurrentSelection(activeState)

    End Sub

    Private Sub CategoryChanges(ByVal newCategory As Integer) Handles ProductProperty.CategoryChanges

        ProductExplorer.SetCategoryCurrentSelection(newCategory)

    End Sub

    ' RequestRecievedToEnableExploer
    ' one of the properties boxes has had its content changed
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub RequestRecievedToEnableExploer(ByVal newState As Boolean) Handles PositionProperty.EnableExplorer, ProductProperty.EnableExplorer

        ProductExplorer.Enable(newState)
        ReturnToVendButton.Enabled = newState

    End Sub

    ' ReturnToVendButton_Click
    ' the user wants to return to the vend application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ReturnToVendButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReturnToVendButton.Click

        Dim processStart As ProcessStartInfo = New ProcessStartInfo
        Dim applicationFolder As String = My.Application.Info.DirectoryPath

        processStart.UseShellExecute = False
        processStart.FileName = applicationFolder & "\vend.exe"
        processStart.WorkingDirectory = applicationFolder
        processStart.RedirectStandardOutput = False

        serialManager.Disconnect()
        System.Diagnostics.Process.Start(processStart)
        End

    End Sub

    ' Show the managers
    ' responding to button presses
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub FridgeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FridgeButton.Click
        If Not fridgeManager Is Nothing Then
            fridgeManager.Show()
        End If
    End Sub

    Private Sub SerialButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SerialButton.Click
        If Not serialManager Is Nothing Then
            serialManager.Show()
        End If
    End Sub

    Private Sub DebugButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebugButton.Click
        If Not debugInformation Is Nothing Then
            debugInformation.Show()
        End If
    End Sub

    Private Sub VideoButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VideoButton.Click
        If Not videoManager Is Nothing Then
            videoManager.Show()
        End If
    End Sub

    Private Sub SettingsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsButton.Click
        If Not settingsManager Is Nothing Then
            settingsManager.Show()
        End If
    End Sub

    Private Sub MechButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MechButton.Click
        If Not mechManager Is Nothing Then
            mechManager.Show()
        End If
    End Sub

    Private Sub PaymentButtonButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PaymentButton.Click

        Select Case settingsManager.GetValue("PaymentType")

            Case "MDB"
                If Not mdbManager Is Nothing Then
                    mdbManager.Show()
                End If

            Case "EPORT"
                If Not ePortManager Is Nothing Then
                    ePortManager.Show()
                End If

            Case "YESPAY"
                If Not yespayManager Is Nothing Then
                    yespayManager.Show()
                End If

        End Select

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        recieptPrinter.Reset()
        recieptPrinter.AddLine("[LogoHeader]")
        recieptPrinter.Print()
    End Sub

    Private Sub ActiveStatusChangesOccur(activeState As System.String) Handles ProductProperty.ActiveStatusChanges, PositionProperty.ActiveStatusChanges

    End Sub
End Class

