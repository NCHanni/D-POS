Imports System.Web.Services.Description
Imports Infragistics.Win.UltraWinEditors
Imports Infragistics.Win.UltraWinGrid

Friend Class FormMain

#Region "Constructors"

    Public Sub New(settings As SettingsPreferences)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings

        Current.FormatUI.Apply(Me)
    End Sub

#End Region

#Region "Constants"

    Private Const NAV_TRANSACTED_FROM As String = "BPRO"
    Private Const DEFAULT_UOM As String = "Piece"
    Private Const DEBUG_MODE As String = "DEBUG MODE"
    Private Const GC_CODE As String = "GC" ' Gift Certificate code @ inventory.item_classes
    Private Const GC_UOM_NAV As String = "PC"
    Private Const DISCOUNT_GROUP_SC As String = "SC"
    Private Const DISCOUNT_GROUP_PWD As String = "PWD"

#End Region

#Region "Declarations"

    Private m_ShowSettingsColumns As Boolean
    Private m_IsWideScreen As Boolean
    Private ReadOnly m_IsGiftCertEnabled As Boolean

    Private m_Customer As Customer
    Private m_CustomerCode As String = String.Empty
    Private m_CustomerName As String = String.Empty
    Private m_IsCustomerScPwd As Boolean
    Private m_ScPwdDiscountData As DataTable
    Private m_LoggingOff As Boolean
    Private m_OnRestart As Boolean

    Private ReadOnly m_Settings As SettingsPreferences
    Private m_AuditTrail As AuditTrail

    Private m_TotalSales As Double = 0.0
    Private m_TotalVATAmount As Double = 0.0
    Private m_TotalVATableSales As Double = 0.0
    Private m_TotalNonVATSales As Double = 0.0
    Private m_TotalZeroRatedSales As Double = 0.0
    Private m_TotalRegularDiscount As Double = 0.0
    Private m_TotalRegularLessVAT As Double = 0.0
    Private m_TotalSCPwdDiscount As Double = 0.0
    Private m_TotalScPwdVATExempt As Double = 0.0

    Private m_dtItemsSale As DataTable
    Private m_dtItemsList As DataTable

    Private m_SalespersonList As Salesperson.Struct()
    Private m_HasSalespersons As Boolean

    Private m_LoadItemsDone As Boolean
    Private m_InvoiceNo As String = String.Empty
    Private m_SaleCode As String = String.Empty
    Private m_SuspendSaleCode As String = String.Empty
    Private m_PaymentCode As String = String.Empty
    Private m_SalesCreditMemoNo As String = String.Empty
    Private m_LoadedFromSales As Boolean
    Private m_HasCreditMemo As Boolean
    Private m_RefundSale As Boolean
    Private m_VoidAllowed As Boolean
    Private m_HasDiscount As Boolean
    Private m_frmItemBrowserCancelled As Boolean

    Private m_frmLoadButtonsHandled As Boolean
    Private WithEvents m_frmLoadButtons As FormLoadSalesButtons

    Private m_CreditMemoData As DataTable
    Private m_GiftCertData As DataTable
    Private m_CreditMemoAmount As Double
    Private m_TotalDeductions As Double

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_OnRestart Then
                Return ' Ignore other checking below
            End If

            If grdItems.Rows.Count > 0 AndAlso Not m_LoadedFromSales Then
                ShowMessage("Application cannot be exited because there is an active transaction.", "Exit Failed", MessageIcon.Exclamation)
                e.Cancel = True
                Exit Sub
            Else
                If Not m_LoggingOff Then
                    If ShowExitQuestion() = QuestionResult.No Then
                        e.Cancel = True
                    End If
                End If
            End If

            If Not e.Cancel AndAlso Not m_LoggingOff Then
                SaveAuditTrail("Suspend Cashier Session - " & Current.User.Name)
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Delete
                    If Not m_VoidAllowed Then
                        RemoveItem(e)
                    End If

                Case Keys.Back, Keys.Left, Keys.Right
                'active object will handle these keys

                Case Keys.Up, Keys.Down
                    If grdItems.Rows.Count > 0 Then
                        If grdItems.ActiveRow Is Nothing Then
                            If e.KeyCode = Keys.Up Then
                                grdItems.ActiveRow = grdItems.Rows(grdItems.Rows.Count - 1)
                            Else
                                grdItems.ActiveRow = grdItems.Rows(0)
                            End If
                        End If
                        If e.KeyCode = Keys.Up Then
                            If grdItems.ActiveRow.Index = 0 Then
                                grdItems.ActiveRow = grdItems.Rows(grdItems.Rows.Count - 1)
                            Else
                                grdItems.ActiveRow = grdItems.Rows(grdItems.ActiveRow.Index - 1)
                            End If
                        Else
                            If grdItems.ActiveRow.Index = grdItems.Rows.Count - 1 Then
                                grdItems.ActiveRow = grdItems.Rows(0)
                            Else
                                grdItems.ActiveRow = grdItems.Rows(grdItems.ActiveRow.Index + 1)
                            End If
                        End If
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

                Case Keys.F1 'Shift+F1 for released version
#If Not DEBUG Then
                    If Not e.Shift Then
                        Return
                    End If
#End If
                    m_ShowSettingsColumns = Not m_ShowSettingsColumns

                    If picBanner.Image IsNot Nothing Then
                        picBanner.Visible = Not m_ShowSettingsColumns
                        picSeparator.Visible = picBanner.Visible
                    End If

                    grdItems.DataBind()
                    grdItems.Refresh()

                Case Keys.F2
                    btnSelectItem.PerformClick()

                Case Keys.F3
                    If e.Control Then
                        btnScPwd.PerformClick()
                    Else
                        btnSelectCustomer.PerformClick()
                    End If

                Case Keys.F4
                    If e.Control Then
                        btnReprintReceipt.PerformClick()
                    Else
                        btnPriceLookUp.PerformClick()
                    End If

                Case Keys.F5
                    If e.Control Then
                        btnLoadSale.PerformClick()
                    Else
                        btnSetQty.PerformClick()
                    End If

                Case Keys.F6
                    If e.Control Then
                        btnSalesperson.PerformClick()
                    Else
                        btnSetDiscount.PerformClick()
                    End If

                Case Keys.F7
                    If e.Control Then
                        btnVoidSale.PerformClick()
                    Else
                        btnSuspendSale.PerformClick()
                    End If

                Case Keys.F8
                    If e.Control Then
                        btnRefundSale.PerformClick()
                    Else
                        btnCreditMemo.PerformClick()
                    End If

                Case Keys.F9
                    btnCheckOut.PerformClick()

                Case Keys.F10, Keys.Escape
                    btnCancelSale.PerformClick()

                Case Keys.F11
                    If e.Control Then
                        Static normalWindowState As FormWindowState
                        If Me.FormBorderStyle = FormBorderStyle.None Then
                            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
                            Me.WindowState = normalWindowState
                        Else
                            normalWindowState = Me.WindowState
                            Me.SuspendLayout()
                            Me.ResizeRedraw = False
                            Me.WindowState = FormWindowState.Normal
                            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                            Me.WindowState = FormWindowState.Maximized
                            Me.ResizeRedraw = True
                            Me.ResumeLayout()
                        End If
                    Else
                        btnCashUp.PerformClick()
                    End If

                Case Keys.F12
                    If e.Control Then
                        Me.Close()
                    Else
                        btnLogOff.PerformClick()
                    End If

                Case Else
                    If e.Control Then
                        Select Case e.KeyCode
                            Case Keys.Add, Keys.Oemplus
                                If grdItems.Font.Size < 16 Then
                                    grdItems.Font = New Font(grdItems.Font.FontFamily, grdItems.Font.Size + 1)
                                End If

                            Case Keys.Subtract, Keys.OemMinus
                                If grdItems.Font.Size > 8 Then
                                    grdItems.Font = New Font(grdItems.Font.FontFamily, grdItems.Font.Size - 1)
                                End If

                            Case Else
                                Return
                        End Select

                        e.SuppressKeyPress = True
                        e.Handled = True
                    Else
                        If Not txtSKU.Focused Then
                            Dim keysNumPad As Keys() = {
                                Keys.NumPad0,
                                Keys.NumPad1,
                                Keys.NumPad2,
                                Keys.NumPad3,
                                Keys.NumPad4,
                                Keys.NumPad5,
                                Keys.NumPad6,
                                Keys.NumPad7,
                                Keys.NumPad8,
                                Keys.NumPad9
                            }

                            Dim val As Integer = Array.IndexOf(keysNumPad, e.KeyCode)

                            If val > -1 Then
                                txtSKU.Text &= val.ToString
                            Else
                                Dim ch As Char = Chr(e.KeyValue)
                                If Char.IsLetterOrDigit(ch) Then

                                    Dim kb As New Devices.Keyboard
                                    If e.Shift Then
                                        If kb.CapsLock Then
                                            txtSKU.Text &= ch.ToString.ToLower
                                        Else
                                            txtSKU.Text &= ch.ToString.ToUpper
                                        End If
                                    Else
                                        If kb.CapsLock Then
                                            txtSKU.Text &= ch.ToString.ToUpper
                                        Else
                                            txtSKU.Text &= ch.ToString.ToLower
                                        End If
                                    End If
                                End If
                            End If

                            txtSKU.Focus()
                            txtSKU.SelectionStart = txtSKU.TextLength
                        End If
                    End If
            End Select
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            With sbarMain
                .Panels("Cashier").Text = "Cashier: " & Current.User.Name
                .Panels("Company").Text = "Company: " & Current.CompanyInfo.Name
                .Panels("MachineId").Text = "MIN: " & Current.CompanyInfo.PermitMIN
                .Panels("Terminal").Text = "Terminal: " & Current.Settings.TerminalCode
                .Panels("Status").Appearance.ForeColor = Color.Blue

                Dim conString As String = Core.Current.ConnectionString.LocalConnectionString

                If conString.StartsWith("Data Source=.\") OrElse conString.StartsWith("Data Source=" & My.Computer.Name) Then
                    .Panels("Status").Text = "Local Connection"
                Else
                    .Panels("Status").Text = "Online via LAN"
                End If

                .Panels("Status").Text &= " @ " & conString.Substring(12, conString.IndexOf(";") - 12)
            End With

            pnlTitle.BackColor = Color.FromArgb(27, 117, 186)
            grpItemSelector.UseAppStyling = False
            grpItemSelector.Appearance.BackColor = pnlTitle.BackColor

            lblDescription.Text = My.Application.Info.Description
            lblCompany.Text = My.Application.Info.CompanyName & " © " & Today.Year
            lblVersion.Text = String.Format("v{0} Build {1}", Application.ProductVersion, My.Application.Info.Version.Build)

            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Try
            lblShippingAddressCaption.Visible = (Me.Width >= 1140)
            lblShippingAddress.Visible = lblShippingAddressCaption.Visible
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        Try
            picBanner.MaximumSize = picBanner.Size
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
        Try
            If m_Settings.FullscreenCashier Then
                SendKeys.Send("^{F11}") ' Ctrl+F11
                Application.DoEvents()
            End If

            Dim cu As New CashUp
            If Current.Settings.CashInSet Then
                If Current.Settings.CashInDate.Date < Today.Date Then
                    btnCashUp.PerformClick()
                ElseIf Current.Settings.CashInDate.Date > Today.Date Then
                    Me.Enabled = False
                    ShowWarning("Cash Up has been initialized prior to the current system date!\n\nPlease contact your system administrator.", "")
                    m_LoggingOff = True
                    Close()
                End If
            Else
                Using f As New FormCashIn
                    With f
                        Me.Enabled = False
                        Application.DoEvents()

                        If LaunchForm(f) = DialogResult.Cancel Then
                            m_LoggingOff = True
                            Me.Close()
                            Return
                        Else
                            Me.Enabled = True
                        End If

                        cu.Save(Current.Settings.TerminalCode, Current.User.Code, .txtAmount.Value)

                        Main.SetSessionSettings(
                            cu.DaySessionId,
                            cu.CashierSessionId,
                            cu.ServerDate,
                            .txtAmount.Value,
                            True
                        )

                        Try
                            Today = cu.ServerDate
                            TimeOfDay = cu.ServerDate
                        Catch
                        End Try

                        SaveAuditTrail("Cashed-in amount of " & CDbl(.txtAmount.Value).ToString("#,##0.00"))
                    End With
                End Using
            End If

            Form_Resize(Me, New EventArgs)
            LoadItemList(True)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnSelectItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectItem.Click
        Try
            If String.IsNullOrWhiteSpace(m_CustomerCode) Then
                ShowWarning("Please select a customer first to continue!", "Select Item Failed")
                Return
            End If

            Using frm As New FormItemBrowser(m_Settings)
                With frm
                    .IsCustomerScPwd = m_IsCustomerScPwd
                    .CategoryList = Nothing
                    .ItemList = m_dtItemsList
                    .HasVATExclusiveItems = False

                    If LaunchForm(frm, True, Me) = DialogResult.OK Then

                        For Each item As DataRow In frm.SelectedItems.Rows
                            AddItem(item)
                        Next

                        ComputeTotals()
                    End If
                End With
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnSelectCustomer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectCustomer.Click
        Try
            Const moduleName As String = "Customers"
            Const moduleGroup As String = "POS"

            Dim customerCode As String = If(IsWalkinCustomer(), "", m_CustomerCode)
            Dim showInformation As Boolean = Current.Rights.IsAllowed(moduleName, moduleGroup)

            Using frm As New FormCustomer(True, showInformation)
                With frm
                    .Code = customerCode
                    .ModuleName = moduleName
                    .ModuleGroup = moduleGroup
                    .TerminalCode = Current.Settings.TerminalCode
                    .UserCode = Current.User.Code
                    .UserCompany = Current.CompanyInfo.Name
                    .UserName = Current.User.Name

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                        If customerCode = .Code Then
                            Return
                        Else
                            If grdItems.Rows.Count > 0 Then
                                If ShowQuestion("There is an active transaction.\n\nDo you want to cancel current transaction?", "Select Customer") = QuestionResult.No Then
                                    Return
                                End If
                            End If

                            CleanScPwdData()
                        End If

                        Dim discountType As String = GetDiscountType(IIf(.Data.DiscountType <> "", .Data.DiscountType, .Data.CustomerDiscGroup))

                        If IsDiscountTypeSCPWD(discountType) Then
                            Dim copyScPwdData As DataTable = m_ScPwdDiscountData
                            Dim copyCustDetails As Customer = m_Customer

                            m_Customer = .Data
                            m_IsCustomerScPwd = True

                            btnSelectCustomer.Tag = frm
                            btnScPwd_Click(sender, e)
                            btnSelectCustomer.Tag = Nothing

                            If m_ScPwdDiscountData.Rows.Count = 0 Then
                                m_ScPwdDiscountData = copyScPwdData
                                m_Customer = copyCustDetails
                                Return
                            End If
                        End If
                    ElseIf .IsDeleted Then
                        InitializeWalkInCustomer()
                        Clean()
                        Return
                    Else
                        Return
                    End If

                    Clean(m_IsCustomerScPwd)

                    m_Customer = .Data
                    m_Customer.AllowLineDisc = Current.Rights.IsAllowed("Set Discount", "POS")
                End With
            End Using

            With m_Customer
                m_CustomerCode = .Code
                m_CustomerName = .Name

                .CustomerDiscGroup = CStrEx(.CustomerDiscGroup).Trim
                .CustomerPriceGroup = CStrEx(.CustomerPriceGroup).Trim

                lblCustomerName.Text = .Name & If(.CustomerDiscGroup = "", "", " - " & .CustomerDiscGroup)

                If String.IsNullOrWhiteSpace(.Address) OrElse Me.Width < 1200 Then
                    lblShippingAddressCaption.Text = String.Empty
                    lblShippingAddress.Text = String.Empty
                Else
                    lblShippingAddressCaption.Text = "Shipping Address"
                    lblShippingAddress.Text = CStr(CStrEx(.Address) & " " & CStrEx(.Address2)).Trim
                End If
            End With

            btnCreditMemo.CheckAccess()
            btnCheckOut.Enabled = grdItems.Rows.Count > 0
            btnCancelSale.Enabled = True
            btnSuspendSale.CheckAccess()

            SaveAuditTrail(String.Format("Customer Selected" & If(m_IsCustomerScPwd, " (SC/PWD)", "") & " - {0} - {1}", m_CustomerCode, m_CustomerName), False)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnScPwd_Click(sender As Object, e As EventArgs) Handles btnScPwd.Click
        Try
            Dim hasScPwdData As Boolean = (m_ScPwdDiscountData.Rows.Count > 0)
            Dim authorizedBy As String = String.Empty

            If Not hasScPwdData AndAlso sender Is btnScPwd Then
                If grdItems.Rows.Count > 0 Then
                    If ShowQuestion("There is an active transaction.\n\nDo you want to cancel current transaction?", "Select Customer") = QuestionResult.No Then
                        Return
                    End If
                End If

#If DEBUG Then
                Clean()
                authorizedBy = DEBUG_MODE
#Else
                Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "SENIOR CITIZEN / PWD DISCOUNT"}
                    If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                        Clean()
                        authorizedBy = frmAuth.AuthorizedBy
                    Else
                        Return
                    End If
                End Using
