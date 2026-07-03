Imports Infragistics.Win.UltraWinGrid

<System.ComponentModel.DefaultEvent("InitializeGridLayout")> _
Public Class ListSearch

#Region "Events"

    Public Event AfterRowSelect(ByVal SelectedRow As UltraGridRow)
    Public Event CellClickButtonGrid(ByVal e As CellEventArgs)
    Public Event InitializeGridLayout(ByVal e As InitializeLayoutEventArgs)
    Public Event InitializeGridRow(ByVal e As InitializeRowEventArgs)
    Public Event OnCancel(ByRef CloseForm As Boolean)
    Public Event OnOk(ByVal SelectedRow As UltraGridRow)
    Public Event OnOkMultiSelect(ByVal SelectedRows As SelectedRowsCollection)

    Public Event RowDoubleClick(ByVal SelectedRow As UltraGridRow)
    Public Event KeyDownPress(ByVal e As System.Windows.Forms.KeyEventArgs)

#End Region

#Region "Properties"

    Public Property ActiveRow() As UltraGridRow
        Get
            Return grdList.ActiveRow
        End Get
        Set(ByVal value As UltraGridRow)
            grdList.ActiveRow = value
        End Set
    End Property

    Public ReadOnly Property ButtonOK As Infragistics.Win.Misc.UltraButton
        Get
            Return btnOk
        End Get
    End Property

    Public ReadOnly Property ButtonCancel As Infragistics.Win.Misc.UltraButton
        Get
            Return btnCancel
        End Get
    End Property

    Property CloseParentFormOnOK As Boolean = True

    Public Property DataSource() As Object
        Get
            txtSearch.Clear()
            Return grdList.DataSource
        End Get
        Set(ByVal value As Object)
            grdList.DataSource = value
        End Set
    End Property

    ReadOnly Property Grid() As Infragistics.Win.UltraWinGrid.UltraGrid
        Get
            Return grdList
        End Get
    End Property

    Property MultiSelect As Boolean
    Property MultiSelectHeader As Boolean ' Checkbox on header

    Private m_SearchKeys As String
    Public Property SearchKeys() As String
        Get
            Return m_SearchKeys
        End Get
        Set(ByVal value As String)
            m_SearchKeys = value
            If value IsNot Nothing Then
                txtSearch.Enabled = True
            End If
        End Set
    End Property

    Private m_SelectedRow As UltraGridRow
    Public ReadOnly Property SelectedRow() As UltraGridRow
        Get
            Return m_SelectedRow
        End Get
    End Property

    Private m_SelectedRows As SelectedRowsCollection
    Public ReadOnly Property SelectedRows() As SelectedRowsCollection
        Get
            Return m_SelectedRows
        End Get
    End Property

    Private m_ShowButtons As Boolean = True
    Public Property ShowButtons() As Boolean
        Get
            Return m_ShowButtons
        End Get
        Set(ByVal value As Boolean)
            m_ShowButtons = value
            If value Then
                btnCancel.Visible = True
                If m_UsedInBrowser Then
                    btnOk.Visible = True
                    txtSearch.Width = btnOk.Left - txtSearch.Left - 6 ' margin
                Else
                    txtSearch.Width = btnOk.Left + btnOk.Width - txtSearch.Left
                End If
            Else
                txtSearch.Width = btnCancel.Left + btnCancel.Width - txtSearch.Left
                btnCancel.Visible = False
                If m_UsedInBrowser Then
                    btnOk.Visible = False
                End If
            End If
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

#End Region

