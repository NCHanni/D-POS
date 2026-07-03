Imports System.IO

Friend Class Reporting
    Inherits DataSource

#Region "Declarations"

    Private Const LINE_WIDTH As Integer = POS.Admin.Reporting.LINE_WIDTH
    Private Const LINE_SEPARATOR As String = POS.Admin.Reporting.LINE_SEPARATOR

    Private Const MESSAGE_NO_DATA_CAPTION As String = "No Data"
    Private Const MESSAGE_NO_DATA As String = "Report contains no data for printing."
    Private Const DEFAULT_FOOTER1 As String = "THIS SERVES AS YOUR OFFICIAL RECEIPT!"
    Private Const DEFAULT_FOOTER2 As String = "THANK YOU. PLEASE COME AGAIN!"
    Private Const DEFAULT_FOOTER As String = "DEFAULT"

    Private ReadOnly m_Settings As SettingsPreferences
    Private WithEvents m_ReportViewer As POS.Admin.FormReportViewer

    Public Enum EJournalTypes
        [Default]
        Receipts
        ReceiptsVoid
        ReceiptsRefund
        XReadings
        ZReadings
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(settings As SettingsPreferences)
        Try
            m_Settings = settings
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

    Property AllowShowEmptyReport As Boolean
    Property SuppressEmptyDataMessage As Boolean
    Property HasOwnReportViewer As Boolean

    Property Cashier As String
    Property SalesTax As Double
    Property TransactionDate As DateTime

#End Region

#Region "Report Viewer"

    Private Sub m_ReportViewer_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ReportViewer.Shown
        Try
            Application.UseWaitCursor = False
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Methods"

