'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Public Class fConfirmNewLayout

    Private confirmNewLayout As Boolean = False

    Public Function Ask() As Boolean

        ShowDialog()

        Return confirmNewLayout

    End Function


    Private Sub YesHoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YesHoverButton.Click
        confirmNewLayout = True
        Hide()
    End Sub

    Private Sub NoHoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NoHoverButton.Click
        confirmNewLayout = False
        Hide()
    End Sub
End Class