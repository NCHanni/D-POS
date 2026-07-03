Imports System.IO

Friend Class ZReading
    Inherits DataSource

#Region "Properties"

    Private m_Data As DataSet
    Public ReadOnly Property Data() As DataSet
        Get
            Return m_Data
        End Get
    End Property

    Property User As String

#End Region

#Region "Methods"

    Function IsFinalized() As Boolean
        Try
            Dim sql As String =
                "SELECT TOP 1 1 FROM [z_reading] WHERE is_finalized = 1 AND CAST(date AS DATE) = '" & Now.ToDateString & "'"
            Return Exists(sql)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function CanGenerate() As Boolean
        Try
            Return ExecuteQuery("SELECT dbo.CanGenerateZReading()").Tables(0).Rows(0).Item(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub GenerateZReading()
        Try
            m_Data = ExecuteQuery("EXEC dbo.GenerateZReading")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub GenerateZReading(dateFilter As Date)
        Try
            m_Data = ExecuteQuery("EXEC dbo.GenerateZReading DEFAULT, '" & dateFilter.ToShortDateString() & "'")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Function FinalizeZReading() As Boolean
        Try
            Return ExecuteNonQuery("EXEC dbo.FinalizeZReading " & User.Quote(False))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
