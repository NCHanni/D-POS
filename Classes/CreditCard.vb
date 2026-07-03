Friend Class CreditCard
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property Description As String
    Property IsActive As Boolean
    Property EmployeeCode As String
    Property Operation As String

#End Region

#Region "SQL"

    Private Function SaveCreditCardSQL() As String
        Dim sql As String =
            "EXEC dbo.SaveCreditCard " &
                Code.Quote &
                Description.Quote &
                IsActive.Quote &
                EmployeeCode.Quote &
                Operation.Quote(False)
        Return sql
    End Function

#End Region

#Region "Private Methods"

    Private Sub GetNextCreditCardCode()
        Try
            GetNextCode("CC", "credit_card", 7, Code)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Function SaveCreditCard() As Boolean
        Try
            If Operation = "INSERT" Then
                GetNextCreditCardCode()
            End If
            Return ExecuteNonQuery(SaveCreditCardSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
