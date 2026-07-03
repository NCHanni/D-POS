Public Class FormSerialNumber
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub FormSerialNumber_FormClosing(sender As Object, e As FormClosingEventArgs)
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub FormSerialNumber_Load(sender As Object, e As EventArgs)
        txtSerialNbr.Height = 34
        btnOk.Enabled = False
    End Sub

    Private Sub txtSerialNbr_ValueChanged(sender As Object, e As EventArgs) Handles txtSerialNbr.ValueChanged
        btnOk.Enabled = Not String.IsNullOrEmpty(txtSerialNbr.Value)
    End Sub
End Class