'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
' Imports
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports DebugWindow
Imports System.Threading
Imports System.Runtime.InteropServices
Imports HIDLibrary
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic.Compatibility
Imports SettingsManager

Public Class fEportManager

#Region "Structures"

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Structure CardDataStructure

        Dim Track1EncryptedData As Byte()
        Dim Track2EncryptedData As Byte()
        Dim Track3EncryptedData As Byte()
        Dim Track1MaskedData As Byte()
        Dim Track2MaskedData As Byte()
        Dim Track3MaskedData As Byte()
        Dim dukptPTSerial As Byte()
        Dim deviceSerial As Byte()
        Dim cardEncodeType As Integer

    End Structure

#End Region

#Region "Delegates"

    ' Delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub EventCallback(ByVal shortCode As Integer, ByVal eventCode As Integer, ByVal messageContent As String)
    Private Delegate Sub ProgressCallback(ByVal messageText As String)
    Private Delegate Sub StatusBarTextCallback(ByVal messageText As String)
    Private Delegate Sub SetTextBoxTextCallback(ByVal textBox As TextBox, ByVal message As String)
    Private Delegate Sub ReadHandlerDelegate(ByVal Report As HidReport)

#End Region

#Region "Constants"

    ' Constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Const TRACK_1_DECODE_STATUS = 0
    Const TRACK_2_DECODE_STATUS = 1
    Const TRACK_3_DECODE_STATUS = 2
    Const TRACK_1_ENCRYPT_LEN = 3
    Const TRACK_2_ENCRYPT_LEN = 4
    Const TRACK_3_ENCRYPT_LEN = 5
    Const CARD_ENCODE_TYPE = 6
    Const TRACK_1_ENCRYPT = 7
    Const TRACK_2_ENCRYPT = 119
    Const TRACK_3_ENCRYPT = 231
    Const CARD_STATUS = 343
    Const MAGNAPRINT_STATUS = 344
    Const MAGNAPRINT_DATA_LEN = 348
    Const MAGNAPRINT_DATA = 349
    Const DEVICE_SERIAL_NUMBER = 477
    Const READER_ENCRYPT_STATUS = 493
    Const DUKPT_SERIAL_NUMBER = 495
    Const TRACK_1_MASKED_LEN = 505
    Const TRACK_2_MASKED_LEN = 506
    Const TRACK_3_MASKED_LEN = 507
    Const TRACK_1_MASKED = 508
    Const TRACK_2_MASKED = 620
    Const TRACK_3_MASKED = 732
    Const ENCRYPT_SESSION_ID = 844
    Const TRACK_1_ABS_DATA_LEN = 852
    Const TRACK_2_ABS_DATA_LEN = 853
    Const TRACK_3_ABS_DATA_LEN = 854
    Const TRACK_3_ABS_MAGNAPRINT_DATA_LEN = 855

    Const ENCODE_ISA_ABA = 0
    Const ENCODE_AAMVA = 1
    Const ENCODE_CADL = 2
    Const ENCODE_BLANK = 3
    Const ENCODE_OTHER = 4
    Const ENCODE_UNDETERMINED = 5
    Const ENCODE_NONE = 6

    Const VEND_TYPE = 200
    Const HASHED_CARD_DATA = Nothing
    Const HASHED_CARD_DATA_LEN = 0
    Const QUANTITY = 1

    Const UNABLE_TO_LOG_TRANSACTION = -1

    Const ERROR_MESSAGE_BUFFER_LEN = 1025

#End Region

#Region "Enumerations"

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Message
        EP_APPROVAL_SWIPE_CARD
        EP_APPROVAL_RESWIPE_CARD
        EP_APPROVAL_SWIPED_OK
        EP_APPROVAL_FAIL
        EP_APPROVAL_DECLINED
        EP_APPROVAL_AUTHORISED
        EP_APPROVAL_COMMUNICATIONS_FAIL
    End Enum

    Public Enum EportSequenceStatus
        NULL
        CHECK_DATABASE
        INITIALISE
        IDLE
        AUTHORISE_START
        AUTHORISE_READ_CARD
        AUTHORISE_PROCESS
        APPROVAL_VENDING
        APPROVAL_BATCH
        APPROVAL_END
        RETRY_BATCH
    End Enum

    Public Enum AuthorisationOutcomes
        UNKNOWN
        AUTHORISED
        DECLINED
        COMMUNICATIONS_FAIL
        APPROVAL_FAIL
    End Enum

#End Region

#Region "Variables"

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private ePort As cEportInterface
    Private eventCallbackDelegate As EventCallback = Nothing
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private managementThread As Thread
    Private continueThread As Boolean = True
    Private readerConnected As Boolean = False
    Private eportStatus As EportSequenceStatus = EportSequenceStatus.CHECK_DATABASE
    Private recordedEportStatus As EportSequenceStatus = EportSequenceStatus.NULL
    Private batchRetryDue As Date
    Private cardHasBeenRead As Boolean

    Private currentCard As CardDataStructure
    Private selectionCode As String
    Private terminalSerialNo As String = ""
    Private mLast4Digits As String
    Private cardReaderActive As Boolean = False
    Private currentTransactionId As Integer
    Private databaseConnectionString As String = ""

    Private communicationMethod As Eport.cEportInterface.CommMethod = cEportInterface.CommMethod.TCPIP_LAN

    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory

    Private WithEvents hidDevice As HidDevice

