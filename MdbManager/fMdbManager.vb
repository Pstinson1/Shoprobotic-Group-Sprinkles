'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.Threading
Imports System.IO
Imports System.Data.SqlClient
Imports System.Diagnostics.FileVersionInfo
Imports System.Reflection.Assembly

Imports DebugWindow
Imports HelperFunctions
Imports SerialManager
Imports SettingsManager

' the serial manager form
' event driven serial IO
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fMdbManager


    Public AustraliaMDB As Boolean = False


    ' enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Message

        MDB_INSERT_CARD
        MDB_TRANSACTION_START_OK
        MDB_TRANSACTION_START_FAIL
        MDB_CARD_ACTIVITY
        MDB_CANCELLED
        MDB_AUTHORISATION_OK
        MDB_AUTHORISATION_FAIL
        MDB_FTL_TRANSFER_START
        MDB_FTL_TRANSFER
        MDB_RECIEPT_READY
        MDB_REPORT_READY
        MDB_FLT_RECEIPT_FAILURE
        MDB_FLT_AUDIT_FAILURE
        MDB_DISPLAY_REQUEST
    End Enum

    Public Enum FLT_TYPE

        FLT_NONE
        FLT_RECIEPT
        FLT_AUDIT
        FLT_REPORT
        FLT_UNKNOWN

    End Enum

    Public Enum AUTH_OUTCOME

        AUTH_NONE
        AUTH_APPROVED
        AUTH_DECLINED
        AUTH_CANCELED

    End Enum

    Public Enum VEND_OUTCOME

        VEND_NONE
        VEND_SUCCESS
        VEND_FAILED

    End Enum

    Public Enum SESSION_STATE

        SESSION_NONE
        SESSION_STARTED
        SESSION_ENDED

    End Enum

    Enum PAYMENT_STATE
        INACTIVE
        DISABLED
        ENABLED
        IDLE
        VEND
    End Enum

    Enum DeviceStates

        READER_RESET
        READER_CONFIG
        READER_MAXMIN
        READER_EXP_REQUESTID
        READER_EXP_ENABLEOPTIONS
        READER_DIAG_TIMEDATE
        READER_DIAGNOSTICS1
        READER_DIAGNOSTICS2
        READER_GPRS1
        READER_MAXMIN_EXP
        READER_DIAGNOSTICS
        READER_POLL
        READER_ENABLE
        READER_DISABLE
        READER_CANCEL
        READER_AUTHORISE
        READER_VEND_SUCCESS
        READER_VEND_FAIL
        READER_SESSION_COMPLETE_PRE_POLL
        READER_SESSION_COMPLETE
        READER_FTL_OKTOSEND
        READER_FTL_SEND_REQUEST
        READER_FTL_SEND_DATA
        READER_FTL_SEND
        READER_FTL_DENY
        READER_REQUEST_TO_RECIEVE
        READER_POLL_THEN_FORWARD

        READER_NULL

    End Enum

    ' structure
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Structure Transaction
        Dim transactionValuePence As Integer
        Dim productName As String
        Dim productId As Integer
        Dim recieptText As String
        Dim transactionId As String
    End Structure

    ' constants 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Const DELAY_SLOW = 500
    Private Const DELAY_NORMAL = 250 '150 australia 250 Europe
    Private Const DELAY_FAST = 150 '75 Europe 150 Australia 


    Private Const FLT_TIMEOUT = 100 '50 for Europe 100 for Australia
    Private Const REPORT_THRESHOLD = 20              ' reciepts are less than 20 blocks

    ' delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub CallbackDelegate(ByVal shortCode As Integer, ByVal eventCode As Integer, ByVal messageContent As String)

    ' managers
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    ' data tables
    Private currenciesTable() As String = {"AUD-036", "CAD-124", "CHF-756", "CNY-156", "EEK-233", "EGP-818", "EUR-978", "GBP-826", "HKD-344", "HUF-348", "JPY-392", "USD-840"}
    Private commandTable() As String = { _
        "MC5*OPT*TET*LNG*EN", _
        "MC5*OPT*TET*LNG*DE" _
    }




    ' variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private continueProcess As Boolean
    Private managementThread As Thread
    Private eventCallbackDelegate As CallbackDelegate = Nothing
    Private postPollDeviceState As DeviceStates
    Private deviceState As DeviceStates
    Private deviceStateDisplayed As DeviceStates
    Private deviceStateLogged As DeviceStates
    Private currentTransaction As Transaction
    Private incommingDataEvent As ManualResetEvent
    Private latestIncommingData As String
    Private pollCount As Integer
    Private sendBlockCount As Integer
    Private incommingFtlType As FLT_TYPE = FLT_TYPE.FLT_NONE
    Private sendBlockArray() As String
    Private databaseUpdateRequiredForSelection As Boolean = True
    Private pollingLoopToggle As Boolean = False
    Private expectedNumberOfBlocks As Integer
    Private loopDelay As Integer = DELAY_NORMAL
    Private transactionAuthorised As Boolean
    Private authorisationOutcome As AUTH_OUTCOME = AUTH_OUTCOME.AUTH_NONE
    Private vendOutcome As VEND_OUTCOME = VEND_OUTCOME.VEND_NONE
    Private sessionState As SESSION_STATE = SESSION_STATE.SESSION_NONE
    Private fltCountdown As Integer = 0
    Private paymentState As PAYMENT_STATE = PAYMENT_STATE.INACTIVE
    Private transactionNumber As String = ""
    Private incommingFltStore() As String
    Private lastMessageSent As String = ""

