Imports System.IO
Imports System.Text.RegularExpressions
Imports CrystalDecisions.CrystalReports.Engine

Public Class Reporting
    Inherits DataSource

#Region "Declarations"

    Public Const LINE_WIDTH As Integer = 50
    Public Const LINE_SEPARATOR As String = "--------------------------------------------------"

    Private Const MESSAGE_NO_DATA_CAPTION As String = "No Data"
    Private Const MESSAGE_NO_DATA As String = "Report contains no data for printing."
    Private Const DEFAULT_FOOTER As String = "DEFAULT"
    Private Const DEFAULT_FOOTER1 As String = "THIS SERVES AS YOUR OFFICIAL RECEIPT!"
    Private Const DEFAULT_FOOTER2 As String = "THANK YOU. PLEASE COME AGAIN!"

    Private WithEvents m_ReportViewer As FormReportViewer

    Public Enum EJournalTypes
        [Default]
        ZReadings
    End Enum

#End Region

#Region "Properties"

    Property ApplicationCaption As String = String.Empty
    Property HasRefundAccess As Boolean
    Property HasOwnReportViewer As Boolean
    Property ModuleName As String

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

    Private Function ShowReport(ByVal report As Object, ByVal data As Object, ByVal reportTitle As String, Optional isCustomerReturn As Boolean = False) As Object
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
                If Not HasOwnReportViewer Then
                    Application.UseWaitCursor = False
                    ShowMessage(MESSAGE_NO_DATA, MESSAGE_NO_DATA_CAPTION, MessageIcon.Exclamation)
                End If
            End If

            With report
                If .Filename = "" Then
                    .SetDataSource(data)
                End If

                .SetParameterValue("company_name", Current.CompanyInfo.Name)
                .SetParameterValue("company_address", Regex.Replace(Current.CompanyInfo.Address.Replace(",,", ","), "\t|\n|\r", " "))
                .SetParameterValue("company_phone", Current.CompanyInfo.ContactNo)
                .SetParameterValue("report_title", reportTitle.ToUpper) ' ALL CAPS REPORT TITLE

                If isCustomerReturn Then
                    .SetParameterValue("company_name_2", Current.CompanyInfo.Name2 & " ")
                    .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo & " ")
                    .SetParameterValue("has_refund", HasRefundAccess)
                End If
            End With

            If Not HasOwnReportViewer Then
                m_ReportViewer = New FormReportViewer(report, reportTitle) With {.ModuleName = Me.ModuleName}
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
            Dim dirBPro As String = "BusinessPro"
            Dim dirPOS As String = My.Application.Info.Description.Replace(dirBPro, "").Trim
            Dim pathAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

            Dim eJournalPath As String = Path.Combine(
                pathAppData, My.Application.Info.CompanyName & "\" & dirBPro & "\" & dirPOS & "\")

            If Not Directory.Exists(eJournalPath) Then
                Directory.CreateDirectory(eJournalPath)
            End If

            Const ZREADINGS As String = "ZReadings\"

            Select Case eJournalType
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

    Private Sub SaveZReadingToTextFile(data As DataSet, reportDate As Date, isFinalized As Boolean, isReprint As Boolean)
        Try
            Dim eJournalPath As String = PrepareJournalDirectory(EJournalTypes.ZReadings)
            Dim fileName As String = reportDate.ToString("yyyy-MM-dd") & " Z-Reading.txt"

            eJournalPath = Path.Combine(eJournalPath, fileName)

            Dim header As DataRow = data.Tables(0).Rows(0)
            Dim items As DataRowCollection = data.Tables(1).Rows
            Dim discount As DataRow = data.Tables(2).Rows(0)
            Dim vat As DataRow = data.Tables(3).Rows(0)
            Dim payments As DataRowCollection = data.Tables(4).Rows
            Dim creditCards As DataRowCollection = data.Tables(5).Rows
            Dim cashierSales As DataRowCollection = data.Tables(6).Rows
            Dim cashierDiscounts As DataRowCollection = data.Tables(7).Rows
            Dim cumulativeGrandTotals As DataRow = data.Tables(8).Rows(0)
            Dim voidsRefunds As DataRowCollection = data.Tables(9).Rows
            Dim totals As Double = 0

            Using file As New StreamWriter(eJournalPath, False)
                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("Z-READING REPORT"))
                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(("  Date: " & reportDate.ToString("MMMM dd, yyyy")).PadRight(25) & ("Time: " & reportDate.ToShortTimeString).PadLeft(23))
                file.WriteLine("  Z-Counter No.: " & CInt(header("z_reading_id")).ToString("00000"))
                file.WriteLine("  Reset Counter: " & CDbl(header("reset_counter")).ToString("000000000000000"))
                file.WriteLine(LINE_SEPARATOR)

                file.WriteLine("  Beginning SI No.".PadRight(25) & header("beginning_si").ToString.PadLeft(23))
                file.WriteLine("  Ending SI No.".PadRight(25) & header("ending_si").ToString.PadLeft(23))
                file.WriteLine("  Cash Beginning Balance".PadRight(25) & FormatAmount(header("beginning_balance")).PadLeft(23))
                file.WriteLine("  Cash Ending Balance".PadRight(25) & FormatAmount(header("ending_balance")).PadLeft(23))

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("SALES SUMMARY"))
                file.WriteLine(LINE_SEPARATOR)

                totals = 0
                For Each item As DataRow In items
                    totals += CDbl(item("product_amount"))
                Next
                totals += discount("sc_vat_amount")
                totals += discount("pwd_vat_amount")
                file.WriteLine("  GROSS SALES".PadRight(30) & FormatAmount(totals).PadLeft(18))
                file.WriteLine(String.Empty)

                file.WriteLine("  Regular Discount".PadRight(25) & FormatAmount(discount("reg_discount_amount")).PadLeft(23))
                file.WriteLine("  SC Discount".PadRight(25) & FormatAmount(discount("sc_discount_amount")).PadLeft(23))
                file.WriteLine("  SC Less VAT".PadRight(25) & FormatAmount(discount("sc_vat_amount")).PadLeft(23))
                file.WriteLine("  PWD Discount".PadRight(25) & FormatAmount(discount("pwd_discount_amount")).PadLeft(23))
                file.WriteLine("  PWD Less VAT".PadRight(25) & FormatAmount(discount("pwd_vat_amount")).PadLeft(23))
                file.WriteLine("  NET SALES".PadRight(25) & FormatAmount(discount("sales_net_discount")).PadLeft(23))
                file.WriteLine()

                file.WriteLine("  VATable Sales".PadRight(25) & FormatAmount(vat("vat_sales_amount")).PadLeft(23))
                file.WriteLine("  VAT Amount".PadRight(25) & FormatAmount(vat("vat_amount")).PadLeft(23))
                file.WriteLine("  VAT-Exempt Sales".PadRight(25) & FormatAmount(vat("vat_exempted_sales_amount")).PadLeft(23))
                file.WriteLine("  Zero-Rated Sales".PadRight(25) & FormatAmount(vat("zero_rated_sales_amount")).PadLeft(23))

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("PAYMENT DETAILS"))
                file.WriteLine(LINE_SEPARATOR)

                totals = 0
                For Each payment As DataRow In payments
                    file.WriteLine(CStr("  " & payment("payment_type")).PadRight(20) & FormatCount(payment("payment_count")).PadLeft(8) & FormatAmount(payment("payment_amount")).PadLeft(20))
                    totals += payment("payment_amount")
                Next
                If payments.Count > 0 Then
                    file.WriteLine("  TOTAL PAYMENTS".PadRight(20) & FormatAmount(totals).PadLeft(28))
                End If

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("CREDIT CARD BREAKDOWN"))
                file.WriteLine(LINE_SEPARATOR)

                totals = 0
                For Each card As DataRow In creditCards
                    file.WriteLine(CStr("  " & card("cc_type")).PadRight(20) & FormatCount(card("cc_count")).PadLeft(8) & FormatAmount(card("cc_amount")).PadLeft(20))
                    totals += card("cc_amount")
                Next
                If creditCards.Count > 0 Then
                    file.WriteLine("  TOTAL AMOUNT".PadRight(20) & FormatAmount(totals).PadLeft(28))
                End If

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("CASHIER SALES TOTAL"))
                file.WriteLine(LINE_SEPARATOR)

                For Each sale As DataRow In cashierSales
                    file.WriteLine(CStr("  " & sale("cashier_name")).PadRight(28) & FormatAmount(sale("cashier_total_sales_amount")).PadLeft(20))
                Next

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("CASHIER DISCOUNT TOTALS"))
                file.WriteLine(LINE_SEPARATOR)

                For Each discount In cashierDiscounts
                    file.WriteLine(CStr("  " & discount("cashier_name")).PadRight(28) & FormatAmount(discount("cashier_total_discount_amount")).PadLeft(20))
                Next

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("ACCUMULATIVE ADJUSTMENTS"))
                file.WriteLine(LINE_SEPARATOR)

                For Each vr As DataRow In voidsRefunds
                    If IsNumeric(vr("value")) Then
                        file.WriteLine(CStr("  " & vr("description")).PadRight(28) & FormatAmount(vr("value")).PadLeft(20))
                    Else
                        file.WriteLine(CStr("  " & vr("description")).PadRight(28) & vr("value").ToString.PadLeft(20))
                    End If
                Next

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText("CUMULATIVE GRAND TOTAL"))
                file.WriteLine(LINE_SEPARATOR)

                file.WriteLine("  Previous Grand Total".PadRight(28) & CDbl(cumulativeGrandTotals("prev_grand_total")).ToString("0,000,000,000,000.00").PadLeft(20))
                file.WriteLine("  Current Sales Total".PadRight(28) & CDbl(cumulativeGrandTotals("current_sales_total")).ToString("0,000,000,000,000.00").PadLeft(20))
                file.WriteLine("  Cumulative Grand Total".PadRight(28) & CDbl(cumulativeGrandTotals("cumulative_grand_total")).ToString("0,000,000,000,000.00").PadLeft(20))

                file.WriteLine(LINE_SEPARATOR)
                file.WriteLine(CenterText(If(isFinalized AndAlso Not isReprint, "Finalized", "Printed") & " by: " & Current.User.Name))
                file.WriteLine(CenterText(Now.ToString))
                file.Write(LINE_SEPARATOR.Substring(2) & "*/")
            End Using

            SaveEJournalConsolidated(eJournalPath, reportDate)
        Catch ex As Exception
            Throw
        End Try
    End Sub

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

    Shared Function FormatAmount(value As Double) As String
        Return value.ToString("#,##0.00")
    End Function

    Shared Function FormatCount(value As Integer) As String
        Return value.ToString("#,##0")
    End Function

    Shared Function CenterText(text As String) As String
        Return text.PadLeft((LINE_WIDTH / 2) + (text.Length / 2))
    End Function

