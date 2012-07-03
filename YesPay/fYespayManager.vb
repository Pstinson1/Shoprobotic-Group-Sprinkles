'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Net.Sockets
Imports System.Threading
Imports System.Windows.Forms
Imports System.Diagnostics.Process
Imports System.IO.Path
Imports System.IO
Imports System.Data.SqlClient

Imports DebugWindow
Imports HelperFunctions
Imports SettingsManager

' class fYespayManager
' provide information to the Yespay module and handle the outcome
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fYespayManager

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Message

        YP_AUTHORISATION_OK
        YP_AUTHORISATION_FAIL

    End Enum

    Public Enum State

        IDLE
        AUTH
        CANCEL

    End Enum

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Structure Reciept

        Dim last4Digits As String
        Dim cardDescription As String
        Dim cardHolder As String
        Dim startDate As String
        Dim expiryDate As String
        Dim issueNumber As String
        Dim timeDate As String
        Dim merchantId As String
        Dim merchantName As String
        Dim merchantAddress As String
        Dim terminalId As String
        Dim retentionReminder As String
        Dim customerDeclaration As String
        Dim recieptNumber As String
        Dim authorisationCode As String
        Dim pgtrCode As String
        Dim transactionRef As String

    End Structure

    ' Constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Const UNABLE_TO_LOG_TRANSACTION = -1
    Const NO_UNBATCHED_TRANSACTIONS = -1

    ' Delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Sub CallbackDelegate(ByVal shortCode As Integer, ByVal eventCode As Integer, ByVal messageContent As String)

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    Private eventCallbackDelegate As CallbackDelegate = Nothing
    Private socketConnection As TcpClient = Nothing
    Private socketStream As NetworkStream
    Private socketThread As Thread
    Private continueThread As Boolean = True
    Private responseRecieved As Boolean = False
    Private transactionState As State = State.IDLE
    Private databaseConnectionString As String
    Private batchRetryDue As Date
    Private currentCardNumber As String
    Private currentAuthoriseId As Integer
    Private currentBatchId As Integer
    Private displayedState As Integer = -1
    Private transactionTimeoutCounter As Integer = 0
    Private manageConnectDue As Date
    Private manageExistDue As Date = Now.AddSeconds(0)
    Private socketIsConnected As Boolean = False

#Region "Startup and shutdown"

    ' Initialise & Shutdown
    ' set up and shutdown the Yespay interface
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise(ByVal databaseConnection As String)

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the gui invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle

        ' grab the debug box & the settings manager
        debugInformation = debugInformationFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

        databaseConnectionString = databaseConnection

        batchRetryDue = Now.AddSeconds(20)
        manageConnectDue = Now.AddSeconds(10)

        ClearTransactionDetails()

        ' create the management thread.
        socketThread = New Thread(AddressOf BackgroundThreadProcess)
        socketThread.Priority = ThreadPriority.BelowNormal
        socketThread.Name = "Yespay Socket"
        socketThread.Start()

        'customise the tabs.
        ProgressListBox.UseCustomTabOffsets = True
        ProgressListBox.CustomTabOffsets.Add(8)
        ProgressListBox.CustomTabOffsets.Add(20)

        ' yespay
        ServerAddressTextBox.Text = settingsManager.GetValue("YespayServerAddress").ToString
        ServerPortTextBox.Text = settingsManager.GetValue("YespayServerPort").ToString

        CheckDatabaseExists()

    End Sub

    ' CheckDatabaseExists
    ' ensure that the Yespay table exists
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function CheckDatabaseExists() As Boolean

        Dim createResult As Boolean = False
        Dim createTableCommand = "CREATE TABLE [dbo].[YespayTransaction](" & _
                                                    "[Id] [int] IDENTITY(1,1) NOT NULL," & _
                                                    "[Status] [int] NULL," & _
                                                    "[Pan] [varchar](50) NULL," & _
                                                    "[ExpiryDate] [varchar](50) NULL," & _
                                                    "[Pgtr] [varchar](50) NULL," & _
                                                    "[Amount] [money] NULL," & _
                                                    "[TransactionNumber] [int] NULL," & _
                                                    "[ProductId] [varchar](100) NULL," & _
                                                    "[ProductDescription] [varchar](100) NULL" & _
                                                    ") ON [PRIMARY]"

        If settingsManager.ConnectToDatabase() Then

            createResult = settingsManager.RunDatabaseNonQuery(createTableCommand)
            settingsManager.DisconnectFromDatabase()
        End If

        Return createResult

    End Function

    ' Shutdown
    ' close down the library
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Shutdown()

        ' shut down the management thread.
        continueThread = False
        Thread.Sleep(100)

    End Sub

