Friend Class Refund
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property CustomerReturnCode As String
    Property SaleCode As String
    Property TransactionDate As Date
    Property CustomerCode As String
    Property CustomerName As String
    Property TotalAmount As Double
    Property CashierSessionId As Long
    Property CashierCode As String
    Property Cashier As String
    Property Terminal As String

    Property Details As DataTable

#End Region

#Region "Methods"

    Function Save() As Boolean
        Try
            Core.Methods.GetNextCode("RS", "[refund]", 8, Code)

            Dim sql As String =
                "EXEC dbo.SaveRefund" & vbCrLf &
                "  " & Code.Quote &
                "  " & CustomerReturnCode.Quote &
                "  " & SaleCode.Quote &
                "  " & TransactionDate.Quote &
                "  " & CustomerCode.Quote &
                "  " & CustomerName.Quote &
                "  " & TotalAmount.Quote &
                "  " & CashierSessionId.Quote &
                "  " & CashierCode.Quote &
                "  " & Cashier.Quote &
                "  " & Terminal.Quote(False) & ";" & vbCrLf

            For Each row As DataRow In Details.Rows
                sql &=
                    "EXEC dbo.SaveRefundDetail" & vbCrLf &
                    "  " & Code.Quote &
                    "  " & row("ItemLineId").ToString.Quote &
                    "  " & row("ItemCode").ToString.Quote &
                    "  " & row("Description").ToString.Quote &
                    "  " & row("UnitOfMeasure").ToString.Quote &
                    "  " & row("UnitPrice").ToString.Quote &
                    "  " & row("Quantity").ToString.Quote &
                    "  " & row("DiscountAmount").ToString.Quote &
                    "  " & row("VATAmount").ToString.Quote &
                    "  " & row("LineTotal").ToString.Quote(False) & ";" & vbCrLf
            Next

            sql = CreateTryCatchBlockSQL(sql)

            Return ExecuteNonQuery(sql)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
