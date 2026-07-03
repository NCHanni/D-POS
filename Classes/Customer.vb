Public Class Customer
    Inherits DataSource

#Region "Properties"

    Property Code As String ' No
    Property CodeName As String
    Property Name As String
    Property Address As String
    Property ContactNo As String
    Property TIN As String ' VATRegistrationNo
    Property BusinessStyle As String
    Property DiscountType As String
    Property Gender As String
    Property BirthDate As Date
    Property SCPWDNo As String
    Property IssuedDate As Date
    Property IsActive As Boolean
    Property EmployeeCode As String
    Property Operation As String

    Property SalesPersonCode As String
    Property AllowLineDisc As Boolean
    Property CustomerDiscGroup As String
    Property GenBusPostingGroup As String
    Property CustomerPriceGroup As String
    Property Address2 As String
    Property ScPwdId As String
    Property DateIssued As Date

    Property No As String
        Get
            Return Code
        End Get
        Set(value As String)
            Code = value
        End Set
    End Property

    Property VATRegistrationNo As String
        Get
            Return TIN
        End Get
        Set(value As String)
            TIN = value
        End Set
    End Property

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Try
            Dim sql As String =
                "EXEC dbo.GetCustomer " & Code.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SaveSQL() As String
        Dim sql As String =
            "EXEC dbo.SaveCustomer " &
                 Code.Quote &
                 CodeName.Quote &
                 Name.Quote &
                 Address.Quote &
                 ContactNo.Quote &
                 TIN.Quote &
                 BusinessStyle.Quote &
                 DiscountType.Quote &
                 Gender.Quote &
                 BirthDate.Quote &
                 SCPWDNo.Quote &
                 IssuedDate.Quote &
                 IsActive.Quote &
                 EmployeeCode.Quote &
                 Operation.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Private Sub GetNextCustomerCode()
        Try
            GetNextCode("C-", "[customer]", 6, Code, "WHERE code LIKE ''C-%'' AND LEN(code) = 8")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Fill()
        Try
            Dim dt As DataTable = ExecuteQuery(FillSQL()).Tables(0)

            If dt.Rows.Count > 0 Then
                With dt.Rows(0)
                    CodeName = .Item("codename")
                    Name = .Item("name")
                    Address = .Item("address")
                    ContactNo = .Item("contact_no")
                    TIN = .Item("tin")
                    BusinessStyle = .Item("business_style")
                    DiscountType = .Item("discount_type")
                    Gender = .Item("gender")
                    BirthDate = If(IsDBNull(.Item("birthdate")), Nothing, .Item("birthdate"))
                    SCPWDNo = .Item("sc_pwd_no")
                    IssuedDate = If(IsDBNull(.Item("issued_date")), Nothing, .Item("issued_date"))
                    IsActive = .Item("is_active")
                End With
            Else
                Throw New Exception("Customer not found")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function Save() As Boolean
        Try
            If Operation = "INSERT" Then
                GetNextCustomerCode()
            End If
            Return ExecuteNonQuery(SaveSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
