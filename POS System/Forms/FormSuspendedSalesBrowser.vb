Imports Infragistics.Win.UltraWinGrid

Friend Class FormSuspendedSalesBrowser

#Region "Declarations"

    Private m_Settings As SettingsPreferences
    Private m_SuspendedSale As SuspendedSale

    Public Sub New(setting As SettingsPreferences)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = setting
    End Sub

#End Region

#Region "Properties"

    Property CashierSessionId As Long
    Property CustomerCode As String = ""
    Property IsFromCashupForm As Boolean
    Property SuspendCode As String
    Property TerminalCode As String

    Property Header As DataRow
    Property Details As DataTable

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                DialogResult = DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.Delete AndAlso grdList.ActiveRow IsNot Nothing Then
                btnDelete.PerformClick()
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
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

    Private Sub grdList_AfterRowActivate(sender As Object, e As EventArgs) Handles grdList.AfterRowActivate
        Try
            btnDelete.Enabled = True
            btnOk.Enabled = True
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_Click(sender As Object, e As EventArgs) Handles grdList.Click
        Try
            If grdList.ActiveRow Is Nothing Then
                btnDelete.Enabled = False
                btnOk.Enabled = False
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_DoubleClickRow(sender As Object, e As DoubleClickRowEventArgs) Handles grdList.DoubleClickRow
        btnOk.PerformClick()
    End Sub

    Private Sub grdList_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                .Override.CellClickAction = CellClickAction.RowSelect
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns

                With .Bands(0)
                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "code"
                                    .Header.Caption = "Code"
                                    .SetWidth(120)
                                Case "transaction_date"
                                    .Header.Caption = "Time"
                                    .Format = "hh:mm tt"
                                    .SetWidth(70)
                                    .Style = ColumnStyle.Time
                                Case "customer_name"
                                    .Header.Caption = "Customer Name"
                                Case "total_amount"
                                    .Header.Caption = "Total Amount"
                                    .Style = ColumnStyle.DoubleNonNegative
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .CellActivation = Activation.NoEdit
                                    .Format = "#,##0.00"
                                    .SetWidth(100)
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next
                End With

                If .Bands.Count > 1 Then
                    .Bands(1).Hidden = True ' Details
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If grdList.ActiveRow Is Nothing Then
                btnDelete.Enabled = False
                Return
            End If

            Dim code As String = grdList.ActiveRow.Cells("code").Value
            Dim authorizedBy As String = ""
            Dim reason As String = ""

            Using frmAuth As New FormAuthorization(m_Settings, True) With {.Message = "DELETE SUSPENDED SALE"}
                If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                    authorizedBy = frmAuth.AuthorizedBy
                    reason = frmAuth.Reason
                Else
                    btnDelete.Enabled = (grdList.ActiveRow IsNot Nothing)
                    btnOk.Enabled = btnDelete.Enabled
                    Return
                End If
            End Using

            If m_SuspendedSale.Delete(code, reason, authorizedBy) Then
                With New TaskProcess(Me)
                    .Start(
                    Sub()
                        With New AuditTrail
                            .TerminalCode = Current.Settings.TerminalCode
                            .UserCode = Current.User.Code
                            .UserName = Current.User.Name
                            .Activity = "Suspended Sale deleted - " & code & "|Authorized by: " & authorizedBy
                            .SaveAuditTrail()
                        End With
                    End Sub
                )
                End With

                ShowMessage("Selected suspended sale has been successfully deleted.", "Suspended Sale Deleted")
                RefreshList()
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Dim activeRow As UltraGridRow = grdList.ActiveRow
            Dim data As DataSet = grdList.DataSource

            SuspendCode = activeRow.Cells("code").Value
            CustomerCode = activeRow.Cells("customer_code").Value
            Header = data.Tables(0).Select("code = " & SuspendCode.Quote(False))(0)
            Details = data.Tables(1).Select("suspend_code = " & SuspendCode.Quote(False)).CopyToDataTable

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_SuspendedSale = New SuspendedSale

            If IsFromCashupForm Then
                Text = Text.Replace("Browser", "Listing")
                btnDelete.Anchor = btnOk.Anchor
                btnDelete.Location = btnOk.Location
                btnOk.Visible = False
                btnCancel.Text = "Close"
            End If

            Dim dt As New DataTable
            With dt.Columns
                .Add("transaction_date", GetType(Date))
                .Add("code")
                .Add("customer_name")
                .Add("total_amount", GetType(Double))
            End With
            grdList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            Dim data As DataSet = Nothing
            With New TaskProcess(Me, grdList)
                .ShowLoader = grdList.Visible
                .Start(
                    Sub()
                        data = m_SuspendedSale.GetList(CashierSessionId, TerminalCode, CustomerCode)
                        With data
                            .Relations.Add(.Tables(0).Columns("code"), .Tables(1).Columns("suspend_code"))
                        End With
                    End Sub,
                    Sub()
                        With grdList
                            .DataSource = data

                            If .Rows.Count > 0 Then
                                .Rows(0).Activate()
                            Else
                                btnDelete.Enabled = False
                                btnOk.Enabled = False
                            End If
                        End With
                    End Sub)
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

End Class