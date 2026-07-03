Public Class FormReportViewer

#Region "Declarations"

    Private m_ReportTitle As String
    Private m_DateFilter As DateFilters

    Public Event OnRefresh(sender As FormReportViewer, e As DateParams)

    Public Enum DateFilters
        None
        DateOnly
        DateRange
    End Enum

    Public Structure DateParams
        Dim DateFilter As DateFilters
        Dim [Date] As Date
        Dim [DateFrom] As Date
        Dim [DateTo] As Date
    End Structure

#End Region

#Region "Constructor"

    Public Sub New(ByVal report As Object, ByVal reportTitle As String, Optional dateFilter As DateFilters = DateFilters.None)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            m_Report = report
            m_ReportTitle = reportTitle & If(reportTitle.EndsWith("Report"), "", " Report")
            m_DateFilter = dateFilter
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

    Private m_Report As Object
    Public Property Report() As Object
        Get
            Return m_Report
        End Get
        Set(ByVal value As Object)
            m_Report = value
            If m_IsLoaded Then
                ucViewer.ReportSource = m_Report
            End If
        End Set
    End Property

    Private m_IsLoaded As Boolean
    Public ReadOnly Property IsLoaded() As Boolean
        Get
            Return m_IsLoaded
        End Get
    End Property

    Public Property ModuleName() As String
        Get
            Return ucViewer.ModuleName
        End Get
        Set(ByVal value As String)
            ucViewer.ModuleName = value
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.F AndAlso e.Modifiers = Keys.Shift Then
                pnlDateRange.Visible = Not pnlDateRange.Visible
                e.SuppressKeyPress = True

                If pnlDateRange.Visible Then
                    FocusDateFilter()
                End If

            ElseIf e.KeyCode = Keys.Enter AndAlso e.Modifiers = Keys.None Then
                btnRefresh.PerformClick()

            ElseIf e.KeyCode = Keys.Escape Then
                pnlDateRange.Visible = False
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            pnlDateRange.BackColor = Color.FromKnownColor(KnownColor.Control)
            Text = m_ReportTitle
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            ucViewer.ReportSource = m_Report
            ucViewer.Zoom(100)

            If m_DateFilter <> DateFilters.None Then
                ucViewer.AllowFind = False
                pnlDateRange.Visible = True

                If m_DateFilter = DateFilters.DateOnly Then
                    lblFrom.Visible = False
                    dtpDateFrom.Visible = False
                    lblTo.Visible = False
                    lblDate.Visible = True
                End If

                ActiveControl = pnlDateRange
                FocusDateFilter()
            End If

            m_IsLoaded = True
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            Dim ee As New DateParams With {.DateFilter = m_DateFilter}

            If m_DateFilter = DateFilters.DateOnly Then
                ee.Date = CDate(dtpDateTo.Value).Date
            Else
                ee.DateFrom = CDate(dtpDateFrom.Value).Date
                ee.DateTo = CDate(dtpDateTo.Value).AddHours(23).AddMinutes(59).AddSeconds(59)
            End If

            pnlDateRange.Visible = False

            RaiseEvent OnRefresh(Me, ee)
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub dtpDateFromTo_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFrom.ValueChanged, dtpDateTo.ValueChanged
        Try
            If m_IsLoaded AndAlso m_DateFilter = DateFilters.DateRange Then
                Dim startDate As Date = dtpDateFrom.Value
                Dim endDate As Date = dtpDateTo.Value

                If startDate > endDate Then
                    If sender Is dtpDateFrom Then
                        dtpDateTo.Value = startDate
                    Else
                        dtpDateFrom.Value = endDate
                    End If
                End If
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Sub InitReportViewer()
        Try
            ucViewer.crvViewer.InitReportViewer()
        Catch ex As Exception
            '
        End Try
    End Sub

    Private Sub FocusDateFilter()
        Try
            If m_DateFilter = DateFilters.DateOnly Then
                With dtpDateTo
                    .Focus()
                    .SelectAll()
                End With
            Else
                With dtpDateFrom
                    .Focus()
                    .SelectAll()
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class