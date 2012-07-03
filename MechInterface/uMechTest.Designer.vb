<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uMechTest
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DoorCloseButton = New System.Windows.Forms.Button()
        Me.DoorOpenButton = New System.Windows.Forms.Button()
        Me.ArmGroup = New System.Windows.Forms.GroupBox()
        Me.ArmVectorLabel = New System.Windows.Forms.Label()
        Me.ArmKnobPanel = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.MechGroup = New System.Windows.Forms.GroupBox()
        Me.HomeButton = New System.Windows.Forms.Button()
        Me.LimitButton = New System.Windows.Forms.Button()
        Me.MechVectorLabel = New System.Windows.Forms.Label()
        Me.RackKnobPanel = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.FanOffButton = New System.Windows.Forms.Button()
        Me.CompressorOffButton = New System.Windows.Forms.Button()
        Me.CompressorOnButton = New System.Windows.Forms.Button()
        Me.FanOnButton = New System.Windows.Forms.Button()
        Me.VacuumOffButton = New System.Windows.Forms.Button()
        Me.VacuumOnButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MechTestTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ProductHeldPanel = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.RearSensorPanel = New System.Windows.Forms.Panel()
        Me.FrontSensorPanel = New System.Windows.Forms.Panel()
        Me.ShelfDetectionPanel = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.HorzHomePanel = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.HorzLimitPanel = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.VertHomePanel = New System.Windows.Forms.Panel()
        Me.VertLimitPanel = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.DoorOpenPanel = New System.Windows.Forms.Panel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.DoorClosePanel = New System.Windows.Forms.Panel()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.ArmHomePanel = New System.Windows.Forms.Panel()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.GroupBox1.SuspendLayout()
        Me.ArmGroup.SuspendLayout()
        Me.MechGroup.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.DoorCloseButton)
        Me.GroupBox1.Controls.Add(Me.DoorOpenButton)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(119, 83)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Door"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(72, 21)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "CLOSE"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(37, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "OPEN"
        '
        'DoorCloseButton
        '
        Me.DoorCloseButton.Location = New System.Drawing.Point(71, 37)
        Me.DoorCloseButton.Name = "DoorCloseButton"
        Me.DoorCloseButton.Size = New System.Drawing.Size(40, 40)
        Me.DoorCloseButton.TabIndex = 13
        Me.DoorCloseButton.UseVisualStyleBackColor = True
        '
        'DoorOpenButton
        '
        Me.DoorOpenButton.Location = New System.Drawing.Point(31, 37)
        Me.DoorOpenButton.Name = "DoorOpenButton"
        Me.DoorOpenButton.Size = New System.Drawing.Size(40, 40)
        Me.DoorOpenButton.TabIndex = 12
        Me.DoorOpenButton.UseVisualStyleBackColor = True
        '
        'ArmGroup
        '
        Me.ArmGroup.Controls.Add(Me.ArmVectorLabel)
        Me.ArmGroup.Controls.Add(Me.ArmKnobPanel)
        Me.ArmGroup.Controls.Add(Me.Panel2)
        Me.ArmGroup.Location = New System.Drawing.Point(128, 3)
        Me.ArmGroup.Name = "ArmGroup"
        Me.ArmGroup.Size = New System.Drawing.Size(85, 249)
        Me.ArmGroup.TabIndex = 1
        Me.ArmGroup.TabStop = False
        Me.ArmGroup.Text = "Arm"
        '
        'ArmVectorLabel
        '
        Me.ArmVectorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ArmVectorLabel.Location = New System.Drawing.Point(8, 230)
        Me.ArmVectorLabel.Name = "ArmVectorLabel"
        Me.ArmVectorLabel.Size = New System.Drawing.Size(50, 13)
        Me.ArmVectorLabel.TabIndex = 6
        Me.ArmVectorLabel.Text = "0"
        Me.ArmVectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ArmKnobPanel
        '
        Me.ArmKnobPanel.BackColor = System.Drawing.Color.Transparent
        Me.ArmKnobPanel.BackgroundImage = Global.MechInterface.My.Resources.Resources.Knob
        Me.ArmKnobPanel.Location = New System.Drawing.Point(36, 84)
        Me.ArmKnobPanel.Name = "ArmKnobPanel"
        Me.ArmKnobPanel.Size = New System.Drawing.Size(43, 43)
        Me.ArmKnobPanel.TabIndex = 3
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Location = New System.Drawing.Point(54, 37)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(6, 140)
        Me.Panel2.TabIndex = 1
        '
        'MechGroup
        '
        Me.MechGroup.Controls.Add(Me.HomeButton)
        Me.MechGroup.Controls.Add(Me.LimitButton)
        Me.MechGroup.Controls.Add(Me.MechVectorLabel)
        Me.MechGroup.Controls.Add(Me.RackKnobPanel)
        Me.MechGroup.Controls.Add(Me.Panel3)
        Me.MechGroup.Controls.Add(Me.Panel1)
        Me.MechGroup.Location = New System.Drawing.Point(219, 3)
        Me.MechGroup.Name = "MechGroup"
        Me.MechGroup.Size = New System.Drawing.Size(200, 249)
        Me.MechGroup.TabIndex = 2
        Me.MechGroup.TabStop = False
        Me.MechGroup.Text = "Mech"
        '
        'HomeButton
        '
        Me.HomeButton.Location = New System.Drawing.Point(70, 210)
        Me.HomeButton.Name = "HomeButton"
        Me.HomeButton.Size = New System.Drawing.Size(62, 33)
        Me.HomeButton.TabIndex = 5
        Me.HomeButton.Text = "Home"
        Me.HomeButton.UseVisualStyleBackColor = True
        '
        'LimitButton
        '
        Me.LimitButton.Location = New System.Drawing.Point(132, 210)
        Me.LimitButton.Name = "LimitButton"
        Me.LimitButton.Size = New System.Drawing.Size(62, 33)
        Me.LimitButton.TabIndex = 6
        Me.LimitButton.Text = "Limit"
        Me.LimitButton.UseVisualStyleBackColor = True
        '
        'MechVectorLabel
        '
        Me.MechVectorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.MechVectorLabel.Location = New System.Drawing.Point(10, 230)
        Me.MechVectorLabel.Name = "MechVectorLabel"
        Me.MechVectorLabel.Size = New System.Drawing.Size(50, 13)
        Me.MechVectorLabel.TabIndex = 5
        Me.MechVectorLabel.Text = "0,0"
        Me.MechVectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RackKnobPanel
        '
        Me.RackKnobPanel.BackColor = System.Drawing.Color.Transparent
        Me.RackKnobPanel.BackgroundImage = Global.MechInterface.My.Resources.Resources.Knob
        Me.RackKnobPanel.Location = New System.Drawing.Point(90, 84)
        Me.RackKnobPanel.Name = "RackKnobPanel"
        Me.RackKnobPanel.Size = New System.Drawing.Size(43, 43)
        Me.RackKnobPanel.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Location = New System.Drawing.Point(42, 104)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(140, 6)
        Me.Panel3.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Location = New System.Drawing.Point(109, 37)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(6, 140)
        Me.Panel1.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.Label4)
        Me.GroupBox4.Controls.Add(Me.FanOffButton)
        Me.GroupBox4.Controls.Add(Me.CompressorOffButton)
        Me.GroupBox4.Controls.Add(Me.CompressorOnButton)
        Me.GroupBox4.Controls.Add(Me.FanOnButton)
        Me.GroupBox4.Controls.Add(Me.VacuumOffButton)
        Me.GroupBox4.Controls.Add(Me.VacuumOnButton)
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Location = New System.Drawing.Point(3, 92)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(119, 160)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Power"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(78, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(27, 13)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "OFF"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(39, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "ON"
        '
        'FanOffButton
        '
        Me.FanOffButton.Location = New System.Drawing.Point(71, 72)
        Me.FanOffButton.Name = "FanOffButton"
        Me.FanOffButton.Size = New System.Drawing.Size(40, 40)
        Me.FanOffButton.TabIndex = 11
        Me.FanOffButton.UseVisualStyleBackColor = True
        '
        'CompressorOffButton
        '
        Me.CompressorOffButton.Location = New System.Drawing.Point(71, 112)
        Me.CompressorOffButton.Name = "CompressorOffButton"
        Me.CompressorOffButton.Size = New System.Drawing.Size(40, 40)
        Me.CompressorOffButton.TabIndex = 10
        Me.CompressorOffButton.UseVisualStyleBackColor = True
        '
        'CompressorOnButton
        '
        Me.CompressorOnButton.Location = New System.Drawing.Point(31, 112)
        Me.CompressorOnButton.Name = "CompressorOnButton"
        Me.CompressorOnButton.Size = New System.Drawing.Size(40, 40)
        Me.CompressorOnButton.TabIndex = 9
        Me.CompressorOnButton.UseVisualStyleBackColor = True
        '
        'FanOnButton
        '
        Me.FanOnButton.Location = New System.Drawing.Point(31, 72)
        Me.FanOnButton.Name = "FanOnButton"
        Me.FanOnButton.Size = New System.Drawing.Size(40, 40)
        Me.FanOnButton.TabIndex = 8
        Me.FanOnButton.UseVisualStyleBackColor = True
        '
        'VacuumOffButton
        '
        Me.VacuumOffButton.Location = New System.Drawing.Point(71, 32)
        Me.VacuumOffButton.Name = "VacuumOffButton"
        Me.VacuumOffButton.Size = New System.Drawing.Size(40, 40)
        Me.VacuumOffButton.TabIndex = 7
        Me.VacuumOffButton.UseVisualStyleBackColor = True
        '
        'VacuumOnButton
        '
        Me.VacuumOnButton.Location = New System.Drawing.Point(31, 32)
        Me.VacuumOnButton.Name = "VacuumOnButton"
        Me.VacuumOnButton.Size = New System.Drawing.Size(40, 40)
        Me.VacuumOnButton.TabIndex = 4
        Me.VacuumOnButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 126)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(28, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cmp"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Fan"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(24, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Vac"
        '
        'MechTestTimer
        '
        Me.MechTestTimer.Interval = 50
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(78, 364)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Key:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(128, 364)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(71, 13)
        Me.Label19.TabIndex = 35
        Me.Label19.Text = "Switch closed"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(222, 364)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(65, 13)
        Me.Label18.TabIndex = 34
        Me.Label18.Text = "Switch open"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ProductHeldPanel)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.RearSensorPanel)
        Me.GroupBox2.Controls.Add(Me.FrontSensorPanel)
        Me.GroupBox2.Controls.Add(Me.ShelfDetectionPanel)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.HorzHomePanel)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.HorzLimitPanel)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.VertHomePanel)
        Me.GroupBox2.Controls.Add(Me.VertLimitPanel)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.DoorOpenPanel)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Controls.Add(Me.DoorClosePanel)
        Me.GroupBox2.Controls.Add(Me.Label20)
        Me.GroupBox2.Controls.Add(Me.ArmHomePanel)
        Me.GroupBox2.Controls.Add(Me.Label21)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 254)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(416, 100)
        Me.GroupBox2.TabIndex = 36
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Switches"
        '
        'ProductHeldPanel
        '
        Me.ProductHeldPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.ProductHeldPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ProductHeldPanel.Location = New System.Drawing.Point(392, 41)
        Me.ProductHeldPanel.Name = "ProductHeldPanel"
        Me.ProductHeldPanel.Size = New System.Drawing.Size(13, 13)
        Me.ProductHeldPanel.TabIndex = 32
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(318, 41)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(67, 13)
        Me.Label9.TabIndex = 31
        Me.Label9.Text = "Product held"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(154, 75)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(105, 13)
        Me.Label10.TabIndex = 30
        Me.Label10.Text = "Rear product sensor"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label11
        '
        Me.Label11.AccessibleDescription = ""
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(151, 58)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(108, 13)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "Front product sensor"
        '
        'RearSensorPanel
        '
        Me.RearSensorPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.RearSensorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RearSensorPanel.Location = New System.Drawing.Point(263, 75)
        Me.RearSensorPanel.Name = "RearSensorPanel"
        Me.RearSensorPanel.Size = New System.Drawing.Size(13, 13)
        Me.RearSensorPanel.TabIndex = 28
        '
        'FrontSensorPanel
        '
        Me.FrontSensorPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.FrontSensorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FrontSensorPanel.Location = New System.Drawing.Point(263, 58)
        Me.FrontSensorPanel.Name = "FrontSensorPanel"
        Me.FrontSensorPanel.Size = New System.Drawing.Size(13, 13)
        Me.FrontSensorPanel.TabIndex = 27
        '
        'ShelfDetectionPanel
        '
        Me.ShelfDetectionPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.ShelfDetectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ShelfDetectionPanel.Location = New System.Drawing.Point(392, 58)
        Me.ShelfDetectionPanel.Name = "ShelfDetectionPanel"
        Me.ShelfDetectionPanel.Size = New System.Drawing.Size(13, 13)
        Me.ShelfDetectionPanel.TabIndex = 26
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(308, 58)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(79, 13)
        Me.Label12.TabIndex = 25
        Me.Label12.Text = "Shelf detection"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(298, 24)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(89, 13)
        Me.Label14.TabIndex = 22
        Me.Label14.Text = "Picking arm home"
        '
        'HorzHomePanel
        '
        Me.HorzHomePanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.HorzHomePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HorzHomePanel.Location = New System.Drawing.Point(103, 58)
        Me.HorzHomePanel.Name = "HorzHomePanel"
        Me.HorzHomePanel.Size = New System.Drawing.Size(13, 13)
        Me.HorzHomePanel.TabIndex = 14
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(161, 41)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(98, 13)
        Me.Label16.TabIndex = 24
        Me.Label16.Text = "Secure door closed"
        '
        'HorzLimitPanel
        '
        Me.HorzLimitPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.HorzLimitPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HorzLimitPanel.Location = New System.Drawing.Point(103, 75)
        Me.HorzLimitPanel.Name = "HorzLimitPanel"
        Me.HorzLimitPanel.Size = New System.Drawing.Size(13, 13)
        Me.HorzLimitPanel.TabIndex = 14
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(167, 24)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(92, 13)
        Me.Label15.TabIndex = 23
        Me.Label15.Text = "Secure door open"
        '
        'VertHomePanel
        '
        Me.VertHomePanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.VertHomePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VertHomePanel.Location = New System.Drawing.Point(103, 24)
        Me.VertHomePanel.Name = "VertHomePanel"
        Me.VertHomePanel.Size = New System.Drawing.Size(13, 13)
        Me.VertHomePanel.TabIndex = 14
        '
        'VertLimitPanel
        '
        Me.VertLimitPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.VertLimitPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VertLimitPanel.Location = New System.Drawing.Point(103, 41)
        Me.VertLimitPanel.Name = "VertLimitPanel"
        Me.VertLimitPanel.Size = New System.Drawing.Size(13, 13)
        Me.VertLimitPanel.TabIndex = 15
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 58)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(84, 13)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "Horizontal home"
        '
        'DoorOpenPanel
        '
        Me.DoorOpenPanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.DoorOpenPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DoorOpenPanel.Location = New System.Drawing.Point(263, 41)
        Me.DoorOpenPanel.Name = "DoorOpenPanel"
        Me.DoorOpenPanel.Size = New System.Drawing.Size(13, 13)
        Me.DoorOpenPanel.TabIndex = 16
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(25, 24)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(71, 13)
        Me.Label17.TabIndex = 20
        Me.Label17.Text = "Vertical home"
        '
        'DoorClosePanel
        '
        Me.DoorClosePanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.DoorClosePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DoorClosePanel.Location = New System.Drawing.Point(263, 24)
        Me.DoorClosePanel.Name = "DoorClosePanel"
        Me.DoorClosePanel.Size = New System.Drawing.Size(13, 13)
        Me.DoorClosePanel.TabIndex = 14
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(33, 41)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(63, 13)
        Me.Label20.TabIndex = 19
        Me.Label20.Text = "Vertical limit"
        '
        'ArmHomePanel
        '
        Me.ArmHomePanel.BackColor = System.Drawing.Color.LightSlateGray
        Me.ArmHomePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ArmHomePanel.Location = New System.Drawing.Point(392, 24)
        Me.ArmHomePanel.Name = "ArmHomePanel"
        Me.ArmHomePanel.Size = New System.Drawing.Size(13, 13)
        Me.ArmHomePanel.TabIndex = 17
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(20, 75)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(76, 13)
        Me.Label21.TabIndex = 18
        Me.Label21.Text = "Horizontal limit"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(307, 364)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(113, 13)
        Me.Label22.TabIndex = 33
        Me.Label22.Text = "No VMC data recieved"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Red
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Location = New System.Drawing.Point(113, 364)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(13, 13)
        Me.Panel4.TabIndex = 30
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.Maroon
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Location = New System.Drawing.Point(207, 364)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(13, 13)
        Me.Panel5.TabIndex = 31
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.LightSlateGray
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Location = New System.Drawing.Point(292, 364)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(13, 13)
        Me.Panel6.TabIndex = 32
        '
        'uMechTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.MechGroup)
        Me.Controls.Add(Me.ArmGroup)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uMechTest"
        Me.Size = New System.Drawing.Size(422, 389)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ArmGroup.ResumeLayout(False)
        Me.MechGroup.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ArmGroup As System.Windows.Forms.GroupBox
    Friend WithEvents MechGroup As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents FanOnButton As System.Windows.Forms.Button
    Friend WithEvents VacuumOffButton As System.Windows.Forms.Button
    Friend WithEvents VacuumOnButton As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DoorCloseButton As System.Windows.Forms.Button
    Friend WithEvents DoorOpenButton As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents FanOffButton As System.Windows.Forms.Button
    Friend WithEvents CompressorOffButton As System.Windows.Forms.Button
    Friend WithEvents CompressorOnButton As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ArmKnobPanel As System.Windows.Forms.Panel
    Friend WithEvents RackKnobPanel As System.Windows.Forms.Panel
    Friend WithEvents MechVectorLabel As System.Windows.Forms.Label
    Friend WithEvents MechTestTimer As System.Windows.Forms.Timer
    Friend WithEvents ArmVectorLabel As System.Windows.Forms.Label
    Friend WithEvents HomeButton As System.Windows.Forms.Button
    Friend WithEvents LimitButton As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ProductHeldPanel As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents RearSensorPanel As System.Windows.Forms.Panel
    Friend WithEvents FrontSensorPanel As System.Windows.Forms.Panel
    Friend WithEvents ShelfDetectionPanel As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents HorzHomePanel As System.Windows.Forms.Panel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents HorzLimitPanel As System.Windows.Forms.Panel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents VertHomePanel As System.Windows.Forms.Panel
    Friend WithEvents VertLimitPanel As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents DoorOpenPanel As System.Windows.Forms.Panel
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents DoorClosePanel As System.Windows.Forms.Panel
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents ArmHomePanel As System.Windows.Forms.Panel
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel

End Class
