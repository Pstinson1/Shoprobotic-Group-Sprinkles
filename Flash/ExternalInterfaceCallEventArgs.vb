'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************


Imports System

Namespace Flash.External

    Public Delegate Sub ExternalInterfaceCallEventHandler(ByVal sender As Object, ByVal e As ExternalInterfaceCallEventArgs)

    ' Event arguments for the ExternalInterfaceCallEventHandler. 
    Public Class ExternalInterfaceCallEventArgs
        Inherits System.EventArgs
        Private _functionCall As ExternalInterfaceCall

        Public Sub New(ByVal functionCall As ExternalInterfaceCall)
            MyBase.New()
            _functionCall = functionCall
        End Sub

        Public ReadOnly Property FunctionCall() As ExternalInterfaceCall
            Get
                Return _functionCall
            End Get
        End Property
    End Class

End Namespace