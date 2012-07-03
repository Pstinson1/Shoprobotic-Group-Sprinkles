<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fVideoManager
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
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.PickHeadLabel = New System.Windows.Forms.Label
        Me.ShowPickHeadButton = New System.Windows.Forms.Button
        Me.HidePickHeadButton = New System.Windows.Forms.Button
        Me.RecordPickHeadButton = New System.Windows.Forms.Button
        Me.StopPickHeadButton = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.MechanismLabel = New System.Windows.Forms.Label
        Me.ShowMechanismButton = New System.Windows.Forms.Button
        Me.HideMechanismButton = New System.Windows.Forms.Button
        Me.RecordMechanismButton = New System.Windows.Forms.Button
        Me.StopMechanismButton = New System.Windows.Forms.Button
        Me.DeviceListBox = New System.Windows.Forms.ListBox
        Me.CompressorListBox = New System.Windows.Forms.ListBox
        Me.StopCustomerButton = New System.Windows.Forms.Button
        Me.RecordCustomerButton = New System.Windows.Forms.Button
        Me.HideCustomerButton = New System.Windows.Forms.Button
        Me.ShowCustomerButton = New System.Windows.Forms.Button
        Me.CustomerLabel = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.HideButton = New System.Windows.Forms.Button
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 303)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(441, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.PickHeadLabel)
        Me.GroupBox2.Controls.Add(Me.ShowPickHeadButton)
        Me.GroupBox2.Controls.Add(Me.HidePickHeadButton)
        Me.GroupBox2.Controls.Add(Me.RecordPickHeadButton)
        Me.GroupBox2.Controls.Add(Me.StopPickHeadButton)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(428, 58)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Pick Head"
        '
        'PickHeadLabel
        '
        Me.PickHeadLabel.Location = New System.Drawing.Point(9, 20)
        Me.PickHeadLabel.Name = "PickHeadLabel"
        Me.PickHeadLabel.Size = New System.Drawing.Size(172, 35)
        Me.PickHeadLabel.TabIndex = 6
        '
        'ShowPickHeadButton
        '
        Me.ShowPickHeadButton.Location = New System.Drawing.Point(182, 23)
        Me.ShowPickHeadButton.Name = "ShowPickHeadButton"
        Me.ShowPickHeadButton.Size = New System.Drawing.Size(59, 27)
        Me.ShowPickHeadButton.TabIndex = 5
        Me.ShowPickHeadButton.Tag = "0"
        Me.ShowPickHeadButton.Text = "Show"
        Me.ShowPickHeadButton.UseVisualStyleBackColor = True
        '
        'HidePickHeadButton
        '
        Me.HidePickHeadButton.Location = New System.Drawing.Point(242, 23)
        Me.HidePickHeadButton.Name = "HidePickHeadButton"
        Me.HidePickHeadButton.Size = New System.Drawing.Size(59, 27)
        Me.HidePickHeadButton.TabIndex = 4
        Me.HidePickHeadButton.Tag = "0"
        Me.HidePickHeadButton.Text = "Hide"
        Me.HidePickHeadButton.UseVisualStyleBackColor = True
        '
        'RecordPickHeadButton
        '
        Me.RecordPickHeadButton.Location = New System.Drawing.Point(303, 23)
        Me.RecordPickHeadButton.Name = "RecordPickHeadButton"
        Me.RecordPickHeadButton.Size = New System.Drawing.Size(59, 27)
        Me.RecordPickHeadButton.TabIndex = 2
        Me.RecordPickHeadButton.Tag = "0"
        Me.RecordPickHeadButton.Text = "Record"
        Me.RecordPickHeadButton.UseVisualStyleBackColor = True
        '
        'StopPickHeadButton
        '
        Me.StopPickHeadButton.Location = New System.Drawing.Point(363, 23)
        Me.StopPickHeadButton.Name = "StopPickHeadButton"
        Me.StopPickHeadButton.Size = New System.Drawing.Size(59, 27)
        Me.StopPickHeadButton.TabIndex = 3
        Me.StopPickHeadButton.Tag = "0"
        Me.StopPickHeadButton.Text = "Stop"
        Me.StopPickHeadButton.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.MechanismLabel)
        Me.GroupBox3.Controls.Add(Me.ShowMechanismButton)
        Me.GroupBox3.Controls.Add(Me.HideMechanismButton)
        Me.GroupBox3.Controls.Add(Me.RecordMechanismButton)
        Me.GroupBox3.Controls.Add(Me.StopMechanismButton)
        Me.GroupBox3.Location = New System.Drawing.Point(7, 129)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(428, 58)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Mechanism"
        '
        'MechanismLabel
        '
        Me.MechanismLabel.Location = New System.Drawing.Point(9, 20)
        Me.MechanismLabel.Name = "MechanismLabel"
        Me.MechanismLabel.Size = New System.Drawing.Size(172, 35)
        Me.MechanismLabel.TabIndex = 6
        '
        'ShowMechanismButton
        '
        Me.ShowMechanismButton.Location = New System.Drawing.Point(182, 23)
        Me.ShowMechanismButton.Name = "ShowMechanismButton"
        Me.ShowMechanismButton.Size = New System.Drawing.Size(59, 27)
        Me.ShowMechanismButton.TabIndex = 5
        Me.ShowMechanismButton.Tag = "2"
        Me.ShowMechanismButton.Text = "Show"
        Me.ShowMechanismButton.UseVisualStyleBackColor = True
        '
        'HideMechanismButton
        '
        Me.HideMechanismButton.Location = New System.Drawing.Point(242, 23)
        Me.HideMechanismButton.Name = "HideMechanismButton"
        Me.HideMechanismButton.Size = New System.Drawing.Size(59, 27)
        Me.HideMechanismButton.TabIndex = 4
        Me.HideMechanismButton.Tag = "2"
        Me.HideMechanismButton.Text = "Hide"
        Me.HideMechanismButton.UseVisualStyleBackColor = True
        '
        'RecordMechanismButton
        '
        Me.RecordMechanismButton.Location = New System.Drawing.Point(303, 23)
        Me.RecordMechanismButton.Name = "RecordMechanismButton"
        Me.RecordMechanismButton.Size = New System.Drawing.Size(59, 27)
        Me.RecordMechanismButton.TabIndex = 2
        Me.RecordMechanismButton.Tag = "2"
        Me.RecordMechanismButton.Text = "Record"
        Me.RecordMechanismButton.UseVisualStyleBackColor = True
        '
        'StopMechanismButton
        '
        Me.StopMechanismButton.Location = New System.Drawing.Point(363, 23)
        Me.StopMechanismButton.Name = "StopMechanismButton"
        Me.StopMechanismButton.Size = New System.Drawing.Size(59, 27)
        Me.StopMechanismButton.TabIndex = 3
        Me.StopMechanismButton.Tag = "2"
        Me.StopMechanismButton.Text = "Stop"
        Me.StopMechanismButton.UseVisualStyleBackColor = True
        '
        'DeviceListBox
        '
        Me.DeviceListBox.FormattingEnabled = True
        Me.DeviceListBox.Location = New System.Drawing.Point(183, 20)
        Me.DeviceListBox.Name = "DeviceListBox"
        Me.DeviceListBox.Size = New System.Drawing.Size(175, 69)
        Me.DeviceListBox.TabIndex = 8
        '
        'CompressorListBox
        '
        Me.CompressorListBox.FormattingEnabled = True
        Me.CompressorListBox.Location = New System.Drawing.Point(6, 20)
        Me.CompressorListBox.Name = "CompressorListBox"
        Me.CompressorListBox.Size = New System.Drawing.Size(175, 69)
        Me.CompressorListBox.TabIndex = 9
        '
        'StopCustomerButton
        '
        Me.StopCustomerButton.Location = New System.Drawing.Point(363, 23)
        Me.StopCustomerButton.Name = "StopCustomerButton"
        Me.StopCustomerButton.Size = New System.Drawing.Size(59, 27)
        Me.StopCustomerButton.TabIndex = 3
        Me.StopCustomerButton.Tag = "1"
        Me.StopCustomerButton.Text = "Stop"
        Me.StopCustomerButton.UseVisualStyleBackColor = True
        '
        'RecordCustomerButton
        '
        Me.RecordCustomerButton.Location = New System.Drawing.Point(303, 23)
        Me.RecordCustomerButton.Name = "RecordCustomerButton"
        Me.RecordCustomerButton.Size = New System.Drawing.Size(59, 27)
        Me.RecordCustomerButton.TabIndex = 2
        Me.RecordCustomerButton.Tag = "1"
        Me.RecordCustomerButton.Text = "Record"
        Me.RecordCustomerButton.UseVisualStyleBackColor = True
        '
        'HideCustomerButton
        '
        Me.HideCustomerButton.Location = New System.Drawing.Point(242, 23)
        Me.HideCustomerButton.Name = "HideCustomerButton"
        Me.HideCustomerButton.Size = New System.Drawing.Size(59, 27)
        Me.HideCustomerButton.TabIndex = 4
        Me.HideCustomerButton.Tag = "1"
        Me.HideCustomerButton.Text = "Hide"
        Me.HideCustomerButton.UseVisualStyleBackColor = True
        '
        'ShowCustomerButton
        '
        Me.ShowCustomerButton.Location = New System.Drawing.Point(182, 23)
        Me.ShowCustomerButton.Name = "ShowCustomerButton"
        Me.ShowCustomerButton.Size = New System.Drawing.Size(59, 27)
        Me.ShowCustomerButton.TabIndex = 5
        Me.ShowCustomerButton.Tag = "1"
        Me.ShowCustomerButton.Text = "Show"
        Me.ShowCustomerButton.UseVisualStyleBackColor = True
        '
        'CustomerLabel
        '
        Me.CustomerLabel.Location = New System.Drawing.Point(9, 20)
        Me.CustomerLabel.Name = "CustomerLabel"
        Me.CustomerLabel.Size = New System.Drawing.Size(172, 35)
        Me.CustomerLabel.TabIndex = 6
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CustomerLabel)
        Me.GroupBox1.Controls.Add(Me.ShowCustomerButton)
        Me.GroupBox1.Controls.Add(Me.HideCustomerButton)
        Me.GroupBox1.Controls.Add(Me.RecordCustomerButton)
        Me.GroupBox1.Controls.Add(Me.StopCustomerButton)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 66)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(428, 58)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Customer"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.CompressorListBox)
        Me.GroupBox4.Controls.Add(Me.DeviceListBox)
        Me.GroupBox4.Location = New System.Drawing.Point(7, 193)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(365, 100)
        Me.GroupBox4.TabIndex = 10
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Compressors and devices"
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(378, 266)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 11
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'fVideoManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fVideoManager"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Video Inputs"
        Me.TopMost = True
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents PickHeadLabel As System.Windows.Forms.Label
    Friend WithEvents ShowPickHeadButton As System.Windows.Forms.Button
    Friend WithEvents HidePickHeadButton As System.Windows.Forms.Button
    Friend WithEvents RecordPickHeadButton As System.Windows.Forms.Button
    Friend WithEvents StopPickHeadButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents MechanismLabel As System.Windows.Forms.Label
    Friend WithEvents ShowMechanismButton As System.Windows.Forms.Button
    Friend WithEvents HideMechanismButton As System.Windows.Forms.Button
    Friend WithEvents RecordMechanismButton As System.Windows.Forms.Button
    Friend WithEvents StopMechanismButton As System.Windows.Forms.Button
    Friend WithEvents DeviceListBox As System.Windows.Forms.ListBox
    Friend WithEvents CompressorListBox As System.Windows.Forms.ListBox
    Friend WithEvents StopCustomerButton As System.Windows.Forms.Button
    Friend WithEvents RecordCustomerButton As System.Windows.Forms.Button
    Friend WithEvents HideCustomerButton As System.Windows.Forms.Button
    Friend WithEvents ShowCustomerButton As System.Windows.Forms.Button
    Friend WithEvents CustomerLabel As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents HideButton As System.Windows.Forms.Button
End Class
