<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucDateTimeRange
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
        Me.dtFrom = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.lblDateFrom = New Infragistics.Win.Misc.UltraLabel()
        Me.lblDateTo = New Infragistics.Win.Misc.UltraLabel()
        Me.dtTo = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        CType(Me.dtFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtTo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dtFrom
        '
        Me.dtFrom.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear
        Me.dtFrom.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime
        Me.dtFrom.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never
        Me.dtFrom.Location = New System.Drawing.Point(35, 0)
        Me.dtFrom.MaskInput = "{date} {time}"
        Me.dtFrom.Name = "dtFrom"
        Me.dtFrom.Size = New System.Drawing.Size(144, 21)
        Me.dtFrom.TabIndex = 0
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(-2, 4)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(31, 14)
        Me.lblDateFrom.TabIndex = 1
        Me.lblDateFrom.Text = "From"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(185, 4)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(17, 14)
        Me.lblDateTo.TabIndex = 3
        Me.lblDateTo.Text = "To"
        '
        'dtTo
        '
        Me.dtTo.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never
        Me.dtTo.Location = New System.Drawing.Point(208, 1)
        Me.dtTo.MaskInput = "{date} {time}"
        Me.dtTo.Name = "dtTo"
        Me.dtTo.Size = New System.Drawing.Size(144, 21)
        Me.dtTo.TabIndex = 4
        '
        'ucDateTimeRange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dtTo)
        Me.Controls.Add(Me.lblDateTo)
        Me.Controls.Add(Me.lblDateFrom)
        Me.Controls.Add(Me.dtFrom)
        Me.Name = "ucDateTimeRange"
        Me.Size = New System.Drawing.Size(358, 23)
        CType(Me.dtFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtTo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Protected WithEvents lblDateFrom As Infragistics.Win.Misc.UltraLabel
    Protected WithEvents lblDateTo As Infragistics.Win.Misc.UltraLabel
    Protected WithEvents dtFrom As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Protected WithEvents dtTo As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
End Class
