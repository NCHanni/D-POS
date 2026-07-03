Imports Infragistics.Win.UltraWinGrid

Friend Class FormSalesDetailsBrowser

#Region "Constructors"

    Public Sub New(ByVal saleCode As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_SalesCode = saleCode

    End Sub

#End Region

#Region "Declarations"

    Private m_SalesList As SalesList
    Private m_Data As DataTable
    Private m_SalesCode As String = ""

#End Region

#Region "Properties"

    Private m_SelectedItem As DataTable = Nothing
    Public ReadOnly Property SelectedItems() As DataTable
        Get
            Return m_SelectedItem
        End Get
    End Property

    Property References As String = ""

#End Region

#Region "Event Handlers"

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridRow(ByVal e As InitializeRowEventArgs) Handles ctrlList.InitializeGridRow
        Try
            If e.Row.Band.Index = 0 Then
                For Each itemCode In References.Split(",")
                    If e.Row.Cells("item_code").Value = itemCode Then
                        e.Row.Hidden = True
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_OnOkMultiSelect(SelectedRows As SelectedRowsCollection) Handles ctrlList.OnOkMultiSelect
        Try
            m_SelectedItem = m_Data.Clone

            Dim selectedRow As DataRow
            For Each row As UltraGridRow In SelectedRows
                selectedRow = m_SelectedItem.NewRow
                For Each col As DataColumn In m_SelectedItem.Columns
                    selectedRow(col.ColumnName) = row.Cells(col.ColumnName).Value
                Next
                m_SelectedItem.Rows.Add(selectedRow)
            Next

            If m_SelectedItem.Rows.Count = 0 Then
                ShowSelectMessage()
                ctrlList.CloseParentFormOnOK = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            With e.Layout
                With .Bands(0)
                    .Override.CellClickAction = CellClickAction.RowSelect

                    For Each col As UltraGridColumn In .Columns
                        With col
                            Select Case .Key
                                Case "selected"
                                Case "qty"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Total/Remaining Qty"
                                    .SetWidth(60)
                                Case "unit_of_measure"
                                    .Header.Caption = "UOM"
                                    .SetWidth(50, 80)
                                Case "description"
                                    .MinWidth = 150
                                Case "price"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .SetWidth(80)
                                Case "discount_percent"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .Header.Caption = "Discount (%)"
                                    .SetWidth(80)
                                Case "discounted_price"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .SetWidth(100)
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_SalesList = New SalesList

            With ctrlList
                .SearchKeys = "description"

                AcceptButton = .ButtonOK
                CancelButton = .ButtonCancel
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            m_SalesList.FillDetails(m_SalesCode)
            m_Data = m_SalesList.Data

            ctrlList.DataSource = m_Data
            ctrlList.Activate()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class