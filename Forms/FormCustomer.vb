Imports System.Web.Services.Description
Imports BusinessPro.Core.Extensions

Public Class FormCustomer

#Region "Constructor"

    Public Sub New(ByVal isBrowser As Boolean, showInformation As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            m_IsBrowser = isBrowser
            m_ShowInformation = showInformation
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Declarations"

    Private m_Settings As SettingsPreferences
    Private ReadOnly m_IsBrowser As Boolean
    Private ReadOnly m_ShowInformation As Boolean

    Private m_CustomerList As CustomerList

    Private ReadOnly m_List As DataTable
    Private m_Operation As String = ""
    Private m_CheckFormChanges As CheckFormChanges
    Private m_Initialized As Boolean

    Private m_CodeLoaded As String = ""
    Private m_CodeName As String = ""

#End Region

#Region "Properties"

    Property Data As Customer

    Property Code As String
    Property ModuleName As String = "Customers"
    Property ModuleGroup As String = "Libraries"
    Property TerminalCode As String
    Property UserCode As String
    Property UserCompany As String
    Property UserName As String

    Property IsDeleted As Boolean ' if loaded record is deleted

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_CheckFormChanges.HasChanges Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    e.Cancel = True
                End If
            End If
            If m_IsBrowser AndAlso Not e.Cancel Then
                If DialogResult <> DialogResult.OK Then
                    DialogResult = DialogResult.Cancel
                End If
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

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            RefreshList()

            If tabCustomers.Tabs("INFO").Visible Then
                If Code <> "" AndAlso Code <> "WALK-IN" Then
                    FillInformation(Code)
                    ctrlMenu.SetMenuState(MenuButtons.MenuStates.Select)
                    tabCustomers.Tabs("INFO").Selected = True
                Else
                    ctrlMenu.SetMenuState(MenuButtons.MenuStates.Default)
                End If
            End If

            If m_IsBrowser Then
                AcceptButton = ctrlList.ButtonOK
                ctrlList.Activate(True)
            End If

            m_CodeLoaded = Code
            m_Initialized = True
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Menu"

    Private Sub ctrlMenu_MenuClick(ByVal State As MenuButtons.MenuButtons) Handles ctrlMenu.MenuClick
        Try
            Select Case State
                Case MenuButtons.MenuButtons.New : NewCustomer()
                Case MenuButtons.MenuButtons.Edit : EditCustomer()
                Case MenuButtons.MenuButtons.Save : ctrlMenu.ErrorOccured = Not SaveCustomer()
                Case MenuButtons.MenuButtons.Delete : DeleteCustomer()
                Case MenuButtons.MenuButtons.Cancel : CancelCustomer()
            End Select
        Catch ex As Exception
            ctrlMenu.ErrorOccured = True
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "User Control"

    Private Sub ctrlList_OnOK(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.OnOk
        Try
            With Data
                .Code = SelectedRow.Cells("code").Text
                .Name = SelectedRow.Cells("name").Text
                .Address = SelectedRow.Cells("address").Text
                .ContactNo = SelectedRow.Cells("contact_no").Text
                .TIN = SelectedRow.Cells("tin").Text
                .BusinessStyle = SelectedRow.Cells("business_style").Text
                .DiscountType = GetDiscountType(SelectedRow.Cells("discount_type").Value)
                .Gender = SelectedRow.Cells("gender").Text
                .BirthDate = GetDateForVariable(SelectedRow.Cells("birthdate").Value)
                .SCPWDNo = SelectedRow.Cells("sc_pwd_no").Text
                .IssuedDate = GetDateForVariable(SelectedRow.Cells("issued_date").Value)
                .IsActive = SelectedRow.Cells("is_active").Value
            End With

            Me.Code = Data.Code

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub ctrlList_RowDoubleClick(ByVal SelectedRow As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles ctrlList.RowDoubleClick
        Try
            If m_IsBrowser OrElse Not btnViewInfo.Visible Then
                ctrlList_OnOK(ctrlList.ActiveRow)
            Else
                btnViewInfo.PerformClick()
            End If
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

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "No"
                                    .Header.Caption = "No."
                                    .SetWidth(80, 100)

                                Case "Name", "name"
                                    .SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Ascending

                                Case "Contact", "contact_no"
                                    .Header.Caption = "Contact No."

                                Case "CustomerDiscGroup", "discount_type"
                                    .Header.Caption = "Discount Type"
                                    .SetWidth(120, 140)

                                Case "is_active"
                                    .Header.Caption = "Active"
                                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                                    .Hidden = Not chkShowInactive.Checked
                                    .SetWidth(60, 80)

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

#Region "Others"

    Private Sub chkShowInactive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShowInactive.CheckedChanged
        Try
            If m_Initialized Then
                RefreshList()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub tabCustomers_SelectedTabChanged(sender As Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles tabCustomers.SelectedTabChanged
        Try
            If e.Tab.Index = 0 Then
                ctrlList.Activate()
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnViewInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewInfo.Click
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

                With ctrlList.ActiveRow
                    Code = .Cells("code").Value
                    FillInformation(Code)
                End With

                ctrlMenu.SetMenuState(MenuButtons.MenuStates.Select)
                tabCustomers.Tabs("INFO").Selected = True
                grpCustomer.Enabled = False
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub optDiscountType_ValueChanged(sender As Object, e As EventArgs) Handles optDiscountType.ValueChanged
        Try
            If optDiscountType.Value = "" Then
                lblSCPWDNo.Enabled = False
                txtScPwdIDNo.Enabled = False
                txtScPwdIDNo.Clear()
                lblDateIssued.Enabled = False
                dtpDateIssued.Enabled = False
                dtpDateIssued.Value = Nothing
            Else
                lblSCPWDNo.Enabled = True
                txtScPwdIDNo.Enabled = True
                lblDateIssued.Enabled = True
                dtpDateIssued.Enabled = True
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

#Region "Menu Methods"

    Private Sub NewCustomer()
        Try
            Clear(False)
            m_Operation = "INSERT"
            grpCustomer.Enabled = True
            optDiscountType.Value = ""
            chkIsActive.Checked = True
            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EditCustomer()
        Try
            If Code = "" Then
                ShowSelectMessage("edit")
                Exit Sub
            End If

            m_Operation = "UPDATE"
            grpCustomer.Enabled = True
            txtName.Focus()

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DeleteCustomer()
        Try
            If ShowDeleteQuestion() = QuestionResult.Yes Then
                m_Operation = "DELETE"

                With Data
                    .Code = Code
                    .Operation = m_Operation
                    .EmployeeCode = UserCode
                    If .Save() AndAlso Code = m_CodeLoaded Then
                        IsDeleted = True
                    End If
                End With

                SaveAuditTrail(String.Format("Deleted customer {0}", txtName.Text))
                ShowDeleteMessage()

                RefreshList()
                grpCustomer.Enabled = False
                Clear()
            Else
                ctrlMenu.ErrorOccured = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function SaveCustomer() As Boolean
        Try
            If Not IsValid() Then
                Return False
            End If

            Dim saveSuccessful As Boolean

            UIToClass()

            saveSuccessful = Data.Save()
            Code = Data.Code

            If saveSuccessful Then
                If m_Operation = "INSERT" Then
                    SaveAuditTrail(String.Format("Newly created customer {0}", txtName.Text))
                    ShowSaveMessage()
                Else
                    SaveAuditTrail(String.Format("Updated customer information {0}", txtName.Text))
                    ShowUpdateMessage()
                End If

                chkShowInactive.Checked = chkIsActive.Checked

                RefreshList()

                m_Operation = ""
                grpCustomer.Enabled = False
                m_CheckFormChanges.ClearChanges()
            Else
                ShowWarning("There is a problem saving the customer record!\n\nPlease contact your system administrator.", "Save Failed")
            End If

            Return saveSuccessful
        Catch
            Throw
        End Try
    End Function

    Private Sub CancelCustomer()
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
                FillInformation(Code)
                m_Operation = ""
            End If
            grpCustomer.Enabled = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UIToClass()
        Try
            With Data
                .Code = Me.Code
                .CodeName = m_CodeName
                .Name = txtName.Text
                .Address = txtAddress.Text
                .ContactNo = txtContactNo.Text
                .TIN = txtTIN.Text
                .BusinessStyle = txtBusinessStyle.Text
                .DiscountType = optDiscountType.Value
                .Gender = Core.Extensions.IfNothing(cmbGender.Value, "")
                .BirthDate = GetDateForVariable(dtpBirthdate.Value)
                .SCPWDNo = txtScPwdIDNo.Text
                .IssuedDate = GetDateForVariable(dtpDateIssued.Value)
                .IsActive = chkIsActive.Checked
                .EmployeeCode = Me.UserCode
                .Operation = m_Operation
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Other Methods"

    Private Sub Initialize()
        Try
            m_Settings = New SettingsPreferences

            Data = New Customer
            m_CustomerList = New CustomerList

            grpCustomer.Enabled = False
            btnViewInfo.Visible = (Not m_IsBrowser OrElse m_ShowInformation)
            ctrlList.UsedInBrowser = m_IsBrowser

            If m_IsBrowser Then
                chkShowInactive.Visible = False
            End If

            InitializeGrid()

            m_CheckFormChanges = New CheckFormChanges
            m_CheckFormChanges.Add(grpCustomer)

            With ctrlList.Grid
                If .Rows.Count > 0 Then
                    .Rows(0).Activate()
                End If
            End With

            pnlListHeader.Visible = btnViewInfo.Visible
            tabCustomers.Tabs("INFO").Visible = btnViewInfo.Visible

            If btnViewInfo.Visible Then
                With ctrlMenu
                    .ModuleName = Me.ModuleName
                    .ModuleGroup = Me.ModuleGroup
                End With
            Else
                tabCustomers.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Wizard
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeGrid()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("name")
                .Add("contact_no")
                .Add("discount_type")
                .Add("is_active", GetType(Boolean))
                ctrlList.SearchKeys = "name"
            End With
            ctrlList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillInformation(ByVal code As String)
        Try
            With Data
                .Code = code
                .Fill()

                m_CodeName = .CodeName
                txtName.Text = .Name
                txtAddress.Text = .Address
                txtContactNo.Text = .ContactNo
                txtTIN.Text = .TIN
                txtBusinessStyle.Text = .BusinessStyle
                optDiscountType.Value = GetDiscountType(.DiscountType)
                cmbGender.Value = .Gender
                dtpBirthdate.Value = GetDateForControl(.BirthDate)
                txtScPwdIDNo.Text = .SCPWDNo
                dtpDateIssued.Value = GetDateForControl(.IssuedDate)
                chkIsActive.Checked = .IsActive
            End With

            m_CheckFormChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetDiscountType(value As String) As String
        Try
            If (value = "SC" OrElse (value.ToUpper.Contains("SENIOR") AndAlso value.ToUpper.Contains("CITIZEN"))) Then
                Return "SC"
            ElseIf (value = "PWD" OrElse (value.ToUpper.Contains("PERSON") AndAlso value.ToUpper.Contains("DISABILITY"))) Then
                Return "PWD"
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub RefreshList()
        Try
            Dim showInactiveCustomer As Boolean = chkShowInactive.Checked
            Dim dtList As DataTable = Nothing

            With New TaskProcess(Me, ctrlList.Grid)
                .ShowLoader = ctrlList.Grid.Visible
                .Start(
                    Sub()
                        m_CustomerList.Fill(showInactiveCustomer)
                        dtList = m_CustomerList.Data
                    End Sub,
                    Sub()
                        ctrlList.Clear()
                        ctrlList.DataSource = dtList

                        If dtList.Rows.Count > 0 Then
                            ctrlList.Grid.Rows(0).Activate()
                        End If
                    End Sub)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Clear(Optional clearChanges As Boolean = True)
        Try
            grpCustomer.Clear()
            dtpBirthdate.Value = Nothing

            Code = ""

            m_CodeName = ""
            m_Operation = ""

            If clearChanges Then
                m_CheckFormChanges.ClearChanges()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            Return Not grpCustomer.HasErrors
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SaveAuditTrail(activity As String)
        Try
            With New TaskProcess(Me)
                .Start(
                    Sub()
                        With New AuditTrail
                            .TerminalCode = Me.TerminalCode
                            .UserCode = Me.UserCode
                            .UserName = Me.UserName
                            .Activity = activity
                            .SaveAuditTrail()
                        End With
                    End Sub
                )
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Overrides"

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, keyData As System.Windows.Forms.Keys) As Boolean
        Try
            Select Case keyData
                Case Keys.Escape
                    DialogResult = DialogResult.Cancel
                    Close()
                    Return True

                Case Keys.F1
                    btnViewInfo.PerformClick()

                Case Keys.F4
                    With ctrlList.Grid
                        .Rows(0).Activate()
                        .Focus()
                    End With

                Case Keys.Enter
                    If ctrlList.ActiveRow IsNot Nothing Then
                        ctrlList_OnOK(ctrlList.ActiveRow)
                    End If
            End Select

            Return MyBase.ProcessCmdKey(msg, keyData)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#End Region

End Class