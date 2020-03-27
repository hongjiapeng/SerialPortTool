using System;
using System.IO.Ports;
using System.Text;
using System.Windows;

namespace SerialPortTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///当采用响应模式 应声明一个委托 实现不同线程的控件实验
        /// </summary>
        /// <param name="text"></param>
        public delegate void showReceiveDelegate(string text);

        private SerialPort com = new SerialPort();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var portNames = SerialPort.GetPortNames();

            cmbReceivePort.ItemsSource = portNames;

            if (portNames.Length > 0)
            {
                cmbReceivePort.SelectedIndex = 0;
            }

            //接受模式初始化（默认为相应模式）
            rbResponse.IsChecked = true;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenPort_Click(object sender, RoutedEventArgs e)
        {
            if (btnOpenPort.Content.ToString() == "打开串口")
            {
                try
                {
                    //如果串口没开
                    if (!com.IsOpen)
                    {
                        //设置串口参数**********************************开始
                        com.PortName = cmbReceivePort.Text;
                        com.BaudRate = int.Parse(cmbBaudRate.Text);
                        com.DataBits = int.Parse(cmbDataBits.Text);
                        switch (cmbStopBits.SelectedIndex)
                        {
                            case 0:
                                com.StopBits = StopBits.One;
                                break;
                            case 1:
                                com.StopBits = StopBits.Two;
                                break;
                            case 2:
                                com.StopBits = StopBits.OnePointFive;
                                break;
                            case 3:
                                com.StopBits = StopBits.None;
                                break;
                        }
                        switch (cmbParity.SelectedIndex)
                        {
                            case 0:
                                com.Parity = Parity.None;
                                break;
                            case 1:
                                com.Parity = Parity.Odd;
                                break;
                            case 2:
                                com.Parity = Parity.Even;
                                break;
                        }
                        //设置串口参数**********************************结束
                        com.Open();
                    }
                    btnOpenPort.Content = "关闭串口";
                    txtStatus.Text = "串口已打开！";
                    //启动发送按钮
                    btnSend.IsEnabled = true;
                    if ((bool)rbAck.IsChecked)
                        btnReceive.IsEnabled = true;//应答模式，接受按钮有效                    
                }
                catch
                {
                    txtStatus.Text = "串口打开错误或串口不存在！";
                }
            }
            else//串口已经打开
            {
                try
                {
                    if (com.IsOpen)
                        com.Close(); //关闭串口
                    btnOpenPort.Content = "打开串口";
                    txtStatus.Text = "串口已关闭！";
                    btnSend.IsEnabled = false;
                    if ((bool)rbAck.IsChecked)
                        btnReceive.IsEnabled = false;
                }
                catch
                {
                    txtStatus.Text = "串口关闭错误或串口不存在！";
                }
            }
        }

        /// <summary>
        /// 串口发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //发送的数据
                byte[] data = null;
                //是否16进制发送
                if ((bool)chkSendHex.IsChecked)
                {
                    data = GetByteFromString(txtSend.Text);//将数据转换成16进制字符串
                }
                else
                {
                    data = Encoding.Default.GetBytes(txtSend.Text);//将数据转换成字节数组
                }
                //向串口写入数据
                com.Write(data, 0, data.Length);
            }
            catch (Exception err)
            {
                txtStatus.Text = err.ToString();
            }
        }

        /// <summary>
        /// 把十六进制格式的字符串转换成字节数组。
        /// </summary>
        /// <param name="pString">要转换的十六进制格式的字符串</param>
        /// <returns>返回字节数组</returns>
        private byte[] GetByteFromString(string pString)
        {
            string[] str = pString.Split(' ');//把十六进制格式的字符串按空格转换为字符串数组
            byte[] bytes = new byte[str.Length];//定义字节数组并初始化，长度为字符串数组的长度
            for (int i = 0; i < str.Length; i++)//遍历字符串数组，把每个字符串换成字节类型赋值给每个字节变量
            {
                bytes[i] = Convert.ToByte(Convert.ToInt32(str[i],16));
            }
            return bytes;//返回字节数组
        }

        /// <summary>
        /// 串口接受数据，应答模式时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //应答模式
                //获取串口缓冲区字节数
                int count = com.BytesToRead;
                //实例化接受串口数据的数组
                byte[] readBuffer = new byte[count];
                //从串口缓冲区读取数据
                com.Read(readBuffer, 0, count);

                if ((bool)chkReceiveHex.IsChecked)
                {
                    txtReceive.Text = GetStringFromBytes(readBuffer);//转16进制
                }
                else
                {
                    txtReceive.Text = Encoding.Default.GetString(readBuffer);//字母、数字、汉字 转换成字符串
                }
            }
            catch (Exception err)
            {

                txtStatus.Text = err.ToString();
            }
        }

        /// <summary>
        /// 将字节数组转换为十六进制格式的字符串
        /// </summary>
        /// <param name="readBuffer">要转换的字节字节数组</param>
        /// <returns>返回十六进制格式的字符串</returns>
        private string GetStringFromBytes(byte[] readBuffer)
        {
            string str = "";
            //遍历字节数组，把每个字节转换成十六进制字符串，不足两位前面添“0”，以空格分隔累加到字符串变量里。
            for (int i = 0; i < readBuffer.Length; i++)
            {
                str += (readBuffer[i].ToString("X").PadLeft(2, '0') + "");
            }
            str = str.TrimEnd(' ');//去掉字符串末尾的空格
            return str;
        }

        /// <summary>
        /// 响应模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbResponse_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置接受按钮的启用属性
                btnReceive.IsEnabled = (bool)rbAck.IsChecked;
                if ((bool)rbResponse.IsChecked)
                {
                    com.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived);//加载接受事件
                }
            }
            catch (Exception err)
            {
                txtStatus.Text = err.ToString();
            }
        }

        /// <summary>
        /// 响应模式时 串口接受数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                bool recHex = false;
                Dispatcher.Invoke(new Action(() =>
                {
                    //获取界面控件复选框是否以16进制显示
                    recHex = (bool)chkReceiveHex.IsChecked;
                }));
                //获取串口-缓冲区字节数
                int count = com.BytesToRead;
                //实例化接受串口数据的数组
                byte[] readBuffer = new byte[count];
                //从缓冲区读取数据
                com.Read(readBuffer, 0, count);

                string strReceive = "";
                if (recHex)
                {
                    strReceive = GetStringFromBytes(readBuffer);//转16进制
                }
                else
                {
                    strReceive = Encoding.Default.GetString(readBuffer);//字母、数字、汉字 转换成字符串
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    txtStatus.Text = strReceive;
                }));
            }
            catch (Exception err)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    txtStatus.Text = err.ToString();
                }));
                //throw;
            }
        }

        /// <summary>
        /// 以16进制数据显示接受数据时 显示对应数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkSendHex_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)chkReceiveHex.IsChecked)
                {
                    txtSend.Text = GetStringFromBytes(Encoding.Default.GetBytes(txtSend.Text));
                }
                else
                {
                    txtSend.Text = Encoding.Default.GetString(GetByteFromString(txtSend.Text));
                }
            }
            catch
            {
                txtStatus.Text = "数据转换出错，请输入正确的数据格式";
            }
        }

        private void ChkReceiveHex_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)chkReceiveHex.IsChecked)
                {
                    txtReceive.Text = GetStringFromBytes(Encoding.Default.GetBytes(txtReceive.Text));
                }
                else
                {
                    txtReceive.Text = Encoding.Default.GetString(GetByteFromString(txtReceive.Text));
                }
            }
            catch
            {
                txtStatus.Text = "数据转换出错，请输入正确的数据格式";
            }
        }

        /// <summary>
        /// 应答模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbAck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btnReceive.IsEnabled = (bool)rbAck.IsChecked;
                if ((bool)rbAck.IsChecked)
                {
                    com.DataReceived -= new SerialDataReceivedEventHandler(Com_DataReceived);//移除接受事件
                }
            }
            catch (Exception err)
            {
                txtStatus.Text = err.ToString();                
            }
        }
    }
}
