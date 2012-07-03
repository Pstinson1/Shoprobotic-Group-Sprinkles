'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading

Public Module mHook

    ' Dll declarations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Integer) As Integer
    Private Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As KeyboardHookDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Integer
    Private Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As KBDLLHOOKSTRUCT) As Integer

    ' Delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Delegate Function KeyboardHookDelegate(ByVal Code As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer
    Public Delegate Sub KeyboardEventCallback(ByVal lParam As KBDLLHOOKSTRUCT)

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Structure KBDLLHOOKSTRUCT

        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer

    End Structure

    ' Low-Level Keyboard Constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Const HC_ACTION As Integer = 0

    Public Const LLKHF_EXTENDED As Integer = &H1
    Public Const LLKHF_INJECTED As Integer = &H10
    Public Const LLKHF_ALTDOWN As Integer = &H20
    Public Const LLKHF_UP As Integer = &H80

    ' Virtual Keys
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Const VK_TAB = &H9
    Public Const VK_CONTROL = &H11
    Public Const VK_ESCAPE = &H1B
    Public Const VK_DELETE = &H2E

    Private Const WH_KEYBOARD_LL As Integer = 13&

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public keyboardHandle As Integer
    <MarshalAs(UnmanagedType.FunctionPtr)> Private internalCallback As KeyboardHookDelegate
    Private externallCallback As KeyboardEventCallback = Nothing

    ' KeyboardCallback
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function KeyboardCallback(ByVal Code As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Integer

        If (Code = HC_ACTION) Then

            If Not externallCallback Is Nothing Then
                externallCallback.Invoke(lParam)
            End If
         End If

        Return CallNextHookEx(keyboardHandle, Code, wParam, lParam)

    End Function

    ' HookKeyboard
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub HookKeyboard(ByVal callbackFunction As KeyboardEventCallback)

        internalCallback = New KeyboardHookDelegate(AddressOf KeyboardCallback)
        externallCallback = callbackFunction

        keyboardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, internalCallback, Marshal.GetHINSTANCE([Assembly].GetExecutingAssembly.GetModules()(0)).ToInt32, 0)

    End Sub

    ' UnhookKeyboard
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub UnhookKeyboard()

        Call UnhookWindowsHookEx(keyboardHandle)

    End Sub


End Module
