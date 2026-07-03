Imports Infragistics.Win.UltraWinGrid

Friend Class FormPaymentGiftCertificates

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences

    Public Sub New(settings As SettingsPreferences)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
    End Sub

#End Region

#Region "Property"

    Property GiftCertificates As DataTable
    Property RemainingAmountDue As Double

    Private m_ExcessAmount As Double
    Private m_TotalAmount As Double

    Public ReadOnly Property ExcessAmount() As Double
        Get
            Return m_ExcessAmount
        End Get
    End Property

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

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
            Throw
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
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
            If e.Cell.Column.Key = "gc_no" Then
                ValidateData(e.Cell)
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

    Private Sub grdList_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdList.InitializeLayout
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
                            Select Case column.Key
                                Case "gc_no"
                                    .Header.Caption = "GC No."
                                    .SetWidth(100, 120)
                                Case "description"
                                    .CellActivation = Activation.NoEdit
                                    .TabStop = False
                                Case "amount"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .SetWidth(100, 120)
                                    .Style = ColumnStyle.DoubleNonNegative
                                    .TabStop = False
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

    Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
        Try
            grdList.UpdateData()
            ComputeTotal()

            If TotalAmount > RemainingAmountDue Then
                m_ExcessAmount = TotalAmount - RemainingAmountDue
            End If

            DialogResult = DialogResult.OK
            Close()
        Catch ex As DataException
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

#End Region

#Region "Method"

    Private Sub Initialize()
        Try
            RemainingAmountDue = Math.Round(RemainingAmountDue, 2)
            grdList.DataSource = GiftCertificates

            If grdList.Rows.Count > 0 Then
                grdList.Focus()
                grdList.Rows(0).Activate()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComputeTotal()
        Try
            m_TotalAmount = 0.0

            For Each row As UltraGridRow In grdList.Rows
                m_TotalAmount += CDblEx(row.Cells("amount").Value)
            Next

            m_TotalAmount = Math.Round(m_TotalAmount, 2)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ValidateData(gcNoCell As UltraGridCell)
        Try
            Dim gcNo As String = gcNoCell.Text.Trim
            Dim isFound As Boolean
            Dim description As String = String.Empty
            Dim expiryDate As Date
            Dim amount As Double
            Dim isReleased As Boolean
            Dim isUsed As Boolean
            Dim errorText As String = String.Empty

            If String.IsNullOrWhiteSpace(gcNo) Then
                Return
            End If

            With New GiftCertificateList
                .Code = gcNo
                If .Fill(True) Then
                    isFound = True
                    description = .Description
                    Date.TryParse(.ExpiryDate, expiryDate)
                    amount = .Amount
                    isReleased = .IsSold
                    isUsed = .IsUsed
                End If
            End With

            If Not isFound Then
                errorText = String.Format("GC No. {0} does not exist!", gcNo)
            ElseIf IsDateValid(expiryDate) AndAlso Now.Date > expiryDate Then
                errorText = String.Format("GC No. {0} is no longer valid!", gcNo)
            ElseIf Not isReleased Then
                errorText = String.Format("GC No. {0} has not released yet!", gcNo)
            ElseIf isUsed Then
                errorText = String.Format("GC No. {0} has already been used!", gcNo)
            End If

            With gcNoCell.Row
                If String.IsNullOrEmpty(errorText) Then
                    .Cells("description").Value = description
                    .Cells("amount").Value = amount
                Else
                    ShowWarning(errorText, "Invalid Gift Certificate")
                    .CancelUpdate()
                End If
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class