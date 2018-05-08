namespace _01_SerialPort
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labDataBits = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDataBits = new System.Windows.Forms.ComboBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.rbAck = new System.Windows.Forms.RadioButton();
            this.rbResponse = new System.Windows.Forms.RadioButton();
            this.btnReceive = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRecHex = new System.Windows.Forms.CheckBox();
            this.chkSendHex = new System.Windows.Forms.CheckBox();
            this.labReceive = new System.Windows.Forms.Label();
            this.labSend = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(274, 14);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(50, 26);
            this.btnSend.TabIndex = 17;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(148, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "停止位：";
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Location = new System.Drawing.Point(12, 165);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(53, 12);
            this.labStatus.TabIndex = 19;
            this.labStatus.Text = "执行状态";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "奇偶校验：";
            // 
            // labDataBits
            // 
            this.labDataBits.AutoSize = true;
            this.labDataBits.Location = new System.Drawing.Point(27, 58);
            this.labDataBits.Name = "labDataBits";
            this.labDataBits.Size = new System.Drawing.Size(53, 12);
            this.labDataBits.TabIndex = 11;
            this.labDataBits.Text = "数据位：";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(183, 87);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(82, 20);
            this.btnOpen.TabIndex = 18;
            this.btnOpen.Text = "打开串口";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(148, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "波特率：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbParity);
            this.groupBox1.Controls.Add(this.cmbBaudRate);
            this.groupBox1.Controls.Add(this.cmbStopBits);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbPort);
            this.groupBox1.Controls.Add(this.btnOpen);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.labDataBits);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbDataBits);
            this.groupBox1.Location = new System.Drawing.Point(14, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 134);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口设置";
            // 
            // cmbParity
            // 
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.cmbParity.Location = new System.Drawing.Point(79, 87);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(64, 20);
            this.cmbParity.TabIndex = 6;
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbBaudRate.Location = new System.Drawing.Point(197, 19);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(68, 20);
            this.cmbBaudRate.TabIndex = 2;
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.FormattingEnabled = true;
            this.cmbStopBits.Items.AddRange(new object[] {
            "One",
            "Two",
            "OnePointFive",
            "None"});
            this.cmbStopBits.Location = new System.Drawing.Point(197, 52);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.Size = new System.Drawing.Size(68, 20);
            this.cmbStopBits.TabIndex = 7;
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3"});
            this.cmbPort.Location = new System.Drawing.Point(79, 20);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(63, 20);
            this.cmbPort.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "接收端口：";
            // 
            // cmbDataBits
            // 
            this.cmbDataBits.FormattingEnabled = true;
            this.cmbDataBits.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.cmbDataBits.Location = new System.Drawing.Point(79, 50);
            this.cmbDataBits.Name = "cmbDataBits";
            this.cmbDataBits.Size = new System.Drawing.Size(63, 20);
            this.cmbDataBits.TabIndex = 3;
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(8, 43);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(316, 21);
            this.txtSend.TabIndex = 1;
            // 
            // txtReceive
            // 
            this.txtReceive.Location = new System.Drawing.Point(8, 105);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceive.Size = new System.Drawing.Size(316, 76);
            this.txtReceive.TabIndex = 2;
            // 
            // txtStatus
            // 
            this.txtStatus.Enabled = false;
            this.txtStatus.Location = new System.Drawing.Point(71, 165);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(228, 41);
            this.txtStatus.TabIndex = 23;
            // 
            // rbAck
            // 
            this.rbAck.AutoSize = true;
            this.rbAck.Checked = true;
            this.rbAck.Location = new System.Drawing.Point(134, 80);
            this.rbAck.Name = "rbAck";
            this.rbAck.Size = new System.Drawing.Size(71, 16);
            this.rbAck.TabIndex = 5;
            this.rbAck.TabStop = true;
            this.rbAck.Text = "应答模式";
            this.rbAck.UseVisualStyleBackColor = true;
            // 
            // rbResponse
            // 
            this.rbResponse.AutoSize = true;
            this.rbResponse.Location = new System.Drawing.Point(66, 80);
            this.rbResponse.Name = "rbResponse";
            this.rbResponse.Size = new System.Drawing.Size(71, 16);
            this.rbResponse.TabIndex = 6;
            this.rbResponse.Text = "响应模式";
            this.rbResponse.UseVisualStyleBackColor = true;
            this.rbResponse.CheckedChanged += new System.EventHandler(this.rbResponse_CheckedChanged);
            // 
            // btnReceive
            // 
            this.btnReceive.Enabled = false;
            this.btnReceive.Location = new System.Drawing.Point(274, 77);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(48, 22);
            this.btnReceive.TabIndex = 22;
            this.btnReceive.Text = "接收";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnReceive);
            this.groupBox2.Controls.Add(this.rbAck);
            this.groupBox2.Controls.Add(this.rbResponse);
            this.groupBox2.Controls.Add(this.chkRecHex);
            this.groupBox2.Controls.Add(this.chkSendHex);
            this.groupBox2.Controls.Add(this.labReceive);
            this.groupBox2.Controls.Add(this.txtSend);
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Controls.Add(this.txtReceive);
            this.groupBox2.Controls.Add(this.labSend);
            this.groupBox2.Location = new System.Drawing.Point(325, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(341, 196);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "通讯测试";
            // 
            // chkRecHex
            // 
            this.chkRecHex.AutoSize = true;
            this.chkRecHex.Location = new System.Drawing.Point(211, 80);
            this.chkRecHex.Name = "chkRecHex";
            this.chkRecHex.Size = new System.Drawing.Size(66, 16);
            this.chkRecHex.TabIndex = 23;
            this.chkRecHex.Tag = "";
            this.chkRecHex.Text = "HEX显示";
            this.chkRecHex.UseVisualStyleBackColor = true;
            this.chkRecHex.CheckedChanged += new System.EventHandler(this.chkRecHex_CheckedChanged);
            // 
            // chkSendHex
            // 
            this.chkSendHex.AutoSize = true;
            this.chkSendHex.Location = new System.Drawing.Point(113, 19);
            this.chkSendHex.Name = "chkSendHex";
            this.chkSendHex.Size = new System.Drawing.Size(66, 16);
            this.chkSendHex.TabIndex = 19;
            this.chkSendHex.Tag = "";
            this.chkSendHex.Text = "HEX发送";
            this.chkSendHex.UseVisualStyleBackColor = true;
            this.chkSendHex.CheckedChanged += new System.EventHandler(this.chkSendHex_CheckedChanged);
            // 
            // labReceive
            // 
            this.labReceive.AutoSize = true;
            this.labReceive.Location = new System.Drawing.Point(6, 82);
            this.labReceive.Name = "labReceive";
            this.labReceive.Size = new System.Drawing.Size(65, 12);
            this.labReceive.TabIndex = 4;
            this.labReceive.Text = "接收模式：";
            // 
            // labSend
            // 
            this.labSend.AutoSize = true;
            this.labSend.Location = new System.Drawing.Point(6, 23);
            this.labSend.Name = "labSend";
            this.labSend.Size = new System.Drawing.Size(101, 12);
            this.labSend.TabIndex = 3;
            this.labSend.Text = "发送数据输入框：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 227);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtStatus);
            this.Name = "Form1";
            this.Text = "串口测试应用程序";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labDataBits;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStopBits;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.ComboBox cmbDataBits;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.RadioButton rbResponse;
        private System.Windows.Forms.RadioButton rbAck;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labReceive;
        private System.Windows.Forms.CheckBox chkSendHex;
        private System.Windows.Forms.Label labSend;
        private System.Windows.Forms.CheckBox chkRecHex;
    }
}

