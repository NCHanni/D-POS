Friend Module Methods

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
                Core.Current.FormMain.WindowState = sender.WindowState
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Friend Function LaunchForm(ByRef frm As Form, Optional ByVal isModal As Boolean = True, Optional ByVal owner As Form = Nothing) As DialogResult
        Try
            Dim ret As DialogResult = DialogResult.None

            Core.Current.FormatUI.Apply(frm)

            If isModal Then
                With frm
                    .StartPosition = FormStartPosition.CenterScreen
                    .ShowInTaskbar = True

                    m_ModalFormShown = True

                    AddHandler frm.Paint, AddressOf Form_Paint

                    If owner Is Nothing Then
                        ret = .ShowDialog(Core.Current.FormMain)
                    Else
                        ret = .ShowDialog(owner)
                    End If

                    If m_NewFormActivated Then
                        EnsureFormView(m_NewFormCaption)
                        m_NewFormActivated = False
                        m_NewFormCaption = ""
                    End If

                    m_ModalFormShown = False

                    Return ret
                End With
            Else
                With frm
                    Dim found As Boolean = False
                    Dim formMain As FormMain = Core.Current.FormMain
                    For Each frmChild As Form In formMain.MdiChildren
                        If frmChild.Text = frm.Text Then
                            found = True
                            With formMain.TabbedMdiManager.TabFromForm(frmChild)
                                .Show()
                                .Activate()
                            End With
                            Exit For
                        End If
                    Next
                    If Not found Then
                        .WindowState = FormWindowState.Maximized
                        .MdiParent = formMain
                        .Padding = New Padding(3)
                        .Show()
                        ret = DialogResult.None
                    End If
                    m_NewFormCaption = .Text
                    m_NewFormActivated = True

                    AddHandler frm.Activated, AddressOf Form_Activated
                    Return ret
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Friend Function EnsureFormView(ByVal caption As String, Optional ByVal findOnly As Boolean = False) As Boolean
        Try
            Dim formMain As FormMain = Core.Current.FormMain
            For Each group As Infragistics.Win.UltraWinTabbedMdi.MdiTabGroup In formMain.TabbedMdiManager.TabGroups
                For Each tab As Infragistics.Win.UltraWinTabbedMdi.MdiTab In group.Tabs
                    If tab.Form.Text = caption Then
                        If Not findOnly Then
                            With tab
                                .Show()
                                .EnsureTabInView()
                                .Activate()
                            End With
                        End If
                        Return True
                    End If
                Next
            Next
            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Message Box"

    Friend Function ShowSaveDraftQuestion(Optional ByVal owner As Object = Nothing) As DialogResult
        Try
            Dim f As New FormSaveDraftPrompt
            If owner Is Nothing Then
                Return f.ShowDialog
            Else
                Return f.ShowDialog(owner)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Friend Sub ShowDeleteNotPossibleMessage()
        ShowMessage("Record is already being referenced.\n\nDelete not permitted!", "Delete Failed", MessageIcon.Exclamation)
    End Sub

#End Region

#Region "Others"

    Friend Function CStrEx(value As Object) As String
        Try
            If IsDBNull(value) OrElse value Is Nothing Then
                Return ""
            Else
                Return CStr(value).Trim
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Friend Sub LoadSettingsAndPreferences()
        Try
            Dim settings As New SettingsPreferences
            With settings
                .Fill()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Friend Sub SaveAuditTrail(activity As String, Optional form As Form = Nothing)
        Try
            If form Is Nothing Then
                With New AuditTrail
                    .TerminalCode = Current.Settings.TerminalCode
                    .UserCode = Current.User.Code
                    .UserName = Current.User.Name
                    .Activity = activity
                    .SaveAuditTrail()
                End With
            Else
                With New TaskProcess(form)
                    .Start(
                        Sub()
                            With New AuditTrail
                                .TerminalCode = Current.Settings.TerminalCode
                                .UserCode = Current.User.Code
                                .UserName = Current.User.Name
                                .Activity = activity
                                .SaveAuditTrail()
                            End With
                        End Sub
                    )
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Module