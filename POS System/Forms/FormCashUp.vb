Friend Class FormCashUp

#Region "Declarations"

    Private ReadOnly m_Settings As Core.SettingsPreferences

    Private m_AuditTrail As AuditTrail
    Private m_TrackChanges As TrackChanges
    Private m_Initialized As Boolean
    Private m_CashierSessionId As Long = Current.Settings.CashierSessionId

    Private m_DenominationData As DataTable
    Private m_XReadingData As DataSet
    Private m_LockedCashEnd As Boolean
    Private m_AuthorizedBy As String
    Private m_AuthorizedRoleName As String

    Public Sub New(settings As Core.SettingsPreferences)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
    End Sub

#End Region

#Region "Properties"

    Property IsPreviousCashupUnfinalized As Boolean
    Property IsFinalized As Boolean

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
#If Not DEBUG Then
            If Not IsFinalized AndAlso m_TrackChanges.HasChanges Then
                If ShowQuestion(
                    "Closing this form will undo the inputted cash-end amount.\n\n" &
                    "Do you want to continue?", "") <> QuestionResult.Yes Then
                    e.Cancel = True
                End If
            End If
#End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            If IsFinalized Then
                Dim formMain As FormMain = Current.FormMain
                formMain.RestartApplication()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.F9
                    If e.Control Then
#If Not DEBUG Then
                        If Not IsFinalized Then
                            Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "PRINT UNFINALIZED X-READING REPORT"}
                                If LaunchForm(frmAuth, True, Me) <> DialogResult.OK Then
                                    Return
                                End If
                            End Using
                        End If
#End If
                        Print()
                    Else
                        btnFinalizeCashEnd.PerformClick()
                    End If
                Case Keys.F11
                    txtPettyCashOut_EditorButtonClick(numCashEnd, Nothing)
            End Select
        Catch ex As Exception
            ex.ShowError
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

    Private Sub btnFinalizeCashEnd_Click(sender As Object, e As EventArgs) Handles btnFinalizeCashEnd.Click
        Try
            If (New SuspendedSale).HasPending(Current.Settings.CashierSessionId) Then
                If IsPreviousCashupUnfinalized Then
                    ShowWarning("Please remove all pending suspended sales to continue!", "Finalize Failed")

                    Using frm As New FormSuspendedSalesBrowser(m_Settings) With {
                        .CashierSessionId = Current.Settings.CashierSessionId,
                        .IsFromCashupForm = True,
                        .TerminalCode = Current.Settings.TerminalCode
                    }
                        LaunchForm(frm, True, Me)
                    End Using
                Else
                    ShowWarning("Please finalize or remove all pending suspended sales to continue!", "Finalize Failed")
                End If
                Return
            End If

            With New CashUp
                Enabled = False
                Using f As New FormCashUpConfirm With {
                        .CashEndAmount = numCashEnd.Value,
                        .ShortcutKey = Keys.F9,
                        .UseBarcode = m_Settings.UseBarcodeLogin AndAlso Current.User.HasBarcode
                }
                    If LaunchForm(f) <> DialogResult.OK Then
                        Return
                    End If
                End Using

                .CashierSessionId = m_CashierSessionId

                m_XReadingData = Nothing
                m_XReadingData = .GetCashUp(numCashEnd.Value, m_DenominationData)
                m_LockedCashEnd = True

                If m_XReadingData IsNot Nothing AndAlso Finalize() Then
                    IsFinalized = True

                    If IsPreviousCashupUnfinalized Then
                        IsPreviousCashupUnfinalized = False
                        grpDetails.Text = ""
                        Height -= 20
                    End If

                    btnFinalizeCashEnd.Enabled = False
                    numCashEnd.Enabled = False
                    btnRePrintShiftReport.Visible = True
                    Refresh()

                    Print()
                    SaveAuditTrail("Generated Shift Report (X-Reading)")
                End If
            End With
        Catch ex As Exception
            ex.ShowError
        Finally
            Enabled = True
        End Try
    End Sub

    Private Sub btnRePrintShiftReport_Click(sender As Object, e As EventArgs) Handles btnRePrintShiftReport.Click
        Try
            Print()
            SaveAuditTrail("Reprint Shift Report (X-Reading)")
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnUndoFinalize_Click(sender As Object, e As EventArgs) Handles btnUndoFinalize.Click
        Try
            With New CashUp
                .CashierSessionId = m_CashierSessionId

                If m_LockedCashEnd Then
                    Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "UNLOCK CASH END AMOUNT"}
                        If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                            btnFinalizeCashEnd.Enabled = True
                            btnUndoFinalize.Enabled = False

                            .LockCashEnd(Current.Settings.CashierSessionId, numCashEnd.Value, True)

                            m_LockedCashEnd = False
                        End If
                    End Using
                End If
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

#End Region

