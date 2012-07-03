'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Drawing.Printing
Imports System.Reflection
Imports System.Drawing

Imports HelperFunctions
Imports DebugWindow

' the serial manager form
' event driven serial IO
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class fSettingsManager

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Structure SettingStructure

        Dim settingObject As Object
        Dim settingType As uSettingsSection.ObjectType
        Dim settingName As String
        Dim settingRow As DataGridViewRow

    End Structure

    ' Managers
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private databaseConnection As SqlConnection
    Private queryCommand As SqlCommand
    Private previousValue As String = ""
    Private settingList() As uSettingsSection.SettingStructure = Nothing
    Private tablesPopulated As Boolean = False


    ' Initialise
    ' connect to the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise(ByVal connectionString As String)

        helperFunctions = helperFunctionsFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()

        databaseConnection = New SqlConnection
        databaseConnection.ConnectionString = connectionString

        queryCommand = New SqlCommand
        queryCommand.Connection = databaseConnection

        If TestConnection() Then
            PopluteTable()

        Else
            debugInformation.Progress(fDebugWindow.Level.ERR, 2000, "UNABLE TO CONNECT TO THE DATABASE:" & connectionString, True)
            MsgBox("UNABLE TO CONNECT TO THE DATABASE")
        End If

    End Sub

    ' TestConnection
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function TestConnection() As Boolean

        Dim testResult As Boolean = False

        If ConnectToDatabase() Then
            testResult = True
            DisconnectFromDatabase()
        End If

        Return testResult

    End Function

    ' MakeSqlSafe
    ' get rid of bad charactors
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function MakeSqlSafe(ByVal sqlString As String, ByVal maximumLength As Integer) As String

        Dim returnString As String = sqlString

        returnString = returnString.Replace("'", "''")
        returnString = returnString.Replace("|", "")
        returnString = returnString.Replace("-", "")

        If returnString.Length >= maximumLength Then
            returnString = returnString.Substring(0, maximumLength)
        End If

        Return returnString

    End Function

    ' PopluteTable
    ' connect to the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PopluteTable()

        Dim commandResult As SqlDataReader = Nothing
        Dim settingId As Integer
        Dim settingName As String
        Dim settingValue As String
        Dim settingExplaination As String
        Dim settingSection As String = ""
        Dim settingType As String
        Dim lastSection As String = ""
        Dim newSettingObject As uSettingsSection.SettingStructure
        Dim currentNumberOfSettings As Integer
        Dim currentSettingsSection As uSettingsSection = Nothing

        If Not tablesPopulated Then

            If ConnectToDatabase() Then

                commandResult = RunDatabaseQuery("select isnull(settings.id, 0) as id, isnull(settings.name, '') as name, isnull(settings.type, 'String') as type, isnull(settings.value, '') as value, isnull(settings.section, '') as section, isnull(settings.explaination, '') as explaination from settings order by section, name")

                If Not commandResult Is Nothing Then

                    If commandResult.HasRows Then

                        While commandResult.Read

                            ' do we need to assign a new tab for this setting, add a handler for the control
                            settingSection = commandResult("section")
                            If (settingSection <> lastSection) Then

                                lastSection = settingSection
                                currentSettingsSection = AddNewSectionTab(settingSection)
                                AddHandler currentSettingsSection.SettingChanges, AddressOf Me.SettingChanges
                                AddHandler currentSettingsSection.SelectionChanges, AddressOf Me.SelectionChanges
                            End If

                            ' if no currents section, add a handler for the control
                            If currentSettingsSection Is Nothing Then

                                lastSection = settingSection
                                currentSettingsSection = AddNewSectionTab("UNDETERMINED")
                                AddHandler currentSettingsSection.SettingChanges, AddressOf Me.SettingChanges
                                AddHandler currentSettingsSection.SelectionChanges, AddressOf Me.SelectionChanges
                            End If

                            ' extract the setting fields from the data query
                            settingId = Convert.ToInt32(commandResult("id"))
                            settingName = commandResult("name")
                            settingValue = commandResult("value")
                            settingType = commandResult("type")

                            ' not all the settings require explainations, they may be null
                            Try
                                settingExplaination = commandResult("explaination")
                            Catch ex As Exception
                                settingExplaination = ""
                            End Try


                            ' insert the setting into the current tab
                            newSettingObject = currentSettingsSection.AddSetting(settingId, settingType, settingName, settingValue, settingExplaination)

                            ' if we made a new object container this pass, add it to the list
                            If IsNothing(settingList) Then

                                Array.Resize(settingList, 1)
                                settingList(0) = newSettingObject
                            Else

                                currentNumberOfSettings = settingList.Length
                                Array.Resize(settingList, currentNumberOfSettings + 1)
                                settingList(currentNumberOfSettings) = newSettingObject
                            End If

                        End While

                    End If

                End If

                tablesPopulated = True
                DisconnectFromDatabase()

            End If
        End If

    End Sub

    ' SelectionChanges
    ' when the user clicks on a setting we may need to give a line or two of explaination
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SelectionChanges(ByVal explainationTest As String)

        helperFunctions.SetLabelText(ExplainationLabel, explainationTest)

    End Sub

    ' GetDatabaseDescription
    ' what database are we connected to?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetDatabaseDescription() As String

        Dim databaseDescription As String = ""

        If ConnectToDatabase() Then
            databaseDescription = databaseConnection.Database.ToString()
            DisconnectFromDatabase()
        End If

        Return databaseDescription

    End Function

    ' SettingChanges
    ' we need to update the database..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SettingChanges(ByVal changedSetting As uSettingsSection.SettingStructure, ByVal newValue As String)

        Dim settingIndex As Integer
        Dim returnResult As Boolean = False
        Dim updateInstruction As String

        If Not settingList Is Nothing Then

            ' change the setting on the object
            Select Case changedSetting.settingType

                Case uSettingsSection.ObjectType.Boolean_Type
                    changedSetting.settingObject = Convert.ToBoolean(newValue)

                Case uSettingsSection.ObjectType.Integer_Type
                    changedSetting.settingObject = Convert.ToInt32(newValue)

                Case uSettingsSection.ObjectType.Double_Type
                    changedSetting.settingObject = Convert.ToDouble(newValue)

                Case uSettingsSection.ObjectType.Point_Type
                    changedSetting.settingObject = ConvertToPoint(newValue)

                Case uSettingsSection.ObjectType.Size_Type
                    changedSetting.settingObject = ConvertToSize(newValue)

                Case uSettingsSection.ObjectType.String_Type
                    changedSetting.settingObject = newValue

            End Select

            ' update the object list
            For settingIndex = 0 To settingList.Length - 1

                If settingList(settingIndex).settingId = changedSetting.settingId Then
                    settingList(settingIndex).settingObject = changedSetting.settingObject
                End If

            Next

        End If

        ' ensure that the strings are database safe
        newValue = newValue.Replace("'", "''")

        ' update the database
        updateInstruction = "update settings set value = '" & newValue & "' where id = " & changedSetting.settingId

        If ConnectToDatabase() Then

            RunDatabaseNonQuery(updateInstruction)
            DisconnectFromDatabase()

            returnResult = True
        End If


    End Sub

    ' AddNewTab
    ' create a new comm control tab
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function AddNewSectionTab(ByVal newSectionName As String) As uSettingsSection

        Dim newTabPage As TabPage
        Dim settingSection As New uSettingsSection

        ' create a tab and add a section control to it.
        newTabPage = New TabPage()
        newTabPage.Controls.Add(settingSection)

        ' add the section tab to the  form
        GroupTabControl.TabPages.Add(newTabPage)
        newTabPage.Text = newSectionName

        Return settingSection

    End Function

    ' GetValue
    ' return a value as an object read from the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetValue(ByVal valueName As String, Optional ByRef returnResult As Boolean = False) As Object

        Dim returnObject As Object = Nothing

        returnResult = False

        If Not settingList Is Nothing Then

            For Each settingObject As uSettingsSection.SettingStructure In settingList

                If settingObject.settingName.ToUpper = valueName.ToUpper Then

                    returnObject = settingObject.settingObject
                    returnResult = True
                    Exit For

                End If

            Next


        End If

        Return returnObject

    End Function

    ' GetValue
    ' return a value as an object read from the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function SetValue(ByVal settingName As String, ByVal newValue As Object) As Boolean

        Dim returnResult As Boolean = False
        Dim updateCommand As String = ""
        Dim settingIndex As Integer = 0

        If Not settingList Is Nothing Then

            While settingIndex < settingList.Length

                With settingList(settingIndex)

                    If .settingName.ToUpper = settingName.ToUpper Then

                        returnResult = True

                        ' update the object
                        .settingObject = newValue

                        ' update the row
                        Select Case .settingType

                            Case uSettingsSection.ObjectType.Boolean_Type, uSettingsSection.ObjectType.Integer_Type, uSettingsSection.ObjectType.String_Type, uSettingsSection.ObjectType.Double_Type
                                .settingRow.Cells(1).Value = newValue

                            Case uSettingsSection.ObjectType.Size_Type
                                .settingRow.Cells(1).Value = newValue.Width & "," & newValue.Height

                            Case uSettingsSection.ObjectType.Point_Type
                                .settingRow.Cells(1).Value = newValue.X & "," & newValue.Y

                        End Select

                        ' update the database
                        If ConnectToDatabase() Then

                            Select Case .settingType

                                Case uSettingsSection.ObjectType.Boolean_Type, uSettingsSection.ObjectType.Integer_Type, uSettingsSection.ObjectType.String_Type, uSettingsSection.ObjectType.Double_Type
                                    updateCommand = "update settings set value = '" & newValue & "' where name = '" & settingName & "'"

                                Case uSettingsSection.ObjectType.Size_Type
                                    updateCommand = "update settings set value = '" & newValue.Width & "," & newValue.Height & "' where name = '" & settingName & "'"

                                Case uSettingsSection.ObjectType.Point_Type
                                    updateCommand = "update settings set value = '" & newValue.X & "," & newValue.Y & "' where name = '" & settingName & "'"

                            End Select

                            RunDatabaseNonQuery(updateCommand)
                            DisconnectFromDatabase()
                        End If

                    End If

                End With

                settingIndex += 1
            End While

        End If

        Return returnResult

    End Function

    ' ConvertToSize
    ' converts a string to a size
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ConvertToSize(ByVal valueString) As System.Drawing.Size

        Dim returnSize As New System.Drawing.Size
        Dim axisChunk() As String

        Try
            axisChunk = Split(valueString, ",")

            returnSize.Width = Convert.ToInt32(axisChunk(0))
            returnSize.Height = Convert.ToInt32(axisChunk(1))

        Catch ex As Exception
        End Try

        Return returnSize

    End Function

    ' ConvertToPoint
    ' converts a string to a point
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ConvertToPoint(ByVal valueString) As System.Drawing.Point

        Dim returnPoint As New System.Drawing.Point
        Dim axisChunk() As String

        Try
            axisChunk = Split(valueString, ",")

            returnPoint.X = Convert.ToInt32(axisChunk(0))
            returnPoint.Y = Convert.ToInt32(axisChunk(1))

        Catch ex As Exception
        End Try

        Return returnPoint

    End Function

    ' IsSize
    ' validate a size
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function IsSize(ByVal valueString) As Boolean

        Dim validResult As Boolean = False
        Dim axisChunk() As String

        axisChunk = Split(valueString, ",")

        If axisChunk.Length = 2 And IsNumeric(axisChunk(0)) And IsNumeric(axisChunk(1)) Then
            validResult = True
        End If

        Return validResult

    End Function

    ' myDataGridView_CellEndEdit
    ' validate the value data
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ValidateCellValue(ByVal rowToValidate As System.Windows.Forms.DataGridViewRow) As Boolean

        Dim commandResult As SqlDataReader = Nothing
        Dim requiredType As String = ""
        Dim validResult As Boolean = False
        Dim parameterValue As String = rowToValidate.Cells.Item(1).Value.ToString.ToLower

        Try

            If ConnectToDatabase() Then
                commandResult = RunDatabaseQuery("select type from settings where id = " & rowToValidate.Tag)

                If commandResult.HasRows Then

                    commandResult.Read()
                    requiredType = commandResult("Type")

                End If

                Select Case requiredType

                    Case "Boolean"
                        validResult = parameterValue = "true" Or parameterValue = "false"

                    Case "Integer"
                        validResult = IsNumeric(parameterValue)

                    Case "Double"
                        validResult = IsNumeric(parameterValue)

                    Case "String"
                        validResult = True

                    Case "Size"
                        validResult = IsSize(parameterValue)

                    Case "Point"
                        validResult = IsSize(parameterValue)

                End Select

                DisconnectFromDatabase()

            End If

        Catch ex As Exception
        End Try

        Return validResult

    End Function

    ' UpdateTableValue
    ' update the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateTableValue(ByVal rowToUpdate As System.Windows.Forms.DataGridViewRow)

        Try
            Dim parameterId As String
            Dim parameterName As String
            Dim parameterValue As String

            parameterId = rowToUpdate.Tag
            parameterName = rowToUpdate.Cells.Item(0).Value
            parameterValue = rowToUpdate.Cells.Item(1).Value

            If ConnectToDatabase() Then
                RunDatabaseNonQuery("update settings set value = '" & parameterValue & "' where id = " & parameterId)
                DisconnectFromDatabase()
            End If

        Catch ex As Exception
        End Try

    End Sub

    ' RunDataBaseQuery
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function RunDatabaseQuery(ByVal commandText As String) As SqlDataReader

        Dim commandResult As SqlDataReader = Nothing

        Try
            queryCommand.CommandText = commandText
            commandResult = queryCommand.ExecuteReader()

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 2001, "RunDatabaseQuery failed:" & commandText, True)
            debugInformation.Progress(fDebugWindow.Level.ERR, 2002, ex.Message, True)
        End Try

        Return commandResult

    End Function

    ' RunDataBaseQuery
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub CloseQuery(ByVal existingQuery As SqlDataReader)

        Try
            existingQuery.Close()
        Catch ex As Exception
        End Try

    End Sub

    ' RunDatabaseNonQuery
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function RunDatabaseNonQuery(ByVal commandText As String) As Boolean

        Dim commandResult As Boolean = False

        Try
            queryCommand.CommandText = commandText
            queryCommand.ExecuteNonQuery()

            commandResult = True

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 2003, "RunDatabaseNonQuery failed:" & commandText, True)
            debugInformation.Progress(fDebugWindow.Level.ERR, 2004, ex.Message, True)
        End Try

        Return commandResult

    End Function

    Public Function RunStoredProcedure(ByVal procedureName As String, ByRef parameterList() As SqlParameter)

        Dim sqlCommand As New SqlCommand(procedureName, databaseConnection)
        Dim parameterIndex As Integer = 0

        sqlCommand.CommandType = CommandType.StoredProcedure

        For Each singleParameter As SqlParameter In parameterList
            sqlCommand.Parameters.Add(parameterList(parameterIndex))
            parameterIndex = parameterIndex + 1
        Next
        'While parameterIndex < argumentList.Length

        '    singleParameter = New SqlParameter

        '    singleParameter.Direction = ParameterDirection.Input
        '    singleParameter.ParameterName = argumentList(parameterIndex)
        '    singleParameter.Value = argumentList(parameterIndex + 1)

        '    sqlCommand.Parameters.Add(singleParameter)

        '       sqlCommand.Parameters.Add(

        '    parameterIndex = parameterIndex + 2
        'End While

        Try
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        End Try

        Return True

    End Function


    ' ConnectToDatabase
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function ConnectToDatabase() As Boolean

        Dim connectResult As Boolean = False

        Try
            databaseConnection.Open()
            connectResult = True
        Catch ex As SqlClient.SqlException

        Finally
        End Try

        Return connectResult

    End Function

    ' DisconnectFromDatabase
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub DisconnectFromDatabase()

        Try
            databaseConnection.Close()
        Catch ex As Exception
        End Try

    End Sub

    ' TableExists
    ' does the given tablename exist in the current database ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function TableExists(ByVal tableName As String) As Boolean

        Dim commandResult As SqlDataReader = Nothing
        Dim existsResult As Boolean = False

        Try
            queryCommand.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" & tableName & "') AND type in (N'U')"
            commandResult = queryCommand.ExecuteReader()
            existsResult = commandResult.HasRows
            commandResult.Close()

        Catch ex As Exception
        End Try

        Return existsResult

    End Function


    ' TableColumnExists
    ' does the given tablename exist in the current database ?
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function TableColumnExists(ByVal tableName As String, ByVal columnName As String) As Boolean

        Dim commandResult As SqlDataReader = Nothing
        Dim existsResult As Boolean = False

        Try
            queryCommand.CommandText = "select * from syscolumns where id = object_id ('" & tableName & "') and name = '" & columnName & "'"
            commandResult = queryCommand.ExecuteReader()
            existsResult = commandResult.HasRows
            commandResult.Close()

        Catch ex As Exception
        End Try

        Return existsResult

    End Function





    ' SerialManagerForm_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialManagerForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

    ' PrintButton_Click
    ' print all the settings
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PrintButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintButton.Click

        Dim settingsPrintout As PrintDocument = New PrintDocument

        settingsPrintout.PrintController = New StandardPrintController
        settingsPrintout.DocumentName = "PC settings - " & Now.ToShortTimeString & " " & Now.ToShortDateString

        AddHandler settingsPrintout.PrintPage, AddressOf PrintSettingsPage

        Try
            settingsPrintout.Print()
        Catch ex As Exception
        End Try

    End Sub

    ' PrintSettingsPage - event is raised for each page to be printed.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PrintSettingsPage(ByVal sender As Object, ByVal eventArgs As PrintPageEventArgs)

        Dim yPosition As Integer = 0
        Dim settingsFont = New Font("Tahoma", 8)
        Dim parameterName As String
        Dim parameterValue As String

        PrintSettingsLine("PC Settings " & " " & Now.ToShortDateString & " " & Now.ToShortTimeString, yPosition, eventArgs, settingsFont)
        PrintSettingsLine("", yPosition, eventArgs, settingsFont)

        For settingIndex As Integer = 0 To settingList.Length - 1

            parameterName = ShortenStringWithEllipsis(settingList(settingIndex).settingName, 110, settingsFont)
            parameterValue = settingList(settingIndex).settingObject.ToString

            PrintSettingsLine(parameterName & ControlChars.Tab & parameterValue, yPosition, eventArgs, settingsFont)

        Next

        PrintSettingsLine(" ", yPosition, eventArgs, settingsFont)
        PrintSettingsLine(" ", yPosition, eventArgs, settingsFont)
        PrintSettingsLine(".", yPosition, eventArgs, settingsFont)

        eventArgs.HasMorePages = False

    End Sub

    ' PrintSettingsLine
    ' print an individual line of settings.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PrintSettingsLine(ByVal outputText As String, ByRef yPosition As Integer, ByVal EventArgs As PrintPageEventArgs, ByVal settingsFont As Font)

        Dim xPosition As Integer = 0
        Dim tabIndex As Integer
        Dim tabBlock() As String
        Dim blockSize As New SizeF
        Dim tabStops() As Integer = {0, 120}
        Dim settingsStringFormat = New StringFormat

        tabBlock = outputText.Split(ControlChars.Tab)

        For tabIndex = 0 To tabBlock.Length - 1
            EventArgs.Graphics.DrawString(tabBlock(tabIndex), settingsFont, Brushes.Black, tabStops(tabIndex), yPosition, settingsStringFormat)
        Next

        yPosition = yPosition + settingsFont.GetHeight(EventArgs.Graphics)

    End Sub

    Function ShortenStringWithEllipsis(ByVal targetString As String, ByVal targetWidth As Integer, ByVal targetFont As Drawing.Font) As String

        Dim returnString As String = targetString
        Dim ellipsesRequired As Boolean = False

        Try

            Do While TextRenderer.MeasureText(returnString, targetFont).Width > targetWidth

                ellipsesRequired = True
                returnString = returnString.Substring(0, returnString.Length - 2)

            Loop

            If ellipsesRequired Then
                returnString = returnString & ".."
            End If

        Catch ex As Exception
        End Try

        Return returnString

    End Function

End Class

' class cSettingsManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cSettingsManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared settingsManager As fSettingsManager = Nothing

    Public Function GetManager() As fSettingsManager

        If IsNothing(settingsManager) Then

            settingsManager = New fSettingsManager

        End If

        Return settingsManager

    End Function

End Class


