using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;  //导入串口的命名空间

namespace _01_SerialPort
{
    public partial class Form1 : Form
    {
        public delegate void showReceiveDelegate(string text); //当采用响应模式，应申明一个委托，实现不同线程的控件实验

        SerialPort com = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);//初始化构造函数
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            cmbPort.SelectedIndex = 0;
            cmbBaudRate.SelectedIndex = 0;
            cmbDataBits.SelectedIndex = 0;
            cmbStopBits.SelectedIndex = 0;
            cmbParity.SelectedIndex = 0;
        }
        /// <summary>
        /// 串口打开与关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (btnOpen.Text == "打开串口")
            {
                try
                {
                    if (!com.IsOpen)//串口未打开   
                    {
                        com.PortName = cmbPort.Text;
                        com.BaudRate = int.Parse(cmbBaudRate.Text);
                        com.DataBits = int.Parse(cmbDataBits.Text);
                        switch (cmbStopBits.SelectedIndex)
                        {
                            case 0:
                                com.StopBits = StopBits.One; break;
                            case 1:
                                com.StopBits = StopBits.Two; break;
                            case 2:
                                com.StopBits = StopBits.OnePointFive; break;
                            case 3:
                                com.StopBits = StopBits.None; break;
                        }
                        switch (cmbParity.SelectedIndex)
                        {
                            case 0: com.Parity = Parity.None; break;
                            case 1: com.Parity = Parity.Odd; break;
                            case 2: com.Parity = Parity.Even; break;
                        }

                        com.Open();//打开串口
                    }
                    btnOpen.Text = "关闭串口";
                    txtStatus.Text = "串口已打开！";
                    btnSend.Enabled = true;
                    if (rbAck.Checked)
                        btnReceive.Enabled = true; //选中应答模式，接收按钮有效
                }
                catch
                { txtStatus.Text = "串口打开错误或串口不存在！"; }
            }
            else //关闭串口

                try
                {
                    if (com.IsOpen)
                        com.Close(); //关闭串口
                    btnOpen.Text = "打开串口";
                    txtStatus.Text = "串口已关闭！";
                    btnSend.Enabled = false;
                    if (rbAck.Checked)
                        btnReceive.Enabled = false; //应答模式，接收按钮无效
                }
                catch
                { txtStatus.Text = "串口关闭错误或串口不存在！"; }
        }

        /// <summary>
        /// 串口发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = null;
                if (chkSendHex.Checked)
                    data = GetBytesFromString(txtSend.Text);
                else
                    data = Encoding.Default.GetBytes(txtSend.Text);
                com.Write(data, 0, data.Length);//data 包含要写入端口的数据的字节数组。 

            }
            catch (Exception err)
            { txtStatus.Text = err.ToString(); }
        }

        /// <summary>
        /// 串口接收数据，应答模式时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                //应答模式
                int count = com.BytesToRead;
                byte[] readBuffer = new byte[count];
                com.Read(readBuffer, 0, count);
                if (chkRecHex.Checked)
                    txtReceive.Text = GetStringFromBytes(readBuffer);  //转十六进制
                else
                    txtReceive.Text = Encoding.Default.GetString(readBuffer);  //字母、数字、汉字转换为字符串 

            }
            catch (Exception err)
            { txtStatus.Text = err.ToString(); }
        }

        /// <summary>
        /// 数据接收模式变化时，设置串口的数据接收侦听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbResponse_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                btnReceive.Enabled = rbAck.Checked;
                if (rbResponse.Checked)
                    com.DataReceived += new SerialDataReceivedEventHandler(com_DataReceived);  //加载接收事件
                else
                    com.DataReceived -= new SerialDataReceivedEventHandler(com_DataReceived);  //移除接收事件
            }
            catch (Exception err)
            { txtStatus.Text = err.ToString(); }

        }

        /// <summary>
        /// 响应模式时，串口接收数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void com_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int count = com.BytesToRead;
                byte[] readBuffer = new byte[count];
                com.Read(readBuffer, 0, count);
                string strReceive = "";
                if (chkRecHex.Checked)
                    strReceive = GetStringFromBytes(readBuffer);  //转十六进制
                else
                    strReceive = Encoding.Default.GetString(readBuffer);  //字母、数字、汉字转换为字符串          
                this.Invoke(new showReceiveDelegate(doShowReceive), strReceive);

            }
            catch (Exception err)
            { txtStatus.Text = err.ToString(); }

        }

        /// <summary>
        /// //异步线程处理接受的字符，显示在接收的文本框中
        /// </summary>
        /// <param name="str"></param>
        public void doShowReceive(string str)
        {
            txtReceive.Text += str;
        }

        /// <summary>
        /// 以十六进制数据发送转换时，显示转换对应数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSendHex_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSendHex.Checked)
                    txtSend.Text = GetStringFromBytes(Encoding.Default.GetBytes(txtSend.Text));
                else
                    txtSend.Text = Encoding.Default.GetString(GetBytesFromString(txtSend.Text));
            }
            catch
            { txtStatus.Text = "数据转换出错，请输入正确的数据格式"; }
        }
        /// <summary>
        /// 以十六进制数据显示接收数据时，显示对应数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRecHex_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkRecHex.Checked)
                    txtReceive.Text = GetStringFromBytes(Encoding.Default.GetBytes(txtReceive.Text));
                else
                    txtReceive.Text = Encoding.Default.GetString(GetBytesFromString(txtReceive.Text));
            }
            catch
            { txtStatus.Text = "数据转换出错，请输入正确的数据格式"; }
        }

        /// <summary>
        /// 把十六进制格式的字符串转换成字节数组。
        /// </summary>
        /// <param name="pString">要转换的十六进制格式的字符串</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetBytesFromString(string pString)
        {
            string[] str = pString.Split(' ');     //把十六进制格式的字符串按空格转换为字符串数组。
            //pString.Trim();
            byte[] bytes = new byte[str.Length];     //定义字节数组并初始化，长度为字符串数组的长度。
            for (int i = 0; i < str.Length; i++)     //遍历字符串数组，把每个字符串转换成字节类型赋值给每个字节变量。
                bytes[i] = Convert.ToByte(Convert.ToInt32(str[i], 16));//ToInt32( , ) 第一个参数字符串和字符串数组都一样吗？？？？
            return bytes;     //返回字节数组。
        }

        /// <summary>
        /// 把字节数组转换为十六进制格式的字符串。
        /// </summary>
        /// <param name="pByte">要转换的字节数组。</param>
        /// <returns>返回十六进制格式的字符串。</returns>
        public static string GetStringFromBytes(byte[] pByte)
        {
            string str = "";     //定义字符串类型临时变量。
            //遍历字节数组，把每个字节转换成十六进制字符串，不足两位前面添“0”，以空格分隔累加到字符串变量里。
            for (int i = 0; i < pByte.Length; i++)
                str += (pByte[i].ToString("X").PadLeft(2, '0') + " ");// PadLeft 位数不足时，给予补足 
            str = str.TrimEnd(' ');     //去掉字符串末尾的空格。
            return str;     //返回字符串临时变量。
        }
    }
}
