Friend Class ItemList
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Dim sql As String =
            "EXEC dbo.GetItemsForSale"
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

#End Region

End Class
