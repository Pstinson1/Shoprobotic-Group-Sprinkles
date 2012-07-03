<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uAutoHome
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
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.PositionXlabel = New System.Windows.Forms.Label
        Me.PositionYlabel = New System.Windows.Forms.Label
        Me.StorePositionButton = New System.Windows.Forms.Button
        Me.FindHomeButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(5, 147)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(17, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "2."
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(5, 55)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(17, 13)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "1."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(296, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "These buttons will let you configure the head home position."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 147)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(391, 26)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Once the head has stopped moving, check that the values in the 'head position' " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
            "boxes below look sensible. if they do click the 'position OK' button."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(365, 26)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Press the 'find home' button, this will move the head to the home position, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "fro" & _
            "m the correct direction."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(49, 193)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Head position"
        '
        'PositionXlabel
        '
        Me.PositionXlabel.BackColor = System.Drawing.Color.Black
        Me.PositionXlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PositionXlabel.Font = New System.Drawing.Font("Trebuchet MS", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PositionXlabel.ForeColor = System.Drawing.Color.White
        Me.PositionXlabel.Location = New System.Drawing.Point(127, 186)
        Me.PositionXlabel.Name = "PositionXlabel"
        Me.PositionXlabel.Size = New System.Drawing.Size(62, 25)
        Me.PositionXlabel.TabIndex = 15
        Me.PositionXlabel.Text = "???"
        Me.PositionXlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PositionYlabel
        '
        Me.PositionYlabel.BackColor = System.Drawing.Color.Black
        Me.PositionYlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PositionYlabel.Font = New System.Drawing.Font("Trebuchet MS", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PositionYlabel.ForeColor = System.Drawing.Color.White
        Me.PositionYlabel.Location = New System.Drawing.Point(195, 186)
        Me.PositionYlabel.Name = "PositionYlabel"
        Me.PositionYlabel.Size = New System.Drawing.Size(62, 25)
        Me.PositionYlabel.TabIndex = 14
        Me.PositionYlabel.Text = "???"
        Me.PositionYlabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'StorePositionButton
        '
        Me.StorePositionButton.Location = New System.Drawing.Point(305, 212)
        Me.StorePositionButton.Name = "StorePositionButton"
        Me.StorePositionButton.Size = New System.Drawing.Size(89, 28)
        Me.StorePositionButton.TabIndex = 13
        Me.StorePositionButton.Text = "Position OK"
        Me.StorePositionButton.UseVisualStyleBackColor = True
        '
        'FindHomeButton
        '
        Me.FindHomeButton.Location = New System.Drawing.Point(305, 88)
        Me.FindHomeButton.Name = "FindHomeButton"
        Me.FindHomeButton.Size = New System.Drawing.Size(89, 28)
        Me.FindHomeButton.TabIndex = 12
        Me.FindHomeButton.Text = "Find Home"
        Me.FindHomeButton.UseVisualStyleBackColor = True
        '
        'uAutoHome
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PositionXlabel)
        Me.Controls.Add(Me.PositionYlabel)
        Me.Controls.Add(Me.StorePositionButton)
        Me.Controls.Add(Me.FindHomeButton)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uAutoHome"
        Me.Size = New System.Drawing.Size(422, 254)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PositionXlabel As System.Windows.Forms.Label
    Friend WithEvents PositionYlabel As System.Windows.Forms.Label
    Friend WithEvents StorePositionButton As System.Windows.Forms.Button
    Friend WithEvents FindHomeButton As System.Windows.Forms.Button

End Class
