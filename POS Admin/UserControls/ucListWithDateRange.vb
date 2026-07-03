Imports Infragistics.Win.UltraWinGrid

<System.ComponentModel.DefaultEvent("InitializeGridLayout")> _
Public Class ucListWithDateRange

#Region "Events"

    Public Event InitializeGridLayout(ByVal e As InitializeLayoutEventArgs)
    Public Event InitializeGridRowLayout(ByVal e As InitializeRowEventArgs)
    Public Event CellClickButtonGrid(ByVal e As CellEventArgs)
    Public Event AfterRowSelect(ByVal SelectedRow As UltraGridRow)
    Public Event OnRefresh()
    Public Event OnSearch()
    Public Event OnView(ByVal SelectedRow As UltraGridRow)
    Public Event OnOK(ByVal sender As ucListWithDateRange, ByVal SelectedRow As UltraGridRow)
    Public Event OnCancel(ByRef CloseForm As Boolean)

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

    Public ReadOnly Property ButtonOK() As Infragistics.Win.Misc.UltraButton
        Get
            Return ctrlList.ButtonOK
        End Get
    End Property

    Public ReadOnly Property ButtonCancel() As Infragistics.Win.Misc.UltraButton
        Get
            Return ctrlList.ButtonCancel
        End Get
    End Property

    Public ReadOnly Property Grid() As UltraGrid
        Get
            Return ctrlList.Grid
        End Get
    End Property

    Public Property SearchKeys() As String
        Get
            Return ctrlList.SearchKeys
        End Get
        Set(ByVal value As String)
            ctrlList.SearchKeys = value
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

    Public Property ShowSearch() As Boolean
        Get
            Return ctrlList.ShowSearch
        End Get
        Set(ByVal value As Boolean)
            ctrlList.ShowSearch = value
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

    Public ReadOnly Property StartDate() As Date
        Get
            Return ctrlDateRange.StartDate
        End Get
    End Property

    Public ReadOnly Property EndDate() As Date
        Get
            Return ctrlDateRange.EndDate
        End Get
    End Property

    Public Property UsedInBrowser() As Boolean
        Get
            Return ctrlList.UsedInBrowser
        End Get
        Set(ByVal value As Boolean)
            ctrlList.UsedInBrowser = value
        End Set
    End Property

    Public Property HasSearch() As Boolean
        Get
            Return btnSearch.Visible
        End Get
        Set(ByVal value As Boolean)
            btnSearch.Visible = value
        End Set
    End Property

    Public Property HasView() As Boolean
        Get
            Return btnView.Visible
        End Get
        Set(ByVal value As Boolean)
            If Not value Then
                btnRefresh.Location = New Point(btnView.Location.X + 50, btnView.Location.Y)
            End If
            btnView.Visible = value
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Try
            RaiseEvent OnRefresh()
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
            RaiseEvent AfterRowSelect(SelectedRow)
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

    Private Sub ctrlList_OnOK(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.OnOK
        Try
            RaiseEvent OnOK(Me, SelectedRow)
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

    Private Sub ctrlList_RowDoubleClick(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.RowDoubleClick
        Try
            If Not ctrlList.UsedInBrowser Then
                btnView.PerformClick()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            RaiseEvent InitializeGridLayout(e)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridRow(ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles ctrlList.InitializeGridRow
        Try
            RaiseEvent InitializeGridRowLayout(e)
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

    Public Sub Initialize()
        Try
            ctrlDateRange.Clear()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearSearch()
        Try
            ctrlList.Clear()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
