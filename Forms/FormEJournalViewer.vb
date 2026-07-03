Imports System.IO
Imports Infragistics.Win.UltraWinListView

Friend Class FormEJournalViewer

#Region "Declarations"

    Private m_eJournalPath As String

#End Region

#Region "Event Handlers"

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub treeJournals_AfterSelect(sender As Object, e As Infragistics.Win.UltraWinTree.SelectEventArgs) Handles treeJournals.AfterSelect
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub lsvJournals_ItemActivated(sender As Object, e As ItemActivatedEventArgs) Handles lsvJournals.ItemActivated
        Try
            If e.Item IsNot Nothing Then
                txtJournal.Text = File.ReadAllText(e.Item.Tag)
                btnExport.Enabled = True
            Else
                btnExport.Enabled = False
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            Using dlg As New SaveFileDialog
                With dlg
                    .DefaultExt = "txt"
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    .FileName = lsvJournals.ActiveItem.Text & ".txt"
                    .Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                    .FilterIndex = 0
                    .Title = "Save " & .FileName & " to File"
                    If .ShowDialog(Me) = DialogResult.OK Then
                        File.Copy(lsvJournals.ActiveItem.Tag, .FileName)
                    End If
                End With
            End Using
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            Const DIR_BIZPRO As String = "BusinessPro"

            Dim dirPOS As String = My.Application.Info.Description.Replace(DIR_BIZPRO, "").Trim
            Dim pathAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

            m_eJournalPath = Path.Combine(
                pathAppData, My.Application.Info.CompanyName & "\" & DIR_BIZPRO & "\" & dirPOS & "\")

            Try
                If Not Directory.Exists(m_eJournalPath) Then
                    Directory.CreateDirectory(m_eJournalPath)
                End If
            Catch ex As Exception
                '
            End Try

            treeJournals.ExpandAll()
            lblPath.Visible = Core.Current.User.IsSuperRole
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            lsvJournals.ActiveItem = Nothing
            lsvJournals.Items.Clear()
            txtJournal.Text = String.Empty

            Dim path As String = m_eJournalPath
            Dim pattern As String = ""

            With treeJournals
                With If(.ActiveNode, .Nodes(0))
                    Select Case .Key
                        Case ""
                            pattern = "????-??-?? E-Journal.txt"
                        Case "Receipts"
                            pattern = "SI????????????*.txt"
                        Case "ReceiptsRefund"
                            pattern = "SI????????????-Refund.txt"
                        Case "ReceiptsVoid"
                            pattern = "SI????????????-Void.txt"
                        Case "XReadings"
                            pattern = "????-??-?? X-Reading-????.txt"
                        Case "ZReadings"
                            pattern = "????-??-?? Z-Reading.txt"
                    End Select
                    path = m_eJournalPath & .Key
                End With
            End With

            If Directory.Exists(path) Then
                lblPath.Text = "Path: " & path

                For Each file As String In Directory.EnumerateFiles(path, pattern)
                    Dim item As New UltraListViewItem With {
                    .Key = file.Substring(file.LastIndexOf("\") + 1),
                    .Tag = file,
                    .Value = .Key.Substring(0, .Key.Length - 4)
                }

                    lsvJournals.Items.Insert(0, item)
                Next

                If lsvJournals.Items.Count > 0 Then
                    lsvJournals.Items.Item(0).Activate()
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class