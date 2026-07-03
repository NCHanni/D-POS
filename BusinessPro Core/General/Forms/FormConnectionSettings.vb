Public Class FormConnectionSettings

#Region "Variables"

    Public Const LOCAL_DATASOURCE As String = "LocalDatasource"
    Public Const LOCAL_DATABASE As String = "LocalDatabase"
    Public Const LOCAL_USERNAME As String = "LocalUsername"
    Public Const LOCAL_PASSWORD As String = "LocalPassword"

    Private m_ConnectionError As String = ""
    Private m_ConnectionString As String = ""
    Private m_fWait As BusinessPro.Core.Wait

    Event OtherSettingClick As EventHandler(Of EventArgs)

#End Region

#Region "Constructor"

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Registry.CompanyName = Application.CompanyName
        Registry.ProductName = Application.ProductName
    End Sub

#End Region

#Region "Properties"

    Shared Property DefaultDatasource As String
    Shared Property DefaultDatabase As String
    Shared Property DefaultUsername As String
    Shared Property DefaultPassword As String

    ReadOnly Property ConnectionString As DataSource.ConnectionString
        Get
            Return New DataSource.ConnectionString With {.LocalConnectionString = m_ConnectionString}
        End Get
    End Property

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> Windows.Forms.DialogResult.OK Then
                DialogResult = Windows.Forms.DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.H AndAlso (e.Control OrElse e.Alt) Then
            txtDatabaseName.ReadOnly = Not txtDatabaseName.ReadOnly
            txtDatabaseName.Enabled = Not txtDatabaseName.ReadOnly
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            LoadSettings()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnOtherSetting_Click(sender As Object, e As EventArgs)
        RaiseEvent OtherSettingClick(Me, e)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            If btnCancel.Text = "Close" Then
                Close()
            Else
                LoadSettings()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnTestSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestSave.Click
        Dim cacheConnectionString = m_ConnectionString
        Try

            lblTestStatus.Visible = False
            grpDataSource.Enabled = False

            If sender.Text.Contains("Test") Then

                If grpDataSource.HasErrors("Test Failed") Then
                    GoTo _ExitSub
                End If

                m_ConnectionString = BusinessPro.Database.SQLStringBuilder.Create(
                    txtDataSource.Text,
                    txtDatabaseName.Text,
                    txtUserName.Text,
                    txtPassword.Text)

                m_fWait = New BusinessPro.Core.Wait(New Threading.Thread(AddressOf TestConnection)) With {
                    .Text = "Connecting to Data Source..."
                }

                If m_fWait.Show(Me) <> Windows.Forms.DialogResult.OK Then
                    m_ConnectionString = cacheConnectionString
                    GoTo _ExitSub
                ElseIf m_ConnectionError <> "" Then
                    m_fWait.Hide()
                    m_ConnectionString = cacheConnectionString
                    ShowMessage(m_ConnectionError, "Connection Failed", MessageIcon.Exclamation)
                    GoTo _ExitSub
                Else
                    lblTestStatus.Visible = True
                End If

                ShowMessage("Connection to Data Source Successful!", "Connection Successful")
                sender.Text = If(sender.Text.StartsWith("&"), "&", "") & "Save"

                GoTo _ExitSub
            Else
                If SaveSettings() Then
                    DialogResult = Windows.Forms.DialogResult.OK
                    Close()
                Else
                    m_ConnectionString = cacheConnectionString
                    DialogResult = DialogResult.None
                End If
            End If
        Catch ex As Exception
            m_ConnectionString = cacheConnectionString
            ex.ShowError()
        End Try
_ExitSub:
        grpDataSource.Enabled = True
    End Sub

#End Region