#End Region

    ' BackgroundThreadProcess
    ' polling loop for the client socket
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub BackgroundThreadProcess()

        Dim incommingDataBuffer As String = ""
        Dim timeNow As Date

        Do While continueThread

            timeNow = Now

            ManageConnectionStatus()
            ManageIncommingData(incommingDataBuffer)
            ParseResponse(incommingDataBuffer)

            If responseRecieved Then
                ProcessTransactionResult()
            End If


            If displayedState <> transactionState Then

                helperFunctions.StatusStripMessage(StatusStrip, 1, transactionState.ToString)
                displayedState = transactionState

            End If

            Select Case transactionState

                Case State.IDLE

                    If timeNow > batchRetryDue Then
                        ReplayCancels()
                    End If

                Case State.AUTH

                Case State.CANCEL

                    If transactionTimeoutCounter = 0 Then

                        batchRetryDue = timeNow.AddSeconds(120)
                        transactionState = State.IDLE
                    Else
                        transactionTimeoutCounter -= 1
                    End If

            End Select

            If continueThread Then
                Thread.Sleep(100)
            End If
        Loop

    End Sub

    Public Sub Manage()

        Dim timeNow As Date = Now
        Dim processList() As Process
        Dim yespayProcess As Process

        If settingsManager.GetValue("YespayEnsureRunning") Then

            If timeNow > manageExistDue Then

                processList = Process.GetProcessesByName(settingsManager.GetValue("YespayProcessName"))

                Try
                    If processList.Length = 0 Then

                        helperFunctions.AddToListBox(ProgressListBox, "!" & ControlChars.Tab & "Yespay not detected")
                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Not connected")

                        yespayProcess = New Process
                        yespayProcess.StartInfo.FileName = settingsManager.GetValue("YespayApplicationPath")
                        yespayProcess.Start()

                        socketIsConnected = False
                        transactionState = State.IDLE

                    End If

                Catch ex As Exception
                End Try
                manageExistDue = timeNow.AddSeconds(20)

            End If

        End If

    End Sub

    Private Sub ReplayCancels()

        Dim databaseConnection As SqlConnection
        Dim queryCommand As SqlCommand = New SqlCommand
        Dim queryResult As SqlDataReader
        Dim transactionToBatch As Integer = NO_UNBATCHED_TRANSACTIONS

        Try
            databaseConnection = New SqlConnection(databaseConnectionString)
            queryCommand.CommandText = "Select Top 1 Id from YespayTransaction where Status=30"
            queryCommand.Connection = databaseConnection

            ' recover the data field
            databaseConnection.Open()

            queryResult = queryCommand.ExecuteReader()
            If queryResult.HasRows Then

                queryResult.Read()
                transactionToBatch = Convert.ToInt32(queryResult("Id"))
            End If

            ' shut down the datadase connection
            databaseConnection.Close()
            databaseConnection = Nothing

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 1286, "Unable to recover unbatched transaction", True)
        End Try

        If transactionToBatch = NO_UNBATCHED_TRANSACTIONS Then
            batchRetryDue = Now.AddSeconds(settingsManager.GetValue("YespayBatchRetryInterval"))
        Else
            Cancel(transactionToBatch)
            batchRetryDue = Now.AddSeconds(120)
        End If

    End Sub

    ' ProcessTransactionResult
    ' when a complete response is recieved, process it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProcessTransactionResult()

        Dim outcome As String

        outcome = Trim(TransactionResultTextBox.Text)

        responseRecieved = False

        Select Case transactionState

            Case State.IDLE

            Case "9", "42"

                SendApplicationMessage(Message.YP_AUTHORISATION_FAIL, 0, 0)

            Case State.AUTH

                debugInformation.Progress(fDebugWindow.Level.INF, 1288, "Yespay authorisation response " & outcome.ToString, True)

                Select Case outcome

                    Case "1", "2"

                        '        SetTransactionField(currentAuthoriseId, "Pan", "'" & currentCardNumber & "'")
                        SetTransactionField(currentAuthoriseId, "Status", "20")

                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Authorised OK")
                        SendApplicationMessage(Message.YP_AUTHORISATION_OK, 0, 0)

                    Case Else

                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Authorisation failed")
                        SendApplicationMessage(Message.YP_AUTHORISATION_FAIL, 0, 0)
                        SetTransactionField(currentAuthoriseId, "Status", "10")

                End Select

                transactionState = State.IDLE

            Case State.CANCEL

                debugInformation.Progress(fDebugWindow.Level.INF, 1289, "Yespay cancel transaction response " & outcome.ToString, True)

                Select Case outcome


                    Case "1"
                        SetTransactionField(currentBatchId, "Status", "99")
                        MaskDatabasePan(currentBatchId)

                    Case "20"

                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Cancelled OK")
                        SetTransactionField(currentBatchId, "Status", "99")
                        MaskDatabasePan(currentBatchId)

                    Case "21"

                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Cancel failed, code = " & outcome)
                        SetTransactionField(currentBatchId, "Status", "70")
                        ' to do, what the hell is 21?

                    Case "9"

                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Cancel failed, code = " & outcome)
                        SetTransactionField(currentBatchId, "Status", "71")

                    Case "42"     ' busy

                    Case Else

                        helperFunctions.StatusStripMessage(StatusStrip, 0, "Cancel failed, code = " & outcome)

                End Select

                transactionState = State.IDLE

        End Select

    End Sub

    ' ManageIncommingData
    ' any data sitting on the socket ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ManageIncommingData(ByRef incommingDataBuffer As String)

        Dim incommingBytes(1024) As Byte
        Dim bytesActuallyRead As Integer

        Try
            If socketStream.DataAvailable Then

                bytesActuallyRead = socketStream.Read(incommingBytes, 0, incommingBytes.Length)
                incommingDataBuffer = incommingDataBuffer & System.Text.Encoding.ASCII.GetString(incommingBytes, 0, bytesActuallyRead)

            End If
        Catch ex As Exception
        End Try

    End Sub

    ' ParseResponse
    ' while there is data in the incomming buffer, extract it.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ParseResponse(ByRef incommingDataBuffer As String)

        Dim recordString As String = ""
        Dim codeString As String = ""
        Dim valueString As String = ""

        While ParseBufferForRecords(incommingDataBuffer, recordString)

            If (ParseRecord(recordString, codeString, valueString)) Then

                FillInTransactionDetails(codeString, valueString)
            End If

        End While

    End Sub

    ' ParseRecord
    ' while there is data in the incomming buffer, extract it.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ParseRecord(ByVal recordString As String, ByRef codeString As String, ByRef valueString As String) As Boolean

        Dim terminationIndex As Integer = recordString.IndexOf("=")

        If terminationIndex <> -1 Then

            codeString = Trim(recordString.Substring(0, terminationIndex))
            valueString = Trim(recordString.Substring(terminationIndex + 1))

            Return True
        Else
            codeString = ""
            valueString = ""

            Return False
        End If

    End Function

    ' ParseBufferForChunk
    ' examine a buffer for terminated chunks
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ParseBufferForRecords(ByRef dataBuffer As String, ByRef recordString As String) As Boolean

        Dim terminationIndex As Integer = dataBuffer.IndexOf(RECORD_SEPARATOR)
        Dim lineFound As Boolean = False

        recordString = ""

        ' look for a terminator at the start of the line first
        If dataBuffer.Length >= 4 Then

            If dataBuffer.Substring(0, 4) = "99=0" Then

                recordString = dataBuffer.Substring(0, 4)
                dataBuffer = dataBuffer.Substring(4)

                responseRecieved = True
                helperFunctions.AddToListBox(ProgressListBox, ControlChars.Tab & recordString)
                lineFound = True

            End If

        End If

        ' if it's not a terminator, loom for a record separator
        If Not lineFound Then

            terminationIndex = dataBuffer.IndexOf(RECORD_SEPARATOR)

            If terminationIndex <> -1 Then

                recordString = dataBuffer.Substring(0, terminationIndex)
                dataBuffer = dataBuffer.Substring(terminationIndex + 1)

                helperFunctions.AddToListBox(ProgressListBox, ControlChars.Tab & recordString)
                lineFound = True

            End If

        End If

        Return lineFound

    End Function

    ' SendTerminal
    ' send to the yespay software
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function SendTerminal(ByVal parameterId As Integer, ByVal parameterValue As String) As Boolean

        Dim codeString As String = ""
        Dim valueString As String = ""
        Dim outgoingDataBuffer As String = parameterId.ToString & "=" & parameterValue & RECORD_SEPARATOR
        Dim outgoingBytes As [Byte]() = System.Text.Encoding.ASCII.GetBytes(outgoingDataBuffer)

        socketStream.Write(outgoingBytes, 0, outgoingBytes.Length)
        helperFunctions.AddToListBox(ProgressListBox, ">" & ControlChars.Tab & parameterId.ToString & "=" & parameterValue)

        responseRecieved = False

    End Function

