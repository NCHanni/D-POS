Public Class ucDateTimeRange
#Region "Declarations"

    Public Event ValueChanged(ByVal StartDate As Date, ByVal EndDate As Date)
    Private m_SkipEvents As Boolean

#End Region
#Region "Properties"
    Public Property StartDate() As DateTime
        Get
            Return CDate(dtFrom.Value)
        End Get
        Set(ByVal value As DateTime)
            dtFrom.Value = value
        End Set
    End Property

    Public Property EndDate() As DateTime
        Get
            Return CDate(dtTo.Value)
        End Get
        Set(ByVal value As DateTime)
            dtTo.Value = value
        End Set
    End Property

    Private Sub dtFrom_ValueChanged(sender As Object, e As EventArgs) Handles dtFrom.ValueChanged, dtTo.ValueChanged
        Try
            If m_SkipEvents Then Return

            If IsNothing(sender.Value) Then Return

            If dtFrom.Value > dtTo.Value Then
                m_SkipEvents = True
                If sender Is dtFrom Then
                    dtTo.Value = CDate(dtFrom.Value).AddDays(1)
                Else
                    dtFrom.Value = CDate(dtTo.Value).AddDays(-1)
                End If
                m_SkipEvents = False
            End If

            RaiseEvent ValueChanged(Me.StartDate, Me.EndDate)
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub
#End Region

#Region "Methods"
    Public Sub Clear()
        Try
            m_SkipEvents = True
            dtTo.Value = Today
            dtFrom.Value = Today.AddDays(-1)
            m_SkipEvents = False
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ucDateTimeRange_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Clear()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub
#End Region
End Class