#End Region

    ' Initialise
    ' store global manager settings and get it going..
    ' ---------------------------------------------------------
    '-------------------------------------------------------------------------------------------
    Public Sub Initialise(ByVal databaseConnectionString As String, ByVal batchRetryInterval As Integer, ByVal hidBufferLength As Integer, ByVal communicationsMethod As Eport.cEportInterface.CommMethod)

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the reader invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle

        debugInformation = debugInformationFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()

        Me.databaseConnectionString = databaseConnectionString
        '   Me.batchRetryInterval = batchRetryInterval
        '   Me.hidBufferLength = hidBufferLength
        Me.communicationMethod = communicationsMethod

        Progress("> Initialising ePort Manager")
        Progress("   Database connection string = " & databaseConnectionString)
        Progress("   Batch retry interval (seconds) = " & batchRetryInterval)
        Progress("   HID buffer length = " & hidBufferLength)
        Progress("   Communications method = " & communicationsMethod.ToString)

        managementThread = New Thread(AddressOf BackgroundThreadProcess)
        managementThread.Priority = ThreadPriority.BelowNormal
        managementThread.Start()

       
        batchRetryDue = Now().AddSeconds(20)
        ePort = New cEportInterface

    End Sub

    ' BackgroundThreadProcess
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub BackgroundThreadProcess()

        Dim loopDelay As Integer = 1000
        Dim authorisationResult As Eport.fEportManager.AuthorisationOutcomes

        Do While continueThread

            If eportStatus <> recordedEportStatus Then
                recordedEportStatus = eportStatus
                debugInformation.Progress(fDebugWindow.Level.INF, 9999, "ePort status changes to " & eportStatus.ToString, True)
            End If

            DetermineReaderConnection()
            StartBatchRetry()

            Select Case eportStatus

                Case EportSequenceStatus.CHECK_DATABASE

                    If CheckDatabase(databaseConnectionString) Then
                        eportStatus = EportSequenceStatus.INITIALISE
                        loopDelay = 100
                    Else
                        loopDelay = 5000
                    End If

                Case EportSequenceStatus.INITIALISE

                    If (InitialiseEport()) Then
                        eportStatus = EportSequenceStatus.IDLE
                        loopDelay = 100
                    Else
                        loopDelay = 5000
                    End If

                Case EportSequenceStatus.IDLE
                    loopDelay = 100

                Case EportSequenceStatus.AUTHORISE_START

                    If StartSession() Then

                        eportStatus = EportSequenceStatus.AUTHORISE_READ_CARD
                        SendApplicationMessage(Message.EP_APPROVAL_SWIPE_CARD, 0, "")

                    Else
                        eportStatus = EportSequenceStatus.IDLE
                        SendApplicationMessage(Message.EP_APPROVAL_COMMUNICATIONS_FAIL, 0, "")

                    End If

                Case EportSequenceStatus.AUTHORISE_READ_CARD

                    If cardHasBeenRead Then

                        ActivateReader(False)
                        eportStatus = EportSequenceStatus.AUTHORISE_PROCESS
                    End If

                Case EportSequenceStatus.AUTHORISE_PROCESS

                    authorisationResult = ProcessAuthorisation()

                    Select Case authorisationResult

                        Case AuthorisationOutcomes.APPROVAL_FAIL

                            eportStatus = EportSequenceStatus.APPROVAL_END
                            SendApplicationMessage(Message.EP_APPROVAL_FAIL, 0, "")
                            SetTransactionField(currentTransactionId, "Success", "99")
                            SetTransactionField(currentTransactionId, "ErrorCode", authorisationResult)

                        Case AuthorisationOutcomes.DECLINED

                            eportStatus = EportSequenceStatus.APPROVAL_END
                            SendApplicationMessage(Message.EP_APPROVAL_DECLINED, 0, "")
                            SetTransactionField(currentTransactionId, "Success", "99")
                            SetTransactionField(currentTransactionId, "ErrorCode", authorisationResult)

                        Case AuthorisationOutcomes.COMMUNICATIONS_FAIL

                            eportStatus = EportSequenceStatus.APPROVAL_END
                            SendApplicationMessage(Message.EP_APPROVAL_COMMUNICATIONS_FAIL, 0, "")
                            SetTransactionField(currentTransactionId, "Success", "99")
                            SetTransactionField(currentTransactionId, "ErrorCode", authorisationResult)

                        Case AuthorisationOutcomes.AUTHORISED

                            eportStatus = EportSequenceStatus.APPROVAL_VENDING
                            SendApplicationMessage(Message.EP_APPROVAL_AUTHORISED, 0, "")
                            SetTransactionField(currentTransactionId, "ErrorCode", authorisationResult)

                        Case Else

                            SendApplicationMessage(Message.EP_APPROVAL_COMMUNICATIONS_FAIL, 0, "")
                            SetTransactionField(currentTransactionId, "Success", "99")
                            SetTransactionField(currentTransactionId, "ErrorCode", authorisationResult)
                            Reset()

                    End Select

                Case EportSequenceStatus.APPROVAL_VENDING

                Case EportSequenceStatus.APPROVAL_BATCH
                    BatchTransaction(currentTransactionId)
                    eportStatus = EportSequenceStatus.APPROVAL_END

                Case EportSequenceStatus.APPROVAL_END
                    EndSession()
                    eportStatus = EportSequenceStatus.IDLE

                Case EportSequenceStatus.RETRY_BATCH
                    BatchRetry()
                    eportStatus = EportSequenceStatus.IDLE

            End Select

            If continueThread Then
                StatusBarText(eportStatus.ToString)
                Thread.Sleep(loopDelay)
            End If

        Loop

    End Sub

#Region "Load and close the form"

    ' ManagerFormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ManagerFormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

