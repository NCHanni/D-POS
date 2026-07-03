<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListWithDateRange
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.btnView = New Infragistics.Win.Misc.UltraButton
        Me.btnPrint = New Infragistics.Win.Misc.UltraButton
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox
        Me.btnSearch = New Infragistics.Win.Misc.UltraButton
        Me.ctrlList = New BusinessPro.Core.ListSearch
        Me.ctrlDateRange = New BusinessPro.Core.DateRange
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.Location = New System.Drawing.Point(646, 13)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(120, 23)
        Me.btnView.TabIndex = 5
        Me.btnView.Text = "View Information"
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.Location = New System.Drawing.Point(484, 13)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 5
        Me.btnPrint.Text = "Print"
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(565, 13)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 5
        Me.btnRefresh.Text = "Refresh"
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.btnSearch)
        Me.UltraGroupBox1.Controls.Add(Me.btnView)
        Me.UltraGroupBox1.Controls.Add(Me.btnRefresh)
        Me.UltraGroupBox1.Controls.Add(Me.ctrlDateRange)
        Me.UltraGroupBox1.Controls.Add(Me.btnPrint)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(775, 48)
        Me.UltraGroupBox1.TabIndex = 8
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(261, 13)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "Search"
        '
        'ctrlList
        '
        Me.ctrlList.ActiveRow = Nothing
        Me.ctrlList.CloseParentFormOnOK = True
        Me.ctrlList.DataSource = Nothing
        Me.ctrlList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlList.Location = New System.Drawing.Point(0, 48)
        Me.ctrlList.MultiSelect = False
        Me.ctrlList.MultiSelectHeader = False
        Me.ctrlList.Name = "ctrlList"
        Me.ctrlList.SearchKeys = Nothing
        Me.ctrlList.ShowButtons = True
        Me.ctrlList.ShowSearch = True
        Me.ctrlList.Size = New System.Drawing.Size(775, 410)
        Me.ctrlList.TabIndex = 6
        Me.ctrlList.UsedInBrowser = False
        '
        'ctrlDateRange
        '
        Me.ctrlDateRange.EndDate = New Date(2009, 11, 12, 23, 59, 59, 0)
        Me.ctrlDateRange.Location = New System.Drawing.Point(10, 13)
        Me.ctrlDateRange.Name = "ctrlDateRange"
        Me.ctrlDateRange.Size = New System.Drawing.Size(245, 27)
        Me.ctrlDateRange.StartDate = New Date(2009, 10, 12, 0, 0, 0, 0)
        Me.ctrlDateRange.TabIndex = 4
        '
        'ListWithDateRange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ctrlList)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Name = "ListWithDateRange"
        Me.Size = New System.Drawing.Size(775, 458)
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctrlDateRange As Core.DateRange
    Friend WithEvents btnView As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnPrint As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents ctrlList As Core.ListSearch
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnSearch As Infragistics.Win.Misc.UltraButton

End Class
