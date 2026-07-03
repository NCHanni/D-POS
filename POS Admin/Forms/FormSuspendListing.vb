Friend Class FormSuspendListing

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
            With e.Layout
                .Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                .Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            End With

            With e.Layout.Bands(0)
                For Each column In .Columns
                    With column
                        Select Case .Key
                            Case "date"
                                .SetWidth(130)
                                .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown
                            Case "suspend_code"
                                .SetWidth(100)
                            Case "customer_name"
                                .Header.Caption = "Customer"
                            Case "total_amount"
                                .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                .Format = "#,##0.00"
                                .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleNonNegative
                            Case "cashier_name"
                                .Header.Caption = "Cashier"
                            Case "terminal_code"
                                .Header.Caption = "Terminal"
                            Case "is_finalized"
                                .Header.Caption = "Finalized"
                                .SetWidth(60)
                                .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                            Case "is_disposed"
                                .Header.Caption = "Disposed"
                                .SetWidth(60)
                                .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                            Case "authorized_by"
                                .Header.Caption = "Delete Authorization"
                                .SetWidth(120)
                            Case Else
                                .Hidden = True
                        End Select
                    End With
                Next
            End With

            If e.Layout.Bands.Count > 1 Then
                With e.Layout.Bands(1)
                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "description"
                                    .Header.Caption = "Item"
                                Case "is_gc"
                                    .Header.Caption = "GC"
                                    .SetWidth(40)
                                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                                Case "quantity", "unit_price", "discount_amount", "vat_amount", "amount"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleNonNegative
                                    Select Case .Key
                                        Case "quantity"
                                            .Header.Caption = "Qty"
                                        Case "unit_price"
                                            .Header.Caption = "Price"
                                        Case "discount_amount"
                                            .Header.Caption = "Discount"
                                        Case "vat_amount"
                                            .Header.Caption = "VAT"
                                    End Select
                                Case "unit_of_measure"
                                    .Header.Caption = "UOM"
                                    .SetWidth(50, 60)
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub dateRange_ValueChanged(StartDate As Date, EndDate As Date) Handles dateRange.ValueChanged
        btnRefresh.PerformClick()
    End Sub

    Private Sub txtSearch_ValueChanged(sender As Object, e As EventArgs) Handles txtSearch.ValueChanged
        Try
            Dim searchKeys As String = "suspend_code,customer_name,cashier_name,terminal_code"

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
                .Add("date", GetType(Date))
                .Add("suspend_code")
                .Add("customer_name")
                .Add("total_amount", GetType(Double))
                .Add("cashier_name")
                .Add("terminal_code")
                .Add("is_finalized", GetType(Boolean))
                .Add("is_disposed", GetType(Boolean))
                .Add("authorized_by")
            End With
            grdList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            With New SalesList
                grdList.DataSource = .GetListSuspend(dateRange.StartDate, dateRange.EndDate)
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