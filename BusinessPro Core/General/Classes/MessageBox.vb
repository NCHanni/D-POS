#Region "Imports"

Imports System.Windows
Imports System.Windows.Forms.MessageBoxButtons
Imports System.Windows.Forms.MessageBoxIcon
Imports Infragistics.Win.UltraMessageBox

#End Region

Public Class MessageBox

#Region "Declarations"

    Public Enum QuestionResult
        Yes = DialogResult.Yes
        No = DialogResult.No
    End Enum

    Public Enum MessageIcon
        Information = MessageBoxIcon.Information
        Exclamation = MessageBoxIcon.Exclamation
        ErrorIcon = MessageBoxIcon.Error
    End Enum

#End Region

#Region "Default Captions and Messages"

    Private Const DEFAULT_MESSAGE_CAPTION As String = "Information"
    Private Const DEFAULT_QUESTION_CAPTION As String = "Question"
    Private Const DEFAULT_ERROR_CAPTION As String = "Error"

    Private Const DEFAULT_SAVE_MESSAGE_CAPTION As String = "Save Successful"
    Private Const DEFAULT_SAVE_MESSAGE As String = "New record has been successfully created."

    Private Const DEFAULT_SAVE_SUCCESFULL As String = "Record(s) succesfully saved."

    Private Const DEFAULT_UPDATE_MESSAGE_CAPTION As String = "Update Successful"
    Private Const DEFAULT_UPDATE_MESSAGE As String = "Changes to record(s) have been successfully applied."

    Private Const DEFAULT_DELETE_MESSAGE_CAPTION As String = "Delete Successful"
    Private Const DEFAULT_DELETE_MESSAGE As String = "Selected record has been successfully deleted."

    Private Const DEFAULT_CANCEL_QUESTION_CAPTION As String = "Confirm Cancel"
    Private Const DEFAULT_CANCEL_QUESTION As String = ("Record(s) have been modified, unsaved changes will be lost." & vbNewLine & vbNewLine & "Do you want to continue?")

    Private Const DEFAULT_DELETE_QUESTION_CAPTION As String = "Confirm Delete"
    Private Const DEFAULT_DELETE_QUESTION As String = "Selected record will be deleted." & vbNewLine & vbNewLine & "Do you want to continue?"

    Private Const DEFAULT_REMOVE_QUESTION_CAPTION As String = "Confirm Remove"
    Private Const DEFAULT_REMOVE_QUESTION As String = "Selected record will be removed." & vbNewLine & vbNewLine & "Do you want to continue?"

    Private Const DEFAULT_INVALID_MESSAGE_CAPTION As String = "Save Failed"
    Private Const DEFAULT_INVALID_MESSAGE As String = "The following error(s) occured:" & vbNewLine & vbNewLine & "<invalid_fields>"

    Private Const DEFAULT_SELECT_MESSAGE_CAPTION As String = "No Selection"
    Private Const DEFAULT_SELECT_MESSAGE As String = "Please select a record<action>!"

    Private Const DEFAULT_NO_DATA_CAPTION As String = "No Data"
    Private Const DEFAULT_NO_DATA_PRINT As String = "No data <action>!"

    Private Const DEFAULT_NO_DATA_PRINTING As String = "Report contains no data for printing"

    Private Const DEFAULT_SELECT_QUESTION_CAPTION As String = "Confirm Selection"
    Private Const DEFAULT_SELECT_QUESTION As String = "Another record is currently being modified, unsaved changes will be lost." & vbNewLine & vbNewLine & "Do you want to continue?"

    Private Const DEFAULT_EXITAPP_QUESTION_CAPTION As String = "Confirm Exit"
    Private Const DEFAULT_EXITAPP_QUESTION As String = "The application will be closed. Do you want to continue?"

    Private Const DEFAULT_LOGOFF_QUESTION_CAPTION As String = "Confirm Log Off"
    Private Const DEFAULT_LOGOFF_QUESTION As String = "Current user will be logged off. Do you want to continue?"

    Private Const DEFAULT_SAVE_NO_CHANGES_CAPTION As String = "Save failed"
    Private Const DEFAULT_SAVE_NO_CHANGES As String = "No changes have been made."

#End Region

