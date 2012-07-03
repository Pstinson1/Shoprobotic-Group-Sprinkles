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
        Me.components = New System.ComponentModel.Container
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.VmcSettingsTab = New System.Windows.Forms.TabPage
        Me.StopButton = New System.Windows.Forms.Button
        Me.StartButton = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.BlockSizeLabel = New System.Windows.Forms.Label
        Me.VerticalPositionLabel = New System.Windows.Forms.Label
        Me.HorizontalPositionLabel = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SwitchTestTimer = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundScanWorker = New System.ComponentModel.BackgroundWorker
        Me.MechButton = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.VmcSettingsTab.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.VmcSettingsTab)
        Me.TabControl1.Location = New System.Drawing.Point(2, 4)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(589, 434)
        Me.TabControl1.TabIndex = 0
        '
        'VmcSettingsTab
        '
        Me.VmcSettingsTab.Controls.Add(Me.MechButton)
        Me.VmcSettingsTab.Controls.Add(Me.StopButton)
        Me.VmcSettingsTab.Controls.Add(Me.StartButton)
        Me.VmcSettingsTab.Controls.Add(Me.GroupBox1)
        Me.VmcSettingsTab.Controls.Add(Me.PictureBox1)
        Me.VmcSettingsTab.Location = New System.Drawing.Point(4, 22)
        Me.VmcSettingsTab.Name = "VmcSettingsTab"
        Me.VmcSettingsTab.Padding = New System.Windows.Forms.Padding(3)
        Me.VmcSettingsTab.Size = New System.Drawing.Size(581, 408)
        Me.VmcSettingsTab.TabIndex = 0
        Me.VmcSettingsTab.Text = "Shelf scan"
        Me.VmcSettingsTab.UseVisualStyleBackColor = True
        '
        'StopButton
        '
        Me.StopButton.Location = New System.Drawing.Point(503, 7)
        Me.StopButton.Name = "StopButton"
        Me.StopButton.Size = New System.Drawing.Size(75, 34)
        Me.StopButton.TabIndex = 3
        Me.StopButton.Text = "Stop"
        Me.StopButton.UseVisualStyleBackColor = True
        '
        'StartButton
        '
        Me.StartButton.Location = New System.Drawing.Point(503, 43)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 34)
        Me.StartButton.TabIndex = 2
        Me.StartButton.Text = "Start"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.BlockSizeLabel)
        Me.GroupBox1.Controls.Add(Me.VerticalPositionLabel)
        Me.GroupBox1.Controls.Add(Me.HorizontalPositionLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 332)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(569, 68)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Knowns"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(421, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Block size"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(401, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Head position"
        '
        'BlockSizeLabel
        '
        Me.BlockSizeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BlockSizeLabel.Location = New System.Drawing.Point(479, 40)
        Me.BlockSizeLabel.Name = "BlockSizeLabel"
        Me.BlockSizeLabel.Size = New System.Drawing.Size(39, 18)
        Me.BlockSizeLabel.TabIndex = 2
        Me.BlockSizeLabel.Text = "-"
        Me.BlockSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'VerticalPositionLabel
        '
        Me.VerticalPositionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VerticalPositionLabel.Location = New System.Drawing.Point(520, 21)
        Me.VerticalPositionLabel.Name = "VerticalPositionLabel"
        Me.VerticalPositionLabel.Size = New System.Drawing.Size(39, 18)
        Me.VerticalPositionLabel.TabIndex = 1
        Me.VerticalPositionLabel.Text = "-"
        Me.VerticalPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HorizontalPositionLabel
        '
        Me.HorizontalPositionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HorizontalPositionLabel.Location = New System.Drawing.Point(479, 21)
        Me.HorizontalPositionLabel.Name = "HorizontalPositionLabel"
        Me.HorizontalPositionLabel.Size = New System.Drawing.Size(39, 18)
        Me.HorizontalPositionLabel.TabIndex = 0
        Me.HorizontalPositionLabel.Text = "-"
        Me.HorizontalPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Black
        Me.PictureBox1.Location = New System.Drawing.Point(3, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(494, 318)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'SwitchTestTimer
        '
        Me.SwitchTestTimer.Interval = 200
        '
        'BackgroundScanWorker
        '
        '
        'MechButton
        '
        Me.MechButton.Location = New System.Drawing.Point(500, 292)
        Me.MechButton.Name = "MechButton"
        Me.MechButton.Size = New System.Drawing.Size(75, 34)
        Me.MechButton.TabIndex = 4
        Me.MechButton.Text = "Mech"
        Me.MechButton.UseVisualStyleBackColor = True
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 479)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fMain"
        Me.Text = "Mechanical management."
        Me.TabControl1.ResumeLayout(False)
        Me.VmcSettingsTab.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents VmcSettingsTab As System.Windows.Forms.TabPage
    Friend WithEvents SwitchTestTimer As System.Windows.Forms.Timer
    Friend WithEvents BackgroundScanWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents StopButton As System.Windows.Forms.Button
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents VerticalPositionLabel As System.Windows.Forms.Label
    Friend WithEvents HorizontalPositionLabel As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BlockSizeLabel As System.Windows.Forms.Label
    Friend WithEvents MechButton As System.Windows.Forms.Button

End Class
