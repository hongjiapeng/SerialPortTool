using System;
using System.IO.Ports;

namespace SerialPortTool.Core.Services
{
    /// <summary>
    /// Serial port service interface providing comprehensive communication capabilities
    /// </summary>
    public interface ISerialPortService : IDisposable
    {
        /// <summary>
        /// Event fired when data is received from the serial port
        /// </summary>
        event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Event fired when the serial port connection status changes
        /// </summary>
        event EventHandler<SerialPortStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Gets a value indicating whether the serial port is currently connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the current serial port configuration
        /// </summary>
        SerialPortConfiguration CurrentConfiguration { get; }

        /// <summary>
        /// Gets an array of available serial port names on the system
        /// </summary>
        /// <returns>Array of available port names</returns>
        string[] GetAvailablePorts();

        /// <summary>
        /// Opens the serial port with the specified configuration
        /// </summary>
        /// <param name="configuration">Serial port configuration parameters</param>
        /// <returns>Operation result indicating success or failure</returns>
        OperationResult OpenPort(SerialPortConfiguration configuration);

        /// <summary>
        /// Closes the currently open serial port
        /// </summary>
        /// <returns>Operation result indicating success or failure</returns>
        OperationResult ClosePort();

        /// <summary>
        /// Sends raw byte data through the serial port
        /// </summary>
        /// <param name="data">Byte array to send</param>
        /// <returns>Operation result indicating success or failure</returns>
        OperationResult SendData(byte[] data);

        /// <summary>
        /// Sends text data through the serial port using default encoding
        /// </summary>
        /// <param name="text">Text string to send</param>
        /// <returns>Operation result indicating success or failure</returns>
        OperationResult SendText(string text);

        /// <summary>
        /// Sends hexadecimal formatted data through the serial port
        /// </summary>
        /// <param name="hexString">Hexadecimal string (e.g., "01 02 03 FF")</param>
        /// <returns>Operation result indicating success or failure</returns>
        OperationResult SendHexData(string hexString);

        /// <summary>
        /// Manually reads available data from the serial port buffer (for acknowledgment mode)
        /// </summary>
        /// <returns>Read operation result with received data</returns>
        ReadDataResult ReadData();

        /// <summary>
        /// Sets the data reception mode (automatic or manual)
        /// </summary>
        /// <param name="mode">Reception mode to use</param>
        void SetReceiveMode(ReceiveMode mode);
    }

    /// <summary>
    /// Serial port configuration parameters
    /// </summary>
    public class SerialPortConfiguration
    {
        /// <summary>
        /// Gets or sets the port name (e.g., "COM1", "/dev/ttyUSB0")
        /// </summary>
        public string PortName { get; set; } = "COM1";

        /// <summary>
        /// Gets or sets the baud rate for communication
        /// </summary>
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// Gets or sets the number of data bits per byte
        /// </summary>
        public int DataBits { get; set; } = 8;

        /// <summary>
        /// Gets or sets the stop bits configuration
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// Gets or sets the parity checking protocol
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;

        /// <summary>
        /// Gets or sets the read timeout in milliseconds
        /// </summary>
        public int ReadTimeout { get; set; } = 500;

        /// <summary>
        /// Gets or sets the write timeout in milliseconds
        /// </summary>
        public int WriteTimeout { get; set; } = 500;
    }

    /// <summary>
    /// Data reception mode options
    /// </summary>
    public enum ReceiveMode
    {
        /// <summary>
        /// Automatic response mode - data is received and processed automatically
        /// </summary>
        Response,

        /// <summary>
        /// Manual acknowledgment mode - data must be manually read from buffer
        /// </summary>
        Acknowledgment
    }

    /// <summary>
    /// Base class for operation results
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the result message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception that occurred during the operation, if any
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Creates a successful operation result
        /// </summary>
        /// <param name="message">Success message</param>
        /// <returns>Successful operation result</returns>
        public static OperationResult Successful(string message = "Operation completed successfully")
        {
            return new OperationResult { Success = true, Message = message };
        }

        /// <summary>
        /// Creates a failed operation result
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Optional exception details</param>
        /// <returns>Failed operation result</returns>
        public static OperationResult Failed(string message, Exception exception = null)
        {
            return new OperationResult { Success = false, Message = message, Exception = exception };
        }
    }

    /// <summary>
    /// Result class for read operations containing received data
    /// </summary>
    public class ReadDataResult : OperationResult
    {
        /// <summary>
        /// Gets or sets the raw byte data received
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the received data as text string
        /// </summary>
        public string TextData { get; set; }

        /// <summary>
        /// Gets or sets the received data as hexadecimal string
        /// </summary>
        public string HexData { get; set; }
    }

    /// <summary>
    /// Event arguments for serial data reception events
    /// </summary>
    public class SerialDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the raw byte data received
        /// </summary>
        public byte[] Data { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Gets or sets the received data as text string
        /// </summary>
        public string TextData { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the received data as hexadecimal string
        /// </summary>
        public string HexData { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the data was received
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Event arguments for serial port status change events
    /// </summary>
    public class SerialPortStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the port is connected
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Gets or sets the status change message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception that caused the status change, if any
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the status changed
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
