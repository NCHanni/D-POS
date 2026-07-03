<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormLogin
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
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormLogin))
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.picLoader = New System.Windows.Forms.PictureBox()
        Me.txtUsername = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtPassword = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.btnLogin = New Infragistics.Win.Misc.UltraButton()
        Me.btnCancel = New Infragistics.Win.Misc.UltraButton()
        Me.lblMessage = New Infragistics.Win.Misc.UltraLabel()
        Me.lblApplicationCaption = New Infragistics.Win.Misc.UltraLabel()
        Me.chkRememberMe = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.UltraPictureBox1 = New Infragistics.Win.UltraWinEditors.UltraPictureBox()
        Me.lblStatus = New Infragistics.Win.Misc.UltraLabel()
        Me.picStatus = New System.Windows.Forms.PictureBox()
        Me.lblTerminalDetails = New Infragistics.Win.Misc.UltraLabel()
        Me.picBarcode = New Infragistics.Win.Misc.UltraPanel()
        Me.pnlBarcodeLogin = New Infragistics.Win.Misc.UltraPanel()
        Me.lblScanBarcode = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.picLoader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUsername, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkRememberMe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picBarcode.SuspendLayout()
        Me.pnlBarcodeLogin.ClientArea.SuspendLayout()
        Me.pnlBarcodeLogin.SuspendLayout()
        Me.SuspendLayout()
        '
        'picLoader
        '
        Me.picLoader.BackColor = System.Drawing.Color.Transparent
        Me.picLoader.Location = New System.Drawing.Point(203, 301)
        Me.picLoader.Name = "picLoader"
        Me.picLoader.Size = New System.Drawing.Size(30, 30)
        Me.picLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picLoader.TabIndex = 5
        Me.picLoader.TabStop = False
        Me.picLoader.Visible = False
        '
        'txtUsername
        '
        Appearance1.BackColor = System.Drawing.Color.White
        Appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent
        Appearance1.TextVAlignAsString = "Middle"
        Me.txtUsername.Appearance = Appearance1
        Me.txtUsername.AutoSize = False
        Me.txtUsername.BackColor = System.Drawing.Color.White
        Me.txtUsername.Font = New System.Drawing.Font("Segoe UI Light", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.Location = New System.Drawing.Point(54, 147)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.NullText = "Username"
        Appearance2.BackColor = System.Drawing.Color.White
        Appearance2.FontData.Name = "Segoe UI"
        Appearance2.FontData.SizeInPoints = 10.0!
        Appearance2.ForeColor = System.Drawing.Color.Gray
        Appearance2.TextVAlignAsString = "Middle"
        Me.txtUsername.NullTextAppearance = Appearance2
        Me.txtUsername.Size = New System.Drawing.Size(330, 32)
        Me.txtUsername.TabIndex = 2
        Me.txtUsername.UseAppStyling = False
        Me.txtUsername.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'txtPassword
        '
        Appearance3.BackColor = System.Drawing.Color.White
        Appearance3.BorderAlpha = Infragistics.Win.Alpha.Transparent
        Appearance3.TextVAlignAsString = "Middle"
        Me.txtPassword.Appearance = Appearance3
        Me.txtPassword.AutoSize = False
        Me.txtPassword.BackColor = System.Drawing.Color.White
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI Light", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(54, 185)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.NullText = "Password"
        Appearance4.BackColor = System.Drawing.Color.White
        Appearance4.FontData.Name = "Segoe UI"
        Appearance4.FontData.SizeInPoints = 10.0!
        Appearance4.ForeColor = System.Drawing.Color.Gray
        Appearance4.TextVAlignAsString = "Middle"
        Me.txtPassword.NullTextAppearance = Appearance4
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtPassword.Size = New System.Drawing.Size(330, 32)
        Me.txtPassword.TabIndex = 4
        Me.txtPassword.UseAppStyling = False
        Me.txtPassword.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'btnLogin
        '
        Appearance5.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance5.BorderColor = System.Drawing.Color.White
        Appearance5.ForeColor = System.Drawing.Color.White
        Appearance5.TextHAlignAsString = "Center"
        Me.btnLogin.Appearance = Appearance5
        Me.btnLogin.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat
        Me.btnLogin.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Appearance6.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance6.BorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnLogin.HotTrackAppearance = Appearance6
        Me.btnLogin.Location = New System.Drawing.Point(54, 259)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.ShowFocusRect = False
        Me.btnLogin.ShowOutline = False
        Me.btnLogin.Size = New System.Drawing.Size(160, 32)
        Me.btnLogin.TabIndex = 6
        Me.btnLogin.Text = "LOGIN"
        Me.btnLogin.UseAppStyling = False
        Me.btnLogin.UseHotTracking = Infragistics.Win.DefaultableBoolean.[True]
        Me.btnLogin.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'btnCancel
        '
        Appearance7.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance7.BorderColor = System.Drawing.Color.White
        Appearance7.ForeColor = System.Drawing.Color.White
        Appearance7.TextHAlignAsString = "Center"
        Me.btnCancel.Appearance = Appearance7
        Me.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Appearance8.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance8.BorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnCancel.HotTrackAppearance = Appearance8
        Me.btnCancel.Location = New System.Drawing.Point(224, 259)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.ShowFocusRect = False
        Me.btnCancel.ShowOutline = False
        Me.btnCancel.Size = New System.Drawing.Size(160, 32)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "CANCEL"
        Me.btnCancel.UseAppStyling = False
        Me.btnCancel.UseHotTracking = Infragistics.Win.DefaultableBoolean.[True]
        Me.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'lblMessage
        '
        Appearance9.BackColor = System.Drawing.Color.Transparent
        Appearance9.ForeColor = System.Drawing.Color.White
        Appearance9.TextHAlignAsString = "Right"
        Me.lblMessage.Appearance = Appearance9
        Me.lblMessage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(54, 306)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(330, 20)
        Me.lblMessage.TabIndex = 8
        Me.lblMessage.Text = "Password"
        Me.lblMessage.UseAppStyling = False
        Me.lblMessage.Visible = False
        '
        'lblApplicationCaption
        '
        Appearance10.BackColor = System.Drawing.Color.Transparent
        Appearance10.ForeColor = System.Drawing.Color.White
        Appearance10.TextHAlignAsString = "Right"
        Appearance10.TextVAlignAsString = "Middle"
        Me.lblApplicationCaption.Appearance = Appearance10
        Me.lblApplicationCaption.Font = New System.Drawing.Font("Segoe UI Light", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApplicationCaption.Location = New System.Drawing.Point(124, 98)
        Me.lblApplicationCaption.Name = "lblApplicationCaption"
        Me.lblApplicationCaption.Size = New System.Drawing.Size(254, 20)
        Me.lblApplicationCaption.TabIndex = 0
        Me.lblApplicationCaption.Text = "Application Name"
        Me.lblApplicationCaption.UseAppStyling = False
        '
        'chkRememberMe
        '
        Appearance11.FontData.Name = "Segoe UI"
        Appearance11.FontData.SizeInPoints = 10.0!
        Appearance11.ForeColor = System.Drawing.Color.White
        Me.chkRememberMe.Appearance = Appearance11
        Me.chkRememberMe.AutoSize = True
        Me.chkRememberMe.BackColor = System.Drawing.Color.Transparent
        Me.chkRememberMe.BackColorInternal = System.Drawing.Color.Transparent
        Appearance12.BorderAlpha = Infragistics.Win.Alpha.Transparent
        Me.chkRememberMe.CheckedAppearance = Appearance12
        Me.chkRememberMe.Location = New System.Drawing.Point(54, 223)
        Me.chkRememberMe.Name = "chkRememberMe"
        Me.chkRememberMe.Size = New System.Drawing.Size(153, 23)
        Me.chkRememberMe.TabIndex = 5
        Me.chkRememberMe.Text = "Remember Username"
        Me.chkRememberMe.UseAppStyling = False
        '
        'UltraPictureBox1
        '
        Appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent
        Me.UltraPictureBox1.Appearance = Appearance13
        Me.UltraPictureBox1.BorderShadowColor = System.Drawing.Color.Empty
        Me.UltraPictureBox1.Image = CType(resources.GetObject("UltraPictureBox1.Image"), Object)
        Me.UltraPictureBox1.Location = New System.Drawing.Point(54, 38)
        Me.UltraPictureBox1.Name = "UltraPictureBox1"
        Me.UltraPictureBox1.Size = New System.Drawing.Size(330, 73)
        Me.UltraPictureBox1.TabIndex = 9
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Appearance14.BackColor = System.Drawing.Color.Transparent
        Appearance14.ForeColor = System.Drawing.Color.White
        Me.lblStatus.Appearance = Appearance14
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblStatus.Location = New System.Drawing.Point(34, 324)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(36, 16)
        Me.lblStatus.TabIndex = 12
        Me.lblStatus.Text = "Online"
        Me.lblStatus.UseAppStyling = False
        '
        'picStatus
        '
        Me.picStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.picStatus.BackColor = System.Drawing.Color.Transparent
        Me.picStatus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picStatus.Image = CType(resources.GetObject("picStatus.Image"), System.Drawing.Image)
        Me.picStatus.Location = New System.Drawing.Point(13, 322)
        Me.picStatus.Name = "picStatus"
        Me.picStatus.Size = New System.Drawing.Size(16, 16)
        Me.picStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picStatus.TabIndex = 11
        Me.picStatus.TabStop = False
        '
        'lblTerminalDetails
        '
        Appearance15.BackColor = System.Drawing.Color.Transparent
        Appearance15.ForeColor = System.Drawing.Color.White
        Appearance15.TextHAlignAsString = "Right"
        Appearance15.TextVAlignAsString = "Middle"
        Me.lblTerminalDetails.Appearance = Appearance15
        Me.lblTerminalDetails.Font = New System.Drawing.Font("Segoe UI Light", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTerminalDetails.Location = New System.Drawing.Point(124, 117)
        Me.lblTerminalDetails.Name = "lblTerminalDetails"
        Me.lblTerminalDetails.Size = New System.Drawing.Size(254, 20)
        Me.lblTerminalDetails.TabIndex = 13
        Me.lblTerminalDetails.Text = "Terminal Code"
        Me.lblTerminalDetails.UseAppStyling = False
        Me.lblTerminalDetails.Visible = False
        '
        'picBarcode
        '
        Appearance16.ImageBackground = Global.BusinessPro.Core.My.Resources.Resources.scan_barcode
        Me.picBarcode.Appearance = Appearance16
        Me.picBarcode.Location = New System.Drawing.Point(125, 5)
        Me.picBarcode.Name = "picBarcode"
        Me.picBarcode.Size = New System.Drawing.Size(100, 100)
        Me.picBarcode.TabIndex = 14
        '
        'pnlBarcodeLogin
        '
        '
        'pnlBarcodeLogin.ClientArea
        '
        Me.pnlBarcodeLogin.ClientArea.Controls.Add(Me.lblScanBarcode)
        Me.pnlBarcodeLogin.ClientArea.Controls.Add(Me.picBarcode)
        Me.pnlBarcodeLogin.Location = New System.Drawing.Point(44, 136)
        Me.pnlBarcodeLogin.Name = "pnlBarcodeLogin"
        Me.pnlBarcodeLogin.Size = New System.Drawing.Size(350, 120)
        Me.pnlBarcodeLogin.TabIndex = 15
        Me.pnlBarcodeLogin.Visible = False
        '
        'lblScanBarcode
        '
        Appearance17.BackColor = System.Drawing.Color.Transparent
        Appearance17.FontData.BoldAsString = "True"
        Appearance17.FontData.SizeInPoints = 16.0!
        Appearance17.ForeColor = System.Drawing.Color.White
        Appearance17.TextHAlignAsString = "Center"
        Me.lblScanBarcode.Appearance = Appearance17
        Me.lblScanBarcode.Font = New System.Drawing.Font("Segoe UI Light", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScanBarcode.Location = New System.Drawing.Point(19, 40)
        Me.lblScanBarcode.Name = "lblScanBarcode"
        Me.lblScanBarcode.Size = New System.Drawing.Size(315, 32)
        Me.lblScanBarcode.TabIndex = 15
        Me.lblScanBarcode.Text = "SCAN BARCODE"
        Me.lblScanBarcode.UseAppStyling = False
        '
        'FormLogin
        '
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(19, Byte), Integer), CType(CType(86, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(438, 350)
        Me.Controls.Add(Me.pnlBarcodeLogin)
        Me.Controls.Add(Me.picLoader)
        Me.Controls.Add(Me.lblApplicationCaption)
        Me.Controls.Add(Me.lblTerminalDetails)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.picStatus)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.chkRememberMe)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.UltraPictureBox1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FormLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        CType(Me.picLoader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUsername, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPassword, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkRememberMe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picBarcode.ResumeLayout(False)
        Me.pnlBarcodeLogin.ClientArea.ResumeLayout(False)
        Me.pnlBarcodeLogin.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picLoader As System.Windows.Forms.PictureBox
    Friend WithEvents txtUsername As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtPassword As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents btnLogin As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents lblMessage As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblApplicationCaption As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents chkRememberMe As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents UltraPictureBox1 As Infragistics.Win.UltraWinEditors.UltraPictureBox
    Friend WithEvents lblStatus As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents picStatus As PictureBox
    Friend WithEvents lblTerminalDetails As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents picBarcode As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents pnlBarcodeLogin As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents lblScanBarcode As Infragistics.Win.Misc.UltraLabel
End Class
