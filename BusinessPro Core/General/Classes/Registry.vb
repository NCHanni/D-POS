Imports Microsoft.Win32

Public Class Registry

#Region "Variables"

    Private Shared m_SubKey As String

#End Region

#Region "Properties"

    Private Shared m_CompanyName As String
    Public Shared Property CompanyName() As String
        Get
            Return m_CompanyName
        End Get
        Set(ByVal value As String)
            m_CompanyName = value
        End Set
    End Property

    Private Shared m_ProductName As String
    Public Shared Property ProductName() As String
        Get
            Return m_ProductName
        End Get
        Set(ByVal value As String)
            m_ProductName = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Shared Sub CreateKey()
        Dim newKey As RegistryKey
        newKey = My.Computer.Registry.LocalMachine.CreateSubKey("Software\" & CompanyName & "\" & ProductName)
    End Sub

    Public Shared Sub DeleteKey()
        My.Computer.Registry.LocalMachine.DeleteSubKey("Software\" & CompanyName & "\" & ProductName & "\" & m_SubKey)
    End Sub

    Public Shared Function ReadValue(ByVal name As String) As String
        Dim keyValue As String
        keyValue = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\Software\" & CompanyName & "\" & ProductName & "\" & m_SubKey, name, Nothing)

        Return keyValue
    End Function

    Public Shared Function ReadGlobalSetting(ByVal name As String, Optional defaultValue As String = "") As String
        Dim keyValue As String
        keyValue = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\Software\" & CompanyName & "\" & ProductName, name, defaultValue)

        Return If(keyValue, defaultValue)
    End Function

    Public Shared Sub WriteValue(ByVal name As String, ByVal value As String)
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\Software\" & CompanyName & "\" & ProductName & "\" & m_SubKey, name, value)
    End Sub

    Public Shared Sub WriteGlobalSetting(ByVal name As String, ByVal value As String)
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\Software\" & CompanyName & "\" & ProductName, name, value)
    End Sub

    Public Shared Sub SetSubKey(ByVal key As String)
        m_SubKey = key
    End Sub

    Public Shared Function GetProfileList() As String()
        If Exists() Then
            Dim Key As RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("Software\" & CompanyName & "\" & ProductName)
            Return Key.GetSubKeyNames
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function Exists() As Boolean
        Dim IsExisting As Boolean = False
        Try
            If My.Computer.Registry.LocalMachine.OpenSubKey("Software\" & CompanyName & "\" & ProductName) IsNot Nothing Then
                IsExisting = True
            End If
        Finally
            My.Computer.Registry.CurrentUser.Close()
        End Try
        Return IsExisting
    End Function

#End Region

End Class