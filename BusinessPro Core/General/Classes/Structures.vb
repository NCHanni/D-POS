Public Class Structures

    Public Structure User
        Dim Code As String
        Dim RoleCode As String
        Dim RoleName As String
        Dim IsSuperRole As Boolean
        Dim Name As String
        Dim UserName As String
        Dim Password As String ' Encrypted
        Dim SecureHash As String
        Dim HasBarcode As Boolean
    End Structure

    Public Structure CompanyInfo
        Dim Name As String
        Dim Name2 As String
        Dim PermitNo As String
        Dim PermitMIN As String
        Dim PermitIssued As Date
        Dim PermitExpiry As Date
        Dim VatRegistrationNo As String
        Dim BusinessStyle As String
        Dim Address As String
        Dim ContactNo As String
        Dim FaxNo As String
        Dim EmailAddress As String
        Dim WebsiteUrl As String
    End Structure

    Public Structure Settings
        Dim SalesTax As Double
        Dim IsGiftCertEnabled As Boolean

        Dim TerminalCode As String
        Dim TerminalSerialNo As String

        Dim DaySessionId As Long
        Dim CashierSessionId As Long
        Dim CashInDate As Date
        Dim CashInAmount As Double
        Dim CashInSet As Boolean
    End Structure

End Class
