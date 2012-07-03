'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.Win32

Imports HelperFunctions
Imports DebugWindow

' the serial manager form
' event driven serial IO
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fAccessPayment

    ' enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    Private inputDevice As cInputDevice
    Private numberOfKeyboards As Integer
    Private currentCardNumber As String = ""
    Private buildingCardNumber As String = ""

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the reader invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle

        helperFunctions = helperFunctionsFactory.GetManager
        debugInformation = debugInformationFactory.GetManager

        'Dim filter As MyFilter = New MyFilter(Me.Handle)

        'Application.AddMessageFilter(Filter)
        'Filter.SetCallback(AddressOf CaptureDeviceEventCallback)


        inputDevice = New cInputDevice(Me.Handle)
        inputDevice.SetCallback(AddressOf InputDeviceEventCallback)

        numberOfKeyboards = inputDevice.EnumerateDevices()

        For Each singleDevice As cInputDevice.DeviceInfo In inputDevice.deviceList

            DevicesCombo.Items.Add(singleDevice.deviceHandle.ToString & "-" & singleDevice.Name)

        Next

    End Sub

    ' The WndProc is overridden to allow InputDevice to intercept messages to the window and thus catch WM_INPUT messages
    Protected Overrides Sub WndProc(ByRef message As Message)

        Dim messageProcessed As Boolean

        If inputDevice IsNot Nothing Then
            messageProcessed = inputDevice.ProcessMessage(message)
        End If

        MyBase.WndProc(message)

    End Sub

    Public Sub InputDeviceEventCallback(ByVal m_deviceInfo As cInputDevice.DeviceInfo) ', ByVal m_device As cInputDevice.DeviceType)

        If m_deviceInfo.Name = "HID Keyboard Device" Then

            If m_deviceInfo.key = 13 Then
                currentCardNumber = buildingCardNumber
                buildingCardNumber = ""

                helperFunctions.AddToListBox(ProgressList, m_deviceInfo.deviceName)
                helperFunctions.AddToListBox(ProgressList, m_deviceInfo.deviceHandle)
                helperFunctions.AddToListBox(ProgressList, m_deviceInfo.Name)
                helperFunctions.SetLabelText(CardNumberLabel, currentCardNumber)
                helperFunctions.AddToListBox(ProgressList, "Card read: " & currentCardNumber)
            Else
                buildingCardNumber = buildingCardNumber & Chr(m_deviceInfo.key)
            End If

        End If

    End Sub

    ' AccessPaymentManagerForm_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AccessPaymentManagerForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

    'Public Sub New()

    '    Dim filter As MyFilter = New MyFilter(Me.)

    '    Application.AddMessageFilter(filter)
    '    filter.SetCallback(AddressOf CaptureDeviceEventCallback)
    '    ' This call is required by the Windows Form Designer.

    '' This call is required by the Windows Form Designer.
    '    InitializeComponent()

    '' Add any initialization after the InitializeComponent() call.

    'End Sub

    'Public Sub CaptureDeviceEventCallback(ByVal keyPressed As UShort)

    '    If keyPressed = 13 Then

    '        currentCardNumber = buildingCardNumber
    '        buildingCardNumber = ""


    '        helperFunctions.AddToListBox(ProgressList, m_deviceInfo.deviceName)
    '        helperFunctions.AddToListBox(ProgressList, m_deviceInfo.deviceHandle)
    '        helperFunctions.AddToListBox(ProgressList, m_deviceInfo.Name)
    '        helperFunctions.SetLabelText(CardNumberLabel, currentCardNumber)
    '        helperFunctions.AddToListBox(ProgressList, "Card read: " & currentCardNumber)

    '    Else
    '        buildingCardNumber = buildingCardNumber & Chr(keyPressed)


    '    End If

    'End Sub


    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click

        Hide()

    End Sub

End Class


'Public Class MyFilter

'    Implements System.Windows.Forms.IMessageFilter

