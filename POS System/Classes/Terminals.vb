Friend Class Terminals
    Inherits DataSource

#Region "Properties"

    Property Data As DataTable
    Property UserName As String

#End Region

#Region "Methods"

    Private Function GetMacAddress() As String
        Try
            Dim netInterface() As System.Net.NetworkInformation.NetworkInterface =
                System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()

            Return netInterface(0).GetPhysicalAddress.ToString.Replace("-", "").Replace(":", "")
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function GetComputerName() As String
        Try
            Return My.Computer.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Sub SetTerminalActive(ByVal terminalId As Integer)
        Try
            Dim sql As String =
                "UPDATE [terminal] SET " & vbCrLf &
                "  is_active = 'True', " & vbCrLf &
                "  mac_address = " & GetMacAddress.Quote &
                "  computer_name = " & GetComputerName.Quote(False) &
                "WHERE id = " & terminalId

            ExecuteNonQuery(sql)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Fill()
        Try
            Dim sql As String =
                "SELECT " & vbCrLf &
                "   id," & vbCrLf &
                "   code," & vbCrLf &
                "   mac_address," & vbCrLf &
                "   computer_name," & vbCrLf &
                "   is_active " & vbCrLf &
                "FROM " & vbCrLf &
                "   [terminal] " & vbCrLf &
                "ORDER BY code"
            Data = ExecuteQuery(sql).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetTerminalCode() As String
        Try
            Dim sql As String =
                "SELECT TOP 1 code" & vbCrLf &
                "FROM [terminal]" & vbCrLf &
                "WHERE is_active = 'True'" & vbCrLf &
                "  AND mac_address = " & GetMacAddress.Quote(False) &
                "  AND computer_name = " & GetComputerName.Quote(False)

            Dim dt As DataTable = ExecuteQuery(sql).Tables(0)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)(0).ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetSerialNo(terminalCode As String) As String
        Try
            Dim sql As String =
                "SELECT machine_no, serial_no FROM [terminal] WHERE code = " & terminalCode.Quote(False)

            Dim dt As DataTable =
                ExecuteQuery(sql).Tables(0)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("serial_no")
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetMachineId() As String
        Try
            Dim mos As New System.Management.ManagementObjectSearcher(
                "SELECT * FROM Win32_BaseBoard")
            For Each id In mos.Get()
                Return id("SerialNumber").ToString
            Next
            Return ""
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetSerialNo() As String
        Try
            Dim mos As New System.Management.ManagementObjectSearcher(
                "SELECT * FROM Win32_BIOS")
            For Each id In mos.Get()
                Return id("SerialNumber").ToString
            Next
            Return ""
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
