using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using SerialPortTool.Utils;

namespace SerialPortTool.Services
{
    /// <summary>
    /// 串口服务实现
    /// </summary>
    public class SerialPortService : ISerialPortService, IDisposable
    {
        private SerialPort _serialPort;
        private ReceiveMode _receiveMode = ReceiveMode.Response;
        private readonly object _lockObject = new object();
        private volatile bool _disposed = false;

        public event EventHandler<SerialDataReceivedArgs> DataReceived;
        public event EventHandler<SerialPortStatusChangedArgs> StatusChanged;

        public bool IsConnected => _serialPort?.IsOpen == true;

        public SerialPortConfig CurrentConfig { get; private set; }

        public SerialPortService()
        {
            _serialPort = new SerialPort();
            CurrentConfig = new SerialPortConfig();
        }

        public string[] GetAvailablePorts()
        {
            try
            {
                return SerialPort.GetPortNames();
            }
            catch (Exception ex)
            {
                OnStatusChanged(false, $"获取串口列表失败: {ex.Message}", ex);
                return new string[0];
            }
        }

        public OperationResult OpenPort(SerialPortConfig config)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_disposed)
                    {
                        return OperationResult.Failed("服务已被释放");
                    }

                    if (IsConnected)
                    {
                        ClosePort();
                    }

                    // 配置串口参数
                    _serialPort.PortName = config.PortName;
                    _serialPort.BaudRate = config.BaudRate;
                    _serialPort.DataBits = config.DataBits;
                    _serialPort.StopBits = config.StopBits;
                    _serialPort.Parity = config.Parity;
                    _serialPort.ReadTimeout = config.ReadTimeout;
                    _serialPort.WriteTimeout = config.WriteTimeout;

                    // 设置事件处理器
                    if (_receiveMode == ReceiveMode.Response)
                    {
                        _serialPort.DataReceived += OnSerialPortDataReceived;
                    }

                    // 打开串口
                    _serialPort.Open();
                    CurrentConfig = config;

                    OnStatusChanged(true, "串口打开成功");
                    return OperationResult.Successful("串口打开成功");
                }
            }
            catch (Exception ex)
            {
                var message = $"串口打开失败: {ex.Message}";
                OnStatusChanged(false, message, ex);
                return OperationResult.Failed(message, ex);
            }
        }

        public OperationResult ClosePort()
        {
            try
            {
                lock (_lockObject)
                {
                    if (_serialPort?.IsOpen == true)
                    {
                        _serialPort.DataReceived -= OnSerialPortDataReceived;
                        _serialPort.Close();
                    }

                    OnStatusChanged(false, "串口关闭成功");
                    return OperationResult.Successful("串口关闭成功");
                }
            }
            catch (Exception ex)
            {
                var message = $"串口关闭失败: {ex.Message}";
                OnStatusChanged(false, message, ex);
                return OperationResult.Failed(message, ex);
            }
        }

        public OperationResult SendData(byte[] data)
        {
            try
            {
                if (!IsConnected)
                {
                    return OperationResult.Failed("串口未连接");
                }

                if (data == null || data.Length == 0)
                {
                    return OperationResult.Failed("发送数据为空");
                }

                lock (_lockObject)
                {
                    _serialPort.Write(data, 0, data.Length);
                }

                return OperationResult.Successful($"成功发送 {data.Length} 字节数据");
            }
            catch (Exception ex)
            {
                var message = $"数据发送失败: {ex.Message}";
                return OperationResult.Failed(message, ex);
            }
        }

        public OperationResult SendText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OperationResult.Failed("发送文本为空");
            }

            try
            {
                var data = DataConverter.TextToBytes(text, Encoding.Default);
                return SendData(data);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"文本转换失败: {ex.Message}", ex);
            }
        }

        public OperationResult SendHexData(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return OperationResult.Failed("十六进制字符串为空");
            }

            try
            {
                var data = DataConverter.HexStringToBytes(hexString);
                return SendData(data);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"十六进制数据转换失败: {ex.Message}", ex);
            }
        }

        public ReadDataResult ReadData()
        {
            try
            {
                if (!IsConnected)
                {
                    return new ReadDataResult
                    {
                        Success = false,
                        Message = "串口未连接"
                    };
                }

                lock (_lockObject)
                {
                    int count = _serialPort.BytesToRead;
                    if (count == 0)
                    {
                        return new ReadDataResult
                        {
                            Success = true,
                            Message = "无数据可读",
                            Data = new byte[0]
                        };
                    }

                    byte[] buffer = new byte[count];
                    int bytesRead = _serialPort.Read(buffer, 0, count);

                    // 如果实际读取的字节数少于期望值，调整数组大小
                    if (bytesRead < count)
                    {
                        Array.Resize(ref buffer, bytesRead);
                    }

                    return new ReadDataResult
                    {
                        Success = true,
                        Message = $"成功读取 {bytesRead} 字节数据",
                        Data = buffer,
                        TextData = DataConverter.BytesToText(buffer, Encoding.Default),
                        HexData = DataConverter.BytesToHexString(buffer)
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReadDataResult
                {
                    Success = false,
                    Message = $"数据读取失败: {ex.Message}",
                    Exception = ex
                };
            }
        }

        public void SetReceiveMode(ReceiveMode mode)
        {
            lock (_lockObject)
            {
                if (_receiveMode == mode) return;

                // 移除旧的事件处理器
                if (_serialPort != null)
                {
                    _serialPort.DataReceived -= OnSerialPortDataReceived;
                }

                _receiveMode = mode;

                // 如果是响应模式且串口已打开，添加事件处理器
                if (_receiveMode == ReceiveMode.Response && IsConnected)
                {
                    _serialPort.DataReceived += OnSerialPortDataReceived;
                }
            }
        }

        private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var result = ReadData();
                if (result.Success && result.Data != null && result.Data.Length > 0)
                {
                    OnDataReceived(new SerialDataReceivedArgs
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
                OnStatusChanged(IsConnected, $"数据接收处理失败: {ex.Message}", ex);
            }
        }

        private void OnDataReceived(SerialDataReceivedArgs args)
        {
            DataReceived?.Invoke(this, args);
        }

        private void OnStatusChanged(bool isConnected, string message, Exception exception = null)
        {
            StatusChanged?.Invoke(this, new SerialPortStatusChangedArgs
            {
                IsConnected = isConnected,
                Message = message,
                Exception = exception,
                Timestamp = DateTime.Now
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                lock (_lockObject)
                {
                    if (_serialPort?.IsOpen == true)
                    {
                        try
                        {
                            _serialPort.DataReceived -= OnSerialPortDataReceived;
                            _serialPort.Close();
                        }
                        catch
                        {
                            // 忽略关闭时的异常
                        }
                    }

                    _serialPort?.Dispose();
                    _serialPort = null;
                    _disposed = true;
                }
            }
        }

        ~SerialPortService()
        {
            Dispose(false);
        }
    }
}
