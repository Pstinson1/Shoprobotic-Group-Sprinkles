'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Data.SqlClient
Imports System.Windows.Forms

Imports DebugWindow

Partial Class fAttractManager

    ' structures
    Structure AvailablePositionStruct

        Dim positionId As Integer
        Dim name As String
        Dim activeWander As Boolean
        Dim sequenceScore As Integer
        Dim preName As String
        Dim xPosition As Integer
        Dim yPosition As Integer

    End Structure

    ' variables
    Private availablePosition() As AvailablePositionStruct
    Private totalPositionCount As Integer
    Private attractPositionIndex As Integer
    Private sequenceMatchFound As Boolean

    ' InitialisePositions
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub InitialisePositions()
        Try

      
        Dim resultSet As SqlDataReader
        Dim positionIndex As Integer
        Dim randomGenerator As New Random()
        Dim randomIndex As Integer
        Dim numbersRequired As Boolean = True

        attractPositionIndex = 0
        totalPositionCount = 0

        debugManager.Progress(fDebugWindow.Level.INF, 2260, "Assessing available positions", True)

        ' extract the product positions for all the active products
        If settingsManager.ConnectToDatabase() Then


            resultSet = settingsManager.RunDatabaseQuery("select ProductPosition.PosId As positionId, Product.ProdName As productName, isnull (Product.PreName, '') As productPreName, " & _
                                                                                            "ProductPosition.xPos As xPosition, ProductPosition.yPos As yPosition, " & _
                                                                                            "ISNULL(ProductPosition.activeWander, 0) As activeWander, ISNULL(ProductPosition.sequenceScore, 9999) As sequenceScore " & _
                                                                                            "from Product inner join ProductPosition on Product.ProdId=ProductPosition.ProdId " & _
                                                                                            "where Product.IsActive=1 and ProductPosition.IsActive=1 Order By SequenceScore")

            If Not resultSet Is Nothing Then

                While resultSet.Read()

                    ReDim Preserve availablePosition(totalPositionCount)

                    availablePosition(totalPositionCount).positionId = resultSet("positionId")
                    availablePosition(totalPositionCount).name = resultSet("productName")
                    availablePosition(totalPositionCount).preName = resultSet("productPreName")
                    availablePosition(totalPositionCount).xPosition = resultSet("xPosition")
                    availablePosition(totalPositionCount).yPosition = resultSet("yPosition")
                    availablePosition(totalPositionCount).activeWander = resultSet("activeWander")
                    availablePosition(totalPositionCount).sequenceScore = resultSet("sequenceScore")


                    totalPositionCount = totalPositionCount + 1

                End While

                settingsManager.CloseQuery(resultSet)
            End If

            settingsManager.DisconnectFromDatabase()
        End If

        ' swap them randomly
        For randomIndex = 0 To totalPositionCount - 1

            Dim swapPosition As AvailablePositionStruct
            Dim fromIndex As Integer

            fromIndex = randomGenerator.Next(0, totalPositionCount - 1)

            swapPosition = availablePosition(randomIndex)
            availablePosition(randomIndex) = availablePosition(fromIndex)
            availablePosition(fromIndex) = swapPosition
        Next




        '' random seqence score
        'For randomIndex = 0 To totalPositionCount - 1
        '    Dim randomScore = randomGenerator.Next(1, 100000000)
        '    availablePosition(randomIndex).sequenceScore = randomScore
        'Next

        ' insert the products in the list view
        initialProductLoading = True

        For positionIndex = 0 To totalPositionCount - 1

            Dim newProductPosition As New ListViewItem

            newProductPosition.Tag = availablePosition(positionIndex)
            newProductPosition.Checked = availablePosition(positionIndex).activeWander
            newProductPosition.Text = availablePosition(positionIndex).name
            newProductPosition.SubItems.Add(availablePosition(positionIndex).preName)
            newProductPosition.SubItems.Add(availablePosition(positionIndex).sequenceScore)
            newProductPosition.SubItems.Add(availablePosition(positionIndex).xPosition)
            newProductPosition.SubItems.Add(availablePosition(positionIndex).yPosition)

            'The remaining columns are subitems  
            helperFunctions.AddToListView(ProductPositionListView, newProductPosition)

        Next

        '    ProductPositionListView.ListViewItemSorter = New ListViewComparer()
        '    ProductPositionListView.Sort()

        initialProductLoading = False
        Catch ex As Exception
            debugManager.Progress(fDebugWindow.Level.INF, 2260, ex.ToString, True)
        End Try
    End Sub

    Private Sub ProductPositionListView_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles ProductPositionListView.ItemChecked

        Dim selectedListItem As ListViewItem
        Dim selectedItem As AvailablePositionStruct
        Dim positionIndex As Integer

        If Not initialProductLoading Then

            selectedListItem = e.Item
            selectedItem = e.Item.Tag

            If settingsManager.ConnectToDatabase() Then

                '  change the structure
                For positionIndex = 0 To availablePosition.Length - 1

                    If availablePosition(positionIndex).positionId = selectedItem.positionId Then
                        availablePosition(positionIndex).activeWander = selectedListItem.Checked
                    End If
                Next

                settingsManager.RunDatabaseNonQuery("update productposition set activewander='" & selectedListItem.Checked & "' where posid=" & selectedItem.positionId)
                settingsManager.DisconnectFromDatabase()

            End If
        End If

    End Sub

    Private Sub ProductPositionListView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProductPositionListView.SelectedIndexChanged

        Dim selectedItem As AvailablePositionStruct

        If ProductPositionListView.SelectedItems.Count = 0 Then

            helperFunctions.SetTextBoxText(ScoreText, "")
            helperFunctions.SetButtonEnable(ApplyScoreButton, False)

        Else

            selectedItem = ProductPositionListView.SelectedItems(0).Tag
            helperFunctions.SetTextBoxText(ScoreText, selectedItem.sequenceScore.ToString)
            helperFunctions.SetButtonEnable(ApplyScoreButton, True)

        End If

    End Sub

    ' ScoreText_KeyPressed
    ' ensure that this is a decimal
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ScoreText_KeyPressed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ScoreText.KeyPress

        If (e.KeyChar < "0" Or e.KeyChar > "9") And e.KeyChar <> "." And e.KeyChar <> Chr(8) Then
            e.Handled = True
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim nextPosition As New AvailablePositionStruct

        If Not NextAttractPosition(nextPosition) Then
            Progress("FAILED")

        Else
        End If

    End Sub

    Private lastItemPresented As Integer = 0

    ' NextAttractPosition
    ' returns by reference the next position to highlight..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function NextAttractPosition(ByRef nextPosition As AvailablePositionStruct) As Boolean

        Dim usablePositions As Integer = 0
        Dim positionCursor As AvailablePositionStruct
        Dim foundPosition As Boolean = False
        Dim positionIndex As Integer
        Dim currentPos As AvailablePositionStruct

        '      Progress("Next attract position")
        If availablePosition Is Nothing OrElse availablePosition.Length = 0 Then
            Return False
        End If

        '  count the usable positions
        For positionIndex = 0 To availablePosition.Length - 1

            positionCursor = availablePosition(positionIndex)
            If positionCursor.activeWander Then
                usablePositions = usablePositions + 1
            End If

        Next

        ' fail if no available positions
        If usablePositions = 0 Then
            Progress("Non available.")
            Return False

        Else
            '     Progress("Usable Positions" & usablePositions)
        End If

        ' find an item with a lower score than last time
        '      Progress("Last position was" & lastItemPresented)


        '     currentPos = ProductPositionListView.Items(lastItemPresented).Tag
        currentPos = GetListviewItemTag(ProductPositionListView, (lastItemPresented))

        While Not currentPos.activeWander
            lastItemPresented = lastItemPresented + 1
            If lastItemPresented = availablePosition.Length - 1 Then
                Progress("The wander has Looped")
                lastItemPresented = 0
            End If
            currentPos = ProductPositionListView.Items(lastItemPresented).Tag
        End While

        nextPosition = GetListviewItemTag(ProductPositionListView, (lastItemPresented))
        '   nextPosition = ProductPositionListView.Items(lastItemPresented).Tag
        Progress(lastItemPresented & ". " & nextPosition.name & ": " & nextPosition.xPosition & "," & nextPosition.yPosition)

        lastItemPresented = lastItemPresented + 1
        If lastItemPresented = availablePosition.Length - 1 Then
            Progress("The wander has Looped")
            lastItemPresented = 0
        End If

        Return True

    End Function

    ' the canonical form (VB.NET producer)

    Private Delegate Function GetListviewItemTagCallback(ByVal listViewToInspect As ListView, ByVal itemIndex As Integer) As Object    ' defines a delegate type

    Private Function GetListviewItemTag(ByVal listViewToInspect As ListView, ByVal itemIndex As Integer) As Object

        If listViewToInspect.InvokeRequired Then
            Return listViewToInspect.Invoke(New GetListviewItemTagCallback(AddressOf GetListviewItemTag), New Object() {listViewToInspect, itemIndex})
        Else
            Return listViewToInspect.Items(itemIndex).Tag    ' the "functional part", executing only on the main thread
        End If
    End Function


End Class




' Implements a comparer for ListView columns.
'Class ListViewComparer
'    Implements IComparer

'    ' Compare the items in the appropriate column' for objects x and y.
'    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

'        Dim firstItem As ListViewItem = DirectCast(x, ListViewItem)
'        Dim secondItem As ListViewItem = DirectCast(y, ListViewItem)
'        Dim firstText As String
'        Dim secondText As String

'        ' Get the sub-item values.
'        secondText = IIf(firstItem.SubItems.Count <= 2, "", firstItem.SubItems(2).Text)
'        firstText = IIf(secondItem.SubItems.Count <= 2, "", secondItem.SubItems(2).Text)

'        ' Compare them.
'        If IsNumeric(secondText) And IsNumeric(firstText) Then
'            Return Not Val(firstText).CompareTo(Val(secondText))
'        Else
'            Return Not String.Compare(firstText, secondText)
'        End If

'    End Function
'End Class
