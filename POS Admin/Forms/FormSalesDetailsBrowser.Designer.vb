<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSalesDetailsBrowser
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
        Me.ctrlList = New BusinessPro.Core.ListSearch()
        Me.SuspendLayout()
        '
        'ctrlList
        '
        Me.ctrlList.ActiveRow = Nothing
        Me.ctrlList.CloseParentFormOnOK = True
        Me.ctrlList.DataSource = Nothing
        Me.ctrlList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlList.Location = New System.Drawing.Point(0, 0)
        Me.ctrlList.MultiSelect = True
        Me.ctrlList.MultiSelectHeader = True
        Me.ctrlList.Name = "ctrlList"
        Me.ctrlList.Padding = New System.Windows.Forms.Padding(3, 0, 3, 5)
        Me.ctrlList.SearchKeys = Nothing
        Me.ctrlList.ShowButtons = True
        Me.ctrlList.ShowSearch = True
        Me.ctrlList.Size = New System.Drawing.Size(684, 361)
        Me.ctrlList.TabIndex = 5
        Me.ctrlList.UsedInBrowser = True
        '
        'FormSalesDetailsBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 361)
        Me.Controls.Add(Me.ctrlList)
        Me.MaximizeBox = False
        Me.Name = "FormSalesDetailsBrowser"
        Me.Text = "Sales Details Browser"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctrlList As BusinessPro.Core.ListSearch
End Class
