'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Data.SqlClient
Imports System.Threading
Imports DebugWindow

Public Class fMain

    ' managers
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    ' forms
    Private confirmLayout As fConfirmNewLayout = New fConfirmNewLayout

    ' variables
    Private mSelectedBin As uBinItem



    ' fMain_Load
    ' set up the helpers 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub fMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ArgCounter As Integer = 0

        For Each argumentItem As String In My.Application.CommandLineArgs
            Console.Write("Here is argument number " & ArgCounter.ToString & ": " & argumentItem & vbCrLf)


            Select Case argumentItem

                Case "-NS"
                    MinusHoverButton.Visible = False
                    AddHoverButton.Visible = False
                    QtyPanel.Visible = False
                    SetStockHoverButton.Visible = False

            End Select

            ArgCounter += 1

        Next


        debugInformation = debugInformationFactory.GetManager

        ' CheckForNewLayout()
        PopulateTheBins()

    End Sub

    ' PopulateTheBins
    ' clear and rebuild the bin arangement
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PopulateTheBins()

        Me.SuspendLayout()
        Dim ProductBLL As New Teknovation.Data.ProductBinBLL
        Dim mBins As IList(Of Teknovation.ProductBin)

        Try

            mBins = ProductBLL.GetAllBins

            For Each iBin In mBins
                Dim mBinItem As New uBinItem(iBin)

                mBinItem.Location = New System.Drawing.Point((iBin.xPosition * 102) + 6, _
                                                        (iBin.yPosition * 70) + 2)
                '  mBinItem.Location = New System.Drawing.Point((iBin.xPosition * 105) + 0, _
                '                                             (iBin.yPosition * 130) + 0)

                '   AddHandler Me.BinSelected, AddressOf mBinItem.ListenSelected
                AddHandler mBinItem.BinClicked, AddressOf BinClicked
                AddHandler mBinItem.BinDoubleClicked, AddressOf BinDoubleClicked
                Me.Controls.Add(mBinItem)

            Next
            Me.ResumeLayout()
            ProductBLL.Dispose()

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 3000, "PopulateTheBins():" & ex.Message.ToString, True)
        End Try

    End Sub

    Private Sub CheckForNewLayout()

        Dim updateRequired As Boolean
        Dim newLayoutQuery As New cCheckLayout
        Dim newLayoutUpdate As New cUpdateLayout

        Try
            updateRequired = newLayoutQuery.Check()
        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 3002, "CheckForNewLayout():" & ex.Message.ToString, True)

        End Try

        If updateRequired Then

            updateRequired = confirmLayout.Ask()

            If updateRequired Then
                Try
                    newLayoutUpdate.Go()

                Catch ex As Exception
                    debugInformation.Progress(fDebugWindow.Level.ERR, 3003, "CheckForNewLayout():" & ex.Message.ToString, True)
                End Try

            End If
        End If

    End Sub



    Private Sub BinClicked(ByVal Sender As System.Object, ByVal BinID As Integer, ByVal Qty As Integer)
        If TypeOf Sender Is uBinItem Then
            If Not mSelectedBin Is Nothing Then
                mSelectedBin.Deselect()
            End If

            mSelectedBin = Sender

            mSelectedBin.Selected()
            SetStockQuantityDisplayed(mSelectedBin.Qty)
        End If

    End Sub
    Private Sub BinDoubleClicked(ByVal Sender As System.Object, ByVal BinID As Integer, ByVal Qty As Integer)
        If TypeOf Sender Is uBinItem Then
            If Not mSelectedBin Is Nothing Then
                mSelectedBin.Deselect()
            End If

            mSelectedBin = Sender

            mSelectedBin.Selected()

            Dim mQty As Integer = StockQuantityLabel.Text
            Dim mBLL As New Teknovation.Data.ProductBinBLL

            Try
                mBLL.UpdateStock(Sender.BinID, 5)
                Sender.Qty = 5
                mBLL.Dispose()
                SetStockQuantityDisplayed(5)
                Sender.ItemStockChanged()

            Catch ex As Exception
                debugInformation.Progress(fDebugWindow.Level.ERR, 3001, "SetdoubleStockHoverButton():" & ex.Message.ToString, True)
            End Try

        End If

    End Sub

    ' AddHoverButton_Click & MinusHoverButton_Click
    ' increment and decrement the desired stock level.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AddHoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddHoverButton.Click
        If Not mSelectedBin Is Nothing Then
            SetStockQuantityDisplayed(StockQuantityLabel.Text + 1)
        End If

    End Sub

    Private Sub MinusHoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinusHoverButton.Click
        If Not mSelectedBin Is Nothing Then
            If StockQuantityLabel.Text > 0 Then
                SetStockQuantityDisplayed(StockQuantityLabel.Text - 1)
            End If
        End If
    End Sub

    ' SetStockQuantityDisplayed
    ' update the desired stock level box.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetStockQuantityDisplayed(ByVal newStockLevel As Integer)
        StockQuantityLabel.Text = newStockLevel.ToString
    End Sub

    ' ExitButton_Click
    ' close the application
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ExitHoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitHoverButton.Click

        'Dim processStart As ProcessStartInfo = New ProcessStartInfo
        'Dim applicationFolder As String = My.Application.Info.DirectoryPath

        'processStart.UseShellExecute = False
        'processStart.FileName = applicationFolder & "\Vend.exe"
        'processStart.WorkingDirectory = applicationFolder
        'processStart.RedirectStandardOutput = False
        'System.Diagnostics.Process.Start(processStart)

        Application.Exit()
    End Sub

    Private Sub SetStockHoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetStockHoverButton.Click
        'save the stock
        If StockQuantityLabel.Text <> mSelectedBin.Qty Then
            Dim mQty As Integer = StockQuantityLabel.Text
            Dim mBLL As New Teknovation.Data.ProductBinBLL

            Try
                mBLL.UpdateStock(mSelectedBin.BinID, mQty)
                mSelectedBin.Qty = mQty
                mBLL.Dispose()
            Catch ex As Exception
                debugInformation.Progress(fDebugWindow.Level.ERR, 3001, "SetStockHoverButton():" & ex.Message.ToString, True)
            End Try
        End If

    End Sub

End Class
