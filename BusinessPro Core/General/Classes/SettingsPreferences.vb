Public Class SettingsPreferences
    Inherits DataSource

#Region "Constants"

    Public Const FLAG_AUTH_CASH_UP As String = "AUTH_CASH_UP"
    Public Const FLAG_AUTH_OFFICER As String = "AUTH_OFFICER"
    Public Const FLAG_AUTH_LOAD_POSTED_SALE As String = "AUTH_LOAD_POSTED_SALE"
    Public Const FLAG_AUTH_REPRINT_RECEIPT As String = "AUTH_REPRINT_RECEIPT"
    Public Const FLAG_BANNER_DEFAULT As String = "BANNER_DEFAULT"
    Public Const FLAG_BANNER_IMAGE As String = "BANNER_IMAGE"
    Public Const FLAG_BANNER_STYLE As String = "BANNER_STYLE"
    Public Const FLAG_BUSINESS_STYLE As String = "BUSINESS_STYLE"
    Public Const FLAG_DATE_FORMAT As String = "DATE_FORMAT"
    Public Const FLAG_DISCOUNT_GROUP_PWD As String = "DISCOUNT_GROUP_PWD"
    Public Const FLAG_DISCOUNT_GROUP_SC As String = "DISCOUNT_GROUP_SC"
    Public Const FLAG_INV_END As String = "INV_END"
    Public Const FLAG_INV_START As String = "INV_START"
    Public Const FLAG_SALES_TAX As String = "SALES_TAX"
    Public Const FLAG_AUTH_QUANTITY As String = "RESTRICT_QUANTITY"
    Public Const FLAG_AUTH_DELETION As String = "RESTRICT_DELETE"
    Public Const FLAG_AUTH_CANCEL As String = "RESTRICT_CANCEL"
    Public Const FLAG_ITEM_CODE_INVOICE As String = "ITEM_CODE_INVOICE"
    Public Const FLAG_NAV_ITEM_EXTEXT As String = "NAV_ITEM_EXTEXT"
    Public Const FLAG_NAV_USE_A4INVOICE As String = "NAV_USE_A4INVOICE"
    Public Const FLAG_USE_BARCODE_LOGIN As String = "USE_BARCODE_LOGIN"
    Public Const FLAG_TRAINING_MODE As String = "TRAINING_MODE"
    Public Const FLAG_FULLSCREEN As String = "FULLSCREEN"
    Public Const FLAG_RECEIPT_TIN As String = "RECEIPT_TIN"
    Public Const FLAG_RESET_THRESHOLD As String = "RESET_THRESHOLD"

    Public Const FLAG_ACU_INTEGRATION As String = "ACU_INTEGRATION"
    Public Const FLAG_ACU_DEFAULT_ATC As String = "ACU_ATC"

    Public Const FLAG_SHOW_SALESPERSON As String = "INTEG_SHOW_SALESPERSON"
    Public Const FLAG_REQUIRE_SALESPERSON As String = "INTEG_REQUIRE_SALESPERSON"
    Public Const FLAG_AUTO_FINALIZE_Z As String = "AUTO_FINALIZE_Z"

#End Region

#Region "Properties"

    Property Data As DataTable

    Property AuthorizedOfficerCode As String
    Property AuthorizeSaleQuantityReduction As Boolean
    Property AuthorizeSaleItemDeletion As Boolean
    Property AuthorizeSaleCancellation As Boolean
    Property AuthorizeLoadPostedSale As Boolean
    Property AuthorizeReprintReceipt As Boolean
    Property AuthorizeCashUp As Boolean
    Property ResetThreshold As Double
    Property InvoiceNoStart As Long
    Property InvoiceNoEnd As Long
    Property SalesTax As Double
    Property ShowItemCodeOnInvoice As Boolean
    Property ShowItemExtendedText As Boolean
    Property UsePrePrintedInvoice As Boolean
    Property UseBarcodeLogin As Boolean
    Property TrainingMode As Boolean
    Property DisplayBannerDefault As Boolean
    Property DisplayBannerImage As Image
    Property DisplayBannerStyle As PictureBoxSizeMode
    Property FullscreenCashier As Boolean
    Property ServerDateFormat As String

    Property AcuIntegration As Boolean
    Property AcuDefaultATC As String

    Property ShowSalesperson As Boolean
    Property RequireSalesperson As Boolean
    Property AutoFinalizeZReading As Boolean