#Region "Textbox"

    Private Sub txtTextBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles txtDataSource.ValueChanged,
                txtDatabaseName.ValueChanged,
                txtUserName.ValueChanged,
                txtPassword.ValueChanged
        Try
            If lblTestStatus.Visible Then
                lblTestStatus.Visible = False
            End If
            If btnTestSave.Text.Contains("Save") Then
                btnTestSave.Text = IIf(btnTestSave.Text.StartsWith("&"), "&", "") & "Test"
            End If
            btnCancel.Text = "Cancel"
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub LoadSettings()
        Try
            txtDataSource.Text = CStrEx(Registry.ReadGlobalSetting(LOCAL_DATASOURCE, DefaultDatasource))
            txtDatabaseName.Text = CStrEx(Registry.ReadGlobalSetting(LOCAL_DATABASE, DefaultDatabase))
            txtUserName.Text = CStrEx(Registry.ReadGlobalSetting(LOCAL_USERNAME, DefaultUsername))
            txtPassword.Text = CStrEx(Registry.ReadGlobalSetting(LOCAL_PASSWORD, DefaultPassword)).Decrypt()

            m_ConnectionString = BusinessPro.Database.SQLStringBuilder.Create(
                txtDataSource.Text,
                txtDatabaseName.Text,
                txtUserName.Text,
                txtPassword.Text)

            lblTestStatus.Visible = False
            btnTestSave.Text = "Test"
            btnCancel.Text = "Close"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function CStrEx(str As String) As String
        If String.IsNullOrWhiteSpace(str) Then
            Return String.Empty
        Else
            Return str.Trim
        End If
    End Function

    Private Function SaveSettings() As Boolean
        Try
            If IsWindowsAdmin() Then
                Registry.WriteGlobalSetting(LOCAL_DATASOURCE, txtDataSource.Text)
                Registry.WriteGlobalSetting(LOCAL_DATABASE, txtDatabaseName.Text)
                Registry.WriteGlobalSetting(LOCAL_USERNAME, txtUserName.Text)
                Registry.WriteGlobalSetting(LOCAL_PASSWORD, txtPassword.Text.Encrypt)
                Return True
            Else
                Dim startInfo As New ProcessStartInfo With {
                    .Arguments =
                        "/admin " &
                        "/datasource=" & txtDataSource.Text.Replace(" ", "%20") & " " &
                        "/database=" & txtDatabaseName.Text.Replace(" ", "%20") & " " &
                        "/username=" & txtUserName.Text.Replace(" ", "%20") & " " &
                        "/password=" & txtPassword.Text.Encrypt & " " &
                        "/exit",
                    .CreateNoWindow = True,
                    .FileName = Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf("\") + 1),
                    .UseShellExecute = True,
                    .Verb = "runas",
                    .WindowStyle = ProcessWindowStyle.Hidden,
                    .WorkingDirectory = Application.StartupPath
                }
                Try
                    Process.Start(startInfo)
                    Return True
                Catch ex As Exception
                    ' ignore error when user cancels admin check
                    Return False
                End Try
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function TestConnection() As Boolean
        Try
            Dim sqlClientConnection As New SqlClient.SqlConnection(m_ConnectionString)

            sqlClientConnection.Open()

            If sqlClientConnection.State = ConnectionState.Open Then
                m_ConnectionError = ""
                Return True
            End If

            With sqlClientConnection
                .Close()
                .Dispose()
            End With
        Catch
            m_ConnectionError = "Connection to Data Source Failed!"
        End Try
    End Function

#End Region

#Region "Shared Methods"

    Public Shared Function HasSettings() As Boolean
        Try
            If Registry.ReadGlobalSetting(LOCAL_DATASOURCE, DefaultDatasource) = "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetSettings() As DataSource.ConnectionString
        Try
            Dim localDataSource As String = Registry.ReadGlobalSetting(LOCAL_DATASOURCE, DefaultDatasource)
            Dim localDatabase As String = Registry.ReadGlobalSetting(LOCAL_DATABASE, DefaultDatabase)
            Dim localUsername As String = Registry.ReadGlobalSetting(LOCAL_USERNAME, DefaultUsername)
            Dim localPassword As String = Registry.ReadGlobalSetting(LOCAL_PASSWORD, DefaultPassword).Decrypt

            Return New DataSource.ConnectionString With {
                    .LocalConnectionString = BusinessPro.Database.SQLStringBuilder.Create(
                        localDataSource,
                        localDatabase,
                        localUsername,
                        localPassword)
                }
        Catch
            Throw
        End Try
    End Function

#End Region

End Class