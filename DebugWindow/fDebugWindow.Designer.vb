<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fDebugWindow
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
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ProgressListBox = New System.Windows.Forms.ListBox
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.HideButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.AddressList = New System.Windows.Forms.ListBox
        Me.StatusStrip.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 303)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(441, 22)
        Me.StatusStrip.SizingGrip = False
        Me.StatusStrip.TabIndex = 3
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(300, 17)
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressListBox
        '
        Me.ProgressListBox.FormattingEnabled = True
        Me.ProgressListBox.Location = New System.Drawing.Point(4, 5)
        Me.ProgressListBox.Name = "ProgressListBox"
        Me.ProgressListBox.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.ProgressListBox.Size = New System.Drawing.Size(420, 225)
        Me.ProgressListBox.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ProgressListBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(429, 236)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Progress"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(2, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(437, 262)
        Me.TabControl1.TabIndex = 5
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(380, 266)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 12
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 277)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Local End Point"
        '
        'AddressList
        '
        Me.AddressList.FormattingEnabled = True
        Me.AddressList.Location = New System.Drawing.Point(98, 268)
        Me.AddressList.Name = "AddressList"
        Me.AddressList.Size = New System.Drawing.Size(101, 30)
        Me.AddressList.TabIndex = 15
        '
        'fDebugWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.AddressList)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fDebugWindow"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Debug information"
        Me.TopMost = True
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ProgressListBox As System.Windows.Forms.ListBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents HideButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AddressList As System.Windows.Forms.ListBox
End Class