#End Region

    ' InitialiseLibrary
    ' start a session
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function InitialiseLibrary() As Boolean

        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim initialiseResult As Boolean = False

        debugInformation.Progress(fDebugWindow.Level.INF, 1204, "Initialising ePort library", True)

        Try
            If ePort.InitV3(communicationMethod, Nothing, errorMessage.Value) = cEportInterface.ServerResponse.RES_OK Then
                initialiseResult = True

            End If

        Catch ex As Exception
        End Try

        ' output debug data
        If initialiseResult Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1205, "ePort library initialised OK", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.INF, 1206, "ePort library initialisation failed", True)
        End If

        Return initialiseResult

    End Function

    ' Shutdown
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Shutdown()

        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim shutdownResult As Boolean = False

        ' allow the management thread to drop out
        continueThread = False
        Thread.Sleep(100)

        ' shut down the eport library
        debugInformation.Progress(fDebugWindow.Level.INF, 1271, "Shutting down ePort library", True)

        Try

            If ePort.Shutdown(errorMessage.Value) = cEportInterface.ServerResponse.RES_OK Then _
                shutdownResult = True

        Catch ex As Exception
        End Try

        If shutdownResult = False Then _
            debugInformation.Progress(fDebugWindow.Level.WRN, 1272, "Error shutting down ePort", True)

    End Sub

    ' UploadConfiguration
    ' upload configuration
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function UploadConfiguration() As cEportInterface.ServerResponse

        Dim configurationUploaded = False
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim uploadResult As cEportInterface.ServerResponse
        Dim attemptCount As Integer = 0

        debugInformation.Progress(fDebugWindow.Level.INF, 1211, "Starting to upload ePort configuration", True)

        While attemptCount < 3 And configurationUploaded = False

            Try
                uploadResult = ePort.UploadConfig(errorMessage.Value)

                If uploadResult = cEportInterface.ServerResponse.RES_OK Or uploadResult = cEportInterface.ServerResponse.RES_OK_NO_UPDATE Then _
                    configurationUploaded = True

            Catch ex As Exception
            End Try

            If configurationUploaded = False Then

                debugInformation.Progress(fDebugWindow.Level.WRN, 1213, "Failed to upload ePort configuration; retrying", True)
                Thread.Sleep(500)
                attemptCount += 1
            End If

        End While

        If configurationUploaded Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1212, "Uploaded ePort configuration OK", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.WRN, 1277, "Unable to upload ePort configuration", True)

        End If

        Return configurationUploaded

    End Function

    ' ProcessUpdate
    ' process update
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function ProcessUpdate() As Boolean

        Dim processUpdated = False
        Dim processResult As cEportInterface.ServerResponse
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim fileNames As String : fileNames = New String(vbNullChar, 10240)
        Dim fileTypes As String : fileTypes = New String(vbNullChar, 200)
        Dim filesReceived As Integer
        Dim attemptCount As Integer = 0

        debugInformation.Progress(fDebugWindow.Level.INF, 1214, "Starting ePort process updates", True)

        While attemptCount < 3 And processUpdated = False

            Try
                processResult = ePort.ProcessUpdates(filesReceived, fileNames, fileTypes, errorMessage.Value)
                If processResult = cEportInterface.ServerResponse.RES_OK Or processResult <> cEportInterface.ServerResponse.RES_OK_NO_UPDATE Or processResult <> cEportInterface.ServerResponse.RES_PARTIAL_OK Then _
                    processUpdated = True

            Catch ex As Exception
            End Try

            If processUpdated = False Then

                debugInformation.Progress(fDebugWindow.Level.WRN, 1216, "Failed to process ePort updates; retrying", True)
                Application.DoEvents()
                Thread.Sleep(50)
                attemptCount += 1
            End If

        End While

        If processUpdated Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1215, "Process ePort updates OK", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.WRN, 1275, "Unable to process ePort updates", True)

        End If

        Return processUpdated

    End Function

    ' StartSession
    ' start a session
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function StartSession() As Boolean

        Dim sessionBegan = False
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim attemptCount As Integer = 0

        debugInformation.Progress(fDebugWindow.Level.INF, 1208, "Starting ePort session", True)

        While attemptCount < 3 And sessionBegan = False
            Try
                Dim result As Eport.cEportInterface.ServerResponse
                result = ePort.SessionStart(errorMessage.Value)
                Select Case result
                    Case cEportInterface.ServerResponse.NOT_INITED
                        debugInformation.Progress(fDebugWindow.Level.INF, 1220, "Eport has not been inited", True)
                        sessionBegan = False
                    Case cEportInterface.ServerResponse.RES_ERROR
                        sessionBegan = False
                    Case cEportInterface.ServerResponse.RES_OK
                        sessionBegan = True
                End Select
             

            Catch ex As Exception
            End Try

            If sessionBegan = False Then

                debugInformation.Progress(fDebugWindow.Level.INF, 1210, "Failed to start ePort session; retrying", True)
                Thread.Sleep(500)
                attemptCount += 1
            End If

        End While

        If sessionBegan Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1209, "ePort session started OK", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.WRN, 1273, "Unable to start ePort session", True)

        End If

        Return sessionBegan

    End Function
    Public Sub ResetTransaction()

        EndSession()
        eportStatus = EportSequenceStatus.IDLE
    End Sub
    ' EndSession
    ' end a session
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function EndSession() As Boolean

        Dim sessionEnded = False
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim attemptCount As Integer = 0

        debugInformation.Progress(fDebugWindow.Level.INF, 1217, "Ending ePort session", True)

        While attemptCount < 3 And sessionEnded = False

            Try
                Dim result As Eport.cEportInterface.ServerResponse
                result = ePort.SessionClose(errorMessage.Value)
                Select Case result
                    Case cEportInterface.ServerResponse.RES_OK
                        sessionEnded = True
                    Case cEportInterface.ServerResponse.NO_SESSION
                        sessionEnded = True

                End Select

            Catch ex As Exception
            End Try

            If sessionEnded = False Then

                debugInformation.Progress(fDebugWindow.Level.INF, 1219, "Failed to end ePort session; retrying", True)
                Thread.Sleep(500)
                attemptCount += 1
            End If

        End While

        If sessionEnded Then
            debugInformation.Progress(fDebugWindow.Level.INF, 1218, "ePort session ended OK", True)

        Else
            debugInformation.Progress(fDebugWindow.Level.WRN, 1274, "Unable to end ePort session", True)

        End If

        Return sessionEnded

    End Function

    ' ProcessAuthorisation
    ' complete an authorisation, returning the outcome through the parameter
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ProcessAuthorisation() As AuthorisationOutcomes

        Dim authorisationResultCode As cEportInterface.ServerResponse
        Dim authOutcome As AuthorisationOutcomes
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim approvedAmount As Integer
        Dim orderNumber As Integer
        Dim transactionAmount As Integer
        Dim startSessionResult As Integer
        Dim attemptCount As Integer = 0
        Dim authorisationComplete As Boolean = False


        ' authorisation has started so mark it in the database
        SetTransactionField(currentTransactionId, "Success", "11")

        ' get what we need to ask for authorisation
        approvedAmount = 0
        orderNumber = Val(GetTransactionField(currentTransactionId, "TransactionID"))
        transactionAmount = Val(GetTransactionField(currentTransactionId, "TransactionAmount"))

        Progress("> Process Authorisation")
        Progress("   Transaction ID=" & orderNumber)
        Progress("   Transaction Amount=" & transactionAmount)



    

        Try
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "Starting authorisation", True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "order Number =" & orderNumber, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "transaction Amount =" & transactionAmount, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "approvedAmount =" & approvedAmount, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "currentCard.Track2MaskedData.Length =" & currentCard.Track2MaskedData.Length, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "currentCard.Track2EncryptedData.Length =" & currentCard.Track2EncryptedData.Length, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "currentCard.dukptPTSerial =" & currentCard.dukptPTSerial.Length, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "ByteArrayToString(currentCard.Track2MaskedData) =" & ByteArrayToString(currentCard.Track2MaskedData), True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "ePortInterface.CardType.CARD_TYPE_CREDIT =" & cEportInterface.CardType.CARD_TYPE_CREDIT, True)
            debugInformation.Progress(fDebugWindow.Level.INF, 7777, "ePortInterface.CardReaderTypes.CARD_READER_MAGTEK_MAGNESAFE =" & cEportInterface.CardReaderTypes.CARD_READER_MAGTEK_MAGNESAFE, True)

        Catch ex As Exception

        End Try

        If transactionAmount <= 0 Then
            authOutcome = AuthorisationOutcomes.APPROVAL_FAIL

        Else

            Try

                While attemptCount < 3 And authorisationComplete = False

                    ' get authorisation
                    authorisationResultCode = ePort.AuthV3_1 _
                    ( _
                        orderNumber, _
                        transactionAmount, _
                        approvedAmount, _
                        cEportInterface.CardType.CARD_TYPE_CREDIT, _
                        cEportInterface.CardReaderTypes.CARD_READER_MAGTEK_MAGNESAFE, _
                        currentCard.Track2MaskedData.Length, _
                        currentCard.Track2EncryptedData.Length, _
                        currentCard.Track2EncryptedData, _
                        currentCard.dukptPTSerial.Length, _
                        currentCard.dukptPTSerial, _
                        currentCard.Track2MaskedData.Length, _
                        ByteArrayToString(currentCard.Track2MaskedData), _
                        HASHED_CARD_DATA_LEN, HASHED_CARD_DATA, _
                        errorMessage.Value)

                    ' look at the result
                    Select Case authorisationResultCode

                        Case cEportInterface.ServerResponse.RES_AUTH

                            authOutcome = AuthorisationOutcomes.AUTHORISED
                            authorisationComplete = True
                            debugInformation.Progress(fDebugWindow.Level.INF, 1240, "Authorisation = AUTHORISED", True)

                        Case cEportInterface.ServerResponse.RES_DECL
                            authOutcome = AuthorisationOutcomes.DECLINED
                            authorisationComplete = True
                            debugInformation.Progress(fDebugWindow.Level.INF, 1236, "Authorisation = DECLINED", True)

                        Case cEportInterface.ServerResponse.NO_SESSION

                            attemptCount += 1

                            If (attemptCount = 2) Then

                                authorisationComplete = True
                                authOutcome = AuthorisationOutcomes.APPROVAL_FAIL
                                debugInformation.Progress(fDebugWindow.Level.WRN, 1238, "Authorisation = FAILED (NO SESSION)", True)

                            Else

                                ' start a new session
                                debugInformation.Progress(fDebugWindow.Level.INF, 1238, "Authorisation = FAILED (NO SESSION), opening session and having another go", True)

                                startSessionResult = StartSession()

                                If (startSessionResult = False) Then

                                    authorisationComplete = True
                                    debugInformation.Progress(fDebugWindow.Level.ERR, 1286, "Unable to start session on authorisation", True)
                                    authOutcome = AuthorisationOutcomes.APPROVAL_FAIL
                                End If

                            End If

                        Case cEportInterface.ServerResponse.RES_BUSY

                            attemptCount += 1

                            If (attemptCount = 3) Then

                                authOutcome = AuthorisationOutcomes.COMMUNICATIONS_FAIL
                                authorisationComplete = True
                                debugInformation.Progress(fDebugWindow.Level.WRN, 1237, "Authorisation = FAILED (BUSY)", True)

                            Else

                                Thread.Sleep(250)
                                debugInformation.Progress(fDebugWindow.Level.INF, 1287, "Authorisation = FAILED (BUSY), trying again", True)

                            End If

                        Case Else

                            authOutcome = AuthorisationOutcomes.COMMUNICATIONS_FAIL
                            authorisationComplete = True
                            debugInformation.Progress(fDebugWindow.Level.WRN, 1239, "Authorisation = FAILED (OTHER)", True)
                            debugInformation.Progress(fDebugWindow.Level.INF, 9998, authorisationResultCode.ToString, True)
                            debugInformation.Progress(fDebugWindow.Level.INF, 9999, errorMessage.ToString, True)

                    End Select

                End While

            Catch ex As Exception
                debugInformation.Progress(fDebugWindow.Level.INF, 9999, authorisationResultCode.ToString, True)
                debugInformation.Progress(fDebugWindow.Level.INF, 9999, ex.ToString, True)
                debugInformation.Progress(fDebugWindow.Level.ERR, 1278, "Authorisation = FAILED (EXCEPTION)", True)
                debugInformation.Progress(fDebugWindow.Level.ERR, 1278, errorMessage.ToString, True)

                authOutcome = AuthorisationOutcomes.APPROVAL_FAIL
            End Try

        End If
        Progress("   Message = " & errorMessage.ToString)

        Progress("   Approved Amount=" & approvedAmount)
        Progress("   " & authOutcome.ToString)

        ' after the authorisation has gone through, erase the card details from the debug edit boxes
        SetTextBoxText(Track1Edit, "")
        SetTextBoxText(Track2Edit, "")
        SetTextBoxText(Track3Edit, "")
        SetTextBoxText(Track1StatusEdit, "")
        SetTextBoxText(Track2StatusEdit, "")
        SetTextBoxText(Track3StatusEdit, "")

        Return authOutcome

    End Function

    ' StartBatch
    ' store the outcome of the vend process and start the approval batch
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StartBatch(ByVal vendSuccessful As Boolean)

        Progress("  Start batch: vend successful=" & vendSuccessful.ToString)
        debugInformation.Progress(fDebugWindow.Level.INF, 1201, "Start batch: vendSuccessful=" & vendSuccessful.ToString, True)

        If (vendSuccessful) Then

            SetTransactionField(currentTransactionId, "TransactionResult", cEportInterface.TranResult.TRAN_RESULT_SUCCESS)
        Else

            SetTransactionField(currentTransactionId, "TransactionResult", cEportInterface.TranResult.TRAN_RESULT_CANCELLED)
            SetTransactionField(currentTransactionId, "TransactionAmount", "0")

        End If

        eportStatus = EportSequenceStatus.APPROVAL_BATCH

    End Sub

    ' BatchRetry
    ' periodically try to batch previous failures 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub BatchRetry()

        Dim myConnection As SqlConnection
        Dim myCommand As SqlCommand
        Dim strSql As String
        Dim dr As SqlDataReader
        Dim dbTransactionID As Integer
        Dim Result As cEportInterface.ServerResponse

        Progress("> Batch retry")

        myConnection = New SqlConnection(databaseConnectionString)
        strSql = "SELECT EportTransactionID FROM  EportTransaction where Success = 11 And TransactionResult <> 0"

        Try
            myConnection.Open()
            myCommand = New SqlCommand(strSql, myConnection)

            dr = myCommand.ExecuteReader()

            If dr.HasRows Then

                debugInformation.Progress(fDebugWindow.Level.WRN, 1261, "Batch replay - failed batches has rows", True)
                Result = StartSession()

                If Result Then

                    While dr.Read()

                        'reading from the datareader
                        dbTransactionID = dr("EportTransactionID")

                        BatchTransaction(dbTransactionID)
                        Application.DoEvents()
                        Thread.Sleep(10)

                    End While
                    EndSession()

                Else
                    debugInformation.Progress(fDebugWindow.Level.WRN, 1264, "Cannot Open session to Replay batches", True)
                End If

            Else
                debugInformation.Progress(fDebugWindow.Level.INF, 1266, "No batches to replay", True)
            End If

        Catch ex As Exception

            If ex Is Nothing Then
                debugInformation.Progress(fDebugWindow.Level.WRN, 1267, "Failed on replaying failed batches ", True)

            Else
                Debug.WriteLine(ex.ToString)
                debugInformation.Progress(fDebugWindow.Level.WRN, 1267, "Failed on replaying failed batches " & ex.ToString, True)
            End If

        Finally
            myConnection.Close()
            myCommand = Nothing
            myConnection = Nothing

        End Try

    End Sub

    ' BatchTransaction
    ' completea given transaction
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub BatchTransaction(ByVal requiredTransactiion As Integer)

        Dim resultCode As cEportInterface.ServerResponse = cEportInterface.ServerResponse.RES_UNKNOWN
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)
        Dim transactionResult As Integer
        Dim transactionID As Integer
        Dim transactionAmount As Integer
        Dim transactionDetails As String
        Dim transactionDate As String

        transactionResult = Val(GetTransactionField(requiredTransactiion, "TransactionResult"))
        transactionID = Val(GetTransactionField(requiredTransactiion, "TransactionID"))
        transactionAmount = Val(GetTransactionField(requiredTransactiion, "TransactionAmount"))
        transactionDetails = GetTransactionField(requiredTransactiion, "TransactionDetails")
        transactionDate = GetTransactionField(requiredTransactiion, "Date")

        Progress("> Batching transaction: #" & requiredTransactiion)
        Progress("   Transaction result=" & transactionResult.ToString)
        Progress("   Transaction ID=" & transactionID)
        Progress("   Transaction Details=" & transactionDetails)
        Progress("   Transaction Date=" & transactionDate)

        Try

            'transactionID,TransactionAmount,TransactionDetails,lenTransactionDetails,Status
            If transactionResult = cEportInterface.TranResult.TRAN_RESULT_SUCCESS Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1258, "Batch successfull vend " & requiredTransactiion.ToString, True)
                resultCode = ePort.BatchV3(transactionID, transactionAmount, transactionResult, transactionDetails, Len(transactionDetails), errorMessage.Value)

            Else
                debugInformation.Progress(fDebugWindow.Level.WRN, 1259, "Batch failed vend " & requiredTransactiion.ToString, True)
                resultCode = ePort.BatchV3(transactionID, 0, transactionResult, selectionCode, Len(selectionCode), errorMessage.Value)
            End If


            Progress("   Batching result = " & resultCode.ToString)
            SetTransactionField(requiredTransactiion, "ErrorCode", resultCode)

            Select Case resultCode

                ' process the outcome
                Case cEportInterface.ServerResponse.RES_BATCH_PASS
                    SetTransactionField(requiredTransactiion, "Success", "12")
                    debugInformation.Progress(fDebugWindow.Level.INF, 1245, "Batch pass" & "VendStatus=" & transactionResult.ToString & " Amount =" & transactionAmount, True)

                Case cEportInterface.ServerResponse.RES_BATCH_FAIL       ' RES_BATCH_FAIL Batch failed ' can also happen when transaction was never authed
                    Progress("   Fail: " & errorMessage.Value)
                    SetTransactionField(requiredTransactiion, "Success", "12")

                Case cEportInterface.ServerResponse.RES_BATCH_ERROR   ' RES_BATCH_ERROR Batch failed, error occurred
                    Progress("   Fail: " & errorMessage.Value)

                Case cEportInterface.ServerResponse.RES_BUSY                    ' RES_BUSY Unable to communicate with server
                    Progress("   Busy: " & errorMessage.Value)

                Case cEportInterface.ServerResponse.RES_ERROR                ' RES_ERROR Function failed, errors encountered
                    Progress("   Error: " & errorMessage.Value)

                Case cEportInterface.ServerResponse.NO_SESSION
                    Progress("   Error: " & errorMessage.Value)

                Case Else
                    Progress("   Error: " & errorMessage.Value)

            End Select

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 1267, "Failed on BatchTransaction: " & ex.ToString, True)
            debugInformation.Progress(fDebugWindow.Level.ERR, 1267, "Failed on BatchTransaction: " & errorMessage.ToString, True)

            Progress("   Exception code = " & ex.Message)
        End Try

    End Sub

    ' GetSystemTime
    ' number of seconds since 1st January 1970
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function GetSystemTime(ByVal EndDateTime As Date, Optional ByVal StartDateTime As Date = #1/1/1970#) As Integer
        GetSystemTime = DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartDateTime, EndDateTime)
    End Function

    ' LogEportTransaction
    ' enter the transaction into the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function LogEportTransaction(ByVal productId As Integer, ByVal productName As String, ByVal transactionAmount As Integer) As Integer

        Dim databaseConnection As SqlConnection
        Dim insertionCommand As SqlCommand = New SqlCommand
        Dim getIdentifierCommand As SqlCommand = New SqlCommand
        Dim queryResult As SqlDataReader
        Dim newTransactionId = UNABLE_TO_LOG_TRANSACTION
        Dim transactionId As Integer
        Dim transactionDetails As String

        ' build the transaction string and generate a transaction Id for ePort from the system time
        transactionId = GetSystemTime(Now())
        transactionDetails = selectionCode & "|" & StripBadChars(VEND_TYPE.ToString) & "|" & transactionAmount & "|" & StripBadChars(QUANTITY.ToString) & "|" & transactionId & "-" & StripBadChars(productName) & "|"

        debugInformation.Progress(fDebugWindow.Level.INF, 1285, "Logging Eport trans: " & transactionDetails, True)

        Try
            databaseConnection = New SqlConnection(databaseConnectionString)

            insertionCommand.Connection = databaseConnection
            insertionCommand.CommandText = _
                        "Insert into EportTransaction (TransactionID, TransactionAmount, TransactionDetails, TransactionResult, ErrorCode,  Success)" & _
                        "Values  (" & transactionId & "," & transactionAmount & "," & "'" & transactionDetails & "'," & " 0, 0, 10)"

            getIdentifierCommand.CommandText = "Select max(EportTransactionId) as maximumId from EportTransaction"
            getIdentifierCommand.Connection = databaseConnection

            ' run the query to insert the transactionand get the identifier
            databaseConnection.Open()
            insertionCommand.ExecuteNonQuery()

            queryResult = getIdentifierCommand.ExecuteReader()

            ' recover the new Identifier
            If queryResult.HasRows Then

                queryResult.Read()
                newTransactionId = Val(queryResult("maximumId").ToString)

            Else
                newTransactionId = UNABLE_TO_LOG_TRANSACTION
            End If

            ' cleanup
            databaseConnection.Close()
            databaseConnection = Nothing

        Catch ex As Exception
        End Try

        Return newTransactionId

    End Function


    ' GetTransactionField
    ' recover a field from the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function GetTransactionField(ByVal transactionID As Integer, ByVal fieldName As String) As String

        Dim databaseConnection As SqlConnection
        Dim queryCommand As SqlCommand = New SqlCommand
        Dim queryResult As SqlDataReader
        Dim fieldResult As String = ""

        Try
            databaseConnection = New SqlConnection(databaseConnectionString)
            queryCommand.CommandText = "Select " & fieldName & " from EportTransaction where EportTransactionID=" & transactionID
            queryCommand.Connection = databaseConnection

            ' recover the data field
            databaseConnection.Open()

            queryResult = queryCommand.ExecuteReader()
            If queryResult.HasRows Then

                queryResult.Read()
                fieldResult = queryResult(fieldName)
            End If

            ' shut down the datadase connection
            databaseConnection.Close()
            databaseConnection = Nothing

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 1283, "Unable to recover field from EportTransaction" & queryCommand.ToString, True)
        End Try

        Return fieldResult

    End Function

    ' SetTransactionField
    ' recover a field from the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function SetTransactionField(ByVal transactionID As Integer, ByVal fieldName As String, ByVal fieldValue As String) As Boolean

        Dim databaseConnection As SqlConnection
        Dim updateCommand As SqlCommand = New SqlCommand
        Dim updateResult As Boolean = False

        Try
            databaseConnection = New SqlConnection(databaseConnectionString)
            updateCommand.CommandText = "Update EportTransaction Set " & fieldName & "=" & fieldValue & " where EportTransactionID=" & transactionID
            updateCommand.Connection = databaseConnection

            ' set the data field
            databaseConnection.Open()

            updateCommand.ExecuteNonQuery()

            ' shut down the datadase connection
            databaseConnection.Close()
            databaseConnection = Nothing

            updateResult = True

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 1284, "Unable to execute update on EportTransaction" & updateCommand.ToString, True)
        End Try

        Return updateResult

    End Function

    ' InitialiseEport
    ' sort out the configuration
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function InitialiseEport() As Boolean

        Progress("> Initialising Eport")

        Progress("   Initialising Library")                               ' InitSynchronous
        If InitialiseLibrary() = False Then
            Progress("   Failed")
            Return False
        End If

        Progress("   Begin Session")                                    ' begin the session
        If StartSession() = False Then
            Progress("   Failed")
            Return False
        End If

        Progress("   Upload Configuration")                        ' upload configuration
        If UploadConfiguration() = False Then
            Progress("   Failed")
            Return False
        End If

        Progress("   Process Update")                                 ' process update 
        If ProcessUpdate() = False Then
            Progress("   Failed")
            Return False
        End If

        Progress("   End Session")                                       ' end the session
        If EndSession() = False Then
            Progress("   Failed")
            Return False
        End If

        Progress("   OK")

        Return True

    End Function

    ' Authorise
    ' if the reader is ready start the autorisation process
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Authorise(ByVal newTransactionValue As Integer, ByVal newProductDescription As String, ByVal newProductId As String) As Boolean

        Progress("> Transaction requested ")
        Progress("   Value " & newTransactionValue)
        Progress("   Description " & newProductDescription)
        Progress("   Product Id " & newProductId)

        debugInformation.Progress(fDebugWindow.Level.INF, 1254, "Product description " & newProductDescription, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1255, "Product value " & newTransactionValue, True)
        debugInformation.Progress(fDebugWindow.Level.INF, 1257, "Product id " & newProductId, True)

        selectionCode = "A0"
        mLast4Digits = Last4Digits()

        If eportStatus = EportSequenceStatus.IDLE Then

            currentTransactionId = LogEportTransaction(newProductId, newProductDescription, newTransactionValue)

            cardHasBeenRead = False
            ActivateReader(True)

            eportStatus = EportSequenceStatus.AUTHORISE_START

            Return True
        Else

            debugInformation.Progress(fDebugWindow.Level.WRN, 1279, "Unable to authorise, ePort not ready", True)
            Progress("   Eport not ready")

            Return False

        End If

    End Function

    ' Last4Digits
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public ReadOnly Property Last4Digits() As String

        Get
            Dim track1Data As String = ByteArrayToString(currentCard.Track1MaskedData)

            Try
                If Mid(track1Data, 1, 1) = "%" Then

                    Return Mid(Mid(track1Data, 2, InStr(track1Data, "^") - 2), Mid(track1Data, 2, InStr(track1Data, "^") - 2).Length - 3, 4)

                ElseIf Mid(track1Data, 1, 1) = ";" Then
                    'Track 2
                    Return Mid(Mid(track1Data, 2, InStr(track1Data, "=") - 2), Mid(track1Data, 2, InStr(track1Data, "=") - 2).Length - 3, 4)

                End If

                Return mLast4Digits
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    ' StartBatchRetry
    ' periodically try to batch previous failures 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub StartBatchRetry()

        If eportStatus = EportSequenceStatus.IDLE And Now > batchRetryDue Then

            eportStatus = EportSequenceStatus.RETRY_BATCH
            batchRetryDue = Now.AddSeconds(settingsManager.GetValue("EportBatchRetryInterval"))

        End If

    End Sub

    ' StripBadChars
    ' remove charactors that will make an sql statement or eport transaction fail
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function StripBadChars(ByVal strInput As String) As String

        strInput = Replace(strInput, "'", "")
        strInput = Replace(strInput, "|", "")
        strInput = Replace(strInput, "-", "")

        Return strInput

    End Function

    ' Reset
    ' if not setting up, return to a 'neutral' non active state.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Reset()

        If eportStatus > EportSequenceStatus.IDLE Then
            ActivateReader(False)
            Progress("> Resetting credit card")
            EndSession()

            eportStatus = EportSequenceStatus.IDLE

        End If

    End Sub

    ' GetUnitSerialNo
    ' return the units serial number
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function GetUnitSerialNo() As String

        Dim paramKey As String = "SSNHex"
        Dim paramValue As String = ""
        Dim errorMessage As New VB6.FixedLengthString(ERROR_MESSAGE_BUFFER_LEN)

        Try
            ePort.GetConfigValue(paramKey, paramValue, errorMessage.Value)
        Catch ex As Exception
        End Try

        Return paramValue

    End Function


 #Region "Hid - card reader"

    ' DetermineReaderConnection
    ' ensure that the reader is connected and assign set up of reads when one is.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DetermineReaderConnection()

        Dim vendorID As Short ' = CShort(Val("&h" & "0801"))
        Dim productID As Short ' = CShort(Val("&h" & "0011"))
        Dim hidDeviceList As HIDLibrary.HidDevice()

        vendorID = CShort(Val("&h" & settingsManager.GetValue("EportVendorID")))
        productID = CShort(Val("&h" & settingsManager.GetValue("EportProductID")))

        hidDeviceList = HidDevices.Enumerate(vendorID, productID)

        If (readerConnected) Then

            If hidDeviceList.Length = 0 Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1223, "Card reader removed", True)
                Progress("> Hid disconnected")
                readerConnected = False
            End If

        Else

            If hidDeviceList.Length > 0 Then

                debugInformation.Progress(fDebugWindow.Level.INF, 1224, "Card reader detected", True)
                hidDevice = hidDeviceList(0)
                hidDevice.Open()
                hidDevice.ReadReport(AddressOf ReadProcess)
                Progress("> Hid Connected = " & hidDevice.Description)
                readerConnected = True

            End If

        End If

    End Sub

    ' ReadProcess
    ' invoke the read handler when ready
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ReadProcess(ByVal Report As HidReport)
        Me.BeginInvoke(New ReadHandlerDelegate(AddressOf ReadHandler), New Object() {Report})
    End Sub

    ' ReadHandler
    ' the reading process
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ReadHandler(ByVal readReport As HidReport)

        Dim cardData() As Byte

        If readerConnected Then

            Select Case readReport.ReadStatus

                Case HidDeviceData.ReadStatus.Success

                    cardData = readReport.Data

                    If Not cardReaderActive Then
                        Progress("> Read complete - but not requested")
                        debugInformation.Progress(fDebugWindow.Level.INF, 1225, "Card read when not requested", True)

                    ElseIf (ProcessCard(cardData)) Then
                        Progress("> Read complete - OK")
                        debugInformation.Progress(fDebugWindow.Level.INF, 1251, "Card read OK: " & ByteArrayToString(currentCard.Track2MaskedData), True)
                        cardHasBeenRead = True
                        SendApplicationMessage(Message.EP_APPROVAL_SWIPED_OK, 0, "")

                    Else
                        Progress("> Read complete - invalid, reswipe required")
                        debugInformation.Progress(fDebugWindow.Level.INF, 1227, "Card read invalid, reswipe required", True)
                        SendApplicationMessage(Message.EP_APPROVAL_RESWIPE_CARD, 0, "")

                    End If

                Case HidDeviceData.ReadStatus.WaitTimedOut
                    debugInformation.Progress(fDebugWindow.Level.INF, 1249, "Card read failed: HidDeviceData.ReadStatus.WaitTimedOut", True)

                Case HidDeviceData.ReadStatus.WaitFail
                    debugInformation.Progress(fDebugWindow.Level.INF, 1249, "Card read failed: HidDeviceData.ReadStatus.WaitFail", True)

                Case HidDeviceData.ReadStatus.NoDataRead
                    debugInformation.Progress(fDebugWindow.Level.INF, 1249, "Card read failed: HidDeviceData.ReadStatus.NoDataRead", True)

                Case HidDeviceData.ReadStatus.ReadError
                    debugInformation.Progress(fDebugWindow.Level.INF, 1249, "Card read failed: HidDeviceData.ReadStatus.ReadError", True)
                    Progress("> Read Fail")

                Case HidDeviceData.ReadStatus.NotConnected
                    debugInformation.Progress(fDebugWindow.Level.INF, 1250, "Card read not connected ?", True)

            End Select

            ' set up for the next read
            ' hidDevice.ReadReport(AddressOf ReadProcess)

        End If

        ' set up for the next read
        hidDevice.ReadReport(AddressOf ReadProcess)

    End Sub

    ' ActivateReader
    ' start processing card reader data
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ActivateReader(ByVal newReaderState As Boolean)

        cardReaderActive = newReaderState

        debugInformation.Progress(fDebugWindow.Level.INF, 1222, "Activating card reader = " & newReaderState.ToString, True)
        Progress("> Activate card reader = " & newReaderState.ToString)

    End Sub

    ' CopyByteBuffer
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function CopyByteBuffer(ByVal dataBuffer() As Byte, ByVal startIndex As Integer, ByVal bufferLength As Integer) As Byte()

        Dim returnArray() As Byte = Nothing
        Dim byteIndex As Integer

        returnArray = New Byte(bufferLength - 1) {}
        For byteIndex = 0 To bufferLength - 1
            returnArray(byteIndex) = dataBuffer(byteIndex + startIndex)
        Next

        Return returnArray

    End Function

    ' ProcessCard
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ProcessCard(ByVal data() As Byte) As Boolean

        ' decode data 
        If data.Length <> settingsManager.GetValue("ReaderHidBufferLength") Then

            debugInformation.Progress(fDebugWindow.Level.INF, 1282, "   Unreadable card buffer: " & data.Length, True)
            Progress("   Unreadable card")
            Return False
        End If

        currentCard.Track1MaskedData = CopyByteBuffer(data, TRACK_1_MASKED, data(TRACK_1_MASKED_LEN))
        currentCard.Track1EncryptedData = CopyByteBuffer(data, TRACK_1_ENCRYPT, data(TRACK_1_ENCRYPT_LEN))
        currentCard.Track2MaskedData = CopyByteBuffer(data, TRACK_2_MASKED, data(TRACK_2_MASKED_LEN))
        currentCard.Track2EncryptedData = CopyByteBuffer(data, TRACK_2_ENCRYPT, data(TRACK_2_ENCRYPT_LEN))
        currentCard.Track3MaskedData = CopyByteBuffer(data, TRACK_3_MASKED, data(TRACK_3_MASKED_LEN))
        currentCard.Track3EncryptedData = CopyByteBuffer(data, TRACK_3_ENCRYPT, data(TRACK_3_ENCRYPT_LEN))
        currentCard.cardEncodeType = data(CARD_ENCODE_TYPE)
        currentCard.deviceSerial = CopyByteBuffer(data, DEVICE_SERIAL_NUMBER, 15)
        currentCard.dukptPTSerial = CopyByteBuffer(data, DUKPT_SERIAL_NUMBER, 10)

        SetTextBoxText(Track1Edit, ByteArrayToString(currentCard.Track1MaskedData))
        SetTextBoxText(Track2Edit, ByteArrayToString(currentCard.Track2MaskedData))
        SetTextBoxText(Track3Edit, ByteArrayToString(currentCard.Track3MaskedData))
        SetTextBoxText(Track1StatusEdit, Val(data(TRACK_1_DECODE_STATUS)))
        SetTextBoxText(Track2StatusEdit, Val(data(TRACK_2_DECODE_STATUS)))
        SetTextBoxText(Track3StatusEdit, Val(data(TRACK_3_DECODE_STATUS)))
        SetTextBoxText(DeviceSerialEdit, ByteArrayToString(currentCard.deviceSerial))
        SetTextBoxText(Last4DigitsEdit, Last4Digits())

        Select Case data(CARD_ENCODE_TYPE)

            Case ENCODE_ISA_ABA
                SetTextBoxText(EncodingEdit, "ENCODE_ISA_ABA")
            Case ENCODE_AAMVA
                SetTextBoxText(EncodingEdit, "ENCODE_AAMVA")
            Case ENCODE_CADL
                SetTextBoxText(EncodingEdit, "ENCODE_CADL")
            Case ENCODE_BLANK
                SetTextBoxText(EncodingEdit, "ENCODE_BLANK")
            Case ENCODE_UNDETERMINED
                SetTextBoxText(EncodingEdit, "ENCODE_UNDETERMINED")
            Case ENCODE_NONE
                SetTextBoxText(EncodingEdit, "ENCODE_NONE")

        End Select

        ' If bit 0 is set an error occurred 
        If (data(TRACK_1_DECODE_STATUS) And 1) = 0 AndAlso (data(TRACK_2_DECODE_STATUS) And 1) = 0 AndAlso (data(TRACK_3_DECODE_STATUS) And 1) = 0 Then

            Progress("> Success")
        Else

            Progress("> Bad Card")
            Return False
        End If

        Return True

    End Function

    ' ByteArrayToString
    ' convert a byte array into a human readable string
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Shared Function ByteArrayToString(ByVal data As Byte()) As String

        Dim returnString As String = ""
        Dim singleByte As Byte

        If Not data Is Nothing Then

            For byteIndex = 0 To data.Length - 1
                singleByte = data(byteIndex)

                If (singleByte < 32) Then
                    returnString = returnString & "[" & data(byteIndex).ToString & "]"
                Else
                    returnString = returnString & Chr(singleByte)
                End If
            Next

        End If

        Return returnString

    End Function

