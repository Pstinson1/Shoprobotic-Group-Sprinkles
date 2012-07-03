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
        Me.StockQuantityLabel = New System.Windows.Forms.Label()
        Me.QtyPanel = New System.Windows.Forms.Panel()
        Me.ExitHoverButton = New Layout.PictureHoverButton()
        Me.SetStockHoverButton = New Layout.PictureHoverButton()
        Me.MinusHoverButton = New Layout.PictureHoverButton()
        Me.AddHoverButton = New Layout.PictureHoverButton()
        Me.QtyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'StockQuantityLabel
        '
        Me.StockQuantityLabel.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StockQuantityLabel.Location = New System.Drawing.Point(8, 27)
        Me.StockQuantityLabel.Name = "StockQuantityLabel"
        Me.StockQuantityLabel.Size = New System.Drawing.Size(72, 49)
        Me.StockQuantityLabel.TabIndex = 14
        Me.StockQuantityLabel.Text = "0"
        Me.StockQuantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'QtyPanel
        '
        Me.QtyPanel.BackColor = System.Drawing.Color.Transparent
        Me.QtyPanel.BackgroundImage = Global.Layout.My.Resources.Resources.qty
        Me.QtyPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.QtyPanel.Controls.Add(Me.StockQuantityLabel)
        Me.QtyPanel.Location = New System.Drawing.Point(545, 932)
        Me.QtyPanel.Name = "QtyPanel"
        Me.QtyPanel.Size = New System.Drawing.Size(168, 76)
        Me.QtyPanel.TabIndex = 15
        '
        'ExitHoverButton
        '
        Me.ExitHoverButton.BackColor = System.Drawing.Color.Transparent
        Me.ExitHoverButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ExitHoverButton.Border = False
        Me.ExitHoverButton.BorderColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.ExitHoverButton.GroupKey = Nothing
        Me.ExitHoverButton.HoverColor = System.Drawing.Color.Empty
        Me.ExitHoverButton.HoverForeColor = System.Drawing.Color.Empty
        Me.ExitHoverButton.HoverImage = Global.Layout.My.Resources.Resources.exit_down
        Me.ExitHoverButton.Image = Global.Layout.My.Resources.Resources._Exit
        Me.ExitHoverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ExitHoverButton.Location = New System.Drawing.Point(12, 936)
        Me.ExitHoverButton.Name = "ExitHoverButton"
        Me.ExitHoverButton.Selected = False
        Me.ExitHoverButton.SelectedColor = System.Drawing.Color.LightGray
        Me.ExitHoverButton.Size = New System.Drawing.Size(182, 76)
        Me.ExitHoverButton.TabIndex = 17
        Me.ExitHoverButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SetStockHoverButton
        '
        Me.SetStockHoverButton.BackColor = System.Drawing.Color.Transparent
        Me.SetStockHoverButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SetStockHoverButton.Border = False
        Me.SetStockHoverButton.BorderColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.SetStockHoverButton.GroupKey = Nothing
        Me.SetStockHoverButton.HoverColor = System.Drawing.Color.Empty
        Me.SetStockHoverButton.HoverForeColor = System.Drawing.Color.Empty
        Me.SetStockHoverButton.HoverImage = Global.Layout.My.Resources.Resources.apply_down
        Me.SetStockHoverButton.Image = Global.Layout.My.Resources.Resources.Apply
        Me.SetStockHoverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SetStockHoverButton.Location = New System.Drawing.Point(1095, 936)
        Me.SetStockHoverButton.Name = "SetStockHoverButton"
        Me.SetStockHoverButton.Selected = False
        Me.SetStockHoverButton.SelectedColor = System.Drawing.Color.LightGray
        Me.SetStockHoverButton.Size = New System.Drawing.Size(182, 76)
        Me.SetStockHoverButton.TabIndex = 20
        Me.SetStockHoverButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MinusHoverButton
        '
        Me.MinusHoverButton.AutoSize = True
        Me.MinusHoverButton.BackColor = System.Drawing.Color.Transparent
        Me.MinusHoverButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.MinusHoverButton.Border = False
        Me.MinusHoverButton.BorderColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.MinusHoverButton.GroupKey = Nothing
        Me.MinusHoverButton.HoverColor = System.Drawing.Color.Empty
        Me.MinusHoverButton.HoverForeColor = System.Drawing.Color.Empty
        Me.MinusHoverButton.HoverImage = Global.Layout.My.Resources.Resources.minus_down
        Me.MinusHoverButton.Image = Global.Layout.My.Resources.Resources.Minus
        Me.MinusHoverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.MinusHoverButton.Location = New System.Drawing.Point(895, 936)
        Me.MinusHoverButton.Name = "MinusHoverButton"
        Me.MinusHoverButton.Selected = False
        Me.MinusHoverButton.SelectedColor = System.Drawing.Color.LightGray
        Me.MinusHoverButton.Size = New System.Drawing.Size(181, 76)
        Me.MinusHoverButton.TabIndex = 19
        Me.MinusHoverButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AddHoverButton
        '
        Me.AddHoverButton.AutoSize = True
        Me.AddHoverButton.BackColor = System.Drawing.Color.Transparent
        Me.AddHoverButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.AddHoverButton.Border = False
        Me.AddHoverButton.BorderColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.AddHoverButton.GroupKey = Nothing
        Me.AddHoverButton.HoverColor = System.Drawing.Color.Empty
        Me.AddHoverButton.HoverForeColor = System.Drawing.Color.Empty
        Me.AddHoverButton.HoverImage = Global.Layout.My.Resources.Resources.plus_down
        Me.AddHoverButton.Image = Global.Layout.My.Resources.Resources.Plus
        Me.AddHoverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.AddHoverButton.Location = New System.Drawing.Point(719, 936)
        Me.AddHoverButton.Name = "AddHoverButton"
        Me.AddHoverButton.Selected = False
        Me.AddHoverButton.SelectedColor = System.Drawing.Color.LightGray
        Me.AddHoverButton.Size = New System.Drawing.Size(182, 76)
        Me.AddHoverButton.TabIndex = 18
        Me.AddHoverButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fMain
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackgroundImage = Global.Layout.My.Resources.Resources.Background
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(1280, 1024)
        Me.ControlBox = False
        Me.Controls.Add(Me.ExitHoverButton)
        Me.Controls.Add(Me.SetStockHoverButton)
        Me.Controls.Add(Me.MinusHoverButton)
        Me.Controls.Add(Me.AddHoverButton)
        Me.Controls.Add(Me.QtyPanel)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fMain"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Layout"
        Me.QtyPanel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StockQuantityLabel As System.Windows.Forms.Label
    Friend WithEvents QtyPanel As System.Windows.Forms.Panel
    Friend WithEvents ExitHoverButton As Layout.PictureHoverButton
    Friend WithEvents AddHoverButton As Layout.PictureHoverButton
    Friend WithEvents MinusHoverButton As Layout.PictureHoverButton
    Friend WithEvents SetStockHoverButton As Layout.PictureHoverButton

End Class