#Region "PUBLIC INTERFACES"

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()



        ' this line is suprizing important, it forced .net to assign a widow handle enabling the gui invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle
        Dim versionInfo As FileVersionInfo
        Dim lastFtlSend As String = ""

        ' get the managers
        helperFunctions = helperFunctionsFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()

        ' get serial comms events
        serialManager.AddCallback(AddressOf SerialPortEvent)

        ' create an event for when robo realm responds
        incommingDataEvent = New ManualResetEvent(False)

        ' initialise the states
        continueProcess = True
        deviceStateDisplayed = DeviceStates.READER_NULL
        deviceStateLogged = DeviceStates.READER_NULL

        PollThenOnTo(DeviceStates.READER_RESET, 1)

        ' create the database table
        If settingsManager.ConnectToDatabase() Then

            If Not settingsManager.TableExists("MdbCardTransaction") Then

                settingsManager.RunDatabaseNonQuery("CREATE TABLE [dbo].[MdbCardTransaction](" & _
                                                                                      "[TransactionId] [int] IDENTITY(1,1) NOT NULL," & _
                                                                                      "[Amount] [int] NULL," & _
                                                                                      "[Status] [int] NULL," & _
                                                                                      "[Date] [datetime] NULL," & _
                                                                                      "[Reciept] [nchar](500) NULL," & _
                                                                                      "[Audit] [nchar](500) NULL," & _
                                                                                      "[ProductDescription] [nchar](100) NULL" & _
                                                                                     ") ON [PRIMARY]")
            End If

            settingsManager.DisconnectFromDatabase()
        End If

        ' populate and select the currency combo
        PopulateAndSelectCurrency()

        ' fill in the about labels
        versionInfo = GetVersionInfo(GetExecutingAssembly.Location)

        helperFunctions.SetLabelText(CopyrightLabel, versionInfo.LegalCopyright.ToString())
        helperFunctions.SetLabelText(VersionLabel, versionInfo.ProductVersion.ToString())
        helperFunctions.SetLabelText(ProductLabel, versionInfo.ProductName.ToString())
        helperFunctions.SetLabelText(ModuleLabel, versionInfo.Comments.ToString())
        helperFunctions.SetLabelText(CompanyLabel, versionInfo.CompanyName.ToString())
        helperFunctions.SetLabelText(OperatingSystemLabel, helperFunctions.GetOSVersion())

        ' recover the last FTL send data.
        lastFtlSend = settingsManager.GetValue("SendFtlRecord")
        helperFunctions.SetTextBoxText(FtlSendText, lastFtlSend)

#If Australian = "True" Then
        AustraliaMDB = True
        LogMdbProgress("Device is set to Australian MDB : " & deviceState.ToString)
#Else
        LogMdbProgress("Device is set to Normal To change set the custom Constant at Build time: " & deviceState.ToString)
#End If


        ' start the management thread
        managementThread = New Thread(AddressOf BackgroundThreadProcess)
        managementThread.Name = "MDB"
        managementThread.Priority = ThreadPriority.Normal
        managementThread.Start()

    End Sub

    ' PopulateAndSelectCurrency
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PopulateAndSelectCurrency()

        Dim splitTableLine() As String
        Dim currencySelection As Integer

        ' populate
        CurrencyCombo.Items.Clear()

        For Each sngleCurrency As String In currenciesTable

            splitTableLine = sngleCurrency.Split("-")
            CurrencyCombo.Items.Add(splitTableLine(0))
        Next

        ' select
        currencySelection = CurrencyCombo.FindString(settingsManager.GetValue("MdbCurrencyCode"))

        If (currencySelection <> -1) Then

            databaseUpdateRequiredForSelection = False
            CurrencyCombo.SelectedIndex = currencySelection
            databaseUpdateRequiredForSelection = True
        End If

    End Sub

    ' CurrencyCodeFromMnenomic
    ' return a currency code from a given tla or etla - iso 4217
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function CurrencyCodeFromMnenomic(ByVal mnenomic As String) As String

        Dim requiredCode As String = ""
        Dim splitTableLine() As String

        For Each sngleCurrency As String In currenciesTable

            splitTableLine = sngleCurrency.Split("-")

            If splitTableLine.Length = 2 AndAlso splitTableLine(0) = mnenomic Then
                requiredCode = splitTableLine(1)
            End If
        Next

        Return requiredCode

    End Function

    ' Cancel
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Cancel() As Boolean

        debugInformation.Progress(fDebugWindow.Level.WRN, 2100, "MDB transaction cancel from state " & paymentState.ToString, True)

        Select Case paymentState

            Case PAYMENT_STATE.INACTIVE

            Case PAYMENT_STATE.DISABLED
                PollThenOnTo(DeviceStates.READER_RESET, 10)

            Case PAYMENT_STATE.ENABLED
                PollThenOnTo(DeviceStates.READER_DISABLE, 10)

            Case PAYMENT_STATE.IDLE
                PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 10)

            Case PAYMENT_STATE.VEND

                ' uninterruptable - possibly try a vend failed command to cancel the vend
                If authorisationOutcome = AUTH_OUTCOME.AUTH_NONE Then
                    debugInformation.Progress(fDebugWindow.Level.WRN, 2101, "no authorisation - cancel ", True)
                    PollThenOnTo(DeviceStates.READER_CANCEL, 1)
                End If


        End Select

        loopDelay = DELAY_NORMAL

        Return True

    End Function

    ' ResetTransaction
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ResetTransaction()

        ' initial state at the start of the vend
        vendOutcome = VEND_OUTCOME.VEND_NONE
        sessionState = SESSION_STATE.SESSION_NONE

    End Sub

    ' Authorise
    ' pre-authorise a sale
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Authorise(ByVal transactionValuePence As Integer, ByVal productName As String, ByVal productId As Integer) As Boolean

        Dim authoriseResult As Boolean = False
        Dim resultSet As System.Data.SqlClient.SqlDataReader = Nothing
        Dim safeProductName As String = ""

        If paymentState <> PAYMENT_STATE.DISABLED Then

            LogMdbProgress("Starting authorisation for " & transactionValuePence & " Failed, transaction not in correct state")
            LogMdbProgress("------------------------------------------------------------------------------------------------------------------")
            SendApplicationMessage(Message.MDB_TRANSACTION_START_FAIL, 0, "")

        Else

            LogMdbProgress("Starting authorisation for " & transactionValuePence & " OK")
            LogMdbProgress("------------------------------------------------------------------------------------------------------------------")

            PollThenOnTo(DeviceStates.READER_ENABLE, 0)

            transactionAuthorised = False

            currentTransaction.productId = productId
            currentTransaction.productName = productName
            currentTransaction.transactionValuePence = transactionValuePence
            currentTransaction.recieptText = ""
            authoriseResult = True

            ' initial state at the start of the vend required here because the config app doesn't issue Reset
            authorisationOutcome = AUTH_OUTCOME.AUTH_NONE
            vendOutcome = VEND_OUTCOME.VEND_NONE
            sessionState = SESSION_STATE.SESSION_NONE

            ' insert this transaction into the database
            If settingsManager.ConnectToDatabase() Then

                ' make sure that the name is not too long and dodgy charactors are removed
                safeProductName = settingsManager.MakeSqlSafe(productName, 99)

                ' enter the transaction into the table
                settingsManager.RunDatabaseNonQuery("Insert into mdbcardtransaction (Amount, Status, Date, ProductDescription) " & _
                                                                                       "Values  (" & transactionValuePence & ", 0, getdate(), " & "'" & safeProductName & "')")

                resultSet = settingsManager.RunDatabaseQuery("select max(transactionid) as maximumId from mdbcardtransaction")

                If resultSet.Read Then

                    currentTransaction.transactionId = resultSet("maximumId")

                    settingsManager.CloseQuery(resultSet)
                    settingsManager.DisconnectFromDatabase()

                    SendApplicationMessage(Message.MDB_INSERT_CARD, 0, "")

                Else
                    SendApplicationMessage(Message.MDB_TRANSACTION_START_FAIL, 0, "")
                End If
            Else

                SendApplicationMessage(Message.MDB_TRANSACTION_START_FAIL, 0, "")
            End If

        End If

        Return authoriseResult

    End Function

    ' StartBatch
    ' complete a pre-authorised sale
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StartBatch(ByVal vendSuccessful As Boolean)

        loopDelay = DELAY_NORMAL

        If (settingsManager.ConnectToDatabase()) Then
            settingsManager.RunDatabaseNonQuery("update mdbcardtransaction set status=" & IIf(vendSuccessful, 3, 4) & " where transactionid=" & currentTransaction.transactionId)
            settingsManager.DisconnectFromDatabase()
        End If

        If vendSuccessful Then
            vendOutcome = VEND_OUTCOME.VEND_SUCCESS
            PollThenOnTo(DeviceStates.READER_VEND_SUCCESS, 0)

        Else
            vendOutcome = VEND_OUTCOME.VEND_FAILED
            PollThenOnTo(DeviceStates.READER_VEND_FAIL, 0)

        End If

        currentTransaction.transactionValuePence = 0
        currentTransaction.productName = ""
        currentTransaction.productId = 0

    End Sub


    ' GetPrinterOutput
    ' return the printable reciept
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub GetPrinterOutput(ByRef salesReciept() As String)

        Dim lineIndex As Integer = 0
        Dim topmostLneIndex As Integer = 0
        Dim singleLine As String

        ' skim off empty lines at the end
        topmostLneIndex = 0

        For lineIndex = 0 To IncommingFltText.Lines.Length - 1
            singleLine = IncommingFltText.Lines(lineIndex)
            If singleLine.Trim() <> "" Then
                topmostLneIndex = lineIndex
            End If
        Next

        ' copy required lines into the reciept return
        ReDim salesReciept(topmostLneIndex)

        For lineIndex = 0 To topmostLneIndex
            singleLine = IncommingFltText.Lines(lineIndex)
            salesReciept(lineIndex) = singleLine
        Next

    End Sub

    Public Function GetTransactionNumber() As String
        Return transactionNumber
    End Function

    Public Sub Shutdown()

        ' terminate the thread
        continueProcess = False
        Thread.Sleep(200)

    End Sub

