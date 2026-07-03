Friend Class FormSetDiscount

#Region "Variables"

    Private m_IsUpdating As Boolean = True ' True by default to not allow events to trigger when set outside form
    Private m_OldDiscountPercent As Double

#End Region

#Region "Properties"

    Property UnitPrice As Double
        Get
            Return numPrice.Value
        End Get
        Set(ByVal value As Double)
            numPrice.Value = value
        End Set
    End Property

    Property DiscountPercent As Double
        Get
            Return numDiscountPercent.Value
        End Get
        Set(ByVal value As Double)
            numDiscountPercent.Value = value
        End Set
    End Property

    ReadOnly Property DiscountPercentOld As Double
        Get
            Return m_OldDiscountPercent
        End Get
    End Property

    Property DiscountAmount As Double
        Get
            Return numDiscountAmount.Value
        End Get
        Set(ByVal value As Double)
            numDiscountAmount.Value = value
        End Set
    End Property

    Property DiscountActual As Double
        Get
            Return numDiscountActual.Value
        End Get
        Set(ByVal value As Double)
            numDiscountActual.Value = value
        End Set
    End Property

    Property DiscountedPrice As Double
        Get
            Return numDiscountedPrice.Value
        End Get
        Set(ByVal value As Double)
            numDiscountedPrice.Value = value
        End Set
    End Property

    Public Property VATAmount As Double
        Get
            Return numVAT.Value
        End Get
        Set(value As Double)
            numVAT.Value = value
        End Set
    End Property

    Public Property VATAmountComputed As Double
        Get
            Return numVatComputed.Value
        End Get
        Set(value As Double)
            numVatComputed.Value = value
        End Set
    End Property

    Property IsVatable As Boolean
    Property IsVatExempt As Boolean
    Property VATPercent As Double
    Property ViewMode As Boolean

#End Region

#Region "Event Handlers"

    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            lblVat.Text = lblVat.Text.Replace("(%)", "(" & VATPercent & "%)")
            numVAT.Value = numPrice.Value / (1 + (VATPercent / 100)) * (VATPercent / 100)

            If ROUNDOFF_VAT_PER_LINE Then
                numVAT.Value = Math.Round(numVAT.Value, 8)
            End If

            numNetPrice.Value = numPrice.Value - numVAT.Value

            If ROUNDOFF_NET_PER_LINE Then
                numNetPrice.Value = Math.Round(numNetPrice.Value, 8)
            End If

            If numDiscountAmount.Value > numNetPrice.Value Then
                numDiscountAmount.Value = numNetPrice.Value
            End If

            m_OldDiscountPercent = DiscountPercent
            numDiscountPercent.MaxValue = 100
            numDiscountAmount.MaxValue = numNetPrice.Value
            numDiscountActual.MaxValue = numPrice.Value
            numDiscountPercent.Focus()
            m_IsUpdating = False
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If ViewMode Then
            Me.Text = "View Discount Details"
            numDiscountPercent.Enabled = False
            numDiscountAmount.Enabled = False
            numDiscountActual.Enabled = False
            btnOK.Visible = False
            btnCancel.Text = "Close"
        End If
    End Sub

    Private Sub numDiscountPercent_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numDiscountPercent.ValueChanged ', numVAT.ValueChanged, numNetPrice.ValueChanged
        Try
            If Not m_IsUpdating Then
                If CDblEx(numDiscountPercent.Value) > 0 Then
                    m_IsUpdating = True

                    Dim amountDue As Double = If(DISCOUNT_NET_PRICE, numNetPrice.Value, numPrice.Value)

                    DiscountAmount = amountDue * (DiscountPercent / 100)
                    If ROUNDOFF_DISCOUNT_AMOUNT Then
                        DiscountAmount = Math.Round(DiscountAmount, 8)
                    End If

                    ' Price exclusive of VAT
                    DiscountedPrice = amountDue - DiscountAmount
                    If ROUNDOFF_DISCOUNT_PRICE Then
                        DiscountedPrice = Math.Round(DiscountedPrice, 8)
                    End If

                    ComputeNewVAT()

                    DiscountActual = numPrice.Value - numNewPrice.Value

                    m_IsUpdating = False
                Else
                    DiscountAmount = 0.0
                    DiscountedPrice = 0.0
                    DiscountActual = 0.0
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub numDiscountAmount_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numDiscountAmount.ValueChanged
        Try
            If Not m_IsUpdating Then
                If CDblEx(numDiscountAmount.Value) > 0 Then
                    m_IsUpdating = True

                    Dim amountDue As Double = If(DISCOUNT_NET_PRICE, numNetPrice.Value, numPrice.Value)

                    DiscountPercent = (DiscountAmount / amountDue) * 100
                    If ROUNDOFF_DISCOUNT_PERCENT Then
                        DiscountPercent = Math.Round(DiscountPercent, 8)
                    End If

                    ' Price exclusive of VAT
                    DiscountedPrice = amountDue - DiscountAmount
                    If ROUNDOFF_DISCOUNT_PRICE Then
                        DiscountedPrice = Math.Round(DiscountedPrice, 8)
                    End If

                    ComputeNewVAT()

                    DiscountActual = numPrice.Value - numNewPrice.Value

                    m_IsUpdating = False
                Else
                    DiscountPercent = 0.0
                    DiscountedPrice = 0.0
                    DiscountActual = 0.0
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub numActualDiscount_ValueChanged(sender As Object, e As EventArgs) Handles numDiscountActual.ValueChanged
        Try
            If Not m_IsUpdating Then
                If CDblEx(numDiscountActual.Value) > 0 Then
                    m_IsUpdating = True

                    Dim amountDue As Double = If(DISCOUNT_NET_PRICE, numNetPrice.Value, numPrice.Value)

                    DiscountPercent = DiscountActual / numPrice.Value * 100
                    If ROUNDOFF_DISCOUNT_PERCENT Then
                        DiscountPercent = Math.Round(DiscountPercent, 8)
                    End If

                    DiscountAmount = amountDue * (DiscountPercent / 100)
                    If ROUNDOFF_DISCOUNT_AMOUNT Then
                        DiscountAmount = Math.Round(DiscountAmount, 8)
                    End If

                    ' Price exclusive of VAT
                    DiscountedPrice = amountDue - DiscountAmount
                    If ROUNDOFF_DISCOUNT_PRICE Then
                        DiscountedPrice = Math.Round(DiscountedPrice, 8)
                    End If

                    ComputeNewVAT()

                    m_IsUpdating = False
                Else
                    DiscountPercent = 0.0
                    DiscountedPrice = 0.0
                    DiscountActual = 0.0
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
        If IsDBNull(DiscountPercent) Then
            DiscountPercent = 0.0
        End If

        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

#End Region

#Region "Methods"

    Private Sub ComputeNewVAT()
        Try
            If IsVatExempt Then
                lblVatComputed.Text = "VAT Exempt (" & VATPercent & "%)"
                numVatComputed.Value = numVAT.Value
            Else
                lblVatComputed.Text = "New VAT (" & VATPercent & "%)"
                If IsVatable Then
                    numVatComputed.Value = (DiscountedPrice * (VATPercent / 100))
                Else
                    numVatComputed.Value = 0.0
                End If
            End If

            numNewPrice.Value = numDiscountedPrice.Value + numVatComputed.Value
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class