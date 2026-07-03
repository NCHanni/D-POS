Imports Infragistics.Win.UltraWinGrid

Friend Class FormTerminals

#Region "Declarations"

    Private ReadOnly m_Settings As SettingsPreferences
    Private m_Terminal As Terminals
    Private m_AuthorizedOfficerCode As String
    Private m_Data As DataTable
    Private m_MacAddress As String
    Private m_ComputerName As String

#End Region

#Region "Constructor"

    Public Sub New(settings As SettingsPreferences)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Settings = settings
    End Sub

#End Region

#Region "Properties"

    Private m_TerminalId As Integer = 0
    Public ReadOnly Property Id As Integer
        Get
            Return m_TerminalId
        End Get
    End Property

    Private m_TerminalCode As String = ""
    Public ReadOnly Property Code As String
        Get
            Return m_TerminalCode
        End Get
    End Property

#End Region

#Region "Event Handlers"

#Region "Form"

    Private Sub Form_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            grdList.UpdateData()
            If m_Data.HasChanges Then
                If ShowCancelQuestion() = QuestionResult.No Then
                    e.Cancel = True
                End If
            End If

            If Not e.Cancel Then
                If DialogResult <> DialogResult.OK Then
                    If lblWarning.Visible Then
                        DialogResult = DialogResult.Cancel
                    Else
                        DialogResult = DialogResult.Retry
                    End If
                End If
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Initialize()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub Forms_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            LoadData()
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

#End Region

#Region "Buttons"

    Private Sub btnManageTerminals_Click(sender As Object, e As EventArgs) Handles btnManageTerminals.Click
        Try
            Using frmAuth As New FormAuthorization(m_Settings, False, True) With {.Message = "MANAGE TERMINALS"}
                If LaunchForm(frmAuth,, Me) = DialogResult.OK Then
                    Using frmTerm As New POS.Admin.FormTerminals
                        frmTerm.MinimizeBox = False
                        LaunchForm(frmTerm, True, Me)
                        LoadData()
                    End Using
                End If
            End Using
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If grdList.ActiveRow IsNot Nothing Then
                m_TerminalId = grdList.ActiveRow.Cells("id").Value
                m_TerminalCode = grdList.ActiveRow.Cells("code").Value

                DialogResult = DialogResult.OK
                Close()
            End If
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Close()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#Region "Grid"

    Private Sub grdList_AfterSelectChange(sender As Object, e As AfterSelectChangeEventArgs) Handles grdList.AfterSelectChange
        Try
            If grdList.ActiveRow Is Nothing Then
                btnOk.Enabled = False
            Else
                With grdList.ActiveRow
                    If String.Compare(.Cells("computer_name").Value, m_ComputerName, False) = 0 AndAlso String.Compare(.Cells("mac_address").Value, m_MacAddress, False) = 0 Then
                        btnOk.Enabled = True
                    Else
                        btnOk.Enabled = Not CBool(.Cells("is_active").Value)
                    End If
                End With
            End If
        Catch ex As Exception
            ex.ShowError
        End Try
    End Sub

    Private Sub grdList_DoubleClickRow(ByVal sender As Object, ByVal e As DoubleClickRowEventArgs) Handles grdList.DoubleClickRow
        Try
            btnOk.PerformClick()
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

    Private Sub grdList_InitializeLayout(ByVal sender As System.Object, ByVal e As InitializeLayoutEventArgs) Handles grdList.InitializeLayout
        Try
            With e.Layout
                With .Bands(0)
                    .Columns("id").Hidden = True
                    .Columns("mac_address").Hidden = True
                    .Columns("computer_name").Hidden = True

                    With .Columns("code")
                        .Header.Caption = "Name"
                        .CellClickAction = CellClickAction.RowSelect
                        .CellActivation = Activation.NoEdit
                    End With

                    With .Columns("is_active")
                        .CellActivation = Activation.NoEdit
                        .CellClickAction = CellClickAction.RowSelect
                        .Header.Caption = "Active"
                        .SetWidth(60)
                        .Style = ColumnStyle.CheckBox
                    End With
                End With
            End With
        Catch ex As Exception
            ex.ShowError()
        End Try
    End Sub

#End Region

#End Region

#Region "Methods"

    Private Sub Initialize()
        Try
            m_Terminal = New Terminals

            With New SettingsPreferences
                .Fill()
                m_AuthorizedOfficerCode = .AuthorizedOfficerCode
            End With

            m_MacAddress = GetMacAddress()
            m_ComputerName = GetComputerName()

            InitializeGrid()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeGrid()
        Try
            Dim dt As New DataTable
            With dt.Columns
                .Add("id", GetType(Integer))
                .Add("code")
                .Add("mac_address")
                .Add("computer_name")
                .Add("is_active", GetType(Boolean))
            End With
            grdList.DataSource = dt
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadData()
        Try
            With m_Terminal
                .Fill()
                m_Data = .Data

                If m_Data.Select("is_active = 'False'").Count = 0 Then
                    lblWarning.Visible = True
                Else
                    lblWarning.Visible = False
                End If

                btnOk.Enabled = False
            End With

            With grdList
                .DataSource = m_Data

                If m_Data.Rows.Count > 0 Then
                    .Rows.First.Activate()
                End If

                .CheckEdit()
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetMacAddress() As String
        Try
            Dim netInterface() As System.Net.NetworkInformation.NetworkInterface =
                System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()

            Return netInterface(0).GetPhysicalAddress.ToString.Replace("-", "").Replace(":", "")
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function GetComputerName() As String
        Try
            Return My.Computer.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

#End Region

End Class