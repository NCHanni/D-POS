Public Class Rights
    Inherits DataSource

#Region "Declarations"

    Private m_IsSuperRole As Boolean

    Enum AccessRights
        Access = 0
        Add = 1
        Edit = 2
        Delete = 3
        View = 4
        Print = 5
    End Enum

#End Region

#Region "Properties"

    Property RoleCode As String
    Property Data As DataTable

    ReadOnly Property IsSuperRole As Boolean
        Get
            Return m_IsSuperRole
        End Get
    End Property

#End Region

#Region "SQL"

    Private Function FillSQL() As String
        Dim sql As String =
            "EXEC dbo.GetRole " & RoleCode.Quote(False)
        Return sql
    End Function

    Private Function IsGiftCertEnabledSQL() As String
        Dim sql As String =
            "SELECT TOP 1 1
            FROM dbo.[role_modules] rm
             JOIN dbo.[module] m ON m.id = rm.module_id
	            AND m.[name] = 'Gift Certificates'
              JOIN dbo.[role] r ON r.code = rm.role_code
	            AND r.is_super_role = 'False'"
        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub Fill()
        Try
            Dim ds As DataSet = ExecuteQuery(FillSQL)

            If CBool(ds.Tables(0).Rows(0)("is_super_role")) Then
                m_IsSuperRole = True
            Else
                Data = ds.Tables(1)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function IsGiftCertEnabled() As Boolean
        Try
            Return Exists(IsGiftCertEnabledSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsAllowed(moduleName As String, Optional moduleGroup As String = "")
        Return IsAllowed(moduleName, AccessRights.Access, moduleGroup)
    End Function

    Public Function IsAllowed(ByVal moduleName As String, ByVal accessRights As AccessRights, Optional ByVal moduleGroup As String = "") As Boolean
        Try
            If IsSuperRole Then
                Return True
            End If

            Dim query As EnumerableRowCollection(Of DataRow) = Nothing
            Dim row As DataRow

            If moduleGroup = "" Then
                query = From list In Data.AsEnumerable
                        Where list("module_name") = moduleName
                        Select list
            Else
                query = From list In Data.AsEnumerable
                        Where list("module_name") = moduleName _
                            AndAlso list("module_group") = moduleGroup
                        Select list
            End If

            Try
                row = query.CopyToDataTable.Rows(0)
            Catch ex As Exception
                Return False
            End Try

            Select Case accessRights
                Case AccessRights.Access : Return row("can_access")
                Case AccessRights.Add : Return row("can_add")
                Case AccessRights.Edit : Return row("can_edit")
                Case AccessRights.Delete : Return row("can_delete")
                Case AccessRights.View : Return row("can_view")
                Case AccessRights.Print : Return row("can_print")
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
