Imports Infragistics.Win.UltraWinGrid

Friend Class FormSalesBrowser

#Region "Declarations"

    Private m_SalesList As SalesList
    Private m_SelectedRow As UltraGridRow

    Enum SaleTypes
        SalesForReturn
    End Enum

#End Region

#Region "Properties"

    Property SaleType As SaleTypes = SaleTypes.SalesForReturn

    ReadOnly Property SelectedRow As UltraGridRow
        Get
            Return m_SelectedRow
        End Get
    End Property

#End Region

#Region "Constructor"

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            m_SalesList = New SalesList
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                DialogResult = DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

    Private Sub ctrlList_OnOK(ByVal SelectedRow As UltraGridRow) Handles ctrlList.OnOk
        Try
            m_SelectedRow = SelectedRow
            DialogResult = DialogResult.OK
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns
                .Override.CellClickAction = CellClickAction.RowSelect

                With .Bands(0)
                    Select Case SaleType
                        Case SaleTypes.SalesForReturn
                            With .Columns("transaction_date")
                                .CellActivation = Activation.NoEdit
                                .Header.Caption = "Date"
                                .SetWidth(130)
                                .Style = ColumnStyle.DateTime
                            End With
                            With .Columns("code")
                                .Header.Caption = "Code"
                                .SetWidth(150)
                            End With
                            .Columns("customer_code").Hidden = True
                            .Columns("customer_name").Header.Caption = "Customer"
                            .Columns("invoice_code").Hidden = True
                            With .Columns("total_amount")
                                .Header.Caption = "Total Amount"
                                .SetWidth(100)
                            End With
                    End Select
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
            With ctrlList
                .SearchKeys = "code,customer_name"

                AcceptButton = .ButtonOK
                CancelButton = .ButtonCancel
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            m_SalesList.FillSalesForReturn(Today.AddDays(-7), Today.Date)
            ctrlList.DataSource = m_SalesList.Data
            Me.ActiveControl = ctrlList.Grid

            If ctrlList.Grid.Rows.Count > 0 Then
                ctrlList.Grid.Rows(0).Activate()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class