#Region "Connect/Disconnect to server"

    ' ManageConnectionStatus
    ' remember connection status and post events..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ManageConnectionStatus()

        Dim connectionState As Boolean = False
        Dim timeNow As Date = Now

        If timeNow > manageConnectDue Then

            manageConnectDue = timeNow.AddSeconds(10)

            If Not socketIsConnected Then

                helperFunctions.AddToListBox(ProgressListBox, "!" & ControlChars.Tab & "Trying to connect to yespay")

                Try

                    ' create the socket to connect to the server
                    socketConnection = New TcpClient

                    socketConnection.Connect(settingsManager.GetValue("YespayServerAddress").ToString, settingsManager.GetValue("YespayServerPort"))
                    socketStream = socketConnection.GetStream()
                    helperFunctions.AddToListBox(ProgressListBox, "!" & ControlChars.Tab & "Connected to Yespay")

                    helperFunctions.StatusStripMessage(StatusStrip, 0, "Connected to Yespay")
                    socketIsConnected = True

                Catch ex As Exception
                    helperFunctions.StatusStripMessage(StatusStrip, 0, "Unable to connect to yespay")
                End Try

            End If

        End If


    End Sub

#End Region

#Region "Application messages"

    ' SetCallback
    ' store the callback address of the application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetCallback(ByVal dataCallbackFunction As CallbackDelegate)
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

    ' YesPayFormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub YesPayFormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

    ' ResetTransaction
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ResetTransaction()




    End Sub


    ' Authorise
    ' pre-authorise a sale
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Authorise(ByVal transactionValuePence As Integer, ByVal productName As String, ByVal productId As Integer) As Boolean

        Dim transactionOutcome As Boolean = False
        Dim yespayTransactionNumber As String
        Dim transactionValuePounds As Double = Convert.ToDouble(transactionValuePence) / 100

        If transactionState = State.IDLE And socketIsConnected And transactionValuePence > 0 Then

            ClearTransactionDetails()

            currentAuthoriseId = LogTransaction(transactionValuePounds, productName, productId)

            yespayTransactionNumber = GetTransactionField(currentAuthoriseId, "TransactionNumber")

            helperFunctions.StatusStripMessage(StatusStrip, 0, "Pre-authorising sale")

            SendTerminal(IN_TXN_REF, yespayTransactionNumber)
            SendTerminal(IN_TXN_TYPE, ISO_GOODSnSERVICES)
            SendTerminal(IN_TXN_AMOUNT, transactionValuePounds.ToString)
            SendTerminal(IN_END, "0")

            ' remember what we are doing
            transactionState = State.AUTH
            transactionOutcome = True

        Else

            transactionOutcome = False

        End If

        Return transactionOutcome

    End Function

