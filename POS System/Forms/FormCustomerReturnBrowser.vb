Imports Infragistics.Win.UltraWinGrid

Friend Class FormCustomerReturnBrowser

#Region "Constructor"

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#End Region

#Region "Property"

    Property CustomerCode As String
    Property DocumentNo As String
    Property SaleCode As String

#End Region

#Region "Event Handler"

#Region "Form"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                DialogResult = DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdList_AfterRowActivate(sender As Object, e As EventArgs) Handles grdList.AfterRowActivate
        Try
            btnOk.Enabled = True
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns

                .Override.AllowAddNew = AllowAddNew.No
                .Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                .Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
                .Override.CellClickAction = CellClickAction.RowSelect
                .Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center

                With .Bands(0)
                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "code"
                                    .Header.Caption = "Refund Code"

                                Case "sale_code"
                                    .Header.Caption = "Invoice No."

                                Case "remarks"

                                Case "total_amount"
                                    .Header.Caption = "Total Amount"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Style = ColumnStyle.DoubleNonNegative
                                    .Format = "#,##0.00"

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

    Private Sub grdList_DoubleClickRow(sender As Object, e As DoubleClickRowEventArgs) Handles grdList.DoubleClickRow
        Try
            btnOk.PerformClick()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub txtSearch_ValueChanged(sender As Object, e As EventArgs) Handles txtSearch.ValueChanged
        Try
            Dim searchKeys As String = "code,sale_code"

            grdList.ActiveRow = Nothing
            grdList.Selected.Rows.Clear()

            With grdList.DisplayLayout.Bands(0)
                .ColumnFilters.ClearAllFilters()
                If txtSearch.Text.Trim = "" Then
                    Activate()
                    Return
                End If
                .ColumnFilters.LogicalOperator = FilterLogicalOperator.Or
                For Each searchKey In searchKeys.Split(",")
                    .ColumnFilters(searchKey.Trim).FilterConditions.Add(FilterComparisionOperator.Contains, txtSearch.Text.Trim())
                Next
            End With

            btnOk.Enabled = (grdList.Rows.Count > 0)
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Dim gridRow As UltraGridRow = grdList.ActiveRow

            DocumentNo = gridRow.Cells("code").Value
            SaleCode = gridRow.Cells("sale_code").Value

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Method"

    Private Sub RefreshList()
        Try
            Dim returns As New CustomerReturns

            Dim dt As New DataTable
            With New TaskProcess(Me, grdList)
                .ShowLoader = grdList.Visible
                .Start(
                    Sub()
                        dt = returns.GetList(CustomerCode, Now.Date)
                    End Sub,
                    Sub()
                        With grdList
                            .DataSource = dt
                            If .Rows.Count > 0 Then
                                .Rows(0).Activate()
                            Else
                                btnOk.Enabled = False
                            End If
                        End With
                    End Sub)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class