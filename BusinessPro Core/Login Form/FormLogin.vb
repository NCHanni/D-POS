Imports System.Windows.Forms
Imports System.Drawing

Friend Class FormLogin

#Region "Declarations"

    Private WithEvents m_Flasher As Timer
    Private WithEvents m_Timer As Timer

    Private m_Initialized As Boolean = False
    Private m_IsBusy As Boolean = False
    Private m_IsDone As Boolean = False ' validating...
    Private m_IsValid As Boolean = False
    Private m_CloseForm As Boolean = False
    Private m_IsXP As Boolean = False
    Private m_Barcode As String = ""

    Private WithEvents m_frmSettingsLocal As FormConnectionSettings

#End Region

#Region "Events"

    Public Event ValidateLogin(ByVal Username As String, ByVal Password As String, ByRef IsValid As Boolean)
    Public Event ValidateBarcode(ByVal Barcode As String, ByRef IsValid As Boolean)
    Public Event Cancelled(ByRef CloseForm As Boolean)

#End Region

#Region "Properties"

    Property ApplicationBuildNo As Integer

    Public Property ApplicationCaption() As String
        Get
            Return lblApplicationCaption.Text
        End Get
        Set(ByVal value As String)
            If Not m_Initialized Then
                lblApplicationCaption.Tag = value
                m_Initialized = True
            End If

            lblApplicationCaption.Text = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return txtUsername.Text.Trim
        End Get
        Set(ByVal value As String)
            txtUsername.Text = value.Trim
            chkRememberMe.Checked = (txtUsername.Text <> "")
        End Set
    End Property

    Public ReadOnly Property Password() As String
        Get
            Return txtPassword.Text.Trim
        End Get
    End Property

    Private m_PasswordRequired As Boolean = True
    Public Property PasswordRequired() As Boolean
        Get
            Return m_PasswordRequired
        End Get
        Set(ByVal value As Boolean)
            m_PasswordRequired = value
        End Set
    End Property

    Public ReadOnly Property Barcode As String
        Get
            Return m_Barcode
        End Get
    End Property

    Public ReadOnly Property RememberUser() As Boolean
        Get
            Return chkRememberMe.Checked
        End Get
    End Property

    Public Property DisplayMessage() As String
        Get
            Return lblMessage.Text
        End Get
        Set(ByVal value As String)
            lblMessage.Text = value
        End Set
    End Property

    Public ReadOnly Property DefaultMessage() As String = "*Either the specified username or password is invalid."

    Private m_UseRememberMe As Boolean
    Public Property UseRememberMe() As Boolean
        Get
            Return m_UseRememberMe
        End Get
        Set(ByVal value As Boolean)
            m_UseRememberMe = value
            chkRememberMe.Visible = value
        End Set
    End Property

    Property ShowBarcodeLogin As Boolean
    Overloads Property CompanyName As String = ""
    Property TerminalCode As String = ""
    Property SerialNo As String = ""

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        FocusAndSelect(txtUsername)
        If Not m_IsBusy Then
            txtPassword.Clear()
            lblMessage.Visible = False
        End If
    End Sub

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult <> Windows.Forms.DialogResult.OK Then
            DialogResult = Windows.Forms.DialogResult.Cancel
        End If
        If m_IsXP Then
            HideFadeForm(Me)
        End If
    End Sub

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Down
                If txtUsername.Focused Then
                    FocusAndSelect(txtPassword)
                End If
            Case Keys.Up
                If txtPassword.Focused Then
                    FocusAndSelect(txtUsername)
                End If
            Case Keys.Back, Keys.Delete
                If pnlBarcodeLogin.Visible Then
                    lblMessage.Visible = False
                    m_Barcode = ""
                End If
            Case Else
                If pnlBarcodeLogin.Visible Then
                    Dim character As Char
                    Dim isAccepted As Boolean = True
                    Select Case e.KeyCode
                        Case Keys.A To Keys.Z
                        Case Keys.D0 To Keys.D9
                        Case Keys.NumPad0 To Keys.NumPad9
                            character = Chr(e.KeyValue - Keys.D0)
                        Case Keys.OemMinus, Keys.Subtract
                            character = "-"
                        Case Keys.Space
                        Case Else
                            isAccepted = False
                    End Select
                    If isAccepted Then
                        lblMessage.Visible = False
                        m_Barcode &= If(character = vbNullChar, Chr(e.KeyValue), character)
                    End If
                End If
        End Select
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If m_IsXP Then
            Refresh()
            Visible = False
            ShowFadeForm(Me)
        End If

        If pnlBarcodeLogin.Visible Then
            m_Flasher = New Timer() With {
                .Interval = 400
            }
            m_Flasher.Start()
        End If

        If txtUsername.Text <> "" Then
            txtPassword.Focus()
        End If
    End Sub

