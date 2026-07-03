Friend Class SuspendedSale
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property Terminal As String
    Property TransactionDate As DateTime
    Property CustomerCode As String
    Property Customer As String
    Property IsSC As Boolean
    Property IsPWD As Boolean
    Property ScPwdNo As String
    Property ScPwdGender As String
    Property ScPwdBirthdate As Date
    Property ScPwdIssuedDate As Date
    Property TotalDiscount As Double
    Property TotalVAT As Double
    Property TotalAmount As Double
    Property CustomerDiscountGroup As String
    Property Reason As String = ""
    Property CashierCode As String
    Property Cashier As String
    Property CashierSessionId As Long
    Property Operation As String

    Property Details As DataTable

#End Region

#Region "SQL"

    Private Function DeleteSQL(code As String, reason As String, authorizedBy As String) As String
        Dim sql As String =
            "UPDATE [suspended_sale] SET reason = " & reason.Quote(False) & ", is_disposed = 'True', authorized_by = " & authorizedBy.Quote(False) & " WHERE code = " & code.Quote(False)
        Return sql
    End Function

    Private Function SaveSQL() As String
        Try
            Dim sql As String =
                "DECLARE @code VARCHAR(255) = " & Code.Quote(False) & vbCrLf &
                "EXEC dbo.SaveSuspendedSale " & vbCrLf &
                "  @code," & vbCrLf &
                "  " & Terminal.Quote &
                "  " & TransactionDate.Quote &
                "  " & CustomerCode.Quote &
                "  " & Customer.Quote &
                "  " & IsSC.Quote &
                "  " & IsPWD.Quote &
                "  " & ScPwdNo.Quote &
                "  " & ScPwdGender.Quote &
                "  " & ScPwdBirthdate.Quote &
                "  " & ScPwdIssuedDate.Quote &
                "  " & TotalDiscount.Quote &
                "  " & TotalVAT.Quote &
                "  " & TotalAmount.Quote &
                "  " & CustomerDiscountGroup.Quote &
                "  " & Reason.Quote &
                "  " & CashierCode.Quote &
                "  " & Cashier.Quote &
                "  " & CashierSessionId.Quote &
                "  " & Operation.Quote(False, ";" & vbCrLf)

            For Each row As DataRow In Details.Rows
                Dim detailOperation As String
                Select Case row.RowState
                    Case DataRowState.Added : detailOperation = "INSERT"
                    Case DataRowState.Modified : detailOperation = "UPDATE"
                    Case DataRowState.Deleted : detailOperation = "DELETE"
                    Case Else
                        Continue For
                End Select

                If detailOperation <> "DELETE" Then
                    sql &=
                        "EXEC dbo.SaveSuspendedSaleDetails " & vbCrLf &
                        "  @code," & vbCrLf &
                        "  " & row("ItemLineId").ToString.Quote &
                        "  " & row("ItemCode").ToString.Quote &
                        "  " & row("SKU").ToString.Quote &
                        "  " & row("Description").ToString.Quote &
                        "  " & row("ItemCategoryCode").ToString.Quote &
                        "  " & row("UnitPrice").ToString.Quote &
                        "  " & row("BasePrice").ToString.Quote &
                        "  " & row("Quantity").ToString.Quote &
                        "  " & row("UnitOfMeasure").ToString.Quote &
                        "  " & row("DiscountGroup").ToString.Quote &
                        "  " & row("DiscountIsAllowed").ToString.Quote &
                        "  " & row("DiscountIsRegular").ToString.Quote &
                        "  " & row("DiscountPercent").ToString.Quote &
                        "  " & row("DiscountAmount").ToString.Quote &
                        "  " & row("DiscountedPrice").ToString.Quote &
                        "  " & row("VATAmount").ToString.Quote &
                        "  " & row("VATExemptAmount").ToString.Quote &
                        "  " & row("LineTotal").ToString.Quote &
                        "  " & row("IsVATable").ToString.Quote &
                        "  " & row("IsZeroRated").ToString.Quote &
                        "  " & row("IsVATExempt").ToString.Quote &
                        "  " & row("IsGiftCertificate").ToString.Quote &
                        "  " & detailOperation.Quote(False, ";" & vbCrLf)
                Else
                    sql &=
                        "EXEC dbo.SaveSuspendedSaleDetails " & vbCrLf &
                        "  @code," & vbCrLf &
                        "  " & row("ItemLineId", DataRowVersion.Original).ToString.Quote &
                        "  " & row("ItemCode", DataRowVersion.Original).ToString.Quote &
                        "  " & row("SKU", DataRowVersion.Original).ToString.Quote &
                        "  " & row("Description", DataRowVersion.Original).ToString.Quote &
                        "  " & row("ItemCategoryCode", DataRowVersion.Original).ToString.Quote &
                        "  " & row("UnitPrice", DataRowVersion.Original).ToString.Quote &
                        "  " & row("BasePrice", DataRowVersion.Original).ToString.Quote &
                        "  " & row("Quantity", DataRowVersion.Original).ToString.Quote &
                        "  " & row("UnitOfMeasure", DataRowVersion.Original).ToString.Quote &
                        "  " & row("DiscountGroup", DataRowVersion.Original).ToString.Quote &
                        "  " & row("DiscountIsAllowed", DataRowVersion.Original).ToString.Quote &
                        "  " & row("DiscountIsRegular", DataRowVersion.Original).ToString.Quote &
                        "  " & row("DiscountPercent", DataRowVersion.Original).ToString.Quote &
                        "  " & row("DiscountAmount", DataRowVersion.Original).ToString.Quote &
                        "  " & row("DiscountedPrice", DataRowVersion.Original).ToString.Quote &
                        "  " & row("VATAmount", DataRowVersion.Original).ToString.Quote &
                        "  " & row("VATExemptAmount", DataRowVersion.Original).ToString.Quote &
                        "  " & row("LineTotal", DataRowVersion.Original).ToString.Quote &
                        "  " & row("IsVATable", DataRowVersion.Original).ToString.Quote &
                        "  " & row("IsZeroRated", DataRowVersion.Original).ToString.Quote &
                        "  " & row("IsVATExempt", DataRowVersion.Original).ToString.Quote &
                        "  " & row("IsGiftCertificate", DataRowVersion.Original).ToString.Quote &
                        "  " & detailOperation.Quote(False, ";" & vbCrLf)
                End If
            Next

            sql = CreateTryCatchBlockSQL(sql)

            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetListSQL(cashierSessionId As Long, ByVal terminalCode As String, ByVal customerCode As String) As String
        Dim sql As String =
            "EXEC dbo.GetSuspendedSales " & vbCrLf &
             "  " & cashierSessionId.Quote &
             "  " & terminalCode.Quote &
             "  " & customerCode.Quote(False)
        Return sql
    End Function

    Private Function HasPendingSQL(cashierSessionId As Long) As String
        Dim sql As String =
            "SELECT TOP 1 1 FROM [suspended_sale] WHERE cashier_session_id = " & cashierSessionId & " AND is_finalized = 'False' AND is_disposed = 'False'"
        Return sql
    End Function

#End Region

#Region "Methods"

    Function Delete(code As String, reason As String, authorizedBy As String) As Boolean
        Try
            Return ExecuteNonQuery(DeleteSQL(code, reason, authorizedBy))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function Save() As Boolean
        Try
            If Operation = "INSERT" Then
                Core.Methods.GetNextCode("SS", "[suspended_sale]", 8, Code)
            End If

            Return ExecuteNonQuery(SaveSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function GetList(cashierSessionId As Long, ByVal terminalCode As String, ByVal customerCode As String) As DataSet
        Try
            Return ExecuteQuery(GetListSQL(cashierSessionId, terminalCode, customerCode))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function HasPending(cashierSessionId As Long) As Boolean
        Try
            Return Exists(HasPendingSQL(cashierSessionId))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
