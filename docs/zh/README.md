# 串口调试助手

[![许可证: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-4.8%20%7C%208.0-blue)](https://dotnet.microsoft.com/)
[![平台](https://img.shields.io/badge/Platform-Windows-lightgrey)](https://www.microsoft.com/windows/)

一个基于 WPF 和 .NET 构建的现代化、功能丰富的串口通信和调试工具。具有清晰的 MVVM 架构、完善的数据转换工具和直观的用户界面。

[English Documentation](../en/README.md) | [中文文档](../zh/README.md)

## ✨ 特性

- **现代化架构**：采用 MVVM 模式和依赖注入构建
- **多框架支持**：支持 .NET Framework 4.8 和 .NET 8.0
- **实时通信**：自动和手动数据接收模式
- **数据格式转换**：无缝的文本 ↔ 十六进制转换
- **跨平台核心**：核心库可在其他应用程序中使用
- **线程安全操作**：稳健的错误处理和资源管理
- **直观界面**：清晰、响应式的 Windows 界面

## 🚀 快速开始

### 系统要求

- Windows 10/11
- .NET Framework 4.8 或 .NET 8.0 运行时
- 可用的串口（物理或虚拟）

### 安装

1. **下载**：从 [Releases](https://github.com/hongjiapeng/SerialPortTool/releases) 获取最新版本
2. **解压**：解压到您喜欢的目录
3. **运行**：执行 `SerialPortTool.exe`

### 从源代码构建

```bash
# 克隆仓库
git clone https://github.com/hongjiapeng/SerialPortTool.git
cd SerialPortTool

# 构建解决方案
dotnet build

# 运行应用程序
dotnet run --project src/SerialPortTool.UI
```

## 📁 项目结构

```
SerialPortTool/
├── src/
│   ├── SerialPortTool.Core/          # 核心库（可复用）
│   │   ├── Services/                 # 串口服务
│   │   └── Utils/                    # 数据转换工具
│   └── SerialPortTool.UI/            # WPF 用户界面
│       ├── ViewModels/               # MVVM 视图模型
│       ├── Infrastructure/           # 依赖注入容器
│       └── Assets/                   # 界面资源
├── docs/                             # 文档
│   ├── en/                          # 英文文档
│   └── zh/                          # 中文文档
├── .editorconfig                     # 代码风格配置
├── Directory.Build.props             # MSBuild 属性
└── global.json                      # .NET SDK 版本
```

## 🎯 使用方法

### 基本操作

1. **选择端口**：从可用的 COM 端口中选择
2. **配置参数**：设置波特率、数据位、校验位、停止位
3. **连接**：点击"连接"建立连接
4. **发送数据**：输入文本或十六进制数据并点击"发送"
5. **接收数据**：在实时或手动模式下查看传入数据

### 数据格式

- **文本模式**：发送/接收纯文本字符串
- **十六进制模式**：发送/接收十六进制格式数据（例如："01 02 03 FF"）

### 接收模式

- **响应模式**：自动接收并显示传入数据
- **应答模式**：手动从缓冲区读取数据

## 🏗️ 架构

应用程序遵循现代软件架构原则：

- **核心库**：可复用的串口通信组件
- **MVVM 模式**：UI 和业务逻辑的清晰分离
- **依赖注入**：松耦合和可测试性
- **事件驱动**：响应式数据处理
- **线程安全**：稳健的并发操作

详细的架构信息，请参阅[架构文档](ARCHITECTURE.md)。

## 📦 NuGet 包

核心库作为 NuGet 包提供，可在其他应用程序中使用：

```xml
<PackageReference Include="SerialPortTool.Core" Version="2.0.0" />
```

```csharp
using SerialPortTool.Core.Services;

// 创建服务实例
var serialService = new SerialPortService();

// 配置连接
var config = new SerialPortConfiguration
{
    PortName = "COM1",
    BaudRate = 9600
};

// 打开端口并发送数据
var result = serialService.OpenPort(config);
if (result.Success)
{
    serialService.SendText("Hello, World!");
}
```

## 🤝 贡献

欢迎贡献！在提交拉取请求之前，请阅读我们的[贡献指南](../../CONTRIBUTING.md)。

### 开发环境设置

1. 安装 Visual Studio 2022 或 VS Code
2. 安装 .NET 8.0 SDK
3. 克隆并打开解决方案
4. 构建并运行测试

## 📄 许可证

本项目基于 MIT 许可证 - 详情请查看 [LICENSE](../../LICENSE) 文件。

## 🔗 链接

- [项目主页](https://github.com/hongjiapeng/SerialPortTool)
- [问题跟踪](https://github.com/hongjiapeng/SerialPortTool/issues)
- [发布说明](https://github.com/hongjiapeng/SerialPortTool/releases)
- [文档](../)

## 🙏 致谢

- 使用 [.NET](https://dotnet.microsoft.com/) 和 [WPF](https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/) 构建
- 串口通信基于 [System.IO.Ports](https://www.nuget.org/packages/System.IO.Ports/)
- 图标来自 [Material Design Icons](https://materialdesignicons.com/)

---

<div align="center">
由 <a href="https://github.com/hongjiapeng">hongjiapeng</a> 用 ❤️ 制作
</div>
