Imports Infragistics.Win.UltraWinGrid

Public Class FormRole

#Region "Constructor"

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ctrlMenu.ModuleName = "Roles"
    End Sub

#End Region

#Region "Declarations"

    Private m_SecurityRole As Role
    Private m_SecurityRoleList As RoleList

    Private m_Operation As String = ""
    Private m_Code As String = ""
    Private m_IsSuperRole As Boolean
    Private m_OnSelectChange As Boolean
    Private m_OnCellChange As Boolean
    Private m_SkipCellChange As Boolean

    Private m_CheckFormChanges As CheckFormChanges

    Private m_RoleList As DataTable
    Private m_ModuleSettings As DataSet
    Private m_ModuleSettingsBusy As Boolean

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_CheckFormChanges.HasChanges And m_Operation <> "" Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.Escape AndAlso e.Modifiers = Keys.None Then
                Me.Close()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Menu"

    Private Sub ctrlMenu_MenuClick(ByVal State As MenuButtons.MenuButtons) Handles ctrlMenu.MenuClick
        Try
            Select Case State
                Case MenuButtons.MenuButtons.New : NewSecurityRole()
                Case MenuButtons.MenuButtons.Edit : EditSecurityRole()
                Case MenuButtons.MenuButtons.Delete : DeleteSecurityRole()
                Case MenuButtons.MenuButtons.Save : SaveSecurityRole()
                Case MenuButtons.MenuButtons.Cancel : CancelSecurityRole()
            End Select
        Catch ex As Exception
            ctrlMenu.ErrorOccured = True
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdModuleSettings_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles grdModuleSettings.AfterSelectChange
        m_OnSelectChange = False
    End Sub

    Private Sub grdModuleSettings_BeforeSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventArgs) Handles grdModuleSettings.BeforeSelectChange
        m_OnSelectChange = True
    End Sub

    Private Sub grdModuleSettings_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles grdModuleSettings.CellChange
        Dim trackerEnabled As Boolean = m_CheckFormChanges.Enabled
        m_CheckFormChanges.Enabled = False

        Try
            If m_SkipCellChange Then
                Return
            Else
                m_OnCellChange = True
            End If

            Dim hasAccess As String = e.Cell.Row.Cells("has_access").Value.ToString
            Dim hasAdd As Boolean = (hasAccess.Substring(0, 1) = "1")
            Dim hasEdit As Boolean = (hasAccess.Substring(1, 1) = "1")
            Dim hasDelete As Boolean = (hasAccess.Substring(2, 1) = "1")
            Dim hasPrint As Boolean = (hasAccess.Substring(3, 1) = "1")

            If e.Cell.Column.Key = "can_access" Then
                With e.Cell.Row.Cells
                    .Item("can_view").Value = e.Cell.Text
                    If hasAdd Then .Item("can_add").Value = e.Cell.Text
                    If hasEdit Then .Item("can_edit").Value = e.Cell.Text
                    If hasDelete Then .Item("can_delete").Value = e.Cell.Text
                    If hasPrint Then .Item("can_print").Value = e.Cell.Text
                End With
            Else
                With e.Cell.Row.Cells
                    If e.Cell.Text = True Then
                        .Item("can_access").Value = True
                    End If

                    If e.Cell.Column.Key = "can_view" Then
                        '

                    ElseIf e.Cell.Column.Key = "can_add" And hasAdd Then
                        If e.Cell.Text = True And .Item("can_access").Text = True Then
                            .Item("can_view").Value = True
                        End If

                    ElseIf e.Cell.Column.Key = "can_edit" And hasEdit Then
                        If e.Cell.Text = True And .Item("can_access").Text = True Then
                            .Item("can_view").Value = True
                        End If

                    ElseIf e.Cell.Column.Key = "can_delete" And hasDelete Then
                        If e.Cell.Text = True And .Item("can_access").Text = True Then
                            .Item("can_view").Value = True
                        End If
                    End If

                    If .Item("can_view").Text = False Then
                        If hasAdd Then
                            hasAdd = (.Item("can_add").Text)
                        End If
                        If Not hasAdd Then
                            If hasEdit Then
                                hasEdit = (.Item("can_edit").Text)
                            End If
                            If Not hasEdit Then
                                If hasDelete Then
                                    hasDelete = (.Item("can_delete").Text)
                                End If
                                If Not hasDelete Then
                                    If hasPrint Then
                                        hasPrint = (.Item("can_print").Text)
                                    End If
                                    If Not hasPrint Then
                                        e.Cell.Row.Cells("can_access").Value = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                End With
            End If

            m_OnCellChange = False
        Catch ex As Exception
            m_OnCellChange = False
            ex.ShowError()
        Finally
            m_CheckFormChanges.Enabled = trackerEnabled
        End Try
    End Sub

    Private Sub grdModuleSettings_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdModuleSettings.InitializeLayout
        Try
            With e.Layout
                .Override.HeaderClickAction = HeaderClickAction.Select
                .Override.ExpansionIndicator = ShowExpansionIndicator.Never
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns

                With .Bands(0)
                    With .Columns("group")
                        .CellActivation = Activation.NoEdit
                        .CellClickAction = CellClickAction.RowSelect
                    End With
                    .ColHeadersVisible = False
                End With

                If m_IsSuperRole Then
                    .Bands(1).Hidden = True
                Else
                    With .Bands(1)
                        .Hidden = False

                        .Columns("id").Hidden = True
                        .Columns("group").Hidden = True
                        .Columns("role_code").Hidden = True
                        .Columns("module_id").Hidden = True
                        .Columns("has_access").Hidden = True

                        .Columns("module_name").Header.Caption = "Module"
                        .Columns("module_name").CellClickAction = CellClickAction.RowSelect
                        .Columns("module_name").CellActivation = Activation.NoEdit

                        .Columns("can_access").Header.Caption = "Access"
                        .Columns("can_view").Header.Caption = "View"
                        .Columns("can_add").Header.Caption = "Add"
                        .Columns("can_edit").Header.Caption = "Edit"
                        .Columns("can_delete").Header.Caption = "Delete"
                        .Columns("can_print").Header.Caption = "Print"

                        .Columns("can_access").MaxWidth = 50
                        .Columns("can_view").MaxWidth = 50
                        .Columns("can_add").MaxWidth = 50
                        .Columns("can_edit").MaxWidth = 50
                        .Columns("can_delete").MaxWidth = 50
                        .Columns("can_print").MaxWidth = 50

                        .Columns("can_access").MinWidth = 50
                        .Columns("can_view").MinWidth = 50
                        .Columns("can_add").MinWidth = 50
                        .Columns("can_edit").MinWidth = 50
                        .Columns("can_delete").MinWidth = 50
                        .Columns("can_print").MinWidth = 50

                        .Columns("can_access").Style = ColumnStyle.CheckBox
                        .Columns("can_add").Style = ColumnStyle.CheckBox
                        .Columns("can_edit").Style = ColumnStyle.CheckBox
                        .Columns("can_delete").Style = ColumnStyle.CheckBox
                        .Columns("can_view").Style = ColumnStyle.CheckBox
                        .Columns("can_print").Style = ColumnStyle.CheckBox
                    End With
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdModuleSettings_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles grdModuleSettings.InitializeRow
        If m_OnCellChange OrElse m_OnSelectChange Then
            Return
        Else
            m_SkipCellChange = True
        End If

        Dim trackerEnabled As Boolean = m_CheckFormChanges.Enabled
        m_CheckFormChanges.Enabled = False

        Try
            With e.Row
                If .Band.Index = 1 Then
                    Select Case .Cells("module_name").Value
                        Case "Credit Cards", "Items", "Discount Groups", "Gift Certificates", "Entity Details"
                        Case "NAV Services"
                            .Hidden = True
                    End Select

                    If .Hidden Then
                        Return
                    End If

                    Dim hasAccess As String = .Cells("has_access").Value.ToString
                    Dim hasAdd As Boolean = (hasAccess.Substring(0, 1) = "1")
                    Dim hasEdit As Boolean = (hasAccess.Substring(1, 1) = "1")
                    Dim hasDelete As Boolean = (hasAccess.Substring(2, 1) = "1")
                    Dim hasPrint As Boolean = (hasAccess.Substring(3, 1) = "1")

                    If hasAdd Then
                        .Cells("can_add").Activation = Activation.AllowEdit
                    Else
                        .Cells("can_add").Activation = Activation.Disabled
                        .Cells("can_add").Value = Nothing
                    End If

                    If hasEdit Then
                        .Cells("can_edit").Activation = Activation.AllowEdit
                    Else
                        .Cells("can_edit").Activation = Activation.Disabled
                        .Cells("can_edit").Value = Nothing
                    End If

                    If hasDelete Then
                        .Cells("can_delete").Activation = Activation.AllowEdit
                    Else
                        .Cells("can_delete").Activation = Activation.Disabled
                        .Cells("can_delete").Value = Nothing
                    End If

                    If hasPrint Then
                        .Cells("can_print").Activation = Activation.AllowEdit
                    Else
                        .Cells("can_print").Activation = Activation.Disabled
                        .Cells("can_print").Value = Nothing
                    End If
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        Finally
            m_CheckFormChanges.Enabled = trackerEnabled
        End Try

        m_SkipCellChange = False
    End Sub

    Private Sub grdList_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles grdList.AfterSelectChange
        Try
            FillInformation(m_Code)
            EnableRoleInfo(False)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_BeforeSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventArgs) Handles grdList.BeforeSelectChange
        Try
            If grdList.ActiveRow Is Nothing Then
                e.Cancel = True
                Return
            End If
            If m_Operation <> "" Then
                If ShowSelectQuestion() = QuestionResult.No Then
                    e.Cancel = True
                    Return
                Else
                    m_Operation = ""
                End If
            End If
            m_Code = grdList.ActiveRow.Cells("code").Value
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                With .Bands(0)
                    .Columns("code").Hidden = True
                    .Columns("name").Header.Caption = "Name"
                    .Columns("description").Header.Caption = "Description"
                    .Columns("is_super_role").Hidden = True
                End With
                .Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                .AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeRow(sender As Object, e As InitializeRowEventArgs) Handles grdList.InitializeRow
        Try
            If CBool(e.Row.Cells("is_super_role").Value) Then
                e.Row.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
            Else
                e.Row.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.Default
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCheckAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckAll.Click
        Try
            CheckModules()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnUncheckAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUncheckAll.Click
        Try
            CheckModules(False)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "GroupBox"

    Private Sub grpModuleSettings_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grpModuleSettings.EnabledChanged
        Try
            If m_ModuleSettingsBusy Then
                Return
            Else
                m_ModuleSettingsBusy = True
            End If

            With grpModuleSettings
                If .Enabled Then
                    grdModuleSettings.DisplayLayout.Bands(1).Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Default
                ElseIf grdModuleSettings.DataSource IsNot Nothing Then
                    grdModuleSettings.DisplayLayout.Bands(1).Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                End If

                .Enabled = True
            End With

            m_ModuleSettingsBusy = False
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_SecurityRole = New Role
            m_SecurityRoleList = New RoleList

            RefreshRoleList()

            m_CheckFormChanges = New CheckFormChanges
            m_CheckFormChanges.Add(grpSecurityRoleInformation)
            m_CheckFormChanges.Add(grpModuleSettings)

            ctrlMenu.SetToDefault()
            EnableRoleInfo(False)
            Clear()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshRoleList()
        Try
            With m_SecurityRoleList
                .Fill(False)
                m_RoleList = .List
                grdList.DataSource = m_RoleList
            End With

            If m_Code <> "" Then
                For Each row As UltraGridRow In grdList.Rows
                    If row.Cells("code").Value = m_Code Then
                        row.Activate()
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NewSecurityRole()
        Try
            grdList.ActiveRow = Nothing
            Clear(True)
            EnableRoleInfo(True)
            m_Operation = "INSERT"
            txtName.Focus()
            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EditSecurityRole()
        Try
            EnableRoleInfo(True)
            m_Operation = "UPDATE"
            txtName.Focus()
            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveSecurityRole()
        Try
            If Not IsValid() Then
                ctrlMenu.ErrorOccured = True
                Exit Sub
            End If

            ObjectToClass()

            With m_SecurityRole
                If .SaveRole() Then
                    If m_Operation = "INSERT" Then
                        ShowSaveMessage()
                        m_Code = .Code
                    Else
                        ShowUpdateMessage()
                        If .Code = Current.User.RoleCode Then
                            ShowMessage("You have modified the user role you are currently using.\n\nChanges will be applied on next login.", "Information")
                        End If
                    End If

                    m_Operation = ""
                    RefreshRoleList()

                    m_CheckFormChanges.ClearChanges()
                End If
            End With

            EnableRoleInfo(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DeleteSecurityRole()
        Try
            If m_SecurityRoleList.IsReferenced(m_Code) Then
                ShowMessage("Record is already being referenced.\n\nDelete not permitted!", "Delete Failed", MessageIcon.Exclamation)
                ctrlMenu.ErrorOccured = True
                Exit Sub
            End If

            If ShowDeleteQuestion() = QuestionResult.Yes Then
                m_SecurityRole.Operation = "DELETE"
                m_SecurityRole.SecurityModules = m_ModuleSettings
                m_SecurityRole.SaveRole()

                ShowDeleteMessage()

                Clear()
                RefreshRoleList()
                EnableRoleInfo(False)
            Else
                ctrlMenu.ErrorOccured = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelSecurityRole()
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
            End If

            m_Operation = ""
            EnableRoleInfo(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ObjectToClass()
        Try
            With Me.m_SecurityRole
                .Code = m_Code
                .Name = txtName.Text
                .Description = txtDescription.Text
                .Operation = m_Operation
                .SecurityModules = m_ModuleSettings
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Clear(Optional ByVal prepareModuleSettings As Boolean = False)
        Try
            txtName.Clear()
            txtDescription.Clear()

            grdList.ActiveRow = Nothing

            m_Code = ""
            m_Operation = ""
            m_IsSuperRole = False
            m_SecurityRole.Code = ""

            If prepareModuleSettings Then
                m_SecurityRoleList.FillDetails("")
                m_ModuleSettings = m_SecurityRoleList.ModuleList

                With grdModuleSettings
                    .DataSource = Nothing
                    .DataSource = m_ModuleSettings
                    .Rows.ExpandAll(True)
                End With

                lblMessage.Visible = False
            Else
                grdModuleSettings.DataSource = Nothing
                lblMessage.Visible = True
            End If

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            ClearInvalidFields()

            grpSecurityRoleInformation.GetErrors()

            If m_SecurityRoleList.IsUserRoleExisting(txtName.Text, m_Code) Then
                AddToInvalidFields("Name already exists")
            End If

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields)
            End If

            Return Not HasInvalidFields()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub FillInformation(ByVal code As String)
        Try
            Application.UseWaitCursor = True

            With m_RoleList.Select("code = " & code.Quote(False)).First
                m_Code = .Item("code")
                m_SecurityRole.Code = m_Code
                m_IsSuperRole = CBool(.Item("is_super_role"))
                txtName.Text = .Item("name")
                txtDescription.Text = .Item("description")

                m_SecurityRole.Name = txtName.Text
                m_SecurityRole.Description = txtDescription.Text
            End With

            m_SecurityRoleList.FillDetails(m_Code)
            m_ModuleSettings = m_SecurityRoleList.ModuleList

            lblMessage.Visible = False

            With grdModuleSettings
                .BeginUpdate()
                .DataSource = m_ModuleSettings
                If Not m_IsSuperRole Then
                    .Rows.ExpandAll(True)
                End If
                .EndUpdate()
            End With

            m_CheckFormChanges.ClearChanges()
            Application.UseWaitCursor = False
        Catch ex As Exception
            Application.UseWaitCursor = False
            Throw
        End Try
    End Sub

    Private Sub CheckModules(Optional ByVal checkAll As Boolean = True)
        Try
            Application.UseWaitCursor = True

            For Each row As DataRow In m_ModuleSettings.Tables(1).Rows
                Dim hasAccess As String = row("has_access")
                Dim hasAdd As Boolean = (hasAccess.Substring(0, 1) = "1")
                Dim hasEdit As Boolean = (hasAccess.Substring(1, 1) = "1")
                Dim hasDelete As Boolean = (hasAccess.Substring(2, 1) = "1")
                Dim hasPrint As Boolean = (hasAccess.Substring(3, 1) = "1")

                row("can_access") = checkAll
                row("can_view") = checkAll

                If hasAdd Then row("can_add") = checkAll
                If hasEdit Then row("can_edit") = checkAll
                If hasDelete Then row("can_delete") = checkAll
                If hasPrint Then row("can_print") = checkAll
            Next

            grdModuleSettings.Refresh()
            Application.UseWaitCursor = False
        Catch ex As Exception
            Application.UseWaitCursor = False
            Throw
        End Try
    End Sub

    Private Sub EnableRoleInfo(ByVal enable As Boolean)
        grpSecurityRoleInformation.Enabled = enable
        grpModuleSettings.Enabled = enable

        If enable Then
            grpModuleSettings_EnabledChanged(Nothing, Nothing)
        End If

        If m_IsSuperRole Then
            lblSuperRoleNote.Visible = True
            ctrlMenu.SetMenuState(MenuButtons.MenuStates.Default)
        Else
            lblSuperRoleNote.Visible = False
            ctrlMenu.SetMenuState(MenuButtons.MenuStates.Select)
        End If

        btnCheckAll.Enabled = enable
        btnUncheckAll.Enabled = enable
    End Sub

#End Region

End Class