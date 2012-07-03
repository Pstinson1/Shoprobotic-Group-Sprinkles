<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fMain))
        Me.WatchdogTimer = New System.Timers.Timer()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.VideoButton = New System.Windows.Forms.Button()
        Me.SettingsButton = New System.Windows.Forms.Button()
        Me.ReturnToVendButton = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PaymentButton = New System.Windows.Forms.Button()
        Me.MechButton = New System.Windows.Forms.Button()
        Me.DebugButton = New System.Windows.Forms.Button()
        Me.SerialButton = New System.Windows.Forms.Button()
        Me.FridgeButton = New System.Windows.Forms.Button()
        Me.MainPanel = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ProductExplorer = New Configure.uProductExplorer()
        Me.ProductProperty = New Configure.uProduct()
        Me.PositionProperty = New Configure.uPosition()
        Me.SplashScreen = New Configure.uSplashScreen()
        CType(Me.WatchdogTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.MainPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'WatchdogTimer
        '
        Me.WatchdogTimer.Enabled = True
        Me.WatchdogTimer.Interval = 10000.0R
        Me.WatchdogTimer.SynchronizingObject = Me
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.White
        Me.ImageList1.Images.SetKeyName(0, "Position.bmp")
        Me.ImageList1.Images.SetKeyName(1, "StockLevels.bmp")
        Me.ImageList1.Images.SetKeyName(2, "Description.bmp")
        Me.ImageList1.Images.SetKeyName(3, "Administration.bmp")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2, Me.ToolStripStatusLabel3})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 552)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(800, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 28
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.AutoSize = False
        Me.ToolStripStatusLabel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ToolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(200, 17)
        Me.ToolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.AutoSize = False
        Me.ToolStripStatusLabel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ToolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(500, 17)
        Me.ToolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(640, 17)
        Me.ToolStripStatusLabel1.Text = "Ready."
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList2.Images.SetKeyName(0, "Favourite.png")
        '
        'VideoButton
        '
        Me.VideoButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.VideoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.VideoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.VideoButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VideoButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.VideoButton.Location = New System.Drawing.Point(171, 4)
        Me.VideoButton.Name = "VideoButton"
        Me.VideoButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.VideoButton.Size = New System.Drawing.Size(56, 56)
        Me.VideoButton.TabIndex = 22
        Me.VideoButton.Tag = ""
        Me.VideoButton.Text = "Video"
        Me.VideoButton.UseVisualStyleBackColor = True
        '
        'SettingsButton
        '
        Me.SettingsButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.SettingsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.SettingsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.SettingsButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SettingsButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SettingsButton.Location = New System.Drawing.Point(115, 4)
        Me.SettingsButton.Margin = New System.Windows.Forms.Padding(0)
        Me.SettingsButton.Name = "SettingsButton"
        Me.SettingsButton.Padding = New System.Windows.Forms.Padding(3)
        Me.SettingsButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.SettingsButton.Size = New System.Drawing.Size(56, 56)
        Me.SettingsButton.TabIndex = 23
        Me.SettingsButton.Tag = ""
        Me.SettingsButton.Text = "Sett- ings"
        Me.SettingsButton.UseVisualStyleBackColor = True
        '
        'ReturnToVendButton
        '
        Me.ReturnToVendButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.ReturnToVendButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.ReturnToVendButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.ReturnToVendButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReturnToVendButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ReturnToVendButton.Location = New System.Drawing.Point(395, 4)
        Me.ReturnToVendButton.Name = "ReturnToVendButton"
        Me.ReturnToVendButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ReturnToVendButton.Size = New System.Drawing.Size(56, 56)
        Me.ReturnToVendButton.TabIndex = 21
        Me.ReturnToVendButton.Tag = ""
        Me.ReturnToVendButton.Text = "Back to Vend"
        Me.ReturnToVendButton.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel2.Controls.Add(Me.PaymentButton)
        Me.Panel2.Controls.Add(Me.MechButton)
        Me.Panel2.Controls.Add(Me.DebugButton)
        Me.Panel2.Controls.Add(Me.SerialButton)
        Me.Panel2.Controls.Add(Me.FridgeButton)
        Me.Panel2.Controls.Add(Me.ReturnToVendButton)
        Me.Panel2.Controls.Add(Me.SettingsButton)
        Me.Panel2.Controls.Add(Me.VideoButton)
        Me.Panel2.Location = New System.Drawing.Point(334, 1)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(454, 66)
        Me.Panel2.TabIndex = 103
        '
        'PaymentButton
        '
        Me.PaymentButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.PaymentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.PaymentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.PaymentButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PaymentButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PaymentButton.Location = New System.Drawing.Point(339, 4)
        Me.PaymentButton.Margin = New System.Windows.Forms.Padding(1)
        Me.PaymentButton.Name = "PaymentButton"
        Me.PaymentButton.Padding = New System.Windows.Forms.Padding(2)
        Me.PaymentButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.PaymentButton.Size = New System.Drawing.Size(56, 56)
        Me.PaymentButton.TabIndex = 29
        Me.PaymentButton.Tag = ""
        Me.PaymentButton.Text = "Pay ment"
        Me.PaymentButton.UseVisualStyleBackColor = True
        '
        'MechButton
        '
        Me.MechButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.MechButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.MechButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.MechButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MechButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MechButton.Location = New System.Drawing.Point(283, 4)
        Me.MechButton.Name = "MechButton"
        Me.MechButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MechButton.Size = New System.Drawing.Size(56, 56)
        Me.MechButton.TabIndex = 28
        Me.MechButton.Tag = ""
        Me.MechButton.Text = "Mech"
        Me.MechButton.UseVisualStyleBackColor = True
        '
        'DebugButton
        '
        Me.DebugButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.DebugButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.DebugButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.DebugButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DebugButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DebugButton.Location = New System.Drawing.Point(227, 4)
        Me.DebugButton.Name = "DebugButton"
        Me.DebugButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.DebugButton.Size = New System.Drawing.Size(56, 56)
        Me.DebugButton.TabIndex = 26
        Me.DebugButton.Tag = ""
        Me.DebugButton.Text = "Debug"
        Me.DebugButton.UseVisualStyleBackColor = True
        '
        'SerialButton
        '
        Me.SerialButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.SerialButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.SerialButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.SerialButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SerialButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SerialButton.Location = New System.Drawing.Point(3, 4)
        Me.SerialButton.Name = "SerialButton"
        Me.SerialButton.Size = New System.Drawing.Size(56, 56)
        Me.SerialButton.TabIndex = 25
        Me.SerialButton.Tag = ""
        Me.SerialButton.Text = "Serial"
        Me.SerialButton.UseVisualStyleBackColor = True
        '
        'FridgeButton
        '
        Me.FridgeButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.FridgeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.FridgeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.FridgeButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FridgeButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FridgeButton.Location = New System.Drawing.Point(59, 4)
        Me.FridgeButton.Name = "FridgeButton"
        Me.FridgeButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.FridgeButton.Size = New System.Drawing.Size(56, 56)
        Me.FridgeButton.TabIndex = 24
        Me.FridgeButton.Tag = ""
        Me.FridgeButton.Text = "Fridge"
        Me.FridgeButton.UseVisualStyleBackColor = True
        '
        'MainPanel
        '
        Me.MainPanel.Controls.Add(Me.Button1)
        Me.MainPanel.Controls.Add(Me.ProductExplorer)
        Me.MainPanel.Controls.Add(Me.Panel2)
        Me.MainPanel.Controls.Add(Me.ProductProperty)
        Me.MainPanel.Controls.Add(Me.PositionProperty)
        Me.MainPanel.Controls.Add(Me.SplashScreen)
        Me.MainPanel.Location = New System.Drawing.Point(1, 2)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.Size = New System.Drawing.Size(794, 544)
        Me.MainPanel.TabIndex = 105
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(561, 481)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 106
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'ProductExplorer
        '
        Me.ProductExplorer.AutoSize = True
        Me.ProductExplorer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ProductExplorer.BackColor = System.Drawing.Color.Transparent
        Me.ProductExplorer.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProductExplorer.Location = New System.Drawing.Point(1, 1)
        Me.ProductExplorer.Margin = New System.Windows.Forms.Padding(0)
        Me.ProductExplorer.Name = "ProductExplorer"
        Me.ProductExplorer.Size = New System.Drawing.Size(332, 543)
        Me.ProductExplorer.TabIndex = 29
        '
        'ProductProperty
        '
        Me.ProductProperty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ProductProperty.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProductProperty.Location = New System.Drawing.Point(334, 69)
        Me.ProductProperty.Margin = New System.Windows.Forms.Padding(0)
        Me.ProductProperty.Name = "ProductProperty"
        Me.ProductProperty.Size = New System.Drawing.Size(458, 472)
        Me.ProductProperty.TabIndex = 0
        Me.ProductProperty.Visible = False
        '
        'PositionProperty
        '
        Me.PositionProperty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PositionProperty.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PositionProperty.Location = New System.Drawing.Point(334, 69)
        Me.PositionProperty.Name = "PositionProperty"
        Me.PositionProperty.Size = New System.Drawing.Size(458, 472)
        Me.PositionProperty.TabIndex = 101
        Me.PositionProperty.Visible = False
        '
        'SplashScreen
        '
        Me.SplashScreen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplashScreen.Location = New System.Drawing.Point(334, 69)
        Me.SplashScreen.Name = "SplashScreen"
        Me.SplashScreen.Size = New System.Drawing.Size(458, 472)
        Me.SplashScreen.TabIndex = 104
        '
        'fMain
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(800, 574)
        Me.Controls.Add(Me.MainPanel)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Configuration"
        CType(Me.WatchdogTimer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.MainPanel.ResumeLayout(False)
        Me.MainPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Friend WithEvents ReturnToVendButton As System.Windows.Forms.Button
    Friend WithEvents WatchdogTimer As System.Timers.Timer
    Friend WithEvents VideoButton As System.Windows.Forms.Button
    Friend WithEvents SettingsButton As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
    Friend WithEvents PositionProperty As Configure.uPosition
    Friend WithEvents ProductExplorer As Configure.uProductExplorer
    Friend WithEvents ProductProperty As Configure.uProduct
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SplashScreen As Configure.uSplashScreen
    Friend WithEvents MainPanel As System.Windows.Forms.Panel
    Friend WithEvents FridgeButton As System.Windows.Forms.Button
    Friend WithEvents SerialButton As System.Windows.Forms.Button
    Friend WithEvents DebugButton As System.Windows.Forms.Button
    Friend WithEvents MechButton As System.Windows.Forms.Button
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents PaymentButton As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
