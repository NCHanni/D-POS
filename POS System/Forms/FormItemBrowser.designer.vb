<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormItemBrowser
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
        Me.grpCategoryList = New Infragistics.Win.Misc.UltraGroupBox()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.cmbCategories = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.ctrlList = New BusinessPro.POS.ucListSearch()
        CType(Me.grpCategoryList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCategoryList.SuspendLayout()
        CType(Me.cmbCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpCategoryList
        '
        Me.grpCategoryList.Controls.Add(Me.cmbCategories)
        Me.grpCategoryList.Controls.Add(Me.UltraLabel1)
        Me.grpCategoryList.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpCategoryList.Location = New System.Drawing.Point(0, 0)
        Me.grpCategoryList.Name = "grpCategoryList"
        Me.grpCategoryList.Size = New System.Drawing.Size(624, 34)
        Me.grpCategoryList.TabIndex = 0
        Me.grpCategoryList.Visible = False
        '
        'UltraLabel1
        '
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 11)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(50, 14)
        Me.UltraLabel1.TabIndex = 0
        Me.UltraLabel1.Text = "Category"
        '
        'cmbCategories
        '
        Me.cmbCategories.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList
        Me.cmbCategories.Location = New System.Drawing.Point(68, 7)
        Me.cmbCategories.Name = "cmbCategories"
        Me.cmbCategories.Size = New System.Drawing.Size(200, 21)
        Me.cmbCategories.TabIndex = 1
        '
        'ctrlList
        '
        Me.ctrlList.ActiveRow = Nothing
        Me.ctrlList.AutoClose = True
        Me.ctrlList.DataSource = Nothing
        Me.ctrlList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlList.Location = New System.Drawing.Point(0, 34)
        Me.ctrlList.Name = "ctrlList"
        Me.ctrlList.Padding = New System.Windows.Forms.Padding(3, 0, 3, 6)
        Me.ctrlList.SearchKeys = Nothing
        Me.ctrlList.ShowSearch = True
        Me.ctrlList.Size = New System.Drawing.Size(624, 397)
        Me.ctrlList.TabIndex = 1
        Me.ctrlList.UsedInBrowser = True
        '
        'FormItemBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 431)
        Me.Controls.Add(Me.ctrlList)
        Me.Controls.Add(Me.grpCategoryList)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormItemBrowser"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Browser"
        CType(Me.grpCategoryList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCategoryList.ResumeLayout(False)
        Me.grpCategoryList.PerformLayout()
        CType(Me.cmbCategories, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctrlList As BusinessPro.POS.ucListSearch
    Friend WithEvents grpCategoryList As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents cmbCategories As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
End Class
