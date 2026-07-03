<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSeniorDiscountDialog
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
        Dim ValueListItem3 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim ValueListItem4 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim ValueListItem1 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim ValueListItem2 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Me.btnOk = New Infragistics.Win.Misc.UltraButton()
        Me.btnRemove = New Infragistics.Win.Misc.UltraButton()
        Me.btnCancel = New Infragistics.Win.Misc.UltraButton()
        Me.grpDetails = New Infragistics.Win.Misc.UltraGroupBox()
        Me.dtpDateIssued = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo()
        Me.dtpBirthdate = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo()
        Me.txtIDNo = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtCustomerName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.cmbGender = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.cmbDiscountType = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.grpDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetails.SuspendLayout()
        CType(Me.dtpDateIssued, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpBirthdate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtIDNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCustomerName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbGender, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbDiscountType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(136, 206)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "OK"
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.Location = New System.Drawing.Point(12, 206)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 1
        Me.btnRemove.Text = "Remove"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(217, 206)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'grpDetails
        '
        Me.grpDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDetails.Controls.Add(Me.dtpDateIssued)
        Me.grpDetails.Controls.Add(Me.dtpBirthdate)
        Me.grpDetails.Controls.Add(Me.txtIDNo)
        Me.grpDetails.Controls.Add(Me.txtCustomerName)
        Me.grpDetails.Controls.Add(Me.cmbGender)
        Me.grpDetails.Controls.Add(Me.cmbDiscountType)
        Me.grpDetails.Controls.Add(Me.UltraLabel6)
        Me.grpDetails.Controls.Add(Me.UltraLabel5)
        Me.grpDetails.Controls.Add(Me.UltraLabel4)
        Me.grpDetails.Controls.Add(Me.UltraLabel3)
        Me.grpDetails.Controls.Add(Me.UltraLabel2)
        Me.grpDetails.Controls.Add(Me.UltraLabel1)
        Me.grpDetails.Location = New System.Drawing.Point(12, 12)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(280, 188)
        Me.grpDetails.TabIndex = 0
        '
        'dtpDateIssued
        '
        Me.dtpDateIssued.DateButtons.Add(DateButton1)
        Me.dtpDateIssued.Location = New System.Drawing.Point(164, 147)
        Me.dtpDateIssued.Name = "dtpDateIssued"
        Me.dtpDateIssued.NonAutoSizeHeight = 21
        Me.dtpDateIssued.Size = New System.Drawing.Size(100, 21)
        Me.dtpDateIssued.TabIndex = 11
        Me.dtpDateIssued.Tag = "Date Issued"
        '
        'dtpBirthdate
        '
        Me.dtpBirthdate.DateButtons.Add(DateButton2)
        Me.dtpBirthdate.Location = New System.Drawing.Point(164, 120)
        Me.dtpBirthdate.Name = "dtpBirthdate"
        Me.dtpBirthdate.NonAutoSizeHeight = 21
        Me.dtpBirthdate.Size = New System.Drawing.Size(100, 21)
        Me.dtpBirthdate.TabIndex = 9
        Me.dtpBirthdate.Tag = "Birthdate"
        '
        'txtIDNo
        '
        Me.txtIDNo.Location = New System.Drawing.Point(104, 66)
        Me.txtIDNo.MaxLength = 50
        Me.txtIDNo.Name = "txtIDNo"
        Me.txtIDNo.Size = New System.Drawing.Size(160, 21)
        Me.txtIDNo.TabIndex = 5
        Me.txtIDNo.Tag = "ID No."
        '
        'txtCustomerName
        '
        Me.txtCustomerName.Location = New System.Drawing.Point(104, 39)
        Me.txtCustomerName.MaxLength = 50
        Me.txtCustomerName.Name = "txtCustomerName"
        Me.txtCustomerName.Size = New System.Drawing.Size(160, 21)
        Me.txtCustomerName.TabIndex = 3
        Me.txtCustomerName.Tag = "Customer Name"
        '
        'cmbGender
        '
        Me.cmbGender.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        ValueListItem3.DataValue = "Male"
        ValueListItem4.DataValue = "Female"
        Me.cmbGender.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem3, ValueListItem4})
        Me.cmbGender.Location = New System.Drawing.Point(164, 93)
        Me.cmbGender.Name = "cmbGender"
        Me.cmbGender.Size = New System.Drawing.Size(100, 21)
        Me.cmbGender.TabIndex = 7
        Me.cmbGender.Tag = "Gender"
        '
        'cmbDiscountType
        '
        Me.cmbDiscountType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        ValueListItem1.DataValue = "SC"
        ValueListItem2.DataValue = "PWD"
        Me.cmbDiscountType.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem1, ValueListItem2})
        Me.cmbDiscountType.Location = New System.Drawing.Point(164, 12)
        Me.cmbDiscountType.Name = "cmbDiscountType"
        Me.cmbDiscountType.Size = New System.Drawing.Size(100, 21)
        Me.cmbDiscountType.TabIndex = 1
        Me.cmbDiscountType.Tag = "Discount Type"
        '
        'UltraLabel6
        '
        Me.UltraLabel6.AutoSize = True
        Me.UltraLabel6.Location = New System.Drawing.Point(9, 150)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(64, 14)
        Me.UltraLabel6.TabIndex = 10
        Me.UltraLabel6.Text = "Date Issued"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.AutoSize = True
        Me.UltraLabel5.Location = New System.Drawing.Point(9, 123)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(50, 14)
        Me.UltraLabel5.TabIndex = 8
        Me.UltraLabel5.Text = "Birthdate"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.AutoSize = True
        Me.UltraLabel4.Location = New System.Drawing.Point(9, 97)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(42, 14)
        Me.UltraLabel4.TabIndex = 6
        Me.UltraLabel4.Text = "Gender"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(9, 70)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(36, 14)
        Me.UltraLabel3.TabIndex = 4
        Me.UltraLabel3.Text = "ID No."
        '
        'UltraLabel2
        '
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Location = New System.Drawing.Point(9, 43)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(86, 14)
        Me.UltraLabel2.TabIndex = 2
        Me.UltraLabel2.Text = "Customer Name"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(9, 16)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(77, 14)
        Me.UltraLabel1.TabIndex = 0
        Me.UltraLabel1.Text = "Discount Type"
        '
        'FormSeniorDiscountDialog
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(304, 241)
        Me.Controls.Add(Me.grpDetails)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormSeniorDiscountDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Senior Citizen / PWD Information"
        CType(Me.grpDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        CType(Me.dtpDateIssued, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpBirthdate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtIDNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCustomerName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbGender, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbDiscountType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOk As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnRemove As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents grpDetails As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents dtpDateIssued As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents dtpBirthdate As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents txtIDNo As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtCustomerName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents cmbGender As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents cmbDiscountType As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
End Class
