using System;
using System.Globalization;
using System.Text;

namespace SerialPortTool.Core.Utils
{
    /// <summary>
    /// Utility class for data conversion between different formats
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Converts a hexadecimal string to byte array
        /// </summary>
        /// <param name="hexString">Hexadecimal string (e.g., "01 02 03" or "010203")</param>
        /// <returns>Byte array representation</returns>
        /// <exception cref="ArgumentException">Thrown when hex string format is invalid</exception>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                return Array.Empty<byte>();

            // Remove spaces and convert to uppercase
            hexString = hexString.Replace(" ", "").Replace("-", "").ToUpperInvariant();

            // Ensure even length
            if (hexString.Length % 2 != 0)
                throw new ArgumentException("Hex string must have even length", nameof(hexString));

            var bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var hexByte = hexString.Substring(i * 2, 2);
                if (!byte.TryParse(hexByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out bytes[i]))
                {
                    throw new ArgumentException($"Invalid hex character at position {i * 2}: '{hexByte}'", nameof(hexString));
                }
            }

            return bytes;
        }

        /// <summary>
        /// Converts byte array to hexadecimal string representation
        /// </summary>
        /// <param name="bytes">Byte array to convert</param>
        /// <param name="separator">Separator between hex bytes (default: space)</param>
        /// <returns>Hexadecimal string representation</returns>
        public static string BytesToHexString(byte[] bytes, string separator = " ")
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            var result = new StringBuilder(bytes.Length * (2 + separator.Length));
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i > 0 && !string.IsNullOrEmpty(separator))
                    result.Append(separator);

                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts byte array to text string using UTF-8 encoding
        /// </summary>
        /// <param name="bytes">Byte array to convert</param>
        /// <param name="encoding">Text encoding to use (default: UTF-8)</param>
        /// <returns>Text string representation</returns>
        public static string BytesToText(byte[] bytes, Encoding encoding = null)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// Converts text string to byte array using UTF-8 encoding
        /// </summary>
        /// <param name="text">Text string to convert</param>
        /// <param name="encoding">Text encoding to use (default: UTF-8)</param>
        /// <returns>Byte array representation</returns>
        public static byte[] TextToBytes(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<byte>();

            encoding ??= Encoding.UTF8;
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Validates if a string contains valid hexadecimal characters
        /// </summary>
        /// <param name="hexString">String to validate</param>
        /// <returns>True if valid hex string, false otherwise</returns>
        public static bool IsValidHexString(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                return false;

            // Remove spaces and dashes
            hexString = hexString.Replace(" ", "").Replace("-", "");

            // Must have even length
            if (hexString.Length % 2 != 0)
                return false;

            // Check each character
            foreach (char c in hexString)
            {
                if (!IsHexDigit(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a character is a valid hexadecimal digit
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>True if valid hex digit, false otherwise</returns>
        private static bool IsHexDigit(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }

        /// <summary>
        /// Formats byte array as a readable string with both hex and ASCII representation
        /// </summary>
        /// <param name="bytes">Byte array to format</param>
        /// <param name="bytesPerLine">Number of bytes per line (default: 16)</param>
        /// <returns>Formatted string representation</returns>
        public static string FormatBytesAsHexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            var result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += bytesPerLine)
            {
                // Address
                result.AppendFormat("{0:X8}: ", i);

                // Hex values
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < bytes.Length)
                        result.AppendFormat("{0:X2} ", bytes[i + j]);
                    else
                        result.Append("   ");
                }

                result.Append(" ");

                // ASCII representation
                for (int j = 0; j < bytesPerLine && i + j < bytes.Length; j++)
                {
                    byte b = bytes[i + j];
                    char c = (b >= 32 && b <= 126) ? (char)b : '.';
                    result.Append(c);
                }

                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