#End Region

#Region "MANAGEMENT THREAD"

    Private Sub PollThenOnTo(ByVal nextDeviceState As DeviceStates, ByVal pollCountBeforeForwarding As Integer)

        If pollCountBeforeForwarding = 0 Then

            deviceState = nextDeviceState
            LogMdbProgress("Device state changed to: " & deviceState.ToString)
        Else

            pollCount = pollCountBeforeForwarding
            postPollDeviceState = nextDeviceState
            deviceState = DeviceStates.READER_POLL_THEN_FORWARD

        End If

    End Sub

    Private Function EncodeGprsData(ByVal settingString As String, ByRef lengthString As String, ByRef dataString As String) As Boolean

        Dim lengthHigh As Integer
        Dim lengthLow As Integer
        Dim singleCharactor As Integer

        If settingString Is Nothing Then

            dataString = ""
            lengthString = ""

        Else

            lengthHigh = settingString.Length / &H100
            lengthLow = settingString.Length Mod &H100
            lengthString = lengthHigh.ToString("X2") & lengthLow.ToString("X2")

            dataString = ""

            For charactorIndex As Integer = 0 To settingString.Length - 1

                singleCharactor = Asc(settingString.Substring(charactorIndex, 1))
                dataString = dataString & singleCharactor.ToString("X2")
            Next

        End If

    End Function


    ' BackgroundThreadProcess
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub BackgroundThreadProcess()

        Dim readerResponse As String = ""
        Dim timeNow As Date
        Dim strData As String
        While continueProcess

            timeNow = Now

            If ProcessCheck.Checked Then

                Select Case deviceState

                    Case DeviceStates.READER_POLL_THEN_FORWARD

                        ' poll function that will pass to another state after a given number of polls
                        ReaderPoll()

                        ' do we move on now
                        If pollCount <> 0 Then _
                            pollCount = pollCount - 1

                        If pollCount = 0 Then


                            deviceState = postPollDeviceState
                            LogMdbProgress("Device state changed to: " & postPollDeviceState.ToString)
                        End If

                    Case DeviceStates.READER_RESET

                        ' reset the reader
                        If SendReader("MDB 10", readerResponse) Then
                            PollThenOnTo(DeviceStates.READER_CONFIG, 1)
                            LogMdbProgress("Resetting reader")
                        End If

                        loopDelay = DELAY_FAST

                    Case DeviceStates.READER_CONFIG

                        If AustraliaMDB Then
                            strData = "MDB 110083140201"
                        Else
                            strData = "MDB 110003100201"
                        End If
                        ' send config data
                        If SendReader(strData, readerResponse) Then        ' level 3
                            If AustraliaMDB Then
                                PollThenOnTo(DeviceStates.READER_MAXMIN, 4)
                            Else
                                PollThenOnTo(DeviceStates.READER_MAXMIN, 1)
                            End If

                            LogMdbProgress("Sending config data")
                        End If

                    Case DeviceStates.READER_MAXMIN

                        ' send unexpanded maximum and minimum prices
                        If SendReader("MDB 1101FFFF0000", readerResponse) Then
                            If AustraliaMDB Then
                                PollThenOnTo(DeviceStates.READER_DIAG_TIMEDATE, 1)
                            Else
                                PollThenOnTo(DeviceStates.READER_EXP_REQUESTID, 1)
                            End If

                            '       PollThenOnTo(DeviceStates.READER_EXP_ENABLEOPTIONS, 1)
                            LogMdbProgress("Sending max and min prices")
                        End If

                    Case DeviceStates.READER_DIAG_TIMEDATE


                        If SendReader("MDB 17FF030B" & timeNow.ToString("yyMMddHHmmss") & "FFFFFFFFFF", readerResponse) Then

                            loopDelay = DELAY_NORMAL
                            paymentState = PAYMENT_STATE.DISABLED
                            LogMdbProgress("Sending date and time")
                            PollThenOnTo(DeviceStates.READER_EXP_REQUESTID, 1)
                        End If
                    Case DeviceStates.READER_EXP_REQUESTID
                        If AustraliaMDB Then
                            strData = "MDB 17004845433030303030303130303431303230000000000000000000000000"
                        Else
                            strData = "MDB 17005445540000000000000000000000000000000000000000000000000000"
                        End If

                        ' request device id
                        If SendReader(strData, readerResponse) Then
                            PollThenOnTo(DeviceStates.READER_EXP_ENABLEOPTIONS, 1)
                            LogMdbProgress("Requesting Id")
                        End If

                    Case DeviceStates.READER_EXP_ENABLEOPTIONS
                        If AustraliaMDB Then
                            strData = "MDB 170400000001"
                        Else
                            strData = "MDB 170400000007"
                        End If
                        ' enable expansion options
                        If SendReader(strData, readerResponse) Then                          ' Multi currency/32bit/FTL 
                            If AustraliaMDB Then
                                PollThenOnTo(DeviceStates.READER_GPRS1, 1)
                            Else
                                PollThenOnTo(DeviceStates.READER_MAXMIN_EXP, 1)
                            End If

                            LogMdbProgress("Enable options")
                        End If

                    Case DeviceStates.READER_GPRS1

                        Dim apnDataLengthString As String = ""
                        Dim apnDataString As String = ""
                        Dim usernameDataLengthString As String = ""
                        Dim usernameDataString As String = ""

                        Dim passwordDataLengthString As String = ""
                        Dim passwordDataString As String = ""
                        Dim pinDataLengthString As String = ""
                        Dim pinDataString As String = ""

                        EncodeGprsData(settingsManager.GetValue("GprsAPN"), apnDataLengthString, apnDataString)
                        EncodeGprsData(settingsManager.GetValue("GprsUsername"), usernameDataLengthString, usernameDataString)
                        EncodeGprsData(settingsManager.GetValue("GprsPassword"), passwordDataLengthString, passwordDataString)
                        EncodeGprsData(settingsManager.GetValue("GprsPin"), pinDataLengthString, pinDataString)

                        ' GPRS APN, Username, Password and Pin
                        If SendReader("MDB 17FC1000C004" & apnDataLengthString & apnDataString & _
                            "C005" & usernameDataLengthString & usernameDataString & _
                            "C006" & passwordDataLengthString & passwordDataString & _
                            "C009" & pinDataLengthString & pinDataString, readerResponse) Then

                            PollThenOnTo(DeviceStates.READER_POLL, 1)
                            paymentState = PAYMENT_STATE.DISABLED
                            LogMdbProgress("Enable options")
                        End If

                    Case DeviceStates.READER_MAXMIN_EXP

                        ' set up the expanded maximum and minimum prices
                        If SendReader("MDB 1101FFFFFFFF00000000" & "1" & CurrencyCodeFromMnenomic(settingsManager.GetValue("MdbCurrencyCode")), readerResponse) Then

                            PollThenOnTo(DeviceStates.READER_DIAGNOSTICS, 1)
                            LogMdbProgress("Sending max and min prices (expanded)")
                        End If

                    Case DeviceStates.READER_DIAGNOSTICS

                        timeNow = Now
                        If SendReader("MDB 17FF030B" & timeNow.ToString("yyMMddHHmmss") & "FFFFFFFFFF", readerResponse) Then

                            loopDelay = DELAY_NORMAL
                            paymentState = PAYMENT_STATE.DISABLED
                            LogMdbProgress("Sending date and time")
                            PollThenOnTo(DeviceStates.READER_POLL, 1)
                        End If

                    Case DeviceStates.READER_ENABLE

                        ' enable the card reader
                        If SendReader("MDB 1401", readerResponse) Then

                            PollThenOnTo(DeviceStates.READER_POLL, 1)
                            LogMdbProgress("Enabling the reader")
                            paymentState = PAYMENT_STATE.ENABLED
                        End If

                    Case DeviceStates.READER_CANCEL

                        ' enable the card reader
                        LogMdbProgress("Cancelling the session")
                        If SendReader("MDB 1301", readerResponse) Then
                            LogMdbProgress("Cancel accepted")

                            PollThenOnTo(DeviceStates.READER_DISABLE, 4)
                        End If

                    Case DeviceStates.READER_FTL_SEND_REQUEST

                        If SendReader("MDB 17FE100040" & sendBlockArray.Length.ToString("X2") & "02", readerResponse) Then

                            PollThenOnTo(DeviceStates.READER_POLL, 0)
                            LogMdbProgress("Request to send FTL data to the terminal")

                        End If

                    Case DeviceStates.READER_FTL_SEND_DATA

                        If SendReader("MDB 17FC10" & sendBlockCount.ToString("X2") & sendBlockArray(sendBlockCount)) Then

                            LogMdbProgress("Sent ftl data block")
                            sendBlockCount = sendBlockCount + 1

                            If sendBlockCount >= sendBlockArray.Length - 1 Then
                                PollThenOnTo(DeviceStates.READER_POLL, 0)
                                LogMdbProgress("All ftl data sent")

                            End If

                        End If

                    Case DeviceStates.READER_POLL
                        ReaderPoll()

                    Case DeviceStates.READER_FTL_OKTOSEND

                        If SendReader("MDB 17FD1000", readerResponse) Then
                            LogMdbProgress("Sending ok to send block")
                            PollThenOnTo(DeviceStates.READER_POLL, 0)
                        End If

                    Case DeviceStates.READER_FTL_DENY

                        If SendReader("MDB 17FB100000", readerResponse) Then
                            LogMdbProgress("Denying permission to send")
                            PollThenOnTo(DeviceStates.READER_POLL, 0)
                        End If


                    Case DeviceStates.READER_AUTHORISE
                        If AustraliaMDB Then
                            strData = "X4" '16bit
                        Else
                            strData = "X8" ' 32 bit
                        End If

                        '16Bit MDB
                        If SendReader("MDB 1300" & currentTransaction.transactionValuePence.ToString(strData) & "FFFF", readerResponse) Then
                            paymentState = PAYMENT_STATE.VEND
                            PollThenOnTo(DeviceStates.READER_POLL, 0)
                            LogMdbProgress("Requested authorisation")
                        End If



                    Case DeviceStates.READER_VEND_SUCCESS

                        ' tell the reader that the vend succedded
                        If SendReader("MDB 1302FFFF", readerResponse) Then
                            paymentState = PAYMENT_STATE.IDLE

                            PollThenOnTo(DeviceStates.READER_POLL, 0)
                            '     PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 1)
                            LogMdbProgress("Told the reader that the vend succeded")
                        End If

                    Case DeviceStates.READER_VEND_FAIL

                        ' tell the reader that the vend failed
                        If SendReader("MDB 1303", readerResponse) Then
                            paymentState = PAYMENT_STATE.IDLE
                            PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE_PRE_POLL, 0)
                            LogMdbProgress("Told the reader that the vend failed")
                        End If

                    Case DeviceStates.READER_SESSION_COMPLETE_PRE_POLL

                        ' when the device starts polling again, complete the session
                        If SendReader("MDB 12", readerResponse) Then
                            PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 40)
                            loopDelay = DELAY_SLOW

                            LogMdbProgress("Reader detected on poll")
                        End If

                    Case DeviceStates.READER_SESSION_COMPLETE

                        ' complete the session
                        If SendReader("MDB 1304", readerResponse) Then

                            LogMdbProgress("Completing the session")
                            If AustraliaMDB Then
                                PollThenOnTo(DeviceStates.READER_POLL, 0)
                            Else
                                PollThenOnTo(DeviceStates.READER_DISABLE, 15)
                            End If

                        End If

                    Case DeviceStates.READER_DISABLE

                        ' disable the card reader
                        If SendReader("MDB 1400", readerResponse) Then

                            paymentState = PAYMENT_STATE.DISABLED
                            If AustraliaMDB Then
                                PollThenOnTo(DeviceStates.READER_POLL, 0)
                            Else
                                PollThenOnTo(DeviceStates.READER_RESET, 0) ' JON DEBUG
                            End If

                            '         PollThenOnTo(DeviceStates.READER_POLL, 0)
                            LogMdbProgress("Disabling the reader")
                        End If

                    Case DeviceStates.READER_REQUEST_TO_RECIEVE

                        If SendReader("MDB 17FA100010FF00", readerResponse) Then
                            LogMdbProgress("Requesting reconcilliation")
                            PollThenOnTo(DeviceStates.READER_POLL, 0)
                        End If

                End Select

                ' indicate the speed of the polling loop with the test panel
                If pollingLoopToggle Then
                    helperFunctions.SetPanelColour(PollingLoopPanel, System.Drawing.Color.Black)
                Else
                    helperFunctions.SetPanelColour(PollingLoopPanel, System.Drawing.Color.White)
                End If

                pollingLoopToggle = Not pollingLoopToggle

            End If

            ' display the device status
            If deviceState <> deviceStateDisplayed Then
                helperFunctions.SetLabelText(DeviceStateLabel, deviceState.ToString)
                deviceStateDisplayed = deviceState
            End If

            ' fltFailure - detect a loss of flt transmission
            If fltCountdown <> 0 Then

                fltCountdown = fltCountdown - 1
                If fltCountdown = 0 Then

                    LogMdbProgress("FLT failure, ending session")

                    Select Case incommingFtlType

                        Case FLT_TYPE.FLT_AUDIT
                            SendApplicationMessage(Message.MDB_FLT_AUDIT_FAILURE, 0, "")

                        Case FLT_TYPE.FLT_RECIEPT
                            SendApplicationMessage(Message.MDB_FLT_RECEIPT_FAILURE, 0, "")

                        Case Else

                    End Select

                    Select Case vendOutcome

                        Case VEND_OUTCOME.VEND_NONE

                        Case VEND_OUTCOME.VEND_FAILED
                            PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 6)

                        Case VEND_OUTCOME.VEND_SUCCESS
                            PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 6)
                    End Select

                End If

            End If

            ' this should be enough time for any data->data->ack sequences to complete
            If continueProcess Then _
                Thread.Sleep(loopDelay)

        End While

    End Sub


    Private Function ReaderPoll() As Boolean

        Dim pollResult As Boolean = False
        Dim readerResponse As String = ""
        Dim incommingBlockNumber As Integer
        Dim lastFtlBlock As Boolean = False
        Dim sqlSafeText As String = ""
        Dim totalIncomming As String = ""
        Dim transactionStartIndex As Integer

        If SendReader("MDB 12", readerResponse) Then

            ' straight Ack
            If readerResponse = "" Then

            ElseIf readerResponse.Length >= 2 Then

                Select Case readerResponse.Substring(0, 2)

                    Case "00"

                        ' just reset
                        ClearReaderConfigInfo()
                        ClearSessionInfo()
                        ClearManufacturerUnitData()

                        paymentState = PAYMENT_STATE.INACTIVE
                        sessionState = SESSION_STATE.SESSION_NONE

                        PollThenOnTo(DeviceStates.READER_CONFIG, 2)
                        LogMdbProgress("Poll: Just reset")

                    Case "01"
                        ' reader config information
                        DecodeReaderConfig(readerResponse.Substring(2))
                        LogMdbProgress("Poll: Config information")

                    Case "02"
                        ' display request
                        DecodeDisplayRequest(readerResponse.Substring(2))
                        SendApplicationMessage(Message.MDB_CARD_ACTIVITY, 0, "")
                        If AustraliaMDB Then
                            Dim displayText As String = DecodeDisplayRequest(readerResponse.Substring(2))
                            ' display request
                            SendApplicationMessage(Message.MDB_DISPLAY_REQUEST, 0, displayText)
                        End If
                    Case "03"
                        ' begin session
                        PollThenOnTo(DeviceStates.READER_AUTHORISE, 1)
                        sessionState = SESSION_STATE.SESSION_STARTED
                        paymentState = PAYMENT_STATE.IDLE
                        DecodeBeginSession(readerResponse.Substring(2))
                        LogMdbProgress("Poll: Begin session")
                        SendApplicationMessage(Message.MDB_TRANSACTION_START_OK, 0, "")
                        SendApplicationMessage(Message.MDB_CARD_ACTIVITY, 0, "")

                    Case "04"

                        ' session cancel request
                        authorisationOutcome = AUTH_OUTCOME.AUTH_CANCELED                        ' JON DEBUG
                        PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 20)
                        SendApplicationMessage(Message.MDB_CANCELLED, 0, "")
                        LogMdbProgress("Poll: Cancel session")

                    Case "05"


                        If authorisationOutcome = AUTH_OUTCOME.AUTH_CANCELED And AustraliaMDB Then    ' JON DEBUG

                            LogMdbProgress("Poll: Transaction approved after cancel - Approval will not be acted upon")    ' JON DEBUG

                        Else    ' JON DEBUG
                            ' card approved
                            authorisationOutcome = AUTH_OUTCOME.AUTH_APPROVED
                            SendApplicationMessage(Message.MDB_AUTHORISATION_OK, 0, "")
                            LogMdbProgress("Poll: Transaction approved")
                            loopDelay = DELAY_SLOW
                            transactionAuthorised = True

                            If (settingsManager.ConnectToDatabase()) Then
                                settingsManager.RunDatabaseNonQuery("update mdbcardtransaction set status=1 where transactionid=" & currentTransaction.transactionId)
                                settingsManager.DisconnectFromDatabase()
                            End If
                        End If

                    Case "06"

                        ' JON DEBUG
                        If authorisationOutcome = AUTH_OUTCOME.AUTH_NONE Then

                            ' card declined
                            SendApplicationMessage(Message.MDB_AUTHORISATION_FAIL, 0, "")
                            authorisationOutcome = AUTH_OUTCOME.AUTH_DECLINED
                            PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 10)  'HERE
                            LogMdbProgress("Poll: Transaction decined")

                            If (settingsManager.ConnectToDatabase()) Then
                                settingsManager.RunDatabaseNonQuery("update mdbcardtransaction set status=2 where transactionid=" & currentTransaction.transactionId)
                                settingsManager.DisconnectFromDatabase()

                            End If



                        End If
                    Case "07"

                        ' session has ended
                        sessionState = SESSION_STATE.SESSION_ENDED
                        PollThenOnTo(DeviceStates.READER_DISABLE, 0)

                        LogMdbProgress("Poll: Session ended")

                    Case "08"
                        ' session cancelled
                        authorisationOutcome = AUTH_OUTCOME.AUTH_CANCELED
                        PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 1)
                        LogMdbProgress("Poll: Session cancelled")

                    Case "09"
                        ' manufacturer unit data
                        DecodeManufacturerUnitData(readerResponse.Substring(2))
                        LogMdbProgress("Poll: Manufacturer/unit information")

                    Case "0B"
                        ' something has gone wrong, reset.
                        LogMdbProgress("Poll: Command out of sequence-resetting")
                        PollThenOnTo(DeviceStates.READER_RESET, 1)

                    Case "11"
                        LogMdbProgress("Time and date request recieved")

                    Case "1D"

                        SendApplicationMessage(Message.MDB_FTL_TRANSFER, 0, "")

                        fltCountdown = FLT_TIMEOUT

                        ' terminate on an empty block or when we get the expected blocks in
                        If readerResponse.Length = 6 Then
                            lastFtlBlock = True

                        ElseIf readerResponse.Length > 6 Then

                            ' some data has come in
                            incommingBlockNumber = CInt("&H" & readerResponse.Substring(4, 2))

                            If incommingBlockNumber = expectedNumberOfBlocks - 1 Then
                                lastFtlBlock = True
                            End If

                            ' store the data
                            If incommingBlockNumber < incommingFltStore.Length Then
                                Select Case incommingFtlType

                                    Case FLT_TYPE.FLT_AUDIT
                                        incommingFltStore(incommingBlockNumber) = helperFunctions.HexStringToAscii(readerResponse.Substring(6), True)

                                    Case FLT_TYPE.FLT_REPORT
                                        incommingFltStore(incommingBlockNumber) = helperFunctions.HexStringToAscii(readerResponse.Substring(6), False)

                                    Case FLT_TYPE.FLT_RECIEPT
                                        incommingFltStore(incommingBlockNumber) = helperFunctions.HexStringToAscii(readerResponse.Substring(6), False)

                                    Case Else
                                        incommingFltStore(incommingBlockNumber) = helperFunctions.HexStringToAscii(readerResponse.Substring(6), True)

                                End Select

                            End If

                            LogMdbProgress("Poll: ftl (block " & incommingBlockNumber & "): " & helperFunctions.HexStringToAscii(readerResponse.Substring(6), True))

                        End If

                        If lastFtlBlock Then

                            LogMdbProgress("Poll: All blocks recieved")
                            loopDelay = DELAY_NORMAL
                            fltCountdown = 0

                            ' collate the flt transmission and convert the line feeds into newlines..
                            For Each singleBlock As String In incommingFltStore
                                totalIncomming = totalIncomming & singleBlock
                            Next

                            '       totalIncomming = totalIncomming.Replace(ControlChars.Lf, ControlChars.NewLine)
                            totalIncomming = totalIncomming.Replace(ControlChars.Lf & ControlChars.Lf, ControlChars.NewLine)

                            ' is there a transaction number ?
                            '   transactionStartIndex = InStr(totalIncomming, "TXN")
                            Try
                                transactionStartIndex = totalIncomming.IndexOf("TXN")

                                If transactionStartIndex = -1 Then
                                    transactionNumber = "0"
                                Else
                                    transactionNumber = totalIncomming.Substring(transactionStartIndex + 4, 4).Trim()
                                End If

                            Catch ex As Exception

                            End Try

                            helperFunctions.SetTextBoxText(IncommingFltText, totalIncomming)

                            ' what has the reader transfered to us..
                            Select Case incommingFtlType

                                Case FLT_TYPE.FLT_RECIEPT

                                    ' log the reciept into the database
                                    LogMdbProgress("Poll: reciept ready.")
                                    sqlSafeText = settingsManager.MakeSqlSafe(IncommingFltText.Text, 499)

                                    If settingsManager.ConnectToDatabase() Then
                                        settingsManager.RunDatabaseNonQuery("update mdbcardtransaction set reciept='" & sqlSafeText & "' where transactionid=" & currentTransaction.transactionId)
                                        settingsManager.DisconnectFromDatabase()
                                    End If

                                    ' tell the application that a reciept is ready
                                    SendApplicationMessage(Message.MDB_RECIEPT_READY, 0, "")

                                Case FLT_TYPE.FLT_REPORT

                                    ' tell the application that a report is ready
                                    LogMdbProgress("Poll: report ready.")
                                    SendApplicationMessage(Message.MDB_REPORT_READY, 0, "")

                                Case FLT_TYPE.FLT_AUDIT

                                    ' log the audit report into the database
                                    LogMdbProgress("Poll: audit ready.")
                                    sqlSafeText = settingsManager.MakeSqlSafe(IncommingFltText.Text, 499)

                                    If settingsManager.ConnectToDatabase() Then
                                        settingsManager.RunDatabaseNonQuery("update mdbcardtransaction set audit='" & sqlSafeText & "' where transactionid=" & currentTransaction.transactionId)
                                        settingsManager.DisconnectFromDatabase()
                                    End If

                                Case Else

                            End Select

                            ' what state shall we go to next ?
                            If incommingFtlType = FLT_TYPE.FLT_REPORT Then
                                PollThenOnTo(DeviceStates.READER_POLL, 6)

                            Else
                                PollThenOnTo(DeviceStates.READER_SESSION_COMPLETE, 6)
                            End If

                        Else
                            ' request another block
                            PollThenOnTo(DeviceStates.READER_FTL_OKTOSEND, 0)
                        End If


                    Case "1E"

                        PollThenOnTo(DeviceStates.READER_FTL_SEND_DATA, 0)
                        LogMdbProgress("Poll: Recieved permission to send ftl data")

                    Case "1F"

                        ' ftl request
                        If readerResponse.Length >= 10 Then

                            expectedNumberOfBlocks = CInt("&H" & readerResponse.Substring(8, 2))

                            ' create a buffer large enough to contain the whole flt transmission
                            ReDim incommingFltStore(expectedNumberOfBlocks)

                            For blockIndex As Integer = 0 To expectedNumberOfBlocks
                                incommingFltStore(blockIndex) = ""
                            Next

                            ' what sort of data is going to arrive ?
                            Select Case readerResponse.Substring(6, 2)

                                Case "A0"

                                    If expectedNumberOfBlocks > REPORT_THRESHOLD Then
                                        incommingFtlType = FLT_TYPE.FLT_REPORT

                                    Else
                                        incommingFtlType = FLT_TYPE.FLT_RECIEPT
                                    End If

                                Case "A1"
                                    incommingFtlType = FLT_TYPE.FLT_AUDIT

                                Case Else

                                    incommingFtlType = FLT_TYPE.FLT_UNKNOWN

                            End Select

                            loopDelay = DELAY_FAST
                            fltCountdown = FLT_TIMEOUT
                            PollThenOnTo(DeviceStates.READER_FTL_OKTOSEND, 0)
                            SendApplicationMessage(Message.MDB_FTL_TRANSFER_START, 0, "")
                            LogMdbProgress("Poll: FTL request:" & incommingFtlType.ToString & "(" & expectedNumberOfBlocks & " blocks)")

                        End If

                    Case Else

                End Select

            End If

            pollResult = True

        End If

        ' decode the polling responses here
        Return pollResult

    End Function

