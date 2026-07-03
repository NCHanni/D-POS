<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCreditMemoBrowser
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
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.grpCreditMemo = New Infragistics.Win.Misc.UltraGroupBox()
        Me.grdCreditMemo = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.grpButtons = New Infragistics.Win.Misc.UltraGroupBox()
        Me.lblHint = New Infragistics.Win.Misc.UltraLabel()
        Me.btnOk = New Infragistics.Win.Misc.UltraButton()
        Me.btnCancel = New Infragistics.Win.Misc.UltraButton()
        CType(Me.grpCreditMemo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCreditMemo.SuspendLayout()
        CType(Me.grdCreditMemo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grpButtons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCreditMemo
        '
        Me.grpCreditMemo.ContentPadding.Bottom = 4
        Me.grpCreditMemo.ContentPadding.Left = 4
        Me.grpCreditMemo.ContentPadding.Right = 4
        Me.grpCreditMemo.ContentPadding.Top = 4
        Me.grpCreditMemo.Controls.Add(Me.grdCreditMemo)
        Me.grpCreditMemo.Controls.Add(Me.lblTotal)
        Me.grpCreditMemo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCreditMemo.Location = New System.Drawing.Point(0, 0)
        Me.grpCreditMemo.Name = "grpCreditMemo"
        Me.grpCreditMemo.Size = New System.Drawing.Size(484, 246)
        Me.grpCreditMemo.TabIndex = 2
        '
        'grdCreditMemo
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grdCreditMemo.DisplayLayout.Appearance = Appearance1
        Me.grdCreditMemo.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.grdCreditMemo.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdCreditMemo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.grdCreditMemo.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdCreditMemo.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.grdCreditMemo.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdCreditMemo.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.grdCreditMemo.DisplayLayout.MaxColScrollRegions = 1
        Me.grdCreditMemo.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grdCreditMemo.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grdCreditMemo.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.grdCreditMemo.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom
        Me.grdCreditMemo.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[True]
        Me.grdCreditMemo.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.grdCreditMemo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grdCreditMemo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.grdCreditMemo.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grdCreditMemo.DisplayLayout.Override.CellAppearance = Appearance8
        Me.grdCreditMemo.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdCreditMemo.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.grdCreditMemo.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.grdCreditMemo.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.grdCreditMemo.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grdCreditMemo.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.grdCreditMemo.DisplayLayout.Override.RowAppearance = Appearance11
        Me.grdCreditMemo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdCreditMemo.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.grdCreditMemo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdCreditMemo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdCreditMemo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdCreditMemo.Location = New System.Drawing.Point(7, 7)
        Me.grdCreditMemo.Name = "grdCreditMemo"
        Me.grdCreditMemo.Size = New System.Drawing.Size(470, 209)
        Me.grdCreditMemo.TabIndex = 1
        Me.grdCreditMemo.Text = "UltraGrid1"
        '
        'lblTotal
        '
        Me.lblTotal.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(7, 216)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Padding = New System.Windows.Forms.Padding(0, 3, 3, 0)
        Me.lblTotal.Size = New System.Drawing.Size(470, 23)
        Me.lblTotal.TabIndex = 2
        Me.lblTotal.Tag = "Total credit memo amount is {0}"
        Me.lblTotal.Text = "Total credit memo amount is 0.00"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpButtons
        '
        Me.grpButtons.Controls.Add(Me.lblHint)
        Me.grpButtons.Controls.Add(Me.btnOk)
        Me.grpButtons.Controls.Add(Me.btnCancel)
        Me.grpButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpButtons.Location = New System.Drawing.Point(0, 246)
        Me.grpButtons.Name = "grpButtons"
        Me.grpButtons.Size = New System.Drawing.Size(484, 35)
        Me.grpButtons.TabIndex = 3
        '
        'lblHint
        '
        Appearance14.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblHint.Appearance = Appearance14
        Me.lblHint.AutoSize = True
        Me.lblHint.Location = New System.Drawing.Point(12, 7)
        Me.lblHint.Name = "lblHint"
        Me.lblHint.Size = New System.Drawing.Size(273, 15)
        Me.lblHint.TabIndex = 2
        Me.lblHint.Text = "Select and press F5 to remove selected record."
        Me.lblHint.UseAppStyling = False
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(316, 2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(397, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'FormCreditMemoBrowser
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(484, 281)
        Me.Controls.Add(Me.grpCreditMemo)
        Me.Controls.Add(Me.grpButtons)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormCreditMemoBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Credit Memo (Customer Returns)"
        CType(Me.grpCreditMemo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCreditMemo.ResumeLayout(False)
        CType(Me.grdCreditMemo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grpButtons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpButtons.ResumeLayout(False)
        Me.grpButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCreditMemo As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grdCreditMemo As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents grpButtons As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnOk As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents lblHint As Infragistics.Win.Misc.UltraLabel
End Class