#End Region

#Region "GUI thread safe updates"

    ' SetTextBoxText
    ' enter text in to a textbox in a thread safe manner
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetTextBoxText(ByVal textBox As TextBox, ByVal messageText As String)

        If textBox.InvokeRequired Then
            Dim d As New SetTextBoxTextCallback(AddressOf SetTextBoxText)
            Me.Invoke(d, New Object() {textBox, messageText})
        Else
            textBox.Text = messageText
        End If

    End Sub

    ' Progress
    ' enter text in to the textbox in a thread safe manner
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Progress(ByVal messageText As String)

        Dim itemIndex As Integer

        If Me.ProgressList.InvokeRequired Then
            Dim d As New ProgressCallback(AddressOf Progress)            ' invoke this message in the correct thread
            Me.Invoke(d, New Object() {messageText})
        Else
            If ProgressList.Items.Count = 100 Then
                ProgressList.Items.RemoveAt(0)
            End If

            itemIndex = ProgressList.Items.Add(messageText)                ' add the message to the list box and scrol it.
            ProgressList.TopIndex = itemIndex - 7
        End If

    End Sub

    ' StatusBarText
    ' enter text in to the statusbar in a thread safe manner
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StatusBarText(ByVal messageText As String)

        If Me.ProgressList.InvokeRequired Then

            Dim d As New ProgressCallback(AddressOf StatusBarText)            ' invoke this message in the correct thread
            Me.Invoke(d, New Object() {messageText})

        Else
            StatusStrip1.Items(0).Text = messageText

        End If

    End Sub

