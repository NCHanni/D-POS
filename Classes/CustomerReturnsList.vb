Friend Class CustomerReturnsList
    Inherits DataSource

#Region "Properties"

    Private m_Data As DataSet
    Public ReadOnly Property Data() As DataSet
        Get
            Return m_Data
        End Get
    End Property

#End Region

#Region "SQL"

    Private Function FillSQL(ByVal startDate As Date, ByVal endDate As Date) As String
        Try
            Dim sql As String =
                "EXEC dbo.GetCustomerReturnList " & startDate.Quote() & endDate.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Methods"

    Public Sub Fill(ByVal startDate As Date, ByVal endDate As Date)
        Try
            m_Data = ExecuteQuery(FillSQL(startDate, endDate))
            With m_Data
                .Relations.Add("Details", .Tables(0).Columns("code"), .Tables(1).Columns("cr_code"), False)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