#End Region

#Region "Public"

    Public Sub ShowAuditTrail(data As DataTable, startDate As Date, endDate As Date, userFilter As String, terminalFilter As String)
        Try
#If DEBUG Then
            data.WriteXmlSchema("AuditTrail.xml")
#End If

            Dim report As New AuditTrailReport

            With report
                .SetDataSource(data)
                .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)
                .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)

                If startDate.Date = endDate.Date Then
                    .SetParameterValue("date_param", startDate.ToDateString)
                Else
                    .SetParameterValue("date_param", startDate.ToDateString & " - " & endDate.ToDateString)
                End If
            End With

            ShowReport(report, data, "Audit Trail Report")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ShowCustomerReturn(ByVal data As DataSet)
        Try
#If DEBUG Then
            data.WriteXmlSchema("CustomerReturn.xml")
#End If
            ShowReport(New CustomerReturnReport, data, "Customer Return", True)
        Catch
            Throw
        End Try
    End Sub

    Public Function GetDailySales(ByVal data As DataTable, reportDate As Date, Optional isNewDailySales As Boolean = False) As Object
        Try
#If DEBUG Then
            data.WriteXmlSchema("DailySales.xml")
#End If

            Dim rpt As ReportClass
            Dim hasCreditMemo As Boolean = (Current.Rights.IsAllowed("Customer Return", "Sales") OrElse Current.Rights.IsAllowed("Credit Memo", "POS"))
            Dim hasGiftCert As Boolean = (Current.Rights.IsAllowed("Gift Certificates", "Libraries"))

            If isNewDailySales AndAlso hasCreditMemo Then
                rpt = New DailySalesWithCMGCQR
            ElseIf hasCreditMemo AndAlso hasGiftCert Then
                rpt = New DailySalesWithCMGC
            ElseIf hasCreditMemo Then
                rpt = New DailySalesWithCM
            ElseIf hasGiftCert Then
                rpt = New DailySalesWithGC
            Else
                rpt = New DailySales
            End If

            With rpt
                .SetDataSource(data)

                .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)
                .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                .SetParameterValue("generated_datetime", "Date Printed: " & Now.ToString)
                .SetParameterValue("generated_by", "Prepared By: " & Current.User.Name)
                .SetParameterValue("application_caption", ApplicationCaption)
                .SetParameterValue("report_date", reportDate)
            End With

            Return ShowReport(rpt, data, "Daily Sales Report")
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetSalesSummary(ByVal data As DataTable) As Object
        Try
