Imports System.IO

Public Class frmSettings
    Public Property ProjectFolderPath As String = ""
    Public Property OutputFolderPath As String = ""
    Public Property DatabaseFiles As New List(Of String) ' Changed to support multiple files
    Public Property IncludeDatabase As Boolean = False

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load current settings into the form
        txtProjectFolder.Text = ProjectFolderPath
        txtOutputFolder.Text = OutputFolderPath

        ' Display selected database files
        UpdateDatabaseFilesList()
        chkIncludeDatabase.Checked = IncludeDatabase

        ' Enable/disable database controls based on checkbox
        UpdateDatabaseControls()
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

    Private Sub btnBrowseDatabase_Click(sender As Object, e As EventArgs) Handles btnBrowseDatabase.Click
        ' Configure for multiple SQL files only
        openFileDialog1.Filter = "SQL Script Files|*.sql"
        openFileDialog1.Title = "Select SQL Database Files"
        openFileDialog1.Multiselect = True ' Enable multiple file selection

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
        DatabaseFiles.Clear()
        UpdateDatabaseFilesList()
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

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        ' Validate inputs
        If String.IsNullOrWhiteSpace(txtProjectFolder.Text) Then
            MessageBox.Show("Please select a project folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtProjectFolder.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtOutputFolder.Text) Then
            MessageBox.Show("Please select an output folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtOutputFolder.Focus()
            Return
        End If

        If chkIncludeDatabase.Checked Then
            If DatabaseFiles.Count = 0 Then
                MessageBox.Show("Please select at least one SQL database file.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                btnBrowseDatabase.Focus()
                Return
            End If

            ' Validate that all selected files exist and are SQL files
            For Each filePath In DatabaseFiles.ToList() ' Use ToList to avoid modification during iteration
                If Not File.Exists(filePath) Then
                    MessageBox.Show($"Database file not found: {filePath}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    DatabaseFiles.Remove(filePath) ' Remove non-existent files
                    UpdateDatabaseFilesList()
                    Return
                End If

                If Path.GetExtension(filePath).ToLower() <> ".sql" Then
                    MessageBox.Show($"Only SQL files are supported: {filePath}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    DatabaseFiles.Remove(filePath) ' Remove non-SQL files
                    UpdateDatabaseFilesList()
                    Return
                End If
            Next
        End If

        ' Save settings
        ProjectFolderPath = txtProjectFolder.Text.Trim()
        OutputFolderPath = txtOutputFolder.Text.Trim()
        IncludeDatabase = chkIncludeDatabase.Checked

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class