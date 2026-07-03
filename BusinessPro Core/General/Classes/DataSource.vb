Imports BusinessPro.Database

Public Class DataSource

#Region "Structure"

    Public Structure ConnectionString
        Dim LocalConnectionString As String
        Dim ServerAddress As String
        Dim ServerIP As String
    End Structure

    Public Enum SQLType As Integer
        Insert = 0
        Update = 1
        Delete = 2
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(Optional connectionString As ConnectionString = Nothing)
        Try
            If String.IsNullOrEmpty(connectionString.LocalConnectionString) Then
                connectionString = Core.Current.ConnectionString
            End If

            With connectionString
                m_ConnectionString = .LocalConnectionString.Encrypt
                m_ServerAddress = .ServerAddress
                m_ServerIP = .ServerIP
            End With

            m_LocalDataSource = New BusinessPro.Database.SQLServer
            m_CurrentConnectionString = connectionString
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Variables"

    Private ReadOnly m_ConnectionString As String
    Private ReadOnly m_ServerAddress As String
    Private ReadOnly m_ServerIP As String

    Private ReadOnly m_LocalDataSource As BusinessPro.Database.SQLServer
    Private m_CurrentConnectionString As ConnectionString

#End Region

#Region "Properties"

    Public ReadOnly Property CurrentConnectionString()
        Get
            Return m_CurrentConnectionString
        End Get
    End Property

#End Region

#Region "Methods"

    Public Function ExecuteNonQuery(ByVal sqlStatement As String) As Boolean
        Try
            Return m_LocalDataSource.ExecuteNonQuery(m_ConnectionString, sqlStatement)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteQuery(ByVal sqlStatement As String) As DataSet
        Try
            Return m_LocalDataSource.ExecuteQuery(m_ConnectionString, sqlStatement)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Exists(ByVal sqlStatement As String) As Boolean
        Try
            Dim sql As String = "IF EXISTS(" & sqlStatement & ") SELECT 'True' ELSE SELECT 'False'"
            Return CBool(m_LocalDataSource.ExecuteQuery(m_ConnectionString, sql).Tables(0).Rows(0)(0))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteScalar(ByVal sqlStatement As String) As Object
        Try
            Return m_LocalDataSource.ExecuteScalar(m_ConnectionString, sqlStatement)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CreateTryCatchBlockSQL(ByVal sqlStatement As String) As String
        Try
            Dim sql As String =
                "BEGIN TRY" & vbCrLf &
                "BEGIN TRANSACTION;" & vbCrLf & vbCrLf &
                "" & sqlStatement & vbCrLf & vbCrLf &
                "COMMIT TRANSACTION;" & vbCrLf &
                "END TRY" & vbCrLf &
                "BEGIN CATCH" & vbCrLf &
                "   ROLLBACK TRANSACTION;" & vbCrLf &
                "   DECLARE @ErrorMessage NVARCHAR(4000);" & vbCrLf &
                "   DECLARE @ErrorSeverity INT;" & vbCrLf &
                "   DECLARE @ErrorState INT; " & vbCrLf &
                "   SELECT" & vbCrLf &
                "       @ErrorMessage = ERROR_MESSAGE()," & vbCrLf &
                "       @ErrorSeverity = ERROR_SEVERITY()," & vbCrLf &
                "       @ErrorState = ERROR_STATE();" & vbCrLf &
                "   RAISERROR (@ErrorMessage, @ErrorSeverity,@ErrorState); " & vbCrLf &
                "END CATCH"
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
