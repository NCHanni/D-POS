Friend Class FormCashIn

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtAmount.Height = 34
        btnOk.Enabled = False
    End Sub

    Private Sub txtAmount_ValueChanged(sender As Object, e As EventArgs) Handles txtAmount.ValueChanged
        If IsDBNull(txtAmount.Value) Then
            btnOk.Enabled = False
        Else
            btnOk.Enabled = txtAmount.Value > 0.0 AndAlso txtAmount.Value <= 999999.99
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

End Class