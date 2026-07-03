Public Class Status

#Region "Variables"

    Private m_ConnectionStatus As Connection
    Private m_ServerAddress As String = ""
    Private WithEvents m_Timer As Timers.Timer

#End Region

#Region "Enum"

    Public Enum Connection
        Online = 0
        OffLine = 1
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(ByVal _ServerAddress As String)
        Try
            Me.m_ServerAddress = _ServerAddress
            Me.m_Timer = New Timers.Timer(1000)

            'Start Timer
            Me.m_Timer.Enabled = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Property"

    ReadOnly Property ConnectionStatus() As Connection
        Get
            Return Me.m_ConnectionStatus
        End Get
    End Property

#End Region

#Region "Event"

    Public Event ConnectionStatusChanged(ByVal _Status As String)

#End Region

#Region "Methods"

    Sub StopTimer()
        m_Timer.Stop()
    End Sub

    Private Sub m_Timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles m_Timer.Elapsed
        Try
            Me.m_Timer.Enabled = False
            Me.CheckConnection()
        Catch ' ignore error
        End Try
    End Sub

    Private Sub CheckConnection()
        Try
            Try
                Dim CheckRequest As Net.HttpWebRequest = Net.HttpWebRequest.Create(Me.m_ServerAddress & "/Database")
                Dim CheckRepond As Net.HttpWebResponse = CheckRequest.GetResponse

                If CheckRepond.StatusCode = Net.HttpStatusCode.OK Then
                    CheckRepond.Close()
                    Me.m_ConnectionStatus = Connection.Online
                    RaiseEvent ConnectionStatusChanged("Online")
                Else
                    CheckRepond.Close()
                    Me.m_ConnectionStatus = Connection.OffLine
                    RaiseEvent ConnectionStatusChanged("Offline")
                End If
            Catch ex As Exception
                Me.m_ConnectionStatus = Connection.OffLine
                RaiseEvent ConnectionStatusChanged("Offline")
            End Try

            Me.m_Timer.Enabled = True
        Catch ' ignore error
        End Try
    End Sub

#End Region

End Class