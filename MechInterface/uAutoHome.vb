'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.Windows.Forms
Imports System.Drawing
Imports HelperFunctions
Imports SerialManager
Imports System.Threading.Thread
Imports System.Threading

Public Class uAutoHome

    ' managers
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' variables
    Private lastReportedRackPosition As Point

    ' events
    Public Event HomePositionConfirmed(ByVal horizontalPosition As Integer, ByVal verticalPosition As Integer)

    ' Initialise
    ' set up the control
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        helperFunctions = helperFunctionsFactory.GetManager()

        serialManager = serialManagerFactory.GetManager()
        serialManager.AddCallback(AddressOf SerialPortEvent)

    End Sub

    ' SerialPortEvent
    ' process messages from the VMC
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = messageContent.Split("=")

            Select Case parameterList(0)

                Case "RACK"

                    Dim axisList() As String = Split(parameterList(1), ",")

                    If (axisList(0) = "?" Or axisList(1) = "?") Then

                        helperFunctions.SetLabelText(PositionXlabel, "???")
                        helperFunctions.SetLabelText(PositionYlabel, "???")

                    Else

                        lastReportedRackPosition.X = Convert.ToInt32(axisList(0))
                        lastReportedRackPosition.Y = Convert.ToInt32(axisList(1))
                        helperFunctions.SetLabelText(PositionXlabel, lastReportedRackPosition.X)
                        helperFunctions.SetLabelText(PositionYlabel, lastReportedRackPosition.Y)

                    End If

            End Select

        End If

    End Sub

    ' FindHomeButton_Click
    ' tell the VMC to find the home switches
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub FindHomeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindHomeButton.Click
        serialManager.SendMessage("FINDHOME ")
    End Sub

    ' StorePositionButton_Click
    ' move these values across to the appropriate write column
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub StorePositionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StorePositionButton.Click

        Dim xPosition As Integer
        Dim yPosition As Integer

        If PositionXlabel.Text = "???" Or PositionYlabel.Text = "???" Then

            MsgBox("Unable to set home position", MsgBoxStyle.Exclamation, "Error")

        Else
            helperFunctions.StringToInteger(PositionXlabel.Text, xPosition)
            helperFunctions.StringToInteger(PositionYlabel.Text, yPosition)

            RaiseEvent HomePositionConfirmed(xPosition, yPosition)

            serialManager.SendMessage("SETPRM HORZ_HP " & xPosition)
            Thread.Sleep(20)
            serialManager.SendMessage("SETPRM VERT_HP " & yPosition)

        End If

    End Sub

End Class
