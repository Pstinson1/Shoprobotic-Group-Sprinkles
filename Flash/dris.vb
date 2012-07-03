Imports System.Runtime.InteropServices
Imports System.Text

Module dris_code

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi, Pack:=1)> _
    Structure DRIS
        ' the first 4 fields are never encrypted
        Dim header1 As Byte                 ' should be set to 'DRIS'
        Dim header2 As Byte
        Dim header3 As Byte
        Dim header4 As Byte
        ' inputs
        Dim size As Integer                 ' size of this structure
        Dim seed1 As Integer                ' seed for data/dris encryption
        Dim seed2 As Integer                ' as above
        ' (maybe encrypted from now on)
        Dim myfunction As Integer           ' specify only one function
        Dim flags As Integer                ' options for the function selected. To use more than one OR them together: OPTION1 Or OPTION2...
        Dim execs_decrement As UInt32       ' amount by which to dec execs if we use flag: DEC_MANY_EXECS
        Dim data_crypt_key_num As Integer   ' number of the key (1-3) that the dongle uses to encrypt or decrypt user data
        Dim rw_offset As Integer            ' offset in the dongle data area to read or write data
        Dim rw_length As Integer            ' length of data are to read/write/encrypt/decrypt
        Dim DoNotUse As IntPtr              ' do not use the rw_data_ptr element use the "Data" argument of the DDProtCheck function
        ' to set this field use the function SetAltProgName instead
        <VBFixedArray(256), MarshalAs(UnmanagedType.ByValArray, SizeConst:=256)> Dim _alt_prog_name As Byte()
        Dim var_a As Integer                ' variable values for user algorithm
        Dim var_b As Integer
        Dim var_c As Integer
        Dim var_d As Integer
        Dim var_e As Integer
        Dim var_f As Integer
        Dim var_g As Integer
        Dim var_h As Integer
        Dim alg_number As Integer           ' the number of the user algorithm that you want to execute
        ' outputs
        Dim ret_code As Integer             ' return code from the protection check
        Dim ext_err As Integer              ' extended error
        Dim type As Integer                 ' type of dongle detected. 1 = Pro, 2 = FD
        Dim model As Integer                ' model of dongle detected. 1= Lite, 2=Pro, 3=N1, 4=N5, 5=N10, 6=N50, 7=NU
        Dim sdsn As Integer                 ' Software Developer's Serial Number
        'to read this field use the function GetProductCode instead
        <VBFixedArray(12), MarshalAs(UnmanagedType.ByValArray, SizeConst:=12)> Dim _prodcode As Byte()
        Dim dongle_number As UInt32
        Dim update_number As Integer
        Dim data_area_size As UInt32        ' size of the data area in the dongle detected
        Dim max_alg_num As Integer          ' the maximum algorithm number in the dongle detected
        Dim execs As Integer                ' executions left: -1 indicates 'no limit'
        Dim exp_day As Integer              ' expiry day: -1 indicates 'no limit'
        Dim exp_month As Integer            ' expiry month: -1 indicates 'no limit'
        Dim exp_year As Integer             ' expiry year: -1 indicates 'no limit'
        Dim features As UInt32              ' features value
        Dim net_users As Integer            ' maximum number of network users for the dongle detected: -1 indicates 'mo limit'
        Dim alg_answer As Integer           ' answer to the user algorithm executed with the given variable values
        Dim fd_capacity As UInt32           ' capacity of the data area in FD dongle. Currently fixed at ~10MB but may change in the future.
        'to read this field use the function GetFDDrive instead
        <VBFixedArray(128), MarshalAs(UnmanagedType.ByValArray, SizeConst:=128)> Dim _fd_drive As Byte()
    End Structure

    Declare Function DDProtCheck32 Lib "dpwin32.dll" Alias "DDProtCheck" (ByRef mydris As DRIS, ByVal data() As Byte) As Integer
    Declare Function DDProtCheck64 Lib "dpwin64.dll" Alias "DDProtCheck" (ByRef mydris As DRIS, ByVal data() As Byte) As Integer

    ' call the correct dll depending on whether we are running 32-bit or 64-bit code
    Function DDProtCheck(ByRef mydris As DRIS, ByVal data() As Byte) As Integer
        If (IntPtr.Size = 4) Then
            DDProtCheck = DDProtCheck32(mydris, data)
        Else
            DDProtCheck = DDProtCheck64(mydris, data)
        End If
    End Function

    ' Functions
    Public Const PROTECTION_CHECK As Integer = 1            ' checks for dongle, check program params...
    Public Const EXECUTE_ALGORITHM As Integer = 2           ' protection check + calculate answer for specified algorithm with specified inputs
    Public Const WRITE_DATA_AREA As Integer = 3             ' protection check + writes dongle data area
    Public Const READ_DATA_AREA As Integer = 4              ' protection check + reads dongle data area
    Public Const ENCRYPT_USER_DATA As Integer = 5           ' protection check + the dongle will encrypt user data
    Public Const DECRYPT_USER_DATA As Integer = 6           ' protection check + the dongle will decrypt user data
    Public Const FAST_PRESENCE_CHECK As Integer = 7         ' checks for the presence of the correct dongle only with minimal security, no flags allowed

    ' Flags
    Public Const DEC_ONE_EXEC As Integer = 1                ' decrement execs by 1
    Public Const DEC_MANY_EXECS As Integer = 2              ' decrement execs by number specified in execs
    Public Const START_NET_USER As Integer = 4              ' starts a network user
    Public Const STOP_NET_USER As Integer = 8               ' stops a network user (a protection check is NOT performed)
    Public Const USE_FUNCTION_ARGUMENT As Integer = 16      ' use the extra argument in the function for pointers
    Public Const CHECK_LOCAL_FIRST As Integer = 32          ' always look in local ports before looking in network ports
    Public Const CHECK_NETWORK_FIRST As Integer = 64        ' always look on the network before looking in local ports
    Public Const USE_ALT_PROG_NAME As Integer = 128         ' use name specified in prog_name instead of this program name
    Public Const DONT_SET_MAXDAYS_EXPIRY As Integer = 256   ' if the max days expiry date has not been calculated then do not do it this time
    Public Const MATCH_DONGLE_NUMBER As Integer = 512       ' restrict the search to match the dongle number specified in the DRIS

    ' !!!!
    Public Const MY_SDSN As Integer = 11986                 ' !!!! change this value to be the value of your SDSN (demo = 10101)

    ' **********************   useful functions for use in sample code  ***************************
    ' use this function to set the alt_prog_name field of the DRIS
    Public Sub SetAltProgName(ByVal mydris As DRIS, ByVal altprogname As String)
        Dim i, num_bytes As Integer
        Dim ascii As New ASCIIEncoding()
        Dim temp As Byte()

        temp = ascii.GetBytes(altprogname)
        num_bytes = ascii.GetByteCount(altprogname)
        For i = 0 To num_bytes - 1
            mydris._alt_prog_name(i) = temp(i)
        Next
        mydris._alt_prog_name(num_bytes) = 0           'null-terminate string
    End Sub

    ' use this function to read the prodcode field of the DRIS
    Public Function GetProductCode(ByVal mydris As DRIS) As String
        Dim ascii As New ASCIIEncoding()
        Dim i As Integer
        For i = 0 To 11
            If mydris._prodcode(i) = 0 Then
                Exit For
            End If
        Next
        Return ascii.GetString(mydris._prodcode, 0, i)
    End Function

    ' use this function to read the fd_drive field of the DRIS
    Public Function GetFDDrive(ByVal mydris As DRIS) As String
        Dim ascii As New ASCIIEncoding()
        Dim i As Integer
        For i = 0 To 127
            If mydris._fd_drive(i) = 0 Then
                Exit For
            End If
        Next
        Return ascii.GetString(mydris._fd_drive, 0, i)
    End Function

    ' copies an integer to offset "index" in trhe specified byte array
    Public Sub Set4Bytes(ByVal data() As Byte, ByVal index As Integer, ByVal value As Integer)
        Dim MyGC As GCHandle = GCHandle.Alloc(value, GCHandleType.Pinned)
        Dim AddressOfValue As IntPtr = MyGC.AddrOfPinnedObject()
        Marshal.Copy(AddressOfValue, data, index, 4)
        MyGC.Free()
    End Sub

    ' converts to DRIS structure to a byte array (so we can do encryption)
    Public Sub DrisToByteArray(ByVal mydris As DRIS, ByVal data() As Byte)
        Dim MyGC As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
        Marshal.StructureToPtr(mydris, MyGC.AddrOfPinnedObject, False)
        MyGC.Free()
    End Sub

    ' converts a byte array to the DRIS structure (so we can do encryption)
    Public Sub ByteArrayToDris(ByVal data() As Byte, ByRef mydris As DRIS)
        Dim MyGC As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
        mydris = Marshal.PtrToStructure(MyGC.AddrOfPinnedObject, mydris.GetType)
        MyGC.Free()
    End Sub

    ' initialise the DRIS with random values
    Public Sub RandomSet(ByRef mydris As DRIS)
        '    Dim temp_dris(Len(mydris)) As Byte
        Dim temp_dris(Marshal.SizeOf(mydris) - 1) As Byte
        Dim rnd As New Random()
        Dim i As Integer

        rnd.NextBytes(temp_dris)
        ByteArrayToDris(temp_dris, mydris)
        For i = 0 To Marshal.SizeOf(mydris) - 1
            temp_dris(i) = 0
        Next
        DrisToByteArray(mydris, temp_dris)
    End Sub

    ' look at the error code and try to display an appropriate message
    Public Sub DisplayError(ByVal ret_code As Integer, ByVal extended_error As Integer)
        Select Case ret_code
            Case 401
                '           MsgBox("Error! No dongles detected!", MsgBoxStyle.OKOnly, "Error")
            Case 403
                '          MsgBox("Error! A dongle was detected but it was a different type to the one specified in DinkeyAdd.", MsgBoxStyle.OKOnly, "Error")
            Case 404
                '         MsgBox("Error! A dongle was detected but it was a different model to those specified in DinkeyAdd.", MsgBoxStyle.OKOnly, "Error")
            Case 409
                '        MsgBox("Error! The dongle detected has not been programmed by DinkeyAdd.", MsgBoxStyle.OKOnly, "Error")
            Case 410
                '       MsgBox("Error! A dongle was detected but it has a different Product Code to the one specified in DinkeyAdd.", MsgBoxStyle.OKOnly, "Error")
            Case 411
                '       MsgBox("Error! A dongle was detected but this program was not found in list of protected programs.", MsgBoxStyle.OKOnly, "Error")
            Case 413
                '       MsgBox("Error! This program has not been protected by DinkeyAdd. For guidance please read the DinkeyAdd chapter of the Dinkey manual.", MsgBoxStyle.OKOnly, "Error")
            Case 417
                '     MsgBox("Error! One or more of the parameters set in the DRIS is incorrect. This could be caused if you are encrypting the DRIS in your code but did not specify DRIS encryption in DinkeyAdd - or vice versa.", MsgBoxStyle.OKOnly, "Error")
            Case 423
                '     MsgBox("Error! The number of network users has been exceeded.", MsgBoxStyle.OKOnly, "Error")
            Case 435
                '      MsgBox("Error! DinkeyServer has not been detected on the network.", MsgBoxStyle.OKOnly, "Error")
            Case Else
                '     MsgBox("An error occurred checking the dongle. Error: " + Str(ret_code) + ". Extended Error: " + Str(extended_error) + ".", MsgBoxStyle.OKOnly, "Error")
        End Select
    End Sub

End Module
