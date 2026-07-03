Friend Class DailySalesList
    Inherits DataSource

#Region "SQL"

    Private Function GetDailySalesSQL(trnDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetBIRSalesDailyReport " & trnDate.Quote(False)
        Return sql
    End Function

    Private Function GetDailySalesSQLQR(trnDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetBIRSalesDailyReportQR " & trnDate.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Function GetData(trnDate As Date, isNewDaily As Boolean) As DataTable
        Try
            If isNewDaily Then
                Return ExecuteQuery(GetDailySalesSQLQR(trnDate)).Tables(0)
            Else
                Return ExecuteQuery(GetDailySalesSQL(trnDate)).Tables(0)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function



#End Region

End Class