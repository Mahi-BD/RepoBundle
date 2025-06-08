# RepoBundle - Project File Combiner

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![VB.NET](https://img.shields.io/badge/VB.NET-Windows%20Forms-green.svg)](https://docs.microsoft.com/en-us/dotnet/visual-basic/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A powerful Windows Forms application built with VB.NET that intelligently combines project files for AI prompts, documentation, or code reviews. Inspired by tools like [Prompt Engineer](https://prompt.16x.engineer/).

![image](https://github.com/user-attachments/assets/f85efafa-f3fa-4759-96c7-6bcf027dfa72)


## ✨ Features

### 🎯 Core Functionality
- **Smart File Selection**: Hierarchical tree view for easy file and folder selection
- **Project Type Filtering**: Intelligent filtering based on Visual Basic Desktop, ASP.NET MVC 5, or ASP.NET Core 8
- **Template Management**: Save, load, copy, and update file selection templates
- **Automatic File Splitting**: Splits output files at 200KB for optimal token usage
- **Token Estimation**: Real-time token count estimation for AI prompt optimization

### 🔧 Advanced Features
- **Configuration Management**: INI-based configuration for persistence
- **Project Backup**: Smart backup with excluded folder filtering
- **Progress Tracking**: Real-time progress bars and status updates
- **Keyboard Shortcuts**: Full keyboard navigation support
- **Window State Persistence**: Remembers window size, position, and layout

### 📁 Project Type Support

#### Visual Basic Desktop
- **Included**: `*.vb`, `*.Designer.vb`, `*.vbproj`, `*.sql`
- **Excluded**: All media files (images, videos, audio)

#### ASP.NET Core 8 / MVC 5
- **C# Files**: `*.cs` (full content)
- **Razor Views**: `*.cshtml` (layout files starting with `_` only)
- **Stylesheets**: `*.css` (site.css only)
- **JavaScript**: `*.js` (site.js only)
- **SQL**: `*.sql` (full content)
- **Excluded**: All media files

## 🚀 Quick Start

### Prerequisites
- Windows 10/11
- .NET 8.0 Runtime
- Visual Studio 2022 (for development)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/repobundle.git
   cd repobundle
   ```

2. Open `RepoBundle.sln` in Visual Studio 2022

3. Build and run the application:
   ```bash
   dotnet build
   dotnet run
   ```

## 📖 Usage Guide

### 1. Initial Setup
1. **Select Project Folder**: File → Select Project Folder
2. **Configure Output**: Settings → Application Settings
3. **Set Project Type**: Choose from dropdown (Visual Basic Desktop, ASP.NET MVC 5, ASP.NET Core 8)

### 2. File Selection
- **Tree Navigation**: Use the left panel to browse your project
- **Folder Selection**: Check folders to auto-select all contents
- **File Selection**: Individual file selection with checkboxes
- **Quick Actions**: Expand All, Collapse All, Refresh Tree

### 3. Template Management
- **Save Template**: Enter name and click "Save" to store current selection
- **Load Template**: Select from dropdown and click "Load"
- **Copy Template**: Clone existing templates for modification
- **Update Template**: Modify and update existing templates

### 4. File Combination
1. Select files/folders in the tree view
2. Choose appropriate project type
3. Click "🔗 Combine Files"
4. Files are automatically split at 200KB and saved as `data001.txt`, `data002.txt`, etc.

## 🎛️ Configuration

### config.ini
Stores application settings:
```ini
[Main]
ProjectFolder=C:\Your\Project\Path
OutputFolder=C:\Output\Path
LastSelectedProjectType=Visual Basic Desktop

[Project]
Title=My Project
Instructions=Project description...

[Backup]
ExcludedFolders=.git,.vs,.svn,bin,obj
```

### template.ini
Stores file selection templates:
```ini
[Template_MyTemplate]
Name=My Template
File1=C:\Project\File1.vb
File2=C:\Project\File2.vb
```

## 🔧 Architecture

### Key Components
- **frmMain**: Main application window with split-panel layout
- **FileCombiner**: Core logic for file processing and combination
- **IniHelper**: Configuration management utility
- **DatabaseReader**: SQL file processing
- **frmSettings**: Application configuration dialog

### File Structure
```
RepoBundle/
├── My Project/
│   ├── Application.Designer.vb
│   └── Application.myapp
├── frmMain.vb              # Main form
├── frmMain.Designer.vb     # Main form designer
├── frmSettings.vb          # Settings form
├── FileCombiner.vb         # File processing engine
├── DatabaseReader.vb       # SQL file handler
├── IniHelper.vb           # Configuration manager
└── RepoBundle.vbproj      # Project file
```

## 🎨 Screenshots

### Main Interface
The application features a clean, intuitive interface with:
- Left panel: File tree with checkboxes
- Right panel: Template management and project configuration
- Bottom panel: Combine actions and progress tracking

### Template Management
Easy-to-use template system for saving and loading file selections:
- Save current selections as named templates
- Load previously saved templates
- Copy and modify existing templates
- Update templates with new file selections

### Settings Panel
Comprehensive settings for:
- Project and output folder configuration
- Database file management
- Excluded folder settings for backups
- Application preferences

## 🔑 Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+O` | Open Project Folder |
| `Ctrl+B` | Backup Project |
| `Ctrl+G` | Go to Output Folder |
| `Ctrl+S` | Save Template |
| `Ctrl+L` | Load Template |
| `Ctrl+Enter` | Combine Files |
| `F5` | Refresh Tree |
| `F1` | About Dialog |

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

### Development Setup
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit changes: `git commit -m 'Add amazing feature'`
4. Push to branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

### Code Style
- Follow VB.NET naming conventions
- Use meaningful variable and method names
- Add XML documentation for public methods
- Include error handling for file operations

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by [Prompt Engineer](https://prompt.16x.engineer/)
- Built with love for the developer community
- Special thanks to all contributors

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/repobundle/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/repobundle/discussions)
- **Wiki**: [Project Wiki](https://github.com/yourusername/repobundle/wiki)

## 🗺️ Roadmap

- [ ] Add support for more project types (Python, Java, etc.)
- [ ] Plugin architecture for custom file processors
- [ ] Web-based version
- [ ] Integration with popular IDEs
- [ ] Advanced filtering options
- [ ] Export to various formats (JSON, XML, etc.)

---

**Made with ❤️ by Samsur Rahman Mahi**
