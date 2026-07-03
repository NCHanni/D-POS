Friend Class FormCreditCardPayments

#Region "Event Handlers"

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            dateRange.StartDate = Date.Now
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout.Bands(0)
                .Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center
                .Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect

                For Each column In .Columns
                    With column
                        Select Case .Key
                            Case "date"
                                .Header.Caption = "Date"
                                .SetWidth(70, 80)
                                .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateWithoutDropDown
                            Case "sale_code"
                                .Header.Caption = "Sale Code"
                            Case "type_name"
                                .Header.Caption = "Card Type"
                            Case "card_no"
                                .Header.Caption = "Card Number"
                            Case "card_holder"
                                .Header.Caption = "Card Holder"
                            Case "bank_name"
                                .Header.Caption = "Bank Name"
                            Case "amount"
                                .Header.Caption = "Amount"
                                .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleNonNegative
                                .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                .Format = "#,##0.00"
                            Case Else
                                .Hidden = True
                        End Select
                    End With
                Next
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub dateRange_ValueChanged(StartDate As Date, EndDate As Date) Handles dateRange.ValueChanged
        btnRefresh.PerformClick()
    End Sub

    Private Sub txtSearch_ValueChanged(sender As Object, e As EventArgs) Handles txtSearch.ValueChanged
        Try
            Dim searchKeys As String = "sale_code,card_no,card_holder"

            grdList.ActiveRow = Nothing
            grdList.Selected.Rows.Clear()

            With grdList.DisplayLayout.Bands(0)
                .ColumnFilters.ClearAllFilters()

                If txtSearch.Text.Trim <> "" Then
                    .ColumnFilters.LogicalOperator = Infragistics.Win.UltraWinGrid.FilterLogicalOperator.Or

                    For Each search_key In searchKeys.Split(",")
                        .ColumnFilters(search_key.Trim).FilterConditions.Add(
                            Infragistics.Win.UltraWinGrid.FilterComparisionOperator.Contains, txtSearch.Text.Trim)
                    Next
                End If
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            Dim dt As DataTable
            Dim rpt As Object

            With New CreditCardList
                .FillSummary(dateRange.StartDate, dateRange.EndDate)
                dt = .Data
            End With

            With New Reporting
                .HasOwnReportViewer = True
                rpt = .PrintCcPaymentsSummaryReport(dt, dateRange.StartDate, dateRange.EndDate)
            End With

            Dim reportTitle As String = "CC Payments Summary"
            If Not EnsureFormView(reportTitle) Then
                LaunchForm(New FormReportViewer(rpt, reportTitle), False)
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("id", GetType(Integer))
                .Add("date", GetType(Date))
                .Add("sale_code")
                .Add("type_name")
                .Add("card_no")
                .Add("card_holder")
                .Add("bank_name")
                .Add("approval_code")
                .Add("amount")
            End With
            grdList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            With New CreditCardList
                .FillPayments(dateRange.StartDate, dateRange.EndDate)
                grdList.DataSource = .Data
                txtSearch.Clear()
            End With

            If grdList.Rows.Count = 0 Then
                btnClose.Focus()
            Else
                grdList.Focus()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class