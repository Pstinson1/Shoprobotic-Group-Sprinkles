<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uProductExplorer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uProductExplorer))
        Me.ProductTree = New System.Windows.Forms.TreeView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.DescriptionLabel = New System.Windows.Forms.Label
        Me.AddPositionButton = New System.Windows.Forms.Button
        Me.DeletePositionButton = New System.Windows.Forms.Button
        Me.AddProductButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ProductTree
        '
        Me.ProductTree.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProductTree.FullRowSelect = True
        Me.ProductTree.HideSelection = False
        Me.ProductTree.ImageIndex = 0
        Me.ProductTree.ImageList = Me.ImageList1
        Me.ProductTree.Indent = 24
        Me.ProductTree.ItemHeight = 24
        Me.ProductTree.LineColor = System.Drawing.Color.FromArgb(CType(CType(113, Byte), Integer), CType(CType(111, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ProductTree.Location = New System.Drawing.Point(0, 68)
        Me.ProductTree.Name = "ProductTree"
        Me.ProductTree.SelectedImageIndex = 0
        Me.ProductTree.ShowRootLines = False
        Me.ProductTree.Size = New System.Drawing.Size(286, 472)
        Me.ProductTree.TabIndex = 5
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Globe_1_Selected.png")
        Me.ImageList1.Images.SetKeyName(1, "Globe_1.png")
        Me.ImageList1.Images.SetKeyName(2, "Globe_1_Inactive.png")
        Me.ImageList1.Images.SetKeyName(3, "Play_1_Selected.png")
        Me.ImageList1.Images.SetKeyName(4, "Play_1.png")
        Me.ImageList1.Images.SetKeyName(5, "Play_1_Inactive.png")
        Me.ImageList1.Images.SetKeyName(6, "Box_1_Selected.png")
        Me.ImageList1.Images.SetKeyName(7, "Box_1.png")
        Me.ImageList1.Images.SetKeyName(8, "Box_1_Inactive.png")
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.BackColor = System.Drawing.SystemColors.Control
        Me.DescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DescriptionLabel.Location = New System.Drawing.Point(0, 0)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.Size = New System.Drawing.Size(286, 23)
        Me.DescriptionLabel.TabIndex = 6
        Me.DescriptionLabel.Text = "Product Explorer"
        Me.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AddPositionButton
        '
        Me.AddPositionButton.Enabled = False
        Me.AddPositionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.AddPositionButton.Location = New System.Drawing.Point(66, 26)
        Me.AddPositionButton.Name = "AddPositionButton"
        Me.AddPositionButton.Size = New System.Drawing.Size(57, 40)
        Me.AddPositionButton.TabIndex = 7
        Me.AddPositionButton.Text = "Add Position"
        Me.AddPositionButton.UseVisualStyleBackColor = True
        '
        'DeletePositionButton
        '
        Me.DeletePositionButton.Enabled = False
        Me.DeletePositionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.DeletePositionButton.Location = New System.Drawing.Point(123, 26)
        Me.DeletePositionButton.Name = "DeletePositionButton"
        Me.DeletePositionButton.Size = New System.Drawing.Size(57, 40)
        Me.DeletePositionButton.TabIndex = 8
        Me.DeletePositionButton.Text = "Delete Position"
        Me.DeletePositionButton.UseVisualStyleBackColor = True
        '
        'AddProductButton
        '
        Me.AddProductButton.Enabled = False
        Me.AddProductButton.Location = New System.Drawing.Point(3, 26)
        Me.AddProductButton.Name = "AddProductButton"
        Me.AddProductButton.Size = New System.Drawing.Size(57, 40)
        Me.AddProductButton.TabIndex = 9
        Me.AddProductButton.Text = "Add Product"
        Me.AddProductButton.UseVisualStyleBackColor = True
        '
        'uProductExplorer
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Controls.Add(Me.AddProductButton)
        Me.Controls.Add(Me.AddPositionButton)
        Me.Controls.Add(Me.DescriptionLabel)
        Me.Controls.Add(Me.DeletePositionButton)
        Me.Controls.Add(Me.ProductTree)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "uProductExplorer"
        Me.Size = New System.Drawing.Size(287, 540)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ProductTree As System.Windows.Forms.TreeView
    Friend WithEvents DescriptionLabel As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents AddPositionButton As System.Windows.Forms.Button
    Friend WithEvents DeletePositionButton As System.Windows.Forms.Button
    Friend WithEvents AddProductButton As System.Windows.Forms.Button

End Class
