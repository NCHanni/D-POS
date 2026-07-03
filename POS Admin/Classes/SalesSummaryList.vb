Friend Class SalesSummaryList
    Inherits DataSource

#Region "Property"

    Property DateFrom As Date
    Property DateTo As Date

#End Region

#Region "SQL"

    Private Function GetSalesSummarySQL() As String
        Dim sql As String =
            "EXEC dbo.GetBIRSalesSummaryReport " & DateFrom.Quote & DateTo.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Function GetData() As DataTable
        Try
            Return ExecuteQuery(GetSalesSummarySQL()).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class