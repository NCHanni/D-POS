Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Friend Class FormSettingsPreferences

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences

    Private m_Data As DataTable
    Private m_BannerChanged As Boolean
    Private m_BannerImage As Image
    Private m_BannerImageText As String

#End Region

#Region "Constructor"

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            m_Settings = New SettingsPreferences
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Event Handlers"

#Region "Form"

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

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            LoadData()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdList_BeforeCellUpdate(ByVal sender As Object, ByVal e As UltraWinGrid.BeforeCellUpdateEventArgs) Handles grdList.BeforeCellUpdate
        Try
            If e.Cell.Style = UltraWinGrid.ColumnStyle.DoubleNonNegative Then
                Dim cellAmount As Double = e.Cell.Text.Replace("_", "")
                If cellAmount > 100.0 Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_ClickCellButton(sender As Object, e As CellEventArgs) Handles grdList.ClickCellButton
        If e.Cell.Row.Cells("flag").Text = SettingsPreferences.FLAG_BANNER_IMAGE Then
            Using dlg As New OpenFileDialog
                With dlg
                    .Filter = "Supported Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*"
                    .FilterIndex = 0
                    .Title = "Select Display Banner Image"

                    If .ShowDialog(Me) = DialogResult.OK Then
                        Dim img As Image = Image.FromFile(.FileName)
                        Dim ext As String = If(.SafeFileName.Contains("."), .SafeFileName.Substring(.SafeFileName.LastIndexOf(".") + 1), "")
                        Dim format As Imaging.ImageFormat

                        Select Case ext.ToLower
                            Case "jpg", "jpeg"
                                format = Imaging.ImageFormat.Jpeg
                            Case "gif"
                                format = Imaging.ImageFormat.Gif
                            Case Else
                                format = Imaging.ImageFormat.Png
                        End Select

                        e.Cell.Appearance.Image = img
                        e.Cell.Tag = img.ToText(format)
                        e.Cell.Value = "IMAGEX" ' just to make row dirty when changing image
                        e.Cell.Value = "IMAGE"

                        m_BannerChanged = True
                        m_BannerImage = e.Cell.Appearance.Image
                        m_BannerImageText = e.Cell.Tag
                    End If
                End With
            End Using
        End If
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As System.Object, ByVal e As UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = UltraWinGrid.AutoFitStyle.ResizeAllColumns
                .Override.HeaderClickAction = UltraWinGrid.HeaderClickAction.Select
                .ScrollBounds = UltraWinGrid.ScrollBounds.ScrollToFill
                With .Bands(0)
                    .Columns("flag").Hidden = True
                    With .Columns("description")
                        .CellActivation = UltraWinGrid.Activation.NoEdit
                    End With
                    With .Columns("value")
                        .CellAppearance.TextHAlign = HAlign.Right
                        .MinWidth = 120
                        .MaxWidth = 250
                    End With
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeRow(ByVal sender As Object, ByVal e As UltraWinGrid.InitializeRowEventArgs) Handles grdList.InitializeRow
        Try
            With e.Row.Cells("value")
                Select Case e.Row.Cells("flag").Value
                    Case SettingsPreferences.FLAG_AUTH_OFFICER
                        .Style = UltraWinGrid.ColumnStyle.DropDownList
                        .ValueList = e.Row.Band.Layout.ValueLists("roles")
                        If Not btnSave.Enabled Then
                            .Activation = UltraWinGrid.Activation.NoEdit
                        End If

                    Case SettingsPreferences.FLAG_RESET_THRESHOLD
                        Dim editorSettings As New UltraWinEditors.DefaultEditorOwnerSettings With {.Format = "#,##0.00"}
                        Dim defaultEditorOwner As New UltraWinEditors.DefaultEditorOwner(editorSettings)
                        Dim editor = New EditorWithText(defaultEditorOwner)
                        .Editor = editor

                    Case SettingsPreferences.FLAG_INV_END,
                         SettingsPreferences.FLAG_INV_START
                        .Style = UltraWinGrid.ColumnStyle.IntegerNonNegative

                    Case SettingsPreferences.FLAG_SALES_TAX
                        .Style = UltraWinGrid.ColumnStyle.DoubleNonNegative

                    Case SettingsPreferences.FLAG_ACU_INTEGRATION,
                         SettingsPreferences.FLAG_AUTH_QUANTITY,
                         SettingsPreferences.FLAG_AUTH_DELETION,
                         SettingsPreferences.FLAG_AUTH_CANCEL,
                         SettingsPreferences.FLAG_AUTH_LOAD_POSTED_SALE,
                         SettingsPreferences.FLAG_AUTH_REPRINT_RECEIPT,
                         SettingsPreferences.FLAG_AUTH_CASH_UP,
                         SettingsPreferences.FLAG_ITEM_CODE_INVOICE,
                         SettingsPreferences.FLAG_NAV_ITEM_EXTEXT,
                         SettingsPreferences.FLAG_NAV_USE_A4INVOICE,
                         SettingsPreferences.FLAG_USE_BARCODE_LOGIN,
                         SettingsPreferences.FLAG_TRAINING_MODE,
                         SettingsPreferences.FLAG_BANNER_DEFAULT,
                         SettingsPreferences.FLAG_FULLSCREEN,
                         SettingsPreferences.FLAG_SHOW_SALESPERSON,
                         SettingsPreferences.FLAG_REQUIRE_SALESPERSON,
                         SettingsPreferences.FLAG_AUTO_FINALIZE_Z

                        If e.Row.Cells("flag").Value = SettingsPreferences.FLAG_ACU_INTEGRATION Then
                            e.Row.Appearance.FontData.Bold = DefaultableBoolean.True
                        End If

                        If btnSave.Enabled Then
                            .Style = UltraWinGrid.ColumnStyle.DropDownList
                            With e.Row.Band.Layout
                                If Not .ValueLists.Exists("boolean") Then
                                    With .ValueLists.Add("boolean")
                                        .ValueListItems.Add(True)
                                        .ValueListItems.Add(False)
                                    End With
                                End If
                            End With
                            .ValueList = e.Row.Band.Layout.ValueLists("boolean")
                        Else
                            .Style = UltraWinGrid.ColumnStyle.Default
                        End If

                    Case SettingsPreferences.FLAG_BANNER_IMAGE
                        .Activation = UltraWinGrid.Activation.NoEdit
                        .Appearance.Image = m_BannerImage
                        .Style = UltraWinGrid.ColumnStyle.EditButton

                    Case SettingsPreferences.FLAG_BANNER_STYLE
                        .Style = UltraWinGrid.ColumnStyle.DropDownList
                        .ValueList = e.Row.Band.Layout.ValueLists("sizemodes")
                        If Not btnSave.Enabled Then
                            .Activation = UltraWinGrid.Activation.NoEdit
                        End If

                    Case Else
                        .Style = UltraWinGrid.ColumnStyle.Default
                End Select
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_KeyDown(sender As Object, e As KeyEventArgs) Handles grdList.KeyDown
        If grdList.ActiveCell IsNot Nothing Then
            If e.Modifiers = Keys.None AndAlso (e.KeyCode = Keys.Back OrElse e.KeyCode = Keys.Delete) Then
                With grdList.ActiveCell
                    If .Column.Key = "value" AndAlso .Row.Cells("flag").Text = SettingsPreferences.FLAG_BANNER_IMAGE Then
                        If .Value = "IMAGE" Then
                            .Appearance.Image = Nothing
                            .Tag = String.Empty
                            .Value = "NONE"

                            m_BannerChanged = True
                            m_BannerImage = Nothing
                            m_BannerImageText = String.Empty
                        End If
                    End If
                End With
            End If
        End If
    End Sub

