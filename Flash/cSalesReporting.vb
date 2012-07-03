'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports SettingsManager
Imports System.Data.SqlClient
Imports System.Threading.Thread
Imports DebugWindow

Public Class cSalesReporting

    Private transactionId As String
    Private productName As String
    Private productId As Integer
    Private positionId As Integer
    Private productPrice As Integer
    Private productBarcode As String
    Private stockLevel As Integer
    Private currentId As Integer

    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        settingsManager = settingsManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()

        If settingsManager.ConnectToDatabase() Then

            If Not settingsManager.TableExists("SalesReporting") Then

                'settingsManager.RunDatabaseNonQuery("CREATE TABLE SalesReporting (" & _
                '                                                                    "[Id] [int] IDENTITY(1,1) NOT NULL, " & _
                '                                                                    "[TransactionId] [nvarchar](50) NOT NULL, " & _
                '                                                                    "[Date] [datetime] NOT NULL, " & _
                '                                                                    "[ProductId] [int] NOT NULL, " & _
                '                                                                    "[ProductName] [nvarchar](255) NOT NULL, " & _
                '                                                                    "[Barcode] [nvarchar](50) NOT NULL, " & _
                '                                                                    "[Price] [int] NOT NULL, " & _
                '                                                                    "[StockLevel] [int] NOT NULL, " & _
                '                                                                    "[Status] [int] NOT NULL " & _
                '                                                                    ") ON [PRIMARY] ")
                settingsManager.RunDatabaseNonQuery("CREATE TABLE SalesReporting ( " & _
                                                                                     "[Id] [int] IDENTITY(1,1) NOT NULL, " & _
                                                                                     "[TransactionId] [nvarchar](50) NOT NULL, " & _
                                                                                     "[Date] [datetime] NOT NULL, " & _
                                                                                     "[ProductId] [int] NOT NULL, " & _
                                                                                     "[PositionId] [int] NULL, " & _
                                                                                     "[ProductName] [nvarchar](255) NOT NULL, " & _
                                                                                     "[Barcode] [nvarchar](50) NOT NULL, " & _
                                                                                     "[Price] [int] NOT NULL, " & _
                                                                                     "[StockLevel] [int] NOT NULL, " & _
                                                                                     "[Status] [int] NOT NULL " & _
                                                                                    ") ON [PRIMARY] ")
            End If

            settingsManager.DisconnectFromDatabase()

        End If

    End Sub

    ' Reset
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Reset()

        transactionId = "TXN"
        productId = 0
        positionId = 0
        productPrice = 0
        stockLevel = 0
        productName = ""
        currentId = -1


    End Sub

    ' SetTransactionId, SetProductId, SetStockLevel & SetProductPrice
    ' set reportable aspects of this transaction.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetTransactionId(ByVal newTransactionId As String)
        transactionId = newTransactionId
    End Sub

    Public Sub SetProductId(ByVal newProductId As Integer)
        productId = newProductId
    End Sub

    Public Sub SetPositionId(ByVal newPositionId As Integer)
        positionId = newPositionId
    End Sub

    Public Sub SetProductName(ByVal newProductName As String)

        If newProductName.Length > 49 Then
            productName = newProductName.Substring(0, 49)
        Else
            productName = newProductName
        End If

    End Sub

    Public Sub SetProductPrice(ByVal newProductPrice As Integer)
        productPrice = newProductPrice
    End Sub

    Public Sub SetStockLevel(ByVal newStockLevel As Integer)
        stockLevel = newStockLevel
    End Sub

    Public Sub SetBarcode(ByVal newBarcode As String)
        productBarcode = newBarcode
    End Sub

    ' CommitToDatabase
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    'Public Function CommitToDatabase() As Boolean

    '    Dim commitalResult As Boolean = False
    '    Dim insertCommand As String = ""
    '    Dim resultSet As SqlDataReader

    '    Try
    '        If settingsManager.ConnectToDatabase() Then

    '            'insertCommand = "insert into salesreporting (transactionid, date, productid, positionid, productname, barcode, price, stocklevel, status) values ('" & _
    '            '                                transactionId & "', getdate(), " & productId & ", " & positionId & ", '" & productName & "', '" & productBarcode & "', " & productPrice & ", " & stockLevel & ", 0)"
    '            insertCommand = "insert into salesreporting (transactionid, date, productid, productname, barcode, price, stocklevel, status) values ('" & _
    '                                              transactionId & "', getdate(), " & productId & ", '" & productName & "', '" & productBarcode & "', " & productPrice & ", " & stockLevel & ", 0)"

    '            If settingsManager.RunDatabaseNonQuery(insertCommand) Then

    '                Sleep(100)
    '                resultSet = settingsManager.RunDatabaseQuery("select max(id) as maximumId from salesreporting")

    '                If Not resultSet Is Nothing Then

    '                    If resultSet.Read() Then
    '                        currentId = resultSet("maximumId")
    '                    End If

    '                    settingsManager.CloseQuery(resultSet)

    '                End If
    '                commitalResult = True
    '            End If


    '            settingsManager.DisconnectFromDatabase()
    '        End If


    '    Catch ex As Exception
    '    End Try


    '    Return commitalResult

    'End Function

    Public Sub UpdateCurrentTransactionId(ByVal newTransactionId As String)

        transactionId = newTransactionId

        If currentId = -1 Then

            debugInformation.Progress(fDebugWindow.Level.INF, 9999, "Unable to update transaction id as not recovered", True)
        Else

            debugInformation.Progress(fDebugWindow.Level.INF, 9999, "Updating transaction id.", True)
            If settingsManager.ConnectToDatabase() Then

                settingsManager.RunDatabaseNonQuery("update salesreporting set transactionid='" & transactionId & "' where id = " & currentId)
                settingsManager.DisconnectFromDatabase()

            End If


        End If


    End Sub


End Class
