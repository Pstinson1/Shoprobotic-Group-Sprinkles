<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uSerialLoader
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
        Me.StartCommsButton = New System.Windows.Forms.Button
        Me.ResetButton = New System.Windows.Forms.Button
        Me.ProgramButton = New System.Windows.Forms.Button
        Me.FileSelectButton = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.FilePathLabel = New System.Windows.Forms.Label
        Me.ProgressList = New System.Windows.Forms.ListBox
        Me.OpenHexFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.StopCommsButton = New System.Windows.Forms.Button
        Me.CaptureButton = New System.Windows.Forms.Button
        Me.DeviceIdLabel = New System.Windows.Forms.Label
        Me.DeviceStatusLabel = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StartCommsButton
        '
        Me.StartCommsButton.Location = New System.Drawing.Point(3, 214)
        Me.StartCommsButton.Name = "StartCommsButton"
        Me.StartCommsButton.Size = New System.Drawing.Size(75, 37)
        Me.StartCommsButton.TabIndex = 0
        Me.StartCommsButton.Text = "Start Comms"
        Me.StartCommsButton.UseVisualStyleBackColor = True
        '
        'ResetButton
        '
        Me.ResetButton.Location = New System.Drawing.Point(155, 214)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(75, 37)
        Me.ResetButton.TabIndex = 2
        Me.ResetButton.Text = "Reset"
        Me.ResetButton.UseVisualStyleBackColor = True
        '
        'ProgramButton
        '
        Me.ProgramButton.Location = New System.Drawing.Point(345, 214)
        Me.ProgramButton.Name = "ProgramButton"
        Me.ProgramButton.Size = New System.Drawing.Size(75, 37)
        Me.ProgramButton.TabIndex = 3
        Me.ProgramButton.Text = "Program"
        Me.ProgramButton.UseVisualStyleBackColor = True
        '
        'FileSelectButton
        '
        Me.FileSelectButton.Location = New System.Drawing.Point(368, 23)
        Me.FileSelectButton.Name = "FileSelectButton"
        Me.FileSelectButton.Size = New System.Drawing.Size(37, 22)
        Me.FileSelectButton.TabIndex = 4
        Me.FileSelectButton.Text = "..."
        Me.FileSelectButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.FilePathLabel)
        Me.GroupBox1.Controls.Add(Me.FileSelectButton)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 153)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(415, 52)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Source File"
        '
        'FilePathLabel
        '
        Me.FilePathLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.FilePathLabel.Location = New System.Drawing.Point(8, 22)
        Me.FilePathLabel.Name = "FilePathLabel"
        Me.FilePathLabel.Size = New System.Drawing.Size(354, 22)
        Me.FilePathLabel.TabIndex = 5
        Me.FilePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressList
        '
        Me.ProgressList.FormattingEnabled = True
        Me.ProgressList.Location = New System.Drawing.Point(4, 4)
        Me.ProgressList.Name = "ProgressList"
        Me.ProgressList.Size = New System.Drawing.Size(296, 147)
        Me.ProgressList.TabIndex = 6
        '
        'OpenHexFileDialog
        '
        Me.OpenHexFileDialog.Filter = "Hex code files|*.hex"
        Me.OpenHexFileDialog.Multiselect = True
        '
        'StopCommsButton
        '
        Me.StopCommsButton.Location = New System.Drawing.Point(79, 214)
        Me.StopCommsButton.Name = "StopCommsButton"
        Me.StopCommsButton.Size = New System.Drawing.Size(75, 37)
        Me.StopCommsButton.TabIndex = 1
        Me.StopCommsButton.Text = "Stop Comms"
        Me.StopCommsButton.UseVisualStyleBackColor = True
        '
        'CaptureButton
        '
        Me.CaptureButton.Location = New System.Drawing.Point(236, 214)
        Me.CaptureButton.Name = "CaptureButton"
        Me.CaptureButton.Size = New System.Drawing.Size(75, 37)
        Me.CaptureButton.TabIndex = 7
        Me.CaptureButton.Text = "Capture"
        Me.CaptureButton.UseVisualStyleBackColor = True
        '
        'DeviceIdLabel
        '
        Me.DeviceIdLabel.AutoSize = True
        Me.DeviceIdLabel.Location = New System.Drawing.Point(307, 4)
        Me.DeviceIdLabel.Name = "DeviceIdLabel"
        Me.DeviceIdLabel.Size = New System.Drawing.Size(75, 13)
        Me.DeviceIdLabel.TabIndex = 8
        Me.DeviceIdLabel.Text = "Device Id: ----"
        '
        'DeviceStatusLabel
        '
        Me.DeviceStatusLabel.AutoSize = True
        Me.DeviceStatusLabel.Location = New System.Drawing.Point(310, 21)
        Me.DeviceStatusLabel.Name = "DeviceStatusLabel"
        Me.DeviceStatusLabel.Size = New System.Drawing.Size(61, 13)
        Me.DeviceStatusLabel.TabIndex = 9
        Me.DeviceStatusLabel.Text = "Status: ----"
        '
        'uSerialLoader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DeviceStatusLabel)
        Me.Controls.Add(Me.DeviceIdLabel)
        Me.Controls.Add(Me.CaptureButton)
        Me.Controls.Add(Me.ProgressList)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ProgramButton)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.StopCommsButton)
        Me.Controls.Add(Me.StartCommsButton)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uSerialLoader"
        Me.Size = New System.Drawing.Size(422, 254)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StartCommsButton As System.Windows.Forms.Button
    Friend WithEvents ResetButton As System.Windows.Forms.Button
    Friend WithEvents ProgramButton As System.Windows.Forms.Button
    Friend WithEvents FileSelectButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents FilePathLabel As System.Windows.Forms.Label
    Friend WithEvents ProgressList As System.Windows.Forms.ListBox
    Friend WithEvents OpenHexFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents StopCommsButton As System.Windows.Forms.Button
    Friend WithEvents CaptureButton As System.Windows.Forms.Button
    Friend WithEvents DeviceIdLabel As System.Windows.Forms.Label
    Friend WithEvents DeviceStatusLabel As System.Windows.Forms.Label

End Class
