Imports System.IO
Imports System.Text

Public Class DatabaseReader
    Public Shared Function GetDatabaseStructure(databasePath As String, databaseName As String) As String
        Try
            Dim result As String = ""
            result += "==================== DATABASE STRUCTURE ====================" + vbCrLf
            result += "Database Name: " + databaseName + vbCrLf
            result += "Database File: " + Path.GetFileName(databasePath) + vbCrLf
            result += "File Size: " + GetFileSize(databasePath) + vbCrLf
            result += "Last Modified: " + File.GetLastWriteTime(databasePath).ToString() + vbCrLf
            result += "============================================================" + vbCrLf
            result += vbCrLf

            If Path.GetExtension(databasePath).ToLower() = ".sql" Then
                result += "SQL Schema File Content:" + vbCrLf
                result += "------------------------" + vbCrLf
                result += File.ReadAllText(databasePath) + vbCrLf
            Else
                result += "Unsupported file type. Only .sql files are supported." + vbCrLf
            End If

            result += vbCrLf
            result += "==================== END DATABASE ====================" + vbCrLf

            Return result
        Catch ex As Exception
            Return "Error reading database structure: " + ex.Message
        End Try
    End Function

    Private Shared Function GetFileSize(filePath As String) As String
        Try
            Dim info As New FileInfo(filePath)
            Dim size As Long = info.Length

            If size < 1024 Then
                Return size.ToString() + " bytes"
            ElseIf size < 1024 * 1024 Then
                Return Math.Round(size / 1024.0, 1).ToString() + " KB"
            Else
                Return Math.Round(size / (1024.0 * 1024.0), 1).ToString() + " MB"
            End If
        Catch
            Return "Unknown"
        End Try
    End Function
End Class