Friend Class ucReportViewer

#Region "Constructor"

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dock = DockStyle.Fill
    End Sub

#End Region

#Region "Properties"

    Public Property ShowPrintButton() As Boolean
        Get
            Return crvViewer.ShowPrintButton
        End Get
        Set(ByVal value As Boolean)
            crvViewer.ShowPrintButton = value
        End Set
    End Property

    Public Property ShowExportButton() As Boolean
        Get
            Return crvViewer.ShowExportButton
        End Get
        Set(ByVal value As Boolean)
            crvViewer.ShowExportButton = value
        End Set
    End Property

    Public Property ReportSource() As Object
        Get
            Return crvViewer.ReportSource
        End Get
        Set(ByVal value As Object)
            crvViewer.ReportSource = value
        End Set
    End Property

    Private m_ModuleName As String = ""
    Public Property ModuleName() As String
        Get
            Return m_ModuleName
        End Get
        Set(ByVal value As String)
            m_ModuleName = value
        End Set
    End Property

#End Region

#Region "Event Handler"

    Private Sub ReportViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With crvViewer
            .BorderStyle = Windows.Forms.BorderStyle.None
            .EnableDrillDown = True
            .ShowCloseButton = False
            .ShowGroupTreeButton = False
            .ShowRefreshButton = False
        End With
    End Sub

#End Region

#Region "Methods"

    Public Sub Zoom(ByVal _ZoomLevel As Integer)
        Try
            crvViewer.Zoom(_ZoomLevel)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ApplyRights()
        Try
            If m_ModuleName <> "" Then
                crvViewer.ShowExportButton = Current.Rights.IsAllowed(m_ModuleName, Rights.AccessRights.Print)
                crvViewer.ShowPrintButton = Current.Rights.IsAllowed(m_ModuleName, Rights.AccessRights.Print)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
