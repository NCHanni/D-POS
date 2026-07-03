Friend Class ScPwd
    Inherits DataSource

    Private m_Data As DataTable
    Public ReadOnly Property Data() As DataTable
        Get
            Return m_Data
        End Get
    End Property

    Private Function FillScPwdSQL(
            ByVal discountType As String,
            ByVal fromDate As Date,
            ByVal toDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetSCPWDSummary " & vbCrLf &
                discountType.Quote &
                fromDate.Quote &
                toDate.Quote(False)
        Return sql
    End Function

    Public Sub FillScPwd(ByVal discountType As String, ByVal fromDate As Date, ByVal toDate As Date)
        Try
            m_Data = ExecuteQuery(FillScPwdSQL(discountType, fromDate, toDate)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
