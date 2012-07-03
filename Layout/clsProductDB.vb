'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Data.SqlClient

Namespace Teknovation.Data
    Public MustInherit Class DALBase : Implements IDisposable
        Private _connection As SqlConnection

        Protected Sub New(ByVal connect As String)
            _connection = New SqlConnection(connect)
        End Sub

        Protected ReadOnly Property Connection() As SqlConnection
            Get
                Return _connection
            End Get
        End Property

        Public Sub Dispose() Implements IDisposable.Dispose
            _connection.Dispose()
        End Sub
    End Class


    Public Class ProductBinBLL : Inherits DALBase
        Public Sub New()
            MyBase.New(My.Settings.DatabaseConnection)
        End Sub

        Public Function GetAllBins() As IList(Of ProductBin)
            Dim cmd As New SqlCommand("spGetLayoutProducts", Me.Connection)
            cmd.CommandType = CommandType.StoredProcedure
            Dim dr As SqlDataReader
            Dim mProducts As IList(Of ProductBin) = New List(Of ProductBin)

            Try
                Me.Connection.Open()
                dr = cmd.ExecuteReader

                Dim mBin As ProductBin
                While dr.Read
                    mBin = New ProductBin
                    With mBin
                        .KeyVend = dr("ContainerNumber") ' dr("KeyVend")
                        .Name = dr("ProdName")

                        .PositionID = dr("PosID")
                        .PreName = dr("PreName")
                        .Price = dr("ProdPrice")
                        .StockQty = dr("ItemsInStock")
                        .xPosition = dr("xVisualPosition")
                        .yPosition = dr("yVisualPosition")

                        'If .Name = "Instax Mini 7 Camera + Film" Then

                        '    MsgBox(.StockQty.ToString & "-" & .PositionID.ToString)
                        'End If
                    End With
                    mProducts.Add(mBin)
                End While

            Catch ex As Exception
                Throw ex
            Finally
                Me.Connection.Close()
            End Try

            Return mProducts
        End Function

        Public Sub UpdateStock(ByVal PositionID As Integer, ByVal Qty As Integer)
            Dim cmd As New SqlCommand("spUpdateStock", Me.Connection)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("PosID", SqlDbType.Int, 1)
            cmd.Parameters("PosID").Value = PositionID

            cmd.Parameters.Add("Qty", SqlDbType.Int, 1)
            cmd.Parameters("Qty").Value = Qty

            cmd.Parameters.Add("Result", SqlDbType.Int, 1)
            cmd.Parameters("Result").Direction = ParameterDirection.Output

            Try
                Me.Connection.Open()
                cmd.ExecuteNonQuery()
                Me.Connection.Close()

            Catch ex As Exception
                Throw ex
            Finally
                Me.Connection.Close()
            End Try

        End Sub
    End Class
End Namespace

Namespace Teknovation
    Public Class ProductBin
        Public PositionID As Integer
        Public KeyVend As String
        Public StockQty As Integer
        Public Price As Decimal
        Public Name As String
        Public PreName As String
        Public xPosition As Integer
        Public yPosition As Integer
        Public currencySymbol As String
    End Class

End Namespace





