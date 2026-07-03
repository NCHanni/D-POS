<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormUser
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.tabPageUsers = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.ctrlList = New BusinessPro.Core.ListSearch()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.btnView = New Infragistics.Win.Misc.UltraButton()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.grpLoginInformation = New Infragistics.Win.Misc.UltraGroupBox()
        Me.chkIsNavAccount = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.chkIsActive = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblLoginBarcodeHint = New System.Windows.Forms.Label()
        Me.lblLoginBarcode = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtUserName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtPassword = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtLoginBarcode = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtConfirmPassword = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.grpUserSetting = New Infragistics.Win.Misc.UltraGroupBox()
        Me.lblSecurityRole = New System.Windows.Forms.Label()
        Me.cbxSecurityRole = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.grpUserInformation = New Infragistics.Win.Misc.UltraGroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtCode = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.ctrlMenu = New BusinessPro.Core.MenuButtons()
        Me.tabSystemUsers = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tabPageUsers.SuspendLayout()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.grpLoginInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpLoginInformation.SuspendLayout()
        CType(Me.chkIsNavAccount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkIsActive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUserName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLoginBarcode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtConfirmPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grpUserSetting, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpUserSetting.SuspendLayout()
        CType(Me.cbxSecurityRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grpUserInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpUserInformation.SuspendLayout()
        CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tabSystemUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSystemUsers.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabPageUsers
        '
        Me.tabPageUsers.Controls.Add(Me.ctrlList)
        Me.tabPageUsers.Controls.Add(Me.UltraGroupBox1)
        Me.tabPageUsers.Location = New System.Drawing.Point(1, 23)
        Me.tabPageUsers.Name = "tabPageUsers"
        Me.tabPageUsers.Size = New System.Drawing.Size(900, 405)
        '
        'ctrlList
        '
        Me.ctrlList.ActiveRow = Nothing
        Me.ctrlList.BackColor = System.Drawing.Color.Transparent
        Me.ctrlList.CloseParentFormOnOK = True
        Me.ctrlList.DataSource = Nothing
        Me.ctrlList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlList.Location = New System.Drawing.Point(0, 41)
        Me.ctrlList.MultiSelect = False
        Me.ctrlList.MultiSelectHeader = False
        Me.ctrlList.Name = "ctrlList"
        Me.ctrlList.Padding = New System.Windows.Forms.Padding(3, 0, 3, 6)
        Me.ctrlList.SearchKeys = Nothing
        Me.ctrlList.ShowButtons = True
        Me.ctrlList.ShowSearch = True
        Me.ctrlList.Size = New System.Drawing.Size(900, 364)
        Me.ctrlList.TabIndex = 1
        Me.ctrlList.UsedInBrowser = False
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.btnView)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(900, 41)
        Me.UltraGroupBox1.TabIndex = 0
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.Location = New System.Drawing.Point(767, 14)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(120, 23)
        Me.btnView.TabIndex = 0
        Me.btnView.Text = "View Information"
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.grpLoginInformation)
        Me.UltraTabPageControl2.Controls.Add(Me.grpUserSetting)
        Me.UltraTabPageControl2.Controls.Add(Me.grpUserInformation)
        Me.UltraTabPageControl2.Controls.Add(Me.ctrlMenu)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(900, 405)
        '
        'grpLoginInformation
        '
        Me.grpLoginInformation.Controls.Add(Me.chkIsNavAccount)
        Me.grpLoginInformation.Controls.Add(Me.chkIsActive)
        Me.grpLoginInformation.Controls.Add(Me.Label8)
        Me.grpLoginInformation.Controls.Add(Me.Label1)
        Me.grpLoginInformation.Controls.Add(Me.Label2)
        Me.grpLoginInformation.Controls.Add(Me.lblLoginBarcodeHint)
        Me.grpLoginInformation.Controls.Add(Me.lblLoginBarcode)
        Me.grpLoginInformation.Controls.Add(Me.Label9)
        Me.grpLoginInformation.Controls.Add(Me.txtUserName)
        Me.grpLoginInformation.Controls.Add(Me.txtPassword)
        Me.grpLoginInformation.Controls.Add(Me.txtLoginBarcode)
        Me.grpLoginInformation.Controls.Add(Me.txtConfirmPassword)
        Me.grpLoginInformation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpLoginInformation.Location = New System.Drawing.Point(0, 185)
        Me.grpLoginInformation.Name = "grpLoginInformation"
        Me.grpLoginInformation.Size = New System.Drawing.Size(900, 220)
        Me.grpLoginInformation.TabIndex = 3
        Me.grpLoginInformation.Text = "Login Information"
        '
        'chkIsNavAccount
        '
        Appearance1.FontData.Name = "Microsoft Sans Serif"
        Appearance1.FontData.SizeInPoints = 8.25!
        Me.chkIsNavAccount.Appearance = Appearance1
        Me.chkIsNavAccount.AutoSize = True
        Me.chkIsNavAccount.Location = New System.Drawing.Point(117, 148)
        Me.chkIsNavAccount.Name = "chkIsNavAccount"
        Me.chkIsNavAccount.Size = New System.Drawing.Size(115, 17)
        Me.chkIsNavAccount.TabIndex = 9
        Me.chkIsNavAccount.Text = "NAV User Account"
        Me.chkIsNavAccount.Visible = False
        '
        'chkIsActive
        '
        Appearance2.FontData.Name = "Microsoft Sans Serif"
        Appearance2.FontData.SizeInPoints = 8.25!
        Me.chkIsActive.Appearance = Appearance2
        Me.chkIsActive.AutoSize = True
        Me.chkIsActive.Checked = True
        Me.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIsActive.Location = New System.Drawing.Point(117, 171)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.Size = New System.Drawing.Size(52, 17)
        Me.chkIsActive.TabIndex = 10
        Me.chkIsActive.Text = "Active"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(16, 34)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Username*"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Password*"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLoginBarcodeHint
        '
        Me.lblLoginBarcodeHint.AutoSize = True
        Me.lblLoginBarcodeHint.Enabled = False
        Me.lblLoginBarcodeHint.Location = New System.Drawing.Point(373, 124)
        Me.lblLoginBarcodeHint.Name = "lblLoginBarcodeHint"
        Me.lblLoginBarcodeHint.Size = New System.Drawing.Size(174, 13)
        Me.lblLoginBarcodeHint.TabIndex = 6
        Me.lblLoginBarcodeHint.Text = "• use alphanumeric characters only"
        Me.lblLoginBarcodeHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLoginBarcode
        '
        Me.lblLoginBarcode.AutoSize = True
        Me.lblLoginBarcode.Location = New System.Drawing.Point(16, 122)
        Me.lblLoginBarcode.Name = "lblLoginBarcode"
        Me.lblLoginBarcode.Size = New System.Drawing.Size(76, 13)
        Me.lblLoginBarcode.TabIndex = 6
        Me.lblLoginBarcode.Text = "Login Barcode"
        Me.lblLoginBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(16, 92)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Confirm Password*"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtUserName
        '
        Me.txtUserName.AutoSize = False
        Me.txtUserName.Location = New System.Drawing.Point(117, 28)
        Me.txtUserName.MaxLength = 30
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(250, 24)
        Me.txtUserName.TabIndex = 1
        Me.txtUserName.Tag = "Username"
        '
        'txtPassword
        '
        Me.txtPassword.AutoSize = False
        Me.txtPassword.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(117, 58)
        Me.txtPassword.MaxLength = 30
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtPassword.Size = New System.Drawing.Size(250, 24)
        Me.txtPassword.TabIndex = 3
        Me.txtPassword.Tag = "Password"
        '
        'txtLoginBarcode
        '
        Me.txtLoginBarcode.AutoSize = False
        Me.txtLoginBarcode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoginBarcode.Location = New System.Drawing.Point(117, 118)
        Me.txtLoginBarcode.MaxLength = 30
        Me.txtLoginBarcode.Name = "txtLoginBarcode"
        Me.txtLoginBarcode.Size = New System.Drawing.Size(250, 24)
        Me.txtLoginBarcode.TabIndex = 7
        Me.txtLoginBarcode.Tag = ""
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.AutoSize = False
        Me.txtConfirmPassword.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConfirmPassword.Location = New System.Drawing.Point(117, 88)
        Me.txtConfirmPassword.MaxLength = 30
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtConfirmPassword.Size = New System.Drawing.Size(250, 24)
        Me.txtConfirmPassword.TabIndex = 5
        Me.txtConfirmPassword.Tag = ""
        '
        'grpUserSetting
        '
        Me.grpUserSetting.Controls.Add(Me.lblSecurityRole)
        Me.grpUserSetting.Controls.Add(Me.cbxSecurityRole)
        Me.grpUserSetting.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpUserSetting.Location = New System.Drawing.Point(0, 115)
        Me.grpUserSetting.Name = "grpUserSetting"
        Me.grpUserSetting.Size = New System.Drawing.Size(900, 70)
        Me.grpUserSetting.TabIndex = 2
        Me.grpUserSetting.Text = "User Settings"
        Me.grpUserSetting.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI
        '
        'lblSecurityRole
        '
        Me.lblSecurityRole.AutoSize = True
        Me.lblSecurityRole.Location = New System.Drawing.Point(16, 33)
        Me.lblSecurityRole.Name = "lblSecurityRole"
        Me.lblSecurityRole.Size = New System.Drawing.Size(58, 13)
        Me.lblSecurityRole.TabIndex = 0
        Me.lblSecurityRole.Text = "User Role*"
        Me.lblSecurityRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbxSecurityRole
        '
        Me.cbxSecurityRole.AutoSize = False
        Me.cbxSecurityRole.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        Me.cbxSecurityRole.Location = New System.Drawing.Point(117, 27)
        Me.cbxSecurityRole.Name = "cbxSecurityRole"
        Me.cbxSecurityRole.ShowOverflowIndicator = True
        Me.cbxSecurityRole.Size = New System.Drawing.Size(250, 24)
        Me.cbxSecurityRole.TabIndex = 1
        Me.cbxSecurityRole.Tag = "Security Role"
        '
        'grpUserInformation
        '
        Me.grpUserInformation.Controls.Add(Me.Label3)
        Me.grpUserInformation.Controls.Add(Me.txtName)
        Me.grpUserInformation.Controls.Add(Me.txtCode)
        Me.grpUserInformation.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpUserInformation.Location = New System.Drawing.Point(0, 40)
        Me.grpUserInformation.Name = "grpUserInformation"
        Me.grpUserInformation.Size = New System.Drawing.Size(900, 75)
        Me.grpUserInformation.TabIndex = 1
        Me.grpUserInformation.Text = "User Information"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 34)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Name*"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtName
        '
        Me.txtName.AutoSize = False
        Me.txtName.Location = New System.Drawing.Point(117, 28)
        Me.txtName.MaxLength = 30
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(250, 24)
        Me.txtName.TabIndex = 1
        Me.txtName.Tag = "Name"
        '
        'txtCode
        '
        Me.txtCode.AutoSize = False
        Me.txtCode.Location = New System.Drawing.Point(373, 28)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.ReadOnly = True
        Me.txtCode.Size = New System.Drawing.Size(140, 24)
        Me.txtCode.TabIndex = 2
        Me.txtCode.Visible = False
        '
        'ctrlMenu
        '
        Me.ctrlMenu.AllowDelete = True
        Me.ctrlMenu.AllowEdit = True
        Me.ctrlMenu.AllowNew = True
        Me.ctrlMenu.BackColor = System.Drawing.Color.Transparent
        Me.ctrlMenu.ButtonHeight = 24
        Me.ctrlMenu.Dock = System.Windows.Forms.DockStyle.Top
        Me.ctrlMenu.HasPrint = False
        Me.ctrlMenu.Location = New System.Drawing.Point(0, 0)
        Me.ctrlMenu.MenuState = BusinessPro.Core.MenuButtons.MenuStates.[Default]
        Me.ctrlMenu.ModuleGroup = ""
        Me.ctrlMenu.ModuleName = Nothing
        Me.ctrlMenu.Name = "ctrlMenu"
        Me.ctrlMenu.Size = New System.Drawing.Size(900, 40)
        Me.ctrlMenu.TabIndex = 0
        Me.ctrlMenu.UseCloseButton = True
        '
        'tabSystemUsers
        '
        Me.tabSystemUsers.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.tabSystemUsers.Controls.Add(Me.tabPageUsers)
        Me.tabSystemUsers.Controls.Add(Me.UltraTabPageControl2)
        Me.tabSystemUsers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabSystemUsers.Location = New System.Drawing.Point(0, 0)
        Me.tabSystemUsers.Name = "tabSystemUsers"
        Me.tabSystemUsers.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.tabSystemUsers.Size = New System.Drawing.Size(904, 431)
        Me.tabSystemUsers.TabIndex = 0
        UltraTab1.Key = "list"
        UltraTab1.TabPage = Me.tabPageUsers
        UltraTab1.Text = "User List"
        UltraTab2.Key = "info"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "User Information"
        Me.tabSystemUsers.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(900, 405)
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Enabled = False
        Me.Label2.Location = New System.Drawing.Point(373, 148)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(517, 53)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "• be careful in using the barcode feature, the system will no longer ask for pass" &
    "word when barcode is used."
        '
        'FormUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(904, 431)
        Me.Controls.Add(Me.tabSystemUsers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FormUser"
        Me.Text = "Users"
        Me.tabPageUsers.ResumeLayout(False)
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        CType(Me.grpLoginInformation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpLoginInformation.ResumeLayout(False)
        Me.grpLoginInformation.PerformLayout()
        CType(Me.chkIsNavAccount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkIsActive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLoginBarcode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtConfirmPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grpUserSetting, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpUserSetting.ResumeLayout(False)
        Me.grpUserSetting.PerformLayout()
        CType(Me.cbxSecurityRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grpUserInformation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpUserInformation.ResumeLayout(False)
        Me.grpUserInformation.PerformLayout()
        CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tabSystemUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSystemUsers.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctrlMenu As MenuButtons
    Friend WithEvents cbxSecurityRole As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents txtName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtCode As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents ctrlList As ListSearch
    Friend WithEvents btnView As Infragistics.Win.Misc.UltraButton
    Friend WithEvents tabSystemUsers As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents tabPageUsers As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents grpUserInformation As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grpLoginInformation As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents txtConfirmPassword As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtPassword As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtUserName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grpUserSetting As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblSecurityRole As System.Windows.Forms.Label
    Friend WithEvents lblLoginBarcode As Label
    Friend WithEvents txtLoginBarcode As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblLoginBarcodeHint As Label
    Friend WithEvents chkIsNavAccount As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents chkIsActive As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents Label2 As Label
End Class