#Region "Custom MessageBox Function"

    ''' <summary>Displays a message box with specified message.</summary>
    ''' <param name="text">The message to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    ''' <param name="icon">One of the MessageBox.MessageIcon values that specifies which icon to display in the message box.</param>
    Public Shared Sub ShowMessage(ByVal text As String, Optional ByVal caption As String = DEFAULT_MESSAGE_CAPTION, Optional ByVal icon As MessageIcon = MessageIcon.Information)
        If IsNumeric(caption) Then
            icon = CInt(caption)
            caption = DEFAULT_MESSAGE_CAPTION
        End If
        text = text.Replace("\n", vbNewLine)
        Show(text, caption, MessageBoxButtons.OK, icon)
    End Sub

    Public Shared Sub ShowSaveSuccesful()
        Show(DEFAULT_SAVE_SUCCESFULL, DEFAULT_SAVE_MESSAGE_CAPTION, OK, Information)
    End Sub

    Public Shared Sub ShowSaveNoChanges()
        Show(DEFAULT_SAVE_NO_CHANGES, DEFAULT_SAVE_NO_CHANGES_CAPTION, OK, Information)
    End Sub

    ''' <summary>Displays a warning message box with specified message.</summary>
    ''' <param name="text">The message to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    Public Shared Sub ShowWarning(ByVal text As String, Optional ByVal caption As String = DEFAULT_MESSAGE_CAPTION)
        If IsNumeric(caption) Then
            caption = DEFAULT_MESSAGE_CAPTION
        End If
        text = text.Replace("\n", vbNewLine)
        Show(text, caption, OK, Exclamation)
    End Sub

    ''' <summary>Display a message box with the specified question requiring the user for an answer.</summary>
    ''' <param name="text">The question to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowQuestion(ByVal text As String, Optional ByVal caption As String = DEFAULT_QUESTION_CAPTION) As QuestionResult
        If IsNumeric(caption) Then
            caption = DEFAULT_QUESTION_CAPTION
        End If
        text = text.Replace("\n", vbNewLine)
        Return Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
    End Function

    Public Shared Function ShowQuestionCancel(ByVal text As String, Optional ByVal caption As String = DEFAULT_QUESTION_CAPTION) As DialogResult
        If IsNumeric(caption) Then
            caption = DEFAULT_QUESTION_CAPTION
        End If
        text = text.Replace("\n", vbNewLine)
        Return Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
    End Function

    ''' <summary>Display a message box with specified error details.</summary>
    ''' <param name="ex">A System.Exception instance that holds the error details.</param>
    ''' <remarks>The source of the error is shown as part of the text displayed in the title bar of the message box.</remarks>
    Public Shared Sub ShowError(ByVal ex As Exception)
        Dim arr As String() = ex.StackTrace.Split(vbCrLf)
        Dim str As String = ""
        Dim enu As IEnumerator = arr.GetEnumerator
        While enu.MoveNext
            With enu.Current
                If .ToString Like "???????" & My.Application.Info.ProductName & "*" Then
                    Try
                        str = enu.Current.ToString.Substring(enu.Current.ToString.IndexOf(".") + 1, enu.Current.ToString.IndexOf("(") - 11)
                    Catch
                        str = My.Application.Info.ProductName
                    End Try
                    Exit While
                End If
            End With
        End While
        Dim msg As String = ex.Message
        If msg.ToLower.Contains("integration settings") Then
            msg = "Integration settings has not been set yet." & vbNewLine & vbNewLine & "Please check your settings!"
        ElseIf msg.Contains("ROLLBACK") Then
            msg = "Unable to continue. Possible cause may be in your integration settings." & vbNewLine & vbNewLine & "Please check your settings!"
        End If
        Show(msg & vbNewLine & ex.StackTrace, "Error" & IIf(str = "", "", " in " & str), OK, Exclamation)
    End Sub

    Public Shared Sub ShowError(ByVal text As String, Optional ByVal caption As String = DEFAULT_ERROR_CAPTION)
        If IsNumeric(caption) Then
            caption = DEFAULT_ERROR_CAPTION
        End If
        text = text.Replace("\n", vbNewLine)
        Show(text, caption, OK, [Error])
    End Sub

    ''' <summary>Display a message box with a predefined text showing a list of invalid field(s) to be inputted.</summary>
    ''' <param name="fields">A comma-delimited string of invalid field names.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    Public Shared Sub ShowInvalidFields(ByVal fields As String, Optional ByVal caption As String = DEFAULT_INVALID_MESSAGE_CAPTION)
        Show(DEFAULT_INVALID_MESSAGE.Replace("<invalid_fields>", fields.Replace(",", vbCrLf)), caption, OK, Exclamation)
    End Sub

    ''' <summary>Display a message box with a predefined message stating a successful saving of record.</summary>
    Public Shared Sub ShowSaveMessage()
        Show(DEFAULT_SAVE_MESSAGE, DEFAULT_SAVE_MESSAGE_CAPTION, OK, Information)
    End Sub

    ''' <summary>Display a message box with a predefined message stating a successful updating of record.</summary>
    Public Shared Sub ShowUpdateMessage()
        Show(DEFAULT_UPDATE_MESSAGE, DEFAULT_UPDATE_MESSAGE_CAPTION, OK, Information)
    End Sub

    ''' <summary>Display a message box with a predefined message stating a successful deletion of record.</summary>
    Public Shared Sub ShowDeleteMessage()
        Show(DEFAULT_DELETE_MESSAGE, DEFAULT_DELETE_MESSAGE_CAPTION, OK, Information)
    End Sub


    ''' <summary>Display a message box with a predefined message stating the user to select a record to process.</summary>
    Public Shared Sub ShowNoDataMessage(Optional ByVal action As String = "")
        Show(DEFAULT_NO_DATA_PRINT.Replace("<action>", If(action = "", "", " to " & action.ToLower)), DEFAULT_NO_DATA_CAPTION, OK, Exclamation)
    End Sub


    Public Shared Sub ShowNoDataPrinting()
        Show(DEFAULT_NO_DATA_PRINTING, DEFAULT_NO_DATA_CAPTION, OK, Exclamation)
    End Sub

    ''' <summary>Display a message box with a predefined message stating the user to select a record to process.</summary>
    Public Shared Sub ShowSelectMessage(Optional ByVal action As String = "")
        Show(DEFAULT_SELECT_MESSAGE.Replace("<action>", If(action = "", "", " to " & action.ToLower)), DEFAULT_SELECT_MESSAGE_CAPTION, OK, Exclamation)
    End Sub

    ''' <summary>Display a message box with a predefined question requiring the user for an action of selecting a record.</summary>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowSelectQuestion() As QuestionResult
        Return Show(DEFAULT_SELECT_QUESTION, DEFAULT_SELECT_QUESTION_CAPTION, YesNo, Question, MessageBoxDefaultButton.Button2)
    End Function

    ''' <summary>Display a message box with a predefined question requiring the user for an action of losing unsaved changes.</summary>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowCancelQuestion(Optional ByVal caption As String = DEFAULT_CANCEL_QUESTION_CAPTION) As QuestionResult
        Return Show(DEFAULT_CANCEL_QUESTION, caption, YesNo, Question, MessageBoxDefaultButton.Button2)
    End Function

    ''' <summary>Display a message box with a predefined question requiring the user for an action of deleting a record.</summary>
    ''' <param name="recordName">The name of record that will be displayed on the question.</param>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowDeleteQuestion(Optional ByVal recordName As String = "") As QuestionResult
        Dim msg As String = DEFAULT_DELETE_QUESTION
        If recordName <> "" Then
            msg = DEFAULT_DELETE_QUESTION.Replace("Selected record", recordName)
        End If
        Return Show(msg, DEFAULT_DELETE_QUESTION_CAPTION, YesNo, Question, MessageBoxDefaultButton.Button2)
    End Function

    ''' <summary>Display a message box with a predefined question requiring the user for an action of removing a record.</summary>
    ''' <param name="recordName">The name of record that will be displayed on the question.</param>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowRemoveQuestion(Optional ByVal recordName As String = "") As QuestionResult
        Dim msg As String = DEFAULT_REMOVE_QUESTION
        If recordName <> "" Then
            msg = DEFAULT_REMOVE_QUESTION.Replace("Selected record", recordName)
        End If
        Return Show(msg, DEFAULT_REMOVE_QUESTION_CAPTION, YesNo, Question, MessageBoxDefaultButton.Button2)
    End Function

    ''' <summary>Display a message box with a predefined question requiring the user for an action of exiting the application.</summary>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowExitQuestion() As QuestionResult
        Return Show(DEFAULT_EXITAPP_QUESTION, DEFAULT_EXITAPP_QUESTION_CAPTION, YesNo, Question, MessageBoxDefaultButton.Button2)
    End Function

    ''' <summary>Display a message box with a predefined question requiring the user for an action of logging off current user login.</summary>
    ''' <returns>One of the MessageBox.QuestionResult values.</returns>
    Public Shared Function ShowLogOffQuestion() As QuestionResult
        Return Show(DEFAULT_LOGOFF_QUESTION, DEFAULT_LOGOFF_QUESTION_CAPTION, YesNo, Question, MessageBoxDefaultButton.Button2)
    End Function

