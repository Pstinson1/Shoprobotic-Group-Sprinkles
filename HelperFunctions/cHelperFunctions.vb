'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.ServiceProcess

' class cHelperFunctions
' A number of commonally used functions
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cHelperFunctions

 
    ' delagates
    Private Delegate Sub StatusMessageCallback(ByVal statusBar As StatusStrip, ByVal panelIndex As Integer, ByVal messageText As String)
    Private Delegate Sub SetLabelTextCallback(ByVal labelToSet As Label, ByVal newText As String)
    Private Delegate Sub SetTextBoxTextCallback(ByVal textBox As TextBox, ByVal message As String)
    Private Delegate Sub AddToListBoxCallback(ByVal listboxToAddTo As ListBox, ByVal messageText As String)
    Private Delegate Sub ClearListBoxCallback(ByVal listboxToClear As ListBox)
    Private Delegate Sub SetPanelColourCallback(ByVal messageText As Panel, ByVal colour As System.Drawing.Color)
    Private Delegate Sub SetButtonVisibleCallback(ByVal buttonToSet As Button, ByVal newVisibility As Boolean)
    Private Delegate Sub SetButtonEnableCallback(ByVal buttonToEnable As Button, ByVal buttonState As Boolean)
    Private Delegate Sub SetButtonTextCallback(ByVal buttonToChange As Button, ByVal newText As String)
    Private Delegate Sub SetCheckCheckedCallback(ByVal checkBox As CheckBox, ByVal newState As Boolean)
    Private Delegate Sub SetListBoxSelectionCallback(ByVal listboxToChange As ListBox, ByVal newSelection As Integer)
    Private Delegate Sub SetProgressBarValueCallback(ByVal progessBarToSet As ProgressBar, ByVal newValue As Integer)
    Private Delegate Sub SetProgressBarMaximumCallback(ByVal progessBarToSet As ProgressBar, ByVal newMaximum As Integer)
    Private Delegate Sub SendPanelToBackCallback(ByVal progessBarToSet As Panel)
    Private Delegate Sub BringPanelToFrontCallback(ByVal progessBarToSet As Panel)
    Private Delegate Sub SetFormTextCallback(ByVal formToSet As Form, ByVal newText As String)
    Private Delegate Sub SetRadioCheckedCallback(ByVal radioToCheck As RadioButton)
    Private Delegate Sub AddToListViewCallback(ByVal listviewToAddTo As ListView, ByVal newItem As ListViewItem)
    Private Delegate Sub ClearListViewCallback(ByVal listviewToClear As ListView)
    Private Delegate Sub SetTrackbarLevelCallback(ByVal trackbarToSet As TrackBar, ByVal newLevel As Integer)
    Private Delegate Sub SetListViewSelectingByTextCallback(ByVal listViewToSearch As ListView, ByVal searchText As String)
    Private Delegate Sub SetListViewSelectionCallback(ByVal listViewToSelect As ListView, ByVal itemIndex As Integer)
    Private Delegate Sub SetVerticalProgressBarValueCallback(ByVal progessBarToSet As VerticalProgressBar, ByVal newValue As Integer)


    ' StringToInteger & StringToDouble
    ' convert a string to either an integer or a double
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function StringToInteger(ByVal numericString As String, ByRef resultingInteger As Integer) As Boolean

        Dim functionResult As Boolean = False

        Try
            resultingInteger = Convert.ToInt32(numericString)
            functionResult = True
        Catch ex As Exception
            resultingInteger = 0
        End Try

        Return functionResult

    End Function

    Public Function StringToDouble(ByVal numericAsString As String, ByRef resultingDouble As Double)

        Dim functionResult As Boolean = False

        If IsNumeric(numericAsString) Then
            resultingDouble = Convert.ToDouble(numericAsString)
            functionResult = True
        Else
            resultingDouble = 0
        End If

        Return functionResult
    End Function

    Public Function StringToBoolean(ByVal booleanAsString As String, ByRef resultingBoolean As Boolean)

        Dim functionResult As Boolean = False
        Try
            resultingBoolean = Convert.ToBoolean(booleanAsString)
            functionResult = True
        Catch ex As Exception
            resultingBoolean = False
        End Try

        Return functionResult
    End Function

    Public Function HexStringToAscii(ByVal hexString As String, ByVal convertUnprintable As Boolean) As String

        Dim asciiString As String = ""
        Dim hexByteString As String = ""
        Dim hexByteValue As Integer

        If hexString.Length >= 2 Then

            For hexIndex As Integer = 0 To hexString.Length - 2 Step 2

                hexByteString = hexString.Substring(hexIndex, 2)
                hexByteValue = CInt("&H" & hexByteString)

                If convertUnprintable AndAlso hexByteValue < 32 Then

                    asciiString = asciiString & "[" & hexByteString & "]"

                Else
                    asciiString = asciiString & Chr(hexByteValue)

                End If
            Next
        End If

        Return asciiString

    End Function

    Public Function HexStringToInteger(ByVal hexString As String, ByRef hexValue As Integer) As Boolean

        Dim successFlag As Boolean = False

        Try
            hexValue = CInt("&H" & hexString)
            successFlag = True

        Catch ex As Exception
            hexValue = 0
        End Try

        Return successFlag

    End Function

    ' StatusMessage
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function StatusStripMessage(ByVal statusBar As StatusStrip, ByVal panelIndex As Integer, ByVal messageText As String)

        Dim functionResult As Boolean = False

        If statusBar.InvokeRequired Then
            Dim d As New StatusMessageCallback(AddressOf StatusStripMessage)
            statusBar.Invoke(d, New Object() {statusBar, panelIndex, messageText})
        Else
            If statusBar.Items.Count > panelIndex Then
                statusBar.Items(panelIndex).Text = messageText
                functionResult = True
            End If

        End If
        Return functionResult

    End Function

    ' SetFormText
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetFormText(ByVal formToSet As Form, ByVal newText As String)

        If formToSet.InvokeRequired Then

            Dim d As New SetFormTextCallback(AddressOf SetFormText)
            formToSet.BeginInvoke(d, New Object() {formToSet, newText})
        Else
            formToSet.Text = newText
        End If

    End Sub


    ' SetLabelText
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetLabelText(ByVal labelToSet As Label, ByVal newText As String)

        If labelToSet.InvokeRequired Then

            Dim d As New SetLabelTextCallback(AddressOf SetLabelText)
            labelToSet.BeginInvoke(d, New Object() {labelToSet, newText})
        Else
            labelToSet.Text = newText
        End If

    End Sub

    ' SetTrackbarLevel
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetTrackbarLevel(ByVal trackbarToSet As TrackBar, ByVal newLevel As Integer)
        If trackbarToSet.InvokeRequired Then
            Dim d As New SetTrackbarLevelCallback(AddressOf SetTrackbarLevel)
            trackbarToSet.BeginInvoke(d, New Object() {trackbarToSet, newLevel})
        Else
            trackbarToSet.Value = newLevel
        End If
    End Sub

    ' SetListViewSelectingByText
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetListViewSelectingByText(ByVal listViewToSearch As ListView, ByVal searchText As String)

        Dim foundItem As ListViewItem

        If listViewToSearch.InvokeRequired Then
            Dim d As New SetListViewSelectingByTextCallback(AddressOf SetListViewSelectingByText)
            listViewToSearch.BeginInvoke(d, New Object() {listViewToSearch, searchText})

        Else
            foundItem = listViewToSearch.FindItemWithText(searchText)

            If Not foundItem Is Nothing Then

                foundItem.Selected = True
                listViewToSearch.Select()
            End If

        End If

    End Sub

    ' SetListViewSelection
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetListViewSelection(ByVal listViewToSelect As ListView, ByVal itemIndex As Integer)

        If listViewToSelect.InvokeRequired Then
            Dim d As New SetListViewSelectionCallback(AddressOf SetListViewSelection)
            listViewToSelect.BeginInvoke(d, New Object() {listViewToSelect, itemIndex})

        Else
            listViewToSelect.SelectedItems.Clear()

            If itemIndex < listViewToSelect.Items.Count Then
                listViewToSelect.Items(itemIndex).Selected = True
                listViewToSelect.Select()
            End If

        End If

    End Sub



    ' SetTextBoxText
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetTextBoxText(ByVal textBox As TextBox, ByVal messageText As String)

        If textBox.InvokeRequired Then
            Dim d As New SetTextBoxTextCallback(AddressOf SetTextBoxText)
            textBox.Invoke(d, New Object() {textBox, messageText})
        Else
            textBox.Text = messageText
        End If
    End Sub

    ' SetCheckChecked
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetCheckChecked(ByVal checkBox As CheckBox, ByVal newState As Boolean)

        If checkBox.InvokeRequired Then
            Dim d As New SetCheckCheckedCallback(AddressOf SetCheckChecked)
            checkBox.Invoke(d, New Object() {checkBox, newState})
        Else
            checkBox.Checked = newState
        End If
    End Sub



    ' SetPanelColour
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetPanelColour(ByVal panelToColour As Panel, ByVal newColour As System.Drawing.Color)

        If panelToColour.InvokeRequired Then

            Dim d As New SetPanelColourCallback(AddressOf SetPanelColour)
            panelToColour.BeginInvoke(d, New Object() {panelToColour, newColour})

        Else
            panelToColour.BackColor = newColour

        End If

    End Sub

    ' SetButtonVisible
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub SetButtonVisible(ByVal buttonToSet As Button, ByVal newVisibility As Boolean)

        If buttonToSet.InvokeRequired Then
            Dim d As New SetButtonVisibleCallback(AddressOf SetButtonVisible)
            buttonToSet.Invoke(d, New Object() {buttonToSet, newVisibility})
        Else
            buttonToSet.Visible = newVisibility
        End If
    End Sub

    ' AddToListBox
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub AddToListBox(ByVal listboxToAddTo As ListBox, ByVal messageText As String)

        Dim itemIndex As Integer

        If listboxToAddTo.InvokeRequired Then
            Dim d As New AddToListBoxCallback(AddressOf AddToListBox)            ' invoke this message in the correct thread
            listboxToAddTo.Invoke(d, New Object() {listboxToAddTo, messageText})
        Else

            Try
                If listboxToAddTo.Items.Count = 200 Then
                    listboxToAddTo.Items.RemoveAt(0)
                End If

                itemIndex = listboxToAddTo.Items.Add(messageText)                ' add the message to the list box and scrol it.
                listboxToAddTo.TopIndex = itemIndex - 1
            Catch ex As Exception
            End Try
        End If

    End Sub

    ' AddToListView
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub AddToListView(ByVal listviewToAddTo As ListView, ByVal newItem As ListViewItem)

        If listviewToAddTo.InvokeRequired Then
            Dim d As New AddToListViewCallback(AddressOf AddToListView)
            listviewToAddTo.Invoke(d, New Object() {listviewToAddTo, newItem})
        Else
            listviewToAddTo.Items.Add(newItem)
        End If

    End Sub

    ' ClearListView
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ClearListView(ByVal listviewToClear As ListView)

        If listviewToClear.InvokeRequired Then
            Dim d As New ClearListViewCallback(AddressOf ClearListView)
            listviewToClear.Invoke(d, New Object() {listviewToClear})
        Else
            listviewToClear.Items.Clear()
        End If

    End Sub

    ' ClearListBox
    ' clear the progress box
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ClearListBox(ByVal listboxToClear As ListBox)

        Try
            If listboxToClear.InvokeRequired Then
                Dim d As New ClearListBoxCallback(AddressOf ClearListBox)
                listboxToClear.Invoke(d, New Object() {listboxToClear})

            Else
                listboxToClear.Items.Clear()
            End If

        Catch ex As Exception
        End Try

    End Sub

    Public Sub SetRadioChecked(ByVal radioToCheck As RadioButton)

        If radioToCheck.InvokeRequired Then

            Dim d As New SetRadioCheckedCallback(AddressOf SetRadioChecked)
            radioToCheck.BeginInvoke(d, New Object() {radioToCheck})

        Else
            radioToCheck.Checked = True

        End If

    End Sub

    Public Sub SetListBoxSelection(ByVal listboxToChange As ListBox, ByVal newSelection As Integer)

        If listboxToChange.InvokeRequired Then

            Dim d As New SetListBoxSelectionCallback(AddressOf SetListBoxSelection)
            listboxToChange.BeginInvoke(d, New Object() {listboxToChange, newSelection})

        Else
            listboxToChange.SelectedIndex = newSelection

        End If

    End Sub

    ' BringProgressBarToFront
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub BringPanelToFront(ByVal progessBarToSet As Panel)

        If progessBarToSet.InvokeRequired Then

            Dim d As New BringPanelToFrontCallback(AddressOf BringPanelToFront)
            progessBarToSet.BeginInvoke(d, New Object() {progessBarToSet})
        Else
            progessBarToSet.BringToFront()
        End If

    End Sub

    ' BringProgressBarToFront
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SendPanelToBack(ByVal progessBarToSet As Panel)

        If progessBarToSet.InvokeRequired Then

            Dim d As New SendPanelToBackCallback(AddressOf SendPanelToBack)
            progessBarToSet.BeginInvoke(d, New Object() {progessBarToSet})
        Else
            progessBarToSet.SendToBack()
        End If

    End Sub

    ' SetProgressBarMaximum
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetProgressBarMaximum(ByVal progessBarToSet As ProgressBar, ByVal newMaximum As Integer)

        If progessBarToSet.InvokeRequired Then

            Dim d As New SetProgressBarMaximumCallback(AddressOf SetProgressBarMaximum)
            progessBarToSet.BeginInvoke(d, New Object() {progessBarToSet, newMaximum})
        Else
            progessBarToSet.Maximum = newMaximum
        End If

    End Sub

    ' SetProgressBarValue
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetProgressBarValue(ByVal progessBarToSet As ProgressBar, ByVal newValue As Integer)

        If progessBarToSet.InvokeRequired Then

            Dim d As New SetProgressBarMaximumCallback(AddressOf SetProgressBarValue)
            progessBarToSet.BeginInvoke(d, New Object() {progessBarToSet, newValue})
        Else
            progessBarToSet.Value = newValue
        End If

    End Sub

    ' SetVerticalProgressBarValue
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetVerticalProgressBarValue(ByVal progessBarToSet As VerticalProgressBar, ByVal newValue As Integer)

        If progessBarToSet.InvokeRequired Then

            Dim d As New SetVerticalProgressBarValueCallback(AddressOf SetVerticalProgressBarValue)
            progessBarToSet.BeginInvoke(d, New Object() {progessBarToSet, newValue})
        Else
            progessBarToSet.Value = newValue
        End If

    End Sub




    ' SetButtonEnable
    ' enable or disable a button
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetButtonEnable(ByVal buttonToEnable As Button, ByVal buttonState As Boolean)

        If buttonToEnable.InvokeRequired Then
            Dim d As New SetButtonEnableCallback(AddressOf SetButtonEnable)
            buttonToEnable.Invoke(d, New Object() {buttonToEnable, buttonState})
        Else
            buttonToEnable.Enabled = buttonState
        End If
    End Sub

    ' SetButtonText
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetButtonText(ByVal buttonToChange As Button, ByVal newText As String)

        If buttonToChange.InvokeRequired Then
            Dim d As New SetButtonTextCallback(AddressOf SetButtonText)
            buttonToChange.Invoke(d, New Object() {buttonToChange, newText})
        Else
            buttonToChange.Text = newText
        End If

    End Sub

    ' SetShell
    ' used to change logon to either shell or explorer..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetShell(ByVal newShellExecutable As String)

        Dim regKey As RegistryKey

        regKey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows NT\CurrentVersion\Winlogon", True)
        regKey.SetValue("Shell", newShellExecutable)
        regKey.Close()

    End Sub

    ' ProcessStart
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ProcessStart(ByVal applicationName As String, ByVal applicationPath As String)

        Dim retValue As Boolean
        Dim pInfo As PROCESS_INFORMATION = New PROCESS_INFORMATION()
        Dim sInfo As STARTUPINFO = New STARTUPINFO()

        retValue = CreateProcess(applicationName, Nothing, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, applicationPath, sInfo, pInfo)

    End Sub

    ' ServiceStart, ServiceStop and ServiceState
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ServiceStart(ByVal serviceName As String)

        Dim serviceControl As ServiceController

        serviceControl = New ServiceController
        serviceControl.MachineName = "."
        serviceControl.ServiceName = serviceName

        serviceControl.Start()

    End Sub

    Public Sub ServiceStop(ByVal serviceName As String)

        Dim serviceControl As ServiceController

        serviceControl = New ServiceController
        serviceControl.MachineName = "."
        serviceControl.ServiceName = serviceName

        serviceControl.Start()

    End Sub

    Public Function ServiceState(ByVal serviceName As String, ByRef serviceStatus As ServiceProcess.ServiceControllerStatus) As Boolean

        Dim serviceControl As ServiceController
        Dim serviceNameParts() = serviceName.Split("\")

        Try
            serviceControl = New ServiceController
            serviceControl.MachineName = "(local)"
            serviceControl.ServiceName = serviceNameParts(serviceNameParts.Length - 1)

            serviceStatus = serviceControl.Status

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function



    Public Function GetOSVersion() As String

        Dim osResult As String = ""

        Select Case Environment.OSVersion.Platform

            Case PlatformID.Win32S
                osResult = "Win 3.1"
            Case PlatformID.Win32Windows
                Select Case Environment.OSVersion.Version.Minor
                    Case 0
                        osResult = "Win95"
                    Case 10
                        osResult = "Win98"
                    Case 90
                        osResult = "WinME"
                    Case Else
                        osResult = "Unknown"
                End Select
            Case PlatformID.Win32NT
                Select Case Environment.OSVersion.Version.Major
                    Case 3
                        osResult = "NT 3.51"
                    Case 4
                        osResult = "NT 4.0"
                    Case 5
                        Select Case _
                            Environment.OSVersion.Version.Minor
                            Case 0
                                osResult = "Win2000"
                            Case 1
                                osResult = "WinXP"
                            Case 2
                                osResult = "Win2003"
                        End Select
                    Case 6
                        osResult = "Vista/Win2008Server"
                    Case Else
                        osResult = "Unknown"
                End Select
            Case PlatformID.WinCE
                osResult = "Win CE"
        End Select



        osResult = osResult & Environment.OSVersion.Platform.ToString & ControlChars.NewLine & _
                            Environment.OSVersion.Version.Major.ToString & ControlChars.NewLine & _
                            Environment.OSVersion.Version.Minor.ToString & ControlChars.NewLine & _
                            Environment.OSVersion.Version.Build.ToString & ControlChars.NewLine & _
                            Environment.OSVersion.Version.Revision.ToString()


        Return osResult

    End Function

    Public Function RecordaSale(ByVal positionId As Integer, ByRef saleId As Integer, ByVal databaseConnectionString As String) As Boolean

        Dim singleParameter1 As New SqlParameter
        Dim singleParameter2 As New SqlParameter
        Dim singleParameter3 As New SqlParameter
        Dim databaseConnection As New SqlConnection(databaseConnectionString)
        Dim sqlCommand As New SqlCommand("spRecordSale", databaseConnection)
        Dim recordResult As Boolean = False

        sqlCommand.CommandType = CommandType.StoredProcedure

        sqlCommand.Parameters.Add("@PosID", SqlDbType.Int, 1)
        sqlCommand.Parameters("@PosID").Value = positionId
        sqlCommand.Parameters("@PosID").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@SaleID", SqlDbType.Int, 1)
        sqlCommand.Parameters("@SaleID").Direction = ParameterDirection.Output

        sqlCommand.Parameters.Add("@Result", SqlDbType.Int, 1)
        sqlCommand.Parameters("@Result").Direction = ParameterDirection.Output
        sqlCommand.CommandType = CommandType.StoredProcedure

        Try
            databaseConnection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        End Try


        If sqlCommand.Parameters("@Result").Value = "1" Then
            recordResult = True
        End If
        StringToInteger(sqlCommand.Parameters("@SaleID").Value, saleId)

        Try
            databaseConnection.Close()
        Catch ex As Exception
        End Try

        Return recordResult

    End Function

    Private currentSaleId As Integer = 0

    Public Function AddTransactionId(ByVal saleId As Integer, ByVal transactionId As String, ByVal databaseConnectionString As String) As Boolean

        Dim databaseConnection As New SqlConnection(databaseConnectionString)
        Dim sqlCommand As New SqlCommand("spAddTransactionID", databaseConnection)
        Dim recordResult As Boolean = False

        sqlCommand.CommandType = CommandType.StoredProcedure

        sqlCommand.Parameters.Add("@SaleID", SqlDbType.Int, 1)
        sqlCommand.Parameters("@SaleID").Value = saleId
        sqlCommand.Parameters("@SaleID").ParameterName = "@SaleID"
        sqlCommand.Parameters("@SaleID").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@TransactionID", SqlDbType.VarChar, 50)
        sqlCommand.Parameters("@TransactionID").Value = transactionId
        sqlCommand.Parameters("@TransactionID").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@Result", SqlDbType.Int, 1)
        sqlCommand.Parameters("@Result").Direction = ParameterDirection.Output

        Dim t As SqlParameterCollection = sqlCommand.Parameters

        Try
            databaseConnection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        End Try


        If sqlCommand.Parameters("@Result").Value = "1" Then
            recordResult = True
        End If

        Try
            databaseConnection.Close()
        Catch ex As Exception
        End Try

        Return recordResult

    End Function

    ' SetStockLevel
    ' update the database to set the current stock level.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function SetStockLevel(ByVal positionId As Integer, ByVal newStockLevel As Integer, ByVal databaseConnectionString As String)

        Dim updateResult As Boolean = False
        Dim databaseConnection As New SqlConnection(databaseConnectionString)
        Dim sqlCommand As New SqlCommand("spUpdateStock", databaseConnection)

        sqlCommand.CommandType = CommandType.StoredProcedure

        sqlCommand.Parameters.Add("@PosID", SqlDbType.Int, 1)
        sqlCommand.Parameters("@PosID").Value = positionId
        sqlCommand.Parameters("@PosID").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@Qty", SqlDbType.Int, 1)
        sqlCommand.Parameters("@Qty").Value = newStockLevel
        sqlCommand.Parameters("@Qty").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@Result", SqlDbType.Int, 1)
        sqlCommand.Parameters("@Result").Direction = ParameterDirection.Output

        Try
            databaseConnection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        End Try

        If sqlCommand.Parameters("@Result").Value = "1" Then
            updateResult = True
        End If

        databaseConnection.Close()

        Return updateResult

    End Function

End Class


' class cHelperFunctionsFactory
' ensure only one helper is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cHelperFunctionsFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared helperFunctions As cHelperFunctions = Nothing

    Public Function GetManager() As cHelperFunctions

        If IsNothing(helperFunctions) Then

            helperFunctions = New cHelperFunctions

        End If

        Return helperFunctions

    End Function

End Class


Public Class VerticalProgressBar
    Inherits ProgressBar
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.Style = cp.Style Or &H4
            Return cp
        End Get
    End Property
End Class


Module apiAccess

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessID As UInteger
        Public dwThreadID As UInteger
    End Structure 'PROCESS_INFORMATION

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure SECURITY_ATTRIBUTES
        Public nLength As Integer
        Public lpSecurityDescriptor As IntPtr
        Public bInheritHandle As Boolean

    End Structure 'SECURITY_ATTRIBUTES

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure STARTUPINFO
        Public cb As UInteger
        Public lpReserved As String
        Public lpDesktop As String
        Public lpTitle As String
        Public dwX As UInteger
        Public dwY As UInteger
        Public dwXSize As UInteger
        Public dwYSize As UInteger
        Public dwXCountChars As UInteger
        Public dwYCountChars As UInteger
        Public dwFillAttribute As UInteger
        Public dwFlags As UInteger
        Public wShowWindow As Short
        Public cbReserved2 As Short
        Public lpReserved2 As IntPtr
        Public hStdInput As IntPtr
        Public hStdOutput As IntPtr
        Public hStdError As IntPtr
    End Structure 'STARTINFO

    <DllImport("kernel32.dll")> _
    Function CreateProcess( _
    ByVal lpApplicationName As String, _
    ByVal lpCommandLine As String, _
    ByVal lpProcessAttributes As IntPtr, _
    ByVal lpThreadAttributes As IntPtr, _
    ByVal bInheritHandles As Boolean, _
    ByVal dwCreationFlags As UInteger, _
    ByVal lpEnvironment As IntPtr, _
    ByVal lpCurrentDirectory As String, _
    ByRef lpStartupInfo As STARTUPINFO, _
    ByRef lpProcessInformation As PROCESS_INFORMATION) As Boolean

    End Function

End Module
