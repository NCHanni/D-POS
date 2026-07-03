Imports Infragistics.Win.UltraWinGrid

Friend Class FormItemBrowser

#Region "Declarations"

    Private m_Initialized As Boolean
    Private ReadOnly m_Settings As SettingsPreferences

    Public Sub New(settings As SettingsPreferences)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
    End Sub

#End Region

#Region "Properties"

    Property CategoryList As DataTable
    Property ItemList As DataTable
    Property IsCustomerScPwd As Boolean
    Property HasVATExclusiveItems As Boolean

    Private m_SelectedItem As DataTable
    Public ReadOnly Property SelectedItems() As DataTable
        Get
            Return m_SelectedItem
        End Get
    End Property

#End Region

#Region "Events"

    Public Event OK(ByVal _Data As DataTable)

#End Region

#Region "Event Handlers"

#Region "Form"

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

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.F2 Then
                With ctrlList
                    If .txtSearch.Focused Then
                        .grdList.Select()
                    Else
                        .txtSearch.Focus()
                        .txtSearch.SelectAll()
                    End If
                End With
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Others"

    Private Sub cmbCategories_ValueChanged(sender As Object, e As EventArgs) Handles cmbCategories.ValueChanged
        Try
            If m_Initialized Then
                ItemList.DefaultView.RowFilter = "ItemCategoryCode=" & cmbCategories.Text.Quote(False)
                ctrlList.DataSource = ItemList.DefaultView.ToTable()
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns
                .LoadStyle = LoadStyle.LoadOnDemand
                .Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
                .Override.CellClickAction = CellClickAction.RowSelect

                For Each col As UltraGridColumn In .Bands(0).Columns
                    With col
                        Select Case .Key
                            Case "BarCode", "codename"
                                .Header.Caption = "Code"
                                .SetWidth(90, 100)
                            Case "sku"
                                .Header.Caption = "SKU"
                                .SetWidth(80, 100)
                            Case "Description", "description"
                                .Header.Caption = "Description"
                                .MinWidth = 200
                            Case "UnitOfMeasure", "unit_of_measure"
                                .Header.Caption = "UOM"
                                .SetWidth(50, 60)
                            Case "UnitPrice", "price"
                                .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                .Format = "#,##0.00"
                                .Header.Caption = "Unit Price"
                                .SetWidth(90)
                                .Style = ColumnStyle.DoubleNonNegative
                            Case "PriceIncludesVAT"
                                .Hidden = Not HasVATExclusiveItems
                                .Header.Caption = "VAT Inclusive"
                                .Header.ToolTipText = "Price Includes VAT"
                                .Style = ColumnStyle.CheckBox
                                .SetWidth(75)
                            Case Else
                                .Hidden = True
                        End Select
                    End With
                Next
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_OnOK(ByVal SelectedRow As UltraGridRow) Handles ctrlList.OnOK
        Try
            Dim dt As DataTable = ItemList.Clone
            Dim dr As DataRow

            With SelectedRow
                dr = dt.NewRow
                dr("code") = .Cells("code").Value
                dr("codename") = .Cells("codename").Value
                dr("sku") = .Cells("sku").Value
                dr("description") = .Cells("description").Value
                dr("class_code") = .Cells("class_code").Value
                dr("unit_of_measure") = .Cells("unit_of_measure").Value
                dr("price") = .Cells("price").Value
                dr("vat_percent") = .Cells("vat_percent").Value
                dr("vat_amount") = .Cells("vat_amount").Value
                dr("discount_group") = .Cells("discount_group").Value
                dr("discount_percent") = If(IsCustomerScPwd, .Cells("discount_percent").Value, 0.0)
                dr("is_vat") = .Cells("is_vat").Value
                dr("is_zero_rated") = .Cells("is_zero_rated").Value
                dr("is_vat_exempt") = If(IsCustomerScPwd, .Cells("is_vat_exempt").Value, False)
                dr("is_gift_certificate") = .Cells("is_gift_certificate").Value
                dr("is_lot") = .Cells("is_lot").Value
                dr("is_serial") = .Cells("is_serial").Value
                dt.Rows.Add(dr)
            End With

            If dt.Rows.Count = 0 Then
                ShowSelectMessage()
                Return
            End If

            m_SelectedItem = dt.Copy
            RaiseEvent OK(dt)

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            grpCategoryList.Visible = False
            Dim dt As New DataTable
            With dt.Columns
                .Add("codename")
                .Add("sku")
                .Add("sku_gh")
                .Add("description")
                .Add("unit_of_measure")
                .Add("price", GetType(Double))
            End With
            ctrlList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            With ctrlList.grdList
                .DataSource = ItemList

                ctrlList.SearchKeys = "codename,sku,description"

                If .Rows.Count > 0 Then
                    .ActiveRow = ctrlList.grdList.Rows(0)
                    ctrlList.txtSearch.Focus()
                End If
            End With
            m_Initialized = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class