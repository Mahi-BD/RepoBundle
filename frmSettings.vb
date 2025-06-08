Imports System.IO
Imports System.Text.RegularExpressions

Public Class frmSettings
    Public Property ProjectFolderPath As String = ""
    Public Property OutputFolderPath As String = ""
    Public Property DatabaseFiles As New List(Of String)
    Public Property IncludeDatabase As Boolean = False
    Public Property ExcludedFolders As New List(Of String)

    ' File extension management with regex support
    Public Property VBDesktopExtensions As New List(Of String)
    Public Property AspCoreExtensions As New List(Of String)
    Public Property AspMvcExtensions As New List(Of String)

    ' Default excluded folders
    Private ReadOnly DefaultExcludedFolders As String() = {".git", ".vs", ".svn", "bin", "obj", "packages", "node_modules"}

    ' Default file extensions with regex patterns
    Private ReadOnly DefaultVBExtensions As String() = {"*.vb", "*.designer.vb", "*.vbproj", "*.resx", "*.config", "*.sql"}
    Private ReadOnly DefaultAspCoreExtensions As String() = {"*.cs", "^_.*\.cshtml$", "site.css", "site.js", "*.json", "*.sql"}
    Private ReadOnly DefaultAspMvcExtensions As String() = {"*.cs", "*.cshtml", "*.css", "*.js", "*.config", "*.sql"}

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize extension lists if empty
        If VBDesktopExtensions.Count = 0 Then
            VBDesktopExtensions.AddRange(DefaultVBExtensions)
        End If
        If AspCoreExtensions.Count = 0 Then
            AspCoreExtensions.AddRange(DefaultAspCoreExtensions)
        End If
        If AspMvcExtensions.Count = 0 Then
            AspMvcExtensions.AddRange(DefaultAspMvcExtensions)
        End If

        ' Load current settings into the form
        txtProjectFolder.Text = ProjectFolderPath
        txtOutputFolder.Text = OutputFolderPath

        ' Initialize excluded folders if empty
        If ExcludedFolders.Count = 0 Then
            ExcludedFolders.AddRange(DefaultExcludedFolders)
        End If

        ' Display excluded folders
        UpdateExcludedFoldersList()

        ' Display selected database files
        UpdateDatabaseFilesList()
        chkIncludeDatabase.Checked = IncludeDatabase

        ' Enable/disable database controls based on checkbox
        UpdateDatabaseControls()

        ' Initialize project type combo
        cmbProjectTypeSettings.Items.AddRange({"Visual Basic Desktop", "Asp MVC 5", "Asp Dotnet Core 8"})
        cmbProjectTypeSettings.SelectedIndex = 0

        ' Update extensions list
        UpdateExtensionsList()

        ' Add event handlers
        AddHandler txtNewExcludedFolder.KeyDown, AddressOf txtNewExcludedFolder_KeyDown
        AddHandler txtNewExtension.KeyDown, AddressOf txtNewExtension_KeyDown
        AddHandler cmbProjectTypeSettings.SelectedIndexChanged, AddressOf cmbProjectTypeSettings_SelectedIndexChanged

        ' Add tooltip for regex help
        SetupExtensionTooltips()
    End Sub

    Private Sub SetupExtensionTooltips()
        Dim toolTip As New ToolTip()
        toolTip.AutoPopDelay = 10000
        toolTip.InitialDelay = 1000
        toolTip.ReshowDelay = 500
        toolTip.ShowAlways = True

        ' Enhanced tooltip for extension help
        Dim tooltipText As String = "Extension Pattern Examples:" & vbCrLf &
                                  "• *.css - All CSS files" & vbCrLf &
                                  "• site.css - Specific file name" & vbCrLf &
                                  "• ^_.*\.cshtml$ - CSHTML files starting with underscore" & vbCrLf &
                                  "• ^(?!_).*\.cshtml$ - CSHTML files NOT starting with underscore" & vbCrLf &
                                  "• .*\.min\.js$ - All minified JS files" & vbCrLf &
                                  "• Use ^ for start, $ for end, .* for any characters"

        toolTip.SetToolTip(lblExtensionHelp, tooltipText)
        toolTip.SetToolTip(txtNewExtension, tooltipText)
    End Sub

    Private Sub cmbProjectTypeSettings_SelectedIndexChanged(sender As Object, e As EventArgs)
        UpdateExtensionsList()
        UpdateExtensionHelpText()
    End Sub

    Private Sub UpdateExtensionHelpText()
        Select Case cmbProjectTypeSettings.SelectedIndex
            Case 0 ' Visual Basic Desktop
                lblExtensionHelp.Text = "VB: Use *.ext for wildcards or filename.ext for specific files"
            Case 1 ' Asp MVC 5
                lblExtensionHelp.Text = "MVC: Use *.ext for wildcards or filename.ext for specific files"
            Case 2 ' Asp Dotnet Core 8
                lblExtensionHelp.Text = "Core: Use regex like ^_.*\.cshtml$ for underscore files, *.ext for wildcards"
        End Select
    End Sub

    Private Sub UpdateExtensionsList()
        lstExtensions.Items.Clear()

        Select Case cmbProjectTypeSettings.SelectedIndex
            Case 0 ' Visual Basic Desktop
                For Each ext In VBDesktopExtensions
                    lstExtensions.Items.Add(ext)
                Next
            Case 1 ' Asp MVC 5
                For Each ext In AspMvcExtensions
                    lstExtensions.Items.Add(ext)
                Next
            Case 2 ' Asp Dotnet Core 8
                For Each ext In AspCoreExtensions
                    lstExtensions.Items.Add(ext)
                Next
        End Select
    End Sub

    Private Sub btnAddExtension_Click(sender As Object, e As EventArgs) Handles btnAddExtension.Click
        AddExtension()
    End Sub

    Private Sub txtNewExtension_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            AddExtension()
        End If
    End Sub

    Private Sub AddExtension()
        Dim extension As String = txtNewExtension.Text.Trim()

        If String.IsNullOrWhiteSpace(extension) Then
            MessageBox.Show("Please enter a file extension or pattern.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewExtension.Focus()
            Return
        End If

        ' Validate regex pattern if it looks like regex
        If IsRegexPattern(extension) Then
            If Not ValidateRegexPattern(extension) Then
                MessageBox.Show("Invalid regex pattern. Please check your syntax.", "Invalid Regex", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtNewExtension.SelectAll()
                txtNewExtension.Focus()
                Return
            End If
        Else
            ' Auto-format simple patterns
            If Not extension.StartsWith("*") AndAlso Not extension.Contains(".") Then
                extension = "*." + extension
            End If
        End If

        Dim currentList As List(Of String) = GetCurrentExtensionList()

        If currentList.Contains(extension) Then
            MessageBox.Show($"Extension '{extension}' already exists.", "Duplicate Extension", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtNewExtension.SelectAll()
            txtNewExtension.Focus()
            Return
        End If

        currentList.Add(extension)
        UpdateExtensionsList()
        txtNewExtension.Clear()
        txtNewExtension.Focus()

        ' Show success message for regex patterns
        If IsRegexPattern(extension) Then
            MessageBox.Show($"Regex pattern '{extension}' added successfully!", "Pattern Added", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function IsRegexPattern(pattern As String) As Boolean
        ' Check if the pattern contains regex metacharacters
        Return pattern.Contains("^") OrElse pattern.Contains("$") OrElse
               pattern.Contains("(") OrElse pattern.Contains(")") OrElse
               pattern.Contains("[") OrElse pattern.Contains("]") OrElse
               pattern.Contains("?") OrElse pattern.Contains("+") OrElse
               (pattern.Contains(".") AndAlso pattern.Contains("\"))
    End Function

    Private Function ValidateRegexPattern(pattern As String) As Boolean
        Try
            ' Test the regex pattern
            Dim regex As New Regex(pattern)
            ' Test with some sample filenames
            regex.IsMatch("_Layout.cshtml")
            regex.IsMatch("Create.cshtml")
            regex.IsMatch("site.css")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub btnRemoveExtension_Click(sender As Object, e As EventArgs) Handles btnRemoveExtension.Click
        If lstExtensions.SelectedIndex >= 0 Then
            Dim selectedExtension As String = lstExtensions.SelectedItem.ToString()
            Dim currentList As List(Of String) = GetCurrentExtensionList()

            currentList.Remove(selectedExtension)
            UpdateExtensionsList()
        Else
            MessageBox.Show("Please select an extension to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnResetExtensions_Click(sender As Object, e As EventArgs) Handles btnResetExtensions.Click
        Dim projectType As String = cmbProjectTypeSettings.SelectedItem.ToString()
        Dim result As DialogResult = MessageBox.Show($"Reset extensions for '{projectType}' to default values?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Select Case cmbProjectTypeSettings.SelectedIndex
                Case 0 ' Visual Basic Desktop
                    VBDesktopExtensions.Clear()
                    VBDesktopExtensions.AddRange(DefaultVBExtensions)
                Case 1 ' Asp MVC 5
                    AspMvcExtensions.Clear()
                    AspMvcExtensions.AddRange(DefaultAspMvcExtensions)
                Case 2 ' Asp Dotnet Core 8
                    AspCoreExtensions.Clear()
                    AspCoreExtensions.AddRange(DefaultAspCoreExtensions)
            End Select
            UpdateExtensionsList()
        End If
    End Sub

    Private Function GetCurrentExtensionList() As List(Of String)
        Select Case cmbProjectTypeSettings.SelectedIndex
            Case 0 ' Visual Basic Desktop
                Return VBDesktopExtensions
            Case 1 ' Asp MVC 5
                Return AspMvcExtensions
            Case 2 ' Asp Dotnet Core 8
                Return AspCoreExtensions
            Case Else
                Return New List(Of String)
        End Select
    End Function

    Private Sub UpdateExcludedFoldersList()
        ' Clear and update the excluded folders display
        lstExcludedFolders.Items.Clear()
        For Each folder In ExcludedFolders
            lstExcludedFolders.Items.Add(folder)
        Next
    End Sub

    Private Sub UpdateDatabaseFilesList()
        ' Clear and update the database files display
        lstDatabaseFiles.Items.Clear()
        For Each filePath In DatabaseFiles
            lstDatabaseFiles.Items.Add(Path.GetFileName(filePath) & " - " & filePath)
        Next

        ' Update label to show count
        lblDatabaseFiles.Text = $"Selected SQL Files ({DatabaseFiles.Count}):"
    End Sub

    Private Sub btnBrowseProject_Click(sender As Object, e As EventArgs) Handles btnBrowseProject.Click
        folderBrowserDialog1.Description = "Select Project Folder"
        folderBrowserDialog1.ShowNewFolderButton = True

        If Not String.IsNullOrEmpty(txtProjectFolder.Text) AndAlso Directory.Exists(txtProjectFolder.Text) Then
            folderBrowserDialog1.SelectedPath = txtProjectFolder.Text
        End If

        If folderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            txtProjectFolder.Text = folderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub btnBrowseOutput_Click(sender As Object, e As EventArgs) Handles btnBrowseOutput.Click
        folderBrowserDialog1.Description = "Select Output Folder"
        folderBrowserDialog1.ShowNewFolderButton = True

        If Not String.IsNullOrEmpty(txtOutputFolder.Text) AndAlso Directory.Exists(txtOutputFolder.Text) Then
            folderBrowserDialog1.SelectedPath = txtOutputFolder.Text
        End If

        If folderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            txtOutputFolder.Text = folderBrowserDialog1.SelectedPath
        End If
    End Sub

    ' === EXCLUDED FOLDERS MANAGEMENT ===
    Private Sub btnAddExcluded_Click(sender As Object, e As EventArgs) Handles btnAddExcluded.Click
        AddExcludedFolder()
    End Sub

    Private Sub txtNewExcludedFolder_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            AddExcludedFolder()
        End If
    End Sub

    Private Sub AddExcludedFolder()
        Dim folderName As String = txtNewExcludedFolder.Text.Trim()

        If String.IsNullOrWhiteSpace(folderName) Then
            MessageBox.Show("Please enter a folder name to exclude.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewExcludedFolder.Focus()
            Return
        End If

        ' Remove any path separators and get just the folder name
        folderName = Path.GetFileName(folderName.Replace("/", "\").TrimEnd("\"c))

        If String.IsNullOrWhiteSpace(folderName) Then
            MessageBox.Show("Please enter a valid folder name.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewExcludedFolder.Focus()
            Return
        End If

        ' Check if already exists (case insensitive)
        If ExcludedFolders.Any(Function(f) String.Equals(f, folderName, StringComparison.OrdinalIgnoreCase)) Then
            MessageBox.Show($"Folder '{folderName}' is already in the excluded list.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtNewExcludedFolder.SelectAll()
            txtNewExcludedFolder.Focus()
            Return
        End If

        ' Add to list
        ExcludedFolders.Add(folderName)
        UpdateExcludedFoldersList()

        ' Clear input and focus
        txtNewExcludedFolder.Clear()
        txtNewExcludedFolder.Focus()
    End Sub

    Private Sub btnRemoveExcluded_Click(sender As Object, e As EventArgs) Handles btnRemoveExcluded.Click
        If lstExcludedFolders.SelectedIndex >= 0 Then
            Dim selectedIndex As Integer = lstExcludedFolders.SelectedIndex
            Dim folderName As String = ExcludedFolders(selectedIndex)

            ' Confirm removal
            Dim result As DialogResult = MessageBox.Show($"Are you sure you want to remove '{folderName}' from the excluded folders list?",
                                                        "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                ExcludedFolders.RemoveAt(selectedIndex)
                UpdateExcludedFoldersList()

                ' Select next item or previous if we removed the last one
                If lstExcludedFolders.Items.Count > 0 Then
                    If selectedIndex < lstExcludedFolders.Items.Count Then
                        lstExcludedFolders.SelectedIndex = selectedIndex
                    Else
                        lstExcludedFolders.SelectedIndex = lstExcludedFolders.Items.Count - 1
                    End If
                End If
            End If
        Else
            MessageBox.Show("Please select a folder to remove from the excluded list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnResetExcluded_Click(sender As Object, e As EventArgs) Handles btnResetExcluded.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to reset the excluded folders list to default values?" & vbCrLf & vbCrLf &
                                                    "Default folders: " & String.Join(", ", DefaultExcludedFolders),
                                                    "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ExcludedFolders.Clear()
            ExcludedFolders.AddRange(DefaultExcludedFolders)
            UpdateExcludedFoldersList()
        End If
    End Sub

    ' === DATABASE FILES MANAGEMENT ===
    Private Sub btnBrowseDatabase_Click(sender As Object, e As EventArgs) Handles btnBrowseDatabase.Click
        ' Configure for multiple SQL files only
        openFileDialog1.Filter = "SQL Script Files|*.sql"
        openFileDialog1.Title = "Select SQL Database Files"
        openFileDialog1.Multiselect = True

        If DatabaseFiles.Count > 0 Then
            ' Set initial directory to the first selected file's directory
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(DatabaseFiles(0))
        End If

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Add selected files to the list (avoid duplicates)
            For Each selectedFile In openFileDialog1.FileNames
                If Not DatabaseFiles.Contains(selectedFile) Then
                    DatabaseFiles.Add(selectedFile)
                End If
            Next

            UpdateDatabaseFilesList()
        End If
    End Sub

    Private Sub btnRemoveSelected_Click(sender As Object, e As EventArgs) Handles btnRemoveSelected.Click
        ' Remove selected items from the list
        If lstDatabaseFiles.SelectedIndex >= 0 Then
            Dim indexToRemove As Integer = lstDatabaseFiles.SelectedIndex
            DatabaseFiles.RemoveAt(indexToRemove)
            UpdateDatabaseFilesList()
        End If
    End Sub

    Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
        ' Clear all database files
        If DatabaseFiles.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show($"Are you sure you want to remove all {DatabaseFiles.Count} SQL files from the list?",
                                                        "Confirm Clear All", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                DatabaseFiles.Clear()
                UpdateDatabaseFilesList()
            End If
        Else
            MessageBox.Show("The SQL files list is already empty.", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub chkIncludeDatabase_CheckedChanged(sender As Object, e As EventArgs) Handles chkIncludeDatabase.CheckedChanged
        UpdateDatabaseControls()
    End Sub

    Private Sub UpdateDatabaseControls()
        Dim enabled As Boolean = chkIncludeDatabase.Checked
        lstDatabaseFiles.Enabled = enabled
        btnBrowseDatabase.Enabled = enabled
        btnRemoveSelected.Enabled = enabled
        btnClearAll.Enabled = enabled
        lblDatabaseFiles.Enabled = enabled
    End Sub

    ' === FORM VALIDATION AND SAVE ===
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        ' Validate inputs
        If String.IsNullOrWhiteSpace(txtProjectFolder.Text) Then
            MessageBox.Show("Please select a project folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtProjectFolder.Focus()
            Return
        End If

        If Not Directory.Exists(txtProjectFolder.Text.Trim()) Then
            MessageBox.Show("The specified project folder does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtProjectFolder.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtOutputFolder.Text) Then
            MessageBox.Show("Please select an output folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtOutputFolder.Focus()
            Return
        End If

        ' Validate excluded folders
        If ExcludedFolders.Count = 0 Then
            Dim result As DialogResult = MessageBox.Show("You have no excluded folders. This means ALL folders will be included in backups." & vbCrLf & vbCrLf &
                                                        "Do you want to continue?", "No Excluded Folders", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.No Then
                Return
            End If
        End If

        ' Validate regex patterns in extensions
        If Not ValidateAllExtensionPatterns() Then
            Return
        End If

        ' Validate database settings
        If chkIncludeDatabase.Checked Then
            If DatabaseFiles.Count = 0 Then
                MessageBox.Show("Please select at least one SQL database file, or uncheck 'Include SQL Database Files'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                btnBrowseDatabase.Focus()
                Return
            End If

            ' Validate that all selected files exist and are SQL files
            For Each filePath In DatabaseFiles.ToList()
                If Not File.Exists(filePath) Then
                    Dim result As DialogResult = MessageBox.Show($"Database file not found: {filePath}" & vbCrLf & vbCrLf &
                                                               "Do you want to remove it from the list and continue?", "Missing File", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    If result = DialogResult.Yes Then
                        DatabaseFiles.Remove(filePath)
                        UpdateDatabaseFilesList()
                    Else
                        Return
                    End If
                End If

                If Path.GetExtension(filePath).ToLower() <> ".sql" Then
                    MessageBox.Show($"Only SQL files are supported: {filePath}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            Next

            ' Check again after cleanup
            If DatabaseFiles.Count = 0 Then
                MessageBox.Show("No valid SQL files remain. Please add SQL files or uncheck 'Include SQL Database Files'.", "No Valid Files", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        ' Save settings
        ProjectFolderPath = txtProjectFolder.Text.Trim()
        OutputFolderPath = txtOutputFolder.Text.Trim()
        IncludeDatabase = chkIncludeDatabase.Checked

        ' Ensure output folder exists
        Try
            If Not Directory.Exists(OutputFolderPath) Then
                Directory.CreateDirectory(OutputFolderPath)
            End If
        Catch ex As Exception
            MessageBox.Show($"Could not create output folder: {ex.Message}", "Folder Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtOutputFolder.Focus()
            Return
        End Try

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Function ValidateAllExtensionPatterns() As Boolean
        ' Validate all extension lists for regex patterns
        Dim allExtensions As New List(Of String)
        allExtensions.AddRange(VBDesktopExtensions)
        allExtensions.AddRange(AspCoreExtensions)
        allExtensions.AddRange(AspMvcExtensions)

        For Each pattern In allExtensions
            If IsRegexPattern(pattern) Then
                If Not ValidateRegexPattern(pattern) Then
                    MessageBox.Show($"Invalid regex pattern found: '{pattern}'" & vbCrLf & vbCrLf &
                                  "Please fix or remove this pattern before saving.", "Invalid Regex Pattern", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class