'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
' uPosition
' deal with the positioning of the head and the storage in the database
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports SettingsManager
Imports DebugWindow
Imports SerialManager
Imports System.Data.SqlClient
Imports System.IO.Path
Imports System.Text

Imports HelperFunctions

Public Class uProduct

    ' api calls
    Declare Auto Function PathRelativePathTo Lib "shlwapi.dll" (ByVal pszPath As StringBuilder, ByVal pszFrom As String, ByVal dwAttrFrom As Integer, ByVal pszTo As String, ByVal dwAttrTo As Integer) As Boolean

    ' manager components
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' variables
    Private imageFolder As String
    Private currentProductId As Integer
    Private dataLoading As Boolean

    ' events
    Public Event NameIdentifierChanges(ByVal newName As String)
    Public Event ActiveStatusChanges(ByVal activeState As String)
    Public Event EnableExplorer(ByVal requiredState As Boolean)
    Public Event CategoryChanges(ByVal newCategory As Integer)

    ' constants
    Const FILE_ATTRIBUTE_DIRECTORY As Integer = &H10
    Private Const MAX_PATH As Long = 260
    Private Const FILE_ATTRIBUTE_NORMAL As Long = &H80

    ' Initialise
    ' tell the settings manager the connectuion string
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        settingsManager = settingsManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

        imageFolder = My.Application.Info.DirectoryPath.ToString & "\images\"

        PopulateTheCategoryBox()

    End Sub

    ' PopulateTheCategoryBox
    ' list all available categoriues in the category combo box
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PopulateTheCategoryBox()

        Dim resultSet As SqlDataReader

        If settingsManager.ConnectToDatabase() Then

            resultSet = settingsManager.RunDatabaseQuery("select * from productcategories")

            If Not resultSet Is Nothing Then

                While resultSet.Read()

                    CategoryCombo.Items.Add(resultSet("prodcatid") & "-" & resultSet("prodcatdesc"))
                End While

            End If

            settingsManager.CloseQuery(resultSet)
            settingsManager.DisconnectFromDatabase()

        End If

    End Sub

    ' SetSelectedCategory
    ' list all available categoriues in the category combo box
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetSelectedCategory(ByVal newCategory As Integer)

        Dim parameterList() As String

        For Each itemInList As Object In CategoryCombo.Items

            parameterList = itemInList.ToString.Split("-")

            If Convert.ToInt32(parameterList(0)) = newCategory Then
                CategoryCombo.SelectedItem = itemInList
            End If

        Next

    End Sub

    ' GetSelectedCategory
    ' list all available categoriues in the category combo box
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function GetSelectedCategory() As Integer

        Dim parameterList() As String

        parameterList = CategoryCombo.SelectedItem.ToString.Split("-")

        If parameterList.Length = 0 Then
            Return -1
        Else
            Return Convert.ToInt32(parameterList(0))
        End If

    End Function

    ' LoadItem
    ' update the properties for the product
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub LoadItem(ByVal itemIndex As Integer)

        Dim resultSet As SqlDataReader

        currentProductId = itemIndex

        ApplyButton.Enabled = False
        CancelButton.Enabled = False
        dataLoading = True

        If settingsManager.ConnectToDatabase() Then

            resultSet = settingsManager.RunDatabaseQuery("select " & _
                                                         "prodname, prename, barcode,proddesc, fullprice, prodprice, keyvend, xpos, ypos, itemid, isactive, prodimgurl1,prodimgurl2, product.prodcatid " & _
                                                         "from product inner join productcategories on product.prodcatid=productcategories.prodcatid where product.prodid=" & itemIndex)

            If Not resultSet Is Nothing Then

                If resultSet.Read() Then

                    Try
                        EnterTextIntoTextBox(NameText, resultSet("prodname"), "")
                        EnterTextIntoTextBox(txtBarcode, resultSet("barcode"), "")
                        EnterTextIntoTextBox(PreNameText, resultSet("prename"), "")
                        EnterTextIntoTextBox(DescriptionText, resultSet("proddesc"), "")
                        EnterTextIntoTextBox(FullPriceText, resultSet("fullprice"), "")
                        EnterTextIntoTextBox(PriceText, resultSet("prodprice"), "")
                        EnterTextIntoTextBox(KeyVendText, resultSet("keyvend"), "")
                        EnterTextIntoTextBox(VisualTextX, resultSet("xpos"), "0")
                        EnterTextIntoTextBox(VisualTextY, resultSet("ypos"), "0")
                        EnterTextIntoTextBox(ReportingIdText, resultSet("itemid"), "")

                        ' fill out the text boxes
                        ActiveCheck.Checked = (resultSet("isactive") = "True")
                        ImageLabel1.Text = resultSet("prodimgurl1")
                        ImageLabel2.Text = resultSet("prodimgurl2")
                        PreviewImage1()
                        PreviewImage2()
                        SetSelectedCategory(resultSet("prodcatid"))
                    Catch ex As Exception
                    End Try
                End If

                settingsManager.CloseQuery(resultSet)

            End If

            resultSet = settingsManager.RunDatabaseQuery("select " & _
                                                         "size " & _
                                                         "from product where prodid=" & itemIndex)

            If Not resultSet Is Nothing Then

                If resultSet.Read() Then

                    Try
                        EnterTextIntoTextBox(SizeText, resultSet("size"), "")

                    Catch ex As Exception
                    End Try
                End If

                settingsManager.CloseQuery(resultSet)

            End If

            settingsManager.DisconnectFromDatabase()
        End If
        categorySelectionHasChanged = False
        dataLoading = False
    End Sub

    Private Sub EnterTextIntoTextBox(ByVal targetTextbox As TextBox, ByVal newValue As Object, ByVal defaultValue As String)

        Try
            targetTextbox.Text = newValue.ToString
        Catch ex As Exception
            targetTextbox.Text = defaultValue
        End Try

    End Sub

    ' PreviewImage
    ' draw the current image
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PreviewImage1()

        Dim aspectRatio As Double
        Dim bitmapImage As Bitmap = ProductImage1.BackgroundImage
        Dim thumbnailWidth As Integer
        Dim thumbnailHeight As Integer

        Try
            ProductImage1.BackgroundImage = System.Drawing.Bitmap.FromFile(imageFolder & ImageLabel1.Text)
        Catch ex As Exception
            ProductImage1.BackgroundImage = Nothing
        End Try

        ' update the picture
        Try
            bitmapImage = ProductImage1.BackgroundImage
            aspectRatio = bitmapImage.Width / bitmapImage.Height

            If aspectRatio > 1 Then                          ' if the image is wider than it is tall, set it's width to be the width of the
                thumbnailWidth = ProductImage1.Width
                thumbnailHeight = Convert.ToInt32(thumbnailWidth / aspectRatio)

            Else                                                          ' if the image is taller than it is wide, set it's height to be the height of the
                thumbnailHeight = ProductImage1.Height
                thumbnailWidth = Convert.ToInt32(thumbnailHeight * aspectRatio)

            End If

            ' check we have valid heights and widths
            If thumbnailHeight > 0 And thumbnailWidth > 0 Then _
                ProductImage1.BackgroundImage = bitmapImage.GetThumbnailImage(thumbnailWidth, thumbnailHeight, Nothing, IntPtr.Zero)

            ' dispose of the bitmap 
            bitmapImage.Dispose()
            ProductImage1.Visible = True
        Catch ex As Exception
            ProductImage1.Visible = False
        End Try

    End Sub
    ' PreviewImage
    ' draw the current image
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PreviewImage2()

        Dim aspectRatio As Double
        Dim bitmapImage As Bitmap = ProductImage1.BackgroundImage
        Dim thumbnailWidth As Integer
        Dim thumbnailHeight As Integer

        Try
            ProductImage2.BackgroundImage = System.Drawing.Bitmap.FromFile(imageFolder & ImageLabel2.Text)
        Catch ex As Exception
            ProductImage2.BackgroundImage = Nothing
        End Try

        ' update the picture
        Try
            bitmapImage = ProductImage2.BackgroundImage
            aspectRatio = bitmapImage.Width / bitmapImage.Height

            If aspectRatio > 1 Then                          ' if the image is wider than it is tall, set it's width to be the width of the
                thumbnailWidth = ProductImage2.Width
                thumbnailHeight = Convert.ToInt32(thumbnailWidth / aspectRatio)

            Else                                                          ' if the image is taller than it is wide, set it's height to be the height of the
                thumbnailHeight = ProductImage2.Height
                thumbnailWidth = Convert.ToInt32(thumbnailHeight * aspectRatio)

            End If

            ' check we have valid heights and widths
            If thumbnailHeight > 0 And thumbnailWidth > 0 Then _
                ProductImage2.BackgroundImage = bitmapImage.GetThumbnailImage(thumbnailWidth, thumbnailHeight, Nothing, IntPtr.Zero)

            ' dispose of the bitmap 
            bitmapImage.Dispose()
            ProductImage2.Visible = True
        Catch ex As Exception
            ProductImage2.Visible = False
        End Try

    End Sub

    ' StoreCurrentProduct
    ' store the details off the screen into the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub StoreCurrentProduct()

        Dim updateCommand As String

        ' ensure that all the boxes have a legal value
        If Not IsNumeric(VisualTextX.Text) Then helperFunctions.SetTextBoxText(VisualTextX, "0")
        If Not IsNumeric(VisualTextY.Text) Then helperFunctions.SetTextBoxText(VisualTextY, "0")
        If Not IsNumeric(ReportingIdText.Text) Then helperFunctions.SetTextBoxText(ReportingIdText, "0")


        If settingsManager.ConnectToDatabase() Then

            updateCommand = "update product set " & _
                                            "xpos=" & VisualTextX.Text & ", " & _
                                            "ypos=" & VisualTextY.Text & ", " & _
                                            "keyvend='" & KeyVendText.Text & "', " & _
                                            "prodimgurl1='" & ImageLabel1.Text & "', " & _
                                            "prodimgurl2='" & ImageLabel2.Text & "', " & _
                                            "prodcatid='" & GetSelectedCategory() & "', " & _
                                            "prename = '" & SqlSafe(PreNameText.Text) & "', " & _
                                            "barcode = '" & SqlSafe(txtBarcode.Text) & "', " & _
                                            "prodname = '" & SqlSafe(NameText.Text) & "', " & _
                                            "isactive='" & ActiveCheck.Checked & "', " & _
                                            "prodprice=" & PriceText.Text & ", " & _
                                            "fullprice=" & IIf(FullPriceText.Text = "", "NULL", FullPriceText.Text) & ", " & _
                                            "itemid=" & ReportingIdText.Text & ", " & _
                                            "proddesc='" & SqlSafe(DescriptionText.Text) & "' where prodid=" & currentProductId

            settingsManager.RunDatabaseNonQuery(updateCommand)



            updateCommand = "update product set " & _
                                             "size='" & SizeText.Text & "' where prodid=" & currentProductId

            settingsManager.RunDatabaseNonQuery(updateCommand)





            settingsManager.DisconnectFromDatabase()

        End If

    End Sub

    ' MakeSqlSafe
    ' double up all the single quotes etc.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Function SqlSafe(ByVal sqlString) As String

        sqlString = Replace(sqlString, "'", "''")
        sqlString = Replace(sqlString, "|", "")
        sqlString = Replace(sqlString, "-", "")

        Return sqlString

    End Function

    ' ProductDetails_TextChanged
    ' an item detail has changed
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProductDetails_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportingIdText.TextChanged, CategoryCombo.SelectedIndexChanged, PreNameText.TextChanged, SizeText.TextChanged, VisualTextX.TextChanged, VisualTextY.TextChanged, KeyVendText.TextChanged, DescriptionText.TextChanged, NameText.TextChanged, FullPriceText.TextChanged, PriceText.TextChanged, ActiveCheck.CheckedChanged, txtBarcode.TextChanged

        If Not dataLoading Then

            ApplyButton.Enabled = True
            CancelButton.Enabled = True

            RaiseEvent EnableExplorer(False)

        End If

    End Sub

    Private categorySelectionHasChanged As Boolean = False

    Private Sub ProductCategory_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoryCombo.SelectedIndexChanged

        If Not dataLoading Then

            ApplyButton.Enabled = True
            CancelButton.Enabled = True

            RaiseEvent EnableExplorer(False)
            categorySelectionHasChanged = True
        End If

    End Sub

    ' Numeric_KeyPressed
    ' ensure that this is a numeric
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub Numeric_KeyPressed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ReportingIdText.KeyPress, VisualTextX.KeyPress, VisualTextY.KeyPress

        If (e.KeyChar < "0" Or e.KeyChar > "9") And e.KeyChar <> Chr(8) Then
            e.Handled = True
        End If

    End Sub

    ' Decimal_KeyPressed
    ' ensure that this is a decimal
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub Decimal_KeyPressed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles PriceText.KeyPress, FullPriceText.KeyPress

        If (e.KeyChar < "0" Or e.KeyChar > "9") And e.KeyChar <> "." And e.KeyChar <> Chr(8) And e.KeyChar <> "-" Then
            e.Handled = True
        End If

    End Sub

    ' StoreCurrentProduct
    ' the user wants to store this product detail
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click

        Dim selectedCategory As Integer = GetSelectedCategory()
        ApplyButton.Enabled = False
        CancelButton.Enabled = False

        StoreCurrentProduct()

        If categorySelectionHasChanged Then
            RaiseEvent CategoryChanges(selectedCategory)
        End If

        RaiseEvent NameIdentifierChanges(PreNameText.Text & "-" & NameText.Text)
        RaiseEvent ActiveStatusChanges(ActiveCheck.Checked)

        RaiseEvent EnableExplorer(True)

    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click

        ApplyButton.Enabled = False
        CancelButton.Enabled = False

        LoadItem(currentProductId)

        RaiseEvent EnableExplorer(True)

    End Sub

    ' ProductImage_Click
    ' the user wants to select a picture
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProductImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProductImage1.Click, ErrorImage1.Click

        SelectFileDialog.Multiselect = False
        SelectFileDialog.InitialDirectory = imageFolder

        If SelectFileDialog.ShowDialog() = DialogResult.OK Then

            ImageLabel1.Text = GetPathRelativePathTo(imageFolder, SelectFileDialog.FileName)
            PreviewImage1()

            ApplyButton.Enabled = True
            CancelButton.Enabled = True

        End If

    End Sub

    ' ProductImage_Click
    ' the user wants to select a picture
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProductImage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProductImage2.Click, ErrorImage2.Click

        SelectFileDialog.Multiselect = False
        SelectFileDialog.InitialDirectory = imageFolder

        If SelectFileDialog.ShowDialog() = DialogResult.OK Then

            ImageLabel2.Text = GetPathRelativePathTo(imageFolder, SelectFileDialog.FileName)
            PreviewImage2()

            ApplyButton.Enabled = True
            CancelButton.Enabled = True

        End If

    End Sub

    ' GetPathRelativePathTo
    ' find the path from on e location to another
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetPathRelativePathTo(ByVal parentPath As String, ByVal childPath As String) As String

        Dim relativePath As New StringBuilder(260)

        PathRelativePathTo(relativePath, parentPath, FILE_ATTRIBUTE_DIRECTORY, childPath, FILE_ATTRIBUTE_NORMAL)

        Return relativePath.ToString

    End Function

End Class
