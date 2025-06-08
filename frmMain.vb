Imports System.IO
Imports System.Text
Imports System.Linq
Imports System.IO.Compression
Imports System.Diagnostics

Public Class frmMain
    Private iniHelper As IniHelper
    Private templateIni As IniHelper
    Private projectFolder As String = ""
    Private outputFolder As String = ""
    Private databaseFiles As New List(Of String)
    Private excludedFolders As New List(Of String) ' Added for dynamic excluded folders
    Private includeDatabase As Boolean = False
    Private lastSelectedTemplate As String = ""
    Private lastSelectedProjectType As String = ""
    Private projectTitle As String = ""
    Private projectInstructions As String = ""
    Private otherInstructions As String = ""
    Private isUpdatingNodes As Boolean = False
    Private isLoadingTemplate As Boolean = False
    Private vbDesktopExtensions As New List(Of String)
    Private aspCoreExtensions As New List(Of String)
    Private aspMvcExtensions As New List(Of String)
    Private configSaveTimer As System.Windows.Forms.Timer = Nothing
    Private Sub InitializeApplication()
        iniHelper = New IniHelper(Path.Combine(Application.StartupPath, "config.ini"))
        templateIni = New IniHelper(Path.Combine(Application.StartupPath, "template.ini"))

        ' Initialize project types
        cmbProjectType.Items.AddRange({"Visual Basic Desktop", "Asp MVC 5", "Asp Dotnet Core 8"})

        ' Load configuration FIRST
        LoadConfiguration()
        LoadTemplates()

        ' Set default output folder if not configured
        If String.IsNullOrEmpty(outputFolder) Then
            outputFolder = Path.Combine(Application.StartupPath, "Output")
        End If

        If Not Directory.Exists(outputFolder) Then
            Try
                Directory.CreateDirectory(outputFolder)
                toolStripStatusLabel1.Text = "Created default output folder: " & outputFolder
            Catch ex As Exception
                toolStripStatusLabel1.Text = "Warning: Could not create default output folder"
            End Try
        End If

        ' Restore last selected project type
        If Not String.IsNullOrEmpty(lastSelectedProjectType) Then
            For i As Integer = 0 To cmbProjectType.Items.Count - 1
                If cmbProjectType.Items(i).ToString().Equals(lastSelectedProjectType, StringComparison.OrdinalIgnoreCase) Then
                    cmbProjectType.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            cmbProjectType.SelectedIndex = 0
        End If

        ' Load project folder if configured
        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            LoadProjectFolder()
        End If

        ' Set initial button states
        btnLoadTemplate.Enabled = (cmbTemplate.Items.Count > 0 AndAlso cmbTemplate.SelectedItem IsNot Nothing)
        btnCopyTemplate.Enabled = (cmbTemplate.Items.Count > 0 AndAlso cmbTemplate.SelectedItem IsNot Nothing)
        btnUpdateTemplate.Enabled = (cmbTemplate.Items.Count > 0 AndAlso cmbTemplate.SelectedItem IsNot Nothing)

        ' Add event handlers for auto-save with proper timing
        AddHandler txtProjectTitle.TextChanged, AddressOf DelayedConfigurationSave
        AddHandler txtProjectInstructions.TextChanged, AddressOf DelayedConfigurationSave
        AddHandler txtOtherInstructions.TextChanged, AddressOf DelayedConfigurationSave

        ' Add event handlers for split container position saving
        AddHandler splitContainer1.SplitterMoved, AddressOf SplitContainer_SplitterMoved
        AddHandler splitContainer2.SplitterMoved, AddressOf SplitContainer_SplitterMoved

        ' Set status
        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Ready - Select project folder to begin"
        Else
            toolStripStatusLabel1.Text = "Ready - Project folder loaded: " & Path.GetFileName(projectFolder)
        End If
    End Sub


    Private Sub DelayedConfigurationSave(sender As Object, e As EventArgs)
        ' Cancel previous timer if it exists
        If configSaveTimer IsNot Nothing Then
            configSaveTimer.Stop()
            configSaveTimer.Dispose()
        End If

        ' Create new timer for delayed save
        configSaveTimer = New System.Windows.Forms.Timer()
        configSaveTimer.Interval = 2000 ' 2 second delay
        AddHandler configSaveTimer.Tick, Sub()
                                             SaveConfiguration()
                                             configSaveTimer.Stop()
                                             configSaveTimer.Dispose()
                                             configSaveTimer = Nothing
                                         End Sub
        configSaveTimer.Start()
    End Sub

    ' === IMPROVED LOAD CONFIGURATION ===
    Private Sub LoadConfiguration()
        ' Load main configuration
        projectFolder = iniHelper.ReadValue("Main", "ProjectFolder", "")
        outputFolder = iniHelper.ReadValue("Main", "OutputFolder", "")
        includeDatabase = Boolean.Parse(iniHelper.ReadValue("Main", "IncludeDatabase", "False"))
        lastSelectedTemplate = iniHelper.ReadValue("Main", "LastSelectedTemplate", "")
        lastSelectedProjectType = iniHelper.ReadValue("Main", "LastSelectedProjectType", "")

        ' Load excluded folders
        Dim excludedFoldersString As String = iniHelper.ReadValue("Backup", "ExcludedFolders", ".git,.vs,.svn,bin,obj")
        excludedFolders.Clear()
        If Not String.IsNullOrWhiteSpace(excludedFoldersString) Then
            Dim folders() As String = excludedFoldersString.Split(New Char() {","c, ";"c}, StringSplitOptions.RemoveEmptyEntries)
            For Each folder In folders
                Dim trimmedFolder As String = folder.Trim()
                If Not String.IsNullOrWhiteSpace(trimmedFolder) Then
                    excludedFolders.Add(trimmedFolder)
                End If
            Next
        End If

        ' Ensure we have some excluded folders
        If excludedFolders.Count = 0 Then
            excludedFolders.AddRange({".git", ".vs", ".svn", "bin", "obj"})
        End If

        ' Load project information
        projectTitle = iniHelper.ReadValue("Project", "Title", "")
        projectInstructions = iniHelper.ReadValue("Project", "Instructions", "").Replace("\n", vbCrLf)
        otherInstructions = iniHelper.ReadValue("Project", "OtherInstructions", "").Replace("\n", vbCrLf)

        ' Load file extensions
        LoadFileExtensions()

        ' Load split container positions with error handling
        Try
            Dim splitter1Pos As String = iniHelper.ReadValue("UI", "SplitContainer1Position", "379")
            Dim splitter2Pos As String = iniHelper.ReadValue("UI", "SplitContainer2Position", "189")

            Dim pos1 As Integer = Integer.Parse(splitter1Pos)
            Dim pos2 As Integer = Integer.Parse(splitter2Pos)

            ' Validate positions are within reasonable bounds
            If pos1 > 100 And pos1 < Me.Width - 100 Then
                splitContainer1.SplitterDistance = pos1
            End If

            If pos2 > 50 And pos2 < splitContainer2.Height - 50 Then
                splitContainer2.SplitterDistance = pos2
            End If

        Catch ex As Exception
            ' Use defaults if parsing fails
            splitContainer1.SplitterDistance = 379
            splitContainer2.SplitterDistance = 189
        End Try

        ' Load database files
        databaseFiles.Clear()
        Dim dbFileKeys As List(Of String) = iniHelper.GetKeys("DatabaseFiles")
        For Each key In dbFileKeys
            Dim dbFile As String = iniHelper.ReadValue("DatabaseFiles", key, "")
            If Not String.IsNullOrEmpty(dbFile) AndAlso File.Exists(dbFile) Then
                databaseFiles.Add(dbFile)
            End If
        Next

        ' Update UI
        txtProjectTitle.Text = projectTitle
        txtProjectInstructions.Text = projectInstructions
        txtOtherInstructions.Text = otherInstructions
    End Sub

    Private Sub LoadFileExtensions()
        ' Load VB Desktop extensions
        Dim vbExtString As String = iniHelper.ReadValue("Extensions", "VBDesktop", "*.vb,*.designer.vb,*.vbproj,*.resx,*.config,*.sql")
        vbDesktopExtensions.Clear()
        vbDesktopExtensions.AddRange(vbExtString.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) Not String.IsNullOrEmpty(x)))

        ' Load ASP.NET Core extensions with regex support
        Dim coreExtString As String = iniHelper.ReadValue("Extensions", "AspCore", "*.cs,^_.*\.cshtml$,site.css,site.js,*.json,*.sql")
        aspCoreExtensions.Clear()
        aspCoreExtensions.AddRange(coreExtString.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) Not String.IsNullOrEmpty(x)))

        ' Load ASP.NET MVC extensions
        Dim mvcExtString As String = iniHelper.ReadValue("Extensions", "AspMvc", "*.cs,*.cshtml,*.css,*.js,*.config,*.sql")
        aspMvcExtensions.Clear()
        aspMvcExtensions.AddRange(mvcExtString.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) Not String.IsNullOrEmpty(x)))
    End Sub

    Private Sub SaveFileExtensions()
        ' Save VB Desktop extensions
        iniHelper.WriteValue("Extensions", "VBDesktop", String.Join(",", vbDesktopExtensions))

        ' Save ASP.NET Core extensions
        iniHelper.WriteValue("Extensions", "AspCore", String.Join(",", aspCoreExtensions))

        ' Save ASP.NET MVC extensions
        iniHelper.WriteValue("Extensions", "AspMvc", String.Join(",", aspMvcExtensions))
    End Sub

    ' === IMPROVED CONFIGURATION SAVING ===
    Private Sub SaveConfiguration()
        Try
            ' Save main configuration
            iniHelper.WriteValue("Main", "ProjectFolder", projectFolder)
            iniHelper.WriteValue("Main", "OutputFolder", outputFolder)
            iniHelper.WriteValue("Main", "IncludeDatabase", includeDatabase.ToString())
            iniHelper.WriteValue("Main", "LastSelectedTemplate", If(cmbTemplate.SelectedItem?.ToString(), ""))
            iniHelper.WriteValue("Main", "LastSelectedProjectType", If(cmbProjectType.SelectedItem?.ToString(), ""))

            ' Save excluded folders
            iniHelper.WriteValue("Backup", "ExcludedFolders", String.Join(",", excludedFolders))

            ' Save project information
            iniHelper.WriteValue("Project", "Title", txtProjectTitle.Text.Trim())
            iniHelper.WriteValue("Project", "Instructions", txtProjectInstructions.Text.Replace(vbCrLf, "\n"))
            iniHelper.WriteValue("Project", "OtherInstructions", txtOtherInstructions.Text.Replace(vbCrLf, "\n"))

            ' Save file extensions
            SaveFileExtensions()

            ' Save split container positions (call the dedicated method)
            SaveSplitContainerPositions()

            ' Clear and save database files
            Dim dbFileKeys As List(Of String) = iniHelper.GetKeys("DatabaseFiles")
            For Each key In dbFileKeys
                iniHelper.DeleteKey("DatabaseFiles", key)
            Next

            For i As Integer = 0 To databaseFiles.Count - 1
                iniHelper.WriteValue("DatabaseFiles", "File" & (i + 1).ToString(), databaseFiles(i))
            Next

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error saving configuration: " & ex.Message
        End Try
    End Sub


    Private Sub SplitContainer_SplitterMoved(sender As Object, e As SplitterEventArgs)
        ' Auto-save split container positions with proper delay handling
        Static lastSaveTimer As System.Windows.Forms.Timer = Nothing

        ' Cancel previous timer if it exists
        If lastSaveTimer IsNot Nothing Then
            lastSaveTimer.Stop()
            lastSaveTimer.Dispose()
        End If

        ' Create new timer for delayed save
        lastSaveTimer = New System.Windows.Forms.Timer()
        lastSaveTimer.Interval = 1000 ' 1 second delay
        AddHandler lastSaveTimer.Tick, Sub()
                                           SaveSplitContainerPositions()
                                           lastSaveTimer.Stop()
                                           lastSaveTimer.Dispose()
                                           lastSaveTimer = Nothing
                                       End Sub
        lastSaveTimer.Start()
    End Sub

    Private Sub SaveSplitContainerPositions()
        Try
            ' Save split container positions
            iniHelper.WriteValue("UI", "SplitContainer1Position", splitContainer1.SplitterDistance.ToString())
            iniHelper.WriteValue("UI", "SplitContainer2Position", splitContainer2.SplitterDistance.ToString())
        Catch ex As Exception
            ' Ignore errors during saving
        End Try
    End Sub

    Private Sub ConfigurationChanged(sender As Object, e As EventArgs)
        ' Auto-save configuration when text boxes change (with delay to avoid constant saving)
        Static lastSave As DateTime = DateTime.MinValue
        If DateTime.Now.Subtract(lastSave).TotalSeconds > 2 Then
            SaveConfiguration()
            lastSave = DateTime.Now
        End If
    End Sub

    Private Sub LoadTemplates()
        ' Store current selection before clearing
        Dim currentSelection As String = ""
        If cmbTemplate.SelectedItem IsNot Nothing Then
            currentSelection = cmbTemplate.SelectedItem.ToString()
        End If

        cmbTemplate.Items.Clear()

        Try
            ' Get all template sections
            Dim sections As List(Of String) = templateIni.GetSections()

            For Each section In sections
                If section.StartsWith("Template_") Then
                    Dim templateName As String = templateIni.ReadValue(section, "Name", "")
                    If Not String.IsNullOrWhiteSpace(templateName) Then
                        cmbTemplate.Items.Add(templateName)
                    End If
                End If
            Next

            ' Restore previous selection if it exists
            If Not String.IsNullOrEmpty(currentSelection) Then
                For i As Integer = 0 To cmbTemplate.Items.Count - 1
                    If cmbTemplate.Items(i).ToString().Equals(currentSelection, StringComparison.OrdinalIgnoreCase) Then
                        cmbTemplate.SelectedIndex = i
                        Return
                    End If
                Next
            End If

            ' Try to restore last selected template from config
            If Not String.IsNullOrEmpty(lastSelectedTemplate) Then
                For i As Integer = 0 To cmbTemplate.Items.Count - 1
                    If cmbTemplate.Items(i).ToString().Equals(lastSelectedTemplate, StringComparison.OrdinalIgnoreCase) Then
                        cmbTemplate.SelectedIndex = i
                        Return
                    End If
                Next
            End If

            ' If no previous selection found, select first template if available
            If cmbTemplate.Items.Count > 0 Then
                cmbTemplate.SelectedIndex = 0
            End If

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error loading templates: " & ex.Message
        End Try
    End Sub

    Private Sub LoadProjectFolder()
        If Directory.Exists(projectFolder) Then
            treeView1.Nodes.Clear()
            treeView1.BeginUpdate()

            Try
                Dim rootNode As TreeNode = New TreeNode(Path.GetFileName(projectFolder))
                rootNode.Tag = projectFolder
                rootNode.ImageIndex = 0
                treeView1.Nodes.Add(rootNode)
                LoadDirectoryNodes(rootNode, projectFolder)

                toolStripStatusLabel1.Text = "Project folder loaded: " & projectFolder & " (" & CountTotalNodes(rootNode) & " items)"
            Catch ex As Exception
                toolStripStatusLabel1.Text = "Error loading project folder: " & ex.Message
            Finally
                treeView1.EndUpdate()
            End Try
        Else
            treeView1.Nodes.Clear()
            toolStripStatusLabel1.Text = "Project folder not found: " & projectFolder
        End If
    End Sub

    Private Function CountTotalNodes(parentNode As TreeNode) As Integer
        Dim count As Integer = parentNode.Nodes.Count
        For Each childNode As TreeNode In parentNode.Nodes
            count += CountTotalNodes(childNode)
        Next
        Return count
    End Function

    Private Sub LoadDirectoryNodes(parentNode As TreeNode, directoryPath As String)
        Try
            ' Add directories
            Dim directories() As String = Directory.GetDirectories(directoryPath)
            For Each directory As String In directories
                Dim dirNode As TreeNode = New TreeNode(Path.GetFileName(directory))
                dirNode.Tag = directory
                parentNode.Nodes.Add(dirNode)
                LoadDirectoryNodes(dirNode, directory)
            Next

            ' Add files
            Dim files() As String = Directory.GetFiles(directoryPath)
            For Each file As String In files
                Dim fileNode As TreeNode = New TreeNode(Path.GetFileName(file))
                fileNode.Tag = file
                parentNode.Nodes.Add(fileNode)
            Next
        Catch ex As UnauthorizedAccessException
            ' Skip directories we can't access
        End Try
    End Sub

    ' === EXPAND/COLLAPSE FUNCTIONALITY ===
    Private Sub btnExpandAll_Click(sender As Object, e As EventArgs) Handles btnExpandAll.Click
        If treeView1.Nodes.Count = 0 Then
            toolStripStatusLabel1.Text = "No tree loaded to expand"
            Return
        End If

        Try
            treeView1.BeginUpdate()
            ExpandAllNodes(treeView1.Nodes)
            toolStripStatusLabel1.Text = "All tree nodes expanded"
        Finally
            treeView1.EndUpdate()
        End Try
    End Sub

    Private Sub btnCollapseAll_Click(sender As Object, e As EventArgs) Handles btnCollapseAll.Click
        If treeView1.Nodes.Count = 0 Then
            toolStripStatusLabel1.Text = "No tree loaded to collapse"
            Return
        End If

        Try
            treeView1.BeginUpdate()
            CollapseAllNodes(treeView1.Nodes)
            toolStripStatusLabel1.Text = "All tree nodes collapsed"
        Finally
            treeView1.EndUpdate()
        End Try
    End Sub

    Private Sub ExpandAllNodes(nodes As TreeNodeCollection)
        For Each node As TreeNode In nodes
            node.Expand()
            If node.Nodes.Count > 0 Then
                ExpandAllNodes(node.Nodes)
            End If
        Next
    End Sub

    Private Sub CollapseAllNodes(nodes As TreeNodeCollection)
        For Each node As TreeNode In nodes
            node.Collapse()
            If node.Nodes.Count > 0 Then
                CollapseAllNodes(node.Nodes)
            End If
        Next
    End Sub

    Private Sub btnRefreshTree_Click(sender As Object, e As EventArgs) Handles btnRefreshTree.Click
        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first"
            Return
        End If

        RefreshTreeView()
        toolStripStatusLabel1.Text = "Tree view refreshed successfully"
    End Sub

    Private Sub RefreshTreeView()
        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            ' Show progress for tree refresh
            ShowProgress()

            Try
                ' Step 1: Store current state (20%)
                UpdateProgress(20, "Storing current tree state...")
                Dim checkedFiles As List(Of String) = GetCheckedFiles()
                Dim treeState As Dictionary(Of String, Boolean) = GetTreeExpansionState()

                ' Step 2: Reload tree (60%)
                UpdateProgress(60, "Reloading project tree...")
                LoadProjectFolder()

                ' Step 3: Restore state (80%)
                UpdateProgress(80, "Restoring file selections...")

                If checkedFiles.Count > 0 OrElse treeState.Count > 0 Then
                    isUpdatingNodes = True
                    Try
                        ' Restore checked files
                        For Each filePath In checkedFiles
                            If File.Exists(filePath) Then
                                SelectFileInTree(filePath)
                            End If
                        Next

                        ' Restore expansion state
                        If treeState.Count > 0 Then
                            RestoreTreeExpansionState(treeState)
                        End If
                    Finally
                        isUpdatingNodes = False
                    End Try
                    UpdateTokenCount()
                End If

                ' Step 4: Complete (100%)
                UpdateProgress(100, "Tree refresh complete!")
                System.Threading.Thread.Sleep(300)

            Finally
                HideProgress()
            End Try
        End If
    End Sub

    ' === FILE MENU EVENTS ===
    Private Sub selectProjectFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles selectProjectFolderToolStripMenuItem.Click
        folderBrowserDialog1.Description = "Select Project Folder"
        folderBrowserDialog1.ShowNewFolderButton = True

        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            folderBrowserDialog1.SelectedPath = projectFolder
        End If

        If folderBrowserDialog1.ShowDialog = DialogResult.OK Then
            projectFolder = folderBrowserDialog1.SelectedPath
            LoadProjectFolder()
            SaveConfiguration()
            RefreshTreeView()
        End If
    End Sub

    ' === BACKUP FUNCTIONALITY - FIXED VERSION ===
    Private Sub backupProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles backupProjectToolStripMenuItem.Click
        If String.IsNullOrEmpty(projectFolder) Then
            MessageBox.Show("Please select a project folder first.", "Project Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Directory.Exists(projectFolder) Then
            MessageBox.Show("Project folder does not exist: " & projectFolder, "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrEmpty(outputFolder) Then
            MessageBox.Show("Please set an output folder first in Settings.", "Output Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Ensure output folder exists
            If Not Directory.Exists(outputFolder) Then
                Directory.CreateDirectory(outputFolder)
            End If

            ' Generate backup filename
            Dim projectName As String = If(String.IsNullOrWhiteSpace(txtProjectTitle.Text), Path.GetFileName(projectFolder), txtProjectTitle.Text.Trim())
            ' Clean project name for filename
            For Each invalidChar In Path.GetInvalidFileNameChars()
                projectName = projectName.Replace(invalidChar, "_")
            Next

            Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
            Dim zipFileName As String = $"{projectName}_{timestamp}.zip"
            Dim zipFilePath As String = Path.Combine(outputFolder, zipFileName)

            ' Show progress
            ShowProgress()
            btnCombine.Enabled = False

            Try
                ' Create backup with progress
                CreateProjectBackup(projectFolder, zipFilePath)

                ' Verify backup was created successfully
                If File.Exists(zipFilePath) Then
                    Dim fileInfo As New FileInfo(zipFilePath)
                    If fileInfo.Length > 0 Then
                        toolStripStatusLabel1.Text = $"Project backed up successfully: {zipFileName} ({FormatFileSize(fileInfo.Length)})"

                        ' Ask if user wants to open output folder
                        Dim result As DialogResult = MessageBox.Show($"Backup created successfully:{vbCrLf}{zipFilePath}{vbCrLf}Size: {FormatFileSize(fileInfo.Length)}{vbCrLf}{vbCrLf}Would you like to open the output folder?",
                                                                   "Backup Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                        If result = DialogResult.Yes Then
                            goToOutputToolStripMenuItem_Click(sender, e)
                        End If
                    Else
                        MessageBox.Show("Backup file was created but is empty. No files were found to backup.", "Backup Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Else
                    MessageBox.Show("Backup file was not created.", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            Finally
                HideProgress()
                btnCombine.Enabled = True
            End Try

        Catch ex As Exception
            MessageBox.Show($"Error creating backup: {ex.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            toolStripStatusLabel1.Text = "Backup failed: " & ex.Message
        End Try
    End Sub

    ' Main backup creation method
    Private Sub CreateProjectBackup(sourceFolder As String, zipFilePath As String)
        UpdateProgress(5, "Initializing backup process...")

        ' Delete existing zip file if it exists
        If File.Exists(zipFilePath) Then
            File.Delete(zipFilePath)
        End If

        UpdateProgress(10, "Scanning project files...")

        ' Get all files to backup using simple method
        Dim filesToBackup As New List(Of String)

        Try
            ' Simple scan - get all files and then filter
            Dim allFiles() As String = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories)

            For Each filePath In allFiles
                If ShouldIncludeFileInBackup(filePath, sourceFolder) Then
                    filesToBackup.Add(filePath)
                End If
            Next

        Catch ex As Exception
            Throw New Exception($"Error scanning project folder: {ex.Message}")
        End Try

        UpdateProgress(20, $"Found {filesToBackup.Count} files to backup...")

        If filesToBackup.Count = 0 Then
            ' Try to get a file count for debugging
            Dim totalFiles As Integer = 0
            Try
                totalFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories).Length
            Catch
            End Try

            Throw New Exception($"No files found to backup.{vbCrLf}{vbCrLf}" &
                              $"Project folder: {sourceFolder}{vbCrLf}" &
                              $"Total files in folder: {totalFiles}{vbCrLf}" &
                              $"Excluded folders: {String.Join(", ", excludedFolders)}{vbCrLf}{vbCrLf}" &
                              $"Try adjusting excluded folders in Settings.")
        End If

        ' Create zip file
        Using archive As ZipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create)
            Dim totalFiles As Integer = filesToBackup.Count

            For i As Integer = 0 To totalFiles - 1
                Dim filePath As String = filesToBackup(i)
                Dim fileName As String = Path.GetFileName(filePath)
                Dim progress As Integer = 20 + CInt((i + 1) * 70 / totalFiles)

                UpdateProgress(progress, $"Backing up {i + 1}/{totalFiles}: {fileName}")

                Try
                    ' Calculate relative path
                    Dim relativePath As String = GetBackupRelativePath(filePath, sourceFolder)

                    If String.IsNullOrEmpty(relativePath) Then
                        Continue For
                    End If

                    ' Normalize path separators for zip file
                    relativePath = relativePath.Replace("\", "/")

                    ' Create zip entry
                    Dim entry As ZipArchiveEntry = archive.CreateEntry(relativePath, CompressionLevel.Optimal)
                    entry.LastWriteTime = File.GetLastWriteTime(filePath)

                    ' Copy file content
                    Using entryStream As Stream = entry.Open()
                        Using fileStream As FileStream = File.OpenRead(filePath)
                            fileStream.CopyTo(entryStream)
                        End Using
                    End Using

                Catch ex As Exception
                    ' Skip files that can't be read
                    Continue For
                End Try

                ' Update UI periodically
                If i Mod 10 = 0 Then
                    Application.DoEvents()
                End If
            Next
        End Using

        UpdateProgress(95, "Finalizing backup...")

        ' Verify the backup
        If File.Exists(zipFilePath) Then
            Dim fileInfo As New FileInfo(zipFilePath)
            If fileInfo.Length = 0 Then
                Throw New Exception("Backup file was created but is empty")
            End If
            UpdateProgress(100, $"Backup completed: {FormatFileSize(fileInfo.Length)}")
        Else
            Throw New Exception("Backup file was not created")
        End If
    End Sub

    ' Check if a file should be included in backup
    Private Function ShouldIncludeFileInBackup(filePath As String, rootPath As String) As Boolean
        Try
            ' Get file info
            Dim fileInfo As New FileInfo(filePath)

            ' Skip hidden and system files
            If fileInfo.Attributes.HasFlag(FileAttributes.Hidden) OrElse fileInfo.Attributes.HasFlag(FileAttributes.System) Then
                Return False
            End If

            ' Check if file is in an excluded folder
            Dim relativePath As String = GetBackupRelativePath(filePath, rootPath)
            Dim pathParts() As String = relativePath.Split("\"c, "/"c)

            ' Check each part of the path against excluded folders
            For Each part In pathParts
                If excludedFolders.Any(Function(excluded) String.Equals(excluded, part, StringComparison.OrdinalIgnoreCase)) Then
                    Return False
                End If
            Next

            Return True

        Catch
            Return False
        End Try
    End Function

    ' Get relative path for backup
    Private Function GetBackupRelativePath(fullPath As String, basePath As String) As String
        Try
            ' Normalize paths to use consistent separators
            Dim normalizedBase As String = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar)
            Dim normalizedFull As String = Path.GetFullPath(fullPath)

            ' Check if the file is actually under the base path
            If normalizedFull.StartsWith(normalizedBase & Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) Then
                ' Calculate relative path by removing the base path
                Dim relativePath As String = normalizedFull.Substring(normalizedBase.Length + 1)
                Return relativePath
            Else
                ' File is not under base path, return just filename
                Return Path.GetFileName(fullPath)
            End If

        Catch ex As Exception
            ' Fallback to filename only
            Return Path.GetFileName(fullPath)
        End Try
    End Function


    ' Debug method to test backup scanning
    Private Sub TestBackupScanning()
        If String.IsNullOrEmpty(projectFolder) OrElse Not Directory.Exists(projectFolder) Then
            MessageBox.Show("Please select a valid project folder first.", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Show current settings
            Dim excludedList As String = String.Join(", ", excludedFolders)

            ' Count total files
            Dim allFiles() As String = Directory.GetFiles(projectFolder, "*.*", SearchOption.AllDirectories)

            ' Count files that would be included
            Dim filesToBackup As New List(Of String)
            For Each filePath In allFiles
                If ShouldIncludeFileInBackup(filePath, projectFolder) Then
                    filesToBackup.Add(filePath)
                End If
            Next

            ' Show results
            Dim message As String = $"Backup Scan Test Results:{vbCrLf}{vbCrLf}" &
                                   $"Project folder: {projectFolder}{vbCrLf}" &
                                   $"Excluded folders: {excludedList}{vbCrLf}{vbCrLf}" &
                                   $"Total files in project: {allFiles.Length}{vbCrLf}" &
                                   $"Files that would be backed up: {filesToBackup.Count}{vbCrLf}{vbCrLf}"

            If filesToBackup.Count > 0 Then
                message += "Sample files to backup:"
                For i As Integer = 0 To Math.Min(9, filesToBackup.Count - 1)
                    message += $"{vbCrLf}  {Path.GetFileName(filesToBackup(i))}"
                Next
                If filesToBackup.Count > 10 Then
                    message += $"{vbCrLf}  ... and {filesToBackup.Count - 10} more files"
                End If
            Else
                message += "❌ NO FILES WOULD BE BACKED UP!{vbCrLf}{vbCrLf}Check your excluded folders in Settings."
            End If

            MessageBox.Show(message, "Backup Test Results", MessageBoxButtons.OK,
                          If(filesToBackup.Count > 0, MessageBoxIcon.Information, MessageBoxIcon.Warning))

        Catch ex As Exception
            MessageBox.Show($"Error during backup test: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub goToOutputToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles goToOutputToolStripMenuItem.Click
        If String.IsNullOrEmpty(outputFolder) Then
            MessageBox.Show("Please set an output folder first in Settings.", "Output Folder Not Set", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Ensure output folder exists
            If Not Directory.Exists(outputFolder) Then
                Directory.CreateDirectory(outputFolder)
                toolStripStatusLabel1.Text = "Created output folder: " & outputFolder
            End If

            ' Open the output folder in Windows Explorer
            Process.Start("explorer.exe", outputFolder)
            toolStripStatusLabel1.Text = "Opened output folder: " & outputFolder

        Catch ex As Exception
            MessageBox.Show($"Error opening output folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            toolStripStatusLabel1.Text = "Error opening output folder: " & ex.Message
        End Try
    End Sub

    Private Sub makeShortcutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles makeShortcutToolStripMenuItem.Click
        Try
            If String.IsNullOrEmpty(projectFolder) Then
                MessageBox.Show("Please select a project folder first.", "Project Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim exePath As String = Application.ExecutablePath
            Dim shortcutPath As String = Path.Combine(projectFolder, "RepoBundle.lnk")

            ' Create shortcut using Windows Script Host
            Dim shell As Object = CreateObject("WScript.Shell")
            Dim shortcut As Object = shell.CreateShortcut(shortcutPath)
            shortcut.TargetPath = exePath
            shortcut.WorkingDirectory = Path.GetDirectoryName(exePath)
            shortcut.Description = "RepoBundle - Project File Combiner"
            shortcut.Save()

            toolStripStatusLabel1.Text = "Shortcut created: " & shortcutPath
            MessageBox.Show($"Shortcut created successfully:{vbCrLf}{shortcutPath}", "Shortcut Created", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show($"Error creating shortcut: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            toolStripStatusLabel1.Text = "Error creating shortcut: " & ex.Message
        End Try
    End Sub

    Private Sub exitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles exitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    ' === CONTEXT MENU EVENTS ===
    Private Sub selectFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles selectFileToolStripMenuItem.Click
        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim selectedCount As Integer = 0
            For Each fileName In openFileDialog1.FileNames
                SelectFileInTree(fileName)
                selectedCount += 1
            Next
            toolStripStatusLabel1.Text = $"Selected {selectedCount} files from dialog"
        End If
    End Sub

    Private Sub selectFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles selectFolderToolStripMenuItem.Click
        If folderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            SelectFolderInTree(folderBrowserDialog1.SelectedPath)
            toolStripStatusLabel1.Text = "Selected folder: " & Path.GetFileName(folderBrowserDialog1.SelectedPath)
        End If
    End Sub

    ' === TREE VIEW EVENTS ===
    Private Sub treeView1_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles treeView1.AfterCheck
        If isUpdatingNodes Then Return

        isUpdatingNodes = True
        Try
            ' When a folder is checked, check all children
            If e.Node.Nodes.Count > 0 Then
                CheckAllChildNodes(e.Node, e.Node.Checked)
            End If

            ' Update parent nodes based on children
            UpdateParentNodeCheck(e.Node)

            ' Update token count
            UpdateTokenCount()
        Finally
            isUpdatingNodes = False
        End Try
    End Sub

    Private Sub CheckAllChildNodes(parentNode As TreeNode, isChecked As Boolean)
        ' Don't trigger events while updating
        For Each childNode As TreeNode In parentNode.Nodes
            childNode.Checked = isChecked
            If childNode.Nodes.Count > 0 Then
                CheckAllChildNodes(childNode, isChecked)
            End If
        Next
    End Sub

    Private Sub UpdateParentNodeCheck(node As TreeNode)
        If node.Parent IsNot Nothing Then
            Dim checkedCount As Integer = 0
            Dim totalCount As Integer = node.Parent.Nodes.Count

            For Each sibling As TreeNode In node.Parent.Nodes
                If sibling.Checked Then checkedCount += 1
            Next

            ' Set parent as checked if any children are checked
            node.Parent.Checked = checkedCount > 0

            ' Recursively update grandparent
            UpdateParentNodeCheck(node.Parent)
        End If
    End Sub

    Private Sub SelectFileInTree(filePath As String)
        If String.IsNullOrEmpty(filePath) Then Return

        isUpdatingNodes = True
        Try
            Dim node As TreeNode = FindNodeByPath(treeView1.Nodes, filePath)
            If node IsNot Nothing Then
                node.Checked = True
                EnsureNodeVisible(node)
                UpdateParentNodeCheck(node)
            Else
                ' If node not found, try to refresh tree and find again
                If Not String.IsNullOrEmpty(projectFolder) AndAlso filePath.StartsWith(projectFolder, StringComparison.OrdinalIgnoreCase) Then
                    RefreshAndSelectFile(filePath)
                End If
            End If
        Finally
            isUpdatingNodes = False
        End Try
    End Sub

    Private Sub RefreshAndSelectFile(filePath As String)
        ' This method refreshes the tree and then tries to select the file
        If File.Exists(filePath) OrElse Directory.Exists(filePath) Then
            ' Store current selections
            Dim currentSelections As List(Of String) = GetCheckedFiles()

            ' Reload tree structure
            LoadProjectFolder()

            ' Restore all previous selections plus the new one
            isUpdatingNodes = True
            Try
                For Each selectedFile In currentSelections
                    Dim node As TreeNode = FindNodeByPath(treeView1.Nodes, selectedFile)
                    If node IsNot Nothing Then
                        node.Checked = True
                    End If
                Next

                ' Now try to select the target file again
                Dim targetNode As TreeNode = FindNodeByPath(treeView1.Nodes, filePath)
                If targetNode IsNot Nothing Then
                    targetNode.Checked = True
                    EnsureNodeVisible(targetNode)
                End If
            Finally
                isUpdatingNodes = False
            End Try
        End If
    End Sub

    Private Sub EnsureNodeVisible(node As TreeNode)
        ' Only expand parent nodes if they are already expanded or if the node is directly checked
        If Not isLoadingTemplate Then
            Dim parentNode As TreeNode = node.Parent
            While parentNode IsNot Nothing
                If parentNode.IsExpanded Then
                    parentNode.Expand()
                End If
                parentNode = parentNode.Parent
            End While
        End If

        Try
            node.EnsureVisible()
        Catch
            ' Ignore if node can't be made visible
        End Try
    End Sub

    Private Sub SelectFolderInTree(folderPath As String)
        isUpdatingNodes = True
        Try
            Dim node As TreeNode = FindNodeByPath(treeView1.Nodes, folderPath)
            If node IsNot Nothing Then
                CheckAllChildNodes(node, True)
                node.Checked = True
            End If
        Finally
            isUpdatingNodes = False
        End Try
    End Sub

    Private Function FindNodeByPath(nodes As TreeNodeCollection, path As String) As TreeNode
        For Each node As TreeNode In nodes
            If node.Tag IsNot Nothing Then
                Dim nodePath As String = node.Tag.ToString()
                ' Use case-insensitive comparison for Windows file paths
                If String.Equals(nodePath, path, StringComparison.OrdinalIgnoreCase) Then
                    Return node
                End If
            End If

            ' Recursively search child nodes
            Dim childNode As TreeNode = FindNodeByPath(node.Nodes, path)
            If childNode IsNot Nothing Then
                Return childNode
            End If
        Next
        Return Nothing
    End Function

    Private Sub UpdateTokenCount()
        Dim checkedFiles As List(Of String) = GetCheckedFiles()

        If String.IsNullOrEmpty(projectFolder) OrElse String.IsNullOrEmpty(outputFolder) Then
            lblTokenCount.Text = "Token Count: 0"
            Return
        End If

        If cmbProjectType.SelectedItem Is Nothing Then
            lblTokenCount.Text = "Token Count: 0"
            Return
        End If

        Try
            Dim combiner As New FileCombiner(projectFolder, outputFolder)
            Dim tokenCount As Integer = combiner.EstimateTokenCount(checkedFiles, cmbProjectType.SelectedItem.ToString())
            lblTokenCount.Text = "Token Count: " & tokenCount.ToString("N0")
        Catch ex As Exception
            lblTokenCount.Text = "Token Count: Error"
        End Try
    End Sub

    ' === TEMPLATE MANAGEMENT ===
    Private Sub btnSaveTemplate_Click(sender As Object, e As EventArgs) Handles btnSaveTemplate.Click
        If String.IsNullOrWhiteSpace(txtTemplateName.Text) Then
            toolStripStatusLabel1.Text = "Error: Please enter a template name"
            Return
        End If

        Dim checkedFiles As List(Of String) = GetCheckedFiles()
        If checkedFiles.Count = 0 Then
            toolStripStatusLabel1.Text = "Error: Please select at least one file before saving template"
            Return
        End If

        Try
            SaveCurrentTemplate(txtTemplateName.Text.Trim())
            LoadTemplates()

            ' Select the newly saved template
            For i As Integer = 0 To cmbTemplate.Items.Count - 1
                If cmbTemplate.Items(i).ToString().Equals(txtTemplateName.Text.Trim(), StringComparison.OrdinalIgnoreCase) Then
                    cmbTemplate.SelectedIndex = i
                    Exit For
                End If
            Next

            txtTemplateName.Clear()
            toolStripStatusLabel1.Text = "Template saved successfully with " & checkedFiles.Count & " files"
        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error saving template: " & ex.Message
        End Try
    End Sub

    Private Sub btnUpdateTemplate_Click(sender As Object, e As EventArgs) Handles btnUpdateTemplate.Click
        If cmbTemplate.SelectedItem Is Nothing Then
            toolStripStatusLabel1.Text = "Error: Please select a template to update"
            Return
        End If

        Dim checkedFiles As List(Of String) = GetCheckedFiles()
        If checkedFiles.Count = 0 Then
            toolStripStatusLabel1.Text = "Error: Please select at least one file before updating template"
            Return
        End If

        ' Show progress and disable controls
        ShowProgress()
        btnUpdateTemplate.Enabled = False
        btnUpdateTemplate.Text = "Updating..."

        Try
            Dim templateName As String = cmbTemplate.SelectedItem.ToString()

            ' Step 1: Initialize (10%)
            UpdateProgress(10, "Initializing template update...")

            ' Step 2: Store tree expansion state (20%)
            UpdateProgress(20, "Storing tree expansion state...")
            Dim treeState As Dictionary(Of String, Boolean) = GetTreeExpansionState()

            ' Step 3: Delete old template (40%)
            UpdateProgress(40, "Removing old template version...")
            DeleteTemplate(templateName)

            ' Step 4: Save new template (60%)
            UpdateProgress(60, "Saving updated template...")
            SaveCurrentTemplate(templateName)

            ' Step 5: Refresh template list (80%)
            UpdateProgress(80, "Refreshing template list...")
            LoadTemplates()

            ' Step 6: Restore tree state (90%)
            UpdateProgress(90, "Restoring tree expansion state...")
            RestoreTreeExpansionState(treeState)

            ' Step 7: Complete (100%)
            UpdateProgress(100, "Template update complete!")
            System.Threading.Thread.Sleep(500)

            toolStripStatusLabel1.Text = "Template '" & templateName & "' updated with " & checkedFiles.Count & " files"

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error updating template: " & ex.Message
        Finally
            ' Hide progress and restore controls
            HideProgress()
            btnUpdateTemplate.Enabled = True
            btnUpdateTemplate.Text = "Update"
        End Try
    End Sub

    Private Sub btnCopyTemplate_Click(sender As Object, e As EventArgs) Handles btnCopyTemplate.Click
        If cmbTemplate.SelectedItem Is Nothing Then
            toolStripStatusLabel1.Text = "Error: Please select a template to copy"
            Return
        End If

        Try
            Dim selectedTemplate As String = cmbTemplate.SelectedItem.ToString()
            txtTemplateName.Text = selectedTemplate & "_Copy"
            txtTemplateName.SelectAll()
            txtTemplateName.Focus()
            toolStripStatusLabel1.Text = "Template copied - enter new name and click Save to create copy"
        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error copying template: " & ex.Message
        End Try
    End Sub

    Private Sub btnLoadTemplate_Click(sender As Object, e As EventArgs) Handles btnLoadTemplate.Click
        If cmbTemplate.SelectedItem Is Nothing Then
            toolStripStatusLabel1.Text = "Error: Please select a template to load"
            Return
        End If

        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first"
            Return
        End If

        ' Show progress and disable controls
        ShowProgress()
        btnLoadTemplate.Enabled = False
        btnLoadTemplate.Text = "Loading..."

        Try
            ' Step 1: Initialize (10%)
            UpdateProgress(10, "Initializing template loading...")

            EnsureTreeLoaded()
            Dim selectedTemplate = cmbTemplate.SelectedItem.ToString

            ' Step 2: Clear current selections (30%)
            UpdateProgress(30, "Clearing current selections...")
            ClearAllChecks(treeView1.Nodes)

            ' Step 3: Read template (50%)
            UpdateProgress(50, "Reading template: " & selectedTemplate)

            Dim sectionName As String = "Template_" & selectedTemplate.Replace(" ", "_")
            Dim keys As List(Of String) = templateIni.GetKeys(sectionName)
            Dim filesToLoad As New List(Of String)

            ' Collect files from template
            For Each key In keys
                If key.StartsWith("File") Then
                    Dim filePath As String = templateIni.ReadValue(sectionName, key, "")
                    If Not String.IsNullOrEmpty(filePath) Then
                        filesToLoad.Add(filePath)
                    End If
                End If
            Next

            ' Step 4: Load files with progress (50% - 90%)
            UpdateProgress(60, $"Loading {filesToLoad.Count} files...")

            Dim filesLoaded As Integer = 0
            Dim progressStep As Integer = 30 \ Math.Max(filesToLoad.Count, 1)

            For i As Integer = 0 To filesToLoad.Count - 1
                Dim filePath As String = filesToLoad(i)
                Dim currentProgress As Integer = 60 + (i * progressStep)
                UpdateProgress(currentProgress, $"Loading file {i + 1}/{filesToLoad.Count}: {Path.GetFileName(filePath)}")

                If File.Exists(filePath) OrElse Directory.Exists(filePath) Then
                    If filePath.StartsWith(projectFolder, StringComparison.OrdinalIgnoreCase) Then
                        SelectFileInTree(filePath)
                        filesLoaded += 1
                    End If
                End If

                If i Mod 3 = 0 Then ' Update every 3rd file to avoid too many updates
                    System.Threading.Thread.Sleep(50)
                End If
            Next

            ' Step 5: Finalize (100%)
            UpdateProgress(95, "Finalizing template load...")
            UpdateTokenCount()
            UpdateProgress(100, "Template loaded successfully!")
            System.Threading.Thread.Sleep(500)

            toolStripStatusLabel1.Text = "Template '" & selectedTemplate & "' loaded - " & filesLoaded & " files selected"

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error loading template: " & ex.Message
        Finally
            ' Hide progress and restore controls
            HideProgress()
            btnLoadTemplate.Enabled = True
            btnLoadTemplate.Text = "Load"
        End Try
    End Sub

    Private Sub SaveCurrentTemplate(templateName As String)
        Dim checkedFiles As List(Of String) = GetCheckedFiles()
        Dim sectionName As String = "Template_" & templateName.Replace(" ", "_")

        ' Clear existing template data
        Dim keys As List(Of String) = templateIni.GetKeys(sectionName)
        For Each key In keys
            templateIni.DeleteKey(sectionName, key)
        Next

        ' Save template data
        templateIni.WriteValue(sectionName, "Name", templateName)
        For i As Integer = 0 To checkedFiles.Count - 1
            templateIni.WriteValue(sectionName, "File" & (i + 1).ToString(), checkedFiles(i))
        Next
    End Sub

    Private Sub LoadTemplate(templateName As String)
        If String.IsNullOrEmpty(projectFolder) OrElse Not Directory.Exists(projectFolder) Then
            Return
        End If

        EnsureTreeLoaded()

        Try
            ClearAllChecks(treeView1.Nodes)

            Dim sectionName As String = "Template_" & templateName.Replace(" ", "_")
            Dim keys As List(Of String) = templateIni.GetKeys(sectionName)
            Dim filesLoaded As Integer = 0
            Dim filesNotFound As Integer = 0

            For Each key In keys
                If key.StartsWith("File") Then
                    Dim filePath As String = templateIni.ReadValue(sectionName, key, "")
                    If Not String.IsNullOrEmpty(filePath) Then
                        If File.Exists(filePath) OrElse Directory.Exists(filePath) Then
                            If filePath.StartsWith(projectFolder, StringComparison.OrdinalIgnoreCase) Then
                                SelectFileInTree(filePath)
                                Dim node As TreeNode = FindNodeByPath(treeView1.Nodes, filePath)
                                If node IsNot Nothing AndAlso node.Checked Then
                                    filesLoaded += 1
                                Else
                                    filesNotFound += 1
                                End If
                            Else
                                filesNotFound += 1
                            End If
                        Else
                            filesNotFound += 1
                        End If
                    End If
                End If
            Next

            UpdateTokenCount()

        Catch ex As Exception
            toolStripStatusLabel1.Text = $"Error loading template '{templateName}': {ex.Message}"
        End Try
    End Sub

    Private Sub EnsureTreeLoaded()
        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            If treeView1.Nodes.Count = 0 Then
                LoadProjectFolder()
            Else
                ' Check if the root node matches current project folder
                If treeView1.Nodes.Count > 0 AndAlso treeView1.Nodes(0).Tag IsNot Nothing Then
                    Dim rootPath As String = treeView1.Nodes(0).Tag.ToString()
                    If Not String.Equals(rootPath, projectFolder, StringComparison.OrdinalIgnoreCase) Then
                        LoadProjectFolder()
                    End If
                Else
                    LoadProjectFolder()
                End If
            End If
        End If
    End Sub

    Private Sub ClearAllChecks(nodes As TreeNodeCollection)
        isUpdatingNodes = True
        Try
            For Each node As TreeNode In nodes
                node.Checked = False
                ClearAllChecks(node.Nodes)
            Next
        Finally
            isUpdatingNodes = False
        End Try
    End Sub

    Private Function GetCheckedFiles() As List(Of String)
        Dim checkedFiles As New List(Of String)
        CollectCheckedFiles(treeView1.Nodes, checkedFiles)
        Return checkedFiles
    End Function

    Private Sub CollectCheckedFiles(nodes As TreeNodeCollection, checkedFiles As List(Of String))
        For Each node As TreeNode In nodes
            If node.Checked AndAlso node.Tag IsNot Nothing Then
                Dim path As String = node.Tag.ToString()
                If File.Exists(path) Then
                    checkedFiles.Add(path)
                End If
            End If
            CollectCheckedFiles(node.Nodes, checkedFiles)
        Next
    End Sub

    Private Sub cmbTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTemplate.SelectedIndexChanged
        If isLoadingTemplate Then Return

        btnLoadTemplate.Enabled = (cmbTemplate.SelectedItem IsNot Nothing)
        btnCopyTemplate.Enabled = (cmbTemplate.SelectedItem IsNot Nothing)
        btnUpdateTemplate.Enabled = (cmbTemplate.SelectedItem IsNot Nothing)

        If cmbTemplate.SelectedItem IsNot Nothing Then
            toolStripStatusLabel1.Text = "Template selected: " & cmbTemplate.SelectedItem.ToString()
            AutoLoadTemplate(cmbTemplate.SelectedItem.ToString())
        Else
            toolStripStatusLabel1.Text = "No template selected"
        End If
    End Sub

    Private Sub AutoLoadTemplate(templateName As String)
        If String.IsNullOrEmpty(projectFolder) OrElse Not Directory.Exists(projectFolder) Then
            Return
        End If

        EnsureTreeLoaded()

        Try
            isLoadingTemplate = True
            LoadTemplate(templateName)
            Dim loadedFiles As List(Of String) = GetCheckedFiles()
            toolStripStatusLabel1.Text = "Template '" & templateName & "' auto-loaded - " & loadedFiles.Count & " files selected"
        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error auto-loading template: " & ex.Message
        Finally
            isLoadingTemplate = False
        End Try
    End Sub

    ' === TREE STATE MANAGEMENT ===
    Private Function GetTreeExpansionState() As Dictionary(Of String, Boolean)
        Dim state As New Dictionary(Of String, Boolean)
        CollectTreeExpansionState(treeView1.Nodes, state)
        Return state
    End Function

    Private Sub CollectTreeExpansionState(nodes As TreeNodeCollection, state As Dictionary(Of String, Boolean))
        For Each node As TreeNode In nodes
            If node.Tag IsNot Nothing Then
                Dim path As String = node.Tag.ToString()
                state(path) = node.IsExpanded
                If node.Nodes.Count > 0 Then
                    CollectTreeExpansionState(node.Nodes, state)
                End If
            End If
        Next
    End Sub

    Private Sub RestoreTreeExpansionState(state As Dictionary(Of String, Boolean))
        RestoreNodeExpansionState(treeView1.Nodes, state)
    End Sub

    Private Sub RestoreNodeExpansionState(nodes As TreeNodeCollection, state As Dictionary(Of String, Boolean))
        For Each node As TreeNode In nodes
            If node.Tag IsNot Nothing Then
                Dim path As String = node.Tag.ToString()
                If state.ContainsKey(path) Then
                    If state(path) Then
                        node.Expand()
                    Else
                        node.Collapse()
                    End If
                End If
                If node.Nodes.Count > 0 Then
                    RestoreNodeExpansionState(node.Nodes, state)
                End If
            End If
        Next
    End Sub

    Private Sub DeleteTemplate(templateName As String)
        Try
            Dim sectionName As String = "Template_" & templateName.Replace(" ", "_")
            If templateIni.SectionExists(sectionName) Then
                templateIni.DeleteSection(sectionName)
            End If
        Catch ex As Exception
            Throw New Exception("Error deleting template: " & ex.Message)
        End Try
    End Sub

    ' === COMBINE FILES WITH ENHANCED PROGRESS ===
    ' Enhanced btnCombine_Click method for frmMain.vb
    Private Sub btnCombine_Click(sender As Object, e As EventArgs) Handles btnCombine.Click
        If String.IsNullOrWhiteSpace(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first"
            Return
        End If

        If String.IsNullOrWhiteSpace(outputFolder) Then
            toolStripStatusLabel1.Text = "Error: Please set output folder in Settings"
            Return
        End If

        If cmbProjectType.SelectedItem Is Nothing Then
            toolStripStatusLabel1.Text = "Error: Please select a project type"
            Return
        End If

        Dim checkedFiles As List(Of String) = GetCheckedFiles()
        If checkedFiles.Count = 0 Then
            toolStripStatusLabel1.Text = "Error: Please select at least one file to combine"
            Return
        End If

        Try
            If Not Directory.Exists(outputFolder) Then
                Directory.CreateDirectory(outputFolder)
            End If

            ' Test write permissions
            Dim testFile As String = Path.Combine(outputFolder, "test_write_permission.tmp")
            File.WriteAllText(testFile, "test")
            File.Delete(testFile)

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error: Cannot write to output folder - " & ex.Message
            Return
        End Try

        ' Show progress and disable controls
        ShowProgress()
        btnCombine.Enabled = False
        btnCombine.Text = "Combining..."
        Me.UseWaitCursor = True

        Try
            ' Step 1: Clear existing data files (5%)
            UpdateProgress(5, "Clearing previous output files...")
            ClearExistingDataFiles()

            ' Step 2: Initialize (10%)
            UpdateProgress(10, "Initializing file combination...")

            ' Step 3: Analyze files (20%)
            UpdateProgress(20, "Analyzing selected files...")

            ' Find SQL files in the selected files
            Dim selectedSqlFiles As New List(Of String)
            For Each filePath In checkedFiles
                If File.Exists(filePath) AndAlso Path.GetExtension(filePath).ToLower() = ".sql" Then
                    selectedSqlFiles.Add(filePath)
                End If
            Next

            ' Combine configured SQL files with selected SQL files
            Dim allSqlFiles As New List(Of String)(databaseFiles)
            For Each sqlFile In selectedSqlFiles
                If Not allSqlFiles.Contains(sqlFile) Then
                    allSqlFiles.Add(sqlFile)
                End If
            Next

            ' Step 4: Preparation (30%)
            UpdateProgress(30, "Preparing file list and combiner...")

            Dim combiner As New FileCombiner(projectFolder, outputFolder)

            ' Step 5: Process files with detailed progress (30% - 85%)
            Dim result As CombineResult = CombineFilesWithEnhancedProgress(combiner, checkedFiles, allSqlFiles)

            ' Step 6: Finalizing (95%)
            UpdateProgress(95, "Finalizing output files...")
            System.Threading.Thread.Sleep(500)

            ' Step 7: Complete (100%)
            UpdateProgress(100, "File combination complete!")
            System.Threading.Thread.Sleep(1000)

            If result.Success Then
                If allSqlFiles.Count > 0 Then
                    toolStripStatusLabel1.Text = result.Message & $" (Including {allSqlFiles.Count} SQL file(s))"
                Else
                    toolStripStatusLabel1.Text = result.Message
                End If

                ' Show completion message with option to open output folder
                Dim message As String = $"File combination completed successfully!{vbCrLf}{vbCrLf}" &
                                  $"Files processed: {result.FilesIncluded}{vbCrLf}" &
                                  $"Output files created: {result.FileCount}{vbCrLf}" &
                                  $"Output location: {outputFolder}{vbCrLf}{vbCrLf}" &
                                  $"Would you like to open the output folder?"

                Dim dialogResult As DialogResult = MessageBox.Show(message, "Combination Complete",
                                                             MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                If dialogResult = DialogResult.Yes Then
                    goToOutputToolStripMenuItem_Click(sender, e)
                End If
            Else
                toolStripStatusLabel1.Text = "Error: " & result.Message
                MessageBox.Show("Error during file combination: " & result.Message,
                          "Combination Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error combining files: " & ex.Message
            MessageBox.Show("Unexpected error during file combination: " & ex.Message,
                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Hide progress and restore controls
            HideProgress()
            btnCombine.Enabled = True
            btnCombine.Text = "🔗 Combine Files"
            Me.UseWaitCursor = False
        End Try
    End Sub

    ' New method to clear existing data files (only dataXXX.txt files)
    Private Sub ClearExistingDataFiles()
        Try
            If Directory.Exists(outputFolder) Then
                ' Get all files in output folder
                Dim allFiles() As String = Directory.GetFiles(outputFolder, "*.txt", SearchOption.TopDirectoryOnly)
                Dim deletedCount As Integer = 0

                For Each filePath In allFiles
                    Dim fileName As String = Path.GetFileNameWithoutExtension(filePath).ToLower()

                    ' Check if filename matches pattern: data + 3 digits (like data001, data002, etc.)
                    If fileName.Length = 7 AndAlso fileName.StartsWith("data") Then
                        Dim numberPart As String = fileName.Substring(4) ' Get the last 3 characters
                        Dim number As Integer

                        ' Check if the last 3 characters are digits
                        If Integer.TryParse(numberPart, number) AndAlso numberPart.Length = 3 Then
                            Try
                                File.Delete(filePath)
                                deletedCount += 1
                            Catch
                                ' Continue even if some files can't be deleted
                            End Try
                        End If
                    End If
                Next

                If deletedCount > 0 Then
                    toolStripStatusLabel1.Text = $"Cleared {deletedCount} existing data files (data001.txt, data002.txt, etc.)"
                End If
            End If
        Catch ex As Exception
            ' Log but don't stop the process
            toolStripStatusLabel1.Text = "Warning: Could not clear existing data files - " & ex.Message
        End Try
    End Sub

    Private Function CombineFilesWithEnhancedProgress(combiner As FileCombiner, checkedFiles As List(Of String), allSqlFiles As List(Of String)) As CombineResult
        ' Filter files that will actually be processed
        Dim filesToProcess As New List(Of String)
        For Each filePath In checkedFiles
            If File.Exists(filePath) Then
                filesToProcess.Add(filePath)
            End If
        Next

        UpdateProgress(30, $"Found {filesToProcess.Count} files to process...")
        System.Threading.Thread.Sleep(200)

        ' Create progress steps for file processing (30% to 85% = 55% range)
        Dim progressRange As Integer = 55
        Dim baseProgress As Integer = 30

        ' Show detailed file processing progress
        For i As Integer = 0 To Math.Min(filesToProcess.Count - 1, 9) ' Show progress for first 10 files
            Dim currentProgress As Integer = baseProgress + CInt((i + 1) * progressRange / Math.Max(filesToProcess.Count, 10))
            Dim fileName As String = Path.GetFileName(filesToProcess(i))
            UpdateProgress(currentProgress, $"Processing file {i + 1}/{filesToProcess.Count}: {fileName}")
            System.Threading.Thread.Sleep(100) ' Brief delay to show progress
        Next

        ' If more than 10 files, show bulk processing
        If filesToProcess.Count > 10 Then
            UpdateProgress(70, $"Processing remaining {filesToProcess.Count - 10} files...")
            System.Threading.Thread.Sleep(300)
        End If

        ' Now perform the actual file combination
        UpdateProgress(85, "Combining all files into output format...")
        System.Threading.Thread.Sleep(200)

        Dim result As CombineResult = combiner.CombineFiles(
            checkedFiles,
            cmbProjectType.SelectedItem.ToString(),
            treeView1.Nodes,
            txtProjectTitle.Text.Trim(),
            "",
            "",
            allSqlFiles.Count > 0,
            allSqlFiles
        )

        UpdateProgress(90, "Writing output files...")
        System.Threading.Thread.Sleep(300)

        Return result
    End Function

    Private Sub cmbProjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProjectType.SelectedIndexChanged
        UpdateTokenCount()
        SaveConfiguration()

        If cmbProjectType.SelectedItem IsNot Nothing Then
            toolStripStatusLabel1.Text = "Project type changed to: " & cmbProjectType.SelectedItem.ToString()
        End If
    End Sub

    ' === SETTINGS - FIXED: Updated to include excluded folders ===
    Private Sub applicationSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles applicationSettingsToolStripMenuItem.Click
        Dim settingsForm As New frmSettings()
        settingsForm.ProjectFolderPath = projectFolder
        settingsForm.OutputFolderPath = outputFolder
        settingsForm.DatabaseFiles = New List(Of String)(databaseFiles)
        settingsForm.IncludeDatabase = includeDatabase
        settingsForm.ExcludedFolders = New List(Of String)(excludedFolders)

        ' FIXED: Set extension lists properly
        settingsForm.VBDesktopExtensions = New List(Of String)(vbDesktopExtensions)
        settingsForm.AspCoreExtensions = New List(Of String)(aspCoreExtensions)
        settingsForm.AspMvcExtensions = New List(Of String)(aspMvcExtensions)

        If settingsForm.ShowDialog() = DialogResult.OK Then
            projectFolder = settingsForm.ProjectFolderPath
            outputFolder = settingsForm.OutputFolderPath
            databaseFiles = settingsForm.DatabaseFiles
            includeDatabase = settingsForm.IncludeDatabase
            excludedFolders = settingsForm.ExcludedFolders

            ' FIXED: Get updated extension lists and save them
            vbDesktopExtensions = settingsForm.VBDesktopExtensions
            aspCoreExtensions = settingsForm.AspCoreExtensions
            aspMvcExtensions = settingsForm.AspMvcExtensions

            ' Save all configuration including extensions
            SaveConfiguration()
            LoadProjectFolder()

            toolStripStatusLabel1.Text = $"Settings updated - {databaseFiles.Count} SQL file(s), {excludedFolders.Count} excluded folder(s), Extensions saved"
        End If
    End Sub

    ' === PROGRESS AND STATUS UPDATES ===
    Private Sub UpdateProgress(percentage As Integer, message As String)
        ' Ensure percentage is within bounds
        percentage = Math.Max(0, Math.Min(100, percentage))

        ' Update main progress bar
        progressBar1.Value = percentage
        progressBar1.Visible = True

        ' Update status message
        toolStripStatusLabel1.Text = message

        ' Also update the status strip progress bar
        toolStripProgressBar1.Visible = True
        toolStripProgressBar1.Value = percentage

        ' Force UI update
        Application.DoEvents()
        Me.Refresh()
    End Sub

    Private Sub HideProgress()
        progressBar1.Visible = False
        toolStripProgressBar1.Visible = False
        Application.DoEvents()
    End Sub

    Private Sub ShowProgress()
        progressBar1.Visible = True
        progressBar1.Value = 0
        toolStripProgressBar1.Visible = True
        toolStripProgressBar1.Value = 0
        Application.DoEvents()
    End Sub

    ' === FILE STATUS AND COUNTING ===
    Private Sub UpdateFileStatusDisplay()
        Try
            Dim checkedFiles As List(Of String) = GetCheckedFiles()
            Dim totalSize As Long = 0

            For Each filePath In checkedFiles
                If File.Exists(filePath) Then
                    Try
                        Dim fileInfo As New FileInfo(filePath)
                        totalSize += fileInfo.Length
                    Catch
                        ' Skip files we can't access
                    End Try
                End If
            Next

            Dim sizeText As String = FormatFileSize(totalSize)
            toolStripStatusLabelFiles.Text = $"Files: {checkedFiles.Count} | Size: {sizeText}"

        Catch ex As Exception
            toolStripStatusLabelFiles.Text = "Files: 0 | Size: 0 KB"
        End Try
    End Sub

    Private Function FormatFileSize(bytes As Long) As String
        If bytes < 1024 Then
            Return bytes.ToString() & " bytes"
        ElseIf bytes < 1024 * 1024 Then
            Return Math.Round(bytes / 1024.0, 1).ToString() & " KB"
        ElseIf bytes < 1024 * 1024 * 1024 Then
            Return Math.Round(bytes / (1024.0 * 1024.0), 1).ToString() & " MB"
        Else
            Return Math.Round(bytes / (1024.0 * 1024.0 * 1024.0), 1).ToString() & " GB"
        End If
    End Function

    ' === ADDITIONAL EVENT HANDLERS ===
    Private Sub treeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles treeView1.AfterSelect
        ' Update file status when selection changes
        UpdateFileStatusDisplay()
    End Sub

    Private Sub txtProjectTitle_TextChanged(sender As Object, e As EventArgs) Handles txtProjectTitle.TextChanged
        ' Update window title with project name
        If Not String.IsNullOrWhiteSpace(txtProjectTitle.Text) Then
            Me.Text = $"RepoBundle - {txtProjectTitle.Text.Trim()}"
        Else
            Me.Text = "RepoBundle - Project File Combiner v2.0"
        End If
    End Sub

    ' === HELP AND ABOUT ===
    Private Sub aboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles aboutToolStripMenuItem.Click
        Dim aboutMessage As String = $"RepoBundle - Project File Combiner v2.0{vbCrLf}{vbCrLf}" &
                                   $"A powerful tool for combining project files with intelligent filtering.{vbCrLf}{vbCrLf}" &
                                   $"Features:{vbCrLf}" &
                                   $"• Smart project type filtering{vbCrLf}" &
                                   $"• Template management system{vbCrLf}" &
                                   $"• Automatic file splitting (200KB limit){vbCrLf}" &
                                   $"• Token counting and estimation{vbCrLf}" &
                                   $"• INI-based configuration{vbCrLf}" &
                                   $"• Project backup functionality{vbCrLf}" &
                                   $"• Dynamic excluded folders{vbCrLf}" &
                                   $"• Quick output folder access{vbCrLf}{vbCrLf}" &
                                   $"Configuration files:{vbCrLf}" &
                                   $"• config.ini - Application settings{vbCrLf}" &
                                   $"• template.ini - Saved file templates{vbCrLf}{vbCrLf}" &
                                   $"© 2025 Samsur Rahman Mahi"

        MessageBox.Show(aboutMessage, "About RepoBundle", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' === KEYBOARD SHORTCUTS ===
    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Handle keyboard shortcuts
        Select Case e.KeyCode
            Case Keys.F5
                ' Refresh tree view
                If Not String.IsNullOrEmpty(projectFolder) Then
                    RefreshTreeView()
                    e.Handled = True
                End If
            Case Keys.F1
                ' Show about dialog
                aboutToolStripMenuItem_Click(sender, EventArgs.Empty)
                e.Handled = True
        End Select

        ' Handle Ctrl key combinations
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.O
                    ' Open project folder
                    selectProjectFolderToolStripMenuItem_Click(sender, EventArgs.Empty)
                    e.Handled = True
                Case Keys.B
                    ' Backup project
                    If Not String.IsNullOrEmpty(projectFolder) Then
                        backupProjectToolStripMenuItem_Click(sender, EventArgs.Empty)
                        e.Handled = True
                    End If
                Case Keys.G
                    ' Go to output folder
                    If Not String.IsNullOrEmpty(outputFolder) Then
                        goToOutputToolStripMenuItem_Click(sender, EventArgs.Empty)
                        e.Handled = True
                    End If
                Case Keys.S
                    ' Save template (if template name is entered)
                    If Not String.IsNullOrWhiteSpace(txtTemplateName.Text) Then
                        btnSaveTemplate_Click(sender, EventArgs.Empty)
                        e.Handled = True
                    End If
                Case Keys.L
                    ' Load template (if template is selected)
                    If cmbTemplate.SelectedItem IsNot Nothing Then
                        btnLoadTemplate_Click(sender, EventArgs.Empty)
                        e.Handled = True
                    End If
                Case Keys.Enter
                    ' Combine files
                    btnCombine_Click(sender, EventArgs.Empty)
                    e.Handled = True
            End Select
        End If
    End Sub

    ' === TOOLTIP SETUP ===
    Private Sub SetupTooltips()
        Dim toolTip As New ToolTip()
        toolTip.AutoPopDelay = 5000
        toolTip.InitialDelay = 1000
        toolTip.ReshowDelay = 500
        toolTip.ShowAlways = True

        ' Add tooltips to controls
        toolTip.SetToolTip(btnRefreshTree, "Refresh the file tree (F5)")
        toolTip.SetToolTip(btnExpandAll, "Expand all tree nodes")
        toolTip.SetToolTip(btnCollapseAll, "Collapse all tree nodes")
        toolTip.SetToolTip(btnSaveTemplate, "Save current file selection as template (Ctrl+S)")
        toolTip.SetToolTip(btnLoadTemplate, "Load selected template (Ctrl+L)")
        toolTip.SetToolTip(btnCopyTemplate, "Copy template name for creating a new template")
        toolTip.SetToolTip(btnUpdateTemplate, "Update selected template with current file selection")
        toolTip.SetToolTip(btnCombine, "Combine selected files (Ctrl+Enter)")
        toolTip.SetToolTip(cmbProjectType, "Select project type for file filtering")
        toolTip.SetToolTip(lblTokenCount, "Estimated token count for selected files")
    End Sub

    ' === WINDOW STATE MANAGEMENT ===
    Private Sub SaveWindowState()
        Try
            iniHelper.WriteValue("Window", "WindowState", Me.WindowState.ToString())
            If Me.WindowState = FormWindowState.Normal Then
                iniHelper.WriteValue("Window", "Width", Me.Width.ToString())
                iniHelper.WriteValue("Window", "Height", Me.Height.ToString())
                iniHelper.WriteValue("Window", "Left", Me.Left.ToString())
                iniHelper.WriteValue("Window", "Top", Me.Top.ToString())
            End If
        Catch
            ' Ignore errors saving window state
        End Try
    End Sub

    Private Sub LoadWindowState()
        Try
            Dim windowState As String = iniHelper.ReadValue("Window", "WindowState", "Normal")
            Dim width As Integer = Integer.Parse(iniHelper.ReadValue("Window", "Width", "1138"))
            Dim height As Integer = Integer.Parse(iniHelper.ReadValue("Window", "Height", "692"))
            Dim left As Integer = Integer.Parse(iniHelper.ReadValue("Window", "Left", "100"))
            Dim top As Integer = Integer.Parse(iniHelper.ReadValue("Window", "Top", "100"))

            ' Validate screen bounds
            If left >= 0 AndAlso top >= 0 AndAlso left < Screen.PrimaryScreen.WorkingArea.Width AndAlso top < Screen.PrimaryScreen.WorkingArea.Height Then
                Me.StartPosition = FormStartPosition.Manual
                Me.Left = left
                Me.Top = top
            End If

            If width > Me.MinimumSize.Width AndAlso height > Me.MinimumSize.Height Then
                Me.Size = New Size(width, height)
            End If

            Select Case windowState
                Case "Maximized"
                    Me.WindowState = FormWindowState.Maximized
                Case "Minimized"
                    Me.WindowState = FormWindowState.Normal ' Don't start minimized
                Case Else
                    Me.WindowState = FormWindowState.Normal
            End Select

        Catch
            ' Use defaults if loading fails
        End Try
    End Sub

    ' === ENHANCED INITIALIZATION ===
    Private Sub InitializeAdvancedFeatures()
        ' Setup tooltips
        SetupTooltips()

        ' Load window state
        LoadWindowState()

        ' Set up advanced event handlers
        AddHandler Me.FormClosing, AddressOf SaveWindowStateOnClose
        AddHandler Me.Resize, AddressOf OnWindowResize

        ' Initialize file status display
        UpdateFileStatusDisplay()

        ' Set keyboard preview
        Me.KeyPreview = True
    End Sub

    Private Sub SaveWindowStateOnClose(sender As Object, e As FormClosingEventArgs)
        SaveWindowState()
        SaveConfiguration()
    End Sub

    Private Sub OnWindowResize(sender As Object, e As EventArgs)
        ' Update file status when window is resized (in case tree visibility changes)
        Static lastResize As DateTime = DateTime.MinValue
        If DateTime.Now.Subtract(lastResize).TotalMilliseconds > 500 Then
            UpdateFileStatusDisplay()
            lastResize = DateTime.Now
        End If
    End Sub
    ' FIXED: Get relative path for general use without URL encoding
    Private Function GetRelativePath(fullPath As String, basePath As String) As String
        Try
            ' Normalize paths to use consistent separators
            Dim normalizedBase As String = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar)
            Dim normalizedFull As String = Path.GetFullPath(fullPath)

            ' Check if the file is actually under the base path
            If normalizedFull.StartsWith(normalizedBase & Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) Then
                ' Calculate relative path by removing the base path
                Dim relativePath As String = normalizedFull.Substring(normalizedBase.Length + 1)
                Return relativePath
            Else
                ' File is not under base path, return just filename
                Return Path.GetFileName(fullPath)
            End If

        Catch ex As Exception
            ' Fallback to filename only
            Return Path.GetFileName(fullPath)
        End Try
    End Function

    ' FIXED: Generate file tree method in FileCombiner (if you're using it there too)
    ' You may need to update the FileCombiner.vb as well with this method:
    Private Function GetRelativePathForTree(fullPath As String, basePath As String) As String
        Try
            ' Simple string-based relative path calculation
            Dim normalizedBase As String = Path.GetFullPath(basePath).TrimEnd("\"c) & "\"
            Dim normalizedFull As String = Path.GetFullPath(fullPath)

            If normalizedFull.StartsWith(normalizedBase, StringComparison.OrdinalIgnoreCase) Then
                Return normalizedFull.Substring(normalizedBase.Length)
            Else
                Return Path.GetFileName(fullPath)
            End If
        Catch
            Return Path.GetFileName(fullPath)
        End Try
    End Function

    ' === DEBUG AND TESTING METHODS ===
    Private Sub TestBackupContents(zipFilePath As String)
        Try
            Using archive As ZipArchive = ZipFile.OpenRead(zipFilePath)
                Dim sb As New System.Text.StringBuilder()
                sb.AppendLine($"Backup file: {Path.GetFileName(zipFilePath)}")
                sb.AppendLine($"Total entries: {archive.Entries.Count}")
                sb.AppendLine($"File size: {FormatFileSize(New FileInfo(zipFilePath).Length)}")
                sb.AppendLine()
                sb.AppendLine("Contents:")

                For Each entry In archive.Entries.Take(20) ' Show first 20 entries
                    sb.AppendLine($"  {entry.FullName} ({FormatFileSize(entry.Length)})")
                Next

                If archive.Entries.Count > 20 Then
                    sb.AppendLine($"  ... and {archive.Entries.Count - 20} more files")
                End If

                MessageBox.Show(sb.ToString(), "Backup Contents", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error reading backup file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' === MAIN LOAD EVENT ===
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeApplication()
        InitializeAdvancedFeatures()
    End Sub

    ' === FORM CLOSING ===
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Save configuration on exit
        SaveConfiguration()
        SaveWindowState()
    End Sub

End Class