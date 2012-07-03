<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uPowerMonitor
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
        Me.components = New System.ComponentModel.Container
        Me.DataBlockLabel = New System.Windows.Forms.Label
        Me.SampleRateLabel = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SampleIntervalTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataBlockLabel
        '
        Me.DataBlockLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataBlockLabel.Location = New System.Drawing.Point(235, 233)
        Me.DataBlockLabel.Name = "DataBlockLabel"
        Me.DataBlockLabel.Size = New System.Drawing.Size(132, 18)
        Me.DataBlockLabel.TabIndex = 15
        Me.DataBlockLabel.Text = "00"
        Me.DataBlockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SampleRateLabel
        '
        Me.SampleRateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SampleRateLabel.Location = New System.Drawing.Point(106, 233)
        Me.SampleRateLabel.Name = "SampleRateLabel"
        Me.SampleRateLabel.Size = New System.Drawing.Size(50, 18)
        Me.SampleRateLabel.TabIndex = 16
        Me.SampleRateLabel.Text = "???"
        Me.SampleRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(169, 233)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 18)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Last sample"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 233)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 18)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Samples per second"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(3, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(416, 230)
        Me.PictureBox1.TabIndex = 14
        Me.PictureBox1.TabStop = False
        '
        'SampleIntervalTimer
        '
        Me.SampleIntervalTimer.Interval = 500
        '
        'uPowerMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SampleRateLabel)
        Me.Controls.Add(Me.DataBlockLabel)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uPowerMonitor"
        Me.Size = New System.Drawing.Size(422, 254)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents DataBlockLabel As System.Windows.Forms.Label
    Friend WithEvents SampleRateLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SampleIntervalTimer As System.Windows.Forms.Timer

End Class
