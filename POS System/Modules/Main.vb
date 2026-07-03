Imports System.Deployment.Application
Imports System.Diagnostics
Imports System.Reflection

Module Main

    Private WithEvents m_LoginForm As Login
    Private m_LastZReadingDate As Date?
    Private m_IsGiftCertEnabled As Boolean

    Sub Main()
        Try
            Dim checkInstance As Boolean = True
            Dim priceCheckMode As Boolean

            Registry.CompanyName = Application.CompanyName
            Registry.ProductName = Application.ProductName

            If ProcessArguments(Environment.GetCommandLineArgs, checkInstance, priceCheckMode) = False Then
                Return
            Else
                Application.EnableVisualStyles()
            End If

            'Load Resource file
            Dim fs As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Application.ProductName & ".DefaultTheme.isl")
            Infragistics.Win.AppStyling.StyleManager.Load(fs)

            Current.ApplicationIcon = My.Resources.ResourceManager.GetObject("app_icon")
            Current.CompanyInfo = New Structures.CompanyInfo
            Current.FormatUI = New FormatUI
            Current.ProductName = My.Application.Info.ProductName

            FormConnectionSettings.DefaultDatasource = Variables.DEFAULT_DATASOURCE
            FormConnectionSettings.DefaultDatabase = Variables.DEFAULT_DATABASE
            FormConnectionSettings.DefaultUsername = Variables.DEFAULT_USERNAME
            FormConnectionSettings.DefaultPassword = Variables.DEFAULT_PASSWORD

            ' Load connection settings
            If FormConnectionSettings.HasSettings Then
                Current.ConnectionString = FormConnectionSettings.GetSettings()
            Else
PromptSettings:
                Using f As New FormConnectionSettings() With {.TopMost = True}
                    Current.FormatUI.Apply(f)

                    If f.ShowDialog() = DialogResult.OK Then
                        Current.ConnectionString = f.ConnectionString
                    Else
                        Return
                    End If
                End Using
            End If

            Dim settings As SettingsPreferences = GetSettingsAndPreferences()
            Dim terminalCode As String = (New Terminals).GetTerminalCode()
            Dim terminalId As Integer

            DATE_FORMAT = settings.ServerDateFormat
            DATETIME_FORMAT = settings.ServerDateFormat & IIf(settings.ServerDateFormat.Length = 10, " hh:mm tt", "")

            Dim mainFormCaption As String =
                My.Application.Info.Description & "{0} v" &
                My.Application.Info.Version.Major & "." &
                My.Application.Info.Version.Minor

#If DEBUG Then
            mainFormCaption &= String.Format(" - DEVELOPER MODE Build {0}", My.Application.Info.Version.Build)
#Else
            If settings.TrainingMode Then
                mainFormCaption &= " - TRAINING MODE"
            End If
#End If

            If priceCheckMode Then
                Using f As New FormPriceCheck(settings)
                    f.Text = String.Format(mainFormCaption, " Price Inquiry Tool")
                    f.ShowDialog()
                End Using
                Return

            ElseIf String.IsNullOrWhiteSpace(terminalCode) Then
                Dim frm As New FormTerminals(settings)
ASK_TERMINAL:
                If LaunchForm(frm) = DialogResult.OK Then
                    terminalId = frm.Id
                    terminalCode = frm.Code

                ElseIf frm.DialogResult = DialogResult.Retry Then
                    If ShowQuestion(
                        "Setting a terminal is required to run POS.\n\n" &
                        "Do you want to set a terminal now?", "Select Terminal") = QuestionResult.Yes Then
                        GoTo ASK_TERMINAL
                    Else
                        Return ' End application
                    End If
                Else
                    Return
                End If
            End If

            With New Terminals
                If terminalId > 0 Then
                    .SetTerminalActive(terminalId)
                End If

                SetTerminalSettings(terminalCode, .GetSerialNo(terminalCode))
            End With

            Dim hasActiveSession As Boolean = (New CashUp).HasActiveSession(Current.Settings.TerminalCode)
            Dim authOfficerCode As String = settings.AuthorizedOfficerCode

            If Not hasActiveSession Then
