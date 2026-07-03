Friend Class FormDiscountGroups

#Region "Declarations"

    Private m_Discounts As DiscountGroups
    Private m_Data As DataTable

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                grdList.UpdateData()
                If m_Data.HasChanges Then
                    If ShowCancelQuestion() = QuestionResult.No Then
                        DialogResult = DialogResult.None
                        e.Cancel = True
                    End If
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

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Dim editor As Infragistics.Win.EmbeddableEditorBase

            Me.grdList.DisplayLayout.Bands(0).AddNew()

            grdList.Focus()
            grdList.ActiveCell = grdList.ActiveRow.Cells("description")
            grdList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode)

            editor = grdList.ActiveCell.EditorResolved
            editor.SelectAll()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            If grdList.ActiveRow Is Nothing Then
                ShowSelectMessage("remove")
            Else
                With grdList.ActiveRow
                    If .Cells("code").Value <> "" Then
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

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not IsValid() Then
                Return
            End If

            With m_Discounts
                .Data = m_Data
                .User = Core.Current.User.Name

                If .Save Then
                    ShowSaveSuccesful()
                    RefreshList()
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout.Bands(0)
                With .Columns("code")
                    .Header.Caption = "Code"
                    .CellActivation = IIf(btnAdd.Enabled, Infragistics.Win.UltraWinGrid.Activation.AllowEdit, Infragistics.Win.UltraWinGrid.Activation.NoEdit)
                    .DefaultCellValue = ""
                    .MinWidth = 100
                    .MaxWidth = 120
                End With
                With .Columns("description")
                    .Header.Caption = "Description"
                    .DefaultCellValue = ""
                    .MaxLength = 30
                End With
                With .Columns("is_vat_exempt")
                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                    .DefaultCellValue = False
                    .Header.Caption = "VAT Exempt"
                    .MinWidth = 80
                    .MaxWidth = 100
                End With
                With .Columns("discount_rate")
                    .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleNonNegative
                    .DefaultCellValue = 0.0
                    .Header.Caption = "Discount Rate"
                    .MinWidth = 100
                    .MaxWidth = 120
                End With
            End With
            e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_Discounts = New DiscountGroups

            InitializeAccessRights()
            RefreshList()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeAccessRights()
        Try
            If Core.Current.Rights IsNot Nothing Then
                Dim moduleName As String = "Discount Groups"
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
            m_Data = m_Discounts.List.GetList()

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

            grdList.GetErrors("")
            grdList.GetDuplicateRecords("description")

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields)
            End If

            Return Not HasInvalidFields()
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class