#End If
            End If

            Using dialog As New FormSeniorDiscountDialog
                With m_Customer
                    If m_IsCustomerScPwd Then
                        dialog.CustomerCode = .Code
                    Else
                        dialog.CustomerCode = m_CustomerCode
                    End If

                    dialog.IsWalkin = IsWalkinCustomer()
                    dialog.Data = m_ScPwdDiscountData.Copy
                    dialog.DiscountType = GetDiscountType(IIf(.DiscountType <> "", .DiscountType, .CustomerDiscGroup))
                    dialog.CustomerName = .Name
                    dialog.IDNo = .ScPwdId
                    dialog.Gender = .Gender

                    Date.TryParse(.BirthDate, dialog.BirthDate)
                    Date.TryParse(.DateIssued, dialog.IssuedDate)
                End With

                If LaunchForm(dialog, True, If(sender Is btnSelectCustomer, btnSelectCustomer.Tag, Me)) = Windows.Forms.DialogResult.OK Then
                    m_ScPwdDiscountData = dialog.Data

                    If sender Is btnScPwd Then
                        Dim isRegCustomer As Boolean = Not IsWalkinCustomer()

                        If m_ScPwdDiscountData.Rows.Count > 0 AndAlso (Not isRegCustomer OrElse (isRegCustomer AndAlso Not m_IsCustomerScPwd)) Then
                            InitializeWalkInCustomer(m_ScPwdDiscountData.Rows(0).Item("discount_type"))
                            m_IsCustomerScPwd = True

                        ElseIf m_ScPwdDiscountData.Rows.Count = 0 AndAlso hasScPwdData Then
                            InitializeWalkInCustomer()
                        End If
                    End If

                    btnCreditMemo.CheckAccess()
                    btnCheckOut.Enabled = True
                    btnCancelSale.Enabled = True
                    btnSuspendSale.CheckAccess()

                    If m_ScPwdDiscountData.Rows.Count > 0 AndAlso sender Is btnScPwd Then
                        SaveAuditTrail(
                            String.Format(
                                "Customer Selected (SC/PWD) - {0} - {1}|Authorized By: {2}",
                                    m_ScPwdDiscountData.Rows(0).Item("id_no"),
                                    m_ScPwdDiscountData.Rows(0).Item("name"),
                                    authorizedBy), False)
                    End If

                ElseIf sender Is btnSelectCustomer Then
                    m_ScPwdDiscountData.Rows.Clear()
                End If
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnPriceLookUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPriceLookUp.Click
        Try
            Using frm As New FormPriceLookUp(m_Settings)
                With frm
                    .IsCustomerScPwd = m_IsCustomerScPwd
                    .ItemList = m_dtItemsList
                    .HasVATExclusiveItems = False
                End With
                LaunchForm(frm, True, Me)
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnReprintReceipt_Click(sender As Object, e As EventArgs) Handles btnReprintReceipt.Click
        Try
            ReprintSalesReceipt()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnSetQty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetQty.Click
        Try
            Dim row As UltraGridRow

            If Not IsNothing(grdItems.ActiveRow) Then
                row = grdItems.ActiveRow
            Else
                row = grdItems.Rows(grdItems.Rows.Count - 1)
            End If

            Using frm As New FormSetQty(m_Settings)
                With frm
                    .UnitPrice = If(row.Cells("BasePrice").Value > 0.0, row.Cells("BasePrice").Value, row.Cells("UnitPrice").Value)
                    .OldQuantity = (row.Cells("Quantity").Value * row.Cells("QtyPerUOM").Value)
                    .QtyPerUOM = row.Cells("QtyPerUOM").Value
                    .UOMCode = row.Cells("UnitOfMeasure").Value

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK AndAlso .OldQuantity <> .NewQuantity Then
#If Not DEBUG Then
                        If .OldQuantity > .NewQuantity AndAlso m_Settings.AuthorizeSaleQuantityReduction Then
                            Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "DECREASE ITEM QUANTITY"}
                                If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                                    Dim itemDesc As String = row.Cells("Description").Value

                                    If itemDesc.Contains(vbCrLf) Then
                                        itemDesc = itemDesc.Substring(0, itemDesc.IndexOf(vbCrLf))
                                    End If

                                    SaveAuditTrail(
                                        String.Format("Item Quantity Reduced from {0} to {1}|Item: {2}|Authorized By: {3}",
                                            .OldQuantity,
                                            .NewQuantity,
                                            itemDesc,
                                            frmAuth.AuthorizedBy))
                                Else
                                    Return
                                End If
                            End Using
                        End If
#End If

                        row.Cells("Quantity").Value = .NewQuantity

                        ComputeVATDiscount(DirectCast(row.ListObject, DataRowView).Row, True, True)
                        ComputeLineTotal(row)
                        ComputeTotals()
                    End If
                End With
            End Using

            txtSKU.Focus()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnSetDiscount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetDiscount.Click
        Try
            Dim row As UltraGridRow
            Dim isViewMode As Boolean
            Dim authorizedBy As String = DEBUG_MODE

            If grdItems.ActiveRow Is Nothing Then
                row = grdItems.Rows(grdItems.Rows.Count - 1)
            Else
                row = grdItems.ActiveRow
            End If

            If m_IsCustomerScPwd Then
                isViewMode = Not CBool(row.Cells("DiscountIsRegular").Value)
            End If

            If Not isViewMode Then
#If DEBUG Then
                authorizedBy = DEBUG_MODE
#Else
                Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "SET ITEM DISCOUNT"}
                    If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                        authorizedBy = frmAuth.AuthorizedBy
                    Else
                        Return
                    End If
                End Using
