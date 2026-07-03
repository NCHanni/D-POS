Public Class CompanyInfo
    Inherits DataSource

#Region "Properties"

    Property Data As Structures.CompanyInfo

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Dim sql As String =
            "EXEC dbo.GetCompanyInfo"
        Return sql
    End Function

    Private Function SaveSQL() As String
        Dim sql As String =
            "EXEC dbo.SaveCompanyInfo" & vbCrLf &
            "  " & Data.Name.Quote &
            "  " & Data.Name2.Quote &
            "  " & Data.PermitNo.Quote &
            "  " & Data.PermitMIN.Quote &
            "  " & Data.PermitIssued.Quote &
            "  " & Data.PermitExpiry.Quote &
            "  " & Data.VatRegistrationNo.Quote &
            "  " & Data.BusinessStyle.Quote &
            "  " & Data.Address.Quote &
            "  " & Data.ContactNo.Quote &
            "  " & Data.FaxNo.Quote &
            "  " & Data.EmailAddress.Quote &
            "  " & Data.WebsiteUrl.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Function Fill() As Boolean
        Try
            Dim dt As DataTable =
                ExecuteQuery(FillSQL).Tables(0)

            With dt
                If .Rows.Count > 0 Then
                    With .Rows(0)
                        _Data.Name = .Item("name")
                        _Data.Name2 = .Item("name_2")
                        _Data.PermitNo = .Item("permit_number")
                        _Data.PermitMIN = .Item("permit_min")
                        _Data.PermitIssued = If(IsDBNull(.Item("permit_issued")), Nothing, .Item("permit_issued"))
                        _Data.PermitExpiry = If(IsDBNull(.Item("permit_expiry")), Nothing, .Item("permit_expiry"))
                        _Data.VatRegistrationNo = .Item("vat_registration_no")
                        _Data.BusinessStyle = .Item("business_style")
                        _Data.Address = .Item("address")
                        _Data.ContactNo = .Item("contact_no")
                        _Data.FaxNo = .Item("fax_no")
                        _Data.EmailAddress = .Item("email_address")
                        _Data.WebsiteUrl = .Item("website_url")
                    End With
                    Return True
                Else
                    Return False
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function Save() As Boolean
        Try
            Return ExecuteNonQuery(SaveSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