ASK_LOGIN:
                m_LoginForm = New Login

                With m_LoginForm
                    .ApplicationBuildNo = My.Application.Info.Version.Build
                    .ApplicationTitle = My.Application.Info.Description
                    .ProductName = Application.ProductName
                    .ProductVersion = String.Format("v{0}", Application.ProductVersion)
                    .ShowBackgroundForm = settings.FullscreenCashier
                    .ShowBarcodeLogin = settings.UseBarcodeLogin
                    .CompanyName = Current.CompanyInfo.Name
                    .TerminalCode = Current.Settings.TerminalCode
                    .SerialNo = Current.Settings.TerminalSerialNo
                End With

                If m_LoginForm.Show <> Login.Result.Successful Then
                    Return
                Else
                    settings = GetSettingsAndPreferences()

                    With New FormReportViewer(Nothing, "")
                        .InitReportViewer()
                        .WindowState = FormWindowState.Minimized
                        .ShowInTaskbar = False
                        .Show()
                        .Close()
                    End With

                    If Current.User.IsSuperRole OrElse Current.User.RoleCode = authOfficerCode Then
                        SaveAuditTrail("Administrative Login - " & Current.User.Name & " (" & Current.User.RoleName & ")")
                    Else
                        If Not CanTransact() Then
                            ShowWarning("POS transactions are no longer allowed for the day!\n\nZ-Reading has already been finalized.", "Z-Reading Finalized")
                            Return
                        End If

                        Dim row As DataRow
                        With New CashUp
                            .LogInSession(Current.User.Code, Current.Settings.TerminalCode)
                            row = .Data.Tables(0)(0)
                        End With

                        If CLng(row("session_id")) > 0 Then
                            SetSessionSettings(
                                row("day_session_id"),
                                row("session_id"),
                                row("date_in"),
                                row("cash_in"),
                                True
                            )
                        End If

                        SaveAuditTrail("Cashier Login - " & Current.User.Name)
                    End If
                End If
            Else
                If Not ResumeSession(settings) Then
                    GoTo ASK_LOGIN
                End If
            End If

            Dim isCustomerService As Boolean = Current.Rights.IsAllowed("Customer Return", "Sales")

            If ApplicationDeployment.IsNetworkDeployed Then
                With ApplicationDeployment.CurrentDeployment.CurrentVersion
                    mainFormCaption &= String.Format(" (ClickOnce-Deployed rev.{0})", .Revision)
                End With
            End If

            If Current.User.RoleCode = authOfficerCode OrElse isCustomerService Then
                Current.FormMain = New POS.Admin.FormMain With {
                    .ApplicationCaption = My.Application.Info.Description & " v" & Application.ProductVersion,
                    .Text = String.Format(mainFormCaption, " Administration"),
                    .UseBarcodeLogin = settings.UseBarcodeLogin
                }

                Current.FormMain.ShowDialog()
            Else
                If Not settings.TrainingMode Then
                    If Not IsBIRLicenseValid() Then
                        GoTo ASK_LOGIN
                    End If
                End If

                With New Sale
                    .GetNextCode(.Code)

                    If String.IsNullOrWhiteSpace(.Code) Then
                        ShowWarning(
                            "No more available invoice series to continue this transaction!\n\n" &
                            "Please contact you system administrator.",
                            "Check Out Failed")
                        SaveAuditTrail("Login Failed! No more available invoice series.")
                        Return
                    End If
                End With

                Current.FormMain = New FormMain(settings) With {.Text = String.Format(mainFormCaption, "")}
                Current.FormMain.ShowDialog()
            End If
        Catch ex As SqlClient.SqlException
            Try
                Dim msg As String =
                    "Unable to connect to database using the current settings.\n\n" &
                    "Error: " & ex.Message & "\n\n" &
                    "Would you like to reconfigure these settings now?"

                If ShowQuestion(msg, "Confirmation") = QuestionResult.Yes Then
                    GoTo PromptSettings
                End If
            Catch exx As Exception
