Friend Class CreditCardList
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Dim sql As String =
            "SELECT * FROM dbo.vwCreditCards ORDER BY [description]"
        Return sql
    End Function

    Private Function FillPaymentSQL(startDate As Date, endDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetCreditCardPayments " & startDate.Quote & endDate.Quote(False)
        Return sql
    End Function

    Private Function FillSummarySQL(startDate As Date, endDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetCreditCardPaymentsSummary " & startDate.Quote & endDate.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub Fill()
        Try
            Data = ExecuteQuery(FillSQL).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub FillPayments(startDate As Date, endDate As Date)
        Try
            Data = ExecuteQuery(FillPaymentSQL(startDate, endDate)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub FillSummary(startDate As Date, endDate As Date)
        Try
            Data = ExecuteQuery(FillSummarySQL(startDate, endDate)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
