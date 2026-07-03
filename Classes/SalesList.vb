Friend Class SalesList
    Inherits DataSource

#Region "Properties"

    Private m_Data As DataTable
    Public ReadOnly Property Data As DataTable
        Get
            Return m_Data
        End Get
    End Property

#End Region

#Region "SQL"

    Private Function FillDetailsSQL(ByVal code As String) As String
        Try
            Dim sql As String =
                "EXEC dbo.GetSalesDetails " & code.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function FillSalesForReturnSQL(startDate As Date, endDate As Date) As String
        Try
            Dim sql As String =
                "EXEC dbo.GetSalesForReturn " & startDate.Quote & endDate.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetListSQL(startDate As Date, endDate As Date, Optional isVoid As Boolean = False) As String
        Dim sql As String =
            "EXEC dbo.GetSalesListByDate " &
                startDate.Quote(False) & ", " &
                endDate.Quote(False) & ", " &
                isVoid.Quote(False)
        Return sql
    End Function

    Private Function GetListRefundSQL(startDate As Date, endDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetCustomerRefundList " &
                startDate.Quote(False) & ", " &
                endDate.Quote(False)
        Return sql
    End Function

    Private Function GetListSuspendSQL(startDate As Date, endDate As Date) As String
        Dim sql As String =
            "EXEC dbo.GetSuspendedSalesList " &
                startDate.Quote(False) & ", " &
                endDate.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub FillDetails(ByVal code As String)
        Try
            m_Data = ExecuteQuery(FillDetailsSQL(code)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub FillSalesForReturn(startDate As Date, endDate As Date)
        Try
            m_Data = ExecuteQuery(FillSalesForReturnSQL(startDate, endDate)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetList(startDate As Date, endDate As Date) As DataSet
        Try
            Dim ds As DataSet = ExecuteQuery(GetListSQL(startDate, endDate))
            With ds
                .Relations.Add(.Tables(0).Columns("sale_code"), .Tables(1).Columns("sale_code"))
            End With
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetListVoid(startDate As Date, endDate As Date) As DataSet
        Try
            Dim ds As DataSet = ExecuteQuery(GetListSQL(startDate, endDate, True))
            With ds
                .Relations.Add(.Tables(0).Columns("sale_code"), .Tables(1).Columns("sale_code"))
            End With
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetListRefund(startDate As Date, endDate As Date) As DataSet
        Try
            Dim ds As DataSet = ExecuteQuery(GetListRefundSQL(startDate, endDate))
            With ds
                .Relations.Add(.Tables(0).Columns("refund_code"), .Tables(1).Columns("refund_code"))
            End With
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetListSuspend(startDate As Date, endDate As Date) As DataSet
        Try
            Dim ds As DataSet = ExecuteQuery(GetListSuspendSQL(startDate, endDate))
            With ds
                .Relations.Add(.Tables(0).Columns("suspend_code"), .Tables(1).Columns("suspend_code"))
            End With
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
