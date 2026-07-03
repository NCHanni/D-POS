Public Class RoleList
    Inherits DataSource

#Region "Properties"

    Private m_List As DataTable
    Public ReadOnly Property List() As DataTable
        Get
            Return m_List
        End Get
    End Property

    Private m_ModuleList As DataSet
    Public ReadOnly Property ModuleList() As DataSet
        Get
            Return m_ModuleList
        End Get
    End Property

#End Region

#Region "SQL"

    Private Function FillSQL(hideSuperRole As Boolean) As String
        Dim sql As String =
            "SELECT" & vbCrLf &
            "  code," & vbCrLf &
            "  [name]," & vbCrLf &
            "  [description]," & vbCrLf &
            "  is_super_role" & vbCrLf &
            "FROM" & vbCrLf &
            "  dbo.vwRoles" & vbCrLf &
            If(hideSuperRole, "WHERE is_super_role = 'False'", "") &
            "ORDER BY [name]"
        Return sql
    End Function

    Private Function FillDetailsSQL(ByVal roleCode As String) As String
        Dim sql As String =
            "EXEC dbo.GetRoleModules " & roleCode.Quote(False)
        Return sql
    End Function

    Private Function IsUserRoleExistingSQL(ByVal roleName As String, ByVal code As String) As String
        Try
            Dim sql As String =
                "SELECT" & vbCrLf &
                "  code" & vbCrLf &
                "FROM" & vbCrLf &
                "  dbo.vwRoles" & vbCrLf &
                "WHERE" & vbCrLf &
                "  [name] = " & roleName.Quote(False) &
                "  AND code <> " & code.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsReferencedSQL(ByVal code As String) As String
        Dim sql As String =
            "SELECT dbo.IsRoleReferenced(" & code.Quote(False) & ")"
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub Fill(hideSuperRole As Boolean)
        Try
            m_List = ExecuteQuery(FillSQL(hideSuperRole)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub FillDetails(ByVal roleCode As String)
        Try
            m_ModuleList = ExecuteQuery(FillDetailsSQL(roleCode))
            With m_ModuleList
                .Relations.Add(.Tables(0).Columns("group"), .Tables(1).Columns("group"))
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function IsUserRoleExisting(ByVal name As String, ByVal code As String) As Boolean
        Try
            Return Exists(IsUserRoleExistingSQL(name, code))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsReferenced(ByVal code As String) As Boolean
        Try
            Return CBool(ExecuteQuery(IsReferencedSQL(code)).Tables(0).Rows(0).Item(0))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
