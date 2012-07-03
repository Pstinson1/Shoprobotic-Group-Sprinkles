'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Module mShellReady

    'Declarations
    Private Declare Function OpenEvent Lib "kernel32.dll" Alias "OpenEventA" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal lpName As String) As Long
    Private Declare Function SetEvent Lib "kernel32.dll" (ByVal hEvent As Long) As Long
    Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As Long) As Long
    Private Const EVENT_MODIFY_STATE As Long = &H2

    'Put this part in the load event/start event
    Public Sub setShellReady()
        Dim H As Long
        H = OpenEvent(EVENT_MODIFY_STATE, False, "msgina: ShellReadyEvent")
        If H <> 0 Then
            SetEvent(H)
            CloseHandle(H)
        End If
    End Sub


End Module
