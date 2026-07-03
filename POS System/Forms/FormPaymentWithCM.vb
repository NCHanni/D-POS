Imports Infragistics.Win.UltraWinEditors

Friend Class FormPaymentWithCM

#Region "Constructors"

    Public Sub New(settings As SettingsPreferences)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings

        Core.Current.FormatUI.Apply(Me)
    End Sub

#End Region

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences

    Private m_Initialized As Boolean
    Private m_TotalSales As Double
    Private m_IsCreditMemo As Boolean
    Private m_ExcessGCAmount As Double

#End Region

#Region "Properties"

    Property CustomerName As String
    Property CustomerTIN As String
    Property CustomerBusinessStyle As String
    Property CustomerAddress As String
    Property IsCustomerWalkin As Boolean
    Property IsCustomerScPwd As Boolean
    Property IsGiftCertEnabled As Boolean = True

    Property VATableSales As Double
    Property VATAmount As Double
    Property NonVATSales As Double
    Property ZeroRatedSales As Double
    Property RegularDiscount As Double
    Property RegularLessVAT As Double
    Property SCPwdDiscount As Double
    Property SCPwdVatExempt As Double

    Property CashData As DataTable
    Property ChecksData As DataTable
    Property CreditCardData As DataTable
    Property GiftCertificateData As DataTable
    Property CreditMemoData As DataTable

    Property SalespersonCode As String
    Property SalespersonName As String

    ReadOnly Property ExcessGCAmount As Double
        Get
            Return m_ExcessGCAmount
        End Get
    End Property

#End Region

#Region "Event Handlers"

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

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.F5
                    numCashAmount.Focus()

                Case Keys.F6
                    Using frm As New FormPaymentCreditCards(m_Settings)
                        With frm
                            .CreditCards = CreditCardData.Copy
                            .RemainingAmountDue = m_TotalSales - CDbl(lblAmountTenderedValue.Text) + numCreditCardAmount.Value
                            If LaunchForm(frm, True, Me) = DialogResult.OK Then
                                CreditCardData = .CreditCards.Copy
                                numCreditCardAmount.Value = .TotalAmount
                            End If
                        End With
                    End Using

                Case Keys.F7
                    If numGiftCertAmount.Visible Then
                        Using frm As New FormPaymentGiftCertificates(m_Settings)
                            With frm
                                .GiftCertificates = GiftCertificateData.Copy
                                .RemainingAmountDue = m_TotalSales - CDbl(lblAmountTenderedValue.Text) + numGiftCertAmount.Value
                                If LaunchForm(frm, True, Me) = DialogResult.OK Then
                                    GiftCertificateData = .GiftCertificates
                                    m_ExcessGCAmount = .ExcessAmount
                                    numGiftCertAmount.Value = .TotalAmount
                                End If
                            End With
                        End Using
                    End If

                Case Keys.F8
                    numCreditMemoAmount_EditorButtonClick(numCreditMemoAmount, New EditorButtonEventArgs(Nothing, Nothing))

                Case Keys.F9
                    If txtCustomerName.Focused Then
                        With txtCustomerTIN
                            .Focus()
                            .SelectAll()
                        End With
                    ElseIf txtCustomerTIN.Focused Then
                        With txtCustomerBusinessStyle
                            .Focus()
                            .SelectAll()
                        End With
                    ElseIf txtCustomerBusinessStyle.Focused Then
                        With txtCustomerAddress
                            .Focus()
                            .SelectAll()
                        End With
                    Else
                        With txtCustomerName
                            .Focus()
                            .SelectAll()
                        End With
                    End If
            End Select
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.ActiveControl = numCashAmount
    End Sub

#End Region

#Region "Button"

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try
            If IsValid() Then
                CustomerName = txtCustomerName.Text.Trim
                CustomerTIN = txtCustomerTIN.Text.Trim
                CustomerBusinessStyle = txtCustomerBusinessStyle.Text.Trim
                CustomerAddress = txtCustomerAddress.Text.Trim
            Else
                Return
            End If

            Dim totalChange As Double = CDbl(lblAmountTenderedValue.Text) - CDbl(lblAmountDueValue.Text)
            Dim row As DataRow

            CashData.Rows.Clear()

            If numCashAmount.Value > 0 Then
                row = CashData.NewRow
                row("id") = -1
                row("sale_code") = ""
                row("amount") = numCashAmount.Value
                row("change") = totalChange
                CashData.Rows.Add(row)
            End If

            If String.IsNullOrWhiteSpace(SalespersonCode) Then
                SalespersonName = ""
            End If

            DialogResult = DialogResult.OK
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

#End Region

