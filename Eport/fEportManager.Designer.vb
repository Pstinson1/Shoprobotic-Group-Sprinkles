<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fEportManager
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ProgressList = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Track1Edit = New System.Windows.Forms.TextBox
        Me.Track2Edit = New System.Windows.Forms.TextBox
        Me.Track3Edit = New System.Windows.Forms.TextBox
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.Track3StatusEdit = New System.Windows.Forms.TextBox
        Me.Track2StatusEdit = New System.Windows.Forms.TextBox
        Me.Track1StatusEdit = New System.Windows.Forms.TextBox
        Me.DeviceSerialEdit = New System.Windows.Forms.TextBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Label6 = New System.Windows.Forms.Label
        Me.Last4DigitsEdit = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.EncodingEdit = New System.Windows.Forms.TextBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Label4 = New System.Windows.Forms.Label
        Me.HideButton = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ProgressList)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(429, 157)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Progress"
        '
        'ProgressList
        '
        Me.ProgressList.FormattingEnabled = True
        Me.ProgressList.HorizontalScrollbar = True
        Me.ProgressList.Location = New System.Drawing.Point(5, 16)
        Me.ProgressList.Name = "ProgressList"
        Me.ProgressList.Size = New System.Drawing.Size(418, 134)
        Me.ProgressList.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 21)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Track 1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Track 2"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(12, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 21)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Track 3"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Track1Edit
        '
        Me.Track1Edit.Location = New System.Drawing.Point(105, 9)
        Me.Track1Edit.Name = "Track1Edit"
        Me.Track1Edit.Size = New System.Drawing.Size(248, 21)
        Me.Track1Edit.TabIndex = 6
        '
        'Track2Edit
        '
        Me.Track2Edit.Location = New System.Drawing.Point(105, 31)
        Me.Track2Edit.Name = "Track2Edit"
        Me.Track2Edit.Size = New System.Drawing.Size(248, 21)
        Me.Track2Edit.TabIndex = 7
        '
        'Track3Edit
        '
        Me.Track3Edit.Location = New System.Drawing.Point(105, 53)
        Me.Track3Edit.Name = "Track3Edit"
        Me.Track3Edit.Size = New System.Drawing.Size(248, 21)
        Me.Track3Edit.TabIndex = 8
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 301)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(439, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 9
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(300, 17)
        Me.ToolStripStatusLabel1.Text = "Status: UNKNOWN"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Track3StatusEdit
        '
        Me.Track3StatusEdit.Location = New System.Drawing.Point(63, 53)
        Me.Track3StatusEdit.Name = "Track3StatusEdit"
        Me.Track3StatusEdit.Size = New System.Drawing.Size(41, 21)
        Me.Track3StatusEdit.TabIndex = 12
        '
        'Track2StatusEdit
        '
        Me.Track2StatusEdit.Location = New System.Drawing.Point(63, 31)
        Me.Track2StatusEdit.Name = "Track2StatusEdit"
        Me.Track2StatusEdit.Size = New System.Drawing.Size(41, 21)
        Me.Track2StatusEdit.TabIndex = 11
        '
        'Track1StatusEdit
        '
        Me.Track1StatusEdit.Location = New System.Drawing.Point(63, 9)
        Me.Track1StatusEdit.Name = "Track1StatusEdit"
        Me.Track1StatusEdit.Size = New System.Drawing.Size(41, 21)
        Me.Track1StatusEdit.TabIndex = 10
        '
        'DeviceSerialEdit
        '
        Me.DeviceSerialEdit.Location = New System.Drawing.Point(58, 10)
        Me.DeviceSerialEdit.Name = "DeviceSerialEdit"
        Me.DeviceSerialEdit.Size = New System.Drawing.Size(295, 21)
        Me.DeviceSerialEdit.TabIndex = 13
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(1, 168)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(367, 130)
        Me.TabControl1.TabIndex = 14
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Last4DigitsEdit)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.EncodingEdit)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.Track3StatusEdit)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Track2StatusEdit)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Track1StatusEdit)
        Me.TabPage1.Controls.Add(Me.Track1Edit)
        Me.TabPage1.Controls.Add(Me.Track2Edit)
        Me.TabPage1.Controls.Add(Me.Track3Edit)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(359, 104)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Card Data"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(224, 75)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 21)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Last 4 digits"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Last4DigitsEdit
        '
        Me.Last4DigitsEdit.Location = New System.Drawing.Point(299, 75)
        Me.Last4DigitsEdit.Name = "Last4DigitsEdit"
        Me.Last4DigitsEdit.Size = New System.Drawing.Size(54, 21)
        Me.Last4DigitsEdit.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(1, 73)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 21)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Encoding"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'EncodingEdit
        '
        Me.EncodingEdit.Location = New System.Drawing.Point(63, 75)
        Me.EncodingEdit.Name = "EncodingEdit"
        Me.EncodingEdit.Size = New System.Drawing.Size(126, 21)
        Me.EncodingEdit.TabIndex = 13
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.DeviceSerialEdit)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(359, 104)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Device Data"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(9, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 21)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Serial"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(374, 264)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 15
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'fEportManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(439, 323)
        Me.ControlBox = False
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fEportManager"
        Me.ShowInTaskbar = False
        Me.Text = "ePort Manager"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ProgressList As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Track1Edit As System.Windows.Forms.TextBox
    Friend WithEvents Track2Edit As System.Windows.Forms.TextBox
    Friend WithEvents Track3Edit As System.Windows.Forms.TextBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Track3StatusEdit As System.Windows.Forms.TextBox
    Friend WithEvents Track2StatusEdit As System.Windows.Forms.TextBox
    Friend WithEvents Track1StatusEdit As System.Windows.Forms.TextBox
    Friend WithEvents DeviceSerialEdit As System.Windows.Forms.TextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents EncodingEdit As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Last4DigitsEdit As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents HideButton As System.Windows.Forms.Button
End Class
