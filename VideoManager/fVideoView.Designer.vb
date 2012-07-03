<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fVideoView
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
        Me.ViewPanel = New System.Windows.Forms.Panel
        Me.SuspendLayout()
        '
        'ViewPanel
        '
        Me.ViewPanel.Location = New System.Drawing.Point(0, 0)
        Me.ViewPanel.Name = "ViewPanel"
        Me.ViewPanel.Size = New System.Drawing.Size(429, 294)
        Me.ViewPanel.TabIndex = 6
        '
        'fVideoView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(428, 294)
        Me.Controls.Add(Me.ViewPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fVideoView"
        Me.ShowInTaskbar = False
        Me.Text = "fVideoView"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ViewPanel As System.Windows.Forms.Panel
End Class
