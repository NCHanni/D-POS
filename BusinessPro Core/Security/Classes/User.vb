Public Class User
    Inherits DataSource

#Region "Properties"

    Property RoleCode As String
    Property Code As String
    Property Name As String
    Property UserName As String
    Property Password As String
    Property Barcode As String
    Property IsNavAccount As Boolean
    Property IsActive As Boolean
    Property Operation As String

    Private ReadOnly Property SecureHashSource As String
        Get
            Return RoleCode & Code & Name & UserName & Barcode
        End Get
    End Property

    ReadOnly Property SecureHash As String
        Get
            Return SecureHashSource.Encrypt
        End Get
    End Property

#End Region

#Region "SQL"

    Private Function SaveUserSQL() As String
        Dim sql As String =
            "EXEC dbo.SaveUser " &
                RoleCode.Quote &
                Code.Quote &
                Name.Quote &
                UserName.Quote &
                Password.Quote &
                Barcode.Quote &
                IsNavAccount.Quote &
                IsActive.Quote &
                SecureHash.Quote &
                Operation.Quote(False)
        Return sql
    End Function

#End Region

#Region "Methods"

    Private Sub GetNextUserCode()
        Try
            GetNextCode("U-", "[user]", 3, Code)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function SaveUser() As Boolean
        Try
            If Operation = "INSERT" Then
                GetNextUserCode()
            End If

            Return ExecuteNonQuery(SaveUserSQL)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
