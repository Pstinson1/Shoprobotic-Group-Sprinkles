<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uBinItem
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
        Me.KeyvendLabel = New System.Windows.Forms.Label()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.StockLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'KeyvendLabel
        '
        Me.KeyvendLabel.BackColor = System.Drawing.Color.Transparent
        Me.KeyvendLabel.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyvendLabel.Location = New System.Drawing.Point(9, 7)
        Me.KeyvendLabel.Name = "KeyvendLabel"
        Me.KeyvendLabel.Size = New System.Drawing.Size(49, 29)
        Me.KeyvendLabel.TabIndex = 1
        Me.KeyvendLabel.Text = "XX-00"
        Me.KeyvendLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.KeyvendLabel.UseCompatibleTextRendering = True
        '
        'NameLabel
        '
        Me.NameLabel.BackColor = System.Drawing.Color.Transparent
        Me.NameLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NameLabel.Location = New System.Drawing.Point(3, 36)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(73, 27)
        Me.NameLabel.TabIndex = 3
        Me.NameLabel.Text = "Name"
        Me.NameLabel.UseCompatibleTextRendering = True
        '
        'StockLabel
        '
        Me.StockLabel.BackColor = System.Drawing.Color.Transparent
        Me.StockLabel.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StockLabel.Location = New System.Drawing.Point(55, 3)
        Me.StockLabel.Name = "StockLabel"
        Me.StockLabel.Size = New System.Drawing.Size(24, 29)
        Me.StockLabel.TabIndex = 4
        Me.StockLabel.Text = "0"
        Me.StockLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.StockLabel.UseCompatibleTextRendering = True
        '
        'uBinItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.BackgroundImage = Global.Layout.My.Resources.Resources.tile
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Controls.Add(Me.StockLabel)
        Me.Controls.Add(Me.KeyvendLabel)
        Me.Controls.Add(Me.NameLabel)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uBinItem"
        Me.Size = New System.Drawing.Size(102, 70)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents KeyvendLabel As System.Windows.Forms.Label
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents StockLabel As System.Windows.Forms.Label

End Class
