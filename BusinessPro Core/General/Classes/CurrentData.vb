Imports System.Globalization

Public Class Current

    Shared Property ApplicationIcon As Object
    Shared Property ConnectionString As DataSource.ConnectionString
    Shared Property CompanyInfo As Structures.CompanyInfo
    Shared Property CurrentCulture As CultureInfo
    Shared Property FormMain As Form
    Shared Property FormatUI As FormatUI
    Shared Property ProductName As String = My.Application.Info.ProductName
    Shared Property Rights As Rights
    Shared Property Status As Status
    Shared Property Settings As Structures.Settings
    Shared Property User As Structures.User

End Class
