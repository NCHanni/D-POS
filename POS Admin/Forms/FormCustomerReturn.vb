Imports Infragistics.Win.UltraWinGrid

Friend Class FormCustomerReturn

#Region "Constructor"

    Public Sub New()
        Try
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.

            m_CustomerReturn = New CustomerReturn
            m_CustomerReturnList = New CustomerReturnsList
            m_ZReading = New ZReading
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Declarations"

    Private Const MODULE_NAME As String = "Customer Return"
    Private Const MODULE_GROUP As String = "Sales"

    Private m_Settings As SettingsPreferences
    Private ReadOnly m_CustomerReturn As CustomerReturn
    Private ReadOnly m_CustomerReturnList As CustomerReturnsList
    Private ReadOnly m_ZReading As ZReading
    Private m_CheckFormChanges As CheckFormChanges
    Private m_List As DataSet

    Private ReadOnly m_IsUpdating As Boolean
    Private m_RefreshOnListActivate As Boolean

    Private m_CreditMemoNo As String = ""
    Private m_CustomerCode As String = ""
    Private m_SalesInvoiceCode As String = "" 'NAV Posted Sales Invoice Code (of reference sale)
    Private m_SaleDate As Date
    Private m_VATSales As Double
    Private m_VATExemptSales As Double
    Private m_ZeroRatedSales As Double
    Private m_ScPwdDiscount As Double
    Private m_ScPwdLessVAT As Double
    Private m_IsProcessed As Boolean
    Private m_IsDraft As Boolean = True
    Private m_Operation As String = ""

    Enum ItemType
        None = 0
        GLAccount = 1
        Item = 2
        Resource = 3
        FixedAsset = 4
        ChargeItem = 5
        GiftCertificate = 6
    End Enum

    Private Enum VATTypes
        None
        VATable
        ZeroRated
        VATExempt
        NonVAT
    End Enum

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_CheckFormChanges.HasChanges Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

