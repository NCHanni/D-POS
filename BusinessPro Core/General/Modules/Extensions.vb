Imports System.Runtime.CompilerServices
Imports System.Text
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win.UltraWinEditors
Imports Infragistics.Win.Misc
Imports System.Reflection
Imports Infragistics.Win.UltraWinSchedule
Imports System.Drawing.Imaging

Public Module Extensions

    Public DATE_FORMAT As String = "MM/dd/yyyy"
    Public DATETIME_FORMAT As String = "MM/dd/yyyy hh:mm tt"

#Region "DataTable"

    <Extension()>
    Public Function HasChanges(ByVal value As DataTable) As Boolean
        Try
            If value IsNot Nothing Then
                For Each row As DataRow In value.Rows
                    If row.RowState <> DataRowState.Detached And row.RowState <> DataRowState.Unchanged Then
                        Return True
                    End If
                Next
            End If
            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function HasData(ByVal value As DataTable) As Boolean
        Try
            Return (value.Rows.Count > 0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function HasData(ByVal value As DataSet, Optional ByVal tableIndex As Integer = 0) As Boolean
        Try
            Return (value.Tables(tableIndex).Rows.Count > 0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function ToDataTable(Of T)(Collection As IEnumerable(Of T)) As DataTable
        Try
            Dim dt As New DataTable()
            Dim type As Type = GetType(T)
            Dim pia As PropertyInfo() = type.GetProperties()

            For Each pi As PropertyInfo In pia
                Dim ColumnType As Type = pi.PropertyType

                If (ColumnType.IsGenericType) Then
                    ColumnType = ColumnType.GetGenericArguments()(0)
                End If

                dt.Columns.Add(pi.Name, ColumnType)
            Next

            For Each item As T In Collection
                Dim dr As DataRow = dt.NewRow()
                dr.BeginEdit()
                For Each pi As PropertyInfo In pia
                    If pi.GetValue(item, Nothing) IsNot Nothing Then
                        dr(pi.Name) = pi.GetValue(item, Nothing)
                    End If
                Next
                dr.EndEdit()
                dt.Rows.Add(dr)
            Next

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Date"

    <Extension()>
    Public Function ToEndDate(ByVal value As Date) As Date
        Try
            Return CDate(value.ToString(DATE_FORMAT) & " 23:59:59")
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function ToStartDate(ByVal value As Date) As Date
        Try
            Return CDate(value.ToString(DATE_FORMAT) & " 00:00:00")
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function ToDateString(ByVal value As Date) As String
        Try
            Return value.ToString(DATETIME_FORMAT)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Exception"

    <Extension()>
    Public Sub ShowError(ByVal ex As Exception)
        MessageBox.ShowError(ex)
    End Sub

#End Region

#Region "Object"

    <Extension()>
    Public Function IfNothing(ByVal value As Object, ByVal defaultValue As Object) As Object
        Try
            If value Is Nothing Then
                Return defaultValue
            Else
                Return value
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function IfDBNull(ByVal value As Object, ByVal defaultValue As Object) As Object
        Try
            If IsDBNull(value) Then
                Return defaultValue
            Else
                Return value
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Sub IsAllowed(ByVal control As Control, ByVal accessRight As Rights.AccessRights, Optional moduleGroup As String = "")
        Try
            If Current.Rights IsNot Nothing Then
                control.Enabled = Current.Rights.IsAllowed(control.Tag, accessRight, moduleGroup)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Function Quote(ByVal value As Object, Optional ByVal comma As Boolean = True, Optional ByVal additional As String = "") As String
        Try
            If IsDBNull(value) Then
                Return "NULL" & If(comma, "," & vbCrLf, additional)

            ElseIf value.GetType.Name = "Date" Then
                If value Is Nothing OrElse (IsDate(value) AndAlso CDate(value).Year < 1900) Then
                    Return "NULL" & If(comma, "," & vbCrLf, additional)
                Else
                    Return "'" & DateTime.Parse(value).ToString(DATE_FORMAT) & "'" & If(comma, "," & vbCrLf, additional)
                End If

            ElseIf value.GetType.Name = "DateTime" Then
                If value Is Nothing OrElse (IsDate(value) AndAlso CDate(value).Year < 1900) Then
                    Return "NULL" & If(comma, "," & vbCrLf, additional)
                Else
                    Return "'" & DateTime.Parse(value).ToString(DATETIME_FORMAT) & "'" & If(comma, "," & vbCrLf, additional)
                End If
            Else
                value = IfNothing(value, "")

                If value.GetType.Name = "String" Then
                    value = value.ToString.TrimEnd.Filter
                End If

                Return "'" & value.ToString & "'" & If(comma, "," & vbCrLf, additional)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "String"

    <Extension()>
    Public Function Decrypt(ByVal value As String) As String
        Try
            Return ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(If(value, "").ToString))
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function Encrypt(ByVal value As String) As String
        Try
            Return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(If(value, "").ToString))
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function Filter(ByVal text As String) As String
        Try
            text = text.Replace("[", "")
            text = text.Replace("]", "")
            text = text.Replace("{", "")
            text = text.Replace("}", "")
            text = text.Replace("'", "''")
            Return text
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function ToProperCase(ByVal text As String) As String
        Dim new_str As String = ""
        Try
            Dim toUpper As Boolean = True
            text = Trim(text).Replace("_", " ")
            For Each character As Char In text
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
            new_str = text
        End Try
        Return new_str.Replace(" Of ", " of ")
    End Function

    <Extension()>
    Public Function ToImage(ByVal text As String) As Image
        Try
            If text.Length > 0 Then
                Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(text)))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function ToText(ByVal image As Image, Optional format As ImageFormat = Nothing) As String
        Try
            Dim ms As New IO.MemoryStream
            image.Save(ms, IfNothing(format, Imaging.ImageFormat.Png))
            Dim st As String = Convert.ToBase64String(ms.ToArray)
            ms.Close()
            Return st
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()>
    Public Function ToSQLType(ByVal value As String) As Integer
        Try
            Select Case value
                Case "INSERT" : Return 0
                Case "UPDATE" : Return 1
                Case "DELETE" : Return 2
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "ToolStripMenuItem"

    <Extension()>
    Public Sub CheckAdd(ByVal button As UltraButton, Optional moduleName As String = "")
        Try
            If Core.Current.Rights IsNot Nothing Then
                moduleName = If(moduleName = "", button.Tag, moduleName)
                button.Enabled = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Add)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub CheckEdit(ByVal button As UltraButton, Optional moduleName As String = "")
        Try
            If Core.Current.Rights IsNot Nothing Then
                moduleName = If(moduleName = "", button.Tag, moduleName)
                button.Enabled = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Edit)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub CheckDelete(ByVal button As UltraButton, Optional moduleName As String = "")
        Try
            If Core.Current.Rights IsNot Nothing Then
                moduleName = If(moduleName = "", button.Tag, moduleName)
                button.Enabled = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Delete)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub CheckPrint(ByVal button As UltraButton, Optional moduleName As String = "", Optional moduleOwner As String = "")
        Try
            If Core.Current.Rights IsNot Nothing Then
                moduleName = If(moduleName = "", button.Tag, moduleName)
                button.Enabled = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Print, moduleOwner)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub CheckEdit(ByVal grid As UltraGrid, Optional moduleName As String = "", Optional retainEditActivation As Boolean = False)
        Try
            If Core.Current.Rights Is Nothing Then
                Return
            Else
                If moduleName = "" Then
                    moduleName = grid.Tag
                End If
                Dim hasEdit As Boolean = Core.Current.Rights.IsAllowed(moduleName, Rights.AccessRights.Edit)
                For Each row As UltraGridRow In grid.Rows
                    If hasEdit Then
                        If Not retainEditActivation Then
                            row.Activation = Activation.AllowEdit
                        End If
                    Else
                        row.Activation = Activation.NoEdit
                    End If
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "UltraGrid"

    ''' <summary>Check the specified grid for possible errors. If an error is found, </summary>
    ''' <param name="grid">Specifies the grid to be checked.</param>
    ''' <param name="gridDataDescription">A string value that describes the data in grid. Is used for display when error(s) occur.</param>
    ''' <param name="requiredColumnKeys">A comma-delimited list of column keys that are required for checking.</param>
    ''' <param name="skipColumnKeys">A comma-delimited list of column keys that will be skipped for checking.</param>
    ''' <param name="mustNotZeroColumnKeys">A comma-delimited list of column keys that must not be zero, else it's invalid.</param>
    ''' <param name="emptyGridIsInvalid">If True then Grid is invalid if it contains no rows.</param>
    ''' <param name="ifThisColumnIsSet">If this cell is set, then the cells specified in _ThenSkipThisColumns will be skipped.</param>
    ''' <param name="thenSkipThisColumns">A comma-delimited list of column keys that will be skipped if the cell specified by _IfThisColumnIsSet is set.</param>
    ''' <param name="startDateColumnKey">Start Date column key. Will be checked if start date exceeds the end date.</param>
    ''' <param name="endDateColumnKey">End Date column key. Will be checked if start date exceeds end date.</param>
    ''' <remarks>If _RequiredColumnKeys is set, _SkipColumnKeys is disregarded and vise versa.</remarks>
    <Extension()>
    Public Sub GetErrors(
            ByVal grid As UltraGrid,
            Optional ByVal gridDataDescription As String = "Details",
            Optional ByVal requiredColumnKeys As String = "",
            Optional ByVal skipColumnKeys As String = "",
            Optional ByVal mustNotZeroColumnKeys As String = "",
            Optional ByVal emptyGridIsInvalid As Boolean = False,
            Optional ByVal ifThisColumnIsSet As String = "",
            Optional ByVal thenSkipThisColumns As String = "",
            Optional ByVal startDateColumnKey As String = "",
            Optional ByVal endDateColumnKey As String = "",
            Optional ByVal mustGreaterThanZeroColumn As String = "")
        Try
            If gridDataDescription <> "" Then
                gridDataDescription &= " "
            End If

            If emptyGridIsInvalid Then
                If grid.Rows.Count = 0 Then
                    AddToInvalidFields(IIf(gridDataDescription.Trim = "", "Details ", gridDataDescription) & " is required")
                    Return ' No rows to process
                End If
            End If

            grid.UpdateData() ' Accept grid changes (not the datasource)

            Dim arrRequired As String() = requiredColumnKeys.Split(",")
            Dim arrSkipped As String() = skipColumnKeys.Split(",")
            Dim arrNotZero As String() = mustNotZeroColumnKeys.Split(",")
            Dim arrOptionalSkip As String() = thenSkipThisColumns.Split(",")
            Dim arrGreaterThanZero As String() = mustGreaterThanZeroColumn.Split(",")

            Dim hasError As Boolean = False

            For Each row As UltraGridRow In grid.Rows
                If startDateColumnKey <> "" And endDateColumnKey <> "" Then
                    If IsDate(row.Cells(startDateColumnKey).Value) And IsDate(row.Cells(endDateColumnKey).Value) Then
                        If CDate(row.Cells(startDateColumnKey).Value) > CDate(row.Cells(endDateColumnKey).Value) Then
                            AddToInvalidFields("Start date must not exceed the end date")
                            row.Activate()
                        End If
                    End If
                End If
                For Each cell As UltraGridCell In row.Cells
                    If cell.Column.Hidden Then
                        Continue For
                    End If
                    If requiredColumnKeys <> "" Then
                        If Array.IndexOf(arrRequired, cell.Column.Key) < 0 Then
                            Continue For ' field is not required
                        End If
                    ElseIf skipColumnKeys <> "" Then
                        If ifThisColumnIsSet <> "" Then
                            If IsDBNull(row.Cells(ifThisColumnIsSet).Value) Then
                                '
                            ElseIf row.Cells(ifThisColumnIsSet).Value Is Nothing Then
                                '
                            ElseIf row.Cells(ifThisColumnIsSet).Value = "" Then
                                '
                            ElseIf thenSkipThisColumns <> "" Then
                                If Array.IndexOf(arrOptionalSkip, cell.Column.Key) >= 0 Then
                                    Continue For ' skip this column 
                                End If
                            End If
                        End If
                        If Array.IndexOf(arrSkipped, cell.Column.Key) >= 0 Then
                            Continue For ' skip this column
                        End If
                    End If

                    If IsDBNull(cell.Value) Then
                        hasError = True
                    ElseIf cell.Text = "" Then
                        hasError = True
                    ElseIf mustNotZeroColumnKeys <> "" Then
                        If Array.IndexOf(arrNotZero, cell.Column.Key) >= 0 Then
                            If cell.Value = 0 Then
                                If gridDataDescription = "" Then
                                    AddToInvalidFields(cell.Column.Header.Caption & " must no be zero")
                                Else
                                    AddToInvalidFields(cell.Column.Header.Caption & " on " & gridDataDescription & "grid must not be zero")
                                End If
                            End If
                        End If
                    End If

                    If mustGreaterThanZeroColumn <> "" Then
                        If Array.IndexOf(arrGreaterThanZero, cell.Column.Key) >= 0 Then
                            If Not cell.Value > 0 Then
                                AddToInvalidFields(cell.Column.Header.Caption & " must be greater than zero")
                            End If
                        End If
                    End If


                    If hasError Then
                        If gridDataDescription = "" Then
                            AddToInvalidFields(cell.Column.Header.Caption & " is required")
                        Else
                            AddToInvalidFields(cell.Column.Header.Caption & " on " & gridDataDescription & "grid is required")
                        End If
                        hasError = False
                    End If
                Next
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub GetDuplicateRecords(ByVal grid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal columnName As String, Optional ByVal description As String = "records")
        Try
            With grid
                .UpdateData()

                Dim hasDuplicate As Boolean = False
                For i As Integer = 0 To .Rows.Count - 1
                    If String.IsNullOrEmpty(.Rows(i).Cells(columnName).Value) Then
                        Continue For
                    End If
                    For j As Integer = 0 To .Rows.Count - 1
                        If IsDBNull(.Rows(j).Cells(columnName).Value) Then
                            Continue For
                        ElseIf i = j Then
                            Continue For
                        ElseIf String.IsNullOrEmpty(.Rows(j).Cells(columnName).Value) Then
                            Continue For
                        End If
                        If .Rows(i).Cells(columnName).Value.ToString.ToLower = .Rows(j).Cells(columnName).Value.ToString.ToLower Then
                            AddToInvalidFields("Duplicate " & description & " found")
                            .Rows(i).Activate()
                            hasDuplicate = True
                            Exit For
                        End If
                    Next
                    If hasDuplicate Then
                        Exit For
                    End If
                Next
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub SetWidth(ByRef column As UltraGridColumn, ByVal width As Integer, Optional ByVal maxWidth As Integer = -1)
        Try
            If width > maxWidth Then
                maxWidth = width
            End If
            With column
                .Width = width
                .MinWidth = 0
                .MaxWidth = 0
                .MaxWidth = maxWidth
                .MinWidth = width
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "UltraGroupBox"

    <Extension()>
    Public Sub Clean(ByVal groupBox As UltraGroupBox)
        Try
            For Each ctrl As Control In groupBox.Controls
                With ctrl
                    Select Case .GetType.Name
                        Case "UltraTextEditor"
                            With DirectCast(ctrl, UltraTextEditor)
                                If Not .ReadOnly And .Enabled Then
                                    ctrl.Text = ctrl.Text.Trim
                                End If
                            End With
                        Case "UltraComboEditor"
                            With DirectCast(ctrl, UltraComboEditor)
                                If .DropDownStyle <> Infragistics.Win.DropDownStyle.DropDownList And Not .ReadOnly And .Enabled Then
                                    .Text = .Text.Trim
                                End If
                            End With
                    End Select
                End With
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub Clear(ByVal grp As UltraGroupBox, Optional ByVal excludeRadioButtons As Boolean = False)
        Try
            For Each ctrl As Control In grp.Controls
                With ctrl
                    Select Case .GetType.Name
                        Case "UltraCalendarCombo"
                            DirectCast(ctrl, UltraCalendarCombo).Value = Now
                        Case "UltraCheckEditor"
                            DirectCast(ctrl, UltraCheckEditor).Checked = False
                        Case "UltraComboEditor"
                            DirectCast(ctrl, UltraComboEditor).SelectedIndex = -1
                        Case "UltraDateTimeEditor"
                            DirectCast(ctrl, UltraDateTimeEditor).Value = Now
                        Case "UltraGrid"
                            With DirectCast(ctrl, UltraGrid)
                                If .DataSource IsNot Nothing Then
                                    If .DataSource.GetType.Name.ToLower = "datatable" Then
                                        .DataSource.Rows.Clear()
                                    Else
                                        For Each table As DataTable In .DataSource.Tables
                                            table.Rows.Clear()
                                        Next
                                    End If
                                End If
                            End With
                        Case "UltraNumericEditor"
                            DirectCast(ctrl, UltraNumericEditor).Value = 0
                        Case "UltraOptionSet"
                            If Not excludeRadioButtons Then
                                DirectCast(ctrl, UltraOptionSet).CheckedIndex = -1
                            End If
                        Case "UltraTextEditor"
                            DirectCast(ctrl, UltraTextEditor).Clear()
                        Case "UltraCurrencyEditor"
                            DirectCast(ctrl, UltraCurrencyEditor).Value = 0
                            ' CUSTOM CONTROLS
                    End Select
                End With
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Sub GetErrors(ByVal groupBox As UltraGroupBox, Optional ByVal skipHidden As Boolean = True)
        Try
            groupBox.Clean()

            For Each ctrl As Control In groupBox.Controls
                With ctrl
                    If skipHidden Then
                        If .Visible = False Then
                            Continue For
                        End If
                    End If
                    If .Tag IsNot Nothing Then
                        If .Tag.ToString.Trim = "" Then
                            Continue For
                        End If
                        Select Case .GetType.Name
                            Case "UltraComboEditor", "UltraTextEditor"
                                If .Text = "" Then
                                    AddToInvalidFields(.Tag.ToString & " is required")
                                End If
                            Case "UltraNumericEditor"
                                If DirectCast(ctrl, UltraNumericEditor).Value = 0 Then
                                    AddToInvalidFields(.Tag.ToString & " is required")
                                End If
                            Case "UltraOptionSet"
                                If DirectCast(ctrl, UltraOptionSet).CheckedIndex = -1 Then
                                    AddToInvalidFields(.Tag.ToString & " is required")
                                End If
                        End Select
                    End If
                End With
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    <Extension()>
    Public Function HasErrors(ByVal groupBox As UltraGroupBox, Optional ByVal errorTitle As String = "Save Failed", Optional ByVal hasExistingError As Boolean = False) As Boolean
        Try
            If Not hasExistingError Then
                ClearInvalidFields()
            End If

            groupBox.GetErrors()

            If HasInvalidFields() Then
                ShowInvalidFields(GetInvalidFields, errorTitle)
            End If

            Return HasInvalidFields()
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Module