Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Friend Class FormDenomination

#Region "Properties"

    Property Data As DataTable

    Public Property TotalAmount() As Double
        Get
            Return txtTotalAmount.Value
        End Get
        Set(value As Double)
            txtTotalAmount.Value = value
        End Set
    End Property

#End Region

#Region "EventHandler"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                DialogResult = DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            grdDenomination.DataSource = Data
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            With grdDenomination
                .Rows.First.Activate()
                .Rows.First.Cells("pieces").Activate()
                .PerformAction(UltraGridAction.EnterEditMode)
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdDenomination_AfterCellUpdate(sender As Object, e As CellEventArgs) Handles grdDenomination.AfterCellUpdate
        Try
            If e.Cell.Column.Key = "pieces" Then
                grdDenomination.UpdateData()

                Dim denomination As Double
                denomination = e.Cell.Row.Cells("denomination").Value

                e.Cell.Row.Cells("amount").Value = denomination * e.Cell.Value
                TotalAmount = Data.Compute("SUM(amount)", "")
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdDenomination_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdDenomination.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns

                With .Bands(0)
                    .Override.AllowUpdate = DefaultableBoolean.True
                    .Override.HeaderAppearance.TextHAlign = HAlign.Center

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "denomination"
                                    .Header.Caption = "Denomination"
                                    .CellActivation = Activation.NoEdit
                                    .CellClickAction = CellClickAction.RowSelect
                                    .CellAppearance.TextHAlign = HAlign.Right
                                    .Format = "#,##0.00"
                                    .TabStop = False

                                Case "pieces"
                                    .Header.Caption = "Pieces"

                                Case "amount"
                                    .Header.Caption = "Amount"
                                    .CellActivation = Activation.NoEdit
                                    .CellClickAction = CellClickAction.RowSelect
                                    .CellAppearance.TextHAlign = HAlign.Right
                                    .Format = "#,##0.00"
                                    .TabStop = False

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

    Private Sub grdDenomination_KeyDown(sender As Object, e As KeyEventArgs) Handles grdDenomination.KeyDown
        Try
            With grdDenomination
                If e.KeyCode = Keys.Up Then
                    SendKeys.Send("+{TAB}")
                    e.SuppressKeyPress = True
                ElseIf e.KeyCode = Keys.Down Then
                    SendKeys.Send("{TAB}")
                    e.SuppressKeyPress = True
                ElseIf .ActiveCell IsNot Nothing AndAlso e.KeyCode = Keys.Escape Then
                    .ActiveCell.Row.Activate()
                ElseIf .ActiveCell Is Nothing AndAlso .ActiveRow IsNot Nothing AndAlso e.KeyCode = Keys.Tab Then
                    .ActiveRow.Cells("pieces").Activate()
                End If
            End With
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            grdDenomination.UpdateData()
            If TotalAmount > 0.0 Then
                DialogResult = DialogResult.OK
            Else
                Data.RejectChanges()
                Close()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

End Class