#End Region

#Region "Application messages"

    ' SetCallback
    ' store the callback address of the application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetCallback(ByVal dataCallbackFunction As EventCallback)
        eventCallbackDelegate = dataCallbackFunction
    End Sub

    ' SendApplicationMessage
    ' notify the application that something has happenned
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendApplicationMessage(ByVal eventCode As Integer, ByVal integerValue As Integer, ByVal stringValue As String)

        If eventCallbackDelegate <> Nothing Then
            eventCallbackDelegate.Invoke(eventCode, integerValue, stringValue)
        End If
    End Sub

#End Region

    ' CheckDatabase
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function CheckDatabase(ByVal connectionString As String) As Boolean

        Dim sqlCheckTable As String = "SELECT isnull(COUNT(*),0) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EportTransaction]') AND type in (N'U')"
        Dim myConnection As SqlConnection
        Dim myCommand As SqlCommand
        Dim sqlCreateString(1) As String
        Dim operationResult As Boolean

        sqlCreateString(0) = "CREATE TABLE [dbo].[EportTransaction](  [EportTransactionID] [int] IDENTITY(1,1) NOT NULL, [TransactionID] [int] NULL,  [TransactionAmount] [int] NULL,  [TransactionDetails] [varchar](100) NULL,  [TransactionResult] [int] NULL,  [ErrorCode] [int] NULL,  [Success] [int] NULL,  [Date] [datetime] NULL) "
        sqlCreateString(1) = "ALTER TABLE [dbo].[EportTransaction] ADD  CONSTRAINT [DF_EportTransaction_Date]  DEFAULT (getdate()) FOR [Date]"



        myConnection = New SqlConnection(connectionString)

        Try
            myConnection.Open()
            'opening the connection
            myCommand = New SqlCommand(sqlCheckTable, myConnection)
            'executing the command and assigning it to connection 
            Dim m As Integer = myCommand.ExecuteScalar()
            If m = 0 Then
                'Create the Table
                For i As Integer = 0 To sqlCreateString.Length - 1
                    myCommand.CommandText = sqlCreateString(i)
                    Dim result As Integer
                    result = myCommand.ExecuteNonQuery

                Next
            End If

            operationResult = True

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            debugInformation.Progress(fDebugWindow.Level.ERR, 1270, "Failed on Testing or Creating ePort Interface " & ex.ToString, True)
            Progress("   Failed checking database for eport")
            operationResult = False

        Finally
            myConnection.Close()
            myCommand = Nothing
            myConnection = Nothing
        End Try

        Return operationResult

    End Function

#Region "Application transaction details"

    ' GetTerminalSerialNo, GetTransactionID, GetTransactionDateTime, and GetLast4Digits
    ' wrappers for eport object nessacary for reciepts.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetTerminalSerialNo() As String
        If terminalSerialNo = "" Then
            terminalSerialNo = GetUnitSerialNo()
        End If
        Return terminalSerialNo
    End Function

  


    Public Function GetTransactionID() As String
        Return GetTransactionField(currentTransactionId, "TransactionId")
    End Function

    Public Function GetTransactionDateTime() As String
        Return GetTransactionField(currentTransactionId, "Date") 'mTransactionDateTime
    End Function

    Public Function GetLast4Digits() As String
        Return Last4Digits
    End Function

#End Region

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

End Class

#Region "Factory class"

' class cEportManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cEportManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared ePortManager As fEportManager = Nothing

    Public Function GetManager() As fEportManager

        If IsNothing(ePortManager) Then
            ePortManager = New fEportManager
        End If

        Return ePortManager
    End Function

End Class

#End Region
