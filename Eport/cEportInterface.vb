'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.Compatibility
Imports System.Text

Public Class cEportInterface

    Private Declare Function EportNW_Init_V3 Lib "StitchAuth.dll" Alias "_EportNW_Init_V3@12" _
    (ByVal CommMethod As Integer, ByVal CommInfo As String, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_Init_V3_1 Lib "StitchAuth.dll" Alias "_EportNW_Init_V3_1@28" _
   (ByVal CommMethod As Integer, ByVal CommInfo As String, ByVal WorkDirectory As String, _
    ByVal TerminalInfoLength As Integer, ByVal TerminalInfo As Object, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_SessionStart Lib "StitchAuth.dll" Alias "_EportNW_SessionStart@4" (ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_SessionClose Lib "StitchAuth.dll" Alias "_EportNW_SessionClose@4" (ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_Auth_V3 Lib "StitchAuth.dll" Alias "_EportNW_Auth_V3@20" (ByVal CardData As String, ByVal Amount As Integer, _
                                                                                               ByVal OrderNumber As Integer, ByVal ErrMsg As String, _
                                                                                               ByVal CardType As Byte) As Integer




    Private Declare Function EportNW_Batch_V3 Lib "StitchAuth.dll" Alias "_EportNW_Batch_V3@24" (ByVal OrderNumber As Integer, ByVal Amount As Integer, ByVal TranResult As Byte, ByVal TranDetail As String, ByVal TranDetailLength As Integer, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_ForceBatch_V3 Lib "StitchAuth.dll" Alias "_EportNW_ForceBatch_V3@36" (ByVal CardData As String, ByVal Amount As Integer, ByVal tval As Integer, ByVal OrderNumber As Integer, ByVal TranResult As Byte, ByVal TranDetail As String, ByVal TranDetailLength As Integer, ByVal ErrMsg As String, ByVal CardType As Byte) As Integer

    Private Declare Function EportNW_LogToServer Lib "StitchAuth.dll" Alias "_EportNW_LogToServer@16" (ByVal MsgType As Byte, ByVal Message As String, ByVal MessageLength As Integer, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_Shutdown Lib "StitchAuth.dll" Alias "_EportNW_Shutdown@4" (ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_GetVersion Lib "StitchAuth.dll" Alias "_EportNW_GetVersion@0" () As Integer

    Private Declare Function EportNW_AESEncrypt Lib "StitchAuth.dll" Alias "_EportNW_AESEncrypt@16" (ByVal StringToEncrypt As String, ByVal EncryptedString As String, ByVal EncryptedStringLength As Integer, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_AESDecrypt Lib "StitchAuth.dll" Alias "_EportNW_AESDecrypt@16" (ByVal StringToDecrypt As String, ByVal DecryptedString As String, ByVal EncryptedStringLength As Integer, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_AESEncryptWithKey Lib "StitchAuth.dll" Alias "_EportNW_AESEncryptWithKey@36" (ByVal StringToEncrypt As String, ByVal EncryptedString As String, ByVal EncryptedStringLength As Integer, ByVal EncryptionKey As String, ByVal ErrMsg As String, ByVal EncryptionKeyLength As Integer, ByVal BlockSize As Integer, ByVal EncryptionMode As Integer, ByVal InitialChainBlock As String) As Integer

    Private Declare Function EportNW_AESDecryptWithKey Lib "StitchAuth.dll" Alias "_EportNW_AESDecryptWithKey@36" (ByVal StringToDecrypt As String, ByVal DecryptedString As String, ByVal EncryptedStringLength As Integer, ByVal EncryptionKey As String, ByVal ErrMsg As String, ByVal EncryptionKeyLength As Integer, ByVal BlockSize As Integer, ByVal EncryptionMode As Integer, ByVal InitialChainBlock As String) As Integer

    Private Declare Function EportNW_CalculateMD5Hash Lib "StitchAuth.dll" Alias "_EportNW_CalculateMD5Hash@12" (ByVal PlainText As String, ByVal MD5 As String, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_ProcessUpdates Lib "StitchAuth.dll" Alias "_EportNW_ProcessUpdates@16" (ByRef NumFilesReceived As Integer, ByVal FileNames As String, ByVal FileTypes As String, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_UploadFile Lib "StitchAuth.dll" Alias "_EportNW_UploadFile@12" (ByVal FileName As String, ByVal FileType As Byte, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_UploadConfig Lib "StitchAuth.dll" Alias "_EportNW_UploadConfig@4" (ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_SessionKeepAlive Lib "StitchAuth.dll" Alias "_EportNW_SessionKeepAlive@4" (ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_CalculateCRC Lib "StitchAuth.dll" Alias "_EportNW_CalculateCRC@16" (ByVal Data As String, ByVal DataLength As Integer, ByVal CRC As String, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_GetConfigValue Lib "StitchAuth.dll" Alias "_EportNW_GetConfigValue@12" (ByVal ParamKey As String, ByVal ParamValue As String, ByVal ErrMsg As String) As Integer

    Private Declare Function EportNW_QueryModem Lib "StitchAuth.dll" Alias "_EportNW_QueryModem@16" (ByVal ModemCommand As String, ByVal ResponseLines As Integer, ByVal ModemResponse As String, ByVal ErrMsg As String) As Integer

    Private Const AES_BLOCK_SIZE As Short = 16

    '/**** API Function Return Codes ****/
    Public Enum ServerResponse
        RES_OK = 0 ' Response from server is good
        RES_BUSY = 1 ' Unable to communicate with server
        RES_AUTH = 2 ' Authorization success, Response from server is good
        RES_DECL = 3 ' Authorization declined, Response from server is good
        RES_ERROR = 4 ' An error occurred during the shutdown process
        RES_INVALID_TYPE = 5 ' The MsgType value is not correct (see EportNW_LogToServer() declaration)
        NO_SESSION = 6 ' A session with the server has not been established. Use EportNW_SessionStart() to start the session.
        RES_FAIL = 7 ' Authorization failed
        RES_OK_NO_UPDATE = 8 ' Function succeeded, no update available
        RES_PARTIAL_OK = 9 ' Function partially succeeded, i.e. only some of the available updates were downloaded
        NOT_INITED = 10 ' Function cannot be run because DLL has not been inited
        RES_BATCH_PASS = 11 ' Batch succeeded
        RES_BATCH_FAIL = 12 ' Batch failed
        RES_BATCH_ERROR = 13 ' Batch failed, error occurred
        RES_UNKNOWN = 1000 ' An unknown error occurred
    End Enum

    '/**** Communication Methods ****/
    Public Enum CommMethod
        CDMA_MODEM = 30 ' CDMA Modem
        POTS_MODEM = 31 ' Modem connected to POTS (wire modem)
        TCPIP_LAN = 32 ' TCP/IP connection
        CDPD_MODEM = 33 ' CDPD Modem
        GPRS_MODEM = 34 ' GPRS Modem
    End Enum

    '/**** File Types ****/
    Public Enum FileType
        CONFIG_FILE = 1 ' Configuration file
        APPLICATION_UPGRADE = 5 ' Application software upgrade
        EXECUTABLE_FILE = 7 ' Executable file
        LOG_FILE = 9 ' Log file
        GENERIC_FILE = 16 ' Generic file
    End Enum

    '/**** Transaction Results ****/
    Public Enum TranResult
        TRAN_RESULT_FAILED = 70 'F - Vend failed
        TRAN_RESULT_CANCELLED = 67 'C - Vend cancelled
        TRAN_RESULT_TIMED_OUT = 84 'T - Vend timed out
        TRAN_RESULT_SUCCESS = 83 'S - Vend was successful and receipt printed
        TRAN_RESULT_SUCCESS_RECEIPT_PROBLEM = 82 'R - Vend was successful but there was a receipt printing problem
        TRAN_RESULT_SUCCESS_NO_RECEIPT_REQUESTED = 78 'N - Vend was successful and no receipt was requested
        TRAN_RESULT_INCOMPLETE = 73 'I - Vend was incomplete (all transaction data not available)
        TRAN_RESULT_SUCCESS_NO_USE_PRINTER = 81 'Q - Vend was successful but Use Printer was set to FALSE
        TRAN_RESULT_UNABLE_TO_AUTH = 85 'U - Unable to authorize
    End Enum

    '/**** Card Types ****/
    Public Enum CardType
        CARD_TYPE_CREDIT = 67 ' C - Credit Card - Mag stripe
        CARD_TYPE_SPECIAL = 83 ' S - Special Card - Mag stripe
        CARD_TYPE_CREDIT_RFID = 82 ' R - Credit Card - RFID
        CARD_TYPE_SPECIAL_RFID = 80 ' P - Special Card - RFID
    End Enum

    '/**** Encryption Modes ****/
    Public Enum EncryptionMode
        ECB = 0
        CBC = 1
        CFB = 2
    End Enum

    Public Enum CardReaderTypes
        CARD_READER_MAGTEK_MAGNESAFE = 1

    End Enum
    Public Function InitV3(ByVal CommMethod As Integer, ByRef CommInfo As String, ByRef ErrMsg As String) As Integer
        InitV3 = EportNW_Init_V3(CommMethod, CommInfo, ErrMsg)
        'InitV3 = EportNW_Init_V3_1(CommMethod, CommInfo, My.Application.Info.DirectoryPath, 0, Nothing, ErrMsg)
    End Function

    Public Function SessionStart(ByRef ErrMsg As String) As Integer
        SessionStart = EportNW_SessionStart(ErrMsg)
    End Function

    Public Function SessionClose(ByRef ErrMsg As String) As Integer
        SessionClose = EportNW_SessionClose(ErrMsg)
    End Function

    Public Function AuthV3(ByRef CardData As String, ByVal Amount As Integer, ByVal OrderNumber As Integer, ByRef ErrMsg As String, Optional ByVal CardType As Byte = 0) As Integer
        AuthV3 = EportNW_Auth_V3(CardData, Amount, OrderNumber, ErrMsg, CardType)
    End Function

    Public Function AuthV3_1(ByVal OrderNumber As UInteger, _
                                          ByVal Amount As UInteger, ByRef ApprovedAmount As UInteger, ByVal CardType As Byte, _
                                          ByVal CardReaderType As Byte, ByVal DecryptedCardDataLength As Byte, ByVal EncryptedCardDataLength As Byte, _
                                          ByVal EncryptedCardData() As Byte, ByVal KeyIDLength As Byte, _
                                          ByVal KeyID() As Byte, ByVal MaskedCardDataLength As Byte, _
                                          ByVal MaskedCardData As String, ByVal HashedCardDataLength As Byte, _
                                           ByVal HashedCardData As String, ByRef ErrMsg As String) As Integer

        AuthV3_1 = EportNW_Auth_V3_1(OrderNumber, Amount, ApprovedAmount, _
   CardType, CardReaderType, DecryptedCardDataLength, EncryptedCardDataLength, _
   EncryptedCardData, KeyIDLength, KeyID, MaskedCardDataLength, _
   MaskedCardData, HashedCardDataLength, _
   HashedCardData, ErrMsg)
    End Function

    Private Declare Function EportNW_Auth_V3_1 Lib "StitchAuth.dll" Alias "_EportNW_Auth_V3_1@60" (ByVal OrderNumber As Integer, _
                                          ByVal Amount As Integer, ByRef ApprovedAmount As Integer, ByVal CardType As Byte, _
                                          ByVal CardReaderType As Byte, ByVal DecryptedCardDataLength As Byte, ByVal EncryptedCardDataLength As Byte, _
                                          ByVal EncryptedCardData() As Byte, ByVal KeyIDLength As Byte, _
                                          ByVal KeyID() As Byte, ByVal MaskedCardDataLength As Byte, _
                                          ByVal MaskedCardData As String, ByVal HashedCardDataLength As Byte, _
                                          ByVal HashedCardData As String, ByVal ErrMsg As String) As Integer



    Public Function BatchV3(ByVal OrderNumber As Integer, ByVal Amount As Integer, ByVal TranResult As Byte, ByVal TranDetail As String, ByVal TranDetailLength As Integer, ByRef ErrMsg As String) As Integer
        BatchV3 = EportNW_Batch_V3(OrderNumber, Amount, TranResult, TranDetail, TranDetailLength, ErrMsg)
    End Function

    Public Function ForceBatchV3(ByRef CardData As String, ByVal Amount As Integer, ByVal tval As Integer, ByVal OrderNumber As Integer, ByVal TranResult As Byte, ByVal TranDetail As String, ByVal TranDetailLength As Integer, ByRef ErrMsg As String, Optional ByVal CardType As Byte = 0) As Integer
        ForceBatchV3 = EportNW_ForceBatch_V3(CardData, Amount, tval, OrderNumber, TranResult, TranDetail, TranDetailLength, ErrMsg, CardType)
    End Function

    Public Function LogToServer(ByVal MsgType As Byte, ByRef Message As String, ByVal MessageLength As Integer, ByRef ErrMsg As String) As Integer
        LogToServer = EportNW_LogToServer(MsgType, Message, MessageLength, ErrMsg)
    End Function

    Public Function Shutdown(ByRef ErrMsg As String) As Integer
        Shutdown = EportNW_Shutdown(ErrMsg)
    End Function

    Public Function GetVersion() As Integer
        GetVersion = EportNW_GetVersion()
    End Function

    Private Function AESEncryptString(ByVal StringToEncrypt As String, ByRef EncryptedString As String, ByRef ErrMsg As String, ByVal blnEncryptWithKey As Boolean, Optional ByVal EncryptionKey As String = "", Optional ByVal EncryptionKeyLength As Integer = 16, Optional ByVal BlockSize As Integer = 16, Optional ByVal EncryptionMode As Integer = EncryptionMode.ECB, Optional ByVal InitialChainBlock As String = vbNullString) As Integer
        Dim strStringToEncrypt As String
        Dim lngEncryptedStringLength, lngStringToEncryptLength As Integer

        lngStringToEncryptLength = Len(StringToEncrypt)

        If lngStringToEncryptLength Mod AES_BLOCK_SIZE = 0 Then
            lngEncryptedStringLength = lngStringToEncryptLength
            strStringToEncrypt = StringToEncrypt
        Else
            lngEncryptedStringLength = lngStringToEncryptLength + AES_BLOCK_SIZE - (lngStringToEncryptLength Mod AES_BLOCK_SIZE)
            strStringToEncrypt = StringToEncrypt & New String(vbNullChar, lngEncryptedStringLength - lngStringToEncryptLength)
        End If

        EncryptedString = New String(vbNullChar, lngEncryptedStringLength)

        If blnEncryptWithKey = True Then
            AESEncryptString = EportNW_AESEncryptWithKey(strStringToEncrypt, EncryptedString, lngEncryptedStringLength, EncryptionKey, ErrMsg, EncryptionKeyLength, BlockSize, EncryptionMode, InitialChainBlock)
        Else
            AESEncryptString = EportNW_AESEncrypt(strStringToEncrypt, EncryptedString, lngEncryptedStringLength, ErrMsg)
        End If
    End Function

    Public Function AESEncrypt(ByVal StringToEncrypt As String, ByRef EncryptedString As String, ByRef ErrMsg As String) As Integer
        AESEncrypt = AESEncryptString(StringToEncrypt, EncryptedString, ErrMsg, False)
    End Function

    Public Function AESDecrypt(ByVal StringToDecrypt As String, ByRef DecryptedString As String, ByRef ErrMsg As String) As Integer
        Dim lngEncryptedStringLength As Integer : lngEncryptedStringLength = Len(StringToDecrypt)
        DecryptedString = New String(vbNullChar, lngEncryptedStringLength)
        AESDecrypt = EportNW_AESDecrypt(StringToDecrypt, DecryptedString, lngEncryptedStringLength, ErrMsg)
    End Function

    Public Function AESEncryptWithKey(ByVal StringToEncrypt As String, ByRef EncryptedString As String, ByVal EncryptionKey As String, ByRef ErrMsg As String, ByVal EncryptionKeyLength As Integer, ByVal BlockSize As Integer, ByVal EncryptionMode As Integer, ByVal InitialChainBlock As String) As Integer
        AESEncryptWithKey = AESEncryptString(StringToEncrypt, EncryptedString, ErrMsg, True, EncryptionKey, EncryptionKeyLength, BlockSize, EncryptionMode, InitialChainBlock)
    End Function

    Public Function AESDecryptWithKey(ByVal StringToDecrypt As String, ByRef DecryptedString As String, ByVal EncryptionKey As String, ByRef ErrMsg As String, ByVal EncryptionKeyLength As Integer, ByVal BlockSize As Integer, ByVal EncryptionMode As Integer, ByVal InitialChainBlock As String) As Integer
        Dim lngEncryptedStringLength As Integer : lngEncryptedStringLength = Len(StringToDecrypt)
        DecryptedString = New String(vbNullChar, lngEncryptedStringLength)
        AESDecryptWithKey = EportNW_AESDecryptWithKey(StringToDecrypt, DecryptedString, lngEncryptedStringLength, EncryptionKey, ErrMsg, EncryptionKeyLength, BlockSize, EncryptionMode, InitialChainBlock)
    End Function

    Public Function CalculateMD5Hash(ByVal PlainText As String, ByRef MD5 As String, ByRef ErrMsg As String) As Integer
        CalculateMD5Hash = EportNW_CalculateMD5Hash(PlainText, MD5, ErrMsg)
    End Function

    Public Function ProcessUpdates(ByRef NumFilesReceived As Integer, ByRef FileNames As String, ByRef FileTypes As String, ByRef ErrMsg As String) As Integer

        FileNames = New String(vbNullChar, 10240)
        FileTypes = New String(vbNullChar, 200)

        ProcessUpdates = EportNW_ProcessUpdates(NumFilesReceived, FileNames, FileTypes, ErrMsg)

        FileNames = GetVBString(FileNames)
        FileTypes = GetVBString(FileTypes)
    End Function

    Public Function UploadFile(ByVal FileName As String, ByVal FileType As Byte, ByRef ErrMsg As String) As Integer
        UploadFile = EportNW_UploadFile(FileName, FileType, ErrMsg)
    End Function

    Public Function UploadConfig(ByRef ErrMsg As String) As Integer
        UploadConfig = EportNW_UploadConfig(ErrMsg)
    End Function

    Public Function SessionKeepAlive(ByRef ErrMsg As String) As Integer
        SessionKeepAlive = EportNW_SessionKeepAlive(ErrMsg)
    End Function

    Public Function CalculateCRC(ByVal Data As String, ByVal DataLength As Integer, ByRef CRC As String, ByRef ErrMsg As String) As Integer
        CalculateCRC = EportNW_CalculateCRC(Data, DataLength, CRC, ErrMsg)
    End Function

    Public Function GetConfigValue(ByVal ParamKey As String, ByRef ParamValue As String, ByRef ErrMsg As String) As Integer
        Dim strParamValue As New FixedLengthString(200)
        '       Dim strParamValue As String*200

        GetConfigValue = EportNW_GetConfigValue(ParamKey, strParamValue.Value, ErrMsg)
        ParamValue = GetVBString(strParamValue.Value)
    End Function

    Public Function QueryModem(ByVal ModemCommand As String, ByVal ResponseLines As Integer, ByRef ModemResponse As String, ByRef ErrMsg As String) As Integer
        Dim strModemResponse As New VB6.FixedLengthString(1024)
        QueryModem = EportNW_QueryModem(ModemCommand, ResponseLines, strModemResponse.Value, ErrMsg)
        ModemResponse = GetVBString(strModemResponse.Value)
    End Function

    Public Function GetServerResponse(ByVal lngResponse As Integer) As String
        Dim strResponse As String
        Select Case lngResponse
            Case ServerResponse.RES_OK : strResponse = "RES_OK"
            Case ServerResponse.RES_BUSY : strResponse = "RES_BUSY"
            Case ServerResponse.RES_AUTH : strResponse = "RES_AUTH"
            Case ServerResponse.RES_DECL : strResponse = "RES_DECL"
            Case ServerResponse.RES_ERROR : strResponse = "RES_ERROR"
            Case ServerResponse.RES_INVALID_TYPE : strResponse = "RES_INVALID_TYPE"
            Case ServerResponse.NO_SESSION : strResponse = "NO_SESSION"
            Case ServerResponse.RES_FAIL : strResponse = "RES_FAIL"
            Case ServerResponse.RES_OK_NO_UPDATE : strResponse = "RES_OK_NO_UPDATE"
            Case ServerResponse.RES_PARTIAL_OK : strResponse = "RES_PARTIAL_OK"
            Case ServerResponse.NOT_INITED : strResponse = "NOT_INITED"
            Case ServerResponse.RES_BATCH_PASS : strResponse = "RES_BATCH_PASS"
            Case ServerResponse.RES_BATCH_FAIL : strResponse = "RES_BATCH_FAIL"
            Case ServerResponse.RES_BATCH_ERROR : strResponse = "RES_BATCH_ERROR"
            Case ServerResponse.RES_UNKNOWN : strResponse = "RES_UNKNOWN"
            Case Else : strResponse = "UNDEFINED"
        End Select
        GetServerResponse = strResponse
    End Function
    Public Function GetVBString(ByVal CString As String) As String
        Dim intNullPos As Short
        intNullPos = InStr(CString, vbNullChar)

        If intNullPos = 0 Then
            GetVBString = Trim(CString)
        Else
            GetVBString = Trim(Left(CString, intNullPos - 1))
        End If
    End Function
End Class