#End Region

#Region "DECODE DATA FROM TERMINAL"

    Private Sub DecodeReaderConfig(ByVal configData As String)

        If configData.Length >= 14 Then

            helperFunctions.SetLabelText(FeatureLevelLabel, configData.Substring(0, 2))
            helperFunctions.SetLabelText(CountryCodeLabel, configData.Substring(2, 4))
            helperFunctions.SetLabelText(ScaleFactorLabel, configData.Substring(6, 2))
            helperFunctions.SetLabelText(DecimalPlacesLabel, configData.Substring(8, 2))
            helperFunctions.SetLabelText(MaximumResponseTimeLabel, configData.Substring(10, 2))
            helperFunctions.SetLabelText(MiscellaneousLabel, configData.Substring(12, 2))

        End If

    End Sub

    Private Function DecodeDisplayRequest(ByVal displayData As String)

        Dim displayMessage As String = ""
        Dim labelText As String = ""
        Dim linesDisplayed As Integer = 0

        If displayData.Length >= 2 Then displayMessage = helperFunctions.HexStringToAscii(displayData.Substring(2), False)

        LogMdbProgress("Poll: Display : " & displayMessage)

        While displayMessage.Length > 0 And linesDisplayed < 2

            If displayMessage.Length >= 16 Then
                labelText = labelText & displayMessage.Substring(0, 16) & Chr(10) & Chr(13)
            Else
                labelText = labelText & displayMessage & Chr(10) & Chr(13)
            End If

            If displayMessage.Length >= 16 Then
                displayMessage = displayMessage.Substring(16)
            End If

            linesDisplayed = linesDisplayed + 1
        End While

        helperFunctions.SetLabelText(DisplayLabel, labelText)
        Return labelText
    End Function

    Private Function EncodeFtlSendDataRequest(ByVal asciiString As String) As String

        Dim fltSendString As String = ""
        Dim singleCharactor As String = ""
        Dim byteCount As Integer = 0

        While byteCount < 26 And byteCount < asciiString.Length
            singleCharactor = asciiString.Substring(byteCount, 1)

            fltSendString = fltSendString & Asc(singleCharactor).ToString("X2")
            byteCount = byteCount + 1

        End While

        Return fltSendString

    End Function


    Private Sub DecodeManufacturerUnitData(ByVal readerData As String)

        If readerData.Length >= 58 Then
            helperFunctions.SetLabelText(ManufacturerCodeLabel, helperFunctions.HexStringToAscii(readerData.Substring(0, 6), False))
            helperFunctions.SetLabelText(SerialNumberLabel, helperFunctions.HexStringToAscii(readerData.Substring(6, 24), False))
            helperFunctions.SetLabelText(ModelNumberLabel, helperFunctions.HexStringToAscii(readerData.Substring(30, 24), False))
            helperFunctions.SetLabelText(SoftwareVersionLabel, readerData.Substring(54, 4))

        End If

    End Sub

    Private Sub DecodeBeginSession(ByVal sessionData As String)

        ' level 1 reader
        If sessionData.Length >= 8 Then helperFunctions.SetLabelText(FundsAvailableLabel, sessionData.Substring(0, 8))

        ' level 2-3 reader
        If sessionData.Length >= 16 Then helperFunctions.SetLabelText(PaymentMediaIdLabel, sessionData.Substring(8, 8))
        If sessionData.Length >= 18 Then helperFunctions.SetLabelText(PaymentTypeLabel, sessionData.Substring(16, 2))
        If sessionData.Length >= 22 Then helperFunctions.SetLabelText(PaymentDataLabel, sessionData.Substring(18, 4))

        ' level 3 (EXPANDED CURRENCY MODE) readers
        If sessionData.Length >= 26 Then helperFunctions.SetLabelText(UserLanguageLabel, sessionData.Substring(22, 4))
        If sessionData.Length >= 30 Then helperFunctions.SetLabelText(UserCurrencyCodeLabel, sessionData.Substring(26, 4))
        If sessionData.Length >= 32 Then helperFunctions.SetLabelText(CardOptionsLabel, sessionData.Substring(30, 2))

    End Sub

