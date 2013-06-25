'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

' Import these modules
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Text
Imports DebugWindow
Imports SettingsManager
Imports System.IO

Public Class cDatabaseManager

    ' enumerations
    Enum XmlRequirementStruct

        XML_DEFAULT = 0
        XML_COORD = 1
        XML_WALLPAPER = 2
        XML_HARVEYNICHOLS = 3
        XML_CALVINKLEIN = 4
        XML_SPRINKLES = 5
        XML_EYECANDY = 6
    End Enum

    ' structures
    Public Structure ProductStructure

        ' default, transmit this with all databases
        Dim productId As Long
        Dim authourisationPrice As Integer
        Dim displayPrice As Decimal
        Dim fullPrice As Decimal
        Dim productName As String
        Dim categoryName As String
        Dim shortDescription As String
        Dim namePrefix As String
        Dim keyVend As String
        Dim imageUrl1 As String
        Dim imageUrl2 As String
        Dim imageUrl3 As String
        Dim imageUrl4 As String
        Dim imageUrl5 As String
        Dim imageUrl6 As String
        Dim swatch As String
        Dim taxable As Integer

        ' coord
        Dim xPos As String
        Dim yPos As String

        ' wall paper
        Dim artistName As String
        Dim artistLocation As String
        Dim longDescription As String
        Dim collaborator1 As String
        Dim collaboratorName1 As String
        Dim collaborator2 As String
        Dim collaboratorName2 As String
        Dim collaborator3 As String
        Dim collaboratorName3 As String
        Dim collaborator4 As String
        Dim collaboratorName4 As String
        Dim collaborator5 As String
        Dim collaboratorName5 As String
        Dim collaborator6 As String
        Dim collaboratorName6 As String

        Dim productColour As String
        Dim productSize As String

        ' eye candy

        Dim polar As String
        Dim brand As String
        Dim shape As String
        Dim gender As String
        Dim vto As String
        Dim style As String

    End Structure

    Public Structure PositionStructure

        Dim posId As Integer
        Dim prodId As Integer
        Dim xPos As Integer
        Dim yPos As Integer
        Dim vdo As Integer
        Dim pickAttempts As Integer
        Dim lastVended As Boolean
        Dim fridgeDoor As Integer
        Dim itemsInStock As Integer
    End Structure

    ' constants
    Private Const AVAILABLE_PRODUCTS As Integer = 399
    Private Const AVAILABLE_POSITIONS As Integer = 399

    Public Const NO_DOOR = 0

    ' variables
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private databaseConnection As SqlConnection
    Private activeProduct As ProductStructure
    Private activePosition As PositionStructure
    Private productData(AVAILABLE_PRODUCTS) As ProductStructure
    Private productsTotal As Integer
    Private positionData(AVAILABLE_POSITIONS) As PositionStructure
    Private positionsTotal As Integer

    Private NextPosToVend As Integer
    Private xmlDataStructure As XmlRequirementStruct = XmlRequirementStruct.XML_DEFAULT

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise(ByVal connectionString As String)

        ' acquire the debug window
        debugInformation = debugInformationFactory.GetManager()
        settingsManager = settingsManagerFactory.GetManager()

        databaseConnection = New SqlConnection(connectionString)

        ' ensure that the saved orders folder exists
        If Not Directory.Exists("C:\Control\SavedOrders\") Then _
            Directory.CreateDirectory("C:\Control\SavedOrders\")

    End Sub

    ' Log Important PC Events 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function LogErroredOrder(ByVal errorMessage As String, ByVal payMethod As String, ByVal theDate As Date) As Boolean

        Dim fileStreamObject As FileStream
        Dim fileStreamWriterObject As StreamWriter
        Dim logFileName As String

        logFileName = "ErroredOrder_" & Format(Now.Month, "##00") & "/" & Format(Now.Day, "##00") & "/" & Format(Now.Year, "####0000") & " " & _
                   Format(Now.Hour, "##00") & ":" & Format(Now.Minute, "##00") & ":" & Format(Now.Second, "##00")

        Try
            fileStreamObject = New FileStream("C:\Control\SavedOrders\" & logFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
            fileStreamWriterObject = New StreamWriter(fileStreamObject)
            fileStreamWriterObject.BaseStream.Seek(0, SeekOrigin.End)

            fileStreamWriterObject.WriteLine(theDate & "|" & payMethod & "|" & errorMessage & ControlChars.CrLf)
            fileStreamWriterObject.Flush()

            fileStreamWriterObject.Close()
            fileStreamObject.Close()

            fileStreamObject = Nothing
            fileStreamWriterObject = Nothing

        Catch ex As Exception
        End Try

    End Function

    ' RecoverPositionData
    ' update the display with a new position
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function RecoverStockByProduct(ByVal productId As Integer) As Integer

        Dim resultSet As SqlDataReader
        Dim productCount As Integer = 0

        If settingsManager.ConnectToDatabase() Then

            ' general positioning data
            resultSet = settingsManager.RunDatabaseQuery("select sum(itemsinstock) as productcount from productPosition where prodid=" & productId)

            If Not resultSet Is Nothing Then

                If resultSet.Read() Then
                    productCount = resultSet("productcount")
                End If

                settingsManager.CloseQuery(resultSet)
            End If

            settingsManager.DisconnectFromDatabase()
        End If

        Return productCount

    End Function

    'Public Function RecoverPositionData(ByVal productId As Integer, ByRef currentPosition As PositionStructure) As Boolean

    '    Dim resultSet As SqlDataReader
    '    Dim biggestStockLevel As Integer = -99999
    '    Dim positionsFound As Integer = 0
    '    Dim positionIdentifiers() As Integer = Nothing
    '    Dim positionStockLevel As Integer
    '    Dim mostStockedPosition As Integer = 0

    '    positionsTotal = 0

    '    If settingsManager.ConnectToDatabase() Then

    '        ' build a list of positions for this product
    '        resultSet = settingsManager.RunDatabaseQuery("select posid from productposition where productposition.isactive=1 and prodid=" & productId)

    '        If Not resultSet Is Nothing Then

    '            While resultSet.Read()
    '                ReDim Preserve positionIdentifiers(positionsFound)
    '                positionIdentifiers(positionsFound) = resultSet("PosId")
    '                positionsFound = positionsFound + 1
    '            End While

    '            settingsManager.CloseQuery(resultSet)

    '        End If

    '        ' get the position idetifier of the most stocked position
    '        If positionsFound > 0 Then

    '            For Each postionIdentifier In positionIdentifiers

    '                resultSet = settingsManager.RunDatabaseQuery("select sum(sa.adjustment) as totalstock FROM stockadjustments sa WHERE(sa.PosId = " & postionIdentifier & ") GROUP BY sa.PosId")

    '                If Not resultSet Is Nothing Then

    '                    If resultSet.Read() Then
    '                        positionStockLevel = resultSet("totalstock")

    '                        If positionStockLevel > biggestStockLevel Then
    '                            biggestStockLevel = positionStockLevel
    '                            mostStockedPosition = postionIdentifier
    '                        End If

    '                    End If
    '                    settingsManager.CloseQuery(resultSet)

    '                End If

    '            Next

    '        End If

    '        ' get all the data for the mostr stocked position
    '        resultSet = settingsManager.RunDatabaseQuery("select * from productposition where posid=" & mostStockedPosition)

    '        If Not resultSet Is Nothing Then

    '            If resultSet.HasRows Then

    '                While resultSet.Read()

    '                    positionData(positionsTotal).posId = resultSet("PosId")
    '                    positionData(positionsTotal).prodId = resultSet("ProdId")
    '                    positionData(positionsTotal).xPos = resultSet("xPos")
    '                    positionData(positionsTotal).yPos = resultSet("yPos")
    '                    positionData(positionsTotal).vdo = resultSet("vdo")
    '                    positionData(positionsTotal).lastVended = resultSet("LastVended")
    '                    positionData(positionsTotal).pickAttempts = resultSet("PickAttempts")
    '                    positionData(positionsTotal).fridgeDoor = resultSet("fridgeid")
    '                    positionData(positionsTotal).itemsInStock = resultSet("itemsinstock")

    '                    positionsTotal += 1
    '                End While

    '            End If

    '            settingsManager.CloseQuery(resultSet)
    '        End If

    '        settingsManager.DisconnectFromDatabase()
    '    End If

    '    debugInformation.Progress(fDebugWindow.Level.INF, 1301, "found positions (" & positionsTotal & ")", True)

    '    ' how many positions are there ?
    '    If (positionsTotal > 0) Then

    '        activePosition = positionData(0)

    '    Else

    '        Return False
    '    End If


    '    currentPosition = activePosition

    '    Return True

    'End Function

    Public Function RecoverPositionData(ByVal productId As Integer, ByRef currentPosition As PositionStructure) As Boolean

        Dim resultSet As SqlDataReader
        Dim positionsFound As Integer = 0

        positionsTotal = 0

        If settingsManager.ConnectToDatabase() Then

            ' build a list of positions for this product
            resultSet = settingsManager.RunDatabaseQuery("select top 1 * from productposition where productposition.isactive=1 and itemsinstock> 0 and prodid=" & productId)

            If Not resultSet Is Nothing Then

                While resultSet.Read()

                    activePosition.posId = resultSet("PosId")
                    activePosition.prodId = resultSet("ProdId")
                    activePosition.xPos = resultSet("xPos")
                    activePosition.yPos = resultSet("yPos")
                    activePosition.vdo = resultSet("vdo")
                    activePosition.lastVended = resultSet("LastVended")
                    activePosition.pickAttempts = resultSet("PickAttempts")
                    activePosition.fridgeDoor = resultSet("fridgeid")
                    activePosition.itemsInStock = resultSet("itemsinstock")

                    positionsFound = positionsFound + 1

                End While

                settingsManager.CloseQuery(resultSet)

            End If

            settingsManager.DisconnectFromDatabase()
        End If

        If positionsFound = 0 Then
            Return False
        End If

        debugInformation.Progress(fDebugWindow.Level.INF, 1301, "found positions (" & positionsFound & ")", True)

        currentPosition = activePosition

        Return True
    End Function

    Public Function RecoverBarcodeByProduct(ByVal productId As Integer) As String

        Dim returnBarcode As String = ""
        Dim resultSet As SqlDataReader

        ' get the xml requirement flag
        If settingsManager.ConnectToDatabase Then

            resultSet = settingsManager.RunDatabaseQuery("select isnull (barcode, ' ') as barcode from product where prodid=" & productId)

            If resultSet.HasRows AndAlso resultSet.Read() Then
                returnBarcode = resultSet("barcode")
            End If

            settingsManager.DisconnectFromDatabase()
        End If

        Return returnBarcode

    End Function

    Public Function RecoverProductData(ByVal productId As Integer, ByRef currentProduct As ProductStructure) As Boolean

        Dim productIndex As Integer

        For productIndex = 0 To productsTotal - 1

            If productId = productData(productIndex).productId Then

                activeProduct = productData(productIndex)
                currentProduct = activeProduct

                Return True

            End If

        Next

        Return False

    End Function

    ' GetFullProductList
    ' this is the sub that generates and sends the product data xml to the flash app
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetFullProductList() As String

        Dim xmlContent As New StringBuilder
        Dim xmlSettings As New XmlWriterSettings
        Dim currentItemsCategory As String = ""
        Dim lastItemsCategory As String = ""
        Dim xmlWriter As XmlWriter
        Dim productIndex As Integer = 0
        Dim productList As SqlDataReader = Nothing
        Dim xmlDataStructure As XmlRequirementStruct = XmlRequirementStruct.XML_DEFAULT
        Dim productListCommand As String = ""
        Dim productCategoryImage As String = ""
        Dim productCategoryColour As String = ""
        Dim sellPrice As String = ""
        Dim taxPrice As String = ""
        Dim fullPrice As String = ""
        Dim taxRate As Integer = settingsManager.GetValue("TaxRate")
        Dim currencySymbol As String = settingsManager.GetValue("CurrencySymbol")


        If currencySymbol Is Nothing Then
            currencySymbol = ""
        End If




        ' get the xml requirement flag
        If settingsManager.ConnectToDatabase Then
            xmlDataStructure = settingsManager.GetValue("DatabaseXmlFormat")
            settingsManager.DisconnectFromDatabase()
        End If

        ' what command to we use to get
        Select Case xmlDataStructure

            Case XmlRequirementStruct.XML_DEFAULT

                productListCommand = "SELECT p.ProdId, p.ProdName As 'name', ISNULL(p.preName, '') as 'preName', ISNULL(p.keyVend, '') as 'keyVend', CAST(ROUND(prodPrice,2) as Dec(10,2))price, " & _
                                                        "ISNULL(CAST(ROUND(p.fullPrice,2) as Dec(10,2)), '-1')fullPrice, p.ProdDesc as 'desc', p.ProdImgUrl1 as 'url', " & _
                                                        "ISNULL(p.ProdimgUrl2, 'images/blank.jpg') as 'url2', ISNULL(p.ProdimgUrl3, 'images/blank.jpg') as 'url3', " & _
                                                        "ISNULL(p.ProdimgUrl4, 'images/blank.jpg') as 'url4', pc.ProdCatName as 'ProdCatId', " & _
                                                        "ISNULL(p.xPos, 0) as 'xPos', ISNULL(p.yPos, 0) as 'yPos' " & _
                                                        "FROM Product p " & _
                                                        "INNER JOIN ProductCategories pc on p.ProdCatId = pc.ProdCatId " & _
                                                        "WHERE p.IsActive = 'true' AND Exists (SELECT sa.prodID, Sum(sa.Adjustment)  " & _
                                                        "FROM StockAdjustments sa " & _
                                                        "WHERE(sa.ProdID = p.ProdID) " & _
                                                        "Group By sa.prodId " & _
                                                        "HAVING SUM(sa.Adjustment) > 0) " & _
                                                        "ORDER BY pc.ProdCatId "

            Case XmlRequirementStruct.XML_COORD

                productListCommand = "      SELECT DISTINCT  " & _
                                                        "                    p.ProdId, p.ProdName AS 'name', ISNULL(p.PreName, '') AS 'preName', ISNULL(p.KeyVend, '') AS 'keyVend', CAST(ROUND(p.ProdPrice, 2) AS Dec(10, 2)) AS price, " & _
                                                        "                    ISNULL(CAST(ROUND(p.FullPrice, 2) AS Dec(10, 2)), '-1') AS fullPrice, p.ProdDesc AS 'desc', p.ProdImgUrl1 AS 'url', ISNULL(p.ProdImgUrl2, 'images/blank.jpg') " & _
                                                        "                    AS 'url2', ISNULL(p.ProdImgUrl3, 'images/blank.jpg') AS 'url3', ISNULL(p.ProdImgUrl4, 'images/blank.jpg') AS 'url4', pc.ProdCatName AS 'ProdCatId', ISNULL(p.xPos, 0) " & _
                                                        "                    AS 'xPos', ISNULL(p.yPos, 0) AS 'yPos' " & _
                                                        " FROM         Product AS p INNER JOIN " & _
                                                        "                      ProductCategories AS pc ON p.ProdCatId = pc.ProdCatId INNER JOIN " & _
                                                        "                      ProductPosition ON p.ProdId = ProductPosition.ProdId " & _
                                                        " WHERE     (ProductPosition.IsActive = 1) AND (ProductPosition.ItemsInStock > 0) and p.IsActive = 'true' order by prodcatid"

            Case XmlRequirementStruct.XML_WALLPAPER

                productListCommand = "SELECT p.ProdId as 'ProductId', p.ProdName As 'ProductName', " & _
                                                        "CAST(ROUND(prodPrice,2) as Dec(10,2))price, " & _
                                                        "ISNULL(p.ArtistName, '') as 'ArtistName',  " & _
                                                        "ISNULL(p.ArtistLocation, '') as 'ArtistLocation',  " & _
                                                        "ISNULL(p.ProdDesc, '') as 'ShortDescription',  " & _
                                                        "ISNULL(p.LongDescription, '') as 'LongDescription',  " & _
                                                        "ISNULL(p.ProdimgUrl1, 'images/blank.jpg') as 'url',  " & _
                                                        "ISNULL(p.ProdimgUrl2, 'images/blank.jpg') as 'url2',  " & _
                                                        "ISNULL(p.ProdimgUrl3, 'images/blank.jpg') as 'url3', " & _
                                                        "ISNULL(p.ProdimgUrl4, 'images/blank.jpg') as 'url4',  " & _
                                                        "ISNULL(p.ProdimgUrl5, 'images/blank.jpg') as 'url5',  " & _
                                                        "ISNULL(p.CollaboratorName1, '') as 'CollaboratorName1',  " & _
                                                        "ISNULL(p.Collaborator1, '') as 'Collaborator1',  " & _
                                                        "ISNULL(p.CollaboratorName2, '') as 'CollaboratorName2',  " & _
                                                        "ISNULL(p.Collaborator2, '') as 'Collaborator2',  " & _
                                                        "ISNULL(p.CollaboratorName3, '') as 'CollaboratorName3',  " & _
                                                        "ISNULL(p.Collaborator3, '') as 'Collaborator3',  " & _
                                                        "ISNULL(p.CollaboratorName4, '') as 'CollaboratorName4',  " & _
                                                        "ISNULL(p.Collaborator4, '') as 'Collaborator4',  " & _
                                                        "ISNULL(p.CollaboratorName5, '') as 'CollaboratorName5',  " & _
                                                        "ISNULL(p.Collaborator5, '') as 'Collaborator5',  " & _
                                                        "ISNULL(p.CollaboratorName6, '') as 'CollaboratorName6',  " & _
                                                        "ISNULL(p.Collaborator6, '') as 'Collaborator6',  " & _
                                                        "ISNULL(p.xPos, 0) as 'xPosition', " & _
                                                        "ISNULL(p.yPos, 0) as 'yPosition', " & _
                                                        "ISNULL(pc.ProdCatName, '') as 'ProdCatId', " & _
                                                        "ISNULL(pc.ProdCatImgUrl, '') as 'ProdCatImage' " & _
                                                        "FROM Product p " & _
                                                        "INNER JOIN ProductCategories pc on p.ProdCatId = pc.ProdCatId " & _
                                                        "WHERE p.IsActive = 'true' " & _
                                                        "ORDER BY pc.ProdCatId "

            Case XmlRequirementStruct.XML_HARVEYNICHOLS

                productListCommand = "SELECT p.ProdId as 'ProductId', p.ProdName As 'ProductName', " & _
                                                         "CAST(ROUND(prodPrice,2) as Dec(10,2))price, " & _
                                                         "ISNULL(p.KeyVend, '') as 'productColour',  " & _
                                                         "ISNULL(p.Swatch, '') as 'ProductSwatch',  " & _
                                                         "ISNULL(p.Size, '') as 'ProductSize',  " & _
                                                         "ISNULL(p.PreName, '') as 'ShortDescription',  " & _
                                                         "ISNULL(p.ProdDesc, '') as 'LongDescription',  " & _
                                                         "ISNULL(p.ProdimgUrl1, 'images/blank.jpg') as 'url',  " & _
                                                         "ISNULL(p.ProdimgUrl2, 'images/blank.jpg') as 'url2',  " & _
                                                         "ISNULL(p.ProdimgUrl3, 'images/blank.jpg') as 'url3', " & _
                                                         "ISNULL(p.ProdimgUrl4, 'images/blank.jpg') as 'url4',  " & _
                                                         "ISNULL(p.ProdimgUrl5, 'images/blank.jpg') as 'url5',  " & _
                                                         "ISNULL(p.ProdimgUrl6, 'images/blank.jpg') as 'url6',  " & _
                                                         "ISNULL(pc.ProdCatName, '') as 'ProdCatId', " & _
                                                         "ISNULL(pc.ProdCatImgUrl, '') as 'ProdCatImage', " & _
                                                         "ISNULL(pc.ProdCatColour, '') as 'ProdCatColour' " & _
                                                         "FROM Product p " & _
                                                        "INNER JOIN ProductCategories pc on p.ProdCatId = pc.ProdCatId " & _
                                                        "WHERE p.IsActive = 'true' AND Exists (SELECT sa.prodID, Sum(sa.Adjustment)  " & _
                                                        "FROM StockAdjustments sa " & _
                                                        "WHERE(sa.ProdID = p.ProdID) " & _
                                                        "Group By sa.prodId " & _
                                                        "HAVING SUM(sa.Adjustment) > 0) " & _
                                                        "ORDER BY pc.ProdCatId "

            Case XmlRequirementStruct.XML_CALVINKLEIN

                productListCommand = "SELECT " & _
                                                        "p.prodid as 'productid', " & _
                                                        "p.prodname As 'productname', " & _
                                                        "p.prename As 'prename', " & _
                                                        "p.keyvend As 'keyvend', " & _
                                                        "CAST(ROUND(prodprice,2) as Dec(10,2))price, " & _
                                                        "ISNULL(p.size, '') as 'size', " & _
                                                        "ISNULL(p.proddesc, '') as 'description',  " & _
                                                        "ISNULL(p.prodimgurl1, 'images/blank.jpg') as 'url1',  " & _
                                                        "ISNULL(p.prodimgurl2, 'images/blank.jpg') as 'url2',  " & _
                                                        "ISNULL(p.prodimgurl3, 'images/blank.jpg') as 'url3', " & _
                                                        "ISNULL(p.prodimgurl4, 'images/blank.jpg') as 'url4',  " & _
                                                        "ISNULL(p.prodimgurl5, 'images/blank.jpg') as 'url5',  " & _
                                                        "ISNULL(p.prodimgurl6, 'images/blank.jpg') as 'url6',  " & _
                                                        "ISNULL(p.swatch, '') as 'swatch',  " & _
                                                        "ISNULL(pc.ProdCatName, '') as 'ProdCatId', " & _
                                                        "ISNULL(pc.ProdCatImgUrl, '') as 'ProdCatImage', " & _
                                                        "ISNULL(pc.ProdCatColour, '') as 'ProdCatColour' " & _
                                                        "FROM Product p " & _
                                                        "INNER JOIN ProductCategories pc on p.ProdCatId = pc.ProdCatId " & _
                                                        "WHERE p.IsActive = 'true' AND Exists (SELECT pp.prodID, pp.itemsinstock  " & _
                                                        "FROM productposition pp " & _
                                                        "WHERE(pp.prodID = p.prodid) and (pp.itemsinstock) > 0) " & _
                                                        "ORDER BY pc.prodcatid "
            Case XmlRequirementStruct.XML_SPRINKLES
                productListCommand = "      SELECT DISTINCT  " & _
                                                       "                    p.ProdId, p.ProdName AS 'name', ISNULL(p.PreName, '') AS 'preName', ISNULL(p.KeyVend, '') AS 'keyVend', CAST(ROUND(p.ProdPrice, 2) AS Dec(10, 2)) AS price, " & _
                                                       "                    ISNULL(CAST(ROUND(p.FullPrice, 2) AS Dec(10, 2)), '-1') AS fullPrice, p.ProdDesc AS 'desc', p.ProdImgUrl1 AS 'url', ISNULL(p.ProdImgUrl2, 'images/blank.jpg') " & _
                                                       "                    AS 'url2', ISNULL(p.ProdImgUrl3, 'images/blank.jpg') AS 'url3', ISNULL(p.ProdImgUrl4, 'images/blank.jpg') AS 'url4', pc.ProdCatName AS 'ProdCatId', ISNULL(p.xPos, 0) " & _
                                                       "                    AS 'xPos', ISNULL(p.yPos, 0) AS 'yPos' ,pc.prodcatid,p.SortRank,Taxable" & _
                                                       " FROM         Product AS p INNER JOIN " & _
                                                       "                      ProductCategories AS pc ON p.ProdCatId = pc.ProdCatId INNER JOIN " & _
                                                       "                      ProductPosition ON p.ProdId = ProductPosition.ProdId " & _
                                                       " WHERE     (ProductPosition.IsActive = 1) AND (ProductPosition.ItemsInStock > 0) and p.IsActive = 'true' order by pc.prodcatid,p.SortRank"
            Case XmlRequirementStruct.XML_EYECANDY
                productListCommand = "      SELECT DISTINCT  " & _
                                                       "                    p.ProdId, p.ItemId, p.ProdName AS 'name', ISNULL(p.PreName, '') AS 'preName', " & _
                                                       "                    ISNULL(CAST(ROUND(p.ProdPrice, 2) AS Dec(10, 2)), '-1') AS ProdPrice, p.ProdDesc AS 'desc', p.ProdImgUrl1 AS 'url', ISNULL(p.ProdImgUrl2, '') " & _
                                                       "                    AS 'url2', ISNULL(p.ProdImgUrl3, '') AS 'url3', ISNULL(p.ProdImgUrl4, 'images/blank.jpg') AS 'url4', pc.ProdCatName AS 'ProdCatId', ISNULL(p.xPos, 0) " & _
                                                       "                    AS 'xPos', ISNULL(p.yPos, 0) AS 'yPos', ISNULL(p.Polarised,'N') AS 'Polarised', ISNULL(p.Brand,'') AS 'Brand', ISNULL(p.Shape,'') AS 'Shape', ISNULL(p.Gender,'U') AS 'Gender', ISNULL(p.VTO,'') AS 'VTO', ISNULL(p.Style,'') AS 'Style'  " & _
                                                       " FROM         Product AS p INNER JOIN " & _
                                                       "              ProductCategories AS pc ON p.ProdCatId = pc.ProdCatId INNER JOIN " & _
                                                        "             ProductPosition ON p.ProdId = ProductPosition.ProdId " & _
                                                        " WHERE     (ProductPosition.IsActive = 1) AND (ProductPosition.ItemsInStock > 0) and p.IsActive = 'true'"

        End Select

        ' get ready to count and store the database locally
        productsTotal = 0

        ' create the XML writer
        xmlSettings.OmitXmlDeclaration = True
        xmlSettings.Indent = True
        xmlWriter = xmlWriter.Create(xmlContent, xmlSettings)

        ' beginning writing the xml
        xmlWriter.WriteStartElement("Data")

        ' ensure that the temperature table exists
        If settingsManager.ConnectToDatabase() Then

            productList = settingsManager.RunDatabaseQuery(productListCommand)

            If Not productList Is Nothing Then

                If productList.HasRows Then

                    While productList.Read()

                        Select Case xmlDataStructure

                            Case XmlRequirementStruct.XML_DEFAULT
                                currentItemsCategory = productList("ProdCatId")

                            Case XmlRequirementStruct.XML_COORD, XmlRequirementStruct.XML_SPRINKLES
                                currentItemsCategory = productList("ProdCatId")

                            Case XmlRequirementStruct.XML_WALLPAPER
                                currentItemsCategory = productList("ProdCatId")
                                productCategoryImage = productList("ProdCatImage")

                            Case XmlRequirementStruct.XML_HARVEYNICHOLS
                                currentItemsCategory = productList("ProdCatId")
                                productCategoryImage = productList("ProdCatImage")
                                productCategoryColour = productList("ProdCatColour")

                            Case XmlRequirementStruct.XML_CALVINKLEIN
                                currentItemsCategory = productList("ProdCatId")
                                productCategoryImage = productList("ProdCatImage")
                                productCategoryColour = productList("ProdCatColour")
                            Case XmlRequirementStruct.XML_EYECANDY
                                currentItemsCategory = productList("ProdCatId")

                        End Select

                        ' category change ?
                        If lastItemsCategory = "" OrElse lastItemsCategory <> currentItemsCategory Then

                            If lastItemsCategory <> "" Then
                                xmlWriter.WriteEndElement()
                            End If

                            xmlWriter.WriteStartElement("Category", Nothing)
                            '       xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)

                            Select Case xmlDataStructure

                                Case XmlRequirementStruct.XML_DEFAULT
                                    xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)

                                Case XmlRequirementStruct.XML_COORD, XmlRequirementStruct.XML_SPRINKLES
                                    xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)

                                Case XmlRequirementStruct.XML_WALLPAPER
                                    xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)
                                    xmlWriter.WriteAttributeString("url", productCategoryImage)

                                Case XmlRequirementStruct.XML_HARVEYNICHOLS
                                    xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)
                                    xmlWriter.WriteAttributeString("url", productCategoryImage)
                                    xmlWriter.WriteAttributeString("colour", productCategoryColour)

                                Case XmlRequirementStruct.XML_CALVINKLEIN
                                    xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)
                                    xmlWriter.WriteAttributeString("url", productCategoryImage)
                                    xmlWriter.WriteAttributeString("colour", productCategoryColour)
                                Case XmlRequirementStruct.XML_EYECANDY
                                    xmlWriter.WriteAttributeString("ProdCatId", currentItemsCategory)


                            End Select


                        End If

                        xmlWriter.WriteStartElement("product", Nothing)

                        ' populate the product stucture - database specific.
                        Select Case xmlDataStructure

                            Case XmlRequirementStruct.XML_DEFAULT

                                productData(productsTotal).productId = productList("ItemId")
                                productData(productsTotal).productName = productList("name")
                                productData(productsTotal).namePrefix = productList("preName")
                                productData(productsTotal).displayPrice = productList("price")
                                productData(productsTotal).shortDescription = productList("desc")
                                productData(productsTotal).keyVend = productList("keyVend")
                                productData(productsTotal).fullPrice = productList("fullPrice")
                                productData(productsTotal).imageUrl1 = productList("url")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).categoryName = productList("ProdCatId")

                                productData(productsTotal).authourisationPrice = productData(productsTotal).displayPrice * 100

                            Case XmlRequirementStruct.XML_COORD

                                productData(productsTotal).productId = productList("ProdId")
                                productData(productsTotal).productName = productList("name")
                                productData(productsTotal).namePrefix = productList("preName")
                                productData(productsTotal).displayPrice = productList("price")
                                productData(productsTotal).shortDescription = productList("desc")
                                productData(productsTotal).keyVend = productList("keyVend")
                                productData(productsTotal).fullPrice = productList("fullPrice")
                                productData(productsTotal).imageUrl1 = productList("url")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).categoryName = productList("ProdCatId")
                                productData(productsTotal).xPos = productList("xPos")
                                productData(productsTotal).yPos = productList("yPos")
                                productData(productsTotal).authourisationPrice = productData(productsTotal).displayPrice * 100

                            Case XmlRequirementStruct.XML_WALLPAPER

                                productData(productsTotal).productId = productList("ProductId")
                                productData(productsTotal).productName = productList("ProductName")
                                productData(productsTotal).artistName = productList("ArtistName")
                                productData(productsTotal).artistLocation = productList("ArtistLocation")
                                productData(productsTotal).shortDescription = productList("ShortDescription")
                                productData(productsTotal).longDescription = productList("LongDescription")
                                productData(productsTotal).collaboratorName1 = productList("CollaboratorName1")
                                productData(productsTotal).collaboratorName2 = productList("CollaboratorName2")
                                productData(productsTotal).collaboratorName3 = productList("CollaboratorName3")
                                productData(productsTotal).collaboratorName4 = productList("CollaboratorName4")
                                productData(productsTotal).collaboratorName5 = productList("CollaboratorName5")
                                productData(productsTotal).collaboratorName6 = productList("CollaboratorName6")
                                productData(productsTotal).collaborator1 = productList("Collaborator1")
                                productData(productsTotal).collaborator2 = productList("Collaborator2")
                                productData(productsTotal).collaborator3 = productList("Collaborator3")
                                productData(productsTotal).collaborator4 = productList("Collaborator4")
                                productData(productsTotal).collaborator5 = productList("Collaborator5")
                                productData(productsTotal).collaborator6 = productList("Collaborator6")
                                productData(productsTotal).imageUrl1 = productList("url")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).imageUrl5 = productList("url5")
                                productData(productsTotal).displayPrice = productList("price")
                                productData(productsTotal).xPos = productList("xPosition")
                                productData(productsTotal).yPos = productList("yPosition")

                                productData(productsTotal).authourisationPrice = productData(productsTotal).displayPrice * 100

                            Case XmlRequirementStruct.XML_HARVEYNICHOLS

                                productData(productsTotal).productId = productList("ProductId")
                                productData(productsTotal).productName = productList("ProductName")
                                productData(productsTotal).displayPrice = productList("price")
                                productData(productsTotal).productColour = productList("productColour")
                                productData(productsTotal).productSize = productList("ProductSize")
                                productData(productsTotal).swatch = productList("ProductSwatch")
                                productData(productsTotal).shortDescription = productList("ShortDescription")
                                productData(productsTotal).longDescription = productList("LongDescription")
                                productData(productsTotal).imageUrl1 = productList("url")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).imageUrl5 = productList("url5")
                                productData(productsTotal).imageUrl5 = productList("url6")

                                productData(productsTotal).authourisationPrice = productData(productsTotal).displayPrice * 100

                            Case XmlRequirementStruct.XML_CALVINKLEIN

                                productData(productsTotal).productId = productList("ProductId")
                                productData(productsTotal).productName = productList("ProductName")
                                productData(productsTotal).displayPrice = productList("price")
                                productData(productsTotal).productColour = productList("keyvend")
                                productData(productsTotal).productSize = productList("size")
                                productData(productsTotal).shortDescription = productList("prename")
                                productData(productsTotal).longDescription = productList("description")
                                productData(productsTotal).imageUrl1 = productList("url1")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).imageUrl5 = productList("url5")
                                productData(productsTotal).imageUrl5 = productList("url6")
                                productData(productsTotal).swatch = productList("swatch")

                                productData(productsTotal).authourisationPrice = productData(productsTotal).displayPrice * 100
                            Case XmlRequirementStruct.XML_SPRINKLES

                                productData(productsTotal).productId = productList("ProdId")
                                productData(productsTotal).productName = productList("name")
                                productData(productsTotal).namePrefix = productList("preName")
                                productData(productsTotal).displayPrice = productList("price")
                                productData(productsTotal).shortDescription = productList("desc")
                                productData(productsTotal).keyVend = productList("keyVend")
                                productData(productsTotal).fullPrice = productList("fullPrice")
                                productData(productsTotal).imageUrl1 = productList("url")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).categoryName = productList("ProdCatId")
                                productData(productsTotal).xPos = productList("xPos")
                                productData(productsTotal).yPos = productList("yPos")
                                productData(productsTotal).authourisationPrice = productData(productsTotal).displayPrice * 100
                                productData(productsTotal).taxable = productList("Taxable")

                            Case XmlRequirementStruct.XML_EYECANDY

                                productData(productsTotal).productId = productList("ItemId")
                                productData(productsTotal).polar = productList("Polarised")
                                productData(productsTotal).brand = productList("Brand")
                                productData(productsTotal).shape = productList("Shape")
                                productData(productsTotal).gender = productList("Gender")
                                productData(productsTotal).vto = productList("VTO")
                                productData(productsTotal).namePrefix = productList("preName")
                                productData(productsTotal).productName = productList("name")
                                productData(productsTotal).displayPrice = productList("ProdPrice")
                                productData(productsTotal).shortDescription = productList("desc")
                                productData(productsTotal).style = productList("Style")
                                productData(productsTotal).imageUrl1 = productList("url")
                                productData(productsTotal).imageUrl2 = productList("url2")
                                productData(productsTotal).imageUrl3 = productList("url3")
                                productData(productsTotal).imageUrl4 = productList("url4")
                                productData(productsTotal).categoryName = productList("ProdCatId")
                                productData(productsTotal).xPos = productList("xPos")
                                productData(productsTotal).yPos = productList("yPos")

                        End Select

                        ' write the XML - database specific.
                        Select Case xmlDataStructure

                            Case XmlRequirementStruct.XML_DEFAULT

                                ' write the XML
                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("name", productData(productsTotal).productName)
                                xmlWriter.WriteElementString("preName", productData(productsTotal).namePrefix)
                                xmlWriter.WriteElementString("keyVend", productData(productsTotal).keyVend)
                                xmlWriter.WriteElementString("price", productData(productsTotal).displayPrice)
                                xmlWriter.WriteElementString("fullPrice", productData(productsTotal).fullPrice)
                                xmlWriter.WriteElementString("desc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)

                            Case XmlRequirementStruct.XML_COORD

                                Dim priceDecimal As Decimal = productData(productsTotal).displayPrice
                                Dim taxDecimal As Decimal = (productData(productsTotal).displayPrice * taxRate) / 10000
                                Dim priceString As String = Format(priceDecimal, currencySymbol & "#,##0.00")
                                Dim taxString As String = Format(taxDecimal, currencySymbol & "#,##0.00")
                                Dim totalString As String = Format(priceDecimal + taxDecimal, currencySymbol & "#,##0.00")

                                ' write the XML
                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("name", productData(productsTotal).productName)
                                xmlWriter.WriteElementString("preName", productData(productsTotal).namePrefix)
                                xmlWriter.WriteElementString("keyVend", productData(productsTotal).keyVend)

                                If taxRate = 0 Then


                                    xmlWriter.WriteElementString("price", priceString)
                                    xmlWriter.WriteElementString("fullPrice", priceString)
                                    xmlWriter.WriteElementString("taxPrice", Format(0.0, currencySymbol & "#,##0.00"))

                                Else

                                    xmlWriter.WriteElementString("price", priceString)
                                    xmlWriter.WriteElementString("fullPrice", totalString)
                                    xmlWriter.WriteElementString("taxPrice", taxString)

                                End If


                                xmlWriter.WriteElementString("desc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)
                                xmlWriter.WriteElementString("xPos", productData(productsTotal).xPos)
                                xmlWriter.WriteElementString("yPos", productData(productsTotal).yPos)

                            Case XmlRequirementStruct.XML_WALLPAPER

                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("productName", productData(productsTotal).productName)
                                xmlWriter.WriteElementString("artistName", productData(productsTotal).artistName)
                                xmlWriter.WriteElementString("location", productData(productsTotal).artistLocation)
                                xmlWriter.WriteElementString("price", productData(productsTotal).displayPrice)
                                xmlWriter.WriteElementString("desc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("collName1", productData(productsTotal).collaboratorName1)
                                xmlWriter.WriteElementString("collName2", productData(productsTotal).collaboratorName2)
                                xmlWriter.WriteElementString("collName3", productData(productsTotal).collaboratorName3)
                                xmlWriter.WriteElementString("collName4", productData(productsTotal).collaboratorName4)
                                xmlWriter.WriteElementString("collName5", productData(productsTotal).collaboratorName5)
                                xmlWriter.WriteElementString("collName6", productData(productsTotal).collaboratorName6)
                                xmlWriter.WriteElementString("coll1", productData(productsTotal).collaborator1)
                                xmlWriter.WriteElementString("coll2", productData(productsTotal).collaborator2)
                                xmlWriter.WriteElementString("coll3", productData(productsTotal).collaborator3)
                                xmlWriter.WriteElementString("coll4", productData(productsTotal).collaborator4)
                                xmlWriter.WriteElementString("coll5", productData(productsTotal).collaborator5)
                                xmlWriter.WriteElementString("coll6", productData(productsTotal).collaborator6)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)
                                xmlWriter.WriteElementString("url5", productData(productsTotal).imageUrl5)
                                xmlWriter.WriteElementString("fullprice", "")

                                xmlWriter.WriteElementString("xPos", productData(productsTotal).xPos)
                                xmlWriter.WriteElementString("yPos", productData(productsTotal).yPos)

                            Case XmlRequirementStruct.XML_HARVEYNICHOLS

                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("prodName", productData(productsTotal).productName)
                                xmlWriter.WriteElementString("prodDesc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("prodColour", productData(productsTotal).productColour)
                                xmlWriter.WriteElementString("price", productData(productsTotal).displayPrice)
                                xmlWriter.WriteElementString("size", productData(productsTotal).productSize)
                                xmlWriter.WriteElementString("desc", productData(productsTotal).longDescription)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)
                                xmlWriter.WriteElementString("url5", productData(productsTotal).imageUrl5)
                                xmlWriter.WriteElementString("url6", productData(productsTotal).imageUrl6)
                                xmlWriter.WriteElementString("swatch", productData(productsTotal).swatch)

                            Case XmlRequirementStruct.XML_CALVINKLEIN

                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("prodName", productData(productsTotal).productName)
                                xmlWriter.WriteElementString("prodDesc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("prodColour", productData(productsTotal).productColour)
                                xmlWriter.WriteElementString("price", productData(productsTotal).displayPrice)
                                xmlWriter.WriteElementString("size", productData(productsTotal).productSize)
                                xmlWriter.WriteElementString("desc", productData(productsTotal).longDescription)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)
                                xmlWriter.WriteElementString("url5", productData(productsTotal).imageUrl5)
                                xmlWriter.WriteElementString("url6", productData(productsTotal).imageUrl6)
                                xmlWriter.WriteElementString("swatch", productData(productsTotal).swatch)

                            Case XmlRequirementStruct.XML_SPRINKLES

                                Dim priceDecimal As Decimal = productData(productsTotal).displayPrice
                                Dim taxDecimal As Decimal = (productData(productsTotal).displayPrice * taxRate) / 10000
                                Dim priceString As String = Format(priceDecimal, currencySymbol & "#,##0.00")
                                Dim taxString As String = Format(taxDecimal, currencySymbol & "#,##0.00")
                                Dim totalString As String = Format(priceDecimal + taxDecimal, currencySymbol & "#,##0.00")

                                ' write the XML
                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("name", productData(productsTotal).productName)
                                xmlWriter.WriteElementString("preName", productData(productsTotal).namePrefix)
                                xmlWriter.WriteElementString("keyVend", productData(productsTotal).keyVend)

                                If taxRate = 0 Then
                                  
                                    xmlWriter.WriteElementString("price", priceString)
                                    xmlWriter.WriteElementString("fullPrice", productData(productsTotal).fullPrice)
                                    xmlWriter.WriteElementString("taxPrice", "0.00")

                                Else
                                    If productData(productsTotal).taxable = 1 Then
                                        xmlWriter.WriteElementString("price", priceString)
                                        xmlWriter.WriteElementString("fullPrice", totalString)
                                        xmlWriter.WriteElementString("taxPrice", taxString)
                                    Else
                                        xmlWriter.WriteElementString("price", priceString)
                                        xmlWriter.WriteElementString("fullPrice", priceString)
                                        xmlWriter.WriteElementString("taxPrice", "0.00")
                                    End If


                                End If


                                xmlWriter.WriteElementString("desc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)
                                xmlWriter.WriteElementString("xPos", productData(productsTotal).xPos)
                                xmlWriter.WriteElementString("yPos", productData(productsTotal).yPos)

                            Case XmlRequirementStruct.XML_EYECANDY

                                Dim priceDecimal As Decimal = productData(productsTotal).displayPrice
                                Dim taxDecimal As Decimal = (productData(productsTotal).displayPrice * taxRate) / 10000
                                Dim priceString As String = Format(priceDecimal - taxDecimal, currencySymbol & "#,##0.00")
                                Dim taxString As String = Format(taxDecimal, currencySymbol & "#,##0.00")
                                Dim totalString As String = Format(priceDecimal, currencySymbol & "#,##0.00")

                                ' write the XML
                                xmlWriter.WriteAttributeString("ProdId", productData(productsTotal).productId)
                                xmlWriter.WriteElementString("polar", productData(productsTotal).polar)
                                xmlWriter.WriteElementString("brand", productData(productsTotal).brand)
                                xmlWriter.WriteElementString("shape", productData(productsTotal).shape)
                                xmlWriter.WriteElementString("gender", productData(productsTotal).gender)
                                xmlWriter.WriteElementString("vto", productData(productsTotal).vto)
                                xmlWriter.WriteElementString("preName", productData(productsTotal).namePrefix)
                                xmlWriter.WriteElementString("name", productData(productsTotal).productName)

                                If taxRate = 0 Then
                                    xmlWriter.WriteElementString("price", priceString)
                                    xmlWriter.WriteElementString("taxPrice", "0.00")
                                    xmlWriter.WriteElementString("fullPrice", productData(productsTotal).fullPrice)
                                Else
                                    xmlWriter.WriteElementString("price", priceString)
                                    xmlWriter.WriteElementString("taxPrice", taxString)
                                    xmlWriter.WriteElementString("fullPrice", totalString)
                                End If

                                xmlWriter.WriteElementString("desc", productData(productsTotal).shortDescription)
                                xmlWriter.WriteElementString("style", productData(productsTotal).style)
                                xmlWriter.WriteElementString("url", productData(productsTotal).imageUrl1)
                                xmlWriter.WriteElementString("url2", productData(productsTotal).imageUrl2)
                                xmlWriter.WriteElementString("url3", productData(productsTotal).imageUrl3)
                                xmlWriter.WriteElementString("url4", productData(productsTotal).imageUrl4)
                                xmlWriter.WriteElementString("xPos", productData(productsTotal).xPos)
                                xmlWriter.WriteElementString("yPos", productData(productsTotal).yPos)

                        End Select

                        ' next product
                        xmlWriter.WriteEndElement()
                        productsTotal += 1
                        lastItemsCategory = currentItemsCategory
                    End While

                End If
                xmlWriter.WriteEndElement()
                xmlWriter.WriteEndElement()

                settingsManager.CloseQuery(productList)
            End If
            settingsManager.DisconnectFromDatabase()
        End If

        xmlWriter.Flush()

        SaveTextToFile(xmlContent.ToString(), My.Application.Info.DirectoryPath & "\Transmitted.xml")

        Return xmlContent.ToString

    End Function

    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try


            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message

        End Try
        Return bAns
    End Function

End Class

' Class cDatabaseManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cDatabaseManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared databaseManager As cDatabaseManager = Nothing

    Public Function GetManager() As cDatabaseManager

        If IsNothing(databaseManager) Then
            databaseManager = New cDatabaseManager
            '     databaseManager.Initialise()
        End If

        Return databaseManager

    End Function

End Class