'    Private inputDevice As cInputDevice
'    Private numberOfKeyboards As Integer
'    Private handleToTrap As IntPtr = 0

'    Private Const WM_INPUT = &HFF
'    Private Const RID_INPUT As Integer = &H10000003
'    Private Const RIM_TYPEMOUSE As Integer = 0
'    Private Const RIM_TYPEKEYBOARD As Integer = 1
'    Private Const RIM_TYPEHID As Integer = 2
'    Private Const WM_KEYDOWN As Integer = &H100
'    Private Const WM_SYSKEYDOWN As Integer = &H104
'    Private Const RIDEV_INPUTSINK As Integer = &H100                 ' the following constants are defined in Windows.h

'    Private eventCallback As InputDeviceEventCallback = Nothing

'    <DllImport("User32.dll")> Private Shared Function RegisterRawInputDevices(ByVal pRawInputDevice As RAWINPUTDEVICE(), ByVal uiNumDevices As UInteger, ByVal cbSize As UInteger) As Boolean
'    End Function

'    Public Delegate Sub InputDeviceEventCallback(ByVal keyPressed As UShort) ', ByVal m_device As DeviceType)

'    <DllImport("User32.dll")> Private Shared Function GetRawInputData(ByVal hRawInput As IntPtr, ByVal uiCommand As UInteger, ByVal pData As IntPtr, ByRef pcbSize As UInteger, ByVal cbSizeHeader As UInteger) As UInteger
'    End Function

'    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWHID
'        <MarshalAs(UnmanagedType.U4)> Public dwSizHid As Integer
'        <MarshalAs(UnmanagedType.U4)> Public dwCount As Integer
'    End Structure

'    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWINPUTDEVICE
'        <MarshalAs(UnmanagedType.U2)> Public usUsagePage As UShort
'        <MarshalAs(UnmanagedType.U2)> Public usUsage As UShort
'        <MarshalAs(UnmanagedType.U4)> Public dwFlags As Integer
'        Public hwndTarget As IntPtr
'    End Structure

'    <StructLayout(LayoutKind.Sequential)> Friend Structure BUTTONSSTR
'        <MarshalAs(UnmanagedType.U2)> Public usButtonFlags As UShort
'        <MarshalAs(UnmanagedType.U2)> Public usButtonData As UShort
'    End Structure

'    <StructLayout(LayoutKind.Explicit)> Friend Structure RAWMOUSE
'        <MarshalAs(UnmanagedType.U2)> <FieldOffset(0)> Public usFlags As UShort
'        <MarshalAs(UnmanagedType.U4)> <FieldOffset(4)> Public ulButtons As UInteger
'        <FieldOffset(4)> Public buttonsStr As BUTTONSSTR
'        <MarshalAs(UnmanagedType.U4)> <FieldOffset(8)> Public ulRawButtons As UInteger
'        <FieldOffset(12)> Public lLastX As Integer
'        <FieldOffset(16)> Public lLastY As Integer
'        <MarshalAs(UnmanagedType.U4)> <FieldOffset(20)> Public ulExtraInformation As UInteger
'    End Structure

'    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWKEYBOARD
'        <MarshalAs(UnmanagedType.U2)> Public MakeCode As UShort
'        <MarshalAs(UnmanagedType.U2)> Public Flags As UShort
'        <MarshalAs(UnmanagedType.U2)> Public Reserved As UShort
'        <MarshalAs(UnmanagedType.U2)> Public VKey As UShort
'        <MarshalAs(UnmanagedType.U4)> Public Message As UInteger
'        <MarshalAs(UnmanagedType.U4)> Public ExtraInformation As UInteger
'    End Structure
'    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWINPUTHEADER
'        <MarshalAs(UnmanagedType.U4)> Public dwType As Integer
'        <MarshalAs(UnmanagedType.U4)> Public dwSize As Integer
'        Public hDevice As IntPtr
'        <MarshalAs(UnmanagedType.U4)> Public wParam As Integer

'    End Structure


