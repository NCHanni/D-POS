Friend Class Reset
    Inherits DataSource

#Region "SQL"

    Private Function GetDetailsSQL() As String
        Dim sql As String =
            "EXEC dbo.GetResetDetails"
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Function GetDetails() As DataSet

        Try
            Return ExecuteQuery(GetDetailsSQL())
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
