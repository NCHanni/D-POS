Friend Class FormSetQty

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences
    Private m_TrackChanges As TrackChanges

    Public Sub New(settings As SettingsPreferences)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
    End Sub

#End Region

#Region "Properties"

    Property OldQuantity As Double
    Property NewQuantity As Double
    Property UnitPrice As Double
    Property UOMCode As String
    Property QtyPerUOM As Double

#End Region

#Region "Event Handlers"

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

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Button"

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click
        Try
            Dim newQty As Double = CDblEx(numQty.Value)

            UnitPrice *= QtyPerUOM

            If newQty > 0 Then
                NewQuantity = newQty
                UOMCode = cbxUOM.Text
                DialogResult = DialogResult.OK
            Else
                ShowWarning("Quantity cannot be zero!", "Set Qty Failed")
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_TrackChanges = New TrackChanges
            m_TrackChanges.Add(numQty)
            m_TrackChanges.Add(cbxUOM)

            numQty.MaxValue = 100000
            numQty.Value = OldQuantity

            With cbxUOM
                .Items.Add(UOMCode)
                .Text = UOMCode
                .Enabled = False
            End With

            m_TrackChanges.ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class