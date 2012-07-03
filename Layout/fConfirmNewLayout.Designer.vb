<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fConfirmNewLayout
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
        Me.NoHoverButton = New Layout.PictureHoverButton
        Me.YesHoverButton = New Layout.PictureHoverButton
        Me.SuspendLayout()
        '
        'NoHoverButton
        '
        Me.NoHoverButton.BorderColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.NoHoverButton.GroupKey = Nothing
        Me.NoHoverButton.HoverColor = System.Drawing.Color.Empty
        Me.NoHoverButton.HoverForeColor = System.Drawing.Color.Empty
        Me.NoHoverButton.HoverImage = Global.Layout.My.Resources.Resources.No__Selected
        Me.NoHoverButton.Image = Global.Layout.My.Resources.Resources.No1
        Me.NoHoverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.NoHoverButton.Location = New System.Drawing.Point(157, 118)
        Me.NoHoverButton.Name = "NoHoverButton"
        Me.NoHoverButton.Selected = False
        Me.NoHoverButton.SelectedColor = System.Drawing.Color.LightGray
        Me.NoHoverButton.Size = New System.Drawing.Size(71, 52)
        Me.NoHoverButton.TabIndex = 1
        Me.NoHoverButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'YesHoverButton
        '
        Me.YesHoverButton.BorderColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.YesHoverButton.GroupKey = Nothing
        Me.YesHoverButton.HoverColor = System.Drawing.Color.Empty
        Me.YesHoverButton.HoverForeColor = System.Drawing.Color.Empty
        Me.YesHoverButton.HoverImage = Global.Layout.My.Resources.Resources.Yes_Selected
        Me.YesHoverButton.Image = Global.Layout.My.Resources.Resources.Yes
        Me.YesHoverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.YesHoverButton.Location = New System.Drawing.Point(76, 118)
        Me.YesHoverButton.Name = "YesHoverButton"
        Me.YesHoverButton.Selected = False
        Me.YesHoverButton.SelectedColor = System.Drawing.Color.LightGray
        Me.YesHoverButton.Size = New System.Drawing.Size(75, 52)
        Me.YesHoverButton.TabIndex = 0
        Me.YesHoverButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fConfirmNewLayout
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.Layout.My.Resources.Resources.ConfirmLayout
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(300, 200)
        Me.Controls.Add(Me.NoHoverButton)
        Me.Controls.Add(Me.YesHoverButton)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fConfirmNewLayout"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents YesHoverButton As Layout.PictureHoverButton
    Friend WithEvents NoHoverButton As Layout.PictureHoverButton
End Class
