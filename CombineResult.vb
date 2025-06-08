' CombineResult class - Add this to a new file or at the end of FileCombiner.vb

Public Class CombineResult
    Public Property Success As Boolean
    Public Property Message As String
    Public Property FileCount As Integer = 0
    Public Property FilesIncluded As Integer = 0

    Public Sub New()
        Success = False
        Message = ""
    End Sub

    Public Sub New(success As Boolean, message As String)
        Me.Success = success
        Me.Message = message
    End Sub

    Public Sub New(success As Boolean, message As String, fileCount As Integer, filesIncluded As Integer)
        Me.Success = success
        Me.Message = message
        Me.FileCount = fileCount
        Me.FilesIncluded = filesIncluded
    End Sub
End Class