#End Region

#Region "Button"

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            grdList.UpdateData()

            ClearInvalidFields()
            grdList.GetErrors("")

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields)
            Else
                With m_Settings
                    .Data = m_Data
                    If .Data.HasChanges AndAlso .Save(m_BannerImageText) Then
                        LoadData()
                        LoadSettingsAndPreferences()
                        DirectCast(Me.MdiParent, FormMain).InitializeAccessRight()
                        ShowMessage("Changes has been successfully saved!\n\nPlease restart the application to fully apply the changes.", "Save Successful")
                    End If
                End With
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            btnSave.CheckEdit()

            InitializeGrid()

            With grdList.DisplayLayout.Bands(0).Override
                If btnSave.Enabled Then
                    .CellClickAction = UltraWinGrid.CellClickAction.Default
                Else
                    .CellClickAction = UltraWinGrid.CellClickAction.RowSelect
                    btnSave.Visible = False
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeGrid()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("description")
                .Add("flag")
                .Add("value")
            End With
            grdList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillPictureBoxSizeModes()
        Try
            Const key As String = "sizemodes"

            With grdList.DisplayLayout.ValueLists
                If .Exists(key) Then
                    .Remove(key)
                End If
                .Add(key)

                With .Item(key)
                    .ValueListItems.Add(0, PictureBoxSizeMode.Normal.ToString)
                    .ValueListItems.Add(1, PictureBoxSizeMode.StretchImage.ToString)
                    .ValueListItems.Add(2, PictureBoxSizeMode.AutoSize.ToString)
                    .ValueListItems.Add(3, PictureBoxSizeMode.CenterImage.ToString)
                    .ValueListItems.Add(4, PictureBoxSizeMode.Zoom.ToString)
                End With
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FillRoles()
        Try
            Const key As String = "roles"

            With grdList.DisplayLayout.ValueLists
                If .Exists(key) Then
                    .Remove(key)
                End If
                .Add(key)
            End With

            With New RoleList
                .Fill(True)

                For Each role As DataRow In .List.Rows
                    With grdList.DisplayLayout.ValueLists(key)
                        .ValueListItems.Add(role("code"), role("name"))
                    End With
                Next
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadData()
        Try
            With m_Settings
                .Fill()
                m_Data = .Data
            End With

            FillPictureBoxSizeModes()
            FillRoles()

            m_BannerChanged = False
            m_BannerImage = Nothing
            m_BannerImageText = String.Empty
            For Each row As DataRow In m_Data.Rows
                If row("flag") = SettingsPreferences.FLAG_BANNER_IMAGE Then
                    If row("value") <> "NONE" Then
                        m_BannerImageText = row("value")
                        m_BannerImage = m_BannerImageText.ToImage
                        row("value") = "IMAGE"
                    End If
                    Exit For
                End If
            Next
            If m_Data.HasChanges Then
                m_Data.AcceptChanges()
            End If

            grdList.DataSource = m_Data
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class