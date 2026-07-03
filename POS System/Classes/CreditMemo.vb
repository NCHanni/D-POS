Friend Class CreditMemo
    Inherits DataSource

    Property Code As String
    Property NAVCode As String
    Property SaleCode As String
    Property CustomerName As String
    Property TotalAmount As Double
    Property IsProcessed As Boolean

    Public Function FillBasic(code As String, customerCode As String) As Boolean
        Try
            Dim dt As DataTable
            Dim sql As String =
                "EXEC dbo.GetCustomerReturnByCustomer " & code.Quote & customerCode.Quote(False)

            dt = ExecuteQuery(sql).Tables(0)

            If dt.Rows.Count = 0 Then
                Return False
            Else
                code = dt.Rows(0)("code")
                NAVCode = dt.Rows(0)("credit_memo_no")
                SaleCode = dt.Rows(0)("reference_code")
                CustomerName = dt.Rows(0)("customer_name")
                TotalAmount = dt.Rows(0)("total_amount")
                IsProcessed = CBool(dt.Rows(0)("is_processed"))
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
