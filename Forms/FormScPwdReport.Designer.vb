<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormScPwdReport
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
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.grpButtons = New Infragistics.Win.Misc.UltraGroupBox()
        Me.btnPrint = New Infragistics.Win.Misc.UltraButton()
        Me.dateRange = New BusinessPro.Core.DateRange()
        Me.btnClose = New Infragistics.Win.Misc.UltraButton()
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton()
        Me.grpMainReport = New Infragistics.Win.Misc.UltraGroupBox()
        Me.grdScPwd = New Infragistics.Win.UltraWinGrid.UltraGrid()
        CType(Me.grpButtons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpButtons.SuspendLayout()
        CType(Me.grpMainReport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpMainReport.SuspendLayout()
        CType(Me.grdScPwd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpButtons
        '
        Me.grpButtons.Controls.Add(Me.btnPrint)
        Me.grpButtons.Controls.Add(Me.dateRange)
        Me.grpButtons.Controls.Add(Me.btnClose)
        Me.grpButtons.Controls.Add(Me.btnRefresh)
        Me.grpButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpButtons.Location = New System.Drawing.Point(0, 316)
        Me.grpButtons.Name = "grpButtons"
        Me.grpButtons.Size = New System.Drawing.Size(664, 45)
        Me.grpButtons.TabIndex = 1
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.Location = New System.Drawing.Point(415, 9)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Text = "Print"
        '
        'dateRange
        '
        Me.dateRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dateRange.EndDate = New Date(2018, 6, 19, 23, 59, 59, 0)
        Me.dateRange.Location = New System.Drawing.Point(12, 9)
        Me.dateRange.Name = "dateRange"
        Me.dateRange.Size = New System.Drawing.Size(245, 24)
        Me.dateRange.StartDate = New Date(2018, 5, 19, 0, 0, 0, 0)
        Me.dateRange.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(577, 9)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(496, 9)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 1
        Me.btnRefresh.Text = "Refresh"
        '
        'grpMainReport
        '
        Me.grpMainReport.ContentPadding.Left = 4
        Me.grpMainReport.ContentPadding.Right = 4
        Me.grpMainReport.ContentPadding.Top = 5
        Me.grpMainReport.Controls.Add(Me.grdScPwd)
        Me.grpMainReport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpMainReport.Location = New System.Drawing.Point(0, 0)
        Me.grpMainReport.Name = "grpMainReport"
        Me.grpMainReport.Size = New System.Drawing.Size(664, 316)
        Me.grpMainReport.TabIndex = 0
        '
        'grdScPwd
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grdScPwd.DisplayLayout.Appearance = Appearance1
        Me.grdScPwd.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdScPwd.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.grdScPwd.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdScPwd.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.grdScPwd.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdScPwd.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.grdScPwd.DisplayLayout.MaxColScrollRegions = 1
        Me.grdScPwd.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grdScPwd.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grdScPwd.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.grdScPwd.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grdScPwd.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.grdScPwd.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grdScPwd.DisplayLayout.Override.CellAppearance = Appearance8
        Me.grdScPwd.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdScPwd.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.grdScPwd.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.grdScPwd.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.grdScPwd.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grdScPwd.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.grdScPwd.DisplayLayout.Override.RowAppearance = Appearance11
        Me.grdScPwd.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdScPwd.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.grdScPwd.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdScPwd.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdScPwd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdScPwd.Location = New System.Drawing.Point(7, 8)
        Me.grdScPwd.Name = "grdScPwd"
        Me.grdScPwd.Size = New System.Drawing.Size(650, 305)
        Me.grdScPwd.TabIndex = 0
        Me.grdScPwd.Text = "UltraGrid1"
        '
        'FormScPwdReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(664, 361)
        Me.Controls.Add(Me.grpMainReport)
        Me.Controls.Add(Me.grpButtons)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(656, 377)
        Me.Name = "FormScPwdReport"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SC/PWD Report"
        CType(Me.grpButtons,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpButtons.ResumeLayout(false)
        CType(Me.grpMainReport,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpMainReport.ResumeLayout(false)
        CType(Me.grdScPwd,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents grpButtons As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grpMainReport As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grdScPwd As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents dateRange As DateRange
    Friend WithEvents btnClose As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnPrint As Infragistics.Win.Misc.UltraButton
End Class
