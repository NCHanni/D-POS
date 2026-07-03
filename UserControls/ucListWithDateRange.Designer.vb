<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucListWithDateRange
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
        Me.btnView = New Infragistics.Win.Misc.UltraButton()
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.ctrlDateRange = New Core.DateRange()
        Me.btnSearch = New Infragistics.Win.Misc.UltraButton()
        Me.ctrlList = New BusinessPro.Core.ListSearch()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.Location = New System.Drawing.Point(507, 14)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(120, 23)
        Me.btnView.TabIndex = 5
        Me.btnView.Text = "View Information"
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Location = New System.Drawing.Point(426, 14)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 4
        Me.btnRefresh.Text = "Refresh"
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.ctrlDateRange)
        Me.UltraGroupBox1.Controls.Add(Me.btnView)
        Me.UltraGroupBox1.Controls.Add(Me.btnSearch)
        Me.UltraGroupBox1.Controls.Add(Me.btnRefresh)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(640, 41)
        Me.UltraGroupBox1.TabIndex = 0
        '
        'ctrlDateRange
        '
        Me.ctrlDateRange.Location = New System.Drawing.Point(13, 11)
        Me.ctrlDateRange.Name = "ctrlDateRange"
        Me.ctrlDateRange.Size = New System.Drawing.Size(245, 25)
        Me.ctrlDateRange.TabIndex = 0
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(264, 12)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Search"
        '
        'ctrlList
        '
        Me.ctrlList.ActiveRow = Nothing
        Me.ctrlList.CloseParentFormOnOK = True
        Me.ctrlList.DataSource = Nothing
        Me.ctrlList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlList.Location = New System.Drawing.Point(0, 41)
        Me.ctrlList.MultiSelect = False
        Me.ctrlList.MultiSelectHeader = False
        Me.ctrlList.Name = "ctrlList"
        Me.ctrlList.Padding = New System.Windows.Forms.Padding(3, 0, 3, 6)
        Me.ctrlList.SearchKeys = Nothing
        Me.ctrlList.ShowButtons = True
        Me.ctrlList.ShowSearch = True
        Me.ctrlList.Size = New System.Drawing.Size(640, 409)
        Me.ctrlList.TabIndex = 1
        Me.ctrlList.UsedInBrowser = False
        '
        'ucListWithDateRange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ctrlList)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Name = "ucListWithDateRange"
        Me.Size = New System.Drawing.Size(640, 450)
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctrlDateRange As Core.DateRange
    Friend WithEvents btnView As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents ctrlList As BusinessPro.Core.ListSearch
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnSearch As Infragistics.Win.Misc.UltraButton

End Class
