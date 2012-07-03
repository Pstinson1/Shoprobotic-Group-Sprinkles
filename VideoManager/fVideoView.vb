'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Public Class fVideoView


    ' Panel
    ' return the panel object on the form
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise(ByVal viewDescription As String)

        ' this line is suprizing important.
        Dim temporaryHandle As System.IntPtr = Me.Handle

        Me.Text = "View: " & viewDescription

    End Sub

    ' Panel
    ' return the panel object on the form
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function Panel() As System.Windows.Forms.Panel

        Return ViewPanel

    End Function

    ' fVideoView_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub fVideoView_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        e.Cancel = True
        Hide()

    End Sub

    Private Sub fVideoView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Hide()
    End Sub

    Private Sub fVideoView_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ViewPanel.Size = Me.Size
    End Sub

End Class