#Region "Event Handlers"

    Private Sub ucListSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            UsedInBrowser = m_UsedInBrowser
            ShowButtons = m_ShowButtons

            If Not ParentForm Is Nothing And m_ShowButtons Then
                With ParentForm
                    If .Modal Then
                        .AcceptButton = btnOk
                        .CancelButton = btnCancel
                    End If
                End With
            End If

            If m_SearchKeys Is Nothing Then
                txtSearch.Enabled = False
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            m_SelectedRow = Nothing
            m_SelectedRows = Nothing

            Dim hasSelection As Boolean

            If MultiSelect Then
                For Each row As UltraGridRow In grdList.Rows
                    If CBool(row.Cells("selected").Value) Then
                        hasSelection = True
                        Exit For
                    End If
                Next
            ElseIf ActiveRow IsNot Nothing Then
                hasSelection = True
            End If

            If hasSelection Then
                If MultiSelect Then
                    For Each row As UltraGridRow In grdList.Rows
                        row.Selected = CBool(row.Cells("selected").Text)
                    Next
                    m_SelectedRows = grdList.Selected.Rows
                    RaiseEvent OnOkMultiSelect(m_SelectedRows)
                Else
                    m_SelectedRow = ActiveRow
                    RaiseEvent OnOk(ActiveRow)
                End If

                If m_UsedInBrowser Then
                    If ParentForm IsNot Nothing Then
                        If CloseParentFormOnOK Then
                            ParentForm.DialogResult = DialogResult.OK
                            ParentForm.Close()
                        Else
                            CloseParentFormOnOK = True
                        End If
                    End If
                End If
            Else
                ShowSelectMessage()
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
            If m_UsedInBrowser AndAlso Not MultiSelect Then
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
            btnOk.Enabled = (grdList.Rows.Count > 0)

            If MultiSelect Then
                With e.Layout.Bands(0)
                    If Not .Columns.Exists("selected") Then
                        .Columns.Insert(0, "selected")
                    End If
                    With .Columns("selected")
                        .Header.Caption = ""
                        If MultiSelectHeader Then
                            .Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center
                            .Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection
                        End If
                        .CellActivation = Activation.AllowEdit
                        .CellClickAction = CellClickAction.Edit
                        .Style = ColumnStyle.CheckBox
                        .Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor
                        .SetWidth(50)
                        .DefaultCellValue = False
                        .DataType = GetType(Boolean)
                    End With
                End With
                e.Layout.Override.MaxSelectedRows = 0 ' multi select
            Else
                e.Layout.Override.MaxSelectedRows = 1
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

    Private Sub grdList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdList.KeyDown
        Try
            RaiseEvent KeyDownPress(e)

            If Not e.Handled AndAlso Not e.SuppressKeyPress Then
                If MultiSelect AndAlso UsedInBrowser Then
                    If e.KeyCode = Keys.Space AndAlso grdList.ActiveRow IsNot Nothing Then
                        With grdList.ActiveRow.Cells("selected")
                            .Value = Not .Value
                        End With
                    ElseIf e.KeyCode = Keys.Enter AndAlso e.Modifiers = Keys.None Then
                        grdList.UpdateData()
                        btnOk.PerformClick()
                    End If
                End If
            End If

                txtSearch.Focus()
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

    Private Sub txtSearch_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.ValueChanged
        Try
            If m_SearchKeys Is Nothing Then
                Throw New Exception("Search keys has not been set")
            End If

            grdList.ActiveRow = Nothing
            grdList.Selected.Rows.Clear()

            Dim strSearch As String = txtSearch.Text.Trim

            With grdList.DisplayLayout.Bands(0)
                .ColumnFilters.ClearAllFilters()
                .ColumnFilters.LogicalOperator = FilterLogicalOperator.Or

                If strSearch = "" Then
                    Activate()
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

    Public Sub Activate(Optional browserMode As Boolean = False)
        Try
            Focus()
            With grdList
                .Focus()
                If .Rows.Count > 0 Then
                    If .Rows.First.Activate() Then
                        .Rows.First.Selected = True
                    End If
                End If
            End With
            If browserMode Then
                txtSearch.Focus()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Clear()
        Try
            txtSearch.Clear()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LockGridRows()
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

    Public Sub UnlockGridRow(ByVal rowIndex As Integer, Optional ByVal columnKeys() As String = Nothing)
        Try
            grdList.Rows(rowIndex).Activation = Activation.AllowEdit
            If columnKeys IsNot Nothing Then
                For i As Integer = 0 To columnKeys.Count - 1
                    grdList.Rows(rowIndex).Cells(columnKeys(i)).Activation = Activation.NoEdit
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
