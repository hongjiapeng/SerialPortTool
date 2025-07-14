using System;
using SerialPortTool.Core.Services;
using SerialPortTool.UI.ViewModels;

namespace SerialPortTool.UI.Infrastructure
{
    /// <summary>
    /// Simple dependency injection container for managing service lifetimes
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
        /// Registers all required services
        /// </summary>
        public void RegisterServices()
        {
            // Register serial port service
            _serialPortService = new SerialPortService();

            // Register ViewModel
            _mainWindowViewModel = new MainWindowViewModel(_serialPortService);
        }

        /// <summary>
        /// Gets the serial port service instance
        /// </summary>
        /// <returns>Serial port service instance</returns>
        public ISerialPortService GetSerialPortService()
        {
            return _serialPortService ?? throw new InvalidOperationException("Services not registered. Call RegisterServices first.");
        }

        /// <summary>
        /// Gets the main window ViewModel instance
        /// </summary>
        /// <returns>Main window ViewModel instance</returns>
        public MainWindowViewModel GetMainWindowViewModel()
        {
            return _mainWindowViewModel ?? throw new InvalidOperationException("ViewModel not registered. Call RegisterServices first.");
        }

        /// <summary>
        /// Cleans up all registered services
        /// </summary>
        public void Cleanup()
        {
            _mainWindowViewModel?.Dispose();
            _serialPortService?.Dispose();
        }
    }
}