#Region "Move form without title bar"

    Private m_MouseDown As Boolean = False
    Private m_MouseX As Integer
    Private m_MouseY As Integer
    Private Sub Form_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            m_MouseDown = True
            m_MouseX = e.X
            m_MouseY = e.Y
        End If
    End Sub

    Private Sub Form_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If m_MouseDown Then
            Left = Left + e.X - m_MouseX
            Top = Top + e.Y - m_MouseY
        End If
    End Sub

    Private Sub Form_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        m_MouseDown = False
    End Sub

    Private Sub Label_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Form_MouseDown(sender, e)
    End Sub

    Private Sub Label_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Form_MouseMove(sender, e)
    End Sub

    Private Sub Label_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Form_MouseUp(sender, e)
    End Sub

#End Region

#End Region

#Region "Timer"

    Private Sub m_Flasher_Tick(sender As Object, e As EventArgs) Handles m_Flasher.Tick
        lblScanBarcode.Visible = Not lblScanBarcode.Visible
    End Sub

    Private Sub m_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Timer.Tick
        If m_CloseForm Then
            Close()
            Return
        End If

        If Not m_IsDone Then ' Is done validating?
            m_Timer.Interval = 1000 ' If not, wait another second and so on...
            Return
        Else
            m_Timer.Stop()
            Application.UseWaitCursor = False
        End If

        picLoader.Image = Nothing
        picLoader.Visible = False

        m_IsBusy = False

        If m_IsValid Then
            If pnlBarcodeLogin.Visible Then
                lblScanBarcode.Text = "BARCODE ACCEPTED"
            End If
            DialogResult = Windows.Forms.DialogResult.OK
            Close()
        Else
            lblMessage.Visible = True

            If pnlBarcodeLogin.Visible Then
                lblScanBarcode.Text = "SCAN BARCODE"
                m_Barcode = ""
            Else
                txtUsername.Enabled = True
                txtPassword.Enabled = True
                chkRememberMe.Enabled = True

                txtPassword.Clear()
                FocusAndSelect(txtUsername)
            End If
        End If
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Try
            DoLogin()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If pnlBarcodeLogin.Visible Then
            m_Flasher.Stop()
            pnlBarcodeLogin.Visible = False
            txtUsername.Enabled = True
            txtUsername.Focus()
            txtUsername.SelectAll()
            txtPassword.Enabled = True
            lblMessage.Visible = False
            DialogResult = DialogResult.None
        Else
            If m_Timer Is Nothing Then
                m_Timer = New Timer
            End If
            m_CloseForm = True
            m_Timer.Interval = 250
            m_Timer.Start()
        End If
    End Sub

#End Region

