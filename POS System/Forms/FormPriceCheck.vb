Friend Class FormPriceCheck

#Region "Constructors"

    Public Sub New(settings As SettingsPreferences)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
        Core.Current.FormatUI.Apply(Me)
    End Sub

#End Region

#Region "Declarations"

    Private m_Settings As SettingsPreferences

#End Region

#Region "Event Handlers"

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.F11
                    Static normalWindowState As FormWindowState
                    If Me.FormBorderStyle = FormBorderStyle.None Then
                        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
                        Me.WindowState = normalWindowState
                    Else
                        normalWindowState = Me.WindowState
                        Me.SuspendLayout()
                        Me.ResizeRedraw = False
                        Me.WindowState = FormWindowState.Normal
                        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                        Me.WindowState = FormWindowState.Maximized
                        Me.ResizeRedraw = True
                        Me.ResumeLayout()
                    End If
            End Select
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            pnlTitle.BackColor = Color.FromArgb(27, 117, 186)

            lblDescription.Text = My.Application.Info.Description
            lblCompany.Text = My.Application.Info.CompanyName & " © " & Today.Year
            lblVersion.Text = String.Format("v{0} Build {1}", Application.ProductVersion, My.Application.Info.Version.Build)

            With sbarMain
                .Panels("Company").Text = "Company: " & Current.CompanyInfo.Name
                .Panels("Status").Appearance.ForeColor = Color.Blue

                Dim conString As String = Core.Current.ConnectionString.LocalConnectionString

                If conString.StartsWith("Data Source=.\") OrElse conString.StartsWith("Data Source=" & My.Computer.Name) Then
                    .Panels("Status").Text = "Local Connection"
                Else
                    .Panels("Status").Text = "Online via LAN"
                End If

                .Panels("Status").Text &= " @ " & conString.Substring(12, conString.IndexOf(";") - 12)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class