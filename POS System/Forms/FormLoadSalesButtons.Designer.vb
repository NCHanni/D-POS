<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormLoadSalesButtons
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
        Me.btnPostedSales = New Infragistics.Win.Misc.UltraButton()
        Me.btnSuspendedSale = New Infragistics.Win.Misc.UltraButton()
        Me.btnRefundSale = New Infragistics.Win.Misc.UltraButton()
        Me.SuspendLayout()
        '
        'btnPostedSales
        '
        Me.btnPostedSales.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnPostedSales.Location = New System.Drawing.Point(1, 0)
        Me.btnPostedSales.Name = "btnPostedSales"
        Me.btnPostedSales.Size = New System.Drawing.Size(91, 42)
        Me.btnPostedSales.TabIndex = 12
        Me.btnPostedSales.TabStop = False
        Me.btnPostedSales.Text = "Posted"
        '
        'btnSuspendedSale
        '
        Me.btnSuspendedSale.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnSuspendedSale.Location = New System.Drawing.Point(1, 84)
        Me.btnSuspendedSale.Name = "btnSuspendedSale"
        Me.btnSuspendedSale.Size = New System.Drawing.Size(91, 42)
        Me.btnSuspendedSale.TabIndex = 13
        Me.btnSuspendedSale.TabStop = False
        Me.btnSuspendedSale.Text = "Suspended"
        '
        'btnRefundSale
        '
        Me.btnRefundSale.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnRefundSale.Location = New System.Drawing.Point(1, 42)
        Me.btnRefundSale.Name = "btnRefundSale"
        Me.btnRefundSale.Size = New System.Drawing.Size(91, 42)
        Me.btnRefundSale.TabIndex = 14
        Me.btnRefundSale.TabStop = False
        Me.btnRefundSale.Text = "Refund"
        '
        'FormLoadSalesButtons
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(92, 126)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnRefundSale)
        Me.Controls.Add(Me.btnSuspendedSale)
        Me.Controls.Add(Me.btnPostedSales)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormLoadSalesButtons"
        Me.Padding = New System.Windows.Forms.Padding(1, 0, 0, 0)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnPostedSales As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnSuspendedSale As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnRefundSale As Infragistics.Win.Misc.UltraButton
End Class
