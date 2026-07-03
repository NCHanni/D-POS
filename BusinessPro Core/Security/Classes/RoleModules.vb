Friend Class RoleModules
    Inherits DataSource

#Region "Properties"

    Property ID As Long
    Property RoleCode As String
    Property ModuleID As Long
    Property CanAccess As Boolean
    Property CanAdd As Boolean
    Property CanEdit As Boolean
    Property CanDelete As Boolean
    Property CanView As Boolean
    Property CanPrint As Boolean
    Property Operation As String

#End Region

#Region "SQL"

    Private Function SaveRoleModulesSQL() As String
        Dim SQL As String =
            "EXECUTE dbo.SaveRoleModules " &
                ID.Quote &
                RoleCode.Quote &
                ModuleID.Quote &
                CanAccess.Quote &
                CanAdd.Quote &
                CanEdit.Quote &
                CanDelete.Quote &
                CanView.Quote &
                CanPrint.Quote &
                Operation.Quote(False) & "; " & vbCrLf
        Return SQL
    End Function

#End Region

#Region "Methods"

    Public Function SaveRoleModules() As String
        Try
            Return SaveRoleModulesSQL()
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
