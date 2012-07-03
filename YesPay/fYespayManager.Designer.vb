<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fYespayManager
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
        Me.ProgressListBox = New System.Windows.Forms.ListBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TaskTabPage = New System.Windows.Forms.TabPage
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.KeyTextBox = New System.Windows.Forms.TextBox
        Me.CardNumberTextBox = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.IssueNumberTextBox = New System.Windows.Forms.TextBox
        Me.ExpiryDateTextBox = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.StartDateTextBox = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.RecieptNumberTextBox = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.DateTextBox = New System.Windows.Forms.TextBox
        Me.MerchantIdTextBox = New System.Windows.Forms.TextBox
        Me.TimeTextBox = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.CardHolderExtendedTextBox = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.MerchantNameTextBox = New System.Windows.Forms.TextBox
        Me.CardHolderTextBox = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TransactionResultTextBox = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.PgtrTextBox = New System.Windows.Forms.TextBox
        Me.CustomerDeclarationTextBox = New System.Windows.Forms.TextBox
        Me.MerchantAddressTextBox = New System.Windows.Forms.TextBox
        Me.RetentionReminderTextBox = New System.Windows.Forms.TextBox
        Me.CardDescriptionTextBox = New System.Windows.Forms.TextBox
        Me.PTGR = New System.Windows.Forms.Label
        Me.TerminalIdTextBox = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.AuthorisationTextBox = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.BackgroundTabPage = New System.Windows.Forms.TabPage
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.ServerPortTextBox = New System.Windows.Forms.TextBox
        Me.ServerAddressTextBox = New System.Windows.Forms.TextBox
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.StatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.StatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.FindFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.HideButton = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TaskTabPage.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.BackgroundTabPage.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ProgressListBox
        '
        Me.ProgressListBox.FormattingEnabled = True
        Me.ProgressListBox.IntegralHeight = False
        Me.ProgressListBox.Location = New System.Drawing.Point(6, 6)
        Me.ProgressListBox.Name = "ProgressListBox"
        Me.ProgressListBox.Size = New System.Drawing.Size(417, 222)
        Me.ProgressListBox.TabIndex = 1
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TaskTabPage)
        Me.TabControl1.Controls.Add(Me.BackgroundTabPage)
        Me.TabControl1.Location = New System.Drawing.Point(2, 4)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(437, 260)
        Me.TabControl1.TabIndex = 8
        '
        'TaskTabPage
        '
        Me.TaskTabPage.Controls.Add(Me.Panel1)
        Me.TaskTabPage.Location = New System.Drawing.Point(4, 22)
        Me.TaskTabPage.Name = "TaskTabPage"
        Me.TaskTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.TaskTabPage.Size = New System.Drawing.Size(429, 234)
        Me.TaskTabPage.TabIndex = 0
        Me.TaskTabPage.Text = "Transaction"
        Me.TaskTabPage.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.AutoScrollMargin = New System.Drawing.Size(0, 4)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.KeyTextBox)
        Me.Panel1.Controls.Add(Me.CardNumberTextBox)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.IssueNumberTextBox)
        Me.Panel1.Controls.Add(Me.ExpiryDateTextBox)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.StartDateTextBox)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.RecieptNumberTextBox)
        Me.Panel1.Controls.Add(Me.GroupBox3)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.DateTextBox)
        Me.Panel1.Controls.Add(Me.MerchantIdTextBox)
        Me.Panel1.Controls.Add(Me.TimeTextBox)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.CardHolderExtendedTextBox)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.MerchantNameTextBox)
        Me.Panel1.Controls.Add(Me.CardHolderTextBox)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.TransactionResultTextBox)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.PgtrTextBox)
        Me.Panel1.Controls.Add(Me.CustomerDeclarationTextBox)
        Me.Panel1.Controls.Add(Me.MerchantAddressTextBox)
        Me.Panel1.Controls.Add(Me.RetentionReminderTextBox)
        Me.Panel1.Controls.Add(Me.CardDescriptionTextBox)
        Me.Panel1.Controls.Add(Me.PTGR)
        Me.Panel1.Controls.Add(Me.TerminalIdTextBox)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.AuthorisationTextBox)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Location = New System.Drawing.Point(6, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(417, 222)
        Me.Panel1.TabIndex = 39
        '
        'KeyTextBox
        '
        Me.KeyTextBox.AcceptsTab = True
        Me.KeyTextBox.Location = New System.Drawing.Point(121, 152)
        Me.KeyTextBox.Name = "KeyTextBox"
        Me.KeyTextBox.ReadOnly = True
        Me.KeyTextBox.Size = New System.Drawing.Size(266, 21)
        Me.KeyTextBox.TabIndex = 52
        '
        'CardNumberTextBox
        '
        Me.CardNumberTextBox.AcceptsTab = True
        Me.CardNumberTextBox.Location = New System.Drawing.Point(121, 7)
        Me.CardNumberTextBox.Name = "CardNumberTextBox"
        Me.CardNumberTextBox.ReadOnly = True
        Me.CardNumberTextBox.Size = New System.Drawing.Size(266, 21)
        Me.CardNumberTextBox.TabIndex = 51
        Me.CardNumberTextBox.Text = "Card Number (5)"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(45, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 50
        Me.Label1.Text = "Card Number"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(43, 130)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(72, 13)
        Me.Label18.TabIndex = 49
        Me.Label18.Text = "Issue number"
        '
        'IssueNumberTextBox
        '
        Me.IssueNumberTextBox.AcceptsTab = True
        Me.IssueNumberTextBox.Location = New System.Drawing.Point(121, 127)
        Me.IssueNumberTextBox.Name = "IssueNumberTextBox"
        Me.IssueNumberTextBox.ReadOnly = True
        Me.IssueNumberTextBox.Size = New System.Drawing.Size(120, 21)
        Me.IssueNumberTextBox.TabIndex = 48
        Me.IssueNumberTextBox.Text = "Issue Number (30)"
        '
        'ExpiryDateTextBox
        '
        Me.ExpiryDateTextBox.AcceptsTab = True
        Me.ExpiryDateTextBox.Location = New System.Drawing.Point(243, 103)
        Me.ExpiryDateTextBox.Name = "ExpiryDateTextBox"
        Me.ExpiryDateTextBox.ReadOnly = True
        Me.ExpiryDateTextBox.Size = New System.Drawing.Size(120, 21)
        Me.ExpiryDateTextBox.TabIndex = 47
        Me.ExpiryDateTextBox.Text = "Expiry Date (37)"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(5, 106)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(110, 13)
        Me.Label17.TabIndex = 46
        Me.Label17.Text = "Start and Expiry date"
        '
        'StartDateTextBox
        '
        Me.StartDateTextBox.AcceptsTab = True
        Me.StartDateTextBox.Location = New System.Drawing.Point(121, 103)
        Me.StartDateTextBox.Name = "StartDateTextBox"
        Me.StartDateTextBox.ReadOnly = True
        Me.StartDateTextBox.Size = New System.Drawing.Size(120, 21)
        Me.StartDateTextBox.TabIndex = 45
        Me.StartDateTextBox.Text = "Start Date (15)"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(30, 428)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(83, 13)
        Me.Label14.TabIndex = 43
        Me.Label14.Text = "Reciept Number"
        '
        'RecieptNumberTextBox
        '
        Me.RecieptNumberTextBox.AcceptsTab = True
        Me.RecieptNumberTextBox.Location = New System.Drawing.Point(121, 425)
        Me.RecieptNumberTextBox.Name = "RecieptNumberTextBox"
        Me.RecieptNumberTextBox.ReadOnly = True
        Me.RecieptNumberTextBox.Size = New System.Drawing.Size(120, 21)
        Me.RecieptNumberTextBox.TabIndex = 42
        Me.RecieptNumberTextBox.Text = "Reciept Number (36)"
        '
        'GroupBox3
        '
        Me.GroupBox3.Location = New System.Drawing.Point(12, 180)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(375, 3)
        Me.GroupBox3.TabIndex = 41
        Me.GroupBox3.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(68, 274)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 40
        Me.Label8.Text = "Terminal"
        '
        'GroupBox2
        '
        Me.GroupBox2.Location = New System.Drawing.Point(12, 213)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(375, 3)
        Me.GroupBox2.TabIndex = 39
        Me.GroupBox2.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(12, 368)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(375, 3)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        '
        'DateTextBox
        '
        Me.DateTextBox.AcceptsTab = True
        Me.DateTextBox.Location = New System.Drawing.Point(243, 188)
        Me.DateTextBox.Name = "DateTextBox"
        Me.DateTextBox.ReadOnly = True
        Me.DateTextBox.Size = New System.Drawing.Size(120, 21)
        Me.DateTextBox.TabIndex = 22
        Me.DateTextBox.Text = "Date (8)"
        '
        'MerchantIdTextBox
        '
        Me.MerchantIdTextBox.AcceptsTab = True
        Me.MerchantIdTextBox.Location = New System.Drawing.Point(121, 247)
        Me.MerchantIdTextBox.Name = "MerchantIdTextBox"
        Me.MerchantIdTextBox.ReadOnly = True
        Me.MerchantIdTextBox.Size = New System.Drawing.Size(120, 21)
        Me.MerchantIdTextBox.TabIndex = 33
        Me.MerchantIdTextBox.Text = "Merchant Id (12)"
        '
        'TimeTextBox
        '
        Me.TimeTextBox.AcceptsTab = True
        Me.TimeTextBox.Location = New System.Drawing.Point(121, 188)
        Me.TimeTextBox.Name = "TimeTextBox"
        Me.TimeTextBox.ReadOnly = True
        Me.TimeTextBox.Size = New System.Drawing.Size(120, 21)
        Me.TimeTextBox.TabIndex = 21
        Me.TimeTextBox.Text = "Time (9)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(39, 191)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Time and Date"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(22, 404)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(93, 13)
        Me.Label16.TabIndex = 38
        Me.Label16.Text = "Transaction result"
        '
        'CardHolderExtendedTextBox
        '
        Me.CardHolderExtendedTextBox.AcceptsTab = True
        Me.CardHolderExtendedTextBox.Location = New System.Drawing.Point(121, 79)
        Me.CardHolderExtendedTextBox.Name = "CardHolderExtendedTextBox"
        Me.CardHolderExtendedTextBox.ReadOnly = True
        Me.CardHolderExtendedTextBox.Size = New System.Drawing.Size(266, 21)
        Me.CardHolderExtendedTextBox.TabIndex = 30
        Me.CardHolderExtendedTextBox.Text = "Card Holder Extended (11)"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(58, 82)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(57, 13)
        Me.Label12.TabIndex = 29
        Me.Label12.Text = "Holder Ext"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(63, 226)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Merchant"
        '
        'MerchantNameTextBox
        '
        Me.MerchantNameTextBox.Location = New System.Drawing.Point(121, 223)
        Me.MerchantNameTextBox.Name = "MerchantNameTextBox"
        Me.MerchantNameTextBox.ReadOnly = True
        Me.MerchantNameTextBox.Size = New System.Drawing.Size(266, 21)
        Me.MerchantNameTextBox.TabIndex = 15
        Me.MerchantNameTextBox.Text = "Merchant Name (23)"
        '
        'CardHolderTextBox
        '
        Me.CardHolderTextBox.AcceptsTab = True
        Me.CardHolderTextBox.Location = New System.Drawing.Point(121, 55)
        Me.CardHolderTextBox.Name = "CardHolderTextBox"
        Me.CardHolderTextBox.ReadOnly = True
        Me.CardHolderTextBox.Size = New System.Drawing.Size(266, 21)
        Me.CardHolderTextBox.TabIndex = 28
        Me.CardHolderTextBox.Text = "Card Holder (10)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(54, 346)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Declaration"
        '
        'TransactionResultTextBox
        '
        Me.TransactionResultTextBox.AcceptsTab = True
        Me.TransactionResultTextBox.Location = New System.Drawing.Point(121, 401)
        Me.TransactionResultTextBox.Name = "TransactionResultTextBox"
        Me.TransactionResultTextBox.ReadOnly = True
        Me.TransactionResultTextBox.Size = New System.Drawing.Size(120, 21)
        Me.TransactionResultTextBox.TabIndex = 37
        Me.TransactionResultTextBox.Text = "Result (3)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 322)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Retention reminder"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(77, 58)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(38, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "Holder"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(69, 298)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Address"
        '
        'PgtrTextBox
        '
        Me.PgtrTextBox.AcceptsTab = True
        Me.PgtrTextBox.Location = New System.Drawing.Point(121, 377)
        Me.PgtrTextBox.Name = "PgtrTextBox"
        Me.PgtrTextBox.ReadOnly = True
        Me.PgtrTextBox.Size = New System.Drawing.Size(120, 21)
        Me.PgtrTextBox.TabIndex = 25
        Me.PgtrTextBox.Text = "PGTR (28)"
        '
        'CustomerDeclarationTextBox
        '
        Me.CustomerDeclarationTextBox.Location = New System.Drawing.Point(121, 343)
        Me.CustomerDeclarationTextBox.Name = "CustomerDeclarationTextBox"
        Me.CustomerDeclarationTextBox.ReadOnly = True
        Me.CustomerDeclarationTextBox.Size = New System.Drawing.Size(266, 21)
        Me.CustomerDeclarationTextBox.TabIndex = 11
        Me.CustomerDeclarationTextBox.Text = "Customer Declaration (34)"
        '
        'MerchantAddressTextBox
        '
        Me.MerchantAddressTextBox.Location = New System.Drawing.Point(121, 295)
        Me.MerchantAddressTextBox.Name = "MerchantAddressTextBox"
        Me.MerchantAddressTextBox.ReadOnly = True
        Me.MerchantAddressTextBox.Size = New System.Drawing.Size(266, 21)
        Me.MerchantAddressTextBox.TabIndex = 10
        Me.MerchantAddressTextBox.Text = "Merchant Address (22)"
        '
        'RetentionReminderTextBox
        '
        Me.RetentionReminderTextBox.AcceptsTab = True
        Me.RetentionReminderTextBox.Location = New System.Drawing.Point(121, 319)
        Me.RetentionReminderTextBox.Name = "RetentionReminderTextBox"
        Me.RetentionReminderTextBox.ReadOnly = True
        Me.RetentionReminderTextBox.Size = New System.Drawing.Size(266, 21)
        Me.RetentionReminderTextBox.TabIndex = 9
        Me.RetentionReminderTextBox.Text = "Retention Reminder (33)"
        '
        'CardDescriptionTextBox
        '
        Me.CardDescriptionTextBox.AcceptsTab = True
        Me.CardDescriptionTextBox.Location = New System.Drawing.Point(121, 31)
        Me.CardDescriptionTextBox.Name = "CardDescriptionTextBox"
        Me.CardDescriptionTextBox.ReadOnly = True
        Me.CardDescriptionTextBox.Size = New System.Drawing.Size(266, 21)
        Me.CardDescriptionTextBox.TabIndex = 23
        Me.CardDescriptionTextBox.Text = "Card Description (6)"
        '
        'PTGR
        '
        Me.PTGR.AutoSize = True
        Me.PTGR.Location = New System.Drawing.Point(82, 380)
        Me.PTGR.Name = "PTGR"
        Me.PTGR.Size = New System.Drawing.Size(33, 13)
        Me.PTGR.TabIndex = 26
        Me.PTGR.Text = "PGTR"
        '
        'TerminalIdTextBox
        '
        Me.TerminalIdTextBox.AcceptsTab = True
        Me.TerminalIdTextBox.Location = New System.Drawing.Point(121, 271)
        Me.TerminalIdTextBox.Name = "TerminalIdTextBox"
        Me.TerminalIdTextBox.ReadOnly = True
        Me.TerminalIdTextBox.Size = New System.Drawing.Size(120, 21)
        Me.TerminalIdTextBox.TabIndex = 34
        Me.TerminalIdTextBox.Text = "Terminal Id (13)"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(18, 452)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(97, 13)
        Me.Label13.TabIndex = 31
        Me.Label13.Text = "Authorisation code"
        '
        'AuthorisationTextBox
        '
        Me.AuthorisationTextBox.AcceptsTab = True
        Me.AuthorisationTextBox.Location = New System.Drawing.Point(121, 449)
        Me.AuthorisationTextBox.Name = "AuthorisationTextBox"
        Me.AuthorisationTextBox.ReadOnly = True
        Me.AuthorisationTextBox.Size = New System.Drawing.Size(120, 21)
        Me.AuthorisationTextBox.TabIndex = 32
        Me.AuthorisationTextBox.Text = "Authorisation Code (4)"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(53, 34)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Description"
        '
        'BackgroundTabPage
        '
        Me.BackgroundTabPage.Controls.Add(Me.Label21)
        Me.BackgroundTabPage.Controls.Add(Me.Label20)
        Me.BackgroundTabPage.Controls.Add(Me.ServerPortTextBox)
        Me.BackgroundTabPage.Controls.Add(Me.ServerAddressTextBox)
        Me.BackgroundTabPage.Controls.Add(Me.ProgressListBox)
        Me.BackgroundTabPage.Location = New System.Drawing.Point(4, 22)
        Me.BackgroundTabPage.Name = "BackgroundTabPage"
        Me.BackgroundTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.BackgroundTabPage.Size = New System.Drawing.Size(429, 234)
        Me.BackgroundTabPage.TabIndex = 1
        Me.BackgroundTabPage.Text = "Background"
        Me.BackgroundTabPage.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(191, 242)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(27, 13)
        Me.Label21.TabIndex = 49
        Me.Label21.Text = "Port"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(6, 243)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(80, 13)
        Me.Label20.TabIndex = 48
        Me.Label20.Text = "Server address"
        '
        'ServerPortTextBox
        '
        Me.ServerPortTextBox.Location = New System.Drawing.Point(224, 240)
        Me.ServerPortTextBox.Name = "ServerPortTextBox"
        Me.ServerPortTextBox.ReadOnly = True
        Me.ServerPortTextBox.Size = New System.Drawing.Size(65, 21)
        Me.ServerPortTextBox.TabIndex = 47
        '
        'ServerAddressTextBox
        '
        Me.ServerAddressTextBox.Location = New System.Drawing.Point(93, 240)
        Me.ServerAddressTextBox.Name = "ServerAddressTextBox"
        Me.ServerAddressTextBox.ReadOnly = True
        Me.ServerAddressTextBox.Size = New System.Drawing.Size(83, 21)
        Me.ServerAddressTextBox.TabIndex = 46
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(300, 17)
        Me.ToolStripStatusLabel1.Text = "Disconnected."
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusStrip
        '
        Me.StatusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel1, Me.StatusLabel2})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 303)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(441, 22)
        Me.StatusStrip.SizingGrip = False
        Me.StatusStrip.TabIndex = 9
        Me.StatusStrip.Text = "StatusStrip"
        '
        'StatusLabel1
        '
        Me.StatusLabel1.AutoSize = False
        Me.StatusLabel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.StatusLabel1.Name = "StatusLabel1"
        Me.StatusLabel1.Size = New System.Drawing.Size(300, 17)
        Me.StatusLabel1.Text = "Ready."
        Me.StatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusLabel2
        '
        Me.StatusLabel2.AutoSize = False
        Me.StatusLabel2.Name = "StatusLabel2"
        Me.StatusLabel2.Size = New System.Drawing.Size(136, 17)
        Me.StatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FindFileDialog
        '
        Me.FindFileDialog.InitialDirectory = """C:\\"""
        Me.FindFileDialog.Title = "Browse to the Yes-Pay interface.."
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(378, 265)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 10
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'fYespayManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fYespayManager"
        Me.ShowIcon = False
        Me.Text = "YesPay"
        Me.TopMost = True
        Me.TabControl1.ResumeLayout(False)
        Me.TaskTabPage.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.BackgroundTabPage.ResumeLayout(False)
        Me.BackgroundTabPage.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressListBox As System.Windows.Forms.ListBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TaskTabPage As System.Windows.Forms.TabPage
    Friend WithEvents BackgroundTabPage As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CustomerDeclarationTextBox As System.Windows.Forms.TextBox
    Friend WithEvents MerchantAddressTextBox As System.Windows.Forms.TextBox
    Friend WithEvents RetentionReminderTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents MerchantNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CardDescriptionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TimeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents PTGR As System.Windows.Forms.Label
    Friend WithEvents PgtrTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CardHolderTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents AuthorisationTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents CardHolderExtendedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TerminalIdTextBox As System.Windows.Forms.TextBox
    Friend WithEvents MerchantIdTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TransactionResultTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents ExpiryDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents StartDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents RecieptNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents IssueNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents FindFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents ServerPortTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ServerAddressTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CardNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents StatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents KeyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents HideButton As System.Windows.Forms.Button
End Class
