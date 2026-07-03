<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormReportViewer
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
        Dim DateButton1 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton()
        Dim DateButton2 As Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton = New Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton()
        Me.pnlDateRange = New System.Windows.Forms.Panel()
        Me.lblInfo = New Infragistics.Win.Misc.UltraLabel()
        Me.lblFrom = New Infragistics.Win.Misc.UltraLabel()
        Me.lblTo = New Infragistics.Win.Misc.UltraLabel()
        Me.dtpDateFrom = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo()
        Me.dtpDateTo = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo()
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton()
        Me.lblDate = New Infragistics.Win.Misc.UltraLabel()
        Me.ucViewer = New BusinessPro.POS.Admin.ucReportViewer()
        Me.pnlDateRange.SuspendLayout()
        CType(Me.dtpDateFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpDateTo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlDateRange
        '
        Me.pnlDateRange.BackColor = System.Drawing.SystemColors.Info
        Me.pnlDateRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDateRange.Controls.Add(Me.lblInfo)
        Me.pnlDateRange.Controls.Add(Me.lblFrom)
        Me.pnlDateRange.Controls.Add(Me.lblTo)
        Me.pnlDateRange.Controls.Add(Me.dtpDateFrom)
        Me.pnlDateRange.Controls.Add(Me.dtpDateTo)
        Me.pnlDateRange.Controls.Add(Me.btnRefresh)
        Me.pnlDateRange.Controls.Add(Me.lblDate)
        Me.pnlDateRange.Location = New System.Drawing.Point(12, 61)
        Me.pnlDateRange.Name = "pnlDateRange"
        Me.pnlDateRange.Size = New System.Drawing.Size(265, 75)
        Me.pnlDateRange.TabIndex = 1
        Me.pnlDateRange.Visible = False
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Enabled = False
        Me.lblInfo.Location = New System.Drawing.Point(6, 46)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(106, 14)
        Me.lblInfo.TabIndex = 0
        Me.lblInfo.Text = "Show/Hide (Shift+F)"
        Me.lblInfo.UseAppStyling = False
        '
        'lblFrom
        '
        Me.lblFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(6, 15)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(31, 14)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'lblTo
        '
        Me.lblTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(139, 15)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(14, 14)
        Me.lblTo.TabIndex = 3
        Me.lblTo.Text = "to"
        '
        'dtpDateFrom
        '
        Me.dtpDateFrom.AllowNull = False
        Me.dtpDateFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpDateFrom.DateButtons.Add(DateButton1)
        Me.dtpDateFrom.Location = New System.Drawing.Point(43, 12)
        Me.dtpDateFrom.Name = "dtpDateFrom"
        Me.dtpDateFrom.NonAutoSizeHeight = 21
        Me.dtpDateFrom.Size = New System.Drawing.Size(90, 21)
        Me.dtpDateFrom.TabIndex = 1
        '
        'dtpDateTo
        '
        Me.dtpDateTo.AllowNull = False
        Me.dtpDateTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpDateTo.DateButtons.Add(DateButton2)
        Me.dtpDateTo.Location = New System.Drawing.Point(161, 12)
        Me.dtpDateTo.Name = "dtpDateTo"
        Me.dtpDateTo.NonAutoSizeHeight = 21
        Me.dtpDateTo.Size = New System.Drawing.Size(90, 21)
        Me.dtpDateTo.TabIndex = 4
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(176, 41)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 5
        Me.btnRefresh.Text = "Refresh"
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.AutoSize = True
        Me.lblDate.Location = New System.Drawing.Point(125, 15)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(28, 14)
        Me.lblDate.TabIndex = 2
        Me.lblDate.Text = "Date"
        Me.lblDate.Visible = False
        '
        'ucViewer
        '
        Me.ucViewer.AllowFind = False
        Me.ucViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ucViewer.Location = New System.Drawing.Point(0, 0)
        Me.ucViewer.ModuleName = ""
        Me.ucViewer.Name = "ucViewer"
        Me.ucViewer.ReportSource = Nothing
        Me.ucViewer.ShowExportButton = True
        Me.ucViewer.ShowPrintButton = True
        Me.ucViewer.Size = New System.Drawing.Size(632, 447)
        Me.ucViewer.TabIndex = 0
        '
        'FormReportViewer
        '
        Me.AcceptButton = Me.btnRefresh
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 447)
        Me.Controls.Add(Me.pnlDateRange)
        Me.Controls.Add(Me.ucViewer)
        Me.KeyPreview = True
        Me.Name = "FormReportViewer"
        Me.Text = "ReportViewer"
        Me.pnlDateRange.ResumeLayout(False)
        Me.pnlDateRange.PerformLayout()
        CType(Me.dtpDateFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpDateTo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ucViewer As BusinessPro.POS.Admin.ucReportViewer
    Friend WithEvents pnlDateRange As System.Windows.Forms.Panel
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents lblDate As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents dtpDateTo As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents lblTo As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents dtpDateFrom As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents lblFrom As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblInfo As Infragistics.Win.Misc.UltraLabel
End Class
