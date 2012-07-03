<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fFridgeManager
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
        Me.components = New System.ComponentModel.Container
        Me.SerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.ProgressList = New System.Windows.Forms.ListBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.OpenPanel7 = New System.Windows.Forms.Panel
        Me.ClosedPanel7 = New System.Windows.Forms.Panel
        Me.RequiredCloseCheck7 = New System.Windows.Forms.CheckBox
        Me.AutoCloseDoorsCheck = New System.Windows.Forms.CheckBox
        Me.ClosePollLabel = New System.Windows.Forms.Label
        Me.OpenPollLabel = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.RequiredCloseCheck2 = New System.Windows.Forms.CheckBox
        Me.RequiredCloseCheck3 = New System.Windows.Forms.CheckBox
        Me.RequiredCloseCheck4 = New System.Windows.Forms.CheckBox
        Me.RequiredCloseCheck5 = New System.Windows.Forms.CheckBox
        Me.RequiredCloseCheck6 = New System.Windows.Forms.CheckBox
        Me.RequiredCloseCheck1 = New System.Windows.Forms.CheckBox
        Me.ClosedPanel6 = New System.Windows.Forms.Panel
        Me.Label8 = New System.Windows.Forms.Label
        Me.ClosedPanel5 = New System.Windows.Forms.Panel
        Me.Label6 = New System.Windows.Forms.Label
        Me.ClosedPanel2 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.ClosedPanel4 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.ClosedPanel3 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.ClosedPanel1 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.OpenPanel1 = New System.Windows.Forms.Panel
        Me.OpenPanel6 = New System.Windows.Forms.Panel
        Me.OpenPanel2 = New System.Windows.Forms.Panel
        Me.OpenPanel3 = New System.Windows.Forms.Panel
        Me.OpenPanel5 = New System.Windows.Forms.Panel
        Me.OpenPanel4 = New System.Windows.Forms.Panel
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.CompressorStatusText = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.TemperaturePanel = New System.Windows.Forms.Panel
        Me.TemperatureLabel = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.HideButton = New System.Windows.Forms.Button
        Me.TabPage1.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SerialPort
        '
        Me.SerialPort.ReadTimeout = 500
        Me.SerialPort.WriteTimeout = 500
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ProgressList)
        Me.TabPage1.Controls.Add(Me.GroupBox7)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(429, 236)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Summary"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ProgressList
        '
        Me.ProgressList.FormattingEnabled = True
        Me.ProgressList.IntegralHeight = False
        Me.ProgressList.Location = New System.Drawing.Point(6, 91)
        Me.ProgressList.Name = "ProgressList"
        Me.ProgressList.Size = New System.Drawing.Size(418, 139)
        Me.ProgressList.TabIndex = 28
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Label11)
        Me.GroupBox7.Controls.Add(Me.OpenPanel7)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel7)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck7)
        Me.GroupBox7.Controls.Add(Me.AutoCloseDoorsCheck)
        Me.GroupBox7.Controls.Add(Me.ClosePollLabel)
        Me.GroupBox7.Controls.Add(Me.OpenPollLabel)
        Me.GroupBox7.Controls.Add(Me.Label10)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck2)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck3)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck4)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck5)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck6)
        Me.GroupBox7.Controls.Add(Me.RequiredCloseCheck1)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel6)
        Me.GroupBox7.Controls.Add(Me.Label8)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel5)
        Me.GroupBox7.Controls.Add(Me.Label6)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel2)
        Me.GroupBox7.Controls.Add(Me.Label5)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel4)
        Me.GroupBox7.Controls.Add(Me.Label4)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel3)
        Me.GroupBox7.Controls.Add(Me.Label3)
        Me.GroupBox7.Controls.Add(Me.ClosedPanel1)
        Me.GroupBox7.Controls.Add(Me.Label2)
        Me.GroupBox7.Controls.Add(Me.Label7)
        Me.GroupBox7.Controls.Add(Me.Label1)
        Me.GroupBox7.Controls.Add(Me.OpenPanel1)
        Me.GroupBox7.Controls.Add(Me.OpenPanel6)
        Me.GroupBox7.Controls.Add(Me.OpenPanel2)
        Me.GroupBox7.Controls.Add(Me.OpenPanel3)
        Me.GroupBox7.Controls.Add(Me.OpenPanel5)
        Me.GroupBox7.Controls.Add(Me.OpenPanel4)
        Me.GroupBox7.Location = New System.Drawing.Point(6, 5)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(417, 80)
        Me.GroupBox7.TabIndex = 23
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Switches"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(229, 13)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(18, 13)
        Me.Label11.TabIndex = 40
        Me.Label11.Text = "7"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenPanel7
        '
        Me.OpenPanel7.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel7.Location = New System.Drawing.Point(230, 27)
        Me.OpenPanel7.Name = "OpenPanel7"
        Me.OpenPanel7.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel7.TabIndex = 39
        '
        'ClosedPanel7
        '
        Me.ClosedPanel7.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel7.Location = New System.Drawing.Point(230, 45)
        Me.ClosedPanel7.Name = "ClosedPanel7"
        Me.ClosedPanel7.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel7.TabIndex = 38
        '
        'RequiredCloseCheck7
        '
        Me.RequiredCloseCheck7.AutoSize = True
        Me.RequiredCloseCheck7.Location = New System.Drawing.Point(230, 60)
        Me.RequiredCloseCheck7.Name = "RequiredCloseCheck7"
        Me.RequiredCloseCheck7.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck7.TabIndex = 37
        Me.RequiredCloseCheck7.Tag = "7"
        Me.RequiredCloseCheck7.UseVisualStyleBackColor = True
        '
        'AutoCloseDoorsCheck
        '
        Me.AutoCloseDoorsCheck.AutoSize = True
        Me.AutoCloseDoorsCheck.Checked = True
        Me.AutoCloseDoorsCheck.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoCloseDoorsCheck.Location = New System.Drawing.Point(333, 56)
        Me.AutoCloseDoorsCheck.Name = "AutoCloseDoorsCheck"
        Me.AutoCloseDoorsCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.AutoCloseDoorsCheck.Size = New System.Drawing.Size(78, 17)
        Me.AutoCloseDoorsCheck.TabIndex = 36
        Me.AutoCloseDoorsCheck.Text = "Auto Close"
        Me.AutoCloseDoorsCheck.UseVisualStyleBackColor = True
        '
        'ClosePollLabel
        '
        Me.ClosePollLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ClosePollLabel.Location = New System.Drawing.Point(248, 42)
        Me.ClosePollLabel.Name = "ClosePollLabel"
        Me.ClosePollLabel.Size = New System.Drawing.Size(32, 16)
        Me.ClosePollLabel.TabIndex = 35
        Me.ClosePollLabel.Text = "???"
        Me.ClosePollLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OpenPollLabel
        '
        Me.OpenPollLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.OpenPollLabel.Location = New System.Drawing.Point(248, 27)
        Me.OpenPollLabel.Name = "OpenPollLabel"
        Me.OpenPollLabel.Size = New System.Drawing.Size(32, 16)
        Me.OpenPollLabel.TabIndex = 34
        Me.OpenPollLabel.Text = "???"
        Me.OpenPollLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(30, 60)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(83, 13)
        Me.Label10.TabIndex = 33
        Me.Label10.Text = "Required closed"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RequiredCloseCheck2
        '
        Me.RequiredCloseCheck2.AutoSize = True
        Me.RequiredCloseCheck2.Location = New System.Drawing.Point(137, 60)
        Me.RequiredCloseCheck2.Name = "RequiredCloseCheck2"
        Me.RequiredCloseCheck2.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck2.TabIndex = 32
        Me.RequiredCloseCheck2.Tag = "2"
        Me.RequiredCloseCheck2.UseVisualStyleBackColor = True
        '
        'RequiredCloseCheck3
        '
        Me.RequiredCloseCheck3.AutoSize = True
        Me.RequiredCloseCheck3.Location = New System.Drawing.Point(156, 60)
        Me.RequiredCloseCheck3.Name = "RequiredCloseCheck3"
        Me.RequiredCloseCheck3.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck3.TabIndex = 31
        Me.RequiredCloseCheck3.Tag = "3"
        Me.RequiredCloseCheck3.UseVisualStyleBackColor = True
        '
        'RequiredCloseCheck4
        '
        Me.RequiredCloseCheck4.AutoSize = True
        Me.RequiredCloseCheck4.Location = New System.Drawing.Point(175, 60)
        Me.RequiredCloseCheck4.Name = "RequiredCloseCheck4"
        Me.RequiredCloseCheck4.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck4.TabIndex = 30
        Me.RequiredCloseCheck4.Tag = "4"
        Me.RequiredCloseCheck4.UseVisualStyleBackColor = True
        '
        'RequiredCloseCheck5
        '
        Me.RequiredCloseCheck5.AutoSize = True
        Me.RequiredCloseCheck5.Location = New System.Drawing.Point(194, 60)
        Me.RequiredCloseCheck5.Name = "RequiredCloseCheck5"
        Me.RequiredCloseCheck5.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck5.TabIndex = 29
        Me.RequiredCloseCheck5.Tag = "5"
        Me.RequiredCloseCheck5.UseVisualStyleBackColor = True
        '
        'RequiredCloseCheck6
        '
        Me.RequiredCloseCheck6.AutoSize = True
        Me.RequiredCloseCheck6.Location = New System.Drawing.Point(212, 60)
        Me.RequiredCloseCheck6.Name = "RequiredCloseCheck6"
        Me.RequiredCloseCheck6.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck6.TabIndex = 28
        Me.RequiredCloseCheck6.Tag = "6"
        Me.RequiredCloseCheck6.UseVisualStyleBackColor = True
        '
        'RequiredCloseCheck1
        '
        Me.RequiredCloseCheck1.AutoSize = True
        Me.RequiredCloseCheck1.Location = New System.Drawing.Point(118, 60)
        Me.RequiredCloseCheck1.Name = "RequiredCloseCheck1"
        Me.RequiredCloseCheck1.Size = New System.Drawing.Size(15, 14)
        Me.RequiredCloseCheck1.TabIndex = 27
        Me.RequiredCloseCheck1.Tag = "1"
        Me.RequiredCloseCheck1.UseVisualStyleBackColor = True
        '
        'ClosedPanel6
        '
        Me.ClosedPanel6.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel6.Location = New System.Drawing.Point(212, 44)
        Me.ClosedPanel6.Name = "ClosedPanel6"
        Me.ClosedPanel6.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel6.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(79, 45)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(33, 13)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Close"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ClosedPanel5
        '
        Me.ClosedPanel5.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel5.Location = New System.Drawing.Point(194, 44)
        Me.ClosedPanel5.Name = "ClosedPanel5"
        Me.ClosedPanel5.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel5.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(211, 13)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "6"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ClosedPanel2
        '
        Me.ClosedPanel2.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel2.Location = New System.Drawing.Point(137, 44)
        Me.ClosedPanel2.Name = "ClosedPanel2"
        Me.ClosedPanel2.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel2.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(193, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(18, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "5"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ClosedPanel4
        '
        Me.ClosedPanel4.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel4.Location = New System.Drawing.Point(175, 44)
        Me.ClosedPanel4.Name = "ClosedPanel4"
        Me.ClosedPanel4.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel4.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(174, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(18, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "4"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ClosedPanel3
        '
        Me.ClosedPanel3.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel3.Location = New System.Drawing.Point(156, 44)
        Me.ClosedPanel3.Name = "ClosedPanel3"
        Me.ClosedPanel3.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel3.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(155, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(18, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "3"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ClosedPanel1
        '
        Me.ClosedPanel1.BackColor = System.Drawing.Color.LightSlateGray
        Me.ClosedPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ClosedPanel1.Location = New System.Drawing.Point(118, 44)
        Me.ClosedPanel1.Name = "ClosedPanel1"
        Me.ClosedPanel1.Size = New System.Drawing.Size(13, 13)
        Me.ClosedPanel1.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(136, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "2"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(68, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(44, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Open"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(117, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenPanel1
        '
        Me.OpenPanel1.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel1.Location = New System.Drawing.Point(118, 28)
        Me.OpenPanel1.Name = "OpenPanel1"
        Me.OpenPanel1.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel1.TabIndex = 6
        '
        'OpenPanel6
        '
        Me.OpenPanel6.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel6.Location = New System.Drawing.Point(212, 28)
        Me.OpenPanel6.Name = "OpenPanel6"
        Me.OpenPanel6.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel6.TabIndex = 12
        '
        'OpenPanel2
        '
        Me.OpenPanel2.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel2.Location = New System.Drawing.Point(137, 28)
        Me.OpenPanel2.Name = "OpenPanel2"
        Me.OpenPanel2.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel2.TabIndex = 8
        '
        'OpenPanel3
        '
        Me.OpenPanel3.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel3.Location = New System.Drawing.Point(156, 28)
        Me.OpenPanel3.Name = "OpenPanel3"
        Me.OpenPanel3.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel3.TabIndex = 10
        '
        'OpenPanel5
        '
        Me.OpenPanel5.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel5.Location = New System.Drawing.Point(194, 28)
        Me.OpenPanel5.Name = "OpenPanel5"
        Me.OpenPanel5.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel5.TabIndex = 14
        '
        'OpenPanel4
        '
        Me.OpenPanel4.BackColor = System.Drawing.Color.LightSlateGray
        Me.OpenPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OpenPanel4.Location = New System.Drawing.Point(175, 28)
        Me.OpenPanel4.Name = "OpenPanel4"
        Me.OpenPanel4.Size = New System.Drawing.Size(13, 13)
        Me.OpenPanel4.TabIndex = 12
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(2, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(437, 262)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.CompressorStatusText)
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(429, 236)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Temperature"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'CompressorStatusText
        '
        Me.CompressorStatusText.Location = New System.Drawing.Point(6, 210)
        Me.CompressorStatusText.Name = "CompressorStatusText"
        Me.CompressorStatusText.Size = New System.Drawing.Size(420, 20)
        Me.CompressorStatusText.TabIndex = 29
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TemperaturePanel)
        Me.GroupBox3.Controls.Add(Me.TemperatureLabel)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Location = New System.Drawing.Point(9, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(189, 67)
        Me.GroupBox3.TabIndex = 28
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Temperature"
        '
        'TemperaturePanel
        '
        Me.TemperaturePanel.BackColor = System.Drawing.Color.Blue
        Me.TemperaturePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TemperaturePanel.Location = New System.Drawing.Point(170, 45)
        Me.TemperaturePanel.Name = "TemperaturePanel"
        Me.TemperaturePanel.Size = New System.Drawing.Size(13, 13)
        Me.TemperaturePanel.TabIndex = 28
        '
        'TemperatureLabel
        '
        Me.TemperatureLabel.BackColor = System.Drawing.Color.Black
        Me.TemperatureLabel.Font = New System.Drawing.Font("Trebuchet MS", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TemperatureLabel.ForeColor = System.Drawing.Color.White
        Me.TemperatureLabel.Location = New System.Drawing.Point(66, 26)
        Me.TemperatureLabel.Name = "TemperatureLabel"
        Me.TemperatureLabel.Size = New System.Drawing.Size(60, 32)
        Me.TemperatureLabel.TabIndex = 27
        Me.TemperatureLabel.Text = "99.9"
        Me.TemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 26)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 13)
        Me.Label9.TabIndex = 26
        Me.Label9.Text = "degrees c"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 301)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(439, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(280, 17)
        Me.ToolStripStatusLabel1.Text = "Ready"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.AutoSize = False
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(150, 17)
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(378, 264)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 12
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'fFridgeManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(439, 323)
        Me.ControlBox = False
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fFridgeManager"
        Me.ShowInTaskbar = False
        Me.Text = "Fridge Manager"
        Me.TopMost = True
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SerialPort As System.IO.Ports.SerialPort
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents ProgressList As System.Windows.Forms.ListBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ClosedPanel6 As System.Windows.Forms.Panel
    Friend WithEvents ClosedPanel1 As System.Windows.Forms.Panel
    Friend WithEvents ClosedPanel2 As System.Windows.Forms.Panel
    Friend WithEvents ClosedPanel5 As System.Windows.Forms.Panel
    Friend WithEvents ClosedPanel3 As System.Windows.Forms.Panel
    Friend WithEvents ClosedPanel4 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents OpenPanel1 As System.Windows.Forms.Panel
    Friend WithEvents OpenPanel6 As System.Windows.Forms.Panel
    Friend WithEvents OpenPanel2 As System.Windows.Forms.Panel
    Friend WithEvents OpenPanel3 As System.Windows.Forms.Panel
    Friend WithEvents OpenPanel5 As System.Windows.Forms.Panel
    Friend WithEvents OpenPanel4 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents RequiredCloseCheck2 As System.Windows.Forms.CheckBox
    Friend WithEvents RequiredCloseCheck3 As System.Windows.Forms.CheckBox
    Friend WithEvents RequiredCloseCheck4 As System.Windows.Forms.CheckBox
    Friend WithEvents RequiredCloseCheck5 As System.Windows.Forms.CheckBox
    Friend WithEvents RequiredCloseCheck6 As System.Windows.Forms.CheckBox
    Friend WithEvents RequiredCloseCheck1 As System.Windows.Forms.CheckBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents OpenPollLabel As System.Windows.Forms.Label
    Friend WithEvents ClosePollLabel As System.Windows.Forms.Label
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents AutoCloseDoorsCheck As System.Windows.Forms.CheckBox
    Friend WithEvents HideButton As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents OpenPanel7 As System.Windows.Forms.Panel
    Friend WithEvents ClosedPanel7 As System.Windows.Forms.Panel
    Friend WithEvents RequiredCloseCheck7 As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TemperaturePanel As System.Windows.Forms.Panel
    Friend WithEvents TemperatureLabel As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CompressorStatusText As System.Windows.Forms.Label
End Class