#Region "Private"

    Private Function ShowReport(
            ByVal report As Object,
            ByVal data As Object,
            ByVal reportTitle As String,
            Optional ByVal header1 As String = "",
            Optional ByVal _Header2 As String = "",
            Optional ByVal _Header3 As String = "",
            Optional ByVal _Header4 As String = "",
            Optional ByVal _Footer1 As String = "",
            Optional hasCustomContactDetails As Boolean = False) As Object
        Try
            If report Is Nothing Then
                Return Nothing
            Else
                Application.UseWaitCursor = True
            End If

            Dim hasNoData As Boolean
            If data.GetType.Name = "DataTable" Then
                With DirectCast(data, DataTable)
                    If .Rows.Count = 0 Then
                        hasNoData = True
                    End If
                End With
            ElseIf data.GetType.Name = "DataSet" Then
                With DirectCast(data, DataSet)
                    If .Tables.Count = 0 Then
                        hasNoData = True
                    ElseIf .Tables(0).Rows.Count = 0 Then
                        hasNoData = True
                    End If
                End With
            End If

            If hasNoData Then
                Application.UseWaitCursor = False
                If Not HasOwnReportViewer AndAlso Not SuppressEmptyDataMessage Then
                    Application.UseWaitCursor = False
                    ShowMessage(MESSAGE_NO_DATA, MESSAGE_NO_DATA_CAPTION, MessageIcon.Exclamation)
                End If
                If Not AllowShowEmptyReport Then
                    Application.UseWaitCursor = False
                    Return Nothing
                End If
            End If

            With report
                If .Filename = "" Then
                    .SetDataSource(data)
                End If

                .SetParameterValue("company_name", Current.CompanyInfo.Name)
                .SetParameterValue("company_address", Current.CompanyInfo.Address)

                If Not hasCustomContactDetails Then
                    .SetParameterValue("company_phone", Current.CompanyInfo.ContactNo)
                End If

                .SetParameterValue("report_title", reportTitle.ToUpper) ' ALL CAPS REPORT TITLE

                If header1 <> "" Then .SetParameterValue("report_header1", header1)
                If _Header2 <> "" Then .SetParameterValue("report_header2", _Header2)
                If _Header3 <> "" Then .SetParameterValue("report_header3", _Header3)
                If _Header4 <> "" Then .SetParameterValue("report_header4", _Header4)
                If _Footer1 <> "" Then .SetParameterValue("report_footer1", _Footer1)
            End With

            If Not HasOwnReportViewer Then
                m_ReportViewer = New POS.Admin.FormReportViewer(report, reportTitle)
                LaunchForm(m_ReportViewer, False)
            Else
                Application.UseWaitCursor = False
            End If

            Return report
        Catch ex As Exception
            Application.UseWaitCursor = False
            Throw
        End Try
    End Function

    Private Function PrepareJournalDirectory(eJournalType As EJournalTypes) As String
        Try
            Const DIR_BIZPRO As String = "BusinessPro"

            Dim dirPOS As String = My.Application.Info.Description.Replace(DIR_BIZPRO, "").Trim
            Dim pathAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

            Dim eJournalPath As String = Path.Combine(
                pathAppData, My.Application.Info.CompanyName & "\" & DIR_BIZPRO & "\" & dirPOS & "\")

            If Not Directory.Exists(eJournalPath) Then
                Directory.CreateDirectory(eJournalPath)
            End If

            Const RECEIPTS As String = "Receipts\"
            Const RECEIPTSVOID As String = "ReceiptsVoid\"
            Const RECEIPTSREFUND As String = "ReceiptsRefund\"
            Const XREADINGS As String = "XReadings\"
            Const ZREADINGS As String = "ZReadings\"

            Select Case eJournalType
                Case EJournalTypes.Receipts
                    eJournalPath = Path.Combine(eJournalPath, RECEIPTS)
                Case EJournalTypes.ReceiptsVoid
                    eJournalPath = Path.Combine(eJournalPath, RECEIPTSVOID)
                Case EJournalTypes.ReceiptsRefund
                    eJournalPath = Path.Combine(eJournalPath, RECEIPTSREFUND)
                Case EJournalTypes.XReadings
                    eJournalPath = Path.Combine(eJournalPath, XREADINGS)
                Case EJournalTypes.ZReadings
                    eJournalPath = Path.Combine(eJournalPath, ZREADINGS)
            End Select

            If Not Directory.Exists(eJournalPath) Then
                Directory.CreateDirectory(eJournalPath)
            End If

            Return eJournalPath
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function FormatSales(
                ByVal ds As DataSet,
                ByRef hasScPwdDiscount As Boolean,
                Optional isVoid As Boolean = False,
                Optional isRefund As Boolean = False) As DataSet
        Try
            Dim rowHeader As DataRow
            Dim subTotal As Object = DBNull.Value
            Dim discount As Object = DBNull.Value
            Dim scPwdDiscount As Object = DBNull.Value
            Dim scPwdVatExempt As Object = DBNull.Value
            Dim scPwdVatExemptSales As Object = DBNull.Value
            Dim amountDue As Object = DBNull.Value
            Dim cashAmount As Object = DBNull.Value
            Dim creditMemoNo As Object = DBNull.Value
            Dim creditMemoAmount As Object = DBNull.Value
            Dim changeAmount As Object = DBNull.Value
            Dim vatSales As Object = DBNull.Value
            Dim nonVatSales As Object = DBNull.Value
            Dim zeroRatesSales As Object = DBNull.Value
            Dim vatAmount As Object = DBNull.Value
            Dim isPWD As Boolean
            Dim isSC As Boolean

            If ds.Tables(0).Rows.Count > 0 Then
                rowHeader = ds.Tables(0).Rows(0)

                isSC = CBool(rowHeader("is_sc"))
                isPWD = CBool(rowHeader("is_pwd"))
                subTotal = rowHeader("subtotal")
                amountDue = rowHeader("amount_due")
                discount = rowHeader("discount")
                scPwdDiscount = rowHeader("sc_pwd_discount")
                scPwdVatExempt = rowHeader("sc_pwd_vat_exempt")
                scPwdVatExemptSales = rowHeader("sc_pwd_vat_exempt_sales")

                cashAmount = rowHeader("cash")
                changeAmount = rowHeader("change")
                creditMemoNo = rowHeader("credit_memo_no")
                creditMemoAmount = rowHeader("credit_memo")

                vatSales = rowHeader("vat_sales")
                vatAmount = rowHeader("vat_amount")
                nonVatSales = rowHeader("non_vat_sales")
                zeroRatesSales = rowHeader("zero_rates_sales")
            End If

            hasScPwdDiscount = (scPwdDiscount <> 0.0)

            ds.Tables(0).TableName = "Header"
            ds.Tables(1).TableName = "Items"
            ds.Tables(2).TableName = "CreditCard"
            ds.Tables(3).TableName = "GiftCertificate"
            ds.Tables(4).TableName = "SrPwdDiscDetails"
            ds.Tables(5).TableName = "CustomerReturns"

            If isVoid Then
                cashAmount = (Math.Abs(cashAmount) - Math.Abs(changeAmount)) * -1
                changeAmount = 0
            End If

            Dim key As DataColumn
            Dim value As DataColumn
            Dim desc As DataColumn

            Dim dtSummary As New DataTable("Summary")
            With dtSummary
                key = .Columns.Add("Key")
                value = .Columns.Add("Value", GetType(Double))
                desc = .Columns.Add("Description")

                key.DefaultValue = ""
                value.AllowDBNull = True

                .Rows.Add(New Object() {"Gross Total", subTotal, ""})

                If Not isRefund AndAlso (scPwdDiscount <> 0.0 OrElse discount <> 0.0) Then
                    .Rows.Add(New Object() {"", Nothing, ""})
                    .Rows.Add(New Object() {"Total Price", subTotal, ""})

                    If discount <> 0.0 Then
                        .Rows.Add(New Object() {"Less: Regular Discount", discount, ""})
                    End If

                    If scPwdDiscount <> 0.0 Then
                        .Rows.Add(New Object() {"Less: SC/PWD Discount", scPwdDiscount, ""})
                    End If

                    If scPwdVatExempt <> 0.0 Then
                        .Rows.Add(New Object() {"Less: VAT Exempt", scPwdVatExempt, ""})
                    End If
                End If

                .Rows.Add(New Object() {"", Nothing, ""})
                .Rows.Add(New Object() {"TOTAL AMOUNT DUE", amountDue, ""})

                If cashAmount <> 0.0 Then
                    .Rows.Add(New Object() {"Cash", cashAmount, ""})
                End If

                If Not isRefund Then
                    'Credit Memo
                    If Not String.IsNullOrWhiteSpace(creditMemoNo) Then
                        .Rows.Add(New Object() {"Credit Memo", creditMemoAmount, ""})
                    End If
                End If

                'Credit Card
                For Each row As DataRow In ds.Tables("CreditCard").Rows
                    .Rows.Add(New Object() {String.Format("CC - {0}", row("card_type")), row("amount"), ""})
                    .Rows.Add(New Object() {"    " & row("bank_name") & " - " & row("card_holder"), Nothing, ""})
                Next

                'Gift Certificate
                For Each row As DataRow In ds.Tables("GiftCertificate").Rows
                    .Rows.Add(New Object() {String.Format("GC - {0}", row("code")), row("amount"), ""})
                Next

                If Not isRefund AndAlso cashAmount <> 0.0 Then
                    .Rows.Add(New Object() {"CHANGE", changeAmount, ""})
                End If
            End With
            ds.Tables.Add(dtSummary)

            Dim dtVATDetails As New DataTable("VAT Summary")
            With dtVATDetails
                key = .Columns.Add("Key")
                value = .Columns.Add("Value", GetType(Double))
                desc = .Columns.Add("Description")

                key.DefaultValue = ""
                value.AllowDBNull = True

                If Not isRefund Then
                    .Rows.Add(New Object() {"VATable Sale", vatSales, ""})
                    .Rows.Add(New Object() {String.Format("VAT Amount ({0}%)", SalesTax), vatAmount, ""})
                    .Rows.Add(New Object() {"VAT-Exempt Sale", scPwdVatExemptSales, ""})
                    .Rows.Add(New Object() {"Zero-Rated Sale", zeroRatesSales, ""})
                End If
            End With
            ds.Tables.Add(dtVATDetails)

            Dim dtScPwdDiscDetails As DataTable = ds.Tables("SrPwdDiscDetails")
            Dim dtSrPwdSummary As New DataTable("SrPwdSummary")
            With dtSrPwdSummary
                key = .Columns.Add("Key")
                value = .Columns.Add("Value", GetType(Double))
                desc = .Columns.Add("Description")

                key.DefaultValue = ""
                value.AllowDBNull = True

                If hasScPwdDiscount Then
                    For Each row As DataRow In dtScPwdDiscDetails.Rows
                        If .Rows.Count > 0 Then
                            .Rows.Add(New Object() {"", Nothing, ""})
                        End If

                        If row("total_sales") > 0.0 Then
                            .Rows.Add(New Object() {"SC/PWD Sales", String.Format("{0:N}", row("total_sales")), ""})
                        End If

                        If row("vat_exempt_sales") > 0.0 Then
                            .Rows.Add(New Object() {"VAT-Exempt Sales", String.Format("{0:N}", row("vat_exempt_sales")), ""})
                        End If

                        If row("vat_exempt_amount") > 0.0 Then
                            .Rows.Add(New Object() {"VAT Adjustments", String.Format("{0:N}", row("vat_exempt_amount")), ""})
                        End If

                        .Rows.Add(New Object() {
                            String.Format("Discount ({0:0}%)", row("discount_percent")),
                                String.Format("{0:N}", row("discount_amount")), ""})
                    Next
                End If
            End With
            ds.Tables.Add(dtSrPwdSummary)

            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SaveEJournalConsolidated(eJournalFile As String, journalDate As Date)
        Try
            Dim eJournalDaily As String = PrepareJournalDirectory(EJournalTypes.Default)
            Dim fileName As String = String.Concat(journalDate.ToString("yyyy-MM-dd E-Journal"), ".txt")

            eJournalDaily = Path.Combine(eJournalDaily, fileName)

            Dim content As String() = File.ReadAllLines(eJournalFile)

            Using file As New StreamWriter(eJournalDaily, True)
                For Each line As String In content
                    file.WriteLine(line)
                Next

                file.WriteLine(vbCrLf & vbCrLf)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function FormatAmount(value As Double) As String
        Return POS.Admin.Reporting.FormatAmount(value)
    End Function

    Private Function FormatCount(value As Integer) As String
        Return POS.Admin.Reporting.FormatCount(value)
    End Function

    Private Function CenterText(text As String) As String
        Return POS.Admin.Reporting.CenterText(text)
    End Function

#End Region

#Region "Public"

    Public Function ShowSale(
                ByRef data As DataSet,
                ByVal postedSalesDocNo As String,
                Optional isRePrint As Boolean = False,
                Optional isVoid As Boolean = False,
                Optional isRefund As Boolean = False) As Object
        Try
            Dim hasSrPwdDiscount As Boolean

            If data Is Nothing Then
                Dim sql As String =
                    "EXEC dbo.GetSalesReport '" & postedSalesDocNo & "', " & If(isVoid, 1, 0) & ", " & If(isRefund, 1, 0)

                data = ExecuteQuery(sql)
                data = FormatSales(data, hasSrPwdDiscount, isVoid, isRefund)
            End If

#If DEBUG Then
            data.WriteXmlSchema("SalesReceipt.xml")
#End If

            Dim rpt As Object
            Dim contactDetails As String = Current.CompanyInfo.ContactNo

            If m_Settings.UsePrePrintedInvoice Then
                rpt = New SalesReceiptReportA4
            Else
                rpt = New SalesReceiptReport
            End If

            If Not String.IsNullOrWhiteSpace(Current.CompanyInfo.FaxNo) Then
                contactDetails &= " • Fax: " & Current.CompanyInfo.FaxNo
            End If

            If Not String.IsNullOrWhiteSpace(Current.CompanyInfo.EmailAddress) Then
                contactDetails &= vbNewLine & Current.CompanyInfo.EmailAddress
            End If

            If Not String.IsNullOrWhiteSpace(Current.CompanyInfo.WebsiteUrl) Then
                If String.IsNullOrWhiteSpace(Current.CompanyInfo.EmailAddress) Then
                    contactDetails &= vbNewLine & Current.CompanyInfo.WebsiteUrl
                Else
                    contactDetails &= " • " & Current.CompanyInfo.WebsiteUrl
                End If
            End If

            With rpt
                .SetDataSource(data)
                With rpt
                    .SetParameterValue("company_name", Current.CompanyInfo.Name)
                    .SetParameterValue("company_name_2", Current.CompanyInfo.Name2)
                    .SetParameterValue("company_address", Current.CompanyInfo.Address)
                    .SetParameterValue("company_phone", contactDetails & If(String.IsNullOrWhiteSpace(contactDetails), " ", ""))
                    .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                    .SetParameterValue("report_title", "")

                    .SetParameterValue("permit_no", Current.CompanyInfo.PermitNo)
                    .SetParameterValue("permit_issued", Current.CompanyInfo.PermitIssued)
                    .SetParameterValue("permit_expiry", Current.CompanyInfo.PermitExpiry)
                    .SetParameterValue("company_serial_no", Current.Settings.TerminalSerialNo)
                    .SetParameterValue("company_min_no", Current.CompanyInfo.PermitMIN)
                    .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)

                    .SetParameterValue("has_sc_pwd_discount", hasSrPwdDiscount)
                    .SetParameterValue("is_vatable", Not String.IsNullOrWhiteSpace(Current.CompanyInfo.VatRegistrationNo))
                    .SetParameterValue("is_reprint", isRePrint)
                    .SetParameterValue("is_void", isVoid)
                    .SetParameterValue("is_refund", isRefund)
                    .SetParameterValue("is_customer_copy", False)
                    .SetParameterValue("is_store_copy", False)
                    .SetParameterValue("is_training_mode", m_Settings.TrainingMode)
                End With

                Dim reportTitle As String = "Sales Receipt"

                If isVoid Then
                    reportTitle = "Void Receipt"
                ElseIf isRefund Then
                    reportTitle = "Refund Receipt"
                End If

                Return ShowReport(rpt, data, reportTitle, hasCustomContactDetails:=True)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ShowCashup(cashierSessionId As Long, data As DataSet)
        Try