#If DEBUG Then
            data.WriteXmlSchema("SalesSummary.xml")
#End If

            Dim rpt As ReportClass

            If Current.Rights.IsAllowed("Customer Return", "Sales") OrElse Current.Rights.IsAllowed("Credit Memo", "POS") Then
                rpt = New SalesSummaryWithCM
            Else
                rpt = New SalesSummary
            End If

            With rpt
                .SetDataSource(data)

                .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)
                .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                .SetParameterValue("generated_datetime", "Date Printed: " & Now.ToString)
                .SetParameterValue("generated_by", "Prepared By: " & Current.User.Name)
                .SetParameterValue("application_caption", ApplicationCaption)
            End With

            Return ShowReport(rpt, data, "Sales Summary Report")
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ShowZReading(data As DataSet, isFinalized As Boolean, isReprint As Boolean)
        Try
#If DEBUG Then
            data.WriteXmlSchema("ZReading.xml")
#End If

            Dim reportDate As Date = Now
            Dim report As New ZReadingReport

            With report
                Try
                    .SetDataSource(data)
                    With report
                        .SetParameterValue("company_name", Current.CompanyInfo.Name)
                        .SetParameterValue("company_name_2", Current.CompanyInfo.Name2)
                        .SetParameterValue("company_address", Current.CompanyInfo.Address)
                        .SetParameterValue("company_phone", Current.CompanyInfo.ContactNo & If(String.IsNullOrWhiteSpace(Current.CompanyInfo.ContactNo), " ", ""))
                        .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                        .SetParameterValue("company_ptu_no", Current.CompanyInfo.PermitNo)

                        .SetParameterValue("footer1", If(isFinalized AndAlso Not isReprint, "Finalized", "Printed") & " by: " & Current.User.Name)
                        .SetParameterValue("footer2", Now.ToString)
                    End With
                Catch ex As Exception
                    ' ignore
                Finally
                    Try
                        SaveZReadingToTextFile(data, reportDate, isFinalized, isReprint)
                    Catch ex As Exception
                        'ignore
                    End Try
                End Try
            End With

            Return report
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function PrintScReport(dt As DataTable, dateFrom As Date, dateTo As Date) As Object
        Try
