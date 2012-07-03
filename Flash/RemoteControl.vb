'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Public Class RemoteControl
    Public Event ButtonClicked(ByVal strButton As String)

    Private Sub RemoteControl_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        RaiseEvent ButtonClicked("RestockScreen")
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        RaiseEvent ButtonClicked("Reset")
    End Sub
End Class