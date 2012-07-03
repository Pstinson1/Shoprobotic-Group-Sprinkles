'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Data.SqlClient

Imports HelperFunctions


Public Class uSplashScreen

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------

    ' managers
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory
    Private printerStatus As cPrinterStatus

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        Dim printerName As String
        'Dim databaseConnection As SqlConnection
        'Dim dataSource As String
        'Dim serviceStatus As ServiceProcess.ServiceControllerStatus

        ' get any required managers
        helperFunctions = helperFunctionsFactory.GetManager()

        printerStatus = New cPrinterStatus
        'databaseConnection = New SqlConnection
        'databaseConnection.ConnectionString = databaseConnectionString

        'dataSource = databaseConnection.DataSource.ToString

        'SqlServiceNameLabel.Text = dataSource

        'helperFunctions.ServiceState(dataSource, serviceStatus)
        printerName = printerStatus.DefaultPrinterName()

        helperFunctions.SetLabelText(PrinterNameLabel, printerName)

    End Sub


    Public Sub Shutdown()
        If Not printerStatus Is Nothing Then printerStatus.Shutdown()
    End Sub

    ' ExplorerButton_Click & ShellButton_Click
    ' set what we boot to.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ExplorerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExplorerButton.Click
        helperFunctions.SetShell("explorer.exe")
    End Sub

    Private Sub ShellButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShellButton.Click
        helperFunctions.SetShell("shell.exe")
    End Sub

    Private Sub CheckPrinterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckPrinterButton.Click

        Dim printerReady As cPrinterStatus.STATUS
        Dim extendedDetectedErrorState As Integer
        Dim extendedPrinterStatus As Integer

        printerStatus.CheckPrinterStatus(printerReady, extendedDetectedErrorState, extendedPrinterStatus)

        helperFunctions.SetLabelText(ErrorDetailLabel, extendedDetectedErrorState & "," & extendedPrinterStatus)
        helperFunctions.SetLabelText(PrinterStatusLabel, "Status: " & printerReady.ToString)

    End Sub

End Class