#End If
            End If

            Using frm As New FormSetDiscount
                With frm
                    .UnitPrice = row.Cells("UnitPrice").Value
                    .DiscountPercent = row.Cells("DiscountPercent").Value
                    .DiscountAmount = row.Cells("DiscountAmount").Value
                    .DiscountedPrice = row.Cells("DiscountedPrice").Value
                    .VATPercent = row.Cells("VATPercent").Value
                    .VATAmountComputed = row.Cells("VATAmount").Value
                    .IsVatable = row.Cells("IsVATable").Value
                    .IsVatExempt = row.Cells("IsVATExempt").Value
                    .ViewMode = isViewMode

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK AndAlso .DiscountPercentOld <> .DiscountPercent Then
                        row.Cells("DiscountPercent").Value = .DiscountPercent
                        row.Cells("DiscountAmount").Value = .DiscountAmount
                        row.Cells("DiscountedPrice").Value = .DiscountedPrice

                        If CBool(row.Cells("DiscountIsRegular").Value) Then
                            row.Cells("VATExemptAmount").Value = (.VATAmount - .VATAmountComputed)
                        End If

                        Dim hasDiscount As Boolean = m_HasDiscount

                        ComputeVATDiscount(DirectCast(row.ListObject, DataRowView).Row, True, True)
                        ComputeLineTotal(row)
                        ComputeTotals()

                        If hasDiscount <> m_HasDiscount Then
                            grdItems.DataBind()
                        End If

                        Dim itemDesc As String = row.Cells("Description").Value
                        If itemDesc.Contains(vbCrLf) Then
                            itemDesc = itemDesc.Substring(0, itemDesc.IndexOf(vbCrLf))
                        End If

                        If .DiscountPercentOld = 0.0 Then
                            SaveAuditTrail(
                                String.Format("Discount Applied - " & .DiscountPercent.ToString("#0.##") & "%|Item: {0}|Authorized By: {1}",
                                    itemDesc,
                                    authorizedBy))
                        ElseIf .DiscountPercent = 0.0 Then
                            SaveAuditTrail(
                                String.Format("Discount Removed - from {0}%|Item: {1}|Authorized By: {2}",
                                    .DiscountPercentOld.ToString("#0.##"),
                                    itemDesc,
                                    authorizedBy))
                        Else
                            SaveAuditTrail(
                                String.Format("Discount Changed - from {0}% to {1}%|Item: {2}|Authorized By: {3}",
                                    .DiscountPercentOld.ToString("#0.##"),
                                    .DiscountPercent.ToString("#0.##"),
                                    itemDesc,
                                    authorizedBy))
                        End If
                    End If
                End With
            End Using

            txtSKU.Focus()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnLoadSale_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadSale.Click
        Try
            m_frmLoadButtonsHandled = False
            If m_frmLoadButtons Is Nothing Then
                m_frmLoadButtons = New FormLoadSalesButtons(btnRefundSale.Visible)
                With m_frmLoadButtons
                    .AllowPostedSales = (Current.Rights.IsAllowed(btnLoadSale.Tag) OrElse Current.Rights.IsAllowed(btnVoidSale.Tag))
                    .AllowSuspendedSales = Current.Rights.IsAllowed("Suspend Sale", Rights.AccessRights.View, "POS")
                    .Left = btnLoadSale.PointToScreen(New Point(btnLoadSale.Width, 0)).X - .Width
                    .ShortcutControl = True
                    .ShortcutKey = Keys.F5
                    .StartPosition = FormStartPosition.Manual
                    .Tag = btnLoadSale.Text
                    .Top = btnLoadSale.PointToScreen(New Point(0, - .Height)).Y
                    .Show(Me)
                End With
            Else
                m_frmLoadButtons.Close()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnSalesperson_Click(sender As Object, e As EventArgs) Handles btnSalesperson.Click
        Try
            Using f As New FormSetSalesperson()
                f.SalesPersonList = m_SalespersonList
                f.SalespersonCode = m_Customer.SalesPersonCode
                f.SalespersonName = lblSalesperson.Text
                If LaunchForm(f,, Me) = DialogResult.OK Then
                    m_Customer.SalesPersonCode = f.SalespersonCode
                    lblSalesperson.Text = f.SalespersonName
                End If
            End Using
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnVoidSale_Click(sender As Object, e As EventArgs) Handles btnVoidSale.Click
        Try
            Dim voidReason As String
            Dim authorizedBy As String = String.Empty

            Using frmAuth As New FormAuthorization(m_Settings, True) With {.Message = "VOID POSTED SALE"}
                If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                    voidReason = frmAuth.Reason
                    authorizedBy = frmAuth.AuthorizedBy
                Else
                    Return
                End If
            End Using

            Dim isVoid As Boolean
            Dim voidCode As String = String.Empty

            With New TaskProcess(Me, grdItems)
                .ShowLoader = grdItems.Visible
                .Start(
                    Sub()
                        Try
                            Dim doVoidSale As Boolean

                            m_SalesCreditMemoNo = String.Empty
                            doVoidSale = True

                            If doVoidSale Then
                                With New Sale
                                    .TerminalCode = Current.Settings.TerminalCode
                                    .CashierCode = Current.User.Code
                                    .Code = m_SaleCode
                                    .InvoiceCode = m_InvoiceNo
                                    .PaymentCode = m_SalesCreditMemoNo ' Posted Credit Memo No. (NAV)
                                    .TotalAmount = m_TotalSales

                                    voidCode = .Void(voidReason, authorizedBy)
                                    isVoid = Not String.IsNullOrWhiteSpace(voidCode)
                                End With
                            End If
                        Catch ex As Exception
                            ex.ShowError()
                        End Try
                    End Sub,
                    Sub()
                        If isVoid Then
                            PrintVoidedSalesReceipt(m_SaleCode)

                            SaveAuditTrail(
                                String.Format("Void Sale - {0}|Reference Invoice No: {1}|Authorized By: {2}",
                                    voidCode,
                                    m_SaleCode,
                                    authorizedBy),
                                False)

                            Clean()

                            ShowMessage("You have successfully void the sale.", "Void Sale")
                        End If
                    End Sub)
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnRefundSale_Click(sender As Object, e As EventArgs) Handles btnRefundSale.Click
        Try
            'No authorization needed, customer return is already processed by an admin

            Dim refundSucessful As Boolean
            Dim refundCode As String = ""

            With New TaskProcess(Me, grdItems)
                .ShowLoader = grdItems.Visible
                .Start(
                    Sub()
                        Try
                            Dim isProcessed As Boolean

                            With New CustomerReturns
                                isProcessed = .Process(m_InvoiceNo, Current.User.Name)
                            End With

                            If isProcessed Then
                                With New Refund
                                    .CustomerReturnCode = m_InvoiceNo
                                    .SaleCode = m_SaleCode
                                    .TransactionDate = Now
                                    .CustomerCode = m_CustomerCode
                                    .CustomerName = m_CustomerName
                                    .TotalAmount = m_TotalSales
                                    .CashierSessionId = Current.Settings.CashierSessionId
                                    .CashierCode = Current.User.Code
                                    .Cashier = Current.User.Name
                                    .Terminal = Current.Settings.TerminalCode
                                    .Details = grdItems.DataSource

                                    refundSucessful = .Save()
                                    refundCode = .Code
                                End With
                            End If
                        Catch ex As Exception
                            ex.ShowError()
                        End Try
                    End Sub,
                    Sub()
                        If refundSucessful Then
                            PrintRefundSaleReceipt(refundCode)

                            SaveAuditTrail(
                                String.Format("Refund Sale - {0}|:Customer Return Code {1}",
                                    refundCode,
                                    m_InvoiceNo),
                                False)

                            ShowMessage("Refund sale has been successfully processed.", "Refund Successful")
                            Clean()
                        End If
                    End Sub)
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnSuspendSale_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuspendSale.Click
        Try
            If grdItems.Rows.Count > 0 Then
                Dim suspendSuccessful As Boolean

                With New TaskProcess(Me, grdItems)
                    .ShowLoader = grdItems.Visible
                    .Start(
                        Sub()
                            Try
                                suspendSuccessful = SuspendSales()
                            Catch ex As Exception
                                ex.ShowError()
                            End Try
                        End Sub,
                        Sub()
                            If suspendSuccessful Then
                                SaveAuditTrail(
                                String.Format(
                                    "Suspend Sale - {0}|Total Amount: {1}",
                                        m_SuspendSaleCode,
                                        m_TotalSales.ToString("#,##0.00")), False)

                                Clean()
                                ShowMessage("Sale has been successfully suspended.", "Suspend Successful")
                            End If
                        End Sub)
                End With
            Else
                ShowWarning("There are no items to suspend!", "Suspend Failed")
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCreditMemo_Click(sender As Object, e As EventArgs) Handles btnCreditMemo.Click
        Try
            If grdItems.Rows.Count = 0 Then
                ShowWarning("There are no items in the current transaction!", "Credit Memo Failed")
                Return
            End If

            Dim authorizedBy As String = String.Empty

#If DEBUG Then
            authorizedBy = DEBUG_MODE
#Else
            If String.IsNullOrWhiteSpace(m_SalesCreditMemoNo) Then
                Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "SALES CREDIT MEMO"}
                    If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                        authorizedBy = frmAuth.AuthorizedBy
                    Else
                        Return
                    End If
                End Using
            End If
#End If

            Using frm As New FormCreditMemoBrowser(m_Settings)
                frm.Data = m_CreditMemoData.Copy
                frm.CustomerNo = m_CustomerCode
                frm.CustomerName = m_CustomerName
                frm.TotalSales = m_TotalSales

                If LaunchForm(frm, True, Me) = DialogResult.OK Then
                    m_CreditMemoData = frm.Data
                    m_HasCreditMemo = (m_CreditMemoData.Rows.Count > 0)

                    If m_HasCreditMemo Then
                        m_SalesCreditMemoNo = m_CreditMemoData.Rows(0).Item("No.").ToString()
                        SaveAuditTrail(
                            String.Format("Sales Credit Memo Applied - {0} - {1}|Authorized By: {2}",
                                If(m_HasCreditMemo, m_SalesCreditMemoNo, "NONE"), m_CustomerName, authorizedBy), False)
                    Else
                        m_SalesCreditMemoNo = String.Empty
                        SaveAuditTrail(
                            String.Format("Sales Credit Memo Removed - {0}|Authorized By: {1}",
                                m_CustomerName, authorizedBy), False)
                    End If

                    If frm.TotalAmount > 0 Then
                        btnCheckOut.PerformClick()
                    End If
                End If
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCheckOut_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCheckOut.Click
        Dim frm As Object = Nothing
        Try
            If ValidateCheckOut() Then
                m_SaleCode = String.Empty

                If btnCreditMemo.Visible Then
                    frm = New FormPaymentWithCM(m_Settings)
                ElseIf m_IsGiftCertEnabled Then
                    frm = New FormPaymentWithGC(m_Settings)
                Else
                    frm = New FormPayment(m_Settings)
                End If

                With frm
                    .CustomerName = m_CustomerName
                    .CustomerTIN = CStrEx(m_Customer.VATRegistrationNo)
                    .CustomerBusinessStyle = CStrEx(m_Customer.BusinessStyle)
                    .CustomerAddress = lblShippingAddress.Text
                    .IsCustomerWalkin = IsWalkinCustomer()
                    .IsCustomerScPwd = m_IsCustomerScPwd
                    .IsGiftCertEnabled = m_IsGiftCertEnabled

                    .VATableSales = m_TotalVATableSales
                    .VATAmount = m_TotalVATAmount
                    .NonVATSales = m_TotalNonVATSales
                    .ZeroRatedSales = m_TotalZeroRatedSales
                    .RegularDiscount = m_TotalRegularDiscount
                    .RegularLessVAT = m_TotalRegularLessVAT
                    .SCPwdDiscount = m_TotalSCPwdDiscount
                    .SCPwdVatExempt = m_TotalScPwdVATExempt

                    .SalespersonCode = m_Customer.SalesPersonCode
                    .SalespersonName = lblSalesperson.Text

                    .GiftCertificateData = m_GiftCertData
                    .CreditMemoData = m_CreditMemoData

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                        Dim salePosted As Boolean
                        Dim hasCreditCard As Boolean

                        If m_IsCustomerScPwd Then
                            PrepareScPwdForCheckOut()
                        End If

                        m_CustomerName = .CustomerName
                        m_Customer.VATRegistrationNo = .CustomerTIN
                        m_Customer.BusinessStyle = .CustomerBusinessStyle
                        lblShippingAddress.Text = .CustomerAddress
                        m_CreditMemoAmount = 0.0
                        m_GiftCertData = .GiftCertificateData

                        If frm.ExcessGCAmount > 0.0 Then
                            InsertExcessGCAmount(frm.ExcessGCAmount)
                            ComputeTotals()
                        End If

                        With New TaskProcess(Me, grdItems)
                            .ShowLoader = grdItems.Visible
                            .Start(
                                Sub()
                                    Try
                                        Dim sale As New Sale

                                        With sale
                                            .GetNextCode(.Code)
                                            m_SaleCode = .Code ' Pre-assign code

                                            If String.IsNullOrWhiteSpace(.Code) Then
                                                ShowWarning("No more available invoice series to continue this transaction!\n\nPlease contact you system administrator.", "Check Out Failed")
                                                SaveAuditTrail("Check out failed! No more available invoice series.", False)
                                                Return
                                            End If
                                        End With

                                        With frm
                                            hasCreditCard = (.CreditCardData.Rows.Count > 0)
                                            salePosted = SavePayment(sale, .CashData, .CreditCardData, .CreditMemoData)
                                        End With
                                    Catch ex As Exception
                                        ex.ShowError()
                                    End Try
                                End Sub,
                                Sub()
                                    If salePosted Then
                                        If Printing.PrinterSettings.InstalledPrinters.Count > 0 Then
                                            Try
                                                With New Reporting(m_Settings)
                                                    .Cashier = Current.User.Name
                                                    .TransactionDate = Now
                                                    .HasOwnReportViewer = True
                                                    .SalesTax = m_Settings.SalesTax

                                                    Dim data As DataSet = Nothing
                                                    Dim rpt = .ShowSale(data, m_SaleCode)

                                                    Try
                                                        If data IsNot Nothing Then
                                                            .SaveEJournalReceipt(data, m_SaleCode)
                                                        End If
                                                    Catch
                                                    End Try

                                                    If rpt IsNot Nothing Then
                                                        With rpt
                                                            If m_IsCustomerScPwd OrElse hasCreditCard Then
                                                                .SetParameterValue("is_customer_copy", True)
                                                                .SetParameterValue("is_store_copy", False)
                                                                .PrintToPrinter(1, False, -1, -1)
                                                                .SetParameterValue("is_customer_copy", False)
                                                                .SetParameterValue("is_store_copy", True)
                                                                .PrintToPrinter(1, False, -1, -1)
                                                            Else
                                                                .PrintToPrinter(1, False, -1, -1)
                                                            End If
                                                        End With
                                                    End If
                                                End With
                                            Catch ex As Exception
                                                Dim errmsg As String = ex.Message
                                                If errmsg.Contains("RPC server is unavailable") Then
                                                    errmsg = "Could not connect to a network printer."
                                                End If
                                                ShowMessage(
                                                    "An error has occured while printing the receipt:\n\n" & ex.Message,
                                                    "Printing Error", MessageIcon.Exclamation)
                                            End Try
                                        End If

                                        If String.IsNullOrWhiteSpace(m_SuspendSaleCode) Then
                                            SaveAuditTrail(
                                                String.Format("Sale Transaction Completed from Suspended Sale - {0} - {1}|Total Amount: {2}",
                                                    m_SaleCode,
                                                    m_CustomerName,
                                                    m_TotalSales.ToString("#,##0.00")), True, m_SaleCode)
                                        Else
                                            SaveAuditTrail(
                                                String.Format("Sale Transaction Completed - {0} - {1}|Total Amount: {2}",
                                                    m_SaleCode,
                                                    m_CustomerName,
                                                    m_TotalSales.ToString("#,##0.00")), True, m_SaleCode)
                                        End If

                                        Clean()
                                        ShowMessage("Sale transaction has been successfully posted.", "Transaction Complete")
                                    End If
                                End Sub)
                        End With
                    End If
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancelSale_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelSale.Click
        Try
            If grdItems.Rows.Count > 0 Then
                If Not m_LoadedFromSales Then
                    Dim cancelReason As String = String.Empty
                    Dim authorizedBy As String = String.Empty

                    If m_Settings.AuthorizeSaleCancellation AndAlso grdItems.Rows.Count > 0 Then
#If DEBUG Then
                        authorizedBy = DEBUG_MODE
#Else
                        Using frmAuth As New FormAuthorization(m_Settings, True) With {.Message = "CANCEL SALE TRANSACTION"}
                            If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                                cancelReason = frmAuth.Reason
                                authorizedBy = frmAuth.AuthorizedBy
                            Else
                                Return
                            End If
                        End Using
