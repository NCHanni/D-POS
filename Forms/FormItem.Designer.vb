<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormItem
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
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim ValueListItem4 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim ValueListItem5 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton()
        Me.chkShowInactive = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.btnViewInfo = New Infragistics.Win.Misc.UltraButton()
        Me.ctrlList = New BusinessPro.Core.ListSearch()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.grpItem = New Infragistics.Win.Misc.UltraGroupBox()
        Me.numPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.lblPriceIncludesVAT = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.cmbDiscountGroup = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.chkScPwd = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.txtSpecifications = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.UltraLabel12 = New Infragistics.Win.Misc.UltraLabel()
        Me.txtCodeName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtDescription = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.cmbUnitOfMeasure = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.chkIsActive = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.chkIsZeroRated = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.chkIsVATable = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.txtSKU = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.ctrlMenu = New BusinessPro.Core.MenuButtons()
        Me.tabItems = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.chkShowInactive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.grpItem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpItem.SuspendLayout()
        CType(Me.numPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbDiscountGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkScPwd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSpecifications, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCodeName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbUnitOfMeasure, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkIsActive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkIsZeroRated, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkIsVATable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSKU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tabItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabItems.SuspendLayout()
        Me.SuspendLayout()
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.btnRefresh)
        Me.UltraTabPageControl1.Controls.Add(Me.chkShowInactive)
        Me.UltraTabPageControl1.Controls.Add(Me.btnViewInfo)
        Me.UltraTabPageControl1.Controls.Add(Me.ctrlList)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(620, 405)
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(408, 12)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 1
        Me.btnRefresh.Text = "Refresh"
        '
        'chkShowInactive
        '
        Me.chkShowInactive.AutoSize = True
        Me.chkShowInactive.Location = New System.Drawing.Point(20, 13)
        Me.chkShowInactive.Name = "chkShowInactive"
        Me.chkShowInactive.Size = New System.Drawing.Size(122, 17)
        Me.chkShowInactive.TabIndex = 0
        Me.chkShowInactive.Text = "Show Inactive Items"
        '
        'btnViewInfo
        '
        Me.btnViewInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewInfo.Location = New System.Drawing.Point(489, 12)
        Me.btnViewInfo.Name = "btnViewInfo"
        Me.btnViewInfo.Size = New System.Drawing.Size(120, 23)
        Me.btnViewInfo.TabIndex = 2
        Me.btnViewInfo.Text = "View Information"
        '
        'ctrlList
        '
        Me.ctrlList.ActiveRow = Nothing
        Me.ctrlList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ctrlList.CloseParentFormOnOK = True
        Me.ctrlList.DataSource = Nothing
        Me.ctrlList.Location = New System.Drawing.Point(3, 41)
        Me.ctrlList.MultiSelect = False
        Me.ctrlList.MultiSelectHeader = False
        Me.ctrlList.Name = "ctrlList"
        Me.ctrlList.Padding = New System.Windows.Forms.Padding(0, 0, 0, 3)
        Me.ctrlList.SearchKeys = Nothing
        Me.ctrlList.ShowButtons = True
        Me.ctrlList.ShowSearch = True
        Me.ctrlList.Size = New System.Drawing.Size(614, 361)
        Me.ctrlList.TabIndex = 3
        Me.ctrlList.UsedInBrowser = False
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.grpItem)
        Me.UltraTabPageControl2.Controls.Add(Me.ctrlMenu)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(620, 405)
        '
        'grpItem
        '
        Me.grpItem.Controls.Add(Me.numPrice)
        Me.grpItem.Controls.Add(Me.lblPriceIncludesVAT)
        Me.grpItem.Controls.Add(Me.UltraLabel4)
        Me.grpItem.Controls.Add(Me.UltraLabel13)
        Me.grpItem.Controls.Add(Me.cmbDiscountGroup)
        Me.grpItem.Controls.Add(Me.chkScPwd)
        Me.grpItem.Controls.Add(Me.txtSpecifications)
        Me.grpItem.Controls.Add(Me.UltraLabel12)
        Me.grpItem.Controls.Add(Me.txtCodeName)
        Me.grpItem.Controls.Add(Me.txtDescription)
        Me.grpItem.Controls.Add(Me.cmbUnitOfMeasure)
        Me.grpItem.Controls.Add(Me.chkIsActive)
        Me.grpItem.Controls.Add(Me.chkIsZeroRated)
        Me.grpItem.Controls.Add(Me.chkIsVATable)
        Me.grpItem.Controls.Add(Me.txtSKU)
        Me.grpItem.Controls.Add(Me.UltraLabel7)
        Me.grpItem.Controls.Add(Me.UltraLabel6)
        Me.grpItem.Controls.Add(Me.UltraLabel1)
        Me.grpItem.Controls.Add(Me.UltraLabel3)
        Me.grpItem.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpItem.Location = New System.Drawing.Point(0, 36)
        Me.grpItem.Name = "grpItem"
        Me.grpItem.Size = New System.Drawing.Size(620, 369)
        Me.grpItem.TabIndex = 1
        '
        'numPrice
        '
        Me.numPrice.Location = New System.Drawing.Point(154, 205)
        Me.numPrice.MinValue = 0R
        Me.numPrice.Name = "numPrice"
        Me.numPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numPrice.Size = New System.Drawing.Size(120, 21)
        Me.numPrice.TabIndex = 11
        '
        'lblPriceIncludesVAT
        '
        Appearance1.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblPriceIncludesVAT.Appearance = Appearance1
        Me.lblPriceIncludesVAT.AutoSize = True
        Me.lblPriceIncludesVAT.Enabled = False
        Me.lblPriceIncludesVAT.Location = New System.Drawing.Point(280, 209)
        Me.lblPriceIncludesVAT.Name = "lblPriceIncludesVAT"
        Me.lblPriceIncludesVAT.Size = New System.Drawing.Size(107, 14)
        Me.lblPriceIncludesVAT.TabIndex = 12
        Me.lblPriceIncludesVAT.Text = "(Price includes VAT)"
        Me.lblPriceIncludesVAT.Visible = False
        '
        'UltraLabel4
        '
        Appearance2.TextHAlignAsString = "Left"
        Appearance2.TextVAlignAsString = "Middle"
        Me.UltraLabel4.Appearance = Appearance2
        Me.UltraLabel4.AutoSize = True
        Me.UltraLabel4.Location = New System.Drawing.Point(26, 209)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(30, 14)
        Me.UltraLabel4.TabIndex = 10
        Me.UltraLabel4.Text = "Price"
        '
        'UltraLabel13
        '
        Appearance3.TextHAlignAsString = "Left"
        Appearance3.TextVAlignAsString = "Middle"
        Me.UltraLabel13.Appearance = Appearance3
        Me.UltraLabel13.AutoSize = True
        Me.UltraLabel13.Location = New System.Drawing.Point(26, 305)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(83, 14)
        Me.UltraLabel13.TabIndex = 16
        Me.UltraLabel13.Text = "Discount Group"
        '
        'cmbDiscountGroup
        '
        Me.cmbDiscountGroup.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        Me.cmbDiscountGroup.Enabled = False
        ValueListItem4.DataValue = "WAVG"
        ValueListItem4.DisplayText = "Weighted Average"
        ValueListItem5.DataValue = "FIFO"
        ValueListItem5.DisplayText = "First In, First Out"
        Me.cmbDiscountGroup.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem4, ValueListItem5})
        Me.cmbDiscountGroup.Location = New System.Drawing.Point(154, 301)
        Me.cmbDiscountGroup.Name = "cmbDiscountGroup"
        Me.cmbDiscountGroup.Size = New System.Drawing.Size(250, 21)
        Me.cmbDiscountGroup.TabIndex = 17
        Me.cmbDiscountGroup.Tag = ""
        '
        'chkScPwd
        '
        Me.chkScPwd.AutoSize = True
        Me.chkScPwd.Location = New System.Drawing.Point(154, 278)
        Me.chkScPwd.Name = "chkScPwd"
        Me.chkScPwd.Size = New System.Drawing.Size(114, 17)
        Me.chkScPwd.TabIndex = 15
        Me.chkScPwd.Text = "SC/PWD Discount"
        '
        'txtSpecifications
        '
        Me.txtSpecifications.Location = New System.Drawing.Point(154, 122)
        Me.txtSpecifications.MaxLength = 500
        Me.txtSpecifications.Multiline = True
        Me.txtSpecifications.Name = "txtSpecifications"
        Me.txtSpecifications.Size = New System.Drawing.Size(250, 50)
        Me.txtSpecifications.TabIndex = 7
        Me.txtSpecifications.Tag = ""
        '
        'UltraLabel12
        '
        Appearance4.TextHAlignAsString = "Left"
        Appearance4.TextVAlignAsString = "Middle"
        Me.UltraLabel12.Appearance = Appearance4
        Me.UltraLabel12.AutoSize = True
        Me.UltraLabel12.Location = New System.Drawing.Point(26, 125)
        Me.UltraLabel12.Name = "UltraLabel12"
        Me.UltraLabel12.Size = New System.Drawing.Size(74, 14)
        Me.UltraLabel12.TabIndex = 6
        Me.UltraLabel12.Text = "Specifications"
        '
        'txtCodeName
        '
        Me.txtCodeName.Location = New System.Drawing.Point(154, 22)
        Me.txtCodeName.MaxLength = 30
        Me.txtCodeName.Name = "txtCodeName"
        Me.txtCodeName.Size = New System.Drawing.Size(120, 21)
        Me.txtCodeName.TabIndex = 1
        Me.txtCodeName.Tag = "Code"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(154, 76)
        Me.txtDescription.MaxLength = 250
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(250, 40)
        Me.txtDescription.TabIndex = 5
        Me.txtDescription.Tag = "Description"
        '
        'cmbUnitOfMeasure
        '
        Me.cmbUnitOfMeasure.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        Me.cmbUnitOfMeasure.Location = New System.Drawing.Point(154, 178)
        Me.cmbUnitOfMeasure.Name = "cmbUnitOfMeasure"
        Me.cmbUnitOfMeasure.Size = New System.Drawing.Size(250, 21)
        Me.cmbUnitOfMeasure.TabIndex = 9
        Me.cmbUnitOfMeasure.Tag = "Unit of Measure"
        '
        'chkIsActive
        '
        Me.chkIsActive.AutoSize = True
        Me.chkIsActive.Location = New System.Drawing.Point(154, 328)
        Me.chkIsActive.Name = "chkIsActive"
        Me.chkIsActive.Size = New System.Drawing.Size(52, 17)
        Me.chkIsActive.TabIndex = 18
        Me.chkIsActive.Text = "Active"
        '
        'chkIsZeroRated
        '
        Me.chkIsZeroRated.AutoSize = True
        Me.chkIsZeroRated.Location = New System.Drawing.Point(154, 255)
        Me.chkIsZeroRated.Name = "chkIsZeroRated"
        Me.chkIsZeroRated.Size = New System.Drawing.Size(78, 17)
        Me.chkIsZeroRated.TabIndex = 14
        Me.chkIsZeroRated.Text = "Zero-Rated"
        '
        'chkIsVATable
        '
        Me.chkIsVATable.AutoSize = True
        Me.chkIsVATable.Location = New System.Drawing.Point(154, 232)
        Me.chkIsVATable.Name = "chkIsVATable"
        Me.chkIsVATable.Size = New System.Drawing.Size(65, 17)
        Me.chkIsVATable.TabIndex = 13
        Me.chkIsVATable.Text = "VATable"
        '
        'txtSKU
        '
        Me.txtSKU.Location = New System.Drawing.Point(154, 49)
        Me.txtSKU.MaxLength = 30
        Me.txtSKU.Name = "txtSKU"
        Me.txtSKU.Size = New System.Drawing.Size(120, 21)
        Me.txtSKU.TabIndex = 3
        Me.txtSKU.Tag = ""
        '
        'UltraLabel7
        '
        Appearance5.TextHAlignAsString = "Left"
        Appearance5.TextVAlignAsString = "Middle"
        Me.UltraLabel7.Appearance = Appearance5
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Location = New System.Drawing.Point(26, 53)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(27, 14)
        Me.UltraLabel7.TabIndex = 2
        Me.UltraLabel7.Text = "SKU"
        '
        'UltraLabel6
        '
        Appearance6.TextHAlignAsString = "Left"
        Appearance6.TextVAlignAsString = "Middle"
        Me.UltraLabel6.Appearance = Appearance6
        Me.UltraLabel6.AutoSize = True
        Me.UltraLabel6.Location = New System.Drawing.Point(26, 79)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(65, 14)
        Me.UltraLabel6.TabIndex = 4
        Me.UltraLabel6.Text = "Description*"
        '
        'UltraLabel1
        '
        Appearance7.TextHAlignAsString = "Left"
        Appearance7.TextVAlignAsString = "Middle"
        Me.UltraLabel1.Appearance = Appearance7
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(26, 26)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(36, 14)
        Me.UltraLabel1.TabIndex = 0
        Me.UltraLabel1.Text = "Code*"
        '
        'UltraLabel3
        '
        Appearance8.TextHAlignAsString = "Left"
        Appearance8.TextVAlignAsString = "Middle"
        Me.UltraLabel3.Appearance = Appearance8
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(26, 182)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(88, 14)
        Me.UltraLabel3.TabIndex = 8
        Me.UltraLabel3.Text = "Unit of Measure*"
        '
        'ctrlMenu
        '
        Me.ctrlMenu.AllowDelete = True
        Me.ctrlMenu.AllowEdit = True
        Me.ctrlMenu.AllowNew = True
        Me.ctrlMenu.ButtonHeight = 24
        Me.ctrlMenu.Dock = System.Windows.Forms.DockStyle.Top
        Me.ctrlMenu.HasPrint = False
        Me.ctrlMenu.Location = New System.Drawing.Point(0, 0)
        Me.ctrlMenu.MenuState = BusinessPro.Core.MenuButtons.MenuStates.[Default]
        Me.ctrlMenu.ModuleGroup = ""
        Me.ctrlMenu.ModuleName = ""
        Me.ctrlMenu.Name = "ctrlMenu"
        Me.ctrlMenu.Size = New System.Drawing.Size(620, 36)
        Me.ctrlMenu.TabIndex = 0
        Me.ctrlMenu.UseCloseButton = True
        '
        'tabItems
        '
        Me.tabItems.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.tabItems.Controls.Add(Me.UltraTabPageControl1)
        Me.tabItems.Controls.Add(Me.UltraTabPageControl2)
        Me.tabItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabItems.Location = New System.Drawing.Point(0, 0)
        Me.tabItems.Name = "tabItems"
        Me.tabItems.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.tabItems.Size = New System.Drawing.Size(624, 431)
        Me.tabItems.TabIndex = 0
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Item List"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Item Information"
        Me.tabItems.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(620, 405)
        '
        'FormItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 431)
        Me.Controls.Add(Me.tabItems)
        Me.Name = "FormItem"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Items"
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.chkShowInactive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl2.ResumeLayout(False)
        CType(Me.grpItem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpItem.ResumeLayout(False)
        Me.grpItem.PerformLayout()
        CType(Me.numPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbDiscountGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkScPwd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSpecifications, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCodeName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbUnitOfMeasure, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkIsActive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkIsZeroRated, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkIsVATable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSKU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tabItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabItems.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabItems As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents grpItem As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents ctrlMenu As BusinessPro.Core.MenuButtons
    Friend WithEvents txtSKU As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtDescription As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtCodeName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents chkIsActive As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents chkIsVATable As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents ctrlList As BusinessPro.Core.ListSearch
    Friend WithEvents btnViewInfo As Infragistics.Win.Misc.UltraButton
    Friend WithEvents chkShowInactive As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents cmbUnitOfMeasure As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents txtSpecifications As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents UltraLabel12 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents chkScPwd As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents cmbDiscountGroup As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents numPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblPriceIncludesVAT As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents chkIsZeroRated As Infragistics.Win.UltraWinEditors.UltraCheckEditor
End Class
