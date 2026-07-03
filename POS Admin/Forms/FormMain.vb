Imports Infragistics.Win.UltraWinExplorerBar

Public Class FormMain

#Region "Declarations"

    Private ReadOnly m_AuditTrail As AuditTrail
    Private WithEvents m_FormUser As FormUser

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Current.FormatUI.Apply(Me)
    End Sub

#End Region

#Region "Properties"

    Property ApplicationCaption As String ' For Reports
    Property UseBarcodeLogin As Boolean

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            SaveAuditTrail("Logged-out on POS Admin - " & Current.User.Name & " (" & Current.User.RoleName & ")", Me)
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Space AndAlso e.Modifiers = Keys.Control Then
            xbarSidebar.Visible = Not xbarSidebar.Visible

        ElseIf Me.MdiChildren.Length = 0 Then
            If e.Modifiers = Keys.None AndAlso e.KeyCode = Keys.Escape Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Sidebar"

    Private Sub xbarSidebar_ItemClick(sender As Object, e As Infragistics.Win.UltraWinExplorerBar.ItemEventArgs) Handles xbarSidebar.ItemClick
        Try
            Select Case e.Item.Text
                Case "Information"
                    LaunchForm(New FormCompanyInfo, True, Me)

                Case "Roles"
                    LaunchForm(New FormRole With {
                            .KeyPreview = True
                        }, False)
                Case "Users"
                    m_FormUser = New FormUser(False) With {
                            .UseBarcodeLogin = UseBarcodeLogin
                        }
                    LaunchForm(m_FormUser, False)
                Case "POS Settings"
                    LaunchForm(New FormSettingsPreferences(), False)

                Case "Connection Setup"
                    Using f As New Core.FormConnectionSettings With {.MinimizeBox = False}
                        LaunchForm(f, True, Me)
                    End Using

                Case "Credit Cards"
                    LaunchForm(New FormCreditCard, False)
                Case "Discount Groups"
                    LaunchForm(New FormDiscountGroups, False)
                Case "Items"
                    LaunchForm(New FormItem, False)
                Case "Terminals"
                    LaunchForm(New FormTerminals, False)
                Case "Gift Certificates"
                    LaunchForm(New FormGiftCertificates, False)
                Case "Customers"
                    LaunchForm(New FormCustomer(False, True) With {
                            .ModuleName = "Customers",
                            .ModuleGroup = "Libraries",
                            .TerminalCode = Current.Settings.TerminalCode,
                            .UserCode = Current.User.Code,
                            .UserCompany = Current.CompanyInfo.Name,
                            .UserName = Current.User.Name
                        }, False)

                Case "Customer Return"
                    LaunchForm(New FormCustomerReturn, False)
                Case "CC Payments"
                    LaunchForm(New FormCreditCardPayments, False)
                Case "Sales Listing"
                    LaunchForm(New FormSalesListing, False)
                Case "Void Listing"
                    LaunchForm(New FormVoidListing, False)
                Case "Refund Listing"
                    LaunchForm(New FormRefundListing, False)
                Case "Suspend Listing"
                    LaunchForm(New FormSuspendListing, False)

                Case "Audit Trail"
                    LaunchForm(New FormAuditTrail, False)
                Case "E-Journal Viewer"
                    LaunchForm(New FormEJournalViewer, False)
                Case "SC Report"
                    Dim reportTitle As String = "Senior Citizen (SC)"
                    If Not EnsureFormView(reportTitle & " Report") Then
                        Dim f As New FormReportViewer(
                            GetSCReport(Now.Date, Now.Date),
                            reportTitle,
                            FormReportViewer.DateFilters.DateRange)
                        AddHandler f.OnRefresh, AddressOf FormReportViewer_OnRefresh
                        LaunchForm(f, False)
                    End If
                Case "PWD Report"
                    Dim reportTitle As String = "Persons with Disabilities (PWD)"
                    If Not EnsureFormView(reportTitle & " Report") Then
                        Dim f As New FormReportViewer(
                            GetPWDReport(Now.Date, Now.Date),
                            reportTitle,
                            FormReportViewer.DateFilters.DateRange)
                        AddHandler f.OnRefresh, AddressOf FormReportViewer_OnRefresh
                        LaunchForm(f, False)
                    End If
                Case "Z-Reading"
                    LaunchForm(New FormZReading, False)
                Case "Daily Sales"
                    Dim reportTitle As String = "Daily Sales"
                    If Not EnsureFormView(reportTitle & " Report") Then
                        Dim f As New FormReportViewer(
                            GetDailySalesReport(Now.Date),
                            reportTitle,
                            FormReportViewer.DateFilters.DateOnly)
                        AddHandler f.OnRefresh, AddressOf FormReportViewer_OnRefresh
                        LaunchForm(f, False)
                    End If
                Case "Daily Sales w/ QR"
                    Dim reportTitle As String = "Daily Sales"
                    If Not EnsureFormView(reportTitle & " Report") Then
                        Dim f As New FormReportViewer(
                            GetDailySalesReport(Now.Date, True),
                            reportTitle,
                            FormReportViewer.DateFilters.DateOnly)
                        AddHandler f.OnRefresh, AddressOf FormReportViewer_OnRefresh
                        LaunchForm(f, False)
                    End If
                Case "Sales Summary"
                    Dim reportTitle As String = "Sales Summary"
                    If Not EnsureFormView(reportTitle & " Report") Then
                        Dim f As New FormReportViewer(
                            GetSalesSummary(Now.Date, Now.Date),
                            reportTitle,
                            FormReportViewer.DateFilters.DateRange)
                        AddHandler f.OnRefresh, AddressOf FormReportViewer_OnRefresh
                        LaunchForm(f, False)
                    End If
                Case "Reset Details"
                    LaunchForm(New FormReset, True, Me)

                Case "Log off"
                    If ShowLogOffQuestion() = QuestionResult.Yes Then
                        Diagnostics.Process.Start(Application.ExecutablePath, "-restart")
                        Application.Exit()
                    End If
                Case "Close", "Exit"
                    Application.Exit()
            End Select
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub xbarSidebar_GroupExpanding(sender As Object, e As CancelableGroupEventArgs) Handles xbarSidebar.GroupExpanding
        Try
            With xbarSidebar
                If .Height < 600 Then
                    Select Case e.Group.Key
                        Case "Security"
                            .Groups.Item("Libraries").Expanded = False
                            .Groups.Item("Sales").Expanded = False
                            .Groups.Item("Reports").Expanded = False
                        Case "Libraries"
                            .Groups.Item("Security").Expanded = False
                            .Groups.Item("Sales").Expanded = False
                            .Groups.Item("Reports").Expanded = False
                        Case "Sales"
                            .Groups.Item("Security").Expanded = False
                            .Groups.Item("Libraries").Expanded = False
                            .Groups.Item("Reports").Expanded = False
                        Case "Reports"
                            .Groups.Item("Security").Expanded = False
                            .Groups.Item("Sales").Expanded = False
                            .Groups.Item("Libraries").Expanded = False
                    End Select
                End If
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub FormReportViewer_OnRefresh(sender As FormReportViewer, e As FormReportViewer.DateParams)
        Try
            Select Case sender.Text
                Case "Senior Citizen (SC) Report"
                    sender.Report = GetSCReport(e.DateFrom, e.DateTo)
                Case "Persons with Disabilities (PWD) Report"
                    sender.Report = GetPWDReport(e.DateFrom, e.DateTo)
                Case "Daily Sales Report"
                    sender.Report = GetDailySalesReport(e.Date, True)
                Case "Sales Summary Report"
                    sender.Report = GetSalesSummary(e.DateFrom, e.DateTo)
            End Select
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            With Current.User
                sbStatus.Panels("user").Text = "User: " & .Name & If(.RoleName = "", "", " (" & .RoleName & ")")
                sbStatus.Panels("company").Text = Current.CompanyInfo.Name
            End With

            Dim conString As String = Current.ConnectionString.LocalConnectionString

            If conString.StartsWith("Data Source=.\") OrElse conString.StartsWith("Data Source=" & My.Computer.Name) Then
                sbStatus.Panels("Status").Text = "Local Connection"
            Else
                sbStatus.Panels("Status").Text = "Online via LAN"
            End If

            sbStatus.Panels("Status").Text &= " @ " & conString.Substring(12, conString.IndexOf(";") - 12)

            InitializeAccessRight()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Friend Sub InitializeAccessRight()
        Try
            With xbarSidebar
                Dim isWindowsAdmin As Boolean = Core.IsWindowsAdmin()

                For Each group As UltraExplorerBarGroup In .Groups
                    With group
                        Select Case .Text
                            Case "Company"
                                .Visible = True
                            Case "Exit"
                            Case Else
                                If String.IsNullOrWhiteSpace(.Key) Then
                                    .Key = .Text.Trim
                                End If
                        End Select
                    End With

                    For Each item As UltraExplorerBarItem In group.Items
                        With item
                            Select Case .Text
                                Case "Information"
                                Case "Log off"
                                Case "Close"
                                Case "Daily Sales w/ QR"
                                    .Visible = True
                                Case Else
                                    If String.IsNullOrWhiteSpace(.Key) Then
                                        .Key = .Text.Trim
                                    End If
                                    .Visible = Current.Rights.IsAllowed(.Key, .Group.Key)
                            End Select
                        End With
                    Next
                Next

                Dim hasVisibleItem As Boolean
                For Each group As UltraExplorerBarGroup In .Groups
                    hasVisibleItem = False
                    For Each item As UltraExplorerBarItem In group.Items
                        If item.Visible Then
                            hasVisibleItem = True
                            Exit For
                        End If
                    Next
                    group.Visible = hasVisibleItem
                Next
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetSCReport(dateFrom As Date, dateTo As Date) As Object
        Try
            Dim data As DataTable

            With New ScPwd
                .FillScPwd("SC", dateFrom, dateTo)
                data = .Data
            End With

            Dim report As Object = (New Reporting With {
                    .HasOwnReportViewer = True,
                    .ModuleName = "SC Report"
                }).PrintScReport(data, dateFrom, dateTo)

            Return report
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetPWDReport(dateFrom As Date, dateTo As Date) As Object
        Try
            Dim data As DataTable

            With New ScPwd
                .FillScPwd("PWD", dateFrom, dateTo)
                data = .Data
            End With

            Dim report As Object = (New Reporting With {
                    .HasOwnReportViewer = True,
                    .ModuleName = "PWD Report"
                }).PrintPwdReport(data, dateFrom, dateTo)

            Return report
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetDailySalesReport(dateValue As Date, Optional isNewDaily As Boolean = False) As Object
        Try
            Dim data As DataTable = (New DailySalesList).GetData(dateValue, isNewDaily)

            Dim report As Object = (New Reporting With {
                    .ApplicationCaption = Me.ApplicationCaption,
                    .HasOwnReportViewer = True,
                    .ModuleName = "Daily Sales"
                }).GetDailySales(data, dateValue, isNewDaily)

            Return report
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetSalesSummary(dateFrom As Date, dateTo As Date) As Object
        Try
            Dim data As DataTable = (New SalesSummaryList With {
                    .DateFrom = dateFrom,
                    .DateTo = dateTo
                }).GetData()

            Dim report As Object = (New Reporting With {
                    .ApplicationCaption = Me.ApplicationCaption,
                    .HasOwnReportViewer = True,
                    .ModuleName = "Sales Summary"
                }).GetSalesSummary(data)

            Return report
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class