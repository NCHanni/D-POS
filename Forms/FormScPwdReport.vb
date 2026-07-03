Friend Class FormScPwdReport

#Region "Declrations"

    Private Const MODULE_NAME_SC As String = "SC Report"
    Private Const MODULE_NAME_PWD As String = "PWD Report"

#End Region

#Region "Enumerations"

    Public Enum ReportType
        SC
        PWD
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(reportType As ReportType)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_ReportType = reportType

        Select Case m_ReportType
            Case ReportType.SC
                Me.Text = "Senior Citizen (SC) Report"
            Case ReportType.PWD
                Me.Text = "Persons with Disabilities (PWD) Report"
        End Select
    End Sub

#End Region

#Region "Properties"

    Private ReadOnly m_ReportType As ReportType
    Public ReadOnly Property ReportShown() As ReportType
        Get
            Return m_ReportType
        End Get
    End Property

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try
            dateRange.StartDate = Date.Now
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Escape
                    Me.Close()
                Case Keys.F5
                    btnRefresh.PerformClick()
            End Select
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdScPwd_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdScPwd.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
                .Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select
                .Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
                .Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                .Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                With .Bands(0)
                    .Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "transaction_date"
                                    .Header.Caption = "Date"
                                    .SetWidth(120)
                                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown
                                Case "sale_code"
                                    .Header.Caption = "Sale Code"
                                    .SetWidth(120)
                                Case "osca_id"
                                    .Header.Caption = "OSCA ID"
                                Case "pwd_no"
                                    .Header.Caption = "PWD No."
                                Case "name"
                                    If m_ReportType = ReportType.SC Then
                                        .Header.Caption = "Name of SC"
                                    ElseIf m_ReportType = ReportType.PWD Then
                                        .Header.Caption = "Name of PWD"
                                    Else
                                        .Header.Caption = "Name"
                                    End If
                                Case "total_amount"
                                    .Header.Caption = "Gross Sales"
                                    .Format = "#,##0.00"
                                Case "discount_amount"
                                    .Header.Caption = "Sales Discount"
                                    .Format = "#,##0.00"
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next
                End With
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            Dim dt As DataTable = DirectCast(grdScPwd.DataSource, DataTable)

            Select Case m_ReportType
                Case ReportType.SC
                    ShowScReport(dt, dateRange.StartDate, dateRange.EndDate)

                Case ReportType.PWD
                    ShowPwdReport(dt, dateRange.StartDate, dateRange.EndDate)
            End Select
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("transaction_date", GetType(Date))
                .Add("sale_code")
                If m_ReportType = ReportType.SC Then
                    .Add("osca_id")
                Else
                    .Add("pwd_no")
                End If
                .Add("name")
                .Add("total_amount", GetType(Double))
                .Add("discount_amount", GetType(Double))
            End With
            grdScPwd.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            With New ScPwd
                .FillScPwd(
                    If(m_ReportType = ReportType.SC, "SC", "PWD"),
                    dateRange.StartDate,
                    dateRange.EndDate)

                grdScPwd.DataSource = .Data

                If .Data.Rows.Count > 0 Then
                    If m_ReportType = ReportType.SC Then
                        btnPrint.CheckPrint(MODULE_NAME_SC)
                    Else
                        btnPrint.CheckPrint(MODULE_NAME_PWD)
                    End If
                Else
                    btnPrint.Enabled = False
                End If
            End With
        Catch
            Throw
        End Try
    End Sub

    Private Sub ShowScReport(data As DataTable, dateFrom As Date, dateTo As Date)
        Try
            Dim rpt As Object
            With New Reporting
                .HasOwnReportViewer = True

                rpt = .PrintScReport(data, dateFrom, dateTo)
            End With

            Dim reportTitle As String = "SC Report"
            If Not EnsureFormView(reportTitle) Then
                LaunchForm(New FormReportViewer(rpt, reportTitle), False)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ShowPwdReport(data As DataTable, dateFrom As Date, dateTo As Date)
        Try
            Dim rpt As Object
            With New Reporting
                .HasOwnReportViewer = True

                rpt = .PrintPwdReport(data, dateFrom, dateTo)
            End With

            Dim reportTitle As String = "PWD Report"
            If Not EnsureFormView(reportTitle) Then
                LaunchForm(New FormReportViewer(rpt, reportTitle), False)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

#End Region

End Class