#Region "TextBox"

    Private Sub txtPettyCashOut_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles numCashEnd.EditorButtonClick
        Try
            Using frm As New FormDenomination
                With frm
                    If Not (m_DenominationData.Compute("SUM(amount)", "") = 0) Then
                        ' Authorize the cashier for recounting cash end
                        Using frmAuth As New FormAuthorization(m_Settings) With {.Message = "RECOUNT CASH END AMOUNT"}
                            If LaunchForm(frmAuth, True, Me) = DialogResult.OK Then
                                m_AuthorizedBy = frmAuth.AuthorizedBy
                                m_AuthorizedRoleName = frmAuth.AuthorizedRoleName
                            Else
                                Return
                            End If
                        End Using
                    End If

                    .Data = m_DenominationData.Copy

                    If LaunchForm(frm, True, Me) = DialogResult.OK Then
                        m_XReadingData = Nothing
                        m_DenominationData = .Data
                        numCashEnd.Value = .TotalAmount
                    End If
                End With
            End Using
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdSummary_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs)
        Try
            With e.Layout
                .AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
                .Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select
                .Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
                .Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

                With .Bands(0)
                    .HeaderVisible = True
                    .Header.Caption = "Summary"
                    .ColHeadersVisible = False
                    .Columns("data1").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                    .Columns("data1").MaxWidth = 100
                    .Columns("data1").MinWidth = 100
                    .Columns("data2").Format = "#,##0.00"
                    .Columns("data2").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                    .Columns("data2").MaxWidth = 100
                    .Columns("data2").MinWidth = 100
                End With
                .Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
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
            m_AuditTrail = New AuditTrail
            m_TrackChanges = New TrackChanges
            m_TrackChanges.Add(numCashEnd)

            If IsPreviousCashupUnfinalized Then
                grpDetails.Text = "Previous Cash-Up has not been finalized! Please do so to proceed to today's session."
                Height += 20
            End If

            txtCashierName.Text = Current.User.Name
            txtCashierUserName.Text = Current.User.UserName
            txtCashInDate.Text = Current.Settings.CashInDate.ToString("MM/dd/yyyy hh:mm tt")
            txtCashOutDate.Text = ""
            numCashBeg.Value = Current.Settings.CashInAmount
            numCashEnd.Value = 0.0

            Dim dt As New DataTable("Table")
            With dt.Columns
                .Add("description")
                .Add("data1")
                .Add("data2", GetType(Double))
            End With

            InitializeDenomination()

            m_TrackChanges.ClearChanges()
            m_Initialized = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeDenomination()
        Try
            Dim dt As New DataTable()
            With dt.Columns
                .Add("denomination", GetType(Double))
                .Add("pieces", GetType(Integer)).DefaultValue = 0
                .Add("amount", GetType(Double)).DefaultValue = 0
            End With

            dt.NewRow()
            With dt.Rows
                .Add(1000, 0, 0)
                .Add(500, 0, 0)
                .Add(200, 0, 0)
                .Add(100, 0, 0)
                .Add(50, 0, 0)
                .Add(20, 0, 0)
                .Add(10, 0, 0)
                .Add(5, 0, 0)
                .Add(1, 0, 0)
                .Add(0.25, 0, 0)
                .Add(0.1, 0, 0)
                .Add(0.05, 0, 0)
                .Add(0.01, 0, 0)
            End With

            m_DenominationData = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Shadows Sub Refresh()
        Try
            With New CashUp
                .CashierSessionId = m_CashierSessionId

                Dim dt = .GetCashEnd(Current.Settings.CashierSessionId)

                If dt.Rows.Count > 0 Then
                    Dim dr = dt(0)

                    If CBool(dr("locked")) Then
                        numCashEnd.Value = dr("amount")
                        m_LockedCashEnd = True

                        btnClose.Focus()
                    End If
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Shadows Function Finalize() As Boolean
        Try
            Dim cu As New CashUp
            cu.CashierSessionId = Current.Settings.CashierSessionId

            cu.FinalizeCashUp(
                Current.User.Code,
                numCashEnd.Value,
                Current.Settings.TerminalCode,
                m_DenominationData)

            txtCashOutDate.Text = cu.ServerDate.ToString("MM/dd/yyyy hh:mm tt")
            m_CashierSessionId = cu.CashierSessionId

            ShowMessage(
                "Finalize Cash-up Successful!",
                "Finalize Successful", MessageIcon.Information)

            ' 3907 - Cash Up (F11) : Removed misleading message where application doesn't restart after form closing.
            ' \n\nApplication will restart after closing this form.

            Refresh()

            btnFinalizeCashEnd.Enabled = False

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub Print()
        Try
            With New Reporting(m_Settings)
                .Cashier = Current.User.Name
                .TransactionDate = Now
                .HasOwnReportViewer = True

                If m_XReadingData Is Nothing Then
                    With New CashUp
                        .CashierSessionId = m_CashierSessionId

                        m_XReadingData = .GetCashUp(
                            numCashEnd.Value,
                            m_DenominationData)
                    End With
                End If

                Dim rpt = .ShowCashup(m_CashierSessionId, m_XReadingData)

                If rpt IsNot Nothing Then
                    With DirectCast(rpt, CashupReport)
                        .PrintToPrinter(1, False, -1, -1)
                    End With
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveAuditTrail(activity As String)
        Try
            With New TaskProcess(Me)
                .Start(
                    Sub()
                        With m_AuditTrail
                            .TerminalCode = Current.Settings.TerminalCode
                            .UserCode = Current.User.Code
                            .UserName = Current.User.Name
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

End Class