'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Data.SqlClient

Public Class cCheckLayout

    Private databaseConnection As SqlConnection

    Public Sub New()
        databaseConnection = New SqlConnection(My.Settings.DatabaseConnection)
    End Sub

    Public Function Check() As Boolean

        Dim newLayoutAvailable As Boolean = False
        Dim queryCommand As New SqlCommand("spLookForNewLayout", databaseConnection)
        queryCommand.CommandType = CommandType.StoredProcedure

        queryCommand.Parameters.Add("@Result", SqlDbType.Int, 1)
        queryCommand.Parameters("@Result").Direction = ParameterDirection.Output

        Try
            databaseConnection.Open()
            queryCommand.ExecuteNonQuery()
            databaseConnection.Close()

        Catch ex As Exception
            Throw ex
        Finally
            databaseConnection.Close()
        End Try

        newLayoutAvailable = queryCommand.Parameters("@Result").Value

        Return newLayoutAvailable

    End Function

 End Class

