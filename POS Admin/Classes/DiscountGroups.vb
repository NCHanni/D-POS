Friend Class DiscountGroups
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable
    Property List As DiscountGroupsList
    Property User As String

#End Region

#Region "Constructor"

    Public Sub New()
        Try
            List = New DiscountGroupsList
            Data = New DataTable

            With Data.Columns
                .Add("code")
                .Add("description")
                .Add("is_vat_exempt", GetType(Boolean))
                .Add("discount_rate", GetType(Double))
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Function Save() As Boolean
        Try
            Dim sql As String = ""

            For i As Integer = 0 To Data.Rows.Count - 1
                With Data.Rows(i)
                    If .RowState = DataRowState.Added Then
                        sql &= "EXEC dbo.SaveDiscountGroup" & vbCrLf &
                            .Item("code").ToString.Quote & vbCrLf &
                            .Item("description").ToString.Quote & vbCrLf &
                            .Item("is_vat_exempt").ToString.Quote & vbCrLf &
                            .Item("discount_rate").ToString.Quote & vbCrLf &
                            "'INSERT'"

                    ElseIf .RowState = DataRowState.Modified Then
                        sql &= "EXEC dbo.SaveDiscountGroup" & vbCrLf &
                            .Item("code").ToString.Quote & vbCrLf &
                            .Item("description").ToString.Quote & vbCrLf &
                            .Item("is_vat_exempt").ToString.Quote & vbCrLf &
                            .Item("discount_rate").ToString.Quote & vbCrLf &
                            "'UPDATE'"

                    ElseIf .RowState = DataRowState.Deleted Then
                        sql &= "EXEC dbo.SaveDiscountGroup" & vbCrLf &
                            .Item("code", DataRowVersion.Original).ToString.Quote & vbCrLf &
                            .Item("description", DataRowVersion.Original).ToString.Quote & vbCrLf &
                            .Item("is_vat_exempt", DataRowVersion.Original).ToString.Quote & vbCrLf &
                            .Item("discount_rate", DataRowVersion.Original).ToString.Quote & vbCrLf &
                            "'DELETE'"
                    End If
                End With
            Next

            If sql <> "" Then
                Return ExecuteNonQuery(sql)
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
