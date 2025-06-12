<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        components = New ComponentModel.Container()
        splitContainer1 = New SplitContainer()
        leftPanel = New Panel()
        grpFileSelection = New GroupBox()
        treeView1 = New TreeView()
        contextMenuStrip1 = New ContextMenuStrip(components)
        selectFileToolStripMenuItem = New ToolStripMenuItem()
        selectFolderToolStripMenuItem = New ToolStripMenuItem()
        pnlTreeActions = New Panel()
        btnCollapseAll = New Button()
        btnExpandAll = New Button()
        btnRefreshTree = New Button()
        rightPanel = New Panel()
        grpCombineActions = New GroupBox()
        pnlCombineControls = New Panel()
        progressBar1 = New ProgressBar()
        lblTokenCount = New Label()
        btnCombine = New Button()
        grpProjectSettings = New GroupBox()
        splitContainer2 = New SplitContainer()
        pnlProjectTop = New Panel()
        tlpProjectTop = New TableLayoutPanel()
        lblProjectType = New Label()
        cmbProjectType = New ComboBox()
        lblProjectTitle = New Label()
        txtProjectTitle = New TextBox()
        lblProjectInstructions = New Label()
        txtProjectInstructions = New TextBox()
        pnlProjectBottom = New Panel()
        lblOtherInstructions = New Label()
        txtOtherInstructions = New TextBox()
        grpTemplateManager = New GroupBox()
        tlpTemplateControls = New TableLayoutPanel()
        lblTemplateName = New Label()
        txtTemplateName = New TextBox()
        btnSaveTemplate = New Button()
        lblTemplate = New Label()
        cmbTemplate = New ComboBox()
        btnLoadTemplate = New Button()
        btnCopyTemplate = New Button()
        btnUpdateTemplate = New Button()
        menuStrip1 = New MenuStrip()
        fileToolStripMenuItem = New ToolStripMenuItem()
        selectProjectFolderToolStripMenuItem = New ToolStripMenuItem()
        toolStripSeparator2 = New ToolStripSeparator()
        backupProjectToolStripMenuItem = New ToolStripMenuItem()
        goToOutputToolStripMenuItem = New ToolStripMenuItem()
        toolStripSeparator3 = New ToolStripSeparator()
        makeShortcutToolStripMenuItem = New ToolStripMenuItem()
        toolStripSeparator1 = New ToolStripSeparator()
        exitToolStripMenuItem = New ToolStripMenuItem()
        settingsToolStripMenuItem = New ToolStripMenuItem()
        applicationSettingsToolStripMenuItem = New ToolStripMenuItem()
        helpToolStripMenuItem = New ToolStripMenuItem()
        aboutToolStripMenuItem = New ToolStripMenuItem()
        statusStrip1 = New StatusStrip()
        toolStripStatusLabel1 = New ToolStripStatusLabel()
        toolStripProgressBar1 = New ToolStripProgressBar()
        toolStripStatusLabelFiles = New ToolStripStatusLabel()
        folderBrowserDialog1 = New FolderBrowserDialog()
        openFileDialog1 = New OpenFileDialog()
        saveFileDialog1 = New SaveFileDialog()
        OpenRepoSQLToolStripMenuItem = New ToolStripMenuItem()
        CType(splitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        splitContainer1.Panel1.SuspendLayout()
        splitContainer1.Panel2.SuspendLayout()
        splitContainer1.SuspendLayout()
        leftPanel.SuspendLayout()
        grpFileSelection.SuspendLayout()
        contextMenuStrip1.SuspendLayout()
        pnlTreeActions.SuspendLayout()
        rightPanel.SuspendLayout()
        grpCombineActions.SuspendLayout()
        pnlCombineControls.SuspendLayout()
        grpProjectSettings.SuspendLayout()
        CType(splitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        splitContainer2.Panel1.SuspendLayout()
        splitContainer2.Panel2.SuspendLayout()
        splitContainer2.SuspendLayout()
        pnlProjectTop.SuspendLayout()
        tlpProjectTop.SuspendLayout()
        pnlProjectBottom.SuspendLayout()
        grpTemplateManager.SuspendLayout()
        tlpTemplateControls.SuspendLayout()
        menuStrip1.SuspendLayout()
        statusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' splitContainer1
        ' 
        splitContainer1.Dock = DockStyle.Fill
        splitContainer1.Location = New Point(0, 24)
        splitContainer1.Name = "splitContainer1"
        ' 
        ' splitContainer1.Panel1
        ' 
        splitContainer1.Panel1.Controls.Add(leftPanel)
        ' 
        ' splitContainer1.Panel2
        ' 
        splitContainer1.Panel2.Controls.Add(rightPanel)
        splitContainer1.Size = New Size(1066, 591)
        splitContainer1.SplitterDistance = 279
        splitContainer1.SplitterWidth = 6
        splitContainer1.TabIndex = 0
        ' 
        ' leftPanel
        ' 
        leftPanel.Controls.Add(grpFileSelection)
        leftPanel.Dock = DockStyle.Fill
        leftPanel.Location = New Point(0, 0)
        leftPanel.Name = "leftPanel"
        leftPanel.Padding = New Padding(8)
        leftPanel.Size = New Size(279, 591)
        leftPanel.TabIndex = 0
        ' 
        ' grpFileSelection
        ' 
        grpFileSelection.Controls.Add(treeView1)
        grpFileSelection.Controls.Add(pnlTreeActions)
        grpFileSelection.Dock = DockStyle.Fill
        grpFileSelection.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpFileSelection.Location = New Point(8, 8)
        grpFileSelection.Name = "grpFileSelection"
        grpFileSelection.Padding = New Padding(8)
        grpFileSelection.Size = New Size(263, 575)
        grpFileSelection.TabIndex = 0
        grpFileSelection.TabStop = False
        grpFileSelection.Text = "File Selection"
        ' 
        ' treeView1
        ' 
        treeView1.CheckBoxes = True
        treeView1.ContextMenuStrip = contextMenuStrip1
        treeView1.Dock = DockStyle.Fill
        treeView1.Font = New Font("Segoe UI", 9F)
        treeView1.FullRowSelect = True
        treeView1.HideSelection = False
        treeView1.Location = New Point(8, 26)
        treeView1.Name = "treeView1"
        treeView1.Size = New Size(247, 501)
        treeView1.TabIndex = 0
        ' 
        ' contextMenuStrip1
        ' 
        contextMenuStrip1.Items.AddRange(New ToolStripItem() {selectFileToolStripMenuItem, selectFolderToolStripMenuItem})
        contextMenuStrip1.Name = "contextMenuStrip1"
        contextMenuStrip1.Size = New Size(142, 48)
        ' 
        ' selectFileToolStripMenuItem
        ' 
        selectFileToolStripMenuItem.Name = "selectFileToolStripMenuItem"
        selectFileToolStripMenuItem.Size = New Size(141, 22)
        selectFileToolStripMenuItem.Text = "Select File(s)"
        ' 
        ' selectFolderToolStripMenuItem
        ' 
        selectFolderToolStripMenuItem.Name = "selectFolderToolStripMenuItem"
        selectFolderToolStripMenuItem.Size = New Size(141, 22)
        selectFolderToolStripMenuItem.Text = "Select Folder"
        ' 
        ' pnlTreeActions
        ' 
        pnlTreeActions.Controls.Add(btnCollapseAll)
        pnlTreeActions.Controls.Add(btnExpandAll)
        pnlTreeActions.Controls.Add(btnRefreshTree)
        pnlTreeActions.Dock = DockStyle.Bottom
        pnlTreeActions.Location = New Point(8, 527)
        pnlTreeActions.Name = "pnlTreeActions"
        pnlTreeActions.Size = New Size(247, 40)
        pnlTreeActions.TabIndex = 1
        ' 
        ' btnCollapseAll
        ' 
        btnCollapseAll.BackColor = Color.FromArgb(CByte(255), CByte(193), CByte(7))
        btnCollapseAll.FlatStyle = FlatStyle.Flat
        btnCollapseAll.Font = New Font("Segoe UI", 9F)
        btnCollapseAll.ForeColor = Color.Black
        btnCollapseAll.Location = New Point(148, 7)
        btnCollapseAll.Name = "btnCollapseAll"
        btnCollapseAll.Size = New Size(70, 28)
        btnCollapseAll.TabIndex = 4
        btnCollapseAll.Text = "Collapse"
        btnCollapseAll.UseVisualStyleBackColor = False
        ' 
        ' btnExpandAll
        ' 
        btnExpandAll.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnExpandAll.FlatStyle = FlatStyle.Flat
        btnExpandAll.Font = New Font("Segoe UI", 9F)
        btnExpandAll.ForeColor = Color.White
        btnExpandAll.Location = New Point(74, 7)
        btnExpandAll.Name = "btnExpandAll"
        btnExpandAll.Size = New Size(70, 28)
        btnExpandAll.TabIndex = 3
        btnExpandAll.Text = "Expand"
        btnExpandAll.UseVisualStyleBackColor = False
        ' 
        ' btnRefreshTree
        ' 
        btnRefreshTree.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnRefreshTree.FlatStyle = FlatStyle.Flat
        btnRefreshTree.Font = New Font("Segoe UI", 9F)
        btnRefreshTree.ForeColor = Color.White
        btnRefreshTree.Location = New Point(0, 7)
        btnRefreshTree.Name = "btnRefreshTree"
        btnRefreshTree.Size = New Size(70, 28)
        btnRefreshTree.TabIndex = 2
        btnRefreshTree.Text = "Refresh"
        btnRefreshTree.UseVisualStyleBackColor = False
        ' 
        ' rightPanel
        ' 
        rightPanel.Controls.Add(grpCombineActions)
        rightPanel.Controls.Add(grpProjectSettings)
        rightPanel.Controls.Add(grpTemplateManager)
        rightPanel.Dock = DockStyle.Fill
        rightPanel.Location = New Point(0, 0)
        rightPanel.Name = "rightPanel"
        rightPanel.Padding = New Padding(4, 8, 8, 8)
        rightPanel.Size = New Size(781, 591)
        rightPanel.TabIndex = 0
        ' 
        ' grpCombineActions
        ' 
        grpCombineActions.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        grpCombineActions.Controls.Add(pnlCombineControls)
        grpCombineActions.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpCombineActions.Location = New Point(4, 486)
        grpCombineActions.Name = "grpCombineActions"
        grpCombineActions.Padding = New Padding(8)
        grpCombineActions.Size = New Size(769, 97)
        grpCombineActions.TabIndex = 2
        grpCombineActions.TabStop = False
        grpCombineActions.Text = "Combine Files"
        ' 
        ' pnlCombineControls
        ' 
        pnlCombineControls.Controls.Add(progressBar1)
        pnlCombineControls.Controls.Add(lblTokenCount)
        pnlCombineControls.Controls.Add(btnCombine)
        pnlCombineControls.Dock = DockStyle.Fill
        pnlCombineControls.Font = New Font("Segoe UI", 9F)
        pnlCombineControls.Location = New Point(8, 26)
        pnlCombineControls.Name = "pnlCombineControls"
        pnlCombineControls.Size = New Size(753, 63)
        pnlCombineControls.TabIndex = 0
        ' 
        ' progressBar1
        ' 
        progressBar1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        progressBar1.Location = New Point(224, 31)
        progressBar1.Name = "progressBar1"
        progressBar1.Size = New Size(518, 20)
        progressBar1.Style = ProgressBarStyle.Continuous
        progressBar1.TabIndex = 5
        progressBar1.Visible = False
        ' 
        ' lblTokenCount
        ' 
        lblTokenCount.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        lblTokenCount.AutoSize = True
        lblTokenCount.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblTokenCount.ForeColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        lblTokenCount.Location = New Point(219, 8)
        lblTokenCount.Name = "lblTokenCount"
        lblTokenCount.Size = New Size(108, 19)
        lblTokenCount.TabIndex = 4
        lblTokenCount.Text = "Token Count: 0"
        ' 
        ' btnCombine
        ' 
        btnCombine.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnCombine.FlatStyle = FlatStyle.Flat
        btnCombine.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        btnCombine.ForeColor = Color.White
        btnCombine.Location = New Point(8, 8)
        btnCombine.Name = "btnCombine"
        btnCombine.Size = New Size(200, 45)
        btnCombine.TabIndex = 3
        btnCombine.Text = "🔗 Combine Files"
        btnCombine.UseVisualStyleBackColor = False
        ' 
        ' grpProjectSettings
        ' 
        grpProjectSettings.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        grpProjectSettings.Controls.Add(splitContainer2)
        grpProjectSettings.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpProjectSettings.Location = New Point(4, 128)
        grpProjectSettings.Name = "grpProjectSettings"
        grpProjectSettings.Padding = New Padding(8)
        grpProjectSettings.Size = New Size(769, 355)
        grpProjectSettings.TabIndex = 1
        grpProjectSettings.TabStop = False
        grpProjectSettings.Text = "Project Configuration"
        ' 
        ' splitContainer2
        ' 
        splitContainer2.Dock = DockStyle.Fill
        splitContainer2.Location = New Point(8, 26)
        splitContainer2.Name = "splitContainer2"
        splitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' splitContainer2.Panel1
        ' 
        splitContainer2.Panel1.Controls.Add(pnlProjectTop)
        ' 
        ' splitContainer2.Panel2
        ' 
        splitContainer2.Panel2.Controls.Add(pnlProjectBottom)
        splitContainer2.Size = New Size(753, 321)
        splitContainer2.SplitterDistance = 286
        splitContainer2.TabIndex = 0
        ' 
        ' pnlProjectTop
        ' 
        pnlProjectTop.Controls.Add(tlpProjectTop)
        pnlProjectTop.Dock = DockStyle.Fill
        pnlProjectTop.Location = New Point(0, 0)
        pnlProjectTop.Name = "pnlProjectTop"
        pnlProjectTop.Size = New Size(753, 286)
        pnlProjectTop.TabIndex = 0
        ' 
        ' tlpProjectTop
        ' 
        tlpProjectTop.ColumnCount = 2
        tlpProjectTop.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130F))
        tlpProjectTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        tlpProjectTop.Controls.Add(lblProjectType, 0, 0)
        tlpProjectTop.Controls.Add(cmbProjectType, 1, 0)
        tlpProjectTop.Controls.Add(lblProjectTitle, 0, 1)
        tlpProjectTop.Controls.Add(txtProjectTitle, 1, 1)
        tlpProjectTop.Controls.Add(lblProjectInstructions, 0, 2)
        tlpProjectTop.Controls.Add(txtProjectInstructions, 1, 2)
        tlpProjectTop.Dock = DockStyle.Fill
        tlpProjectTop.Font = New Font("Segoe UI", 9F)
        tlpProjectTop.Location = New Point(0, 0)
        tlpProjectTop.Name = "tlpProjectTop"
        tlpProjectTop.RowCount = 3
        tlpProjectTop.RowStyles.Add(New RowStyle(SizeType.Absolute, 35F))
        tlpProjectTop.RowStyles.Add(New RowStyle(SizeType.Absolute, 35F))
        tlpProjectTop.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        tlpProjectTop.Size = New Size(753, 286)
        tlpProjectTop.TabIndex = 0
        ' 
        ' lblProjectType
        ' 
        lblProjectType.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblProjectType.AutoSize = True
        lblProjectType.Location = New Point(3, 10)
        lblProjectType.Name = "lblProjectType"
        lblProjectType.Size = New Size(124, 15)
        lblProjectType.TabIndex = 0
        lblProjectType.Text = "Project Type:"
        ' 
        ' cmbProjectType
        ' 
        cmbProjectType.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        cmbProjectType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProjectType.FormattingEnabled = True
        cmbProjectType.Location = New Point(133, 6)
        cmbProjectType.Name = "cmbProjectType"
        cmbProjectType.Size = New Size(617, 23)
        cmbProjectType.TabIndex = 1
        ' 
        ' lblProjectTitle
        ' 
        lblProjectTitle.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblProjectTitle.AutoSize = True
        lblProjectTitle.Location = New Point(3, 45)
        lblProjectTitle.Name = "lblProjectTitle"
        lblProjectTitle.Size = New Size(124, 15)
        lblProjectTitle.TabIndex = 2
        lblProjectTitle.Text = "Project Title:"
        ' 
        ' txtProjectTitle
        ' 
        txtProjectTitle.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtProjectTitle.Location = New Point(133, 41)
        txtProjectTitle.Name = "txtProjectTitle"
        txtProjectTitle.Size = New Size(617, 23)
        txtProjectTitle.TabIndex = 3
        ' 
        ' lblProjectInstructions
        ' 
        lblProjectInstructions.AutoSize = True
        lblProjectInstructions.Location = New Point(3, 70)
        lblProjectInstructions.Name = "lblProjectInstructions"
        lblProjectInstructions.Size = New Size(112, 15)
        lblProjectInstructions.TabIndex = 4
        lblProjectInstructions.Text = "Project Instructions:"
        ' 
        ' txtProjectInstructions
        ' 
        txtProjectInstructions.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtProjectInstructions.Font = New Font("Consolas", 9F)
        txtProjectInstructions.Location = New Point(133, 73)
        txtProjectInstructions.Multiline = True
        txtProjectInstructions.Name = "txtProjectInstructions"
        txtProjectInstructions.ScrollBars = ScrollBars.Vertical
        txtProjectInstructions.Size = New Size(617, 210)
        txtProjectInstructions.TabIndex = 5
        ' 
        ' pnlProjectBottom
        ' 
        pnlProjectBottom.Controls.Add(lblOtherInstructions)
        pnlProjectBottom.Controls.Add(txtOtherInstructions)
        pnlProjectBottom.Dock = DockStyle.Fill
        pnlProjectBottom.Location = New Point(0, 0)
        pnlProjectBottom.Name = "pnlProjectBottom"
        pnlProjectBottom.Size = New Size(753, 31)
        pnlProjectBottom.TabIndex = 0
        ' 
        ' lblOtherInstructions
        ' 
        lblOtherInstructions.AutoSize = True
        lblOtherInstructions.Font = New Font("Segoe UI", 9F)
        lblOtherInstructions.Location = New Point(3, 8)
        lblOtherInstructions.Name = "lblOtherInstructions"
        lblOtherInstructions.Size = New Size(105, 15)
        lblOtherInstructions.TabIndex = 6
        lblOtherInstructions.Text = "Other Instructions:"
        ' 
        ' txtOtherInstructions
        ' 
        txtOtherInstructions.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtOtherInstructions.Font = New Font("Consolas", 9F)
        txtOtherInstructions.Location = New Point(133, 6)
        txtOtherInstructions.Multiline = True
        txtOtherInstructions.Name = "txtOtherInstructions"
        txtOtherInstructions.ScrollBars = ScrollBars.Vertical
        txtOtherInstructions.Size = New Size(617, 22)
        txtOtherInstructions.TabIndex = 7
        ' 
        ' grpTemplateManager
        ' 
        grpTemplateManager.Controls.Add(tlpTemplateControls)
        grpTemplateManager.Dock = DockStyle.Top
        grpTemplateManager.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpTemplateManager.Location = New Point(4, 8)
        grpTemplateManager.Name = "grpTemplateManager"
        grpTemplateManager.Padding = New Padding(8)
        grpTemplateManager.Size = New Size(769, 120)
        grpTemplateManager.TabIndex = 0
        grpTemplateManager.TabStop = False
        grpTemplateManager.Text = "Template Manager"
        ' 
        ' tlpTemplateControls
        ' 
        tlpTemplateControls.ColumnCount = 4
        tlpTemplateControls.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 131F))
        tlpTemplateControls.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 76.31579F))
        tlpTemplateControls.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 12.2291021F))
        tlpTemplateControls.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 11.4551086F))
        tlpTemplateControls.Controls.Add(lblTemplateName, 0, 0)
        tlpTemplateControls.Controls.Add(txtTemplateName, 1, 0)
        tlpTemplateControls.Controls.Add(btnSaveTemplate, 2, 0)
        tlpTemplateControls.Controls.Add(lblTemplate, 0, 1)
        tlpTemplateControls.Controls.Add(cmbTemplate, 1, 1)
        tlpTemplateControls.Controls.Add(btnLoadTemplate, 2, 1)
        tlpTemplateControls.Controls.Add(btnCopyTemplate, 3, 1)
        tlpTemplateControls.Controls.Add(btnUpdateTemplate, 3, 0)
        tlpTemplateControls.Dock = DockStyle.Fill
        tlpTemplateControls.Font = New Font("Segoe UI", 9F)
        tlpTemplateControls.Location = New Point(8, 26)
        tlpTemplateControls.Name = "tlpTemplateControls"
        tlpTemplateControls.RowCount = 2
        tlpTemplateControls.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        tlpTemplateControls.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        tlpTemplateControls.Size = New Size(753, 86)
        tlpTemplateControls.TabIndex = 0
        ' 
        ' lblTemplateName
        ' 
        lblTemplateName.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblTemplateName.AutoSize = True
        lblTemplateName.Location = New Point(3, 14)
        lblTemplateName.Name = "lblTemplateName"
        lblTemplateName.Size = New Size(125, 15)
        lblTemplateName.TabIndex = 0
        lblTemplateName.Text = "Template Name:"
        ' 
        ' txtTemplateName
        ' 
        txtTemplateName.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtTemplateName.Location = New Point(134, 10)
        txtTemplateName.Name = "txtTemplateName"
        txtTemplateName.Size = New Size(468, 23)
        txtTemplateName.TabIndex = 1
        ' 
        ' btnSaveTemplate
        ' 
        btnSaveTemplate.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        btnSaveTemplate.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnSaveTemplate.FlatStyle = FlatStyle.Flat
        btnSaveTemplate.ForeColor = Color.White
        btnSaveTemplate.Location = New Point(608, 3)
        btnSaveTemplate.Name = "btnSaveTemplate"
        btnSaveTemplate.Size = New Size(70, 37)
        btnSaveTemplate.TabIndex = 2
        btnSaveTemplate.Text = "Save"
        btnSaveTemplate.UseVisualStyleBackColor = False
        ' 
        ' lblTemplate
        ' 
        lblTemplate.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblTemplate.AutoSize = True
        lblTemplate.Location = New Point(3, 57)
        lblTemplate.Name = "lblTemplate"
        lblTemplate.Size = New Size(125, 15)
        lblTemplate.TabIndex = 3
        lblTemplate.Text = "Select Template:"
        ' 
        ' cmbTemplate
        ' 
        cmbTemplate.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        cmbTemplate.DropDownStyle = ComboBoxStyle.DropDownList
        cmbTemplate.FormattingEnabled = True
        cmbTemplate.Location = New Point(134, 53)
        cmbTemplate.Name = "cmbTemplate"
        cmbTemplate.Size = New Size(468, 23)
        cmbTemplate.TabIndex = 4
        ' 
        ' btnLoadTemplate
        ' 
        btnLoadTemplate.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        btnLoadTemplate.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnLoadTemplate.FlatStyle = FlatStyle.Flat
        btnLoadTemplate.ForeColor = Color.White
        btnLoadTemplate.Location = New Point(608, 46)
        btnLoadTemplate.Name = "btnLoadTemplate"
        btnLoadTemplate.Size = New Size(70, 37)
        btnLoadTemplate.TabIndex = 5
        btnLoadTemplate.Text = "Load"
        btnLoadTemplate.UseVisualStyleBackColor = False
        ' 
        ' btnCopyTemplate
        ' 
        btnCopyTemplate.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        btnCopyTemplate.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnCopyTemplate.FlatStyle = FlatStyle.Flat
        btnCopyTemplate.ForeColor = Color.White
        btnCopyTemplate.Location = New Point(684, 46)
        btnCopyTemplate.Name = "btnCopyTemplate"
        btnCopyTemplate.Size = New Size(66, 37)
        btnCopyTemplate.TabIndex = 6
        btnCopyTemplate.Text = "Copy"
        btnCopyTemplate.UseVisualStyleBackColor = False
        ' 
        ' btnUpdateTemplate
        ' 
        btnUpdateTemplate.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        btnUpdateTemplate.BackColor = Color.FromArgb(CByte(255), CByte(193), CByte(7))
        btnUpdateTemplate.FlatStyle = FlatStyle.Flat
        btnUpdateTemplate.ForeColor = Color.Black
        btnUpdateTemplate.Location = New Point(684, 3)
        btnUpdateTemplate.Name = "btnUpdateTemplate"
        btnUpdateTemplate.Size = New Size(66, 37)
        btnUpdateTemplate.TabIndex = 7
        btnUpdateTemplate.Text = "Update"
        btnUpdateTemplate.UseVisualStyleBackColor = False
        ' 
        ' menuStrip1
        ' 
        menuStrip1.Items.AddRange(New ToolStripItem() {fileToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem})
        menuStrip1.Location = New Point(0, 0)
        menuStrip1.Name = "menuStrip1"
        menuStrip1.Size = New Size(1066, 24)
        menuStrip1.TabIndex = 1
        ' 
        ' fileToolStripMenuItem
        ' 
        fileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {selectProjectFolderToolStripMenuItem, toolStripSeparator2, backupProjectToolStripMenuItem, goToOutputToolStripMenuItem, OpenRepoSQLToolStripMenuItem, toolStripSeparator3, makeShortcutToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem})
        fileToolStripMenuItem.Name = "fileToolStripMenuItem"
        fileToolStripMenuItem.Size = New Size(37, 20)
        fileToolStripMenuItem.Text = "&File"
        ' 
        ' selectProjectFolderToolStripMenuItem
        ' 
        selectProjectFolderToolStripMenuItem.Name = "selectProjectFolderToolStripMenuItem"
        selectProjectFolderToolStripMenuItem.Size = New Size(181, 22)
        selectProjectFolderToolStripMenuItem.Text = "Select Project &Folder"
        ' 
        ' toolStripSeparator2
        ' 
        toolStripSeparator2.Name = "toolStripSeparator2"
        toolStripSeparator2.Size = New Size(178, 6)
        ' 
        ' backupProjectToolStripMenuItem
        ' 
        backupProjectToolStripMenuItem.Name = "backupProjectToolStripMenuItem"
        backupProjectToolStripMenuItem.Size = New Size(181, 22)
        backupProjectToolStripMenuItem.Text = "&Backup Project"
        ' 
        ' goToOutputToolStripMenuItem
        ' 
        goToOutputToolStripMenuItem.Name = "goToOutputToolStripMenuItem"
        goToOutputToolStripMenuItem.Size = New Size(181, 22)
        goToOutputToolStripMenuItem.Text = "&Go to Output"
        ' 
        ' toolStripSeparator3
        ' 
        toolStripSeparator3.Name = "toolStripSeparator3"
        toolStripSeparator3.Size = New Size(178, 6)
        ' 
        ' makeShortcutToolStripMenuItem
        ' 
        makeShortcutToolStripMenuItem.Name = "makeShortcutToolStripMenuItem"
        makeShortcutToolStripMenuItem.Size = New Size(181, 22)
        makeShortcutToolStripMenuItem.Text = "Make &Shortcut"
        ' 
        ' toolStripSeparator1
        ' 
        toolStripSeparator1.Name = "toolStripSeparator1"
        toolStripSeparator1.Size = New Size(178, 6)
        ' 
        ' exitToolStripMenuItem
        ' 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        exitToolStripMenuItem.ShortcutKeys = Keys.Alt Or Keys.F4
        exitToolStripMenuItem.Size = New Size(181, 22)
        exitToolStripMenuItem.Text = "E&xit"
        ' 
        ' settingsToolStripMenuItem
        ' 
        settingsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {applicationSettingsToolStripMenuItem})
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem"
        settingsToolStripMenuItem.Size = New Size(61, 20)
        settingsToolStripMenuItem.Text = "&Settings"
        ' 
        ' applicationSettingsToolStripMenuItem
        ' 
        applicationSettingsToolStripMenuItem.Name = "applicationSettingsToolStripMenuItem"
        applicationSettingsToolStripMenuItem.Size = New Size(180, 22)
        applicationSettingsToolStripMenuItem.Text = "&Application Settings"
        ' 
        ' helpToolStripMenuItem
        ' 
        helpToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {aboutToolStripMenuItem})
        helpToolStripMenuItem.Name = "helpToolStripMenuItem"
        helpToolStripMenuItem.Size = New Size(44, 20)
        helpToolStripMenuItem.Text = "&Help"
        ' 
        ' aboutToolStripMenuItem
        ' 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem"
        aboutToolStripMenuItem.Size = New Size(107, 22)
        aboutToolStripMenuItem.Text = "&About"
        ' 
        ' statusStrip1
        ' 
        statusStrip1.Items.AddRange(New ToolStripItem() {toolStripStatusLabel1, toolStripProgressBar1, toolStripStatusLabelFiles})
        statusStrip1.Location = New Point(0, 615)
        statusStrip1.Name = "statusStrip1"
        statusStrip1.Padding = New Padding(1, 0, 16, 0)
        statusStrip1.Size = New Size(1066, 22)
        statusStrip1.TabIndex = 2
        ' 
        ' toolStripStatusLabel1
        ' 
        toolStripStatusLabel1.Name = "toolStripStatusLabel1"
        toolStripStatusLabel1.Size = New Size(39, 17)
        toolStripStatusLabel1.Text = "Ready"
        ' 
        ' toolStripProgressBar1
        ' 
        toolStripProgressBar1.Name = "toolStripProgressBar1"
        toolStripProgressBar1.Size = New Size(100, 16)
        toolStripProgressBar1.Visible = False
        ' 
        ' toolStripStatusLabelFiles
        ' 
        toolStripStatusLabelFiles.Name = "toolStripStatusLabelFiles"
        toolStripStatusLabelFiles.Size = New Size(100, 17)
        toolStripStatusLabelFiles.Text = "Files: 0 | Size: 0 KB"
        ' 
        ' folderBrowserDialog1
        ' 
        folderBrowserDialog1.Description = "Select project folder"
        ' 
        ' openFileDialog1
        ' 
        openFileDialog1.Filter = "All Files (*.*)|*.*|VB Files (*.vb)|*.vb|C# Files (*.cs)|*.cs|Web Files (*.html;*.css;*.js)|*.html;*.css;*.js"
        openFileDialog1.Multiselect = True
        openFileDialog1.Title = "Select files to add"
        ' 
        ' saveFileDialog1
        ' 
        saveFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        saveFileDialog1.Title = "Save combined files to"
        ' 
        ' OpenRepoSQLToolStripMenuItem
        ' 
        OpenRepoSQLToolStripMenuItem.Name = "OpenRepoSQLToolStripMenuItem"
        OpenRepoSQLToolStripMenuItem.Size = New Size(181, 22)
        OpenRepoSQLToolStripMenuItem.Text = "Open RepoSQL"
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(250))
        ClientSize = New Size(1066, 637)
        Controls.Add(splitContainer1)
        Controls.Add(statusStrip1)
        Controls.Add(menuStrip1)
        Font = New Font("Segoe UI", 9F)
        MainMenuStrip = menuStrip1
        MinimumSize = New Size(1000, 600)
        Name = "frmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "RepoBundle - Project File Combiner v2.0"
        splitContainer1.Panel1.ResumeLayout(False)
        splitContainer1.Panel2.ResumeLayout(False)
        CType(splitContainer1, ComponentModel.ISupportInitialize).EndInit()
        splitContainer1.ResumeLayout(False)
        leftPanel.ResumeLayout(False)
        grpFileSelection.ResumeLayout(False)
        contextMenuStrip1.ResumeLayout(False)
        pnlTreeActions.ResumeLayout(False)
        rightPanel.ResumeLayout(False)
        grpCombineActions.ResumeLayout(False)
        pnlCombineControls.ResumeLayout(False)
        pnlCombineControls.PerformLayout()
        grpProjectSettings.ResumeLayout(False)
        splitContainer2.Panel1.ResumeLayout(False)
        splitContainer2.Panel2.ResumeLayout(False)
        CType(splitContainer2, ComponentModel.ISupportInitialize).EndInit()
        splitContainer2.ResumeLayout(False)
        pnlProjectTop.ResumeLayout(False)
        tlpProjectTop.ResumeLayout(False)
        tlpProjectTop.PerformLayout()
        pnlProjectBottom.ResumeLayout(False)
        pnlProjectBottom.PerformLayout()
        grpTemplateManager.ResumeLayout(False)
        tlpTemplateControls.ResumeLayout(False)
        tlpTemplateControls.PerformLayout()
        menuStrip1.ResumeLayout(False)
        menuStrip1.PerformLayout()
        statusStrip1.ResumeLayout(False)
        statusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    ' Control declarations
    Friend WithEvents splitContainer1 As SplitContainer
    Friend WithEvents leftPanel As Panel
    Friend WithEvents rightPanel As Panel

    ' File Selection Group
    Friend WithEvents grpFileSelection As GroupBox
    Friend WithEvents treeView1 As TreeView
    Friend WithEvents contextMenuStrip1 As ContextMenuStrip
    Friend WithEvents selectFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents selectFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlTreeActions As Panel
    Friend WithEvents btnRefreshTree As Button
    Friend WithEvents btnExpandAll As Button
    Friend WithEvents btnCollapseAll As Button

    ' Template Manager Group
    Friend WithEvents grpTemplateManager As GroupBox
    Friend WithEvents tlpTemplateControls As TableLayoutPanel
    Friend WithEvents lblTemplateName As Label
    Friend WithEvents txtTemplateName As TextBox
    Friend WithEvents btnSaveTemplate As Button
    Friend WithEvents lblTemplate As Label
    Friend WithEvents cmbTemplate As ComboBox
    Friend WithEvents btnLoadTemplate As Button
    Friend WithEvents btnCopyTemplate As Button
    Friend WithEvents btnUpdateTemplate As Button

    ' Project Settings Group with SplitContainer
    Friend WithEvents grpProjectSettings As GroupBox
    Friend WithEvents splitContainer2 As SplitContainer
    Friend WithEvents pnlProjectTop As Panel
    Friend WithEvents tlpProjectTop As TableLayoutPanel
    Friend WithEvents lblProjectType As Label
    Friend WithEvents cmbProjectType As ComboBox
    Friend WithEvents lblProjectTitle As Label
    Friend WithEvents txtProjectTitle As TextBox
    Friend WithEvents lblProjectInstructions As Label
    Friend WithEvents txtProjectInstructions As TextBox
    Friend WithEvents pnlProjectBottom As Panel
    Friend WithEvents lblOtherInstructions As Label
    Friend WithEvents txtOtherInstructions As TextBox

    ' Combine Actions Group
    Friend WithEvents grpCombineActions As GroupBox
    Friend WithEvents pnlCombineControls As Panel
    Friend WithEvents btnCombine As Button
    Friend WithEvents lblTokenCount As Label
    Friend WithEvents progressBar1 As ProgressBar

    ' Menu and Status
    Friend WithEvents menuStrip1 As MenuStrip
    Friend WithEvents fileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents selectProjectFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents toolStripSeparator2 As ToolStripSeparator
    Friend WithEvents backupProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents goToOutputToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents toolStripSeparator3 As ToolStripSeparator
    Friend WithEvents makeShortcutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents toolStripSeparator1 As ToolStripSeparator
    Friend WithEvents exitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents settingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents applicationSettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents helpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents aboutToolStripMenuItem As ToolStripMenuItem

    Friend WithEvents statusStrip1 As StatusStrip
    Friend WithEvents toolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents toolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents toolStripStatusLabelFiles As ToolStripStatusLabel

    ' Dialogs
    Friend WithEvents folderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents openFileDialog1 As OpenFileDialog
    Friend WithEvents saveFileDialog1 As SaveFileDialog
    Friend WithEvents OpenRepoSQLToolStripMenuItem As ToolStripMenuItem

End Class