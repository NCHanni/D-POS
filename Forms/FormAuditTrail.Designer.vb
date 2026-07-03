<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAuditTrail
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
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.pnlBottom = New Infragistics.Win.Misc.UltraGroupBox()
        Me.lblNote = New Infragistics.Win.Misc.UltraLabel()
        Me.btnPrint = New Infragistics.Win.Misc.UltraButton()
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton()
        Me.btnClose = New Infragistics.Win.Misc.UltraButton()
        Me.grpAuditTrail = New Infragistics.Win.Misc.UltraGroupBox()
        Me.grdAuditTrail = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dtpDateRange = New BusinessPro.Core.DateRange()
        Me.cmbUser = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.cmbTerminal = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        CType(Me.pnlBottom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBottom.SuspendLayout()
        CType(Me.grpAuditTrail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAuditTrail.SuspendLayout()
        CType(Me.grdAuditTrail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.cmbUser, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbTerminal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.lblNote)
        Me.pnlBottom.Controls.Add(Me.btnPrint)
        Me.pnlBottom.Controls.Add(Me.btnRefresh)
        Me.pnlBottom.Controls.Add(Me.btnClose)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 321)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(724, 40)
        Me.pnlBottom.TabIndex = 1
        '
        'lblNote
        '
        Appearance9.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblNote.Appearance = Appearance9
        Me.lblNote.AutoSize = True
        Me.lblNote.Location = New System.Drawing.Point(93, 12)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(33, 14)
        Me.lblNote.TabIndex = 1
        Me.lblNote.Text = "*Note"
        Me.lblNote.UseAppStyling = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnPrint.Location = New System.Drawing.Point(12, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "Print"
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnRefresh.Location = New System.Drawing.Point(556, 4)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "Refresh"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(637, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'grpAuditTrail
        '
        Me.grpAuditTrail.ContentPadding.Bottom = 5
        Me.grpAuditTrail.ContentPadding.Left = 3
        Me.grpAuditTrail.ContentPadding.Right = 3
        Me.grpAuditTrail.ContentPadding.Top = 4
        Me.grpAuditTrail.Controls.Add(Me.grdAuditTrail)
        Me.grpAuditTrail.Controls.Add(Me.Panel1)
        Me.grpAuditTrail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpAuditTrail.Location = New System.Drawing.Point(0, 0)
        Me.grpAuditTrail.Name = "grpAuditTrail"
        Me.grpAuditTrail.Size = New System.Drawing.Size(724, 321)
        Me.grpAuditTrail.TabIndex = 0
        '
        'grdAuditTrail
        '
        Appearance2.BackColor = System.Drawing.SystemColors.Window
        Appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grdAuditTrail.DisplayLayout.Appearance = Appearance2
        Me.grdAuditTrail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.grdAuditTrail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdAuditTrail.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.grdAuditTrail.DisplayLayout.EmptyRowSettings.ShowEmptyRows = True
        Me.grdAuditTrail.DisplayLayout.MaxColScrollRegions = 1
        Me.grdAuditTrail.DisplayLayout.MaxRowScrollRegions = 1
        Appearance3.BackColor = System.Drawing.SystemColors.Window
        Appearance3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grdAuditTrail.DisplayLayout.Override.ActiveCellAppearance = Appearance3
        Appearance4.BackColor = System.Drawing.SystemColors.Highlight
        Appearance4.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grdAuditTrail.DisplayLayout.Override.ActiveRowAppearance = Appearance4
        Me.grdAuditTrail.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grdAuditTrail.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Raised
        Appearance5.BackColor = System.Drawing.Color.Transparent
        Me.grdAuditTrail.DisplayLayout.Override.CardAreaAppearance = Appearance5
        Appearance6.BorderColor = System.Drawing.Color.Silver
        Appearance6.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grdAuditTrail.DisplayLayout.Override.CellAppearance = Appearance6
        Me.grdAuditTrail.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdAuditTrail.DisplayLayout.Override.CellPadding = 0
        Me.grdAuditTrail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grdAuditTrail.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Appearance7.BorderColor = System.Drawing.Color.Silver
        Me.grdAuditTrail.DisplayLayout.Override.RowAppearance = Appearance7
        Me.grdAuditTrail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance8.BackColor = System.Drawing.Color.Navy
        Appearance8.ForeColor = System.Drawing.Color.White
        Me.grdAuditTrail.DisplayLayout.Override.SelectedRowAppearance = Appearance8
        Me.grdAuditTrail.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdAuditTrail.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdAuditTrail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdAuditTrail.Location = New System.Drawing.Point(6, 39)
        Me.grdAuditTrail.Name = "grdAuditTrail"
        Me.grdAuditTrail.Size = New System.Drawing.Size(712, 274)
        Me.grdAuditTrail.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.UltraLabel2)
        Me.Panel1.Controls.Add(Me.UltraLabel1)
        Me.Panel1.Controls.Add(Me.cmbTerminal)
        Me.Panel1.Controls.Add(Me.cmbUser)
        Me.Panel1.Controls.Add(Me.dtpDateRange)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(6, 7)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(712, 32)
        Me.Panel1.TabIndex = 0
        '
        'dtpDateRange
        '
        Me.dtpDateRange.EndDate = New Date(2018, 11, 13, 23, 59, 59, 0)
        Me.dtpDateRange.Location = New System.Drawing.Point(3, 4)
        Me.dtpDateRange.Name = "dtpDateRange"
        Me.dtpDateRange.Size = New System.Drawing.Size(245, 24)
        Me.dtpDateRange.StartDate = New Date(2018, 10, 13, 0, 0, 0, 0)
        Me.dtpDateRange.TabIndex = 0
        '
        'cmbUser
        '
        Me.cmbUser.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        Me.cmbUser.Location = New System.Drawing.Point(288, 4)
        Me.cmbUser.Name = "cmbUser"
        Me.cmbUser.Size = New System.Drawing.Size(200, 21)
        Me.cmbUser.TabIndex = 2
        '
        'UltraLabel1
        '
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(254, 7)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(28, 14)
        Me.UltraLabel1.TabIndex = 1
        Me.UltraLabel1.Text = "User"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Location = New System.Drawing.Point(494, 7)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(48, 14)
        Me.UltraLabel2.TabIndex = 3
        Me.UltraLabel2.Text = "Terminal"
        '
        'cmbTerminal
        '
        Me.cmbTerminal.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        Me.cmbTerminal.Location = New System.Drawing.Point(548, 4)
        Me.cmbTerminal.Name = "cmbTerminal"
        Me.cmbTerminal.Size = New System.Drawing.Size(150, 21)
        Me.cmbTerminal.TabIndex = 4
        '
        'FormAuditTrail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(724, 361)
        Me.Controls.Add(Me.grpAuditTrail)
        Me.Controls.Add(Me.pnlBottom)
        Me.Name = "FormAuditTrail"
        Me.Text = "Audit Trail"
        CType(Me.pnlBottom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        CType(Me.grpAuditTrail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAuditTrail.ResumeLayout(False)
        CType(Me.grdAuditTrail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.cmbUser, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbTerminal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlBottom As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnClose As Infragistics.Win.Misc.UltraButton
    Friend WithEvents grpAuditTrail As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grdAuditTrail As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents dtpDateRange As DateRange
    Friend WithEvents btnPrint As Infragistics.Win.Misc.UltraButton
    Friend WithEvents lblNote As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents cmbUser As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents cmbTerminal As Infragistics.Win.UltraWinEditors.UltraComboEditor
End Class
