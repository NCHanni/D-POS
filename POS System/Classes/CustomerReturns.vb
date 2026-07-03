Friend Class CustomerReturns
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property ReferenceCode As String ' Sale Code
    Property TransactionDate As Date
    Property CustomerCode As String
    Property CustomerName As String
    Property Remarks As String
    Property TotalVAT As Double
    Property TotalDiscount As Double
    Property TotalAmount As Double
    Property PreparedBy As String

    Property Details As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL(ByVal code As String) As String
        Dim sql As String =
            "EXECUTE dbo.GetCustomerReturn " & code.Quote(False)
        Return sql
    End Function

    Private Function GetListSQL(customerCode As String, trnDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetCustomerReturnsForRefund " & customerCode & "', '" & trnDate.ToDateString & "';"
        Return sql
    End Function

    Private Function ProcessSQL(code As String, user As String)
        Dim sql As String =
            "UPDATE [customer_return] SET " &
                "is_processed = 'True', " &
                "date_processed = CURRENT_TIMESTAMP, " &
                "processed_by = " & user.Quote(False) & ", " &
                "[status] = 'Processed' " &
            "WHERE " &
                "code = " & code.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Function Fill(ByVal code As String) As Boolean
        Try
            Dim ds As DataSet = ExecuteQuery(FillSQL(code))
            Dim dr As DataRow

            If ds.Tables(0).Rows.Count > 0 Then
                dr = ds.Tables(0).Rows(0)

                Me.Code = dr("code")
                ReferenceCode = dr("reference_code")
                TransactionDate = dr("transaction_date")
                CustomerCode = dr("customer_code")
                CustomerName = dr("customer_name")
                Remarks = dr("remarks")
                TotalVAT = dr("total_vat")
                TotalDiscount = dr("total_discount")
                TotalAmount = dr("total_amount")
                PreparedBy = dr("prepared_by")

                Me.Details = ds.Tables(1).Copy
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetList(customerCode As String, trnDate As Date) As DataTable
        Try
            Return ExecuteQuery(GetListSQL(customerCode, trnDate)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Process(code As String, user As String) As Boolean
        Try
            Return ExecuteNonQuery(ProcessSQL(code, user))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
