<System.ComponentModel.DefaultEvent("ValueChanged")>
Public Class DateRange

#Region "Declarations"

    Public Event ValueChanged(ByVal StartDate As Date, ByVal EndDate As Date)
    Private m_SkipEvents As Boolean

#End Region

#Region "Properties"

    Public Property StartDate() As Date
        Get
            Return CDate(dtpDateFrom.Value).ToStartDate
        End Get
        Set(ByVal value As Date)
            dtpDateFrom.Value = value.ToStartDate
        End Set
    End Property

    Public Property EndDate() As Date
        Get
            Return CDate(dtpDateTo.Value).ToEndDate
        End Get
        Set(ByVal value As Date)
            dtpDateTo.Value = value.ToEndDate
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub dtpDateTo_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles dtpDateFrom.ValueChanged, dtpDateTo.ValueChanged
        Try
            If m_SkipEvents Then Return
            If IsNothing(sender.Value) Then Return

            If dtpDateFrom.Value > dtpDateTo.Value Then
                m_SkipEvents = True
                If sender Is dtpDateFrom Then
                    dtpDateTo.Value = CDate(dtpDateFrom.Value).AddMonths(1)
                Else
                    dtpDateFrom.Value = CDate(dtpDateTo.Value).AddMonths(-1)
                End If
                m_SkipEvents = False
            End If

            RaiseEvent ValueChanged(Me.StartDate, Me.EndDate)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub DateRange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Clear()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Sub Clear()
        Try
            m_SkipEvents = True
            dtpDateTo.Value = Today
            dtpDateFrom.Value = Today.AddMonths(-1)
            m_SkipEvents = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
