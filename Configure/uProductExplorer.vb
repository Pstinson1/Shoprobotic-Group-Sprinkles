'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports DebugWindow
Imports SerialManager
Imports System.Data.SqlClient
Imports SettingsManager
Imports HelperFunctions

Public Class uProductExplorer

    ' managers
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' events
    Public Event SelectionChanges(ByVal Tag As String)

    ' delegates
    Public Delegate Sub EnumerationCallback(ByVal currentNode As TreeNode)

    ' variables
    Private currentEnabledState As Boolean = True
    Private lastSelectedCategoryId As Integer = -1

    ' Initialise
    ' instantiale any required manager, etc..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        settingsManager = settingsManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

    End Sub

    ' Populate
    ' clear and repopulate the product tree
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Populate()

        Dim resultSet As SqlDataReader
        Dim categoryNode As TreeNode
        Dim productNode As TreeNode
        Dim positionNode As TreeNode
        Dim nodeParameters() As String
        Dim doorIndex As Integer
        Dim doorIdent As String

        ' categories
        If (settingsManager.ConnectToDatabase()) Then

            resultSet = settingsManager.RunDatabaseQuery("select prodcatid, prodcatname from productcategories")

            If Not resultSet Is Nothing Then

                While resultSet.Read()
                    categoryNode = ProductTree.Nodes.Add(resultSet("prodcatname"))
                    categoryNode.Tag = "C," & resultSet("prodcatid") & ",True"
                    categoryNode.ImageIndex = 1
                    categoryNode.SelectedImageIndex = 1
                End While

            End If
            settingsManager.DisconnectFromDatabase()
        End If

        ' products
        For Each categoryNode In ProductTree.Nodes

            nodeParameters = categoryNode.Tag.Split(",")
            If (settingsManager.ConnectToDatabase()) Then

                resultSet = settingsManager.RunDatabaseQuery("select * from product where prodcatid=" & nodeParameters(1) & " order by prename, prodname")

                If Not resultSet Is Nothing Then

                    While resultSet.Read()


                        productNode = categoryNode.Nodes.Add(resultSet("prodname"))
                        productNode.Tag = "P," & resultSet("prodid") & ",True"

                        Try
                            SetNodeActive(productNode, resultSet("isactive"))

                        Catch ex As Exception
                            SetNodeActive(productNode, False)
                        End Try

                    End While

                End If
                settingsManager.DisconnectFromDatabase()
            End If
        Next

        ' positions
        For Each categoryNode In ProductTree.Nodes
            For Each productNode In categoryNode.Nodes

                nodeParameters = productNode.Tag.Split(",")

                If (settingsManager.ConnectToDatabase()) Then

                    resultSet = settingsManager.RunDatabaseQuery("select * from productposition where prodid=" & nodeParameters(1))

                    If Not resultSet Is Nothing Then

                        While resultSet.Read()

                            doorIndex = resultSet("fridgeid")
                            doorIdent = IIf(doorIndex = -1, "no door", "door " & doorIndex.ToString)

                            positionNode = productNode.Nodes.Add("Position: " & resultSet("ContainerNumber") & "-" & resultSet("xPos") & ", " & resultSet("yPos"))
                            positionNode.Tag = "R," & resultSet("posid") & ",True"

                            SetNodeActive(positionNode, resultSet("isactive"))

                        End While

                    End If
                    settingsManager.DisconnectFromDatabase()
                End If
            Next
        Next

    End Sub

    ' ProductTree_AfterSelect
    ' enable the appropreate add or delete buttons
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProductTree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ProductTree.AfterSelect

        AssessUtilityButtons()
        RaiseEvent SelectionChanges(e.Node.Tag)




        Dim nodeParameters() As String = e.Node.Tag.Split(",")

        If nodeParameters.Length >= 2 Then

            ' select the correct tab on the properties control
            Select Case nodeParameters(0)

                Case "C"
                    helperFunctions.StringToInteger(nodeParameters(1), lastSelectedCategoryId)

                Case "P"

                Case "R"

            End Select






        End If






    End Sub


    Private Sub AssessUtilityButtons()

        Dim nodeParameters() As String = ProductTree.SelectedNode.Tag.Split(",")

        Select Case nodeParameters(0)

            Case "C"
                AddPositionButton.Enabled = False
                DeletePositionButton.Enabled = False
                AddProductButton.Enabled = True
            Case "P"
                AddPositionButton.Enabled = True
                DeletePositionButton.Enabled = False
                AddProductButton.Enabled = False
            Case "R"
                AddPositionButton.Enabled = False
                DeletePositionButton.Enabled = True
                AddProductButton.Enabled = False
        End Select


    End Sub

    ' Enable
    ' ensure the components fit the control
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Enable(ByVal newState As Boolean)

        If newState <> currentEnabledState Then

            ProductTree.Enabled = newState

            If newState Then
                AssessUtilityButtons()

                EnumerateNodes(ProductTree.Nodes, AddressOf RestoreAllNodesCallback)

            Else
                AddPositionButton.Enabled = False
                DeletePositionButton.Enabled = False
                AddProductButton.Enabled = False

                EnumerateNodes(ProductTree.Nodes, AddressOf GreyAllTreeNodesCallback)

            End If



            currentEnabledState = newState

        End If

    End Sub

    Private Sub GreyAllTreeNodesCallback(ByVal currentNode As TreeNode)

        Dim parameterList() As String = currentNode.Tag.Split(",")

        Select Case parameterList(0)

            Case "C"
                currentNode.SelectedImageIndex = 2
            Case "P"
                currentNode.SelectedImageIndex = 8
            Case "R"
                currentNode.SelectedImageIndex = 5

        End Select

        currentNode.ForeColor = Color.Gray
        currentNode.ImageIndex = currentNode.SelectedImageIndex

    End Sub


    Private Sub RestoreAllNodesCallback(ByVal currentNode As TreeNode)

        Dim parameterList() As String = currentNode.Tag.Split(",")

        Select Case parameterList(0)

            Case "C"
                currentNode.SelectedImageIndex = 1

            Case "P"
                currentNode.SelectedImageIndex = IIf(parameterList(2) = "True", 7, 8)

            Case "R"
                currentNode.SelectedImageIndex = IIf(parameterList(2) = "True", 4, 5)
        End Select

        currentNode.ForeColor = IIf(parameterList(2) = "True", Color.Black, Color.Gray)
        currentNode.ImageIndex = currentNode.SelectedImageIndex

    End Sub



    ' EnumerateNodes
    ' tree node list enumetrator
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub EnumerateNodes(ByVal treeRootNodes As TreeNodeCollection, ByVal callbackfunction As EnumerationCallback)

        For Each nodeCursor As TreeNode In treeRootNodes

            callbackfunction.Invoke(nodeCursor)

            If nodeCursor.Nodes.Count > 0 Then
                EnumerateNodes(nodeCursor.Nodes, callbackfunction)
            End If

        Next

    End Sub


    Private Sub uProductExplorer_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged

        DescriptionLabel.Width = Width - 3
        ProductTree.Width = Width - 3

    End Sub

    ' AddNodeButton_Click
    ' add a RackPosition to the current product
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AddNodeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPositionButton.Click

        Dim positionNode As TreeNode
        Dim resultSet As SqlDataReader
        Dim nodeParameters() As String = ProductTree.SelectedNode.Tag.Split(",")
        Dim productId As Integer = Convert.ToInt32(nodeParameters(1))

        If (settingsManager.ConnectToDatabase()) Then

            settingsManager.RunDatabaseNonQuery("insert into productposition (itemsinstock, lastvended, createdate, prodid, xpos, ypos, pickattempts, vdo, restocklevel, isactive, fridgeid,activewander,sequencescore) values (0, 0, getdate()," & productId & ",100,100,3,0,0,'False', -1,'false',100)")

            resultSet = settingsManager.RunDatabaseQuery("select max (posid) as maxid from productposition")

            If resultSet.Read Then

                positionNode = ProductTree.SelectedNode.Nodes.Add("Position: 100, 100")
                positionNode.ImageIndex = 3
                positionNode.SelectedImageIndex = 2


                positionNode.Tag = "R," & resultSet("maxid") & ",True"

            End If

            settingsManager.DisconnectFromDatabase()

        End If

    End Sub

    ' SetCategoryCurrentSelection
    ' change the category for the current selection
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetCategoryCurrentSelection(ByVal newCategory As Integer)

        Dim parameterList() As String
        Dim categoryNode As TreeNode
        Dim selectedProductNode As TreeNode = ProductTree.SelectedNode
        Dim newParentNode As TreeNode = Nothing
        Dim oldParentNode As TreeNode = selectedProductNode.Parent

        For Each categoryNode In ProductTree.Nodes

            parameterList = categoryNode.Tag.Split(",")

            If parameterList.Length >= 2 AndAlso Convert.ToInt32(parameterList(1)) = newCategory Then
                newParentNode = categoryNode
            End If
        Next

        oldParentNode.Nodes.Remove(selectedProductNode)
        newParentNode.Nodes.Add(selectedProductNode)

        ProductTree.SelectedNode = selectedProductNode

    End Sub

    ' RenameCurrentSelection
    ' name and active state
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub RenameCurrentSelection(ByVal changedName As String)

        ProductTree.SelectedNode.Text = changedName

    End Sub

    Sub SetActiveCurrentSelection(ByVal changedState As Boolean)

        SetNodeActive(ProductTree.SelectedNode, changedState)

    End Sub

    ' SetNodeActive
    ' activate/deactivate a node.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub SetNodeActive(ByVal node As TreeNode, ByVal changedState As Boolean)

        Dim currentParameter() As String = node.Tag.Split(",")
        Dim newImageIndex As Integer

        Select Case currentParameter(0)

            Case "C"
                newImageIndex = 1

            Case "P"
                newImageIndex = IIf(changedState, 7, 8)

            Case "R"
                newImageIndex = IIf(changedState, 4, 5)

        End Select

        ' update the tag, the image
        node.Tag = currentParameter(0) & "," & currentParameter(1) & "," & changedState.ToString
        node.ImageIndex = newImageIndex
        node.SelectedImageIndex = newImageIndex
        node.ForeColor = IIf(changedState, Color.Black, Color.Gray)

    End Sub

    ' DeleteNodeButton_Click
    ' remove the currently selected rack position.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DeleteNodeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeletePositionButton.Click

        Dim nodeParameters() As String = ProductTree.SelectedNode.Tag.Split(",")
        Dim operationSucessfull As Boolean

        If (settingsManager.ConnectToDatabase()) Then

            operationSucessfull = settingsManager.RunDatabaseNonQuery("delete from productposition where posid=" & nodeParameters(1))
            settingsManager.DisconnectFromDatabase()
        End If

        If operationSucessfull Then
            ProductTree.SelectedNode.Remove()

        Else
            MsgBox("Stock adjustments have been made on this product position", MsgBoxStyle.OkOnly, "Operation Aborted")

        End If

    End Sub





    Private Sub AddProductButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddProductButton.Click

        Dim insertCommand As String
        Dim parameterList() As String
        Dim resultSet As SqlDataReader
        Dim productNode As TreeNode


        If lastSelectedCategoryId <> -1 Then

            parameterList = (ProductTree.SelectedNode.Tag.ToString.Split(","))

            insertCommand = "insert into product (" & _
                "ProdCatId," & _
                "ProdName," & _
                "preName," & _
                "keyVend," & _
                "ProdPrice," & _
                "ProdDesc," & _
                "ProdImgUrl1," & _
                "IsActive," & _
                "fullPrice," & _
                "ItemId," & _
                "xpos," & _
                "ypos) values (" & _
                lastSelectedCategoryId.ToString & "," & _
                "'Product'," & _
                "'Prename'," & _
                "'UNK_00'," & _
                parameterList(1) & "," & _
                "''," & _
                "''," & _
                "'False'," & _
                "0," & _
                "0," & _
                "0," & _
                "0)"

            If (settingsManager.ConnectToDatabase()) Then

                settingsManager.RunDatabaseNonQuery(insertCommand)
                resultSet = settingsManager.RunDatabaseQuery("select max (prodid) as maxid from product")
                '   resultSet = settingsManager.RunDatabaseQuery("@@identity")
                If resultSet.Read Then

                    productNode = ProductTree.SelectedNode.Nodes.Add("Prename-Product")
                    productNode.ImageIndex = 4
                    productNode.SelectedImageIndex = 5
                    productNode.Tag = "P," & resultSet("maxid") & ",False"

                End If

                settingsManager.DisconnectFromDatabase()

            End If
        End If


    End Sub

    Private Sub uProductExplorer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If (Me.DesignMode) Then Return

    End Sub
End Class
