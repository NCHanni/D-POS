<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormReset
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.UltraLabel8 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.lblResetCount = New Infragistics.Win.Misc.UltraLabel()
        Me.lblResetDate = New Infragistics.Win.Misc.UltraLabel()
        Me.lblResetAmount = New Infragistics.Win.Misc.UltraLabel()
        Me.lblCurrentAmount = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.SuspendLayout()
        '
        'UltraLabel8
        '
        Appearance1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.UltraLabel8.Appearance = Appearance1
        Me.UltraLabel8.AutoSize = True
        Me.UltraLabel8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel8.Location = New System.Drawing.Point(14, 17)
        Me.UltraLabel8.Name = "UltraLabel8"
        Me.UltraLabel8.Size = New System.Drawing.Size(86, 15)
        Me.UltraLabel8.TabIndex = 7
        Me.UltraLabel8.Text = "Reset Count:"
        '
        'UltraLabel1
        '
        Appearance2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.UltraLabel1.Appearance = Appearance2
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel1.Location = New System.Drawing.Point(14, 58)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(137, 15)
        Me.UltraLabel1.TabIndex = 8
        Me.UltraLabel1.Text = "Previous Reset Date:"
        '
        'UltraLabel2
        '
        Appearance3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.UltraLabel2.Appearance = Appearance3
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel2.Location = New System.Drawing.Point(14, 79)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(157, 15)
        Me.UltraLabel2.TabIndex = 9
        Me.UltraLabel2.Text = "Previous Reset Amount:"
        '
        'lblResetCount
        '
        Appearance4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Appearance4.TextHAlignAsString = "Right"
        Me.lblResetCount.Appearance = Appearance4
        Me.lblResetCount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResetCount.Location = New System.Drawing.Point(106, 17)
        Me.lblResetCount.Name = "lblResetCount"
        Me.lblResetCount.Size = New System.Drawing.Size(166, 15)
        Me.lblResetCount.TabIndex = 14
        Me.lblResetCount.Text = "0"
        '
        'lblResetDate
        '
        Appearance5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Appearance5.TextHAlignAsString = "Right"
        Me.lblResetDate.Appearance = Appearance5
        Me.lblResetDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResetDate.Location = New System.Drawing.Point(157, 58)
        Me.lblResetDate.Name = "lblResetDate"
        Me.lblResetDate.Size = New System.Drawing.Size(115, 15)
        Me.lblResetDate.TabIndex = 15
        Me.lblResetDate.Text = "0"
        '
        'lblResetAmount
        '
        Appearance6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Appearance6.TextHAlignAsString = "Right"
        Me.lblResetAmount.Appearance = Appearance6
        Me.lblResetAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResetAmount.Location = New System.Drawing.Point(177, 79)
        Me.lblResetAmount.Name = "lblResetAmount"
        Me.lblResetAmount.Size = New System.Drawing.Size(95, 15)
        Me.lblResetAmount.TabIndex = 16
        Me.lblResetAmount.Text = "0.00"
        '
        'lblCurrentAmount
        '
        Appearance7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Appearance7.TextHAlignAsString = "Right"
        Me.lblCurrentAmount.Appearance = Appearance7
        Me.lblCurrentAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentAmount.Location = New System.Drawing.Point(131, 119)
        Me.lblCurrentAmount.Name = "lblCurrentAmount"
        Me.lblCurrentAmount.Size = New System.Drawing.Size(141, 15)
        Me.lblCurrentAmount.TabIndex = 18
        Me.lblCurrentAmount.Text = "0.00"
        '
        'UltraLabel5
        '
        Appearance8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.UltraLabel5.Appearance = Appearance8
        Me.UltraLabel5.AutoSize = True
        Me.UltraLabel5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel5.Location = New System.Drawing.Point(14, 119)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(111, 15)
        Me.UltraLabel5.TabIndex = 17
        Me.UltraLabel5.Text = "Current Amount:"
        '
        'FormReset
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 156)
        Me.Controls.Add(Me.lblCurrentAmount)
        Me.Controls.Add(Me.UltraLabel5)
        Me.Controls.Add(Me.lblResetAmount)
        Me.Controls.Add(Me.lblResetDate)
        Me.Controls.Add(Me.lblResetCount)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel8)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormReset"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Reset Details"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents UltraLabel8 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblResetCount As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblResetDate As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblResetAmount As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblCurrentAmount As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
End Class
