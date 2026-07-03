Imports Infragistics.Win.UltraWinGrid

Friend Class FormSalesBrowser

#Region "Declarations"

    Public Sub New(settings As SettingsPreferences)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

#End Region

#Region "Property"

    Property CustomerCode As String
    Property CustomerName As String
    Property SaleCode As String
    Property InvoiceCode As String
    Property PaymentCode As String
    Property TransactionDate As Date
    Property IsReprintReceipt As Boolean

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
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
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

    Private Sub grdList_DoubleClickRow(sender As Object, e As DoubleClickRowEventArgs) Handles grdList.DoubleClickRow
        Try
            btnOk.PerformClick()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns
                .Override.CellClickAction = CellClickAction.CellSelect

                With .Bands(0)
                    .Override.CellClickAction = CellClickAction.RowSelect

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "SaleCode"
                                    .CellClickAction = CellClickAction.CellSelect
                                    .Header.Caption = "Receipt No."
                                    .SetWidth(110, 120)

                                Case "Customer"
                                    .Width = 200

                                Case "Cashier"
                                    .Hidden = False

                                Case "IsScPwd"
                                    .Header.Caption = "SC/PWD"
                                    .SetWidth(60)
                                    .Style = ColumnStyle.CheckBox

                                Case "TotalAmount"
                                    .Hidden = True
                                    '.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    '.Format = "#,##0.00"
                                    '.Header.Caption = "Amount"
                                    '.SetWidth(60, 80)
                                    '.Style = ColumnStyle.DoubleNonNegative

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

    Private Sub grdList_InitializeRow(sender As Object, e As InitializeRowEventArgs) Handles grdList.InitializeRow
        Try
            With e.Row.Cells("PaymentCode")
                .ToolTipText = .Text
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_KeyDown(sender As Object, e As KeyEventArgs) Handles grdList.KeyDown
        Try
            If grdList.ActiveCell IsNot Nothing Then
                If e.KeyCode = Keys.C AndAlso e.Control Then
                    Clipboard.SetText(grdList.ActiveCell.Text)
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub txtSearch_ValueChanged(sender As Object, e As EventArgs) Handles txtSearch.ValueChanged
        Try
            Dim searchKeys As String = "SaleCode,Customer"

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
            Dim gridRow As UltraGridRow

            If grdList.ActiveRow.Band.Index > 0 Then
                gridRow = grdList.ActiveRow.ParentRow
            Else
                gridRow = grdList.ActiveRow
            End If

            SaleCode = gridRow.Cells("SaleCode").Value
            InvoiceCode = gridRow.Cells("InvoiceCode").Value
            PaymentCode = gridRow.Cells("PaymentCode").Value
            CustomerName = gridRow.Cells("Customer").Value

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

    Private Sub Initialize()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("PostingDate", GetType(Date))
                .Add("SaleCode")
                .Add("InvoiceCode")
                .Add("PaymentCode")
                .Add("Customer")
                .Add("Cashier")
                .Add("IsScPwd", GetType(Boolean))
                .Add("TotalAmount", GetType(Double))
            End With
            grdList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            Dim dt As New DataTable
            With New TaskProcess(Me, grdList)
                .ShowLoader = grdList.Visible
                .Start(
                    Sub()
                        dt = (New Sale).GetList(Today, Current.Settings.CashierSessionId, CustomerCode)
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