Friend Class Salesperson
    Inherits DataSource

#Region "Struct"

    Structure Struct
        Property Code As String
        Property Name As String
        Property Barcode As String
    End Structure

#End Region

#Region "Properties"

    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Dim sql As String =
            "EXEC dbo.GetSalespersonList"
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
