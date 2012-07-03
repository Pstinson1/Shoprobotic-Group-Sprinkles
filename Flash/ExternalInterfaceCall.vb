'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System
Imports System.Collections
Imports System.Text

Namespace Flash.External

    ' Value object containing information about an ExternalInterface call 
    ' sent between a .NET application and a Shockwave Flash object. 

    Public Class ExternalInterfaceCall
#Region "Private Fields"

        Private _functionName As String
        Private _arguments As ArrayList

#End Region

#Region "Constructor"


        ' Creates a new ExternalInterfaceCall instance with the specified 
        ' function name. 
        ' <param name="functionName">The name of the function as provided  by Flash Player</param> 

        Public Sub New(ByVal functionName As String)
            _functionName = functionName
        End Sub

#End Region

#Region "Public Properties"

        ' The name of the function call provided by Flash Player 
        Public ReadOnly Property FunctionName() As String
            Get
                Return _functionName
            End Get
        End Property


        ' The function parameters associated with this function call. 
        Public ReadOnly Property Arguments() As Object()
            Get
                Return DirectCast(_arguments.ToArray(GetType(Object)), Object())
            End Get
        End Property

#End Region

#Region "Public Methods"

        Public Overloads Overrides Function ToString() As String
            Dim result As New StringBuilder()
            result.AppendFormat("Function Name: {0}{1}", _functionName, Environment.NewLine)
            If _arguments IsNot Nothing AndAlso _arguments.Count > 0 Then
                result.AppendFormat("Arguments:{0}", Environment.NewLine)
                For Each arg As Object In _arguments
                    result.AppendFormat("" & Chr(9) & "{0}{1}", arg, Environment.NewLine)
                Next
            End If
            Return result.ToString()
        End Function

#End Region

#Region "Internal Methods"

        Friend Sub AddArgument(ByVal argument As Object)
            If _arguments Is Nothing Then
                _arguments = New ArrayList()
            End If
            _arguments.Add(argument)
        End Sub

#End Region
    End Class
End Namespace