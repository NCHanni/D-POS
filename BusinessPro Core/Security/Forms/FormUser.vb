Imports Infragistics.Win.UltraWinGrid

Public Class FormUser

#Region "Constructor"

    Public Sub New(ByVal isBrowser As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ctrlMenu.ModuleName = "Users"
        m_IsBrowser = isBrowser
    End Sub

#End Region

#Region "Declarations"

    Private m_SecurityUser As User
    Private m_SecurityUserList As UserList
    Private m_CurrentPassword As String = ""
    Private m_RaiseOk As Boolean = False
    Private m_SelectedRow As UltraGridRow = Nothing

    Private m_IsSuperUser As Boolean
    Private m_CurrentUserName As String = ""
    Private m_IsNew As Boolean = False
    Private m_Operation As String = ""
    Private ReadOnly m_IsBrowser As Boolean = True

    Private m_CheckFormChanges As CheckFormChanges

#End Region

#Region "Event"

    Public Event Ok(ByVal _Code As String, ByVal _Name As String)

#End Region

#Region "Properties"

    Private m_Code As String
    Public ReadOnly Property Code() As String
        Get
            Return m_Code
        End Get
    End Property

    Public UseBarcodeLogin As Boolean

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            If m_RaiseOk Then
                With m_SelectedRow
                    m_Code = .Cells("code").Value
                    RaiseEvent Ok(.Cells("code").Value, .Cells("last_name").Value & ", " & .Cells("first_name").Value & " " & .Cells("middle_name").Value)
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_CheckFormChanges.HasChanges Then
                If ShowCancelQuestion() <> QuestionResult.Yes Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape AndAlso tabSystemUsers.SelectedTab.Index = 1 Then
            Close()
        End If
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            If ctrlList.grdList.ActiveRow Is Nothing Then
                ShowSelectMessage("view information")
            Else
                If m_Operation <> "" Then
                    If ShowSelectQuestion() = QuestionResult.No Then
                        Return
                    Else
                        m_Operation = ""
                    End If
                End If

                m_SelectedRow = ctrlList.ActiveRow

                With m_SelectedRow
                    m_Code = .Cells("code").Value
                    m_SecurityUser.Code = m_Code

                    FillInformation(m_Code)
                    ctrlMenu.AllowDelete = Not m_IsSuperUser
                    ctrlMenu.SetMenuState(MenuButtons.MenuStates.Select)

                    tabSystemUsers.Tabs(1).Selected = True
                    SetGroupBox(False)
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Others"

    Private Sub txtLoginBarcode_Leave(sender As Object, e As EventArgs) Handles txtLoginBarcode.Leave
        Try
            txtLoginBarcode.Text = txtLoginBarcode.Text.Trim.ToUpper
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub ctrlMenu_MenuClick(ByVal State As MenuButtons.MenuButtons) Handles ctrlMenu.MenuClick
        Try
            Select Case State
                Case MenuButtons.MenuButtons.New : NewUser()
                Case MenuButtons.MenuButtons.Edit : EditUser()
                Case MenuButtons.MenuButtons.Delete : DeleteUser()
                Case MenuButtons.MenuButtons.Save : SaveUser()
                Case MenuButtons.MenuButtons.Cancel : CancelUser()
            End Select
        Catch ex As Exception
            ctrlMenu.ErrorOccured = True
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_OnOK(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.OnOk
        m_SelectedRow = SelectedRow
        m_RaiseOk = True
    End Sub

    Private Sub ctrlList_RowDoubleClick(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.RowDoubleClick
        Try
            If Not m_IsBrowser Then
                btnView.PerformClick()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_InitializeGridLayout(ByVal e As InitializeLayoutEventArgs) Handles ctrlList.InitializeGridLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns
                .Override.CellClickAction = CellClickAction.RowSelect

                With .Bands(0)
                    For Each col As UltraGridColumn In .Columns
                        With col
                            Select Case .Key
                                Case "name"
                                    .Header.Caption = "Name"
                                Case "role_name"
                                    .Header.Caption = "Access Role"
                                Case "username"
                                    .Header.Caption = "Username"
                                Case "is_active"
                                    If m_IsBrowser Then
                                        .Hidden = True
                                    Else
                                        .Header.Caption = "Active"
                                        .SetWidth(50, 60)
                                        .Style = ColumnStyle.CheckBox
                                    End If
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

    Private Sub ctrlList_InitializeGridRow(e As InitializeRowEventArgs) Handles ctrlList.InitializeGridRow
        If Not Current.User.IsSuperRole Then
            If CBool(e.Row.Cells("is_super_role").Value) Then
                e.Row.Hidden = True
            End If
        End If
    End Sub

    Private Sub tabSystemUsers_SelectedTabChanged(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles tabSystemUsers.SelectedTabChanged
        If sender.SelectedTab.Index = 1 Then
            CancelButton = Nothing
        Else
            CancelButton = ctrlList.btnCancel
        End If
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_SecurityUser = New User
            m_SecurityUserList = New UserList

            ctrlList.SearchKeys = "name,role_name,username"
            ctrlMenu.SetToDefault()

            LoadUserList()
            PopulateSecurityRole()
            SetGroupBox(False)

            ctrlList.UsedInBrowser = m_IsBrowser

            m_CheckFormChanges = New CheckFormChanges
            With m_CheckFormChanges
                .Add(grpUserInformation)
                .Add(grpUserSetting)
                .Add(grpLoginInformation)
            End With

            If ctrlList.grdList.Rows.Count > 0 Then
                ctrlList.Focus()
                ctrlList.grdList.Rows(0).Activate()
            End If

            If Not UseBarcodeLogin Then
                lblLoginBarcode.Visible = False
                txtLoginBarcode.Visible = False
                lblLoginBarcodeHint.Visible = False
                chkIsActive.Top = chkIsNavAccount.Top - 7
                chkIsNavAccount.Top = txtLoginBarcode.Top
            End If

            chkIsNavAccount.Visible = False
            chkIsActive.Top = chkIsNavAccount.Top
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadUserList()
        Try
            With m_SecurityUserList
                .Code = ""
                .Fill()
            End With

            With ctrlList
                .grdList.DataSource = m_SecurityUserList.Data
                .LockGridRows()
                .Activate()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PopulateSecurityRole()
        Try
            Dim cls As New RoleList()

            cls.Fill(Not Current.User.IsSuperRole)

            With Me.cbxSecurityRole
                With .Items
                    .Clear()
                    For Each dr As DataRow In cls.List.Rows
                        .Add(dr("code").ToString(), dr("name").ToString())
                    Next dr
                End With
                .SelectedIndex = -1
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NewUser()
        Try
            m_IsNew = True
            m_Code = ""
            SetGroupBox(True)
            Clear()
            If cbxSecurityRole.Items.Count = 1 Then
                cbxSecurityRole.SelectedIndex = 0
            End If
            chkIsActive.Checked = True
            m_Operation = "INSERT"
            txtName.Focus()
            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EditUser()
        Try
            m_IsNew = False
            SetGroupBox(True)
            m_Operation = "UPDATE"
            txtName.Focus()
            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveUser()
        Try
            If Not IsValid() Then
                ctrlMenu.ErrorOccured = True
                Exit Sub
            End If

            If Not m_CheckFormChanges.HasChanges Then
                ShowSaveNoChanges()
                GoTo No_Changes
            End If

            Me.ObjectToClass()

            If m_SecurityUser.SaveUser Then
                If m_Operation = "INSERT" Then
                    ShowSaveMessage()
                Else
                    ShowUpdateMessage()
                End If
No_Changes:
                m_Code = m_SecurityUser.Code
                m_Operation = ""

                LoadUserList()
                m_CheckFormChanges.ClearChanges()
            End If

            SetGroupBox(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DeleteUser()
        Try
            If m_Code = Current.User.Code Then
                ShowMessage("Cannot delete current user, Unable to continue!", "Delete Failed", MessageIcon.Exclamation)
                Return
            End If
            If ShowDeleteQuestion() = QuestionResult.Yes Then
                m_Operation = "DELETE"
                ObjectToClass()

                If m_SecurityUser.SaveUser Then
                    ShowDeleteMessage()
                    m_CurrentUserName = ""
                End If

                Clear()
                LoadUserList()
                SetGroupBox(False)
            Else
                ctrlMenu.ErrorOccured = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelUser()
        Try
            If m_CheckFormChanges.HasChanges Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    ctrlMenu.ErrorOccured = True
                    Return
                End If
            End If

            If m_IsNew Then
                Clear()
                m_IsNew = False
            Else
                FillInformation(m_SecurityUser.Code)
                m_Operation = ""
            End If

            SetGroupBox(False)

            m_CurrentUserName = ""
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillInformation(ByVal _Code As String)
        Try
            Dim row As DataRow = m_SecurityUserList.Data.Select("code = '" & _Code & "'").First

            txtCode.Text = row("code")
            txtName.Text = row("name")
            cbxSecurityRole.Value = row("role_code")
            m_IsSuperUser = CBool(row("is_super_role"))
            txtUserName.Text = row("username")
            m_CurrentUserName = txtUserName.Text
            m_CurrentPassword = row("password").ToString.Decrypt
            txtPassword.Text = m_CurrentPassword
            txtConfirmPassword.Text = m_CurrentPassword
            txtLoginBarcode.Text = row("barcode")
            chkIsNavAccount.Checked = row("is_nav_account")
            chkIsActive.Checked = row("is_active")

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            ClearInvalidFields()

            grpUserInformation.GetErrors()
            grpUserSetting.GetErrors()
            grpLoginInformation.GetErrors()

            If m_SecurityUserList.IsUsernameExisting(txtUserName.Text, m_Code) Then
                AddToInvalidFields("Username has already been used")
            End If

            If txtLoginBarcode.Visible AndAlso Not String.IsNullOrWhiteSpace(txtLoginBarcode.Text) Then
                If m_SecurityUserList.IsBarcodeExisting(txtLoginBarcode.Text, m_Code) Then
                    AddToInvalidFields("Barcode has already been used")
                End If
            End If

            If txtPassword.TextLength < 5 Then
                AddToInvalidFields("Password must be at least 5 characters long")
            End If

            If txtPassword.Text <> txtConfirmPassword.Text Then
                AddToInvalidFields("Passwords do not match")
            End If

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields)
            End If

            Return Not HasInvalidFields()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub Clear()
        Try
            m_SecurityUser.Code = ""
            m_SecurityUserList.Code = ""
            ctrlList.grdList.ActiveRow = Nothing

            txtName.Clear()
            cbxSecurityRole.SelectedIndex = -1
            txtUserName.Clear()
            txtPassword.Clear()
            txtConfirmPassword.Clear()
            txtLoginBarcode.Clear()

            chkIsNavAccount.Checked = False
            chkIsActive.Checked = True

            m_IsSuperUser = False
            m_CurrentPassword = ""
            m_CurrentUserName = ""
            m_Operation = ""

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ObjectToClass()
        Try
            With m_SecurityUser
                .RoleCode = cbxSecurityRole.Value
                .Name = txtName.Text.Trim
                .UserName = txtUserName.Text.Trim
                .Password = txtPassword.Text.Encrypt
                .Barcode = txtLoginBarcode.Text.Trim
                .IsNavAccount = chkIsNavAccount.Checked
                .IsActive = chkIsActive.Checked
                .Operation = m_Operation
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetGroupBox(ByVal Enabled As Boolean)
        Try
            grpUserInformation.Enabled = Enabled
            If m_IsSuperUser Then
                lblSecurityRole.Text = "User Role"
                With cbxSecurityRole
                    .Appearance.BackColorDisabled = Color.FromKnownColor(KnownColor.Window)
                    .Appearance.ForeColorDisabled = Color.FromKnownColor(KnownColor.WindowText)
                    .DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never
                    .Enabled = False
                End With
                chkIsActive.Visible = False
            Else
                lblSecurityRole.Text = "User Role*"
                grpUserSetting.Enabled = Enabled
                With cbxSecurityRole
                    .Appearance.BackColorDisabled = Nothing
                    .Appearance.ForeColorDisabled = Nothing
                    .DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always
                    .Enabled = Enabled
                End With
                chkIsActive.Visible = True
            End If
            grpLoginInformation.Enabled = Enabled
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class