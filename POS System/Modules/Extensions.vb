#Region "Imports"

Imports System.Runtime.CompilerServices

Imports Infragistics.Win.UltraWinGrid, _
        Infragistics.Win.Misc

#End Region

Friend Module Extensions

#Region "ToolStripMenuItem"

    <Extension()>
    Public Sub CheckAccess(ByVal _Button As UltraButton)
        Try
            If Current.Rights Is Nothing Then Exit Sub
            _Button.Enabled = Current.Rights.IsAllowed(_Button.Tag, Rights.AccessRights.View)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub CheckEdit(ByVal _Grid As UltraGrid)
        Try
            If Current.Rights Is Nothing Then Exit Sub
            For Each row As Infragistics.Win.UltraWinGrid.UltraGridRow In _Grid.Rows
                If Current.Rights.IsAllowed(_Grid.Tag, Rights.AccessRights.Edit) Then
                    row.Activation = Activation.AllowEdit
                Else
                    row.Activation = Activation.NoEdit
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Module