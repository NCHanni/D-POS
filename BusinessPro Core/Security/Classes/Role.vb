Friend Class Role
    Inherits RoleModules

#Region "Declarations"

    Private m_RoleOperation As String

#End Region

#Region "Properties"

    Property Code As String
    Property Name As String
    Property Description As String
    Overloads Property Operation As String
    Property SecurityModules As DataSet

#End Region

#Region "SQL"

    Private Function SaveRoleSQL() As String
        Dim sql As String =
            "EXECUTE dbo.SaveRole " &
                Code.Quote &
                Name.Quote &
                Description.Quote &
                Operation.Quote(False) & "; " & vbCrLf
        Return sql
    End Function

#End Region

#Region "Methods"

    Private Sub GetNextRoleCode()
        Try
            GetNextCode("R-", "[role]", 3, Code)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function SaveRoleModule() As String
        Try
            Dim SQL As String = ""
            For Each row As DataRow In SecurityModules.Tables(1).Rows
                ID = row("id")
                RoleCode = Code
                ModuleID = row("module_id")
                CanAccess = row("can_access")
                CanAdd = If(row("can_add") = "", False, row("can_add"))
                CanEdit = If(row("can_edit") = "", False, row("can_edit"))
                CanDelete = If(row("can_delete") = "", False, row("can_delete"))
                CanView = row("can_view")
                CanPrint = If(row("can_print") = "", False, row("can_print"))
                MyBase.Operation = m_RoleOperation

                SQL &= SaveRoleModules()
            Next
            Return SQL
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function SaveRole() As Boolean
        Try
            Dim sql As String

            m_RoleOperation = Operation

            If Operation = "INSERT" Then
                GetNextRoleCode()
            End If

            If Operation = "DELETE" Then
                sql = SaveRoleModule()
                sql &= SaveRoleSQL()
            Else
                sql = SaveRoleSQL()
                sql &= SaveRoleModule()
            End If

            sql = CreateTryCatchBlockSQL(sql)

            Return ExecuteNonQuery(sql)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
