Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class FileCombiner
    Private projectFolder As String
    Private outputFolder As String
    Private fileDelimiter As String = "==================== FILE: {0} ===================="

    ' Dynamic extension lists (loaded from settings)
    Private vbDesktopExtensions As List(Of String)
    Private aspCoreExtensions As List(Of String)
    Private aspMvcExtensions As List(Of String)

    ' Media extensions that should never be included
    Private ReadOnly mediaExtensions As String() = {".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".webp", ".avif", ".apng",
                                          ".heif", ".heic", ".svg", ".pdf", ".eps", ".ai", ".cdr", ".wmf", ".emf", ".raw",
                                          ".cr2", ".nef", ".orf", ".rw2", ".dng", ".wav", ".aiff", ".au", ".flac", ".alac",
                                          ".ape", ".wv", ".mp3", ".aac", ".m4a", ".ogg", ".opus", ".wma", ".amr", ".mp4",
                                          ".m4v", ".mov", ".avi", ".wmv", ".flv", ".webm", ".mkv", ".mpg", ".mpeg", ".mp2",
                                          ".mpe", ".mpv", ".3gp", ".3g2", ".ts", ".mts", ".m2ts", ".ogv", ".rm", ".rmvb", ".asf"}

    Public Sub New(projectFolderPath As String, outputFolderPath As String)
        projectFolder = projectFolderPath
        outputFolder = outputFolderPath
        LoadExtensionsFromSettings()
    End Sub

    Private Sub LoadExtensionsFromSettings()
        ' Load extensions from INI file
        Dim iniHelper As New IniHelper(Path.Combine(Application.StartupPath, "config.ini"))

        ' Initialize with defaults if not found in config
        vbDesktopExtensions = New List(Of String)
        aspCoreExtensions = New List(Of String)
        aspMvcExtensions = New List(Of String)

        ' Load VB Desktop extensions
        Dim vbExtString As String = iniHelper.ReadValue("Extensions", "VBDesktop", "*.vb,*.designer.vb,*.vbproj,*.resx,*.config,*.sql")
        vbDesktopExtensions.AddRange(vbExtString.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) Not String.IsNullOrEmpty(x)))

        ' Load ASP.NET Core extensions with special rules
        Dim coreExtString As String = iniHelper.ReadValue("Extensions", "AspCore", "*.cs,_*.cshtml,site.css,site.js,*.json,*.sql")
        aspCoreExtensions.AddRange(coreExtString.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) Not String.IsNullOrEmpty(x)))

        ' Load ASP.NET MVC extensions
        Dim mvcExtString As String = iniHelper.ReadValue("Extensions", "AspMvc", "*.cs,*.cshtml,*.css,*.js,*.config,*.sql")
        aspMvcExtensions.AddRange(mvcExtString.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) Not String.IsNullOrEmpty(x)))
    End Sub

    Public Function CombineFiles(checkedFiles As List(Of String), projectType As String, treeNodes As TreeNodeCollection, Optional projectTitle As String = "", Optional databasePath As String = "", Optional databaseName As String = "", Optional includeDatabase As Boolean = False, Optional sqlFiles As List(Of String) = Nothing) As CombineResult
        Try
            ' Validate inputs
            If checkedFiles Is Nothing OrElse checkedFiles.Count = 0 Then
                Return New CombineResult(False, "No files selected for combining")
            End If

            If String.IsNullOrWhiteSpace(outputFolder) Then
                Return New CombineResult(False, "Output folder not specified")
            End If

            If String.IsNullOrWhiteSpace(projectType) Then
                Return New CombineResult(False, "Project type not specified")
            End If

            ' Ensure output folder exists
            If Not Directory.Exists(outputFolder) Then
                Try
                    Directory.CreateDirectory(outputFolder)
                Catch ex As Exception
                    Return New CombineResult(False, "Cannot create output folder: " & ex.Message)
                End Try
            End If

            Dim combinedContent As New StringBuilder()
            Dim fileTreeContent As New StringBuilder()
            Dim fileCounter As Integer = 1
            Dim currentFileSize As Integer = 0
            Dim processedFiles As Integer = 0
            Dim includedFiles As Integer = 0
            Const maxFileSize As Integer = 200 * 1024 ' 200KB

            ' Generate file tree (only for first file)
            fileTreeContent.AppendLine("==================== FILE TREE ====================")
            GenerateFileTree(treeNodes, "", fileTreeContent)
            fileTreeContent.AppendLine("====================================================")
            fileTreeContent.AppendLine()

            ' Use project title if provided, otherwise use project type
            If Not String.IsNullOrWhiteSpace(projectTitle) Then
                fileTreeContent.AppendLine("Project Title: " & projectTitle)
            Else
                fileTreeContent.AppendLine("Project Type: " & projectType)
            End If

            fileTreeContent.AppendLine("Generated: " & DateTime.Now.ToString())
            fileTreeContent.AppendLine()

            ' Add database structure if requested and available
            If includeDatabase Then
                ' Handle single database file (backward compatibility)
                If Not String.IsNullOrWhiteSpace(databasePath) AndAlso Not String.IsNullOrWhiteSpace(databaseName) Then
                    If File.Exists(databasePath) Then
                        Try
                            Dim dbStructure As String = DatabaseReader.GetDatabaseStructure(databasePath, databaseName)
                            fileTreeContent.AppendLine(dbStructure)
                            fileTreeContent.AppendLine()
                        Catch ex As Exception
                            fileTreeContent.AppendLine("==================== DATABASE ERROR ====================")
                            fileTreeContent.AppendLine("Error reading database: " & ex.Message)
                            fileTreeContent.AppendLine("======================================================")
                            fileTreeContent.AppendLine()
                        End Try
                    End If
                End If

                ' Handle multiple SQL schema files
                If sqlFiles IsNot Nothing AndAlso sqlFiles.Count > 0 Then
                    For Each sqlFile In sqlFiles
                        If File.Exists(sqlFile) AndAlso Path.GetExtension(sqlFile).ToLower() = ".sql" Then
                            Try
                                Dim dbName As String = Path.GetFileNameWithoutExtension(sqlFile)
                                Dim dbStructure As String = DatabaseReader.GetDatabaseStructure(sqlFile, dbName)
                                fileTreeContent.AppendLine(dbStructure)
                                fileTreeContent.AppendLine()
                            Catch ex As Exception
                                fileTreeContent.AppendLine("==================== SQL SCHEMA ERROR ====================")
                                fileTreeContent.AppendLine("Error reading SQL schema file '" & sqlFile & "': " & ex.Message)
                                fileTreeContent.AppendLine("=========================================================")
                                fileTreeContent.AppendLine()
                            End Try
                        End If
                    Next
                End If
            End If

            ' Add file tree only to the first file
            combinedContent.Append(fileTreeContent.ToString())
            currentFileSize += fileTreeContent.Length

            ' Process each checked file
            For Each filePath In checkedFiles
                processedFiles += 1
                If ShouldIncludeFile(filePath, projectType) Then
                    Dim fileContent As String = GetFileContent(filePath, projectType)
                    If Not String.IsNullOrEmpty(fileContent) Then
                        Dim delimiter As String = String.Format(fileDelimiter, GetRelativePath(filePath, projectFolder))
                        Dim section As String = delimiter & vbCrLf & fileContent & vbCrLf & vbCrLf

                        ' Check if adding this file would exceed the size limit
                        If currentFileSize + section.Length > maxFileSize AndAlso combinedContent.Length > fileTreeContent.Length Then
                            ' Save current file and start a new one
                            SaveCombinedFile(combinedContent.ToString(), fileCounter)
                            fileCounter += 1
                            combinedContent.Clear()
                            ' Don't add file tree to subsequent files - start clean
                            currentFileSize = 0
                        End If

                        combinedContent.Append(section)
                        currentFileSize += section.Length
                        includedFiles += 1
                    End If
                End If
            Next

            ' Always save at least one file, even if no content was added
            If combinedContent.Length > 0 Then
                ' Save the final file with content
                SaveCombinedFile(combinedContent.ToString(), fileCounter)
            Else
                ' No files were included - save a file with just the tree and explanation
                combinedContent.Append(fileTreeContent.ToString())
                combinedContent.AppendLine("No files matched the '" & projectType & "' project type criteria.")
                combinedContent.AppendLine()
                combinedContent.AppendLine("Possible reasons:")
                combinedContent.AppendLine("- Selected files don't match the project type extensions")
                combinedContent.AppendLine("- Files are empty or unreadable")
                combinedContent.AppendLine("- Project type filter excludes these file types")
                combinedContent.AppendLine()
                combinedContent.AppendLine("Total files processed: " & processedFiles.ToString())
                combinedContent.AppendLine("Files included: " & includedFiles.ToString())

                SaveCombinedFile(combinedContent.ToString(), fileCounter)
            End If

            Dim message As String = "Combined " & includedFiles.ToString() & " of " & processedFiles.ToString() & " files into " & fileCounter.ToString() & " output file(s) in " & outputFolder
            Return New CombineResult(True, message, fileCounter, includedFiles)

        Catch ex As Exception
            Return New CombineResult(False, "Error combining files: " & ex.Message)
        End Try
    End Function

    Private Function ShouldIncludeFile(filePath As String, projectType As String) As Boolean
        If Not File.Exists(filePath) Then
            Return False
        End If

        Dim extension As String = Path.GetExtension(filePath).ToLower()
        Dim fileName As String = Path.GetFileName(filePath).ToLower()
        Dim fileNameWithoutExt As String = Path.GetFileNameWithoutExtension(filePath).ToLower()

        ' Skip media files for all project types
        If mediaExtensions.Contains(extension) Then
            Return False
        End If

        Select Case projectType
            Case "Visual Basic Desktop"
                Return ShouldIncludeVBDesktopFile(extension, fileName, fileNameWithoutExt)
            Case "Asp Dotnet Core 8"
                Return ShouldIncludeAspCoreFile(extension, fileName, fileNameWithoutExt)
            Case "Asp MVC 5"
                Return ShouldIncludeAspMvcFile(extension, fileName, fileNameWithoutExt)
            Case Else
                Return False
        End Select
    End Function

    Private Function ShouldIncludeVBDesktopFile(extension As String, fileName As String, fileNameWithoutExt As String) As Boolean
        ' Check against dynamic extension list
        For Each extPattern In vbDesktopExtensions
            If extPattern.StartsWith("*") Then
                ' Wildcard pattern like *.vb
                Dim extToMatch As String = extPattern.Substring(1).ToLower()
                If extension = extToMatch Then
                    Return True
                End If
            Else
                ' Specific file name
                If fileName = extPattern.ToLower() Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Function ShouldIncludeAspCoreFile(extension As String, fileName As String, fileNameWithoutExt As String) As Boolean
        ' Check against dynamic extension list with regex support
        For Each extPattern In aspCoreExtensions
            If IsRegexPattern(extPattern) Then
                ' Use regex matching
                Try
                    Dim regex As New Regex(extPattern, RegexOptions.IgnoreCase)
                    If regex.IsMatch(fileName) Then
                        Return True
                    End If
                Catch
                    ' If regex fails, skip this pattern
                    Continue For
                End Try
            ElseIf extPattern.StartsWith("*") Then
                ' Wildcard pattern like *.css
                Dim extToMatch As String = extPattern.Substring(1).ToLower()
                If extension = extToMatch Then
                    Return True
                End If
            Else
                ' Specific file name like site.css
                If fileName = extPattern.ToLower() Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Function IsRegexPattern(pattern As String) As Boolean
        ' Check if the pattern contains regex metacharacters
        Return pattern.Contains("^") OrElse pattern.Contains("$") OrElse
               pattern.Contains("(") OrElse pattern.Contains(")") OrElse
               pattern.Contains("[") OrElse pattern.Contains("]") OrElse
               pattern.Contains("?") OrElse pattern.Contains("+") OrElse
               (pattern.Contains(".") AndAlso pattern.Contains("\"))
    End Function

    Private Function ShouldIncludeAspMvcFile(extension As String, fileName As String, fileNameWithoutExt As String) As Boolean
        ' Check against dynamic extension list
        For Each extPattern In aspMvcExtensions
            If extPattern.StartsWith("*") Then
                ' Wildcard pattern like *.cs
                Dim extToMatch As String = extPattern.Substring(1).ToLower()
                If extension = extToMatch Then
                    Return True
                End If
            Else
                ' Specific file name
                If fileName = extPattern.ToLower() Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Function GetFileContent(filePath As String, projectType As String) As String
        Try
            If ShouldIncludeFile(filePath, projectType) Then
                Return File.ReadAllText(filePath)
            Else
                Return ""
            End If
        Catch ex As Exception
            ' If file can't be read, return empty content
            Return ""
        End Try
    End Function

    Private Sub GenerateFileTree(nodes As TreeNodeCollection, indent As String, treeContent As StringBuilder)
        For Each node As TreeNode In nodes
            If node.Checked AndAlso node.Tag IsNot Nothing Then
                Dim path As String = node.Tag.ToString()
                Dim relativePath As String = GetRelativePath(path, projectFolder)

                ' Only include checked items
                If Directory.Exists(path) Then
                    ' This is a folder - only show if it has checked children
                    If HasCheckedChildren(node) Then
                        treeContent.AppendLine(indent & relativePath & "/")
                        GenerateFileTree(node.Nodes, indent & "  ", treeContent)
                    End If
                ElseIf File.Exists(path) Then
                    ' This is a file - show it since it's checked
                    treeContent.AppendLine(indent & relativePath)
                End If
            ElseIf node.Tag IsNot Nothing AndAlso Directory.Exists(node.Tag.ToString()) Then
                ' This folder is not checked, but check if it has checked children
                If HasCheckedChildren(node) Then
                    Dim path As String = node.Tag.ToString()
                    Dim relativePath As String = GetRelativePath(path, projectFolder)
                    treeContent.AppendLine(indent & relativePath & "/")
                    GenerateFileTree(node.Nodes, indent & "  ", treeContent)
                End If
            End If
        Next
    End Sub

    Private Function HasCheckedChildren(parentNode As TreeNode) As Boolean
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Checked Then
                Return True
            End If
            If HasCheckedChildren(childNode) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function GetRelativePath(fullPath As String, basePath As String) As String
        Try
            ' Use simple string manipulation instead of URI to avoid URL encoding
            Dim normalizedBase As String = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar
            Dim normalizedFull As String = Path.GetFullPath(fullPath)

            ' Check if the file is under the base path
            If normalizedFull.StartsWith(normalizedBase, StringComparison.OrdinalIgnoreCase) Then
                ' Remove the base path to get relative path
                Dim relativePath As String = normalizedFull.Substring(normalizedBase.Length)
                Return relativePath
            Else
                ' If not under base path, return just the filename
                Return Path.GetFileName(fullPath)
            End If

        Catch ex As Exception
            ' Fallback to filename only
            Return Path.GetFileName(fullPath)
        End Try
    End Function

    Private Sub SaveCombinedFile(content As String, fileNumber As Integer)
        Try
            Dim fileName As String = $"data{fileNumber:D3}.txt"
            Dim filePath As String = Path.Combine(outputFolder, fileName)

            ' Ensure the output directory exists
            Dim directoryPath As String = Path.GetDirectoryName(filePath)
            If Not Directory.Exists(directoryPath) Then
                Directory.CreateDirectory(directoryPath)
            End If

            ' Always create/overwrite the file
            File.WriteAllText(filePath, content, Encoding.UTF8)

        Catch ex As Exception
            Throw New Exception($"Failed to save file data{fileNumber:D3}.txt: {ex.Message}")
        End Try
    End Sub

    Public Function EstimateTokenCount(checkedFiles As List(Of String), projectType As String) As Integer
        Dim tokenCount As Integer = 0

        For Each filePath In checkedFiles
            If ShouldIncludeFile(filePath, projectType) Then
                Try
                    Dim fileContent As String = GetFileContent(filePath, projectType)
                    If Not String.IsNullOrEmpty(fileContent) Then
                        tokenCount += EstimateTokenCount(fileContent)
                    End If
                Catch
                    ' Skip files that can't be read
                End Try
            End If
        Next

        Return tokenCount
    End Function

    Private Function EstimateTokenCount(text As String) As Integer
        ' Simple token estimation: approximately 4 characters per token
        Return Math.Ceiling(text.Length / 4)
    End Function
End Class