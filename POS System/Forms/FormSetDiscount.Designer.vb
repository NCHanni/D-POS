<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSetDiscount
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
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.UltraGroupBox2 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.btnOK = New Infragistics.Win.Misc.UltraButton()
        Me.btnCancel = New Infragistics.Win.Misc.UltraButton()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.numNewPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.numDiscountedPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.numDiscountAmount = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.lblVatComputed = New Infragistics.Win.Misc.UltraLabel()
        Me.lblVat = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.numNetPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.numVatComputed = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.numVAT = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.numDiscountPercent = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.numPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.numDiscountActual = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        CType(Me.UltraGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox2.SuspendLayout()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        CType(Me.numNewPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numDiscountedPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numDiscountAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numNetPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numVatComputed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numVAT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numDiscountPercent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numDiscountActual, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraGroupBox2
        '
        Me.UltraGroupBox2.Controls.Add(Me.btnOK)
        Me.UltraGroupBox2.Controls.Add(Me.btnCancel)
        Me.UltraGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UltraGroupBox2.Location = New System.Drawing.Point(0, 315)
        Me.UltraGroupBox2.Name = "UltraGroupBox2"
        Me.UltraGroupBox2.Size = New System.Drawing.Size(304, 36)
        Me.UltraGroupBox2.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOK.Location = New System.Drawing.Point(136, 2)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(217, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(18, 24)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(59, 15)
        Me.UltraLabel1.TabIndex = 0
        Me.UltraLabel1.Text = "Unit Price"
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel7)
        Me.UltraGroupBox1.Controls.Add(Me.numDiscountActual)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel6)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel4)
        Me.UltraGroupBox1.Controls.Add(Me.numNewPrice)
        Me.UltraGroupBox1.Controls.Add(Me.numDiscountedPrice)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel3)
        Me.UltraGroupBox1.Controls.Add(Me.numDiscountAmount)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel5)
        Me.UltraGroupBox1.Controls.Add(Me.lblVatComputed)
        Me.UltraGroupBox1.Controls.Add(Me.lblVat)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel2)
        Me.UltraGroupBox1.Controls.Add(Me.numNetPrice)
        Me.UltraGroupBox1.Controls.Add(Me.numVatComputed)
        Me.UltraGroupBox1.Controls.Add(Me.numVAT)
        Me.UltraGroupBox1.Controls.Add(Me.numDiscountPercent)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel1)
        Me.UltraGroupBox1.Controls.Add(Me.numPrice)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(304, 315)
        Me.UltraGroupBox1.TabIndex = 0
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.UltraLabel6.AutoSize = True
        Me.UltraLabel6.Location = New System.Drawing.Point(18, 281)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(70, 15)
        Me.UltraLabel6.TabIndex = 16
        Me.UltraLabel6.Text = "NEW PRICE"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.UltraLabel4.AutoSize = True
        Me.UltraLabel4.Location = New System.Drawing.Point(18, 225)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(99, 15)
        Me.UltraLabel4.TabIndex = 12
        Me.UltraLabel4.Text = "Discounted Price"
        '
        'numNewPrice
        '
        Me.numNewPrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numNewPrice.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.numNewPrice.Location = New System.Drawing.Point(130, 277)
        Me.numNewPrice.MinValue = 0
        Me.numNewPrice.Name = "numNewPrice"
        Me.numNewPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numNewPrice.ReadOnly = True
        Me.numNewPrice.Size = New System.Drawing.Size(150, 22)
        Me.numNewPrice.TabIndex = 17
        Me.numNewPrice.TabStop = False
        '
        'numDiscountedPrice
        '
        Me.numDiscountedPrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numDiscountedPrice.Location = New System.Drawing.Point(130, 221)
        Me.numDiscountedPrice.MinValue = 0
        Me.numDiscountedPrice.Name = "numDiscountedPrice"
        Me.numDiscountedPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numDiscountedPrice.ReadOnly = True
        Me.numDiscountedPrice.Size = New System.Drawing.Size(150, 22)
        Me.numDiscountedPrice.TabIndex = 13
        Me.numDiscountedPrice.TabStop = False
        '
        'UltraLabel3
        '
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(18, 151)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(102, 15)
        Me.UltraLabel3.TabIndex = 8
        Me.UltraLabel3.Text = "Discount Amount"
        '
        'numDiscountAmount
        '
        Appearance2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.numDiscountAmount.Appearance = Appearance2
        Me.numDiscountAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.numDiscountAmount.Location = New System.Drawing.Point(130, 148)
        Me.numDiscountAmount.MinValue = 0
        Me.numDiscountAmount.Name = "numDiscountAmount"
        Me.numDiscountAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numDiscountAmount.Size = New System.Drawing.Size(150, 22)
        Me.numDiscountAmount.TabIndex = 9
        '
        'UltraLabel5
        '
        Me.UltraLabel5.AutoSize = True
        Me.UltraLabel5.Location = New System.Drawing.Point(18, 80)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(55, 15)
        Me.UltraLabel5.TabIndex = 4
        Me.UltraLabel5.Text = "Net Price"
        '
        'lblVatComputed
        '
        Me.lblVatComputed.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblVatComputed.AutoSize = True
        Me.lblVatComputed.Location = New System.Drawing.Point(18, 252)
        Me.lblVatComputed.Name = "lblVatComputed"
        Me.lblVatComputed.Size = New System.Drawing.Size(53, 15)
        Me.lblVatComputed.TabIndex = 14
        Me.lblVatComputed.Text = "VAT (%)"
        '
        'lblVat
        '
        Me.lblVat.AutoSize = True
        Me.lblVat.Location = New System.Drawing.Point(18, 52)
        Me.lblVat.Name = "lblVat"
        Me.lblVat.Size = New System.Drawing.Size(53, 15)
        Me.lblVat.TabIndex = 2
        Me.lblVat.Text = "VAT (%)"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Location = New System.Drawing.Point(18, 124)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(80, 15)
        Me.UltraLabel2.TabIndex = 6
        Me.UltraLabel2.Text = "Discount (%)"
        '
        'numNetPrice
        '
        Me.numNetPrice.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.numNetPrice.Location = New System.Drawing.Point(130, 77)
        Me.numNetPrice.MinValue = 0
        Me.numNetPrice.Name = "numNetPrice"
        Me.numNetPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numNetPrice.ReadOnly = True
        Me.numNetPrice.Size = New System.Drawing.Size(150, 22)
        Me.numNetPrice.TabIndex = 5
        Me.numNetPrice.TabStop = False
        '
        'numVatComputed
        '
        Me.numVatComputed.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numVatComputed.Location = New System.Drawing.Point(130, 249)
        Me.numVatComputed.MinValue = 0
        Me.numVatComputed.Name = "numVatComputed"
        Me.numVatComputed.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numVatComputed.ReadOnly = True
        Me.numVatComputed.Size = New System.Drawing.Size(150, 22)
        Me.numVatComputed.TabIndex = 15
        Me.numVatComputed.TabStop = False
        '
        'numVAT
        '
        Me.numVAT.Location = New System.Drawing.Point(130, 49)
        Me.numVAT.MinValue = 0
        Me.numVAT.Name = "numVAT"
        Me.numVAT.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numVAT.ReadOnly = True
        Me.numVAT.Size = New System.Drawing.Size(150, 22)
        Me.numVAT.TabIndex = 3
        Me.numVAT.TabStop = False
        '
        'numDiscountPercent
        '
        Appearance3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.numDiscountPercent.Appearance = Appearance3
        Me.numDiscountPercent.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.numDiscountPercent.Location = New System.Drawing.Point(130, 121)
        Me.numDiscountPercent.MaxValue = 100.0R
        Me.numDiscountPercent.MinValue = 0
        Me.numDiscountPercent.Name = "numDiscountPercent"
        Me.numDiscountPercent.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numDiscountPercent.Size = New System.Drawing.Size(150, 22)
        Me.numDiscountPercent.TabIndex = 7
        '
        'numPrice
        '
        Me.numPrice.Location = New System.Drawing.Point(130, 21)
        Me.numPrice.MinValue = 0
        Me.numPrice.Name = "numPrice"
        Me.numPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numPrice.ReadOnly = True
        Me.numPrice.Size = New System.Drawing.Size(150, 22)
        Me.numPrice.TabIndex = 1
        Me.numPrice.TabStop = False
        '
        'UltraLabel7
        '
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Location = New System.Drawing.Point(18, 179)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(93, 15)
        Me.UltraLabel7.TabIndex = 10
        Me.UltraLabel7.Text = "Actual Discount"
        '
        'numActualDiscount
        '
        Appearance1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.numDiscountActual.Appearance = Appearance1
        Me.numDiscountActual.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.numDiscountActual.Location = New System.Drawing.Point(130, 176)
        Me.numDiscountActual.MinValue = 0
        Me.numDiscountActual.Name = "numActualDiscount"
        Me.numDiscountActual.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.numDiscountActual.Size = New System.Drawing.Size(150, 22)
        Me.numDiscountActual.TabIndex = 11
        '
        'FormSetDiscount
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(304, 351)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Controls.Add(Me.UltraGroupBox2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormSetDiscount"
        Me.Text = "Set Discount"
        CType(Me.UltraGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox2.ResumeLayout(False)
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.UltraGroupBox1.PerformLayout()
        CType(Me.numNewPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numDiscountedPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numDiscountAmount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numNetPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numVatComputed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numVAT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numDiscountPercent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numDiscountActual, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraGroupBox2 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnOK As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnCancel As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents numPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numDiscountedPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numDiscountAmount As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numDiscountPercent As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents lblVat As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numVAT As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numNetPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents lblVatComputed As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numVatComputed As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numNewPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents numDiscountActual As Infragistics.Win.UltraWinEditors.UltraNumericEditor
End Class
