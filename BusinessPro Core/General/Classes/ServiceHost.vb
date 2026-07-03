Public Class ServiceHost

#Region "Declarations"

    Public Const MAX_RECEIVESIZE As Long = 1073741824 ' Max = 2147483647

    Private m_IPAddress As String
    Private m_PortNo As Integer
    Private m_Timeout As TimeSpan

    Private WithEvents m_ServiceHost As ServiceModel.ServiceHost

#End Region

#Region "Properties"

    Property [Class] As Type
    Property [IClass] As Type

    ReadOnly Property State() As String
        Get
            Return m_ServiceHost.State.ToString
        End Get
    End Property

    Private m_LogEvents As Boolean
    Public Property LogEvents() As Boolean
        Get
            Return m_LogEvents
        End Get
        Set(ByVal value As Boolean)
            m_LogEvents = value
        End Set
    End Property

    Private m_Name As String = "ServiceHost"
    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

#End Region

#Region "Events"

    Public Event Closed As EventHandler(Of EventArgs)
    Public Event Closing As EventHandler(Of EventArgs)
    Public Event Faulted As EventHandler(Of EventArgs)
    Public Event Opened As EventHandler(Of EventArgs)
    Public Event Opening As EventHandler(Of EventArgs)
    Public Event UnknownMessageReceived As EventHandler(Of EventArgs)

#End Region

#Region "Event Handlers"

    Private Sub ServiceHost_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ServiceHost.Closed
        If LogEvents Then
            LogMessage("ServiceHost (" & Name & ") Closed.", EventLogEntryType.Information)
        End If
        RaiseEvent Closed(sender, e)
    End Sub

    Private Sub ServiceHost_Closing(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ServiceHost.Closing
        If LogEvents Then
            LogMessage("ServiceHost (" & Name & ") Closing...", EventLogEntryType.Information)
        End If
        RaiseEvent Closing(sender, e)
    End Sub

    Private Sub ServiceHost_Faulted(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ServiceHost.Faulted
        If LogEvents Then
            LogMessage("ServiceHost (" & Name & ") Faulted!", EventLogEntryType.Error)
        End If
        RaiseEvent Faulted(sender, e)
    End Sub

    Private Sub ServiceHost_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ServiceHost.Opened
        If LogEvents Then
            LogMessage("ServiceHost (" & Name & ") Opened.", EventLogEntryType.Information)
        End If
        RaiseEvent Opened(sender, e)
    End Sub

    Private Sub ServiceHost_Opening(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ServiceHost.Opening
        If LogEvents Then
            LogMessage("ServiceHost (" & Name & ") Opening...", EventLogEntryType.Information)
        End If
        RaiseEvent Opening(sender, e)
    End Sub

    Private Sub ServiceHost_UnknownMessageReceived(ByVal sender As Object, ByVal e As System.ServiceModel.UnknownMessageReceivedEventArgs) Handles m_ServiceHost.UnknownMessageReceived
        If LogEvents Then
            LogMessage("ServiceHost (" & Name & ") UnknownMessageReceived.", EventLogEntryType.Warning)
        End If
        RaiseEvent UnknownMessageReceived(sender, e)
    End Sub

#End Region

#Region "Methods"

    Public Sub Open()
        Try
            Dim ServiceMetadataBehavior As New ServiceModel.Description.ServiceMetadataBehavior With {.HttpGetEnabled = True}
            Dim HttpBindingElement As New ServiceModel.Channels.HttpTransportBindingElement With {.KeepAliveEnabled = False}
            Dim DebugBehavior As New ServiceModel.Description.ServiceDebugBehavior With {.IncludeExceptionDetailInFaults = True}

            Dim BasicHTTPBinding As New ServiceModel.BasicHttpBinding
            With BasicHTTPBinding
                .MaxReceivedMessageSize = MAX_RECEIVESIZE
                .ReaderQuotas.MaxStringContentLength = MAX_RECEIVESIZE
                .CreateBindingElements.Add(HttpBindingElement)
            End With

            m_ServiceHost = New ServiceModel.ServiceHost(Me.Class, New Uri("http://" & m_IPAddress & ":" & m_PortNo & "/Database"))
            With m_ServiceHost
                .AddServiceEndpoint(IClass, BasicHTTPBinding, m_IPAddress)
                .Description.Behaviors.Add(ServiceMetadataBehavior)
                .Description.Behaviors.Remove(DebugBehavior.GetType)
                .Description.Behaviors.Add(DebugBehavior)
                .Open(m_Timeout)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Open(ServiceName As String)
        Try
            Dim ServiceMetadataBehavior As New ServiceModel.Description.ServiceMetadataBehavior With {.HttpGetEnabled = True}
            Dim HttpBindingElement As New ServiceModel.Channels.HttpTransportBindingElement With {.KeepAliveEnabled = False}
            Dim DebugBehavior As New ServiceModel.Description.ServiceDebugBehavior With {.IncludeExceptionDetailInFaults = True}

            Dim BasicHTTPBinding As New ServiceModel.BasicHttpBinding
            With BasicHTTPBinding
                .MaxReceivedMessageSize = MAX_RECEIVESIZE
                .ReaderQuotas.MaxStringContentLength = MAX_RECEIVESIZE
                .CreateBindingElements.Add(HttpBindingElement)
            End With

            m_ServiceHost = New ServiceModel.ServiceHost(Me.Class, New Uri("http://" & m_IPAddress & ":" & m_PortNo & "/" & ServiceName))
            With m_ServiceHost
                .AddServiceEndpoint(IClass, BasicHTTPBinding, m_IPAddress)
                .Description.Behaviors.Add(ServiceMetadataBehavior)
                .Description.Behaviors.Remove(DebugBehavior.GetType)
                .Description.Behaviors.Add(DebugBehavior)
                .Open(m_Timeout)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Close()
        Try
            m_ServiceHost.Close(m_Timeout)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Writes an information entry with the given message text to the event log.
    ''' </summary>
    ''' <param name="message">The string to write to the event log.</param>
    ''' <param name="type">One of the System.Diagnostics.EventLogEntryType values.</param>
    ''' <param name="addToLogFile">Determines the value whether the message will be written to application log file or not.</param>
    Private Sub LogMessage(ByVal message As String, ByVal type As EventLogEntryType, Optional ByVal addToLogFile As Boolean = True)
        Try
            Dim eventMsg As String =
                "Assembly: " & My.Application.Info.ProductName & " " & My.Application.Info.Version.ToString & vbCrLf &
                If(type = EventLogEntryType.Information, "Message: ", "") & message.Replace(vbNewLine, vbCrLf)

            EventLog.WriteEntry(My.Application.Info.ProductName, eventMsg, type, My.Application.Info.Version.Build)

            If addToLogFile Then
                Dim logPath As String =
                    My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".log"
                IO.File.AppendAllText(logPath, Now.ToString & vbCrLf & message & vbCrLf)
            End If
        Catch
            ' ignore error(s)
        End Try
    End Sub

#End Region

#Region "Constructor"

    Public Sub New(ByVal serverIP As String, ByVal portNo As Integer, Optional ByVal timeout As Integer = 300)
        Try
            m_IPAddress = serverIP
            m_PortNo = portNo
            m_Timeout = New TimeSpan(0, 0, timeout)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class

