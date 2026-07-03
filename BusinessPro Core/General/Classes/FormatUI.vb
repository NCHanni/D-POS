#Region "Imports"

Imports Infragistics.Win.Misc
Imports Infragistics.Win.UltraWinEditors
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win.UltraWinSchedule
Imports Infragistics.Win.UltraWinTabControl

#End Region

Public Class FormatUI

#Region "Enum"

    Enum DefaultableBoolean
        [Default]
        [True]
        [False]
    End Enum

    Enum GridColumnHeaderAlignments
        [Auto]
        [Center]
    End Enum

    Enum ScrollBounds
        [ScrollToFill]
        [ScrollToLastItem]
    End Enum

    Enum ScrollStyle
        [Deferred]
        [Immediate]
    End Enum

#End Region

#Region "Declarations"

    Private Const AMOUNT_FORMAT As String = "#,##0.00"
    Private Const DATE_FORMAT As String = "MM/dd/yyyy"
    Private Const DATETIME_FORMAT As String = "MM/dd/yyyy hh:mm tt"
    Private Const DECIMAL_FORMAT As String = "#,##0.00#########"
    Private Const NUMBER_FORMAT As String = "#,##0"
    Private Const TIME_FORMAT As String = "hh:mm tt"

#End Region

#Region "Properties"

    Private m_ApplyButtonMnemonics As Boolean = False
    Public Property ApplyButtonMnemonics() As Boolean
        Get
            Return m_ApplyButtonMnemonics
        End Get
        Set(ByVal value As Boolean)
            m_ApplyButtonMnemonics = value
        End Set
    End Property

    Private m_BackColor As Color = Color.White
    Public Property BackColor() As Color
        Get
            Return m_BackColor
        End Get
        Set(ByVal value As Color)
            m_BackColor = value
        End Set
    End Property

    Private m_GridColumnHeadersAutoSet As Boolean = True
    Public Property GridColumnHeadersAutoSet() As Boolean
        Get
            Return m_GridColumnHeadersAutoSet
        End Get
        Set(ByVal value As Boolean)
            m_GridColumnHeadersAutoSet = value
        End Set
    End Property

    Private m_GridColumnHeaderAlignment As GridColumnHeaderAlignments = GridColumnHeaderAlignments.Center
    Public Property GridColumnHeaderAlignment() As GridColumnHeaderAlignments
        Get
            Return m_GridColumnHeaderAlignment
        End Get
        Set(ByVal value As GridColumnHeaderAlignments)
            m_GridColumnHeaderAlignment = value
        End Set
    End Property

    Private m_GridDateFormat As String = "MM/dd/yyyy"
    Public Property GridDateFormat() As String
        Get
            Return m_GridDateFormat
        End Get
        Set(ByVal value As String)
            m_GridDateFormat = value
        End Set
    End Property

    Private m_GridDateTimeFormat As String = "MM/dd/yyyy hh:mm tt"
    Public Property GridDateTimeFormat() As String
        Get
            Return m_GridDateTimeFormat
        End Get
        Set(ByVal value As String)
            m_GridDateTimeFormat = value
        End Set
    End Property

    Private m_GridScrollBounds As ScrollBounds = ScrollBounds.ScrollToFill
    Public Property GridScrollBounds() As ScrollBounds
        Get
            Return m_GridScrollBounds
        End Get
        Set(ByVal value As ScrollBounds)
            m_GridScrollBounds = value
        End Set
    End Property

    Private m_GridScrollStyle As ScrollStyle = ScrollStyle.Immediate
    Public Property GridScrollStyle() As ScrollStyle
        Get
            Return m_GridScrollStyle
        End Get
        Set(ByVal value As ScrollStyle)
            m_GridScrollStyle = value
        End Set
    End Property

    Private m_GroupBoxForeColorDisabled As Color = Color.FromKnownColor(KnownColor.GrayText)
    Public Property GroupBoxForeColorDisabled() As Color
        Get
            Return m_GroupBoxForeColorDisabled
        End Get
        Set(ByVal value As Color)
            m_GroupBoxForeColorDisabled = value
        End Set
    End Property

    Private m_GridTimeFormat As String = "hh:mm tt"
    Public Property GridTimeFormat() As String
        Get
            Return m_GridTimeFormat
        End Get
        Set(ByVal value As String)
            m_GridTimeFormat = value
        End Set
    End Property

    Private m_TransparentColor As Color = Color.Transparent
    Public Property TransparentColor() As Color
        Get
            Return m_TransparentColor
        End Get
        Set(ByVal value As Color)
            m_TransparentColor = value
        End Set
    End Property

    Private m_UseFlatMode As DefaultableBoolean = DefaultableBoolean.True
    Public Property UseFlatMode() As DefaultableBoolean
        Get
            Return m_UseFlatMode
        End Get
        Set(ByVal value As DefaultableBoolean)
            m_UseFlatMode = value
        End Set
    End Property

    Private m_UseOsThemes As DefaultableBoolean = DefaultableBoolean.False
    Public Property UseOsThemes() As DefaultableBoolean
        Get
            Return m_UseOsThemes
        End Get
        Set(ByVal value As DefaultableBoolean)
            m_UseOsThemes = value
        End Set
    End Property

    Property GridAllowRowFiltering As Boolean
    Property GridColumnHeaderCaptionAutoSet As Boolean = True

    Private m_SkipKeyDownHandler As Boolean
    ''' <summary>
    ''' Skip handler of the next KeyDown event for the ff. controls: UltraTextEditor, UltraComboEditor, UltraGrid.
    ''' </summary>
    Public Sub SkipKeyDownHandler()
        m_SkipKeyDownHandler = True
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>Format an object and its child controls to standard Kinetique specifications.</summary>
    ''' <param name="_Form">Specify the container object (form) to be formatted.</param>
    ''' <remarks>No need to apply on user controls.</remarks>
    Public Sub Apply(ByVal _Form As Object)
        Select Case _Form.GetType.BaseType.Name
            Case "Form"
                With DirectCast(_Form, Form)
                    .BackColor = BackColor

                    Dim Icon As Object = Current.ApplicationIcon
                    If Icon IsNot Nothing Then
                        .Icon = Icon
                    End If

                    If .Modal Or .FormBorderStyle = FormBorderStyle.FixedDialog Then
                        .MinimizeBox = False
                        .MinimumSize = .Size

                        If .MaximizeBox = False Then
                            .MaximumSize = .Size
                        End If

                        Select Case .FormBorderStyle
                            Case FormBorderStyle.Sizable, FormBorderStyle.SizableToolWindow
                                .SizeGripStyle = SizeGripStyle.Auto
                            Case FormBorderStyle.FixedDialog, FormBorderStyle.FixedSingle
                                .MinimizeBox = False
                                .MaximizeBox = False
                                .SizeGripStyle = SizeGripStyle.Hide
                            Case Else
                                .SizeGripStyle = SizeGripStyle.Hide
                        End Select

                        .ShowInTaskbar = False
                    Else
                        .SizeGripStyle = SizeGripStyle.Hide
                    End If

                    If .Owner Is Nothing Then
                        .StartPosition = FormStartPosition.CenterScreen
                    Else
                        .StartPosition = FormStartPosition.CenterParent
                    End If
                End With
            Case "UserControl"
                _Form.BackColor = TransparentColor
        End Select

        Dim en As IEnumerator = _Form.Controls.GetEnumerator
        While en.MoveNext
            Apply(en.Current)
            FormatObject(en.Current)
        End While
    End Sub

    Private Shared m_SkipGridColumnHeaderCaptionAutoSet As Boolean
    Public Shared Sub SkipGridColumnHeaderCaptionAutoSet()
        m_SkipGridColumnHeaderCaptionAutoSet = True
    End Sub

#End Region

#Region "Private Methods"

    Private Sub FormatObject(ByVal obj As Object)
        Select Case obj.GetType.Name
            Case "CrystalReportViewer" : FormatCrystalReportViewer(obj)
            Case "Label" : FormatLabel(obj)
            Case "Panel" : FormatPanel(obj)
            Case "UltraButton" : FormatUltraButton(obj)
            Case "UltraCalendarCombo" : FormatUltraCalendarCombo(obj)
            Case "UltraCheckEditor" : FormatUltraCheckEditor(obj)
            Case "UltraCombo" : FormatUltraCombo(obj)
            Case "UltraComboEditor" : FormatUltraComboEditor(obj)
            Case "UltraCurrencyEditor" : FormatUltraCurrencyEditor(obj)
            Case "UltraDateTimeEditor" : FormatUltraDateTimeEditor(obj)
            Case "UltraDropDown" : FormatUltraDropDown(obj)
            Case "UltraGrid" : FormatUltraGrid(obj)
            Case "UltraGroupBox" : FormatUltraGroupBox(obj)
            Case "UltraLabel" : FormatUltraLabel(obj)
            Case "UltraNumericEditor" : FormatUltraNumericEditor(obj)
            Case "UltraOptionSet" : FormatUltraOptionSet(obj)
            Case "UltraPictureBox" : FormatUltraPictureBox(obj)
            Case "UltraTabControl" : FormatUltraTabControl(obj)
            Case "UltraTextEditor" : FormatUltraTextEditor(obj)
            Case "UltraTabPageControl" : FormatTabPageControl(obj)
        End Select
    End Sub

    Private Sub FormatCrystalReportViewer(ByVal obj As CrystalDecisions.Windows.Forms.CrystalReportViewer)
        With obj
            If obj.Parent.GetType.BaseType.Name = "Form" Then
                If .Dock = DockStyle.Fill Then
                    .BorderStyle = BorderStyle.None
                End If
            Else
                .BorderStyle = BorderStyle.FixedSingle
            End If

            .BorderStyle = Windows.Forms.BorderStyle.None
            .ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None

            .DisplayStatusBar = True
            .DisplayToolbar = True
            .ShowExportButton = True
            .ShowGotoPageButton = True
            .ShowPageNavigateButtons = True
            .ShowPrintButton = True
            .ShowTextSearchButton = True
            .ShowZoomButton = True

            .EnableDrillDown = False
            .EnableRefresh = False
            .ReuseParameterValuesOnRefresh = False
            .ShowCloseButton = False
            .ShowGroupTreeButton = False
            .ShowLogo = False
            .ShowParameterPanelButton = False
            .ShowRefreshButton = False
        End With
    End Sub

    Private Sub FormatLabel(ByVal obj As Label)
        With obj
            .BackColor = TransparentColor
        End With
    End Sub

    Private Sub FormatPanel(ByVal obj As Panel)
        With obj
            .BackColor = TransparentColor
        End With
    End Sub

    Private Sub FormatUltraButton(ByVal obj As UltraButton)
        With obj
            .ShowFocusRect = False
            .ShowOutline = False
            If .UseAppStyling Then
                .UseMnemonic = True
                '.UseFlatMode = Infragistics.Win.DefaultableBoolean.True
                .UseOsThemes = Infragistics.Win.DefaultableBoolean.False
            End If
            .DialogResult = DialogResult.None
        End With
    End Sub

    Private Sub FormatUltraCalendarCombo(ByVal obj As UltraCalendarCombo)
        With obj
            If .AutoSize Then
                .AutoSize = False
                .Height = 24
            End If
            If .AllowNull Then
                .Value = Nothing
            Else
                .Value = Now.Date
            End If
            .Format = "d" ' Date only
            .NullDateLabel = ""
            AddHandler .GotFocus, AddressOf UltraCalendarCombo_GotFocus
            AddHandler .Invalidated, AddressOf UltraTextEditor_Invalidated
        End With
    End Sub

    Private Sub UltraCalendarCombo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            sender.SelectAll()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FormatUltraCheckEditor(ByVal obj As UltraCheckEditor)
        With obj
            .UseAppStyling = True
        End With
    End Sub

    Private Sub FormatUltraCombo(ByVal obj As UltraCombo)
        With obj
            .UseAppStyling = True
        End With
    End Sub

    Private Sub FormatUltraComboEditor(ByVal obj As UltraComboEditor)
        With obj
            .UseAppStyling = True

            If .AutoSize Then
                .AutoSize = False
                .Height = 24
            End If
            If .ButtonsRight.Count > 0 Then
                AddHandler .EnabledChanged, AddressOf UltraComboEditor_EnableChanged
            End If
            AddHandler .Invalidated, AddressOf UltraTextEditor_Invalidated
        End With
    End Sub

    Private Sub UltraComboEditor_EnableChanged(ByVal sender As UltraComboEditor, ByVal e As System.EventArgs)
        Try
            If e.ToString = "" Then
                ' CA1801 : Microsoft.Usage : Parameter 'e' is never used.
            End If
            For i As Integer = 0 To sender.ButtonsRight.Count - 1
                sender.ButtonsRight(i).Visible = sender.Enabled
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FormatUltraCurrencyEditor(ByVal obj As UltraCurrencyEditor)
        With obj
            .UseAppStyling = True
            Try
                .FormatProvider = New Globalization.CultureInfo("en-PH")
            Catch
                Try
                    .FormatProvider = New Globalization.CultureInfo("fil-PH") ' for Vista
                Catch
                End Try
            End Try
            .TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl
            .MaxValue = 1.0E+24
            .MinValue = .MaxValue * -1
            With obj
                If .UseAppStyling Then
                    If .Tag <> "" Then
                        With .Appearance
                            .BackColor = Color.FromArgb(255, 246, 190)
                        End With
                    Else
                        With .Appearance
                            .BackColor = Nothing
                        End With

                    End If
                End If
            End With
        End With
    End Sub

    Private Sub FormatUltraDateTimeEditor(ByVal obj As UltraDateTimeEditor)
        With obj
            .UseAppStyling = True
            .Value = Now
            AddHandler .GotFocus, AddressOf UltraDateTimeEditor_GotFocus
            AddHandler .ValueChanged, AddressOf UltraDateTimeEditor_ValueChanged
            With obj
                If .UseAppStyling Then
                    If .Tag <> "" Then
                        With .Appearance
                            .BackColor = Color.FromArgb(255, 246, 190)
                        End With
                    Else
                        With .Appearance
                            .BackColor = Nothing
                        End With

                    End If
                End If
            End With
        End With
    End Sub

    Private Sub UltraDateTimeEditor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        sender.SelectAll()
    End Sub

    Private Sub UltraDateTimeEditor_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If sender.Value Is Nothing Then
                sender.Value = Now ' Include date and time (essential in accounting)
            End If
        Catch ' Ignore error
        End Try
    End Sub

    Private Sub FormatUltraDropDown(ByVal obj As UltraDropDown)
        With obj
            .UseAppStyling = True
            .DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center
        End With
    End Sub

    Private Sub FormatUltraGrid(ByVal obj As UltraGrid)
        With obj

            With .DisplayLayout
                .AutoFitStyle = AutoFitStyle.ExtendLastColumn
                .EmptyRowSettings.ShowEmptyRows = False
                .LoadStyle = LoadStyle.LoadOnDemand

                With .Override
                    .AllowColMoving = AllowColMoving.NotAllowed
                    .AllowColSwapping = AllowColSwapping.NotAllowed
                    .AllowDelete = Infragistics.Win.DefaultableBoolean.False

                    If GridAllowRowFiltering Then
                        .AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
                        .FilterUIType = FilterUIType.FilterRow
                    End If

                    If .CellMultiLine = Infragistics.Win.DefaultableBoolean.Default Then
                        .CellMultiLine = Infragistics.Win.DefaultableBoolean.False
                    End If

                    If .HeaderClickAction = HeaderClickAction.Default Then
                        .HeaderClickAction = HeaderClickAction.SortMulti
                    End If

                    .FixedHeaderIndicator = FixedHeaderIndicator.None
                    .FixedCellSeparatorColor = Color.Transparent
                    .MaxSelectedRows = 1
                    .RowSelectors = Infragistics.Win.DefaultableBoolean.False
                    .TipStyleCell = TipStyle.Show
                    .TipStyleHeader = TipStyle.Show
                    .CellButtonAppearance.BackColor = Color.Lavender

                    If GridScrollStyle = ScrollStyle.Immediate Then
                        .TipStyleScroll = TipStyle.Hide
                    End If
                End With

                .MaxColScrollRegions = 1
                .MaxRowScrollRegions = 1
                .ScrollBounds = GridScrollBounds
                .ScrollStyle = GridScrollStyle
                .TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextCell
                .ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Vertical
            End With

            .Text = ""
            .UseFlatMode = Infragistics.Win.DefaultableBoolean.True
            .UseOsThemes = Infragistics.Win.DefaultableBoolean.False

            AddHandler .BeforeCellUpdate, AddressOf UltraGrid_BeforeCellUpdate
            AddHandler .BeforeCellCancelUpdate, AddressOf UltraGrid_BeforeCellCancelUpdate

            AddHandler .BeforeHeaderCheckStateChanged, AddressOf UltraGrid_BeforeHeaderCheckStateChanged
            AddHandler .Error, AddressOf UltraGrid_Error
            AddHandler .EnabledChanged, AddressOf UltraGrid_EnabledChanged
            AddHandler .KeyDown, AddressOf UltraGrid_KeyDown
            AddHandler .InitializeLayout, AddressOf UltraGrid_InitializeLayout
        End With
    End Sub

    Private Sub UltraGrid_BeforeCellCancelUpdate(sender As Object, e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs)
        Try
            e.Cancel = True
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub UltraGrid_BeforeCellUpdate(ByVal sender As UltraGrid, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs)
        Try
            If IsDBNull(e.NewValue) Then
                Select Case e.Cell.Column.Style
                    Case Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleNonNegative, Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerNonNegative
                        e.Cancel = True
                    Case Else
                        Select Case e.Cell.Column.DataType.Name
                            Case "Double", "Decimal", "Integer", "Int16", "Int32", "Int64"
                                e.Cancel = True
                        End Select
                End Select
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub UltraGrid_BeforeHeaderCheckStateChanged(ByVal sender As UltraGrid, ByVal e As Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventArgs)
        If sender.Rows.Count = 0 Then
            e.NewCheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub UltraGrid_EnabledChanged(ByVal sender As UltraGrid, ByVal e As System.EventArgs)
        With sender
            If Not .Enabled Then
                .ActiveCell = Nothing
                .ActiveRow = Nothing
                .Selected.Cells.Clear()
                .Selected.Rows.Clear()
            End If
        End With
    End Sub

    Private Sub UltraGrid_Error(ByVal sender As UltraGrid, ByVal e As Infragistics.Win.UltraWinGrid.ErrorEventArgs)
        Try
            If e.ErrorType = ErrorType.Data Then
                If e.DataErrorInfo.Cell IsNot Nothing Then
                    If e.DataErrorInfo.Cell.Column.Style = ColumnStyle.Date Then
                        If IsDBNull(e.DataErrorInfo.Cell.OriginalValue) Then
                            e.DataErrorInfo.Cell.Value = DBNull.Value
                        Else
                            If IsDate(e.DataErrorInfo.Cell.OriginalValue) Then
                                e.DataErrorInfo.Cell.Value = e.DataErrorInfo.Cell.OriginalValue
                            Else
                                e.DataErrorInfo.Cell.Value = DBNull.Value
                            End If
                        End If
                    Else
                        e.DataErrorInfo.Cell.Value = e.DataErrorInfo.Cell.OriginalValue
                    End If
                End If
                e.Cancel = True
            End If
        Catch ex As Exception
            ' ex.ShowError()
        End Try
    End Sub

    Private Sub UltraGrid_KeyDown(ByVal sender As UltraGrid, ByVal e As System.Windows.Forms.KeyEventArgs)
        If m_SkipKeyDownHandler Then
            m_SkipKeyDownHandler = False
        Else
            If Not e.Alt And Not e.Shift Then
                If sender Is Nothing OrElse sender.Rows Is Nothing Then
                    Return
                End If
                With sender
                    If .Rows.Count = 0 Then
                        Select Case e.KeyCode
                            Case Keys.Up
                                SendKeys.Send("+{TAB}")
                                e.Handled = True
                                e.SuppressKeyPress = True
                            Case Keys.Down
                                SendKeys.Send("{TAB}")
                                e.Handled = True
                                e.SuppressKeyPress = True
                        End Select
                    ElseIf .ActiveRow Is Nothing Then
                        Select Case e.KeyCode
                            Case Keys.Up
                                .Rows(.Rows.Count - 1).Activate()
                            Case Keys.Down
                                .Rows(0).Activate()
                        End Select
                    ElseIf .ActiveRow IsNot Nothing Then
                        Return
                    ElseIf .ActiveCell IsNot Nothing Then
                        Return
                    Else
                        Select Case e.KeyCode
                            Case Keys.Up
                                If .ActiveRow.Index = 0 Then
                                    SendKeys.Send("+{TAB}")
                                    e.Handled = True
                                    e.SuppressKeyPress = True
                                End If
                            Case Keys.Down
                                If .ActiveRow.Index = .Rows.Count - 1 Then
                                    SendKeys.Send("{TAB}")
                                    e.Handled = True
                                    e.SuppressKeyPress = True
                                End If
                        End Select
                    End If
                End With
            End If
        End If
    End Sub

    Private Sub UltraGrid_InitializeLayout(ByVal sender As UltraGrid, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs)
        With e.Layout
            .AutoFitStyle = AutoFitStyle.ResizeAllColumns
            .Override.DefaultRowHeight = 20
            .Override.RowSizing = RowSizing.AutoFree

            If .Grid.Rows.Count = 0 Then
                .Grid.TabStop = False
            Else
                .Grid.TabStop = True
            End If

            For Each band As UltraGridBand In .Bands

                For Each column As UltraGridColumn In band.Columns
                    With column
                        If .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Default Then
                            .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
                        End If
                        .Header.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle
                        .TipStyleCell = TipStyle.Hide
                        .CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle

                        Select Case .DataType.Name
                            Case "Boolean"
                                If .DefaultCellValue Is Nothing Then
                                    .DefaultCellValue = False
                                End If
                            Case "DateTime"
                                If .DefaultCellValue Is Nothing Then
                                    .DefaultCellValue = Now
                                End If
                                .CellAppearance.TextVAlign = Infragistics.Win.VAlign.Top
                                .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
                            Case "Double", "Decimal", "Single"
                                If .DefaultCellValue Is Nothing Then
                                    .DefaultCellValue = 0.0
                                Else
                                    Try ' causes error if DefaultCellValue is non-numeric
                                        If .DefaultCellValue = -1 Then
                                            .DefaultCellValue = Nothing
                                        End If
                                    Catch ex As Exception
                                        .DefaultCellValue = 0.0
                                    End Try
                                End If
                            Case "Integer", "Int16", "Int32", "Int64", "Long"
                                If .DefaultCellValue Is Nothing Then
                                    .DefaultCellValue = 0
                                Else
                                    Try ' causes error if DefaultCellValue is non-numeric
                                        If .DefaultCellValue = -1 Then
                                            .DefaultCellValue = Nothing
                                        End If
                                    Catch ex As Exception
                                        .DefaultCellValue = 0
                                    End Try
                                End If

                            Case "String"
                                If .DefaultCellValue Is Nothing Then
                                    .DefaultCellValue = ""
                                End If
                                '.ResetWidth()
                                '.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, True)
                                '.CellMultiLine = Infragistics.Win.DefaultableBoolean.True

                        End Select
                    End With

                    If GridColumnHeaderCaptionAutoSet Then
                        With column.Header
                            Dim key As String = column.Key
                            If .Caption = key Then
                                .Caption = key.ToProperCase
                            End If
                        End With
                    End If

                    With column
                        Select Case .DataType.Name
                            Case "DateTime"
                                Select Case .Style
                                    Case ColumnStyle.Date, ColumnStyle.DropDownCalendar, ColumnStyle.DateWithoutDropDown
                                        '
                                    Case ColumnStyle.DateTime, ColumnStyle.DateTimeWithoutDropDown
                                        If .Format = "" Then
                                            .Format = DATETIME_FORMAT
                                        End If
                                        If .MaxWidth = 0 And .MinWidth < 115 Then
                                            .Width = 120
                                            '.MaxWidth = If(.Header.Caption.Length < 15, 130, 200)
                                            .MinWidth = 115
                                        End If
                                    Case ColumnStyle.Time, ColumnStyle.TimeWithSpin
                                        If .Format = "" Then
                                            .Format = TIME_FORMAT
                                        End If
                                        If .MaxWidth = 0 And .MinWidth < 65 Then
                                            .Width = 70
                                            '.MaxWidth = If(.Header.Caption.Length < 15, 80, 200)
                                            .MinWidth = 65
                                        End If
                                    Case Else
                                        .Format = DATE_FORMAT
                                        .Style = ColumnStyle.DateWithoutDropDown
                                End Select
                                If .Format = "" Then
                                    .Format = DATE_FORMAT
                                End If
                                If .MaxWidth = 0 And .MinWidth < 65 Then
                                    .Width = 70
                                    '.MaxWidth = If(.Header.Caption.Length < 15, 80, 200)
                                    .MinWidth = 65
                                End If

                            Case "Double", "Decimal"
                                If .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Default Then
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                End If
                                If .Format = "" Then
                                    .Format = AMOUNT_FORMAT
                                    .CellDisplayStyle = CellDisplayStyle.FormattedText ' solves the partial hiding of numeric values
                                End If
                                If .MaxWidth = 0 And .MinWidth < 100 Then
                                    .Width = 100
                                    .MaxWidth = 100 'If(.Header.Caption.Length < 15, 80, 200)
                                    .MinWidth = 100
                                End If
                                If GridColumnHeaderAlignment = GridColumnHeaderAlignments.Auto Then
                                    .Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                                End If
                                If .Style = ColumnStyle.Default And .ValueList Is Nothing Then
                                    .Style = ColumnStyle.DoubleNonNegative
                                End If

                            Case "Integer", "Int16", "Int32", "Int64"
                                If .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Default Then
                                    .CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                                End If
                                If .Format = "" Then
                                    .Format = NUMBER_FORMAT
                                    .CellDisplayStyle = CellDisplayStyle.FormattedText ' solves the partial hiding of numeric values
                                End If
                                If .MaxWidth = 0 And .MinWidth < 50 Then
                                    .Width = 75
                                    '.MaxWidth = If(.Header.Caption.Length < 15, 100, 200)
                                    .MinWidth = 50
                                End If
                                If .Style = ColumnStyle.Default And .ValueList Is Nothing Then
                                    .Style = ColumnStyle.IntegerNonNegative
                                End If

                            Case Else
                                If .DataType.Name = "Boolean" Or .Style = ColumnStyle.CheckBox Then
                                    If .Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always Then ' if has header checkbox
                                        .CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit
                                        .CellActivation = Activation.AllowEdit
                                        .Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center
                                        .Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection
                                    End If
                                    If .DefaultCellValue Is Nothing Then
                                        .DefaultCellValue = False
                                    End If
                                    If .Style = ColumnStyle.Default Then
                                        .Style = ColumnStyle.CheckBox
                                    End If
                                    If .MaxWidth = 0 And .MinWidth < 50 Then
                                        .Width = If(.Header.Caption.Length < 10, 55, 80)
                                        '.MaxWidth = If(.Header.Caption.Length < 10, 60, 120)
                                        .MinWidth = 50
                                    End If

                                ElseIf .DataType.Name = "String" Then
                                    If .MaxLength = 0 Then
                                        '.MaxLength = 255
                                    End If
                                End If

                                If .Style = ColumnStyle.DropDownList Or .Style = ColumnStyle.EditButton Then
                                    .ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always
                                End If

                                If .Style = ColumnStyle.Button Then
                                    .ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always
                                    .CellButtonAppearance.TextHAlign = Infragistics.Win.HAlign.Center
                                    .CellButtonAppearance.TextVAlign = Infragistics.Win.VAlign.Middle
                                    If .MaxWidth = 0 And .MinWidth < 64 Then
                                        .Width = 128
                                        '.MaxWidth = 128
                                        .MinWidth = 128
                                        .LockedWidth = True
                                    End If
                                Else
                                    If .MaxWidth = 0 And .MinWidth < 50 Then
                                        If .Width < 50 Then .Width = 50
                                        ' If .Width > 4096 Then .MaxWidth = .Width Else .MaxWidth = 4096
                                        .MinWidth = 50
                                    End If
                                End If
                        End Select
                    End With
                Next
            Next
        End With
    End Sub

    Private Sub FormatUltraGroupBox(ByVal obj As UltraGroupBox)
        With obj
            .BorderStyle = GroupBoxBorderStyle.None
        End With
    End Sub

    Private Sub FormatUltraLabel(ByVal obj As UltraLabel)
        With obj
            If .UseAppStyling Then
                .Appearance.BackColor = TransparentColor
            End If
        End With
    End Sub

    Private Sub FormatUltraNumericEditor(ByVal obj As UltraNumericEditor)
        With obj
            .UseAppStyling = True
            .Nullable = True
            .TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl
            .AutoSize = False
            .Height = 24

            Select Case .NumericType
                Case NumericType.Decimal
                    .FormatString = DECIMAL_FORMAT
                    .MaskInput = If(.MaskInput = "", "{double:3.12}", .MaskInput)
                    .MaxValue = 999.999999999999
                    .MinValue = -.MaxValue

                Case NumericType.Double
                    .FormatString = AMOUNT_FORMAT
                    .MaskInput = If(.MaskInput = "", "{double:16.2}", .MaskInput)
                    .MaxValue = 1.0E+16
                    .MinValue = -.MaxValue

                Case NumericType.Integer
                    .FormatString = NUMBER_FORMAT
                    .MaskInput = If(.MaskInput = "", "nnnnnn", .MaskInput)
                    .MaxValue = 999999
                    .MinValue = -.MaxValue
            End Select

            If .ReadOnly Then
                .PromptChar = ""
            Else
                AddHandler .GotFocus, AddressOf UltraNumericEditor_GotFocus
                AddHandler .Leave, AddressOf UltraNumericEditor_Leave
            End If

            AddHandler .Invalidated, AddressOf UltraTextEditor_Invalidated
        End With
    End Sub

    Private Sub UltraNumericEditor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            sender.SelectAll()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub UltraNumericEditor_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If IsDBNull(sender.Value) Then
                sender.Value = 0.0
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FormatUltraOptionSet(ByVal obj As UltraOptionSet)
        With obj
            .BackColor = TransparentColor
            .BorderStyle = Infragistics.Win.UIElementBorderStyle.None
            .Appearance.BackColorDisabled = TransparentColor
        End With
    End Sub

    Private Sub FormatUltraPictureBox(ByVal obj As UltraPictureBox)
        With obj
            .UseAppStyling = False
        End With
    End Sub

    Private Sub FormatTabPageControl(obj As UltraTabPageControl)
        If obj.TabControl.Style = UltraTabControlStyle.Wizard Then Return

        With obj
            .Padding = New Padding(0, 3, 0, 0)
        End With
        AddHandler obj.Paint, AddressOf UltraTabPageControl_Paint
    End Sub

    Public Sub UltraTabPageControl_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs)
        Try
            If sender.TabControl.Style = UltraTabControlStyle.Wizard Then Return
            Dim pen As Pen
            If sender.TabControl.Enabled Then
                pen = New Pen(Color.FromArgb(0, 107, 193), 3)
            Else
                pen = New Pen(Color.FromArgb(149, 149, 149), 3)
            End If
            Dim g As Graphics = e.Graphics
            g.DrawLine(pen, 0, 0, sender.width, 0)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FormatUltraTabControl(ByVal obj As UltraTabControl)
        With obj
            If .UseAppStyling Then
                .TabStop = False
                .InterTabSpacing = New Infragistics.Win.DefaultableInteger(1)
                .ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Default
                .Style = UltraTabControlStyle.Flat
                .UseFlatMode = Infragistics.Win.DefaultableBoolean.True
                .UseHotTracking = Infragistics.Win.DefaultableBoolean.False
                .UseOsThemes = Infragistics.Win.DefaultableBoolean.False
            End If
            .Tabs(0).AllowClosing = Infragistics.Win.DefaultableBoolean.False

            For Each tabs As Infragistics.Win.UltraWinTabControl.UltraTab In .Tabs
                tabs.AllowClosing = Infragistics.Win.DefaultableBoolean.False
            Next
        End With

        Try
            Dim bitmap As New Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType(FormatUI).Namespace & ".tab.png"))
            For i As Integer = 0 To obj.Tabs.Count - 1
                obj.Tabs(i).Appearance.Image = bitmap
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FormatUltraTextEditor(ByVal obj As UltraTextEditor)
        With obj
            .UseAppStyling = True
            If .MaxLength = 32767 Then ' if default
                .MaxLength = 255
            End If
            If Not .PasswordChar = Nothing Then
                .PasswordChar = "●"
            End If
            .UseFlatMode = UseFlatMode
            If .ReadOnly Then
                .Appearance.BackColor = Color.FromKnownColor(KnownColor.Window)
                If .ButtonsRight.Count > 0 Then
                    .Enabled = True
                Else
                    .Enabled = False
                End If
            End If
            AddHandler .Invalidated, AddressOf UltraTextEditor_Invalidated
        End With
    End Sub

    Private Sub UltraTextEditor_Invalidated(ByVal sender As Object, ByVal e As System.Windows.Forms.InvalidateEventArgs)
        Try
            With sender
                If .UseAppStyling Then
                    If .Tag <> "" Then
                        With .Appearance
                            .BackColor = Color.FromArgb(255, 246, 190)
                        End With
                    Else
                        With .Appearance
                            .BackColor = Nothing
                        End With

                    End If
                End If
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub FormatUserControl(ByVal obj As UserControl)
        With obj
            .BackColor = TransparentColor
        End With
    End Sub

    Private Function ToProperCase(ByVal str As String) As String
        Dim new_str As String = ""
        Try
            Dim toUpper As Boolean = True
            str = Trim(str).Replace("_", " ")
            For Each character As Char In str
                If toUpper Then
                    character = character.ToString.ToUpper
                    toUpper = False
                Else
                    character = character.ToString.ToLower
                End If
                Select Case character
                    Case " ", ".", "-"
                        toUpper = True
                End Select
                new_str &= character
            Next
        Catch
            new_str = str
        End Try
        Return new_str.Replace(" Of ", " of ")
    End Function

#End Region

End Class