#Region "Database interface"

    ' LogTransaction
    ' complete a pre-authorised sale
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function LogTransaction(ByVal transactionAmount As Double, ByVal productDescription As String, ByVal productId As Integer) As Integer

        Dim databaseConnection As SqlConnection
        Dim insertionCommand As SqlCommand = New SqlCommand
        Dim getIdentifierCommand As SqlCommand = New SqlCommand
        Dim queryResult As SqlDataReader
        Dim newTransactionId = UNABLE_TO_LOG_TRANSACTION
        Dim yespayTransactionId As Integer
        Dim rootKey As RegistryKey

        ' get and increment the transaction Id
        rootKey = My.Computer.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Teknovation\\Showcase\\YesPayConfig")
        yespayTransactionId = rootKey.GetValue("TransactionNumber", "1")
        rootKey.SetValue("TransactionNumber", yespayTransactionId + 1)
        rootKey.Close()

        debugInformation.Progress(fDebugWindow.Level.INF, 1285, "Logging Yespay trans: " & ProductName, True)

        Try
            databaseConnection = New SqlConnection(databaseConnectionString)

            insertionCommand.Connection = databaseConnection
            insertionCommand.CommandText = _
                        "Insert into YespayTransaction (TransactionNumber, Amount, ProductId, ProductDescription, Status) " & _
                        "Values  (" & yespayTransactionId & "," & transactionAmount & "," & "'" & productId & "','" & productDescription & "', 0)"

            getIdentifierCommand.CommandText = "Select max(Id) as maximumId from YespayTransaction"
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
            debugInformation.Progress(fDebugWindow.Level.ERR, 1525, ex.ToString, True)
        End Try

        Return newTransactionId
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
            updateCommand.CommandText = "Update YespayTransaction Set " & fieldName & "=" & fieldValue & " where Id=" & transactionID
            updateCommand.Connection = databaseConnection

            ' set the data field
            databaseConnection.Open()

            updateCommand.ExecuteNonQuery()

            ' shut down the datadase connection
            databaseConnection.Close()
            databaseConnection = Nothing

            updateResult = True

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 1284, "Unable to execute update on YespayTransaction" & updateCommand.ToString, True)
        End Try

        Return updateResult

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
            queryCommand.CommandText = "Select " & fieldName & " from YespayTransaction where Id=" & transactionID
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
            debugInformation.Progress(fDebugWindow.Level.ERR, 1283, "Unable to recover field from YespayTransaction" & queryCommand.ToString, True)
        End Try

        Return fieldResult

    End Function

