<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAuthorization
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.grpCredentials = New Infragistics.Win.Misc.UltraGroupBox()
        Me.txtReason = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.lblReason = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.txtPassword = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtUsername = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.lblMessage = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraGroupBox2 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.btnOk = New Infragistics.Win.Misc.UltraButton()
        Me.btnCancel = New Infragistics.Win.Misc.UltraButton()
        CType(Me.grpCredentials, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCredentials.SuspendLayout()
        CType(Me.txtReason, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUsername, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCredentials
        '
        Me.grpCredentials.Controls.Add(Me.txtReason)
        Me.grpCredentials.Controls.Add(Me.lblReason)
        Me.grpCredentials.Controls.Add(Me.UltraLabel3)
        Me.grpCredentials.Controls.Add(Me.UltraLabel2)
        Me.grpCredentials.Controls.Add(Me.txtPassword)
        Me.grpCredentials.Controls.Add(Me.txtUsername)
        Me.grpCredentials.Controls.Add(Me.UltraLabel1)
        Me.grpCredentials.Controls.Add(Me.lblMessage)
        Me.grpCredentials.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCredentials.Location = New System.Drawing.Point(0, 0)
        Me.grpCredentials.Name = "grpCredentials"
        Me.grpCredentials.Size = New System.Drawing.Size(258, 168)
        Me.grpCredentials.TabIndex = 0
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(74, 118)
        Me.txtReason.MaxLength = 200
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(171, 30)
        Me.txtReason.TabIndex = 8
        Me.txtReason.Tag = "Reason"
        Me.txtReason.Visible = False
        '
        'lblReason
        '
        Appearance1.BackColor = System.Drawing.Color.Transparent
        Me.lblReason.Appearance = Appearance1
        Me.lblReason.AutoSize = True
        Me.lblReason.Location = New System.Drawing.Point(12, 121)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(43, 14)
        Me.lblReason.TabIndex = 7
        Me.lblReason.Text = "Reason"
        Me.lblReason.Visible = False
        '
        'UltraLabel3
        '
        Appearance2.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel3.Appearance = Appearance2
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(12, 95)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(54, 14)
        Me.UltraLabel3.TabIndex = 5
        Me.UltraLabel3.Text = "Password"
        '
        'UltraLabel2
        '
        Appearance3.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel2.Appearance = Appearance3
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Location = New System.Drawing.Point(12, 68)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(56, 14)
        Me.UltraLabel2.TabIndex = 3
        Me.UltraLabel2.Text = "Username"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(74, 91)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtPassword.Size = New System.Drawing.Size(171, 21)
        Me.txtPassword.TabIndex = 6
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(74, 64)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(171, 21)
        Me.txtUsername.TabIndex = 4
        '
        'UltraLabel1
        '
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 36)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(217, 14)
        Me.UltraLabel1.TabIndex = 1
        Me.UltraLabel1.Text = "Please enter authorized credentials below."
        '
        'lblMessage
        '
        Appearance4.FontData.BoldAsString = "True"
        Me.lblMessage.Appearance = Appearance4
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(12, 15)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(110, 14)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "Message goes here!"
        '
        'UltraGroupBox2
        '
        Me.UltraGroupBox2.Controls.Add(Me.btnOk)
        Me.UltraGroupBox2.Controls.Add(Me.btnCancel)
        Me.UltraGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UltraGroupBox2.Location = New System.Drawing.Point(0, 168)
        Me.UltraGroupBox2.Name = "UltraGroupBox2"
        Me.UltraGroupBox2.Size = New System.Drawing.Size(258, 40)
        Me.UltraGroupBox2.TabIndex = 1
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(89, 2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(170, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'FormAuthorization
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(258, 208)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCredentials)
        Me.Controls.Add(Me.UltraGroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FormAuthorization"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.grpCredentials, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCredentials.ResumeLayout(False)
        Me.grpCredentials.PerformLayout()
        CType(Me.txtReason, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUsername, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCredentials As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents txtPassword As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtUsername As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblMessage As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraGroupBox2 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnOk As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtReason As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents lblReason As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
End Class
