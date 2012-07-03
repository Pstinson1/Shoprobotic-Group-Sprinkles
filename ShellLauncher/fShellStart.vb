'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.ServiceProcess

Imports HelperFunctions

Public Class fShellStart

    ' variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    Private tick As Integer = 30

    Public Sub New()

        setShellReady()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        helperFunctions = helperFunctionsFactory.GetManager()

        Timer1.Enabled = True

        If My.Settings.Hidden Then
            Hide()
        Else

        End If

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If tick = 0 Then
            Timer1.Enabled = False
            Try
                helperFunctions.ProcessStart("c:\\control\\vend.exe", "c:\\control\\")

                System.Threading.Thread.Sleep(500)
                Application.Exit()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            tick -= 1
            Label1.Text = tick
        End If
    End Sub


    Private Sub LogonToExplorerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogonToExplorerButton.Click

        helperFunctions.SetShell("explorer.exe")

    End Sub

    Private Sub LogonToShellButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogonToShellButton.Click

        helperFunctions.SetShell("shell.exe")

    End Sub

End Class

Module ShellReady

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

