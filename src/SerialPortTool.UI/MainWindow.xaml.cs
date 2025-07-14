using System;
using System.Windows;
using SerialPortTool.UI.Infrastructure;
using SerialPortTool.UI.ViewModels;

namespace SerialPortTool.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            // Get ViewModel instance
            _viewModel = ServiceContainer.Instance.GetMainWindowViewModel();
            
            // Set DataContext
            this.DataContext = _viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize port list (handled in ViewModel)
            _viewModel.RefreshPortsCommand.Execute(null);
        }

        protected override void OnClosed(EventArgs e)
        {
            // Ensure resources are cleaned up
            _viewModel?.Dispose();
            base.OnClosed(e);
        }
    }
}