#Region "Others"

    Private Sub lblApplicationCaption_Click(sender As Object, e As EventArgs) _
        Handles lblApplicationCaption.Click,
                lblTerminalDetails.Click
        If ApplicationBuildNo > 0 Then
            With lblApplicationCaption
                .Text = If(.Text = .Tag, .Tag.Replace("Point-of-Sale System", "POS") & " Build " & ApplicationBuildNo.ToString, .Tag)
            End With
        End If
        If Not String.IsNullOrWhiteSpace(TerminalCode) Then
            With lblTerminalDetails
                .Text = If(.Text = .Tag, "Terminal: " & TerminalCode, .Tag)
            End With
        End If
    End Sub

    Private Sub Textbox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyDown, txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Escape Then
            e.Handled = True
            e.SuppressKeyPress = True ' Prevent beep
        End If

        If lblMessage.Visible Then
            lblMessage.Visible = False
        End If
    End Sub

    Private Sub picStatus_Click(sender As Object, e As EventArgs) Handles picStatus.Click
        Try
            m_frmSettingsLocal = New FormConnectionSettings With {
                .MinimizeBox = False,
                .ShowInTaskbar = False
            }
            LaunchForm(m_frmSettingsLocal, True, Me)
            Current.ConnectionString = m_frmSettingsLocal.ConnectionString
            m_frmSettingsLocal.Dispose()
            m_frmSettingsLocal = Nothing
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub picStatus_MouseEnter(sender As Object, e As EventArgs) Handles picStatus.MouseEnter
        With lblStatus
            .Appearance.ForeColor = Color.Silver
            .Text = GetSetting("LocalDatabase") & "@" & GetSetting("LocalDatasource")
        End With
    End Sub

    Private Sub picStatus_MouseLeave(sender As Object, e As EventArgs) Handles picStatus.MouseLeave
        With lblStatus
            .Appearance.ForeColor = Color.White
            .Text = .Tag
        End With
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            txtUsername.BackColor = Color.White
            txtPassword.BackColor = Color.White
            Me.BackColor = Color.FromArgb(19, 86, 139)

            If Not chkRememberMe.Checked Then
                txtUsername.Clear()
            End If

            txtPassword.Clear()

            If Not m_Initialized Then
                With My.Application.Info
                    ApplicationCaption = IIf(.Title <> "", .Title, IIf(.Description <> "", .Description, .ProductName)).ToString
                End With
            End If

            If Not String.IsNullOrWhiteSpace(CompanyName) Then
                With lblTerminalDetails
                    .Tag = CompanyName
                    .Text = .Tag
                    .Visible = True
                End With
            End If

            If My.Computer.Info.OSFullName.Contains("Windows XP") Then
                m_IsXP = True
                Refresh()
                Opacity = 0
            End If

            lblMessage.Text = DefaultMessage
            lblMessage.Visible = False

            Dim localDataSource As String = GetSetting("LocalDatasource")

            If localDataSource.StartsWith(".\") OrElse localDataSource.StartsWith(My.Computer.Name) Then
                lblStatus.Text = "Local Connection"
            Else
                lblStatus.Text = "Online via LAN"
            End If

            lblStatus.Tag = lblStatus.Text

            pnlBarcodeLogin.Visible = ShowBarcodeLogin

            If ShowBarcodeLogin Then
                txtUsername.Enabled = False
                txtPassword.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FocusAndSelect(ByVal ctrl As Infragistics.Win.UltraWinEditors.UltraTextEditor)
        ctrl.Focus()
        ctrl.SelectAll()
    End Sub

    Private Sub HideFadeForm(ByVal frm As Form)
        With frm
            If .Visible And Not m_IsBusy Then
                m_IsBusy = True
                For Each ctrl As Control In frm.Controls
                    ctrl.Visible = False
                Next
                Application.DoEvents()
                Try
                    While .Opacity > 0
                        .Opacity -= 0.005
                        Application.DoEvents()
                    End While
                Catch ' Ignore errors
                End Try
                .Visible = False
                m_IsBusy = False
            End If
        End With
    End Sub

    Private Sub ShowFadeForm(ByVal frm As Form)
        With frm
            If Not .Visible And Not m_IsBusy Then
                m_IsBusy = True
                .Visible = True
                Try
                    While .Opacity < 1
                        .Opacity += 0.005
                        Application.DoEvents()
                    End While
                Catch ' Ignore errors
                End Try
                m_IsBusy = False
            End If
        End With
    End Sub

    Private Sub DoLogin()
        Try
            If m_Timer Is Nothing Then
                m_Timer = New Timer
            ElseIf m_IsBusy Then
                Return
            End If

            lblMessage.Visible = False
            lblMessage.Text = DefaultMessage

            If Not pnlBarcodeLogin.Visible Then
                If txtUsername.Focused Then
                    FocusAndSelect(txtPassword)
                    Return
                Else
                    txtUsername.Text = txtUsername.Text.Trim
                    txtPassword.Text = txtPassword.Text.Trim

                    If txtPassword.TextLength = 0 And PasswordRequired Then
                        FocusAndSelect(txtPassword)
                        Return
                    Else
                        Application.UseWaitCursor = True
                    End If
                End If
            End If

            Dim rnd As New Random(Now.Millisecond)
            m_Timer.Interval = rnd.Next(1000, 3000) ' get random number within 1 to 3 seconds
            m_Timer.Start()

            picLoader.Image = My.Resources.login_loading_4
            picLoader.Visible = True

            If pnlBarcodeLogin.Visible Then
                lblScanBarcode.Text = "VALIDATING BARCODE"
            Else
                txtUsername.Enabled = False
                txtPassword.Enabled = False
                chkRememberMe.Enabled = False
            End If

            m_IsBusy = True
            m_IsValid = False
            m_IsDone = False

            If pnlBarcodeLogin.Visible Then
                RaiseEvent ValidateBarcode(m_Barcode, m_IsValid)
            Else
                RaiseEvent ValidateLogin(Username, Password, m_IsValid)
            End If

            m_IsDone = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetSetting(settingName As String) As String
        Dim obj As String = Registry.ReadGlobalSetting(settingName)
        If String.IsNullOrWhiteSpace(obj) Then
            Return String.Empty
        Else
            Return CStr(obj)
        End If
    End Function

#End Region

End Class