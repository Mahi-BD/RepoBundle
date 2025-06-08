Imports System.IO
Imports System.Text

Public Class IniHelper
    Private filePath As String

    Public Sub New(iniFilePath As String)
        filePath = iniFilePath
        ' Create the file if it doesn't exist
        If Not File.Exists(filePath) Then
            CreateDefaultIniFile()
        End If
    End Sub

    Private Sub CreateDefaultIniFile()
        Try
            ' Ensure directory exists
            Dim directoryPath As String = Path.GetDirectoryName(filePath)
            If Not String.IsNullOrEmpty(directoryPath) AndAlso Not Directory.Exists(directoryPath) Then
                Directory.CreateDirectory(directoryPath)
            End If

            ' Create empty file with UTF-8 encoding
            File.WriteAllText(filePath, "", Encoding.UTF8)
        Catch ex As Exception
            ' Ignore errors during file creation
        End Try
    End Sub

    Public Function ReadValue(section As String, key As String, Optional defaultValue As String = "") As String
        If Not File.Exists(filePath) Then
            Return defaultValue
        End If

        Try
            Dim lines() As String = File.ReadAllLines(filePath, Encoding.UTF8)
            Dim currentSection As String = ""
            Dim inTargetSection As Boolean = False

            For Each line In lines
                Dim trimmedLine As String = line.Trim()

                ' Skip empty lines and comments
                If String.IsNullOrEmpty(trimmedLine) OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then
                    Continue For
                End If

                ' Check for section headers
                If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2)
                    inTargetSection = currentSection.Equals(section, StringComparison.OrdinalIgnoreCase)
                    Continue For
                End If

                ' Check for key-value pairs in the target section
                If inTargetSection AndAlso trimmedLine.Contains("=") Then
                    Dim equalIndex As Integer = trimmedLine.IndexOf("=")
                    Dim currentKey As String = trimmedLine.Substring(0, equalIndex).Trim()
                    Dim value As String = trimmedLine.Substring(equalIndex + 1).Trim()

                    If currentKey.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                        ' FIXED: Proper unescaping - first handle backslashes, then newlines
                        value = value.Replace("\\", "\")      ' Unescape backslashes first
                        value = value.Replace("\n", vbCrLf)   ' Then handle newlines
                        Return value
                    End If
                End If
            Next
        Catch ex As Exception
            ' Handle any file reading errors
            Return defaultValue
        End Try

        Return defaultValue
    End Function

    Public Sub WriteValue(section As String, key As String, value As String)
        Dim lines As New List(Of String)
        Dim sectionExists As Boolean = False
        Dim keyExists As Boolean = False
        Dim currentSection As String = ""
        Dim sectionStartIndex As Integer = -1
        Dim sectionEndIndex As Integer = -1

        ' FIXED: Proper escaping - handle newlines first, then backslashes
        value = value.Replace(vbCrLf, "\n")   ' Escape newlines first
        value = value.Replace("\", "\\")      ' Then escape backslashes

        ' Read existing content if file exists
        If File.Exists(filePath) Then
            Try
                lines.AddRange(File.ReadAllLines(filePath, Encoding.UTF8))
            Catch
                ' If file can't be read, start fresh
                lines.Clear()
            End Try
        End If

        ' Find the section and key
        For i As Integer = 0 To lines.Count - 1
            Dim trimmedLine As String = lines(i).Trim()

            ' Check for section headers
            If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                If sectionExists AndAlso sectionEndIndex = -1 Then
                    sectionEndIndex = i - 1
                End If

                currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2)
                If currentSection.Equals(section, StringComparison.OrdinalIgnoreCase) Then
                    sectionExists = True
                    sectionStartIndex = i
                End If
                Continue For
            End If

            ' Check for key-value pairs in the target section
            If sectionExists AndAlso sectionEndIndex = -1 AndAlso trimmedLine.Contains("=") Then
                Dim equalIndex As Integer = trimmedLine.IndexOf("=")
                Dim currentKey As String = trimmedLine.Substring(0, equalIndex).Trim()

                If currentKey.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                    lines(i) = key & "=" & value
                    keyExists = True
                    Exit For
                End If
            End If
        Next

        ' Set section end index if we reached the end of file
        If sectionExists AndAlso sectionEndIndex = -1 Then
            sectionEndIndex = lines.Count - 1
        End If

        ' Add section if it doesn't exist
        If Not sectionExists Then
            If lines.Count > 0 AndAlso Not String.IsNullOrEmpty(lines(lines.Count - 1)) Then
                lines.Add("")
            End If
            lines.Add("[" & section & "]")
            lines.Add(key & "=" & value)
        ElseIf Not keyExists Then
            ' Add key to existing section
            If sectionEndIndex >= sectionStartIndex Then
                lines.Insert(sectionEndIndex + 1, key & "=" & value)
            Else
                lines.Add(key & "=" & value)
            End If
        End If

        ' Write back to file
        Try
            Dim directoryPath As String = Path.GetDirectoryName(filePath)
            If Not Directory.Exists(directoryPath) Then
                Directory.CreateDirectory(directoryPath)
            End If
            File.WriteAllLines(filePath, lines, Encoding.UTF8)
        Catch ex As Exception
            Throw New Exception("Unable to write to INI file: " & ex.Message)
        End Try
    End Sub

    Public Function GetSections() As List(Of String)
        Dim sections As New List(Of String)

        If Not File.Exists(filePath) Then
            Return sections
        End If

        Try
            Dim lines() As String = File.ReadAllLines(filePath, Encoding.UTF8)

            For Each line In lines
                Dim trimmedLine As String = line.Trim()

                If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    Dim sectionName As String = trimmedLine.Substring(1, trimmedLine.Length - 2)
                    If Not sections.Contains(sectionName, StringComparer.OrdinalIgnoreCase) Then
                        sections.Add(sectionName)
                    End If
                End If
            Next
        Catch ex As Exception
            ' Handle any file reading errors
        End Try

        Return sections
    End Function

    Public Function GetKeys(section As String) As List(Of String)
        Dim keys As New List(Of String)

        If Not File.Exists(filePath) Then
            Return keys
        End If

        Try
            Dim lines() As String = File.ReadAllLines(filePath, Encoding.UTF8)
            Dim currentSection As String = ""
            Dim inTargetSection As Boolean = False

            For Each line In lines
                Dim trimmedLine As String = line.Trim()

                ' Skip empty lines and comments
                If String.IsNullOrEmpty(trimmedLine) OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then
                    Continue For
                End If

                ' Check for section headers
                If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2)
                    inTargetSection = currentSection.Equals(section, StringComparison.OrdinalIgnoreCase)
                    Continue For
                End If

                ' Collect keys from the target section
                If inTargetSection AndAlso trimmedLine.Contains("=") Then
                    Dim equalIndex As Integer = trimmedLine.IndexOf("=")
                    Dim keyName As String = trimmedLine.Substring(0, equalIndex).Trim()
                    If Not keys.Contains(keyName, StringComparer.OrdinalIgnoreCase) Then
                        keys.Add(keyName)
                    End If
                End If
            Next
        Catch ex As Exception
            ' Handle any file reading errors
        End Try

        Return keys
    End Function

    Public Sub DeleteKey(section As String, key As String)
        If Not File.Exists(filePath) Then
            Return
        End If

        Try
            Dim lines As New List(Of String)(File.ReadAllLines(filePath, Encoding.UTF8))
            Dim currentSection As String = ""
            Dim inTargetSection As Boolean = False

            For i As Integer = lines.Count - 1 To 0 Step -1
                Dim trimmedLine As String = lines(i).Trim()

                ' Skip empty lines and comments
                If String.IsNullOrEmpty(trimmedLine) OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then
                    Continue For
                End If

                ' Check for section headers
                If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2)
                    inTargetSection = currentSection.Equals(section, StringComparison.OrdinalIgnoreCase)
                    Continue For
                End If

                ' Delete matching key from target section
                If inTargetSection AndAlso trimmedLine.Contains("=") Then
                    Dim equalIndex As Integer = trimmedLine.IndexOf("=")
                    Dim currentKey As String = trimmedLine.Substring(0, equalIndex).Trim()

                    If currentKey.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                        lines.RemoveAt(i)
                        Exit For
                    End If
                End If
            Next

            File.WriteAllLines(filePath, lines, Encoding.UTF8)
        Catch ex As Exception
            Throw New Exception("Unable to delete key from INI file: " & ex.Message)
        End Try
    End Sub

    Public Sub DeleteSection(section As String)
        If Not File.Exists(filePath) Then
            Return
        End If

        Try
            Dim lines As New List(Of String)(File.ReadAllLines(filePath, Encoding.UTF8))
            Dim currentSection As String = ""
            Dim inTargetSection As Boolean = False

            For i As Integer = lines.Count - 1 To 0 Step -1
                Dim trimmedLine As String = lines(i).Trim()

                ' Check for section headers
                If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2)

                    If inTargetSection Then
                        ' We've reached the previous section, stop deleting
                        inTargetSection = False
                    ElseIf currentSection.Equals(section, StringComparison.OrdinalIgnoreCase) Then
                        inTargetSection = True
                    End If
                End If

                ' Delete lines in target section
                If inTargetSection Then
                    lines.RemoveAt(i)
                End If
            Next

            File.WriteAllLines(filePath, lines, Encoding.UTF8)
        Catch ex As Exception
            Throw New Exception("Unable to delete section from INI file: " & ex.Message)
        End Try
    End Sub

    Public Function SectionExists(section As String) As Boolean
        Return GetSections().Contains(section, StringComparer.OrdinalIgnoreCase)
    End Function

    Public Function KeyExists(section As String, key As String) As Boolean
        Return GetKeys(section).Contains(key, StringComparer.OrdinalIgnoreCase)
    End Function

    Public Sub CreateSection(section As String)
        If Not SectionExists(section) Then
            WriteValue(section, "Created", DateTime.Now.ToString())
            DeleteKey(section, "Created")
        End If
    End Sub

    Public Function GetFilePath() As String
        Return filePath
    End Function

    Public Function GetFileSize() As Long
        Try
            If File.Exists(filePath) Then
                Return New FileInfo(filePath).Length
            End If
        Catch
            ' Ignore errors
        End Try
        Return 0
    End Function
End Class