Friend Class FormItem

#Region "Constructor"

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ctrlMenu.ModuleName = "Items"
        ctrlMenu.ModuleGroup = "Libraries"
    End Sub

#End Region

#Region "Declarations"

    Private m_Item As Item
    Private m_ItemList As ItemList
    Private m_List As DataTable

    Private m_Operation As String
    Private m_Code As String = ""

    Private m_CheckFormChanges As CheckFormChanges

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If m_CheckFormChanges.HasChanges Then
            If ShowCancelQuestion() = QuestionResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

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
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Menu"

    Private Sub ctrlMenu_MenuClick(ByVal State As MenuButtons.MenuButtons) Handles ctrlMenu.MenuClick
        Try
            Select Case State
                Case MenuButtons.MenuButtons.New : NewItem()
                Case MenuButtons.MenuButtons.Edit : EditItem()
                Case MenuButtons.MenuButtons.Delete : DeleteItem()
                Case MenuButtons.MenuButtons.Save : SaveItem()
                Case MenuButtons.MenuButtons.Cancel : CancelItem()
            End Select
        Catch ex As Exception
            ctrlMenu.ErrorOccured = True
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "User Control"

    Private Sub ctrlList_RowDoubleClick(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.RowDoubleClick
        Try
            btnViewInfo.PerformClick()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            With e.Layout
                .AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns

                With .Bands(0)
                    .Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect

                    .Columns("code").Hidden = True
                    .Columns("specifications").Hidden = True
                    .Columns("discount_group").Hidden = True
                    .Columns("base_price").Hidden = True

                    With .Columns("codename")
                        .Header.Caption = "Code"
                        .SetWidth(75)
                    End With
                    With .Columns("sku")
                        .Header.Caption = "SKU"
                        .Header.ToolTipText = "Barcode (Alternate Id)"
                        .SetWidth(90)
                    End With
                    With .Columns("description")
                        .Header.Caption = "Description"
                        .MinWidth = 160
                    End With
                    With .Columns("unit_of_measure")
                        .Header.Caption = "UOM"
                        .SetWidth(50, 60)
                    End With
                    With .Columns("price")
                        .Header.Caption = "Price"
                        .SetWidth(70, 90)
                    End With
                    With .Columns("is_vat")
                        .Header.Caption = "VAT"
                        .Header.ToolTipText = "Vatable"
                        .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                        .SetWidth(60)
                    End With
                    With .Columns("is_zero_rated")
                        .Header.Caption = "0-Rated"
                        .Header.ToolTipText = "Zero-rated"
                        .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                        .SetWidth(60)
                    End With
                    With .Columns("is_senior_pwd")
                        .Header.Caption = "SC/PWD"
                        .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                        .SetWidth(60)
                    End With
                    With .Columns("is_vat_exempt")
                        .Header.Caption = "VAT Ex"
                        .Header.ToolTipText = "VAT Exempt"
                        .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                        .SetWidth(60)
                    End With
                    With .Columns("is_lot")
                        .Header.Caption = "Lot"
                        .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                        .SetWidth(60)
                    End With
                    With .Columns("is_serial")
                        .Header.Caption = "Serial"
                        .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                        .SetWidth(60)
                    End With
                    With .Columns("discount_group_desc")
                        .Header.Caption = "Disc. Group"
                        .Header.ToolTipText = "Discount Group"
                        .SetWidth(80, 100)
                    End With
                    With .Columns("is_active")
                        If chkShowInactive.Checked Then
                            .Header.Caption = "Active"
                            .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                            .SetWidth(60)
                        Else
                            .Hidden = True
                        End If
                    End With
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "CheckBox"

    Private Sub chkShowInactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowInactive.CheckedChanged
        Try
            btnRefresh.PerformClick()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub chkIsVATable_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsVATable.CheckedChanged
        lblPriceIncludesVAT.Visible = chkIsVATable.Checked
        If chkIsVATable.Checked AndAlso chkIsZeroRated.Checked Then
            chkIsZeroRated.Checked = False
        End If
    End Sub

    Private Sub chkZeroRated_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsZeroRated.CheckedChanged
        If chkIsZeroRated.Checked AndAlso chkIsVATable.Checked Then
            chkIsVATable.Checked = False
        End If
    End Sub

    Private Sub chkScPwdDiscount_CheckedChanged(sender As Object, e As EventArgs) Handles chkScPwd.CheckedChanged
        Try
            cmbDiscountGroup.Enabled = chkScPwd.Checked
            If Not chkScPwd.Checked Then
                cmbDiscountGroup.SelectedIndex = -1
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Try
            RefreshList()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewInfo.Click
        Try
            If ctrlList.ActiveRow Is Nothing Then
                ShowSelectMessage("view information")
            Else
                If m_Operation <> "" Then
                    If ShowSelectQuestion() = QuestionResult.No Then
                        Return
                    Else
                        m_Operation = ""
                    End If
                End If

                Dim binString As String = ""

                With ctrlList.ActiveRow
                    m_Code = .Cells("code").Value
                    FillInformation(m_Code)
                End With

                ctrlMenu.SetMenuState(BusinessPro.Core.MenuButtons.MenuStates.Select)

                tabItems.Tabs(1).Selected = True
                grpItem.Enabled = False
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Tab"

    Private Sub tabItems_SelectedTabChanged(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles tabItems.SelectedTabChanged
        If sender.SelectedTab.Index = 1 Then
            Me.CancelButton = ctrlMenu.ButtonCancel
        Else
            Me.CancelButton = ctrlList.ButtonCancel
        End If
    End Sub

#End Region

#End Region

#Region "Methods"

#Region "Menu Methods"

    Private Sub NewItem()
        Try
            Clear()
            grpItem.Enabled = True
            chkIsActive.Checked = True
            m_Operation = "INSERT"
            txtCodeName.Focus()
            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EditItem()
        Try
            If m_Code = "" Then
                ShowSelectMessage("edit")
                ctrlMenu.ErrorOccured = True
                Exit Sub
            End If

            m_Operation = "UPDATE"
            grpItem.Enabled = True
            txtCodeName.Focus()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DeleteItem()
        Try
            If m_ItemList.IsReferenced(m_Code) Then
                ShowDeleteNotPossibleMessage()
                ctrlMenu.ErrorOccured = True
                Return
            End If

            If ShowDeleteQuestion() = QuestionResult.Yes Then
                m_Operation = "DELETE"
                With m_Item
                    .Code = m_Code
                    .EmployeeCode = Core.Current.User.Code
                    .Operation = m_Operation
                    If .SaveItem Then
                        ShowDeleteMessage()
                        Clear()
                        RefreshList()
                        grpItem.Enabled = False
                    End If
                End With
            Else
                ctrlMenu.ErrorOccured = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveItem()
        Try
            If Not IsValid() Then
                ctrlMenu.ErrorOccured = True
                Return
            End If

            With m_Item
                .Code = m_Code
                .CodeName = txtCodeName.Text
                .SKU = txtSKU.Text
                .Description = txtDescription.Text
                .Specifications = txtSpecifications.Text
                .UnitOfMeasure = cmbUnitOfMeasure.Value
                .Price = numPrice.Value
                .IsVAT = chkIsVATable.Checked
                .IsZeroRated = chkIsZeroRated.Checked
                .IsSeniorPwd = chkScPwd.Checked
                .DiscountGroup = Core.Extensions.IfNothing(cmbDiscountGroup.Value, "")
                .IsActive = chkIsActive.Checked
                .EmployeeCode = Core.Current.User.Code
                .Operation = m_Operation

                If .SaveItem Then
                    If m_Operation = "INSERT" Then
                        ShowSaveMessage()
                    Else
                        ShowUpdateMessage()
                    End If

                    RefreshList()
                    grpItem.Enabled = False
                    m_CheckFormChanges.ClearChanges()
                    m_Operation = ""
                    m_Code = .Code
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelItem()
        Try
            If m_CheckFormChanges.HasChanges Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    ctrlMenu.ErrorOccured = True
                    Return
                End If
            End If

            If m_Operation = "INSERT" Then
                Clear()
            Else
                FillInformation(m_Code)
                m_Operation = ""
            End If

            grpItem.Enabled = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Other Methods"

    Private Sub Initialize()
        Try
            m_Item = New Item
            m_ItemList = New ItemList

            ctrlList.SearchKeys = "codename,sku,description"
            grpItem.Enabled = False

            PopulateUOM()
            PopulateDiscountGroup()

            m_CheckFormChanges = New CheckFormChanges
            m_CheckFormChanges.Add(grpItem)

            InitializeGrid()

            ctrlList.Activate()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeGrid()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("code")
                .Add("codename")
                .Add("sku")
                .Add("description")
                .Add("specifications")
                .Add("unit_of_measure")
                .Add("base_price", GetType(Double))
                .Add("price", GetType(Double))
                .Add("is_vat", GetType(Boolean))
                .Add("is_zero_rated", GetType(Boolean))
                .Add("is_senior_pwd", GetType(Boolean))
                .Add("is_vat_exempt", GetType(Boolean))
                .Add("is_lot", GetType(Boolean))
                .Add("is_serial", GetType(Boolean))
                .Add("discount_group")
                .Add("discount_group_desc")
                .Add("is_active", GetType(Boolean))
            End With
            ctrlList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillInformation(ByVal code As String)
        Try
            Dim row As DataRow = m_List.Select("code = '" & code & "'").First

            txtCodeName.Text = row("codename")
            txtDescription.Text = row("description")
            txtSKU.Text = CStrEx(row("sku"))
            txtSpecifications.Text = row("specifications")
            cmbUnitOfMeasure.Value = row("unit_of_measure")
            numPrice.Value = row("price")
            chkIsVATable.Checked = CBool(row("is_vat"))
            chkIsZeroRated.Checked = CBool(row("is_zero_rated"))
            chkScPwd.Checked = CBool(row("is_senior_pwd"))
            cmbDiscountGroup.Value = row("discount_group")
            cmbDiscountGroup.Enabled = chkScPwd.Checked
            chkIsActive.Checked = CBool(row("is_active"))

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            Dim th As New Threading.Thread(AddressOf RefreshData)
            Dim wait As New BusinessPro.Core.Wait(th)

            If wait.Show = BusinessPro.Core.Wait.DialogResult.OK Then
                With ctrlList
                    .DataSource = Nothing
                    .DataSource = m_List
                    .Activate()
                    .Grid.DisplayLayout.Bands(0).Columns("codename").SortIndicator = 1 ' Ascending
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshData()
        Try
            m_ItemList.Fill(chkShowInactive.Checked)
            m_List = m_ItemList.Data
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PopulateUOM()
        Try
            Dim dt As DataTable

            With New UnitOfMeasureList
                .Fill()
                dt = .Data
            End With

            With cmbUnitOfMeasure
                .DataSource = dt
                .ValueMember = "code"
                .DisplayMember = "name"
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PopulateDiscountGroup()
        Try
            Dim dt As DataTable

            With New DiscountGroupsList
                dt = .GetList()
            End With

            With cmbDiscountGroup
                .DataSource = dt
                .ValueMember = "code"
                .DisplayMember = "description"
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            ClearInvalidFields()

            If m_ItemList.IsItemExisting(txtCodeName.Text, m_Code) Then
                AddToInvalidFields("Code has already been used")
            End If

            If chkScPwd.Checked Then
                If cmbDiscountGroup.Value Is Nothing Then
                    AddToInvalidFields("Discount Group is required")
                End If
            End If

            Return Not grpItem.HasErrors(, True)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub Clear()
        Try
            Core.Extensions.Clean(grpItem)
            m_CheckFormChanges.ClearChanges()
            m_Operation = ""
            m_Code = ""
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#End Region

End Class