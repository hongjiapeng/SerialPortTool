using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using SerialPortTool.Core.Utils;

namespace SerialPortTool.Core.Services
{
    /// <summary>
    /// Implementation of serial port communication service with thread-safe operations
    /// </summary>
    public class SerialPortService : ISerialPortService
    {
        private SerialPort _serialPort;
        private readonly object _lockObject = new object();
        private bool _disposed;
        private ReceiveMode _receiveMode = ReceiveMode.Response;

        /// <summary>
        /// Initializes a new instance of the SerialPortService class
        /// </summary>
        public SerialPortService()
        {
            _serialPort = new SerialPort();
        }

        #region Events

        /// <inheritdoc />
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        /// <inheritdoc />
        public event EventHandler<SerialPortStatusChangedEventArgs> StatusChanged;

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool IsConnected => _serialPort?.IsOpen ?? false;

        /// <inheritdoc />
        public SerialPortConfiguration CurrentConfiguration { get; private set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public string[] GetAvailablePorts()
        {
            try
            {
                return SerialPort.GetPortNames();
            }
            catch (Exception ex)
            {
                OnStatusChanged(false, $"Failed to get available ports: {ex.Message}", ex);
                return Array.Empty<string>();
            }
        }

        /// <inheritdoc />
        public OperationResult OpenPort(SerialPortConfiguration configuration)
        {
            if (configuration == null)
                return OperationResult.Failed("Configuration cannot be null");

            lock (_lockObject)
            {
                try
                {
                    if (IsConnected)
                    {
                        ClosePort();
                    }

                    _serialPort = new SerialPort
                    {
                        PortName = configuration.PortName,
                        BaudRate = configuration.BaudRate,
                        DataBits = configuration.DataBits,
                        StopBits = configuration.StopBits,
                        Parity = configuration.Parity,
                        ReadTimeout = configuration.ReadTimeout,
                        WriteTimeout = configuration.WriteTimeout
                    };

                    // Subscribe to data received event if in response mode
                    if (_receiveMode == ReceiveMode.Response)
                    {
                        _serialPort.DataReceived += SerialPort_DataReceived;
                    }

                    _serialPort.Open();
                    CurrentConfiguration = configuration;

                    var message = $"Serial port {configuration.PortName} opened successfully";
                    OnStatusChanged(true, message);
                    return OperationResult.Successful(message);
                }
                catch (Exception ex)
                {
                    var message = $"Failed to open serial port: {ex.Message}";
                    OnStatusChanged(false, message, ex);
                    return OperationResult.Failed(message, ex);
                }
            }
        }

        /// <inheritdoc />
        public OperationResult ClosePort()
        {
            lock (_lockObject)
            {
                try
                {
                    if (_serialPort?.IsOpen == true)
                    {
                        _serialPort.DataReceived -= SerialPort_DataReceived;
                        _serialPort.Close();
                    }

                    var message = "Serial port closed successfully";
                    OnStatusChanged(false, message);
                    return OperationResult.Successful(message);
                }
                catch (Exception ex)
                {
                    var message = $"Error closing serial port: {ex.Message}";
                    OnStatusChanged(false, message, ex);
                    return OperationResult.Failed(message, ex);
                }
            }
        }

        /// <inheritdoc />
        public OperationResult SendData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return OperationResult.Failed("Data cannot be null or empty");

            lock (_lockObject)
            {
                try
                {
                    if (!IsConnected)
                        return OperationResult.Failed("Serial port is not connected");

                    _serialPort.Write(data, 0, data.Length);
                    return OperationResult.Successful($"Sent {data.Length} bytes successfully");
                }
                catch (Exception ex)
                {
                    var message = $"Failed to send data: {ex.Message}";
                    return OperationResult.Failed(message, ex);
                }
            }
        }

        /// <inheritdoc />
        public OperationResult SendText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return OperationResult.Failed("Text cannot be null or empty");

            try
            {
                var data = Encoding.UTF8.GetBytes(text);
                return SendData(data);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Failed to convert text to bytes: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public OperationResult SendHexData(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                return OperationResult.Failed("Hex string cannot be null or empty");

            try
            {
                var data = DataConverter.HexStringToBytes(hexString);
                return SendData(data);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Invalid hex string format: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public ReadDataResult ReadData()
        {
            lock (_lockObject)
            {
                try
                {
                    if (!IsConnected)
                    {
                        return new ReadDataResult
                        {
                            Success = false,
                            Message = "Serial port is not connected"
                        };
                    }

                    int bytesToRead = _serialPort.BytesToRead;
                    if (bytesToRead == 0)
                    {
                        return new ReadDataResult
                        {
                            Success = true,
                            Message = "No data available to read",
                            Data = Array.Empty<byte>(),
                            TextData = string.Empty,
                            HexData = string.Empty
                        };
                    }

                    byte[] buffer = new byte[bytesToRead];
                    int bytesRead = _serialPort.Read(buffer, 0, bytesToRead);

                    // Resize buffer if necessary
                    if (bytesRead < bytesToRead)
                    {
                        Array.Resize(ref buffer, bytesRead);
                    }

                    var textData = DataConverter.BytesToText(buffer);
                    var hexData = DataConverter.BytesToHexString(buffer);

                    return new ReadDataResult
                    {
                        Success = true,
                        Message = $"Read {bytesRead} bytes successfully",
                        Data = buffer,
                        TextData = textData,
                        HexData = hexData
                    };
                }
                catch (Exception ex)
                {
                    return new ReadDataResult
                    {
                        Success = false,
                        Message = $"Failed to read data: {ex.Message}",
                        Exception = ex
                    };
                }
            }
        }

        /// <inheritdoc />
        public void SetReceiveMode(ReceiveMode mode)
        {
            lock (_lockObject)
            {
                _receiveMode = mode;

                if (_serialPort?.IsOpen == true)
                {
                    // Unsubscribe first
                    _serialPort.DataReceived -= SerialPort_DataReceived;

                    // Subscribe only if in response mode
                    if (mode == ReceiveMode.Response)
                    {
                        _serialPort.DataReceived += SerialPort_DataReceived;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles data received event from serial port
        /// </summary>
        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                var result = ReadData();
                if (result.Success && result.Data != null && result.Data.Length > 0)
                {
                    OnDataReceived(new SerialDataReceivedEventArgs
                    {
                        Data = result.Data,
                        TextData = result.TextData,
                        HexData = result.HexData,
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged(IsConnected, $"Error in data received handler: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Raises the DataReceived event
        /// </summary>
        private void OnDataReceived(SerialDataReceivedEventArgs args)
        {
            DataReceived?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the StatusChanged event
        /// </summary>
        private void OnStatusChanged(bool isConnected, string message, Exception exception = null)
        {
            StatusChanged?.Invoke(this, new SerialPortStatusChangedEventArgs
            {
                IsConnected = isConnected,
                Message = message,
                Exception = exception,
                Timestamp = DateTime.Now
            });
        }

        #endregion

        #region IDisposable Implementation

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing">True if disposing managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                try
                {
                    ClosePort();
                    _serialPort?.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~SerialPortService()
        {
            Dispose(false);
        }

        #endregion
    }
}
