<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fAccessPayment
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
        Me.SerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ProgressList = New System.Windows.Forms.ListBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.DevicesCombo = New System.Windows.Forms.ComboBox
        Me.CardNumberLabel = New System.Windows.Forms.Label
        Me.HideButton = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SerialPort
        '
        Me.SerialPort.ReadTimeout = 500
        Me.SerialPort.WriteTimeout = 500
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 303)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(441, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ProgressList
        '
        Me.ProgressList.FormattingEnabled = True
        Me.ProgressList.IntegralHeight = False
        Me.ProgressList.Location = New System.Drawing.Point(3, 5)
        Me.ProgressList.Name = "ProgressList"
        Me.ProgressList.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.ProgressList.Size = New System.Drawing.Size(420, 108)
        Me.ProgressList.TabIndex = 2
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(3, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(436, 257)
        Me.TabControl1.TabIndex = 15
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.DevicesCombo)
        Me.TabPage1.Controls.Add(Me.CardNumberLabel)
        Me.TabPage1.Controls.Add(Me.ProgressList)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(428, 231)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Activity"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'DevicesCombo
        '
        Me.DevicesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DevicesCombo.FormattingEnabled = True
        Me.DevicesCombo.Location = New System.Drawing.Point(139, 119)
        Me.DevicesCombo.Name = "DevicesCombo"
        Me.DevicesCombo.Size = New System.Drawing.Size(284, 21)
        Me.DevicesCombo.TabIndex = 4
        '
        'CardNumberLabel
        '
        Me.CardNumberLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CardNumberLabel.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CardNumberLabel.Location = New System.Drawing.Point(244, 180)
        Me.CardNumberLabel.Name = "CardNumberLabel"
        Me.CardNumberLabel.Size = New System.Drawing.Size(179, 31)
        Me.CardNumberLabel.TabIndex = 3
        Me.CardNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(378, 266)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 16
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'fAccessPayment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fAccessPayment"
        Me.ShowInTaskbar = False
        Me.Text = "Access Card Payment"
        Me.TopMost = True
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SerialPort As System.IO.Ports.SerialPort
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ProgressList As System.Windows.Forms.ListBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents CardNumberLabel As System.Windows.Forms.Label
    Friend WithEvents DevicesCombo As System.Windows.Forms.ComboBox
    Friend WithEvents HideButton As System.Windows.Forms.Button
End Class