#Region "Text Boxes"

    Private Sub txtSalesCode_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles txtReferenceCode.EditorButtonClick
        Try
            If grdItems.Rows.Count > 0 Then
                If ShowQuestion("Changing the sales reference will clear the details.\n\nDo you want to continue?", "Confirm Change") = QuestionResult.No Then
                    Return
                End If
            End If

            Using frm As New FormSalesBrowser
                With frm
                    .SaleType = FormSalesBrowser.SaleTypes.SalesForReturn

                    If LaunchForm(frm) = Windows.Forms.DialogResult.OK Then
                        Clear(True)

                        m_SaleDate = .SelectedRow.Cells("transaction_date").Value
                        txtReferenceCode.Text = .SelectedRow.Cells("code").Value
                        m_CustomerCode = .SelectedRow.Cells("customer_code").Value
                        txtCustomer.Text = .SelectedRow.Cells("customer_name").Value
                        m_SalesInvoiceCode = .SelectedRow.Cells("invoice_code").Value

                        InitializeGridDetails()
                        EnableInformation(True)
                    End If
                End With
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnAddDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDetails.Click
        Try
            Using frm As New FormSalesDetailsBrowser(txtReferenceCode.Text)
                With frm
                    For Each row As UltraGridRow In grdItems.Rows
                        If .References = "" Then
                            .References = row.Cells("item_code").Text
                        Else
                            .References &= "," & row.Cells("item_code").Text
                        End If
                    Next

                    If LaunchForm(frm, , Me) = Windows.Forms.DialogResult.OK Then
                        Dim items As DataTable = grdItems.DataSource

                        For Each row As DataRow In frm.SelectedItems.Rows
                            Dim newRow As DataRow = items.NewRow

                            newRow("item_line_id") = GetNextItemLineID()
                            newRow("cr_code") = txtCode.Text
                            newRow("item_code") = row("item_code")
                            newRow("item_description") = row("description")
                            newRow("class_code") = row("class_code")
                            newRow("unit_of_measure") = row("unit_of_measure")
                            newRow("price") = row("price")
                            newRow("qty") = row("qty")
                            newRow("qty_returned") = row("qty_returned")
                            newRow("qty_per_uom") = row("qty_per_uom")
                            newRow("is_regular_discount") = row("is_regular_discount")
                            newRow("discount_percent") = row("discount_percent")
                            newRow("discount_amount") = row("discount_amount")
                            newRow("discounted_price") = row("discounted_price")
                            newRow("vat_percent") = row("vat_percent")
                            newRow("vat_amount") = row("vat_amount")
                            newRow("vat_exempt_amount") = row("vat_exempt_amount")
                            newRow("line_total") = row("line_total")
                            newRow("is_vatable") = row("is_vatable")
                            newRow("is_zero_rated") = row("is_zero_rated")
                            newRow("is_vat_exempt") = row("is_vat_exempt")
                            newRow("is_gift_certificate") = row("is_gift_certificate")

                            items.Rows.Add(newRow)
                        Next

                        ComputeTotals()
                    End If
                End With
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnRemoveDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveDetails.Click
        Try
            If grdItems.ActiveRow Is Nothing Then
                ShowSelectMessage("remove")
            Else
                If ShowRemoveQuestion() = QuestionResult.Yes Then
                    grdItems.ActiveRow.Delete(False)
                    ComputeTotals()
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdItems_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles grdItems.AfterCellUpdate
        Try
            If e.Cell.Column.Key = "qty_returned" Then
                With e.Cell.Row
                    If .Cells("discount_amount").Value > 0.0 Then
                        If CBool(.Cells("is_vat_exempt").Value) Then
                            .Cells("line_total").Value = (.Cells("discounted_price").Value * .Cells("qty_returned").Value)
                        Else
                            .Cells("line_total").Value = ((.Cells("discounted_price").Value + .Cells("vat_amount").Value) * .Cells("qty_returned").Value)
                        End If
                    Else
                        .Cells("line_total").Value = .Cells("price").Value * .Cells("qty_returned").Value
                    End If
                End With

                ComputeTotals()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdItems_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles grdItems.BeforeCellUpdate
        Try
            If e.Cell.Column.Key = "qty_returned" Then
                If IsDBNull(e.NewValue) Then
                    e.Cancel = True
                ElseIf e.NewValue > e.Cell.Row.Cells("qty").Value Then
                    ShowWarning("Returned cannot be more than the Qty Sold!", "Edit Failed")
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdItems_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdItems.InitializeLayout
        Try
            With e.Layout
                With .Bands(0)
                    .Override.MaxSelectedRows = 1
                    .Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True

                    For Each col As UltraGridColumn In .Columns
                        With col
                            Select Case .Key
                                Case "item_description"
                                    .CellActivation = Activation.NoEdit
                                    .Header.Caption = "Description"
                                    .MinWidth = 150
                                Case "qty"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Qty Sold"
                                    .SetWidth(60, 70)
                                Case "qty_returned"
                                    .CellActivation = Activation.AllowEdit
                                    .CellAppearance.BackColor = Color.LightYellow
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Qty Returned"
                                    .SetWidth(80)
                                Case "unit_of_measure"
                                    .CellActivation = Activation.NoEdit
                                    .Header.Caption = "UOM"
                                    .SetWidth(50, 60)
                                Case "price"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Price"
                                    .SetWidth(70, 80)
                                Case "discount_percent"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Discount (%)"
                                    .SetWidth(80)
                                Case "discounted_price"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Discounted Price"
                                    .SetWidth(100)
                                Case "line_total"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Line Total"
                                    .SetWidth(80)
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

#End Region

