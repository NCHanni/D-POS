Friend Class CreditCardList
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Dim sql As String =
            "EXEC dbo.GetCreditCardList"
        Return sql
    End Function

    Private Function ExistsSQL(ByVal description As String) As String
        Dim sql As String =
            "SELECT code FROM dbo.vwCreditCards WHERE [description] = " & description.Quote(False)
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

    Public Overloads Function Exists(ByVal description As String) As Boolean
        Try
            Return ExecuteQuery(ExistsSQL(description)).HasData
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
