using System.Windows;
using SerialPortTool.UI.Infrastructure;

namespace SerialPortTool.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize service container
            ServiceContainer.Instance.RegisterServices();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Clean up resources
            ServiceContainer.Instance.Cleanup();
            
            base.OnExit(e);
        }
    }
}
