<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMechInterface
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
        Me.SampleIntervalTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideButton = New System.Windows.Forms.Button()
        Me.GeneralTab = New System.Windows.Forms.TabControl()
        Me.SummaryTab = New System.Windows.Forms.TabPage()
        Me.ServiceState = New MechInterface.uServiceState()
        Me.VmcSettingsTab = New System.Windows.Forms.TabPage()
        Me.VmcSettings = New MechInterface.uVmcSettings()
        Me.HomePositionTab = New System.Windows.Forms.TabPage()
        Me.AutoHome = New MechInterface.uAutoHome()
        Me.MechTestTab = New System.Windows.Forms.TabPage()
        Me.MechTest = New MechInterface.uMechTest()
        Me.PowerTab = New System.Windows.Forms.TabPage()
        Me.PowerMonitor = New MechInterface.uPowerMonitor()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.SerialLoader = New MechInterface.uSerialLoader()
        Me.ExplainationLabel = New System.Windows.Forms.Label()
        Me.GeneralTab.SuspendLayout()
        Me.SummaryTab.SuspendLayout()
        Me.VmcSettingsTab.SuspendLayout()
        Me.HomePositionTab.SuspendLayout()
        Me.MechTestTab.SuspendLayout()
        Me.PowerTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SampleIntervalTimer
        '
        Me.SampleIntervalTimer.Enabled = True
        Me.SampleIntervalTimer.Interval = 500
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'HideButton
        '
        Me.HideButton.Location = New System.Drawing.Point(377, 417)
        Me.HideButton.Name = "HideButton"
        Me.HideButton.Size = New System.Drawing.Size(59, 34)
        Me.HideButton.TabIndex = 11
        Me.HideButton.Text = "Hide"
        Me.HideButton.UseVisualStyleBackColor = True
        '
        'GeneralTab
        '
        Me.GeneralTab.Controls.Add(Me.SummaryTab)
        Me.GeneralTab.Controls.Add(Me.VmcSettingsTab)
        Me.GeneralTab.Controls.Add(Me.HomePositionTab)
        Me.GeneralTab.Controls.Add(Me.MechTestTab)
        Me.GeneralTab.Controls.Add(Me.PowerTab)
        Me.GeneralTab.Controls.Add(Me.TabPage1)
        Me.GeneralTab.Location = New System.Drawing.Point(1, 3)
        Me.GeneralTab.Name = "GeneralTab"
        Me.GeneralTab.SelectedIndex = 0
        Me.GeneralTab.Size = New System.Drawing.Size(438, 412)
        Me.GeneralTab.TabIndex = 12
        '
        'SummaryTab
        '
        Me.SummaryTab.Controls.Add(Me.ServiceState)
        Me.SummaryTab.Location = New System.Drawing.Point(4, 22)
        Me.SummaryTab.Name = "SummaryTab"
        Me.SummaryTab.Padding = New System.Windows.Forms.Padding(3)
        Me.SummaryTab.Size = New System.Drawing.Size(430, 386)
        Me.SummaryTab.TabIndex = 4
        Me.SummaryTab.Text = "Summary"
        Me.SummaryTab.UseVisualStyleBackColor = True
        '
        'ServiceState
        '
        Me.ServiceState.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ServiceState.Location = New System.Drawing.Point(3, 4)
        Me.ServiceState.Name = "ServiceState"
        Me.ServiceState.Size = New System.Drawing.Size(422, 379)
        Me.ServiceState.TabIndex = 0
        '
        'VmcSettingsTab
        '
        Me.VmcSettingsTab.Controls.Add(Me.VmcSettings)
        Me.VmcSettingsTab.Location = New System.Drawing.Point(4, 22)
        Me.VmcSettingsTab.Name = "VmcSettingsTab"
        Me.VmcSettingsTab.Padding = New System.Windows.Forms.Padding(3)
        Me.VmcSettingsTab.Size = New System.Drawing.Size(430, 386)
        Me.VmcSettingsTab.TabIndex = 0
        Me.VmcSettingsTab.Text = "VMC Settings"
        Me.VmcSettingsTab.UseVisualStyleBackColor = True
        '
        'VmcSettings
        '
        Me.VmcSettings.AutoSize = True
        Me.VmcSettings.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VmcSettings.Location = New System.Drawing.Point(4, 4)
        Me.VmcSettings.Name = "VmcSettings"
        Me.VmcSettings.Size = New System.Drawing.Size(426, 412)
        Me.VmcSettings.TabIndex = 0
        '
        'HomePositionTab
        '
        Me.HomePositionTab.Controls.Add(Me.AutoHome)
        Me.HomePositionTab.Location = New System.Drawing.Point(4, 22)
        Me.HomePositionTab.Name = "HomePositionTab"
        Me.HomePositionTab.Padding = New System.Windows.Forms.Padding(3)
        Me.HomePositionTab.Size = New System.Drawing.Size(430, 386)
        Me.HomePositionTab.TabIndex = 1
        Me.HomePositionTab.Text = "Home Position"
        Me.HomePositionTab.UseVisualStyleBackColor = True
        '
        'AutoHome
        '
        Me.AutoHome.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoHome.Location = New System.Drawing.Point(5, 2)
        Me.AutoHome.Name = "AutoHome"
        Me.AutoHome.Size = New System.Drawing.Size(422, 254)
        Me.AutoHome.TabIndex = 0
        '
        'MechTestTab
        '
        Me.MechTestTab.Controls.Add(Me.MechTest)
        Me.MechTestTab.Location = New System.Drawing.Point(4, 22)
        Me.MechTestTab.Name = "MechTestTab"
        Me.MechTestTab.Padding = New System.Windows.Forms.Padding(3)
        Me.MechTestTab.Size = New System.Drawing.Size(430, 386)
        Me.MechTestTab.TabIndex = 5
        Me.MechTestTab.Text = "Mech Test"
        Me.MechTestTab.UseVisualStyleBackColor = True
        '
        'MechTest
        '
        Me.MechTest.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MechTest.Location = New System.Drawing.Point(4, 2)
        Me.MechTest.Name = "MechTest"
        Me.MechTest.Size = New System.Drawing.Size(422, 384)
        Me.MechTest.TabIndex = 0
        '
        'PowerTab
        '
        Me.PowerTab.Controls.Add(Me.PowerMonitor)
        Me.PowerTab.Location = New System.Drawing.Point(4, 22)
        Me.PowerTab.Name = "PowerTab"
        Me.PowerTab.Padding = New System.Windows.Forms.Padding(3)
        Me.PowerTab.Size = New System.Drawing.Size(430, 386)
        Me.PowerTab.TabIndex = 6
        Me.PowerTab.Text = "Power"
        Me.PowerTab.UseVisualStyleBackColor = True
        '
        'PowerMonitor
        '
        Me.PowerMonitor.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PowerMonitor.Location = New System.Drawing.Point(6, -1)
        Me.PowerMonitor.Name = "PowerMonitor"
        Me.PowerMonitor.Size = New System.Drawing.Size(422, 254)
        Me.PowerMonitor.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.SerialLoader)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(430, 386)
        Me.TabPage1.TabIndex = 7
        Me.TabPage1.Text = "Serial Loader"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'SerialLoader
        '
        Me.SerialLoader.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SerialLoader.Location = New System.Drawing.Point(5, 5)
        Me.SerialLoader.Name = "SerialLoader"
        Me.SerialLoader.Size = New System.Drawing.Size(422, 254)
        Me.SerialLoader.TabIndex = 0
        '
        'ExplainationLabel
        '
        Me.ExplainationLabel.Location = New System.Drawing.Point(2, 291)
        Me.ExplainationLabel.Name = "ExplainationLabel"
        Me.ExplainationLabel.Size = New System.Drawing.Size(300, 32)
        Me.ExplainationLabel.TabIndex = 15
        '
        'fMechInterface
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 456)
        Me.ControlBox = False
        Me.Controls.Add(Me.GeneralTab)
        Me.Controls.Add(Me.ExplainationLabel)
        Me.Controls.Add(Me.HideButton)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fMechInterface"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Mechanism Interface"
        Me.TopMost = True
        Me.GeneralTab.ResumeLayout(False)
        Me.SummaryTab.ResumeLayout(False)
        Me.VmcSettingsTab.ResumeLayout(False)
        Me.VmcSettingsTab.PerformLayout()
        Me.HomePositionTab.ResumeLayout(False)
        Me.MechTestTab.ResumeLayout(False)
        Me.PowerTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SampleIntervalTimer As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HideButton As System.Windows.Forms.Button
    Friend WithEvents GeneralTab As System.Windows.Forms.TabControl
    Friend WithEvents HomePositionTab As System.Windows.Forms.TabPage
    Friend WithEvents VmcSettingsTab As System.Windows.Forms.TabPage
    Friend WithEvents VmcSettings As MechInterface.uVmcSettings
    Friend WithEvents AutoHome As MechInterface.uAutoHome
    Friend WithEvents SummaryTab As System.Windows.Forms.TabPage
    Friend WithEvents ServiceState As MechInterface.uServiceState
    Friend WithEvents ExplainationLabel As System.Windows.Forms.Label
    Friend WithEvents MechTestTab As System.Windows.Forms.TabPage
    Friend WithEvents MechTest As MechInterface.uMechTest
    Friend WithEvents PowerTab As System.Windows.Forms.TabPage
    Friend WithEvents PowerMonitor As MechInterface.uPowerMonitor
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents SerialLoader As MechInterface.uSerialLoader
End Class
