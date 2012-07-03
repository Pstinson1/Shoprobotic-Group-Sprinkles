'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Threading.Thread
Imports System.IO
Imports System.Threading
Imports HelperFunctions
Imports MechInterface
Imports SerialManager
Imports SettingsManager


Public Class fMain

    ' managers
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private mechanicalInterface As fMechInterface
    Private mechanicalInterfaceFactory As cMechInterfaceFactory = New cMechInterfaceFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' constants

    ' variables
    Private currentlyMapping As Boolean = False
    Private moveCompleteEvent As ManualResetEvent
    Private rackPosition As Point

    Private Sub fMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim vmcSerialPort As String

        settingsManager = settingsManagerFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()
        mechanicalInterface = mechanicalInterfaceFactory.GetManager()

        settingsManager.Initialise(My.Settings.DatabaseConnection)
        mechanicalInterface.Initialise()

        vmcSerialPort = settingsManager.GetValue("VmcSerialPort")

        serialManager.Connect(vmcSerialPort)
        serialManager.AddCallback(AddressOf SerialPortEvent)
        serialManager.Show()

        ' create events
        moveCompleteEvent = New ManualResetEvent(False)

    End Sub

    ' SerialPortEvent
    ' process incomming vmc data
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = messageContent.Split("=")

            If parameterList.Length >= 2 Then

                Select Case parameterList(0)

                    Case "BLOCK"
                        ProcessBlockData(parameterList(1))

                    Case "RACK"
                        ProcessRackData(parameterList(1))

                    Case "INFO"
                        ProcessInfoCode(parameterList(1))

                End Select

            End If

        End If

    End Sub


    Private Function ProcessBlockData(ByVal blockData As String) As Boolean

        Dim transferCoordinate As Point
        Dim parameterList As String() = blockData.Split(",")

        If parameterList.Length >= 3 AndAlso currentlyMapping Then

            helperFunctions.SetLabelText(HorizontalPositionLabel, parameterList(0))
            helperFunctions.SetLabelText(VerticalPositionLabel, parameterList(1))
            helperFunctions.SetLabelText(BlockSizeLabel, parameterList(2))

            If helperFunctions.StringToInteger(parameterList(0), transferCoordinate.X) AndAlso helperFunctions.StringToInteger(parameterList(1), transferCoordinate.Y) Then
                rackPosition = transferCoordinate

            Else
                MsgBox("malformed")

            End If

        End If

    End Function

    Private Function ProcessRackData(ByVal rackData As String) As Boolean

        Dim parameterList As String() = rackData.Split(",")
        Dim transferCoordinate As Point

        If parameterList.Length >= 2 Then
            helperFunctions.SetLabelText(HorizontalPositionLabel, parameterList(0))
            helperFunctions.SetLabelText(VerticalPositionLabel, parameterList(1))
            helperFunctions.SetLabelText(BlockSizeLabel, "")

            If helperFunctions.StringToInteger(parameterList(0), transferCoordinate.X) AndAlso helperFunctions.StringToInteger(parameterList(1), transferCoordinate.Y) Then
                rackPosition = transferCoordinate

            Else
                MsgBox("malformed")

            End If

        End If

    End Function

    Private Function ProcessInfoCode(ByVal infoCodeData As String) As Boolean

        Dim infoCode As Integer

        If helperFunctions.StringToInteger(infoCodeData, infoCode) Then

            If infoCode = 114 Then
                currentlyMapping = False
                moveCompleteEvent.Set()
            End If
        End If

    End Function

    Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click

        ' Start the worker.
        BackgroundScanWorker.WorkerReportsProgress = True
        BackgroundScanWorker.WorkerSupportsCancellation = True
        BackgroundScanWorker.RunWorkerAsync()

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundScanWorker.DoWork

        Dim nextPosition As Point

        currentlyMapping = False
        moveCompleteEvent.Reset()
        serialManager.SendMessage("MOVEHEADMIN")

        WaitForMoveToComplete()

        currentlyMapping = False
        moveCompleteEvent.Reset()
        nextPosition.X = rackPosition.X
        nextPosition.Y = rackPosition.Y + 20
        serialManager.SendMessage("MOVEHEADSCAN " & nextPosition.X.ToString("0000") & "," & nextPosition.Y.ToString("0000"))

        WaitForMoveToComplete()






        currentlyMapping = True
        moveCompleteEvent.Reset()

        nextPosition.X = 880
        nextPosition.Y = rackPosition.Y

        serialManager.SendMessage("MOVEHEADSCAN " & nextPosition.X.ToString("0000") & "," & nextPosition.Y.ToString("0000"))

        WaitForMoveToComplete()









    End Sub

    Private Sub WaitForMoveToComplete()
        If moveCompleteEvent.WaitOne(10000) Then


        End If
    End Sub

    Private Sub MechButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MechButton.Click
        mechanicalInterface.Show()
    End Sub

End Class
