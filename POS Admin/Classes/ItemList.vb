Friend Class ItemList
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL(showInactive As Boolean) As String
        Dim sql As String =
            "EXEC dbo.GetItemsBasic " & showInactive.Quote(False)
        Return sql
    End Function

    Private Function IsReferencedSQL(ByVal code As String) As String
        Try
            Dim sql As String =
                "SELECT * FROM dbo.GetItemReferences(" & code.Quote(False) & ")"
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsItemExistingSQL(ByVal codename As String, ByVal code As String) As String
        Try
            Dim sql As String

            If code = "" Then
                sql = "SELECT code FROM [item] WHERE codename = " & codename.Quote(False)
            Else
                sql = "SELECT code FROM [item] WHERE codename = " & codename.Quote(False) & " AND code <> " & code.Quote(False)
            End If

            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Methods"

    Public Sub Fill(showInactive As Boolean)
        Try
            Data = ExecuteQuery(FillSQL(showInactive)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function IsReferenced(ByVal code As String) As Boolean
        Try
            Return Exists(IsReferencedSQL(code))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsItemExisting(ByVal codename As String, ByVal code As String) As Boolean
        Try
            Return Exists(IsItemExistingSQL(codename, code))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
