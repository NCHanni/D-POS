Friend Class Item
    Inherits DataSource

#Region "Properties"

    Property Code As String
    Property CodeName As String = String.Empty
    Property SKU As String = String.Empty
    Property Description As String = String.Empty
    Property Specifications As String = String.Empty
    Property UnitOfMeasure As String = String.Empty
    Property Price As Double = 0.0
    Property IsVAT As Boolean
    Property IsZeroRated As Boolean
    Property IsSeniorPwd As Boolean
    Property DiscountGroup As String = String.Empty
    Property IsActive As Boolean
    Property EmployeeCode As String = String.Empty
    Property Operation As String = String.Empty

#End Region

#Region "SQL"

    Private Function SaveItemSQL() As String
        Dim sql As String =
            "EXEC dbo.SaveItemBasic " &
                Code.Quote &
                CodeName.Quote &
                SKU.Quote &
                Description.Quote &
                Specifications.Quote &
                UnitOfMeasure.Quote &
                Price.Quote &
                IsVAT.Quote &
                IsZeroRated.Quote &
                IsSeniorPwd.Quote &
                DiscountGroup.Quote &
                IsActive.Quote &
                EmployeeCode.Quote &
                Operation.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Private Sub GetNextItemCode()
        Try
            GetNextCode("ITEM-", "[item]", 7, Code)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function SaveItem() As Boolean
        Try
            If Operation = "INSERT" Then
                GetNextItemCode()
            End If

            Dim sql As String =
                CreateTryCatchBlockSQL(SaveItemSQL())

            Return ExecuteNonQuery(sql)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