#End Region

#Region "CLEAR TEXTBOXES"
    Private Sub ClearReaderConfigInfo()

        helperFunctions.SetLabelText(FeatureLevelLabel, "")
        helperFunctions.SetLabelText(CountryCodeLabel, "")
        helperFunctions.SetLabelText(ScaleFactorLabel, "")
        helperFunctions.SetLabelText(DecimalPlacesLabel, "")
        helperFunctions.SetLabelText(MaximumResponseTimeLabel, "")
        helperFunctions.SetLabelText(MiscellaneousLabel, "")

    End Sub

    Private Sub ClearSessionInfo()

        helperFunctions.SetLabelText(FundsAvailableLabel, "")
        helperFunctions.SetLabelText(PaymentMediaIdLabel, "")
        helperFunctions.SetLabelText(PaymentTypeLabel, "")
        helperFunctions.SetLabelText(PaymentDataLabel, "")
        helperFunctions.SetLabelText(UserLanguageLabel, "")
        helperFunctions.SetLabelText(UserCurrencyCodeLabel, "")
        helperFunctions.SetLabelText(CardOptionsLabel, "")

    End Sub

    Private Sub ClearManufacturerUnitData()

        helperFunctions.SetLabelText(ManufacturerCodeLabel, "")
        helperFunctions.SetLabelText(SerialNumberLabel, "")
        helperFunctions.SetLabelText(ModelNumberLabel, "")
        helperFunctions.SetLabelText(SoftwareVersionLabel, "")

    End Sub

    Public Sub ClearIncommingFlt()
        helperFunctions.SetTextBoxText(IncommingFltText, "")
    End Sub

