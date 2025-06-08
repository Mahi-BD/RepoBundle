Imports System.IO
Imports System.Text
Imports System.Linq

Public Class frmMain
    Private configFile As String = "config.ini"
    Private templateFile As String = "template.ini"
    Private projectFolder As String = ""
    Private outputFolder As String = ""
    Private databaseFiles As New List(Of String)
    Private includeDatabase As Boolean = False
    Private lastSelectedTemplate As String = ""
    Private lastSelectedProjectType As String = ""
    Private projectTitle As String = ""
    Private projectInstructions As String = ""
    Private otherInstructions As String = ""
    Private isUpdatingNodes As Boolean = False
    Private isLoadingTemplate As Boolean = False

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeApplication()
    End Sub

    Private Sub InitializeApplication()
        ' Initialize project types
        cmbProjectType.Items.AddRange({"Visual Basic Desktop", "Asp MVC 5", "Asp Dotnet Core 8"})

        ' Ensure output folder exists or set default
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

        ' Load configuration and templates
        LoadConfiguration()
        LoadTemplates()

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

        ' Add event handlers for auto-save
        AddHandler txtProjectTitle.TextChanged, AddressOf ConfigurationChanged
        AddHandler txtProjectInstructions.TextChanged, AddressOf ConfigurationChanged
        AddHandler txtOtherInstructions.TextChanged, AddressOf ConfigurationChanged

        ' Set status
        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Ready - Select project folder to begin"
        Else
            toolStripStatusLabel1.Text = "Ready - Project folder loaded: " & Path.GetFileName(projectFolder)
        End If
    End Sub

    ' === NEW EXPAND/COLLAPSE FUNCTIONALITY ===
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
    ' === END NEW EXPAND/COLLAPSE FUNCTIONALITY ===

    Private Sub ConfigurationChanged(sender As Object, e As EventArgs)
        ' Auto-save configuration when text boxes change (with delay to avoid constant saving)
        Static lastSave As DateTime = DateTime.MinValue
        If DateTime.Now.Subtract(lastSave).TotalSeconds > 2 Then
            SaveConfiguration()
            lastSave = DateTime.Now
        End If
    End Sub

    Private Sub LoadConfiguration()
        If Not File.Exists(configFile) Then
            CreateDefaultConfigFile()
        End If

        If File.Exists(configFile) Then
            Dim lines() As String = File.ReadAllLines(configFile)
            databaseFiles.Clear() ' Clear existing list

            For Each line In lines
                If line.StartsWith("ProjectFolder=") Then
                    Dim newProjectFolder As String = line.Substring("ProjectFolder=".Length).Trim()
                    If Not String.IsNullOrEmpty(newProjectFolder) AndAlso Directory.Exists(newProjectFolder) Then
                        projectFolder = newProjectFolder
                    End If
                ElseIf line.StartsWith("OutputFolder=") Then
                    outputFolder = line.Substring("OutputFolder=".Length).Trim()
                ElseIf line.StartsWith("DatabaseFile=") Then
                    ' Support multiple database files
                    Dim dbFile As String = line.Substring("DatabaseFile=".Length).Trim()
                    If Not String.IsNullOrEmpty(dbFile) AndAlso File.Exists(dbFile) Then
                        databaseFiles.Add(dbFile)
                    End If
                ElseIf line.StartsWith("IncludeDatabase=") Then
                    Boolean.TryParse(line.Substring("IncludeDatabase=".Length).Trim(), includeDatabase)
                ElseIf line.StartsWith("LastSelectedTemplate=") Then
                    lastSelectedTemplate = line.Substring("LastSelectedTemplate=".Length).Trim()
                ElseIf line.StartsWith("LastSelectedProjectType=") Then
                    lastSelectedProjectType = line.Substring("LastSelectedProjectType=".Length).Trim()
                ElseIf line.StartsWith("ProjectTitle=") Then
                    projectTitle = line.Substring("ProjectTitle=".Length).Trim()
                    txtProjectTitle.Text = projectTitle
                ElseIf line.StartsWith("ProjectInstructions=") Then
                    projectInstructions = line.Substring("ProjectInstructions=".Length).Replace("\n", vbCrLf)
                    txtProjectInstructions.Text = projectInstructions
                ElseIf line.StartsWith("OtherInstructions=") Then
                    otherInstructions = line.Substring("OtherInstructions=".Length).Replace("\n", vbCrLf)
                    txtOtherInstructions.Text = otherInstructions
                End If
            Next
        End If
    End Sub

    Private Sub CreateDefaultConfigFile()
        Try
            Dim defaultConfig As New StringBuilder()
            defaultConfig.AppendLine("# Project File Combiner Configuration")
            defaultConfig.AppendLine("# This file stores application settings")
            defaultConfig.AppendLine("")
            defaultConfig.AppendLine("# Main project folder path")
            defaultConfig.AppendLine("ProjectFolder=")
            defaultConfig.AppendLine("")
            defaultConfig.AppendLine("# Output folder for combined files")
            defaultConfig.AppendLine("OutputFolder=")
            defaultConfig.AppendLine("")
            defaultConfig.AppendLine("# Database settings - multiple SQL files supported")
            defaultConfig.AppendLine("IncludeDatabase=False")
            defaultConfig.AppendLine("")
            defaultConfig.AppendLine("# Last selected template and project type")
            defaultConfig.AppendLine("LastSelectedTemplate=")
            defaultConfig.AppendLine("LastSelectedProjectType=")
            defaultConfig.AppendLine("")
            defaultConfig.AppendLine("# Project information")
            defaultConfig.AppendLine("ProjectTitle=")
            defaultConfig.AppendLine("ProjectInstructions=")
            defaultConfig.AppendLine("OtherInstructions=")

            File.WriteAllText(configFile, defaultConfig.ToString())
            toolStripStatusLabel1.Text = "Created default config.ini file"
        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error creating config file: " & ex.Message
        End Try
    End Sub

    Private Sub SaveConfiguration()
        Try
            Dim config As New StringBuilder()
            config.AppendLine("# Project File Combiner Configuration")
            config.AppendLine("# This file stores application settings")
            config.AppendLine("")
            config.AppendLine("# Main project folder path")
            config.AppendLine("ProjectFolder=" & projectFolder)
            config.AppendLine("")
            config.AppendLine("# Output folder for combined files")
            config.AppendLine("OutputFolder=" & outputFolder)
            config.AppendLine("")
            config.AppendLine("# Database settings - multiple SQL files supported")
            For Each dbFile In databaseFiles
                config.AppendLine("DatabaseFile=" & dbFile)
            Next
            config.AppendLine("IncludeDatabase=" & includeDatabase.ToString())
            config.AppendLine("")
            config.AppendLine("# Last selected template and project type")
            config.AppendLine("LastSelectedTemplate=" & If(cmbTemplate.SelectedItem?.ToString(), ""))
            config.AppendLine("LastSelectedProjectType=" & If(cmbProjectType.SelectedItem?.ToString(), ""))
            config.AppendLine("")
            config.AppendLine("# Project information")
            config.AppendLine("ProjectTitle=" & txtProjectTitle.Text.Trim())
            config.AppendLine("ProjectInstructions=" & txtProjectInstructions.Text.Replace(vbCrLf, "\n"))
            config.AppendLine("OtherInstructions=" & txtOtherInstructions.Text.Replace(vbCrLf, "\n"))

            File.WriteAllText(configFile, config.ToString())

            ' Optional: Show debug info in status (remove in production)
            ' toolStripStatusLabel1.Text = "Configuration saved successfully"

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error saving configuration: " & ex.Message
        End Try
    End Sub

    Private Sub LoadTemplates()
        ' Store current selection before clearing
        Dim currentSelection As String = ""
        If cmbTemplate.SelectedItem IsNot Nothing Then
            currentSelection = cmbTemplate.SelectedItem.ToString()
        End If

        cmbTemplate.Items.Clear()

        If Not File.Exists(templateFile) Then
            CreateDefaultTemplateFile()
        End If

        If File.Exists(templateFile) Then
            Try
                Dim content As String = File.ReadAllText(templateFile)

                ' Skip if file is empty or only contains comments
                If String.IsNullOrWhiteSpace(content) OrElse Not content.Contains("[TEMPLATE]") Then
                    Return
                End If

                ' Split by [TEMPLATE] sections
                Dim sections() As String = content.Split(New String() {"[TEMPLATE]"}, StringSplitOptions.RemoveEmptyEntries)

                For Each section In sections
                    If Not String.IsNullOrWhiteSpace(section) Then
                        ' Split section into lines
                        Dim lines() As String = section.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)

                        For Each line In lines
                            Dim trimmedLine As String = line.Trim()
                            ' Skip comments and empty lines
                            If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                                If trimmedLine.StartsWith("Name=") Then
                                    Dim templateName As String = trimmedLine.Substring("Name=".Length).Trim()
                                    If Not String.IsNullOrWhiteSpace(templateName) Then
                                        cmbTemplate.Items.Add(templateName)
                                        Exit For ' Found the name, move to next section
                                    End If
                                End If
                            End If
                        Next
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
        End If
    End Sub

    Private Sub CreateDefaultTemplateFile()
        Try
            Dim defaultTemplate As New StringBuilder()
            defaultTemplate.AppendLine("# Project File Combiner Templates")
            defaultTemplate.AppendLine("# This file stores saved file selection templates")
            defaultTemplate.AppendLine("# Format:")
            defaultTemplate.AppendLine("# [TEMPLATE]")
            defaultTemplate.AppendLine("# Name=TemplateName")
            defaultTemplate.AppendLine("# File=C:\Path\To\File1.ext")
            defaultTemplate.AppendLine("# File=C:\Path\To\File2.ext")
            defaultTemplate.AppendLine("")

            File.WriteAllText(templateFile, defaultTemplate.ToString())
        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error creating template file: " & ex.Message
        End Try
    End Sub

    Private Sub LoadProjectFolder()
        If Directory.Exists(projectFolder) Then
            treeView1.Nodes.Clear()
            treeView1.BeginUpdate() ' Prevent flickering during load

            Try
                Dim rootNode As TreeNode = New TreeNode(Path.GetFileName(projectFolder))
                rootNode.Tag = projectFolder
                rootNode.ImageIndex = 0 ' Folder icon
                treeView1.Nodes.Add(rootNode)
                LoadDirectoryNodes(rootNode, projectFolder)

                toolStripStatusLabel1.Text = "Project folder loaded: " & projectFolder & " (" & CountTotalNodes(rootNode) & " items) - Tree collapsed"
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

    Private Sub selectProjectFolderToolStripMenuItem_Click(sender As Object, e As EventArgs)
        folderBrowserDialog1.Description = "Select Project Folder"
        folderBrowserDialog1.ShowNewFolderButton = True

        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            folderBrowserDialog1.SelectedPath = projectFolder
        End If

        If folderBrowserDialog1.ShowDialog = DialogResult.OK Then
            projectFolder = folderBrowserDialog1.SelectedPath
            LoadProjectFolder()
            SaveConfiguration()

            ' Refresh templates after changing project folder
            RefreshTreeView()
        End If
    End Sub

    Private Sub RefreshTreeView()
        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            ' Store currently checked files and expansion state
            Dim checkedFiles As List(Of String) = GetCheckedFiles()
            Dim treeState As Dictionary(Of String, Boolean) = GetTreeExpansionState()

            ' Reload tree
            LoadProjectFolder()

            ' Restore checked state and expansion state
            If checkedFiles.Count > 0 OrElse treeState.Count > 0 Then
                isUpdatingNodes = True
                Try
                    ' Restore checked files
                    For Each filePath In checkedFiles
                        If File.Exists(filePath) Then
                            SelectFileInTree(filePath)
                        End If
                    Next

                    ' Restore expansion state (or keep collapsed if no previous state)
                    If treeState.Count > 0 Then
                        RestoreTreeExpansionState(treeState)
                    End If
                Finally
                    isUpdatingNodes = False
                End Try
                UpdateTokenCount()
            End If
        End If
    End Sub

    Private Sub refreshTreeViewToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first"
            Return
        End If

        RefreshTreeView()
        toolStripStatusLabel1.Text = "Tree view refreshed successfully"
    End Sub


    Private Sub outputFolderToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If folderBrowserDialog1.ShowDialog = DialogResult.OK Then
            outputFolder = folderBrowserDialog1.SelectedPath
            SaveConfiguration()
            toolStripStatusLabel1.Text = "Output folder set: " & outputFolder
        End If
    End Sub

    Private Sub exitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles exitToolStripMenuItem.Click
        Application.Exit()
    End Sub

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

    Private Sub SelectFileInTree(filePath As String)
        If String.IsNullOrEmpty(filePath) Then Return

        isUpdatingNodes = True
        Try
            Dim node As TreeNode = FindNodeByPath(treeView1.Nodes, filePath)
            If node IsNot Nothing Then
                node.Checked = True
                ' Ensure parent nodes are expanded and visible
                EnsureNodeVisible(node)
                ' Update parent folder states
                UpdateParentNodeCheck(node)
            Else
                ' If node not found, try to refresh tree and find again
                If Not String.IsNullOrEmpty(projectFolder) AndAlso filePath.StartsWith(projectFolder, StringComparison.OrdinalIgnoreCase) Then
                    ' File should be in our project tree, but node wasn't found
                    ' This might happen if tree is not fully loaded
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
        ' Don't auto-expand during template loading to keep tree collapsed
        If Not isLoadingTemplate Then
            Dim parentNode As TreeNode = node.Parent
            While parentNode IsNot Nothing
                ' Only expand if parent was already expanded
                If parentNode.IsExpanded Then
                    parentNode.Expand()
                End If
                parentNode = parentNode.Parent
            End While
        End If

        ' Ensure the node itself is visible (scroll to it) but don't force expansion
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

    Private Sub CheckAllChildNodes(parentNode As TreeNode, isChecked As Boolean)
        ' Don't trigger events while updating
        For Each childNode As TreeNode In parentNode.Nodes
            childNode.Checked = isChecked
            If childNode.Nodes.Count > 0 Then
                CheckAllChildNodes(childNode, isChecked)
            End If
        Next
    End Sub

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
            LoadTemplates() ' Refresh the template list

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

        ' Show progress bar and disable button
        progressBar1.Visible = True
        progressBar1.Minimum = 0
        progressBar1.Maximum = 100
        progressBar1.Value = 0
        btnUpdateTemplate.Enabled = False
        btnUpdateTemplate.Text = "Updating..."

        Try
            Dim templateName As String = cmbTemplate.SelectedItem.ToString()

            ' Step 1: Initialize (10%)
            UpdateProgress(10, "Initializing template update...")
            Application.DoEvents()

            ' Step 2: Store tree expansion state (20%)
            UpdateProgress(20, "Storing tree expansion state...")
            Application.DoEvents()
            Dim treeState As Dictionary(Of String, Boolean) = GetTreeExpansionState()

            ' Step 3: Delete old template (40%)
            UpdateProgress(40, "Removing old template version...")
            Application.DoEvents()
            DeleteTemplate(templateName)

            ' Step 4: Save new template (60%)
            UpdateProgress(60, "Saving updated template...")
            Application.DoEvents()
            SaveCurrentTemplate(templateName)

            ' Step 5: Refresh template list (80%)
            UpdateProgress(80, "Refreshing template list...")
            Application.DoEvents()
            LoadTemplates()

            ' Step 6: Restore tree state (90%)
            UpdateProgress(90, "Restoring tree expansion state...")
            Application.DoEvents()
            RestoreTreeExpansionState(treeState)

            ' Step 7: Complete (100%)
            UpdateProgress(100, "Template update complete!")
            Application.DoEvents()

            toolStripStatusLabel1.Text = "Template '" & templateName & "' updated with " & checkedFiles.Count & " files"

            ' Small delay to show completion
            System.Threading.Thread.Sleep(300)

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error updating template: " & ex.Message
        Finally
            ' Hide progress bar and restore button
            progressBar1.Visible = False
            btnUpdateTemplate.Enabled = True
            btnUpdateTemplate.Text = "Update"
        End Try
    End Sub

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
        If Not File.Exists(templateFile) Then Return

        Try
            Dim content As String = File.ReadAllText(templateFile)
            Dim sections() As String = content.Split(New String() {"[TEMPLATE]"}, StringSplitOptions.RemoveEmptyEntries)
            Dim newContent As New StringBuilder()

            For Each section In sections
                If Not String.IsNullOrWhiteSpace(section) Then
                    Dim lines() As String = section.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)
                    Dim isTargetTemplate As Boolean = False

                    ' Check if this is the template to delete
                    For Each line In lines
                        Dim trimmedLine As String = line.Trim()
                        If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                            If trimmedLine.StartsWith("Name=") Then
                                Dim currentTemplateName As String = trimmedLine.Substring("Name=".Length).Trim()
                                If currentTemplateName.Equals(templateName, StringComparison.OrdinalIgnoreCase) Then
                                    isTargetTemplate = True
                                End If
                                Exit For
                            End If
                        End If
                    Next

                    ' If this is not the template to delete, keep it
                    If Not isTargetTemplate Then
                        newContent.AppendLine("[TEMPLATE]")
                        newContent.Append(section)
                        If Not section.EndsWith(vbCrLf) Then
                            newContent.AppendLine()
                        End If
                    End If
                End If
            Next

            File.WriteAllText(templateFile, newContent.ToString())
        Catch ex As Exception
            Throw New Exception("Error deleting template: " & ex.Message)
        End Try
    End Sub

    Private Sub SaveCurrentTemplate(templateName As String)
        Dim checkedFiles As List(Of String) = GetCheckedFiles()

        ' Read existing templates and rebuild without the one we're saving
        Dim newContent As New StringBuilder()
        If File.Exists(templateFile) Then
            Dim existingContent As String = File.ReadAllText(templateFile)
            If Not String.IsNullOrWhiteSpace(existingContent) AndAlso existingContent.Contains("[TEMPLATE]") Then
                Dim sections() As String = existingContent.Split(New String() {"[TEMPLATE]"}, StringSplitOptions.RemoveEmptyEntries)

                For Each section In sections
                    If Not String.IsNullOrWhiteSpace(section) Then
                        Dim lines() As String = section.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)
                        Dim sectionTemplateName As String = ""

                        ' Find the template name in this section
                        For Each line In lines
                            Dim trimmedLine As String = line.Trim()
                            If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                                If trimmedLine.StartsWith("Name=") Then
                                    sectionTemplateName = trimmedLine.Substring("Name=".Length).Trim()
                                    Exit For
                                End If
                            End If
                        Next

                        ' Only keep sections that don't match the template we're saving
                        If Not sectionTemplateName.Equals(templateName, StringComparison.OrdinalIgnoreCase) Then
                            newContent.AppendLine("[TEMPLATE]")
                            newContent.Append(section)
                            If Not section.EndsWith(vbCrLf) Then
                                newContent.AppendLine()
                            End If
                        End If
                    End If
                Next
            End If
        End If

        ' Add the new/updated template
        newContent.AppendLine("[TEMPLATE]")
        newContent.AppendLine("Name=" & templateName)
        For Each filePath In checkedFiles
            newContent.AppendLine("File=" & filePath)
        Next
        newContent.AppendLine() ' Add blank line after template

        File.WriteAllText(templateFile, newContent.ToString())
    End Sub

    Private Sub btnCopyTemplate_Click(sender As Object, e As EventArgs) Handles btnCopyTemplate.Click
        If cmbTemplate.SelectedItem Is Nothing Then
            toolStripStatusLabel1.Text = "Error: Please select a template to copy"
            Return
        End If

        Try
            Dim selectedTemplate As String = cmbTemplate.SelectedItem.ToString()

            ' Copy template name to text box for creating a new template
            txtTemplateName.Text = selectedTemplate & "_Copy"
            txtTemplateName.SelectAll()
            txtTemplateName.Focus()

            toolStripStatusLabel1.Text = "Template copied - enter new name in text box and click Save to create copy"
        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error copying template: " & ex.Message
        End Try
    End Sub

    Private Sub btnLoadTemplate_Click(sender As Object, e As EventArgs) Handles btnLoadTemplate.Click
        If cmbTemplate.SelectedItem Is Nothing Then
            toolStripStatusLabel1.Text = "Error: Please select a template to load"
            Return
        End If

        ' Ensure we have a project folder and tree loaded
        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first using File > Select Project Folder"
            Return
        End If

        ' Show progress bar and disable button
        progressBar1.Visible = True
        progressBar1.Minimum = 0
        progressBar1.Maximum = 100
        progressBar1.Value = 0
        btnLoadTemplate.Enabled = False
        btnLoadTemplate.Text = "Loading..."

        Try
            ' Step 1: Initialize (10%)
            UpdateProgress(10, "Initializing template loading...")
            Application.DoEvents()

            ' Step 2: Ensure tree is loaded (20%)
            UpdateProgress(20, "Ensuring tree is loaded...")
            Application.DoEvents()
            EnsureTreeLoaded()

            ' Step 3: Start loading template (30%)
            UpdateProgress(30, "Reading template file...")
            Application.DoEvents()

            Dim selectedTemplate = cmbTemplate.SelectedItem.ToString

            ' Step 4: Clear current selections (40%)
            UpdateProgress(40, "Clearing current selections...")
            Application.DoEvents()

            ' Step 5: Load template with progress (40% - 90%)
            UpdateProgress(50, "Loading template: " & selectedTemplate)
            Application.DoEvents()

            ' Set flag to show detailed status for manual loading
            isLoadingTemplate = False
            LoadTemplateWithProgress(selectedTemplate)

            ' Step 6: Finalizing (95%)
            UpdateProgress(95, "Finalizing template load...")
            Application.DoEvents()

            ' Count loaded files
            Dim loadedFiles = GetCheckedFiles()

            ' Step 7: Complete (100%)
            UpdateProgress(100, "Template loaded successfully!")
            Application.DoEvents()

            toolStripStatusLabel1.Text = "Template '" & selectedTemplate & "' loaded manually - " & loadedFiles.Count & " files selected"

            ' Small delay to show completion
            System.Threading.Thread.Sleep(300)

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error loading template: " & ex.Message
        Finally
            ' Hide progress bar and restore button
            progressBar1.Visible = False
            btnLoadTemplate.Enabled = True
            btnLoadTemplate.Text = "Load"
        End Try
    End Sub

    Private Sub EnsureTreeLoaded()
        ' Make sure the tree view is loaded with current project folder
        If Not String.IsNullOrEmpty(projectFolder) AndAlso Directory.Exists(projectFolder) Then
            If treeView1.Nodes.Count = 0 Then
                LoadProjectFolder()
            Else
                ' Check if the root node matches current project folder
                If treeView1.Nodes.Count > 0 AndAlso treeView1.Nodes(0).Tag IsNot Nothing Then
                    Dim rootPath As String = treeView1.Nodes(0).Tag.ToString()
                    If Not String.Equals(rootPath, projectFolder, StringComparison.OrdinalIgnoreCase) Then
                        ' Store expansion state before reloading
                        Dim treeState As Dictionary(Of String, Boolean) = GetTreeExpansionState()
                        LoadProjectFolder() ' Reload if path changed
                        ' Restore expansion state if there was any
                        If treeState.Count > 0 Then
                            RestoreTreeExpansionState(treeState)
                        End If
                    End If
                Else
                    LoadProjectFolder() ' Reload if root node is invalid
                End If
            End If
        End If
    End Sub

    Private Sub LoadTemplateWithProgress(templateName As String)
        If Not File.Exists(templateFile) Then
            If Not isLoadingTemplate Then
                toolStripStatusLabel1.Text = "Error: Template file not found"
            End If
            Return
        End If

        ' Ensure project folder is loaded first
        If String.IsNullOrEmpty(projectFolder) OrElse Not Directory.Exists(projectFolder) Then
            If Not isLoadingTemplate Then
                toolStripStatusLabel1.Text = "Error: Project folder not found - please select project folder first"
            End If
            Return
        End If

        ' Ensure tree is loaded
        If treeView1.Nodes.Count = 0 Then
            LoadProjectFolder()
        End If

        Try
            ' Step 1: Clear current selection (50%)
            UpdateProgress(50, "Clearing current selections...")
            Application.DoEvents()
            ClearAllChecks(treeView1.Nodes)

            ' Step 2: Read template file (60%)
            UpdateProgress(60, "Reading template file...")
            Application.DoEvents()
            Dim content As String = File.ReadAllText(templateFile)
            Dim filesLoaded As Integer = 0
            Dim filesNotFound As Integer = 0
            Dim filesToLoad As New List(Of String)

            ' Step 3: Parse template sections (65%)
            UpdateProgress(65, "Parsing template sections...")
            Application.DoEvents()

            ' First pass: collect all files from the template
            Dim sections() As String = content.Split(New String() {"[TEMPLATE]"}, StringSplitOptions.RemoveEmptyEntries)

            For Each section In sections
                If Not String.IsNullOrWhiteSpace(section) Then
                    Dim lines() As String = section.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)
                    Dim foundTemplate As Boolean = False

                    ' Check if this section contains our template
                    For Each line In lines
                        Dim trimmedLine As String = line.Trim()
                        If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                            If trimmedLine.StartsWith("Name=") Then
                                Dim currentTemplateName As String = trimmedLine.Substring("Name=".Length).Trim()
                                If currentTemplateName.Equals(templateName, StringComparison.OrdinalIgnoreCase) Then
                                    foundTemplate = True
                                End If
                                Exit For
                            End If
                        End If
                    Next

                    ' If we found the right template, collect its files
                    If foundTemplate Then
                        For Each line In lines
                            Dim trimmedLine As String = line.Trim()
                            If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                                If trimmedLine.StartsWith("File=") Then
                                    Dim filePath As String = trimmedLine.Substring("File=".Length).Trim()
                                    If Not String.IsNullOrWhiteSpace(filePath) Then
                                        filesToLoad.Add(filePath)
                                    End If
                                End If
                            End If
                        Next
                        Exit For ' Found and processed the template, exit loop
                    End If
                End If
            Next

            ' Step 4: Process files with progress (70% - 90%)
            UpdateProgress(70, "Preparing to load " & filesToLoad.Count & " files...")
            Application.DoEvents()

            Dim progressStep As Double = 20.0 / Math.Max(filesToLoad.Count, 1) ' 20% range for file processing
            Dim fileIndex As Integer = 0

            For Each filePath In filesToLoad
                fileIndex += 1
                Dim currentProgress As Integer = 70 + CInt(fileIndex * progressStep)
                Dim fileName As String = Path.GetFileName(filePath)
                UpdateProgress(currentProgress, $"Loading file {fileIndex}/{filesToLoad.Count}: {fileName}")
                Application.DoEvents()

                If File.Exists(filePath) OrElse Directory.Exists(filePath) Then
                    ' Check if file is within project folder
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

                ' Small delay to show progress
                System.Threading.Thread.Sleep(10)
            Next

            UpdateTokenCount()

            ' Show status message
            If Not isLoadingTemplate Then
                If filesLoaded > 0 Then
                    If filesNotFound > 0 Then
                        toolStripStatusLabel1.Text = $"Template '{templateName}' loaded: {filesLoaded} files found, {filesNotFound} missing"
                    Else
                        toolStripStatusLabel1.Text = $"Template '{templateName}' loaded successfully: {filesLoaded} files selected"
                    End If
                Else
                    toolStripStatusLabel1.Text = $"Template '{templateName}' loaded but no files found in project"
                End If
            End If

        Catch ex As Exception
            If Not isLoadingTemplate Then
                toolStripStatusLabel1.Text = $"Error loading template '{templateName}': {ex.Message}"
            End If
        End Try
    End Sub

    Private Sub LoadTemplate(templateName As String)
        If Not File.Exists(templateFile) Then
            If Not isLoadingTemplate Then
                toolStripStatusLabel1.Text = "Error: Template file not found"
            End If
            Return
        End If

        ' Ensure project folder is loaded first
        If String.IsNullOrEmpty(projectFolder) OrElse Not Directory.Exists(projectFolder) Then
            If Not isLoadingTemplate Then
                toolStripStatusLabel1.Text = "Error: Project folder not found - please select project folder first"
            End If
            Return
        End If

        ' Ensure tree is loaded
        If treeView1.Nodes.Count = 0 Then
            LoadProjectFolder()
        End If

        Try
            ' Clear current selection first
            ClearAllChecks(treeView1.Nodes)

            Dim content As String = File.ReadAllText(templateFile)
            Dim filesLoaded As Integer = 0
            Dim filesNotFound As Integer = 0
            Dim filesToLoad As New List(Of String)

            ' First pass: collect all files from the template
            Dim sections() As String = content.Split(New String() {"[TEMPLATE]"}, StringSplitOptions.RemoveEmptyEntries)

            For Each section In sections
                If Not String.IsNullOrWhiteSpace(section) Then
                    Dim lines() As String = section.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)
                    Dim foundTemplate As Boolean = False

                    ' Check if this section contains our template
                    For Each line In lines
                        Dim trimmedLine As String = line.Trim()
                        If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                            If trimmedLine.StartsWith("Name=") Then
                                Dim currentTemplateName As String = trimmedLine.Substring("Name=".Length).Trim()
                                If currentTemplateName.Equals(templateName, StringComparison.OrdinalIgnoreCase) Then
                                    foundTemplate = True
                                End If
                                Exit For
                            End If
                        End If
                    Next

                    ' If we found the right template, collect its files
                    If foundTemplate Then
                        For Each line In lines
                            Dim trimmedLine As String = line.Trim()
                            If Not String.IsNullOrWhiteSpace(trimmedLine) AndAlso Not trimmedLine.StartsWith("#") Then
                                If trimmedLine.StartsWith("File=") Then
                                    Dim filePath As String = trimmedLine.Substring("File=".Length).Trim()
                                    If Not String.IsNullOrWhiteSpace(filePath) Then
                                        filesToLoad.Add(filePath)
                                    End If
                                End If
                            End If
                        Next
                        Exit For ' Found and processed the template, exit loop
                    End If
                End If
            Next

            ' Second pass: try to select each file
            For Each filePath In filesToLoad
                If File.Exists(filePath) OrElse Directory.Exists(filePath) Then
                    ' Check if file is within project folder
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
            Next

            UpdateTokenCount()

            ' Show status message
            If Not isLoadingTemplate Then
                If filesLoaded > 0 Then
                    If filesNotFound > 0 Then
                        toolStripStatusLabel1.Text = $"Template '{templateName}' loaded: {filesLoaded} files found, {filesNotFound} missing"
                    Else
                        toolStripStatusLabel1.Text = $"Template '{templateName}' loaded successfully: {filesLoaded} files selected"
                    End If
                Else
                    toolStripStatusLabel1.Text = $"Template '{templateName}' loaded but no files found in project"
                End If
            End If

        Catch ex As Exception
            If Not isLoadingTemplate Then
                toolStripStatusLabel1.Text = $"Error loading template '{templateName}': {ex.Message}"
            End If
        End Try
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
        ' Prevent recursive calls during template loading
        If isLoadingTemplate Then Return

        ' Enable/disable buttons based on selection
        btnLoadTemplate.Enabled = (cmbTemplate.SelectedItem IsNot Nothing)
        btnCopyTemplate.Enabled = (cmbTemplate.SelectedItem IsNot Nothing)
        btnUpdateTemplate.Enabled = (cmbTemplate.SelectedItem IsNot Nothing)

        If cmbTemplate.SelectedItem IsNot Nothing Then
            toolStripStatusLabel1.Text = "Template selected: " & cmbTemplate.SelectedItem.ToString()

            ' Auto-load the selected template
            AutoLoadTemplate(cmbTemplate.SelectedItem.ToString())
        Else
            toolStripStatusLabel1.Text = "No template selected"
        End If
    End Sub

    Private Sub AutoLoadTemplate(templateName As String)
        ' Only auto-load if we have a project folder
        If String.IsNullOrEmpty(projectFolder) OrElse Not Directory.Exists(projectFolder) Then
            Return
        End If

        ' Ensure tree is loaded
        EnsureTreeLoaded()

        Try
            isLoadingTemplate = True
            LoadTemplate(templateName)

            ' Count loaded files for status
            Dim loadedFiles As List(Of String) = GetCheckedFiles()
            toolStripStatusLabel1.Text = "Template '" & templateName & "' auto-loaded - " & loadedFiles.Count & " files selected"

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error auto-loading template: " & ex.Message
        Finally
            isLoadingTemplate = False
        End Try
    End Sub

    Private Sub btnCombine_Click(sender As Object, e As EventArgs) Handles btnCombine.Click
        If String.IsNullOrWhiteSpace(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first"
            Return
        End If

        If String.IsNullOrWhiteSpace(outputFolder) Then
            toolStripStatusLabel1.Text = "Error: Please set output folder in Settings > Application Settings"
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

        ' Ensure output folder exists and is writable
        Try
            If Not Directory.Exists(outputFolder) Then
                Directory.CreateDirectory(outputFolder)
                toolStripStatusLabel1.Text = "Created output folder: " & outputFolder
                Application.DoEvents()
            End If

            ' Test write permissions by creating a temporary file
            Dim testFile As String = Path.Combine(outputFolder, "test_write_permission.tmp")
            File.WriteAllText(testFile, "test")
            File.Delete(testFile)

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error: Cannot write to output folder - " & ex.Message
            Return
        End Try

        ' Show and initialize progress bar
        progressBar1.Visible = True
        progressBar1.Minimum = 0
        progressBar1.Maximum = 100 ' Use percentage
        progressBar1.Value = 0
        progressBar1.Style = ProgressBarStyle.Continuous

        ' Disable the combine button to prevent multiple operations
        btnCombine.Enabled = False
        btnCombine.Text = "Combining..."

        Try
            ' Step 1: Initialize (5%)
            UpdateProgress(5, "Initializing...")
            Application.DoEvents()

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

            ' Step 2: Preparation (10%)
            UpdateProgress(10, "Preparing file list...")
            Application.DoEvents()

            ' Step 3: Start combining (15%)
            UpdateProgress(15, "Starting file combination...")
            Application.DoEvents()

            Dim combiner As New FileCombiner(projectFolder, outputFolder)

            ' Step 4: Processing files (15% - 90%)
            Dim result As CombineResult = CombineFilesWithProgress(combiner, checkedFiles, allSqlFiles)

            ' Step 5: Finalizing (95%)
            UpdateProgress(95, "Finalizing output files...")
            Application.DoEvents()

            ' Step 6: Complete (100%)
            UpdateProgress(100, "Combination complete!")
            Application.DoEvents()

            If result.Success Then
                If allSqlFiles.Count > 0 Then
                    toolStripStatusLabel1.Text = result.Message & $" (Including {allSqlFiles.Count} SQL file(s))"
                Else
                    toolStripStatusLabel1.Text = result.Message
                End If
            Else
                toolStripStatusLabel1.Text = "Error: " & result.Message
            End If

            ' Small delay to show completion
            System.Threading.Thread.Sleep(500)

        Catch ex As Exception
            toolStripStatusLabel1.Text = "Error combining files: " & ex.Message
        Finally
            ' Hide progress bar and restore button
            progressBar1.Visible = False
            btnCombine.Enabled = True
            btnCombine.Text = "🔗 Combine Files"
        End Try
    End Sub

    Private Sub UpdateProgress(percentage As Integer, message As String)
        ' Ensure percentage is within bounds
        percentage = Math.Max(0, Math.Min(100, percentage))

        progressBar1.Value = percentage
        toolStripStatusLabel1.Text = message

        ' Also update the status strip progress bar if it exists
        If toolStripProgressBar1 IsNot Nothing Then
            toolStripProgressBar1.Visible = True
            toolStripProgressBar1.Value = percentage
        End If
    End Sub

    Private Function CombineFilesWithProgress(combiner As FileCombiner, checkedFiles As List(Of String), allSqlFiles As List(Of String)) As CombineResult
        ' Filter files that will actually be processed
        Dim filesToProcess As New List(Of String)
        For Each filePath In checkedFiles
            If File.Exists(filePath) Then
                filesToProcess.Add(filePath)
            End If
        Next

        ' Process files with progress updates
        Dim processedCount As Integer = 0
        Dim totalFiles As Integer = filesToProcess.Count

        ' Create a progress callback
        Dim progressStep As Double = 75.0 / totalFiles ' 75% of progress for file processing (15% to 90%)
        Dim baseProgress As Integer = 15

        ' Simulate file processing progress by updating during the combine operation
        For i As Integer = 0 To totalFiles - 1
            Dim currentProgress As Integer = baseProgress + CInt(i * progressStep)
            Dim fileName As String = Path.GetFileName(filesToProcess(i))
            UpdateProgress(currentProgress, $"Processing {i + 1}/{totalFiles}: {fileName}")
            Application.DoEvents()

            ' Small delay to make progress visible (remove in production if too slow)
            System.Threading.Thread.Sleep(25)
        Next

        ' Now perform the actual file combination
        UpdateProgress(85, "Combining all files...")
        Application.DoEvents()

        Return combiner.CombineFiles(
        checkedFiles,
        cmbProjectType.SelectedItem.ToString(),
        treeView1.Nodes,
        txtProjectTitle.Text.Trim(),
        "",
        "",
        allSqlFiles.Count > 0,
        allSqlFiles
    )
    End Function

    Private Sub cmbProjectType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProjectType.SelectedIndexChanged
        ' Update token count
        UpdateTokenCount()

        ' Save the selected project type to configuration
        SaveConfiguration()

        ' Update status to show the selection was saved
        If cmbProjectType.SelectedItem IsNot Nothing Then
            toolStripStatusLabel1.Text = "Project type changed to: " & cmbProjectType.SelectedItem.ToString()
        End If
    End Sub

    Private Sub settingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles settingsToolStripMenuItem.Click
        Dim settingsForm As New frmSettings()
        settingsForm.ProjectFolderPath = projectFolder
        settingsForm.OutputFolderPath = outputFolder
        settingsForm.DatabaseFiles = New List(Of String)(databaseFiles)
        settingsForm.IncludeDatabase = includeDatabase

        If settingsForm.ShowDialog() = DialogResult.OK Then
            projectFolder = settingsForm.ProjectFolderPath
            outputFolder = settingsForm.OutputFolderPath
            databaseFiles = settingsForm.DatabaseFiles
            includeDatabase = settingsForm.IncludeDatabase

            SaveConfiguration()
            LoadProjectFolder()

            toolStripStatusLabel1.Text = $"Settings updated successfully - {databaseFiles.Count} SQL file(s) configured"
        End If
    End Sub

    Private Sub btnRefreshTree_Click(sender As Object, e As EventArgs) Handles btnRefreshTree.Click
        If String.IsNullOrEmpty(projectFolder) Then
            toolStripStatusLabel1.Text = "Error: Please select a project folder first"
            Return
        End If

        RefreshTreeView()
        toolStripStatusLabel1.Text = "Tree view refreshed successfully"
    End Sub
End Class