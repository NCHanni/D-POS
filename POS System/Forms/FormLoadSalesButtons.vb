Friend Class FormLoadSalesButtons

#Region "Declarations"

    Private m_AllowRefundSales As Boolean

    Public Enum Buttons
        None
        PostedSales
        RefundSales
        SuspendedSales
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(allowRefundSales As Boolean)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With Me
            If Not allowRefundSales Then
                .Size = New Size(Me.Width, Me.Height - btnRefundSale.Height - 6)
                btnRefundSale.Visible = False
            End If

            .MaximumSize = .Size
            .MinimumSize = .Size
        End With

        m_AllowRefundSales = allowRefundSales
    End Sub

#End Region

#Region "Properties"

    Private m_Selected As Buttons = Buttons.None

    Public ReadOnly Property Selected() As Buttons
        Get
            Return m_Selected
        End Get
    End Property

    Property AllowPostedSales As Boolean
    Property AllowSuspendedSales As Boolean
    Property ShortcutControl As Boolean
    Property ShortcutKey As Keys

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                DialogResult = DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.Escape OrElse (e.Control = Me.ShortcutControl AndAlso e.KeyCode = Me.ShortcutKey) Then
                Close()
            Else
                Select Case e.KeyCode
                    Case Keys.P
                        btnPostedSales.PerformClick()
                    Case Keys.R
                        btnRefundSale.PerformClick()
                    Case Keys.S
                        btnSuspendedSale.PerformClick()
                    Case Else
                        Return
                End Select
                e.Handled = True
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            btnPostedSales.Enabled = AllowPostedSales
            btnRefundSale.Enabled = m_AllowRefundSales
            btnSuspendedSale.Enabled = AllowSuspendedSales
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnLoadButtons_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnPostedSales.Click,
                btnRefundSale.Click,
                btnSuspendedSale.Click
        Try
            ' We can't hide form using the existing HIDE method
            Opacity = 0 ' so...we'll hide it the other way  :)

            If sender Is btnPostedSales Then
                m_Selected = Buttons.PostedSales

            ElseIf sender Is btnRefundSale Then
                m_Selected = Buttons.RefundSales

            ElseIf sender Is btnSuspendedSale Then
                m_Selected = Buttons.SuspendedSales
            End If

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

End Class