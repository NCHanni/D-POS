Public Class SQLStringBuilder

    Public Shared Function Create(ByVal datasource As String, ByVal database As String, ByVal username As String, ByVal password As String) As String
        Try
            With New SqlClient.SqlConnectionStringBuilder
                .DataSource = datasource
                .InitialCatalog = database
                .UserID = username
                .Password = password
                Return .ConnectionString
            End With
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
