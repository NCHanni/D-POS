<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSaveDraftPrompt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSaveDraftPrompt))
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.picIcon = New Infragistics.Win.UltraWinEditors.UltraPictureBox
        Me.lblText = New Infragistics.Win.Misc.UltraLabel
        Me.btnSaveAsDraft = New Infragistics.Win.Misc.UltraButton
        Me.btnPostTransaction = New Infragistics.Win.Misc.UltraButton
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox
        Me.UltraGroupBox2 = New Infragistics.Win.Misc.UltraGroupBox
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        CType(Me.UltraGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picIcon
        '
        Me.picIcon.BorderShadowColor = System.Drawing.Color.Empty
        Me.picIcon.Image = CType(resources.GetObject("picIcon.Image"), Object)
        Me.picIcon.Location = New System.Drawing.Point(6, 6)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(64, 64)
        Me.picIcon.TabIndex = 1
        Me.picIcon.UseAppStyling = False
        '
        'lblText
        '
        Me.lblText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblText.Location = New System.Drawing.Point(76, 20)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(183, 49)
        Me.lblText.TabIndex = 2
        Me.lblText.Text = "Message here"
        '
        'btnSaveAsDraft
        '
        Me.btnSaveAsDraft.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAsDraft.Location = New System.Drawing.Point(43, 9)
        Me.btnSaveAsDraft.Name = "btnSaveAsDraft"
        Me.btnSaveAsDraft.Size = New System.Drawing.Size(100, 23)
        Me.btnSaveAsDraft.TabIndex = 0
        Me.btnSaveAsDraft.Text = "Save as Draft"
        '
        'btnPostTransaction
        '
        Me.btnPostTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPostTransaction.Location = New System.Drawing.Point(149, 9)
        Me.btnPostTransaction.Name = "btnPostTransaction"
        Me.btnPostTransaction.Size = New System.Drawing.Size(110, 23)
        Me.btnPostTransaction.TabIndex = 1
        Me.btnPostTransaction.Text = "Post Transaction"
        '
        'UltraGroupBox1
        '
        Appearance2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.UltraGroupBox1.Appearance = Appearance2
        Me.UltraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None
        Me.UltraGroupBox1.Controls.Add(Me.btnPostTransaction)
        Me.UltraGroupBox1.Controls.Add(Me.btnSaveAsDraft)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 83)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(271, 44)
        Me.UltraGroupBox1.TabIndex = 0
        Me.UltraGroupBox1.UseAppStyling = False
        '
        'UltraGroupBox2
        '
        Appearance1.BackColor = System.Drawing.Color.Gainsboro
        Me.UltraGroupBox2.Appearance = Appearance1
        Me.UltraGroupBox2.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None
        Me.UltraGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UltraGroupBox2.Location = New System.Drawing.Point(0, 82)
        Me.UltraGroupBox2.Name = "UltraGroupBox2"
        Me.UltraGroupBox2.Size = New System.Drawing.Size(271, 1)
        Me.UltraGroupBox2.TabIndex = 3
        Me.UltraGroupBox2.UseAppStyling = False
        '
        'FormSaveDraftPrompt
        '
        Me.AcceptButton = Me.btnSaveAsDraft
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(271, 127)
        Me.ControlBox = False
        Me.Controls.Add(Me.UltraGroupBox2)
        Me.Controls.Add(Me.lblText)
        Me.Controls.Add(Me.picIcon)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormSaveDraftPrompt"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Save Transaction"
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        CType(Me.UltraGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picIcon As Infragistics.Win.UltraWinEditors.UltraPictureBox
    Friend WithEvents lblText As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents btnSaveAsDraft As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnPostTransaction As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents UltraGroupBox2 As Infragistics.Win.Misc.UltraGroupBox
End Class
