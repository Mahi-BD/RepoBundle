<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSettings
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
        grpFolders = New GroupBox()
        btnBrowseProject = New Button()
        btnBrowseOutput = New Button()
        txtProjectFolder = New TextBox()
        txtOutputFolder = New TextBox()
        lblProjectFolder = New Label()
        lblOutputFolder = New Label()
        grpExcludeFolders = New GroupBox()
        btnResetExcluded = New Button()
        btnRemoveExcluded = New Button()
        btnAddExcluded = New Button()
        txtNewExcludedFolder = New TextBox()
        lstExcludedFolders = New ListBox()
        lblExcludedFolders = New Label()
        lblNewExcludedFolder = New Label()
        grpDatabase = New GroupBox()
        chkIncludeDatabase = New CheckBox()
        lblDatabaseFiles = New Label()
        lstDatabaseFiles = New ListBox()
        btnBrowseDatabase = New Button()
        btnRemoveSelected = New Button()
        btnClearAll = New Button()
        grpFileExtensions = New GroupBox()
        lblProjectType = New Label()
        cmbProjectTypeSettings = New ComboBox()
        lstExtensions = New ListBox()
        lblNewExtension = New Label()
        txtNewExtension = New TextBox()
        btnAddExtension = New Button()
        btnRemoveExtension = New Button()
        lblExtensionHelp = New Label()
        btnResetExtensions = New Button()
        btnOK = New Button()
        btnCancel = New Button()
        folderBrowserDialog1 = New FolderBrowserDialog()
        openFileDialog1 = New OpenFileDialog()
        grpFolders.SuspendLayout()
        grpExcludeFolders.SuspendLayout()
        grpDatabase.SuspendLayout()
        grpFileExtensions.SuspendLayout()
        SuspendLayout()
        ' 
        ' grpFolders
        ' 
        grpFolders.Controls.Add(btnBrowseProject)
        grpFolders.Controls.Add(btnBrowseOutput)
        grpFolders.Controls.Add(txtProjectFolder)
        grpFolders.Controls.Add(txtOutputFolder)
        grpFolders.Controls.Add(lblProjectFolder)
        grpFolders.Controls.Add(lblOutputFolder)
        grpFolders.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpFolders.Location = New Point(12, 13)
        grpFolders.Name = "grpFolders"
        grpFolders.Padding = New Padding(8)
        grpFolders.Size = New Size(440, 120)
        grpFolders.TabIndex = 0
        grpFolders.TabStop = False
        grpFolders.Text = "Folder Settings"
        ' 
        ' btnBrowseProject
        ' 
        btnBrowseProject.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnBrowseProject.FlatStyle = FlatStyle.Flat
        btnBrowseProject.Font = New Font("Segoe UI", 9F)
        btnBrowseProject.ForeColor = Color.White
        btnBrowseProject.Location = New Point(356, 29)
        btnBrowseProject.Name = "btnBrowseProject"
        btnBrowseProject.Size = New Size(70, 28)
        btnBrowseProject.TabIndex = 2
        btnBrowseProject.Text = "Browse..."
        btnBrowseProject.UseVisualStyleBackColor = False
        ' 
        ' btnBrowseOutput
        ' 
        btnBrowseOutput.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnBrowseOutput.FlatStyle = FlatStyle.Flat
        btnBrowseOutput.Font = New Font("Segoe UI", 9F)
        btnBrowseOutput.ForeColor = Color.White
        btnBrowseOutput.Location = New Point(356, 69)
        btnBrowseOutput.Name = "btnBrowseOutput"
        btnBrowseOutput.Size = New Size(70, 28)
        btnBrowseOutput.TabIndex = 5
        btnBrowseOutput.Text = "Browse..."
        btnBrowseOutput.UseVisualStyleBackColor = False
        ' 
        ' txtProjectFolder
        ' 
        txtProjectFolder.Font = New Font("Segoe UI", 9F)
        txtProjectFolder.Location = New Point(120, 32)
        txtProjectFolder.Name = "txtProjectFolder"
        txtProjectFolder.Size = New Size(226, 23)
        txtProjectFolder.TabIndex = 1
        ' 
        ' txtOutputFolder
        ' 
        txtOutputFolder.Font = New Font("Segoe UI", 9F)
        txtOutputFolder.Location = New Point(120, 72)
        txtOutputFolder.Name = "txtOutputFolder"
        txtOutputFolder.Size = New Size(226, 23)
        txtOutputFolder.TabIndex = 4
        ' 
        ' lblProjectFolder
        ' 
        lblProjectFolder.AutoSize = True
        lblProjectFolder.Font = New Font("Segoe UI", 9F)
        lblProjectFolder.Location = New Point(20, 35)
        lblProjectFolder.Name = "lblProjectFolder"
        lblProjectFolder.Size = New Size(83, 15)
        lblProjectFolder.TabIndex = 0
        lblProjectFolder.Text = "Project Folder:"
        ' 
        ' lblOutputFolder
        ' 
        lblOutputFolder.AutoSize = True
        lblOutputFolder.Font = New Font("Segoe UI", 9F)
        lblOutputFolder.Location = New Point(20, 75)
        lblOutputFolder.Name = "lblOutputFolder"
        lblOutputFolder.Size = New Size(84, 15)
        lblOutputFolder.TabIndex = 3
        lblOutputFolder.Text = "Output Folder:"
        ' 
        ' grpExcludeFolders
        ' 
        grpExcludeFolders.Controls.Add(btnResetExcluded)
        grpExcludeFolders.Controls.Add(btnRemoveExcluded)
        grpExcludeFolders.Controls.Add(btnAddExcluded)
        grpExcludeFolders.Controls.Add(txtNewExcludedFolder)
        grpExcludeFolders.Controls.Add(lstExcludedFolders)
        grpExcludeFolders.Controls.Add(lblExcludedFolders)
        grpExcludeFolders.Controls.Add(lblNewExcludedFolder)
        grpExcludeFolders.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpExcludeFolders.Location = New Point(474, 13)
        grpExcludeFolders.Name = "grpExcludeFolders"
        grpExcludeFolders.Padding = New Padding(8)
        grpExcludeFolders.Size = New Size(440, 245)
        grpExcludeFolders.TabIndex = 1
        grpExcludeFolders.TabStop = False
        grpExcludeFolders.Text = "Excluded Folders (for Backup)"
        ' 
        ' btnResetExcluded
        ' 
        btnResetExcluded.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnResetExcluded.FlatStyle = FlatStyle.Flat
        btnResetExcluded.Font = New Font("Segoe UI", 9F)
        btnResetExcluded.ForeColor = Color.White
        btnResetExcluded.Location = New Point(355, 91)
        btnResetExcluded.Name = "btnResetExcluded"
        btnResetExcluded.Size = New Size(70, 30)
        btnResetExcluded.TabIndex = 6
        btnResetExcluded.Text = "Reset"
        btnResetExcluded.UseVisualStyleBackColor = False
        ' 
        ' btnRemoveExcluded
        ' 
        btnRemoveExcluded.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnRemoveExcluded.FlatStyle = FlatStyle.Flat
        btnRemoveExcluded.Font = New Font("Segoe UI", 9F)
        btnRemoveExcluded.ForeColor = Color.White
        btnRemoveExcluded.Location = New Point(355, 127)
        btnRemoveExcluded.Name = "btnRemoveExcluded"
        btnRemoveExcluded.Size = New Size(70, 30)
        btnRemoveExcluded.TabIndex = 5
        btnRemoveExcluded.Text = "Remove"
        btnRemoveExcluded.UseVisualStyleBackColor = False
        ' 
        ' btnAddExcluded
        ' 
        btnAddExcluded.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnAddExcluded.FlatStyle = FlatStyle.Flat
        btnAddExcluded.Font = New Font("Segoe UI", 9F)
        btnAddExcluded.ForeColor = Color.White
        btnAddExcluded.Location = New Point(355, 55)
        btnAddExcluded.Name = "btnAddExcluded"
        btnAddExcluded.Size = New Size(70, 30)
        btnAddExcluded.TabIndex = 4
        btnAddExcluded.Text = "Add"
        btnAddExcluded.UseVisualStyleBackColor = False
        ' 
        ' txtNewExcludedFolder
        ' 
        txtNewExcludedFolder.Font = New Font("Segoe UI", 9F)
        txtNewExcludedFolder.Location = New Point(118, 202)
        txtNewExcludedFolder.Name = "txtNewExcludedFolder"
        txtNewExcludedFolder.PlaceholderText = "e.g. node_modules"
        txtNewExcludedFolder.Size = New Size(225, 23)
        txtNewExcludedFolder.TabIndex = 3
        ' 
        ' lstExcludedFolders
        ' 
        lstExcludedFolders.Font = New Font("Segoe UI", 9F)
        lstExcludedFolders.FormattingEnabled = True
        lstExcludedFolders.ItemHeight = 15
        lstExcludedFolders.Location = New Point(18, 55)
        lstExcludedFolders.Name = "lstExcludedFolders"
        lstExcludedFolders.Size = New Size(325, 139)
        lstExcludedFolders.TabIndex = 1
        ' 
        ' lblExcludedFolders
        ' 
        lblExcludedFolders.AutoSize = True
        lblExcludedFolders.Font = New Font("Segoe UI", 9F)
        lblExcludedFolders.Location = New Point(18, 30)
        lblExcludedFolders.Name = "lblExcludedFolders"
        lblExcludedFolders.Size = New Size(177, 15)
        lblExcludedFolders.TabIndex = 0
        lblExcludedFolders.Text = "Folders to exclude from backup:"
        ' 
        ' lblNewExcludedFolder
        ' 
        lblNewExcludedFolder.AutoSize = True
        lblNewExcludedFolder.Font = New Font("Segoe UI", 9F)
        lblNewExcludedFolder.Location = New Point(18, 205)
        lblNewExcludedFolder.Name = "lblNewExcludedFolder"
        lblNewExcludedFolder.Size = New Size(91, 15)
        lblNewExcludedFolder.TabIndex = 2
        lblNewExcludedFolder.Text = "Add new folder:"
        ' 
        ' grpDatabase
        ' 
        grpDatabase.Controls.Add(chkIncludeDatabase)
        grpDatabase.Controls.Add(lblDatabaseFiles)
        grpDatabase.Controls.Add(lstDatabaseFiles)
        grpDatabase.Controls.Add(btnBrowseDatabase)
        grpDatabase.Controls.Add(btnRemoveSelected)
        grpDatabase.Controls.Add(btnClearAll)
        grpDatabase.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpDatabase.Location = New Point(474, 279)
        grpDatabase.Name = "grpDatabase"
        grpDatabase.Padding = New Padding(8)
        grpDatabase.Size = New Size(440, 187)
        grpDatabase.TabIndex = 2
        grpDatabase.TabStop = False
        grpDatabase.Text = "Database Settings"
        ' 
        ' chkIncludeDatabase
        ' 
        chkIncludeDatabase.AutoSize = True
        chkIncludeDatabase.Font = New Font("Segoe UI", 9F)
        chkIncludeDatabase.Location = New Point(20, 30)
        chkIncludeDatabase.Name = "chkIncludeDatabase"
        chkIncludeDatabase.Size = New Size(166, 19)
        chkIncludeDatabase.TabIndex = 0
        chkIncludeDatabase.Text = "Include SQL Database Files"
        chkIncludeDatabase.UseVisualStyleBackColor = True
        ' 
        ' lblDatabaseFiles
        ' 
        lblDatabaseFiles.AutoSize = True
        lblDatabaseFiles.Font = New Font("Segoe UI", 9F)
        lblDatabaseFiles.Location = New Point(222, 31)
        lblDatabaseFiles.Name = "lblDatabaseFiles"
        lblDatabaseFiles.Size = New Size(121, 15)
        lblDatabaseFiles.TabIndex = 1
        lblDatabaseFiles.Text = "Selected SQL Files (0):"
        ' 
        ' lstDatabaseFiles
        ' 
        lstDatabaseFiles.Font = New Font("Segoe UI", 9F)
        lstDatabaseFiles.FormattingEnabled = True
        lstDatabaseFiles.ItemHeight = 15
        lstDatabaseFiles.Location = New Point(20, 60)
        lstDatabaseFiles.Name = "lstDatabaseFiles"
        lstDatabaseFiles.Size = New Size(323, 109)
        lstDatabaseFiles.TabIndex = 2
        ' 
        ' btnBrowseDatabase
        ' 
        btnBrowseDatabase.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnBrowseDatabase.FlatStyle = FlatStyle.Flat
        btnBrowseDatabase.Font = New Font("Segoe UI", 9F)
        btnBrowseDatabase.ForeColor = Color.White
        btnBrowseDatabase.Location = New Point(355, 65)
        btnBrowseDatabase.Name = "btnBrowseDatabase"
        btnBrowseDatabase.Size = New Size(70, 30)
        btnBrowseDatabase.TabIndex = 3
        btnBrowseDatabase.Text = "Add SQL..."
        btnBrowseDatabase.UseVisualStyleBackColor = False
        ' 
        ' btnRemoveSelected
        ' 
        btnRemoveSelected.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnRemoveSelected.FlatStyle = FlatStyle.Flat
        btnRemoveSelected.Font = New Font("Segoe UI", 9F)
        btnRemoveSelected.ForeColor = Color.White
        btnRemoveSelected.Location = New Point(355, 101)
        btnRemoveSelected.Name = "btnRemoveSelected"
        btnRemoveSelected.Size = New Size(70, 30)
        btnRemoveSelected.TabIndex = 4
        btnRemoveSelected.Text = "Remove"
        btnRemoveSelected.UseVisualStyleBackColor = False
        ' 
        ' btnClearAll
        ' 
        btnClearAll.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnClearAll.FlatStyle = FlatStyle.Flat
        btnClearAll.Font = New Font("Segoe UI", 9F)
        btnClearAll.ForeColor = Color.White
        btnClearAll.Location = New Point(355, 139)
        btnClearAll.Name = "btnClearAll"
        btnClearAll.Size = New Size(70, 30)
        btnClearAll.TabIndex = 5
        btnClearAll.Text = "Clear All"
        btnClearAll.UseVisualStyleBackColor = False
        ' 
        ' grpFileExtensions
        ' 
        grpFileExtensions.Controls.Add(lblProjectType)
        grpFileExtensions.Controls.Add(cmbProjectTypeSettings)
        grpFileExtensions.Controls.Add(lstExtensions)
        grpFileExtensions.Controls.Add(lblNewExtension)
        grpFileExtensions.Controls.Add(txtNewExtension)
        grpFileExtensions.Controls.Add(btnAddExtension)
        grpFileExtensions.Controls.Add(btnRemoveExtension)
        grpFileExtensions.Controls.Add(lblExtensionHelp)
        grpFileExtensions.Controls.Add(btnResetExtensions)
        grpFileExtensions.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpFileExtensions.Location = New Point(12, 145)
        grpFileExtensions.Name = "grpFileExtensions"
        grpFileExtensions.Padding = New Padding(8)
        grpFileExtensions.Size = New Size(440, 321)
        grpFileExtensions.TabIndex = 3
        grpFileExtensions.TabStop = False
        grpFileExtensions.Text = "File Extension Management"
        ' 
        ' lblProjectType
        ' 
        lblProjectType.AutoSize = True
        lblProjectType.Font = New Font("Segoe UI", 9F)
        lblProjectType.Location = New Point(20, 35)
        lblProjectType.Name = "lblProjectType"
        lblProjectType.Size = New Size(74, 15)
        lblProjectType.TabIndex = 0
        lblProjectType.Text = "Project Type:"
        ' 
        ' cmbProjectTypeSettings
        ' 
        cmbProjectTypeSettings.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProjectTypeSettings.Font = New Font("Segoe UI", 9F)
        cmbProjectTypeSettings.FormattingEnabled = True
        cmbProjectTypeSettings.Location = New Point(120, 32)
        cmbProjectTypeSettings.Name = "cmbProjectTypeSettings"
        cmbProjectTypeSettings.Size = New Size(302, 23)
        cmbProjectTypeSettings.TabIndex = 1
        ' 
        ' lstExtensions
        ' 
        lstExtensions.Font = New Font("Consolas", 9F)
        lstExtensions.FormattingEnabled = True
        lstExtensions.ItemHeight = 14
        lstExtensions.Location = New Point(120, 68)
        lstExtensions.Name = "lstExtensions"
        lstExtensions.Size = New Size(302, 186)
        lstExtensions.TabIndex = 3
        ' 
        ' lblNewExtension
        ' 
        lblNewExtension.AutoSize = True
        lblNewExtension.Font = New Font("Segoe UI", 9F)
        lblNewExtension.Location = New Point(18, 268)
        lblNewExtension.Name = "lblNewExtension"
        lblNewExtension.Size = New Size(86, 15)
        lblNewExtension.TabIndex = 4
        lblNewExtension.Text = "Add Extension:"
        ' 
        ' txtNewExtension
        ' 
        txtNewExtension.Font = New Font("Consolas", 9F)
        txtNewExtension.Location = New Point(120, 265)
        txtNewExtension.Name = "txtNewExtension"
        txtNewExtension.PlaceholderText = "*.css or site.css"
        txtNewExtension.Size = New Size(302, 22)
        txtNewExtension.TabIndex = 5
        ' 
        ' btnAddExtension
        ' 
        btnAddExtension.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnAddExtension.FlatStyle = FlatStyle.Flat
        btnAddExtension.Font = New Font("Segoe UI", 9F)
        btnAddExtension.ForeColor = Color.White
        btnAddExtension.Location = New Point(30, 70)
        btnAddExtension.Name = "btnAddExtension"
        btnAddExtension.Size = New Size(70, 30)
        btnAddExtension.TabIndex = 6
        btnAddExtension.Text = "Add"
        btnAddExtension.UseVisualStyleBackColor = False
        ' 
        ' btnRemoveExtension
        ' 
        btnRemoveExtension.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnRemoveExtension.FlatStyle = FlatStyle.Flat
        btnRemoveExtension.Font = New Font("Segoe UI", 9F)
        btnRemoveExtension.ForeColor = Color.White
        btnRemoveExtension.Location = New Point(30, 106)
        btnRemoveExtension.Name = "btnRemoveExtension"
        btnRemoveExtension.Size = New Size(70, 30)
        btnRemoveExtension.TabIndex = 7
        btnRemoveExtension.Text = "Remove"
        btnRemoveExtension.UseVisualStyleBackColor = False
        ' 
        ' lblExtensionHelp
        ' 
        lblExtensionHelp.AutoSize = True
        lblExtensionHelp.Font = New Font("Segoe UI", 8.25F, FontStyle.Italic)
        lblExtensionHelp.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        lblExtensionHelp.Location = New Point(20, 293)
        lblExtensionHelp.Name = "lblExtensionHelp"
        lblExtensionHelp.Size = New Size(264, 13)
        lblExtensionHelp.TabIndex = 9
        lblExtensionHelp.Text = "Use * for wildcard (*.css) or specific files (site.css, app.js)"
        ' 
        ' btnResetExtensions
        ' 
        btnResetExtensions.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnResetExtensions.FlatStyle = FlatStyle.Flat
        btnResetExtensions.Font = New Font("Segoe UI", 9F)
        btnResetExtensions.ForeColor = Color.White
        btnResetExtensions.Location = New Point(30, 142)
        btnResetExtensions.Name = "btnResetExtensions"
        btnResetExtensions.Size = New Size(70, 30)
        btnResetExtensions.TabIndex = 8
        btnResetExtensions.Text = "Reset"
        btnResetExtensions.UseVisualStyleBackColor = False
        ' 
        ' btnOK
        ' 
        btnOK.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnOK.FlatStyle = FlatStyle.Flat
        btnOK.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        btnOK.ForeColor = Color.White
        btnOK.Location = New Point(744, 481)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(80, 35)
        btnOK.TabIndex = 4
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = False
        ' 
        ' btnCancel
        ' 
        btnCancel.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        btnCancel.ForeColor = Color.White
        btnCancel.Location = New Point(834, 481)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(80, 35)
        btnCancel.TabIndex = 5
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' folderBrowserDialog1
        ' 
        folderBrowserDialog1.Description = "Select folder"
        ' 
        ' openFileDialog1
        ' 
        openFileDialog1.Filter = "SQL Script Files|*.sql|All Files|*.*"
        openFileDialog1.Multiselect = True
        openFileDialog1.Title = "Select SQL Database Files"
        ' 
        ' frmSettings
        ' 
        AcceptButton = btnOK
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(250))
        CancelButton = btnCancel
        ClientSize = New Size(926, 527)
        Controls.Add(btnCancel)
        Controls.Add(btnOK)
        Controls.Add(grpFileExtensions)
        Controls.Add(grpDatabase)
        Controls.Add(grpExcludeFolders)
        Controls.Add(grpFolders)
        Font = New Font("Segoe UI", 9F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSettings"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Application Settings - RepoBundle"
        grpFolders.ResumeLayout(False)
        grpFolders.PerformLayout()
        grpExcludeFolders.ResumeLayout(False)
        grpExcludeFolders.PerformLayout()
        grpDatabase.ResumeLayout(False)
        grpDatabase.PerformLayout()
        grpFileExtensions.ResumeLayout(False)
        grpFileExtensions.PerformLayout()
        ResumeLayout(False)

    End Sub

    ' Control declarations for Folder Settings
    Friend WithEvents grpFolders As GroupBox
    Friend WithEvents btnBrowseProject As Button
    Friend WithEvents btnBrowseOutput As Button
    Friend WithEvents txtProjectFolder As TextBox
    Friend WithEvents txtOutputFolder As TextBox
    Friend WithEvents lblProjectFolder As Label
    Friend WithEvents lblOutputFolder As Label

    ' Control declarations for Excluded Folders
    Friend WithEvents grpExcludeFolders As GroupBox
    Friend WithEvents btnResetExcluded As Button
    Friend WithEvents btnRemoveExcluded As Button
    Friend WithEvents btnAddExcluded As Button
    Friend WithEvents txtNewExcludedFolder As TextBox
    Friend WithEvents lstExcludedFolders As ListBox
    Friend WithEvents lblExcludedFolders As Label
    Friend WithEvents lblNewExcludedFolder As Label

    ' Control declarations for Database Settings
    Friend WithEvents grpDatabase As GroupBox
    Friend WithEvents btnClearAll As Button
    Friend WithEvents btnRemoveSelected As Button
    Friend WithEvents btnBrowseDatabase As Button
    Friend WithEvents lstDatabaseFiles As ListBox
    Friend WithEvents lblDatabaseFiles As Label
    Friend WithEvents chkIncludeDatabase As CheckBox

    ' Control declarations for File Extensions
    Friend WithEvents grpFileExtensions As GroupBox
    Friend WithEvents lblProjectType As Label
    Friend WithEvents cmbProjectTypeSettings As ComboBox
    Friend WithEvents lstExtensions As ListBox
    Friend WithEvents lblNewExtension As Label
    Friend WithEvents txtNewExtension As TextBox
    Friend WithEvents btnAddExtension As Button
    Friend WithEvents btnRemoveExtension As Button
    Friend WithEvents btnResetExtensions As Button
    Friend WithEvents lblExtensionHelp As Label

    ' Control declarations for Form Actions
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents folderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents openFileDialog1 As OpenFileDialog

End Class