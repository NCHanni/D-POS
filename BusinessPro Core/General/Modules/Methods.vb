Imports System.Security.Principal

Public Module Methods

#Region "GetNextCode"

    Public Sub GetNextCode(ByVal prefix As String, ByVal tableName As String, ByVal length As Integer, ByRef code As String, Optional ByVal whereClause As String = "")
        Try
            Dim sql As String =
                "DECLARE @code VARCHAR(255);" & vbCrLf &
                "EXECUTE dbo.GetNextCode '" & prefix & "', '" & tableName & "', " & length & ", @code OUTPUT, '" & whereClause & "';" & vbCrLf &
                "SELECT @code;"

            Dim dataSource As New DataSource(Current.ConnectionString)
            code = dataSource.ExecuteQuery(sql).Tables(0).Rows(0).Item(0).ToString
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetNextCodeSQL(ByVal prefix As String, ByVal tableName As String, ByVal length As Integer, Optional ByVal varName As String = "@code") As String
        Try
            Dim sql As String =
                "DECLARE " & varName & " varchar(255);" & vbCrLf &
                "EXEC dbo.GetNextCode '" & prefix & "', '" & tableName & "', " & length & ", " & varName & " OUTPUT;" & vbCrLf

            Return sql
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Invalid Data Handler"

    ' Sample Usage:
    ' Public Function IsValid() As Boolean
    '   ClearInvalidFields()
    '   If txtCode.Text = "" Then
    '       AddToInvalidFields("Code is required")
    '   End If
    '   If txtName.Text = "" Then
    '       AddToInvalidFields("Name is required")
    '   End If
    '   If HasInvalidFields() Then
    '       ShowInvalidFields(GetInvalidFields) 
    '   End If
    '   Return Not HasInvalidFields()
    ' End Function

    Private m_InvalidFieldsList As String = ""

    Public Sub AddToInvalidFields(ByVal _Description As String)
        If m_InvalidFieldsList.StartsWith(_Description) Or m_InvalidFieldsList.Contains("," & _Description) Then
            Return ' error is already listed
        End If
        m_InvalidFieldsList &= IIf(m_InvalidFieldsList = "", "", ",") & _Description & "!"
    End Sub

    Public Sub ClearInvalidFields()
        m_InvalidFieldsList = ""
    End Sub

    Public Function GetInvalidFields(Optional ByVal _ClearInvalidFieldsAfter As Boolean = False) As String
        Dim str As String = m_InvalidFieldsList
        If _ClearInvalidFieldsAfter Then
            ClearInvalidFields()
        End If
        Return str
    End Function

    Public Function HasInvalidFields() As Boolean
        Return (m_InvalidFieldsList <> "")
    End Function

#End Region

#Region "LaunchForm"

    Private m_NewFormActivated As Boolean
    Private m_NewFormCaption As String = ""
    Private m_ModalFormShown As Boolean

    Private Sub Form_Activated(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If m_ModalFormShown Then
                Return
            End If
            m_NewFormActivated = True
            m_NewFormCaption = sender.Text
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        Try
            If sender.WindowState <> FormWindowState.Normal Then
                Current.FormMain.WindowState = sender.WindowState
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Public Function LaunchForm(ByRef _Form As Form, Optional ByVal _IsModal As Boolean = True, Optional ByVal _Owner As System.Windows.Forms.IWin32Window = Nothing) As DialogResult
        Try
            Dim ret As DialogResult = DialogResult.None

            If Current.FormatUI Is Nothing Then
                Current.FormatUI = New FormatUI
            End If

            Current.FormatUI.Apply(_Form)

            If _IsModal Then
                With _Form
                    .StartPosition = FormStartPosition.CenterScreen
                    .ShowInTaskbar = True

                    m_ModalFormShown = True

                    AddHandler _Form.Paint, AddressOf Form_Paint

                    If _Owner Is Nothing Then
                        ret = .ShowDialog(Current.FormMain)
                    Else
                        ret = .ShowDialog(_Owner)
                    End If

                    If m_NewFormActivated Then
                        m_NewFormActivated = False
                        m_NewFormCaption = ""
                    End If

                    m_ModalFormShown = False

                    Return ret
                End With
            Else
                With _Form
                    .WindowState = FormWindowState.Maximized
                    .MdiParent = Current.FormMain
                    .Padding = New Padding(3)
                    .Show()

                    ret = DialogResult.None

                    m_NewFormCaption = .Text
                    m_NewFormActivated = True

                    AddHandler _Form.Activated, AddressOf Form_Activated
                    Return ret
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Null Replacer"

    ''' <summary>
    ''' Check if a value is Nothing or DBNull and replace it with a new value if True.
    ''' </summary>
    ''' <param name="value">Value to check if Nothing or DBNull.</param>
    ''' <param name="newValue">New value if Nothing or DBNull.</param>
    Public Function IfEmpty(ByVal value As Object, ByVal newValue As Object) As Object
        If IsEmpty(value) Then
            Return newValue
        Else
            Return value
        End If
    End Function


    ''' <summary>
    ''' Check if a value is Nothing or DBNull.
    ''' </summary>
    ''' <param name="value">Value to check if Nothing or DBNull.</param>
    Public Function IsEmpty(ByVal value As Object) As Boolean
        If value Is Nothing Then
            Return True
        ElseIf IsDBNull(value) Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region "Others"

    Public Function IsDateValid(dateValue As Object) As Boolean
        If IsDBNull(dateValue) OrElse String.IsNullOrWhiteSpace(dateValue) OrElse dateValue = CDate(Nothing) OrElse dateValue < CDate("1/1/1753") Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function GetDateForControl(dateValue As Object) As Object
        If IsDateValid(dateValue) Then
            Return dateValue
        Else
            Return DBNull.Value
        End If
    End Function

    Public Function GetDateForVariable(dateValue As Object) As Date
        If IsDateValid(dateValue) Then
            Return dateValue
        Else
            Return Nothing
        End If
    End Function

    Public Function GetDateForNAVString(dateValue As Object, Optional includeTime As Boolean = False) As String
        If IsDateValid(dateValue) Then
            If includeTime Then
                Return dateValue.ToString
            Else
                Return CDate(dateValue).ToDateString
            End If
        Else
            Return ""
        End If
    End Function

    Public Function IsWindowsAdmin() As Boolean
        Try
            Dim identity As WindowsIdentity = WindowsIdentity.GetCurrent()
            Dim principal As WindowsPrincipal = New WindowsPrincipal(identity)

            Return principal.IsInRole(WindowsBuiltInRole.Administrator)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

End Module