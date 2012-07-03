Imports System.Collections
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.Win32

' Handles raw input from keyboard devices.
Public NotInheritable Class cInputDevice

    Public Delegate Sub InputDeviceEventCallback(ByVal m_deviceInfo As DeviceInfo) ', ByVal m_device As DeviceType)

    ' constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Const RIDEV_INPUTSINK As Integer = &H100                 ' the following constants are defined in Windows.h
    Private Const RID_INPUT As Integer = &H10000003

    Private Const FAPPCOMMAND_MASK As Integer = &HF000
    Private Const FAPPCOMMAND_MOUSE As Integer = &H8000
    Private Const FAPPCOMMAND_OEM As Integer = &H1000

    Private Const RIM_TYPEMOUSE As Integer = 0
    Private Const RIM_TYPEKEYBOARD As Integer = 1
    Private Const RIM_TYPEHID As Integer = 2

    Private Const RIDI_DEVICENAME As Integer = &H20000007

    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_SYSKEYDOWN As Integer = &H104
    Private Const WM_INPUT As Integer = &HFF
    Private Const VK_OEM_CLEAR As Integer = &HFE
    Private Const VK_LAST_KEY As Integer = VK_OEM_CLEAR

    ' structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Class DeviceInfo
        Public deviceName As String
        Public deviceHandle As IntPtr
        Public Name As String
        Public source As String
        Public key As UShort
        Public vKey As String
    End Class

    ' The following structures are defined in Windows.h
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWINPUTDEVICELIST
        Public hDevice As IntPtr
        <MarshalAs(UnmanagedType.U4)> Public dwType As Integer
    End Structure

    <StructLayout(LayoutKind.Explicit)> Friend Structure RAWINPUT
        <FieldOffset(0)> Public header As RAWINPUTHEADER
        <FieldOffset(16)> Public mouse As RAWMOUSE
        <FieldOffset(16)> Public keyboard As RAWKEYBOARD
        <FieldOffset(16)> Public hid As RAWHID
    End Structure

    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWINPUTHEADER
        <MarshalAs(UnmanagedType.U4)> Public dwType As Integer
        <MarshalAs(UnmanagedType.U4)> Public dwSize As Integer
        Public hDevice As IntPtr
        <MarshalAs(UnmanagedType.U4)> Public wParam As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWHID
        <MarshalAs(UnmanagedType.U4)> Public dwSizHid As Integer
        <MarshalAs(UnmanagedType.U4)> Public dwCount As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> Friend Structure BUTTONSSTR
        <MarshalAs(UnmanagedType.U2)> Public usButtonFlags As UShort
        <MarshalAs(UnmanagedType.U2)> Public usButtonData As UShort
    End Structure

    <StructLayout(LayoutKind.Explicit)> Friend Structure RAWMOUSE
        <MarshalAs(UnmanagedType.U2)> <FieldOffset(0)> Public usFlags As UShort
        <MarshalAs(UnmanagedType.U4)> <FieldOffset(4)> Public ulButtons As UInteger
        <FieldOffset(4)> Public buttonsStr As BUTTONSSTR
        <MarshalAs(UnmanagedType.U4)> <FieldOffset(8)> Public ulRawButtons As UInteger
        <FieldOffset(12)> Public lLastX As Integer
        <FieldOffset(16)> Public lLastY As Integer
        <MarshalAs(UnmanagedType.U4)> <FieldOffset(20)> Public ulExtraInformation As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWKEYBOARD
        <MarshalAs(UnmanagedType.U2)> Public MakeCode As UShort
        <MarshalAs(UnmanagedType.U2)> Public Flags As UShort
        <MarshalAs(UnmanagedType.U2)> Public Reserved As UShort
        <MarshalAs(UnmanagedType.U2)> Public VKey As UShort
        <MarshalAs(UnmanagedType.U4)> Public Message As UInteger
        <MarshalAs(UnmanagedType.U4)> Public ExtraInformation As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)> Friend Structure RAWINPUTDEVICE
        <MarshalAs(UnmanagedType.U2)> Public usUsagePage As UShort
        <MarshalAs(UnmanagedType.U2)> Public usUsage As UShort
        <MarshalAs(UnmanagedType.U4)> Public dwFlags As Integer
        Public hwndTarget As IntPtr
    End Structure

    ' dll imports
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    <DllImport("User32.dll")> Private Shared Function GetRawInputDeviceList(ByVal pRawInputDeviceList As IntPtr, ByRef uiNumDevices As UInteger, ByVal cbSize As UInteger) As UInteger
    End Function

    <DllImport("User32.dll")> Private Shared Function GetRawInputDeviceInfo(ByVal hDevice As IntPtr, ByVal uiCommand As UInteger, ByVal pData As IntPtr, ByRef pcbSize As UInteger) As UInteger
    End Function

    <DllImport("User32.dll")> Private Shared Function RegisterRawInputDevices(ByVal pRawInputDevice As RAWINPUTDEVICE(), ByVal uiNumDevices As UInteger, ByVal cbSize As UInteger) As Boolean
    End Function

    <DllImport("User32.dll")> Private Shared Function GetRawInputData(ByVal hRawInput As IntPtr, ByVal uiCommand As UInteger, ByVal pData As IntPtr, ByRef pcbSize As UInteger, ByVal cbSizeHeader As UInteger) As UInteger
    End Function


    ' variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public deviceList() As DeviceInfo
    Private eventCallback As InputDeviceEventCallback = Nothing

    ' InputDevice constructor; registers the raw input devices for the calling window.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub New(ByVal hwnd As IntPtr)
        'Create an array of all the raw input devices we want to 
        'listen to. In this case, only keyboard devices.
        'RIDEV_INPUTSINK determines that the window will continue
        'to receive messages even when it doesn't have the focus.
        Dim rid As RAWINPUTDEVICE() = New RAWINPUTDEVICE(0) {}

        rid(0).usUsagePage = &H1
        rid(0).usUsage = &H6
        rid(0).dwFlags = RIDEV_INPUTSINK
        rid(0).hwndTarget = hwnd

        If Not RegisterRawInputDevices(rid, CUInt(rid.Length), CUInt(Marshal.SizeOf(rid(0)))) Then
            Throw New ApplicationException("Failed to register raw input device(s).")
        End If

    End Sub


    Public Sub New()


    End Sub