'    <StructLayout(LayoutKind.Explicit)> Friend Structure RAWINPUT
'        <FieldOffset(0)> Public header As RAWINPUTHEADER
'        <FieldOffset(16)> Public mouse As RAWMOUSE
'        <FieldOffset(16)> Public keyboard As RAWKEYBOARD
'        <FieldOffset(16)> Public hid As RAWHID
'    End Structure

'    Public Sub SetCallback(ByVal newEventCallback As InputDeviceEventCallback)
'        eventCallback = newEventCallback
'    End Sub

'Public Function PreFilterMessage(ByRef Message As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage

'    Dim dwSize As UInteger = 0
'    Dim rawInput As RAWINPUT

'    If Message.Msg = WM_INPUT Then ' 514 is WM_LBUTTONUP

'        ' First call to GetRawInputData sets the value of dwSize, which can then be used to allocate the appropriate amount of memory, storing the pointer in "buffer".
'        GetRawInputData(Message.LParam, RID_INPUT, IntPtr.Zero, dwSize, CUInt(Marshal.SizeOf(GetType(RAWINPUTHEADER))))

'        Dim buffer As IntPtr = Marshal.AllocHGlobal(CInt(dwSize))
'        Try



'            If buffer <> IntPtr.Zero Then


'                GetRawInputData(Message.LParam, RID_INPUT, IntPtr.Zero, dwSize, CUInt(Marshal.SizeOf(GetType(RAWINPUTHEADER))))

'                rawInput = CType(Marshal.PtrToStructure(buffer, GetType(RAWINPUT)), RAWINPUT)

'                '         If rawInput.header.dwType = RIM_TYPEKEYBOARD AndAlso rawInput.header.hDevice = handleToTrap Then
'                If rawInput.header.hDevice = handleToTrap Then

'                    ' Filter for Key Down events and then retrieve information about the keystroke
'                    If rawInput.keyboard.Message = WM_KEYDOWN OrElse rawInput.keyboard.Message = WM_SYSKEYDOWN Then

'                        Dim key As UShort = rawInput.keyboard.VKey

'                        If Not eventCallback Is Nothing Then
'                            eventCallback.Invoke(key)
'                        End If




'                    End If

'                    Return True                                                                 ' returns True to stop the message

'                End If

'            End If

'        Catch ex As Exception
'        Finally
'            Marshal.FreeHGlobal(buffer)
'        End Try

'    End If

'End Function

'Public Sub New(ByVal hwnd As IntPtr)

'    Dim rid As RAWINPUTDEVICE() = New RAWINPUTDEVICE(0) {}

'    rid(0).usUsagePage = &H1
'    rid(0).usUsage = &H6
'    rid(0).dwFlags = RIDEV_INPUTSINK
'    rid(0).hwndTarget = hwnd

'    If Not RegisterRawInputDevices(rid, CUInt(rid.Length), CUInt(Marshal.SizeOf(rid(0)))) Then
'        Throw New ApplicationException("Failed to register raw input device(s).")
'    End If

'    inputDevice = New cInputDevice()
'    numberOfKeyboards = inputDevice.EnumerateDevices()

'    For Each singleDevice As cInputDevice.DeviceInfo In inputDevice.deviceList

'        Console.WriteLine(">" & ControlChars.Tab & singleDevice.deviceName)
'        Console.WriteLine(ControlChars.Tab & singleDevice.deviceHandle.ToString)
'        '  Console.WriteLine(ControlChars.Tab & )

'        If singleDevice.Name = "HID Keyboard Device" Then
'            Console.WriteLine(ControlChars.Tab & "(trapping)")
'            handleToTrap = singleDevice.deviceHandle
'        End If


'    Next

'End Sub

'End Class




' class cAccessPaymentFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cAccessPaymentFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared accessPaymentManager As fAccessPayment = Nothing

    Public Function GetManager() As fAccessPayment

        If IsNothing(accessPaymentManager) Then

            accessPaymentManager = New fAccessPayment

        End If

        Return accessPaymentManager

    End Function

End Class