#End Region

#Region "Original MessageBox Functions"

    Private Shared Function FormatText(ByVal text As String) As String
        Return text.Replace(vbNewLine, vbCrLf).Trim
    End Function

    Private Shared Function GetIconType(ByVal text As String) As MessageBoxIcon
        Dim Icon As MessageBoxIcon
        Select Case text.Substring(text.Length - 1)
            Case "." : Icon = Information
            Case "!" : Icon = Exclamation
            Case "?" : Icon = Question
            Case Else : Icon = None
        End Select
        Return Icon
    End Function

    ''' <summary>Displays a message box with specified text.</summary>
    ''' <param name="text">The text to display in the message box.</param>
    ''' <returns>One of the System.Windows.Forms.DialogResult values.</returns>
    Public Shared Function Show(ByVal text As String) As DialogResult
        Return Forms.MessageBox.Show(FormatText(text), Application.ProductName, OK, GetIconType(text))
    End Function

    ''' <summary>Displays a message box with specified text and caption.</summary>
    ''' <param name="text">The text to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    ''' <returns>One of the System.Windows.Forms.DialogResult values.</returns>
    Public Shared Function Show(ByVal text As String, ByVal caption As String) As DialogResult
        Return Forms.MessageBox.Show(FormatText(text), caption, OK, GetIconType(text))
    End Function

    ''' <summary>Displays a message box with specified text, caption, and buttons.</summary>
    ''' <param name="text">The text to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    ''' <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons values that specifies which buttons to display in the message box.</param>
    ''' <returns>One of the System.Windows.Forms.DialogResult values.</returns>
    Public Shared Function Show(ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxButtons) As DialogResult
        Return Forms.MessageBox.Show(FormatText(text), caption, buttons, GetIconType(text))
    End Function

    ''' <summary>Displays a message box with specified text, caption, buttons, and icon.</summary>
    ''' <param name="text">The text to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    ''' <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons values that specifies which buttons to display in the message box.</param>
    ''' <param name="icon">One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon to display in the message box.</param>
    ''' <returns>One of the System.Windows.Forms.DialogResult values.</returns>
    Public Shared Function Show(ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxButtons, ByVal icon As MessageBoxIcon) As DialogResult
        Return Forms.MessageBox.Show(FormatText(text), caption, buttons, icon)
    End Function

    ''' <summary>Displays a message box with the specified text, caption, buttons, icon, and default button.</summary>
    ''' <param name="text">The text to display in the message box.</param>
    ''' <param name="caption">The text to display in the title bar of the message box.</param>
    ''' <param name="buttons">One of the System.Windows.Forms.MessageBoxButtons values that specifies which buttons to display in the message box.</param>
    ''' <param name="icon">One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon to display in the message box.</param>
    ''' <param name="defaultButton">One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies the default button for the message box.</param>
    ''' <returns>One of the System.Windows.Forms.DialogResult values.</returns>
    Public Shared Function Show(ByVal text As String, ByVal caption As String, ByVal buttons As MessageBoxButtons, ByVal icon As MessageBoxIcon, ByVal defaultButton As MessageBoxDefaultButton) As DialogResult
        Return Forms.MessageBox.Show(FormatText(text), caption, buttons, icon, defaultButton)
    End Function

#End Region

End Class