#End If
                    End If

                    If ShowQuestion("Current transaction will be cancelled.\n\nDo you want to continue?", "Confirm Cancel") = QuestionResult.Yes Then
                        If m_Settings.AuthorizeSaleCancellation AndAlso authorizedBy <> "" Then
                            SaveAuditTrail(
                                String.Format("Sale Transaction Cancelled - {0}|Reason: {1}|Authorized By: {2}",
                                    lblCustomerName.Text,
                                    cancelReason,
                                    authorizedBy))
                        Else
                            SaveAuditTrail(String.Format("Sale Transaction Cancelled - {0}", lblCustomerName.Text), True, m_InvoiceNo)
                        End If
                    Else
                        Return
                    End If
                End If
            End If

            Clean()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCashUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCashUp.Click
        Try
            If m_Settings.AuthorizeCashUp Then
                Dim isPending As Boolean
                If Current.Settings.CashInSet AndAlso Current.Settings.CashInDate.Date < Today.Date Then
                    isPending = True
                End If
                Using frmAuth As New FormAuthorization(m_Settings) With {.Message = If(isPending, "PENDING ", "") & "CASH UP REPORT"}
                    If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                        SaveAuditTrail(If(isPending, "Pending ", "") & "Cash Up Authorized - " & frmAuth.AuthorizedBy)
                    Else
                        Return
                    End If
                End Using
            End If

            Me.Enabled = False
            Using frm As New FormCashUp(m_Settings) With {
                .IsPreviousCashupUnfinalized = (Current.Settings.CashInDate.Date < Today.Date)
            }
                LaunchForm(frm, True, Me)

                If frm.IsPreviousCashupUnfinalized AndAlso Not frm.IsFinalized Then
                    m_LoggingOff = True
                    Close()
                End If
            End Using
        Catch ex As Exception
            ex.ShowError()
        Finally
            Me.Enabled = True
        End Try
    End Sub

    Private Sub btnLogOff_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogOff.Click
        Try
            If grdItems.Rows.Count > 0 OrElse (Not IsWalkinCustomer() AndAlso Not String.IsNullOrWhiteSpace(lblCustomerName.Text)) Then
                ShowMessage("User cannot log off because there is an active transaction.", "Log Off Failed", MessageIcon.Information)
            Else
                If ShowLogOffQuestion() = QuestionResult.Yes Then
                    LogOff()
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdItems_AfterRowActivate(sender As Object, e As EventArgs) Handles grdItems.AfterRowActivate
        Try
            If (Not m_LoadedFromSales OrElse Not String.IsNullOrEmpty(m_SuspendSaleCode)) AndAlso Not m_VoidAllowed Then
                Dim isGiftCertificate As Boolean
                Boolean.TryParse(grdItems.ActiveRow.Cells("IsGiftCertificate").Value, isGiftCertificate)

                btnSetQty.Enabled = Not isGiftCertificate
                btnSetDiscount.CheckAccess

                If btnSetDiscount.Enabled Then
                    If m_Customer.AllowLineDisc Then
                        Boolean.TryParse(grdItems.ActiveRow.Cells("DiscountIsAllowed").Value, btnSetDiscount.Enabled)
                    Else
                        btnSetDiscount.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdItems_AfterRowsDeleted(ByVal sender As Object, ByVal e As EventArgs) Handles grdItems.AfterRowsDeleted
        Try
            Dim hasItems As Boolean = (grdItems.Rows.Count > 0)
            Dim hasSelectedItem As Boolean

            If hasItems AndAlso grdItems.ActiveRow IsNot Nothing Then
                hasSelectedItem = True
            End If

            btnSetQty.Enabled = hasSelectedItem

            If hasSelectedItem Then
                btnSetDiscount.CheckAccess()
            Else
                btnSetDiscount.Enabled = False
            End If

            If btnCreditMemo.Enabled Then
                btnCreditMemo.Enabled = hasItems
            End If

            ComputeTotals()

            btnCheckOut.Enabled = hasItems
            btnCancelSale.Enabled = hasItems OrElse Not IsWalkinCustomer()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdItems_InitializeLayout(ByVal sender As Object, ByVal e As InitializeLayoutEventArgs) Handles grdItems.InitializeLayout
        Try
            With e.Layout
                .Override.CellClickAction = CellClickAction.RowSelect
                .Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

                With .Bands(0)
                    .Override.ReadOnlyCellAppearance.TextVAlign = Infragistics.Win.VAlign.Top
                    .Override.MaxSelectedRows = grdItems.Rows.Count
                    .Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "Quantity"
                                    .Header.Caption = "Qty"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Header.ToolTipText = "Quantity"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .SetWidth(75)
                                Case "Description"
                                    .Header.Caption = "Description"
                                    .CellMultiLine = Infragistics.Win.DefaultableBoolean.True
                                Case "UnitOfMeasure"
                                    .Header.Caption = "UOM"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Left
                                    .Header.ToolTipText = "Unit of Measure"
                                    .SetWidth(60, 80)
                                Case "UnitPrice"
                                    .Header.Caption = "Unit Price"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    If m_IsWideScreen Then
                                        .SetWidth(120)
                                    Else
                                        .SetWidth(100, 120)
                                    End If
                                Case "DiscountIsRegular"
                                    .Header.Caption = "Regular"
                                    .Hidden = Not m_ShowSettingsColumns
                                    .SetWidth(70)
                                    .Style = ColumnStyle.CheckBox
                                Case "DiscountPercent"
                                    .Header.Caption = "Discount %"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.##"
                                    .Hidden = Not m_HasDiscount
                                    If m_IsWideScreen Then
                                        .SetWidth(120)
                                    Else
                                        .SetWidth(80, 90)
                                    End If
                                Case "DiscountAmount"
                                    .Header.Caption = "Discount" & IIf(m_ShowSettingsColumns, " Amount", "")
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Header.ToolTipText = "Discount Amount"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = If(m_ShowSettingsColumns, "#,##0.00######", "#,##0.00")
                                    .Hidden = (picBanner.Visible AndAlso Not m_ShowSettingsColumns)
                                    If m_IsWideScreen Then
                                        .SetWidth(120)
                                    Else
                                        .SetWidth(100)
                                    End If
                                Case "DiscountedPrice"
                                    .Header.Caption = "Discounted Price"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = If(m_ShowSettingsColumns, "#,##0.00######", "#,##0.00")
                                    .Hidden = Not m_HasDiscount
                                    If m_IsWideScreen Then
                                        .SetWidth(140)
                                    Else
                                        .SetWidth(100)
                                    End If
                                Case "VATAmount"
                                    .Header.Caption = "VAT (" & m_Settings.SalesTax & "%)"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#0.00######"
                                    .SetWidth(100)
                                    .Hidden = Not m_ShowSettingsColumns
                                Case "LineTotal"
                                    .Header.Caption = "Line Total"
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .SetWidth(120)
                                Case "IsVATable"
                                    .Header.Caption = "VATable"
                                    .Hidden = Not m_ShowSettingsColumns
                                    .SetWidth(70)
                                    .Style = ColumnStyle.CheckBox
                                Case "IsZeroRated"
                                    .Header.Caption = IIf(m_ShowSettingsColumns, "Zero-Rated", "0-Rated")
                                    .Hidden = Not m_ShowSettingsColumns
                                    .SetWidth(70)
                                    .Style = ColumnStyle.CheckBox
                                Case "IsVATExempt"
                                    .Header.Caption = "VAT Exempt"
                                    .Hidden = Not m_ShowSettingsColumns
                                    .SetWidth(70)
                                    .Style = ColumnStyle.CheckBox
                                Case "IsGiftCertificate"
                                    .Header.Caption = "Gift Cert"
                                    If m_IsGiftCertEnabled Then
                                        .Hidden = Not m_ShowSettingsColumns
                                    Else
                                        .Hidden = True
                                    End If
                                    .SetWidth(70)
                                    .Style = ColumnStyle.CheckBox
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

    Private Sub grdItems_InitializeRow(sender As Object, e As InitializeRowEventArgs) Handles grdItems.InitializeRow
        Try
            With e.Row
                If .Cells("DiscountPercent").Value = 0.0 Then
                    .Cells("DiscountPercent").Appearance.ForeColor = Color.Transparent
                    .Cells("DiscountAmount").Appearance.ForeColor = Color.Transparent
                    .Cells("DiscountedPrice").Appearance.ForeColor = Color.Transparent
                Else
                    .Cells("DiscountPercent").Appearance.ForeColor = Nothing
                    .Cells("DiscountAmount").Appearance.ForeColor = Nothing
                    .Cells("DiscountedPrice").Appearance.ForeColor = Nothing
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Others"

    Private Sub txtSKU_EditorButtonClick(ByVal sender As Object, ByVal e As EditorButtonEventArgs) Handles txtSKU.EditorButtonClick
        Try
            If txtSKU.Text.Trim <> "" Then
                Dim dtItems As DataTable = Nothing
                Dim gcAlreadyExists As Boolean
                Dim gcIsReleased As Boolean
                Dim gcIsExpired As Boolean
                Dim gcExpiryDate As Date

                If IsItemFound(txtSKU.Text.Trim, dtItems, gcAlreadyExists, gcIsReleased, gcIsExpired, gcExpiryDate) Then
                    If dtItems.Rows.Count = 1 Then
                        AddItem(dtItems.Rows(0))
                        ComputeTotals()
                    End If

                    txtSKU.Text = ""
                    txtSKU.Focus()
                Else
                    If gcAlreadyExists Then
                        ShowMessage("Gift Certificate has already been added on the list!", "GC Already Exist", MessageIcon.Exclamation)
                    ElseIf gcIsReleased Then
                        ShowMessage("Gift Certificate has already been released!", "GC Already Issued", MessageIcon.Exclamation)
                    ElseIf gcIsExpired Then
                        ShowMessage("Gift Certificate has already expired since " & gcExpiryDate.ToDateString & "!", "GC Already Expired", MessageIcon.Exclamation)
                    ElseIf Not m_frmItemBrowserCancelled Then
                        ShowMessage("Item does not exist! Please try again.", "Item Not Found", MessageIcon.Exclamation)
                    End If

                    txtSKU.Focus()
                    txtSKU.SelectAll()
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub txtSKU_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtSKU.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If Not String.IsNullOrWhiteSpace(txtSKU.Text) Then
                    Dim button As EditorButtonBase = txtSKU.ButtonsRight(0)
                    txtSKU_EditorButtonClick(New Object(), New EditorButtonEventArgs(button, Nothing))
                End If
                e.Handled = True
                e.SuppressKeyPress = True
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub m_frmLoadButtons_Deactivate(ByVal sender As Object, ByVal e As EventArgs) Handles m_frmLoadButtons.Deactivate
        Try
            If Not m_frmLoadButtonsHandled Then
                m_frmLoadButtons.Close()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub m_frmLoadButtons_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles m_frmLoadButtons.FormClosed
        Try
            If m_frmLoadButtons.DialogResult = Windows.Forms.DialogResult.OK Then
                m_frmLoadButtonsHandled = True

                If grdItems.Rows.Count > 0 AndAlso Not m_LoadedFromSales Then
                    If ShowQuestion("There is an active transaction.\n\nDo you want to cancel the current transaction?", "Confirm Load Sale") = QuestionResult.No Then
                        m_frmLoadButtons = Nothing
                        Return
                    End If
                End If

                Select Case m_frmLoadButtons.Selected
                    Case FormLoadSalesButtons.Buttons.PostedSales
                        LoadPostedSale()
                    Case FormLoadSalesButtons.Buttons.RefundSales
                        LoadRefundSale()
                    Case FormLoadSalesButtons.Buttons.SuspendedSales
                        LoadSuspendedSale()
                End Select
            End If
        Catch ex As Exception
            ex.ShowError()
        Finally
            m_frmLoadButtons = Nothing
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_AuditTrail = New AuditTrail

            If m_Settings IsNot Nothing Then
                If m_Settings.DisplayBannerDefault Then
                    picBanner.Image = My.Resources.DefaultBanner
                ElseIf m_Settings.DisplayBannerImage IsNot Nothing Then
                    picBanner.Image = m_Settings.DisplayBannerImage
                End If

                If picBanner.Image IsNot Nothing Then
                    picSeparator.Visible = True
                    picBanner.SizeMode = m_Settings.DisplayBannerStyle
                    picBanner.Visible = True
                Else
                    m_IsWideScreen = True
                End If
            End If

            InitializeTables()
            InitializeItemsGrid()
            InitializeSalespersons()
            InitializeWalkInCustomer()
            InitializeButtons(True)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeItemsGrid()
        Try
            m_dtItemsSale = New DataTable
            With m_dtItemsSale.Columns
                .Add("BarCode")
                .Add("Quantity", GetType(Double)).DefaultValue = 1.0
                .Add("ItemLineId", GetType(Long))
                .Add("ItemCode")
                .Add("SKU").DefaultValue = ""
                .Add("Description")
                .Add("UnitOfMeasure")
                .Add("ItemCategoryCode")
                .Add("UnitPrice", GetType(Double))
                .Add("BasePrice", GetType(Double)).DefaultValue = 0.0
                .Add("QtyPerUOM", GetType(Double)).DefaultValue = 1
                .Add("DiscountGroup", GetType(String)).DefaultValue = ""
                .Add("DiscountIsAllowed", GetType(Boolean)).DefaultValue = True
                .Add("DiscountIsRegular", GetType(Boolean)).DefaultValue = True
                .Add("DiscountPercent", GetType(Double))
                .Add("DiscountAmount", GetType(Double)).DefaultValue = 0.0
                .Add("DiscountedPrice", GetType(Double)).DefaultValue = 0.0
                .Add("VATPercent", GetType(Double)).DefaultValue = m_Settings.SalesTax
                .Add("VATAmount", GetType(Double)).DefaultValue = 0.0
                .Add("LineTotal", GetType(Double)).DefaultValue = 0.0

                ' for SC and PWD
                .Add("VATExemptAmount", GetType(Double)).DefaultValue = 0.0
                .Add("Deductions", GetType(Double)).DefaultValue = 0.0

                ' item settings
                .Add("IsVATable", GetType(Boolean)).DefaultValue = True
                .Add("IsZeroRated", GetType(Boolean)).DefaultValue = False
                .Add("IsVATExempt", GetType(Boolean)).DefaultValue = False
                .Add("IsGiftCertificate", GetType(Boolean)).DefaultValue = False
                .Add("SerialNbr")
            End With
            grdItems.DataSource = m_dtItemsSale
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeButtons(Optional onInitialize As Boolean = False)
        Try
            If onInitialize Then
                btnReprintReceipt.CheckAccess()

                With Current.Rights
                    If .IsAllowed(btnLoadSale.Tag) OrElse .IsAllowed(btnRefundSale.Tag) OrElse .IsAllowed(btnVoidSale.Tag) Then
                        btnLoadSale.Enabled = True
                    Else
                        btnLoadSale.Enabled = False
                    End If
                End With

                btnSalesperson.Visible = (m_Settings.AcuIntegration AndAlso m_Settings.ShowSalesperson)
                btnVoidSale.Visible = Current.Rights.IsAllowed("Void Sale", Rights.AccessRights.View, "POS")

                If Not Current.Rights.IsAllowed("Credit Memo", Rights.AccessRights.View, "POS") Then
                    If Current.Rights.IsAllowed("Refund Sale", Rights.AccessRights.View, "POS") Then
                        btnRefundSale.Enabled = False
                    Else
                        btnRefundSale.Visible = False
                    End If

                    If btnRefundSale.Visible Then
                        btnVoidSale.Left = btnRefundSale.Left
                        btnRefundSale.Left = btnSuspendSale.Left
                        btnSuspendSale.Left = btnCreditMemo.Left
                        btnCreditMemo.Visible = False
                    Else
                        btnVoidSale.Left = btnSuspendSale.Left
                        btnSuspendSale.Left = btnCreditMemo.Left
                        btnCreditMemo.Visible = False
                    End If
                Else
                    If Current.Rights.IsAllowed("Refund Sale", Rights.AccessRights.View, "POS") Then
                        btnRefundSale.Enabled = False
                    Else
                        btnVoidSale.Left = btnRefundSale.Left + 1
                        btnRefundSale.Visible = False
                    End If
                End If

                btnCashUp.CheckAccess()
                btnCancelSale.Tag = btnCancelSale.Text
                btnLogOff.Enabled = True
            Else
                btnRefundSale.Enabled = False
            End If

            btnSelectCustomer.Enabled = True
            btnScPwd.Enabled = True
            btnSetQty.Enabled = False
            btnSetDiscount.Enabled = False
            btnSalesperson.Enabled = m_HasSalespersons
            btnVoidSale.Enabled = False
            btnSuspendSale.Enabled = False
            btnCreditMemo.Enabled = False
            btnCheckOut.Enabled = False
            btnCancelSale.Enabled = False
            btnCancelSale.Text = btnCancelSale.Tag
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeSalespersons()
        Try
            btnSalesperson.Enabled = False

            Dim salesPersonList As New List(Of Salesperson.Struct)
            With New Salesperson
                .Fill()
                For Each row In .Data.Rows
                    Dim item As New Salesperson.Struct
                    With item
                        .Code = row("code")
                        .Name = row("name")
                        .Barcode = row("barcode")
                    End With
                    salesPersonList.Add(item)
                Next
                m_SalespersonList = salesPersonList.ToArray
            End With

            If m_Settings.AcuIntegration AndAlso m_Settings.ShowSalesperson Then
                m_HasSalespersons = m_SalespersonList.Length > 0
            End If

            lblSalespersonCaption.Visible = m_HasSalespersons
            lblSalesperson.Visible = m_HasSalespersons
            btnSalesperson.Enabled = m_HasSalespersons
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetDiscountType(code As String) As String
        Try
            If (code = "SC" OrElse code = "Senior Citizen") Then
                Return "SC"
            ElseIf (code = "PWD" OrElse code = "Person w/ Disability") Then
                Return "PWD"
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsDiscountTypeSCPWD(code As String) As Boolean
        Try
            Dim upCode As String = code.ToUpper
            Select Case GetDiscountType(upCode)
                Case "SC", "PWD"
                    Return True
                Case Else
                    If upCode.Contains("SENIOR") AndAlso upCode.Contains("CITIZEN") Then
                        Return True
                    ElseIf upCode.Contains("PERSON") AndAlso upCode.Contains("DISABILITY") Then
                        Return True
                    End If
                    Return False
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub InitializeWalkInCustomer(Optional discountType As String = "", Optional getCustomerList As Boolean = False)
        Try
            If discountType = "" OrElse m_Customer Is Nothing Then
                m_Customer = New Customer
            End If

            Dim customerName As String = CUSTOMER_WALKIN

            If discountType <> "" Then
                Dim row As DataRow = m_ScPwdDiscountData.Rows(0)
                customerName = row("name")
                discountType = row("discount_type")
            End If

            With m_Customer
                .Code = CUSTOMER_WALKIN
                .Name = customerName
                .VATRegistrationNo = String.Empty
                .BusinessStyle = String.Empty
                .SalesPersonCode = String.Empty
                .AllowLineDisc = Current.Rights.IsAllowed("Set Discount", "POS")
                .CustomerDiscGroup = discountType
                .GenBusPostingGroup = .CustomerDiscGroup

                m_CustomerCode = .Code
                m_CustomerName = .Name

                lblCustomerName.Text = .Name & If(discountType = "", "", " - " & discountType)
                lblSalesperson.Text = "---"
                lblShippingAddressCaption.Text = If(String.IsNullOrWhiteSpace(.Address), "", "Shipping Address")
                lblShippingAddress.Text = CStrEx(.Address)

                .CustomerDiscGroup = CStrEx(.CustomerDiscGroup)
            End With

            If discountType = "" Then
                CleanScPwdData()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeTables(Optional excludeScPwdData As Boolean = False)
        Try
            m_GiftCertData = New DataTable()
            With m_GiftCertData.Columns
                .Add("gc_no")
                .Add("description")
                .Add("amount", GetType(Double))
                .Add("payment_code").DefaultValue = ""
            End With
            m_GiftCertData.PrimaryKey = New DataColumn() {m_GiftCertData.Columns("gc_no")}

            m_CreditMemoAmount = 0.0
            m_CreditMemoData = New DataTable()
            With m_CreditMemoData.Columns
                .Add("No.")
                .Add("CreditMemoNo")
                .Add("CustomerName")
                .Add("Amount", GetType(Double))
                .Add("PaymentCode").DefaultValue = ""
            End With
            m_CreditMemoData.PrimaryKey = New DataColumn() {m_CreditMemoData.Columns("CreditMemoNo")}

            If Not excludeScPwdData Then
                m_ScPwdDiscountData = New DataTable
                With m_ScPwdDiscountData.Columns
                    .Add("discount_type").DefaultValue = Nothing
                    .Add("name").DefaultValue = ""
                    .Add("id_no").DefaultValue = ""
                    .Add("gender").DefaultValue = Nothing
                    .Add("birthdate", GetType(Date))
                    .Add("issued_date", GetType(Date))
                    .Add("total_discount", GetType(Double)).DefaultValue = 0.0
                    .Add("total_less_vat", GetType(Double)).DefaultValue = 0.0
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Friend Sub RestartApplication()
        Try
            m_OnRestart = True
            Application.Restart()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadItemList(Optional onInitialize As Boolean = False)
        Try
            If m_LoadItemsDone OrElse onInitialize Then
                m_LoadItemsDone = False
            Else
                Return
            End If

            Dim threadError As Exception = Nothing

            btnSelectItem.Enabled = False
            btnPriceLookUp.Enabled = False
            lblSKU.Enabled = False
            lblSKU.Text = "Loading Items..."
            txtSKU.Enabled = False

            With New TaskProcess(Me)
                .ShowLoader = False
                .Start(
                    Sub()
                        Try
                            With New ItemList
                                .Fill()
                                m_dtItemsList = .Data
                            End With
                        Catch ex As Exception
                            threadError = ex
                        End Try
                    End Sub,
                    Sub()
                        If threadError Is Nothing Then
                            btnSelectItem.Enabled = True
                            btnPriceLookUp.Enabled = True
                            lblSKU.Enabled = True
                            lblSKU.Text = "Type in Code / Description and Press Enter"
                            txtSKU.Enabled = True
                            txtSKU.Focus()

                            m_LoadItemsDone = True
                        Else
                            threadError.ShowError
                            m_OnRestart = True
                            Me.Close()
                        End If
                    End Sub
                )
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub AddItem(ByVal newItem As DataRow)
        Try
            Dim isNewRow As Boolean
            Dim row As DataRow
            Dim gridRow As UltraGridRow = Nothing
            Dim dtItems As DataTable = grdItems.DataSource
            Dim rowsFound As DataRow()

            rowsFound = dtItems.Select("ItemCode = '" & newItem("code") & "' AND SKU = '" & newItem("sku") & "' AND UnitOfMeasure = '" & newItem("unit_of_measure") & "'")

            If rowsFound.Count = 1 Then
                row = rowsFound.First
                row("Quantity") += 1

                ComputeVATDiscount(row)

                For Each r As UltraGridRow In grdItems.Rows
                    'If r.Cells("ItemCode").Value = row("ItemCode") AndAlso r.Cells("UnitOfMeasure").Value = row("UnitOfMeasure") Then
                    If r.Cells("ItemCode").Value = row("ItemCode") AndAlso r.Cells("SKU").Value = row("SKU") AndAlso r.Cells("UnitOfMeasure").Value = row("UnitOfMeasure") Then
                        r.Refresh()
                        gridRow = r
                        Exit For
                    End If
                Next
            Else
                Static itemLineId As Integer

                If dtItems.Rows.Count = 0 Then
                    itemLineId = 1
                Else
                    itemLineId += 1
                End If

                isNewRow = True
                row = dtItems.NewRow()
                row("ItemLineId") = itemLineId

                If newItem("is_lot").Equals("True") Or newItem("is_serial").Equals("True") Then
                    Using f As New FormSerialNumber
                        With f
                            If LaunchForm(f, True) = DialogResult.OK Then
                                row("SerialNbr") = .txtSerialNbr.Value
                            Else
                                Return
                            End If
                        End With
                    End Using
                End If

                row("BarCode") = newItem("codename")
                row("ItemCode") = newItem("code")
                row("SKU") = newItem("sku")
                row("Description") = newItem("description")
                row("UnitOfMeasure") = newItem("unit_of_measure")
                row("ItemCategoryCode") = newItem("class_code")
                row("UnitPrice") = newItem("price")
                row("DiscountGroup") = newItem("discount_group")
                row("DiscountIsRegular") = If(newItem("discount_percent") = 0.0, True, False)
                row("DiscountPercent") = If(m_IsCustomerScPwd, newItem("discount_percent"), 0.0)
                row("VATPercent") = newItem("vat_percent")
                row("VATAmount") = newItem("vat_amount")
                row("IsVATable") = newItem("is_vat")
                row("IsZeroRated") = newItem("is_zero_rated")
                row("IsVATExempt") = newItem("is_vat_exempt")
                row("IsGiftCertificate") = newItem("is_gift_certificate")

                Dim hasDiscount As Boolean = m_HasDiscount

                ComputeVATDiscount(row)
                dtItems.Rows.Add(row)
                gridRow = grdItems.Rows(grdItems.Rows.Count - 1)

                If hasDiscount <> m_HasDiscount Then
                    grdItems.DataBind()
                End If
            End If

            ComputeLineTotal(gridRow)

            If grdItems.Rows.Count > 0 Then
                grdItems.Selected.Rows.Clear()
                grdItems.ActiveRow = gridRow
                btnSuspendSale.CheckAccess()
                btnCreditMemo.CheckAccess()
                btnCheckOut.Enabled = True
                btnCancelSale.Enabled = True
            End If

            If grdItems.Rows.Count = 1 AndAlso IsWalkinCustomer() Then
                SaveAuditTrail("Sale Transaction Started - " & m_CustomerName)
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub RemoveItem(ByRef e As KeyEventArgs)
        Try
            If Not txtSKU.Focused Then
                If grdItems.ActiveRow IsNot Nothing Then
                    Dim rowIndex As Integer = grdItems.ActiveRow.Index
                    Dim authorizedBy As String = String.Empty

                    If m_Settings.AuthorizeSaleItemDeletion Then
