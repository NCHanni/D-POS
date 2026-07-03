Imports System.Data.SqlClient

<ServiceModel.ServiceContract()>
Public Interface ISQLServer
    <ServiceModel.OperationContract()> Function ExecuteNonQuery(ByVal connectionString As String, ByVal sqlStatement As String) As Boolean
    <ServiceModel.OperationContract()> Function ExecuteQuery(ByVal connectionString As String, ByVal sqlStatement As String) As DataSet
    <ServiceModel.OperationContract()> Function ExecuteScalar(ByVal connectionString As String, ByVal sqlStatement As String) As Object
End Interface

Public Class SQLServer
    Implements ISQLServer

    Public Function ExecuteNonQuery(ByVal connectionString As String, ByVal sqlStatement As String) As Boolean Implements ISQLServer.ExecuteNonQuery
        Try
            If String.IsNullOrWhiteSpace(connectionString) Then
                Throw New ArgumentNullException(NameOf(connectionString))
            ElseIf String.IsNullOrWhiteSpace(sqlStatement) Then
                Throw New ArgumentNullException(NameOf(sqlStatement))
            End If

            Using sqlCon As New SqlConnection(connectionString.Decrypt)
                sqlCon.Open()

                Using sqlCmd As New SqlCommand(sqlStatement, sqlCon)
                    sqlCmd.ExecuteNonQuery()
                End Using
            End Using

            Return True
        Catch ex As SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteQuery(ByVal connectionString As String, ByVal sqlStatement As String) As DataSet Implements ISQLServer.ExecuteQuery
        Try
            If String.IsNullOrWhiteSpace(connectionString) Then
                Throw New ArgumentNullException(NameOf(connectionString))
            ElseIf String.IsNullOrWhiteSpace(sqlStatement) Then
                Throw New ArgumentNullException(NameOf(sqlStatement))
            End If

            Using sqlCon As New SqlConnection(connectionString.Decrypt)
                sqlCon.Open()

                Using sqlAdp As New SqlDataAdapter(sqlStatement, sqlCon)
                    Dim data As New DataSet
                    sqlAdp.Fill(data)
                    Return data
                End Using
            End Using
        Catch ex As SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteScalar(ByVal connectionstring As String, ByVal sqlStatement As String) As Object Implements ISQLServer.ExecuteScalar
        Try
            If String.IsNullOrWhiteSpace(connectionstring) Then
                Throw New ArgumentNullException(NameOf(connectionstring))
            ElseIf String.IsNullOrWhiteSpace(sqlStatement) Then
                Throw New ArgumentNullException(NameOf(sqlStatement))
            End If

            Using sqlCon As New SqlConnection(connectionstring.Decrypt)
                sqlCon.Open()

                Using sqlCmd As New SqlCommand(sqlStatement, sqlCon)
                    Dim data As Object
                    data = sqlCmd.ExecuteScalar()
                    Return data
                End Using
            End Using
        Catch ex As SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
