Imports Infragistics.Win.UltraWinGrid

<System.ComponentModel.DefaultEvent("InitializeGridLayout")>
Friend Class ucListSearch

#Region "Declarations"

    Private m_SearchKeys As String

#End Region

#Region "Events"

    Public Event AfterRowSelect(ByVal SelectedRow As UltraGridRow)
    Public Event CellClickButtonGrid(ByVal e As CellEventArgs)
    Public Event InitializeGridLayout(ByVal e As InitializeLayoutEventArgs)
    Public Event InitializeGridRow(ByVal e As InitializeRowEventArgs)
    Public Event OnCancel(ByRef CloseForm As Boolean)
    Public Event OnOK(ByVal SelectedRow As UltraGridRow)
    Public Event OnSearch(ByVal keyword As String)
    Public Event RowDoubleClick(ByVal SelectedRow As UltraGridRow)

#End Region

#Region "Properties"

    Public Property DataSource() As Object
        Get
            Return grdList.DataSource
        End Get
        Set(ByVal value As Object)
            grdList.DataSource = value
        End Set
    End Property

    Public Property ActiveRow() As UltraGridRow
        Get
            Return grdList.ActiveRow
        End Get
        Set(ByVal value As UltraGridRow)
            grdList.ActiveRow = value
        End Set
    End Property

    Public Property SearchKeys() As String
        Get
            Return m_SearchKeys
        End Get
        Set(ByVal value As String)
            m_SearchKeys = value
        End Set
    End Property

    Public Property ShowSearch() As Boolean
        Get
            Return txtSearch.Visible
        End Get
        Set(ByVal value As Boolean)
            lblSearch.Visible = value
            txtSearch.Visible = value
        End Set
    End Property

    Private m_UsedInBrowser As Boolean = False
    Public Property UsedInBrowser() As Boolean
        Get
            Return m_UsedInBrowser
        End Get
        Set(ByVal value As Boolean)
            m_UsedInBrowser = value
            If m_UsedInBrowser Then
                btnOk.Visible = True
                txtSearch.Width = btnOk.Left - txtSearch.Left - 6 ' margin
                btnCancel.Text = "&Cancel"
            Else
                btnOk.Visible = False
                txtSearch.Width = btnOk.Left + btnOk.Width - txtSearch.Left
                btnCancel.Text = "&Close"
            End If
        End Set
    End Property

    Property AutoClose As Boolean = True

#End Region

#Region "Event Handlers"

    Private Sub ucListSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            txtSearch.Clear()
            txtSearch.AutoSize = False
            txtSearch.Height = 21

            If m_UsedInBrowser Then
                btnOk.Visible = True
                txtSearch.Width = btnOk.Left - txtSearch.Left - 6 ' margin
                btnCancel.Text = "&Cancel"
            Else
                btnOk.Visible = False
                txtSearch.Width = btnOk.Left + btnOk.Width - txtSearch.Left
                btnCancel.Text = "&Close"
            End If

            If Not ParentForm Is Nothing Then
                With ParentForm
                    If .Modal Then
                        .AcceptButton = btnOk
                        .CancelButton = btnCancel
                    End If
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If ActiveRow Is Nothing Then
                ShowSelectMessage()
            Else
                RaiseEvent OnOK(ActiveRow)
                If m_UsedInBrowser AndAlso AutoClose Then
                    If ParentForm IsNot Nothing Then
                        ParentForm.DialogResult = DialogResult.OK
                        ParentForm.Close()
                    End If
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Dim CloseForm As Boolean = True
            RaiseEvent OnCancel(CloseForm)
            If CloseForm Then
                If ParentForm IsNot Nothing Then
                    ParentForm.DialogResult = DialogResult.Cancel
                    ParentForm.Close()
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_AfterSelectChange(ByVal sender As Object, ByVal e As AfterSelectChangeEventArgs) Handles grdList.AfterSelectChange
        Try
            If ActiveRow IsNot Nothing Then
                RaiseEvent AfterRowSelect(ActiveRow)
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_ClickCellButton(ByVal sender As Object, ByVal e As CellEventArgs) Handles grdList.ClickCellButton
        Try
            RaiseEvent CellClickButtonGrid(e)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_DoubleClickRow(ByVal sender As Object, ByVal e As DoubleClickRowEventArgs) Handles grdList.DoubleClickRow
        Try
            If m_UsedInBrowser And pnlSearch.Visible Then
                btnOk.PerformClick()
            Else
                If ActiveRow IsNot Nothing Then
                    RaiseEvent RowDoubleClick(ActiveRow)
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As Object, ByVal e As InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            If grdList.Rows.Count = 0 Then
                btnOk.Enabled = False
                btnCancel.Focus()
            Else
                btnOk.Enabled = True
                grdList.Focus()
                grdList.Rows(0).Activate()
            End If
            RaiseEvent InitializeGridLayout(e)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeRow(ByVal sender As Object, ByVal e As InitializeRowEventArgs) Handles grdList.InitializeRow
        Try
            btnOk.Enabled = True
            RaiseEvent InitializeGridRow(e)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Up, Keys.Down
                    If grdList.Rows.Count > 0 Then
                        grdList.Focus()
                        grdList.Selected.Rows.Clear()
                        If grdList.ActiveRow Is Nothing Then
                            If e.KeyCode = Keys.Up Then
                                grdList.Rows(grdList.Rows.VisibleRowCount - 1).Activate()
                            Else
                                grdList.Rows(0).Activate()
                            End If
                        Else
                            If e.KeyCode = Keys.Up Then
                                If grdList.ActiveRow.Index = 0 Then
                                    grdList.Rows(grdList.Rows.Count - 1).Activate()
                                Else
                                    grdList.Rows(grdList.ActiveRow.Index - 1).Activate()
                                End If
                            Else
                                If grdList.ActiveRow.Index = grdList.Rows.Count - 1 Then
                                    grdList.Rows(0).Activate()
                                Else
                                    grdList.Rows(grdList.ActiveRow.Index + 1).Activate()
                                End If
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Filter_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.ValueChanged
        Try
            If m_SearchKeys Is Nothing Then
                Throw New Exception("Search keys has not been set")
            End If

            grdList.ActiveRow = Nothing

            Dim strSearch As String = txtSearch.Text.Trim

            With grdList.DisplayLayout.Bands(0)
                .ColumnFilters.ClearAllFilters()
                .ColumnFilters.LogicalOperator = FilterLogicalOperator.Or

                If strSearch = "" Then
                    Return
                End If

                For Each searchKey In m_SearchKeys.Split(",")
                    For Each searchValue In strSearch.Split(" ")
                        .ColumnFilters(searchKey.Trim).FilterConditions.Add(FilterComparisionOperator.Contains, searchValue)
                    Next
                Next
            End With

            btnOk.Enabled = (grdList.Rows.Count > 0)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Sub Clear()
        Try
            txtSearch.Clear()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub GridLockRows()
        Try
            For Each row As UltraGridRow In grdList.Rows
                If row.Activation = Activation.AllowEdit Then
                    row.Activation = Activation.NoEdit
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub GridUnlockRow(ByVal _RowIndex As Integer, Optional ByVal ColString As String() = Nothing)
        Try
            grdList.Rows(_RowIndex).Activation = Activation.AllowEdit
            If ColString IsNot Nothing Then
                For i As Integer = 0 To ColString.Count - 1
                    grdList.Rows(_RowIndex).Cells(ColString(i)).Activation = Activation.NoEdit
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
