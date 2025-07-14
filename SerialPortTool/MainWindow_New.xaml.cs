using System;
using System.Windows;
using SerialPortTool.Infrastructure;
using SerialPortTool.ViewModels;

namespace SerialPortTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            // 获取ViewModel实例
            _viewModel = ServiceContainer.Instance.GetMainWindowViewModel();
            
            // 设置DataContext
            this.DataContext = _viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化端口列表（这个现在在ViewModel中处理）
            _viewModel.RefreshPortsCommand.Execute(null);
        }

        protected override void OnClosed(EventArgs e)
        {
            // 确保资源被清理
            _viewModel?.Dispose();
            base.OnClosed(e);
        }
    }
}
