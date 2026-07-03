<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MenuButtons
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mnuNew = New Infragistics.Win.Misc.UltraButton()
        Me.mnuEdit = New Infragistics.Win.Misc.UltraButton()
        Me.mnuSave = New Infragistics.Win.Misc.UltraButton()
        Me.mnuDelete = New Infragistics.Win.Misc.UltraButton()
        Me.mnuCancel = New Infragistics.Win.Misc.UltraButton()
        Me.mnuPrint = New Infragistics.Win.Misc.UltraButton()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuNew
        '
        Me.mnuNew.Location = New System.Drawing.Point(6, 8)
        Me.mnuNew.Name = "mnuNew"
        Me.mnuNew.Size = New System.Drawing.Size(75, 24)
        Me.mnuNew.TabIndex = 0
        Me.mnuNew.Text = "&New"
        '
        'mnuEdit
        '
        Me.mnuEdit.Location = New System.Drawing.Point(87, 8)
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.Size = New System.Drawing.Size(75, 24)
        Me.mnuEdit.TabIndex = 1
        Me.mnuEdit.Text = "&Edit"
        '
        'mnuSave
        '
        Me.mnuSave.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.mnuSave.Location = New System.Drawing.Point(330, 8)
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(75, 24)
        Me.mnuSave.TabIndex = 4
        Me.mnuSave.Text = "&Save"
        '
        'mnuDelete
        '
        Me.mnuDelete.Location = New System.Drawing.Point(168, 8)
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.Size = New System.Drawing.Size(75, 24)
        Me.mnuDelete.TabIndex = 2
        Me.mnuDelete.Text = "&Delete"
        '
        'mnuCancel
        '
        Me.mnuCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.mnuCancel.Location = New System.Drawing.Point(411, 8)
        Me.mnuCancel.Name = "mnuCancel"
        Me.mnuCancel.Size = New System.Drawing.Size(75, 24)
        Me.mnuCancel.TabIndex = 5
        Me.mnuCancel.Text = "&Cancel"
        '
        'mnuPrint
        '
        Me.mnuPrint.Location = New System.Drawing.Point(249, 8)
        Me.mnuPrint.Name = "mnuPrint"
        Me.mnuPrint.Size = New System.Drawing.Size(75, 24)
        Me.mnuPrint.TabIndex = 3
        Me.mnuPrint.Text = "&Print"
        Me.mnuPrint.Visible = False
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.mnuPrint)
        Me.UltraGroupBox1.Controls.Add(Me.mnuNew)
        Me.UltraGroupBox1.Controls.Add(Me.mnuCancel)
        Me.UltraGroupBox1.Controls.Add(Me.mnuEdit)
        Me.UltraGroupBox1.Controls.Add(Me.mnuDelete)
        Me.UltraGroupBox1.Controls.Add(Me.mnuSave)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(493, 40)
        Me.UltraGroupBox1.TabIndex = 0
        '
        'MenuButtons
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Name = "MenuButtons"
        Me.Size = New System.Drawing.Size(493, 40)
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuNew As Infragistics.Win.Misc.UltraButton
    Friend WithEvents mnuEdit As Infragistics.Win.Misc.UltraButton
    Friend WithEvents mnuSave As Infragistics.Win.Misc.UltraButton
    Friend WithEvents mnuDelete As Infragistics.Win.Misc.UltraButton
    Friend WithEvents mnuCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents mnuPrint As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox

End Class
