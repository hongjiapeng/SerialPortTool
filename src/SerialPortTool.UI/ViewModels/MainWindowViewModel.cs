using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SerialPortTool.Core.Services;
using SerialPortTool.Core.Utils;

namespace SerialPortTool.UI.ViewModels
{
    /// <summary>
    /// Simple command implementation for WPF
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
    /// Main window ViewModel implementing MVVM pattern
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly ISerialPortService _serialPortService;
        private bool _disposed;

        // Private fields for properties
        private string _selectedPort = "";
        private string _selectedBaudRate = "9600";
        private string _selectedDataBits = "8";
        private int _selectedStopBitsIndex = 0;
        private int _selectedParityIndex = 0;
        private string _statusText = "Ready";
        private bool _isPortOpen;
        private string _sendText = "";
        private string _receivedText = "";
        private bool _sendAsHex;
        private bool _receiveAsHex;
        private bool _isResponseMode = true;
        private bool _isAcknowledgeMode;

        public MainWindowViewModel(ISerialPortService serialPortService)
        {
            _serialPortService = serialPortService ?? throw new ArgumentNullException(nameof(serialPortService));

            // Subscribe to service events
            _serialPortService.DataReceived += OnDataReceived;
            _serialPortService.StatusChanged += OnStatusChanged;

            // Initialize commands
            InitializeCommands();

            // Initialize data
            InitializeData();
        }

        #region Commands

        public ICommand ToggleConnectionCommand { get; private set; }
        public ICommand SendDataCommand { get; private set; }
        public ICommand ReceiveDataCommand { get; private set; }
        public ICommand RefreshPortsCommand { get; private set; }
        public ICommand ClearReceiveCommand { get; private set; }
        public ICommand ClearSendCommand { get; private set; }

        #endregion

        #region Properties

        public ObservableCollection<string> AvailablePorts { get; } = new ObservableCollection<string>();

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
                    OnPropertyChanged(nameof(ConnectButtonText));
                    OnPropertyChanged(nameof(CanSend));
                    OnPropertyChanged(nameof(CanReceive));
                    ((RelayCommand)SendDataCommand)?.RaiseCanExecuteChanged();
                    ((RelayCommand)ReceiveDataCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string ConnectButtonText => IsPortOpen ? "Disconnect" : "Connect";

        public string SendText
        {
            get => _sendText;
            set => SetProperty(ref _sendText, value);
        }

        public string ReceivedText
        {
            get => _receivedText;
            set => SetProperty(ref _receivedText, value);
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
                    IsAcknowledgeMode = false;
                    _serialPortService.SetReceiveMode(ReceiveMode.Response);
                    OnPropertyChanged(nameof(CanReceive));
                    ((RelayCommand)ReceiveDataCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsAcknowledgeMode
        {
            get => _isAcknowledgeMode;
            set
            {
                if (SetProperty(ref _isAcknowledgeMode, value) && value)
                {
                    IsResponseMode = false;
                    _serialPortService.SetReceiveMode(ReceiveMode.Acknowledgment);
                    OnPropertyChanged(nameof(CanReceive));
                    ((RelayCommand)ReceiveDataCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public bool CanSend => IsPortOpen;
        public bool CanReceive => IsPortOpen && IsAcknowledgeMode;

        #endregion

        #region Command Methods

        public void ToggleConnection()
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
                StatusText = "Send text cannot be empty";
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
                ReceivedText = ReceiveAsHex ? result.HexData : result.TextData;
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
            ReceivedText = "";
        }

        public void ClearSendText()
        {
            SendText = "";
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            ToggleConnectionCommand = new RelayCommand(ToggleConnection);
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
                var config = new SerialPortConfiguration
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
                StatusText = $"Configuration error: {ex.Message}";
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

        private void ConvertSendTextFormat()
        {
            if (string.IsNullOrEmpty(SendText)) return;

            try
            {
                if (SendAsHex)
                {
                    // Convert to hexadecimal display
                    var bytes = DataConverter.TextToBytes(SendText);
                    SendText = DataConverter.BytesToHexString(bytes);
                }
                else
                {
                    // Convert to text display
                    var bytes = DataConverter.HexStringToBytes(SendText);
                    SendText = DataConverter.BytesToText(bytes);
                }
            }
            catch
            {
                StatusText = "Send data format conversion failed. Please check data format.";
            }
        }

        private void ConvertReceiveTextFormat()
        {
            if (string.IsNullOrEmpty(ReceivedText)) return;

            try
            {
                if (ReceiveAsHex)
                {
                    // Convert to hexadecimal display
                    var bytes = DataConverter.TextToBytes(ReceivedText);
                    ReceivedText = DataConverter.BytesToHexString(bytes);
                }
                else
                {
                    // Convert to text display
                    var bytes = DataConverter.HexStringToBytes(ReceivedText);
                    ReceivedText = DataConverter.BytesToText(bytes);
                }
            }
            catch
            {
                StatusText = "Receive data format conversion failed. Please check data format.";
            }
        }

        private void OnDataReceived(object sender, SerialPortTool.Core.Services.SerialDataReceivedEventArgs e)
        {
            // Update UI on UI thread
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                ReceivedText = ReceiveAsHex ? e.HexData : e.TextData;
                StatusText = $"Received {e.Data.Length} bytes at {e.Timestamp:HH:mm:ss}";
            });
        }

        private void OnStatusChanged(object sender, SerialPortStatusChangedEventArgs e)
        {
            // Update UI on UI thread
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                IsPortOpen = e.IsConnected;
                StatusText = e.Message;
            });
        }

        #endregion

        #region INotifyPropertyChanged Implementation

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

        #region IDisposable Implementation

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
