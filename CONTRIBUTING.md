# 贡献指南

感谢您对串口调试助手项目的关注！我们欢迎各种形式的贡献，包括但不限于：

- 报告bug
- 提出新功能建议
- 提交代码改进
- 完善文档
- 翻译内容

## 如何贡献

### 报告Bug

如果您发现了bug，请创建一个Issue，并包含以下信息：

1. **Bug描述**：清楚地描述问题是什么
2. **重现步骤**：详细说明如何重现这个问题
3. **期望行为**：描述您期望应该发生什么
4. **实际行为**：描述实际发生了什么
5. **环境信息**：
   - 操作系统版本
   - .NET Framework版本
   - 串口设备信息
6. **截图**：如果可能，请提供相关截图

### 提出功能请求

如果您有新功能的想法，请创建一个Issue，并包含：

1. **功能描述**：清楚地描述您希望添加的功能
2. **使用场景**：解释为什么需要这个功能
3. **可能的实现方案**：如果有想法，请分享
4. **优先级**：说明这个功能对您的重要程度

### 提交代码

1. **Fork仓库**
   ```bash
   # 点击GitHub页面上的Fork按钮，然后克隆您的fork
   git clone https://github.com/YOUR_USERNAME/SerialPortTool.git
   cd SerialPortTool
   ```

2. **创建分支**
   ```bash
   git checkout -b feature/your-feature-name
   # 或者
   git checkout -b bugfix/your-bugfix-name
   ```

3. **编写代码**
   - 遵循现有的代码风格
   - 添加必要的注释
   - 确保代码能够正常编译和运行

4. **测试**
   - 在不同的环境下测试您的修改
   - 确保没有破坏现有功能

5. **提交**
   ```bash
   git add .
   git commit -m "Add: 简短描述您的修改"
   ```

6. **推送**
   ```bash
   git push origin feature/your-feature-name
   ```

7. **创建Pull Request**
   - 在GitHub上创建Pull Request
   - 详细描述您的修改
   - 关联相关的Issue（如果有）

## 代码规范

### C#代码风格

- 使用PascalCase命名类、方法、属性
- 使用camelCase命名变量、参数
- 使用有意义的变量和方法名
- 添加必要的XML文档注释
- 保持代码简洁，避免过长的方法

```csharp
/// <summary>
/// 打开串口连接
/// </summary>
/// <param name="portName">串口名称</param>
/// <param name="baudRate">波特率</param>
/// <returns>是否成功打开</returns>
public bool OpenSerialPort(string portName, int baudRate)
{
    try
    {
        // 实现代码
        return true;
    }
    catch (Exception ex)
    {
        // 错误处理
        return false;
    }
}
```

### XAML代码风格

- 使用合理的缩进
- 保持标签的一致性
- 使用有意义的控件名称

```xml
<Button x:Name="btnOpenPort" 
        Content="打开串口" 
        Click="BtnOpenPort_Click"
        Width="100" 
        Height="30" />
```

## 提交消息格式

使用清晰的提交消息格式：

```
类型: 简短描述

详细描述（可选）

关联的Issue: #123
```

类型包括：
- `Add`: 添加新功能
- `Fix`: 修复bug
- `Update`: 更新现有功能
- `Remove`: 删除功能或代码
- `Docs`: 文档相关修改
- `Style`: 代码格式修改
- `Refactor`: 重构代码
- `Test`: 测试相关

## 开发环境设置

### 必需软件

1. **Visual Studio 2015 或更高版本**
2. **.NET Framework 4.5 或更高版本**
3. **Git**

### 推荐工具

1. **Visual Studio Code**（用于快速编辑）
2. **GitKraken 或 SourceTree**（Git GUI工具）

### 项目设置

1. 克隆仓库后，用Visual Studio打开`SerialPortTool/SerialPortTool.sln`
2. 生成解决方案确保所有依赖项正确加载
3. 设置`SerialPortTool`为启动项目

## 行为准则

参与本项目时，请遵守以下准则：

- 尊重他人，保持友善的沟通
- 接受建设性的反馈
- 专注于对项目最有利的事情
- 对新贡献者表示同理心

## 获得帮助

如果您在贡献过程中遇到问题，可以：

1. 查看现有的Issue和Pull Request
2. 创建新的Issue询问问题
3. 在Pull Request中请求代码审查

感谢您的贡献！
