Public Class FormSaveDraftPrompt

    Enum SaveTypes
        SaveAsDraft
        PostTransaction
    End Enum

    Public Property Title() As String
        Get
            Return Me.Text
        End Get
        Set(ByVal value As String)
            Me.Text = value
        End Set
    End Property

    Private m_Message As String = "Record will now be saved." & vbNewLine & vbNewLine & "What do you want to do?"
    Public Property Message() As String
        Get
            Return m_Message
        End Get
        Set(ByVal value As String)
            m_Message = value
        End Set
    End Property

    Private m_SaveType As SaveTypes
    Public ReadOnly Property SaveType() As SaveTypes
        Get
            Return m_SaveType
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Close()
    End Sub

    Private Sub frmSaveDraftPrompt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DialogResult = Windows.Forms.DialogResult.None
        lblText.Text = m_Message.Replace(vbNewLine, vbCrLf)
    End Sub

    Private Sub btnSaveAsDraft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAsDraft.Click
        DialogResult = Windows.Forms.DialogResult.Yes
        m_SaveType = SaveTypes.SaveAsDraft
    End Sub

    Private Sub btnPostTransaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostTransaction.Click
        DialogResult = Windows.Forms.DialogResult.No
        m_SaveType = SaveTypes.PostTransaction
    End Sub

End Class