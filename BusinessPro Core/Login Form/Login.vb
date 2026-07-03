<System.ComponentModel.DefaultEvent("ValidateLogin")> _
<System.ComponentModel.DefaultProperty("ApplicationTitle")> _
Public Class Login

#Region "Constructors"

    Public Sub New()
        m_FormLogin = New FormLogin
    End Sub

#End Region

#Region "Declarations"

    Private WithEvents m_FormLogin As FormLogin
    Private m_FormLoginBG As FormLoginBG

    Public Enum Result
        Successful = System.Windows.Forms.DialogResult.OK
        Failed = System.Windows.Forms.DialogResult.Cancel
    End Enum

#End Region

#Region "Events"

    ''' <summary>Raised after user clicks the login button for validation of login information.</summary>
    ''' <param name="Username">Username to be validated.</param>
    ''' <param name="Password">Password to be validated.</param>
    ''' <param name="IsValid">Specifies the validation result which will be used by the form to identify a successful login.</param>
    ''' <remarks>IsValid parameter is False by default.</remarks>
    Public Event ValidateLogin(ByVal Username As String, ByVal Password As String, ByRef IsValid As Boolean)

    ''' <summary>Raised after user successfully scans a barcode for validation.</summary>
    ''' <param name="Barcode">Barcode to be validated.</param>
    ''' <param name="IsValid">Specifies the validation result which will be used by the form to identify a successful login.</param>
    ''' <remarks>IsValid parameter is False by default.</remarks>
    Public Event ValidateBarcode(ByVal Barcode As String, ByRef IsValid As Boolean)

    ''' <summary>Raised after user clicks the cancel button or pressed the Esc key on keyboard.</summary>
    ''' <param name="CloseForm">Set to False to prevent the form from closing after this event.</param>
    ''' <remarks>CloseForm parameter is True by default.</remarks>
    Public Event Cancelled(ByRef CloseForm As Boolean)

#End Region

#Region "Properties"

    Public ReadOnly Property Form As Form
        Get
            Return m_FormLogin
        End Get
    End Property

    ''' <summary>Returns the username used for login.</summary>
    Public ReadOnly Property Username() As String
        Get
            Return m_FormLogin.Username
        End Get
    End Property

    ''' <summary>Returns the password entered by the user for login validation.</summary>
    Public ReadOnly Property Password() As String
        Get
            Return m_FormLogin.Password
        End Get
    End Property

    ''' <summary>Returns/sets whether the password is required for login or not.</summary>
    Public Property PasswordRequired() As Boolean
        Get
            Return m_FormLogin.PasswordRequired
        End Get
        Set(ByVal value As Boolean)
            m_FormLogin.PasswordRequired = value
        End Set
    End Property

    ''' <summary>Returns the barcode used for login.</summary>
    Public ReadOnly Property Barcode() As String
        Get
            Return m_FormLogin.Barcode
        End Get
    End Property

    ''' <summary>Returns/sets the message displayed after user enters an invalid username or password.</summary>
    Public Property DisplayMessage() As String
        Get
            Return m_FormLogin.DisplayMessage
        End Get
        Set(ByVal value As String)
            m_FormLogin.DisplayMessage = value
        End Set
    End Property

    ''' <summary>Returns the default display message displayed when user entered an invalid username or password.</summary>
    Public ReadOnly Property DefaultMessage() As String
        Get
            Return m_FormLogin.DefaultMessage
        End Get
    End Property

    Property ApplicationBuildNo As Integer = 0
    Property ApplicationTitle As String = ""
    Property ProductName As String = ""
    Property ProductVersion As String = ""
    Property ShowBackgroundForm As Boolean
    Property ShowBarcodeLogin As Boolean
    Property CompanyName As String = ""
    Property TerminalCode As String = ""
    Property SerialNo As String = ""

#End Region

#Region "Event Handlers"

    Private Sub m_FormLogin_Cancelled(ByRef CloseForm As Boolean) Handles m_FormLogin.Cancelled
        RaiseEvent Cancelled(CloseForm)
    End Sub

    Private Sub m_FormLogin_ValidateLogin(ByVal Username As String, ByVal Password As String, ByRef IsValid As Boolean) Handles m_FormLogin.ValidateLogin
        RaiseEvent ValidateLogin(Username, Password, IsValid)
    End Sub

    Private Sub m_FormLogin_ValidateBarcode(ByVal Barcode As String, ByRef IsValid As Boolean) Handles m_FormLogin.ValidateBarcode
        RaiseEvent ValidateBarcode(Barcode, IsValid)
    End Sub

#End Region

#Region "Methods"

    ''' <summary>Shows the login form.</summary>
    ''' <returns>Returns a login result to identify a successful login.</returns>
    ''' <remarks>Storing of login settings is handled by the DLL itself.</remarks>
    Public Function Show() As Result
        Try
            My.Settings.SettingsKey = "LoginSettings"

            If ShowBackgroundForm Then
                If m_FormLoginBG Is Nothing Then
                    m_FormLoginBG = New FormLoginBG
                End If

                m_FormLoginBG.WindowState = FormWindowState.Maximized
                m_FormLoginBG.Show()
            End If

            With m_FormLogin
                If ProductName <> "" Then
                    .UseRememberMe = True
                    If My.Settings.Item("RememberMe").ToString = "True" Then
                        .Username = My.Settings.Item("DefaultUsername").ToString
                    End If
                Else
                    .UseRememberMe = False
                End If

                .Icon = Current.ApplicationIcon
                .ApplicationBuildNo = Me.ApplicationBuildNo
                .ApplicationCaption = Me.ApplicationTitle & " " & Me.ProductVersion
                .ShowBarcodeLogin = Me.ShowBarcodeLogin
                .CompanyName = Me.CompanyName
                .TerminalCode = Me.TerminalCode
                .SerialNo = Me.SerialNo

                Dim loginResult As DialogResult
                If ShowBackgroundForm Then
                    loginResult = .ShowDialog(m_FormLoginBG)
                Else
                    loginResult = .ShowDialog()
                End If

                If loginResult = System.Windows.Forms.DialogResult.OK Then
                    My.Settings.Item("DefaultUsername") = .Username
                    My.Settings.Item("RememberMe") = .RememberUser.ToString
                    My.Settings.Save()

                    Return Result.Successful
                Else
                    Return Result.Failed
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If ShowBackgroundForm Then
                m_FormLoginBG.Close()
                m_FormLoginBG.Dispose()
                m_FormLoginBG = Nothing
            End If
        End Try
    End Function

#End Region

End Class
