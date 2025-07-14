using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SerialPortTool.Services;
using SerialPortTool.Utils;

namespace SerialPortTool.ViewModels
{
    /// <summary>
    /// 简单的命令实现
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 主窗口ViewModel
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly ISerialPortService _serialPortService;
        private bool _disposed = false;

        // 串口配置属性
        private string _selectedPort = "COM1";
        private string _selectedBaudRate = "9600";
        private string _selectedDataBits = "8";
        private int _selectedStopBitsIndex = 0;
        private int _selectedParityIndex = 0;

        // UI状态属性
        private string _statusText = "就绪";
        private bool _isPortOpen = false;
        private bool _sendEnabled = false;
        private bool _receiveEnabled = false;

        // 数据属性
        private string _sendText = "";
        private string _receiveText = "";
        private bool _sendAsHex = false;
        private bool _receiveAsHex = false;

        // 接收模式
        private bool _isResponseMode = true;
        private bool _isAckMode = false;

        public MainWindowViewModel(ISerialPortService serialPortService)
        {
            _serialPortService = serialPortService ?? throw new ArgumentNullException(nameof(serialPortService));
            
            // 订阅服务事件
            _serialPortService.DataReceived += OnDataReceived;
            _serialPortService.StatusChanged += OnStatusChanged;

            // 初始化命令
            InitializeCommands();

            // 初始化数据
            InitializeData();
        }

        #region 命令属性

        public ICommand ToggleConnectionCommand { get; private set; }
        public ICommand SendDataCommand { get; private set; }
        public ICommand ReceiveDataCommand { get; private set; }
        public ICommand RefreshPortsCommand { get; private set; }
        public ICommand ClearReceiveCommand { get; private set; }
        public ICommand ClearSendCommand { get; private set; }

        #endregion

        #region 属性

