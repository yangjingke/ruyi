using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;
using forkliftDataStruct;
using ruyi;
namespace ruyi
{
    public partial class standalone : Form
    {
        private ConcurrentQueue<string> acceptdata = new ConcurrentQueue<string>(); //接收数据（字符串）的队列
        private ConcurrentQueue<dataStructure.device> onlineDeviceDataQueue = new ConcurrentQueue<dataStructure.device>(); //在线设备状态更新（添加）队列
        private ConcurrentDictionary<string, dataStructure.device> onlineDeviceNowTimeData = new ConcurrentDictionary<string, dataStructure.device>();//在线设备列表
        private ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData> faultDevice = new ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData>();//故障报警列表
        public delegate void refresh(dataStructure.device device);
        string buffer = "";
        private string standloneid="" ;
        private string standlonerunningtime ="";
        public standalone()
        {
            InitializeComponent();
            Thread dataprocess = new Thread(ThreadPrease);
            //Thread updateDeviceInfo = new Thread(ThreadUpdate);
            //updateDeviceInfo.IsBackground = true;
            // beginlisten.IsBackground = true;
            dataprocess.IsBackground = true;
            //updateDeviceInfo.Start();
            dataprocess.Start();


            string[] str = SerialPort.GetPortNames();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                comboBoxforSerialPort.Items.Add(s);
            }
            btnOnlineDev_Click(null, null);
            //串口设置默认选择项
            //   comboBoxforSerialPort.SelectedIndex = 0;         //设置cbSerial的默认选项


        }
        SerialPort _serialPort = new SerialPort();
        static bool _continue;
        private void openSerial_Click(object sender, EventArgs e)
        {
            string[] str = SerialPort.GetPortNames(); //获取系统所有串口
            if (comboBoxforSerialPort.Text == ""||!str.Contains(comboBoxforSerialPort.SelectedItem.ToString()))
            {
                return; //没有选中串口或不包含该串口直接返回
            }
           
            serialPort1.PortName = comboBoxforSerialPort.Text; //串口各项参数赋值
             serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            serialPort1.ReadBufferSize = 2000;
            serialPort1.WriteBufferSize = 2000;
            serialPort1.ReadTimeout = 500;
            serialPort1.WriteTimeout = 500;
            serialPort1.Open(); //打开串口
            _continue = true;
         
            openSerial.Enabled = false;




        }

       


        public void ThreadPrease()
        {
            while (true)
            {
                if (acceptdata.IsEmpty)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    string data = null;
                    if (true == acceptdata.TryDequeue(out data))
                    {
                        dataprocess(data);
                    }
                    else
                    {
                        MessageBox.Show("队列取数失败！ \n  trydequeue failed ");
                    }

                }
            }
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="data"></param>
        private void dataprocess(string data)
        {
            int dataHead = data.IndexOf("@");
            int dataTail = data.IndexOf("$");
            if (dataHead > dataTail)
                return;
            if (dataTail - dataHead < 26)   //信息不完整直接返回；
                return;
            string dataLenth = data.Substring(dataHead + 21, 2);
            int dataLen = stringToInt(dataLenth, 2);
            if (data.Length != 26 + dataLen)
            {
                return;
            }
            string dstSource = data.Substring(dataHead + 1, 4);
            string srcSource = data.Substring(dataHead + 5, 4);
            string id = "20" + data.Substring(dataHead + 9, 10);
            string fc = data.Substring(dataHead + 19, 2);

            string checksum = data.Substring(dataTail - 2, 2);
            int datacheck = stringToInt(checksum, 2);     //数据中的校验和
            string collectData = data.Substring(dataHead + 23, dataLen);
            int check =main.CheckSum(collectData, dataLen);
            if (check != datacheck) return; //校验失败直接退出
            switch (fc)
            {
                //状态采集信息处理
                case "01":

                    dataStructure.device deviceState = new dataStructure.device(id, collectData);
                    //推入在线设备队列
                    standloneid = deviceState.id;
                    standlonerunningtime = deviceState.runningTime;
                    refresh ourrefresh = new refresh(deviceInfo);
                    this.Invoke(ourrefresh, deviceState);





                    //}
                    break;
                case "02":
                    dataStructure.device FaultdeviceState = new dataStructure.device(id, collectData);
                    //推入在线设备队列
                    //onlineDeviceDataQueue.Enqueue(FaultdeviceState);
                    //deviceStateSaveQueue.Enqueue(deviceState);
                    standloneid = FaultdeviceState.id;
                    standlonerunningtime = FaultdeviceState.runningTime;
                    ///检查状态信息中是否有故障
                    errorCheckAndShow(ref faultDevice, FaultdeviceState);
                    break;
                default:
                    break;
            }

            //byte[] idchar = System.Text.Encoding.UTF8.GetBytes(id);
            //dictOfIdforClient[id].Send(idchar);



        }

