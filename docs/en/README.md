# Serial Port Tool

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-4.8%20%7C%208.0-blue)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey)](https://www.microsoft.com/windows/)

A modern, feature-rich serial port communication and debugging tool built with WPF and .NET. Features a clean MVVM architecture, comprehensive data conversion utilities, and an intuitive user interface.

[English Documentation](../en/README.md) | [中文文档](../zh/README.md)

## ✨ Features

- **Modern Architecture**: Built with MVVM pattern and dependency injection
- **Multi-Framework Support**: Supports .NET Framework 4.8 and .NET 8.0
- **Real-time Communication**: Automatic and manual data reception modes
- **Data Format Conversion**: Seamless text ↔ hexadecimal conversion
- **Cross-Platform Core**: Core library can be used in other applications
- **Thread-Safe Operations**: Robust error handling and resource management
- **Intuitive UI**: Clean, responsive Windows interface

## 🚀 Quick Start

### Requirements

- Windows 10/11
- .NET Framework 4.8 or .NET 8.0 Runtime
- Available serial ports (physical or virtual)

### Installation

1. **Download**: Get the latest release from [Releases](https://github.com/hongjiapeng/SerialPortTool/releases)
2. **Extract**: Unzip to your preferred directory
3. **Run**: Execute `SerialPortTool.exe`

### Building from Source

```bash
# Clone the repository
git clone https://github.com/hongjiapeng/SerialPortTool.git
cd SerialPortTool

# Build the solution
dotnet build

# Run the application
dotnet run --project src/SerialPortTool.UI
```

## 📁 Project Structure

```
SerialPortTool/
├── src/
│   ├── SerialPortTool.Core/          # Core library (reusable)
│   │   ├── Services/                 # Serial port services
│   │   └── Utils/                    # Data conversion utilities
│   └── SerialPortTool.UI/            # WPF user interface
│       ├── ViewModels/               # MVVM view models
│       ├── Infrastructure/           # DI container
│       └── Assets/                   # UI resources
├── docs/                             # Documentation
│   ├── en/                          # English documentation
│   └── zh/                          # Chinese documentation
├── .editorconfig                     # Code style configuration
├── Directory.Build.props             # MSBuild properties
└── global.json                      # .NET SDK version
```

## 🎯 Usage

### Basic Operations

1. **Select Port**: Choose from available COM ports
2. **Configure**: Set baud rate, data bits, parity, stop bits
3. **Connect**: Click "Connect" to establish connection
4. **Send Data**: Enter text or hex data and click "Send"
5. **Receive Data**: View incoming data in real-time or manual mode

### Data Formats

- **Text Mode**: Send/receive plain text strings
- **Hex Mode**: Send/receive hexadecimal formatted data (e.g., "01 02 03 FF")

### Reception Modes

- **Response Mode**: Automatically receive and display incoming data
- **Acknowledgment Mode**: Manually read data from the buffer

## 🏗️ Architecture

The application follows modern software architecture principles:

- **Core Library**: Reusable serial communication components
- **MVVM Pattern**: Clean separation of UI and business logic
- **Dependency Injection**: Loose coupling and testability
- **Event-Driven**: Responsive data handling
- **Thread-Safe**: Robust concurrent operations

For detailed architecture information, see [Architecture Documentation](ARCHITECTURE.md).

## 📦 NuGet Package

The core library is available as a NuGet package for use in other applications:

```xml
<PackageReference Include="SerialPortTool.Core" Version="2.0.0" />
```

```csharp
using SerialPortTool.Core.Services;

// Create service instance
var serialService = new SerialPortService();

// Configure connection
var config = new SerialPortConfiguration
{
    PortName = "COM1",
    BaudRate = 9600
};

// Open port and send data
var result = serialService.OpenPort(config);
if (result.Success)
{
    serialService.SendText("Hello, World!");
}
```

## 🤝 Contributing

Contributions are welcome! Please read our [Contributing Guidelines](../../CONTRIBUTING.md) before submitting pull requests.

### Development Setup

1. Install Visual Studio 2022 or VS Code
2. Install .NET 8.0 SDK
3. Clone and open the solution
4. Build and run tests

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## 🔗 Links

- [Project Homepage](https://github.com/hongjiapeng/SerialPortTool)
- [Issue Tracker](https://github.com/hongjiapeng/SerialPortTool/issues)
- [Release Notes](https://github.com/hongjiapeng/SerialPortTool/releases)
- [Documentation](../)

## 🙏 Acknowledgments

- Built with [.NET](https://dotnet.microsoft.com/) and [WPF](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- Serial communication powered by [System.IO.Ports](https://www.nuget.org/packages/System.IO.Ports/)
- Icons from [Material Design Icons](https://materialdesignicons.com/)

---

<div align="center">
Made with ❤️ by <a href="https://github.com/hongjiapeng">hongjiapeng</a>
</div>
