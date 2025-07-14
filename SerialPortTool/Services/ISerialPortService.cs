using System;
using System.IO.Ports;

namespace SerialPortTool.Services
{
    /// <summary>
    /// 串口服务接口
    /// </summary>
    public interface ISerialPortService : IDisposable
    {
        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        event EventHandler<SerialDataReceivedArgs> DataReceived;

        /// <summary>
        /// 串口状态变化事件
        /// </summary>
        event EventHandler<SerialPortStatusChangedArgs> StatusChanged;

        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 当前串口配置
        /// </summary>
        SerialPortConfig CurrentConfig { get; }

        /// <summary>
        /// 获取可用串口列表
        /// </summary>
        /// <returns>串口名称数组</returns>
        string[] GetAvailablePorts();

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="config">串口配置</param>
        /// <returns>操作结果</returns>
        OperationResult OpenPort(SerialPortConfig config);

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>操作结果</returns>
        OperationResult ClosePort();

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>操作结果</returns>
        OperationResult SendData(byte[] data);

        /// <summary>
        /// 发送文本数据
        /// </summary>
        /// <param name="text">要发送的文本</param>
        /// <returns>操作结果</returns>
        OperationResult SendText(string text);

        /// <summary>
        /// 发送十六进制数据
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>操作结果</returns>
        OperationResult SendHexData(string hexString);

        /// <summary>
        /// 手动读取数据（应答模式）
        /// </summary>
        /// <returns>读取结果</returns>
        ReadDataResult ReadData();

        /// <summary>
        /// 设置接收模式
        /// </summary>
        /// <param name="mode">接收模式</param>
        void SetReceiveMode(ReceiveMode mode);
    }

    /// <summary>
    /// 串口配置
    /// </summary>
    public class SerialPortConfig
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Parity Parity { get; set; } = Parity.None;
        public int ReadTimeout { get; set; } = 500;
        public int WriteTimeout { get; set; } = 500;
    }

    /// <summary>
    /// 接收模式
    /// </summary>
    public enum ReceiveMode
    {
        /// <summary>
        /// 响应模式（自动接收）
        /// </summary>
        Response,
        /// <summary>
        /// 应答模式（手动接收）
        /// </summary>
        Acknowledgment
    }

    /// <summary>
    /// 操作结果
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Exception Exception { get; set; }

        public static OperationResult Successful(string message = "操作成功")
        {
            return new OperationResult { Success = true, Message = message };
        }

        public static OperationResult Failed(string message, Exception exception = null)
        {
            return new OperationResult { Success = false, Message = message, Exception = exception };
        }
    }

    /// <summary>
    /// 读取数据结果
    /// </summary>
    public class ReadDataResult : OperationResult
    {
        public byte[] Data { get; set; }
        public string TextData { get; set; }
        public string HexData { get; set; }
    }

    /// <summary>
    /// 串口数据接收参数
    /// </summary>
    public class SerialDataReceivedArgs : EventArgs
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public string TextData { get; set; } = string.Empty;
        public string HexData { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 串口状态变化参数
    /// </summary>
    public class SerialPortStatusChangedArgs : EventArgs
    {
        public bool IsConnected { get; set; }
        public string Message { get; set; } = string.Empty;
        public Exception Exception { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
