Public Class FormTerminals

#Region "Declarations"

    Private m_Terminal As Terminals
    Private m_Data As DataTable

#End Region

#Region "Properties"

    Property ShowMachineDetails As Boolean = True

#End Region

#Region "Event Handlers"

#Region "Forms"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            grdList.UpdateData()
            If m_Data.HasChanges Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    e.Cancel = True
                End If
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

#End Region

#Region "Buttons"

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            grdList.Focus()
            With grdList.DisplayLayout.Bands(0).AddNew()
                .Activate()
                .Cells("is_active").Value = False
                .Cells("code").Activate()
            End With
            grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            If grdList.ActiveRow Is Nothing Then
                ShowSelectMessage("remove")
            Else
                With grdList.ActiveRow
                    If m_Terminal.IsUsedInPOS(.Cells("id").Value) Then
                        ShowWarning("Terminal has already been used and cannot be removed!", "Remove Failed")
                        Exit Sub
                    End If
                    If .Cells("id").Value > 0 Then
                        If ShowRemoveQuestion() <> QuestionResult.Yes Then
                            Return
                        End If
                    End If
                    .Delete(False)
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not IsValid() Then
                Return
            End If

            If m_Data.HasChanges Then
                With m_Terminal
                    .Data = m_Data
                    .UserName = Current.User.Name.IfNothing("")
                    If .Save Then
                        Initialize()
                    End If
                End With

                ShowSaveSuccesful()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub lblNote_Click(sender As Object, e As EventArgs) Handles lblNote.Click
        Clipboard.SetText(GetMacAddress())
    End Sub

#End Region

#Region "Grid"

    Private Sub grdList_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout.Bands(0)
                .Columns("id").Hidden = True
                .Columns("id").DefaultCellValue = 0
                .Columns("machine_no").Hidden = True
                With .Columns("code")
                    .Header.Caption = "Name"
                    .MaxLength = 20
                End With
                With .Columns("serial_no")
                    .Header.Caption = "Serial No."
                    .MaxLength = 30
                End With
                If ShowMachineDetails Then
                    With .Columns("mac_address")
                        .Header.Caption = "MAC Address"
                        .MaxLength = 30
                    End With
                    With .Columns("computer_name")
                        .Header.Caption = "Computer Name"
                        .MaxLength = 30
                    End With
                Else
                    .Columns("mac_address").Hidden = True
                    .Columns("computer_name").Hidden = True
                End If
                With .Columns("is_active")
                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                    .DefaultCellValue = False
                    .Header.Caption = "Active"
                    .Header.ToolTipText = "Clear computer name & MAC address to deactivate terminal."
                    .CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                    .SetWidth(60)
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_Terminal = New Terminals

            InitializeAccessRights()
            RefreshList()

            lblNote.Text = "My MAC Address: " & GetMacAddress() & " | Computer Name: " & My.Computer.Name
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeAccessRights()
        Try
            If Core.Current.Rights IsNot Nothing Then
                Dim moduleName As String = "Terminals"
                Dim hasAdd As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Add)
                Dim hasEdit As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Edit)
                Dim hasDelete As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Delete)

                btnAdd.Enabled = hasAdd
                btnRemove.Enabled = hasDelete

                If Not hasAdd AndAlso Not hasEdit Then
                    btnSave.Enabled = False
                End If

                grdList.Tag = moduleName
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            With m_Terminal
                .Fill()
                m_Data = .Data
            End With

            With grdList
                .ActiveRow = Nothing
                .DataSource = m_Data
                .CheckEdit()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            ClearInvalidFields()

            grdList.GetErrors("", "code")
            grdList.GetDuplicateRecords("code")

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields)
            End If

            Return Not HasInvalidFields()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetMacAddress() As String
        Try
            Dim netInterface() As System.Net.NetworkInformation.NetworkInterface =
                System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()

            Return netInterface(0).GetPhysicalAddress.ToString.Replace("-", "").Replace(":", "")
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

#End Region

End Class