<System.ComponentModel.DefaultProperty("Text")> _
Public Class Wait

#Region "Constructor"

    Public Sub New(ByRef _Thread As Threading.Thread)
        m_FormWait = New FormWait(_Thread)
        m_FormWait.Text = "Processing...please wait."
    End Sub

    Public Sub New(ByRef _Thread As Threading.Thread, ByVal _Text As String)
        m_FormWait = New FormWait(_Thread)
        m_FormWait.Text = _Text
    End Sub

#End Region

#Region "Declarations"

    Private WithEvents m_FormWait As FormWait
    Public Enum DialogResult
        OK = System.Windows.Forms.DialogResult.OK
        Cancel = System.Windows.Forms.DialogResult.Cancel
    End Enum

#End Region

#Region "Properties"

    ''' <summary>Returns the thread that is being processed.</summary>
    Public ReadOnly Property Thread() As Threading.Thread
        Get
            Return m_FormWait.Thread
        End Get
    End Property

    ''' <summary>Returns/sets the displayed text describing the action being processed.</summary>
    Public Property Text() As String
        Get
            Return m_FormWait.Text
        End Get
        Set(ByVal value As String)
            m_FormWait.Text = value
        End Set
    End Property

    ''' <summary>Returns/sets whether a confirmation will be displayed in cancelling the process.</summary>
    Public Property ConfirmCancel() As Boolean
        Get
            Return m_FormWait.ConfirmCancel
        End Get
        Set(ByVal value As Boolean)
            m_FormWait.ConfirmCancel = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub Close()
        Try
            m_FormWait.Close()
        Catch 
        End Try
    End Sub

    Public Sub Hide()
        Try
            m_FormWait.Hide()
        Catch 
        End Try
    End Sub

    Public Function Show() As DialogResult
        Try
            Return m_FormWait.Show
        Catch 
        End Try
    End Function

    Public Function Show(ByVal _Owner As Windows.Forms.IWin32Window) As DialogResult
        Try
            Return m_FormWait.ShowDialog(_Owner)
        Catch 
        End Try
    End Function

#End Region

End Class