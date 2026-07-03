Public Class CustomerList
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL(ByVal showInactive As Boolean) As String
        Dim sql As String =
            "EXEC dbo.GetCustomerList " & showInactive.Quote(False)
        Return sql
    End Function

    Private Function ExistsSQL(ByVal codeName As String) As String
        Dim sql As String =
            "SELECT code FROM [customer] WHERE codename = " & codeName.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub Fill(ByVal showInactive As Boolean)
        Try
            Data = ExecuteQuery(FillSQL(showInactive)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Overloads Function Exists(ByVal codeName As String) As Boolean
        Try
            Return MyBase.Exists(ExistsSQL(codeName))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
