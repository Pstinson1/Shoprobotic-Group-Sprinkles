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
Imports System.Reflection
'Imports DebugWindow


Public Class uSettingsSection

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Enum ObjectType

        Integer_Type
        Boolean_Type
        Size_Type
        Point_Type
        String_Type
        Double_Type

    End Enum

    ' Structures
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Structure SettingStructure

        Dim settingObject As Object
        Dim settingId As String
        Dim settingType As ObjectType
        Dim settingName As String
        Dim settingExplaination As String
        Dim settingRow As DataGridViewRow

    End Structure

    ' Events
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Event SettingChanges(ByVal rowToUpdate As SettingStructure, ByVal newValue As String)
    Public Event SelectionChanges(ByVal newValue As String)

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private previousValue As String = ""

    ' Managers
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' AddSetting
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function AddSetting(ByVal settingId As String, ByVal settingType As String, ByVal settingName As String, ByVal settingValue As String, ByVal settingExplaination As String) As SettingStructure

        Dim newSettingRow As DataGridViewRow
        Dim newSettingObject As SettingStructure

        '     debugInformation = debugInformationFactory.GetManager

        ' create the new row on the data view
        newSettingRow = New DataGridViewRow

        newSettingRow.CreateCells(SettingsGrid)
        newSettingRow.Cells(0).Value = settingName
        newSettingRow.Cells(1).Value = settingValue

        SettingsGrid.Rows.Add(newSettingRow)

        ' create a new settings object
        newSettingObject = New SettingStructure

        ' assign the row and the name to the object
        newSettingObject.settingId = settingId
        newSettingObject.settingRow = newSettingRow
        newSettingObject.settingName = settingName
        newSettingObject.settingExplaination = settingExplaination
        newSettingObject.settingId = settingId

        ' different types require different objects
        Select Case settingType.ToLower

            Case "size"
                newSettingObject.settingObject = ConvertToSize(settingValue)
                newSettingObject.settingType = ObjectType.Size_Type

            Case "point"
                newSettingObject.settingObject = ConvertToPoint(settingValue)
                newSettingObject.settingType = ObjectType.Point_Type

            Case "string"
                newSettingObject.settingObject = settingValue
                newSettingObject.settingType = ObjectType.String_Type

            Case "boolean"
                newSettingObject.settingObject = Convert.ToBoolean(settingValue)
                newSettingObject.settingType = ObjectType.Boolean_Type

            Case "integer"
                newSettingObject.settingObject = Convert.ToInt32(settingValue)
                newSettingObject.settingType = ObjectType.Integer_Type

            Case "double"
                newSettingObject.settingObject = Convert.ToDouble(settingValue)
                newSettingObject.settingType = ObjectType.Double_Type

                ' if all else fails assign a string
            Case Else
                newSettingObject.settingObject = settingValue
                newSettingObject.settingType = ObjectType.String_Type

        End Select

        ' tag the row with the new object
        newSettingRow.Tag = newSettingObject

        Return newSettingObject

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

    ' SettingsGrid_CellBeginEdit
    ' only allow the values column to be edited
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SettingsGrid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles SettingsGrid.CellBeginEdit

        Dim selectedObject As SettingStructure

        ' offer an explaiation of this setting
        selectedObject = SettingsGrid.Rows(e.RowIndex).Tag
        RaiseEvent SelectionChanges(selectedObject.settingExplaination)

        ' do we allow this edit ?
        If e.ColumnIndex = 1 Then
            previousValue = SettingsGrid.Rows(e.RowIndex).Cells(1).Value
        Else
            e.Cancel = True
        End If

    End Sub

    ' SettingsGrid_CellEndEdit
    ' the user has possibly changed a setting
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SettingsGrid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles SettingsGrid.CellEndEdit

        Dim changedRow As System.Windows.Forms.DataGridViewRow
        Dim settingObject As SettingStructure
        Dim newSetting As String

        changedRow = SettingsGrid.Rows(e.RowIndex)
        newSetting = changedRow.Cells(1).Value

        ' an nothing entry should be treated as an empty string
        If newSetting Is Nothing Then
            newSetting = ""
        End If

        If newSetting <> previousValue Then

            settingObject = changedRow.Tag

            'If newSetting Is Nothing Then

            '    MsgBox("Settings cannot be empty.", MsgBoxStyle.OkOnly, "Error")
            '    SettingsGrid.Rows(e.RowIndex).Cells(1).Value = previousValue

            'Else

            If ValidateCellValue(settingObject, newSetting.ToLower) Then

                RaiseEvent SettingChanges(settingObject, newSetting)

            Else

                MsgBox("Not a valid setting, cancelled", MsgBoxStyle.OkOnly, "Error")
                SettingsGrid.Rows(e.RowIndex).Cells(1).Value = previousValue

            End If

        End If

    End Sub

    ' ValidateCellValue
    ' validate the value data
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function ValidateCellValue(ByVal objectToValidate As SettingStructure, ByVal proposedValue As String) As Boolean

        Dim commandResult As SqlDataReader = Nothing
        Dim requiredType As String = ""
        Dim validResult As Boolean = False

        Try

            Select Case objectToValidate.settingType

                Case ObjectType.Boolean_Type
                    validResult = proposedValue = "true" Or proposedValue = "false"

                Case ObjectType.Integer_Type
                    validResult = IsNumeric(proposedValue)

                Case ObjectType.Double_Type
                    validResult = IsNumeric(proposedValue)

                Case ObjectType.String_Type
                    validResult = True

                Case ObjectType.Size_Type
                    validResult = IsSizeOrPoint(proposedValue)

                Case ObjectType.Point_Type
                    validResult = IsSizeOrPoint(proposedValue)

            End Select

        Catch ex As Exception
        End Try

        Return validResult

    End Function

    ' IsSize
    ' validate a size or point (they look the same)
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function IsSizeOrPoint(ByVal valueString) As Boolean

        Dim validResult As Boolean = False
        Dim axisChunk() As String

        axisChunk = Split(valueString, ",")

        If axisChunk.Length = 2 And IsNumeric(axisChunk(0)) And IsNumeric(axisChunk(1)) Then
            validResult = True
        End If

        Return validResult

    End Function

End Class
