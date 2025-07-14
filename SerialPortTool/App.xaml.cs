using System.Windows;
using SerialPortTool.Infrastructure;

namespace SerialPortTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // 初始化服务容器
            ServiceContainer.Instance.RegisterServices();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 清理资源
            ServiceContainer.Instance.Cleanup();
            
            base.OnExit(e);
        }
    }
}
