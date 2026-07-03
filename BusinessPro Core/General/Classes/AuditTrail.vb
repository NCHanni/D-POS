Public Class AuditTrail
    Inherits DataSource

#Region "Properties"

    Property TransactionCode As String
    Property TerminalCode As String
    Property UserCode As String
    Property UserName As String
    Property Activity As String

#End Region

#Region "SQL"

    Public Function SaveAuditTrailSQL() As String
        Dim sql As String =
            "EXEC dbo.SaveAuditTrail " & vbCrLf &
                TerminalCode.Quote &
                UserCode.Quote &
                UserName.Quote &
                Activity.Quote(False)
        Return sql
    End Function

    Public Function UpdateAuditTrailSQL() As String
        Dim sql As String =
            "EXEC dbo.UpdateAuditTrail " & vbCrLf &
                TransactionCode.Quote &
                UserCode.Quote &
                TerminalCode.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Function GetAuditTrailByDateRange(fromDateTime As DateTime, toDateTime As DateTime) As DataTable
        Try
            Dim sql As String =
                String.Format("EXEC dbo.GetAuditTrailByDateRange {0}{1}", fromDateTime.Quote(), toDateTime.Quote(False))

            Return ExecuteQuery(sql).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function SaveAuditTrail() As Boolean
        Try
            Return ExecuteNonQuery(SaveAuditTrailSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function UpdateAuditTrail() As Boolean
        Try
            Return ExecuteNonQuery(UpdateAuditTrailSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
