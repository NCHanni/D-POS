Friend Class DiscountGroupsList
    Inherits DataSource

#Region "Methods"

    Public Function GetList() As DataTable
        Try
            Dim sql As String =
                "SELECT" & vbCrLf &
                "  [code]," & vbCrLf &
                "  [description]," & vbCrLf &
                "  [is_vat_exempt], " & vbCrLf &
                "  [discount_rate] " & vbCrLf &
                "FROM" & vbCrLf &
                "	dbo.vwDiscountGroups" & vbCrLf &
                "ORDER BY [description]"

            Return ExecuteQuery(sql).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
