<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uServiceState
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
        Me.StateListBox = New System.Windows.Forms.ListBox()
        Me.ServiceButton = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SoftwareResetButton = New System.Windows.Forms.Button()
        Me.HardwareResetButton = New System.Windows.Forms.Button()
        Me.ServiceFlagsListBox = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'StateListBox
        '
        Me.StateListBox.Enabled = False
        Me.StateListBox.ForeColor = System.Drawing.Color.Black
        Me.StateListBox.FormattingEnabled = True
        Me.StateListBox.Items.AddRange(New Object() {"VMC_INITIALISING", "VMC_READY", "VMC_SERVICE_MODE", "VMC_SERVICE_REQUIRED", "VMC_BUSY", "VMC_DEAD", "VMC_NOT_USING_HEARTBEAT"})
        Me.StateListBox.Location = New System.Drawing.Point(141, 3)
        Me.StateListBox.Name = "StateListBox"
        Me.StateListBox.Size = New System.Drawing.Size(281, 95)
        Me.StateListBox.TabIndex = 0
        '
        'ServiceButton
        '
        Me.ServiceButton.Location = New System.Drawing.Point(330, 332)
        Me.ServiceButton.Name = "ServiceButton"
        Me.ServiceButton.Size = New System.Drawing.Size(91, 45)
        Me.ServiceButton.TabIndex = 1
        Me.ServiceButton.Text = "Remove Service Flag"
        Me.ServiceButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Last known service state"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusLabel
        '
        Me.StatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusLabel.Location = New System.Drawing.Point(141, 100)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(281, 20)
        Me.StatusLabel.TabIndex = 3
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(4, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Last activity state"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SoftwareResetButton
        '
        Me.SoftwareResetButton.Location = New System.Drawing.Point(91, 332)
        Me.SoftwareResetButton.Name = "SoftwareResetButton"
        Me.SoftwareResetButton.Size = New System.Drawing.Size(91, 45)
        Me.SoftwareResetButton.TabIndex = 5
        Me.SoftwareResetButton.Text = "Software Reset VMC"
        Me.SoftwareResetButton.UseVisualStyleBackColor = True
        '
        'HardwareResetButton
        '
        Me.HardwareResetButton.Location = New System.Drawing.Point(-1, 332)
        Me.HardwareResetButton.Name = "HardwareResetButton"
        Me.HardwareResetButton.Size = New System.Drawing.Size(91, 45)
        Me.HardwareResetButton.TabIndex = 6
        Me.HardwareResetButton.Text = "Hardware Reset VMC"
        Me.HardwareResetButton.UseVisualStyleBackColor = True
        '
        'ServiceFlagsListBox
        '
        Me.ServiceFlagsListBox.Enabled = False
        Me.ServiceFlagsListBox.ForeColor = System.Drawing.Color.Black
        Me.ServiceFlagsListBox.FormattingEnabled = True
        Me.ServiceFlagsListBox.Location = New System.Drawing.Point(141, 122)
        Me.ServiceFlagsListBox.Name = "ServiceFlagsListBox"
        Me.ServiceFlagsListBox.Size = New System.Drawing.Size(281, 69)
        Me.ServiceFlagsListBox.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(4, 122)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 20)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Service flags"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uServiceState
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ServiceFlagsListBox)
        Me.Controls.Add(Me.HardwareResetButton)
        Me.Controls.Add(Me.SoftwareResetButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.StatusLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ServiceButton)
        Me.Controls.Add(Me.StateListBox)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uServiceState"
        Me.Size = New System.Drawing.Size(422, 382)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StateListBox As System.Windows.Forms.ListBox
    Friend WithEvents ServiceButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents StatusLabel As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents SoftwareResetButton As System.Windows.Forms.Button
    Friend WithEvents HardwareResetButton As System.Windows.Forms.Button
    Friend WithEvents ServiceFlagsListBox As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