#Region "Control"

    Private Sub ctrlMenu_MenuClick(ByVal State As MenuButtons.MenuButtons) Handles ctrlMenu.MenuClick
        Try
            Select Case State
                Case MenuButtons.MenuButtons.New
                    Clear()
                    EnableInformation(True)
                    m_Operation = "INSERT"

                Case MenuButtons.MenuButtons.Edit
                    EnableInformation(True)
                    m_Operation = "UPDATE"

                Case MenuButtons.MenuButtons.Delete
                    If m_IsProcessed Then
                        ShowWarning("Selected record has already been processed!\n\nDelete is no longer allowed.", "Delete Failed")

                    ElseIf ShowDeleteQuestion() = QuestionResult.Yes Then
                        UIToClass()

                        With m_CustomerReturn
                            .Operation = "DELETE"
                            If .Save() Then
                                ShowDeleteMessage()
                                EnableInformation(False)
                                Clear()
                                m_RefreshOnListActivate = True
                            Else
                                ShowWarning("Something went wrong while deleting record!\n\nPlease try again later.", "Delete Failed")
                                ctrlMenu.ErrorOccured = True
                            End If
                        End With
                    Else
                        ctrlMenu.ErrorOccured = True
                    End If

                Case MenuButtons.MenuButtons.Save
                    ctrlMenu.ErrorOccured = Not SaveRecord()

                Case MenuButtons.MenuButtons.Cancel
                    If m_CheckFormChanges.HasChanges Then
                        If ShowCancelQuestion() = QuestionResult.No Then
                            ctrlMenu.ErrorOccured = True
                            Return
                        End If
                    End If

                    If m_Operation = "INSERT" Then
                        Clear()
                    Else
                        FillDetails()
                        m_Operation = ""
                    End If

                    EnableInformation(False)

                Case MenuButtons.MenuButtons.Print
                    With New Reporting
                        .HasRefundAccess = chkForPOSRefund.Visible
                        .ShowCustomerReturn(m_CustomerReturn.GetData)
                    End With
            End Select
        Catch ex As Exception
            ctrlMenu.ErrorOccured = True
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            With e.Layout
                With .Bands(0)
                    .Override.CellClickAction = CellClickAction.RowSelect

                    For Each col As UltraGridColumn In .Columns
                        With col
                            Select Case .Key
                                Case "transaction_date"
                                    .Header.Caption = "Date"
                                    .SetWidth(75)
                                    .Style = ColumnStyle.DateWithoutDropDown
                                Case "code"
                                    .Header.ToolTipText = "Return Code"
                                    .SetWidth(100)
                                Case "reference_code"
                                    .Header.Caption = "Reference"
                                    .Header.ToolTipText = "Sale/Invoice No."
                                    .SetWidth(100, 120)
                                Case "customer_name"
                                    .Header.Caption = "Customer"
                                Case "total_amount"
                                    .Style = ColumnStyle.DoubleNonNegative
                                Case "is_draft"
                                    .Style = ColumnStyle.CheckBox
                                    .Header.Caption = "Draft"
                                    .SetWidth(50, 60)
                                Case "is_processed"
                                    .Style = ColumnStyle.CheckBox
                                    .Header.Caption = "Processed"
                                    .SetWidth(70, 80)
                                Case "prepared_by"
                                    .Header.Caption = "Prepared by"
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next
                End With

                If .Bands.Count > 1 Then
                    With .Bands(1)
                        .Override.CellClickAction = CellClickAction.RowSelect

                        For Each col As UltraGridColumn In .Columns
                            With col
                                Select Case .Key
                                    Case "qty_returned"
                                        .Header.Caption = "Qty"
                                        .Header.ToolTipText = "Qty Returned"
                                        .SetWidth(50, 60)
                                    Case "uom"
                                        .Header.Caption = "UOM"
                                        .SetWidth(60, 70)
                                    Case "item_description"
                                        .Header.Caption = "Description"
                                    Case "price"
                                    Case "discount_percent"
                                        .Header.Caption = "Discount (%)"
                                    Case "discounted_price"
                                    Case "line_total"
                                    Case Else
                                        .Hidden = True
                                End Select
                            End With
                        Next
                    End With
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_OnRefresh() Handles ctrlList.OnRefresh
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_OnView(ByVal SelectedRow As UltraGridRow) Handles ctrlList.OnView
        Try
            If m_Operation <> "" Then
                If ShowSelectQuestion() = QuestionResult.No Then
                    Return
                End If
            End If

            With SelectedRow
                txtCode.Text = .Cells("code").Text
                txtReferenceCode.Text = .Cells("reference_code").Value
                m_CreditMemoNo = .Cells("credit_memo_no").Value
                dtpDate.Value = .Cells("transaction_date").Value
                m_CustomerCode = .Cells("customer_code").Text
                txtCustomer.Text = .Cells("customer_name").Text
                txtRemarks.Text = .Cells("remarks").Text
                numTotalVAT.Value = .Cells("total_vat").Value
                numTotalDiscount.Value = .Cells("total_discount").Value
                numTotalAmount.Value = .Cells("total_amount").Value
                chkForPOSRefund.Checked = CBool(.Cells("for_pos_refund").Value)
                m_IsDraft = CBool(.Cells("is_draft").Value)
                ctrlMenu.IsDraft = m_IsDraft
                m_IsProcessed = CBool(.Cells("is_processed").Value)
                m_VATSales = CDbl(.Cells("vat_sales").Value)
                m_VATExemptSales = CDbl(.Cells("vat_exempt_sales").Value)
                m_ZeroRatedSales = CDbl(.Cells("zero_rated_sales").Value)
                m_ScPwdDiscount = CDbl(.Cells("sc_pwd_discount").Value)
                m_ScPwdLessVAT = CDbl(.Cells("sc_pwd_less_vat").Value)
                m_Operation = ""
            End With

            FillDetails()

            ctrlMenu.SetMenuState(MenuButtons.MenuStates.Select)
            EnableInformation(False)
            tabCustomerReturn.Tabs(1).Selected = True

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Tab"

    Private Sub tabCustomerReturn_SelectedTabChanged(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles tabCustomerReturn.SelectedTabChanged
        Try
            If sender.SelectedTab.Index = 0 Then
                CancelButton = Me.ctrlList.ButtonCancel
                If m_RefreshOnListActivate Then
                    RefreshList()
                End If
            Else
                CancelButton = Me.ctrlMenu.ButtonCancel
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_CheckFormChanges = New CheckFormChanges
            m_CheckFormChanges.Add(grpHeader)
            m_CheckFormChanges.Add(grpDetails)

            InitializeGrid()
            InitializeGridDetails()

            With ctrlMenu
                .ModuleName = MODULE_NAME
                .ModuleGroup = MODULE_GROUP
                .SetToDefault()
            End With

            If Not Core.Current.Rights.IsAllowed("Refund Listing", Rights.AccessRights.View, "Sales") Then
                txtRemarks.Multiline = False
                txtRemarks.Height = 21
                chkForPOSRefund.Visible = False
                grpHeader.Height = 150
            End If

            EnableInformation(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeGrid()
        Try
            ctrlList.Initialize()

            Dim dt As New DataTable
            With dt.Columns
                .Add("code")
                .Add("entity_id", GetType(Integer))
                .Add("transaction_date", GetType(Date))
                .Add("reference_code")
                .Add("customer_code")
                .Add("customer_name")
                .Add("remarks")
                .Add("total_vat", GetType(Double))
                .Add("total_discount", GetType(Double))
                .Add("total_amount", GetType(Double))
                .Add("vat_sales", GetType(Double))
                .Add("vat_exempt_sales", GetType(Double))
                .Add("zero_rated_sales", GetType(Double))
                .Add("sc_pwd_discount", GetType(Double))
                .Add("sc_pwd_less_vat", GetType(Double))
                .Add("for_pos_refund", GetType(Boolean))
                .Add("is_draft", GetType(Boolean))
                .Add("is_processed", GetType(Boolean))
                .Add("status")
                .Add("prepared_by")
            End With

            m_List = New DataSet
            m_List.Tables.Add(dt)
            m_List.AcceptChanges()

            ctrlList.DataSource = m_List
            ctrlList.SearchKeys = "code,reference_code,customer_name"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeGridDetails()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("item_line_id", GetType(Integer))
                .Add("cr_code")
                .Add("item_code")
                .Add("qty", GetType(Double))
                .Add("qty_returned", GetType(Double))
                .Add("unit_of_measure")
                .Add("item_description")
                .Add("class_code")
                .Add("price", GetType(Double))
                .Add("qty_per_uom", GetType(Double))
                .Add("is_regular_discount", GetType(Boolean))
                .Add("discount_percent", GetType(Double))
                .Add("discount_amount", GetType(Double))
                .Add("discounted_price", GetType(Double))
                .Add("vat_percent", GetType(Double))
                .Add("vat_amount", GetType(Double))
                .Add("vat_exempt_amount", GetType(Double))
                .Add("line_total", GetType(Double))
                .Add("is_vatable", GetType(Boolean))
                .Add("is_zero_rated", GetType(Boolean))
                .Add("is_vat_exempt", GetType(Boolean))
                .Add("is_gift_certificate", GetType(Boolean))
            End With
            grdItems.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EnableInformation(enable As Boolean)
        Try
            grpHeader.Enabled = enable
            grpDetails.Enabled = enable AndAlso txtReferenceCode.TextLength > 0
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Clear(Optional keepOperation As Boolean = False)
        Try
            m_CreditMemoNo = ""
            m_CustomerCode = ""
            m_SalesInvoiceCode = ""
            m_IsProcessed = False
            m_IsDraft = True

            m_VATSales = 0.0
            m_VATExemptSales = 0.0
            m_ZeroRatedSales = 0.0
            m_ScPwdDiscount = 0.0
            m_ScPwdLessVAT = 0.0

            If Not keepOperation Then
                m_Operation = ""
            End If

            Core.Extensions.Clean(grpHeader)

            dtpDate.Value = DateTime.Now.Date

            InitializeGridDetails()

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadData()
        Try
            With m_CustomerReturnList
                .Fill(ctrlList.StartDate, ctrlList.EndDate)
                m_List = .Data
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            Dim th As New Threading.Thread(AddressOf LoadData)
            Dim wait As New BusinessPro.Core.Wait(th)

            If wait.Show(Me) = BusinessPro.Core.Wait.DialogResult.OK Then
                ctrlList.DataSource = m_List
                m_RefreshOnListActivate = False

                With ctrlMenu
                    If m_ZReading.IsFinalized Then
                        .AllowNew = False
                        .AllowEdit = False
                        .AllowDelete = False
                    Else
                        .AllowNew = True
                        .AllowEdit = True
                        .AllowDelete = True
                    End If
                    .SetToDefault()
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillDetails()
        Try
            With m_CustomerReturn
                .Code = txtCode.Text
                .GetDetails()
                grdItems.DataSource = .Details
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComputeTotals()
        Try
            Dim lineTotal As Double
            Dim vatAmount As Double
            Dim discountAmount As Double
            Dim totalVAT As Double
            Dim totalDiscount As Double
            Dim totalAmount As Double

            m_VATSales = 0.0
            m_VATExemptSales = 0.0
            m_ZeroRatedSales = 0.0
            m_ScPwdDiscount = 0.0
            m_ScPwdLessVAT = 0.0

            grdItems.UpdateData()

            For Each row As UltraGridRow In grdItems.Rows
                lineTotal = CDbl(row.Cells("line_total").Value)
                vatAmount = (row.Cells("vat_amount").Value * row.Cells("qty_returned").Value)
                discountAmount = (row.Cells("discount_amount").Value * row.Cells("qty_returned").Value)

                If CBool(row.Cells("is_zero_rated").Value) Then
                    m_ZeroRatedSales += lineTotal
                ElseIf CBool(row.Cells("is_vat_exempt").Value) Then
                    m_VATExemptSales += lineTotal
                    m_ScPwdLessVAT += CDbl(row.Cells("vat_exempt_amount").Value)
                ElseIf CBool(row.Cells("is_vatable").Value) Then
                    totalVAT += vatAmount
                    m_VATSales += (lineTotal - vatAmount)
                End If

                If Not CBool(row.Cells("is_regular_discount").Value) Then
                    m_ScPwdDiscount += discountAmount
                End If

                totalDiscount += discountAmount
                totalAmount += lineTotal
            Next

            numTotalDiscount.Value = totalDiscount
            numTotalAmount.Value = totalAmount
            numTotalVAT.Value = totalVAT
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub UIToClass()
        Try
            With m_CustomerReturn
                .Code = txtCode.Text
                .ReferenceCode = txtReferenceCode.Text
                .CreditMemoNo = m_CreditMemoNo
                .TransactionDate = dtpDate.Value
                .CustomerCode = m_CustomerCode
                .CustomerName = txtCustomer.Text
                .Remarks = txtRemarks.Text
                .TotalVAT = numTotalVAT.Value
                .TotalDiscount = numTotalDiscount.Value
                .TotalAmount = numTotalAmount.Value
                .VATSales = m_VATSales
                .VATExemptSales = m_VATExemptSales
                .ZeroRatedSales = m_ZeroRatedSales
                .ScPwdDiscount = m_ScPwdDiscount
                .ScPwdLessVAT = m_ScPwdLessVAT
                .ForPOSRefund = chkForPOSRefund.Checked
                .IsDraft = m_IsDraft
                .UserName = Core.Current.User.Name
                .Details = grdItems.DataSource
                .Operation = m_Operation
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            If (New ZReading).IsFinalized Then
                ShowWarning("End-of-Day (Z-Reading) Report has already been finalized.\n\nTransactions are no longer allowed for the day.", "Save Failed")
                Return False
            ElseIf chkForPOSRefund.Checked Then
                If m_SaleDate.Date <> Today.Date Then
                    ShowWarning("Refund is only valid for sale transactions within the day!", "Save Failed")
                    Return False
                End If
            End If

            ClearInvalidFields()

            grpHeader.GetErrors()
            grdItems.GetErrors("Item Details", , "cr_code,uom,bin", "qty", True)

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields())
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetNextItemLineID() As Integer
        Try
            Dim i As Integer = 0
            Dim itemLineId As Integer

            For Each row As UltraGridRow In grdItems.Rows
                itemLineId = CInt(row.Cells("item_line_id").Value)
                If itemLineId > i Then
                    i = itemLineId
                End If
            Next

            Return i + 1
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function SaveRecord() As Boolean
        Try
            If Not IsValid() Then
                Return False
            End If

            If ShowSaveDraftQuestion(Me) = Windows.Forms.DialogResult.Yes Then
                m_IsDraft = True
            Else
                m_IsDraft = False
            End If

            UIToClass()

            With m_CustomerReturn
                If .Save() Then
                    txtCode.Text = .Code
                    FillDetails()

                    If m_Operation = "INSERT" Then
                        SaveAuditTrail(If(.IsDraft, "Draft", "New") & " Customer Return created - " & .Code, Me)
                        ShowSaveMessage()
                    Else
                        ShowUpdateMessage()
                        SaveAuditTrail(If(.IsDraft, "Draft", "") & "Customer Return updated - " & .Code, Me)
                    End If

                    m_Operation = ""
                    m_CheckFormChanges.ClearChanges()

                    EnableInformation(False)
                    ctrlMenu.IsDraft = m_IsDraft
                    m_RefreshOnListActivate = True
                    Return True
                Else
                    ShowWarning("Something went wrong while saving record!\n\nPlease try again later.", "Save Failed")
                    Return False
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class