#If DEBUG Then
                        authorizedBy = DEBUG_MODE
#Else
                        Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "REMOVE TRANSACTION ITEM"}
                            If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                                authorizedBy = frmAuth.AuthorizedBy
                            Else
                                Return
                            End If
                        End Using
#End If
                    End If

                    If ShowQuestion("Selected item will be removed from this transaction.\n\nDo you want to continue?", "Confirm Delete") = QuestionResult.Yes Then
                        grdItems.Rows(rowIndex).Selected = True

                        If authorizedBy <> "" Then
                            Dim itemDesc As String = grdItems.Rows(rowIndex).Cells("Description").Value

                            If itemDesc.Contains(vbCrLf) Then
                                itemDesc = itemDesc.Substring(0, itemDesc.IndexOf(vbCrLf))
                            End If

                            SaveAuditTrail(
                                String.Format("Item Removed from Sale Transaction|Item: {0}|Authorized By: {1}",
                                    itemDesc,
                                    authorizedBy))
                        End If

                        For Each row As UltraGridRow In grdItems.Selected.Rows
                            row.Delete(False)
                        Next

                        If grdItems.Rows.Count > 0 Then
                            grdItems.ActiveRow = grdItems.Rows(grdItems.Rows.Count - 1)
                        End If

                        txtSKU.Focus()
                    End If

                    e.SuppressKeyPress = True
                    e.Handled = True
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComputeVATDiscount(ByRef row As DataRow, Optional recomputeVAT As Boolean = False, Optional useDiscountValue As Boolean = False)
        Try
            Dim isRegularDiscount As Boolean = CBool(row("DiscountIsRegular"))
            Dim hasDiscount As Boolean = CDblEx(row("DiscountPercent")) > 0.0
            Dim isVatable As Boolean = CBool(row("IsVATable"))
            Dim vatAmount As Double = CDblEx(row("VATAmount"))
            Dim unitPrice As Double = CDblEx(row("UnitPrice"))
            Dim salesTax As Double = m_Settings.SalesTax
            Dim isVatExempt As Boolean = row("IsVATExempt")
            Dim originalVat As Double = vatAmount
            Dim netPrice As Double

            If m_LoadedFromSales OrElse recomputeVAT Then
                vatAmount = unitPrice / (1 + (salesTax / 100)) * (salesTax / 100)
                vatAmount = RoundoffVatPerLine(vatAmount)
                originalVat = vatAmount
            Else
                vatAmount = RoundoffVatPerLine(vatAmount)
            End If

            If hasDiscount Then
                Dim priceExcVat As Double = unitPrice
                Dim discountPercent As Double = CDblEx(row("DiscountPercent"))
                Dim discountAmount As Double = 0.0
                Dim discountedPrice As Double = 0.0
                Dim quantity As Double = CDblEx(row("Quantity"))

                If isVatable Then
                    netPrice = unitPrice - vatAmount
                Else
                    netPrice = unitPrice
                End If

                If DISCOUNT_NET_PRICE Then
                    priceExcVat = netPrice
                End If

                If useDiscountValue Then
                    discountAmount = CDblEx(row("DiscountAmount"))
                    discountedPrice = CDblEx(row("DiscountedPrice"))
                Else
                    discountAmount = priceExcVat * (discountPercent / 100)

                    If ROUNDOFF_DISCOUNT_AMOUNT Then
                        discountAmount = Math.Round(discountAmount, 8)
                    End If

                    discountedPrice = priceExcVat - discountAmount
                End If

                row("DiscountAmount") = discountAmount
                row("DiscountedPrice") = discountedPrice

                If isVatable AndAlso Not isVatExempt Then
                    vatAmount = discountedPrice * (m_Settings.SalesTax / 100)
                    vatAmount = RoundoffVatPerLine(vatAmount)
                    row("VATAmount") = vatAmount
                End If

                If m_IsCustomerScPwd AndAlso isVatable Then
                    If isVatExempt Then
                        row("VATExemptAmount") = row("VATAmount")
                        row("Deductions") = row("VATAmount") * row("Quantity")

                        If ROUNDOFF_DEDUCTIONS Then
                            row("Deductions") = Math.Round(row("Deductions"), 2)
                        End If
                    Else
                        row("VATExemptAmount") = (originalVat - vatAmount)
                        row("VATExemptAmount") = RoundoffVatPerLine(row("VATExemptAmount"))
                    End If
                End If

                m_HasDiscount = True
            Else
                row("DiscountAmount") = 0.0
                row("DiscountedPrice") = 0.0
                row("VATAmount") = vatAmount
                row("VATExemptAmount") = If(isVatExempt, vatAmount, 0.0)
                row("Deductions") = 0.0
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComputeLineTotal(ByVal row As UltraGridRow)
        Try
            Dim isVatable As Boolean = CBool(row.Cells("IsVATable").Value)
            Dim isVatExempt As Boolean = CBool(row.Cells("IsVATExempt").Value)
            Dim unitPrice As Double = row.Cells("UnitPrice").Value
            Dim vatAmount As Double = row.Cells("VATAmount").Value
            Dim discountAmount As Double = row.Cells("DiscountAmount").Value
            Dim discountedPrice As Double = row.Cells("DiscountedPrice").Value
            Dim quantity As Double = row.Cells("Quantity").Value

            If discountAmount > 0.0 Then
                If isVatable AndAlso Not isVatExempt Then
                    row.Cells("LineTotal").Value = (discountedPrice + vatAmount) * quantity
                Else
                    row.Cells("LineTotal").Value = discountedPrice * quantity
                End If

                m_HasDiscount = True
            Else
                row.Cells("LineTotal").Value = unitPrice * quantity
            End If

            If ROUNDOFF_NET_PER_LINE Then
                row.Cells("LineTotal").Value = Math.Round(row.Cells("LineTotal").Value, 2)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function RoundoffVatPerLine(amount As Double) As Double
        Try
            If ROUNDOFF_VAT_PER_LINE Then
                amount = Math.Round(amount, ROUNDOFF_VAT_PER_LINE_DECIMALS)
            End If
            Return amount
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ComputeTotals()
        Try
            m_TotalVATableSales = 0.0
            m_TotalVATAmount = 0.0
            m_TotalNonVATSales = 0.0
            m_TotalZeroRatedSales = 0.0
            m_TotalSales = 0.0

            m_TotalRegularDiscount = 0.0
            m_TotalRegularLessVAT = 0.0
            m_TotalSCPwdDiscount = 0.0
            m_TotalScPwdVATExempt = 0.0
            m_TotalDeductions = 0.0

            For Each row As UltraGridRow In grdItems.Rows
                Dim itemQty As Double = row.Cells("Quantity").Value
                Dim isRegularDiscount As Boolean = row.Cells("DiscountIsRegular").Value
                Dim discountAmount As Double = row.Cells("DiscountAmount").Value
                Dim discountLineTotal As Double = (row.Cells("DiscountAmount").Value * itemQty)
                Dim isVatable As Boolean = row.Cells("IsVATable").Value
                Dim vatAmount As Double = row.Cells("VATAmount").Value
                Dim vatLineTotal As Double = (row.Cells("VATAmount").Value * itemQty)
                Dim isZeroRated As Boolean = row.Cells("IsZeroRated").Value
                Dim isVatExempt As Boolean = row.Cells("IsVatExempt").Value
                Dim vatExemptAmount As Double = row.Cells("VATExemptAmount").Value
                Dim vatExemptLineTotal As Double = (row.Cells("VATExemptAmount").Value * itemQty)
                Dim lineTotal As Double = row.Cells("LineTotal").Value
                Dim priceExcVat As Double = row.Cells("UnitPrice").Value
                Dim vatableLineTotal As Double

                If isVatable AndAlso Not isVatExempt Then
                    priceExcVat /= (1 + m_Settings.SalesTax / 100)
                    vatableLineTotal = priceExcVat - discountAmount
                    vatableLineTotal *= itemQty
                    vatableLineTotal = RoundoffVatPerLine(vatableLineTotal)
                End If

                priceExcVat *= itemQty
                priceExcVat = RoundoffVatPerLine(priceExcVat)

                If isZeroRated Then
                    m_TotalZeroRatedSales += priceExcVat - discountLineTotal

                ElseIf isVatable AndAlso Not isVatExempt Then
                    m_TotalVATableSales += vatableLineTotal
                Else
                    priceExcVat = row.Cells("UnitPrice").Value

                    If isVatable Then
                        priceExcVat = (row.Cells("UnitPrice").Value / (1 + (m_Settings.SalesTax / 100)))
                        priceExcVat = RoundoffVatPerLine(priceExcVat)
                    End If

                    priceExcVat *= itemQty
                    priceExcVat -= discountLineTotal
                    priceExcVat = RoundoffVatPerLine(priceExcVat)

                    m_TotalNonVATSales += priceExcVat
                End If

                If isVatExempt Then
                    m_TotalScPwdVATExempt += vatExemptLineTotal
                ElseIf isVatable Then
                    m_TotalVATAmount += vatLineTotal
                End If

                If m_IsCustomerScPwd Then
                    If isRegularDiscount Then
                        m_TotalRegularDiscount += discountLineTotal
                    Else
                        m_TotalSCPwdDiscount += discountLineTotal

                        If isVatable AndAlso Not isVatExempt Then
                            m_TotalScPwdVATExempt += vatExemptLineTotal
                        End If
                    End If
                Else
                    m_TotalRegularDiscount += discountLineTotal
                End If

                If isVatable AndAlso isRegularDiscount AndAlso vatExemptLineTotal > 0.0 Then
                    m_TotalRegularLessVAT += vatExemptLineTotal
                End If

                m_TotalDeductions += row.Cells("Deductions").Value
                m_TotalSales += lineTotal
            Next

            If ROUNDOFF_TOTAL_VATABLE_SALES Then
                m_TotalVATableSales = Math.Round(m_TotalVATableSales, 8)
            End If

            If ROUNDOFF_TOTAL_VAT_AMOUNT Then
                m_TotalVATAmount = Math.Round(m_TotalVATAmount, 8)
            End If

            If ROUNDOFF_TOTAL_VAT_EXEMPT_SALES Then
                m_TotalNonVATSales = Math.Round(m_TotalNonVATSales, 8)
            End If

            If ROUNDOFF_TOTAL_SALES Then
                m_TotalSales = Math.Round(m_TotalSales, 8)
            End If

            If ROUNDOFF_DISCOUNT_TOTAL Then
                m_TotalRegularDiscount = Math.Round(m_TotalRegularDiscount, 8)
            End If

            If ROUNDOFF_DISCOUNT_AMOUNT Then
                m_TotalSCPwdDiscount = Math.Round(m_TotalSCPwdDiscount, 8)
            End If

            If ROUNDOFF_DEDUCTIONS Then
                m_TotalDeductions = Math.Round(m_TotalDeductions, 8)
            End If

            If m_TotalRegularDiscount > 0.0 OrElse m_TotalSCPwdDiscount > 0.0 Then
                m_HasDiscount = True
            End If

            lblTotalSale.Text = Math.Round((m_TotalSales - m_TotalVATAmount), 2).ToString("#,##0.00")
            lblTotalVAT.Text = Math.Round(m_TotalVATAmount, 2).ToString("#,##0.00")
            lblGrandTotal.Text = Math.Round(m_TotalSales, 2).ToString("#,##0.00")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Clean(Optional excludeScPwdData As Boolean = False)
        Try
            Dim hasDiscount As Boolean = m_HasDiscount

            lblShippingAddressCaption.Text = String.Empty
            lblShippingAddress.Text = String.Empty

            txtSKU.Clear()
            m_dtItemsSale.Clear()
            m_HasDiscount = False

            If Not excludeScPwdData Then
                m_ScPwdDiscountData.Rows.Clear()
            End If

            ComputeTotals()

            If hasDiscount Then
                grdItems.DataBind()
            End If

            m_CustomerCode = CUSTOMER_WALKIN
            m_InvoiceNo = String.Empty
            m_SalesCreditMemoNo = String.Empty
            m_PaymentCode = String.Empty
            m_SuspendSaleCode = String.Empty

            m_LoadedFromSales = False
            m_RefundSale = False
            m_VoidAllowed = False
            m_HasCreditMemo = False

            InitializeTables(excludeScPwdData)

            If Not excludeScPwdData Then
                InitializeWalkInCustomer(, True)
            End If

            If m_LoadItemsDone Then