#End Region

#Region "Methods"

    Public Sub Fill()
        Try
            Dim sql As String =
                "EXEC dbo.GetSettingsPreferences"

            Data = ExecuteQuery(sql).Tables(0)

            For Each row As DataRow In Data.Rows
                Select Case row("flag").ToString
                    Case FLAG_AUTH_OFFICER
                        AuthorizedOfficerCode = row("value")
                    Case FLAG_AUTH_QUANTITY
                        AuthorizeSaleQuantityReduction = CBool(row("value"))
                    Case FLAG_AUTH_DELETION
                        AuthorizeSaleItemDeletion = CBool(row("value"))
                    Case FLAG_AUTH_CANCEL
                        AuthorizeSaleCancellation = CBool(row("value"))
                    Case FLAG_AUTH_LOAD_POSTED_SALE
                        AuthorizeLoadPostedSale = CBool(row("value"))
                    Case FLAG_AUTH_REPRINT_RECEIPT
                        AuthorizeReprintReceipt = CBool(row("value"))
                    Case FLAG_AUTH_CASH_UP
                        AuthorizeCashUp = CBool(row("value"))
                    Case FLAG_RESET_THRESHOLD
                        ResetThreshold = CDbl(row("value"))
                    Case FLAG_INV_END
                        InvoiceNoEnd = CLng(row("value"))
                    Case FLAG_INV_START
                        InvoiceNoStart = CLng(row("value"))
                    Case FLAG_SALES_TAX
                        SalesTax = CDbl(row("value"))
                    Case FLAG_ITEM_CODE_INVOICE
                        ShowItemCodeOnInvoice = CBool(row("value"))
                    Case FLAG_NAV_ITEM_EXTEXT
                        ShowItemExtendedText = CBool(row("value"))
                    Case FLAG_NAV_USE_A4INVOICE
                        UsePrePrintedInvoice = CBool(row("value"))
                    Case FLAG_USE_BARCODE_LOGIN
                        UseBarcodeLogin = CBool(row("value"))
                    Case FLAG_TRAINING_MODE
                        TrainingMode = CBool(row("value"))
                    Case FLAG_BANNER_DEFAULT
                        DisplayBannerDefault = CBool(row("value"))
                    Case FLAG_BANNER_IMAGE
                        DisplayBannerImage = If(row("value") = "NONE", Nothing, row("value").ToString.ToImage)
                    Case FLAG_BANNER_STYLE
                        DisplayBannerStyle = CInt(row("value"))
                    Case FLAG_FULLSCREEN
                        FullscreenCashier = CBool(row("value"))
                    Case FLAG_DATE_FORMAT
                        ServerDateFormat = row("value")

                    Case FLAG_ACU_INTEGRATION
                        AcuIntegration = CBool(row("value"))
                    Case FLAG_ACU_DEFAULT_ATC
                        AcuDefaultATC = row("value")

                    Case FLAG_SHOW_SALESPERSON
                        ShowSalesperson = CBool(row("value"))
                    Case FLAG_REQUIRE_SALESPERSON
                        RequireSalesperson = CBool(row("value"))
                    Case FLAG_AUTO_FINALIZE_Z
                        AutoFinalizeZReading = CBool(row("value"))
                End Select
            Next

            Data.AcceptChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function Save(Optional imageText As String = "") As Boolean
        Try
            Dim sql As String = ""
            Dim value As String

            For Each row As DataRow In Data.GetChanges(DataRowState.Modified).Rows
                value = CStr(row("value")).Trim
                If row("flag") = FLAG_BANNER_IMAGE AndAlso value <> "NONE" Then
                    value = imageText
                End If
                sql &=
                    "UPDATE " & vbCrLf &
                    "  [settings_preference]" & vbCrLf &
                    "SET " & vbCrLf &
                    "  [value] = " & value.Quote(False) & vbCrLf &
                    "WHERE flag = '" & row("flag") & "';" & vbCrLf
            Next

            Dim result As Boolean
            If sql <> "" Then
                result = ExecuteNonQuery(sql)
            End If

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
