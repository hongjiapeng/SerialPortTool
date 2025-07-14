# 串口调试助手 (SerialPortTool)

一个基于C# WPF开发的串口调试工具，提供简洁易用的串口通信功能。

![串口调试助手](SerialPortTool/串口.png)

## 功能特性

- 🔌 **串口管理**：自动检测可用串口，支持常用波特率设置
- 📨 **数据收发**：支持文本和十六进制数据的发送和接收
- 📊 **实时显示**：实时显示接收到的数据，支持多种显示模式
- ⚙️ **灵活配置**：可配置串口参数（波特率、数据位、停止位、校验位等）
- 🔄 **响应模式**：支持响应模式和轮询模式
- 💾 **数据保存**：可保存接收到的数据到文件

## 系统要求

### 支持的目标框架

- **.NET Framework 4.8** - Windows 7 SP1 及以上版本
- **.NET 6.0** - Windows 10 1607 及以上版本
- **.NET 7.0** - Windows 10 1607 及以上版本  
- **.NET 8.0** - Windows 10 1607 及以上版本

### 运行时要求

- Windows 7 SP1 及以上版本
- 对于 .NET Framework: .NET Framework 4.8 运行时
- 对于 .NET 6+: 对应的 .NET 运行时
- 串口设备（USB转串口、串口卡等）

## 快速开始

### 下载使用

1. 前往 [Releases](https://github.com/hongjiapeng/SerialPortTool/releases) 页面
2. 下载最新版本的 `串口调试助手.exe`
3. 双击运行，无需安装

### 编译运行

#### 先决条件

- **Visual Studio 2022** 或更高版本（推荐）
- **.NET 8.0 SDK** （用于构建所有目标框架）
- **.NET Framework 4.8 Developer Pack**（用于 .NET Framework 目标）

#### 构建步骤

1. **克隆仓库**

   ```bash
   git clone https://github.com/hongjiapeng/SerialPortTool.git
   cd SerialPortTool
   ```

2. **使用Visual Studio**
   - 用Visual Studio 2022打开 `SerialPortTool/SerialPortTool.sln`
   - 选择目标框架（在项目属性中可以看到多个目标）
   - 生成解决方案 (Ctrl+Shift+B)
   - 运行程序 (F5)

3. **使用.NET CLI**

   ```bash
   # 构建所有目标框架
   dotnet build SerialPortTool/SerialPortTool.csproj
   
   # 构建特定目标框架
   dotnet build SerialPortTool/SerialPortTool.csproj -f net8.0-windows
   dotnet build SerialPortTool/SerialPortTool.csproj -f net48
   
   # 运行程序
   dotnet run --project SerialPortTool/SerialPortTool.csproj -f net8.0-windows
   ```

4. **发布单文件应用**

   ```bash
   # 发布 .NET 8.0 单文件应用
   dotnet publish SerialPortTool/SerialPortTool.csproj -f net8.0-windows -c Release --self-contained true -p:PublishSingleFile=true
   
   # 发布 .NET Framework 4.8 应用
   dotnet publish SerialPortTool/SerialPortTool.csproj -f net48 -c Release
   ```

## 使用说明

### 基本操作

1. **选择串口**：从下拉菜单中选择要使用的串口
2. **设置参数**：配置波特率、数据位、停止位、校验位等参数
3. **打开串口**：点击"打开串口"按钮建立连接
4. **发送数据**：在发送框中输入数据，点击"发送"
5. **接收数据**：接收到的数据会实时显示在接收框中

### 高级功能

- **十六进制模式**：支持以十六进制格式发送和显示数据
- **自动发送**：可设置定时自动发送数据
- **数据清除**：一键清除接收或发送缓冲区
- **响应模式切换**：支持响应模式和轮询模式切换

## 项目结构

```text
SerialPortTool/
├── SerialPortTool.sln          # Visual Studio 解决方案文件
├── SerialPortTool/
│   ├── MainWindow.xaml         # 主窗口界面设计
│   ├── MainWindow.xaml.cs      # 主窗口逻辑代码
│   ├── App.xaml               # 应用程序资源
│   ├── App.xaml.cs            # 应用程序入口
│   ├── SerialPortTool.csproj  # 项目文件
│   ├── 串口.png               # 应用程序图标
│   └── Properties/            # 程序集信息和资源
├── README.md                  # 项目说明文档
├── LICENSE                    # 开源许可证
└── .gitignore                # Git忽略文件配置
```

## 技术栈

- **开发语言**：C#
- **框架**：WPF (Windows Presentation Foundation)
- **目标框架**：
  - .NET Framework 4.8
  - .NET 6.0 (Windows)
  - .NET 7.0 (Windows)
  - .NET 8.0 (Windows)
- **开发工具**：Visual Studio 2022, .NET CLI
- **串口通信**：System.IO.Ports
- **项目类型**：SDK风格项目

## 贡献指南

欢迎贡献代码！请遵循以下步骤：

1. Fork 这个仓库
2. 创建你的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交你的修改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启一个 Pull Request

## 问题反馈

如果您遇到任何问题或有改进建议，请：

1. 查看 [Issues](https://github.com/hongjiapeng/SerialPortTool/issues) 页面
2. 如果问题尚未被报告，请创建新的 Issue
3. 详细描述问题的重现步骤和环境信息

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 更新日志

### v2.0.0 - 2025-07-14

- **框架升级**: 支持多目标框架(.NET Framework 4.8, .NET 8.0)
- **项目现代化**: 迁移到SDK风格项目文件
- **依赖管理**: 使用NuGet包管理System.IO.Ports
- **开发体验**: 支持dotnet CLI命令和现代IDE
- **文档完善**: 添加完整的项目文档和配置文件

### v1.0.0

- 基础串口通信功能
- 支持多种波特率和参数配置
- 文本和十六进制数据收发
- 响应模式和轮询模式

## 致谢

感谢所有为这个项目做出贡献的开发者！

---

⭐ 如果这个项目对您有帮助，请给它一个星标！
