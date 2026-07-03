Public Class UserList
    Inherits DataSource

#Region "Properties"

    Property Code As String = ""
    Property Data As DataTable

#End Region

#Region "SQL"

    Private Function FillSQL()
        Dim sql As String =
            "EXEC dbo.GetUserDetails " & Code.Quote(False)
        Return sql
    End Function

    Private Function FillByStatusSQL(ByVal isActive As Boolean) As String
        Try
            Dim sql As String =
                FillSQL() & ", " & isActive.ToString.Quote(False)
            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsUsernameExistingSQL(ByVal username As String, ByVal code As String) As String
        Dim sql As String

        If Not code = "" Then
            sql = "SELECT code FROM dbo.vwUserBasic WHERE username = " & username.Quote(False) & " AND code <> " & code.Quote(False)
        Else
            sql = "SELECT code FROM dbo.vwUserBasic WHERE username = " & username.Quote(False)
        End If

        Return sql
    End Function

    Private Function IsBarcodeExistingSQL(ByVal barcode As String, ByVal code As String) As String
        Dim sql As String

        If Not code = "" Then
            sql = "SELECT code FROM dbo.vwUserBasic WHERE barcode = " & barcode.Quote(False) & " AND code <> " & code.Quote(False)
        Else
            sql = "SELECT code FROM dbo.vwUserBasic WHERE barcode = " & barcode.Quote(False)
        End If

        Return sql
    End Function

#End Region

#Region "Methods"

    Public Sub Fill()
        Try
            Data = ExecuteQuery(FillSQL).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub FillByStatus(ByVal activeState As Boolean)
        Try
            Data = ExecuteQuery(FillByStatusSQL(activeState)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function IsUsernameExisting(ByVal username As String, ByVal code As String) As Boolean
        Try
            Return Exists(IsUsernameExistingSQL(username, code))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsBarcodeExisting(ByVal barcode As String, ByVal code As String) As Boolean
        Try
            Return Exists(IsBarcodeExistingSQL(barcode, code))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
