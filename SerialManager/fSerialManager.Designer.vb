<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fSerialManager
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
        Me.components = New System.ComponentModel.Container()
        Me.SerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ProgressListBox = New System.Windows.Forms.ListBox()
        Me.CommandLineTextBox = New System.Windows.Forms.TextBox()
        Me.HideButton = New System.Windows.Forms.Button()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.WanderShowRadio = New System.Windows.Forms.RadioButton()
        Me.WanderHideRadio = New System.Windows.Forms.RadioButton()
        Me.DefaultButton = New System.Windows.Forms.Button()
        Me.ShowAllButton = New System.Windows.Forms.Button()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.MechdrvShowRadio = New System.Windows.Forms.RadioButton()
        Me.MechdrvHideRadio = New System.Windows.Forms.RadioButton()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.ArmdrvShowRadio = New System.Windows.Forms.RadioButton()
        Me.ArmdrvHideRadio = New System.Windows.Forms.RadioButton()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GetHealthShowRadio = New System.Windows.Forms.RadioButton()
        Me.GetHealthHideRadio = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GetSwitchesShowRadio = New System.Windows.Forms.RadioButton()
        Me.GetSwitchesHideRadio = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.AdcShowRadio = New System.Windows.Forms.RadioButton()
        Me.AdcHideRadio = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.MdbIgnoreNoneRadio = New System.Windows.Forms.RadioButton()
        Me.MdbIgnorePollRadio = New System.Windows.Forms.RadioButton()
        Me.MdbIgnoreAllRadio = New System.Windows.Forms.RadioButton()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.GetCountersShowRadio = New System.Windows.Forms.RadioButton()
        Me.GetCountersHideRadio = New System.Windows.Forms.RadioButton()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
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
        'ProgressListBox
        '
        Me.ProgressListBox.FormattingEnabled = True
        Me.ProgressListBox.IntegralHeight = False
        Me.ProgressListBox.Location = New System.Drawing.Point(3, 5)
        Me.ProgressListBox.Name = "ProgressListBox"
        Me.ProgressListBox.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.ProgressListBox.Size = New System.Drawing.Size(420, 220)
        Me.ProgressListBox.TabIndex = 2
        '
        'CommandLineTextBox
        '
        Me.CommandLineTextBox.Location = New System.Drawing.Point(4, 279)
        Me.CommandLineTextBox.Name = "CommandLineTextBox"
        Me.CommandLineTextBox.Size = New System.Drawing.Size(298, 21)
        Me.CommandLineTextBox.TabIndex = 4
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(378, 266)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 12
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'ClearButton
        '
        Me.ClearButton.Location = New System.Drawing.Point(317, 266)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(59, 34)
        Me.ClearButton.TabIndex = 13
        Me.ClearButton.Text = "Clear"
        Me.ClearButton.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(3, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(436, 257)
        Me.TabControl1.TabIndex = 15
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ProgressListBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(428, 231)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Activity"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox8)
        Me.TabPage2.Controls.Add(Me.GroupBox7)
        Me.TabPage2.Controls.Add(Me.DefaultButton)
        Me.TabPage2.Controls.Add(Me.ShowAllButton)
        Me.TabPage2.Controls.Add(Me.GroupBox6)
        Me.TabPage2.Controls.Add(Me.GroupBox5)
        Me.TabPage2.Controls.Add(Me.GroupBox4)
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(428, 231)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Options"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.WanderShowRadio)
        Me.GroupBox7.Controls.Add(Me.WanderHideRadio)
        Me.GroupBox7.Location = New System.Drawing.Point(6, 148)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox7.TabIndex = 23
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "WANDER"
        '
        'WanderShowRadio
        '
        Me.WanderShowRadio.AutoSize = True
        Me.WanderShowRadio.Location = New System.Drawing.Point(14, 37)
        Me.WanderShowRadio.Name = "WanderShowRadio"
        Me.WanderShowRadio.Size = New System.Drawing.Size(52, 17)
        Me.WanderShowRadio.TabIndex = 4
        Me.WanderShowRadio.Text = "Show"
        Me.WanderShowRadio.UseVisualStyleBackColor = True
        '
        'WanderHideRadio
        '
        Me.WanderHideRadio.AutoSize = True
        Me.WanderHideRadio.Checked = True
        Me.WanderHideRadio.Location = New System.Drawing.Point(14, 19)
        Me.WanderHideRadio.Name = "WanderHideRadio"
        Me.WanderHideRadio.Size = New System.Drawing.Size(47, 17)
        Me.WanderHideRadio.TabIndex = 3
        Me.WanderHideRadio.TabStop = True
        Me.WanderHideRadio.Text = "Hide"
        Me.WanderHideRadio.UseVisualStyleBackColor = True
        '
        'DefaultButton
        '
        Me.DefaultButton.Location = New System.Drawing.Point(360, 197)
        Me.DefaultButton.Name = "DefaultButton"
        Me.DefaultButton.Size = New System.Drawing.Size(60, 28)
        Me.DefaultButton.TabIndex = 22
        Me.DefaultButton.Text = "Default"
        Me.DefaultButton.UseVisualStyleBackColor = True
        '
        'ShowAllButton
        '
        Me.ShowAllButton.Location = New System.Drawing.Point(299, 197)
        Me.ShowAllButton.Name = "ShowAllButton"
        Me.ShowAllButton.Size = New System.Drawing.Size(60, 28)
        Me.ShowAllButton.TabIndex = 21
        Me.ShowAllButton.Text = "Show All"
        Me.ShowAllButton.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.MechdrvShowRadio)
        Me.GroupBox6.Controls.Add(Me.MechdrvHideRadio)
        Me.GroupBox6.Location = New System.Drawing.Point(146, 148)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox6.TabIndex = 20
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "MECHDRV"
        '
        'MechdrvShowRadio
        '
        Me.MechdrvShowRadio.AutoSize = True
        Me.MechdrvShowRadio.Location = New System.Drawing.Point(14, 37)
        Me.MechdrvShowRadio.Name = "MechdrvShowRadio"
        Me.MechdrvShowRadio.Size = New System.Drawing.Size(52, 17)
        Me.MechdrvShowRadio.TabIndex = 4
        Me.MechdrvShowRadio.Text = "Show"
        Me.MechdrvShowRadio.UseVisualStyleBackColor = True
        '
        'MechdrvHideRadio
        '
        Me.MechdrvHideRadio.AutoSize = True
        Me.MechdrvHideRadio.Checked = True
        Me.MechdrvHideRadio.Location = New System.Drawing.Point(14, 19)
        Me.MechdrvHideRadio.Name = "MechdrvHideRadio"
        Me.MechdrvHideRadio.Size = New System.Drawing.Size(47, 17)
        Me.MechdrvHideRadio.TabIndex = 3
        Me.MechdrvHideRadio.TabStop = True
        Me.MechdrvHideRadio.Text = "Hide"
        Me.MechdrvHideRadio.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.ArmdrvShowRadio)
        Me.GroupBox5.Controls.Add(Me.ArmdrvHideRadio)
        Me.GroupBox5.Location = New System.Drawing.Point(6, 75)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox5.TabIndex = 19
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "ARMDRV"
        '
        'ArmdrvShowRadio
        '
        Me.ArmdrvShowRadio.AutoSize = True
        Me.ArmdrvShowRadio.Location = New System.Drawing.Point(14, 37)
        Me.ArmdrvShowRadio.Name = "ArmdrvShowRadio"
        Me.ArmdrvShowRadio.Size = New System.Drawing.Size(52, 17)
        Me.ArmdrvShowRadio.TabIndex = 4
        Me.ArmdrvShowRadio.Text = "Show"
        Me.ArmdrvShowRadio.UseVisualStyleBackColor = True
        '
        'ArmdrvHideRadio
        '
        Me.ArmdrvHideRadio.AutoSize = True
        Me.ArmdrvHideRadio.Checked = True
        Me.ArmdrvHideRadio.Location = New System.Drawing.Point(14, 19)
        Me.ArmdrvHideRadio.Name = "ArmdrvHideRadio"
        Me.ArmdrvHideRadio.Size = New System.Drawing.Size(47, 17)
        Me.ArmdrvHideRadio.TabIndex = 3
        Me.ArmdrvHideRadio.TabStop = True
        Me.ArmdrvHideRadio.Text = "Hide"
        Me.ArmdrvHideRadio.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.GetHealthShowRadio)
        Me.GroupBox4.Controls.Add(Me.GetHealthHideRadio)
        Me.GroupBox4.Location = New System.Drawing.Point(286, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox4.TabIndex = 18
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "GETHEALTH"
        '
        'GetHealthShowRadio
        '
        Me.GetHealthShowRadio.AutoSize = True
        Me.GetHealthShowRadio.Location = New System.Drawing.Point(18, 37)
        Me.GetHealthShowRadio.Name = "GetHealthShowRadio"
        Me.GetHealthShowRadio.Size = New System.Drawing.Size(52, 17)
        Me.GetHealthShowRadio.TabIndex = 2
        Me.GetHealthShowRadio.Text = "Show"
        Me.GetHealthShowRadio.UseVisualStyleBackColor = True
        '
        'GetHealthHideRadio
        '
        Me.GetHealthHideRadio.AutoSize = True
        Me.GetHealthHideRadio.Checked = True
        Me.GetHealthHideRadio.Location = New System.Drawing.Point(18, 19)
        Me.GetHealthHideRadio.Name = "GetHealthHideRadio"
        Me.GetHealthHideRadio.Size = New System.Drawing.Size(47, 17)
        Me.GetHealthHideRadio.TabIndex = 1
        Me.GetHealthHideRadio.TabStop = True
        Me.GetHealthHideRadio.Text = "Hide"
        Me.GetHealthHideRadio.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.GetSwitchesShowRadio)
        Me.GroupBox3.Controls.Add(Me.GetSwitchesHideRadio)
        Me.GroupBox3.Location = New System.Drawing.Point(146, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox3.TabIndex = 17
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "GETSWITCHES"
        '
        'GetSwitchesShowRadio
        '
        Me.GetSwitchesShowRadio.AutoSize = True
        Me.GetSwitchesShowRadio.Location = New System.Drawing.Point(14, 37)
        Me.GetSwitchesShowRadio.Name = "GetSwitchesShowRadio"
        Me.GetSwitchesShowRadio.Size = New System.Drawing.Size(52, 17)
        Me.GetSwitchesShowRadio.TabIndex = 6
        Me.GetSwitchesShowRadio.Text = "Show"
        Me.GetSwitchesShowRadio.UseVisualStyleBackColor = True
        '
        'GetSwitchesHideRadio
        '
        Me.GetSwitchesHideRadio.AutoSize = True
        Me.GetSwitchesHideRadio.Checked = True
        Me.GetSwitchesHideRadio.Location = New System.Drawing.Point(14, 19)
        Me.GetSwitchesHideRadio.Name = "GetSwitchesHideRadio"
        Me.GetSwitchesHideRadio.Size = New System.Drawing.Size(47, 17)
        Me.GetSwitchesHideRadio.TabIndex = 5
        Me.GetSwitchesHideRadio.TabStop = True
        Me.GetSwitchesHideRadio.Text = "Hide"
        Me.GetSwitchesHideRadio.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.AdcShowRadio)
        Me.GroupBox2.Controls.Add(Me.AdcHideRadio)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox2.TabIndex = 16
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "ADC"
        '
        'AdcShowRadio
        '
        Me.AdcShowRadio.AutoSize = True
        Me.AdcShowRadio.Location = New System.Drawing.Point(14, 37)
        Me.AdcShowRadio.Name = "AdcShowRadio"
        Me.AdcShowRadio.Size = New System.Drawing.Size(52, 17)
        Me.AdcShowRadio.TabIndex = 4
        Me.AdcShowRadio.Text = "Show"
        Me.AdcShowRadio.UseVisualStyleBackColor = True
        '
        'AdcHideRadio
        '
        Me.AdcHideRadio.AutoSize = True
        Me.AdcHideRadio.Checked = True
        Me.AdcHideRadio.Location = New System.Drawing.Point(14, 19)
        Me.AdcHideRadio.Name = "AdcHideRadio"
        Me.AdcHideRadio.Size = New System.Drawing.Size(47, 17)
        Me.AdcHideRadio.TabIndex = 3
        Me.AdcHideRadio.TabStop = True
        Me.AdcHideRadio.Text = "Hide"
        Me.AdcHideRadio.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.MdbIgnoreNoneRadio)
        Me.GroupBox1.Controls.Add(Me.MdbIgnorePollRadio)
        Me.GroupBox1.Controls.Add(Me.MdbIgnoreAllRadio)
        Me.GroupBox1.Location = New System.Drawing.Point(286, 75)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(134, 83)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "MDB"
        '
        'MdbIgnoreNoneRadio
        '
        Me.MdbIgnoreNoneRadio.AutoSize = True
        Me.MdbIgnoreNoneRadio.Location = New System.Drawing.Point(14, 56)
        Me.MdbIgnoreNoneRadio.Name = "MdbIgnoreNoneRadio"
        Me.MdbIgnoreNoneRadio.Size = New System.Drawing.Size(82, 17)
        Me.MdbIgnoreNoneRadio.TabIndex = 2
        Me.MdbIgnoreNoneRadio.Text = "Ignore none"
        Me.MdbIgnoreNoneRadio.UseVisualStyleBackColor = True
        '
        'MdbIgnorePollRadio
        '
        Me.MdbIgnorePollRadio.AutoSize = True
        Me.MdbIgnorePollRadio.Checked = True
        Me.MdbIgnorePollRadio.Location = New System.Drawing.Point(14, 37)
        Me.MdbIgnorePollRadio.Name = "MdbIgnorePollRadio"
        Me.MdbIgnorePollRadio.Size = New System.Drawing.Size(74, 17)
        Me.MdbIgnorePollRadio.TabIndex = 1
        Me.MdbIgnorePollRadio.TabStop = True
        Me.MdbIgnorePollRadio.Text = "Ignore poll"
        Me.MdbIgnorePollRadio.UseVisualStyleBackColor = True
        '
        'MdbIgnoreAllRadio
        '
        Me.MdbIgnoreAllRadio.AutoSize = True
        Me.MdbIgnoreAllRadio.Location = New System.Drawing.Point(14, 18)
        Me.MdbIgnoreAllRadio.Name = "MdbIgnoreAllRadio"
        Me.MdbIgnoreAllRadio.Size = New System.Drawing.Size(68, 17)
        Me.MdbIgnoreAllRadio.TabIndex = 0
        Me.MdbIgnoreAllRadio.Text = "Ignore all"
        Me.MdbIgnoreAllRadio.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.GetCountersShowRadio)
        Me.GroupBox8.Controls.Add(Me.GetCountersHideRadio)
        Me.GroupBox8.Location = New System.Drawing.Point(146, 75)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(134, 67)
        Me.GroupBox8.TabIndex = 24
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "GETCOUNTERS"
        '
        'GetCountersShowRadio
        '
        Me.GetCountersShowRadio.AutoSize = True
        Me.GetCountersShowRadio.Location = New System.Drawing.Point(14, 37)
        Me.GetCountersShowRadio.Name = "GetCountersShowRadio"
        Me.GetCountersShowRadio.Size = New System.Drawing.Size(51, 17)
        Me.GetCountersShowRadio.TabIndex = 6
        Me.GetCountersShowRadio.Text = "Show"
        Me.GetCountersShowRadio.UseVisualStyleBackColor = True
        '
        'GetCountersHideRadio
        '
        Me.GetCountersHideRadio.AutoSize = True
        Me.GetCountersHideRadio.Checked = True
        Me.GetCountersHideRadio.Location = New System.Drawing.Point(14, 19)
        Me.GetCountersHideRadio.Name = "GetCountersHideRadio"
        Me.GetCountersHideRadio.Size = New System.Drawing.Size(46, 17)
        Me.GetCountersHideRadio.TabIndex = 5
        Me.GetCountersHideRadio.TabStop = True
        Me.GetCountersHideRadio.Text = "Hide"
        Me.GetCountersHideRadio.UseVisualStyleBackColor = True
        '
        'fSerialManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.HideButton)
        Me.Controls.Add(Me.CommandLineTextBox)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fSerialManager"
        Me.ShowInTaskbar = False
        Me.Text = "Serial Manager"
        Me.TopMost = True
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SerialPort As System.IO.Ports.SerialPort
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ProgressListBox As System.Windows.Forms.ListBox
    Friend WithEvents CommandLineTextBox As System.Windows.Forms.TextBox
    Friend WithEvents HideButton As System.Windows.Forms.Button
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents MdbIgnoreAllRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GetHealthShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GetHealthHideRadio As System.Windows.Forms.RadioButton
    Friend WithEvents MdbIgnoreNoneRadio As System.Windows.Forms.RadioButton
    Friend WithEvents MdbIgnorePollRadio As System.Windows.Forms.RadioButton
    Friend WithEvents AdcShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents AdcHideRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GetSwitchesShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GetSwitchesHideRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents ArmdrvShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents ArmdrvHideRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents MechdrvShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents MechdrvHideRadio As System.Windows.Forms.RadioButton
    Friend WithEvents DefaultButton As System.Windows.Forms.Button
    Friend WithEvents ShowAllButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents WanderShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents WanderHideRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GetCountersShowRadio As System.Windows.Forms.RadioButton
    Friend WithEvents GetCountersHideRadio As System.Windows.Forms.RadioButton
End Class
