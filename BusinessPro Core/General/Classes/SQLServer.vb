Public Class SQLServer

#Region "Variables"

    Private m_DataBase As KinetiqueServiceReference.SQLServerClient
    Private m_strConnectionstring As String

#End Region

#Region "Constants"

    Private Const MaxRecieveSize As Int64 = 1000000000

#End Region

#Region "Constructor"

    Public Sub New(ByVal _strServerConnectionString As String,
                   ByVal _strServerAddress As String,
                   ByVal _strServerIP As String)
        Try
            m_strConnectionstring = _strServerConnectionString

            Dim ServiceEndPoint As New ServiceModel.EndpointAddress(New Uri(_strServerAddress & "/Database/" & _strServerIP))
            Dim BasicHttpBinding As New ServiceModel.BasicHttpBinding

            With BasicHttpBinding
                .MaxReceivedMessageSize = MaxRecieveSize
                .SendTimeout = New TimeSpan(0, 2, 0)
            End With

            m_DataBase = New KinetiqueServiceReference.SQLServerClient(BasicHttpBinding, ServiceEndPoint)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Function ExecuteNonQuery(ByVal _Connectionstring As String, ByVal _SQLStatement As String, ByVal _IsCached As Boolean, ByVal _EntityID As Long) As Boolean
        Try
            Return m_DataBase.ExecuteNonQuery(_Connectionstring, _SQLStatement, _IsCached, _EntityID)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteQuery(ByVal _Connectionstring As String, ByVal _SQLStatement As String) As System.Data.DataSet
        Try
            Return m_DataBase.ExecuteQuery(_Connectionstring, _SQLStatement)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteNonQueryReturn(ByVal _Connectionstring As String, ByVal _SQLStatement As String, ByVal _IsCached As Boolean, ByVal _EntityID As Long) As DataSet
        Try
            Return m_DataBase.ExecuteNonQueryReturn(_Connectionstring, _SQLStatement, _IsCached, _EntityID)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ExecuteNonQueryEx(ByVal _Connectionstring As String, ByVal _SQLStatement As String, ByVal _EntityID As Long) As Object
        Try
            'Return m_DataBase.ExecuteNonQueryReturn(_Connectionstring, _SQLStatement, _EntityID)
            Return Nothing ' Not finished yet :)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
