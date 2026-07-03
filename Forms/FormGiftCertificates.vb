Imports Infragistics.Win.UltraWinGrid

Friend Class FormGiftCertificates

#Region "Declarations"

    Private m_ConnectionString As DataSource.ConnectionString
    Private m_GiftCertificates As GiftCertificates
    Private m_Data As DataTable
    Private m_Initialized As Boolean
    Private m_HasGridEditAccess As Boolean

#End Region

#Region "Constructor"

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_ConnectionString = Core.Current.ConnectionString
    End Sub

    Sub New(connectionString As DataSource.ConnectionString)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_ConnectionString = connectionString
    End Sub

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

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            RefreshList()
            m_Initialized = True
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            grdList.Focus()
            With grdList.DisplayLayout.Bands(0).AddNew()
                .Activate()
                .Cells("code").Activate()
                .Cells("is_released").Activation = Activation.NoEdit
                .Cells("is_used").Activation = Activation.NoEdit
            End With
            grdList.PerformAction(UltraGridAction.EnterEditMode)
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
                    If CBool(.Cells("is_released").Value) OrElse CBool(.Cells("is_used").Value) Then
                        ShowMessage("Selected gift certificate has already been released!", "Remove Failed")
                    ElseIf ShowRemoveQuestion() = QuestionResult.Yes Then
                        .Delete(False)
                    End If
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
                With m_GiftCertificates
                    .Data = m_Data
                    .UserName = Core.Current.User.Name
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

#End Region

#Region "Grid"

    Private Sub grdList_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                With .Bands(0)
                    With .Columns("id")
                        .Hidden = True
                        .DefaultCellValue = 0
                    End With
                    With .Columns("code")
                        .Header.Caption = "Code"
                        .MaxLength = 30
                    End With
                    With .Columns("description")
                        .Header.Caption = "Description"
                        .MaxLength = 100
                    End With
                    With .Columns("expiry_date")
                        .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left
                        .DefaultCellValue = DBNull.Value
                        .MinValue = Now.Date
                        .SetWidth(80)
                        .Style = ColumnStyle.Date
                    End With
                    With .Columns("amount")
                        .DefaultCellValue = 0.0
                        .Style = ColumnStyle.DoubleNonNegative
                        .SetWidth(80)
                    End With
                    With .Columns("is_active")
                        .Style = ColumnStyle.CheckBox
                        .DefaultCellValue = True
                        .Header.Caption = "Active"
                        .SetWidth(60)
                    End With
                    With .Columns("is_sold")
                        .Style = ColumnStyle.CheckBox
                        .DefaultCellValue = False
                        .Header.Caption = "Sold"
                        .SetWidth(60)
                    End With
                    With .Columns("is_used")
                        .Style = ColumnStyle.CheckBox
                        .DefaultCellValue = False
                        .Header.Caption = "Used"
                        .SetWidth(60)
                    End With
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeRow(sender As Object, e As InitializeRowEventArgs) Handles grdList.InitializeRow
        Try
            Dim isSold As Boolean
            Dim isUsed As Boolean

            With e.Row
                If .Cells("id").Value = 0 Then
                    Return
                ElseIf Not m_HasGridEditAccess Then
                    .Activation = Activation.NoEdit
                    Return
                End If

                isSold = .Cells("is_sold").Value
                isUsed = .Cells("is_used").Value
            End With

            For Each cell As UltraGridCell In e.Row.Cells
                With cell
                    Select Case .Column.Key
                        Case "code", "description", "amount", "expiry_date", "is_active"
                            .Activation = If(isSold OrElse isUsed, Activation.NoEdit, Activation.AllowEdit)
                        Case "is_sold"
                            .Activation = If(isUsed, Activation.NoEdit, Activation.AllowEdit)
                        Case "is_used"
                            .Activation = If(isSold, Activation.AllowEdit, Activation.NoEdit)
                    End Select

                    If Not .Hidden Then
                        .Appearance.BackColor = Nothing
                    End If
                End With
            Next
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Others"

    Private Sub cmbShowSelection_ValueChanged(sender As Object, e As EventArgs) Handles cmbShowSelection.ValueChanged
        Try
            If m_Initialized Then
                RefreshList()
                Me.ActiveControl = grdList
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_GiftCertificates = New GiftCertificates

            InitializeAccessRights()
            cmbShowSelection.Value = 0
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeAccessRights()
        Try
            If Core.Current.Rights IsNot Nothing Then
                Dim moduleName As String = "Gift Certificates"
                Dim hasAdd As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Add)
                Dim hasEdit As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Edit)
                Dim hasDelete As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Delete)

                btnAdd.Enabled = hasAdd
                btnAdd.Tag = moduleName
                btnRemove.Enabled = hasDelete
                btnRemove.Tag = moduleName

                If Not hasAdd AndAlso Not hasEdit Then
                    btnSave.Enabled = False
                End If

                m_HasGridEditAccess = hasEdit
                grdList.Tag = moduleName
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            With m_GiftCertificates
                .Fill(cmbShowSelection.Value)
                m_Data = .Data
            End With

            With grdList
                .ActiveRow = Nothing
                .DataSource = m_Data
            End With

            If cmbShowSelection.Value = 0 OrElse cmbShowSelection.Value = 1 Then
                btnAdd.CheckAdd
                btnRemove.CheckDelete
            Else
                btnAdd.Enabled = False
                btnRemove.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            ClearInvalidFields()

            grdList.GetErrors("", "code", , "amount")
            grdList.GetDuplicateRecords("code")

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