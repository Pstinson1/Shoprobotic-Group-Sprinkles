<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uSplashScreen
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
        Me.SplashTabControl = New System.Windows.Forms.TabControl
        Me.InformationTab = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.AdministrationTab = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.SqlServiceNameLabel = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.StartSqlButton = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ShellButton = New System.Windows.Forms.Button
        Me.ExplorerButton = New System.Windows.Forms.Button
        Me.CheckPrinterButton = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.PrinterStatusLabel = New System.Windows.Forms.Label
        Me.PrinterNameLabel = New System.Windows.Forms.Label
        Me.ErrorDetailLabel = New System.Windows.Forms.Label
        Me.SplashTabControl.SuspendLayout()
        Me.InformationTab.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AdministrationTab.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplashTabControl
        '
        Me.SplashTabControl.Controls.Add(Me.InformationTab)
        Me.SplashTabControl.Controls.Add(Me.AdministrationTab)
        Me.SplashTabControl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SplashTabControl.ItemSize = New System.Drawing.Size(60, 44)
        Me.SplashTabControl.Location = New System.Drawing.Point(2, 2)
        Me.SplashTabControl.Name = "SplashTabControl"
        Me.SplashTabControl.SelectedIndex = 0
        Me.SplashTabControl.Size = New System.Drawing.Size(452, 466)
        Me.SplashTabControl.TabIndex = 2
        '
        'InformationTab
        '
        Me.InformationTab.Controls.Add(Me.Label1)
        Me.InformationTab.Controls.Add(Me.PictureBox1)
        Me.InformationTab.Location = New System.Drawing.Point(4, 48)
        Me.InformationTab.Name = "InformationTab"
        Me.InformationTab.Padding = New System.Windows.Forms.Padding(3)
        Me.InformationTab.Size = New System.Drawing.Size(444, 414)
        Me.InformationTab.TabIndex = 0
        Me.InformationTab.Text = "General Information"
        Me.InformationTab.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Trebuchet MS", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 358)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(432, 53)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please navigate to a category, product, or position using the explorer panel."
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Configure.My.Resources.Resources.SHR_logo
        Me.PictureBox1.Location = New System.Drawing.Point(75, 54)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(291, 254)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'AdministrationTab
        '
        Me.AdministrationTab.Controls.Add(Me.GroupBox3)
        Me.AdministrationTab.Controls.Add(Me.GroupBox2)
        Me.AdministrationTab.Controls.Add(Me.GroupBox1)
        Me.AdministrationTab.Location = New System.Drawing.Point(4, 48)
        Me.AdministrationTab.Name = "AdministrationTab"
        Me.AdministrationTab.Padding = New System.Windows.Forms.Padding(3)
        Me.AdministrationTab.Size = New System.Drawing.Size(444, 414)
        Me.AdministrationTab.TabIndex = 1
        Me.AdministrationTab.Text = "Administration"
        Me.AdministrationTab.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.SqlServiceNameLabel)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.StartSqlButton)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 254)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(432, 74)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "SQL service."
        Me.GroupBox2.Visible = False
        '
        'SqlServiceNameLabel
        '
        Me.SqlServiceNameLabel.Location = New System.Drawing.Point(10, 30)
        Me.SqlServiceNameLabel.Name = "SqlServiceNameLabel"
        Me.SqlServiceNameLabel.Size = New System.Drawing.Size(264, 16)
        Me.SqlServiceNameLabel.TabIndex = 3
        Me.SqlServiceNameLabel.Text = "Service name not determined"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(10, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(264, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Start or stop this service"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(351, 31)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(68, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Stop"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'StartSqlButton
        '
        Me.StartSqlButton.Location = New System.Drawing.Point(280, 31)
        Me.StartSqlButton.Name = "StartSqlButton"
        Me.StartSqlButton.Size = New System.Drawing.Size(68, 32)
        Me.StartSqlButton.TabIndex = 0
        Me.StartSqlButton.Text = "Start"
        Me.StartSqlButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.ShellButton)
        Me.GroupBox1.Controls.Add(Me.ExplorerButton)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 334)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(432, 74)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Boot PC To"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(264, 32)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Set the registry to cause the PC to boot to the following on power up"
        '
        'ShellButton
        '
        Me.ShellButton.Location = New System.Drawing.Point(351, 31)
        Me.ShellButton.Name = "ShellButton"
        Me.ShellButton.Size = New System.Drawing.Size(68, 32)
        Me.ShellButton.TabIndex = 1
        Me.ShellButton.Text = "Vend Shell"
        Me.ShellButton.UseVisualStyleBackColor = True
        '
        'ExplorerButton
        '
        Me.ExplorerButton.Location = New System.Drawing.Point(280, 31)
        Me.ExplorerButton.Name = "ExplorerButton"
        Me.ExplorerButton.Size = New System.Drawing.Size(68, 32)
        Me.ExplorerButton.TabIndex = 0
        Me.ExplorerButton.Text = "Explorer"
        Me.ExplorerButton.UseVisualStyleBackColor = True
        '
        'CheckPrinterButton
        '
        Me.CheckPrinterButton.Location = New System.Drawing.Point(351, 28)
        Me.CheckPrinterButton.Name = "CheckPrinterButton"
        Me.CheckPrinterButton.Size = New System.Drawing.Size(68, 32)
        Me.CheckPrinterButton.TabIndex = 3
        Me.CheckPrinterButton.Text = "Check"
        Me.CheckPrinterButton.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ErrorDetailLabel)
        Me.GroupBox3.Controls.Add(Me.PrinterNameLabel)
        Me.GroupBox3.Controls.Add(Me.PrinterStatusLabel)
        Me.GroupBox3.Controls.Add(Me.CheckPrinterButton)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 7)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(432, 71)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Printer"
        '
        'PrinterStatusLabel
        '
        Me.PrinterStatusLabel.Location = New System.Drawing.Point(12, 41)
        Me.PrinterStatusLabel.Name = "PrinterStatusLabel"
        Me.PrinterStatusLabel.Size = New System.Drawing.Size(264, 16)
        Me.PrinterStatusLabel.TabIndex = 5
        Me.PrinterStatusLabel.Text = "Status: Not Determined"
        '
        'PrinterNameLabel
        '
        Me.PrinterNameLabel.Location = New System.Drawing.Point(12, 25)
        Me.PrinterNameLabel.Name = "PrinterNameLabel"
        Me.PrinterNameLabel.Size = New System.Drawing.Size(264, 16)
        Me.PrinterNameLabel.TabIndex = 6
        Me.PrinterNameLabel.Text = "Printer not yet determined."
        '
        'ErrorDetailLabel
        '
        Me.ErrorDetailLabel.Location = New System.Drawing.Point(267, 41)
        Me.ErrorDetailLabel.Name = "ErrorDetailLabel"
        Me.ErrorDetailLabel.Size = New System.Drawing.Size(78, 16)
        Me.ErrorDetailLabel.TabIndex = 7
        Me.ErrorDetailLabel.Text = "-,-"
        Me.ErrorDetailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uSplashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplashTabControl)
        Me.Name = "uSplashScreen"
        Me.Size = New System.Drawing.Size(454, 469)
        Me.SplashTabControl.ResumeLayout(False)
        Me.InformationTab.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AdministrationTab.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplashTabControl As System.Windows.Forms.TabControl
    Friend WithEvents InformationTab As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents AdministrationTab As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ExplorerButton As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ShellButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents StartSqlButton As System.Windows.Forms.Button
    Friend WithEvents SqlServiceNameLabel As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents PrinterStatusLabel As System.Windows.Forms.Label
    Friend WithEvents CheckPrinterButton As System.Windows.Forms.Button
    Friend WithEvents PrinterNameLabel As System.Windows.Forms.Label
    Friend WithEvents ErrorDetailLabel As System.Windows.Forms.Label

End Class
