'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Option Strict Off
Option Explicit On

Module mVTermInterface

    Public Const IN_TXN_REF As Short = 1
    Public Const IN_TXN_TYPE As Short = 2
    Public Const IN_TXN_AMOUNT As Short = 3
    Public Const IN_TXN_CB_AMOUNT As Short = 4
    Public Const IN_PAN As Short = 5
    Public Const IN_CARD_EXP_DATE As Short = 6
    Public Const IN_CVV As Short = 7
    Public Const IN_ADDRESS As Short = 8
    Public Const IN_ZIP As Short = 9
    Public Const IN_START_DATE As Short = 10
    Public Const IN_ISSUE_NUMBER As Short = 11
    Public Const IN_CNP_INDICATOR As Short = 12
    Public Const IN_PGTR As Short = 13

    Public Const IN_VEHICLE_REG_NO As Short = 21
    Public Const IN_MILEAGE As Short = 22
    Public Const IN_COMPANY_ID As Short = 23
    Public Const IN_FUEL_TRANS As Short = 24
    Public Const IN_REMOVE_FLAG As Short = 25

    Public Const IN_END As Short = 99

    Public Const OUT_CHIP_STRIPE_KEYED As Short = 1
    Public Const OUT_TXN_TYPE As Short = 2
    Public Const OUT_TXN_RESULT As Short = 3
    Public Const OUT_AUTH_CODE As Short = 4
    Public Const OUT_APPL_PAN As Short = 5
    Public Const OUT_APPL_LABEL As Short = 6
    Public Const OUT_APPL_EFFECTIVE_DATE As Short = 7
    Public Const OUT_TXN_DATE As Short = 8
    Public Const OUT_TXN_TIME As Short = 9
    Public Const OUT_CARD_HOLDER_NAME As Short = 10
    Public Const OUT_CARD_HOLDER_NAME_EXT As Short = 11
    Public Const OUT_MID As Short = 12
    Public Const OUT_TID As Short = 13
    Public Const OUT_CARD_VERIFICATION_METHOD As Short = 14
    Public Const OUT_START_DATE As Short = 15
    Public Const OUT_TOTAL_DEBIT_NO_TRANS As Short = 16
    Public Const OUT_TOTAL_CREDIT_NO_TRANS As Short = 17
    Public Const OUT_TOTAL_DEBITS As Short = 18
    Public Const OUT_TOTAL_CREDITS As Short = 19
    Public Const OUT_RECON_STATUS As Short = 20
    Public Const OUT_TXN_SEQ_NO As Short = 21
    Public Const OUT_MERCHANT_ADDRESS As Short = 22
    Public Const OUT_MERCHANT_NAME As Short = 23
    Public Const OUT_SERVER_DATE_TIME As Short = 24
    Public Const OUT_BATCH_NUMBER As Short = 25
    Public Const OUT_REFERRAL_TELEPHONE1 As Short = 26
    Public Const OUT_REFERRAL_TELEPHONE2 As Short = 27
    Public Const OUT_PGTR As Short = 28
    Public Const OUT_APPL_ID As Short = 29
    Public Const OUT_APPL_PAN_SEQ_NUMBER As Short = 30
    Public Const OUT_TSI As Short = 31
    Public Const OUT_TVR As Short = 32
    Public Const OUT_RETAINTION_REMINDER As Short = 33
    Public Const OUT_DECLARATION As Short = 34
    Public Const OUT_ADDITIONAL_RESPONSE_DATA As Short = 35
    Public Const OUT_RECEIPT_NUMBER As Short = 36
    Public Const OUT_CARD_EXPIRY_DATE As Short = 37

    Public Const OUT_END As Short = 99

    Public Const OUT_REQ_CASH_BACK As Short = 70

    Public Const RECORD_SEPARATOR As String = ControlChars.Lf

    'Public Const OUT_TXN_TYPE = 2
    Public Const OUT_CARD_TYPE As Short = 41
    Public Const OUT_PRODS_ALLOWED As Short = 42
    Public Const OUT_VEH_REG_NO_REQD As Short = 43
    Public Const OUT_MILEAGE_REQD As Short = 44
    Public Const OUT_COMPANY_ID_REQD As Short = 45
    Public Const OUT_FUEL_CARD_TYPE As Short = 47

    Public Const INPUT_OK As Short = 0
    Public Const INPUT_INCOMPLETE As Short = 1

    Public Const ISO_GOODSnSERVICES As Short = 0
    Public Const ISO_REFUND As Short = 20
    Public Const ISO_CASHBACK As Short = 9
    Public Const ISO_CHECKCARD As Short = 30
    Public Const ISO_PREAUTH As Short = 1
    Public Const ISO_COMPLETION As Short = 2
    Public Const ISO_CANCEL As Short = 3

End Module
