Imports System.Drawing
Imports System.Windows.Forms

Friend Class FormWait

#Region "Constructors"

    Public Sub New(ByRef _Thread As Threading.Thread)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        lblMessage.Text = "Processing...please wait."
        m_Thread = _Thread
        Worker = New System.ComponentModel.BackgroundWorker()
    End Sub

#End Region

#Region "Declarations"

    Private WithEvents Worker As System.ComponentModel.BackgroundWorker

#End Region

#Region "Property"

    Private m_Thread As Threading.Thread
    Public ReadOnly Property Thread() As Threading.Thread
        Get
            Return m_Thread
        End Get
    End Property

    Public Shadows Property Text() As String
        Get
            Return lblMessage.Text
        End Get
        Set(ByVal value As String)
            lblMessage.Text = value
        End Set
    End Property

    Private m_ConfirmCancel As Boolean = False
    Public Property ConfirmCancel() As Boolean
        Get
            Return m_ConfirmCancel
        End Get
        Set(ByVal value As Boolean)
            m_ConfirmCancel = value
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Owner Is Nothing Then
            CenterToScreen()
        Else
            CenterToParent()
        End If
        TopMost = True
        btnCancel.Cursor = Cursors.Hand
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            DialogResult = Windows.Forms.DialogResult.Cancel
            If m_ConfirmCancel Then
                If MessageBox.Show("Current operation will be cancelled." & vbCrLf & vbCrLf & "Do you want to continue?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    DialogResult = Windows.Forms.DialogResult.None
                End If
            End If
            If DialogResult = Windows.Forms.DialogResult.Cancel Then
                Thread.Abort()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Worker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles Worker.DoWork
        Try
            While m_Thread.IsAlive
                Application.DoEvents()
            End While
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles Worker.RunWorkerCompleted
        If DialogResult <> Windows.Forms.DialogResult.Cancel Then
            DialogResult = Windows.Forms.DialogResult.OK
        End If
        Close()
    End Sub

    Private Sub Label1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseEnter
        sender.ForeColor = Color.Red
    End Sub

    Private Sub Label1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseLeave
        sender.ForeColor = Color.Silver
    End Sub

#End Region

#Region "Methods"

    Public Overloads Function Show() As DialogResult
        Try
            m_Thread.Start()
            Worker.RunWorkerAsync()
            Return MyBase.ShowDialog()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Overloads Function ShowDialog(ByVal _Owner As Windows.Forms.IWin32Window) As DialogResult
        Try
            m_Thread.Start()
            Worker.RunWorkerAsync()
            Return MyBase.ShowDialog(_Owner)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class