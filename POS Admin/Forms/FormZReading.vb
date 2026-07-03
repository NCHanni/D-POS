Imports Infragistics.Win.UltraWinGrid

Friend Class FormZReading

#Region "Declarations"

    Private m_AuditTrail As AuditTrail
    Private m_ZReading As ZReading
    Private m_ZReadingData As DataSet

#End Region

#Region "Constructor"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#End Region

#Region "Properties"

    Property IsFinalized As Boolean

    Private m_FinalizeCashUp As Boolean
    Public ReadOnly Property FinalizeCashUp() As Boolean
        Get
            Return m_FinalizeCashUp
        End Get
    End Property

#End Region

#Region "Event Handlers"

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Escape
                    Me.Close()
                Case Keys.F1
#If DEBUG Then
                    Print(False)
#End If
                Case Keys.F5
                    btnRefresh.PerformClick()
                Case Keys.P
                    If IsFinalized AndAlso e.Control Then
                        btnFinalize.PerformClick()
                    End If
                Case Keys.S
                    If e.Control Then
                        btnFinalize.PerformClick()
                    End If
            End Select
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdZReading_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles _
            grdSalesSummary.InitializeLayout,
            grdPaymentDetails.InitializeLayout,
            grdCCBreakdown.InitializeLayout,
            grdCashierSalesTotal.InitializeLayout,
            grdCashierDiscountTotal.InitializeLayout,
            grdAdjustments.InitializeLayout,
            grdCumulativeTotal.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns
                .Override.CellClickAction = CellClickAction.RowSelect
                .Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

                With .Bands(0)
                    .Override.HeaderClickAction = HeaderClickAction.Select
                    .Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "description"
                                    .Header.Caption = ""
                                Case "qty"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Header.Caption = "Qty"
                                    .SetWidth(40)
                                Case "amount"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Amount"
                                    If sender Is grdCumulativeTotal Then
                                        .SetWidth(120)
                                    Else
                                        .SetWidth(80, 100)
                                    End If
                                Case "value"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Header.Caption = "Value"
                                    .SetWidth(100)
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdZReading_InitializeRow(sender As Object, e As InitializeRowEventArgs) _
        Handles grdSalesSummary.InitializeRow,
                grdPaymentDetails.InitializeRow,
                grdCCBreakdown.InitializeRow
        Try
            Select Case e.Row.Cells("description").Value
                Case "GROSS SALES", "NET SALES", "TOTAL PAYMENTS", "TOTAL"
                    e.Row.Cells("description").Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnFinalize_Click(sender As Object, e As EventArgs) Handles btnFinalize.Click
        Try
            If btnFinalize.Tag = "Finalize" Then
                If ShowQuestion("End-of-Day (Z-Reading) Report will now be finalized.\n\nDo you want to continue?") <> QuestionResult.Yes Then
                    Return
                End If

                If m_ZReading.FinalizeZReading() Then
                    SaveAuditTrail("Finalized End-of-Day Report (Z-Reading)")
                    RefreshList()
                    Print(False)
                    SaveAuditTrail("Printed End-of-Day Report (Z-Reading)")
                End If
            Else
                Print(True)
                SaveAuditTrail("Reprinted End-of-Day Report (Z-Reading)")
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_AuditTrail = New AuditTrail
            m_ZReading = New ZReading With {.User = Core.Current.User.Name}
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            m_ZReadingData = New DataSet

            Dim dr As DataRow
            Dim dt = New DataTable
            Dim dtSource = New DataTable
            Dim grossSales As Double
            Dim total As Double = 0.0
            Dim finalizedBy As String
            Dim hasDiscounts As Boolean
            Dim canGenerateZReading As Boolean

            btnFinalize.Enabled = False

            With New TaskProcess(Me)
                .ShowLoader = True
                .Start(
                    Sub()
                        With m_ZReading
                            If dtpDate.Value < DateTime.Now.Date Then
                                .GenerateZReading(dtpDate.Value)
                            Else
                                .GenerateZReading()
                            End If

                            m_ZReadingData = .Data
                            canGenerateZReading = .CanGenerate()
                        End With
                    End Sub,
                    Sub()
#Region "Header"

                        total = 0
                        dtSource = m_ZReadingData.Tables(0)

                        lblBegSi.Text = dtSource.Rows(0)("beginning_si")
                        lblEndSi.Text = dtSource.Rows(0)("ending_si")
                        lblBegBal.Text = CDbl(dtSource.Rows(0)("beginning_balance")).ToString("#,##0.00")
                        lblEndBal.Text = CDbl(dtSource.Rows(0)("ending_balance")).ToString("#,##0.00")
                        grossSales = dtSource.Rows(0)("gross_sales")
                        IsFinalized = CBool(dtSource.Rows(0)("is_finalized"))
                        finalizedBy = dtSource.Rows(0)("finalized_by").ToString

#End Region

#Region "Sale Summary"

                        dt = New DataTable
                        With dt.Columns
                            .Add("description")
                            .Add("qty", GetType(Integer))
                            .Add("amount", GetType(Decimal))
                        End With

#Region "Products"

                        total = 0
                        dtSource = m_ZReadingData.Tables(1)

                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = row("product_type")
                            dr("qty") = row("product_count")
                            dr("amount") = row("product_amount")
                            dt.Rows.Add(dr)
                            total += row("product_amount")
                        Next

                        dr = dt.NewRow()
                        dr("description") = "GROSS SALES"
                        dr("amount") = grossSales
                        dt.Rows.Add(dr)

                        'Space
                        dt.Rows.Add(dt.NewRow)

#End Region

#Region "Discount"

                        total = 0
                        dtSource = m_ZReadingData.Tables(2)

                        For Each row In dtSource.Rows
                            If CDbl(row("reg_discount_amount")) > 0.0 Then
                                dr = dt.NewRow()
                                dr("description") = "Regular Discount"
                                dr("amount") = row("reg_discount_amount")
                                dt.Rows.Add(dr)
                                hasDiscounts = True
                            End If
                            If CDbl(row("sc_discount_amount")) > 0.0 Then
                                dr = dt.NewRow()
                                dr("description") = "SC Discount"
                                dr("amount") = row("sc_discount_amount")
                                dt.Rows.Add(dr)
                                hasDiscounts = True
                            End If
                            If CDbl(row("sc_vat_amount")) > 0.0 Then
                                dr = dt.NewRow()
                                dr("description") = "SC Less VAT"
                                dr("amount") = row("sc_vat_amount")
                                dt.Rows.Add(dr)
                                hasDiscounts = True
                            End If
                            If CDbl(row("pwd_discount_amount")) > 0.0 Then
                                dr = dt.NewRow()
                                dr("description") = "PWD Discount"
                                dr("amount") = row("pwd_discount_amount")
                                dt.Rows.Add(dr)
                                hasDiscounts = True
                            End If
                            If CDbl(row("pwd_vat_amount")) > 0.0 Then
                                dr = dt.NewRow()
                                dr("description") = "PWD Less VAT"
                                dr("amount") = row("pwd_vat_amount")
                                dt.Rows.Add(dr)
                                hasDiscounts = True
                            End If
                            If CDbl(row("sales_net_discount")) > 0.0 Then
                                dr = dt.NewRow()
                                dr("description") = "NET SALES"
                                dr("amount") = row("sales_net_discount")
                                dt.Rows.Add(dr)
                                hasDiscounts = True
                            End If
                        Next

                        If hasDiscounts Then
                            dt.Rows.Add(dt.NewRow)
                        End If

#End Region

#Region "VAT"

                        total = 0
                        dtSource = m_ZReadingData.Tables(3)

                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = "VATable Sales"
                            dr("amount") = row("vat_sales_amount")
                            dt.Rows.Add(dr)

                            dr = dt.NewRow()
                            dr("description") = "VAT Amount"
                            dr("amount") = row("vat_amount")
                            dt.Rows.Add(dr)

                            dr = dt.NewRow()
                            dr("description") = "VAT-Exempt Sales"
                            dr("amount") = row("vat_exempted_sales_amount")
                            dt.Rows.Add(dr)

                            dr = dt.NewRow()
                            dr("description") = "Zero-Rated Sales"
                            dr("amount") = row("zero_rated_sales_amount")
                            dt.Rows.Add(dr)
                        Next

#End Region

                        grdSalesSummary.DataSource = dt

#End Region

#Region "Payment Details"

                        dt = New DataTable
                        With dt.Columns
                            .Add("description")
                            .Add("qty", GetType(Integer))
                            .Add("amount", GetType(Decimal))
                        End With

                        total = 0
                        dtSource = m_ZReadingData.Tables(4)

                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = row("payment_type")
                            dr("qty") = row("payment_count")
                            dr("amount") = row("payment_amount")
                            dt.Rows.Add(dr)
                            total += row("payment_amount")
                        Next

                        dr = dt.NewRow()
                        dr("description") = "TOTAL PAYMENTS"
                        dr("amount") = total
                        dt.Rows.Add(dr)

                        grdPaymentDetails.DataSource = dt

#End Region

#Region "Credit Card Breakdown"

                        dt = New DataTable
                        With dt.Columns
                            .Add("description")
                            .Add("qty", GetType(Integer))
                            .Add("amount", GetType(Decimal))
                        End With

                        total = 0
                        dtSource = m_ZReadingData.Tables(5)

                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = row("cc_type")
                            dr("qty") = row("cc_count")
                            dr("amount") = row("cc_amount")
                            dt.Rows.Add(dr)
                            total += row("cc_amount")
                        Next

                        dr = dt.NewRow()
                        dr("description") = "TOTAL"
                        dr("amount") = total
                        dt.Rows.Add(dr)

                        grdCCBreakdown.DataSource = dt

#End Region

#Region "Cashier Sales Total"

                        dt = New DataTable
                        With dt.Columns
                            .Add("description")
                            .Add("amount", GetType(Decimal))
                        End With

                        total = 0
                        dtSource = m_ZReadingData.Tables(6)

                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = row("cashier_name")
                            dr("amount") = row("cashier_total_sales_amount")
                            dt.Rows.Add(dr)
                        Next

                        grdCashierSalesTotal.DataSource = dt

#End Region

#Region "Cashier Discount Total"

                        dt = New DataTable
                        With dt.Columns
                            .Add("description")
                            .Add("amount", GetType(Decimal))
                        End With

                        total = 0

                        dtSource = m_ZReadingData.Tables(7)
                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = row("cashier_name")
                            dr("amount") = row("cashier_total_discount_amount")
                            dt.Rows.Add(dr)
                        Next

                        grdCashierDiscountTotal.DataSource = dt

#End Region

#Region "Accumulated Adjustments"

                        grdAdjustments.DataSource = m_ZReadingData.Tables(9)

#End Region

#Region "Accumulated Grand Total"

                        dt = New DataTable
                        With dt.Columns
                            .Add("description")
                            .Add("amount")
                        End With

                        total = 0
                        dtSource = m_ZReadingData.Tables(8)

                        For Each row In dtSource.Rows
                            dr = dt.NewRow()
                            dr("description") = "Previous Grand Total"
                            dr("amount") = String.Format("{0:0,000,000,000,000.00}", row("prev_grand_total"))
                            dt.Rows.Add(dr)

                            dr = dt.NewRow()
                            dr("description") = "Current Sales Total"
                            dr("amount") = String.Format("{0:0,000,000,000,000.00}", row("current_sales_total"))
                            dt.Rows.Add(dr)

                            dr = dt.NewRow()
                            dr("description") = "Accumulated Grand Total"
                            dr("amount") = String.Format("{0:0,000,000,000,000.00}", row("cumulative_grand_total"))
                            dt.Rows.Add(dr)
                        Next

                        grdCumulativeTotal.DataSource = dt

#End Region

                        If IsFinalized Then
                            With btnFinalize
                                .Text = "Re-print"
                                .Tag = "Re-print"
                            End With
                            With lblNote
                                .Appearance.ForeColor = Color.Blue
                                .Text = "Z-Reading has already been finalized " & IIf(finalizedBy = "AUTO", "automatically", "by " & finalizedBy) & "."
                                .Visible = True
                            End With
                            btnFinalize.Enabled = True

                        ElseIf canGenerateZReading Then
                            lblNote.Visible = False
                            btnFinalize.Enabled = True
                        Else
                            lblNote.Text = "Z-Reading above is not final! There are still active cashier sessions with pending cash up."
                            lblNote.Visible = True
                            btnFinalize.Enabled = False
                        End If
                    End Sub)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Print(isReprint As Boolean)
        Try
            Dim zrpt As Object

            With New Reporting
                .HasOwnReportViewer = True

                zrpt = .ShowZReading(m_ZReadingData, IsFinalized, isReprint)
            End With

            If zrpt IsNot Nothing Then
                With DirectCast(zrpt, ZReadingReport)
                    .PrintToPrinter(1, False, -1, -1)
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class