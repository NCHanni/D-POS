<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCompanyInfo
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
        Me.txtBusinessStyle = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAddress = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblCompanyAddress = New System.Windows.Forms.Label()
        Me.txtContactNo = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtVATRegNo = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.pnlBottom = New Infragistics.Win.Misc.UltraGroupBox()
        Me.btnClose = New Infragistics.Win.Misc.UltraButton()
        Me.btnSave = New Infragistics.Win.Misc.UltraButton()
        Me.grpCompany = New Infragistics.Win.Misc.UltraGroupBox()
        Me.calValidUntil = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo()
        Me.calDateIssued = New Infragistics.Win.UltraWinSchedule.UltraCalendarCombo()
        Me.txtWebsiteUrl = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtEmailAddress = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtFaxNo = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtCompanyName2 = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCompanyName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblCompanyName = New System.Windows.Forms.Label()
        Me.txtPermitMIN = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtPermitNumber = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        CType(Me.txtBusinessStyle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAddress, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtContactNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtVATRegNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlBottom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBottom.SuspendLayout()
        CType(Me.grpCompany, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCompany.SuspendLayout()
        CType(Me.calValidUntil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.calDateIssued, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtWebsiteUrl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtEmailAddress, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFaxNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCompanyName2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCompanyName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPermitMIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPermitNumber, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtBusinessStyle
        '
        Me.txtBusinessStyle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusinessStyle.AutoSize = False
        Me.txtBusinessStyle.Location = New System.Drawing.Point(119, 215)
        Me.txtBusinessStyle.MaxLength = 50
        Me.txtBusinessStyle.Name = "txtBusinessStyle"
        Me.txtBusinessStyle.Size = New System.Drawing.Size(243, 21)
        Me.txtBusinessStyle.TabIndex = 15
        Me.txtBusinessStyle.Tag = "Business Style"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 287)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 13)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Contact No."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 219)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Business Style*"
        '
        'txtAddress
        '
        Me.txtAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddress.AutoSize = False
        Me.txtAddress.Location = New System.Drawing.Point(119, 242)
        Me.txtAddress.MaxLength = 255
        Me.txtAddress.Multiline = True
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(243, 35)
        Me.txtAddress.TabIndex = 17
        Me.txtAddress.Tag = "Address"
        '
        'lblCompanyAddress
        '
        Me.lblCompanyAddress.AutoSize = True
        Me.lblCompanyAddress.Location = New System.Drawing.Point(18, 245)
        Me.lblCompanyAddress.Name = "lblCompanyAddress"
        Me.lblCompanyAddress.Size = New System.Drawing.Size(49, 13)
        Me.lblCompanyAddress.TabIndex = 16
        Me.lblCompanyAddress.Text = "Address*"
        '
        'txtContactNo
        '
        Me.txtContactNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtContactNo.AutoSize = False
        Me.txtContactNo.Location = New System.Drawing.Point(119, 283)
        Me.txtContactNo.MaxLength = 50
        Me.txtContactNo.Name = "txtContactNo"
        Me.txtContactNo.Size = New System.Drawing.Size(243, 21)
        Me.txtContactNo.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 192)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "VAT Reg. No.*"
        '
        'txtVATRegNo
        '
        Me.txtVATRegNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVATRegNo.AutoSize = False
        Me.txtVATRegNo.Location = New System.Drawing.Point(119, 188)
        Me.txtVATRegNo.MaxLength = 50
        Me.txtVATRegNo.Name = "txtVATRegNo"
        Me.txtVATRegNo.Size = New System.Drawing.Size(243, 21)
        Me.txtVATRegNo.TabIndex = 13
        Me.txtVATRegNo.Tag = "VAT Reg. No."
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.btnClose)
        Me.pnlBottom.Controls.Add(Me.btnSave)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 409)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(384, 40)
        Me.pnlBottom.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(297, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(216, 8)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        '
        'grpCompany
        '
        Me.grpCompany.Controls.Add(Me.txtAddress)
        Me.grpCompany.Controls.Add(Me.calValidUntil)
        Me.grpCompany.Controls.Add(Me.lblCompanyAddress)
        Me.grpCompany.Controls.Add(Me.calDateIssued)
        Me.grpCompany.Controls.Add(Me.txtWebsiteUrl)
        Me.grpCompany.Controls.Add(Me.txtEmailAddress)
        Me.grpCompany.Controls.Add(Me.txtFaxNo)
        Me.grpCompany.Controls.Add(Me.txtContactNo)
        Me.grpCompany.Controls.Add(Me.txtCompanyName2)
        Me.grpCompany.Controls.Add(Me.Label2)
        Me.grpCompany.Controls.Add(Me.Label5)
        Me.grpCompany.Controls.Add(Me.Label10)
        Me.grpCompany.Controls.Add(Me.txtVATRegNo)
        Me.grpCompany.Controls.Add(Me.Label9)
        Me.grpCompany.Controls.Add(Me.Label4)
        Me.grpCompany.Controls.Add(Me.Label7)
        Me.grpCompany.Controls.Add(Me.txtCompanyName)
        Me.grpCompany.Controls.Add(Me.Label6)
        Me.grpCompany.Controls.Add(Me.lblCompanyName)
        Me.grpCompany.Controls.Add(Me.txtBusinessStyle)
        Me.grpCompany.Controls.Add(Me.txtPermitMIN)
        Me.grpCompany.Controls.Add(Me.txtPermitNumber)
        Me.grpCompany.Controls.Add(Me.Label3)
        Me.grpCompany.Controls.Add(Me.Label11)
        Me.grpCompany.Controls.Add(Me.Label1)
        Me.grpCompany.Controls.Add(Me.Label8)
        Me.grpCompany.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCompany.Location = New System.Drawing.Point(0, 0)
        Me.grpCompany.Name = "grpCompany"
        Me.grpCompany.Size = New System.Drawing.Size(384, 409)
        Me.grpCompany.TabIndex = 0
        '
        'calValidUntil
        '
        Me.calValidUntil.DateButtons.Add(DateButton1)
        Me.calValidUntil.Location = New System.Drawing.Point(119, 161)
        Me.calValidUntil.Name = "calValidUntil"
        Me.calValidUntil.NonAutoSizeHeight = 21
        Me.calValidUntil.Size = New System.Drawing.Size(100, 21)
        Me.calValidUntil.TabIndex = 11
        Me.calValidUntil.Tag = "PTU Valid Until"
        Me.calValidUntil.Value = New Date(2021, 9, 16, 0, 0, 0, 0)
        '
        'calDateIssued
        '
        Me.calDateIssued.DateButtons.Add(DateButton2)
        Me.calDateIssued.Location = New System.Drawing.Point(119, 134)
        Me.calDateIssued.Name = "calDateIssued"
        Me.calDateIssued.NonAutoSizeHeight = 21
        Me.calDateIssued.Size = New System.Drawing.Size(100, 21)
        Me.calDateIssued.TabIndex = 9
        Me.calDateIssued.Tag = "PTU Date Issued"
        Me.calDateIssued.Value = New Date(2021, 9, 16, 0, 0, 0, 0)
        '
        'txtWebsiteUrl
        '
        Me.txtWebsiteUrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWebsiteUrl.AutoSize = False
        Me.txtWebsiteUrl.Location = New System.Drawing.Point(119, 364)
        Me.txtWebsiteUrl.MaxLength = 50
        Me.txtWebsiteUrl.Name = "txtWebsiteUrl"
        Me.txtWebsiteUrl.Size = New System.Drawing.Size(243, 21)
        Me.txtWebsiteUrl.TabIndex = 25
        '
        'txtEmailAddress
        '
        Me.txtEmailAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmailAddress.AutoSize = False
        Me.txtEmailAddress.Location = New System.Drawing.Point(119, 337)
        Me.txtEmailAddress.MaxLength = 50
        Me.txtEmailAddress.Name = "txtEmailAddress"
        Me.txtEmailAddress.Size = New System.Drawing.Size(243, 21)
        Me.txtEmailAddress.TabIndex = 23
        '
        'txtFaxNo
        '
        Me.txtFaxNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFaxNo.AutoSize = False
        Me.txtFaxNo.Location = New System.Drawing.Point(119, 310)
        Me.txtFaxNo.MaxLength = 50
        Me.txtFaxNo.Name = "txtFaxNo"
        Me.txtFaxNo.Size = New System.Drawing.Size(243, 21)
        Me.txtFaxNo.TabIndex = 21
        '
        'txtCompanyName2
        '
        Me.txtCompanyName2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCompanyName2.AutoSize = False
        Me.txtCompanyName2.Location = New System.Drawing.Point(119, 53)
        Me.txtCompanyName2.MaxLength = 100
        Me.txtCompanyName2.Name = "txtCompanyName2"
        Me.txtCompanyName2.Size = New System.Drawing.Size(243, 21)
        Me.txtCompanyName2.TabIndex = 3
        Me.txtCompanyName2.Tag = ""
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 57)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Name (2)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(18, 368)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 13)
        Me.Label10.TabIndex = 24
        Me.Label10.Text = "Website URL"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(18, 341)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Email Address"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 314)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(44, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Fax No."
        '
        'txtCompanyName
        '
        Me.txtCompanyName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCompanyName.AutoSize = False
        Me.txtCompanyName.Location = New System.Drawing.Point(119, 26)
        Me.txtCompanyName.MaxLength = 100
        Me.txtCompanyName.Name = "txtCompanyName"
        Me.txtCompanyName.Size = New System.Drawing.Size(243, 21)
        Me.txtCompanyName.TabIndex = 1
        Me.txtCompanyName.Tag = "Company Name"
        '
        'lblCompanyName
        '
        Me.lblCompanyName.AutoSize = True
        Me.lblCompanyName.Location = New System.Drawing.Point(18, 30)
        Me.lblCompanyName.Name = "lblCompanyName"
        Me.lblCompanyName.Size = New System.Drawing.Size(39, 13)
        Me.lblCompanyName.TabIndex = 0
        Me.lblCompanyName.Text = "Name*"
        '
        'txtPermitMIN
        '
        Me.txtPermitMIN.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPermitMIN.AutoSize = False
        Me.txtPermitMIN.Location = New System.Drawing.Point(119, 107)
        Me.txtPermitMIN.MaxLength = 50
        Me.txtPermitMIN.Name = "txtPermitMIN"
        Me.txtPermitMIN.Size = New System.Drawing.Size(243, 21)
        Me.txtPermitMIN.TabIndex = 7
        Me.txtPermitMIN.Tag = "PTU No."
        '
        'txtPermitNumber
        '
        Me.txtPermitNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPermitNumber.AutoSize = False
        Me.txtPermitNumber.Location = New System.Drawing.Point(119, 80)
        Me.txtPermitNumber.MaxLength = 50
        Me.txtPermitNumber.Name = "txtPermitNumber"
        Me.txtPermitNumber.Size = New System.Drawing.Size(243, 21)
        Me.txtPermitNumber.TabIndex = 5
        Me.txtPermitNumber.Tag = "PTU No."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 164)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Valid Until"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(18, 111)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Machine ID (MIN)*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 137)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Date Issued*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 84)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(91, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Permit (PTU) No.*"
        '
        'FormCompanyInfo
        '
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(384, 449)
        Me.Controls.Add(Me.grpCompany)
        Me.Controls.Add(Me.pnlBottom)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormCompanyInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Company Information"
        CType(Me.txtBusinessStyle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAddress, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtContactNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtVATRegNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlBottom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBottom.ResumeLayout(False)
        CType(Me.grpCompany, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCompany.ResumeLayout(False)
        Me.grpCompany.PerformLayout()
        CType(Me.calValidUntil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.calDateIssued, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtWebsiteUrl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtEmailAddress, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFaxNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCompanyName2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCompanyName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPermitMIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPermitNumber, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtBusinessStyle As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtContactNo As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents pnlBottom As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnClose As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnSave As Infragistics.Win.Misc.UltraButton
    Friend WithEvents Label2 As Label
    Friend WithEvents txtVATRegNo As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents grpCompany As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents txtCompanyName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblCompanyName As Label
    Friend WithEvents txtAddress As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblCompanyAddress As Label
    Friend WithEvents txtPermitNumber As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents Label8 As Label
    Friend WithEvents calValidUntil As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents calDateIssued As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCompanyName2 As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPermitMIN As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents Label11 As Label
    Friend WithEvents txtWebsiteUrl As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtEmailAddress As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtFaxNo As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label7 As Label
End Class
