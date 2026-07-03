<System.ComponentModel.DefaultEvent("MenuClick")> _
Public Class MenuButtons

#Region "Declarations"

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
        [Select] = 7 ' New + Edit
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

    Public Property AllowNew As Boolean = True
    Public Property AllowEdit As Boolean = True
    Public Property AllowDelete As Boolean = True

    Public ReadOnly Property ButtonCancel As Infragistics.Win.Misc.UltraButton
        Get
            Return mnuCancel
        End Get
    End Property

    Private m_ButtonHeight As Integer = 24
    Public Property ButtonHeight() As Integer
        Get
            Return m_ButtonHeight
        End Get
        Set(ByVal value As Integer)
            m_ButtonHeight = value
            mnuNew.Height = value
            mnuEdit.Height = value
            mnuDelete.Height = value
            mnuPrint.Height = value
            mnuSave.Height = value
            mnuCancel.Height = value
        End Set
    End Property

    Private m_ErrorOccured As Boolean
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
            mnuNew.Tag = m_ModuleName
            mnuEdit.Tag = m_ModuleName
            mnuDelete.Tag = m_ModuleName
            mnuPrint.Tag = m_ModuleName
        End Set
    End Property

    Property ModuleGroup As String = ""
    Property MenuState As MenuStates = MenuStates.Default
    Property UseCloseButton As Boolean = True

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
        If DesignMode Then
            mnuNew.Enabled = True
            mnuEdit.Enabled = True
            mnuDelete.Enabled = True
            mnuSave.Enabled = True
            mnuCancel.Enabled = True
            mnuPrint.Enabled = True
        Else
            mnuNew.Enabled = False
            mnuEdit.Enabled = False
            mnuDelete.Enabled = False
            mnuSave.Enabled = False
            mnuCancel.Enabled = False
            mnuPrint.Enabled = False
            SetToDefault()
        End If
    End Sub

    Private Sub mnuNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNew.Click
        Try
            RaiseEvent MenuClick(MenuButtons.[New])
            m_ClickedNew = True
            SetMenuState(MenuStates.[New])
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub mnuEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        Try
            RaiseEvent MenuClick(MenuButtons.Edit)
            m_ClickedNew = False
            SetMenuState(MenuStates.Edit)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        Try
            RaiseEvent MenuClick(MenuButtons.Save)
            SetMenuState(MenuStates.Select)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        Try
            RaiseEvent MenuClick(MenuButtons.Delete)
            SetMenuState(MenuStates.[Default])
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub mnuCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCancel.Click
        Try
            If UseCloseButton AndAlso mnuCancel.Text.Contains("Close") Then
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
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub mnuPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPrint.Click
        RaiseEvent MenuClick(MenuButtons.Print)
    End Sub

#End Region

#Region "Methods"

    Public Sub PerformClick(ByVal button As MenuButtons)
        Try
            Select Case button
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

    Public Sub SetMenuState(ByVal state As MenuStates)
        Try
            If m_ErrorOccured Then
                m_ErrorOccured = False
                Exit Sub
            End If

            MenuState = state

            Select Case state
                Case MenuStates.Default, MenuStates.Select
                    If AllowNew Then
                        mnuNew.IsAllowed(Rights.AccessRights.Add, Me.ModuleGroup)
                    Else
                        mnuNew.Enabled = False
                    End If
                    If state = MenuStates.Default OrElse Not AllowEdit OrElse Not m_IsDraft Then
                        mnuEdit.Enabled = False
                    Else
                        mnuEdit.IsAllowed(Rights.AccessRights.Edit, Me.ModuleGroup)
                    End If
                    mnuSave.Enabled = False
                    If state = MenuStates.Default OrElse Not AllowDelete OrElse Not m_IsDraft Then
                        mnuDelete.Enabled = False
                    Else
                        mnuDelete.IsAllowed(Rights.AccessRights.Delete, Me.ModuleGroup)
                    End If
                    mnuCancel.Enabled = True
                    If UseCloseButton Then
                        mnuCancel.Text = If(mnuCancel.Text.StartsWith("&"), "&", "") & "Close"
                    End If
                    If state = MenuStates.Default Then
                        mnuPrint.Enabled = False
                    Else
                        mnuPrint.IsAllowed(Rights.AccessRights.Print, Me.ModuleGroup)
                    End If

                Case MenuStates.[New]
                    mnuNew.Enabled = False
                    mnuEdit.Enabled = False
                    mnuSave.Enabled = True
                    mnuDelete.Enabled = False
                    mnuCancel.Enabled = True
                    m_ClickedNew = True
                    If UseCloseButton Then
                        mnuCancel.Text = If(mnuCancel.Text.StartsWith("&"), "&", "") & "Cancel"
                    End If
                    mnuPrint.Enabled = False

                Case MenuStates.Edit
                    mnuNew.Enabled = False
                    mnuEdit.Enabled = False
                    mnuSave.Enabled = True
                    mnuDelete.Enabled = False
                    mnuCancel.Enabled = True
                    m_ClickedNew = False
                    If UseCloseButton Then
                        mnuCancel.Text = If(mnuCancel.Text.StartsWith("&"), "&", "") & "Cancel"
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