        /// <summary>
        /// 报警检测（改成数据库较为方便）
        /// </summary>
        /// <param name="faultDevice"></param>
        /// <param name="deviceState"></param>
        public void errorCheckAndShow(ref ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData> faultDevice, dataStructure.device deviceState)
        {

            if ((deviceState.canError & 0x0080) > 0)
            {
                int faultNumber = deviceState.canError & 0x7f;
                //faultData.faultID = deviceState.id;  //报警ID
                //faultData.faultTime = deviceState.time;//报警时间
                //faultData.runningTime = deviceState.runningTime;//运行时间
                if (faultNumber != 0)
                {
                    if (faultDevice.ContainsKey(new KeyValuePair<string, int>(deviceState.id, faultNumber)))  //如果已经有该ID的该类型报警
                    {
                        int errorState = 0;
                        if (faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.LastOrDefault() == 2)
                        {
                            errorState = 0;
                        }
                        else
                        {
                            errorState = 1;
                        }
                        int errorCount = ++faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorCount;
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].faultTime.Add(deviceState.time);
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.Add(errorState);
                        if (faultNumber == 40)
                        {
                            faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorCurrent);
                        }
                        else
                        if (faultNumber == 41)
                        {
                            faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorTemperature);

                        }
                        else
                        {
                            faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(0);

                        }
                        // faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].runningTime = deviceState.runningTime;
                        //存入储存队列
                     //   dataStructure.faultWarnData save = faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)];
                       
                        this.Invoke((Action)(
                            () =>
                            {


                                ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().FirstOrDefault(x => x.SubItems[1].Text == faultNumber.ToString());
                                if (li != null)
                                {
                                    FaultWarnList.BeginUpdate();
                                    li.SubItems[0].Text = deviceState.time.ToString("HH:mm:ss");
                                    li.SubItems[2].Text = errorCount.ToString();
                                    li.BackColor = Color.Red;
                                    FaultWarnList.EndUpdate();
                                }

                              
                                if (btnFaultWarn.Enabled == true) { warningTimer.Enabled = true; }
                            }));


                    }
                    else
                    {
                        dataStructure.faultWarnData errorData = new dataStructure.faultWarnData();
                        errorData.faultID = deviceState.id;
                        //errorData.runningTime = deviceState.runningTime;
                        errorData.errorNumber = faultNumber;
                        errorData.faultTime = new List<DateTime>();
                        errorData.errorValue = new List<int>();
                        errorData.errorState = new List<int>();
                        errorData.faultTime.Add(deviceState.time);                     
                        errorData.errorState.Add(0);
                        if (faultNumber == 40)
                        {
                            errorData.errorValue.Add(deviceState.liftMotorCurrent);
                        }
                        else
                       if (faultNumber == 41)
                        {
                            errorData.errorValue.Add(deviceState.liftMotorCurrent);
                        }
                        else
                        {
                            errorData.errorValue.Add(0);

                        }
                        // errorData.errorArea = dictOfIdAndArea[deviceState.id].Country;
                        //  errorData.errorContent = dataStructure.errorDictionary[errorData.errorNumber];
                        errorData.errorLevel = 1;
                        //  errorData.errorMethod = dataStructure.errorMethod[errorData.errorNumber];
                        errorData.errorCount = 1;
                        faultDevice.TryAdd(new KeyValuePair<string, int>(deviceState.id, faultNumber), errorData);
                     
                                                                  //报警表格中添加信息
                        ListViewItem item = new ListViewItem(deviceState.time.ToString("HH:mm:ss"));
                        item.SubItems.Add(errorData.errorNumber.ToString());
                        //item.SubItems.Add(dictOfIdAndArea[errorData.faultID].Country);
                        item.SubItems.Add(errorData.errorCount.ToString());
                        // item.SubItems.Add(errorData.runningTime.ToString());
                        item.BackColor = Color.Red;
                        this.Invoke((Action)(
                        () => {
                            FaultWarnList.Items.Add(item);
                            if (btnFaultWarn.Enabled == true) { warningTimer.Enabled = true; }
                        })
                            );
                        //errorDeviceDataQueue.Enqueue(faultData);  //推入存储队列

                        //item.SubItems.Add(onlineDeviceNowTimeData["20" + deviceId].canCourse.ToString() + "公里");
                        //if (i % 2 == 0)
                        //{
                        //    item.BackColor = Color.LightSlateGray;
                        //}
                        //i++;
                        //.Items.Add(item);
                    }

                }
            }
            else
            {

                int faultNumber = deviceState.canError & 0x7f;
                int errorCount = 0;
                if (faultDevice.ContainsKey(new KeyValuePair<string, int>(deviceState.id, faultNumber)))
                {
                    errorCount = ++faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorCount;
                    faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].faultTime.Add(deviceState.time);
                    faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.Add(2);
                    if (faultNumber == 40)
                    {
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorCurrent);

                    }
                    else
                        if (faultNumber == 41)
                    {
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorTemperature);

                    }
                    else
                    {
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(0);

                    }
                }
                this.Invoke((Action)(
                 () => {
                     ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().FirstOrDefault(x => x.SubItems[1].Text == faultNumber.ToString());
                     
                     
                     if (li != null)
                     {
                         FaultWarnList.BeginUpdate();
                         li.SubItems[0].Text = deviceState.time.ToString("HH:mm:ss");
                         li.SubItems[2].Text = errorCount.ToString();
                         li.BackColor = Color.Green;
                         FaultWarnList.EndUpdate();
                     }

                 })
                     );

                //ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().First(x => x.Text == deviceState.id && x.SubItems[2].Text == faultNumber.ToString());
                //li.BackColor = Color.Green;

            }

        }







        public void deviceInfo(dataStructure.device device)
        {



            id.Text = device.id;
            time.Text = device.time.ToString();
            runningtime.Text = device.runningTime;
            if (device.isCanConnection == true)
            {
                int canReverse = device.canDirectionandSpeedMode & 0x0004;
                int canInterlock = device.canDirectionandSpeedMode & 0x0010;
                reverse.Text = canReverse.ToString();
                interlock.Text = canInterlock.ToString();
                int Direction = device.canDirectionandSpeedMode & 0x0003;
                int SpeedMode = device.canDirectionandSpeedMode & 0x0008;

                switch (Direction)
                {
                    case 0:
                        direction.Text = "无效";
                        break;
                    case 1:
                        direction.Text = "前进";
                        break;
                    case 2:
                        direction.Text = "后退";
                        break;
                    case 3:
                        direction.Text = "无效";
                        break;
                    default:
                        break;

                }
                if (SpeedMode == 0)
                {
                    speedmode.Text = "高速";
                }
                else
                {
                    speedmode.Text = "低速";
                }

                if (device.canLowPowerMode == 0xaa)
                {
                    lowpowermode.Text = "低功耗";
                }
                else
                {
                    lowpowermode.Text = "其他";
                }

                speed.Text = device.canSpeed.ToString() + "rpm";
                course.Text = device.canCourse.ToString() + "km";
                DirectVoltage.Text = device.canDirectVoltage.ToString() + "V";
                MotorCurrent.Text = device.canMotorCurrent.ToString() + "A";
                MotorTemperature.Text = device.canMotorTemperature.ToString() + "°C";
            }
            else
            {


                reverse.Text = "##";
                interlock.Text = "##";
                direction.Text = "##";
                speedmode.Text = "##";
                lowpowermode.Text = "##";
                speed.Text = "##";
                course.Text = "##";
                DirectVoltage.Text = "##";
                MotorCurrent.Text = "##";
                MotorTemperature.Text = "##";

            }

            //模拟量数据2个
            liftMotorCurrent.Text = device.liftMotorCurrent.ToString() + "A";
            liftMotorTemperature.Text = device.liftMotorTemperature.ToString() + "°C";
            //下面是数字量显示
            hornSwitch.Text = device.hornSwitch ? "正常" : "不正常";
            horn.Text = device.hornCount.ToString() + "次";
            hornGround.Text = device.hornGround ? "不正常" : "正常";
            upBtnSwitch.Text = device.upBtnSwitch ? "正常" : "不正常";
            upBtnContactorCoilUpper.Text = device.upBtnCount.ToString() + "次";
            upBtnContactorCoilDown.Text = device.upBtnContactorCoilDown ? "不正常" : "正常";
            upBtnContactUpper.Text = device.upBtnContactUpper ? "正常" : "不正常";
            upBtnContactDown.Text = device.upBtnContactorCount.ToString() + "次";
            downBtnSwitch.Text = device.downBtnSwitch ? "正常" : "不正常";
            downBtnsolenoidvalveUpper.Text = device.downBtnCount.ToString() + "次";
            downBtnsolenoidvalveDown.Text = device.downBtnsolenoidvalveDown ? "不正常" : "正常";
            masterContactorCoilUpper.Text = device.masterContactorCoilUpper ? "正常" : "不正常";
            masterContactorCoilDown.Text = device.masterContactorCoilDown ? "不正常" : "正常";
            masterContactUpper.Text = device.masterContactUpper ? "正常" : "不正常";
            masterContactDown.Text = device.masterContactorCount.ToString() + "次";
            arresterUpper.Text = device.arresterUpper ? "正常" : "不正常";
            arresterDown.Text = device.arresterCount.ToString() + "次";

        }
       
        public static int stringToInt(string data, int len)
        {
            int nRet = 0;
            for (int i = 0; i < len; ++i)
            {
                nRet <<= 4;
                nRet += CharToInt(data[i]);
            }
            return nRet;

        }


        public static int CharToInt(char uc)
        {
            if (!IsXDigit(uc))      //isxdigi检查是否是16进制数，是则返回1,否者返回0
                return 0;                           //不是16进制数据则返回

            int iVal = 0;
            if (uc > 0x60) iVal = uc - 0x57;            //将a-f转换为10-15
            else if (uc > 0x40) iVal = uc - 0x37;   //将A-F转换为10-15
            else if (uc > 0x30) iVal = uc - 0x30;    //字符0-9转换为数字0-9
            return iVal;
        }

        public static bool IsXDigit(char c)
        {
            if ('0' <= c && c <= '9') return true;
            if ('a' <= c && c <= 'f') return true;
            if ('A' <= c && c <= 'F') return true;
            return false;








        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);

            byte[] bytes = new byte[1024];
            int len;
            try { 
            //string data = _serialPort.ReadLine();
            len = serialPort1.Read(bytes, 0, bytes.Length);
            string data = System.Text.Encoding.UTF8.GetString(bytes, 0, len);
                buffer = buffer + data;
                List<string> standardData = new List<string>();
                while (true)            //根据指定协议进行分包
                {
                    if (buffer.Length > 0)
                    {
                        int end_index = buffer.IndexOf("$");     //查找$的位置
                        if (end_index > 0)   //存在的话
                        {
                            int begin_index = buffer.IndexOf("@");  //查找@的位置
                            if (begin_index >= 0)
                            {
                                string one_frame = buffer.Substring(begin_index, end_index - begin_index + 1);   //插入到要解析的列表中
                                standardData.Add(one_frame);
                            }
                            buffer = buffer.Substring(end_index + 1);  //截取前面一段
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                  foreach (string item in standardData)
            {
                //string dataLenth = dataForCheck.Substring( 20, 2);  //检验
                //int dataLen = stringToInt(dataLenth, 2);
                //string checksum = dataForCheck.Substring(dataForCheck.Length-1 - 2, 2);
                //int datacheck = stringToInt(checksum, 2);     //数据中的校验和


                //string dataForCheck = "@" + item + "$";
                int dataHead = item.IndexOf("@");
                int dataTail = item.IndexOf("$");
                if (dataHead > dataTail)
                    continue;
                if (dataTail - dataHead < 26)   //信息不完整直接返回；
                    continue;
                string dataLenth = item.Substring(dataHead + 21, 2);
                int dataLen = stringToInt(dataLenth, 2);
                if (item.Length != 26 + dataLen)
                {
                    continue;
                }
                string dstSource = item.Substring(dataHead + 1, 4);
                string srcSource = item.Substring(dataHead + 5, 4);
                string id = "20" + item.Substring(dataHead + 9, 10);
                string fc = item.Substring(dataHead + 19, 2);

                string checksum = item.Substring(dataTail - 2, 2);
                int datacheck = stringToInt(checksum, 2);     //数据中的校验和
                string collectData = item.Substring(dataHead + 23, dataLen);
                int check =main.CheckSum(collectData, dataLen);
                if (check != datacheck) continue; //校验失败直接退出
                acceptdata.Enqueue(item);

            }
            }catch(Exception ex)
            {
                serialPort1.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            string[] str = SerialPort.GetPortNames();
        
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                if(!comboBoxforSerialPort.Items.Contains(s))
                comboBoxforSerialPort.Items.Add(s);
            }

          
        }

        private void FaultWarnList_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            ListViewHitTestInfo info = FaultWarnList.HitTest(e.X, e.Y);
            var item = info.Item as ListViewItem;
           
            int errornumber = int.Parse(item.SubItems[1].Text);
            warnInfo form = new warnInfo(faultDevice[new KeyValuePair<string, int>(standloneid, errornumber)],"default");
            form.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定关闭系统吗？", "关闭确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.ExitThread();
            }
        }

        private void systemtimer_Tick(object sender, EventArgs e)
        {
            timetext.Text = DateTime.Now.ToString();
            timetext2.Text= DateTime.Now.ToString();
           
        }

        private void btnOnlineDev_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            this.pnlOnlineDev.Visible = true;
            this.btnOnlineDev.BackColor = Color.Orange;
            btnOnlineDev.Enabled = false;
       
        }

      
        //private void setAllPanelFalse()
        //{
        //    this.pnlOnlineDev.Visible = false;

        //    this.pnlSetting.Visible = false;
        //}
        /// <summary>
        /// 所有按钮最初颜色
        /// </summary>

        private void setAllPanelFalse()
        {
            this.pnlOnlineDev.Visible = false;
            this.pnlFaultWarn.Visible = false;
            
        }
        /// <summary>
        /// 所有按钮最初颜色
        /// </summary>
        private void setAlButtondeafult()
        {


            this.btnFaultWarn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnOnlineDev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
          
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
           
            btnFaultWarn.Enabled = true;
            btnOnlineDev.Enabled = true;
            
        }

        private void standalone_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }

      

     

  

        private void btnFaultWarn_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            this.pnlFaultWarn.Visible = true;
            this.btnFaultWarn.BackColor = Color.Orange;
            btnFaultWarn.Enabled = false;
            warningTimer.Enabled = false;
        }

   
        private void warningTimer_Tick(object sender, EventArgs e)
        {
            if (btnFaultWarn.BackColor == Color.Red)
            { btnFaultWarn.BackColor = Color.Silver; }
            else { btnFaultWarn.BackColor = Color.Red; }

        }

        private void FaultWarnList_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = FaultWarnList.HitTest(e.X, e.Y);
            var item = info.Item as ListViewItem;
            string id = standloneid;
            int errornumber = int.Parse(item.SubItems[1].Text);
            if (faultDevice.ContainsKey(new KeyValuePair<string, int>(id, errornumber)))
            {
                warnInfo form = new warnInfo(faultDevice[new KeyValuePair<string, int>(id, errornumber)], "default");
                form.Show();
            }
           
        }
    }
}