#Region "ReadReg( string item, ref bool isKeyboard )"

    ' Reads the Registry to retrieve a friendly description of the device, and determine whether it is a keyboard.
    Private Function ReadReg(ByVal item As String, ByRef isKeyboard As Boolean) As String
        ' Example Device Identification string
        ' @"\??\ACPI#PNP0303#3&13c0b0c5&0#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}";

        ' remove the \??\
        item = item.Substring(4)

        Dim split As String() = item.Split("#"c)

        Dim id_01 As String = split(0)
        Dim id_02 As String = split(1)
        Dim id_03 As String = split(2)

        'The final part is the class GUID and is not needed here
        'Open the appropriate key as read-only so no permissions
        'are needed.
        Dim OurKey As RegistryKey = Registry.LocalMachine

        Dim findme As String = String.Format("System\CurrentControlSet\Enum\{0}\{1}\{2}", id_01, id_02, id_03)

        OurKey = OurKey.OpenSubKey(findme, False)

        'Retrieve the desired information and set isKeyboard
        Dim deviceDesc As String = DirectCast(OurKey.GetValue("DeviceDesc"), String)
        Dim deviceClass As String = DirectCast(OurKey.GetValue("Class"), String)

        If deviceClass.ToUpper().Equals("KEYBOARD") Then
            isKeyboard = True
        Else
            isKeyboard = False
        End If
        Return deviceDesc
    End Function

#End Region

    Public Sub SetCallback(ByVal newEventCallback As InputDeviceEventCallback)
        eventCallback = newEventCallback
    End Sub

