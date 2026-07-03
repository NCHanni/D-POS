Friend Class CustomerReturn
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property ReferenceCode As String
    Property CreditMemoNo As String
    Property TransactionDate As Date
    Property CustomerCode As String
    Property CustomerName As String
    Property Remarks As String
    Property TotalVAT As Double
    Property TotalDiscount As Double
    Property TotalAmount As Double
    Property VATSales As Double
    Property VATExemptSales As Double
    Property ZeroRatedSales As Double
    Property ScPwdDiscount As Double
    Property ScPwdLessVAT As Double
    Property ForPOSRefund As Boolean
    Property IsDraft As Boolean
    Property UserName As String
    Property Details As DataTable
    Property Operation As String

#End Region

#Region "SQL"

    Private Function SaveSQL() As String
        Try
            Dim sql As String

            If Operation = "INSERT" Then
                sql = GetNextCodeSQL("CR", "[customer_return]", 8) & vbCrLf &
                      "EXEC dbo.SaveCustomerReturn " & vbCrLf & "@code," & vbCrLf
            Else
                sql = "EXEC dbo.SaveCustomerReturn " & vbCrLf & Code.Quote
            End If

            sql &=
                ReferenceCode.Quote &
                CreditMemoNo.Quote &
                TransactionDate.Quote &
                CustomerCode.Quote &
                CustomerName.Quote &
                Remarks.Quote &
                TotalVAT.Quote &
                TotalDiscount.Quote &
                TotalAmount.Quote &
                VATSales.Quote &
                VATExemptSales.Quote &
                ZeroRatedSales.Quote &
                ScPwdDiscount.Quote &
                ScPwdLessVAT.Quote &
                ForPOSRefund.Quote &
                IsDraft.Quote &
                UserName.Quote &
                Operation.Quote(False) & vbCrLf

            If Operation <> "DELETE" Then
                Dim codeSql As String = If(Operation = "INSERT", "@code," & vbCrLf, Code.Quote)
                Dim rowOperation As String = ""

                For Each row As DataRow In Details.Rows
                    Select Case row.RowState
                        Case DataRowState.Added : rowOperation = "INSERT"
                        Case DataRowState.Modified : rowOperation = "UPDATE"
                        Case DataRowState.Deleted : rowOperation = "DELETE"
                        Case DataRowState.Unchanged : Continue For
                    End Select

                    If rowOperation <> "DELETE" Then
                        sql &=
                            "EXEC dbo.SaveCustomerReturnDetails " & vbCrLf &
                                row("item_line_id").ToString.Quote &
                                codeSql &
                                row("item_code").ToString.Quote &
                                row("item_description").ToString.Quote &
                                row("unit_of_measure").ToString.Quote &
                                row("price").ToString.Quote &
                                row("qty").ToString.Quote &
                                row("qty_returned").ToString.Quote &
                                row("qty_per_uom").ToString.Quote &
                                row("is_regular_discount").ToString.Quote &
                                row("discount_percent").ToString.Quote &
                                row("discount_amount").ToString.Quote &
                                row("discounted_price").ToString.Quote &
                                row("vat_percent").ToString.Quote &
                                row("vat_amount").ToString.Quote &
                                row("vat_exempt_amount").ToString.Quote &
                                row("line_total").ToString.Quote &
                                row("is_vatable").ToString.Quote &
                                row("is_zero_rated").ToString.Quote &
                                row("is_vat_exempt").ToString.Quote &
                                row("is_gift_certificate").ToString.Quote &
                                rowOperation.Quote(False) & vbCrLf
                    Else
                        sql &=
                            "EXEC dbo.SaveCustomerReturnDetails " & vbCrLf &
                                row("item_line_id", DataRowVersion.Original).ToString.Quote &
                                codeSql &
                                row("item_code", DataRowVersion.Original).ToString.Quote &
                                row("item_description", DataRowVersion.Original).ToString.Quote &
                                row("unit_of_measure", DataRowVersion.Original).ToString.Quote &
                                row("price", DataRowVersion.Original).ToString.Quote &
                                row("qty", DataRowVersion.Original).ToString.Quote &
                                row("qty_returned", DataRowVersion.Original).ToString.Quote &
                                row("qty_per_uom", DataRowVersion.Original).ToString.Quote &
                                row("is_regular_discount", DataRowVersion.Original).ToString.Quote &
                                row("discount_percent", DataRowVersion.Original).ToString.Quote &
                                row("discount_amount", DataRowVersion.Original).ToString.Quote &
                                row("discounted_price", DataRowVersion.Original).ToString.Quote &
                                row("vat_percent", DataRowVersion.Original).ToString.Quote &
                                row("vat_amount", DataRowVersion.Original).ToString.Quote &
                                row("vat_exempt_amount", DataRowVersion.Original).ToString.Quote &
                                row("line_total", DataRowVersion.Original).ToString.Quote &
                                row("is_vatable", DataRowVersion.Original).ToString.Quote &
                                row("is_zero_rated", DataRowVersion.Original).ToString.Quote &
                                row("is_vat_exempt", DataRowVersion.Original).ToString.Quote &
                                row("is_gift_certificate", DataRowVersion.Original).ToString.Quote &
                                rowOperation.Quote(False) & vbCrLf
                    End If
                Next
            End If

            sql = CreateTryCatchBlockSQL(sql)

            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function UpdateCMSQL(creditMemoNo As String) As String
        Dim sql As String =
            "UPDATE [customer_return] SET credit_memo_no = " & creditMemoNo.Quote(False) & " WHERE code = " & Code.Quote(False)
        Return sql
    End Function

    Private Function GetDataSQL() As String
        Try
            Dim sql As String =
                "EXEC dbo.GetCustomerReturn " & Code.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetDetailsSQL()
        Try
            Dim sql As String =
                "EXEC dbo.GetCustomerReturnDetails " & Code.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Methods"

    Public Function Save() As Boolean
        Try
            If Operation = "INSERT" Then
                Code = ExecuteScalar(SaveSQL) ' scalar
                Return (Code <> "")
            Else
                Return ExecuteNonQuery(SaveSQL)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function UpdateCM(creditMemoNo As String) As Boolean
        Try
            Return ExecuteNonQuery(UpdateCMSQL(creditMemoNo))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetData() As DataSet
        Try
            Dim ds As DataSet = ExecuteQuery(GetDataSQL)
            With ds
                .Relations.Add("Details", .Tables(0).Columns("code"), .Tables(1).Columns("cr_code"), False)
            End With
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub GetDetails()
        Try
            Details = ExecuteQuery(GetDetailsSQL).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
