Friend Class GiftCertificateList
    Inherits DataSource

#Region "Properties"

    Property Id As Long
    Property Code As String
    Property Description As String
    Property ExpiryDate As Date
    Property Amount As Double
    Property IsSold As Boolean
    Property IsUsed As Boolean

#End Region

#Region "SQL"

    Private Function FillSQL(includeReleased As Boolean) As String
        Dim sql As String =
            "EXEC dbo.GetGiftCertificate " & Code.Quote & includeReleased.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Function Fill(includeReleased As Boolean) As Boolean
        Try
            Dim dt As DataTable = ExecuteQuery(FillSQL(includeReleased)).Tables(0)

            If dt.Rows.Count = 0 Then
                Id = 0
                Code = String.Empty
                Description = String.Empty
                ExpiryDate = Nothing
                Amount = 0.0
                IsSold = False
                IsUsed = False
                Return False
            Else
                With dt.Rows(0)
                    Id = CLng(.Item("id"))
                    Code = .Item("code")
                    Description = .Item("description")
                    ExpiryDate = CDateEx(.Item("expiry_date"))
                    Amount = CDbl(.Item("amount"))
                    IsSold = CBool(.Item("is_sold"))
                    IsUsed = CBool(.Item("is_used"))
                End With
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