#Region "Textbox"

    Private Sub numReceivable_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
            numCashAmount.ValueChanged,
            numCreditCardAmount.ValueChanged,
            numGiftCertAmount.ValueChanged
        Try
            If m_Initialized Then
                ComputeTotal()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub numCreditMemoAmount_EditorButtonClick(sender As System.Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles numCreditMemoAmount.EditorButtonClick
        Try
            Using frm As New FormCreditMemoBrowser(m_Settings, FormCreditMemoBrowser.OperationType.View)
                frm.Data = CreditMemoData
                LaunchForm(frm, True, Me)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            txtCustomerName.Text = CustomerName
            txtCustomerTIN.Text = CustomerTIN
            txtCustomerBusinessStyle.Text = CustomerBusinessStyle
            txtCustomerAddress.Text = CustomerAddress

            numVatableSales.Value = VATableSales
            numVatAmount.Value = VATAmount
            numNonVatSales.Value = NonVATSales
            numZeroRatedSales.Value = ZeroRatedSales
            numRegularDiscount.Value = RegularDiscount
            numScPwdDiscount.Value = SCPwdDiscount
            numVatExemptAmount.Value = SCPwdVatExempt

            If RegularLessVAT > 0.0 AndAlso Not IsCustomerScPwd Then
                numVatExemptAmount.Value = RegularLessVAT
            End If

            If Not IsGiftCertEnabled Then
                lblGiftCertAmount.Visible = False
                numGiftCertAmount.Visible = False
            End If

            CashData = New DataTable
            With CashData.Columns
                .Add("id", GetType(Long))
                .Add("sale_code")
                .Add("amount", GetType(Double))
                .Add("change", GetType(Double))
                .Add("payment_code").DefaultValue = ""
            End With

            CreditCardData = New DataTable
            With CreditCardData.Columns
                .Add("id", GetType(Long))
                .Add("sale_code")
                .Add("credit_card_code")
                .Add("description")
                .Add("card_number")
                .Add("card_holder")
                .Add("bank_name")
                .Add("card_approval_code")
                .Add("amount")
                .Add("payment_code").DefaultValue = ""
            End With

            If Not String.IsNullOrEmpty(SalespersonCode) Then
                lblSalespersonCaption.Visible = True
                lblSalesperson.Text = SalespersonName
                lblSalesperson.Visible = True
            End If

            m_IsCreditMemo = CreditMemoData.Rows.Count > 0

            If m_IsCreditMemo Then
                numCreditMemoAmount.Value = CreditMemoData.Compute("SUM(Amount)", "")
            Else
                numCreditMemoAmount.Enabled = False
            End If

            m_Initialized = True

            ComputeTotal()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ComputeTotal()
        Try
            Dim cashAmount As Double = CDblEx(numCashAmount.Value)
            Dim creditCardAmount As Double = CDblEx(numCreditCardAmount.Value)
            Dim giftCertAmount As Double = CDblEx(numGiftCertAmount.Value)
            Dim creditMemoAmount As Double = CDblEx(numCreditMemoAmount.Value)
            Dim amountTendered As Double = 0.0
            Dim totalChange As Double = 0.0

            numTotalSales.Value =
                numVatableSales.Value +
                numVatAmount.Value +
                numNonVatSales.Value +
                numZeroRatedSales.Value

            m_TotalSales = CDblEx(numTotalSales.Value)
            m_TotalSales = Math.Round(m_TotalSales, 2)

            amountTendered =
                cashAmount +
                creditCardAmount +
                giftCertAmount +
                creditMemoAmount

            amountTendered = Math.Round(amountTendered, 2)

            lblChangeValue.ForeColor = Color.FromKnownColor(KnownColor.WindowText)
            lblChange.Text = "Change"

            If amountTendered >= m_TotalSales Then
                totalChange = amountTendered - m_TotalSales

                If totalChange > cashAmount Then
                    totalChange = 0.0
                ElseIf totalChange > 0.0 Then
                    lblChangeValue.ForeColor = Color.Blue
                End If
            ElseIf amountTendered > 0.0 Then
                lblChange.Text = "Balance"
                lblChangeValue.ForeColor = Color.Red
                totalChange = m_TotalSales - amountTendered
            End If

            lblAmountDueValue.Text = String.Format("{0:N}", Math.Abs(m_TotalSales))
            lblAmountTenderedValue.Text = String.Format("{0:N}", amountTendered)
            lblChangeValue.Text = String.Format("{0:N}", Math.Abs(totalChange))
            lblChange.Appearance.ForeColor = lblChangeValue.ForeColor
            btnSave.Enabled = (totalChange >= 0)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            Dim totalAmountPaid As Double =
                numCashAmount.Value +
                numCreditCardAmount.Value +
                numGiftCertAmount.Value +
                numCreditMemoAmount.Value

            If Math.Round(totalAmountPaid, 2) < m_TotalSales Then
                ShowWarning("Payment is less than the total payable amount!", "Payment Failed")
                Return False
            ElseIf numGiftCertAmount.Value = 0.0 AndAlso (totalAmountPaid - numCashAmount.Value) > m_TotalSales Then
                ShowWarning("Total Credit Payment exceeds the amount due!", "Payment Failed")
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class