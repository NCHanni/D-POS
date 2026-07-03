Imports Infragistics.Win.UltraWinGrid

Friend Class FormSeniorDiscountDialog

#Region "Property"

    Property CustomerCode As String
    Property IsWalkin As Boolean

    Property DiscountType As String
    Property IDNo As String
    Property CustomerName As String
    Property Gender As String
    Property BirthDate As Date
    Property IssuedDate As Date

    Property Data As DataTable

#End Region

#Region "Event Handlers"

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub dtpCalendarCombo_KeyDown(sender As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo, e As KeyEventArgs) _
        Handles dtpBirthdate.KeyDown,
                dtpDateIssued.KeyDown
        Try
            If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down Then
                If IsDBNull(sender.Value) Then
                    sender.Value = Now.Date
                    e.Handled = True
                End If
            ElseIf e.KeyCode = Keys.Enter Then
                btnOk.PerformClick()
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub dtpCalendarCombo_ValueChanged(sender As Infragistics.Win.UltraWinSchedule.UltraCalendarCombo, e As EventArgs) _
        Handles dtpBirthdate.ValueChanged,
                dtpDateIssued.ValueChanged
        Try
            If IsDBNull(dtpBirthdate.Value) OrElse IsDBNull(dtpDateIssued.Value) Then
                Return
            End If

            If sender.Value > Now.Date Then
                sender.Value = Now.Date
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Try
            Data.Rows.RemoveAt(0)
            Data.AcceptChanges()

            DialogResult = DialogResult.OK
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(sender As System.Object, e As System.EventArgs) Handles btnOk.Click
        Try
            If CheckRequired() Then
                Dim row As DataRow = Data.Rows(0)

                row("discount_type") = cmbDiscountType.Value
                row("name") = txtCustomerName.Text.Trim
                row("id_no") = txtIDNo.Text.Trim
                row("gender") = cmbGender.Value
                row("birthdate") = GetDateForVariable(dtpBirthdate.Value)
                row("issued_date") = GetDateForVariable(dtpDateIssued.Value)

                Data.AcceptChanges()

                DialogResult = DialogResult.OK
            Else
                DialogResult = DialogResult.None
            End If
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
            If Data.Rows.Count = 0 Then
                Data.Rows.Add(DiscountType, CustomerName, IDNo, Gender, BirthDate, IssuedDate)
                btnRemove.Visible = False
            End If

            Dim foreColorDisabled As Color = Color.FromKnownColor(KnownColor.ControlText)
            If Not (IsWalkin OrElse String.IsNullOrEmpty(DiscountType)) Then
                With cmbDiscountType
                    .Enabled = False
                    .Appearance.ForeColorDisabled = foreColorDisabled
                End With
                With txtCustomerName
                    .Enabled = False
                    .Appearance.ForeColorDisabled = foreColorDisabled
                End With
            End If

            Dim row As DataRow = Data.Rows(0)
            cmbDiscountType.Value = row("discount_type")
            txtCustomerName.Text = row("name")
            txtIDNo.Text = row("id_no")
            cmbGender.Value = row("gender")
            dtpBirthdate.Value = GetDateForControl(row("birthdate"))
            dtpDateIssued.Value = GetDateForControl(row("issued_date"))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function CheckRequired() As Boolean
        Try
            ClearInvalidFields()

            If cmbDiscountType.Text = "" Then
                AddToInvalidFields(cmbDiscountType.Tag & " is required")
            End If

            If txtCustomerName.Text = "" Then
                AddToInvalidFields(txtCustomerName.Tag & " is required")
            End If

            If txtIDNo.Text = "" Then
                AddToInvalidFields(txtIDNo.Tag & " is required")
            End If

            If cmbGender.Text = "" Then
                AddToInvalidFields(cmbGender.Tag & " is required")
            End If

            If Not IsDateValid(dtpBirthdate.Value) Then
                AddToInvalidFields(dtpBirthdate.Tag & " is not valid")
            End If

            If Not IsDateValid(dtpDateIssued.Value) Then
                AddToInvalidFields(dtpDateIssued.Tag & " is not valid")
            End If

            If IsDateValid(dtpBirthdate.Value) AndAlso IsDateValid(dtpDateIssued.Value) Then
                If dtpBirthdate.Value > dtpDateIssued.Value Then
                    AddToInvalidFields("Birthdate must not be greater than Date Issued")
                ElseIf Not SCValidAge() Then
                    AddToInvalidFields("Age is not valid for Senior Citizen discount")
                End If
            End If

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields())
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SCValidAge() As Boolean
        Try
            If cmbDiscountType.Value = "SC" Then
                If IsDate(dtpBirthdate.Value) AndAlso IsDate(dtpDateIssued.Value) AndAlso GetAge(dtpBirthdate.Value, dtpDateIssued.Value) < 60 Then
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class