'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.Threading.Thread
Imports System.Threading
Imports System.IO

Imports DebugWindow
Imports HelperFunctions
Imports SerialManager
Imports SettingsManager

Public Class uVmcSettings

    ' managers
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' variables
    Private managementThread As Thread

    ' enumerations
    Enum GridColumn
        CLM_READ
        CLM_WRITE
    End Enum

    ' events
    Public Event SelectionChanges(ByVal newValue As String)

    ' Initialise
    ' set up the control
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        Dim newSettingRow As DataGridViewRow
        Dim resultSet As SqlClient.SqlDataReader

        ' get the helpers
        helperFunctions = helperFunctionsFactory.GetManager

        ' get the managers
        serialManager = serialManagerFactory.GetManager
        settingsManager = settingsManagerFactory.GetManager

        ' start listenning to serial port messages
        serialManager.AddCallback(AddressOf SerialPortEvent)

        ' populate the settings table
        If settingsManager.ConnectToDatabase() Then

            resultSet = settingsManager.RunDatabaseQuery("select * from vmcsettings order by mnemonic")

            If Not resultSet Is Nothing Then

                While resultSet.Read()

                    newSettingRow = New DataGridViewRow

                    newSettingRow.CreateCells(SettingsDataGrid)
                    newSettingRow.Cells(0).Value = resultSet("description")
                    newSettingRow.Cells(1).Value = resultSet("mnemonic")
                    newSettingRow.Cells(2).Value = resultSet("readvalue")
                    newSettingRow.Cells(3).Value = ""

                    ' not all rows will have an explaination
                    Try
                        newSettingRow.Tag = resultSet("explaination")
                    Catch ex As Exception
                        newSettingRow.Tag = ""
                    End Try

                    SettingsDataGrid.Rows.Add(newSettingRow)

                End While

                settingsManager.CloseQuery(resultSet)

            End If
            settingsManager.DisconnectFromDatabase()

        End If

    End Sub

    ' SerialPortEvent
    ' process incomming vmc data
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = messageContent.Split("=")

            Select Case parameterList(0)

                Case "GETPRM"

                    If parameterList.Length = 2 Then

                        parameterList = parameterList(1).Split(" ")

                        If parameterList.Length >= 2 Then
                            UpdateValueOnGrid(parameterList(0), parameterList(1), GridColumn.CLM_READ)
                            UpdateValueInDatabase(parameterList(0), parameterList(1))
                        End If
                    End If

                Case "SETPRM"

                    If parameterList.Length = 2 Then

                        parameterList = parameterList(1).Split(" ")

                        If parameterList.Length >= 2 Then
                            UpdateValueOnGrid(parameterList(0), parameterList(1), GridColumn.CLM_READ)
                            UpdateValueInDatabase(parameterList(0), parameterList(1))
                        End If
                    End If


                Case Else

            End Select

        End If

    End Sub

    ' SettingsDataGrid_CellBeginEdit
    ' only allow editing in the write column
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SettingsDataGrid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles SettingsDataGrid.CellBeginEdit

        If e.ColumnIndex <> 3 Then
            e.Cancel = True

        Else
        End If

    End Sub

    ' UpdateValueOnGrid
    ' set the value in the grid
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub UpdateValueOnGrid(ByVal paramMnemonic As String, ByVal paramValue As String, ByVal column As GridColumn)

        Dim integerValue As Integer

        For Each singleRow As DataGridViewRow In SettingsDataGrid.Rows

            If singleRow.Cells(1).Value = paramMnemonic Then

                If column = GridColumn.CLM_READ Then

                    helperFunctions.StringToInteger(paramValue, integerValue)
                    singleRow.Cells(2).Value = integerValue.ToString

                ElseIf column = GridColumn.CLM_WRITE Then
                    helperFunctions.StringToInteger(paramValue, integerValue)
                    singleRow.Cells(3).Value = integerValue.ToString

                End If

            End If

        Next

    End Sub

    ' UpdateValueInDatabase
    ' set the value in the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateValueInDatabase(ByVal paramMnemonic As String, ByVal paramValue As String)

        If settingsManager.ConnectToDatabase() Then

            settingsManager.RunDatabaseNonQuery("update vmcsettings set readvalue=" & paramValue & " where mnemonic = '" & paramMnemonic & "'")
            settingsManager.DisconnectFromDatabase()

        End If

    End Sub

    ' ReadButton_Click
    ' read the current settings as on the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadButton.Click

        Dim okayToStartReading As Boolean = False


        If managementThread Is Nothing Then
            okayToStartReading = True

        ElseIf managementThread.ThreadState <> Threading.ThreadState.Running Then
            okayToStartReading = True

        End If

        If okayToStartReading Then

            ' fire up the management thread
            managementThread = New Thread(AddressOf ReadSettingsThread)
            managementThread.Name = "Read Settings"
            managementThread.Priority = ThreadPriority.BelowNormal
            managementThread.Start()

        Else
        End If

    End Sub

    Private Sub ReadSettingsThread()

        Dim paramMnemonic As String
        Dim rowIndex = 0

        helperFunctions.SetProgressBarMaximum(ReadWriteProgressBar, SettingsDataGrid.RowCount)
        helperFunctions.SetProgressBarValue(ReadWriteProgressBar, 0)
        helperFunctions.BringPanelToFront(ReadWriteProgressPanel)
        helperFunctions.SetLabelText(ReadWriteDescriptionLabel, "Reading VMC settings")

        For Each singleRow As DataGridViewRow In SettingsDataGrid.Rows

            paramMnemonic = singleRow.Cells(1).Value
            serialManager.SendMessage("GETPRM " & paramMnemonic)
            Thread.Sleep(20)

            rowIndex = rowIndex + 1
            helperFunctions.SetProgressBarValue(ReadWriteProgressBar, rowIndex)

        Next

        Thread.Sleep(250)
        helperFunctions.SendPanelToBack(ReadWriteProgressPanel)

    End Sub

    ' DefaultButton_Click
    ' recall default values from the database and place them in the grid for writing.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DefaultButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefaultButton.Click

        Dim resultSet As SqlClient.SqlDataReader

        If settingsManager.ConnectToDatabase() Then

            resultSet = settingsManager.RunDatabaseQuery("select * from vmcsettings")

            While resultSet.Read()

                UpdateValueOnGrid(resultSet("mnemonic"), resultSet("defaultvalue"), GridColumn.CLM_WRITE)

            End While

            settingsManager.CloseQuery(resultSet)

        End If
        settingsManager.DisconnectFromDatabase()

    End Sub

    ' WriteButton_Click
    ' write the settings back to the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub WriteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WriteButton.Click

        Dim okayToStartWriting As Boolean = False

        If managementThread Is Nothing Then
            okayToStartWriting = True

        ElseIf managementThread.ThreadState <> Threading.ThreadState.Running Then
            okayToStartWriting = True

        End If

        If okayToStartWriting Then

            ' fire up the management thread
            managementThread = New Thread(AddressOf WriteSettingsThread)
            managementThread.Name = "Write Settings"
            managementThread.Priority = ThreadPriority.BelowNormal
            managementThread.Start()

        End If

    End Sub

    Private Sub WriteSettingsThread()

        Dim paramMnemonic As String
        Dim existingValue As String
        Dim newValue As String
        Dim rowIndex = 0

        helperFunctions.SetProgressBarMaximum(ReadWriteProgressBar, SettingsDataGrid.RowCount)
        helperFunctions.SetProgressBarValue(ReadWriteProgressBar, 0)
        helperFunctions.BringPanelToFront(ReadWriteProgressPanel)
        helperFunctions.SetLabelText(ReadWriteDescriptionLabel, "Writing VMC settings")

        For Each singleRow As DataGridViewRow In SettingsDataGrid.Rows

            paramMnemonic = singleRow.Cells(1).Value
            existingValue = singleRow.Cells(2).Value
            newValue = singleRow.Cells(3).Value

            If newValue <> "" And newValue <> existingValue Then

                serialManager.SendMessage("SETPRM " & paramMnemonic & " " & newValue)
            End If

            rowIndex = rowIndex + 1
            helperFunctions.SetProgressBarValue(ReadWriteProgressBar, rowIndex)
            Thread.Sleep(25)

        Next

        Thread.Sleep(250)
        helperFunctions.SendPanelToBack(ReadWriteProgressPanel)

    End Sub

    ' NumButton_Click
    ' handles all the keypad events
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub NumButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Num0Button.Click, Num1Button.Click, Num2Button.Click, Num3Button.Click, Num4Button.Click, Num5Button.Click, Num6Button.Click, Num7Button.Click, Num8Button.Click, Num9Button.Click, NumDelButton.Click

        Dim existingContent As String = SettingsDataGrid.SelectedCells(0).Value

        If SettingsDataGrid.SelectedCells(0).ColumnIndex = 3 Then

            Select Case sender.Tag

                Case "del"

                    If existingContent.Length > 0 Then
                        SettingsDataGrid.SelectedCells(0).Value = existingContent.Substring(0, existingContent.Length - 1)
                    End If

                Case Else
                    SettingsDataGrid.SelectedCells(0).Value = existingContent & sender.Tag

            End Select

        End If

    End Sub

    ' PrintButton_Click
    ' print the settings
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PrintButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintButton.Click

        Dim settingsPrintout As PrintDocument = New PrintDocument

        settingsPrintout.PrintController = New StandardPrintController
        settingsPrintout.DocumentName = "VMC settings - " & Now.ToShortTimeString & " " & Now.ToShortDateString

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

        PrintSettingsLine("VMC Settings " & " " & Now.ToShortDateString & " " & Now.ToShortTimeString, yPosition, eventArgs, settingsFont)
        PrintSettingsLine("", yPosition, eventArgs, settingsFont)

        For Each singleRow As DataGridViewRow In SettingsDataGrid.Rows
            PrintSettingsLine(singleRow.Cells(0).Value & ControlChars.Tab & singleRow.Cells(1).Value & ControlChars.Tab & singleRow.Cells(2).Value, yPosition, eventArgs, settingsFont)
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
        Dim tabStops() As Integer = {0, 180, 240}
        Dim settingsStringFormat = New StringFormat

        tabBlock = outputText.Split(ControlChars.Tab)

        For tabIndex = 0 To tabBlock.Length - 1

            EventArgs.Graphics.DrawString(tabBlock(tabIndex), settingsFont, Brushes.Black, tabStops(tabIndex), yPosition, settingsStringFormat)
        Next

        yPosition = yPosition + settingsFont.GetHeight(EventArgs.Graphics)

    End Sub

    ' SettingsDataGrid_CellEnter
    ' provide an explaination of this setting
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SettingsDataGrid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles SettingsDataGrid.CellEnter
        RaiseEvent SelectionChanges(SettingsDataGrid.Rows(e.RowIndex).Tag.ToString)
    End Sub

End Class
