#Region "Imports"

Imports Infragistics.Win.Misc, _
        Infragistics.Win.UltraWinEditors, _
        Infragistics.Win.UltraWinGrid

#End Region

Friend Class CheckFormChanges

#Region "Properties"

    Private m_HasChanges As Boolean = False
    ReadOnly Property HasChanges() As Boolean
        Get
            Return m_HasChanges
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub ClearChanges()
        m_HasChanges = False
    End Sub

    Public Sub Add(ByVal _GroupBox As UltraGroupBox)
        Try
            For Each ctrl As Control In _GroupBox.Controls
                Select Case ctrl.GetType.Name
                    Case "UltraCheckEditor"
                        AddHandler DirectCast(ctrl, UltraCheckEditor).CheckedValueChanged, AddressOf Control_ValueChanged
                    Case "UltraGrid"
                        AddHandler DirectCast(ctrl, UltraGrid).AfterCellUpdate, AddressOf Grid_AfterCellUpdate
                        AddHandler DirectCast(ctrl, UltraGrid).AfterRowInsert, AddressOf Grid_AfterRowInsert
                        AddHandler DirectCast(ctrl, UltraGrid).AfterRowsDeleted, AddressOf Grid_AfterRowsDeleted
                        AddHandler DirectCast(ctrl, UltraGrid).AfterRowUpdate, AddressOf Grid_AfterRowUpdate
                    Case "ucListSearch"
                        For Each ctrl2 As Control In ctrl.Controls
                            If ctrl2.GetType.Name = "UltraGroupBox" Then
                                Add(ctrl2)
                            End If
                        Next
                    Case "ucListWithDateRange"
                        For Each ctrl2 As Control In ctrl.Controls
                            If ctrl2.GetType.Name = "UltraGroupBox" Then
                                Add(ctrl2)
                            End If
                        Next
                    Case Else
                        AddHandler ctrl.TextChanged, AddressOf Control_ValueChanged
                End Select
            Next
            ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub Control_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        m_HasChanges = True
    End Sub

    Private Sub Grid_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs)
        m_HasChanges = True
    End Sub

    Private Sub Grid_AfterRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs)
        m_HasChanges = True
    End Sub

    Private Sub Grid_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs)
        m_HasChanges = True
    End Sub

    Private Sub Grid_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs)
        m_HasChanges = True
    End Sub

#End Region

End Class