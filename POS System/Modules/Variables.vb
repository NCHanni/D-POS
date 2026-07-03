Module Variables

    ' Default connection settings
    Public Const DEFAULT_DATASOURCE As String = "BLACKWIDOW\SQL2017"
    Public Const DEFAULT_DATABASE As String = "BUSINESS-PRO-POS"
    Public Const DEFAULT_USERNAME As String = "sa"
    Public Const DEFAULT_PASSWORD As String = "S1NJQGRtIW4=" ' KSI@dm!n

    ' Keep all values ALL CAPS
    Public Const CUSTOMER_WALKIN As String = "WALK-IN"

    ' ---------------------
    ' Customizable Settings
    ' ---------------------

    ''' <summary>
    ''' Round-off VAT amount immediately after being computated per line.
    ''' </summary>
    Public Const ROUNDOFF_VAT_PER_LINE As Boolean = True

    ''' <summary>
    ''' Round-off VAT amount in this number of decimals after being computated per line.
    ''' </summary>
    Public Const ROUNDOFF_VAT_PER_LINE_DECIMALS As Integer = 8

    ''' <summary>
    ''' Round-off VAT amount in this number of decimals (for acumatica integration) after being computated per line.
    ''' </summary>
    Public Const ROUNDOFF_VAT_PER_LINE_DECIMALS_ACU As Integer = 2

    ''' <summary>
    ''' Round-off computed net price. Ignored if ROUNDOFF_VAT is True.
    ''' </summary>
    Public Const ROUNDOFF_NET_PER_LINE As Boolean = True

    ''' <summary>
    ''' Round-off total deductions.
    ''' </summary>
    Public Const ROUNDOFF_DEDUCTIONS As Boolean = True

    ''' <summary>
    ''' Round-off discount amount after computation from percent discount.
    ''' </summary>
    Public Const ROUNDOFF_DISCOUNT_AMOUNT As Boolean = True

    ''' <summary>
    ''' Round-off discounted price.
    ''' </summary>
    Public Const ROUNDOFF_DISCOUNT_PRICE As Boolean = True

    ''' <summary>
    ''' Round-off percent discount computed from discount amount.
    ''' </summary>
    Public Const ROUNDOFF_DISCOUNT_PERCENT As Boolean = True

    ''' <summary>
    ''' Round-off total discount per line.
    ''' </summary>
    Public Const ROUNDOFF_DISCOUNT_TOTAL As Boolean = True

    ''' <summary>
    ''' Use net price to compute discount instead of unit price.
    ''' </summary>
    Public Const DISCOUNT_NET_PRICE As Boolean = True

    ''' <summary>
    ''' Round-off total sales.
    ''' </summary>
    Public Const ROUNDOFF_TOTAL_SALES As Boolean = True

    ''' <summary>
    ''' Round-off total VATable sales.
    ''' </summary>
    Public Const ROUNDOFF_TOTAL_VATABLE_SALES As Boolean = True

    ''' <summary>
    ''' Round-off total NONVAT sales
    ''' </summary>
    Public Const ROUNDOFF_TOTAL_VAT_EXEMPT_SALES As Boolean = True

    ''' <summary>
    ''' Round-off total VAT amount.
    ''' </summary>
    Public Const ROUNDOFF_TOTAL_VAT_AMOUNT As Boolean = True

End Module
