Friend Class ucSearchText

    Private Sub SearchText_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim g As Graphics = e.Graphics
        Dim pn As New Pen(Me.ForeColor)
        'g.DrawEllipse(pn, New Rectangle(0, 0, Me.Width, Me.Height))
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.DrawArc(pn, 0, 0, 35, Me.Height, 90, 180)
        g.DrawArc(pn, Me.Width - 37, 0, 35, Me.Height, 270, 180)
        g.DrawLine(pn, 14, 0, Me.Width - 15, 0)
        g.DrawLine(pn, 14, Me.Height - 1, Me.Width - 15, Me.Height - 1)
    End Sub

End Class