#If DEBUG Then
                Windows.Forms.MessageBox.Show(exx.Message & vbCrLf & exx.StackTrace, "Application Error")
#Else
                Windows.Forms.MessageBox.Show(exx.Message, "Application Error")
#End If
            End Try
        Catch ex As Exception
#If DEBUG Then
            Windows.Forms.MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "Application Error")
#Else
            Windows.Forms.MessageBox.Show(ex.Message, "Application Error")
#End If
        End Try
    End Sub

    Private Function ProcessArguments(args() As String, ByRef checkInstance As Boolean, ByRef priceCheckMode As Boolean) As Boolean
        Try
            If args.Length > 1 Then
                For Each arg As String In args
                    If arg.StartsWith("-") OrElse arg.StartsWith("/") Then
                        arg = arg.Substring(1)
                        Select Case arg.ToLower
                            Case "admin"
                                If IsWindowsAdmin() Then
                                    checkInstance = False
                                Else
                                    ShowWarning("Please run the application as an administrator to continue!", My.Application.Info.Title)
                                    Return False
                                End If
                            Case "price"
                                priceCheckMode = True
                            Case "restart"
                                checkInstance = False
                            Case "exit"
                                Return False
                            Case Else
                                Try
                                    Dim argValue As String = arg.Substring(arg.IndexOf("=") + 1)
                                    If arg.ToLower.StartsWith("datasource=") Then
                                        Registry.WriteGlobalSetting(FormConnectionSettings.LOCAL_DATASOURCE, argValue.Replace("%20", " "))
                                    ElseIf arg.ToLower.StartsWith("database=") Then
                                        Registry.WriteGlobalSetting(FormConnectionSettings.LOCAL_DATABASE, argValue.Replace("%20", " "))
                                    ElseIf arg.ToLower.StartsWith("username=") Then
                                        Registry.WriteGlobalSetting(FormConnectionSettings.LOCAL_USERNAME, argValue.Replace("%20", " "))
                                    ElseIf arg.ToLower.StartsWith("password=") Then
                                        Registry.WriteGlobalSetting(FormConnectionSettings.LOCAL_PASSWORD, argValue)
                                    Else
                                        ShowWarning("Invalid application argument has been defined!\n\nPlease contact your system administrator.", My.Application.Info.Title)
                                        Return False
                                    End If
                                Catch ex As Exception
                                    ' ignore write to registry errors
                                End Try
                        End Select
                    End If
                Next
            End If

            If checkInstance Then
                ' Check for current instance of the application
                With Process.GetCurrentProcess
                    For Each p As Process In Process.GetProcessesByName(.ProcessName)
                        If .Id <> p.Id Then
                            ' Active and focus current instance instead
                            Try
                                AppActivate(p.Id)
                                SendKeys.SendWait("~")
                            Catch ex As Exception
                                ' will error on app restart/log off
                            End Try
                            Return False
                        End If
                    Next
                End With
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsBIRLicenseValid() As Boolean
        Try
            Dim permitNo As String = Current.CompanyInfo.PermitNo
            Dim permitMin As String = Current.CompanyInfo.PermitMIN

            Return True
            'If String.IsNullOrWhiteSpace(permitNo) OrElse String.IsNullOrWhiteSpace(permitMin) Then
            '    ShowWarning(
            '        "BusinessPro-POS License Details from BIR is incomplete!\n" &
            '        If(String.IsNullOrWhiteSpace(permitNo), "\n• Permit-to-Use (PTU) No.", "") &
            '        If(String.IsNullOrWhiteSpace(permitMin), "\n• Machine ID No. (MIN)", "") &
            '        "\n\nPlease contact your System Administrator.", "Cashier Login Failed")
            '    Return False
            'Else
            '    Return True
            'End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub m_LoginForm_ValidateLogin(ByVal Username As String, ByVal Password As String, ByRef IsValid As Boolean) Handles m_LoginForm.ValidateLogin
        Try
            Dim cUsers As New UserList
            Dim cUser As New User

            cUsers.FillByStatus(True)

            Dim dr As DataRow() = cUsers.Data.Select("username = '" & Username & "' and password = '" & Encrypt(Password) & "' and is_active = 'True'")
            If dr.Length > 0 Then
                IsValid = SetCurrentUser(dr(0))
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub m_LoginForm_ValidateBarcode(ByVal Barcode As String, ByRef IsValid As Boolean) Handles m_LoginForm.ValidateBarcode
        Try
            If String.IsNullOrWhiteSpace(Barcode) Then
                m_LoginForm.DisplayMessage = "*Scan login barcode to continue."
                IsValid = False
                Return
            End If

            Dim cUsers As New UserList

            cUsers.FillByStatus(True)

            Dim dr As DataRow() = cUsers.Data.Select("barcode = '" & Barcode & "' and is_active = 'True'")
            If dr.Length > 0 Then
                IsValid = SetCurrentUser(dr(0))
            Else
                m_LoginForm.DisplayMessage = "*Invalid login barcode."
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Function SetCurrentUser(row As DataRow) As Boolean
        Try
            Current.Rights = New Rights
            With Current.Rights
                .RoleCode = row("role_code")
                .Fill()
            End With

            Dim secureUser As New User
            With secureUser
                .RoleCode = row("role_code")
                .Code = row("code")
                .Name = row("name")
                .UserName = row("username")
                .Barcode = row("barcode")
            End With

            Dim currentUser As Structures.User
            With currentUser
                .Code = row("code")
                .Name = row("name")
                .RoleCode = row("role_code")
                .RoleName = row("role_name")
                .IsSuperRole = CBool(row("is_super_role"))
                .UserName = row("username")
                .Password = row("password") ' Encrypted
                .SecureHash = secureUser.SecureHash
                .HasBarcode = Not String.IsNullOrEmpty(row("barcode"))
            End With
            Current.User = currentUser

            If Current.User.SecureHash <> row("secure_hash") Then
                m_LoginForm.DisplayMessage = "*User has been tampered! Account locked."
                Return False
            End If

            If Not Current.User.IsSuperRole Then
                Dim cashUp As New CashUp
                If cashUp.IsCurrentlyLoggedIn(Current.User.Code, Current.Settings.TerminalCode) Then
                    m_LoginForm.DisplayMessage = "*User is currently logged-in to another terminal!"
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetSettingsAndPreferences() As SettingsPreferences
        Try
            Dim settings As New SettingsPreferences
            Dim info As New CompanyInfo
            Dim rights As New Rights

            Dim currentCompanyInfo As Structures.CompanyInfo = Current.CompanyInfo
            Dim currentSettings As Structures.Settings = Current.Settings

            settings.Fill()
            info.Fill()

            With currentSettings
                .SalesTax = settings.SalesTax
                .IsGiftCertEnabled = rights.IsGiftCertEnabled()
            End With

            With currentCompanyInfo
                .Name = info.Data.Name
                .Name2 = info.Data.Name2
                .PermitNo = info.Data.PermitNo
                .PermitMIN = info.Data.PermitMIN
                .PermitIssued = info.Data.PermitIssued
                .PermitExpiry = info.Data.PermitExpiry
            End With

            With currentCompanyInfo
                .VatRegistrationNo = info.Data.VatRegistrationNo
                .BusinessStyle = info.Data.BusinessStyle
                .Address = info.Data.Address
                .ContactNo = info.Data.ContactNo
                .FaxNo = info.Data.FaxNo
                .EmailAddress = info.Data.EmailAddress
                .WebsiteUrl = info.Data.WebsiteUrl
            End With

            With currentCompanyInfo 'Report error fixes (empty data)
                .Name &= " "
                .Name2 &= " "
                .PermitNo &= " "
                .PermitMIN &= " "
                .VatRegistrationNo &= " "
                .BusinessStyle &= " "
                .Address &= " "
                .ContactNo &= " "
            End With

            Current.CompanyInfo = currentCompanyInfo
            Current.Settings = currentSettings

            Return settings
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SetTerminalSettings(code As String, serialNo As String)
        Try
            Dim settings As Structures.Settings = Current.Settings
            With settings
                .TerminalCode = code
                .TerminalSerialNo = serialNo
            End With
            Current.Settings = settings
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Friend Sub SetSessionSettings(daySessionId As Long, sessionId As Long, cashInDate As Date, cashInAmount As Double, cashInSet As Boolean)
        Try
            Dim settings As Structures.Settings = Current.Settings
            With settings
                .DaySessionId = daySessionId
                .CashierSessionId = sessionId
                .CashInDate = cashInDate
                .CashInAmount = cashInAmount
                .CashInSet = cashInSet
            End With
            Current.Settings = settings
        Catch ex As Exception

        End Try
    End Sub

    Private Function ResumeSession(settings As SettingsPreferences) As Boolean
        Try
            Dim data As DataSet
            With New CashUp
                .Fill(Current.Settings.TerminalCode)
                data = .Data
            End With

            If data.Tables(0).Rows.Count > 0 Then
                Dim rowCashup As DataRow = data.Tables(0).Rows(0)
                Dim cashInSet As Boolean

                If data.Tables(1).Rows.Count > 0 Then
                    Dim rowUser As DataRow = data.Tables(1)(0)

                    Dim secureUser As New User
                    With secureUser
                        .RoleCode = rowUser("role_code")
                        .Code = rowUser("code")
                        .Name = rowUser("name")
                        .UserName = rowUser("username")
                        .Barcode = rowUser("barcode")
                    End With

                    Dim currentUser As New Structures.User
                    With currentUser
                        .Code = rowUser("code")
                        .RoleCode = rowUser("role_code")
                        .RoleName = rowUser("role_name")
                        .IsSuperRole = rowUser("is_super_role")
                        .Name = rowUser("name")
                        .UserName = rowUser("username")
                        .Password = rowUser("password") ' Encrypted
                        .SecureHash = rowUser("secure_hash")
                        .HasBarcode = Not String.IsNullOrEmpty(rowUser("barcode"))
                    End With
                    Current.User = currentUser

                    If secureUser.SecureHash <> currentUser.SecureHash Then
                        SaveAuditTrail("Resume Cashier Session Failed! User data has been tampered.")
                        Return False
                    End If

                    Current.Rights = New Rights
                    With Current.Rights
                        .RoleCode = currentUser.RoleCode
                        .Fill()
                    End With

                    cashInSet = True
                    SaveAuditTrail("Resume Cashier Session - " & currentUser.Name)
                End If

                SetSessionSettings(
                    rowCashup("day_session_id"),
                    rowCashup("session_id"),
                    rowCashup("date_in"),
                    rowCashup("cash_in"),
                    cashInSet
                )

                Return True
            Else
                SaveAuditTrail("Resume Cashier Session Failed! Cash Up data not found.")
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function CanTransact() As Boolean
        Try
            Dim zReading As New CashUp

            m_LastZReadingDate = zReading.GetLastZReadingDate

            If Not m_LastZReadingDate.HasValue Then
                Return True
            End If

            Return m_LastZReadingDate.Value.Date <> Today
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SaveAuditTrail(activity As String)
        Try
            With New AuditTrail
                .TerminalCode = Current.Settings.TerminalCode
                .UserCode = Current.User.Code
                .UserName = Current.User.Name
                .Activity = activity
                .SaveAuditTrail()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Module
