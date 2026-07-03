<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateRange
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
        Dim DateButton1 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton
        Dim DateButton2 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton
        Me.lblDateFrom = New Infragistics.Win.Misc.UltraLabel
        Me.lblDateTo = New Infragistics.Win.Misc.UltraLabel
        Me.dtpDateTo = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
        Me.dtpDateFrom = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
        CType(Me.dtpDateTo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpDateFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(-2, 3)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(31, 14)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "From"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(133, 3)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(17, 14)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'dtpDateTo
        '
        Me.dtpDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.dtpDateTo.DateButtons.Add(DateButton1)
        Me.dtpDateTo.Location = New System.Drawing.Point(156, 0)
        Me.dtpDateTo.Name = "dtpDateTo"
        Me.dtpDateTo.NonAutoSizeHeight = 21
        Me.dtpDateTo.Size = New System.Drawing.Size(88, 21)
        Me.dtpDateTo.TabIndex = 3
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.BackColor = System.Drawing.SystemColors.Window
        Me.dtpDateFrom.DateButtons.Add(DateButton2)
        Me.dtpDateFrom.Location = New System.Drawing.Point(35, 0)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.NonAutoSizeHeight = 21
        Me.dtpDateFrom.Size = New System.Drawing.Size(88, 21)
        Me.dtpDateFrom.TabIndex = 1
        '
        'DateRange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblDateFrom)
        Me.Controls.Add(Me.lblDateTo)
        Me.Controls.Add(Me.dtpDateTo)
        Me.Controls.Add(Me.dtpDateFrom)
        Me.Name = "DateRange"
        Me.Size = New System.Drawing.Size(245, 21)
        CType(Me.dtpDateTo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpDateFrom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Protected WithEvents lblDateFrom As Infragistics.Win.Misc.UltraLabel
    Protected WithEvents lblDateTo As Infragistics.Win.Misc.UltraLabel
    Protected WithEvents dtpDateTo As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Protected WithEvents dtpDateFrom As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo

End Class
