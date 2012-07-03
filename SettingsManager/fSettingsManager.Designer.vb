<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fSettingsManager
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
        Me.GroupTabControl = New System.Windows.Forms.TabControl
        Me.HideButton = New System.Windows.Forms.Button
        Me.PrintButton = New System.Windows.Forms.Button
        Me.ExplainationLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'GroupTabControl
        '
        Me.GroupTabControl.ItemSize = New System.Drawing.Size(0, 34)
        Me.GroupTabControl.Location = New System.Drawing.Point(5, 3)
        Me.GroupTabControl.Name = "GroupTabControl"
        Me.GroupTabControl.SelectedIndex = 0
        Me.GroupTabControl.Size = New System.Drawing.Size(433, 284)
        Me.GroupTabControl.TabIndex = 2
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(378, 288)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 12
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'PrintButton
        '
        Me.PrintButton.Location = New System.Drawing.Point(316, 288)
        Me.PrintButton.Name = "PrintButton"
        Me.PrintButton.Size = New System.Drawing.Size(59, 34)
        Me.PrintButton.TabIndex = 13
        Me.PrintButton.Text = "Print All"
        Me.PrintButton.UseVisualStyleBackColor = True
        '
        'ExplainationLabel
        '
        Me.ExplainationLabel.Location = New System.Drawing.Point(6, 290)
        Me.ExplainationLabel.Name = "ExplainationLabel"
        Me.ExplainationLabel.Size = New System.Drawing.Size(300, 32)
        Me.ExplainationLabel.TabIndex = 14
        '
        'fSettingsManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.ExplainationLabel)
        Me.Controls.Add(Me.PrintButton)
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.GroupTabControl)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fSettingsManager"
        Me.ShowInTaskbar = False
        Me.Text = "Settings Manager"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupTabControl As System.Windows.Forms.TabControl
    Friend WithEvents HideButton As System.Windows.Forms.Button
    Friend WithEvents PrintButton As System.Windows.Forms.Button
    Friend WithEvents ExplainationLabel As System.Windows.Forms.Label
End Class
