Imports Infragistics.Win.UltraWinGrid

Friend Class FormPaymentCreditCards

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences

    Private m_CardTypes As Dictionary(Of String, String)

#End Region

#Region "Constructor"

    Public Sub New(settings As SettingsPreferences)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
    End Sub

#End Region

#Region "Properties"

    Property CreditCards As DataTable
    Property RemainingAmountDue As Double

    Private m_TotalAmount As Double = 0.0

    Public ReadOnly Property TotalAmount() As Double
        Get
            Return m_TotalAmount
        End Get
    End Property

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.F5 AndAlso grdList.ActiveRow IsNot Nothing Then
                If Not grdList.ActiveRow.IsAddRow Then
                    If ShowQuestion("Selected payment will be removed. Do you want to continue?", "Confirm Remove") = QuestionResult.Yes Then
                        grdList.ActiveRow.Delete(False)
                    End If
                End If
            ElseIf e.KeyCode = Keys.Enter Then
                btnOk.PerformClick()
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
            With grdList
                .Focus()
                If .Rows.Count = 0 Then
                    .DisplayLayout.Bands(0).AddNew()
                End If
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_AfterCellUpdate(sender As Object, e As CellEventArgs) Handles grdList.AfterCellUpdate
        Try
            If e.Cell.Column.Key = "credit_card_code" Then
                e.Cell.Row.Cells("description").Value = m_CardTypes.Where(
                        Function(c)
                            Return c.Key = e.Cell.Value
                        End Function
                    ).First.Value
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles grdList.BeforeCellUpdate
        Try
            If IsDBNull(e.NewValue) Then
                e.Cancel = True
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns

                With .Bands(0)
                    With .Override
                        .AllowAddNew = AllowAddNew.TemplateOnBottom
                        .AllowDelete = Infragistics.Win.DefaultableBoolean.True
                        .AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                        .AutoResizeColumnWidthOptions = AutoResizeColumnWidthOptions.All
                        .ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
                        .HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center
                    End With

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "id"
                                    .Hidden = True
                                    .DefaultCellValue = -1
                                Case "credit_card_code"
                                    .Header.Caption = "Card Type"
                                    .SetWidth(120, 150)
                                    .Style = ColumnStyle.DropDownList
                                    .ValueList = .Band.Layout.ValueLists("CardTypes")
                                Case "card_number"
                                    .Header.Caption = "Card No."
                                    .MaxLength = 4
                                    .SetWidth(60, 70)
                                Case "card_holder"
                                    .MaxLength = 30
                                    .SetWidth(120, 250)
                                Case "bank_name"
                                    .MaxLength = 30
                                    .SetWidth(120, 250)
                                Case "card_approval_code"
                                    .Header.Caption = "Approval Code"
                                    .MaxLength = 30
                                    .SetWidth(100, 160)
                                Case "amount"
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .MaxValue = 999999.99
                                    .SetWidth(80, 100)
                                    .Style = ColumnStyle.DoubleNonNegative
                                Case Else
                                    .Hidden = True
                            End Select
                        End With
                    Next

                    With .Summaries
                        .Band.Layout.Grid.CalcManager = calcManager

                        With .Add(SummaryType.External, .Band.Columns("amount"), SummaryPosition.Right)
                            .Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
                            .Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                            .DisplayFormat = String.Format("REMAINING DUE: {0:#,##0.00}", RemainingAmountDue)
                        End With
                        With .Add(SummaryType.Sum, .Band.Columns("amount"), SummaryPosition.Right)
                            .Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
                            .Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                            .DisplayFormat = "TOTAL AMOUNT: {0:#,##0.00}"
                        End With
                    End With
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_KeyDown(sender As Object, e As KeyEventArgs) Handles grdList.KeyDown
        Try
            If e.KeyCode = Keys.Escape Then
                If grdList.ActiveRow Is Nothing Then
                    btnCancel.PerformClick()
                Else
                    grdList.ActiveRow.CancelUpdate()
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If IsValid() Then
                grdList.UpdateData()
                ComputeTotal()

                If TotalAmount > RemainingAmountDue Then
                    ShowWarning("Total paid amount is greater than the due amount!", "Payment Failed")
                Else
                    DialogResult = DialogResult.OK
                    Close()
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            RemainingAmountDue = Math.Round(RemainingAmountDue, 2)

            FillCreditCardTypes()
            grdList.DataSource = CreditCards

            If grdList.Rows.Count > 0 Then
                grdList.Focus()
                grdList.Rows(0).Activate()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub FillCreditCardTypes()
        Try
            With grdList.DisplayLayout
                If Not .ValueLists.Exists("CardTypes") Then
                    .ValueLists.Add("CardTypes")
                End If

                Dim cls As New CreditCardList
                cls.Fill()

                m_CardTypes = New Dictionary(Of String, String)(cls.Data.Rows.Count)

                For Each row As DataRow In cls.Data.Rows
                    .ValueLists("CardTypes").ValueListItems.Add(row("code"), row("description"))
                    m_CardTypes.Add(row("code"), row("description"))
                Next
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Public Sub ComputeTotal()
        Try
            m_TotalAmount = 0.0

            For Each row As UltraGridRow In grdList.Rows
                m_TotalAmount += CDblEx(row.Cells("amount").Value)
            Next

            m_TotalAmount = Math.Round(m_TotalAmount, 2)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Function IsValid() As Boolean
        Try
            grdList.ActiveRow = Nothing

            ClearInvalidFields()
            grdList.GetErrors("Payment", "card_name,card_number,card_holder,card_approval_code,card_type,credit_card_code", "id,sale_code", "amount")

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields())
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class