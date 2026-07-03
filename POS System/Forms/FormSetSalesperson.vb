Friend Class FormSetSalesperson

#Region "Property"

    Property SalespersonCode As String
    Property SalespersonName As String
    Property SalesPersonList As Salesperson.Struct()

#End Region

#Region "Event Handlers"

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If SalespersonCode = "" OrElse e.Modifiers <> Keys.None Then
                    FindRecord(txtCode.Text.Trim)
                ElseIf SalespersonCode <> "" Then
                    btnAssign.PerformClick()
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub txtCode_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles txtCode.EditorButtonClick
        Try
            FindRecord(txtCode.Text.Trim)
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub txtCode_TextChanged(sender As Object, e As EventArgs) Handles txtCode.TextChanged
        If lblName.Text <> "" Then
            SalespersonCode = ""
            SalespersonName = ""
            lblName.Text = ""
            btnAssign.Enabled = False
        End If
    End Sub

    Private Sub btnAssign_Click(sender As Object, e As EventArgs) Handles btnAssign.Click
        Try
            DialogResult = DialogResult.OK
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            lblName.Text = SalespersonName
            btnAssign.Enabled = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function FindRecord(text As String) As Boolean
        Try
            If String.IsNullOrEmpty(text) Then
                lblName.Text = "---"
                Return False
            End If

            Dim drResult As List(Of Salesperson.Struct)

            drResult = SalesPersonList.Where(Function(e) String.Compare(e.Code, text, True) = 0 OrElse String.Compare(e.Barcode, text, True) = 0).ToList

            txtCode.Focus()
            btnAssign.Enabled = False
            SalespersonCode = ""
            SalespersonName = ""

            If txtCode.Focused Then
                txtCode.SelectAll()
            End If

            If drResult Is Nothing OrElse drResult.Count = 0 Then
                If text = "" Then
                    lblName.Text = "---"
                Else
                    Beep()
                    lblName.Text = "No records found!"
                End If
            Else
                If drResult.Count = 1 Then
                    SalespersonCode = drResult.First.Code
                    SalespersonName = drResult.First.Name
                    lblName.Text = SalespersonName
                    btnAssign.Enabled = True
                    Return True
                Else
                    lblName.Text = "Multiple records found!"
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class