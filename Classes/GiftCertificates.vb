Friend Class GiftCertificates
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable
    Property UserName As String

#End Region

#Region "Methods"

    Public Sub Fill(displayOption As Integer)
        Try
            Dim sql As String =
                "EXEC dbo.GetGiftCertificateList NULL," & displayOption
            Me.Data = ExecuteQuery(sql).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function Save() As Boolean
        Try
            Dim sql As String = ""

            For Each row As DataRow In Me.Data.Rows
                With row
                    If .RowState = DataRowState.Added Then
                        sql = sql &
                            "EXEC dbo.SaveGiftCertificate" & vbCrLf &
                            "   " & .Item("id").ToString.Quote &
                            "   " & .Item("code").ToString.Quote &
                            "   " & .Item("description").ToString.Quote &
                            "   " & If(.Item("expiry_date").ToString = "", "NULL," & vbCrLf, .Item("expiry_date").ToString.Quote) &
                            "   " & .Item("amount").ToString.Quote &
                            "   " & .Item("is_active").ToString.Quote &
                            "   " & .Item("is_released").ToString.Quote &
                            "   " & .Item("is_used").ToString.Quote &
                            "   " & UserName.Quote &
                            "   " & "'INSERT';"
                    ElseIf .RowState = DataRowState.Modified Then
                        sql = sql &
                            "EXEC dbo.SaveGiftCertificate" & vbCrLf &
                            "   " & .Item("id").ToString.Quote &
                            "   " & .Item("code").ToString.Quote &
                            "   " & .Item("description").ToString.Quote &
                            "   " & If(.Item("expiry_date").ToString = "", "NULL," & vbCrLf, .Item("expiry_date").ToString.Quote) &
                            "   " & .Item("amount").ToString.Quote &
                            "   " & .Item("is_active").ToString.Quote &
                            "   " & .Item("is_released").ToString.Quote &
                            "   " & .Item("is_used").ToString.Quote &
                            "   " & UserName.Quote &
                            "   " & "'UPDATE';"
                    ElseIf .RowState = DataRowState.Deleted Then
                        sql = sql &
                            "EXEC dbo.SaveGiftCertificate" & vbCrLf &
                            "   " & .Item("id", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("code", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("description", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("expiry_date", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("amount", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("is_active", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("is_released", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("is_used", DataRowVersion.Original).ToString.Quote &
                            "   " & UserName.Quote &
                            "   " & "'DELETE';"
                    End If
                End With
            Next

            Dim result As Boolean
            If sql <> "" Then
                result = ExecuteNonQuery(sql)
            End If
            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
