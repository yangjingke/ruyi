using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using forkliftDataStruct;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace ruyi
{

    public partial class main : Form
    {
        private Socket server;  //服务器套接字
        //private readonly BindingList<string,string> _data = new BindingList<string,string>();
        private ConcurrentDictionary<string, Socket> dictOfIdforClient = new ConcurrentDictionary<string, Socket>();//叉车ID、客户端连接套接字字典
        private ConcurrentDictionary<string, Location> dictOfIdAndArea = new ConcurrentDictionary<string, Location>();//叉车ID、地区信息字典
        private BlockingQueue<string> acceptdata = new BlockingQueue<string>(); //接收数据（字符串）的队列
        private BlockingQueue<dataStructure.device> onlineDeviceDataQueue = new BlockingQueue<dataStructure.device>(); //在线设备状态更新（添加）队列
        private BlockingQueue<dataStructure.device> onlineDeviceDatabaseQueue = new BlockingQueue<dataStructure.device>(); //在线设备状态更新数据库（添加）队列
        private BlockingQueue<KeyValuePair<string, string>> ip2locationqueue = new BlockingQueue<KeyValuePair<string,string>>();//叉车ID、ip地址组成的pair值的待查询队列
        private BlockingQueue<dataStructure.device> deviceStateSaveQueue = new BlockingQueue<dataStructure.device>();//设备状态储存队列
        private BlockingQueue<dataStructure.WarnInfo> errorDeviceDataQueue = new BlockingQueue<dataStructure.WarnInfo>(); //设备故障或（解除故障）储存队列;
        private ConcurrentDictionary<string, dataStructure.device> onlineDeviceNowTimeData = new ConcurrentDictionary<string, dataStructure.device>();//在线设备列表                                               
        private ConcurrentDictionary<string, string> buffer = new ConcurrentDictionary<string, string>();//字符串缓存字典
        private ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData> faultDevice = new ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData>();//故障报警列表
        private ConcurrentDictionary<string, int> devicetimestamp = new ConcurrentDictionary<string, int>();
        List<dataStructure.faultWarnData> warnhistory = new List<dataStructure.faultWarnData>();//历史故障信息缓存列表

        private string language = "";
        //private BindingList<dataStructure.faultWarnData> _errordata = new BindingList<dataStructure.faultWarnData>(); //故障报警列表数据源
        //private ConcurrentDictionary<string, bool> deviceFormerState = new ConcurrentDictionary<string, bool>();// XXID叉车上次是否故障字典
        string selectStateName ="";
        string selectNodeText = "";
        private int istoday = DateTime.Now.Day;
        private int port = Properties.Settings.Default.port;
        static string sqlconstr = "Database="+Properties.Settings.Default.database+ ";Data Source="+ Properties.Settings.Default.source+ ";User ID="+Properties.Settings.Default.user +"; password="+ Properties.Settings.Default.password;
        //  QQWry.NET.QQWryLocator qqWry = new QQWry.NET.QQWryLocator("qqwry.dat");


        /// <summary>
        /// 功能：根据叉车ID号获取该id叉车的最新状态信息
        /// </summary>
        /// <param name="id">需要查询的id</param>
        /// <returns>Device类型的该ID的叉车状态信息</returns>
        public dataStructure.device getNewlyDataState(string id)
        {
            if (onlineDeviceNowTimeData.ContainsKey(id)) //检查是否包含该叉车的信息
            {
                return onlineDeviceNowTimeData[id];//包含返回该信息
            }
            else return null;//不包含返回null
        }
        /// <summary>
        /// 功能：根据叉车ID获取对应的TCP套接字
        /// </summary>
        /// <param name="id">需要查询的id</param>
        /// <returns>Device类型的该ID的叉车状态信息</returns>
        public Socket getSocketById(string id)
        {
            if (dictOfIdforClient.ContainsKey(id)) //检查是否包含该叉车的套接字
            {
                return dictOfIdforClient[id];//包含返回该套接字
            }
            else return null;//不包含返回null
        }
        
        System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(main));
        public main(string lang)
        {
            language = lang;//保留选择的语言
            if (language=="en") //英语的话调用英文界面
            { 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                //对当前窗体应用更改后的资源
            }

            InitializeComponent();//系统自带的
           
           //声明以下四个线程，并设置其为后台工作，并启动
            Thread parseip = new Thread(Threadip2location);  
            Thread saveStateData = new Thread(ThreadSaveState);
            Thread saveErrorData = new Thread(ThreadSaveError);
            Thread dataprocess = new Thread(ThreadPrease);
            Thread updateDeviceInfo = new Thread(ThreadUpdate);
            Thread SaveOnlineDate = new Thread(ThreadSaveOnline);
            parseip.IsBackground = true;           
            updateDeviceInfo.IsBackground = true;
            dataprocess.IsBackground = true;
            saveStateData.IsBackground = true;
            saveErrorData.IsBackground = true;
            SaveOnlineDate.IsBackground = true;
            SaveOnlineDate.Start();
            saveStateData.Start();
            saveErrorData.Start();
            parseip.Start(); 
            updateDeviceInfo.Start();
            dataprocess.Start();
            // 初始化连接打印机资源的控件
            printDocument1.DefaultPageSettings.Landscape = true;
            printDocument2.DefaultPageSettings.Landscape = true;
            printDocument3.DefaultPageSettings.Landscape = true;
            //点击在线设备按钮（目的是初始化后显示该页面）
            onlinedev_Click(null, null);
            portnumber.Text = port.ToString();
            connect_Click(null, null);
            //this.ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;

        }

        /// <summary>
        /// 在线设备按钮单击触发函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void onlinedev_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();//所有页面都不可见
            setAlButtondeafult();//所有按钮都变成默认值
            this.pnlOnlineDev.Visible = true;//按钮对应的界面可见
           this.btnOnlineDev.BackColor = Color.Orange;//按钮背景色变色
            btnOnlineDev.Enabled = false;//该按钮点击后不可点击
           
        }

        private void faultCount_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            this.pnlFaultCount.Visible = true;
            this.btnFaultCount.BackColor = Color.Orange;
            btnFaultCount.Enabled = false;
            button1_Click_1(null, null);
        }
        public static int GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (int)(ts.TotalSeconds);
        }

        private void faultwarn_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            this.pnlFaultWarn.Visible = true;
            this.btnFaultWarn.BackColor = Color.Orange;
            warningTimer.Enabled = false;
            btnFaultWarn.Enabled = false;
        }
        private void btnRepairRecordAdd_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            this.pnlRepairRecord.Visible = true;
            this.btnRepairRcd.BackColor = Color.Orange;
            btnRepairRcd.Enabled = false;
           
        }

        private void btnStatehistory_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            pnlStatehistory.Visible = true;
            this.btnStateHistory.BackColor = Color.Orange;
            btnStateHistory.Enabled = false;
           
        }
        /// <summary>
        /// 所有子窗口不可见
        /// </summary>
        private void setAllPanelFalse()
        {
            this.pnlOnlineDev.Visible = false;
            this.pnlFaultWarn.Visible = false;                   
            this.pnlStatehistory.Visible = false;
            this.pnlRepairRecord.Visible = false;
            this.pnlFaultCount.Visible = false;
        }
        /// <summary>
        /// 所有按钮最初颜色
        /// </summary>
        private void setAlButtondeafult()
        {


            this.btnFaultWarn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81))))); 
            this.btnOnlineDev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81))))); 
            this.btnRepairRcd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnStateHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnFaultCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));


            btnFaultWarn.Enabled = true;
            btnOnlineDev.Enabled = true;
            btnStateHistory.Enabled = true;
            btnRepairRcd.Enabled = true;
            btnFaultCount.Enabled = true;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Exit form = new Exit();//生成exit窗口
            form.ShowDialog();//显示
            
        }

        

        public IPAddress GetLocalIPV4()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            //遍历获得的IP集以得到IPV4地址
            foreach (IPAddress ip in ips)
            {
                //筛选出IPV4地址
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            //如果没有则返回IPV6地址
            return ips[0];
        }




        /// <summary>
        /// 监听线程函数
        /// </summary>

        public void ThreadTcpServer(object port)
        {
           
            try {
                IPAddress ipaddress = GetLocalIPV4(); //获取本机ip地址
                
                IPEndPoint iep = new IPEndPoint(ipaddress, (int)port); 
               
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //定义服务器对象

                // 将套接字与本地终结点绑定    
                server.Bind(iep); //绑定ip

                server.Listen(5000);//开始监听

                List<Socket> socketList = new List<Socket>(); //套接字列表
                socketList.Add(server);//将服务端添加进来

                while (true)
                {

                    List<Socket> temp = socketList.ToList(); //复制临时列表
                    Socket.Select(temp, null, null, 1000); //筛选出发生变化的套接字
                    int count = temp.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (temp[i].Equals(server)) //发生变化的是服务端，意味着有新的连接
                        {
                            Socket client = socketList[i].Accept(); //建立新的客户端连接
                            socketList.Add(client);//加入套接字列表
                            try
                            {
                            string nowtime = DateTime.Now.ToString("yyMMddHHmmss");
                            string  sendtime= "@000000010000000000040C"+nowtime+intTostring(CheckSum(nowtime, 12), 2)+"$";
                            byte[] commandchar = System.Text.Encoding.UTF8.GetBytes(sendtime);
                            client.Send(commandchar);
                            }
                            catch (Exception e)
                            {
                                //MessageBox.Show(e.ToString());
                            }
                         }
                        else //发生变化的是客户端，代表着数据需要接收
                        {
                            byte[] bytes = new byte[1024]; //缓存数组
                            int len;

                            try {
                                if ((len = temp[i].Receive(bytes)) > 0) //接收到的数据长度大于0
                                {
                                    string ip = ((IPEndPoint)temp[i].RemoteEndPoint).Address.ToString();  //查询ip
                                    string data = System.Text.Encoding.UTF8.GetString(bytes, 0, len); //转化为string
                                    if (buffer.ContainsKey(((IPEndPoint)temp[i].RemoteEndPoint).Address.ToString()))
                                    {
                                         
                                        buffer[ip] = buffer[ip] + data;
                                    }
                                    else
                                    {
                                      
                                       
                                            if (false == buffer.TryAdd(ip, data))  //添加到缓存字典中
                                                MessageBox.Show("dictionary添加失败 \n Dictionary update failed");
                                    }
                                    List<string> standardData = new List<string>();
                                    while (true)            //根据指定协议进行分包
                                    {
                                        if (buffer[ip].Length>0)
                                        {
                                            int end_index = buffer[ip].IndexOf("$");     //查找$的位置
                                            if (end_index>0)   //存在的话
                                            {
                                                int begin_index = buffer[ip].IndexOf("@");  //查找@的位置
                                                if(begin_index>=0)
                                                {
                                                    string one_frame = buffer[ip].Substring(begin_index, end_index - begin_index + 1);   //插入到要解析的列表中
                                                    standardData.Add(one_frame);
                                                }
                                                buffer[ip] = buffer[ip].Substring(end_index + 1);  //截取前面一段
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

                                    //string[] standardData = data.Split(new Char[] { '@', '$' }, StringSplitOptions.RemoveEmptyEntries); //根据指定协议进行分包
                                    

                                    foreach (string item in standardData) //分别对分包后的数据进行处理
                                    {
                                        

                                        //string dataForCheck = "@" + item + "$"; //重新加上开头与结尾
                                        int dataHead = item.IndexOf("@");//开头位置
                                        int dataTail = item.IndexOf("$");//结尾位置
                                        if (dataHead > dataTail) //开头位置在结尾位置后面，错误，返回
                                            continue;
                                        if (dataTail - dataHead < 26)   //信息不完整直接返回；
                                            continue;
                                        string dataLenth = item.Substring(dataHead + 21, 2); //获取数据长度
                                        int dataLen = stringToInt(dataLenth, 2);
                                        if (item.Length != 26 + dataLen) //如果数据长度不对，返回
                                        {
                                            continue;
                                        }
                                        string dstSource = item.Substring(dataHead + 1, 4); //目的地址
                                        string srcSource = item.Substring(dataHead + 5, 4); //源地址
                                        string id = "20" + item.Substring(dataHead + 9, 10);//叉车id
                                        string fc = item.Substring(dataHead + 19, 2);//功能码

                                        string checksum = item.Substring(dataTail - 2, 2); 
                                        int datacheck = stringToInt(checksum, 2);     //数据中的校验和
                                        string collectData = item.Substring(dataHead + 23, dataLen);
                                        int check = CheckSum(collectData, dataLen); //重新计算校验和
                                        if (check != datacheck) continue; //校验失败直接退出


                                        try
                                        {
                                            if(devicetimestamp.ContainsKey(id)) //添加或更新时间戳
                                            {
                                                devicetimestamp[id] = GetTimeStamp();
                                            }
                                            else
                                            {
                                                if (false == devicetimestamp.TryAdd(id,GetTimeStamp()))    //添加到套接字字典中
                                                    MessageBox.Show("dictionary添加失败 \n Dictionary update failed");
                                            }
                                            if (!dictOfIdforClient.ContainsKey(id))   //是否存在该ID的叉车
                                            {


                                                if (false == dictOfIdforClient.TryAdd(id, temp[i]))    //添加到套接字字典中
                                                    MessageBox.Show("dictionary添加失败 \n Dictionary update failed");
                                              


                                              ip2locationqueue.Enqueue(new KeyValuePair<string, string>(id,ip)); //将该id与ip键值对推入队列中，进行查询
                                             

                                            }
                                           
                                        }
                                        catch (ArgumentNullException) //异常捕获
                                        {
                                            MessageBox.Show("ArgumentNullException");
                                        }
                                       
                                        acceptdata.Enqueue(item);   //数据添加到解析队列 

                                        

                                    }
                                   


                                }
                                else   //接收数据不大于0，意味连接终止，删除对应的各个字典，并对区域分布树状图进行调整。
                                {

                                    //通过IP查询ID号来进行删除(同一个IP可能多个id）
                                    var query = (from d in dictOfIdforClient 
                                                 where d.Value == temp[i]
                                                 select d.Key).ToArray();    
                                    if (query.Count() > 0) {
                                        var Area = dictOfIdAndArea[query[0]];//获取该ip的信息区域信息
                                       



                                        for (int n = 0; n < query.Count(); n++)  //对每个ID删除对应的字典中该ID的值
                                        {
                                            var tempValue = dictOfIdforClient[query[n]];
                                            var tempArea = dictOfIdAndArea[query[n]];
                                            dictOfIdforClient.TryRemove(query[n], out tempValue);
                                            dictOfIdAndArea.TryRemove(query[n], out tempArea);
                                            var tempValue1 = onlineDeviceNowTimeData[query[n]];
                                            onlineDeviceNowTimeData.TryRemove(query[n], out tempValue1);
                                            var temptimestamp = devicetimestamp[query[n]];
                                            devicetimestamp.TryRemove(query[n], out temptimestamp);

                                        }

                                            //查询该下线叉车的所在城市，如果没有该城市的叉车，对该城市节点进行删除
                                            var cityResult = (from item in dictOfIdAndArea where item.Value.city_name.Contains(Area.city_name) select item).ToList();
                                            if (cityResult.Count == 0)
                                            {
                                                this.Invoke((Action)(() => {
                                                    if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name))
                                                    {
                                                        if (onlineDevicetreeView.Nodes[Area.country_name].Nodes.ContainsKey(Area.region_name))
                                                        { 
                                                        if (
                         onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Nodes.ContainsKey(Area.city_name))

                                                            { onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Nodes[Area.city_name].Remove(); }
                                                        } } }));
                                            }
                                        //查询该下线叉车的所在省份，如果没有该省份的叉车，对该省份节点进行删除
                                        var RegionResult = (from item in dictOfIdAndArea where item.Value.region_name.Contains(Area.region_name) select item).ToList();
                                            if (RegionResult.Count == 0)
                                            {
                                                this.Invoke((Action)(() =>
                                                {
                                                    if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name))

                                                    {
                                                        if (onlineDevicetreeView.Nodes[Area.country_name].Nodes.ContainsKey(Area.region_name))
                                                        {
                                                            onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Remove();
                                                        }
                                                    }
                                                }));
                                            }

                                        //查询该下线叉车的所在国家，如果没有该国家的叉车，对该国家节点进行删除
                                        var CountryResult = (from item in dictOfIdAndArea where item.Value.country_name.Contains(Area.country_name) select item).ToList();
                                        if (CountryResult.Count == 0)
                                        {
                                            this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name)) { onlineDevicetreeView.Nodes[Area.country_name].Remove(); } }));
                                        }







                                    }

                                    temp[i].Close();  //关闭对应套接字
                                    socketList.Remove(temp[i]);//列表中删除该套接字

                                    // Console.WriteLine("关闭");
                                }
                            }
                            catch (Exception e) 
                            {
                                if (temp[i] != server)   //同上，异常状态下同样关闭套接字
                                {
                                    var query = (from d in dictOfIdforClient
                                                 where d.Value == temp[i]
                                                 select d.Key).ToArray();
                                    if (query.Count() > 0)
                                    {
                                        if (dictOfIdAndArea.ContainsKey(query[0]))
                                        { 
                                        var Area = dictOfIdAndArea[query[0]];
                                        
                                        for (int n = 0; n < query.Count(); n++)
                                        {

                                            var tempValue = dictOfIdforClient[query[n]];
                                            var tempArea = dictOfIdAndArea[query[n]];
                                            dictOfIdforClient.TryRemove(query[n], out tempValue);
                                            dictOfIdAndArea.TryRemove(query[n], out tempArea);
                                            var tempValue1 = onlineDeviceNowTimeData[query[n]];
                                            onlineDeviceNowTimeData.TryRemove(query[n], out tempValue1);
                                            var temptimestamp = devicetimestamp[query[n]];
                                            devicetimestamp.TryRemove(query[n], out temptimestamp);

                                            var cityResult = (from item in dictOfIdAndArea where item.Value.city_name.Contains(Area.city_name) select item).ToList();
                                            if (cityResult.Count == 0)
                                            {
                                                this.Invoke((Action)(() =>
                                                {
                                                    if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name))
                                                    {
                                                        if (onlineDevicetreeView.Nodes[Area.country_name].Nodes.ContainsKey(Area.region_name))
                                                        {
                                                            if (
                             onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Nodes.ContainsKey(Area.city_name))

                                                            { onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Nodes[Area.city_name].Remove(); }
                                                        }
                                                    }
                                                }));
                                            }
                                            //删除对应省份节点
                                            var RegionResult = (from item in dictOfIdAndArea where item.Value.region_name.Contains(Area.region_name) select item).ToList();
                                            if (RegionResult.Count == 0)
                                            {
                                                this.Invoke((Action)(() =>
                                                {
                                                    if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name))

                                                    {
                                                        if (onlineDevicetreeView.Nodes[Area.country_name].Nodes.ContainsKey(Area.region_name))
                                                        {
                                                            onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Remove();
                                                        }
                                                    }
                                                }));
                                            }

                                            //删除对应国家节点
                                            var CountryResult = (from item in dictOfIdAndArea where item.Value.country_name.Contains(Area.country_name) select item).ToList();
                                            if (CountryResult.Count == 0)
                                            {
                                                this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name)) { onlineDevicetreeView.Nodes[Area.country_name].Remove(); } }));
                                            }


                                        }
                                    }
                                    }

                                    temp[i].Close();
                                    socketList.Remove(temp[i]);
                                }


                            }

                        }
                    }
                    int nowtimestamp = GetTimeStamp();
                    var timequery = (from d in devicetimestamp where (nowtimestamp - d.Value > 120) select d.Key).ToArray();
                    if (timequery.Count() > 0)
                    {
                        var Area = dictOfIdAndArea[timequery[0]];//获取该ip的信息区域信息




                        for (int n = 0; n < timequery.Count(); n++)  //对每个ID删除对应的字典中该ID的值
                        {
                            var tempSocket = dictOfIdforClient[timequery[n]];
                            var tempArea = dictOfIdAndArea[timequery[n]];
                            dictOfIdforClient.TryRemove(timequery[n], out tempSocket);
                            tempSocket.Close();  //关闭对应套接字
                            socketList.Remove(tempSocket);//列表中删除该套接字
                            dictOfIdAndArea.TryRemove(timequery[n], out tempArea);
                            var tempValue1 = onlineDeviceNowTimeData[timequery[n]];
                            onlineDeviceNowTimeData.TryRemove(timequery[n], out tempValue1);
                            var temptimestamp = devicetimestamp[timequery[n]];
                            devicetimestamp.TryRemove(timequery[n], out temptimestamp);

                        }

                        //查询该下线叉车的所在城市，如果没有该城市的叉车，对该城市节点进行删除
                        var cityResult = (from item in dictOfIdAndArea where item.Value.city_name.Contains(Area.city_name) select item).ToList();
                        if (cityResult.Count == 0)
                        {
                            this.Invoke((Action)(() => {
                                if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name))
                                {
                                    if (onlineDevicetreeView.Nodes[Area.country_name].Nodes.ContainsKey(Area.region_name))
                                    {
                                        if (
         onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Nodes.ContainsKey(Area.city_name))

                                        { onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Nodes[Area.city_name].Remove(); }
                                    }
                                }
                            }));
                        }
                        //查询该下线叉车的所在省份，如果没有该省份的叉车，对该省份节点进行删除
                        var RegionResult = (from item in dictOfIdAndArea where item.Value.region_name.Contains(Area.region_name) select item).ToList();
                        if (RegionResult.Count == 0)
                        {
                            this.Invoke((Action)(() =>
                            {
                                if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name))

                                {
                                    if (onlineDevicetreeView.Nodes[Area.country_name].Nodes.ContainsKey(Area.region_name))
                                    {
                                        onlineDevicetreeView.Nodes[Area.country_name].Nodes[Area.region_name].Remove();
                                    }
                                }
                            }));
                        }

                        //查询该下线叉车的所在国家，如果没有该国家的叉车，对该国家节点进行删除
                        var CountryResult = (from item in dictOfIdAndArea where item.Value.country_name.Contains(Area.country_name) select item).ToList();
                        if (CountryResult.Count == 0)
                        {
                            this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(Area.country_name)) { onlineDevicetreeView.Nodes[Area.country_name].Remove(); } }));
                        }







                    }


                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }
      
        /// <summary>
        /// 解析线程函数
        /// </summary>
        public void ThreadPrease()
        {
            while (true)
            {

                
                    string data= acceptdata.Dequeue();    
                    dataprocess(data); //调用解析函数处理
                    


                
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
            string id ="20"+ data.Substring(dataHead + 9, 10);
            string fc = data.Substring(dataHead + 19, 2);

            string checksum = data.Substring(dataTail - 2, 2);
            int datacheck = stringToInt(checksum, 2);     //数据中的校验和
            string collectData = data.Substring(dataHead + 23, dataLen);
            int check = CheckSum(collectData, dataLen);
            if (check != datacheck) return; //校验失败直接退出
            switch (fc)//根据功能码不同分别进行解析
            {
                //状态采集信息处理
                case "01":

                    dataStructure.device deviceState = new dataStructure.device(id, collectData);
                    //推入在线设备更新队列、设备状态信息存储队列
                    onlineDeviceDataQueue.Enqueue(deviceState);
                    deviceStateSaveQueue.Enqueue(deviceState);
                    onlineDeviceDatabaseQueue.Enqueue(deviceState);
               
                    break;
                case"02":
                    dataStructure.device FaultdeviceState = new dataStructure.device(id, collectData);
                    //推入在线设备队列
                    

                    ///检查状态信息中是否有故障
                    errorCheckAndShow(ref faultDevice, FaultdeviceState);
                    break;
                default:
                    break;
            }




        }
        /// <summary>
        /// 报警检测（改成数据库较为方便）
        /// </summary>
        /// <param name="faultDevice"></param>
        /// <param name="deviceState"></param>
         public void errorCheckAndShow(ref ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData> faultDevice, dataStructure.device deviceState)
        {

            if (istoday != DateTime.Now.Day) //faultDevice中仅保留本日故障信息， 每天进行数据清零。
            {


                faultDevice.Clear();
                this.Invoke((Action)(
                    () => {
                        FaultWarnList.Items.Clear();
                    })
                        );
               
                istoday = DateTime.Now.Day;
            }
            if ((deviceState.canError&0x0080)>0)  //如果有故障发生 canError高位为1代表有故障
            {
                int faultNumber = deviceState.canError&0x7f; //获取故障码
                if (faultNumber != 0)  
            {
                    if (faultDevice.ContainsKey(new KeyValuePair<string, int>(deviceState.id, faultNumber)))  //如果已经有该ID的该类型报警
                    {
                        dataStructure.WarnInfo save = new dataStructure.WarnInfo();  //新的故障信息类对象
                        
                        if(faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.LastOrDefault()==2) //如果上次该故障已经恢复
                        {
                            save.errorState = 0;    //将故障信息类的故障状态置为0，代表故障发生
                        }
                        else
                        {
                            save.errorState = 1; //将故障信息类的故障状态置为1，代表故障进行中
                        }
                        //对故障信息类对象的各项属性赋值
                        save.id = deviceState.id;    //id
                        save.errorNumber = faultNumber; //故障码
                        save.time = deviceState.time;//故障发生时间
                        save.errorLevel = 1;//故障等级
                        int errorCount= ++faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorCount; //将faultDevice的该故障次数+1，并赋值给errorCount
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].faultTime.Add(deviceState.time);//faultDevice记录新的故障时间
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.Add(save.errorState);//faultDevice记录新的故障状态
                        if (faultNumber ==40) //故障码为40，代表电流过大 
                            {
                                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorCurrent); //记录新的报警值
                            save.errorValue = deviceState.liftMotorCurrent; //故障报警值赋值
                        }
                            else 
                            if(faultNumber==41)// 故障码为41，代表温度过高，同40，故不赘述
                            {
                                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorTemperature);
                            save.errorValue = deviceState.liftMotorTemperature;
                        }
                            else   //其他故障码没有故障值，故置0
                            {
                                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(0);
                             save.errorValue = 0;
                            }
                       //存入储存队列             
                        errorDeviceDataQueue.Enqueue(save);
                        this.Invoke((Action)(               //对界面中的故障处理界面的当日故障进行界面更新
                            () =>
                        {
                       
                            //查找界面中是否有该故障
                            ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Text == deviceState.id && x.SubItems[2].Text == faultNumber.ToString());
                            if (li != null)   //有的话进行更新 
                            {
                                FaultWarnList.BeginUpdate();
                                li.SubItems[1].Text = deviceState.time.ToString("HH:mm:ss");  //最新报警时间更新
                                li.SubItems[3].Text = errorCount.ToString(); //报警次数更新
                                li.BackColor = Color.Red;  //报警，颜色为红色
                                FaultWarnList.EndUpdate();
                            }
                            if (btnFaultWarn.Enabled == true) { warningTimer.Enabled = true; }   //按钮闪动
                        }));


                    } ////如果已经有该ID的该类型报警
                    else
                    {
                        dataStructure.WarnInfo save = new dataStructure.WarnInfo(); //同上，各成员变量赋值
                        save.id = deviceState.id;
                        save.errorLevel = 1;
                        save.errorNumber = faultNumber;
                        save.errorState = 0;        //之前没有报警，所以为0,代表故障开始
                        save.time = deviceState.time;
                        
                        dataStructure.faultWarnData errorData = new dataStructure.faultWarnData();  //之前没该ID的该类型报警，需要初始化加入到faultDevice中
                        errorData.faultID = deviceState.id;
                        errorData.errorNumber = faultNumber;
                        errorData.faultTime = new List<DateTime>();
                        errorData.errorValue = new List<int>();
                        errorData.errorState = new List<int>();
                        errorData.faultTime.Add(deviceState.time);
                        errorData.errorState.Add(save.errorState);
                            if (faultNumber == 40) //同上
                            {
                                errorData.errorValue.Add(deviceState.liftMotorCurrent);
                                   save.errorValue = deviceState.liftMotorCurrent;
                            }
                            else
                           if (faultNumber == 41)
                            {
                                errorData.errorValue.Add(deviceState.liftMotorTemperature);
                                save.errorValue = deviceState.liftMotorTemperature;
                        }
                            else
                            {
                                errorData.errorValue.Add(0);
                                save.errorValue = 0;
                            }
                            
                            errorData.errorLevel = 1;
                        errorData.errorCount = 1;
                        faultDevice.TryAdd(new KeyValuePair<string, int>(deviceState.id, faultNumber), errorData); //将该id的该类型的报警信息加入到faultDevice中！
                        errorDeviceDataQueue.Enqueue(save);  //存入储存队列
                        //报警表格中添加信息
                        ListViewItem item = new ListViewItem(errorData.faultID); 
                        item.SubItems.Add(deviceState.time.ToString("HH:mm:ss"));
                        item.SubItems.Add(errorData.errorNumber.ToString());                 
                        item.SubItems.Add(errorData.errorCount.ToString());                     
                        item.BackColor = Color.Red;
                        this.Invoke((Action)(         //在故障处理当日故障表格中添加该栏目报警信息
                        () => { FaultWarnList.Items.Add(item); if (btnFaultWarn.Enabled == true) { warningTimer.Enabled = true; }
                            })
                            );
                                    
                    }

                }
            }
            else   //高位不为零，代表故障恢复，以下内容类似部分不再赘述
            {
                int faultNumber = deviceState.canError & 0x007f;
                dataStructure.WarnInfo save = new dataStructure.WarnInfo();
                save.id = deviceState.id;
                save.errorState = 2;
                save.errorNumber =faultNumber;
                save.time = deviceState.time;
                save.errorLevel = 1;
                int errorCount = 0;
                if (faultNumber == 40)
                {
                    
                    save.errorValue = deviceState.liftMotorCurrent;
                }
                else   if (faultNumber == 41)
                {
                  
                    save.errorValue = deviceState.liftMotorTemperature;
                }
                else
                {
                   
                    save.errorValue = 0;
                }
                if (faultDevice.ContainsKey(new KeyValuePair<string, int>(deviceState.id, faultNumber)))
                {
                    errorCount = ++faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorCount;
                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].faultTime.Add(deviceState.time);
                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.Add(save.errorState);
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
                errorDeviceDataQueue.Enqueue(save);  //存入储存队列
               
                this.Invoke((Action)(   //更新故障处理界面的当日故障表格，将对应ID的对应项变成绿色，代表故障恢复
                 () => {
                     ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Text == deviceState.id && x.SubItems[2].Text == faultNumber.ToString());
                 if (li!=null)
                     {
                         FaultWarnList.BeginUpdate();
                         li.SubItems[1].Text = deviceState.time.ToString("HH:mm:ss");
                         li.SubItems[3].Text = errorCount.ToString();
                         li.BackColor = Color.Green;
                         FaultWarnList.EndUpdate();
                         //li.BackColor = Color.Green;
                     }
                 })
                     );

              

            }
           
        }
        /// <summary>
        /// 实时查询ip数据库
        /// </summary>
        public void Threadip2location()
        {
            while(true)
            {

                    KeyValuePair<string, string> idandip = ip2locationqueue.Dequeue();  
                    //ip2locationqueue.TryDequeue(out idandip);   //取出id、ip信息键值对
                    Location location = new Location(idandip.Value);  //通过IP获取地理位置
                    if (location.country_name != null)  //成功读出数据库信息的话
                    {
                        if(!dictOfIdAndArea.ContainsKey(idandip.Key))  //id、ip字典中不包含该项的话
                        { 
                        if (false == dictOfIdAndArea.TryAdd(idandip.Key, location)) //进行添加
                            { 
                            MessageBox.Show("IPdictionary添加失败  \n IPdictionary update failed ");
                            this.BeginInvoke((Action)(() =>                //界面中几个下拉框中添加id信息
                            {
                               if(!comboBoxForChooseInOnlineDev.Items.Contains(idandip.Key))
                                {
                                    comboBoxForChooseInOnlineDev.Items.Add(idandip.Key);
                                }
                                //comboBoxForChooseInOnlineDev.Items.Add(idandip.Key);
                                if (!comboBoxForFindInStateFind.Items.Contains(idandip.Key))
                                {
                                    comboBoxForFindInStateFind.Items.Add(idandip.Key);
                                }
                                if (!comboBoxForFindInWarnFind.Items.Contains(idandip.Key))
                                {
                                    comboBoxForFindInWarnFind.Items.Add(idandip.Key);
                                }
                                if (!comboBoxForChooseInWarn.Items.Contains(idandip.Key))
                                {
                                    comboBoxForChooseInWarn.Items.Add(idandip.Key);
                                }
                                if (!comboBoxofidForRepair.Items.Contains(idandip.Key))
                                {
                                    comboBoxofidForRepair.Items.Add(idandip.Key);
                                }
                                if (!comboBoxForChooseInWarnCount.Items.Contains(idandip.Key))
                                {
                                    comboBoxForChooseInWarnCount.Items.Add(idandip.Key);
                                }
                                
                               
                               
                              
                                
                            }));
                            }
                           //将叉车的区域信息存在数据库中
                            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
                            {
                                try
                                {
                                    thisConnection.Open();
                                    string sql1 = "replace  into  " + "deviceareainfo" + "  (id , ip,country_code,country_name,region_name,city_name)";
                                    string sql2 = "  values (" + "'" + idandip.Key + "'," + "INET_ATON('" + idandip.Value + "'), '" + location.country_code + "'" + ",'" + location.country_name + "','" + location.region_name + "','" + location.city_name + "')";
                                    MySqlCommand cmd = new MySqlCommand(sql1 + sql2, thisConnection);
                                    cmd.ExecuteNonQuery();
                                    thisConnection.Close();

                                }

                                catch (MySqlException ex)
                                {
                                    thisConnection.Close();
                                  //  MessageBox.Show(ex + ex.Message);

                                }

                            }

                        }
                       
                       //对界面区域分布树状图进行更新 
                        onlineDeviceUpdate(new KeyValuePair<string, Location>(idandip.Key, location));
                    }
                

            }
           
        }
        public void ThreadSaveOnline()
        {
             while (true)
            {
                try
                {
                        List<string> SQLStringList = new List<string>();
                    //int m = 0;
                    do
                    {
                        dataStructure.device device = onlineDeviceDatabaseQueue.Dequeue();
                        //onlineDeviceDatabaseQueue.TryDequeue(out device);
                        string sql = "replace  into  " + "onlinedeviceinfo" + "  (id ,time , runningTime,hornSwitch,horn,hornGround,upBtnSwitch,upBtnContactorCoilUpper,upBtnContactorCoilDown,upBtnContactUpper,upBtnContactDown,downBtnSwitch,downBtnsolenoidvalveUpper,downBtnsolenoidvalveDown,masterContactorCoilUpper,masterContactorCoilDown,masterContactUpper,masterContactDown,arresterUpper,arresterDown,liftMotorCurrent,liftMotorTemperature,canDirectionandSpeedMode,canSpeed,canError,canLowPowerMode,canCourse,canDirectVoltage,canMotorCurrent,canMotorTemperature,hornCount,upBtnCount,upBtnContactorCount,downBtnCount,masterContactorCount,arresterCount)" + "  values (" + "'" + device.id + "','" + device.time.ToString("yyyy-MM-dd HH:mm:ss") + "','" + device.runningTime + "'" + "," + (device.hornSwitch ? "1" : "0") + "," + (device.horn ? "1" : "0") + "," + (device.hornGround ? "1" : "0") + "," + (device.upBtnSwitch ? "1" : "0") + "," + (device.upBtnContactorCoilUpper ? "1" : "0") + "," + (device.upBtnContactorCoilDown ? "1" : "0") + "," + (device.upBtnContactUpper ? "1" : "0") + "," + (device.upBtnContactDown ? "1" : "0") + "," + (device.downBtnSwitch ? "1" : "0") + "," + (device.downBtnsolenoidvalveUpper ? "1" : "0") + "," + (device.downBtnsolenoidvalveDown ? "1" : "0") + "," + (device.masterContactorCoilUpper ? "1" : "0") + "," + (device.masterContactorCoilDown ? "1" : "0") + "," + (device.masterContactUpper ? "1" : "0") + "," + (device.masterContactDown ? "1" : "0") + "," + (device.arresterUpper ? "1" : "0") + "," + (device.arresterDown ? "1" : "0") + "," + device.liftMotorCurrent + "," + device.liftMotorTemperature + "," + device.canDirectionandSpeedMode + "," + device.canSpeed + "," + device.canError + "," + device.canLowPowerMode + "," + device.canCourse + "," + device.canDirectVoltage + "," + device.canMotorCurrent + "," + device.canMotorTemperature + "," + device.hornCount + "," + device.upBtnCount + "," + device.upBtnContactorCount + "," + device.downBtnCount + "," + device.masterContactorCount + "," + device.arresterCount + ")";
                        SQLStringList.Add(sql);

                    } while (onlineDeviceDatabaseQueue.Count != 0);


                        using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
                        {
                            try
                            {
                                thisConnection.Open();
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = thisConnection;
                                MySqlTransaction tx = thisConnection.BeginTransaction();
                                cmd.Transaction = tx;
                                try
                                {
                                    for (int n = 0; n < SQLStringList.Count; n++)
                                    {
                                        string strsql = SQLStringList[n].ToString();
                                        if (strsql.Trim().Length > 1)
                                        {
                                            cmd.CommandText = strsql;
                                            cmd.ExecuteNonQuery();
                                        }
                                        //后来加上的  
                                        if (n >= 0 && (n % 500 == 0 || n == SQLStringList.Count - 1))
                                        {
                                            tx.Commit();
                                            tx = thisConnection.BeginTransaction();
                                        }
                                    }
                                    //tx.Commit();//原来一次性提交  
                                }
                                catch (System.Data.SqlClient.SqlException E)
                                {
                                    // tx.Rollback();
                                    throw new Exception(E.Message);
                                }
                            }
                            catch (Exception e)
                            {
                                thisConnection.Close();
                            }
                        }

                    
                }
                catch (Exception e)
                {
                    //  MessageBox.Show(e.ToString());
                }
                finally
                {

                }
            }
        }
        /// <summary>
        /// 数据更新
        /// </summary>
        public void ThreadUpdate()
        {
            while (true)
            {
                try
                {


                    
                        dataStructure.device device=onlineDeviceDataQueue.Dequeue();

                        //onlineDeviceDataQueue.TryDequeue(out device);
                        try
                        {

                            if (!onlineDeviceNowTimeData.ContainsKey(device.id))   //是否存在该ID的叉车
                            {
                                onlineDeviceNowTimeData.TryAdd(device.id, device);
                                this.BeginInvoke((Action)(() =>
                                {
                                    if (!comboBoxForChooseInOnlineDev.Items.Contains(device.id))
                                    {
                                        comboBoxForChooseInOnlineDev.Items.Add(device.id);
                                    }
                                    // comboBoxForChooseInOnlineDev.Items.Add(device.id);
                                    if (!comboBoxForFindInStateFind.Items.Contains(device.id))
                                    {
                                        comboBoxForFindInStateFind.Items.Add(device.id);
                                    }
                                    if (!comboBoxForFindInWarnFind.Items.Contains(device.id))
                                    {
                                        comboBoxForFindInWarnFind.Items.Add(device.id);
                                    }
                                    if (!comboBoxForChooseInWarn.Items.Contains(device.id))
                                    {
                                        comboBoxForChooseInWarn.Items.Add(device.id);
                                    }
                                    if (!comboBoxofidForRepair.Items.Contains(device.id))
                                    {
                                        comboBoxofidForRepair.Items.Add(device.id);
                                    }
                                    if (!comboBoxForChooseInWarnCount.Items.Contains(device.id))
                                    {
                                        comboBoxForChooseInWarnCount.Items.Add(device.id);
                                    }

                                }));
                            }
                            else
                            {
                                onlineDeviceNowTimeData.TryUpdate(device.id, device, onlineDeviceNowTimeData[device.id]);



                            }
                        }
                        catch (ArgumentNullException e)
                        {
                            MessageBox.Show("ArgumentNullException" + e);
                        }

                    
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {

                }

            }
        }
        public void ThreadSaveError()
        {
            while (true)
            {
                try
                {
                        List<string> SQLStringList = new List<string>();
                        //int m = 0;
                        do
                        {
                            dataStructure.WarnInfo errorDevice= errorDeviceDataQueue.Dequeue();
                            string sql1 = "replace  into  " + "warninfo_" + int.Parse(errorDevice.id.Substring(10, 2)) % 10 + "  (id ,errorNumber, errorTime,errorValue,errorLevel,errorState)" + "  values (" + "'" + errorDevice.id + "','" + errorDevice.errorNumber.ToString() + "','" + errorDevice.time.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + errorDevice.errorValue + "," + errorDevice.errorLevel + "," + errorDevice.errorState + ")";
                            SQLStringList.Add(sql1);


                        } while (errorDeviceDataQueue.Count!=0);

                            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
                        {
                            try
                            {
                                thisConnection.Open();
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = thisConnection;
                                MySqlTransaction tx = thisConnection.BeginTransaction();
                                cmd.Transaction = tx;
                                try
                                {
                                    for (int n = 0; n < SQLStringList.Count; n++)
                                    {
                                        string strsql = SQLStringList[n].ToString();
                                        if (strsql.Trim().Length > 1)
                                        {
                                            cmd.CommandText = strsql;
                                            cmd.ExecuteNonQuery();
                                        }
                                        //后来加上的  
                                        if (n >= 0 && (n % 500 == 0 || n == SQLStringList.Count - 1))
                                        {
                                            tx.Commit();
                                            tx = thisConnection.BeginTransaction();
                                        }
                                    }
                                    //tx.Commit();//原来一次性提交  
                                }
                                catch (System.Data.SqlClient.SqlException E)
                                {
                                    // tx.Rollback();
                                    throw new Exception(E.Message);
                                }
                            }
                            catch (Exception e)
                            {
                                thisConnection.Close();
                            }
                        }


                    
                }
                catch (Exception e)
                {
                    //  MessageBox.Show(e.ToString());
                }
                finally
                {

                }
            }
        }
        /// <summary>
        /// 储存数据线程
        /// </summary>
        public void ThreadSaveState()
        {
            while (true)
            {
                try
                {

                        List<string> SQLStringList = new List<string>();  //需要存储的事务列表
                        //int m = 0;
                    do
                    {
                        dataStructure.device device = deviceStateSaveQueue.Dequeue();
                        //deviceStateSaveQueue.TryDequeue(out device); //取出字符串元素
                        string sql = "replace  into  " + "deviceinfo_" + int.Parse(device.id.Substring(10, 2)) % 100 + "  (id ,time , runningTime,hornSwitch,horn,hornGround,upBtnSwitch,upBtnContactorCoilUpper,upBtnContactorCoilDown,upBtnContactUpper,upBtnContactDown,downBtnSwitch,downBtnsolenoidvalveUpper,downBtnsolenoidvalveDown,masterContactorCoilUpper,masterContactorCoilDown,masterContactUpper,masterContactDown,arresterUpper,arresterDown,liftMotorCurrent,liftMotorTemperature,canDirectionandSpeedMode,canSpeed,canError,canLowPowerMode,canCourse,canDirectVoltage,canMotorCurrent,canMotorTemperature,hornCount,upBtnCount,upBtnContactorCount,downBtnCount,masterContactorCount,arresterCount)" + "  values (" + "'" + device.id + "','" + device.time.ToString("yyyy-MM-dd HH:mm:ss") + "','" + device.runningTime + "'" + "," + (device.hornSwitch ? "1" : "0") + "," + (device.horn ? "1" : "0") + "," + (device.hornGround ? "1" : "0") + "," + (device.upBtnSwitch ? "1" : "0") + "," + (device.upBtnContactorCoilUpper ? "1" : "0") + "," + (device.upBtnContactorCoilDown ? "1" : "0") + "," + (device.upBtnContactUpper ? "1" : "0") + "," + (device.upBtnContactDown ? "1" : "0") + "," + (device.downBtnSwitch ? "1" : "0") + "," + (device.downBtnsolenoidvalveUpper ? "1" : "0") + "," + (device.downBtnsolenoidvalveDown ? "1" : "0") + "," + (device.masterContactorCoilUpper ? "1" : "0") + "," + (device.masterContactorCoilDown ? "1" : "0") + "," + (device.masterContactUpper ? "1" : "0") + "," + (device.masterContactDown ? "1" : "0") + "," + (device.arresterUpper ? "1" : "0") + "," + (device.arresterDown ? "1" : "0") + "," + device.liftMotorCurrent + "," + device.liftMotorTemperature + "," + device.canDirectionandSpeedMode + "," + device.canSpeed + "," + device.canError + "," + device.canLowPowerMode + "," + device.canCourse + "," + device.canDirectVoltage + "," + device.canMotorCurrent + "," + device.canMotorTemperature + "," + device.hornCount + "," + device.upBtnCount + "," + device.upBtnContactorCount + "," + device.downBtnCount + "," + device.masterContactorCount + "," + device.arresterCount + ")";
                        SQLStringList.Add(sql); //添加到事务列表中

                    } while (deviceStateSaveQueue.Count != 0);  //需要存储的队列已为空或者已经有1000条需要存储的信息

                            //打开数据库连接，进行存储
                            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
                        {
                            try { thisConnection.Open();
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = thisConnection;
                                MySqlTransaction tx = thisConnection.BeginTransaction();
                                cmd.Transaction = tx;
                                try
                                {
                                    for (int n = 0; n < SQLStringList.Count; n++)  //遍历列表中元素
                                    {
                                        string strsql = SQLStringList[n].ToString();
                                        if (strsql.Trim().Length > 1)       //加入事务中 
                                        {
                                            cmd.CommandText = strsql;
                                            cmd.ExecuteNonQuery();
                                        }
                                        //后来加上的  
                                        if (n >= 0 && (n % 500 == 0 || n == SQLStringList.Count - 1)) //500个或虽然没有到500，但包含所有的话就提交事务
                                        {
                                            tx.Commit();
                                            tx = thisConnection.BeginTransaction();
                                        }
                                    }
                                    //tx.Commit();//原来一次性提交  
                                }
                                catch (System.Data.SqlClient.SqlException E)
                                {
                                    // tx.Rollback();
                                    throw new Exception(E.Message);
                                }
                            }catch(Exception e)
                            {
                                thisConnection.Close();
                            }
                        }  



                    
                    
                }
                catch (Exception e)
                {
                  //  MessageBox.Show(e.ToString());
                }
                finally
                {

                }


            }


        }
        
        bool IsContainsAll(List<DateTime> ListA, List<DateTime> ListB)
        {
            return ListB.All(b => ListA.Any(a =>a==b));
        }
        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="lpBuffer"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public static int CheckSum(string lpBuffer, int nLen)
        {
            int ucCheckVal = 0;


            for (int i = 0; i < nLen; ++i) //按位进行异或
            {
                ucCheckVal ^= lpBuffer[i];
            }
            return ucCheckVal; //返回校验结果
        }

        public static string GetHexChar(int value)
        {
            string sReturn = string.Empty;
            switch (value)  //10：A ，一一对应转换，小于10直接转换
            {
                case 10:
                    sReturn = "A";
                    break;
                case 11:
                    sReturn = "B";
                    break;
                case 12:
                    sReturn = "C";
                    break;
                case 13:
                    sReturn = "D";
                    break;
                case 14:
                    sReturn = "E";
                    break;
                case 15:
                    sReturn = "F";
                    break;
                default:
                    sReturn = value.ToString();
                    break;
            }
            return sReturn;
        }

        public static string intTostring( int ch_double, int dataLenth)
        {
            int shift_i, remend = ch_double, temp1, temp2;
            string Value = null;
            for (shift_i = 0; shift_i <= dataLenth - 1; shift_i++)
            {
                temp1 = (int)Math.Pow(16, (dataLenth - 1 - shift_i));
                temp2 = remend / temp1;
                Value = Value + GetHexChar(temp2);
                remend = remend % temp1;
            }
            return Value;
        }
        public static int stringToInt(string data, int len)
        {
            int nRet = 0;
            for (int i = 0; i < len; ++i)  //len次循环
            {
                nRet <<= 4;  //左移4位，相当于*16
                nRet += CharToInt(data[i]); //加到原结果上
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
        /// <summary>
        /// 更新地区分布列表
        /// </summary>
        private void onlineDeviceUpdate(KeyValuePair<string, Location> item)
        {
            ///数据库中数据不规则 ，有的是广西，有的是广西省，故作如下调整
          
           

         
             
                    if (onlineDevicetreeView.Nodes.ContainsKey(item.Value.country_name))  //是否存在该国家的节点
                    {
                        if (!onlineDevicetreeView.Nodes[item.Value.country_name].Nodes.ContainsKey(item.Value.region_name)) //没有该省份的情况下添加省份、城市
                        {
                            this.Invoke((Action)(() =>
                            {

                                TreeNode regionNode = new TreeNode(item.Value.region_name);
                                regionNode.Name = item.Value.region_name;
                                onlineDevicetreeView.Nodes[item.Value.country_name].Nodes.Add(regionNode);
                                TreeNode cityNode = new TreeNode(item.Value.city_name);
                                cityNode.Name = item.Value.city_name;
                                onlineDevicetreeView.Nodes[item.Value.country_name].Nodes[item.Value.region_name].Nodes.Add(cityNode);
                            }));

                        }else
                {
                    //不包含该城市，包含（该国家和省份）
                    if (!onlineDevicetreeView.Nodes[item.Value.country_name].Nodes[item.Value.region_name].Nodes.ContainsKey(item.Value.city_name))
                    {
                        this.Invoke((Action)(() =>
                        {

                         
                            TreeNode cityNode = new TreeNode(item.Value.city_name);
                            cityNode.Name = item.Value.city_name;
                            onlineDevicetreeView.Nodes[item.Value.country_name].Nodes[item.Value.region_name].Nodes.Add(cityNode);
                        }));

                    }
                }


                    }
                    else  //不存在该国家节点
                    {
                        this.Invoke((Action)(() =>
                        {

                            TreeNode countryNode = new TreeNode(item.Value.country_name);
                             countryNode.Name = item.Value.country_name;
                            onlineDevicetreeView.Nodes.Add(countryNode);
                            TreeNode regionNode = new TreeNode(item.Value.region_name);
                            regionNode.Name = item.Value.region_name;
                            onlineDevicetreeView.Nodes[item.Value.country_name].Nodes.Add(regionNode);
                            TreeNode cityNode = new TreeNode(item.Value.city_name);
                            cityNode.Name = item.Value.city_name;
                            onlineDevicetreeView.Nodes[item.Value.country_name].Nodes[item.Value.region_name].Nodes.Add(cityNode);
                        }));

                    }

        }

      

     

        

 
        private void chooseOnlineDev_Click(object sender, EventArgs e)
        {

            onlineDevicetreeView.SelectedNode = null;
            string textForFind = comboBoxForChooseInOnlineDev.Text;   //获取文本框内容
            
            if (idChecked.Checked) //是否是按编号查询
            {
                if (textForFind == "" || textForFind.Length != 12)   //编号不能为空，且长度为12位
                {
                    MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                    return;
                }

                int number; 

                bool result = Int32.TryParse(textForFind.Substring(10, 2), out number);  //是否是整数
                if (!result)
                {
                    MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                    return;
                }
                string id = textForFind;    
                 var selected = (from itemofdevice in dictOfIdAndArea where itemofdevice.Key == id  select itemofdevice.Key).ToList(); //查询dictOfIdAndArea字典
                if (selected.Count == 1) //存在结果，在在线列表中进行显示
                {
                    onlineDeviceInChooseArea.BeginUpdate();
                    onlineDeviceInChooseArea.Items.Clear();
                    int i = 0, j = 0;
                    foreach (var deviceId in selected)     
                    {
                        ListViewItem item = new ListViewItem((j + 1).ToString());  //每列的各栏目值进行赋值
                        item.SubItems.Add(deviceId);
                        item.SubItems.Add(onlineDeviceNowTimeData[ deviceId].runningTime);
                      item.SubItems.Add(dictOfIdAndArea[deviceId].city_name + " , " + dictOfIdAndArea[deviceId].region_name + " , " + dictOfIdAndArea[deviceId].country_name);
                        j++;
                        if (i % 2 == 0)     //奇偶数列不同颜色
                        {
                            item.BackColor = Color.DimGray;
                        }
                        i++;
                        onlineDeviceInChooseArea.Items.Add(item);



                    }

                    
                   
                    onlineDeviceInChooseArea.EndUpdate();
                }
                else
                {
                    MessageBox.Show("该叉车并不在线  \n the truck isn't online");
                }
            }
            if (areaChecked.Checked)  //按区域查询
            {
                //查询dictOfIdAndArea字典
                var selected = (from item in dictOfIdAndArea where item.Value.country_name.Contains(textForFind)|| item.Value.region_name.Contains(textForFind)|| item.Value.city_name.Contains(textForFind)
                     orderby item.Key   select item.Key).ToList();
                if (selected.Count >= 1) //存在查询结果，以下同上，故不赘述
                {
                    onlineDeviceInChooseArea.BeginUpdate();
                    onlineDeviceInChooseArea.Items.Clear();
                    int i = 0, j = 0;
                    foreach (var deviceId in selected)  
                    {
                        ListViewItem item = new ListViewItem((j + 1).ToString());
                        item.SubItems.Add(deviceId);
                        if (onlineDeviceNowTimeData.ContainsKey(deviceId))
                        { 
                        item.SubItems.Add(onlineDeviceNowTimeData[deviceId].runningTime);
                        }
                        else
                        {
                            item.SubItems.Add("***hour***minute");
                        }
                        item.SubItems.Add(dictOfIdAndArea[deviceId].city_name + " , " + dictOfIdAndArea[deviceId].region_name + " , " + dictOfIdAndArea[deviceId].country_name);
                        j++;
                        if (i % 2 == 0)
                        {
                            item.BackColor = Color.DimGray;
                        }
                        i++;
                        onlineDeviceInChooseArea.Items.Add(item);
                    }
                
                    onlineDeviceInChooseArea.EndUpdate();
   
                }
                else
                {
                    MessageBox.Show("该地区没有在线叉车，请重新检查查询地区\n  no truck in this area, please check it before query");
                }
            }
        
        }

        private void lastFourIdinOnlineDev_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chooseOnlineDev_Click(null, null);
            }
        }
       


       
        /// <summary>
        /// 故障报警时钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void warningTimer_Tick(object sender, EventArgs e)
        {

            if (btnFaultWarn.BackColor == Color.Red)
            { btnFaultWarn.BackColor = Color.Silver; }
            else { btnFaultWarn.BackColor = Color.Red; }
        }





        string lastchoosed = "";

        private void btnStateHistoryFind_Click(object sender, EventArgs e)
        {
            //id完整性检查
            if (comboBoxForFindInStateFind.Text == "" || comboBoxForFindInStateFind.Text.Length != 12) 
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }
            int number;
            bool result = Int32.TryParse(comboBoxForFindInStateFind.Text.Substring(10, 2), out number);
            if (!result)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }
            ////////////////////ChartArea1属性设置///////////////////////////
            //设置网格的颜色
            theChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
                    //设置坐标轴名称
                    if(language=="en")
                {
                    theChart.ChartAreas["ChartArea1"].AxisX.Title = "Time";
                }
                else
                {
                    theChart.ChartAreas["ChartArea1"].AxisX.Title = "时间";
                }                
                    theChart.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("微软雅黑", 12);
                   
                    theChart.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("微软雅黑", 12);
                    //启用3D显示
                    theChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                    

                    //////////////////////Series属性设置///////////////////////////
                    //设置显示类型-线型
                    theChart.Series["Series1"].ChartType = SeriesChartType.Point;

                    //设置坐标轴Value显示类型
                  
                  theChart.Series["Series1"].XValueType = ChartValueType.DateTime;
                    theChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "MM-dd HH:mm:ss";
                    //是否显示标签的数值
                   theChart.Series["Series1"].IsValueShownAsLabel = false;
                    
                    //设置标记图案
                    theChart.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
                    theChart.Series["Series1"].MarkerColor = Color.Red;
                    theChart.Series["Series1"].MarkerSize = 8;
                 //设置图案颜色
                 theChart.Series["Series1"].Color = Color.Green;
                    //设置图案的宽度
                    theChart.Series["Series1"].BorderWidth = 1;
                  
        
                //获取查询日期范围。
                string strbegin = beginMonthTimeInStateFind.Value.ToString("yyyy-MM-dd")+" "+ beginMinuteTimeInStateFind.Value.ToString("HH: mm");
                DateTime dtbegin = DateTime.Parse(strbegin);
                string strend = endMonthTimeInStateFind.Value.ToString("yyyy-MM-dd") + " " + endMinuteTimeInStateFind.Value.ToString("HH: mm");
                DateTime dtend = DateTime.Parse(strend);
            //查询数据库
            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
            {
                try
                {
                    thisConnection.Open(); //打开数据库连接
            

            if (selectStateName=="控制电路" )  
                    {
                       
                        return;
                    }
                                     
                    else if (selectStateName == "转速")   //查询项选择转速
                    {
                        foreach (var series in theChart.Series) //图表清空
                        {
                            series.Points.Clear();
                        }
                        //图表属性设置
                        theChart.Titles.Clear();                  
                        theChart.Titles.Add(selectNodeText);
                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                        string SQLString = "select time,canSpeed from deviceinfo_" + int.Parse  (comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 +" where time>=' "+ dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '"+ dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");  //查询，将结果放在find中
                        foreach(DataRow row in ds.Tables["find"].Rows) //将查询结果添加到图表chart中
                        {
                            theChart.Series["Series1"].Points.AddXY(row["time"], row["canSpeed"]);
                        }
                        lastchoosed = "转速";
                        return;
                }
                    
                    if (selectStateName == "小计里程")
                    {
                        foreach (var series in theChart.Series)
                        {
                            series.Points.Clear();
                        }
                        theChart.Titles.Clear();
                        theChart.Titles.Add(selectNodeText);
                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                        string SQLString = "select time,canCourse from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            theChart.Series["Series1"].Points.AddXY(row["time"], row["canCourse"]);
                        }
                        lastchoosed = "小计里程";
                        return;
                    }
                     if (selectStateName == "电机电压")
                    {
                        foreach (var series in theChart.Series)
                        {
                            series.Points.Clear();
                        }
                        theChart.Titles.Clear();
                        theChart.Titles.Add(selectNodeText);
                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                        string SQLString = "select time,canDirectVoltage from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            theChart.Series["Series1"].Points.AddXY(row["time"], row["canDirectVoltage"]);
                        }

                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.canDirectVoltage);

                        //}
                        lastchoosed = "电机电压";
                         return;
                }
                    if (selectStateName == "电机电流")
                    {
                         foreach (var series in theChart.Series)
                        {
                            series.Points.Clear();
                        }
                        theChart.Titles.Clear();
                        theChart.Titles.Add(selectNodeText);
                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                        string SQLString = "select time,canMotorCurrent from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where id="+ comboBoxForFindInStateFind.Text + " and time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            theChart.Series["Series1"].Points.AddXY(row["time"], row["canMotorCurrent"]);
                        }
                    //foreach (var dev in selectList)
                    //{
                    //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.canMotorCurrent);

                    //}
                        lastchoosed = "电机电流";
                            return;
                    }
                    else if (selectStateName == "电机温度" ||lastchoosed=="")
                    {
                                        if(lastchoosed=="")
                                {
                                    foreach (var series in theChart.Series)
                                    {
                                        series.Points.Clear();
                                    }
                                    theChart.Titles.Clear();
                                    if (language=="en")
                                    {
                                        theChart.Titles.Add("Motor Temperature");
                                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                                     }
                                    else
                                    {
                                        theChart.Titles.Add("电机温度");
                                         theChart.Titles[0].Font = new Font("微软雅黑", 12);
                                     }

                                }   else
                                    {

                                        foreach (var series in theChart.Series)
                                        {
                                            series.Points.Clear();
                                        }
                                        theChart.Titles.Clear();
                                        theChart.Titles.Add(selectNodeText);
                                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                                      }
                    string SQLString = "select time,canMotorTemperature from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        MySqlCommand myCmd = thisConnection.CreateCommand();
                        myCmd.CommandText = SQLString;
                        MySqlDataReader sdr = myCmd.ExecuteReader(CommandBehavior.CloseConnection);
                        theChart.Series["Series1"].Points.DataBindXY(sdr, "time", sdr, "canMotorTemperature");
                        //DataSet ds = new DataSet();
                        //MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        //command.Fill(ds, "find");
                        //foreach (DataRow row in ds.Tables["find"].Rows)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(row["time"], row["canMotorTemperature"]);
                        //}
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.canMotorTemperature);

                        //}
                        lastchoosed = "电机温度";
                            return;
                    }
                    else if (selectStateName == "提升电机电流")
                    {
                        foreach (var series in theChart.Series)
                        {
                            series.Points.Clear();
                        }
                        theChart.Titles.Clear();
                        theChart.Titles.Add(selectNodeText);
                        theChart.Titles[0].Font = new Font("微软雅黑", 12);
                        string SQLString = "select time,liftMotorCurrent from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            theChart.Series["Series1"].Points.AddXY(row["time"], row["liftMotorCurrent"]);
                        }
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.canMotorTemperature);

                        //}
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.liftMotorCurrent);

                        //}
                        lastchoosed = "提升电机电流";
                        return;
                }
                    else if (selectStateName == "提升电机温度")
                    {
                        foreach (var series in theChart.Series)
                        {
                            series.Points.Clear();
                        }
                        theChart.Titles.Clear();
                        theChart.Titles.Add(selectNodeText);
                      theChart.Titles[0].Font = new Font("微软雅黑", 12);
                        string SQLString = "select time,liftMotorTemperature from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            theChart.Series["Series1"].Points.AddXY(row["time"], row["liftMotorTemperature"]);
                        }
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.liftMotorTemperature);

                        //}
                        lastchoosed = "提升电机温度";
                        return;
                    }
                    else if(selectStateName == "喇叭正极") //查询项为 喇叭正极 
                    {
                            digitalName.Text = selectNodeText;
                        
                            string SQLString = "select time,hornSwitch from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                            DataSet ds = new DataSet();
                            MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                            command.Fill(ds, "find");
                            if (ds.Tables["find"].Rows.Count == 0) return;
                            value.Text = ((ds.Tables["find"].Rows[0]["hornSwitch"].ToString()=="true") ? 1: 0).ToString(); //显示喇叭状态
                            Times.Text = "";
                                return;
                    }
                    //查询返回动作次数
                    else if (selectStateName == "喇叭开关")
                    {
                        digitalName.Text = selectNodeText;
                        value.Text = "";
                        string SQLString = "select time,hornCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                           times+= int.Parse(row["hornCount"].ToString());
                        }
                   
                        Times.Text = times.ToString();  //次数累计
                   
                            return;
                    }
                    else if (selectStateName == "喇叭负极")
                    {
                          digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].hornGround ? 1 : 0).ToString();
                        string SQLString = "select time,hornGround from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["hornGround"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";
                    
                        return;
                    }
                    else if (selectStateName == "提升开关正极")
                    {
                       
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].upBtnSwitch ? 1 : 0).ToString();
                        string SQLString = "select time,upBtnSwitch from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["upBtnSwitch"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";


                        return;
                    }
                    //动作2
                    else if (selectStateName == "提升开关")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "上升接触器线圈进线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.upBtnContactorCoilUpper);

                        //}
                        digitalName.Text = selectNodeText;
                        value.Text = "";
                        string SQLString = "select time,upBtnCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            times += int.Parse(row["upBtnCount"].ToString()) ;
                        }

                        Times.Text = times.ToString();

                  
                    //foreach (var dev in selectList)
                    //{

                    //    if (dev.upBtnContactorCoilUpper)
                    //    {
                    //        times++;
                    //    }

               
                   
                         return;
                    }
                    else if (selectStateName == "提升接触器线圈接地端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "上升接触器线圈出线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.upBtnContactorCoilDown);

                        //}
                        digitalName.Text = selectNodeText;
                        string SQLString = "select time,upBtnContactorCoilDown from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["upBtnContactorCoilDown"].ToString() == "true") ? 1 : 0).ToString();
                   
                        //value.Text = (selectList[0].upBtnContactorCoilDown ? 1 : 0).ToString();
                        Times.Text = "";
                    
                         return;
                    }
                    else if (selectStateName == "提升触点进线端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "上升触点进线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.upBtnContactUpper);

                            //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].upBtnContactUpper ? 1 : 0).ToString();
                        string SQLString = "select time,upBtnContactUpper from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["upBtnContactUpper"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";

                  
                        return;
                    }
                    //动作3
                    else if (selectStateName == "提升接触器")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "上升触点出线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.upBtnContactDown);

                        //}
                        digitalName.Text = selectNodeText;
                        value.Text = "";
                    
                        string SQLString = "select time,upBtnContactorCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            times += int.Parse(row["upBtnContactorCount"].ToString());
                          
                        }

                        Times.Text = times.ToString();

                        //int times = 0;
                        //foreach (var dev in selectList)
                        //{

                        //    if (dev.upBtnContactDown)
                        //    {
                        //        times++;
                        //    }
                        //}


                   
                        return;
                    }
                    else if (selectStateName == "下降开关正极")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "下降开关";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.downBtnSwitch);

                            //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].downBtnSwitch ? 1 : 0).ToString();
                        string SQLString = "select time,downBtnSwitch from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["downBtnSwitch"].ToString() == "true") ? 1 : 0).ToString();
                         Times.Text = "";
                   
                        return;
                    }
                    //动作4
                    else if (selectStateName == "下降开关")
                    {
                  
                        digitalName.Text = selectNodeText;
                  
                        value.Text = "";

                        string SQLString = "select time,downBtnCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                           
                            times += int.Parse(row["downBtnCount"].ToString());
                        }

                  
                        //foreach (var dev in selectList)
                        //{

                        //    if (dev.downBtnsolenoidvalveUpper)
                        //    {
                        //        times++;
                        //    }


                        //}
                        Times.Text = times.ToString();
                         return;
                    }
                    else if (selectStateName == "下降电磁阀接地端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "下降电磁阀地线";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.downBtnsolenoidvalveDown);

                        //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].downBtnsolenoidvalveDown ? 1 : 0).ToString();
                        string SQLString = "select time,downBtnsolenoidvalveDown from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["downBtnsolenoidvalveDown"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";
                   
                        return;
                    }
                    else if (selectStateName == "主接触器线圈进线端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "主接触器线圈进线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.masterContactorCoilUpper);

                        //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].masterContactorCoilUpper ? 1 : 0).ToString();
                        string SQLString = "select time,masterContactorCoilUpper from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["masterContactorCoilUpper"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";
                        return;
                    }
                    else if (selectStateName == "主接触器线圈接地端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "主接触器线圈出线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.masterContactorCoilDown);

                        //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].masterContactorCoilDown ? 1 : 0).ToString();
                        string SQLString = "select time,masterContactorCoilDown from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["masterContactorCoilDown"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";
                   

                 
                        return;
                    }
                    else if (selectStateName == "主接触器触点进线端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "主接触器触点进线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.masterContactUpper);

                        //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].masterContactUpper ? 1 : 0).ToString();
                        string SQLString = "select time,masterContactUpper from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["masterContactUpper"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";
                    

                   
                        return;
                    }
                    else if (selectStateName == "主接触器触点出线端")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "主接触器触点出线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.masterContactDown);

                            //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].masterContactDown ? 1 : 0).ToString();
                        string SQLString = "select time,masterContactDown from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["masterContactDown"].ToString() == "true") ? 1 : 0).ToString();                  
                        Times.Text = "";

                 
                        return;
                    }
                    else if (selectStateName == "主接触器")
                    {

                        digitalName.Text = selectNodeText;
                        value.Text = "";


                        string SQLString = "select time,masterContactorCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            times += int.Parse(row["masterContactorCount"].ToString());
                        }


                        //}
                        Times.Text = times.ToString();

                        //foreach (var dev in selectList)
                        //{

                        //    if (dev.arresterUpper)
                        //    {
                        //        times++;
                        //    }


                        //}



                        return;
                    }
                    else if (selectStateName == "制动器线圈控制端")
                    {
                   
                        digitalName.Text = selectNodeText;
                        value.Text = "";


                        string SQLString = "select time,arresterCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            times += int.Parse(row["arresterCount"].ToString());
                        }


                        //}
                        Times.Text = times.ToString();
                   
                        //foreach (var dev in selectList)
                        //{

                        //    if (dev.arresterUpper)
                        //    {
                        //        times++;
                        //    }


                        //}
                 


                        return;
                    }
                    //动作6
                    else if (selectStateName == "制动器线圈控制端")
                    {
                   
                        digitalName.Text = selectNodeText;
                        value.Text = "";


                        string SQLString = "select time,arresterCount from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        int times = 0;
                        foreach (DataRow row in ds.Tables["find"].Rows)
                        {
                            times += int.Parse(row["arresterCount"].ToString());
                        }


                        //}
                        Times.Text = times.ToString();
                   
                        //foreach (var dev in selectList)
                        //{

                        //    if (dev.arresterUpper)
                        //    {
                        //        times++;
                        //    }


                        //}
                 


                        return;
                    }
                    else if (selectStateName == "制动器线圈正极")
                    {
                        //theChart.ChartAreas["ChartArea1"].AxisY.Title = "制动器出线端";
                        //foreach (var dev in selectList)
                        //{
                        //    theChart.Series["Series1"].Points.AddXY(dev.time, dev.arresterDown);

                        //}
                        digitalName.Text = selectNodeText;
                        //value.Text = (selectList[0].arresterDown ? 1 : 0).ToString();
                        string SQLString = "select time,arresterDown from deviceinfo_" + int.Parse(comboBoxForFindInStateFind.Text.Substring(10, 2)) % 100 + " where time>=' " + dtbegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and time<= '" + dtend.ToString("yyyy-MM-dd HH:mm:ss") + "' and id=" + comboBoxForFindInStateFind.Text + " order by time  limit 10000";
                        DataSet ds = new DataSet();
                        MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                        command.Fill(ds, "find");
                        if (ds.Tables["find"].Rows.Count == 0) return;
                        value.Text = ((ds.Tables["find"].Rows[0]["arresterDown"].ToString() == "true") ? 1 : 0).ToString();
                        Times.Text = "";
                  

                  
                        return;
                    }

            }
                catch (MySqlException ex)
            {
                if (ex.Number == 1042)
                {
                    thisConnection.Close();
                    MessageBox.Show("请检查数据库连接  \n  Please check the database connection  ");
                }
                else
                {
                    thisConnection.Close();
                    MessageBox.Show(ex + ex.Message);
                }
            }

        }
        }

    

        private void theChart_MouseMove(object sender, MouseEventArgs e)
        {

            HitTestResult Result = theChart.HitTest(e.X, e.Y); 
            if (Result.Series != null && Result.PointIndex != -1) //存在对应点
                toolTip1.Show(Result.Series.Points[Result.PointIndex].YValues[0].ToString(), theChart, e.X + 20, e.Y + 20); //显示值
            else toolTip1.Show(null, theChart);

        }

        private void btnFindInWarnFind_Click(object sender, EventArgs e)
        {
            
           //id完整性检查
            if (comboBoxForFindInWarnFind.Text == "" || comboBoxForFindInWarnFind.Text.Length != 12)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }

            int number;

            bool result = Int32.TryParse(comboBoxForFindInWarnFind.Text.Substring(10, 2), out number);
            if (!result)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }
            //建立数据库连接
            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
            {
                try
                { 
                thisConnection.Open(); //打开连接
                string SQLString = "select id,errorNumber,errorTime,errorValue,errorLevel,errorState from warninfo_" + int.Parse(comboBoxForFindInWarnFind.Text.Substring(10, 2)) % 10 + " where errorTime>=' " + beginTimeInWarnFind.Value.ToString("yyyy-MM-dd") + " 00:00:00" + "' and errorTime<= '" + endTimeInWarnFind.Value.ToString("yyyy-MM-dd") + " 23:59:59" + "' and id=" + comboBoxForFindInWarnFind.Text + " order by errorNumber, errorTime,errorState ";
                DataSet wnds = new DataSet();
                MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                command.Fill(wnds, "warnfind");  //查询到的数据放到warnfind中
                warnhistory = new List<dataStructure.faultWarnData>(); //warnhistory中的每一个元素代表一个完整的故障，在历史故障报警中显示为1行
                foreach (DataRow row in wnds.Tables["warnfind"].Rows)  //遍历数据库查询到的结果，生成历史故障报警查询的内容
                {
                    if (row["errorState"].ToString() == "0") //是否是一个故障的开始
                    {
                            //检查列表中上一个是否是为该故障码，是的话直接后面添加
                            if (warnhistory.Count != 0 && warnhistory.LastOrDefault().errorNumber.ToString() == row["errorNumber"].ToString()&& warnhistory.LastOrDefault().errorState.LastOrDefault().ToString() !="2")
                            {
                                warnhistory.Last().faultTime.Add(DateTime.Parse(row["errorTime"].ToString()));
                                warnhistory.Last().errorValue.Add(int.Parse(row["errorValue"].ToString()));
                                warnhistory.Last().errorState.Add(int.Parse(row["errorState"].ToString()));
                                warnhistory.Last().errorCount++;
                            }
                            else //不是的话建立一个新的faultWarnData元素，添加到warnhistory中
                            {
                                dataStructure.faultWarnData warnData = new dataStructure.faultWarnData();
                                warnData.faultID = row["id"].ToString();
                                warnData.faultTime = new List<DateTime>();
                                warnData.errorValue = new List<int>();
                                warnData.errorState = new List<int>();
                                warnData.faultTime.Add(DateTime.Parse(row["errorTime"].ToString()));
                                warnData.errorLevel = int.Parse(row["errorLevel"].ToString());
                                warnData.errorNumber = int.Parse(row["errorNumber"].ToString());
                                warnData.errorValue.Add(int.Parse(row["errorValue"].ToString()));
                                warnData.errorState.Add(int.Parse(row["errorState"].ToString()));
                                warnData.errorCount = 1;
                                warnhistory.Add(warnData);
                            }
                        }
                    else if (row["errorState"].ToString() == "1") //是否是一个故障的中间
                    {

                            if (warnhistory.Count != 0) //检查历史故障列表是否为空
                            {
                                //检查列表中上一个是否是为该故障码，是的话直接后面添加
                                if (warnhistory.LastOrDefault().errorNumber.ToString() == row["errorNumber"].ToString()&& warnhistory.LastOrDefault().errorState.LastOrDefault().ToString() != "2")
                                {
                                    warnhistory.Last().faultTime.Add(DateTime.Parse(row["errorTime"].ToString()));
                                    warnhistory.Last().errorValue.Add(int.Parse(row["errorValue"].ToString()));
                                    warnhistory.Last().errorState.Add(int.Parse(row["errorState"].ToString()));
                                    warnhistory.Last().errorCount++;
                                }
                                else //不是的话建立一个新的faultWarnData元素，添加到warnhistory中
                                {
                                    dataStructure.faultWarnData warnData = new dataStructure.faultWarnData();
                                    warnData.faultID = row["id"].ToString();
                                    warnData.faultTime = new List<DateTime>();
                                    warnData.errorValue = new List<int>();
                                    warnData.errorState = new List<int>();
                                    warnData.faultTime.Add(DateTime.Parse(row["errorTime"].ToString()));
                                    warnData.errorLevel = int.Parse(row["errorLevel"].ToString());
                                    warnData.errorNumber = int.Parse(row["errorNumber"].ToString());
                                    warnData.errorValue.Add(int.Parse(row["errorValue"].ToString()));
                                    warnData.errorState.Add(int.Parse(row["errorState"].ToString()));
                                    warnData.errorCount = 1;
                                    warnhistory.Add(warnData);
                                }
                            }
                            }
                    else                                     //是否是一个故障的结束
                    {
                                if(warnhistory.Count!=0)//检查历史故障列表是否为空
                            {
                                //检查列表中上一个是否是为该故障码，是的话直接后面添加
                                if (warnhistory.LastOrDefault().errorNumber.ToString() == row["errorNumber"].ToString())
                                    {
                                        warnhistory.Last().faultTime.Add(DateTime.Parse(row["errorTime"].ToString()));
                                        warnhistory.Last().errorValue.Add(int.Parse(row["errorValue"].ToString()));
                                        warnhistory.Last().errorState.Add(int.Parse(row["errorState"].ToString()));
                                        warnhistory.Last().errorCount++;
                                 }
                                else  //不是的话建立一个新的faultWarnData元素，添加到warnhistory中
                                {
                                        dataStructure.faultWarnData warnData = new dataStructure.faultWarnData();
                                        warnData.faultID = row["id"].ToString();
                                        warnData.faultTime = new List<DateTime>();
                                        warnData.errorValue = new List<int>();
                                        warnData.errorState = new List<int>();
                                        warnData.faultTime.Add(DateTime.Parse(row["errorTime"].ToString()));
                                        warnData.errorLevel = int.Parse(row["errorLevel"].ToString());
                                        warnData.errorNumber = int.Parse(row["errorNumber"].ToString());
                                        warnData.errorValue.Add(int.Parse(row["errorValue"].ToString()));
                                        warnData.errorState.Add(int.Parse(row["errorState"].ToString()));
                                        warnData.errorCount = 1;
                                        warnhistory.Add(warnData);
                                    }
                                }
                        }

                }

                var selectList = warnhistory.OrderByDescending(a => a.errorNumber); //按故障码大小进行排序
                selectList.ToList();
                warnHistoryList.Items.Clear();
                foreach (var item in selectList) //遍历selectList，添加到表格中
                {
                        //对每行的各栏目赋值
                    ListViewItem list_item = new ListViewItem(item.faultID);

                    list_item.SubItems.Add(item.faultTime.Last().ToString("MM-dd HH:mm:ss"));//xx为相应属性
                    list_item.SubItems.Add(item.errorNumber.ToString());                   
                    list_item.SubItems.Add(item.errorCount.ToString());
                    list_item.BackColor = Color.Yellow;
                    warnHistoryList.Items.Add(list_item);//将设置好的listiem添加到items中
                }

            }
                  
                catch (MySqlException ex) //捕获异常
            {
                if (ex.Number == 1042)
                {
                    thisConnection.Close();
                    MessageBox.Show("请检查数据库连接  \n  Please check the database connection  ");
                }
                else
                {
                    thisConnection.Close();
                    MessageBox.Show(ex + ex.Message);
                }
            }
           
            } 

        }

        public void btnRepairFind_Click(object sender, EventArgs e)
        {
            //id完整性检查
            if (comboBoxofidForRepair.Text == "" || comboBoxofidForRepair.Text.Length != 12)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }

            int number;

            bool result = Int32.TryParse(comboBoxofidForRepair.Text.Substring(10, 2), out number);
            if (!result)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }

            //查询数据库
            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
            {
                try {
                    thisConnection.Open(); //打开数据库
                    string SQLString = "select * from repairinfo" + " where id=" + comboBoxofidForRepair.Text;
                    DataSet rpds = new DataSet();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, thisConnection);
                    command.Fill(rpds, "repairfind");   //数据存放到repairfind表中
                    List<dataStructure.repairData> repairData = new List<dataStructure.repairData>();
                    foreach (DataRow row in rpds.Tables["repairfind"].Rows) //遍历该表
                    {
                        //定义repairData变量，并进行赋值
                        dataStructure.repairData repairinfo = new dataStructure.repairData();
                        repairinfo.repairID = row["id"].ToString();
                        repairinfo.repairTime = DateTime.Parse(row["repairTime"].ToString());
                        repairinfo.remark = row["remark"].ToString();
                        repairinfo.repairMan = row["repairMan"].ToString();
                        repairinfo.repairRecord = row["repairRecord"].ToString();
                        repairData.Add(repairinfo);  //添加到列表中

                    }

                    var selectList = repairData.OrderByDescending(a => a.repairTime);  //按维修时间排序
                    selectList.ToList();
                    repairList.Items.Clear();
                    foreach (var item in selectList) //遍历列表添加到表格中
                    {
                        ListViewItem list_item = new ListViewItem(item.repairID);

                        list_item.SubItems.Add(item.repairTime.ToString());//xx为相应属性
                        list_item.SubItems.Add(item.repairMan);
                        list_item.SubItems.Add(item.repairRecord);
                        list_item.SubItems.Add(item.remark);
                        repairList.Items.Add(list_item);//将设置好的listiem添加到items中
                    }


                }
                catch ( MySqlException ex) //捕获异常
                {
                    if(ex.Number==1042)
                    { 
                    thisConnection.Close();
                    MessageBox.Show("请检查数据库连接  \n  Please check the database connection  ");
                    }
                    else
                    {
                        thisConnection.Close();
                        MessageBox.Show(ex + ex.Message);
                    }
                }
                
            }
         }
        
        private void btnRepiarAdd_Click(object sender, EventArgs e)
        {
            //检查ID完整性
            if (comboBoxofidForRepair.Text == "" || comboBoxofidForRepair.Text.Length != 12)
            {
                MessageBox.Show("请检查编号长度，确保正确输入 \n Please enter the correct ID");
                return;
            }

          
            //打开一个新窗口填写维修表单
            repair repairform = new repair(comboBoxofidForRepair.Text,this,language);
            repairform.Show();

        }

       

   
        /// <summary>
        /// 根据ID查询报警信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
     

        private void onlineDevicetreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
           string selectedArea = e.Node.Text; //获取节点值
            //查询该区域的叉车
            var deviceInSelectArea = (from item in dictOfIdAndArea where item.Value.country_name.Contains(selectedArea) || item.Value.region_name.Contains(selectedArea) || item.Value.city_name.Contains(selectedArea) orderby item.Key select item.Key).ToList();
           //对在线叉车列表进行更新
            onlineDeviceInChooseArea.BeginUpdate();
            onlineDeviceInChooseArea.Items.Clear();
            int i = 0;
            foreach (var deviceId in deviceInSelectArea) //遍历
            {   
                ListViewItem item = new ListViewItem(deviceId); //对每行信息进行相应赋值
                item.SubItems.Add(onlineDeviceNowTimeData[deviceId].runningTime);
                item.SubItems.Add(dictOfIdAndArea[deviceId].city_name +" , "+ dictOfIdAndArea[deviceId].region_name + " , "+dictOfIdAndArea[deviceId].country_name);
                item.SubItems.Add(onlineDeviceNowTimeData[deviceId].time.ToString());
                item.SubItems.Add(onlineDeviceNowTimeData[deviceId].canCourse.ToString()+"公里");
                if(i%2==0) //偶数行换个颜色显示
                {
                    item.BackColor = Color.LightSlateGray;
                }
                i++;
                onlineDeviceInChooseArea.Items.Add(item); //添加到表格中
            }
            onlineDeviceInChooseArea.EndUpdate();
        }

        // Implements the manual sorting of items by columns.
        class ListViewItemComparer : IComparer
        {
            private int col;
            private SortOrder order;
            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;

            }
            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }
            public int Compare(object x, object y)
            {
                int returnVal = -1;
               

                    returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                    // Determine whether the sort order is descending.         
                    if (order == SortOrder.Descending)
                        // Invert the value returned by String.Compare.         
                        returnVal *= -1;           
                    return returnVal;         } 

            }
        private int sortColumn = -1;
        private void onlineDeviceInChooseArea_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {                 
                // Set the sort column to the new column.         
                sortColumn = e.Column;
                // Set the sort order to ascending by default.        
                onlineDeviceInChooseArea.Sorting = SortOrder.Ascending;             }
            else { 
                if (onlineDeviceInChooseArea.Sorting == SortOrder.Ascending)
                onlineDeviceInChooseArea.Sorting = SortOrder.Descending;
            else onlineDeviceInChooseArea.Sorting = SortOrder.Ascending;
           
            }
            onlineDeviceInChooseArea.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object.         
            this.onlineDeviceInChooseArea.ListViewItemSorter = new ListViewItemComparer(e.Column, onlineDeviceInChooseArea.Sorting);
            for(int i = 0; i < this.onlineDeviceInChooseArea.Items.Count; i++)

            {
                this.onlineDeviceInChooseArea.Items[i].Text = (i+1).ToString();
                if (i % 2 == 0)
                {
                    this.onlineDeviceInChooseArea.Items[i].BackColor = Color.LightSlateGray;
                }
                else
                {
                    this.onlineDeviceInChooseArea.Items[i].BackColor = Color.Silver;
                }
               
            }
        }
        private void FaultWarnList_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.         
                sortColumn = e.Column;
                // Set the sort order to ascending by default.        
                FaultWarnList.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (FaultWarnList.Sorting == SortOrder.Ascending)
                    FaultWarnList.Sorting = SortOrder.Descending;
                else FaultWarnList.Sorting = SortOrder.Ascending;

            }
            FaultWarnList.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object.         
            this.FaultWarnList.ListViewItemSorter = new ListViewItemComparer(e.Column, FaultWarnList.Sorting);
           
        }
        private void onlineDeviceInChooseArea_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            ListViewHitTestInfo info = onlineDeviceInChooseArea.HitTest(e.X, e.Y); //获得双击的位置
            var item = info.Item as ListViewItem; //获取点击的行
            string id = item.SubItems[1].Text;//获取改行ID
            if (onlineDeviceNowTimeData.ContainsKey(id)) //如果在线列表中包含该ID
            {
                deviceInfo form = new deviceInfo(onlineDeviceNowTimeData[id], this, language); //新建窗口显示
                form.Show();

            }

        }
       

        private void chooseError_Click(object sender, EventArgs e)
        {
            //获取查询的值
            string textForFind = comboBoxForChooseInWarn.Text;
           
            if (warnid.Checked) //根据id进行查询
            {
                //检查id完整性
                if (textForFind == "" || textForFind.Length != 12)
                {
                    MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                    return;
                }

                int number;

                bool result = Int32.TryParse(textForFind.Substring(10, 2), out number);
                if (!result)
                {
                    MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                    return;
                }
                //查询该项是否存在
                var li = FaultWarnList.Items.Cast<ListViewItem>().Where(x => x.Text == textForFind).ToList();
                if (li.Count!=0)//存在的话插入到最前
                {
                    FaultWarnList.BeginUpdate();
                    FaultWarnList.Sorting = SortOrder.None;
                    foreach (var item in li) //遍历 
                    { 
                   
                    FaultWarnList.Items.Remove(item); //移除
                    FaultWarnList.Items.Insert(0, item); //插入到最上面
                    }
                    FaultWarnList.EndUpdate();
                }

                else
                {
                    MessageBox.Show("该叉车并未故障报警，请重新检查查询编号！\n the truck doesn't have fault alarm, please check truck ID before query");
                }
            }
            if (warnnumber.Checked) //根据故障码进行查询，以下内容类似，故不赘述
            {
                if (textForFind == "" || textForFind.Length != 2)
                {
                    MessageBox.Show("请正确输入Error Number   \n Please enter the correct Error Number ");
                    return;
                }

                int number;

                bool result = Int32.TryParse(textForFind, out number);
                if (!result)
                {
                    MessageBox.Show("请正确输入Error Number   \n Please enter the correct Error Number ");
                    return;
                }
                var li = FaultWarnList.Items.Cast<ListViewItem>().Where(x => x.SubItems[2].Text== textForFind).ToList();
                if (li.Count != 0)
                {
                    FaultWarnList.BeginUpdate();
                    FaultWarnList.Sorting = SortOrder.None;
                    foreach (var item in li)
                    {

                        FaultWarnList.Items.Remove(item);
                        FaultWarnList.Items.Insert(0, item);
                    }
                    FaultWarnList.EndUpdate();
                }

                else
                {
                    MessageBox.Show("没有该故障代码的报警信息，请重新检查报警代码！\n the truck doesn't have fault alarm of this code, please check code before query");
                }
            }
        }

        private void FaultWarnList_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            ListViewHitTestInfo info = FaultWarnList.HitTest(e.X, e.Y); //获取点击位置
            var item = info.Item as ListViewItem; //获取点击的行
            string id = item.Text; //获取该行id 
             int errornumber =int.Parse(item.SubItems[2].Text); //获取该行故障码
            warnInfo form = new warnInfo(faultDevice[new KeyValuePair<string, int>(id,errornumber)],language); //新建窗口进行显示
            form.Show();
        }

        private void warnHistoryList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = warnHistoryList.HitTest(e.X, e.Y);
            var item = info.Item as ListViewItem;
            string id = item.Text;
            int errornumber = int.Parse(item.SubItems[2].Text);
            string time = item.SubItems[1].Text;
            var selectWarnData = (from selectone in warnhistory where time == selectone.faultTime.Last().ToString("MM-dd HH:mm:ss") && id == selectone.faultID && selectone.errorNumber == errornumber select selectone).ToList();
            if (selectWarnData.Count != 0)
            { 
            warnInfo form = new warnInfo(selectWarnData[0],language);
            form.Show();
            }
        }
        private int sortColumnInWarnHistoryList = -1;
        private void warnHistoryList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumnInWarnHistoryList)
            {
                // Set the sort column to the new column.         
                sortColumnInWarnHistoryList = e.Column;
                // Set the sort order to ascending by default.        
                warnHistoryList.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (warnHistoryList.Sorting == SortOrder.Ascending)
                    warnHistoryList.Sorting = SortOrder.Descending;
                else warnHistoryList.Sorting = SortOrder.Ascending;

            }
            warnHistoryList.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object.         
            this.warnHistoryList.ListViewItemSorter = new ListViewItemComparer(e.Column, warnHistoryList.Sorting);
        }

        private void stateTreeView_DoubleClick(object sender, EventArgs e)
        {
            btnStateHistoryFind_Click(null, null);
        }

        private void stateTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectStateName = e.Node.Name;
            selectNodeText = e.Node.Text;
        }



        /// <summary>
        /// 每一天报警信息清零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        { 
            if(istoday!=DateTime.Now.Day)
            { 
            faultDevice.Clear();
            FaultWarnList.Items.Clear();
                istoday = DateTime.Now.Day;
            }
        }

        private void onlineDevicetreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           

            
            this.BeginInvoke((Action)(  //委托给UI线程完成以下工作
                     () =>
                     {
                         string selectedArea = e.Node.Text; //获取点击区域地址名称
                         var deviceInSelectArea = (from item in dictOfIdAndArea where item.Value.country_name.Contains(selectedArea)|| item.Value.region_name.Contains(selectedArea) || item.Value.city_name.Contains(selectedArea)orderby item.Key select item.Key).ToList();
                         onlineDeviceInChooseArea.BeginUpdate();
                         onlineDeviceInChooseArea.Items.Clear();
                         int i = 0;
                     for (int j = 0; j<deviceInSelectArea.Count();j++) //遍历
                         {
                             if (onlineDeviceNowTimeData.ContainsKey(deviceInSelectArea[j]) && dictOfIdAndArea.ContainsKey(deviceInSelectArea[j]))//字典中包含该叉车
                             {
                                  //对每行叉车信息进行对应赋值
                                 ListViewItem item = new ListViewItem((j + 1).ToString());  
                                 item.SubItems.Add(deviceInSelectArea[j]);
                                 item.SubItems.Add(onlineDeviceNowTimeData[deviceInSelectArea[j]].runningTime);
                                                                 item.SubItems.Add(dictOfIdAndArea[deviceInSelectArea[j]].city_name + " ," + dictOfIdAndArea[deviceInSelectArea[j]].region_name + " ," + dictOfIdAndArea[deviceInSelectArea[j]].country_name);
                                 
                                 if (i % 2 == 0)  //偶数行换个颜色
                                 {
                                     item.BackColor = Color.DimGray;
                                 }
                                 i++;
                                 onlineDeviceInChooseArea.Items.Add(item); //添加该行
                             }
                         }
                         onlineDeviceInChooseArea.EndUpdate();


                     }));
       
        }

        private void connect_Click(object sender, EventArgs e)
        {


          if (portnumber.Text!="")//不为空的时候打开串口
            { 
                  int portnum;

                bool result = Int32.TryParse(portnumber.Text, out portnum);
                if (!result) //检查是否是数字
                {
                    MessageBox.Show("请正确输入端口号   \n Please enter the correct port");
                    return;
                }
                Properties.Settings.Default.port = portnum;
                Properties.Settings.Default.Save();
               Thread beginlisten = new Thread(ThreadTcpServer); //定义该线程

                beginlisten.IsBackground = true;//置于后台
                connect.Enabled = false; //端口设置按钮不可点击
                beginlisten.Start(portnum);//启动该线程进行监听
            }
        }

        private void systemtimer_Tick(object sender, EventArgs e)
        {
            timetext.Text = DateTime.Now.ToString();
            timetext2.Text = DateTime.Now.ToString();
        }
        

        private void splitContainer1_Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                //Return back signal
                blnMouseDown = false;
        }

        private void splitContainer1_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                //Get the current position of the mouse in the screen
                ptMouseNewPos = Control.MousePosition;
                //Set window position
                ptFormNewPos.X = ptMouseNewPos.X - ptMouseCurrrnetPos.X + ptFormPos.X;
                ptFormNewPos.Y = ptMouseNewPos.Y - ptMouseCurrrnetPos.Y + ptFormPos.Y;
                //Save window position
                Location = ptFormNewPos;
                ptFormPos = ptFormNewPos;
                //Save mouse position
                ptMouseCurrrnetPos = ptMouseNewPos;

            }
        }
        private Point ptMouseCurrrnetPos, ptMouseNewPos, ptFormPos, ptFormNewPos;

        

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit form = new Exit();
            form.ShowDialog();
            e.Cancel = true;

        }

        private bool blnMouseDown = false;
        private void splitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;
                // Save window position and mouse position
                ptMouseCurrrnetPos = Control.MousePosition;
                ptFormPos = Location;
            }
        }



        /// <summary>
        /// 按编号进行故障统计时查询该编号叉车的所有故障
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ArrayList BindA(string id)
        {
            ArrayList al = new ArrayList(); //初始化
            string sql = "select distinct errorNumber from warninfo_" + int.Parse(id.Substring(10, 2)) % 10 + "  where id = " + id + " and errorstate=0"; //查询数据库中发生过的故障
            DataSet ds = MySqlHelper.GetDataSet(sqlconstr, CommandType.Text, sql, new MySqlParameter());
            if (ds != null)
            { 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        al.Add(ds.Tables[0].Rows[j]["errorNumber"].ToString()); //添加到列表中
                    }
                }
            }
            return al; //返回该包含该叉车所有故障的list
        }
        public static ArrayList BindB(DateTimePicker dtpbeg,DateTimePicker dtpend)
        {
            ArrayList al = new ArrayList();
            for(int i=0;i<10;i++)
            { 
                string sql = "select distinct errorNumber from warninfo_"+i.ToString() + " where id>='" + dtpbeg.Value.ToString("yyyyMM")+"000000" + "' and id<= '" + dtpend.Value.ToString("yyyyMM")+"319999'" + "  and  errorstate=0"; 
                DataSet ds = MySqlHelper.GetDataSet(sqlconstr, CommandType.Text, sql, new MySqlParameter());
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (!al.Contains(ds.Tables[0].Rows[j]["errorNumber"].ToString()))
                                al.Add(ds.Tables[0].Rows[j]["errorNumber"].ToString());
                        }
                    }
                }
            }
            return al;
        }
        public static ArrayList BindC()
        {
            ArrayList al = new ArrayList();
            for (int i = 0; i < 10; i++)
            {
                string sql = "select distinct errorNumber from warninfo_" + i.ToString() + " where " + " errorstate=0";
                DataSet ds = MySqlHelper.GetDataSet(sqlconstr, CommandType.Text, sql, new MySqlParameter());
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (!al.Contains(ds.Tables[0].Rows[j]["errorNumber"].ToString()))
                                al.Add(ds.Tables[0].Rows[j]["errorNumber"].ToString());
                        }
                    }
                }
            }
            return al;
        }

 

        private void CountFind_Click(object sender, EventArgs e)
        {
            CreateSingleImage(comboBoxForChooseInWarnCount.Text, pictureBox4, pictureBox5,language);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            pnlsinglefind.Visible = true;
            pnlallfind.Visible = false;
            pnldatefind.Visible = false;
            btnCount1.Enabled = false;
            btnCount2.Enabled = true;
             btnCount3.Enabled = true;
                        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlsinglefind.Visible = false;
            pnlallfind.Visible = false;
            pnldatefind.Visible = true;
            btnCount1.Enabled = true;
            btnCount2.Enabled = false;
            btnCount3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pnlsinglefind.Visible = false;
            pnlallfind.Visible = true;
            pnldatefind.Visible = false;
            btnCount1.Enabled = true;
            btnCount2.Enabled = true;
            btnCount3.Enabled = false;
        }

        private void dateCountFind_Click(object sender, EventArgs e)
        {
            CreateDateImage(beginTimeInWarnCount, endTimeInWarnCount, pictureBox8, pictureBox9,language);
        }

        private void allCountFind_Click(object sender, EventArgs e)
        {
            CreateAllImage(pictureBox6,pictureBox7,language);
        }

        private void btnSetPrintInDateCount_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog1.ShowDialog();
        }

        private void btnPrePrintInDateCount_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void btnPrintInDateCount_Click(object sender, EventArgs e)
        {
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            Bitmap _NewBitmap = new Bitmap(pnldatefind.Width, pnldatefind.Height);
            pnldatefind.DrawToBitmap(_NewBitmap, new Rectangle(0, 0, _NewBitmap.Width, _NewBitmap.Height));
            e.Graphics.DrawImage(_NewBitmap, 0, 0, _NewBitmap.Width, _NewBitmap.Height);
        }

        private void btnSetPrintInSingleCount_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog2.ShowDialog();
        }

        private void btnPrePrintInSingleCount_Click(object sender, EventArgs e)
        {
            printPreviewDialog2.ShowDialog();
        }

        private void btnPrintInSingleCount_Click(object sender, EventArgs e)
        {
            if (this.printDialog2.ShowDialog() == DialogResult.OK)
            {
                this.printDocument2.Print();
            }
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap _NewBitmap = new Bitmap(pnlsinglefind.Width, pnlsinglefind.Height);
            pnlsinglefind.DrawToBitmap(_NewBitmap, new Rectangle(0, 0, _NewBitmap.Width, _NewBitmap.Height));
            e.Graphics.DrawImage(_NewBitmap, 0, 0, _NewBitmap.Width, _NewBitmap.Height);
        }

        private void btnSetPrintInAllCount_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog3.ShowDialog();
        }

        private void btnPrePrintInAllCount_Click(object sender, EventArgs e)
        {
            printPreviewDialog3.ShowDialog();
        }

        private void btnPrintInAllCount_Click(object sender, EventArgs e)
        {
            if (this.printDialog3.ShowDialog() == DialogResult.OK)
            {
                this.printDocument3.Print();
            }
        }

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap _NewBitmap = new Bitmap(pnlallfind.Width, pnlallfind.Height);
            pnlallfind.DrawToBitmap(_NewBitmap, new Rectangle(0, 0, _NewBitmap.Width, _NewBitmap.Height));
            e.Graphics.DrawImage(_NewBitmap, 0, 0, _NewBitmap.Width, _NewBitmap.Height);
        }

        private void clearlist_Click(object sender, EventArgs e)
        {
            faultDevice.Clear();
            FaultWarnList.Items.Clear();
        }




        //一级统计图
        public static void CreateSingleImage(string id,PictureBox pictureBox1, PictureBox pictureBox2,string language)
        {
            //检查ID完整性
            if (id== "" || id.Length != 12)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }
           
            int number;

            bool result = Int32.TryParse(id.Substring(10, 2), out number);
            if (!result)
            {
                MessageBox.Show("请正确输入ID   \n Please enter the correct ID");
                return;
            }
                ArrayList a = BindA(id);
            //把连接字串指定为一个常量
            int Total =Convert.ToInt16( MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "select count(*) from warninfo_"+ int.Parse(id.Substring(10, 2)) % 10 +" where id="+id +" and errorstate =0", new MySqlParameter()));//获取总的条目数
            #region
            //设置字体，fonttitle为主标题的字体
            Font fontlegend = new Font("宋体", 9);
            Font fonttitle = new Font("宋体", 10, FontStyle.Bold);

            //背景宽
            int width=350;
            int bufferspace = 55;
            int legendheight = fontlegend.Height * 12 + bufferspace; //高度
            int titleheight = fonttitle.Height + bufferspace;
            int height = width + legendheight + titleheight + bufferspace;//白色背景高
            int pieheight = width;
            Rectangle pierect = new Rectangle(0, titleheight, width, pieheight);

            //加上各种随机色
            ArrayList colors = new ArrayList();
            Random rnd = new Random();
            for (int i = 0; i < 50; i++)
                colors.Add(new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255))));

            //创建一个bitmap实例
            Bitmap objbitmap = new Bitmap(width, height);
            Graphics objgraphics = Graphics.FromImage(objbitmap);

            Bitmap objbitmap1 = new Bitmap(width*2, height);
            Graphics objgraphics1 = Graphics.FromImage(objbitmap1);

            //画一个白色背景
            objgraphics.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

            //画一个亮黄色背景 
            objgraphics.FillRectangle(new SolidBrush(Color.White), pierect);

            //以下为画饼图(有几行row画几个)
            float currentdegree = 0.0f;
            #endregion
            int childtype = 0;
            ArrayList coun = new ArrayList();
            for (int i = 0; i < a.Count; i++)
            {
                //子事件总数
                childtype = Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "Select count(*) From warninfo_" + int.Parse(id.Substring(10, 2)) % 10 + " Where id=" +id + " and errorNumber = "  + a[i].ToString()+ " and errorstate =0", new MySqlParameter()));
                //画子事件总数
                objgraphics.FillPie((SolidBrush)colors[i], pierect, currentdegree, Convert.ToSingle(childtype) / Total * 360);
                currentdegree += Convert.ToSingle(childtype) / Total * 360;
                coun.Add(childtype);
            }

            //以下为生成主标题
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            SolidBrush bluebrush = new SolidBrush(Color.Black);
            string title = "";
            if(language=="en")
            {
               title = "Fault Statistics Pie Chart: " + "\n \n\n";
            }
            else
            {
                 title = "故障统计饼状图: " + "\n \n\n";
            }
          
           
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            objgraphics.DrawString(title, fonttitle, blackbrush, new Rectangle(10,30, width, titleheight), stringFormat);

            //列出各字段与得数目
            objgraphics1.DrawRectangle(new Pen(Color.White, 2), 0, 0, 400, 400);

            
            if (language == "en") //如果界面选择的是英文的话，显示英文标题
            {
                objgraphics1.DrawString("---------------Fault Statistics---------------------", fontlegend, bluebrush, 0, 10);
                objgraphics1.DrawString("Truck ID: " + id, fontlegend, blackbrush, 40, 60);
                objgraphics1.DrawString("Total Number of Fault: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 100);
            }
            else //显示中文标题
            {
                objgraphics1.DrawString("---------------故障统计信息---------------------", fontlegend, bluebrush, 0, 10);
                objgraphics1.DrawString("叉车ID: " + id, fontlegend, blackbrush, 40, 60);
                objgraphics1.DrawString("故障总数: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 100);
            }
            //objgraphics1.DrawString("叉车ID: " + id, fontlegend, blackbrush, 40, 60);
           // objgraphics1.DrawString("统计年份: ", fontlegend, blackbrush, 40, 70);
            //objgraphics1.DrawString("故障总数: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 100);
            int kuandu = 100;
            int y = 100;
            int x = 40;
            for (int i = 0; i < a.Count; i++)
            {
                kuandu += 15;
                y += 15;
                int columnwidth = 300;
                int columncount = 0;
                if (language == "en")   //列出各项故障发生的次数，并用不同颜色显示图标，并计算显示比例
                {
                    columncount = i/20;
                    objgraphics1.DrawString("Fault " + a[i] + " Times: " + (coun[i]), fontlegend, blackbrush, x + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columncount * columnwidth, (i >= 20) ? y - 300 : y, 10, 10);
                    objgraphics1.DrawString("Percent: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, 180, (i >= 20) ? kuandu - 300 : kuandu);
                }
                else
                {
                    columncount = i / 20;
                    objgraphics1.DrawString("故障 " + a[i] + " 次数: " + (coun[i]), fontlegend, blackbrush, x + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columncount * columnwidth, (i >= 20) ? y - 300 : y, 10, 10);
                    objgraphics1.DrawString("所占比率: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, 180, (i >= 20) ? kuandu - 300 : kuandu);
                }
                


            }
            pictureBox1.Image = objbitmap;
            pictureBox2.Image = objbitmap1;
           
        }

        //二级统计图
        public static void CreateDateImage( DateTimePicker dtpbeg, DateTimePicker dtpend,PictureBox pictureBox1, PictureBox pictureBox2,string language)
        {
            ArrayList a = BindB(dtpbeg,dtpend);
            int Total = 0,TotalNumber=0;
            for(int i=0;i<10;i++)
            { 
            Total += Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "select count(*) from warninfo_"+i+ " where id>='" + dtpbeg.Value.ToString("yyyyMM") + "000000" + "' and id<= '" + dtpend.Value.ToString("yyyyMM") + "319999'" + "  and  errorstate=0" , new MySqlParameter()));//获取总的条目数
                TotalNumber += Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "select count(distinct id) from warninfo_" + i + " where id>='" + dtpbeg.Value.ToString("yyyyMM") + "000000" + "' and id<= '" + dtpend.Value.ToString("yyyyMM") + "319999'" + "  and  errorstate=0", new MySqlParameter()));//获取总的条目数
                if (Total == -1)
                    break;
            }
            #region

            //设置字体，fonttitle为主标题的字体
            Font fontlegend = new Font("宋体", 9);
            Font fonttitle = new Font("宋体", 10);

            //背景宽
            int width = 350;
            int bufferspace = 55;
            int legendheight = fontlegend.Height * 12 + bufferspace; //高度
            int titleheight = fonttitle.Height + bufferspace;
            int height = width + legendheight + titleheight + bufferspace;//白色背景高
            int pieheight = width;
            Rectangle pierect = new Rectangle(0, titleheight, width, pieheight);

            //加上各种随机色
            ArrayList colors = new ArrayList();
            Random rnd = new Random();
            for (int i = 0; i < 50; i++)
                colors.Add(new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255))));

            //创建一个bitmap实例
            Bitmap objbitmap = new Bitmap(width, height);
            Graphics objgraphics = Graphics.FromImage(objbitmap);

            Bitmap objbitmap1 = new Bitmap(width*2, height);
            Graphics objgraphics1 = Graphics.FromImage(objbitmap1);

            //画一个白色背景
            objgraphics.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

            //画一个亮黄色背景 
            objgraphics.FillRectangle(new SolidBrush(Color.White), pierect);

            //以下为画饼图(有几行row画几个)
            float currentdegree = 0.0f;
            #endregion
            
            ArrayList coun = new ArrayList();
            for (int i = 0; i < a.Count; i++)
            {
                int childtype = 0;
                //子事件总数
                for (int j = 0; j < 10; j++)
                {
                    childtype += Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "Select count(*) From warninfo_" + j + " Where errorNumber = " + a[i].ToString() + " and id>='" + dtpbeg.Value.ToString("yyyyMM") + "000000" + "' and id<= '" + dtpend.Value.ToString("yyyyMM") + "319999'" + "  and  errorstate=0", new MySqlParameter()));
                    if (childtype == -1)
                        break;
                }
                //画子事件总数
                objgraphics.FillPie((SolidBrush)colors[i], pierect, currentdegree, Convert.ToSingle(childtype) / Total * 360);
                currentdegree += Convert.ToSingle(childtype) / Total * 360;
                coun.Add(childtype);
                
            }

            //以下为生成主标题
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            SolidBrush bluebrush = new SolidBrush(Color.Black);
            string title = "";
            if (language == "en")
            {
                title = "Fault Statistics Pie Chart: " + "\n \n\n";
            }
            else
            {
                title = "故障统计饼状图: " + "\n \n\n";
            }
            //string title = "故障统计饼状图: " + "\n \n\n";
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            objgraphics.DrawString(title, fonttitle, blackbrush, new Rectangle(10, 30, width, titleheight), stringFormat);

            //列出各字段与得数目
            objgraphics1.DrawRectangle(new Pen(Color.White, 2), 0, 0, 400, 400);
            if (language == "en")
            {
                objgraphics1.DrawString("---------------Fault Statistics---------------------", fontlegend, bluebrush, 0, 10);
               
                objgraphics1.DrawString("Number of Fault Truck: " + TotalNumber, fontlegend, blackbrush, 40, 60);
                objgraphics1.DrawString("Date: " + dtpbeg.Value.ToString("yyyy-MM") + "至" + dtpend.Value.ToString("yyyy-MM"), fontlegend, blackbrush, 40, 75);
                objgraphics1.DrawString("Total Number of Fault: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 90);
              
            }
            else
            {
                objgraphics1.DrawString("---------------故障统计信息---------------------", fontlegend, bluebrush, 0, 10);
                objgraphics1.DrawString("故障叉车数量: " + TotalNumber, fontlegend, blackbrush, 40, 60);
                objgraphics1.DrawString("统计年月: " + dtpbeg.Value.ToString("yyyy-MM") + "至" + dtpend.Value.ToString("yyyy-MM"), fontlegend, blackbrush, 40, 75);
                objgraphics1.DrawString("故障总数: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 90);
            }
            
            int kuandu = 100;
            int y = 100;
            int x = 40;
            int columnwidth =300;
            int columncount = 0;      
            for (int i = 0; i < a.Count; i++)
            {

                kuandu += 15;
                y +=15;
                columncount = i / 20;
                if (language == "en")
                {
                    objgraphics1.DrawString("Fault " + a[i] + " Times: " + (coun[i]), fontlegend, blackbrush, x +columncount* columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columncount * columnwidth, (i >= 20) ? y - 300 : y, 10 , 10);
                    objgraphics1.DrawString("Percent: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush , 180 + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    //i++;
                    //objgraphics1.DrawString("Fault " + a[i] + " Times: " + (coun[i]), fontlegend, blackbrush, x+ columnwidth, kuandu);
                    //objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columnwidth, y, 10, 10);
                    //objgraphics1.DrawString("Percent: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, kuandu+columnwidth, 180);

                }
                else
                {
                    objgraphics1.DrawString("故障 " + a[i] + " 次数: " + (coun[i]), fontlegend, blackbrush, x + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columncount * columnwidth, (i >= 20) ? y - 300 :y , 10, 10);
                    objgraphics1.DrawString("所占比率: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, 180 + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    //i++;
                    //objgraphics1.DrawString("故障 " + a[i] + " 次数: " + (coun[i]), fontlegend, blackbrush, x + columnwidth, kuandu);
                    //objgraphics1.FillRectangle((SolidBrush)colors[i], 20+ columnwidth, y, 10, 10);
                    //objgraphics1.DrawString("所占比率: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, 180 +columnwidth, kuandu);

                }

            }
            pictureBox1.Image = objbitmap;
            pictureBox2.Image = objbitmap1;
        }

        //三级统计图
        public static void CreateAllImage(PictureBox pictureBox1, PictureBox pictureBox2,string language)
        {
            ArrayList a = BindC();
            //把连接字串指定为一个常量
            int Total = 0, TotalNumber = 0;
            for (int i = 0; i < 10; i++)
            {
                Total += Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "select count(*) from warninfo_" + i + " where " + " errorstate=0", new MySqlParameter()));//获取总的条目数
                if (Total == -1)
                    break;
                TotalNumber += Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "select count(distinct id) from warninfo_" + i + " where " +  "  errorstate=0", new MySqlParameter()));//获取总的条目数
                
            }
            #region

            //设置字体，fonttitle为主标题的字体
            Font fontlegend = new Font("宋体", 9);
            Font fonttitle = new Font("宋体", 10, FontStyle.Bold);

            //背景宽
            int width = 350;
            int bufferspace = 55;
            int legendheight = fontlegend.Height * 12 + bufferspace; //高度
            int titleheight = fonttitle.Height + bufferspace;
            int height = width + legendheight + titleheight + bufferspace;//白色背景高
            int pieheight = width;
            Rectangle pierect = new Rectangle(0, titleheight, width, pieheight);

            //加上各种随机色
            ArrayList colors = new ArrayList();
            Random rnd = new Random();
            for (int i = 0; i < 50; i++)
                colors.Add(new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255))));

            //创建一个bitmap实例
            Bitmap objbitmap = new Bitmap(width, height);
            Graphics objgraphics = Graphics.FromImage(objbitmap);

            Bitmap objbitmap1 = new Bitmap(width*2, height);
            Graphics objgraphics1 = Graphics.FromImage(objbitmap1);

            //画一个白色背景
            objgraphics.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

            //画一个亮黄色背景 
            objgraphics.FillRectangle(new SolidBrush(Color.White), pierect);

            //以下为画饼图(有几行row画几个)
            float currentdegree = 0.0f;
            #endregion

            ArrayList coun = new ArrayList();
            for (int i = 0; i < a.Count; i++)
            {
                int childtype = 0;
                //子事件总数
                for (int j = 0; j < 10; j++)
                {
                    childtype += Convert.ToInt16(MySqlHelper.ExecuteScalar(sqlconstr, CommandType.Text, "Select count(*) From warninfo_" + j + " Where errorNumber = " + a[i].ToString() +"  and  errorstate=0", new MySqlParameter()));
                    if (childtype == -1)
                        break;
                }
                //画子事件总数
                objgraphics.FillPie((SolidBrush)colors[i], pierect, currentdegree, Convert.ToSingle(childtype) / Total * 360);
                currentdegree += Convert.ToSingle(childtype) / Total * 360;
                coun.Add(childtype);

            }

            //以下为生成主标题
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            SolidBrush bluebrush = new SolidBrush(Color.Black);
            string title = "";
            if (language == "en")
            {
                title = "Fault Statistics Pie Chart: " + "\n \n\n";
            }
            else
            {
                title = "故障统计饼状图: " + "\n \n\n";
            }
            //string title = "故障统计饼状图: " + "\n \n\n";
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            objgraphics.DrawString(title, fonttitle, blackbrush, new Rectangle(10, 30, width, titleheight), stringFormat);

            //列出各字段与得数目
            objgraphics1.DrawRectangle(new Pen(Color.White, 2), 0, 0, 400, 400);
            if (language == "en")
            {
                objgraphics1.DrawString("---------------Fault Statistics---------------------", fontlegend, bluebrush, 0, 10);
              
                objgraphics1.DrawString("Number of Fault Truck: " + TotalNumber, fontlegend, blackbrush, 40, 60);
                // objgraphics1.DrawString("统计年份: ", fontlegend, blackbrush, 40, 70);
                objgraphics1.DrawString("Total Number of Fault: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 90);               

            }
            else
            {
                objgraphics1.DrawString("---------------故障统计信息---------------------", fontlegend, bluebrush, 0, 10);
                objgraphics1.DrawString("故障叉车数量: " + TotalNumber, fontlegend, blackbrush, 40, 60);
                // objgraphics1.DrawString("统计年份: ", fontlegend, blackbrush, 40, 70);
                objgraphics1.DrawString("故障总数: " + Convert.ToString(Total), fontlegend, blackbrush, 40, 90);
            }
           
            int kuandu = 100;
            int y = 100;
            int x = 40;
            int columnwidth = 300;
            int columncount = 0;            
            for (int i = 0; i < a.Count; i++)
            {
                kuandu += 15;
                y += 15;
                
                    columncount = i/20;
                  
               
                if (language == "en")
                {
                    objgraphics1.DrawString("Fault " + a[i] + " Times: " + (coun[i]), fontlegend, blackbrush, x + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                    objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columncount * columnwidth, (i >= 20) ? y - 300 : y, 10, 10);
                    objgraphics1.DrawString("Percent: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, 180 + columncount * columnwidth, (i >= 20) ? kuandu - 300 : kuandu);
                }
                else
                {
                    objgraphics1.DrawString("故障 " + a[i] + " 次数: " + (coun[i]), fontlegend, blackbrush, x + columncount * columnwidth, (i >=20) ? kuandu-300: kuandu);
                    objgraphics1.FillRectangle((SolidBrush)colors[i], 20 + columncount * columnwidth, (i >= 20) ? y - 300 : y, 10, 10);
                    objgraphics1.DrawString("所占比率: " + Convert.ToString(Math.Round((Convert.ToSingle(coun[i]) / Total), 3) * 100) + " %", fontlegend, blackbrush, 180 + columncount * columnwidth, (i >=20) ? kuandu - 300 : kuandu);
                }
               
            }
            pictureBox1.Image = objbitmap;
            pictureBox2.Image = objbitmap1;
        }


    }
}
