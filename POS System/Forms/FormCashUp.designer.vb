<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormCashUp
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
        Dim EditorButton1 As Infragistics.Win.UltraWinEditors.EditorButton = New Infragistics.Win.UltraWinEditors.EditorButton()
        Me.grpDetails = New Infragistics.Win.Misc.UltraGroupBox()
        Me.txtCashierUserName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.txtCashOutDate = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtCashInDate = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.txtCashierName = New Infragistics.Win.UltraWinEditors.UltraTextEditor()
        Me.numCashBeg = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.numCashEnd = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.btnClose = New Infragistics.Win.Misc.UltraButton()
        Me.btnFinalizeCashEnd = New Infragistics.Win.Misc.UltraButton()
        Me.btnUndoFinalize = New Infragistics.Win.Misc.UltraButton()
        Me.btnRePrintShiftReport = New Infragistics.Win.Misc.UltraButton()
        CType(Me.grpDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetails.SuspendLayout()
        CType(Me.txtCashierUserName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCashOutDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCashInDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCashierName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numCashBeg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numCashEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpDetails
        '
        Me.grpDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDetails.CaptionAlignment = Infragistics.Win.Misc.GroupBoxCaptionAlignment.Center
        Me.grpDetails.Controls.Add(Me.txtCashierUserName)
        Me.grpDetails.Controls.Add(Me.UltraLabel7)
        Me.grpDetails.Controls.Add(Me.txtCashOutDate)
        Me.grpDetails.Controls.Add(Me.txtCashInDate)
        Me.grpDetails.Controls.Add(Me.txtCashierName)
        Me.grpDetails.Controls.Add(Me.numCashBeg)
        Me.grpDetails.Controls.Add(Me.numCashEnd)
        Me.grpDetails.Controls.Add(Me.UltraLabel4)
        Me.grpDetails.Controls.Add(Me.UltraLabel3)
        Me.grpDetails.Controls.Add(Me.UltraLabel5)
        Me.grpDetails.Controls.Add(Me.UltraLabel2)
        Me.grpDetails.Controls.Add(Me.UltraLabel1)
        Me.grpDetails.Location = New System.Drawing.Point(12, 12)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(460, 158)
        Me.grpDetails.TabIndex = 0
        '
        'txtCashierUserName
        '
        Me.txtCashierUserName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCashierUserName.Location = New System.Drawing.Point(95, 20)
        Me.txtCashierUserName.Name = "txtCashierUserName"
        Me.txtCashierUserName.ReadOnly = True
        Me.txtCashierUserName.Size = New System.Drawing.Size(340, 21)
        Me.txtCashierUserName.TabIndex = 1
        '
        'UltraLabel7
        '
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Location = New System.Drawing.Point(21, 24)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(68, 14)
        Me.UltraLabel7.TabIndex = 0
        Me.UltraLabel7.Text = "USERNAME"
        '
        'txtCashOutDate
        '
        Me.txtCashOutDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCashOutDate.Location = New System.Drawing.Point(320, 88)
        Me.txtCashOutDate.Name = "txtCashOutDate"
        Me.txtCashOutDate.ReadOnly = True
        Me.txtCashOutDate.Size = New System.Drawing.Size(115, 21)
        Me.txtCashOutDate.TabIndex = 7
        '
        'txtCashInDate
        '
        Me.txtCashInDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCashInDate.Location = New System.Drawing.Point(95, 88)
        Me.txtCashInDate.Name = "txtCashInDate"
        Me.txtCashInDate.ReadOnly = True
        Me.txtCashInDate.Size = New System.Drawing.Size(115, 21)
        Me.txtCashInDate.TabIndex = 5
        '
        'txtCashierName
        '
        Me.txtCashierName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCashierName.Location = New System.Drawing.Point(95, 47)
        Me.txtCashierName.Name = "txtCashierName"
        Me.txtCashierName.ReadOnly = True
        Me.txtCashierName.Size = New System.Drawing.Size(340, 21)
        Me.txtCashierName.TabIndex = 3
        '
        'numCashBeg
        '
        Me.numCashBeg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numCashBeg.Enabled = False
        Me.numCashBeg.Location = New System.Drawing.Point(95, 115)
        Me.numCashBeg.Name = "numCashBeg"
        Me.numCashBeg.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numCashBeg.Size = New System.Drawing.Size(115, 21)
        Me.numCashBeg.TabIndex = 9
        Me.numCashBeg.TabStop = False
        '
        'numCashEnd
        '
        Me.numCashEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        EditorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button
        EditorButton1.Text = "..."
        Me.numCashEnd.ButtonsRight.Add(EditorButton1)
        Me.numCashEnd.Location = New System.Drawing.Point(320, 115)
        Me.numCashEnd.Name = "numCashEnd"
        Me.numCashEnd.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numCashEnd.ReadOnly = True
        Me.numCashEnd.Size = New System.Drawing.Size(115, 21)
        Me.numCashEnd.TabIndex = 11
        Me.numCashEnd.TabStop = False
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraLabel4.AutoSize = True
        Me.UltraLabel4.Location = New System.Drawing.Point(223, 119)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(91, 14)
        Me.UltraLabel4.TabIndex = 10
        Me.UltraLabel4.Text = "Cash - End (F11)"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(21, 119)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(64, 14)
        Me.UltraLabel3.TabIndex = 8
        Me.UltraLabel3.Text = "Cash - Beg."
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraLabel5.AutoSize = True
        Me.UltraLabel5.Location = New System.Drawing.Point(223, 92)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(55, 14)
        Me.UltraLabel5.TabIndex = 6
        Me.UltraLabel5.Text = "LOG OUT"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Location = New System.Drawing.Point(21, 92)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(42, 14)
        Me.UltraLabel2.TabIndex = 4
        Me.UltraLabel2.Text = "LOG IN"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(21, 51)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(55, 14)
        Me.UltraLabel1.TabIndex = 2
        Me.UltraLabel1.Text = "CASHIER"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(397, 176)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'btnFinalizeCashEnd
        '
        Me.btnFinalizeCashEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnFinalizeCashEnd.Location = New System.Drawing.Point(12, 176)
        Me.btnFinalizeCashEnd.Name = "btnFinalizeCashEnd"
        Me.btnFinalizeCashEnd.Size = New System.Drawing.Size(130, 23)
        Me.btnFinalizeCashEnd.TabIndex = 1
        Me.btnFinalizeCashEnd.Text = "Finalize Cash End (F9)"
        '
        'btnUndoFinalize
        '
        Me.btnUndoFinalize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUndoFinalize.Location = New System.Drawing.Point(282, 176)
        Me.btnUndoFinalize.Name = "btnUndoFinalize"
        Me.btnUndoFinalize.Size = New System.Drawing.Size(130, 23)
        Me.btnUndoFinalize.TabIndex = 2
        Me.btnUndoFinalize.Text = "Unlock Cash - End"
        Me.btnUndoFinalize.Visible = False
        '
        'btnRePrintShiftReport
        '
        Me.btnRePrintShiftReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRePrintShiftReport.Location = New System.Drawing.Point(148, 176)
        Me.btnRePrintShiftReport.Name = "btnRePrintShiftReport"
        Me.btnRePrintShiftReport.Size = New System.Drawing.Size(130, 23)
        Me.btnRePrintShiftReport.TabIndex = 4
        Me.btnRePrintShiftReport.Text = "Re-print Shift Report"
        Me.btnRePrintShiftReport.Visible = False
        '
        'FormCashUp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(484, 211)
        Me.Controls.Add(Me.btnRePrintShiftReport)
        Me.Controls.Add(Me.btnUndoFinalize)
        Me.Controls.Add(Me.btnFinalizeCashEnd)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.grpDetails)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormCashUp"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cash-up Report"
        CType(Me.grpDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        CType(Me.txtCashierUserName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCashOutDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCashInDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCashierName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numCashBeg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numCashEnd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDetails As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnClose As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numCashEnd As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents numCashBeg As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtCashInDate As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtCashierName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtCashOutDate As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents txtCashierUserName As Infragistics.Win.UltraWinEditors.UltraTextEditor
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents btnFinalizeCashEnd As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnUndoFinalize As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnRePrintShiftReport As Infragistics.Win.Misc.UltraButton
End Class
