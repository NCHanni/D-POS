<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEJournalViewer
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
        Dim UltraTreeNode3 As Infragistics.Win.UltraWinTree.UltraTreeNode = New Infragistics.Win.UltraWinTree.UltraTreeNode()
        Dim UltraTreeNode4 As Infragistics.Win.UltraWinTree.UltraTreeNode = New Infragistics.Win.UltraWinTree.UltraTreeNode()
        Dim UltraTreeNode5 As Infragistics.Win.UltraWinTree.UltraTreeNode = New Infragistics.Win.UltraWinTree.UltraTreeNode()
        Dim UltraTreeNode6 As Infragistics.Win.UltraWinTree.UltraTreeNode = New Infragistics.Win.UltraWinTree.UltraTreeNode()
        Dim UltraTreeNode7 As Infragistics.Win.UltraWinTree.UltraTreeNode = New Infragistics.Win.UltraWinTree.UltraTreeNode()
        Dim UltraTreeNode8 As Infragistics.Win.UltraWinTree.UltraTreeNode = New Infragistics.Win.UltraWinTree.UltraTreeNode()
        Dim Override1 As Infragistics.Win.UltraWinTree.Override = New Infragistics.Win.UltraWinTree.Override()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.lblPath = New Infragistics.Win.Misc.UltraLabel()
        Me.btnExport = New Infragistics.Win.Misc.UltraButton()
        Me.btnRefresh = New Infragistics.Win.Misc.UltraButton()
        Me.btnClose = New Infragistics.Win.Misc.UltraButton()
        Me.grpCreditCards = New Infragistics.Win.Misc.UltraGroupBox()
        Me.UltraPanel2 = New Infragistics.Win.Misc.UltraPanel()
        Me.txtJournal = New System.Windows.Forms.RichTextBox()
        Me.UltraPanel1 = New Infragistics.Win.Misc.UltraPanel()
        Me.lsvJournals = New Infragistics.Win.UltraWinListView.UltraListView()
        Me.treeJournals = New Infragistics.Win.UltraWinTree.UltraTree()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        CType(Me.grpCreditCards, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCreditCards.SuspendLayout()
        Me.UltraPanel2.ClientArea.SuspendLayout()
        Me.UltraPanel2.SuspendLayout()
        Me.UltraPanel1.ClientArea.SuspendLayout()
        Me.UltraPanel1.SuspendLayout()
        CType(Me.lsvJournals, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.treeJournals, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraGroupBox1
        '
        Me.UltraGroupBox1.Controls.Add(Me.lblPath)
        Me.UltraGroupBox1.Controls.Add(Me.btnExport)
        Me.UltraGroupBox1.Controls.Add(Me.btnRefresh)
        Me.UltraGroupBox1.Controls.Add(Me.btnClose)
        Me.UltraGroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UltraGroupBox1.Location = New System.Drawing.Point(0, 321)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(704, 40)
        Me.UltraGroupBox1.TabIndex = 1
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Enabled = False
        Me.lblPath.Location = New System.Drawing.Point(12, 13)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(31, 14)
        Me.lblPath.TabIndex = 0
        Me.lblPath.Text = "Path: "
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExport.Enabled = False
        Me.btnExport.Location = New System.Drawing.Point(455, 8)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 1
        Me.btnExport.Text = "Export"
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnRefresh.Location = New System.Drawing.Point(536, 8)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "Refresh"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(617, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'grpCreditCards
        '
        Me.grpCreditCards.ContentPadding.Bottom = 4
        Me.grpCreditCards.ContentPadding.Left = 4
        Me.grpCreditCards.ContentPadding.Right = 4
        Me.grpCreditCards.ContentPadding.Top = 4
        Me.grpCreditCards.Controls.Add(Me.UltraPanel2)
        Me.grpCreditCards.Controls.Add(Me.UltraPanel1)
        Me.grpCreditCards.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCreditCards.Location = New System.Drawing.Point(0, 0)
        Me.grpCreditCards.Name = "grpCreditCards"
        Me.grpCreditCards.Size = New System.Drawing.Size(704, 321)
        Me.grpCreditCards.TabIndex = 0
        '
        'UltraPanel2
        '
        Me.UltraPanel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        'UltraPanel2.ClientArea
        '
        Me.UltraPanel2.ClientArea.Controls.Add(Me.txtJournal)
        Me.UltraPanel2.Location = New System.Drawing.Point(213, 7)
        Me.UltraPanel2.Name = "UltraPanel2"
        Me.UltraPanel2.Size = New System.Drawing.Size(481, 307)
        Me.UltraPanel2.TabIndex = 3
        '
        'txtJournal
        '
        Me.txtJournal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtJournal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtJournal.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtJournal.Location = New System.Drawing.Point(0, 0)
        Me.txtJournal.Name = "txtJournal"
        Me.txtJournal.ReadOnly = True
        Me.txtJournal.Size = New System.Drawing.Size(481, 307)
        Me.txtJournal.TabIndex = 0
        Me.txtJournal.Text = ""
        '
        'UltraPanel1
        '
        '
        'UltraPanel1.ClientArea
        '
        Me.UltraPanel1.ClientArea.Controls.Add(Me.lsvJournals)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.treeJournals)
        Me.UltraPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.UltraPanel1.Location = New System.Drawing.Point(7, 7)
        Me.UltraPanel1.Name = "UltraPanel1"
        Me.UltraPanel1.Size = New System.Drawing.Size(200, 307)
        Me.UltraPanel1.TabIndex = 1
        '
        'lsvJournals
        '
        Me.lsvJournals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsvJournals.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvJournals.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.[False]
        Me.lsvJournals.ItemSettings.DefaultImage = Global.BusinessPro.POS.Admin.My.Resources.Resources.log_icon
        Me.lsvJournals.ItemSettings.HideSelection = False
        Me.lsvJournals.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.[Single]
        Me.lsvJournals.Location = New System.Drawing.Point(0, 140)
        Me.lsvJournals.Name = "lsvJournals"
        Me.lsvJournals.Size = New System.Drawing.Size(200, 167)
        Me.lsvJournals.TabIndex = 1
        Me.lsvJournals.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List
        Me.lsvJournals.ViewSettingsList.MultiColumn = False
        '
        'treeJournals
        '
        Me.treeJournals.Dock = System.Windows.Forms.DockStyle.Top
        Me.treeJournals.DrawsFocusRect = Infragistics.Win.DefaultableBoolean.[False]
        Me.treeJournals.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.treeJournals.FullRowSelect = True
        Me.treeJournals.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.treeJournals.Indent = 20
        Me.treeJournals.Location = New System.Drawing.Point(0, 0)
        Me.treeJournals.Name = "treeJournals"
        Me.treeJournals.NodeConnectorColor = System.Drawing.SystemColors.ControlDark
        UltraTreeNode4.Key = "Receipts"
        UltraTreeNode4.Text = "Receipts"
        UltraTreeNode5.Key = "ReceiptsRefund"
        UltraTreeNode5.Text = "Refund"
        UltraTreeNode6.Key = "ReceiptsVoid"
        UltraTreeNode6.Text = "Void"
        UltraTreeNode7.Key = "XReadings"
        UltraTreeNode7.Text = "X-Readings"
        UltraTreeNode8.Key = "ZReadings"
        UltraTreeNode8.Text = "Z-Readings"
        UltraTreeNode3.Nodes.AddRange(New Infragistics.Win.UltraWinTree.UltraTreeNode() {UltraTreeNode4, UltraTreeNode5, UltraTreeNode6, UltraTreeNode7, UltraTreeNode8})
        UltraTreeNode3.Text = "Root"
        Me.treeJournals.Nodes.AddRange(New Infragistics.Win.UltraWinTree.UltraTreeNode() {UltraTreeNode3})
        Override1.ImageSize = New System.Drawing.Size(16, 16)
        Appearance1.Image = Global.BusinessPro.POS.Admin.My.Resources.Resources.Folder_icon
        Override1.NodeAppearance = Appearance1
        Override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.[Single]
        Me.treeJournals.Override = Override1
        Me.treeJournals.Scrollable = Infragistics.Win.UltraWinTree.Scrollbar.Hide
        Me.treeJournals.Size = New System.Drawing.Size(200, 140)
        Me.treeJournals.TabIndex = 0
        Me.treeJournals.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Grid
        '
        'FormEJournalViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(704, 361)
        Me.Controls.Add(Me.grpCreditCards)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Name = "FormEJournalViewer"
        Me.Text = "E-Journal Viewer"
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.UltraGroupBox1.PerformLayout()
        CType(Me.grpCreditCards, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCreditCards.ResumeLayout(False)
        Me.UltraPanel2.ClientArea.ResumeLayout(False)
        Me.UltraPanel2.ResumeLayout(False)
        Me.UltraPanel1.ClientArea.ResumeLayout(False)
        Me.UltraPanel1.ResumeLayout(False)
        CType(Me.lsvJournals, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.treeJournals, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents grpCreditCards As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents btnClose As Infragistics.Win.Misc.UltraButton
    Friend WithEvents btnRefresh As Infragistics.Win.Misc.UltraButton
    Friend WithEvents UltraPanel2 As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents UltraPanel1 As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents lsvJournals As Infragistics.Win.UltraWinListView.UltraListView
    Friend WithEvents treeJournals As Infragistics.Win.UltraWinTree.UltraTree
    Friend WithEvents txtJournal As RichTextBox
    Friend WithEvents lblPath As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents btnExport As Infragistics.Win.Misc.UltraButton
End Class
