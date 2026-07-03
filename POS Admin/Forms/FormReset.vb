Friend Class FormReset

#Region "Event Handlers"

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape, Keys.Space, Keys.Enter
                Me.Close()
        End Select
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try
            lblResetCount.Text = "0"
            lblResetDate.Text = "None"
            lblResetAmount.Text = "0.00"
            lblCurrentAmount.Text = "0.00"

            Dim reset = New Reset
            Dim ds = reset.GetDetails()

            Dim t1 = ds.Tables(0)
            Dim t2 = ds.Tables(1)
            Dim t3 = ds.Tables(2)

            For Each row As DataRow In t1.Rows
                lblResetCount.Text = CDbl(row("reset_count")).ToString("#,##0.##")
            Next

            For Each row As DataRow In t2.Rows
                lblResetDate.Text = Convert.ToDateTime(row("date")).ToDateString()
                lblResetAmount.Text = CDbl(row("reset_amount")).ToString("#,##0.00")
            Next

            For Each row As DataRow In t3.Rows
                lblCurrentAmount.Text = CDbl(row("current_amount")).ToString("#,##0.00")
            Next
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

End Class