#End Region


    Private Function DecodeVendApproval(ByVal approvalData As String) As Integer

        Dim approvedAmount As Integer = CInt("&H" & approvalData.Substring(0, 4))

        Return approvedAmount

    End Function




#Region "SERIAL PORT"

    ' SendReader
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function SendReader(ByVal outgoingMessage As String, Optional ByRef incommingResponse As String = "") As Boolean

        Dim sendResult As Boolean = False

        incommingDataEvent.Reset()

        ' fix for MDBNAK
        If deviceState <> DeviceStates.READER_POLL Then
            lastMessageSent = outgoingMessage
        End If

        serialManager.SendMessage(outgoingMessage)

        '    If incommingDataEvent.WaitOne(200) Then
        Dim Wait1, Wait2 As Integer

        If AustraliaMDB Then
            Wait1 = 1000
            Wait2 = 200
        Else
            Wait1 = 300
            Wait2 = 300
        End If

        If incommingDataEvent.WaitOne(Wait1) Then
            incommingResponse = latestIncommingData

            If incommingResponse = "MDBNAK" Then

                LogMdbProgress("NAK Recieved, resending: " & outgoingMessage)

                incommingDataEvent.Reset()
                serialManager.SendMessage(outgoingMessage)

                '    If incommingDataEvent.WaitOne(200) Then
                If incommingDataEvent.WaitOne(Wait2) Then
                    incommingResponse = latestIncommingData

                End If

            Else
                sendResult = True

            End If





        End If

        Return sendResult

    End Function

    ' SerialPortEvent
    ' event driven incomming serial messages.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = Split(messageContent, "=")

            ' MDBNAK fix.
            If parameterList.Length = 1 AndAlso parameterList(0) = "MDBNAK" Then
                latestIncommingData = parameterList(0)
                incommingDataEvent.Set()

            ElseIf parameterList.Length = 2 AndAlso parameterList(0) = "MDB" Then
                latestIncommingData = parameterList(1)

                incommingDataEvent.Set()

            End If

        End If

    End Sub

