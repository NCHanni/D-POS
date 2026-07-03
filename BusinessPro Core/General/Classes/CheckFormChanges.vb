#Region "Imports"

Imports Infragistics.Win.Misc, _
        Infragistics.Win.UltraWinEditors, _
        Infragistics.Win.UltraWinGrid

#End Region

Public Class CheckFormChanges

#Region "Declarations"

    Public Event ValueChanged(ByVal sender As Object, ByVal e As EventArgs)

#End Region

#Region "Properties"

    Property HasChanges As Boolean = False

    Private m_Enabled As Boolean = True
    Public Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal value As Boolean)
            m_Enabled = value
            HasChanges = False
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub ClearChanges()
        HasChanges = False
    End Sub

    Public Sub Add(ByVal _Control As Control)
        Try
            Select Case _Control.GetType.Name
                Case "UltraCheckEditor"
                    AddHandler DirectCast(_Control, UltraCheckEditor).CheckedValueChanged, AddressOf Control_ValueChanged
                Case "UltraGrid"
                    AddHandler DirectCast(_Control, UltraGrid).AfterCellUpdate, AddressOf Grid_AfterCellUpdate
                    AddHandler DirectCast(_Control, UltraGrid).AfterRowInsert, AddressOf Grid_AfterRowInsert
                    AddHandler DirectCast(_Control, UltraGrid).AfterRowsDeleted, AddressOf Grid_AfterRowsDeleted
                    AddHandler DirectCast(_Control, UltraGrid).AfterRowUpdate, AddressOf Grid_AfterRowUpdate
                    AddHandler DirectCast(_Control, UltraGrid).AfterRowInsert, AddressOf Grid_AfterRowInsert
                Case "UltraNumericEditor"
                    AddHandler DirectCast(_Control, UltraNumericEditor).ValueChanged, AddressOf Control_ValueChanged
                Case Else
                    If _Control.HasChildren Then
                        For Each ctrl As Control In _Control.Controls
                            Add(ctrl)
                        Next
                    Else
                        AddHandler _Control.TextChanged, AddressOf Control_ValueChanged
                    End If
            End Select

            ClearChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub Control_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If m_Enabled Then
            HasChanges = True
            RaiseEvent ValueChanged(sender, e)
        End If
    End Sub

    Private Sub Grid_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs)
        If m_Enabled Then
            HasChanges = True
        End If
    End Sub

    Private Sub Grid_AfterRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs)
        If m_Enabled Then
            HasChanges = True
        End If
    End Sub

    Private Sub Grid_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs)
        If m_Enabled Then
            HasChanges = True
        End If
    End Sub

    Private Sub Grid_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs)
        If m_Enabled Then
            HasChanges = True
        End If
    End Sub

#End Region

End Class