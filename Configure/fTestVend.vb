'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports System.IO
Imports System.Text
Imports DebugWindow
Imports HelperFunctions

Public Class fTestVend

    Inherits System.Windows.Forms.Form
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents VendTimeout As System.Windows.Forms.Timer
    Friend WithEvents VendMessage As System.Windows.Forms.Label
    Friend WithEvents VendStatus As System.Windows.Forms.Label
    Friend WithEvents Instruction As System.Windows.Forms.Label
    Friend WithEvents VendEvents As System.Windows.Forms.ListBox
    Friend WithEvents ExpandDetail As System.Windows.Forms.Button
    Friend WithEvents DismissButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.VendMessage = New System.Windows.Forms.Label
        Me.VendTimeout = New System.Windows.Forms.Timer(Me.components)
        Me.VendStatus = New System.Windows.Forms.Label
        Me.Instruction = New System.Windows.Forms.Label
        Me.VendEvents = New System.Windows.Forms.ListBox
        Me.ExpandDetail = New System.Windows.Forms.Button
        Me.DismissButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'VendMessage
        '
        Me.VendMessage.BackColor = System.Drawing.Color.Transparent
        Me.VendMessage.ForeColor = System.Drawing.Color.Black
        Me.VendMessage.Location = New System.Drawing.Point(12, 58)
        Me.VendMessage.Name = "VendMessage"
        Me.VendMessage.Size = New System.Drawing.Size(88, 14)
        Me.VendMessage.TabIndex = 0
        Me.VendMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'VendTimeout
        '
        Me.VendTimeout.Interval = 1000
        '
        'VendStatus
        '
        Me.VendStatus.BackColor = System.Drawing.Color.Transparent
        Me.VendStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VendStatus.ForeColor = System.Drawing.Color.Black
        Me.VendStatus.Location = New System.Drawing.Point(12, 9)
        Me.VendStatus.Name = "VendStatus"
        Me.VendStatus.Size = New System.Drawing.Size(168, 14)
        Me.VendStatus.TabIndex = 1
        Me.VendStatus.Text = "Test vend in progress"
        Me.VendStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Instruction
        '
        Me.Instruction.BackColor = System.Drawing.Color.Transparent
        Me.Instruction.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Instruction.ForeColor = System.Drawing.Color.Black
        Me.Instruction.Location = New System.Drawing.Point(12, 25)
        Me.Instruction.Name = "Instruction"
        Me.Instruction.Size = New System.Drawing.Size(168, 14)
        Me.Instruction.TabIndex = 5
        Me.Instruction.Text = "Take the product promptly"
        Me.Instruction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'VendEvents
        '
        Me.VendEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VendEvents.Location = New System.Drawing.Point(4, 96)
        Me.VendEvents.Name = "VendEvents"
        Me.VendEvents.ScrollAlwaysVisible = True
        Me.VendEvents.Size = New System.Drawing.Size(345, 223)
        Me.VendEvents.TabIndex = 6
        '
        'ExpandDetail
        '
        Me.ExpandDetail.Location = New System.Drawing.Point(232, 58)
        Me.ExpandDetail.Name = "ExpandDetail"
        Me.ExpandDetail.Size = New System.Drawing.Size(58, 32)
        Me.ExpandDetail.TabIndex = 7
        Me.ExpandDetail.Text = "More"
        '
        'DismissButton
        '
        Me.DismissButton.Location = New System.Drawing.Point(292, 58)
        Me.DismissButton.Name = "DismissButton"
        Me.DismissButton.Size = New System.Drawing.Size(58, 32)
        Me.DismissButton.TabIndex = 8
        Me.DismissButton.Text = "Close"
        '
        'TestProductVendForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(354, 94)
        Me.ControlBox = False
        Me.Controls.Add(Me.DismissButton)
        Me.Controls.Add(Me.ExpandDetail)
        Me.Controls.Add(Me.VendEvents)
        Me.Controls.Add(Me.Instruction)
        Me.Controls.Add(Me.VendStatus)
        Me.Controls.Add(Me.VendMessage)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TestProductVendForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Please wait.."
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' TestVendForm
    ' Used to show availablity and status of a test vend, if this form is visible, you cant do a test vend..
    ' ****************************************************************************************************************************************************************

    ' Variables
    ' ****************************************************************************************************************************************************************
    Private TimeLeft As Integer
    Private Expanded As Boolean = False
    Private VendSuceeded As Boolean = False
    Private FileStreamObject As FileStream
    Private FileStreamWriterObject As StreamWriter
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' TestProductVendForm_Load - start the test vend dialog
    ' ****************************************************************************************************************************************************************
    Private Sub TestProductVendForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        debugInformation = debugInformationFactory.GetManager
        helperFunctions = helperFunctionsFactory.GetManager

        Dim LogFileName = "ActionLogConfig_" & Now.Day & "." & Now.Month & "." & Now.Year & ".txt"

        If Not Directory.Exists("C:\Control\Logs\") Then _
            Directory.CreateDirectory("C:\Control\Logs\")

        FileStreamObject = New FileStream("C:\Control\Logs\" & LogFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
        FileStreamWriterObject = New StreamWriter(FileStreamObject)
        FileStreamWriterObject.BaseStream.Seek(0, SeekOrigin.End)

    End Sub

    ' TestProductVendForm_Closing - clean up
    ' ****************************************************************************************************************************************************************
    Private Sub TestProductVendForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        FileStreamWriterObject.Close()
        FileStreamObject.Close()
    End Sub

    ' Go - start showing progress
    ' ****************************************************************************************************************************************************************
    Public Sub Go()

        VendSuceeded = False
        VendTimeout.Enabled = True
        TimeLeft = 40
        VendEvents.Items.Clear()
        VendStatus.Text = "Test vend in progress"
        Instruction.Text = "Take the product promptly"
        VendMessage.Text = TimeLeft & " seconds left"
        Visible = True

    End Sub

    ' DisplayInfo - display a vend info code
    ' ****************************************************************************************************************************************************************
    Public Sub DisplayInfoCode(ByVal informationCode As Integer)

        Dim fileOutput As String
        Dim eventDescription As String = ""
        Dim eventLevel As fDebugWindow.Level

        If VendTimeout.Enabled Then

            If debugInformation.VMCEventDetails(eventLevel, informationCode, eventDescription) Then

                fileOutput = Str(informationCode) & ControlChars.Tab & eventDescription

            Else

                fileOutput = "VMC: unrecognised code=" & informationCode.ToString
            End If

            helperFunctions.AddToListBox(VendEvents, fileOutput)

            Try
                FileStreamWriterObject.WriteLine(DateTime.Now & ControlChars.Tab & fileOutput)
                FileStreamWriterObject.Flush()
            Catch

            End Try

        End If

    End Sub

    Public Sub DisplayMetric(ByVal Message)

        If VendTimeout.Enabled Then

            helperFunctions.AddToListBox(VendEvents, "*" & ControlChars.Tab & Mid(Message, 2))
            FileStreamWriterObject.WriteLine(DateTime.Now & ControlChars.Tab & " *" & ControlChars.Tab & Mid(Message, 2))
            FileStreamWriterObject.Flush()

        End If

    End Sub

    Public Sub AddToLog(ByVal Message As String)

        Try

            FileStreamWriterObject.WriteLine(DateTime.Now & ControlChars.Tab & " >" & ControlChars.Tab & Message)
            FileStreamWriterObject.Flush()

        Catch ex As Exception

        End Try

    End Sub

    ' Complete - hide the form, or show that there has been a problem
    ' ****************************************************************************************************************************************************************
    Public Sub Complete(ByVal Result As Boolean)

        If Result Then

            VendSuceeded = True
            helperFunctions.SetLabelText(VendStatus, "Complete vend..")
            helperFunctions.SetLabelText(Instruction, "")
            helperFunctions.SetLabelText(VendMessage, "")

        Else
            helperFunctions.SetLabelText(VendStatus, "A problem has occured..")
            helperFunctions.SetLabelText(Instruction, "")
            helperFunctions.SetLabelText(VendMessage, "")
        End If

    End Sub

    ' Available - should  we start a test vend at the moment ?
    ' ****************************************************************************************************************************************************************
    Public Function Available()
        Return Not Visible
    End Function

    ' VendTimeout_Tick - another second has passed..
    ' ****************************************************************************************************************************************************************
    Private Sub VendTimeout_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendTimeout.Tick

        TimeLeft = TimeLeft - 1

        If Not VendSuceeded Then
            VendMessage.Text = TimeLeft & " seconds left"
        End If

        If TimeLeft = 0 Then
            VendTimeout.Enabled = False

            If Not VendSuceeded Then
                VendStatus.Text = "A problem has occured.."
                Instruction.Text = "Did you take the product ?"
                VendMessage.Text = ""
            End If
        End If

    End Sub

    ' VendErrorAckButton_Click - the user has acknowledged the error
    ' ****************************************************************************************************************************************************************
    Private Sub TestVendAcknowledge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Visible = False
        VendTimeout.Enabled = False

    End Sub

    ' ExpandDetail_Click - expand and collapse the detail view
    ' ****************************************************************************************************************************************************************
    Private Sub ExpandDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandDetail.Click
        Expanded = Not Expanded

        If Expanded Then

            ExpandDetail.Text = "Less"
            Height = 355
        Else
            ExpandDetail.Text = "More"
            Height = 122
        End If
    End Sub

    Private Sub DismissButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DismissButton.Click
        Hide()
    End Sub
End Class
