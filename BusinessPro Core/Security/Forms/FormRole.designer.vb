<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormRole
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
        Me.txtDescription = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.grdModuleSettings = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grdList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.grpSecurityRoleInformation = New Infragistics.Win.Misc.UltraGroupBox()
        Me.grpModuleSettings = New Infragistics.Win.Misc.UltraGroupBox()
        Me.pnlSelectAll = New Infragistics.Win.Misc.UltraPanel()
        Me.lblSuperRoleNote = New System.Windows.Forms.Label()
        Me.btnUncheckAll = New Infragistics.Win.Misc.UltraButton()
        Me.btnCheckAll = New Infragistics.Win.Misc.UltraButton()
        Me.ctrlMenu = New BusinessPro.Core.MenuButtons()
        Me.lblMessage = New Infragistics.Win.Misc.UltraLabel()
        Me.btnClose = New Infragistics.Win.Misc.UltraButton()
        Me.grpSecurityRoleList = New Infragistics.Win.Misc.UltraGroupBox()
        CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdModuleSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grpSecurityRoleInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSecurityRoleInformation.SuspendLayout()
        CType(Me.grpModuleSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpModuleSettings.SuspendLayout()
        Me.pnlSelectAll.ClientArea.SuspendLayout()
        Me.pnlSelectAll.SuspendLayout()
        CType(Me.grpSecurityRoleList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSecurityRoleList.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDescription
        '
        Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescription.AutoSize = False
        Me.txtDescription.Location = New System.Drawing.Point(333, 23)
        Me.txtDescription.MaximumSize = New System.Drawing.Size(314, 24)
        Me.txtDescription.MaxLength = 100
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(314, 24)
        Me.txtDescription.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.AutoSize = False
        Me.txtName.Location = New System.Drawing.Point(61, 23)
        Me.txtName.MaxLength = 32
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(200, 24)
        Me.txtName.TabIndex = 1
        Me.txtName.Tag = "Name"
        '
        'grdModuleSettings
        '
        Appearance1.BackColor = System.Drawing.Color.Silver
        Me.grdModuleSettings.DisplayLayout.Appearance = Appearance1
        Me.grdModuleSettings.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None
        Me.grdModuleSettings.DisplayLayout.EmptyRowSettings.ShowEmptyRows = True
        Me.grdModuleSettings.DisplayLayout.MaxColScrollRegions = 1
        Me.grdModuleSettings.DisplayLayout.MaxRowScrollRegions = 1
        Me.grdModuleSettings.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Raised
        Appearance2.BackColor = System.Drawing.Color.Transparent
        Me.grdModuleSettings.DisplayLayout.Override.CardAreaAppearance = Appearance2
        Me.grdModuleSettings.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdModuleSettings.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Appearance3.BorderColor = System.Drawing.Color.LightGray
        Me.grdModuleSettings.DisplayLayout.Override.RowAppearance = Appearance3
        Appearance4.BackColor = System.Drawing.Color.Navy
        Appearance4.ForeColor = System.Drawing.Color.White
        Me.grdModuleSettings.DisplayLayout.Override.SelectedRowAppearance = Appearance4
        Me.grdModuleSettings.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdModuleSettings.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdModuleSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdModuleSettings.Location = New System.Drawing.Point(4, 20)
        Me.grdModuleSettings.Name = "grdModuleSettings"
        Me.grdModuleSettings.Size = New System.Drawing.Size(651, 450)
        Me.grdModuleSettings.TabIndex = 0
        Me.grdModuleSettings.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(267, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Description"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Name*"
        '
        'grdList
        '
        Appearance5.BackColor = System.Drawing.Color.Silver
        Me.grdList.DisplayLayout.Appearance = Appearance5
        Me.grdList.DisplayLayout.EmptyRowSettings.ShowEmptyRows = True
        Me.grdList.DisplayLayout.MaxColScrollRegions = 1
        Me.grdList.DisplayLayout.MaxRowScrollRegions = 1
        Me.grdList.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Raised
        Appearance6.BackColor = System.Drawing.Color.Transparent
        Me.grdList.DisplayLayout.Override.CardAreaAppearance = Appearance6
        Me.grdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Appearance7.BorderColor = System.Drawing.Color.LightGray
        Me.grdList.DisplayLayout.Override.RowAppearance = Appearance7
        Appearance8.BackColor = System.Drawing.Color.Navy
        Appearance8.ForeColor = System.Drawing.Color.White
        Me.grdList.DisplayLayout.Override.SelectedRowAppearance = Appearance8
        Me.grdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grdList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdList.Location = New System.Drawing.Point(4, 8)
        Me.grdList.Name = "grdList"
        Me.grdList.Size = New System.Drawing.Size(267, 599)
        Me.grdList.TabIndex = 0
        Me.grdList.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'grpSecurityRoleInformation
        '
        Me.grpSecurityRoleInformation.Controls.Add(Me.txtDescription)
        Me.grpSecurityRoleInformation.Controls.Add(Me.Label2)
        Me.grpSecurityRoleInformation.Controls.Add(Me.txtName)
        Me.grpSecurityRoleInformation.Controls.Add(Me.Label5)
        Me.grpSecurityRoleInformation.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpSecurityRoleInformation.Location = New System.Drawing.Point(275, 0)
        Me.grpSecurityRoleInformation.Name = "grpSecurityRoleInformation"
        Me.grpSecurityRoleInformation.Size = New System.Drawing.Size(659, 60)
        Me.grpSecurityRoleInformation.TabIndex = 2
        Me.grpSecurityRoleInformation.Text = "Security Role Information"
        '
        'grpModuleSettings
        '
        Me.grpModuleSettings.ContentPadding.Bottom = 1
        Me.grpModuleSettings.ContentPadding.Left = 1
        Me.grpModuleSettings.ContentPadding.Right = 1
        Me.grpModuleSettings.ContentPadding.Top = 4
        Me.grpModuleSettings.Controls.Add(Me.grdModuleSettings)
        Me.grpModuleSettings.Controls.Add(Me.pnlSelectAll)
        Me.grpModuleSettings.Controls.Add(Me.ctrlMenu)
        Me.grpModuleSettings.Controls.Add(Me.lblMessage)
        Me.grpModuleSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpModuleSettings.Location = New System.Drawing.Point(275, 60)
        Me.grpModuleSettings.Name = "grpModuleSettings"
        Me.grpModuleSettings.Size = New System.Drawing.Size(659, 551)
        Me.grpModuleSettings.TabIndex = 3
        Me.grpModuleSettings.Text = "Module Settings"
        '
        'pnlSelectAll
        '
        '
        'pnlSelectAll.ClientArea
        '
        Me.pnlSelectAll.ClientArea.Controls.Add(Me.lblSuperRoleNote)
        Me.pnlSelectAll.ClientArea.Controls.Add(Me.btnUncheckAll)
        Me.pnlSelectAll.ClientArea.Controls.Add(Me.btnCheckAll)
        Me.pnlSelectAll.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlSelectAll.Location = New System.Drawing.Point(4, 470)
        Me.pnlSelectAll.Name = "pnlSelectAll"
        Me.pnlSelectAll.Size = New System.Drawing.Size(651, 38)
        Me.pnlSelectAll.TabIndex = 1
        '
        'lblSuperRoleNote
        '
        Me.lblSuperRoleNote.AutoSize = True
        Me.lblSuperRoleNote.ForeColor = System.Drawing.Color.Red
        Me.lblSuperRoleNote.Location = New System.Drawing.Point(12, 16)
        Me.lblSuperRoleNote.Name = "lblSuperRoleNote"
        Me.lblSuperRoleNote.Size = New System.Drawing.Size(317, 13)
        Me.lblSuperRoleNote.TabIndex = 8
        Me.lblSuperRoleNote.Text = "SUPER ROLE has access to all modules and cannot be modified."
        Me.lblSuperRoleNote.Visible = False
        '
        'btnUncheckAll
        '
        Me.btnUncheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUncheckAll.Location = New System.Drawing.Point(568, 11)
        Me.btnUncheckAll.Name = "btnUncheckAll"
        Me.btnUncheckAll.Size = New System.Drawing.Size(75, 23)
        Me.btnUncheckAll.TabIndex = 7
        Me.btnUncheckAll.Text = "Uncheck All"
        '
        'btnCheckAll
        '
        Me.btnCheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheckAll.Location = New System.Drawing.Point(487, 11)
        Me.btnCheckAll.Name = "btnCheckAll"
        Me.btnCheckAll.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckAll.TabIndex = 6
        Me.btnCheckAll.Text = "Check All"
        '
        'ctrlMenu
        '
        Me.ctrlMenu.AllowDelete = True
        Me.ctrlMenu.AllowEdit = True
        Me.ctrlMenu.AllowNew = True
        Me.ctrlMenu.ButtonHeight = 23
        Me.ctrlMenu.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ctrlMenu.HasPrint = False
        Me.ctrlMenu.Location = New System.Drawing.Point(4, 508)
        Me.ctrlMenu.MenuState = BusinessPro.Core.MenuButtons.MenuStates.[Default]
        Me.ctrlMenu.ModuleGroup = ""
        Me.ctrlMenu.ModuleName = Nothing
        Me.ctrlMenu.Name = "ctrlMenu"
        Me.ctrlMenu.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.ctrlMenu.Size = New System.Drawing.Size(651, 39)
        Me.ctrlMenu.TabIndex = 4
        Me.ctrlMenu.UseCloseButton = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Enabled = False
        Me.lblMessage.Location = New System.Drawing.Point(19, 40)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(183, 14)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "SELECT ROLE TO VIEW DETAILS"
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(61, 111)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(122, 23)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "HIdden Close Button"
        '
        'grpSecurityRoleList
        '
        Me.grpSecurityRoleList.ContentPadding.Bottom = 1
        Me.grpSecurityRoleList.ContentPadding.Left = 1
        Me.grpSecurityRoleList.ContentPadding.Right = 1
        Me.grpSecurityRoleList.ContentPadding.Top = 5
        Me.grpSecurityRoleList.Controls.Add(Me.grdList)
        Me.grpSecurityRoleList.Controls.Add(Me.btnClose)
        Me.grpSecurityRoleList.Dock = System.Windows.Forms.DockStyle.Left
        Me.grpSecurityRoleList.Location = New System.Drawing.Point(0, 0)
        Me.grpSecurityRoleList.Name = "grpSecurityRoleList"
        Me.grpSecurityRoleList.Size = New System.Drawing.Size(275, 611)
        Me.grpSecurityRoleList.TabIndex = 0
        '
        'FormRole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(934, 611)
        Me.Controls.Add(Me.grpModuleSettings)
        Me.Controls.Add(Me.grpSecurityRoleInformation)
        Me.Controls.Add(Me.grpSecurityRoleList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Name = "FormRole"
        Me.Text = "Roles"
        CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdModuleSettings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grpSecurityRoleInformation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSecurityRoleInformation.ResumeLayout(False)
        Me.grpSecurityRoleInformation.PerformLayout()
        CType(Me.grpModuleSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpModuleSettings.ResumeLayout(False)
        Me.grpModuleSettings.PerformLayout()
        Me.pnlSelectAll.ClientArea.ResumeLayout(False)
        Me.pnlSelectAll.ClientArea.PerformLayout()
        Me.pnlSelectAll.ResumeLayout(False)
        CType(Me.grpSecurityRoleList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSecurityRoleList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents grdModuleSettings As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents txtDescription As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents grpSecurityRoleInformation As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grpModuleSettings As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grpSecurityRoleList As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents pnlSelectAll As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents btnUncheckAll As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnCheckAll As Infragistics.Win.Misc.UltraButton
    Friend WithEvents lblMessage As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents btnClose As Infragistics.Win.Misc.UltraButton
    Friend WithEvents ctrlMenu As MenuButtons
    Friend WithEvents lblSuperRoleNote As Label
End Class
