<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uProduct
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
        Me.PriceText = New System.Windows.Forms.TextBox()
        Me.NameText = New System.Windows.Forms.TextBox()
        Me.ActiveCheck = New System.Windows.Forms.CheckBox()
        Me.DescriptionText = New System.Windows.Forms.TextBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtBarcode = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SizeText = New System.Windows.Forms.TextBox()
        Me.FullPriceText = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ReportingIdText = New System.Windows.Forms.TextBox()
        Me.CategoryCombo = New System.Windows.Forms.ComboBox()
        Me.PreNameText = New System.Windows.Forms.TextBox()
        Me.PreNameLabel = New System.Windows.Forms.Label()
        Me.VisualCoordsLabel = New System.Windows.Forms.Label()
        Me.VisualTextY = New System.Windows.Forms.TextBox()
        Me.VisualTextX = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.KeyVendText = New System.Windows.Forms.TextBox()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.lblProdName = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblProdCat = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ProductImage1 = New System.Windows.Forms.PictureBox()
        Me.ErrorImage1 = New System.Windows.Forms.PictureBox()
        Me.SelectFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.ProductImage2 = New System.Windows.Forms.PictureBox()
        Me.ErrorImage2 = New System.Windows.Forms.PictureBox()
        Me.ImageLabel2 = New System.Windows.Forms.Label()
        Me.ImageLabel1 = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.ProductImage1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorImage1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProductImage2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorImage2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PriceText
        '
        Me.PriceText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PriceText.Location = New System.Drawing.Point(274, 170)
        Me.PriceText.MaxLength = 9
        Me.PriceText.Name = "PriceText"
        Me.PriceText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PriceText.Size = New System.Drawing.Size(165, 21)
        Me.PriceText.TabIndex = 31
        Me.PriceText.Tag = "-T"
        '
        'NameText
        '
        Me.NameText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NameText.Location = New System.Drawing.Point(274, 29)
        Me.NameText.MaxLength = 100
        Me.NameText.Name = "NameText"
        Me.NameText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.NameText.Size = New System.Drawing.Size(165, 21)
        Me.NameText.TabIndex = 20
        Me.NameText.Tag = "-T"
        '
        'ActiveCheck
        '
        Me.ActiveCheck.AutoSize = True
        Me.ActiveCheck.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ActiveCheck.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ActiveCheck.Location = New System.Drawing.Point(232, 216)
        Me.ActiveCheck.Name = "ActiveCheck"
        Me.ActiveCheck.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ActiveCheck.Size = New System.Drawing.Size(56, 17)
        Me.ActiveCheck.TabIndex = 29
        Me.ActiveCheck.Text = "Active"
        Me.ActiveCheck.UseVisualStyleBackColor = True
        '
        'DescriptionText
        '
        Me.DescriptionText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionText.Location = New System.Drawing.Point(6, 235)
        Me.DescriptionText.MaxLength = 1024
        Me.DescriptionText.Multiline = True
        Me.DescriptionText.Name = "DescriptionText"
        Me.DescriptionText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DescriptionText.Size = New System.Drawing.Size(431, 110)
        Me.DescriptionText.TabIndex = 34
        Me.DescriptionText.Tag = "-T"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.ItemSize = New System.Drawing.Size(84, 44)
        Me.TabControl1.Location = New System.Drawing.Point(2, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(452, 466)
        Me.TabControl1.TabIndex = 36
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ImageLabel1)
        Me.TabPage1.Controls.Add(Me.ImageLabel2)
        Me.TabPage1.Controls.Add(Me.ProductImage2)
        Me.TabPage1.Controls.Add(Me.ErrorImage2)
        Me.TabPage1.Controls.Add(Me.txtBarcode)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.SizeText)
        Me.TabPage1.Controls.Add(Me.FullPriceText)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.ReportingIdText)
        Me.TabPage1.Controls.Add(Me.CategoryCombo)
        Me.TabPage1.Controls.Add(Me.PreNameText)
        Me.TabPage1.Controls.Add(Me.PreNameLabel)
        Me.TabPage1.Controls.Add(Me.VisualCoordsLabel)
        Me.TabPage1.Controls.Add(Me.VisualTextY)
        Me.TabPage1.Controls.Add(Me.VisualTextX)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.KeyVendText)
        Me.TabPage1.Controls.Add(Me.DescriptionText)
        Me.TabPage1.Controls.Add(Me.CancelButton)
        Me.TabPage1.Controls.Add(Me.NameText)
        Me.TabPage1.Controls.Add(Me.ApplyButton)
        Me.TabPage1.Controls.Add(Me.lblProdName)
        Me.TabPage1.Controls.Add(Me.PriceText)
        Me.TabPage1.Controls.Add(Me.Label14)
        Me.TabPage1.Controls.Add(Me.ActiveCheck)
        Me.TabPage1.Controls.Add(Me.lblProdCat)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.ProductImage1)
        Me.TabPage1.Controls.Add(Me.ErrorImage1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 48)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(444, 414)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Product Details"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'txtBarcode
        '
        Me.txtBarcode.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBarcode.Location = New System.Drawing.Point(274, 51)
        Me.txtBarcode.MaxLength = 100
        Me.txtBarcode.Name = "txtBarcode"
        Me.txtBarcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBarcode.Size = New System.Drawing.Size(165, 21)
        Me.txtBarcode.TabIndex = 51
        Me.txtBarcode.Tag = "-T"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(224, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 50
        Me.Label3.Text = "Barcode"
        '
        'SizeText
        '
        Me.SizeText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SizeText.Location = New System.Drawing.Point(361, 78)
        Me.SizeText.MaxLength = 100
        Me.SizeText.Name = "SizeText"
        Me.SizeText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SizeText.Size = New System.Drawing.Size(78, 21)
        Me.SizeText.TabIndex = 49
        Me.SizeText.Tag = "-T"
        '
        'FullPriceText
        '
        Me.FullPriceText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FullPriceText.Location = New System.Drawing.Point(274, 147)
        Me.FullPriceText.MaxLength = 9
        Me.FullPriceText.Name = "FullPriceText"
        Me.FullPriceText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FullPriceText.Size = New System.Drawing.Size(165, 21)
        Me.FullPriceText.TabIndex = 47
        Me.FullPriceText.Tag = "-T"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(221, 150)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 46
        Me.Label2.Text = "Full Price"
        '
        'ReportingIdText
        '
        Me.ReportingIdText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReportingIdText.Location = New System.Drawing.Point(274, 78)
        Me.ReportingIdText.MaxLength = 100
        Me.ReportingIdText.Name = "ReportingIdText"
        Me.ReportingIdText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ReportingIdText.Size = New System.Drawing.Size(46, 21)
        Me.ReportingIdText.TabIndex = 45
        Me.ReportingIdText.Tag = "-T"
        '
        'CategoryCombo
        '
        Me.CategoryCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CategoryCombo.FormattingEnabled = True
        Me.CategoryCombo.Location = New System.Drawing.Point(274, 101)
        Me.CategoryCombo.Name = "CategoryCombo"
        Me.CategoryCombo.Size = New System.Drawing.Size(165, 21)
        Me.CategoryCombo.TabIndex = 44
        '
        'PreNameText
        '
        Me.PreNameText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PreNameText.Location = New System.Drawing.Point(274, 6)
        Me.PreNameText.MaxLength = 100
        Me.PreNameText.Name = "PreNameText"
        Me.PreNameText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PreNameText.Size = New System.Drawing.Size(165, 21)
        Me.PreNameText.TabIndex = 43
        Me.PreNameText.Tag = "-T"
        '
        'PreNameLabel
        '
        Me.PreNameLabel.AutoSize = True
        Me.PreNameLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PreNameLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PreNameLabel.Location = New System.Drawing.Point(215, 9)
        Me.PreNameLabel.Name = "PreNameLabel"
        Me.PreNameLabel.Size = New System.Drawing.Size(53, 13)
        Me.PreNameLabel.TabIndex = 42
        Me.PreNameLabel.Text = "Pre-name"
        '
        'VisualCoordsLabel
        '
        Me.VisualCoordsLabel.AutoSize = True
        Me.VisualCoordsLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VisualCoordsLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.VisualCoordsLabel.Location = New System.Drawing.Point(214, 196)
        Me.VisualCoordsLabel.Name = "VisualCoordsLabel"
        Me.VisualCoordsLabel.Size = New System.Drawing.Size(54, 13)
        Me.VisualCoordsLabel.TabIndex = 41
        Me.VisualCoordsLabel.Text = "Visual Pos"
        '
        'VisualTextY
        '
        Me.VisualTextY.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VisualTextY.Location = New System.Drawing.Point(323, 193)
        Me.VisualTextY.MaxLength = 9
        Me.VisualTextY.Name = "VisualTextY"
        Me.VisualTextY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.VisualTextY.Size = New System.Drawing.Size(46, 21)
        Me.VisualTextY.TabIndex = 40
        Me.VisualTextY.Tag = "-T"
        '
        'VisualTextX
        '
        Me.VisualTextX.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VisualTextX.Location = New System.Drawing.Point(274, 193)
        Me.VisualTextX.MaxLength = 9
        Me.VisualTextX.Name = "VisualTextX"
        Me.VisualTextX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.VisualTextX.Size = New System.Drawing.Size(46, 21)
        Me.VisualTextX.TabIndex = 39
        Me.VisualTextX.Tag = "-T"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(218, 127)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 38
        Me.Label1.Text = "Key Vend"
        '
        'KeyVendText
        '
        Me.KeyVendText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyVendText.Location = New System.Drawing.Point(274, 124)
        Me.KeyVendText.MaxLength = 100
        Me.KeyVendText.Name = "KeyVendText"
        Me.KeyVendText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.KeyVendText.Size = New System.Drawing.Size(165, 21)
        Me.KeyVendText.TabIndex = 37
        Me.KeyVendText.Tag = "-T"
        '
        'CancelButton
        '
        Me.CancelButton.Enabled = False
        Me.CancelButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.CancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.CancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.CancelButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CancelButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CancelButton.Image = Global.Configure.My.Resources.Resources.Knob_Cancel
        Me.CancelButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.CancelButton.Location = New System.Drawing.Point(309, 351)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(64, 58)
        Me.CancelButton.TabIndex = 35
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.Enabled = False
        Me.ApplyButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.ApplyButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.ApplyButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.ApplyButton.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplyButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ApplyButton.Image = Global.Configure.My.Resources.Resources.Knob_Valid_Green
        Me.ApplyButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ApplyButton.Location = New System.Drawing.Point(374, 351)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ApplyButton.Size = New System.Drawing.Size(64, 58)
        Me.ApplyButton.TabIndex = 28
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'lblProdName
        '
        Me.lblProdName.AutoSize = True
        Me.lblProdName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProdName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProdName.Location = New System.Drawing.Point(236, 32)
        Me.lblProdName.Name = "lblProdName"
        Me.lblProdName.Size = New System.Drawing.Size(34, 13)
        Me.lblProdName.TabIndex = 19
        Me.lblProdName.Text = "Name"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(203, 81)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(67, 18)
        Me.Label14.TabIndex = 32
        Me.Label14.Text = "Reporting Id"
        Me.Label14.UseCompatibleTextRendering = True
        '
        'lblProdCat
        '
        Me.lblProdCat.AutoSize = True
        Me.lblProdCat.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProdCat.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProdCat.Location = New System.Drawing.Point(218, 104)
        Me.lblProdCat.Name = "lblProdCat"
        Me.lblProdCat.Size = New System.Drawing.Size(52, 13)
        Me.lblProdCat.TabIndex = 21
        Me.lblProdCat.Text = "Category"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(240, 173)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 13)
        Me.Label6.TabIndex = 30
        Me.Label6.Text = "Price"
        '
        'ProductImage1
        '
        Me.ProductImage1.BackColor = System.Drawing.Color.White
        Me.ProductImage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ProductImage1.ErrorImage = Nothing
        Me.ProductImage1.InitialImage = Nothing
        Me.ProductImage1.Location = New System.Drawing.Point(6, 6)
        Me.ProductImage1.Name = "ProductImage1"
        Me.ProductImage1.Size = New System.Drawing.Size(90, 90)
        Me.ProductImage1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.ProductImage1.TabIndex = 18
        Me.ProductImage1.TabStop = False
        '
        'ErrorImage1
        '
        Me.ErrorImage1.BackColor = System.Drawing.Color.White
        Me.ErrorImage1.BackgroundImage = Global.Configure.My.Resources.Resources.NoValidImage
        Me.ErrorImage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ErrorImage1.ErrorImage = Nothing
        Me.ErrorImage1.InitialImage = Nothing
        Me.ErrorImage1.Location = New System.Drawing.Point(6, 6)
        Me.ErrorImage1.Name = "ErrorImage1"
        Me.ErrorImage1.Size = New System.Drawing.Size(90, 90)
        Me.ErrorImage1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ErrorImage1.TabIndex = 48
        Me.ErrorImage1.TabStop = False
        '
        'ProductImage2
        '
        Me.ProductImage2.BackColor = System.Drawing.Color.White
        Me.ProductImage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ProductImage2.ErrorImage = Nothing
        Me.ProductImage2.InitialImage = Nothing
        Me.ProductImage2.Location = New System.Drawing.Point(3, 119)
        Me.ProductImage2.Name = "ProductImage2"
        Me.ProductImage2.Size = New System.Drawing.Size(90, 90)
        Me.ProductImage2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.ProductImage2.TabIndex = 52
        Me.ProductImage2.TabStop = False
        '
        'ErrorImage2
        '
        Me.ErrorImage2.BackColor = System.Drawing.Color.White
        Me.ErrorImage2.BackgroundImage = Global.Configure.My.Resources.Resources.NoValidImage
        Me.ErrorImage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ErrorImage2.ErrorImage = Nothing
        Me.ErrorImage2.InitialImage = Nothing
        Me.ErrorImage2.Location = New System.Drawing.Point(3, 119)
        Me.ErrorImage2.Name = "ErrorImage2"
        Me.ErrorImage2.Size = New System.Drawing.Size(90, 90)
        Me.ErrorImage2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ErrorImage2.TabIndex = 53
        Me.ErrorImage2.TabStop = False
        '
        'ImageLabel2
        '
        Me.ImageLabel2.AutoSize = True
        Me.ImageLabel2.Location = New System.Drawing.Point(7, 216)
        Me.ImageLabel2.Name = "ImageLabel2"
        Me.ImageLabel2.Size = New System.Drawing.Size(53, 13)
        Me.ImageLabel2.TabIndex = 54
        Me.ImageLabel2.Text = "No Image"
        '
        'ImageLabel1
        '
        Me.ImageLabel1.AutoSize = True
        Me.ImageLabel1.Location = New System.Drawing.Point(6, 99)
        Me.ImageLabel1.Name = "ImageLabel1"
        Me.ImageLabel1.Size = New System.Drawing.Size(53, 13)
        Me.ImageLabel1.TabIndex = 55
        Me.ImageLabel1.Text = "No Image"
        '
        'uProduct
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uProduct"
        Me.Size = New System.Drawing.Size(454, 469)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.ProductImage1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorImage1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProductImage2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorImage2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PriceText As System.Windows.Forms.TextBox
    Friend WithEvents ProductImage1 As System.Windows.Forms.PictureBox
    Friend WithEvents NameText As System.Windows.Forms.TextBox
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents ActiveCheck As System.Windows.Forms.CheckBox
    Friend WithEvents DescriptionText As System.Windows.Forms.TextBox
    Friend WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents SelectFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents KeyVendText As System.Windows.Forms.TextBox
    Friend WithEvents lblProdName As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblProdCat As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents VisualCoordsLabel As System.Windows.Forms.Label
    Friend WithEvents VisualTextY As System.Windows.Forms.TextBox
    Friend WithEvents VisualTextX As System.Windows.Forms.TextBox
    Friend WithEvents PreNameText As System.Windows.Forms.TextBox
    Friend WithEvents PreNameLabel As System.Windows.Forms.Label
    Friend WithEvents CategoryCombo As System.Windows.Forms.ComboBox
    Friend WithEvents ReportingIdText As System.Windows.Forms.TextBox
    Friend WithEvents FullPriceText As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ErrorImage1 As System.Windows.Forms.PictureBox
    Friend WithEvents SizeText As System.Windows.Forms.TextBox
    Friend WithEvents txtBarcode As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ProductImage2 As System.Windows.Forms.PictureBox
    Friend WithEvents ErrorImage2 As System.Windows.Forms.PictureBox
    Friend WithEvents ImageLabel1 As System.Windows.Forms.Label
    Friend WithEvents ImageLabel2 As System.Windows.Forms.Label

End Class