#End Region

#Region "HELPER FUNCTIONS"

    Private Sub LogMdbProgress(ByVal progressMessage As String)

        helperFunctions.AddToListBox(ProgressList, progressMessage)
        debugInformation.Progress(fDebugWindow.Level.INF, 2102, progressMessage, True)

    End Sub

#End Region

#Region "APPLICATION MESSAGES"

    ' SetCallback
    ' store the callback address of the application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetCallback(ByVal dataCallbackFunction As CallbackDelegate)
        eventCallbackDelegate = dataCallbackFunction
    End Sub


    ' SendApplicationMessage
    ' notify the application that something has happenned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SendApplicationMessage(ByVal eventCode As Integer, ByVal integerValue As Integer, ByVal stringValue As String)

        If eventCallbackDelegate <> Nothing Then
            eventCallbackDelegate.Invoke(eventCode, integerValue, stringValue)
        End If
    End Sub

#End Region

#Region "BUTTONS"

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

    Private Sub CancelTransactionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelTransactionButton.Click
        Cancel()
    End Sub

    Private Sub AuthoriseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AuthoriseButton.Click

        Dim amountToAuthorise As Integer

        ' send how much a transaction is
        helperFunctions.StringToInteger(AmountText.Text, amountToAuthorise)
        Authorise(amountToAuthorise, "Unknown", 0)

    End Sub

    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click
        PollThenOnTo(DeviceStates.READER_RESET, 1)
    End Sub

    Private Sub VendSuccessButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendSuccessButton.Click
        StartBatch(True)

    End Sub

    Private Sub VendFailedButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendFailedButton.Click
        StartBatch(False)

    End Sub

    Private Sub FtlSendButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FtlSendButton.Click

        Dim messageToSend As String = ""
        Dim lastFtlSend As String
        Dim singleLine As String
        Dim blockIndex As Integer

        ' store the last message sent.
        lastFtlSend = FtlSendText.Text
        settingsManager.SetValue("SendFtlRecord", lastFtlSend)

        ' remove blank lines etc
        For lineIndex As Integer = 0 To FtlSendText.Lines.Length - 1

            singleLine = FtlSendText.Lines(lineIndex).Trim

            If singleLine <> "" Then
                messageToSend = messageToSend & singleLine & Chr(13) & Chr(10)
            End If

        Next

        ' break up the messages
        If messageToSend.Length <> 0 Then

            blockIndex = 0

            While messageToSend <> ""

                ReDim Preserve sendBlockArray(blockIndex)

                If messageToSend.Length <= 26 Then

                    sendBlockArray(blockIndex) = EncodeFtlSendDataRequest(messageToSend)
                    messageToSend = ""
                Else


                    sendBlockArray(blockIndex) = EncodeFtlSendDataRequest(messageToSend.Substring(0, 26))
                    messageToSend = messageToSend.Substring(26)
                End If

                blockIndex = blockIndex + 1


            End While

            sendBlockCount = 0
            PollThenOnTo(DeviceStates.READER_FTL_SEND_REQUEST, 0)

        End If

    End Sub

    Private Sub CurrencyCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrencyCombo.SelectedIndexChanged
        If databaseUpdateRequiredForSelection Then
            settingsManager.SetValue("MdbCurrencyCode", CurrencyCombo.SelectedItem.ToString)
        End If
    End Sub

#End Region

    Private Sub ReqToRecieveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReqToRecieveButton.Click

        PollThenOnTo(DeviceStates.READER_REQUEST_TO_RECIEVE, 0)

    End Sub

End Class

#Region "FACTORY"

' class cPowerMonitorFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cMdbManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared mdbManager As fMdbManager = Nothing

    Public Function GetManager() As fMdbManager

        If IsNothing(mdbManager) Then

            mdbManager = New fMdbManager

        End If

        Return mdbManager

    End Function

End Class

#End Region

