using System;
using SerialPortTool.Services;
using SerialPortTool.ViewModels;

namespace SerialPortTool.Infrastructure
{
    /// <summary>
    /// 简单的依赖注入容器
    /// </summary>
    public class ServiceContainer
    {
        private static readonly Lazy<ServiceContainer> _instance = new Lazy<ServiceContainer>(() => new ServiceContainer());
        public static ServiceContainer Instance => _instance.Value;

        private ISerialPortService _serialPortService;
        private MainWindowViewModel _mainWindowViewModel;

        private ServiceContainer()
        {
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public void RegisterServices()
        {
            // 注册串口服务
            _serialPortService = new SerialPortService();

            // 注册ViewModel
            _mainWindowViewModel = new MainWindowViewModel(_serialPortService);
        }

        /// <summary>
        /// 获取串口服务
        /// </summary>
        /// <returns></returns>
        public ISerialPortService GetSerialPortService()
        {
            return _serialPortService ?? throw new InvalidOperationException("服务未注册，请先调用RegisterServices");
        }

        /// <summary>
        /// 获取主窗口ViewModel
        /// </summary>
        /// <returns></returns>
        public MainWindowViewModel GetMainWindowViewModel()
        {
            return _mainWindowViewModel ?? throw new InvalidOperationException("ViewModel未注册，请先调用RegisterServices");
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Cleanup()
        {
            _mainWindowViewModel?.Dispose();
            _serialPortService?.Dispose();
        }
    }
}
