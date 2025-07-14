# 串口调试助手 - 命令行运行指南

## 📋 前置要求

确保您已安装：
- **.NET 8.0 SDK** (用于运行 .NET 8.0 版本)
- **.NET Framework 4.8** (用于运行 .NET Framework 版本)

## 🚀 运行方式

### 方式一：直接运行（推荐）

从项目根目录 `SerialPortTool` 运行：

```bash
# 运行 .NET 8.0 版本（推荐）
dotnet run --project SerialPortTool\SerialPortTool.csproj -f net8.0-windows

# 运行 .NET Framework 4.8 版本
dotnet run --project SerialPortTool\SerialPortTool.csproj -f net48
```

从项目文件目录 `SerialPortTool\SerialPortTool` 运行：

```bash
# 进入项目目录
cd SerialPortTool

# 运行 .NET 8.0 版本
dotnet run -f net8.0-windows

# 运行 .NET Framework 4.8 版本
dotnet run -f net48
```

### 方式二：构建后运行

```bash
# 1. 构建项目
dotnet build SerialPortTool\SerialPortTool.csproj

# 2. 运行生成的可执行文件

# .NET Framework 4.8 版本（独立可执行文件）
.\SerialPortTool\bin\Debug\net48\串口调试助手.exe

# .NET 8.0 版本（需要dotnet运行时）
dotnet .\SerialPortTool\bin\Debug\net8.0-windows\串口调试助手.dll
```

### 方式三：发布后运行

```bash
# 发布自包含的 .NET 8.0 应用
dotnet publish SerialPortTool\SerialPortTool.csproj -f net8.0-windows -c Release --self-contained true -p:PublishSingleFile=true -o publish

# 运行发布的应用
.\publish\串口调试助手.exe
```

## 🎯 推荐的运行方式

### 对于开发和测试：
```bash
# 切换到项目目录
cd SerialPortTool

# 运行 .NET 8.0 版本（性能更好）
dotnet run -f net8.0-windows
```

### 对于生产部署：
```bash
# 发布独立应用
dotnet publish SerialPortTool\SerialPortTool.csproj -f net8.0-windows -c Release --self-contained true -p:PublishSingleFile=true -o dist

# 分发生成的可执行文件
.\dist\串口调试助手.exe
```

## 📁 目录结构说明

```
SerialPortTool/                    # 项目根目录
├── SerialPortTool/                # 项目源码目录
│   ├── SerialPortTool.csproj     # 项目文件
│   ├── MainWindow.xaml           # 主窗口界面
│   ├── MainWindow.xaml.cs        # 主窗口逻辑
│   └── bin/                      # 构建输出目录
│       ├── Debug/
│       │   ├── net48/           # .NET Framework 4.8 输出
│       │   └── net8.0-windows/  # .NET 8.0 输出
│       └── Release/             # Release 版本输出
├── README.md                     # 项目说明
└── global.json                  # .NET SDK 配置
```

## 🔧 常见问题解决

### 问题1：找不到项目文件
```bash
# 确保在正确的目录下，或使用完整路径
dotnet run --project "完整路径\SerialPortTool.csproj" -f net8.0-windows
```

### 问题2：目标框架不存在
```bash
# 检查可用的目标框架
dotnet build SerialPortTool\SerialPortTool.csproj --verbosity normal
```

### 问题3：运行时错误
```bash
# 检查依赖是否正确安装
dotnet restore SerialPortTool\SerialPortTool.csproj
dotnet build SerialPortTool\SerialPortTool.csproj
```

## ⚡ 性能对比

| 框架版本 | 启动速度 | 内存占用 | 兼容性 | 推荐场景 |
|---------|---------|---------|--------|----------|
| .NET 8.0 | 更快 | 更低 | Windows 10+ | 现代环境 |
| .NET Framework 4.8 | 较慢 | 较高 | Windows 7+ | 传统环境 |

## 📝 注意事项

1. **管理员权限**：访问串口设备可能需要管理员权限
2. **串口驱动**：确保串口设备驱动已正确安装
3. **防火墙**：某些安全软件可能拦截串口访问
4. **端口占用**：确保串口未被其他应用程序占用

## 🎉 快速开始

最简单的运行方式：

```bash
# 1. 克隆或下载项目
git clone https://github.com/hongjiapeng/SerialPortTool.git

# 2. 进入项目目录
cd SerialPortTool

# 3. 运行程序
dotnet run --project SerialPortTool\SerialPortTool.csproj -f net8.0-windows
```

程序启动后，您就可以开始使用串口调试功能了！
