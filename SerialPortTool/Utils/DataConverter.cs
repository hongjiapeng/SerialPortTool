using System;
using System.Linq;
using System.Text;

namespace SerialPortTool.Utils
{
    /// <summary>
    /// 数据转换工具类
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// 将十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="hexString">十六进制字符串，可以包含空格</param>
        /// <returns>字节数组</returns>
        /// <exception cref="ArgumentException">当输入格式无效时抛出</exception>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return Array.Empty<byte>();
            }

            try
            {
                // 移除空格并转换为大写
                hexString = hexString.Replace(" ", "").ToUpper();
                
                // 确保字符串长度为偶数
                if (hexString.Length % 2 != 0)
                {
                    throw new ArgumentException("十六进制字符串长度必须为偶数");
                }

                // 验证字符串只包含有效的十六进制字符
                if (!hexString.All(c => "0123456789ABCDEF".Contains(c)))
                {
                    throw new ArgumentException("字符串包含无效的十六进制字符");
                }

                byte[] bytes = new byte[hexString.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    string hex = hexString.Substring(i * 2, 2);
                    bytes[i] = Convert.ToByte(hex, 16);
                }

                return bytes;
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new ArgumentException($"十六进制字符串转换失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="addSpaces">是否在字节之间添加空格</param>
        /// <returns>十六进制字符串</returns>
        public static string BytesToHexString(byte[] bytes, bool addSpaces = true)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i > 0 && addSpaces)
                {
                    result.Append(' ');
                }
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// 将文本转换为字节数组
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="encoding">编码方式，默认为UTF-8</param>
        /// <returns>字节数组</returns>
        public static byte[] TextToBytes(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Array.Empty<byte>();
            }

            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// 将字节数组转换为文本
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="encoding">编码方式，默认为UTF-8</param>
        /// <returns>文本</returns>
        public static string BytesToText(byte[] bytes, Encoding encoding = null)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 验证十六进制字符串格式
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>是否为有效格式</returns>
        public static bool IsValidHexString(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return false;
            }

            try
            {
                HexStringToBytes(hexString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 格式化十六进制字符串（添加空格分隔）
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatHexString(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return string.Empty;
            }

            try
            {
                var bytes = HexStringToBytes(hexString);
                return BytesToHexString(bytes, true);
            }
            catch
            {
                return hexString; // 如果转换失败，返回原字符串
            }
        }
    }
}
