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
        Me.grpFolders = New System.Windows.Forms.GroupBox()
        Me.btnBrowseProject = New System.Windows.Forms.Button()
        Me.btnBrowseOutput = New System.Windows.Forms.Button()
        Me.txtProjectFolder = New System.Windows.Forms.TextBox()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.lblProjectFolder = New System.Windows.Forms.Label()
        Me.lblOutputFolder = New System.Windows.Forms.Label()
        Me.grpDatabase = New System.Windows.Forms.GroupBox()
        Me.btnClearAll = New System.Windows.Forms.Button()
        Me.btnRemoveSelected = New System.Windows.Forms.Button()
        Me.btnBrowseDatabase = New System.Windows.Forms.Button()
        Me.lstDatabaseFiles = New System.Windows.Forms.ListBox()
        Me.lblDatabaseFiles = New System.Windows.Forms.Label()
        Me.chkIncludeDatabase = New System.Windows.Forms.CheckBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.folderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.grpFolders.SuspendLayout()
        Me.grpDatabase.SuspendLayout()
        Me.SuspendLayout()

        ' grpFolders
        Me.grpFolders.Controls.Add(Me.btnBrowseProject)
        Me.grpFolders.Controls.Add(Me.btnBrowseOutput)
        Me.grpFolders.Controls.Add(Me.txtProjectFolder)
        Me.grpFolders.Controls.Add(Me.txtOutputFolder)
        Me.grpFolders.Controls.Add(Me.lblProjectFolder)
        Me.grpFolders.Controls.Add(Me.lblOutputFolder)
        Me.grpFolders.Location = New System.Drawing.Point(12, 12)
        Me.grpFolders.Name = "grpFolders"
        Me.grpFolders.Size = New System.Drawing.Size(560, 120)
        Me.grpFolders.TabIndex = 0
        Me.grpFolders.TabStop = False
        Me.grpFolders.Text = "Folder Settings"

        ' lblProjectFolder
        Me.lblProjectFolder.AutoSize = True
        Me.lblProjectFolder.Location = New System.Drawing.Point(20, 30)
        Me.lblProjectFolder.Name = "lblProjectFolder"
        Me.lblProjectFolder.Size = New System.Drawing.Size(90, 17)
        Me.lblProjectFolder.TabIndex = 0
        Me.lblProjectFolder.Text = "Project Folder:"

        ' txtProjectFolder
        Me.txtProjectFolder.Location = New System.Drawing.Point(120, 27)
        Me.txtProjectFolder.Name = "txtProjectFolder"
        Me.txtProjectFolder.Size = New System.Drawing.Size(350, 22)
        Me.txtProjectFolder.TabIndex = 1

        ' btnBrowseProject
        Me.btnBrowseProject.Location = New System.Drawing.Point(480, 25)
        Me.btnBrowseProject.Name = "btnBrowseProject"
        Me.btnBrowseProject.Size = New System.Drawing.Size(70, 26)
        Me.btnBrowseProject.TabIndex = 2
        Me.btnBrowseProject.Text = "Browse..."
        Me.btnBrowseProject.UseVisualStyleBackColor = True

        ' lblOutputFolder
        Me.lblOutputFolder.AutoSize = True
        Me.lblOutputFolder.Location = New System.Drawing.Point(20, 70)
        Me.lblOutputFolder.Name = "lblOutputFolder"
        Me.lblOutputFolder.Size = New System.Drawing.Size(90, 17)
        Me.lblOutputFolder.TabIndex = 3
        Me.lblOutputFolder.Text = "Output Folder:"

        ' txtOutputFolder
        Me.txtOutputFolder.Location = New System.Drawing.Point(120, 67)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(350, 22)
        Me.txtOutputFolder.TabIndex = 4

        ' btnBrowseOutput
        Me.btnBrowseOutput.Location = New System.Drawing.Point(480, 65)
        Me.btnBrowseOutput.Name = "btnBrowseOutput"
        Me.btnBrowseOutput.Size = New System.Drawing.Size(70, 26)
        Me.btnBrowseOutput.TabIndex = 5
        Me.btnBrowseOutput.Text = "Browse..."
        Me.btnBrowseOutput.UseVisualStyleBackColor = True

        ' grpDatabase
        Me.grpDatabase.Controls.Add(Me.chkIncludeDatabase)
        Me.grpDatabase.Controls.Add(Me.lblDatabaseFiles)
        Me.grpDatabase.Controls.Add(Me.lstDatabaseFiles)
        Me.grpDatabase.Controls.Add(Me.btnBrowseDatabase)
        Me.grpDatabase.Controls.Add(Me.btnRemoveSelected)
        Me.grpDatabase.Controls.Add(Me.btnClearAll)
        Me.grpDatabase.Location = New System.Drawing.Point(12, 150)
        Me.grpDatabase.Name = "grpDatabase"
        Me.grpDatabase.Size = New System.Drawing.Size(560, 220)
        Me.grpDatabase.TabIndex = 1
        Me.grpDatabase.TabStop = False
        Me.grpDatabase.Text = "Database Settings"

        ' chkIncludeDatabase
        Me.chkIncludeDatabase.AutoSize = True
        Me.chkIncludeDatabase.Location = New System.Drawing.Point(20, 25)
        Me.chkIncludeDatabase.Name = "chkIncludeDatabase"
        Me.chkIncludeDatabase.Size = New System.Drawing.Size(200, 21)
        Me.chkIncludeDatabase.TabIndex = 0
        Me.chkIncludeDatabase.Text = "Include SQL Database Files"
        Me.chkIncludeDatabase.UseVisualStyleBackColor = True

        ' lblDatabaseFiles
        Me.lblDatabaseFiles.AutoSize = True
        Me.lblDatabaseFiles.Location = New System.Drawing.Point(20, 60)
        Me.lblDatabaseFiles.Name = "lblDatabaseFiles"
        Me.lblDatabaseFiles.Size = New System.Drawing.Size(150, 17)
        Me.lblDatabaseFiles.TabIndex = 1
        Me.lblDatabaseFiles.Text = "Selected SQL Files (0):"

        ' lstDatabaseFiles
        Me.lstDatabaseFiles.FormattingEnabled = True
        Me.lstDatabaseFiles.ItemHeight = 16
        Me.lstDatabaseFiles.Location = New System.Drawing.Point(20, 85)
        Me.lstDatabaseFiles.Name = "lstDatabaseFiles"
        Me.lstDatabaseFiles.Size = New System.Drawing.Size(450, 84)
        Me.lstDatabaseFiles.TabIndex = 2

        ' btnBrowseDatabase
        Me.btnBrowseDatabase.Location = New System.Drawing.Point(480, 85)
        Me.btnBrowseDatabase.Name = "btnBrowseDatabase"
        Me.btnBrowseDatabase.Size = New System.Drawing.Size(70, 30)
        Me.btnBrowseDatabase.TabIndex = 3
        Me.btnBrowseDatabase.Text = "Add SQL..."
        Me.btnBrowseDatabase.UseVisualStyleBackColor = True

        ' btnRemoveSelected
        Me.btnRemoveSelected.Location = New System.Drawing.Point(480, 125)
        Me.btnRemoveSelected.Name = "btnRemoveSelected"
        Me.btnRemoveSelected.Size = New System.Drawing.Size(70, 30)
        Me.btnRemoveSelected.TabIndex = 4
        Me.btnRemoveSelected.Text = "Remove"
        Me.btnRemoveSelected.UseVisualStyleBackColor = True

        ' btnClearAll
        Me.btnClearAll.Location = New System.Drawing.Point(480, 165)
        Me.btnClearAll.Name = "btnClearAll"
        Me.btnClearAll.Size = New System.Drawing.Size(70, 30)
        Me.btnClearAll.TabIndex = 5
        Me.btnClearAll.Text = "Clear All"
        Me.btnClearAll.UseVisualStyleBackColor = True

        ' btnOK
        Me.btnOK.Location = New System.Drawing.Point(400, 390)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 30)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True

        ' btnCancel
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(490, 390)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 30)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True

        ' openFileDialog1
        Me.openFileDialog1.Filter = "SQL Script Files|*.sql"
        Me.openFileDialog1.Multiselect = True
        Me.openFileDialog1.Title = "Select SQL Database Files"

        ' frmSettings
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(584, 441)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.grpDatabase)
        Me.Controls.Add(Me.grpFolders)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Application Settings"
        Me.grpFolders.ResumeLayout(False)
        Me.grpFolders.PerformLayout()
        Me.grpDatabase.ResumeLayout(False)
        Me.grpDatabase.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents grpFolders As GroupBox
    Friend WithEvents btnBrowseProject As Button
    Friend WithEvents btnBrowseOutput As Button
    Friend WithEvents txtProjectFolder As TextBox
    Friend WithEvents txtOutputFolder As TextBox
    Friend WithEvents lblProjectFolder As Label
    Friend WithEvents lblOutputFolder As Label
    Friend WithEvents grpDatabase As GroupBox
    Friend WithEvents btnClearAll As Button
    Friend WithEvents btnRemoveSelected As Button
    Friend WithEvents btnBrowseDatabase As Button
    Friend WithEvents lstDatabaseFiles As ListBox
    Friend WithEvents lblDatabaseFiles As Label
    Friend WithEvents chkIncludeDatabase As CheckBox
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents folderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents openFileDialog1 As OpenFileDialog

End Class