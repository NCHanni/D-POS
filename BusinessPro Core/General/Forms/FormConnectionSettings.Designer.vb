<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormConnectionSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.grpDataSource = New Infragistics.Win.Misc.UltraGroupBox()
        Me.lblTestStatus = New Infragistics.Win.Misc.UltraLabel()
        Me.txtPassword = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblPassword = New Infragistics.Win.Misc.UltraLabel()
        Me.txtUserName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblUsername = New Infragistics.Win.Misc.UltraLabel()
        Me.txtDatabaseName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblDatabaseName = New Infragistics.Win.Misc.UltraLabel()
        Me.txtDataSource = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblDataSource = New Infragistics.Win.Misc.UltraLabel()
        Me.btnCancel = New Infragistics.Win.Misc.UltraButton()
        Me.btnTestSave = New Infragistics.Win.Misc.UltraButton()
        CType(Me.grpDataSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDataSource.SuspendLayout()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUserName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDatabaseName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDataSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpDataSource
        '
        Me.grpDataSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDataSource.Controls.Add(Me.lblTestStatus)
        Me.grpDataSource.Controls.Add(Me.txtPassword)
        Me.grpDataSource.Controls.Add(Me.lblPassword)
        Me.grpDataSource.Controls.Add(Me.txtUserName)
        Me.grpDataSource.Controls.Add(Me.lblUsername)
        Me.grpDataSource.Controls.Add(Me.txtDatabaseName)
        Me.grpDataSource.Controls.Add(Me.lblDatabaseName)
        Me.grpDataSource.Controls.Add(Me.txtDataSource)
        Me.grpDataSource.Controls.Add(Me.lblDataSource)
        Me.grpDataSource.Location = New System.Drawing.Point(12, 12)
        Me.grpDataSource.Name = "grpDataSource"
        Me.grpDataSource.Size = New System.Drawing.Size(305, 148)
        Me.grpDataSource.TabIndex = 0
        Me.grpDataSource.Text = "Data Source"
        '
        'lblTestStatus
        '
        Appearance1.ForeColor = System.Drawing.Color.Blue
        Me.lblTestStatus.Appearance = Appearance1
        Me.lblTestStatus.AutoSize = True
        Me.lblTestStatus.Location = New System.Drawing.Point(227, 2)
        Me.lblTestStatus.Name = "lblTestStatus"
        Me.lblTestStatus.Size = New System.Drawing.Size(62, 14)
        Me.lblTestStatus.TabIndex = 0
        Me.lblTestStatus.Text = "Connected!"
        Me.lblTestStatus.UseAppStyling = False
        Me.lblTestStatus.Visible = False
        '
        'txtPassword
        '
        Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.Location = New System.Drawing.Point(91, 117)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.txtPassword.Size = New System.Drawing.Size(190, 21)
        Me.txtPassword.TabIndex = 8
        Me.txtPassword.Tag = "Password"
        '
        'lblPassword
        '
        Appearance2.TextVAlignAsString = "Middle"
        Me.lblPassword.Appearance = Appearance2
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(18, 121)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(54, 14)
        Me.lblPassword.TabIndex = 7
        Me.lblPassword.Text = "Password"
        '
        'txtUserName
        '
        Me.txtUserName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUserName.Location = New System.Drawing.Point(91, 90)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(190, 21)
        Me.txtUserName.TabIndex = 6
        Me.txtUserName.Tag = "Username"
        '
        'lblUsername
        '
        Appearance3.TextVAlignAsString = "Middle"
        Me.lblUsername.Appearance = Appearance3
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(18, 94)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(56, 14)
        Me.lblUsername.TabIndex = 5
        Me.lblUsername.Text = "Username"
        '
        'txtDatabaseName
        '
        Me.txtDatabaseName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDatabaseName.Location = New System.Drawing.Point(91, 63)
        Me.txtDatabaseName.Name = "txtDatabaseName"
        Me.txtDatabaseName.Size = New System.Drawing.Size(190, 21)
        Me.txtDatabaseName.TabIndex = 4
        Me.txtDatabaseName.Tag = "Data Source"
        '
        'lblDatabaseName
        '
        Appearance4.TextVAlignAsString = "Middle"
        Me.lblDatabaseName.Appearance = Appearance4
        Me.lblDatabaseName.AutoSize = True
        Me.lblDatabaseName.Location = New System.Drawing.Point(18, 67)
        Me.lblDatabaseName.Name = "lblDatabaseName"
        Me.lblDatabaseName.Size = New System.Drawing.Size(53, 14)
        Me.lblDatabaseName.TabIndex = 3
        Me.lblDatabaseName.Text = "Database"
        '
        'txtDataSource
        '
        Me.txtDataSource.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataSource.Location = New System.Drawing.Point(91, 36)
        Me.txtDataSource.Name = "txtDataSource"
        Me.txtDataSource.Size = New System.Drawing.Size(190, 21)
        Me.txtDataSource.TabIndex = 2
        Me.txtDataSource.Tag = "Data Source"
        '
        'lblDataSource
        '
        Appearance5.TextVAlignAsString = "Middle"
        Me.lblDataSource.Appearance = Appearance5
        Me.lblDataSource.AutoSize = True
        Me.lblDataSource.Location = New System.Drawing.Point(18, 40)
        Me.lblDataSource.Name = "lblDataSource"
        Me.lblDataSource.Size = New System.Drawing.Size(67, 14)
        Me.lblDataSource.TabIndex = 1
        Me.lblDataSource.Text = "Data Source"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(242, 166)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'btnTestSave
        '
        Me.btnTestSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTestSave.ImageSize = New System.Drawing.Size(15, 15)
        Me.btnTestSave.Location = New System.Drawing.Point(161, 166)
        Me.btnTestSave.Name = "btnTestSave"
        Me.btnTestSave.Size = New System.Drawing.Size(75, 23)
        Me.btnTestSave.TabIndex = 1
        Me.btnTestSave.Text = "Test"
        '
        'FormConnectionSettings
        '
        Me.AcceptButton = Me.btnTestSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(329, 201)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnTestSave)
        Me.Controls.Add(Me.grpDataSource)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FormConnectionSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Connection Setup"
        CType(Me.grpDataSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDataSource.ResumeLayout(False)
        Me.grpDataSource.PerformLayout()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDatabaseName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDataSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDataSource As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents txtPassword As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblPassword As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtUserName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblUsername As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtDataSource As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblDataSource As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents btnCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnTestSave As Infragistics.Win.Misc.UltraButton
    Friend WithEvents lblTestStatus As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtDatabaseName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblDatabaseName As Infragistics.Win.Misc.UltraLabel
End Class