#If DEBUG Then
            dt.WriteXmlSchema("SCReport.xml")
#End If

            Dim dateCoverage As String = DateRangeToString(dateFrom, dateTo)
            Dim rptSC As New ScReport
            With rptSC
                .SetDataSource(dt)
                .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)
                .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                .SetParameterValue("date_coverage", dateCoverage)
            End With

            Return ShowReport(rptSC, dt, "Senior Citizen (SC) Sales")
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function PrintPwdReport(dt As DataTable, dateFrom As Date, dateTo As Date) As Object
        Try
#If DEBUG Then
            dt.WriteXmlSchema("PWDReport.xml")
#End If

            Dim dateCoverage As String = DateRangeToString(dateFrom, dateTo)
            Dim rptPWD As New PwdReport
            With rptPWD
                .SetDataSource(dt)
                .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)
                .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                .SetParameterValue("date_coverage", dateCoverage)
            End With

            Return ShowReport(rptPWD, dt, "Persons with Disabilities (PWD) Sales")
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function PrintCcPaymentsSummaryReport(dt As DataTable, dateFrom As Date, dateTo As Date) As Object
        Try
#If DEBUG Then
            dt.WriteXmlSchema("CCPaymentsSummary.xml")
#End If

            Dim dateCoverage As String = DateRangeToString(dateFrom, dateTo)
            Dim rptSC As New CCPaymentsSummary
            With rptSC
                .SetDataSource(dt)
                .SetParameterValue("business_style", Current.CompanyInfo.BusinessStyle)
                .SetParameterValue("company_tin", Current.CompanyInfo.VatRegistrationNo)
                .SetParameterValue("date_coverage", dateCoverage)
            End With

            Return ShowReport(rptSC, dt, "Credit Card Payments Summary")
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function DateRangeToString(dateFrom As Date, dateTo As Date) As String
        If dateFrom.Date = dateTo.Date Then
            Return dateFrom.ToString("MMMM dd, yyyy")
        ElseIf dateFrom.Year = dateTo.Year AndAlso dateFrom.Month = dateTo.Month Then
            Return MonthName(dateFrom.Month) & " " & dateFrom.Day & "-" & dateTo.Day & ", " & dateTo.Year
        Else
            Return dateFrom.ToString("MM/dd/yyyy") & " - " & dateTo.ToString("MM/dd/yyyy")
        End If
    End Function

#End Region

#End Region

End Class
