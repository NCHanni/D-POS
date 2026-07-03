Imports Infragistics.Win.UltraWinGrid

<System.ComponentModel.DefaultEvent("InitializeGridLayout")> _
Public Class ListWithDateRange

#Region "Events"

    Public Event AfterRowSelect(ByVal SelectedRow As UltraGridRow)
    Public Event CellClickButtonGrid(ByVal e As CellEventArgs)
    Public Event InitializeGridLayout(ByVal e As InitializeLayoutEventArgs)
    Public Event InitializeGridRow(ByVal e As InitializeRowEventArgs)
    Public Event OnCancel(ByRef CloseForm As Boolean)
    Public Event OnRefresh()
    Public Event OnSearch()
    Public Event OnOk(ByVal SelectedRow As UltraGridRow)
    Public Event OnOkMultiSelect(ByVal SelectedRow As SelectedRowsCollection)
    Public Shadows Event OnPrint()
    Public Event OnView(ByVal SelectedRow As UltraGridRow)

#End Region

#Region "Properties"

    Public Property ActiveRow() As UltraGridRow
        Get
            Return ctrlList.ActiveRow
        End Get
        Set(ByVal value As UltraGridRow)
            ctrlList.ActiveRow = value
        End Set
    End Property

    Public Property CloseParentFormOnOK() As Boolean
        Get
            Return ctrlList.CloseParentFormOnOK
        End Get
        Set(ByVal value As Boolean)
            ctrlList.CloseParentFormOnOK = value
        End Set
    End Property

    Public Property DataSource() As Object
        Get
            Return ctrlList.DataSource
        End Get
        Set(ByVal value As Object)
            ctrlList.DataSource = value
        End Set
    End Property

    Public Property Grid() As UltraGrid
        Get
            Return ctrlList.grdList
        End Get
        Set(ByVal value As UltraGrid)
            ctrlList.grdList = value
        End Set
    End Property

    Private m_ModuleName As String
    Public Property ModuleName() As String
        Get
            Return m_ModuleName
        End Get
        Set(ByVal value As String)
            m_ModuleName = value
        End Set
    End Property

    Public Property StartDate() As Date
        Get
            Return ctrlDateRange.StartDate
        End Get
        Set(ByVal value As Date)
            ctrlDateRange.StartDate = value
        End Set
    End Property

    Public Property EndDate() As Date
        Get
            Return ctrlDateRange.EndDate
        End Get
        Set(ByVal value As Date)
            ctrlDateRange.EndDate = value
        End Set
    End Property

    Public Property MultiSelect() As Boolean
        Get
            Return ctrlList.MultiSelect
        End Get
        Set(ByVal value As Boolean)
            ctrlList.MultiSelect = value
        End Set
    End Property

    Public Property MultiSelectHeader() As Boolean
        Get
            Return ctrlList.MultiSelectHeader
        End Get
        Set(ByVal value As Boolean)
            ctrlList.MultiSelectHeader = value
        End Set
    End Property

    Public Property SearchKeys() As String
        Get
            Return ctrlList.SearchKeys
        End Get
        Set(ByVal value As String)
            ctrlList.SearchKeys = value
        End Set
    End Property

    Public ReadOnly Property SelectedRow() As UltraGridRow
        Get
            Return ctrlList.SelectedRow
        End Get
    End Property

    Public ReadOnly Property SelectedRows() As SelectedRowsCollection
        Get
            Return ctrlList.SelectedRows
        End Get
    End Property

    Public Property ShowDateRange() As Boolean
        Get
            Return ctrlDateRange.Visible
        End Get
        Set(ByVal value As Boolean)
            ctrlDateRange.Visible = value
        End Set
    End Property

    Public Property ShowGridButtons() As Boolean
        Get
            Return ctrlList.ShowButtons
        End Get
        Set(ByVal value As Boolean)
            ctrlList.ShowButtons = value
        End Set
    End Property

    Public Property ShowPrintButton() As Boolean
        Get
            Return btnPrint.Visible
        End Get
        Set(ByVal value As Boolean)
            btnPrint.Visible = value
        End Set
    End Property

    Public Property ShowSearch() As Boolean
        Get
            Return ctrlList.ShowSearch
        End Get
        Set(ByVal value As Boolean)
            ctrlList.ShowSearch = value
        End Set
    End Property

    Public Property ShowViewButton() As Boolean
        Get
            Return btnView.Visible
        End Get
        Set(ByVal value As Boolean)
            If Not value Then
                btnPrint.Location = New Point(btnRefresh.Location.X + 50, btnRefresh.Location.Y)
                btnRefresh.Location = New Point(btnView.Location.X + 50, btnView.Location.Y)
            End If
            btnView.Visible = value
        End Set
    End Property

    Public Property ShowSearchButton() As Boolean
        Get
            Return btnSearch.Visible
        End Get
        Set(ByVal value As Boolean)
            btnSearch.Visible = value
        End Set
    End Property

    Public Property UsedInBrowser() As Boolean
        Get
            Return ctrlList.UsedInBrowser
        End Get
        Set(ByVal value As Boolean)
            ctrlList.UsedInBrowser = value
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub ListWithDateRange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not DesignMode Then
                btnPrint.Visible = Current.Rights.IsAllowed(ModuleName, Rights.AccessRights.Print)
                btnView.Enabled = False
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Try
            RaiseEvent OnRefresh()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            RaiseEvent OnPrint()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            If ctrlList.ActiveRow Is Nothing Then
                ShowSelectMessage("view information")
            ElseIf ctrlList.ActiveRow.HasParent Then
                RaiseEvent OnView(ctrlList.ActiveRow.ParentRow)
            Else
                RaiseEvent OnView(ctrlList.ActiveRow)
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_AfterRowSelect(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.AfterRowSelect
        Try
            btnView.Enabled = True
            RaiseEvent AfterRowSelect(SelectedRow)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_CellClickButtonGrid(ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ctrlList.CellClickButtonGrid
        Try
            RaiseEvent CellClickButtonGrid(e)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            RaiseEvent InitializeGridLayout(e)
            btnPrint.Enabled = (Grid.Rows.Count > 0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridRow(ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles ctrlList.InitializeGridRow
        Try
            RaiseEvent InitializeGridRow(e)
            btnPrint.Enabled = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_OnCancel(ByRef CloseForm As Boolean) Handles ctrlList.OnCancel
        Try
            RaiseEvent OnCancel(CloseForm)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_OnOk(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.OnOk
        Try
            RaiseEvent OnOk(SelectedRow)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            RaiseEvent OnSearch()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_OnOkMultiSelect(ByVal SelectedRows As Infragistics.Win.UltraWinGrid.SelectedRowsCollection) Handles ctrlList.OnOkMultiSelect
        Try
            RaiseEvent OnOkMultiSelect(SelectedRows)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_RowDoubleClick(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.RowDoubleClick
        Try
            If Not ctrlList.UsedInBrowser Then
                btnView.PerformClick()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Sub Activate()
        Try
            ctrlList.Activate()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Clear()
        Try
            ctrlList.Clear()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LockGridRows()
        Try
            ctrlList.LockGridRows()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub UnlockGridRow(ByVal _RowIndex As Integer, Optional ByVal _ColumnKeys() As String = Nothing)
        Try
            ctrlList.UnlockGridRow(_RowIndex, _ColumnKeys)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
