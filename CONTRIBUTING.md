# Contributing to RepoBundle

Thank you for your interest in contributing to RepoBundle! This document provides guidelines and information for contributors.

## 🎯 How to Contribute

### 🐛 Reporting Bugs
1. **Check existing issues** to avoid duplicates
2. **Use the bug report template** when creating new issues
3. **Provide detailed information**:
   - OS version and .NET version
   - Steps to reproduce the issue
   - Expected vs actual behavior
   - Screenshots if applicable
   - Log files or error messages

### 💡 Suggesting Features
1. **Check the roadmap** and existing feature requests
2. **Use the feature request template**
3. **Explain the use case** and benefits
4. **Consider backward compatibility**

### 🔧 Code Contributions

#### Prerequisites
- Windows 10/11
- Visual Studio 2022 or VS Code with VB.NET extension
- .NET 8.0 SDK
- Git for version control

#### Setting Up Development Environment
1. **Fork the repository**
   ```bash
   git clone https://github.com/yourusername/repobundle.git
   cd repobundle
   ```

2. **Create a development branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Open in Visual Studio**
   ```bash
   start RepoBundle.sln
   ```

4. **Build and test**
   ```bash
   dotnet build
   dotnet run
   ```

## 📋 Development Guidelines

### Code Style and Standards

#### VB.NET Conventions
```vb
' Use PascalCase for public methods and properties
Public Function GetFileContent(filePath As String) As String

' Use camelCase for private variables
Private currentProjectPath As String

' Use descriptive names
Private Sub LoadProjectFilesFromDirectory(directoryPath As String)

' Add XML documentation for public methods
''' <summary>
''' Combines selected files into output format
''' </summary>
''' <param name="checkedFiles">List of file paths to combine</param>
''' <returns>Result of the combination operation</returns>
Public Function CombineFiles(checkedFiles As List(Of String)) As CombineResult
```

#### Error Handling
```vb
Try
    ' Your code here
    Dim result As String = ProcessFile(filePath)
    Return result
Catch ex As IOException
    ' Handle specific exceptions
    LogError("File IO error: " & ex.Message)
    Return String.Empty
Catch ex As Exception
    ' Handle general exceptions
    LogError("Unexpected error: " & ex.Message)
    Throw
Finally
    ' Cleanup code
    CleanupResources()
End Try
```

#### Configuration Management
```vb
' Always use IniHelper for configuration
Private Sub SaveUserPreferences()
    iniHelper.WriteValue("UI", "WindowWidth", Me.Width.ToString())
    iniHelper.WriteValue("UI", "WindowHeight", Me.Height.ToString())
End Sub
```

### Project Structure

```
RepoBundle/
├── Forms/                  # Windows Forms
│   ├── frmMain.vb         # Main application window
│   ├── frmSettings.vb     # Settings dialog
│   └── ...
├── Core/                   # Core functionality
│   ├── FileCombiner.vb    # File processing engine
│   ├── DatabaseReader.vb  # SQL file processing
│   └── IniHelper.vb       # Configuration manager
├── Models/                 # Data models
│   └── CombineResult.vb   # Result objects
├── Resources/              # Application resources
└── Tests/                  # Unit tests (future)
```

### Testing Guidelines

#### Manual Testing Checklist
- [ ] File selection works correctly
- [ ] Template save/load functionality
- [ ] All project types filter properly
- [ ] File combination produces correct output
- [ ] Configuration persistence works
- [ ] Backup functionality operates correctly
- [ ] Progress indicators work properly
- [ ] Error handling displays appropriate messages

#### Test Cases to Cover
1. **File Selection**
   - Single file selection
   - Folder selection (recursive)
   - Mixed selection
   - Large directory trees

2. **Project Types**
   - Visual Basic Desktop filtering
   - ASP.NET Core 8 filtering
   - ASP.NET MVC 5 filtering

3. **Template Management**
   - Save template with various file selections
   - Load template and verify selection
   - Update existing template
   - Copy template functionality

4. **File Combination**
   - Small files (under 200KB)
   - Large files (requiring splitting)
   - Mixed file types
   - Special characters in file names

5. **Configuration**
   - Settings persistence
   - INI file corruption recovery
   - Default value handling

## 🏗️ Architecture Overview

### Key Classes and Responsibilities

#### frmMain
- Main application UI
- File tree management
- Template operations
- Progress tracking
- Event coordination

#### FileCombiner
- Core file processing logic
- Project type filtering
- File size management
- Output generation

#### IniHelper
- Configuration file management
- Setting persistence
- Template storage

#### DatabaseReader
- SQL file processing
- Database schema extraction

### Data Flow
1. **User selects files** → TreeView updates
2. **Template operations** → IniHelper saves/loads
3. **Combine action** → FileCombiner processes
4. **Progress updates** → UI reflects status
5. **Configuration changes** → IniHelper persists

## 🔍 Common Issues and Solutions

### Build Issues
```bash
# Clean and rebuild
dotnet clean
dotnet build --no-restore

# Restore packages
dotnet restore
```

### Runtime Issues
- **File access errors**: Check permissions and file locks
- **Configuration errors**: Verify INI file format
- **Memory issues**: Monitor large file processing

## 📝 Pull Request Process

### Before Submitting
1. **Test your changes thoroughly**
2. **Update documentation** if needed
3. **Follow the code style guidelines**
4. **Write descriptive commit messages**

### Commit Message Format
```
type(scope): brief description

Detailed explanation of the change (if needed)

Fixes #issue-number
```

Examples:
```
feat(templates): add template export functionality
fix(combiner): resolve file splitting edge case
docs(readme): update installation instructions
```

### Pull Request Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Manual testing completed
- [ ] All scenarios tested
- [ ] No regressions introduced

## Screenshots (if applicable)
Add screenshots for UI changes

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No breaking changes (or properly documented)
```

## 🎉 Recognition

Contributors will be recognized in:
- **README.md** contributors section
- **Release notes** for significant contributions
- **GitHub contributors** page

## 📞 Getting Help

- **Questions**: Use [GitHub Discussions](https://github.com/yourusername/repobundle/discussions)
- **Issues**: Create an issue with detailed information
- **Real-time help**: Check if maintainers are available for discussion

## 📚 Resources

### VB.NET Resources
- [VB.NET Language Reference](https://docs.microsoft.com/en-us/dotnet/visual-basic/)
- [Windows Forms Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)

### Git Resources
- [Git Handbook](https://guides.github.com/introduction/git-handbook/)
- [GitHub Flow](https://guides.github.com/introduction/flow/)

### .NET Resources
- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [C# to VB.NET Converter](https://converter.telerik.com/) (for reference)

---

Thank you for contributing to RepoBundle! Your efforts help make this tool better for everyone. 🙌