#If DEBUG Then
                LoadItemList()
#Else
                btnSelectItem.Enabled = True
                txtSKU.Enabled = True
#End If
            End If

            InitializeButtons()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub CleanScPwdData()
        Try
            m_IsCustomerScPwd = False
            m_ScPwdDiscountData.Rows.Clear()

            With m_Customer
                .ScPwdId = ""
                .Gender = ""
                .BirthDate = Nothing
                .DateIssued = Nothing
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsItemFound(
                keyword As String,
                ByRef dtItems As DataTable,
                ByRef gcAlreadyExists As Boolean,
                ByRef gcIsReleased As Boolean,
                ByRef gcIsExpired As Boolean,
                ByRef gcExpiryDate As Date) As Boolean
        Try
            Dim searchResult As DataRow() = Nothing

            keyword = keyword.Replace("'", "''")

            Try
                searchResult = m_dtItemsList.Select("sku = '" & keyword & "'", "") ' check exact barcode

                If searchResult.Count = 0 Then
                    searchResult = m_dtItemsList.Select("sku = '" & keyword.Substring(0, keyword.Length - 1) & "'", "") ' check broken barcode (less 1 character)
                End If

                If searchResult.Count = 0 Then ' otherwise, check all other fields
                    searchResult = m_dtItemsList.Select(
                            "codename LIKE '%" & keyword &
                            "%' OR sku LIKE '%" & keyword &
                            "%' OR description LIKE '%" & keyword &
                            "%'", "")
                End If
            Catch ex As Exception
                ' ignore possible filtering error
            End Try

            If searchResult Is Nothing OrElse searchResult.Count = 0 Then
                dtItems = m_dtItemsList.Clone
            Else
                dtItems = searchResult.CopyToDataTable
            End If

            If dtItems.Rows.Count > 1 Then
                Using frm As New FormItemBrowser(m_Settings)
                    With frm
                        .ItemList = dtItems
                        .IsCustomerScPwd = m_IsCustomerScPwd
                        .HasVATExclusiveItems = False
                    End With

                    With frm
                        m_frmItemBrowserCancelled = False
                        If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                            For Each item As DataRow In frm.SelectedItems.Rows
                                AddItem(item)
                            Next

                            ComputeTotals()
                            Return True
                        Else
                            m_frmItemBrowserCancelled = True
                        End If
                    End With
                End Using

            ElseIf dtItems.Rows.Count = 1 Then
                With dtItems.Rows(0)
                    If Not m_IsCustomerScPwd Then
                        .Item("discount_percent") = 0.0
                        .Item("is_vat_exempt") = False
                    End If
                End With

                Return True
            Else
                Dim gcCode As String = String.Empty
                Dim gcDescription As String = String.Empty
                Dim gcAmount As Double

                With New GiftCertificateList
                    .Code = keyword
                    If .Fill(True) Then
                        gcCode = .Code
                        gcDescription = .Description
                        gcAmount = .Amount
                        gcExpiryDate = .ExpiryDate
                        gcIsReleased = .IsSold
                    End If
                End With

                If Not String.IsNullOrWhiteSpace(gcCode) Then
                    If m_dtItemsSale.Select("ItemCode = " & gcCode.Quote(False) & " AND IsGiftCertificate = 'True'").Count = 0 Then
                        If gcIsReleased Then
                            gcIsReleased = True

                        ElseIf IsDateValid(gcExpiryDate) AndAlso gcExpiryDate < Now.Date Then
                            gcIsExpired = True
                            gcExpiryDate = gcExpiryDate

                        Else
                            Dim gcRow As DataRow = dtItems.NewRow

                            gcRow("code") = gcCode
                            gcRow("codename") = gcCode
                            gcRow("description") = gcDescription
                            gcRow("unit_of_measure") = DEFAULT_UOM
                            gcRow("class_code") = GC_CODE
                            gcRow("price") = gcAmount
                            gcRow("vat_percent") = 0.0
                            gcRow("vat_amount") = 0.0
                            gcRow("discount_group") = ""
                            gcRow("discount_percent") = 0.0
                            gcRow("is_vat") = False
                            gcRow("is_zero_rated") = False
                            gcRow("is_vat_exempt") = False
                            gcRow("is_gift_certificate") = True

                            dtItems.Rows.Add(gcRow)
                            Return True
                        End If
                    Else
                        gcAlreadyExists = True
                    End If
                End If

                Return False
            End If
        Catch ex As InvalidOperationException
            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ReprintSalesReceipt()
        Try
            Dim reprintReason As String = String.Empty
            Dim authorizedBy As String = String.Empty

            If m_Settings.AuthorizeReprintReceipt Then
