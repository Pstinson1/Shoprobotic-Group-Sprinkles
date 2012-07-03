<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fShellStart
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
        Me.LogonToExplorerButton = New System.Windows.Forms.Button
        Me.LogonToShellButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'LogonToExplorerButton
        '
        Me.LogonToExplorerButton.Location = New System.Drawing.Point(72, 12)
        Me.LogonToExplorerButton.Name = "LogonToExplorerButton"
        Me.LogonToExplorerButton.Size = New System.Drawing.Size(64, 32)
        Me.LogonToExplorerButton.TabIndex = 0
        Me.LogonToExplorerButton.Text = "Off"
        Me.LogonToExplorerButton.UseVisualStyleBackColor = True
        '
        'LogonToShellButton
        '
        Me.LogonToShellButton.Location = New System.Drawing.Point(6, 12)
        Me.LogonToShellButton.Name = "LogonToShellButton"
        Me.LogonToShellButton.Size = New System.Drawing.Size(64, 32)
        Me.LogonToShellButton.TabIndex = 1
        Me.LogonToShellButton.Text = "On"
        Me.LogonToShellButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(154, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(11, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "-"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'fShellStart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(231, 51)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LogonToShellButton)
        Me.Controls.Add(Me.LogonToExplorerButton)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fShellStart"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "ShopRobotic"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LogonToExplorerButton As System.Windows.Forms.Button
    Friend WithEvents LogonToShellButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
