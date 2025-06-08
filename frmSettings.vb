Imports System.IO

Public Class frmSettings
    Public Property ProjectFolderPath As String = ""
    Public Property OutputFolderPath As String = ""
    Public Property DatabaseFiles As New List(Of String)
    Public Property IncludeDatabase As Boolean = False
    Public Property ExcludedFolders As New List(Of String)

    ' Default excluded folders
    Private ReadOnly DefaultExcludedFolders As String() = {".git", ".vs", ".svn", "repobundle", "Resource"}

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ' Add event handlers
        AddHandler txtNewExcludedFolder.KeyDown, AddressOf txtNewExcludedFolder_KeyDown
    End Sub

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

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    ' === HELPER METHODS ===
    Public Sub LoadExcludedFoldersFromString(excludedFoldersString As String)
        ExcludedFolders.Clear()
        If Not String.IsNullOrWhiteSpace(excludedFoldersString) Then
            Dim folders() As String = excludedFoldersString.Split(New Char() {","c, ";"c}, StringSplitOptions.RemoveEmptyEntries)
            For Each folder In folders
                Dim trimmedFolder As String = folder.Trim()
                If Not String.IsNullOrWhiteSpace(trimmedFolder) Then
                    ExcludedFolders.Add(trimmedFolder)
                End If
            Next
        End If

        ' Ensure we have at least the default folders
        If ExcludedFolders.Count = 0 Then
            ExcludedFolders.AddRange(DefaultExcludedFolders)
        End If
    End Sub

    Public Function GetExcludedFoldersAsString() As String
        Return String.Join(",", ExcludedFolders)
    End Function

End Class