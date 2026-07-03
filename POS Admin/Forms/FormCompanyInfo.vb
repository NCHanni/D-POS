Friend Class FormCompanyInfo

#Region "Declarations"

    Private Const MODULE_GROUP As String = "Management"
    Private Const MODULE_COMPANY As String = "Company Info"
    Private Const MODULE_ENTITY As String = "Entity Details"

    Private m_CompanyInfo As CompanyInfo
    Private m_CheckFormChanges As CheckFormChanges

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_CheckFormChanges IsNot Nothing AndAlso m_CheckFormChanges.HasChanges Then
                If ShowQuestion("Unsaved changes have been made and will be lost.\n\nDo you want to continue?", "Confirm Close") <> Windows.Forms.DialogResult.Yes Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            FillDetails()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If IsValid() Then
                Dim data As New Structures.CompanyInfo
                With data
                    .Name = txtCompanyName.Text
                    .Name2 = txtCompanyName2.Text
                    .PermitNo = txtPermitNumber.Text
                    .PermitMIN = txtPermitMIN.Text
                    .PermitIssued = calDateIssued.Value
                    .PermitExpiry = If(IsDBNull(calValidUntil.Value), Nothing, calValidUntil.Value)
                    .VatRegistrationNo = txtVATRegNo.Text
                    .BusinessStyle = txtBusinessStyle.Text
                    .Address = txtAddress.Text
                    .ContactNo = txtContactNo.Text
                    .FaxNo = txtFaxNo.Text
                    .EmailAddress = txtEmailAddress.Text
                    .WebsiteUrl = txtWebsiteUrl.Text
                End With

                With m_CompanyInfo
                    .Data = data
                    If .Save Then
                        ShowMessage("Changes to record has been successfully applied.", "Save Successful")

                        m_CheckFormChanges.ClearChanges()
                    End If
                End With
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            Dim backColor As Color = Color.FromKnownColor(KnownColor.Info)

            m_CompanyInfo = New CompanyInfo
            m_CheckFormChanges = New CheckFormChanges

            txtCompanyName.Appearance.BackColorDisabled = backColor
            txtCompanyName2.Appearance.BackColorDisabled = backColor
            txtPermitNumber.Appearance.BackColorDisabled = backColor
            txtPermitMIN.Appearance.BackColorDisabled = backColor
            calDateIssued.Appearance.BackColorDisabled = backColor
            calValidUntil.Appearance.BackColorDisabled = backColor
            txtVATRegNo.Appearance.BackColorDisabled = backColor
            txtBusinessStyle.Appearance.BackColorDisabled = backColor
            txtAddress.Appearance.BackColorDisabled = backColor

            If Current.User.IsSuperRole Then
                m_CheckFormChanges.Add(grpCompany)
            Else
                grpCompany.Enabled = False
            End If

            If Not Current.User.IsSuperRole Then
                txtVATRegNo.Enabled = False
                txtBusinessStyle.Enabled = False
                txtAddress.Enabled = False
            End If

            btnSave.Enabled = grpCompany.Enabled
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillDetails()
        Try
            With m_CompanyInfo
                If .Fill() Then
                    txtCompanyName.Text = .Data.Name
                    txtCompanyName2.Text = .Data.Name2
                    txtPermitNumber.Text = .Data.PermitNo
                    txtPermitMIN.Text = .Data.PermitMIN
                    calDateIssued.Value = .Data.PermitIssued
                    calValidUntil.Value = .Data.PermitExpiry
                    txtVATRegNo.Text = .Data.VatRegistrationNo
                    txtBusinessStyle.Text = .Data.BusinessStyle
                    txtAddress.Text = .Data.Address
                    txtContactNo.Text = .Data.ContactNo
                    txtFaxNo.Text = .Data.FaxNo
                    txtEmailAddress.Text = .Data.EmailAddress
                    txtWebsiteUrl.Text = .Data.WebsiteUrl
                End If
                m_CheckFormChanges.ClearChanges()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            Dim msg As String = ""

            txtCompanyName.Text = txtCompanyName.Text.Trim
            txtPermitNumber.Text = txtPermitNumber.Text.Trim
            txtVATRegNo.Text = txtVATRegNo.Text.Trim
            txtBusinessStyle.Text = txtBusinessStyle.Text.Trim

            If txtCompanyName.TextLength = 0 Then
                msg = "Company Name is required!"
                txtCompanyName.Focus()
                GoTo DisplayMessage
            End If

            If txtPermitNumber.TextLength = 0 AndAlso txtPermitNumber.Enabled Then
                msg = "Permit (PTU) No. is required!"
                txtPermitNumber.Focus()
                GoTo DisplayMessage
            End If

            If txtPermitMIN.TextLength = 0 AndAlso txtPermitMIN.Enabled Then
                msg = "Machine ID No. (MIN) is required!"
                txtPermitMIN.Focus()
                GoTo DisplayMessage
            End If

            If IsDBNull(calDateIssued.Value) AndAlso calDateIssued.Enabled Then
                msg = "Date Issued (PTU) is required!"
                calDateIssued.Focus()
                GoTo DisplayMessage
            End If

            If txtVATRegNo.TextLength = 0 AndAlso txtVATRegNo.Enabled Then
                msg = "VAT Reg. No. is required!"
                txtVATRegNo.Focus()
                GoTo DisplayMessage
            End If

            If txtBusinessStyle.TextLength = 0 AndAlso txtBusinessStyle.Enabled Then
                msg = "Business Style is required!"
                txtBusinessStyle.Focus()
                GoTo DisplayMessage
            End If

            If txtAddress.TextLength = 0 AndAlso txtAddress.Enabled Then
                msg = "Address is required!"
                txtAddress.Focus()
                GoTo DisplayMessage
            End If

DisplayMessage:
            If msg <> "" Then
                ShowWarning(msg, "Save Failed")
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class