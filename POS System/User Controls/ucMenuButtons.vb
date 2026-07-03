<System.ComponentModel.DefaultEvent("MenuClick")>
Friend Class ucMenuButtons

#Region "Variables"

    Private m_ClickedNew As Boolean = False

#End Region

#Region "Enum"

    ' Modified
    Public Enum MenuStates
        [Default] = 0 ' New
        [New] = 1
        [Edit] = 2
        [Save] = 3
        [Delete] = 4
        [Cancel] = 5
        [Print] = 6
        [Custom] = 7
        [Select] = 8 ' New + Edit
    End Enum

    Public Enum MenuButtons
        [New] = 1
        [Edit] = 2
        [Save] = 3
        [Delete] = 4
        [Cancel] = 5
        [Print] = 6
    End Enum

#End Region

#Region "Property"

    Private m_ErrorOccured As Boolean = False
    Public WriteOnly Property ErrorOccured() As Boolean
        Set(ByVal value As Boolean)
            m_ErrorOccured = value
        End Set
    End Property

    Private m_ModuleName As String = ""
    Public Property ModuleName() As String
        Get
            Return m_ModuleName
        End Get
        Set(ByVal value As String)
            m_ModuleName = value
        End Set
    End Property

    Private m_ModuleOwner As String = ""
    Public Property ModuleOwner() As String
        Get
            Return m_ModuleOwner
        End Get
        Set(ByVal value As String)
            m_ModuleOwner = value
        End Set
    End Property

    Private m_MenuState As MenuStates = MenuStates.Default
    Public Property MenuState() As MenuStates
        Get
            Return m_MenuState
        End Get
        Set(ByVal value As MenuStates)
            m_MenuState = value
        End Set
    End Property

    ' Click Edit first to enable Delete button
    Private m_EditBeforeDelete As Boolean = False
    Public Property EditBeforeDelete() As Boolean
        Get
            Return m_EditBeforeDelete
        End Get
        Set(ByVal value As Boolean)
            m_EditBeforeDelete = value
        End Set
    End Property

    ' Delete is enabled on both select and edit state
    Private m_DeleteOnSelectEdit As Boolean = False
    Public Property DeleteOnSelectEdit() As Boolean
        Get
            Return m_DeleteOnSelectEdit
        End Get
        Set(ByVal value As Boolean)
            m_DeleteOnSelectEdit = value
        End Set
    End Property

    Private m_UseCloseButton As Boolean = True
    Public Property UseCloseButton() As Boolean
        Get
            Return m_UseCloseButton
        End Get
        Set(ByVal value As Boolean)
            m_UseCloseButton = value
        End Set
    End Property

    Public Property HasPrint() As Boolean
        Get
            Return mnuPrint.Visible
        End Get
        Set(ByVal value As Boolean)
            mnuPrint.Visible = value
        End Set
    End Property

    Private m_IsDraft As Boolean = True
    Public WriteOnly Property IsDraft() As Boolean
        Set(ByVal value As Boolean)
            m_IsDraft = value
        End Set
    End Property

#End Region

#Region "Events"

    Public Event MenuClick(ByVal State As MenuButtons)

#End Region

#Region "Event Handlers"

    Private Sub ucMenuButtons_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mnuNew.Enabled = False
        mnuEdit.Enabled = False
        mnuDelete.Enabled = False
        mnuSave.Enabled = False
        mnuCancel.Enabled = False
        mnuPrint.Enabled = False

        SetToDefault()
    End Sub

    Private Sub mnuNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNew.Click
        RaiseEvent MenuClick(MenuButtons.[New])
        m_ClickedNew = True
        SetMenuState(MenuStates.[New])
    End Sub

    Private Sub mnuEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        RaiseEvent MenuClick(MenuButtons.Edit)
        m_ClickedNew = False
        SetMenuState(MenuStates.Edit)
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        RaiseEvent MenuClick(MenuButtons.Save)
        SetMenuState(MenuStates.Select)
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        RaiseEvent MenuClick(MenuButtons.Delete)
        SetMenuState(MenuStates.[Default])
    End Sub

    Private Sub mnuCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCancel.Click
        If m_UseCloseButton And mnuCancel.Text.Contains("Close") Then
            ParentForm.Close()
            Return
        End If
        RaiseEvent MenuClick(MenuButtons.Cancel)
        If m_ClickedNew Then
            If Not m_ErrorOccured Then
                m_ClickedNew = False
            End If
            SetMenuState(MenuStates.[Default])
        Else
            SetMenuState(MenuStates.Select)
        End If
    End Sub

    Private Sub mnuPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPrint.Click
        RaiseEvent MenuClick(MenuButtons.Print)
    End Sub

#End Region

#Region "Methods"

    Public Sub PerformClick(ByVal _Button As MenuButtons)
        Try
            Select Case _Button
                Case MenuButtons.New : mnuNew.PerformClick()
                Case MenuButtons.Edit : mnuEdit.PerformClick()
                Case MenuButtons.Save : mnuSave.PerformClick()
                Case MenuButtons.Delete : mnuDelete.PerformClick()
                Case MenuButtons.Cancel : mnuCancel.PerformClick()
                Case MenuButtons.Print : mnuPrint.PerformClick()
            End Select
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetMenuState(ByVal State As MenuStates)
        Try
            If m_ErrorOccured Then
                m_ErrorOccured = False
                Exit Sub
            End If

            MenuState = State

            Select Case State
                Case MenuStates.Default, MenuStates.Select
                    mnuNew.Enabled = True
                    mnuEdit.Enabled = IIf(State = MenuStates.Default, False, True)
                    If Not m_IsDraft And mnuEdit.Enabled Then
                        mnuEdit.Enabled = False
                    End If
                    mnuSave.Enabled = False
                    If State = MenuStates.Default Then
                        mnuDelete.Enabled = False
                    ElseIf m_DeleteOnSelectEdit Then
                        mnuDelete.Enabled = True
                    Else
                        mnuDelete.Enabled = IIf(EditBeforeDelete, False, True)
                    End If
                    If Not m_IsDraft And mnuDelete.Enabled Then
                        mnuDelete.Enabled = False
                    End If
                    mnuCancel.Enabled = True
                    If m_UseCloseButton Then
                        mnuCancel.Text = IIf(mnuCancel.Text.StartsWith("&"), "&", "") & "Close"
                    End If
                    mnuPrint.Enabled = IIf(State = MenuStates.Default, False, True)

                Case MenuStates.[New]
                    mnuNew.Enabled = False
                    mnuEdit.Enabled = False
                    mnuSave.Enabled = True
                    mnuDelete.Enabled = False
                    mnuCancel.Enabled = True
                    m_ClickedNew = True
                    If m_UseCloseButton Then
                        mnuCancel.Text = IIf(mnuCancel.Text.StartsWith("&"), "&", "") & "Cancel"
                    End If
                    mnuPrint.Enabled = False

                Case MenuStates.Edit
                    mnuNew.Enabled = False
                    mnuEdit.Enabled = False
                    mnuSave.Enabled = True
                    mnuDelete.Enabled = IIf(EditBeforeDelete, True, False)
                    mnuCancel.Enabled = True
                    m_ClickedNew = False
                    If m_UseCloseButton Then
                        mnuCancel.Text = IIf(mnuCancel.Text.StartsWith("&"), "&", "") & "Cancel"
                    End If
                    mnuPrint.Enabled = False
            End Select

            m_IsDraft = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetToDefault()
        Try
            SetMenuState(MenuStates.[Default])
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class
