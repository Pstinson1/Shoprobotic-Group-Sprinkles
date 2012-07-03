'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports System.Data.SqlClient



Public Class cUserList

    Private databaseConnection As SqlConnection
    Private userList As IList(Of cUser)

    Sub New()
        databaseConnection = New SqlConnection(My.Settings.DatabaseConnectionString)
    End Sub

    Public Function GetAllUsers()

        Dim cmd As New SqlCommand("spGetAllUsers", databaseConnection)
        Dim dr As SqlDataReader

        userList = New List(Of cUser)

        cmd.CommandType = CommandType.StoredProcedure

        Try
            databaseConnection.Open()
            dr = cmd.ExecuteReader

            Dim mUser As cUser

            While dr.Read
                mUser = New cUser

                With mUser
                    .userId = dr("id")
                    .userName = dr("name")
                    .userAccess = dr("accesstype")
                End With
                userList.Add(mUser)
            End While

        Catch ex As Exception
            Throw ex
        Finally
            databaseConnection.Close()
        End Try

        Return userList

    End Function


End Class

Public Class cUser

    Private userIdentifier As Integer
    Private displayName As String
    Private accessType As Integer

    Private databaseConnection As SqlConnection

    Public Function CheckUserCredentials(ByVal passcodeAttempt As String) As Integer

        Dim checkResult As Integer = 0
        Dim queryCommand As New SqlCommand("spCheckUserCredentials", databaseConnection)
        queryCommand.CommandType = CommandType.StoredProcedure

        queryCommand.Parameters.Add("@UserID", SqlDbType.Int, 1)
        queryCommand.Parameters("@UserID").Value = userIdentifier

        queryCommand.Parameters.Add("@Passcode", SqlDbType.VarChar, 50)
        queryCommand.Parameters("@Passcode").Value = passcodeAttempt

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

        checkResult = queryCommand.Parameters("@Result").Value

        Return checkResult

    End Function


    Property userId() As Integer
        Get
            Return userIdentifier
        End Get
        Set(ByVal value As Integer)
            userIdentifier = value
        End Set
    End Property

    Property userName() As String
        Get
            Return displayName
        End Get
        Set(ByVal value As String)
            displayName = value
        End Set
    End Property

    Property userAccess() As Integer
        Get
            Return accessType
        End Get
        Set(ByVal value As Integer)
            accessType = value
        End Set
    End Property


    Public Sub New()
        databaseConnection = New SqlConnection(My.Settings.DatabaseConnectionString)
    End Sub

End Class
