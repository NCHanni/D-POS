Imports Infragistics.Win.UltraWinGrid

Friend Class FormCreditMemoBrowser

#Region "Enumerator"

    Public Enum OperationType
        Add
        View
    End Enum

#End Region

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences
    Private ReadOnly m_Operation As OperationType

    Private m_LineNo As String
    Private m_LineNavNo As String
    Private m_LineCustomerName As String
    Private m_LineAmount As Double

#End Region

#Region "Property"

    Property CustomerNo As String
    Property CustomerName As String
    Property TotalSales As Double
    Property TotalAmount As Double

    Public Property Data As DataTable
        Get
            Return grdCreditMemo.DataSource
        End Get
        Set(ByVal value As DataTable)
            grdCreditMemo.DataSource = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Sub New(settings As SettingsPreferences, Optional operation As OperationType = OperationType.Add)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
        m_Operation = operation
    End Sub

#End Region

#Region "Event Handler"

#Region "Form"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If DialogResult <> DialogResult.OK Then
                DialogResult = DialogResult.Cancel
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            If m_Operation <> OperationType.View Then
                If e.KeyCode = Keys.F5 OrElse e.KeyCode = Keys.Delete Then
                    If grdCreditMemo.ActiveRow IsNot Nothing Then
                        If Not grdCreditMemo.ActiveRow.IsAddRow Then
                            If ShowQuestion("Selected credit memo will be removed. Do you want to continue?", "Confirm Delete") = QuestionResult.Yes Then
                                grdCreditMemo.ActiveRow.Delete(False)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            If m_Operation = OperationType.Add Then
                With grdCreditMemo.DisplayLayout.Bands(0).AddNew()
                    .Activate()
                    .Cells("CreditMemoNo").Activate()
                End With

                grdCreditMemo.PerformAction(UltraGridAction.EnterEditMode)
                lblHint.Visible = True
            Else
                AcceptButton = btnOk
                btnOk.Visible = False
                btnCancel.Text = "Close"
                lblHint.Visible = False
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
        Try
            If TotalAmount > TotalSales Then
                ShowWarning("Total Sales must be equal or greater than the Credit Memo Total.", "Credit Memo")
            Else
                grdCreditMemo.UpdateData()

RecheckEmptyRows:
                For Each row As DataRow In Data.Rows
                    If String.IsNullOrWhiteSpace(row("CreditMemoNo")) Then
                        row.Delete()
                        GoTo RecheckEmptyRows
                    End If
                Next

                DialogResult = DialogResult.OK
            End If
        Catch ex As DataException
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdCreditMemo_AfterCellUpdate(sender As Object, e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles grdCreditMemo.AfterCellUpdate
        Try
            Static Dim skipEvent As Boolean

            If e.Cell.Column.Key = "CreditMemoNo" Then
                If Not skipEvent Then
                    skipEvent = True
                    e.Cell.Row.Cells("No.").Value = m_LineNo
                    e.Cell.Row.Cells("CreditMemoNo").Value = m_LineNavNo
                    e.Cell.Row.Cells("CustomerName").Value = m_LineCustomerName
                    e.Cell.Row.Cells("Amount").Value = m_LineAmount
                    skipEvent = False
                End If
            End If

            If e.Cell.Column.Key = "Amount" Then
                ComputeTotal()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdCreditMemo_AfterRowsDeleted(sender As Object, e As System.EventArgs) Handles grdCreditMemo.AfterRowsDeleted
        Try
            ComputeTotal()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdCreditMemo_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles grdCreditMemo.BeforeCellUpdate
        Try
            If e.Cell.Column.Key = "CreditMemoNo" Then
                If IsDBNull(e.NewValue) OrElse String.IsNullOrWhiteSpace(e.NewValue) Then
                    e.Cancel = True
                ElseIf CheckForDuplicate(e.NewValue, e.Cell.Row.Index) Then
                    e.Cancel = True
                ElseIf ValidateNo(e.NewValue) = False Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdCreditMemo_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles grdCreditMemo.InitializeLayout
        Try
            With e.Layout
                .AutoFitStyle = AutoFitStyle.ResizeAllColumns

                With .Bands(0)
                    If m_Operation = OperationType.Add Then
                        .Override.AllowAddNew = AllowAddNew.TemplateOnBottom
                        .Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                    Else
                        .Override.AllowAddNew = AllowAddNew.No
                    End If

                    .Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center

                    For Each column In .Columns
                        With column
                            Select Case .Key
                                Case "CreditMemoNo"
                                    If m_Operation = OperationType.View Then
                                        .CellActivation = Activation.NoEdit
                                    End If

                                    .Header.Caption = "Credit Memo No."
                                    .SetWidth(110, 130)

                                Case "CustomerName"
                                    .CellActivation = Activation.NoEdit
                                    .Header.Caption = "Customer"
                                    .SetWidth(250, 500)

                                Case "Amount"
                                    .CellActivation = Activation.NoEdit
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                    .Format = "#,##0.00"
                                    .SetWidth(80, 100)
                                    .Style = ColumnStyle.DoubleNonNegative

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

    Private Sub grdCreditMemo_KeyDown(sender As Object, e As KeyEventArgs) Handles grdCreditMemo.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If grdCreditMemo.ActiveRow IsNot Nothing AndAlso grdCreditMemo.ActiveRow.IsAddRow Then
                    btnOk.PerformClick()
                End If
            ElseIf e.KeyCode = Keys.Escape Then
                If grdCreditMemo.ActiveRow Is Nothing Then
                    btnCancel.PerformClick()
                Else
                    grdCreditMemo.ActiveRow.CancelUpdate()
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#End Region

#Region "Method"

    Private Sub Initialize()
        Try
            grdCreditMemo.Focus()

            If Data.Rows.Count > 0 Then
                ComputeTotal()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComputeTotal()
        Try
            TotalAmount = 0.0

            For Each row As UltraGridRow In grdCreditMemo.Rows
                TotalAmount += CDblEx(row.Cells("Amount").Value)
            Next

            lblTotal.Text = String.Format(lblTotal.Tag, TotalAmount.ToString("#,##0.00"))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function CheckForDuplicate(cmNo As String, cmIndex As Integer) As Boolean
        Try
            For Each row As UltraGridRow In grdCreditMemo.Rows
                If cmIndex <> row.Index Then
                    If String.Compare(cmNo, row.Cells("CreditMemoNo").Text.Trim, True) = 0 Then
                        ShowWarning("Credit Memo No. " & cmNo & " is already used!", "Credit Memo Error")
                        Return True
                    End If
                End If
            Next

            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function ValidateNo(cmNo As String) As Boolean
        Try
            Dim notFound As Boolean

            Dim creditMemo As New CreditMemo

            If creditMemo.FillBasic(cmNo, CustomerNo) Then
                If creditMemo.IsProcessed Then
                    ShowWarning("Credit Memo has already been processed!", "Credit Memo Error")
                    Return False
                End If

                m_LineNo = creditMemo.Code
                m_LineNavNo = creditMemo.NAVCode
                m_LineCustomerName = creditMemo.CustomerName
                m_LineAmount = creditMemo.TotalAmount
            Else
                notFound = True
            End If

            If notFound Then
                ShowWarning("Credit Memo No. " & cmNo & " not found for the selected customer!", "Credit Memo Error")
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class