#If DEBUG Then
                authorizedBy = DEBUG_MODE
#Else
                Using frmAuth As New FormAuthorization(m_Settings, True) With {.Message = "REPRINT SALES RECEIPT"}
                    If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                        reprintReason = frmAuth.Reason
                        authorizedBy = frmAuth.AuthorizedBy
                    Else
                        Return
                    End If
                End Using
#End If
            End If

            Using frm As New FormSalesBrowser(m_Settings)
                With frm
                    .CustomerCode = If(m_CustomerCode = CUSTOMER_WALKIN, "", m_CustomerCode)
                    .Text &= " - Reprint Receipt (O.R.)"
                    .TransactionDate = Now
                    .IsReprintReceipt = True

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                        If Printing.PrinterSettings.InstalledPrinters.Count > 0 Then
                            Try
                                With New Reporting(m_Settings)
                                    .Cashier = Current.User.Name
                                    .TransactionDate = Now
                                    .SalesTax = m_Settings.SalesTax
                                    .HasOwnReportViewer = True

                                    Dim data As DataSet = Nothing
                                    Dim rpt = .ShowSale(data, frm.SaleCode, True)

                                    Try
                                        If data IsNot Nothing Then
                                            .SaveEJournalReceipt(data, m_SaleCode, True)
                                        End If
                                    Catch
                                    End Try

                                    If rpt IsNot Nothing Then
                                        rpt.PrintToPrinter(1, False, -1, -1)

                                        If String.IsNullOrEmpty(authorizedBy) Then
                                            SaveAuditTrail("Reprint Sale Receipt - " & frm.SaleCode & " - " & frm.CustomerName)
                                        Else
                                            SaveAuditTrail(
                                                "Reprint Sale Receipt - " & frm.SaleCode & " - " & frm.CustomerName & "|" &
                                                "Reason: " & reprintReason & "|" &
                                                "Authorized by: " & authorizedBy)
                                        End If
                                    End If
                                End With
                            Catch ex As Exception
                                Dim errmsg As String = ex.Message
                                If errmsg = "The RPC server is unavailable" Then
                                    errmsg = "Could not connect to a network printer."
                                End If
                                ShowMessage("An error has occured while printing the receipt:\n\n" & ex.Message, "Printing Error", MessageIcon.Exclamation)
                            End Try
                        Else
                            ShowWarning("No printer found installed in the system!", "Printer Not Found")
                        End If
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadPostedSale()
        Try
            Dim loadReason As String = String.Empty
            Dim authorizedBy As String = String.Empty

            If m_Settings.AuthorizeLoadPostedSale Then
#If DEBUG Then
                authorizedBy = DEBUG_MODE
#Else
                Using frmAuth As New FormAuthorization(m_Settings, True) With {.Message = "LOAD POSTED SALES"}
                    If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                        loadReason = frmAuth.Reason
                        authorizedBy = frmAuth.AuthorizedBy
                    Else
                        Return
                    End If
                End Using
