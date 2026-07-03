Friend Class FormAuthorization

#Region "Constructors"

    Public Sub New(settings As SettingsPreferences, Optional withReason As Boolean = False, Optional superUser As Boolean = False)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
        m_RoleCode = If(superUser, "", settings.AuthorizedOfficerCode)
        m_WithReason = withReason
        m_SuperUser = superUser

        If withReason Then
            lblReason.Visible = withReason
            txtReason.Visible = withReason

            Me.Size = New Size(260, 210)
        Else
            Me.Size = New Size(260, 170)
        End If
    End Sub

#End Region

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences
    Private ReadOnly m_WithReason As Boolean
    Private ReadOnly m_RoleCode As String = ""
    Private ReadOnly m_SuperUser As Boolean

    Private m_UserData As DataTable

#End Region

#Region "Properties"

    Property AuthorizedBy As String
    Property AuthorizedRoleName As String

    Public Property Message As String
        Get
            Return lblMessage.Text
        End Get
        Set(ByVal value As String)
            lblMessage.Text = value
        End Set
    End Property

    Public ReadOnly Property Reason As String
        Get
            Return txtReason.Value
        End Get
    End Property

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel
        End If
        Me.Owner.Enabled = True
    End Sub

    Private Sub Form_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Me.MouseDoubleClick
        If m_RoleCode = "" AndAlso Not m_SuperUser Then
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Owner.Enabled = False
    End Sub

    Private Sub txtUsername_Leave(sender As Object, e As EventArgs) Handles txtUsername.Leave
        Try
            If m_Settings.UseBarcodeLogin AndAlso Not txtPassword.Enabled Then
                ' Current authorization has been modified
                If txtUsername.Text <> AuthorizedBy Then
                    AuthorizedBy = ""
                    AuthorizedRoleName = ""
                    txtPassword.Enabled = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If txtUsername.Text.Trim.Length = 0 Then
                txtUsername.Focus()
                Beep()
            Else
                If m_Settings.UseBarcodeLogin AndAlso (AuthorizedBy <> "" OrElse IsAdminBarcode(txtUsername.Text)) Then
                    txtUsername.Text = AuthorizedBy
                    txtPassword.Clear()
                    txtPassword.Enabled = False

                    If txtReason.Text.Trim.Length = 0 AndAlso m_WithReason Then
                        txtReason.Focus()
                    Else
                        DialogResult = DialogResult.OK
                    End If
                Else
                    If txtUsername.TextLength > 0 AndAlso txtPassword.TextLength = 0 Then
                        If txtPassword.Focused Then
                            Beep()
                        Else
                            txtPassword.Focus()
                        End If
                    ElseIf txtUsername.TextLength > 0 AndAlso txtPassword.TextLength > 0 AndAlso txtPassword.Focused AndAlso txtReason.Visible AndAlso txtReason.TextLength = 0 Then
                        txtReason.Focus()
                    ElseIf grpCredentials.HasErrors Then
                        txtReason.Focus()
                    ElseIf IsAuthorized(txtUsername.Text, txtPassword.Text) Then
                        DialogResult = DialogResult.OK
                    Else
                        txtPassword.Clear()
                        txtUsername.Focus()
                        txtUsername.SelectAll()
                        ShowMessage("You are not allowed to authorize this transaction!", "Authorization Failed", MessageIcon.Exclamation)
                    End If
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

#End Region

#Region "Methods"

    Private Sub GetUserData()
        Try
            If m_UserData Is Nothing Then
                Dim cUsers As New UserList

                cUsers.FillByStatus(True)

                m_UserData = cUsers.Data
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsAdminBarcode(text As String) As Boolean
        Try
            GetUserData()

            Dim dr As DataRow() =
                m_UserData.Select("barcode = '" & text.Trim.ToUpper & "'")

            If dr.Length > 0 Then
                If dr(0)("role_code") = m_RoleCode OrElse (m_RoleCode = "" AndAlso dr(0)("is_super_role")) Then
                    AuthorizedBy = dr(0).Item("name")
                    AuthorizedRoleName = dr(0).Item("role_name")

                    Return True
                End If
            End If

            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsAuthorized(ByVal username As String, ByVal password As String) As Boolean
        Try
            GetUserData()

            Dim dr As DataRow() =
                m_UserData.Select("username = '" & username & "' and password = '" & Encrypt(password) & "'")

            If dr.Length > 0 Then
                If dr(0)("role_code") = m_RoleCode OrElse (m_RoleCode = "" AndAlso dr(0)("is_super_role")) Then
                    AuthorizedBy = dr(0).Item("name")
                    AuthorizedRoleName = dr(0).Item("role_name")

                    Return True
                End If
            End If

            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class