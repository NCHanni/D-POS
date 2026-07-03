Imports Infragistics.Win.UltraWinGrid

Friend Class FormAuditTrail

#Region "Declarations"

    Private Const MODULE_NAME As String = "Audit Trail"
    Private Const MODULE_GROUP As String = "Reports"
    Private Const PRINT_NOTE As String = "*Use standard 8.5x11 paper size (Letter)"

    Private m_LoadingData As Boolean

#End Region

#Region "Event Handler"

#Region "Form"

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            btnRefresh.PerformClick()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            PrintReport()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click, btnPrint.Click
        Try
            LoadData(dtpDateRange.StartDate, dtpDateRange.EndDate)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

#End Region

#Region "Others"

    Private Sub cmbFilter_ValueChanged(sender As Object, e As EventArgs) Handles cmbUser.ValueChanged, cmbTerminal.ValueChanged
        Try
            If Not m_LoadingData Then
                FilterData()
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdAuditTrail_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdAuditTrail.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns
                .Override.CellClickAction = CellClickAction.RowSelect
                .Override.HeaderClickAction = HeaderClickAction.Select
                .Override.RowSizing = RowSizing.Fixed

                With .Bands(0)
                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "cashier_name"
                                    .Header.Caption = "User"
                                    .SetWidth(120, 200)
                                Case "terminal"
                                    .SetWidth(100, 150)
                                Case "activity"
                                    .CellMultiLine = Infragistics.Win.DefaultableBoolean.True
                                Case "date"
                                    .Hidden = (dtpDateRange.StartDate.Date = dtpDateRange.EndDate.Date)
                                    .SetWidth(70, 80)
                                Case "time"
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

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("transaction_code")
                .Add("cashier_name")
                .Add("terminal")
                .Add("activity")
                .Add("date", GetType(Date))
                .Add("time")
            End With

            dtpDateRange.StartDate = Today
            dtpDateRange.EndDate = Today

            grdAuditTrail.DataSource = dt
            btnPrint.Enabled = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadData(fromDate As DateTime, toDate As DateTime)
        Try
            Dim auditTrail As New AuditTrail
            Dim data As New DataTable
            Dim users As New DataTable
            Dim terminals As New DataTable
            Dim emptyRow As DataRow

            m_LoadingData = True

            With New TaskProcess(Me, grdAuditTrail)
                .ShowLoader = True
                .Start(
                    Sub()
                        data = auditTrail.GetAuditTrailByDateRange(fromDate.Date, toDate.AddDays(1).AddSeconds(-1))

                        users = data.DefaultView.ToTable(True, "cashier_name")
                        emptyRow = users.NewRow
                        users.Rows.InsertAt(emptyRow, 0)

                        terminals = data.DefaultView.ToTable(True, "terminal")
                        emptyRow = terminals.NewRow
                        terminals.Rows.InsertAt(emptyRow, 0)
                    End Sub,
                    Sub()
                        grdAuditTrail.DataSource = data

                        cmbUser.DataSource = users
                        cmbTerminal.DataSource = terminals

                        With btnPrint
                            If data.Rows.Count > 0 Then
                                .CheckPrint(MODULE_NAME, MODULE_GROUP)

                                If .Enabled Then
                                    lblNote.Text = PRINT_NOTE
                                Else
                                    lblNote.Text = "You have no print access to this module."
                                End If
                            Else
                                .Enabled = False
                                lblNote.Text = ""
                            End If
                        End With

                        m_LoadingData = False
                    End Sub)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FilterData()
        Try
            With grdAuditTrail.DisplayLayout.Bands(0)
                .ColumnFilters.ClearAllFilters()
                If cmbUser.SelectedIndex > 0 Then
                    .ColumnFilters("cashier_name").FilterConditions.Add(FilterComparisionOperator.Equals, cmbUser.Text)
                End If
                If cmbTerminal.SelectedIndex > 0 Then
                    .ColumnFilters("terminal").FilterConditions.Add(FilterComparisionOperator.Equals, cmbTerminal.Text)
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PrintReport()
        Try
            Dim dt As DataTable = grdAuditTrail.DataSource

            If cmbUser.SelectedIndex > 0 Then
                dt.DefaultView.RowFilter = "cashier_name=" & cmbUser.Text.Quote(False)
            End If
            If cmbTerminal.SelectedIndex > 0 Then
                dt.DefaultView.RowFilter = "terminal=" & cmbTerminal.Text.Quote(False)
            End If

            With New Reporting
                .ShowAuditTrail(
                    dt.DefaultView.ToTable,
                    dtpDateRange.StartDate,
                    dtpDateRange.EndDate,
                    cmbUser.Text,
                    cmbTerminal.Text)
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class