#If DEBUG Then
            data.WriteXmlSchema("XReading.xml")
#End If

            Dim rpt As New CashupReport
            With rpt
                .SetDataSource(data)
                With rpt
                    .SetParameterValue("company_name", Current.CompanyInfo.Name)
                    .SetParameterValue("company_address", Current.CompanyInfo.Address)
                    .SetParameterValue("company_phone", Current.CompanyInfo.ContactNo & If(String.IsNullOrWhiteSpace(Current.CompanyInfo.ContactNo), " ", ""))
                    .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                    .SetParameterValue("trader_name", Current.CompanyInfo.BusinessStyle)

                    .SetParameterValue("company_serial_no", Current.Settings.TerminalSerialNo)
                    .SetParameterValue("company_min_no", Current.CompanyInfo.PermitMIN)
                    .SetParameterValue("is_training_mode", m_Settings.TrainingMode)
                End With
            End With

            Try
                SaveEJournalXReading(cashierSessionId, data)
            Catch ex As Exception
                'ignore
            End Try

            Return rpt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub SaveEJournalReceipt(
                ByRef data As DataSet,
                ByVal postedSalesDocNo As String,
                Optional ByVal isRePrint As Boolean = False,
                Optional ByVal isVoid As Boolean = False,
                Optional ByVal isRefund As Boolean = False)
        Try
            Dim eJournalType As EJournalTypes = EJournalTypes.Receipts

            If isVoid Then
                eJournalType = EJournalTypes.ReceiptsVoid
            ElseIf isRefund Then
                eJournalType = EJournalTypes.ReceiptsRefund
            ElseIf String.IsNullOrEmpty(postedSalesDocNo) Then
                Return
            End If

            Dim eJournalPath As String = PrepareJournalDirectory(eJournalType)
            Dim header As DataRow = data.Tables("Header").Rows(0)
            Dim trnDate As Date = CDate(header("date"))
            Dim hasReturns As Boolean = CBool(header("has_returns"))
            Dim items As DataRowCollection = data.Tables("Items").Rows
            Dim creditCards As DataRowCollection = data.Tables("CreditCard").Rows
            Dim giftCertificates As DataRowCollection = data.Tables("GiftCertificate").Rows
            Dim scPwdDiscountDetails As DataRowCollection = data.Tables("SrPwdDiscDetails").Rows
            Dim customerReturns As DataRowCollection = data.Tables("CustomerReturns").Rows
            Dim hasScPwdDiscount As Boolean = (header("sc_pwd_discount") <> 0.0)

            Dim fileName As String = header("transaction_no") &
                If(isRePrint, "-Reprint", "") &
                If(isVoid, "-Void", "") &
                If(isRefund, "-Refund", "") & ".txt"

            eJournalPath = Path.Combine(eJournalPath, fileName)

            Using file As New StreamWriter(eJournalPath, False)
                file.WriteLine("/*" & LINE_SEPARATOR.Substring(2))

                If Not isVoid AndAlso Not isRefund Then
                    file.WriteLine(CenterText("SALES INVOICE"))
                End If

                If isRePrint Then
                    If hasReturns Then
                        file.WriteLine(CenterText("- WITH RETURNS -"))
                    End If
                    file.WriteLine(CenterText("*REPRINT COPY*"))
                    file.WriteLine(CenterText("Reprint Date: " & Now.ToDateString & "  Reprint Time: " & Now.ToShortTimeString))
                ElseIf isVoid Then
                    file.WriteLine(CenterText("VOID SALE"))
                ElseIf isRefund Then
                    file.WriteLine(CenterText("REFUND SALE"))
                    file.WriteLine(CStr("  Refund Date: " & Now.ToDateString).PadRight(25) & CStr("Refund Time: " & Now.ToShortTimeString & "  ").PadLeft(25))
                End If

                If m_Settings.TrainingMode Then
                    file.WriteLine(CenterText("- TRAINING MODE -"))
                End If

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CStr("  Date: " & trnDate.ToDateString).PadRight(25) & ("Time: " & trnDate.ToShortTimeString & "  ").PadLeft(25))
                file.WriteLine()


                If isVoid Then
                    file.WriteLine("  Void No.:".PadRight(20) & header("void_no"))
                ElseIf isRefund Then
                    file.WriteLine("  Refund No.:".PadRight(20) & header("void_no"))
                Else
                    file.WriteLine("  SI No.:".PadRight(20) & header("transaction_no"))
                End If

                file.WriteLine("  Cashier:".PadRight(20) & header("cashier"))
                file.WriteLine("  Terminal No.:".PadRight(20) & header("terminal"))
                file.WriteLine()

                file.WriteLine("  Customer:".PadRight(20) & header("customer"))
                file.WriteLine("  Customer TIN:".PadRight(20) & header("customer_tin") & " ")
                file.WriteLine("  Business Style:".PadRight(20) & header("customer_business_style") & " ")
                file.WriteLine("  Customer Address:".PadRight(20) & header("customer_address") & " ")

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine("Item".PadRight(12) & "Qty".PadLeft(8) & "Price".PadLeft(15) & "Line Total".PadLeft(15))
                file.WriteLine(LINE_SEPARATOR)

                For Each item As DataRow In items
                    'If m_Settings.ShowItemCodeOnInvoice Then
                    '    file.WriteLine(item("item_code"))
                    'End If

                    file.WriteLine(item("description"))
                    file.WriteLine(
                        FormatAmount(item("quantity")).PadLeft(20) &
                        FormatAmount(item("unit_price")).PadLeft(15) &
                        FormatAmount(item("gross_amount")).PadLeft(15))

                    If hasScPwdDiscount AndAlso CDbl(item("vat_amount")) <> 0.0 AndAlso Not CBool(item("is_regular_discount")) Then
                        file.WriteLine(
                            CStr("Less VAT").PadLeft(20) &
                            FormatAmount(item("vat_amount")).PadLeft(15) &
                            FormatAmount(item("vat_exclusive_total")).PadLeft(15))
                    End If

                    If CDbl(item("discount_percent")) <> 0.0 Then
                        file.WriteLine(
                            CStr("Less " & CDbl(item("discount_percent")).ToString("#0.##") & "%").PadLeft(20) &
                            FormatAmount(item("discount_amount")).PadLeft(15) &
                            FormatAmount(item("amount")).PadLeft(15))
                    End If
                Next

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine("  Gross Total:".PadRight(20) & FormatAmount(header("subtotal")).PadLeft(28))

                If Not isRefund AndAlso (header("sc_pwd_discount") <> 0.0 OrElse header("discount") <> 0.0) Then
                    file.WriteLine()
                    file.WriteLine("  Total Price:".PadRight(20) & FormatAmount(header("vat_sales") + header("non_vat_sales") + header("zero_rates_sales")).PadLeft(28))

                    If header("discount") <> 0.0 Then
                        file.WriteLine("  Less: Regular Discount:".PadRight(30) & FormatAmount(header("discount")).PadLeft(18))
                    End If

                    If header("sc_pwd_discount") <> 0.0 Then
                        file.WriteLine("  Less: SC/PWD Discount:".PadRight(30) & FormatAmount(header("sc_pwd_discount")).PadLeft(18))
                    End If

                    If header("sc_pwd_vat_exempt") <> 0.0 Then
                        file.WriteLine("  Less: VAT Exempt:".PadRight(30) & FormatAmount(header("sc_pwd_vat_exempt")).PadLeft(18))
                    End If
                End If

                file.WriteLine()
                file.WriteLine("  TOTAL AMOUNT DUE:".PadRight(20) & FormatAmount(header("amount_due")).PadLeft(28))

                If header("cash") <> 0.0 Then
                    file.WriteLine("  Cash:".PadRight(20) & FormatAmount(header("cash")).PadLeft(28))
                End If

                If Not isRefund Then
                    'Credit Memo
                    If Not String.IsNullOrWhiteSpace(header("credit_memo_no")) Then
                        file.WriteLine("  Credit Memo:".PadRight(35) & FormatAmount(header("credit_memo")).PadLeft(13))
                    End If
                End If

                'Credit Card
                For Each row As DataRow In creditCards
                    file.WriteLine(("  CC - " & row("card_type") & ":").PadRight(35) & FormatAmount(row("amount")).PadLeft(13))
                    file.WriteLine("    " & row("bank_name") & " - " & row("card_holder"))
                Next

                'Gift Certificate
                For Each row As DataRow In giftCertificates
                    file.WriteLine(("  GC - " & row("code") & ":").PadRight(35) & FormatAmount(row("amount")).PadLeft(13))
                Next

                If Not isRefund Then
                    If header("cash") <> 0.0 Then
                        file.WriteLine("  CHANGE:".PadRight(20) & FormatAmount(header("change")).PadLeft(28))
                    End If

                    file.WriteLine(LINE_SEPARATOR)
                    file.WriteLine("  VATable Sale:".PadRight(20) & FormatAmount(header("vat_sales")).PadLeft(28))
                    file.WriteLine(("  VAT Amount (" & SalesTax.ToString("#0.##") & "%):").PadRight(20) & FormatAmount(header("vat_amount")).PadLeft(28))
                    file.WriteLine("  VAT-Exempt Sale:".PadRight(20) & FormatAmount(header("sc_pwd_vat_exempt_sales")).PadLeft(28))
                    file.WriteLine("  Zero-Rated Sale:".PadRight(20) & FormatAmount(header("zero_rates_sales")).PadLeft(28))
                End If

                If hasScPwdDiscount Then
                    file.WriteLine(LINE_SEPARATOR)
                    Dim index As Integer = 0
                    For Each row As DataRow In scPwdDiscountDetails
                        If row("discount_percent") > 0.0 Then
                            If index > 0 Then
                                file.WriteLine()
                            End If

                            If row("total_sales") > 0.0 Then
                                file.WriteLine("  SC/PWD Sales:".PadRight(20) & FormatAmount(row("total_sales")).PadLeft(28))
                            End If

                            If row("vat_exempt_sales") > 0.0 Then
                                file.WriteLine("  VAT-Exempt Sales:".PadRight(20) & FormatAmount(row("vat_exempt_sales")).PadLeft(28))
                            End If

                            If row("vat_exempt_amount") > 0.0 Then
                                file.WriteLine("  VAT Adjustments:".PadRight(20) & FormatAmount(row("vat_exempt_amount")).PadLeft(28))
                            End If

                            file.WriteLine(("  Discount (" & CDbl(row("discount_percent")).ToString("#0.##") & "%):").PadRight(20) & FormatAmount(row("discount_amount")).PadLeft(28))
                            index += 1
                        End If
                    Next
                End If

                'Void Section
                If isVoid Then
                    file.WriteLine(LINE_SEPARATOR)
                    file.WriteLine("  Reference SI No.:".PadRight(20) & header("transaction_no"))
                    file.WriteLine("  Authorized By:".PadRight(20) & header("authorized_by"))
                    file.WriteLine("  Reason:".PadRight(20) & header("reason_for_void"))

                    'Refund
                ElseIf isRefund Then
                    file.WriteLine(LINE_SEPARATOR)
                    file.WriteLine("  Reference SI No.:".PadRight(20) & header("transaction_no"))
                    file.WriteLine("  Customer Service:".PadRight(20) & header("authorized_by"))
                    file.WriteLine("  Remarks:".PadRight(20) & header("reason_for_void"))
                End If

                If isRePrint AndAlso hasReturns Then
                    file.WriteLine(LINE_SEPARATOR)
                    file.WriteLine("  CUSTOMER RETURNS")
                    Dim totalReturns As Double
                    For Each row As DataRow In customerReturns
                        file.WriteLine(("    " & row("code")).PadRight(35) & FormatAmount(row("total_amount")).PadLeft(13))
                        totalReturns += row("total_amount")
                    Next
                    file.WriteLine("".PadRight(30) & LINE_SEPARATOR.Substring(30))
                    file.WriteLine("  Total Returns:".PadRight(28) & FormatAmount(totalReturns).PadLeft(20))
                    file.WriteLine("  Remaining Amount Due:".PadRight(28) & FormatAmount(header("amount_due") - totalReturns).PadLeft(20))
                End If

                file.Write(LINE_SEPARATOR.Substring(2) & "/*")
            End Using

            SaveEJournalConsolidated(eJournalPath, trnDate)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveEJournalXReading(cashierSessionId As Long, data As DataSet)
        Try
            Dim eJournalPath As String = PrepareJournalDirectory(EJournalTypes.XReadings)
            Dim totals As Double

            Dim header As DataRow = data.Tables(0).Rows(0)
            Dim payments As DataRowCollection = data.Tables(1).Rows
            Dim itemSales As DataRowCollection = data.Tables(2).Rows
            Dim itemDiscounts As DataRowCollection = data.Tables(3).Rows
            Dim voids As DataRowCollection = data.Tables(4).Rows
            Dim summaries As DataRowCollection = data.Tables(5).Rows
            Dim denominations As DataRowCollection = data.Tables(6).Rows

            Dim reportDate As Date = Convert.ToDateTime(String.Concat(header("logout_date"), " ", header("logout_time")))
            Dim fileName As String = reportDate.ToString("yyyy-MM-dd") & " X-Reading-" & cashierSessionId.ToString("0000") & ".txt"

            eJournalPath = Path.Combine(eJournalPath, fileName)

            Using file As New StreamWriter(eJournalPath, False)
                file.WriteLine("/*" & LINE_SEPARATOR.Substring(2))
                file.WriteLine(CenterText("X-READING REPORT"))

                If m_Settings.TrainingMode Then
                    file.WriteLine(CenterText("- TRAINING MODE -"))
                End If

                file.WriteLine(LINE_SEPARATOR)

                If m_Settings.TrainingMode Then
                    file.WriteLine(CenterText("- TRAINING MODE -"))
                    file.WriteLine()
                End If
                file.WriteLine("  Cashier:".PadRight(12) & header("cashier_name"))
                file.WriteLine("  Log In:".PadRight(12) & header("login_date").ToString.PadRight(22) & "Time:" & header("login_time").ToString.PadLeft(9))
                file.WriteLine("  Log Out:".PadRight(12) & header("logout_date").ToString.PadRight(22) & "Time:" & header("logout_time").ToString.PadLeft(9))
                file.WriteLine(LINE_SEPARATOR)

                totals = 0
                For Each payment As DataRow In payments
                    file.WriteLine(CStr("  " & payment("payment_type")).PadRight(20) & FormatCount(payment("payment_count")).PadLeft(8) & FormatAmount(payment("payment_sales_amount")).PadLeft(20))
                    totals += CDbl(payment("payment_sales_amount"))
                Next
                If payments.Count > 0 Then
                    file.WriteLine(" TOTAL".PadLeft(14) & FormatAmount(totals).PadLeft(34))
                End If

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("SALES"))
                file.WriteLine(LINE_SEPARATOR)

                totals = 0
                For Each item As DataRow In itemSales
                    file.WriteLine(CStr("  " & item("product_type")).PadRight(20) & FormatCount(item("product_count")).PadLeft(8) & FormatAmount(item("product_sales_amount")).PadLeft(20))
                    totals += CDec(item("product_sales_amount"))
                Next
                If itemSales.Count > 0 Then
                    file.WriteLine("TOTAL".PadLeft(14) & FormatAmount(totals).PadLeft(34))
                End If

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("DISCOUNTS"))
                file.WriteLine(LINE_SEPARATOR)

                totals = 0
                For Each item As DataRow In itemDiscounts
                    file.WriteLine(CStr("  " & item("product_type")).PadRight(20) & FormatCount(item("product_count")).PadLeft(8) & FormatAmount(item("product_discount_amount")).PadLeft(20))
                    totals += CDec(item("product_discount_amount"))
                Next
                If itemSales.Count > 0 Then
                    file.WriteLine("TOTAL".PadLeft(14) & FormatAmount(totals).PadLeft(34))
                End If

                With file
                    .WriteLine(LINE_SEPARATOR)
                    .WriteLine(CenterText("RETURNS/VOIDS"))
                    .WriteLine(LINE_SEPARATOR)

                    For Each void As DataRow In voids
                        .WriteLine(CStr("  " & void("product_type")).PadRight(20) & FormatCount(void("product_count")).PadLeft(8) & FormatAmount(void("total_amount")).PadLeft(20))
                    Next
                End With

                With file
                    .WriteLine(LINE_SEPARATOR)
                    .WriteLine(CenterText("DENOMINATIONS"))
                    .WriteLine(LINE_SEPARATOR)

                    totals = 0
                    .WriteLine("Denomination".PadLeft(14) & "Pieces".PadLeft(14) & "Amount".PadLeft(20))
                    For Each denomination As DataRow In denominations
                        If CDbl(denomination("amount")) > 0.0 Then
                            .WriteLine(FormatAmount(denomination("denomination")).PadLeft(14) & FormatAmount(denomination("pieces")).PadLeft(14) & FormatAmount(denomination("amount")).PadLeft(20))
                            totals += CDbl(denomination("amount"))
                        End If
                    Next
                    .WriteLine("TOTAL".PadLeft(14) & FormatAmount(totals).PadLeft(34))
                End With

                file.WriteLine(LINE_SEPARATOR)

                For Each summary As DataRow In summaries
                    file.WriteLine("  Beginning SI No.".PadRight(25) & summary("beginning_si").ToString.PadLeft(23))
                    file.WriteLine("  Ending SI No.".PadRight(25) & summary("ending_si").ToString.PadLeft(23))
                    file.WriteLine()
                    file.WriteLine("  Beginning Balance".PadRight(25) & FormatAmount(summary("beginning_balance")).PadLeft(23))
                    file.WriteLine("  Ending Balance".PadRight(25) & FormatAmount(summary("ending_balance")).PadLeft(23))
                    file.WriteLine("  Deficit/Excess".PadRight(25) & FormatAmount(summary("deficit_excess_amount")).PadLeft(23))
                Next

                file.Write(LINE_SEPARATOR.Substring(2) & "*/")
            End Using

            SaveEJournalConsolidated(eJournalPath, reportDate)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#End Region

End Class
