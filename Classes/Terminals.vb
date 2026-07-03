Friend Class Terminals
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable
    Property UserName As String

#End Region

#Region "SQL"

    Private Function IsUsedInPOSSQL(ByVal id As String) As String
        Try
            Dim sql As String =
                "SELECT COUNT(t.id) " & vbCrLf &
                "FROM [terminal] t " & vbCrLf &
                "JOIN [cash_up] c ON c.terminal_code = t.code" & vbCrLf &
                "WHERE t.id = '" & id & "'"
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Methods"

    Public Sub Fill()
        Try
            Dim sql As String =
                "SELECT * FROM dbo.vwTerminals"
            Me.Data = ExecuteQuery(sql).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function Save() As Boolean
        Try
            Dim sql As String = ""

            For i As Integer = 0 To Me.Data.Rows.Count - 1
                With Me.Data.Rows(i)
                    If .RowState = DataRowState.Added Then
                        sql = sql &
                            "EXEC dbo.SaveTerminal" & vbCrLf &
                            "   " & .Item("id").ToString.Quote &
                            "   " & .Item("code").ToString.Quote &
                            "   " & .Item("machine_no").ToString.Quote &
                            "   " & .Item("serial_no").ToString.Quote &
                            "   " & .Item("mac_address").ToString.Quote &
                            "   " & .Item("computer_name").ToString.Quote &
                            "   " & .Item("is_active").ToString.Quote &
                            "   " & UserName.Quote &
                            "   " & "'INSERT';"
                    ElseIf .RowState = DataRowState.Modified Then
                        sql = sql &
                            "EXEC dbo.SaveTerminal" & vbCrLf &
                            "   " & .Item("id").ToString.Quote &
                            "   " & .Item("code").ToString.Quote &
                            "   " & .Item("machine_no").ToString.Quote &
                            "   " & .Item("serial_no").ToString.Quote &
                            "   " & .Item("mac_address").ToString.Quote &
                            "   " & .Item("computer_name").ToString.Quote &
                            "   " & .Item("is_active").ToString.Quote &
                            "   " & UserName.Quote &
                            "   " & "'UPDATE';"
                    ElseIf .RowState = DataRowState.Deleted Then
                        sql = sql &
                            "EXEC dbo.SaveTerminal" & vbCrLf &
                            "   " & .Item("id", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("code", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("machine_no", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("serial_no", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("mac_address", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("computer_name", DataRowVersion.Original).ToString.Quote &
                            "   " & .Item("is_active", DataRowVersion.Original).ToString.Quote &
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

    Public Function IsUsedInPOS(ByVal code As String) As Boolean
        Try
            If CInt(ExecuteQuery(IsUsedInPOSSQL(code)).Tables(0).Rows(0).Item(0)) > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