        public ObservableCollection<string> AvailablePorts { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> BaudRates { get; } = new ObservableCollection<string>
        {
            "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200"
        };
        public ObservableCollection<string> DataBitsList { get; } = new ObservableCollection<string>
        {
            "5", "6", "7", "8"
        };
        public ObservableCollection<string> StopBitsList { get; } = new ObservableCollection<string>
        {
            "1", "2", "1.5", "None"
        };
        public ObservableCollection<string> ParityList { get; } = new ObservableCollection<string>
        {
            "None", "Odd", "Even"
        };

        public string SelectedPort
        {
            get => _selectedPort;
            set => SetProperty(ref _selectedPort, value);
        }

        public string SelectedBaudRate
        {
            get => _selectedBaudRate;
            set => SetProperty(ref _selectedBaudRate, value);
        }

        public string SelectedDataBits
        {
            get => _selectedDataBits;
            set => SetProperty(ref _selectedDataBits, value);
        }

        public int SelectedStopBitsIndex
        {
            get => _selectedStopBitsIndex;
            set => SetProperty(ref _selectedStopBitsIndex, value);
        }

        public int SelectedParityIndex
        {
            get => _selectedParityIndex;
            set => SetProperty(ref _selectedParityIndex, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public bool IsPortOpen
        {
            get => _isPortOpen;
            set
            {
                if (SetProperty(ref _isPortOpen, value))
                {
                    OnPropertyChanged(nameof(PortButtonText));
                    UpdateButtonStates();
                }
            }
        }

        public string PortButtonText => IsPortOpen ? "关闭串口" : "打开串口";

        public bool SendEnabled
        {
            get => _sendEnabled;
            set => SetProperty(ref _sendEnabled, value);
        }

        public bool ReceiveEnabled
        {
            get => _receiveEnabled;
            set => SetProperty(ref _receiveEnabled, value);
        }

        public string SendText
        {
            get => _sendText;
            set => SetProperty(ref _sendText, value);
        }

        public string ReceiveText
        {
            get => _receiveText;
            set => SetProperty(ref _receiveText, value);
        }

        public bool SendAsHex
        {
            get => _sendAsHex;
            set
            {
                if (SetProperty(ref _sendAsHex, value))
                {
                    ConvertSendTextFormat();
                }
            }
        }

        public bool ReceiveAsHex
        {
            get => _receiveAsHex;
            set
            {
                if (SetProperty(ref _receiveAsHex, value))
                {
                    ConvertReceiveTextFormat();
                }
            }
        }

        public bool IsResponseMode
        {
            get => _isResponseMode;
            set
            {
                if (SetProperty(ref _isResponseMode, value) && value)
                {
                    IsAckMode = false;
                    _serialPortService.SetReceiveMode(ReceiveMode.Response);
                    UpdateButtonStates();
                }
            }
        }

        public bool IsAckMode
        {
            get => _isAckMode;
            set
            {
                if (SetProperty(ref _isAckMode, value) && value)
                {
                    IsResponseMode = false;
                    _serialPortService.SetReceiveMode(ReceiveMode.Acknowledgment);
                    UpdateButtonStates();
                }
            }
        }

        public bool CanSend => IsPortOpen;
        public bool CanReceive => IsPortOpen && _isAckMode;

        #endregion

        #region 命令方法

        public void TogglePort()
        {
            if (IsPortOpen)
            {
                ClosePort();
            }
            else
            {
                OpenPort();
            }
        }

        public void SendData()
        {
            if (string.IsNullOrWhiteSpace(SendText))
            {
                StatusText = "发送内容不能为空";
                return;
            }

            OperationResult result;
            if (SendAsHex)
            {
                result = _serialPortService.SendHexData(SendText);
            }
            else
            {
                result = _serialPortService.SendText(SendText);
            }

            StatusText = result.Message;
        }

        public void ReceiveData()
        {
            var result = _serialPortService.ReadData();
            if (result.Success && result.Data != null && result.Data.Length > 0)
            {
                ReceiveText = ReceiveAsHex ? result.HexData : result.TextData;
            }
            StatusText = result.Message;
        }

        public void RefreshPorts()
        {
            var ports = _serialPortService.GetAvailablePorts();
            AvailablePorts.Clear();
            foreach (var port in ports)
            {
                AvailablePorts.Add(port);
            }

            if (AvailablePorts.Count > 0 && !AvailablePorts.Contains(SelectedPort))
            {
                SelectedPort = AvailablePorts[0];
            }
        }

        public void ClearReceiveText()
        {
            ReceiveText = "";
        }

        public void ClearSendText()
        {
            SendText = "";
        }

        #endregion

        #region 私有方法

        private void InitializeCommands()
        {
            ToggleConnectionCommand = new RelayCommand(TogglePort);
            SendDataCommand = new RelayCommand(SendData, () => CanSend);
            ReceiveDataCommand = new RelayCommand(ReceiveData, () => CanReceive);
            RefreshPortsCommand = new RelayCommand(RefreshPorts);
            ClearReceiveCommand = new RelayCommand(ClearReceiveText);
            ClearSendCommand = new RelayCommand(ClearSendText);
        }

        private void InitializeData()
        {
            RefreshPorts();
            IsResponseMode = true;
            _serialPortService.SetReceiveMode(ReceiveMode.Response);
        }

        private void OpenPort()
        {
            try
            {
                var config = new SerialPortConfig
                {
                    PortName = SelectedPort,
                    BaudRate = int.Parse(SelectedBaudRate),
                    DataBits = int.Parse(SelectedDataBits),
                    StopBits = GetStopBits(),
                    Parity = GetParity()
                };

                var result = _serialPortService.OpenPort(config);
                StatusText = result.Message;

                if (result.Success)
                {
                    IsPortOpen = true;
                }
            }
            catch (Exception ex)
            {
                StatusText = $"串口配置错误: {ex.Message}";
            }
        }

        private void ClosePort()
        {
            var result = _serialPortService.ClosePort();
            StatusText = result.Message;
            IsPortOpen = false;
        }

        private StopBits GetStopBits()
        {
            return SelectedStopBitsIndex switch
            {
                0 => StopBits.One,
                1 => StopBits.Two,
                2 => StopBits.OnePointFive,
                3 => StopBits.None,
                _ => StopBits.One
            };
        }

        private Parity GetParity()
        {
            return SelectedParityIndex switch
            {
                0 => Parity.None,
                1 => Parity.Odd,
                2 => Parity.Even,
                _ => Parity.None
            };
        }

        private void UpdateButtonStates()
        {
            SendEnabled = IsPortOpen;
            ReceiveEnabled = IsPortOpen && IsAckMode;
        }

        private void ConvertSendTextFormat()
        {
            if (string.IsNullOrEmpty(SendText)) return;

            try
            {
                if (SendAsHex)
                {
                    // 转换为十六进制显示
                    var bytes = System.Text.Encoding.Default.GetBytes(SendText);
                    SendText = Utils.DataConverter.BytesToHexString(bytes);
                }
                else
                {
                    // 转换为文本显示
                    var bytes = Utils.DataConverter.HexStringToBytes(SendText);
                    SendText = Utils.DataConverter.BytesToText(bytes);
                }
            }
            catch
            {
                StatusText = "发送数据格式转换失败，请检查数据格式";
            }
        }

        private void ConvertReceiveTextFormat()
        {
            if (string.IsNullOrEmpty(ReceiveText)) return;

            try
            {
                if (ReceiveAsHex)
                {
                    // 转换为十六进制显示
                    var bytes = System.Text.Encoding.Default.GetBytes(ReceiveText);
                    ReceiveText = Utils.DataConverter.BytesToHexString(bytes);
                }
                else
                {
                    // 转换为文本显示
                    var bytes = Utils.DataConverter.HexStringToBytes(ReceiveText);
                    ReceiveText = Utils.DataConverter.BytesToText(bytes);
                }
            }
            catch
            {
                StatusText = "接收数据格式转换失败，请检查数据格式";
            }
        }

        #endregion

        #region 事件处理

        private void OnDataReceived(object sender, SerialDataReceivedArgs e)
        {
            // 在UI线程中更新接收文本
            Application.Current.Dispatcher.Invoke(() =>
            {
                var newData = ReceiveAsHex ? e.HexData : e.TextData;
                ReceiveText += newData;
                StatusText = $"接收到 {e.Data.Length} 字节数据";
            });
        }

        private void OnStatusChanged(object sender, SerialPortStatusChangedArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsPortOpen = e.IsConnected;
                StatusText = e.Message;
            });
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _serialPortService.DataReceived -= OnDataReceived;
                _serialPortService.StatusChanged -= OnStatusChanged;
                _serialPortService?.Dispose();
                _disposed = true;
            }
        }

        ~MainWindowViewModel()
        {
            Dispose(false);
        }

        #endregion
    }
}