#End Region


    Public Sub Cancel(ByVal transactionId As Integer)

        Dim transactionNumber As String = GetTransactionField(transactionId, "TransactionNumber")
        Dim transactionValue As String = GetTransactionField(transactionId, "Amount")
        Dim cardNumber As String = GetTransactionField(transactionId, "Pan")
        Dim expiryDate As String = GetTransactionField(transactionId, "ExpiryDate")
        Dim pgtr As String = GetTransactionField(transactionId, "Pgtr")

        currentBatchId = transactionId
        debugInformation.Progress(fDebugWindow.Level.INF, 1287, "Cancelling yespay transaction " & transactionNumber.ToString & " PGTR=" & pgtr, True)

        If socketConnection.Connected And transactionValue > 0 Then

            helperFunctions.StatusStripMessage(StatusStrip, 0, "Cancelling transaction")

            SendTerminal(IN_TXN_TYPE, ISO_CANCEL)
            SendTerminal(IN_TXN_REF, transactionNumber)
            SendTerminal(IN_PAN, cardNumber)
            SendTerminal(IN_CARD_EXP_DATE, expiryDate)
            SendTerminal(IN_PGTR, pgtr)
            SendTerminal(IN_END, "0")

            ' remember what we are doing
            transactionState = State.CANCEL

            ' if we don't get a result back from the batch in 30 seconds, return to IDLE
            transactionTimeoutCounter = 300
        Else

        End If

    End Sub



    ' MaskDatabasePan
    ' mask all but the last 4 number of a Pan on a given transaction
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub MaskDatabasePan(ByVal transactionId As Integer)

        Dim unmaskedNumber As String = GetTransactionField(transactionId, "Pan")
        Dim maskedNumber As String = MaskCardNumber(unmaskedNumber)

        SetTransactionField(transactionId, "Pan", "'" & maskedNumber & "'")

    End Sub

    ' MaskDatabasePan
    ' mask all but the last 4 number of a Pan on a given transaction
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function MaskCardNumber(ByVal unmaskedNumber As String) As String

        Dim maskedNumber As String
        Dim numberLength As Integer = unmaskedNumber.Length
        Dim asterixCount As Integer

        asterixCount = numberLength - IIf(numberLength > 8, 4, numberLength / 2)
        maskedNumber = New String("*", asterixCount) & unmaskedNumber.Substring(asterixCount)

        Return maskedNumber

    End Function

    ' StartBatch
    ' complete a pre-authorised sale
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StartBatch(ByVal vendSuccessful As Boolean)

        If vendSuccessful Then

            SetTransactionField(currentAuthoriseId, "Status", "40")

        Else

            SetTransactionField(currentAuthoriseId, "Pan", "'" & currentCardNumber & "'")
            SetTransactionField(currentAuthoriseId, "Status", "30")
            Cancel(currentAuthoriseId)

        End If

    End Sub

    ' FillInTransactionDetails
    ' fill in a record item from a given response
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub GetReciept(ByRef salesReciept As Reciept)

        Try
            salesReciept.transactionRef = currentAuthoriseId
            salesReciept.last4Digits = CardNumberTextBox.Text
            salesReciept.cardDescription = CardDescriptionTextBox.Text
            salesReciept.cardHolder = CardHolderTextBox.Text
            salesReciept.startDate = StartDateTextBox.Text
            salesReciept.expiryDate = ExpiryDateTextBox.Text
            salesReciept.issueNumber = IssueNumberTextBox.Text
            salesReciept.merchantId = MerchantIdTextBox.Text
            salesReciept.merchantName = MerchantNameTextBox.Text
            salesReciept.merchantAddress = MerchantAddressTextBox.Text
            salesReciept.terminalId = TerminalIdTextBox.Text
            salesReciept.retentionReminder = RetentionReminderTextBox.Text
            salesReciept.customerDeclaration = CustomerDeclarationTextBox.Text
            salesReciept.recieptNumber = RecieptNumberTextBox.Text
            salesReciept.authorisationCode = AuthorisationTextBox.Text
            salesReciept.pgtrCode = PgtrTextBox.Text

            ' format date & time

            If TimeTextBox.Text.Length >= 6 And DateTextBox.Text.Length >= 8 Then

                salesReciept.timeDate = TimeTextBox.Text.Substring(0, 2) & ":" & _
                TimeTextBox.Text.Substring(2, 2) & ":" & _
                TimeTextBox.Text.Substring(4, 2) & " " & _
                DateTextBox.Text.Substring(0, 2) & "/" & _
                DateTextBox.Text.Substring(2, 2) & "/" & _
                DateTextBox.Text.Substring(4, 4)

            Else

                salesReciept.timeDate = TimeTextBox.Text & " - " & DateTextBox.Text

            End If

        Catch ex As Exception
        End Try

    End Sub

    ' FillInTransactionDetails
    ' fill in a record item from a given response
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub FillInTransactionDetails(ByVal attributeCode As String, ByVal attributeValue As String)

        Select Case (attributeCode)

            Case OUT_APPL_PAN
                helperFunctions.SetTextBoxText(CardNumberTextBox, MaskCardNumber(attributeValue))
                currentCardNumber = attributeValue

            Case OUT_CARD_EXPIRY_DATE
                helperFunctions.SetTextBoxText(ExpiryDateTextBox, attributeValue)
                SetTransactionField(currentAuthoriseId, "ExpiryDate", "'" & attributeValue & "'")

            Case OUT_PGTR
                helperFunctions.SetTextBoxText(PgtrTextBox, attributeValue)
                SetTransactionField(currentAuthoriseId, "Pgtr", "'" & attributeValue & "'")

            Case OUT_APPL_LABEL
                helperFunctions.SetTextBoxText(CardDescriptionTextBox, attributeValue)
            Case OUT_CARD_HOLDER_NAME
                helperFunctions.SetTextBoxText(CardHolderTextBox, attributeValue)
            Case OUT_CARD_HOLDER_NAME_EXT
                helperFunctions.SetTextBoxText(CardHolderExtendedTextBox, attributeValue)
            Case OUT_START_DATE
                helperFunctions.SetTextBoxText(StartDateTextBox, attributeValue)
            Case OUT_APPL_PAN_SEQ_NUMBER
                helperFunctions.SetTextBoxText(IssueNumberTextBox, attributeValue)
            Case OUT_TXN_TIME
                helperFunctions.SetTextBoxText(TimeTextBox, attributeValue)
            Case OUT_TXN_DATE
                helperFunctions.SetTextBoxText(DateTextBox, attributeValue)
            Case OUT_MERCHANT_NAME
                helperFunctions.SetTextBoxText(MerchantNameTextBox, attributeValue)
            Case OUT_MID
                helperFunctions.SetTextBoxText(MerchantIdTextBox, attributeValue)
            Case OUT_TID
                helperFunctions.SetTextBoxText(TerminalIdTextBox, attributeValue)
            Case OUT_MERCHANT_ADDRESS
                helperFunctions.SetTextBoxText(MerchantAddressTextBox, attributeValue)
            Case OUT_RETAINTION_REMINDER
                helperFunctions.SetTextBoxText(RetentionReminderTextBox, attributeValue)
            Case OUT_DECLARATION
                helperFunctions.SetTextBoxText(CustomerDeclarationTextBox, attributeValue)
            Case OUT_TXN_RESULT
                helperFunctions.SetTextBoxText(TransactionResultTextBox, attributeValue)
            Case OUT_RECEIPT_NUMBER
                helperFunctions.SetTextBoxText(RecieptNumberTextBox, attributeValue)
            Case OUT_AUTH_CODE
                helperFunctions.SetTextBoxText(AuthorisationTextBox, attributeValue)

        End Select

    End Sub

    ' ClaerTransactionDetails
    ' clear all the transaction data from the panel
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ClearTransactionDetails()

        helperFunctions.SetTextBoxText(CardNumberTextBox, "")
        helperFunctions.SetTextBoxText(CardDescriptionTextBox, "")
        helperFunctions.SetTextBoxText(CardHolderTextBox, "")
        helperFunctions.SetTextBoxText(CardHolderExtendedTextBox, "")
        helperFunctions.SetTextBoxText(StartDateTextBox, "")
        helperFunctions.SetTextBoxText(ExpiryDateTextBox, "")
        helperFunctions.SetTextBoxText(IssueNumberTextBox, "")
        helperFunctions.SetTextBoxText(TimeTextBox, "")
        helperFunctions.SetTextBoxText(DateTextBox, "")
        helperFunctions.SetTextBoxText(MerchantNameTextBox, "")
        helperFunctions.SetTextBoxText(MerchantIdTextBox, "")
        helperFunctions.SetTextBoxText(TerminalIdTextBox, "")
        helperFunctions.SetTextBoxText(MerchantAddressTextBox, "")
        helperFunctions.SetTextBoxText(RetentionReminderTextBox, "")
        helperFunctions.SetTextBoxText(CustomerDeclarationTextBox, "")
        helperFunctions.SetTextBoxText(PgtrTextBox, "")
        helperFunctions.SetTextBoxText(TransactionResultTextBox, "")
        helperFunctions.SetTextBoxText(RecieptNumberTextBox, "")
        helperFunctions.SetTextBoxText(AuthorisationTextBox, "")
        helperFunctions.SetTextBoxText(KeyTextBox, "")

    End Sub

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

End Class

#Region "Class factory"

' class cYespayManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cYespayManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared yespayManager As fYespayManager = Nothing

    Public Function GetManager() As fYespayManager

        If IsNothing(yespayManager) Then

            yespayManager = New fYespayManager

        End If

        Return yespayManager

    End Function

End Class

#End Region

