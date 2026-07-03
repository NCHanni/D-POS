Friend Class FormCashUpConfirm

#Region "Properties"

    Property CashEndAmount As Double
    Property UseBarcode As Boolean
    Property ShortcutKey As Keys

#End Region

#Region "Methods"

    Private Function IsAuthorized(ByVal password As String) As Boolean
        Try
            Dim userCode As String = Current.User.Code
            Dim cUsers As New UserList

            cUsers.FillByStatus(True)

            If lblPassword.Text = "Barcode" Then
                Return (cUsers.Data.Select("code = '" & userCode & "' AND barcode = '" & password.ToUpper & "'").Length > 0)
            Else
                Return (cUsers.Data.Select("code = '" & userCode & "' AND password = '" & Encrypt(password) & "'").Length > 0)
            End If

            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = ShortcutKey Then
                lblPassword_Click(lblPassword, New EventArgs)
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            lblAmount.Text = CashEndAmount.ToString("#,##0.00")
            txtName.Text = Current.User.Name

            If UseBarcode Then
                lblPassword.Text = "Barcode"
                txtPassword.PasswordChar = ""
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If IsAuthorized(txtPassword.Text.Trim) Then
                DialogResult = DialogResult.OK
            Else
                txtPassword.Focus()
                txtPassword.SelectAll()

                If lblPassword.Text = "Barcode" Then
                    ShowWarning("Invalid user barcode! Please try again.", "Finalize Failed")
                Else
                    ShowWarning("Incorrect user password! Please try again.", "Finalize Failed")
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub lblPassword_Click(sender As Object, e As EventArgs) Handles lblPassword.Click
        Try
            If lblPassword.Text = "Barcode" Then
                lblPassword.Text = "Password"
                txtPassword.PasswordChar = "●"
            Else
                lblPassword.Text = "Barcode"
                txtPassword.PasswordChar = ""
            End If
            txtPassword.Clear()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

End Class