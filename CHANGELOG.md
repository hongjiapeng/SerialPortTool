# 更新日志 (CHANGELOG)

## [2.0.0] - 2025-07-14

### 🚀 重大升级

- **现代化框架支持**: 项目已从传统的 .NET Framework 4.5 升级到支持多目标框架
  - .NET Framework 4.8
  - .NET 8.0 (Windows)
- **SDK风格项目**: 从传统的项目文件格式迁移到现代的SDK风格项目文件
- **NuGet包管理**: 改用NuGet包引用方式管理 `System.IO.Ports` 依赖

### ✨ 新增功能

- **多目标框架构建**: 单个解决方案同时支持.NET Framework和.NET的最新版本
- **现代化开发体验**: 
  - 支持 `dotnet` CLI 命令
  - 更好的IDE支持
  - 自动程序集信息生成
- **代码质量改进**:
  - 添加 EditorConfig 配置文件
  - 统一代码风格设置
  - 现代化的项目结构

### 🛠 技术改进

- **依赖管理**: 
  - 统一使用NuGet包引用
  - 移除传统的程序集引用
  - 添加NuGet.config配置
- **构建系统**:
  - 支持 `dotnet build`、`dotnet publish` 命令
  - 条件编译符号支持
  - 自动生成版本信息

### 📚 文档完善

- **README.md**: 全面更新项目说明，包含多目标框架信息
- **贡献指南**: 添加 CONTRIBUTING.md 文件
- **许可证**: 更新到 MIT License
- **.gitignore**: 添加现代化的忽略文件配置

### 🔧 配置文件

新增文件：
- `Directory.Build.props` - 统一项目属性管理
- `global.json` - 指定.NET SDK版本
- `.editorconfig` - 代码风格配置
- `NuGet.config` - NuGet包源配置

### ⚡ 性能和兼容性

- **向后兼容**: 保持与现有代码的完全兼容性
- **未来就绪**: 支持最新的.NET版本，便于后续升级
- **开发效率**: 现代化的开发工具链支持

### 🏗 构建说明

**使用.NET CLI构建：**
```bash
# 构建所有目标框架
dotnet build

# 构建特定框架
dotnet build -f net8.0-windows
dotnet build -f net48

# 发布应用
dotnet publish -f net8.0-windows -c Release --self-contained
```

**使用Visual Studio：**
- 需要 Visual Studio 2022 或更高版本
- 自动支持多目标框架切换
- 完整的IntelliSense支持

---

## [1.0.0] - 原始版本

### 基础功能
- 基础串口通信功能
- 支持多种波特率和参数配置
- 文本和十六进制数据收发
- 响应模式和轮询模式
- 基于.NET Framework 4.5的WPF应用
