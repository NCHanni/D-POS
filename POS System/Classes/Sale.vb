Friend Class Sale
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property CashierSessionId As Long
    Property TerminalCode As String
    Property TransactionDate As Date
    Property CustomerCode As String
    Property CustomerName As String
    Property CustomerTIN As String
    Property CustomerBusinessStyle As String
    Property CustomerAddress As String
    Property CustomerIsSC As Boolean
    Property CustomerIsPWD As Boolean
    Property SalespersonCode As String
    Property SalespersonName As String
    Property CashierCode As String
    Property CashierName As String
    Property InvoiceCode As String
    Property PaymentCode As String
    Property TotalVAT As Double
    Property TotalDiscount As Double
    Property TotalAmount As Double
    Property TotalVATSales As Double
    Property TotalVATExemptSales As Double
    Property TotalZeroRatedSales As Double
    Property TotalRegularLessVAT As Double
    Property SuspendedSaleCode As String

    Property Items As DataTable

    Property SeniorPwdDiscountData As DataTable
    Property PaymentCash As DataTable
    Property PaymentCreditCard As DataTable
    Property PaymentGiftCertificate As DataTable
    Property PaymentCreditMemo As DataTable

#End Region

#Region "SQL"

    Private Function SaveSQL() As String
        Try
            Dim sql As String =
                "EXEC dbo.SaveSale" & vbCrLf &
                    Code.Quote &
                    CashierSessionId.Quote &
                    TerminalCode.Quote &
                    TransactionDate.Quote &
                    CustomerCode.Quote &
                    CustomerName.Quote &
                    CustomerTIN.Quote &
                    CustomerBusinessStyle.Quote &
                    CustomerAddress.Quote &
                    SalespersonCode.Quote &
                    SalespersonName.Quote &
                    CashierCode.Quote &
                    CashierName.Quote &
                    InvoiceCode.Quote &
                    PaymentCode.Quote &
                    TotalVAT.Quote &
                    TotalDiscount.Quote &
                    TotalAmount.Quote &
                    TotalVATSales.Quote &
                    TotalVATExemptSales.Quote &
                    TotalZeroRatedSales.Quote &
                    TotalRegularLessVAT.Quote(False, ";" & vbCrLf)

            For Each row As DataRow In Items.Rows
                If row.RowState = DataRowState.Deleted Then
                    Continue For
                End If
                sql &=
                    "EXEC dbo.SaveSaleDetails" & vbCrLf &
                        row("ItemLineId").ToString.Quote &
                        Code.Quote &
                        row("ItemCode").ToString.Quote &
                        row("Description").ToString.Quote &
                        row("ItemCategoryCode").ToString.Quote &
                        row("UnitPrice").ToString.Quote &
                        row("Quantity").ToString.Quote &
                        row("UnitOfMeasure").ToString.Quote &
                        row("QtyPerUOM").ToString.Quote &
                        row("DiscountIsRegular").ToString.Quote &
                        row("DiscountPercent").ToString.Quote &
                        row("DiscountAmount").ToString.Quote &
                        row("DiscountedPrice").ToString.Quote &
                        row("VATPercent").ToString.Quote &
                        row("VATAmount").ToString.Quote &
                        row("VATExemptAmount").ToString.Quote &
                        row("LineTotal").ToString.Quote &
                        row("IsVATable").ToString.Quote &
                        row("IsZeroRated").ToString.Quote &
                        row("IsVATExempt").ToString.Quote &
                        row("IsGiftCertificate").ToString.Quote &
                        row("SerialNbr").ToString.Quote &
                        row("SKU").ToString.Quote(False, ";" & vbCrLf)
            Next

            For Each row As DataRow In PaymentCash.Rows
                sql &=
                    "EXEC dbo.SavePaymentCash" & vbCrLf &
                        Code.Quote &
                        row("amount").ToString.Quote &
                        row("change").ToString.Quote &
                        row("payment_code").ToString.Quote(False, ";" & vbCrLf)
            Next

            For Each row As DataRow In PaymentCreditCard.Rows
                sql &=
                    "EXEC dbo.SavePaymentCreditCard" & vbCrLf &
                        Code.Quote &
                        row("credit_card_code").ToString.Quote &
                        row("description").ToString.Quote &
                        row("card_number").ToString.Quote &
                        row("card_holder").ToString.Quote &
                        row("bank_name").ToString.Quote &
                        row("card_approval_code").ToString.Quote &
                        row("amount").ToString.Quote &
                        row("payment_code").ToString.Quote(False, ";" & vbCrLf)
            Next

            For Each row As DataRow In PaymentGiftCertificate.Rows
                sql &=
                    "EXEC dbo.SavePaymentGiftCertificate" & vbCrLf &
                        Code.Quote &
                        row("gc_no").ToString.Quote &
                        row("description").ToString.Quote &
                        row("amount").ToString.Quote &
                        row("payment_code").ToString.Quote(False, ";" & vbCrLf)
            Next

            For Each row As DataRow In PaymentCreditMemo.Rows
                sql &=
                    "EXEC dbo.SavePaymentCreditMemo" & vbCrLf &
                        Code.Quote &
                        row("CreditMemoNo").ToString.Quote &
                        row("Amount").ToString.Quote &
                        row("PaymentCode").ToString.Quote(False, ";" & vbCrLf)
            Next

            For Each row As DataRow In SeniorPwdDiscountData.Rows
                sql &=
                    "EXEC dbo.SaveSaleDiscountDetails" & vbCrLf &
                        Code.Quote &
                        row("discount_type").ToString.Quote &
                        row("id_no").ToString.Quote &
                        row("name").ToString.Quote &
                        row("gender").ToString.Quote &
                        row("birthdate").ToString.Quote &
                        row("issued_date").ToString.Quote &
                        row("total_discount").ToString.Quote &
                        row("total_less_vat").ToString.Quote(False, ";" & vbCrLf)
            Next

            If Not String.IsNullOrWhiteSpace(SuspendedSaleCode) Then
                sql &= "UPDATE [suspended_sale] SET is_finalized = 'True' WHERE code = " & SuspendedSaleCode.Quote(False) & vbCrLf
            End If

            sql = CreateTryCatchBlockSQL(sql)

            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function FillSQL(ByVal code As String) As String
        Dim sql As String =
            "EXEC dbo.GetSale " & code.Quote(False)
        Return sql
    End Function

    Private Function GetListSQL(transactionDate As Date, cashierSessionId As Long, customerCode As String) As String
        Dim sql As String =
            "EXEC dbo.GetSalesList '" & transactionDate.ToDateString() & "'," & cashierSessionId & ",'" & customerCode & "';"
        Return sql
    End Function

    Private Function VoidSQL(code As String, reason As String, authorizedBy As String) As String
        Dim sql As String =
            "EXEC dbo.SaveVoidSale" & vbCrLf &
            "   " & code.Quote &
            "   " & TerminalCode.Quote &
            "   " & CashierCode.Quote &
            "   " & Me.Code.Quote &
            "   " & Me.InvoiceCode.Quote &
            "   " & Me.PaymentCode.Quote & ' Reference No/Credit Memo No (NAV)
            "   " & TotalAmount.Quote &
            "   " & reason.Quote &
            "   " & authorizedBy.Quote(False, ";")
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub GetNextCode(ByRef code As String)
        Try
            code = ExecuteQuery("EXEC dbo.GetNextSaleCode").Tables(0).Rows(0)(0).ToString
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function Save() As Boolean
        Try
            Return ExecuteNonQuery(SaveSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Fill(ByVal code As String) As Boolean
        Try
            Dim ds As DataSet = ExecuteQuery(FillSQL(code))
            Dim dr As DataRow

            If ds.Tables(0).Rows.Count > 0 Then
                dr = ds.Tables(0).Rows(0)

                Me.Code = dr("code")
                TerminalCode = dr("terminal_code")
                TransactionDate = dr("transaction_date")
                CustomerCode = dr("customer_code")
                CustomerName = dr("customer_name")
                CustomerTIN = dr("customer_tin")
                CustomerBusinessStyle = dr("customer_business_style")
                CustomerAddress = dr("customer_address")
                CustomerIsSC = dr("is_sc")
                CustomerIsPWD = dr("is_pwd")
                CashierCode = dr("cashier_code")
                CashierName = dr("cashier_name")
                InvoiceCode = dr("invoice_code")
                TotalVAT = dr("total_vat")
                TotalDiscount = dr("total_discount")
                TotalAmount = dr("total_amount")
                TotalVATSales = dr("vat_sales")
                TotalVATExemptSales = dr("vat_exempt_sales")
                TotalZeroRatedSales = dr("zero_rated_sales")

                Me.Items = ds.Tables(1).Copy
            Else
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetList(trnDate As Date, cashierSessionId As Long, customerCode As String) As DataTable
        Try
            Return ExecuteQuery(GetListSQL(trnDate, cashierSessionId, customerCode)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Void(reason As String, authorizedBy As String) As String
        Try
            Dim code As String = ""

            Core.Methods.GetNextCode("VS", "[void_sale]", 8, code)

            If ExecuteNonQuery(VoidSQL(code, reason, authorizedBy)) Then
                Return code
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
