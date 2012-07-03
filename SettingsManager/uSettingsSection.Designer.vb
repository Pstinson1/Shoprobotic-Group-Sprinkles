<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uSettingsSection
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
        Me.SettingsGrid = New System.Windows.Forms.DataGridView
        Me.ParameterName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ParameterValue = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.SettingsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SettingsGrid
        '
        Me.SettingsGrid.AllowUserToAddRows = False
        Me.SettingsGrid.AllowUserToDeleteRows = False
        Me.SettingsGrid.AllowUserToResizeRows = False
        Me.SettingsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SettingsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SettingsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ParameterName, Me.ParameterValue})
        Me.SettingsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.SettingsGrid.Location = New System.Drawing.Point(0, 2)
        Me.SettingsGrid.MultiSelect = False
        Me.SettingsGrid.Name = "SettingsGrid"
        Me.SettingsGrid.RowHeadersWidth = 20
        Me.SettingsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.SettingsGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.SettingsGrid.Size = New System.Drawing.Size(423, 238)
        Me.SettingsGrid.TabIndex = 2
        '
        'ParameterName
        '
        Me.ParameterName.HeaderText = "Parameter Name"
        Me.ParameterName.Name = "ParameterName"
        Me.ParameterName.Width = 140
        '
        'ParameterValue
        '
        Me.ParameterValue.HeaderText = "Value"
        Me.ParameterValue.Name = "ParameterValue"
        Me.ParameterValue.Width = 246
        '
        'uSettingsSection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SettingsGrid)
        Me.Name = "uSettingsSection"
        Me.Size = New System.Drawing.Size(427, 250)
        CType(Me.SettingsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SettingsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents ParameterName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ParameterValue As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
