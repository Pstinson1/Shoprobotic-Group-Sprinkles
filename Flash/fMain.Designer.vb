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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fMain))
        Me.AxFlash = New AxShockwaveFlashObjects.AxShockwaveFlash
        CType(Me.AxFlash, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AxFlash
        '
        Me.AxFlash.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxFlash.Enabled = True
        Me.AxFlash.Location = New System.Drawing.Point(0, 0)
        Me.AxFlash.Name = "AxFlash"
        Me.AxFlash.OcxState = CType(resources.GetObject("AxFlash.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxFlash.Size = New System.Drawing.Size(800, 600)
        Me.AxFlash.TabIndex = 1
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.AxFlash)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "fMain"
        Me.Text = "User Interface"
        CType(Me.AxFlash, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AxFlash As AxShockwaveFlashObjects.AxShockwaveFlash

End Class
