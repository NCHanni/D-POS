#Region "Imports"

Imports Infragistics.Win.UltraWinEditors
Imports Infragistics.Win.UltraWinGrid

#End Region

Public Class TrackChanges

#Region "Custom Events"

    ''' <summary>
    ''' Raised whenever a monitored control has changed its value.
    ''' </summary>
    ''' <remarks>Triggered only if new value is not similar to the current HasChanged value or if forced.</remarks>
    Public Event ValueChanged As EventHandler(Of TrackChangesEventArgs)

#End Region

#Region "Properties"

    ''' <summary>
    ''' Returns/sets the value to determine whether monitoring of control changes is enabled or not.
    ''' </summary>
    Public Property Enabled As Boolean = True

    Private _HasChanges As Boolean
    ''' <summary>
    ''' Returns/sets the value to determine whether monitored control(s) has triggered their ValueChanged or similar event.
    ''' </summary>
    Public Property HasChanges() As Boolean
        Get
            Return _HasChanges
        End Get
        Set(ByVal value As Boolean)
            HandleEvent(Me, value)
        End Set
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Add a control and its child controls (if a container object) to list of monitored objects for changes in values.
    ''' </summary>
    ''' <param name="ctrl">System.Windows.Forms.Control</param>
    Public Sub Add(ByVal ctrl As Control)
        Try
            Me.Add(ctrl, True)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Add a control and its child controls (if a container object) to list of monitored objects for changes in values.
    ''' </summary>
    ''' <param name="ctrl">System.Windows.Forms.Control</param>
    ''' <param name="clearChanges">Determines the value whether to clear HasChanges property after the procedure.</param>
    Public Sub Add(ByVal ctrl As Control, ByVal clearChanges As Boolean)
        Try
            Select Case ctrl.GetType.Name
                Case "UltraCheckEditor"
                    AddHandler DirectCast(ctrl, UltraCheckEditor).CheckedValueChanged, AddressOf Control_ValueChanged
                Case "UltraGrid"
                    With DirectCast(ctrl, UltraGrid)
                        AddHandler .AfterCellUpdate, AddressOf Grid_AfterCellUpdate
                        AddHandler .CellChange, AddressOf Grid_CellChange
                        AddHandler .AfterCellListCloseUp, AddressOf Grid_AfterCellListCloseUp
                        AddHandler .AfterHeaderCheckStateChanged, AddressOf Grid_AfterHeaderCheckStateChanged
                        AddHandler .AfterRowInsert, AddressOf Grid_AfterRowInsert
                        AddHandler .AfterRowsDeleted, AddressOf Grid_AfterRowsDeleted
                        AddHandler .Leave, AddressOf Grid_Leave
                        AddHandler .MouseClick, AddressOf Grid_MouseClick
                    End With
                Case "UltraNumericEditor"
                    AddHandler DirectCast(ctrl, UltraNumericEditor).ValueChanged, AddressOf Control_ValueChanged
                Case Else
                    If ctrl.HasChildren Then
                        For Each obj As Control In ctrl.Controls
                            Add(obj)
                        Next
                    Else
                        AddHandler ctrl.TextChanged, AddressOf Control_ValueChanged
                    End If
            End Select

            If clearChanges Then
                _HasChanges = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Remove a control and its child controls (if a container object) to list of monitored objects for changes in values.
    ''' </summary>
    ''' <param name="ctrl">System.Windows.Forms.Control</param>
    Public Sub Remove(ByVal ctrl As Control)
        Try
            Select Case ctrl.GetType.Name
                Case "UltraCheckEditor"
                    RemoveHandler DirectCast(ctrl, UltraCheckEditor).CheckedValueChanged, AddressOf Control_ValueChanged
                Case "UltraGrid"
                    With DirectCast(ctrl, UltraGrid)
                        RemoveHandler .AfterCellUpdate, AddressOf Grid_AfterCellUpdate
                        RemoveHandler .CellChange, AddressOf Grid_CellChange
                        RemoveHandler .AfterCellListCloseUp, AddressOf Grid_AfterCellListCloseUp
                        RemoveHandler .AfterHeaderCheckStateChanged, AddressOf Grid_AfterHeaderCheckStateChanged
                        RemoveHandler .AfterRowInsert, AddressOf Grid_AfterRowInsert
                        RemoveHandler .AfterRowsDeleted, AddressOf Grid_AfterRowsDeleted
                        RemoveHandler .Leave, AddressOf Grid_Leave
                        RemoveHandler .MouseClick, AddressOf Grid_MouseClick
                    End With
                Case "UltraNumericEditor"
                    RemoveHandler DirectCast(ctrl, UltraNumericEditor).ValueChanged, AddressOf Control_ValueChanged
                Case Else
                    If ctrl.HasChildren Then
                        For Each obj As Control In ctrl.Controls
                            Remove(obj)
                        Next
                    Else
                        RemoveHandler ctrl.TextChanged, AddressOf Control_ValueChanged
                    End If
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Sets the HasChanges property to False and raises the ValueChanged event.
    ''' </summary>
    Public Sub ClearChanges()
        Try
            Me.ClearChanges(True)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Sets the HasChanges property to False while having the option whether to raise the ValueChanged event or not.
    ''' </summary>
    ''' <param name="raiseEvent">Determines the value whether to raise the ValueChanged event or not.</param>
    Public Sub ClearChanges(ByVal [raiseEvent] As Boolean)
        Try
            If [raiseEvent] Then
                HandleEvent(Me, False)
            Else
                _HasChanges = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HandleEvent(ByVal sender As Object, ByVal hasChanges As Boolean)
        Try
            If Enabled Then
                If _HasChanges = hasChanges AndAlso hasChanges Then ' always raise event on clear changes
                    Return
                End If

                Dim eventArgs As New TrackChangesEventArgs(hasChanges)
                RaiseEvent ValueChanged(sender, eventArgs)

                _HasChanges = eventArgs.HasChanges
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub Control_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.Enabled Then
            HandleEvent(sender, True)
        End If
    End Sub

    Private Sub Grid_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs)
        HandleEvent(sender, True)
    End Sub

    Private Sub Grid_AfterCellListCloseUp(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs)
        If Enabled Then
            Dim curValue As String = IfEmpty(e.Cell.Value, "").ToString
            Dim orgValue As String = IfEmpty(e.Cell.OriginalValue, "").ToString

            If curValue <> orgValue Then
                HandleEvent(sender, True)
            End If
        End If
    End Sub

    Private Sub Grid_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs)
        If Enabled AndAlso (e.Cell.Activated OrElse e.Cell.IsInEditMode OrElse e.Cell.IsActiveCell) Then
            HandleEvent(sender, True)
        End If
    End Sub

    Private Sub Grid_AfterHeaderCheckStateChanged(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventArgs)
        If m_HeaderCheckBoxClicked Then
            m_HeaderCheckBoxClicked = False
            HandleEvent(sender, True)
        End If
    End Sub

    Private Sub Grid_AfterRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs)
        HandleEvent(sender, True)
    End Sub

    Private Sub Grid_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs)
        HandleEvent(sender, True)
    End Sub

    Private Sub Grid_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        m_HeaderCheckBoxClicked = False
    End Sub

    Private m_HeaderCheckBoxClicked As Boolean
    Private Sub Grid_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left AndAlso sender.DisplayLayout.Bands.Count > 0 Then
            With sender.DisplayLayout.Bands(0)
                ' Check if clicked within header checkbox coordinates
                If e.Y <= .Header.Height Then
                    m_HeaderCheckBoxClicked = True
                End If
            End With
        End If
    End Sub

#End Region

End Class

Public Class TrackChangesEventArgs
    Inherits EventArgs

    Public Sub New(ByVal hasChanges As Boolean)
        Me.HasChanges = hasChanges
    End Sub

    ''' <summary>
    ''' Returns/sets the value to determine whether monitored control(s) has triggered their ValueChanged or similar event.
    ''' </summary>
    Public Property HasChanges As Boolean
End Class