Imports Infragistics.Win.UltraWinEditors

Friend Class FormPriceLookUp

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

    Property IsCustomerScPwd As Boolean
    Property ItemList As DataTable
    Property HasVATExclusiveItems As Boolean

#End Region

#Region "Event Handlers"

    Private Sub txtItemSearch_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles txtItemSearch.EditorButtonClick
        Try
            FindItem(txtItemSearch.Text)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub txtItemSearch_TextChanged(sender As Object, e As EventArgs) Handles txtItemSearch.TextChanged
        If lblDescription.Text <> "" AndAlso lblPrice.Text = "0.00" Then
            lblDescription.Text = ""
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub FindItem(keyword As String, Optional openBrowser As Boolean = False)
        Try
            Dim drResult As DataRow()
            Dim dtItems As DataTable
            Dim itemPrice As Double

            drResult = ItemList.Select(
                "codename LIKE '%" & keyword.Replace("'", "''") &
                "%' OR sku LIKE '%" & keyword.Replace("'", "''") &
                "%' OR description LIKE '%" & keyword.Replace("'", "''") &
                "%'", "")

            txtItemSearch.Focus()

            If txtItemSearch.Focused Then
                txtItemSearch.SelectAll()
            End If

            If drResult Is Nothing OrElse drResult.Length = 0 Then
                Beep()
                lblDescription.Text = "ITEM NOT FOUND!"
                lblPrice.Text = "0.00"
                Return
            Else
                dtItems = drResult.CopyToDataTable
            End If

            If dtItems.Rows.Count > 1 OrElse openBrowser Then
                Dim frm As New FormItemBrowser(m_Settings)
                With frm
                    .ItemList = dtItems
                    .IsCustomerScPwd = IsCustomerScPwd
                    .HasVATExclusiveItems = HasVATExclusiveItems
                End With

                With frm
                    If LaunchForm(frm) = Windows.Forms.DialogResult.OK Then
                        lblDescription.Text = frm.SelectedItems.Rows(0).Item("Description")
                        itemPrice = frm.SelectedItems.Rows(0).Item("price")
                    End If
                End With
            Else
                lblDescription.Text = dtItems.Rows(0)("Description")
                itemPrice = dtItems.Rows(0)("price")
            End If

            lblPrice.Text = itemPrice.ToString("#,##0.00")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Try
            Select Case keyData
                Case Keys.Enter
                    If Not String.IsNullOrWhiteSpace(txtItemSearch.Text) Then
                        Dim button As EditorButtonBase = txtItemSearch.ButtonsRight(0)
                        txtItemSearch_EditorButtonClick(New Object(), New EditorButtonEventArgs(button, Nothing))
                    End If
                Case Keys.F2, Keys.F4
                    FindItem(txtItemSearch.Text, True)
            End Select

            Return MyBase.ProcessCmdKey(msg, keyData)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Function

#End Region

End Class