#End If
            End If

            Using frm As New FormSalesBrowser(m_Settings)
                With frm
                    .CustomerCode = If(IsWalkinCustomer(), "", m_CustomerCode)
                    .Text &= " - Load" & If(btnVoidSale.Visible, "/Void", "") & " Sale"

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                        Dim dtItems As DataTable = grdItems.DataSource
                        dtItems.Rows.Clear()

                        m_HasDiscount = False
                        m_LoadedFromSales = True
                        m_RefundSale = False
                        m_VoidAllowed = True
                        m_SaleCode = .SaleCode
                        m_InvoiceNo = .InvoiceCode
                        m_PaymentCode = .PaymentCode

                        Dim row As DataRow = Nothing

                        With New Sale
                            .Fill(m_SaleCode)

                            m_CustomerCode = .CustomerCode
                            m_IsCustomerScPwd = .CustomerIsSC OrElse .CustomerIsPWD

                            For Each dr As DataRow In .Items.Rows
                                row = dtItems.NewRow
                                row("BarCode") = dr("item_code")
                                row("Quantity") = dr("qty")
                                row("ItemLineId") = dr("item_line_id")
                                row("ItemCode") = dr("item_code")
                                row("Description") = dr("description")
                                row("UnitOfMeasure") = dr("unit_of_measure")
                                row("ItemCategoryCode") = dr("class_code")
                                row("UnitPrice") = dr("price")
                                row("QtyPerUOM") = dr("qty_per_uom")
                                row("DiscountIsAllowed") = False
                                row("DiscountIsRegular") = dr("is_regular_discount")
                                row("DiscountPercent") = dr("discount_percent")
                                row("DiscountAmount") = dr("discount_amount")
                                row("DiscountedPrice") = dr("discounted_price")
                                row("VATPercent") = dr("vat_percent")
                                row("VATAmount") = dr("vat_amount")
                                row("LineTotal") = dr("amount")
                                row("VATExemptAmount") = dr("vat_exempt_amount")
                                row("Deductions") = dr("vat_exempt_amount")
                                row("IsVATable") = dr("is_vatable")
                                row("IsZeroRated") = dr("is_zero_rated")
                                row("IsVatExempt") = dr("is_vat_exempt")
                                row("IsGiftCertificate") = dr("is_gift_certificate")

                                dtItems.Rows.Add(row)
                            Next

                            lblCustomerName.Text = .CustomerName & If(m_IsCustomerScPwd, " - " & If(.CustomerIsSC, "SC", "PWD"), "")
                            lblShippingAddress.Text = .CustomerAddress
                            lblShippingAddressCaption.Text = If(String.IsNullOrWhiteSpace(.CustomerAddress), "", "Shipping Address")
                        End With

                        Dim hasDiscount As Boolean = m_HasDiscount

                        ComputeTotals()

                        If grdItems.Rows.Count > 0 Then
                            If hasDiscount <> m_HasDiscount Then
                                grdItems.DataBind()
                            End If
                            grdItems.Rows(0).Activate()
                        End If

                        btnSelectItem.Enabled = False
                        txtSKU.Enabled = False
                        btnSelectCustomer.Enabled = False
                        btnSetQty.Enabled = False
                        btnSetDiscount.Enabled = False
                        btnScPwd.Enabled = False
                        btnSalesperson.Enabled = False
                        btnVoidSale.CheckAccess()
                        btnSuspendSale.Enabled = False
                        btnCreditMemo.Enabled = False
                        btnCheckOut.Enabled = False
                        btnCancelSale.Enabled = True
                        btnCancelSale.Text = btnCancelSale.Text.Replace("Cancel Sale", "Cancel")

                        SaveAuditTrail(String.Format("Load Posted Sale - {0} - {1}", m_SaleCode, m_CustomerName), False)
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadRefundSale()
        Try
            Using frm As New FormCustomerReturnBrowser
                With frm
                    .CustomerCode = m_CustomerCode
                    .Text &= " - Refund Sale"

                    If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                        Dim dtItems As DataTable = grdItems.DataSource
                        dtItems.Rows.Clear()

                        m_HasDiscount = False
                        m_LoadedFromSales = True
                        m_RefundSale = True
                        m_VoidAllowed = False
                        m_InvoiceNo = .DocumentNo ' Customer Return/Refund Code
                        m_SaleCode = .SaleCode

                        Dim customerReturn As New CustomerReturns
                        Dim newRow As DataRow = Nothing

                        With customerReturn
                            .Fill(m_InvoiceNo)

                            m_CustomerCode = .CustomerCode
                            m_CustomerName = .CustomerName
                            lblCustomerName.Text = .CustomerName
                            lblShippingAddress.Text = .Remarks
                            lblShippingAddressCaption.Text = If(String.IsNullOrWhiteSpace(.Remarks), "", "Refund Remarks")
                        End With

                        For Each row As DataRow In customerReturn.Details.Rows
                            newRow = dtItems.NewRow
                            newRow("BarCode") = row("item_codename")
                            newRow("Quantity") = row("qty_returned")
                            newRow("ItemLineId") = row("item_line_id")
                            newRow("ItemCode") = row("item_code")
                            newRow("SKU") = row("item_sku")
                            newRow("Description") = row("item_description")
                            newRow("UnitOfMeasure") = row("uom")
                            newRow("ItemCategoryCode") = ""
                            newRow("UnitPrice") = row("price")
                            newRow("VATPercent") = m_Settings.SalesTax
                            newRow("VATAmount") = row("vat_amount")
                            newRow("QtyPerUOM") = 1
                            newRow("DiscountGroup") = ""
                            newRow("DiscountIsRegular") = True
                            newRow("DiscountPercent") = row("discount_percent")
                            newRow("DiscountAmount") = row("discount_amount")
                            newRow("DiscountedPrice") = row("discounted_price")
                            newRow("LineTotal") = row("line_total")
                            newRow("IsVATable") = row("is_vat")
                            newRow("IsZeroRated") = row("is_zero_rated")
                            newRow("IsVATExempt") = row("is_vat_exempt")
                            newRow("IsGiftCertificate") = CStr(row("item_code")).StartsWith("GC-")
                            dtItems.Rows.Add(newRow)
                        Next

                        Dim hasDiscount As Boolean = m_HasDiscount

                        ComputeTotals()

                        If grdItems.Rows.Count > 0 Then
                            If hasDiscount <> m_HasDiscount Then
                                grdItems.DataBind()
                            End If
                            grdItems.Rows(0).Activate()
                        End If

                        btnSelectItem.Enabled = False
                        txtSKU.Enabled = False
                        btnSelectCustomer.Enabled = False
                        btnScPwd.Enabled = False
                        btnVoidSale.Enabled = False
                        btnRefundSale.Enabled = True
                        btnSuspendSale.Enabled = False
                        btnCreditMemo.Enabled = False
                        btnCheckOut.Enabled = False
                        btnCancelSale.Enabled = True
                        btnCancelSale.Text = btnCancelSale.Text.Replace("Cancel Sale", "Cancel")

                        SaveAuditTrail(String.Format("Load Sale for Refund - {0} - {1}", frm.DocumentNo, m_CustomerName), False)
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadSuspendedSale()
        Try
            Using frm As New FormSuspendedSalesBrowser(m_Settings) With {
                .CashierSessionId = Current.Settings.CashierSessionId,
                .CustomerCode = If(IsWalkinCustomer(), "", m_CustomerCode),
                .TerminalCode = Current.Settings.TerminalCode
            }
                If LaunchForm(frm, True, Me) = Windows.Forms.DialogResult.OK Then
                    With frm
                        Dim dtItems As DataTable = grdItems.DataSource

                        dtItems.Rows.Clear()

                        m_HasDiscount = False
                        m_LoadedFromSales = True
                        m_SuspendSaleCode = frm.SuspendCode
                        m_CustomerCode = frm.CustomerCode

                        With m_Customer
                            .No = m_CustomerCode
                            .Name = frm.Header("customer_name")
                            .Address = frm.Header("customer_address")
                            .AllowLineDisc = True
                            .CustomerDiscGroup = frm.Header("customer_discount_group")

                            m_CustomerCode = .No
                            m_CustomerName = .Name

                            lblCustomerName.Text = .Name

                            .CustomerDiscGroup = CStrEx(.CustomerDiscGroup).Trim

                            If IsDiscountTypeSCPWD(.CustomerDiscGroup) Then
                                m_IsCustomerScPwd = True
                            Else
                                m_IsCustomerScPwd = False

                                lblShippingAddress.Text = CStr(CStrEx(.Address) & " " & CStrEx(.Address2)).Trim
                                lblShippingAddressCaption.Text = If(String.IsNullOrWhiteSpace(lblShippingAddress.Text), "", "Shipping Address")
                            End If
                        End With

                        If m_IsCustomerScPwd Then
                            Dim scPwdRow As DataRow

                            If m_ScPwdDiscountData.Rows.Count = 0 Then
                                scPwdRow = m_ScPwdDiscountData.NewRow
                                scPwdRow("discount_type") = m_Customer.CustomerDiscGroup
                            Else
                                scPwdRow = m_ScPwdDiscountData.Rows(0)
                            End If

                            scPwdRow("name") = frm.Header("customer_name")
                            scPwdRow("id_no") = frm.Header("sc_pwd_no")
                            scPwdRow("gender") = frm.Header("sc_pwd_gender")
                            scPwdRow("birthdate") = frm.Header("sc_pwd_birthdate")
                            scPwdRow("issued_date") = frm.Header("sc_pwd_issued_date")

                            If scPwdRow.RowState = DataRowState.Detached Then
                                m_ScPwdDiscountData.Rows.Add(scPwdRow)
                            End If

                            If IsWalkinCustomer() Then
                                InitializeWalkInCustomer(m_Customer.CustomerDiscGroup)
                            Else
                                Select Case scPwdRow("discount_type")
                                    Case DISCOUNT_GROUP_SC
                                        lblCustomerName.Text &= " - SC"
                                    Case DISCOUNT_GROUP_PWD
                                        lblCustomerName.Text &= " - PWD"
                                End Select
                            End If
                        End If

                        Dim row As DataRow
                        For Each dr As DataRow In .Details.Rows
                            row = dtItems.NewRow
                            row("BarCode") = dr("item_codename")
                            row("Quantity") = dr("qty")
                            row("ItemLineId") = dr("item_line_id")
                            row("ItemCode") = dr("item_code")
                            row("SKU") = dr("item_codename")
                            row("Description") = dr("item_description")
                            row("ItemCategoryCode") = dr("class_code")
                            row("UnitOfMeasure") = dr("uom")
                            row("UnitPrice") = dr("unit_price")
                            row("BasePrice") = dr("base_price")
                            row("VATPercent") = If(dr("is_vat"), m_Settings.SalesTax, 0.0)
                            row("VATAmount") = dr("vat_amount")
                            row("VATExemptAmount") = dr("vat_exempt_amount")
                            row("DiscountGroup") = dr("item_discount_group")
                            row("DiscountIsAllowed") = dr("is_discount_allowed")
                            row("DiscountIsRegular") = dr("is_regular_discount")
                            row("DiscountPercent") = dr("discount_percent")
                            row("DiscountAmount") = dr("discount_amount")
                            row("DiscountedPrice") = dr("discounted_price")
                            row("LineTotal") = dr("line_total")

                            row("IsVATable") = dr("is_vat")
                            row("IsZeroRated") = dr("is_zero_rated")
                            row("IsVATExempt") = dr("is_vat_exempt")
                            row("IsGiftCertificate") = dr("is_gift_certificate")

                            ComputeVATDiscount(row, False, True)
                            dtItems.Rows.Add(row)
                        Next

                        If grdItems.Rows.Count > 0 Then
                            grdItems.ActiveRow = grdItems.Rows(grdItems.Rows.Count - 1)
                        End If

                        Dim hasDiscount As Boolean = m_HasDiscount

                        ComputeTotals()

                        If hasDiscount <> m_HasDiscount Then
                            m_HasDiscount = True
                        End If

                        dtItems.AcceptChanges()
                        grdItems.DataBind()

                        btnSuspendSale.Enabled = True
                        btnCreditMemo.CheckAccess()
                        btnCheckOut.Enabled = True
                        btnCancelSale.Enabled = True
                        btnCancelSale.Text = btnCancelSale.Text.Replace("Cancel Sale", "Cancel")

                        SaveAuditTrail(String.Format("Load Suspended Sale - {0} - {1}", m_CustomerName, m_SuspendSaleCode), True, m_SuspendSaleCode)
                    End With
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PrintVoidedSalesReceipt(saleCode As String)
        Try
            Try
                With New Reporting(m_Settings)
                    .Cashier = Current.User.Name
                    .TransactionDate = Now
                    .SalesTax = m_Settings.SalesTax
                    .HasOwnReportViewer = True

                    Dim data As DataSet = Nothing
                    Dim rpt = .ShowSale(data, saleCode,, True)

                    Try
                        If data IsNot Nothing Then
                            .SaveEJournalReceipt(data, saleCode,, True)
                        End If
                    Catch
                    End Try

                    If rpt IsNot Nothing Then
                        With DirectCast(rpt, SalesReceiptReport)
                            .PrintToPrinter(1, False, -1, -1)
                        End With
                    End If
                End With
            Catch ex As Exception
                Dim errmsg As String = ex.Message
                If errmsg = "The RPC server is unavailable" Then
                    errmsg = "Could not connect to a network printer."
                End If
                ShowWarning("An error has occured while printing the receipt:\n\n" & ex.Message, "Printing Error")
            End Try
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PrintRefundSaleReceipt(refundCode As String)
        Try
            Try
                With New Reporting(m_Settings)
                    .Cashier = Current.User.Name
                    .TransactionDate = Now
                    .SalesTax = m_Settings.SalesTax
                    .HasOwnReportViewer = True

                    Dim data As DataSet = Nothing
                    Dim rpt = .ShowSale(data, refundCode,,, True)

                    Try
                        If data IsNot Nothing Then
                            .SaveEJournalReceipt(data, refundCode,,, True)
                        End If
                    Catch
                    End Try

                    If rpt IsNot Nothing Then
                        With DirectCast(rpt, SalesReceiptReport)
                            .PrintToPrinter(1, False, -1, -1)
                        End With
                    End If
                End With
            Catch ex As Exception
                Dim errmsg As String = ex.Message
                If errmsg = "The RPC server is unavailable" Then
                    errmsg = "Could not connect to a network printer."
                End If
                ShowWarning("An error has occured while printing the receipt:\n\n" & ex.Message, "Printing Error")
            End Try
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function SuspendSales() As Boolean
        Try
            With New SuspendedSale
                .Code = m_SuspendSaleCode
                .Terminal = Current.Settings.TerminalCode
                .TransactionDate = Now
                .CustomerCode = m_CustomerCode
                .Customer = m_CustomerName
                .IsSC = (m_Customer.CustomerDiscGroup = "SC")
                .IsPWD = (m_Customer.CustomerDiscGroup = "PWD")

                If m_ScPwdDiscountData.Rows.Count > 0 Then
                    Dim row As DataRow = m_ScPwdDiscountData.Rows(0)
                    .ScPwdNo = row("id_no")
                    .ScPwdGender = row("gender")
                    .ScPwdBirthdate = GetDateForVariable(row("birthdate"))
                    .ScPwdIssuedDate = GetDateForVariable(row("issued_date"))
                Else
                    .ScPwdNo = ""
                    .ScPwdGender = ""
                    .ScPwdBirthdate = Nothing
                    .ScPwdIssuedDate = Nothing
                End If

                .TotalDiscount = m_TotalRegularDiscount + m_TotalSCPwdDiscount
                .TotalVAT = m_TotalVATAmount
                .TotalAmount = m_TotalSales
                .CustomerDiscountGroup = m_Customer.CustomerDiscGroup
                .CashierCode = Current.User.Code
                .Cashier = Current.User.Name
                .CashierSessionId = Current.Settings.CashierSessionId
                .Operation = If(String.IsNullOrWhiteSpace(.Code), "INSERT", "UPDATE")

                .Details = grdItems.DataSource

                If .Save() Then
                    m_SuspendSaleCode = .Code
                    Return True
                Else
                    Return False
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function ValidateCheckOut() As Boolean
        Try
            If grdItems.Rows.Count = 0 Then
                ShowWarning("There are no items to check out!", "Check Out Failed")
                Return False
            End If
            If m_IsCustomerScPwd Then
                Dim hasError As Boolean
                For Each item As DataRow In m_dtItemsSale.Rows
                    If (CDblEx(item("UnitPrice")) * CDblEx(item("Quantity"))) > 2500 Then
                        hasError = True
                        Exit For
                    End If
                Next
                If hasError Then
                    ShowWarning("One or more items exceeds the allowed P2,500 line limit.\n\nSC/PWD transaction not allowed to proceed.", "Checkout Validation")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SavePayment(sale As Sale, cash As DataTable, creditCard As DataTable, creditMemo As DataTable) As Boolean
        Try
            With sale
                .Code = m_SaleCode
                .CashierSessionId = Current.Settings.CashierSessionId
                .TerminalCode = Current.Settings.TerminalCode
                .TransactionDate = Now
                .CustomerCode = m_CustomerCode
                .CustomerName = m_CustomerName
                .CustomerTIN = CStrEx(m_Customer.VATRegistrationNo)
                .CustomerBusinessStyle = CStrEx(m_Customer.BusinessStyle)
                .CustomerAddress = lblShippingAddress.Text
                .SalespersonCode = CStrEx(m_Customer.SalesPersonCode)
                .SalespersonName = If(String.IsNullOrWhiteSpace(.SalespersonCode), "", lblSalesperson.Text)
                .CashierCode = Current.User.Code
                .CashierName = Current.User.Name
                .InvoiceCode = m_InvoiceNo
                .PaymentCode = m_PaymentCode
                .TotalVAT = m_TotalVATAmount
                .TotalDiscount = (m_TotalRegularDiscount + m_TotalSCPwdDiscount)
                .TotalAmount = m_TotalSales
                .TotalVATSales = m_TotalVATableSales
                .TotalVATExemptSales = m_TotalNonVATSales
                .TotalZeroRatedSales = m_TotalZeroRatedSales
                .TotalRegularLessVAT = m_TotalRegularLessVAT
                .SuspendedSaleCode = m_SuspendSaleCode

                .Items = m_dtItemsSale

                .SeniorPwdDiscountData = m_ScPwdDiscountData
                .PaymentCash = cash
                .PaymentCreditCard = creditCard
                .PaymentCreditMemo = creditMemo
                .PaymentGiftCertificate = m_GiftCertData

                Return .Save()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub InsertExcessGCAmount(amount As Double)
        Try
            Dim gcxRow As DataRow = m_dtItemsSale.NewRow

            gcxRow("BarCode") = ""
            gcxRow("ItemCode") = "GCX"
            gcxRow("SKU") = "GCX"
            gcxRow("Description") = "Excess GC Amount"
            gcxRow("UnitOfMeasure") = DEFAULT_UOM
            gcxRow("ItemCategoryCode") = "ITEM"
            gcxRow("UnitPrice") = amount
            gcxRow("DiscountGroup") = ""
            gcxRow("DiscountPercent") = 0.0
            gcxRow("VATPercent") = m_Settings.SalesTax
            gcxRow("VATAmount") = (amount / (1 + m_Settings.SalesTax / 100) * (m_Settings.SalesTax / 100))
            gcxRow("IsVATable") = True
            gcxRow("IsZeroRated") = False
            gcxRow("IsVATExempt") = False
            gcxRow("IsGiftCertificate") = False
            gcxRow("LineTotal") = amount

            m_dtItemsSale.Rows.Add(gcxRow)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsWalkinCustomer() As Boolean
        Try
            If m_CustomerCode = CUSTOMER_WALKIN Then
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub PrepareScPwdForCheckOut()
        Try
            Dim totalDiscount As Double
            Dim totalLessVat As Double

            For Each item As DataRow In DirectCast(grdItems.DataSource, DataTable).Rows
                If item.RowState = DataRowState.Deleted Then
                    Continue For
                End If

                If Not CBool(item("DiscountIsRegular")) Then
                    totalDiscount += item("DiscountAmount") * item("Quantity")
                    totalLessVat += item("VATExemptAmount") * item("Quantity")
                End If
            Next

            If (m_ScPwdDiscountData.Rows.Count > 0) Then
                Dim row As DataRow = Nothing

                For Each item As DataRow In m_ScPwdDiscountData.Rows
                    If item.RowState = DataRowState.Deleted Then
                        Continue For
                    Else
                        row = item
                        Exit For
                    End If
                Next

                If row IsNot Nothing Then
                    row("total_discount") = totalDiscount
                    row("total_less_vat") = totalLessVat
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveAuditTrail(activity As String, Optional updateTransactionCode As Boolean = False, Optional transactionCode As String = "")
        Try
            With New TaskProcess(Me)
                .Start(
                    Sub()
                        Try
                            With m_AuditTrail
                                .TerminalCode = Current.Settings.TerminalCode
                                .UserCode = Current.User.Code
                                .UserName = Current.User.Name
                                .Activity = activity
                                .SaveAuditTrail()

                                If updateTransactionCode Then
                                    .TransactionCode = transactionCode
                                    .UpdateAuditTrail()
                                End If
                            End With
                        Catch ex As Exception
                            ex.ShowError()
                        End Try
                    End Sub
                )
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LogOff()
        Try
            With New CashUp
                .LogOutSession(Current.User.Code, Current.Settings.TerminalCode)
            End With

            SaveAuditTrail("Logged-out on POS System - " & Current.User.Name & " (" & Current.User.RoleName & ")", False, "")
            m_LoggingOff = True

            Diagnostics.Process.Start(My.Application.Info.AssemblyName & ".exe", "-restart")
            Application.Exit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
