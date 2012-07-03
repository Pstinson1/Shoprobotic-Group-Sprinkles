'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Public Class uBinItem
    Public Event BinClicked(ByVal Sender As System.Object, ByVal BinID As Integer, ByVal Qty As Integer)
    Public Event BinDoubleClicked(ByVal Sender As System.Object, ByVal BinID As Integer, ByVal Qty As Integer)

    Private mBinItem As Teknovation.ProductBin

    Public ReadOnly Property BinID() As Integer
        Get
            Return mBinItem.PositionID
        End Get
    End Property

    Public Property Qty() As Integer
        Get
            Return mBinItem.StockQty
        End Get
        Set(ByVal value As Integer)
            mBinItem.StockQty = value
            StockLabel.Text = value
        End Set
    End Property

    Public Sub New(ByVal BinData As Teknovation.ProductBin)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        mBinItem = BinData
        PopulateScreeen()

    End Sub

    Private Sub PopulateScreeen()
        Me.SuspendLayout()
        With mBinItem
            KeyvendLabel.Text = .KeyVend
            NameLabel.Text = Mid(.Name, 1, 31)
            '   PreNameLabel.Text = Mid(.PreName, 1, 16)
            '  VendPriceLabel.Text = .Price.ToString("C")
            StockLabel.Text = .StockQty
        End With
        Me.ResumeLayout()
    End Sub

    Public Sub UpdateData(ByVal BinData As Teknovation.ProductBin)
        mBinItem = BinData
        PopulateScreeen()
    End Sub

    Public Sub Selected()
        Me.BackgroundImage = My.Resources.Resources.tileSelected
    End Sub
    Public Sub Deselect()
        Me.BackgroundImage = My.Resources.Resources.tile
    End Sub
    Public Sub ItemStockChanged()
        KeyvendLabel.ForeColor = Color.Green
        NameLabel.ForeColor = Color.Green
        StockLabel.ForeColor = Color.Green
    End Sub


    Private Sub ItemClicked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles Me.Click, KeyvendLabel.Click, _
            NameLabel.Click, StockLabel.Click

        RaiseEvent BinClicked(Me, mBinItem.PositionID, mBinItem.StockQty)
    End Sub
    Private Sub ItemDoubleClicked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
           Handles Me.DoubleClick, KeyvendLabel.DoubleClick, _
           NameLabel.DoubleClick, StockLabel.DoubleClick

        RaiseEvent BinDoubleClicked(Me, mBinItem.PositionID, mBinItem.StockQty)
    End Sub
End Class