#Region "int EnumerateDevices()"

    ''' <summary>
    ''' Iterates through the list provided by GetRawInputDeviceList,
    ''' counting keyboard devices and adding them to deviceList.
    ''' </summary>
    ''' <returns>The number of keyboard devices found.</returns>
    Public Function EnumerateDevices() As Integer

        Dim NumberOfDevices As Integer = 0
        Dim deviceCount As UInteger = 0
        Dim dwSize As Integer = (Marshal.SizeOf(GetType(RAWINPUTDEVICELIST)))

        ' Get the number of raw input devices in the list,
        ' then allocate sufficient memory and get the entire list
        If GetRawInputDeviceList(IntPtr.Zero, deviceCount, CUInt(dwSize)) = 0 Then
            Dim pRawInputDeviceList As IntPtr = Marshal.AllocHGlobal(CInt(dwSize * deviceCount))
            GetRawInputDeviceList(pRawInputDeviceList, deviceCount, CUInt(dwSize))

            ' Iterate through the list, discarding undesired items
            ' and retrieving further information on keyboard devices
            For i As Integer = 0 To deviceCount - 1
                Dim dInfo As DeviceInfo
                Dim deviceName As String
                Dim pcbSize As UInteger = 0

                Dim rid As RAWINPUTDEVICELIST = CType(Marshal.PtrToStructure(New IntPtr((pRawInputDeviceList.ToInt32() + (dwSize * i))), GetType(RAWINPUTDEVICELIST)), RAWINPUTDEVICELIST)

                GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, IntPtr.Zero, pcbSize)

                If pcbSize > 0 Then
                    Dim pData As IntPtr = Marshal.AllocHGlobal(CInt(pcbSize))
                    GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, pData, pcbSize)
                    deviceName = DirectCast(Marshal.PtrToStringAnsi(pData), String)

                    ' Drop the "root" keyboard and mouse devices used for Terminal 
                    ' Services and the Remote Desktop
                    If deviceName.ToUpper().Contains("ROOT") Then
                        Continue For
                    End If

                    ' If the device is identified in the list as a keyboard or 
                    ' HID device, create a DeviceInfo object to store information 
                    ' about it
                    If rid.dwType = RIM_TYPEKEYBOARD OrElse rid.dwType = RIM_TYPEHID Then

                        dInfo = New DeviceInfo()

                        dInfo.deviceName = DirectCast(Marshal.PtrToStringAnsi(pData), String)
                        dInfo.deviceHandle = rid.hDevice

                        ' Check the Registry to see whether this is actually a 
                        ' keyboard, and to retrieve a more friendly description.
                        Dim IsKeyboardDevice As Boolean = False
                        Dim DeviceDesc As String = ReadReg(deviceName, IsKeyboardDevice)
                        dInfo.Name = DeviceDesc

                        ' If it is a keyboard and it isn't already in the list, add it and increase the' NumberOfDevices count
                        Dim deviceFound As Boolean = False

                        If Not deviceList Is Nothing Then

                            Dim deviceIndex As Integer = 0


                            Console.WriteLine("checking " & rid.hDevice.ToString)

                            For deviceIndex = 0 To deviceList.Length - 1
                                '      For Each singleDevice As DeviceInfo In deviceList
                                Console.WriteLine("   " & deviceList(deviceIndex).deviceHandle.ToString)
                                If deviceList(deviceIndex).deviceHandle = rid.hDevice Then

                                    Console.WriteLine("   Found")
                                    deviceFound = True
                                End If

                            Next

                        End If

                        If Not deviceFound AndAlso IsKeyboardDevice Then
                            ReDim Preserve deviceList(NumberOfDevices)
                            deviceList(NumberOfDevices) = dInfo

                            NumberOfDevices += 1

                        End If
                    End If
                    Marshal.FreeHGlobal(pData)
                End If
            Next


            Marshal.FreeHGlobal(pRawInputDeviceList)


            Return NumberOfDevices
        Else
            Throw New ApplicationException("An error occurred while retrieving the list of devices.")
        End If

    End Function

#End Region


#Region "ProcessInputCommand( Message message )"

    ' ProcessInputCommand - Processes WM_INPUT messages to retrieve information about any keyboard events that occur.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function ProcessInputCommand(ByVal message As Message) As Boolean

        Dim dwSize As UInteger = 0
        Dim messageProcessed As Boolean = False

        ' First call to GetRawInputData sets the value of dwSize, which can then be used to allocate the appropriate amount of memory, storing the pointer in "buffer".
        GetRawInputData(message.LParam, RID_INPUT, IntPtr.Zero, dwSize, CUInt(Marshal.SizeOf(GetType(RAWINPUTHEADER))))

        Dim buffer As IntPtr = Marshal.AllocHGlobal(CInt(dwSize))
        Try
            ' Check that buffer points to something, and if so, call GetRawInputData again to fill the allocated memory with information about the input
            If buffer <> IntPtr.Zero AndAlso GetRawInputData(message.LParam, RID_INPUT, buffer, dwSize, CUInt(Marshal.SizeOf(GetType(RAWINPUTHEADER)))) = dwSize Then

                ' Store the message information in "raw", then check' that the input comes from a keyboard device before' processing it to raise an appropriate KeyPressed event.
                Dim raw As RAWINPUT = CType(Marshal.PtrToStructure(buffer, GetType(RAWINPUT)), RAWINPUT)

                If raw.header.dwType = RIM_TYPEKEYBOARD Then

                    ' Filter for Key Down events and then retrieve information about the keystroke
                    If raw.keyboard.Message = WM_KEYDOWN OrElse raw.keyboard.Message = WM_SYSKEYDOWN Then

                        Dim key As UShort = raw.keyboard.VKey

                        ' On most keyboards, "extended" keys such as the arrow or 
                        ' page keys return two codes - the key's own code, and an
                        ' "extended key" flag, which translates to 255. This flag
                        ' isn't useful to us, so it can be disregarded.
                        If key > VK_LAST_KEY Then
                            Return messageProcessed
                        End If

                        ' Retrieve information about the device and the
                        ' key that was pressed.
                        Dim dInfo As DeviceInfo = Nothing

                        '        If deviceList.Contains(raw.header.hDevice) Then
                        ' find the device in the device list
                        Dim myKey As Keys
                        Dim deviceIndex = 0

                        While dInfo Is Nothing AndAlso deviceIndex < deviceList.Length

                            If raw.header.hDevice = deviceList(deviceIndex).deviceHandle Then

                                dInfo = deviceList(deviceIndex)

                            End If
                            deviceIndex = deviceIndex + 1

                        End While

                        ' if the device is found then fill in the key information.
                        If Not dInfo Is Nothing Then

                            myKey = CType([Enum].Parse(GetType(Keys), [Enum].GetName(GetType(Keys), key)), Keys)
                            dInfo.vKey = myKey.ToString()
                            dInfo.key = key
                        Else
                            Dim errMessage As String = [String].Format("Handle :{0} was not in hashtable. The device may support more than one handle or usage page, and is probably not a standard keyboard.", raw.header.hDevice)
                            Throw New ApplicationException(errMessage)
                        End If

                        ' If the key that was pressed is valid and there was no problem retrieving information on the device.
                        If Not eventCallback Is Nothing Then
                            messageProcessed = True
                            eventCallback.Invoke(dInfo)
                        End If

                    End If
                End If
            End If
        Finally
            Marshal.FreeHGlobal(buffer)
        End Try

        Return messageProcessed

    End Function

#End Region

    ' Filters Windows messages for WM_INPUT messages and calls ProcessInputCommand if necessary.
    Public Function ProcessMessage(ByVal message As Message) As Boolean

        Dim messageProcessed As Boolean = False

        Select Case message.Msg
            Case WM_INPUT

                messageProcessed = ProcessInputCommand(message)

                Exit Select
        End Select

        Return messageProcessed

    End Function

End Class

