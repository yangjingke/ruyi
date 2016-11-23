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
    public partial class local : Form
    {
        private Socket server;  //服务器套接字
        //private readonly BindingList<string,string> _data = new BindingList<string,string>();
        private ConcurrentDictionary<string, Socket> dictOfIdforClient = new ConcurrentDictionary<string, Socket>();//叉车ID、客户端连接套接字字典
        private ConcurrentDictionary<string, Location> dictOfIdAndArea = new ConcurrentDictionary<string, Location>();//叉车ID、地区信息字典
        private ConcurrentQueue<string> acceptdata = new ConcurrentQueue<string>(); //接收数据（字符串）的队列
        private ConcurrentQueue<dataStructure.device> onlineDeviceDataQueue = new ConcurrentQueue<dataStructure.device>(); //在线设备状态更新（添加）队列
     
        private ConcurrentQueue<KeyValuePair<string, string>> ip2locationqueue = new ConcurrentQueue<KeyValuePair<string,string>>();
     
        private ConcurrentDictionary<string, dataStructure.device> onlineDeviceNowTimeData = new ConcurrentDictionary<string, dataStructure.device>();//在线设备列表
        private ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData> faultDevice = new ConcurrentDictionary<KeyValuePair<string, int>, dataStructure.faultWarnData>();//故障报警列表
        List<dataStructure.faultWarnData> warnhistory = new List<dataStructure.faultWarnData>();//历史故障信息缓存列表
        private string language = "";
        //private BindingList<dataStructure.faultWarnData> _errordata = new BindingList<dataStructure.faultWarnData>(); //故障报警列表数据源
        //private ConcurrentDictionary<string, bool> deviceFormerState = new ConcurrentDictionary<string, bool>();// XXID叉车上次是否故障字典
       
        private int istoday = DateTime.Now.Day;
        
      //  QQWry.NET.QQWryLocator qqWry = new QQWry.NET.QQWryLocator("qqwry.dat");

        

        public dataStructure.device getNewlyDataState(string id)
        {
            if (onlineDeviceNowTimeData.ContainsKey(id))
            { return onlineDeviceNowTimeData[id]; }
            else return null;
        }
        public Socket getSocketById(string id)
        {
            if (dictOfIdforClient.ContainsKey(id))
            {
                return dictOfIdforClient[id];
            }
            else return null;
        }
        private void applyResources( Control fatherctl)
        {
            foreach (Control ctl in fatherctl.Controls)
            {
                if(ctl.HasChildren)
                {
                    applyResources(ctl);
                }
               

                res.ApplyResources(ctl, ctl.Name);
            }
        }
        
        private void ApplyResource()
        {

            applyResources(this);
            ////菜单
            //foreach(ColumnHeader item in onlineDeviceInChooseArea.Columns)
            //{
            //    res.ApplyResources(item, item.Name);
            //}
            //res.ApplyResources(columnHeader1, "columnHeader1");
            //res.ApplyResources(columnHeader2, "columnHeader2");
            //res.ApplyResources(columnHeader3, "columnHeader3");
            //res.ApplyResources(columnHeader4, "columnHeader4");
            //res.ApplyResources(columnHeader5, "columnHeader5");
            //res.ApplyResources(columnHeader6, "columnHeader6");
            //res.ApplyResources(columnHeader7, "columnHeader7");
            //res.ApplyResources(columnHeader8, "columnHeader8");
            //res.ApplyResources(columnHeader10, "columnHeader10");
            //res.ApplyResources(columnHeader11, "columnHeader11");
            //res.ApplyResources(columnHeader12, "columnHeader12");
            //res.ApplyResources(columnHeader15, "columnHeader15");
            //res.ApplyResources(columnHeader16, "columnHeader16");
            //res.ApplyResources(columnHeader17, "columnHeader17");
            //res.ApplyResources(columnHeader18, "columnHeader18");
            //res.ApplyResources(columnHeader19, "columnHeader19");
            //res.ApplyResources(columnHeader21, "columnHeader21");
            //res.ApplyResources(columnHeader22, "columnHeader22");          
            //res.ApplyResources(columnHeader24, "columnHeader24");
         
           
          

            //Caption
            res.ApplyResources(this, "$this");
        }
        System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(main));
        public local(string lang)
        {
            language = lang;
            if (language=="en")
            { 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                //对当前窗体应用更改后的资源
            }

            InitializeComponent();
            //if(language=="en")
            //{ 
            //ApplyResource();
            //}
            Thread parseip = new Thread(Threadip2location);
           
           
              
           
         
            //Thread beginlisten = new Thread(ThreadTcpServer);
            Thread dataprocess = new Thread(ThreadPrease);
            Thread updateDeviceInfo = new Thread(ThreadUpdate);
            parseip.IsBackground = true;
           
            updateDeviceInfo.IsBackground = true;
           // beginlisten.IsBackground = true;
            dataprocess.IsBackground = true;           
            parseip.Start(); 
            updateDeviceInfo.Start();
            dataprocess.Start();
            // beginlisten.Start();
           
            onlinedev_Click(null, null);

           
        }

        /// <summary>
        /// 主窗口运行函数，加载各子窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void onlinedev_Click(object sender, EventArgs e)
        {
            setAllPanelFalse();
            setAlButtondeafult();
            this.pnlOnlineDev.Visible = true;
           this.btnOnlineDev.BackColor = Color.Orange;
            btnOnlineDev.Enabled = false;
           
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
       

      
        /// <summary>
        /// 所有子窗口不可见
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

        private void exit_Click(object sender, EventArgs e)
        {
            Exit form = new Exit();
            form.ShowDialog();
            
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
            //Dns.GetHostByName();
            //string strHostName = Dns.GetHostName();  //得到本机的主机名
            //IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP
            //string strAddr = ipEntry.AddressList[0].ToString();
            //IPAddress local = IPAddress.Parse(strAddr);
            try {
                IPAddress ipaddress = GetLocalIPV4();
                //IPEndPoint iep = new IPEndPoint(ipaddress, (int) port);
                //IPAddress local = IPAddress.Parse(ipnumber.Text);
                IPEndPoint iep = new IPEndPoint(ipaddress, (int)port);
                //IPAddress local = IPAddress.Parse("192.168.95.1");
                //IPEndPoint iep = new IPEndPoint(local, 9998);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 将套接字与本地终结点绑定    
                server.Bind(iep);

                server.Listen(5000);

                List<Socket> socketList = new List<Socket>();
                socketList.Add(server);

                while (true)
                {

                    List<Socket> temp = socketList.ToList();
                    Socket.Select(temp, null, null, 1000);
                    int count = temp.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (temp[i].Equals(server))
                        {
                            Socket client = socketList[i].Accept();
                            socketList.Add(client);
                        }
                        else
                        {
                            byte[] bytes = new byte[1024];
                            int len;

                            try {
                                if ((len = temp[i].Receive(bytes)) > 0)
                                {
                                    string data = System.Text.Encoding.UTF8.GetString(bytes, 0, len);
                                    string[] standardData = data.Split(new Char[] { '@', '$' }, StringSplitOptions.RemoveEmptyEntries);

                                    //int dataHead = data.IndexOf("@");
                                    //int dataTail = data.IndexOf("$");
                                    //if (dataHead > dataTail)
                                    //    return;
                                    //if (dataTail - dataHead < 26)   //信息不完整直接返回；
                                    //    return;
                                    //string dataLenth = data.Substring(dataHead + 21, 2);
                                    //int dataLen = stringToInt(dataLenth, 2);
                                    //if (data.Length != 26 + dataLen)
                                    //{
                                    //    return;
                                    //}
                                    //string dstSource = data.Substring(dataHead + 1, 4);
                                    //string srcSource = data.Substring(dataHead + 5, 4);
                                    //string id = "20" + data.Substring(dataHead + 9, 10);
                                    //string fc = data.Substring(dataHead + 19, 2);

                                    //string checksum = data.Substring(dataTail - 2, 2);
                                    //int datacheck = stringToInt(checksum, 2);     //数据中的校验和
                                    //string collectData = data.Substring(dataHead + 23, dataLen);
                                    //int check = CheckSum(collectData, dataLen);
                                    //if (check != datacheck) return; //校验失败直接退出


                                    foreach (string item in standardData)
                                    {
                                        //string dataLenth = dataForCheck.Substring( 20, 2);  //检验
                                        //int dataLen = stringToInt(dataLenth, 2);
                                        //string checksum = dataForCheck.Substring(dataForCheck.Length-1 - 2, 2);
                                        //int datacheck = stringToInt(checksum, 2);     //数据中的校验和


                                        string dataForCheck = "@" + item + "$";
                                        int dataHead = dataForCheck.IndexOf("@");
                                        int dataTail = dataForCheck.IndexOf("$");
                                        if (dataHead > dataTail)
                                            continue;
                                        if (dataTail - dataHead < 26)   //信息不完整直接返回；
                                            continue;
                                        string dataLenth = dataForCheck.Substring(dataHead + 21, 2);
                                        int dataLen = stringToInt(dataLenth, 2);
                                        if (dataForCheck.Length != 26 + dataLen)
                                        {
                                            continue;
                                        }
                                        string dstSource = dataForCheck.Substring(dataHead + 1, 4);
                                        string srcSource = dataForCheck.Substring(dataHead + 5, 4);
                                        string id = "20" + dataForCheck.Substring(dataHead + 9, 10);
                                        string fc = dataForCheck.Substring(dataHead + 19, 2);

                                        string checksum = dataForCheck.Substring(dataTail - 2, 2);
                                        int datacheck = stringToInt(checksum, 2);     //数据中的校验和
                                        string collectData = dataForCheck.Substring(dataHead + 23, dataLen);
                                        int check = CheckSum(collectData, dataLen);
                                        if (check != datacheck) continue; //校验失败直接退出







                                        try
                                        {
                                            if (!dictOfIdforClient.ContainsKey(id))   //是否存在该ID的叉车
                                            {


                                                if (false == dictOfIdforClient.TryAdd(id, temp[i]))
                                                    MessageBox.Show("dictionary添加失败 \n Dictionary update failed");
                                                string ip = ((IPEndPoint)temp[i].RemoteEndPoint).Address.ToString();  //查询ip


                                              ip2locationqueue.Enqueue(new KeyValuePair<string, string>(id,ip));
                                               //Location location =new Location(ip);
                                               // if (location.country_name != null)
                                               // {
                                               //     if (false == dictOfIdAndArea.TryAdd(id, location))
                                               //         MessageBox.Show("IPdictionary更新失败");

                                               //     onlineDeviceUpdate(new KeyValuePair<string, Location>(id, location));
                                               // }
                                                //if (result.code == 0)
                                                //  {
                                                //      if (false == dictOfIdAndArea.TryAdd(id,result.dataForCheck))
                                                //           MessageBox.Show("IPdictionary更新失败");
                                                //      onlineDeviceUpdate(new KeyValuePair<string, TaobaoIP_Data>(id, result.dataForCheck)); //地区分布树状表中添加该信息！

                                                //  }
                                                //  else
                                                //  {
                                                //      if (false == dictOfIdAndArea.TryAdd(id,new TaobaoIP_Data()))
                                                //           MessageBox.Show("IPdictionary更新失败");
                                                //      onlineDeviceUpdate(new KeyValuePair<string, TaobaoIP_Data>());
                                                //  }

                                            }
                                            //else if (dictOfIdforClient[id] != temp[i])    //不相等的话进行更新
                                            //{
                                            //    if (false == dictOfIdforClient.TryUpdate(id, temp[i], dictOfIdforClient[id]))
                                            //        MessageBox.Show("dictionary更新失败");
                                            //    string ip = ((IPEndPoint)temp[i].RemoteEndPoint).Address.ToString();  //查询ip

                                            //    //QQWry.NET.IPLocation location = qqWry.Query(ip);
                                            //    //Location location = new Location(ip);
                                            //    //if (location.country_name != null)
                                            //    //{

                                            //    //    if (false == dictOfIdAndArea.TryUpdate(id,dictOfIdAndArea[id] ,location))
                                            //    //        MessageBox.Show("IPdictionary更新失败");

                                            //    //    onlineDeviceUpdate(new KeyValuePair<string, Location>(id, location));

                                            //    //}
                                            //    ip2locationqueue.Enqueue(new KeyValuePair<string, string>(id, ip));
                                            //}
                                        }
                                        catch (ArgumentNullException)
                                        {
                                            MessageBox.Show("ArgumentNullException");
                                        }
                                        //    string dataRecovery = "@" + dataForCheck + "$";
                                        acceptdata.Enqueue(dataForCheck);   //添加到解析队列

                                        //byte[] idchar = System.Text.Encoding.UTF8.GetBytes(id);
                                        //temp[i].Send(idchar,10,SocketFlags.None);

                                    }
                                }
                                else
                                {

                                    //通过IP查询ID号来进行删除(同一个IP可能多个id）
                                    var query = (from d in dictOfIdforClient
                                                 where d.Value == temp[i]
                                                 select d.Key).ToArray();
                                    if (query != null) {
                                        var Area = dictOfIdAndArea[query[0]];
                                        //for (int m = 0; m < query.Count(); m++)
                                        //{

                                        //    var tempValue = dictOfIdforClient[query[m]];
                                        //    var tempArea = dictOfIdAndArea[query[m]];
                                        //    dictOfIdforClient.TryRemove(query[m], out tempValue);
                                        //    dictOfIdAndArea.TryRemove(query[m], out tempArea);
                                        //}
                                        ///数据库中数据不规则 ，有的是广西，有的是广西省，故作如下调整

                                        //if (Area.Contains("广西") && !Area.Contains("省"))
                                        //{
                                        //    Area = Area.Insert(2, "省");
                                        //}
                                        //string[] AreaInfo = Area.Split(new Char[] { '省' }, StringSplitOptions.RemoveEmptyEntries);

                                        //if (AreaInfo.Count() == 2)
                                        //{
                                        //var cityResult = (from item in dictOfIdAndArea where item.Value.Country.Contains(AreaInfo[1]) select item).ToList();
                                        //if (cityResult.Count == 0)
                                        //{
                                        //    this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(AreaInfo[0])) { if (onlineDevicetreeView.Nodes[AreaInfo[0]].Nodes.ContainsKey(AreaInfo[1])) { onlineDevicetreeView.Nodes[AreaInfo[0]].Nodes[AreaInfo[1]].Remove(); } } }));
                                        //}
                                        //var RegionResult = (from item in dictOfIdAndArea where item.Value.Country.Contains(AreaInfo[0]) select item).ToList();
                                        //if (RegionResult.Count == 0)
                                        //{
                                        //    this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(AreaInfo[0])) { onlineDevicetreeView.Nodes[AreaInfo[0]].Remove(); } }));
                                        //}


                                        //}
                                        //else
                                        //{
                                        //    var Result = (from item in dictOfIdAndArea where item.Value.Country.Contains(AreaInfo[0]) select item).ToList();
                                        //    if (Result.Count == 0)
                                        //    {
                                        //        this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(AreaInfo[0])) { onlineDevicetreeView.Nodes[AreaInfo[0]].Remove(); } }));
                                        //    }

                                        //}




                                        for (int n = 0; n < query.Count(); n++)
                                        {
                                            var tempValue = dictOfIdforClient[query[n]];
                                            var tempArea = dictOfIdAndArea[query[n]];
                                            dictOfIdforClient.TryRemove(query[n], out tempValue);
                                            dictOfIdAndArea.TryRemove(query[n], out tempArea);
                                            var tempValue1 = onlineDeviceNowTimeData[query[n]];
                                            onlineDeviceNowTimeData.TryRemove(query[n], out tempValue1);


                                        }

                                      
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

                                    temp[i].Close();
                                    socketList.Remove(temp[i]);

                                    // Console.WriteLine("关闭");
                                }
                            }
                            catch (Exception e)
                            {
                                if (temp[i] != server)
                                {
                                    var query = (from d in dictOfIdforClient
                                                 where d.Value == temp[i]
                                                 select d.Key).ToArray();
                                    if (query.Count() > 0)
                                    {
                                        if (dictOfIdAndArea.ContainsKey(query[0]))
                                        { 
                                        var Area = dictOfIdAndArea[query[0]];
                                        //for (int m = 0; m < query.Count(); m++)
                                        //{

                                        //    var tempValue = dictOfIdforClient[query[m]];
                                        //    var tempArea = dictOfIdAndArea[query[m]];
                                        //    dictOfIdforClient.TryRemove(query[m], out tempValue);
                                        //    dictOfIdAndArea.TryRemove(query[m], out tempArea);
                                        //}

                                        //if (Area.Contains("广西") && !Area.Contains("省"))
                                        //{
                                        //    Area = Area.Insert(2, "省");
                                        //}
                                        //string[] AreaInfo = Area.Split(new Char[] { '省' }, StringSplitOptions.RemoveEmptyEntries);

                                        //if (AreaInfo.Count() == 2)
                                        //{
                                        //    var cityResult = (from item in dictOfIdAndArea where item.Value.Country.Contains(AreaInfo[1]) select item).ToList();
                                        //    if (cityResult.Count == 0)
                                        //    {
                                        //        this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(AreaInfo[0])) { if (onlineDevicetreeView.Nodes[AreaInfo[0]].Nodes.ContainsKey(AreaInfo[1])) { onlineDevicetreeView.Nodes[AreaInfo[0]].Nodes[AreaInfo[1]].Remove(); } } }));
                                        //    }
                                        //    var RegionResult = (from item in dictOfIdAndArea where item.Value.Country.Contains(AreaInfo[0]) select item).ToList();
                                        //    if (RegionResult.Count == 0)
                                        //    {
                                        //        this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(AreaInfo[0])) { onlineDevicetreeView.Nodes[AreaInfo[0]].Remove(); } }));
                                        //    }


                                        //}
                                        //else
                                        //{
                                        //    var Result = (from item in dictOfIdAndArea where item.Value.Country.Contains(AreaInfo[0]) select item).ToList();
                                        //    if (Result.Count == 0)
                                        //    {
                                        //        this.Invoke((Action)(() => { if (onlineDevicetreeView.Nodes.ContainsKey(AreaInfo[0])) { onlineDevicetreeView.Nodes[AreaInfo[0]].Remove(); } }));
                                        //    }

                                        //}
                                        for (int n = 0; n < query.Count(); n++)
                                        {

                                            var tempValue = dictOfIdforClient[query[n]];
                                            var tempArea = dictOfIdAndArea[query[n]];
                                            dictOfIdforClient.TryRemove(query[n], out tempValue);
                                            dictOfIdAndArea.TryRemove(query[n], out tempArea);
                                            var tempValue1 = onlineDeviceNowTimeData[query[n]];
                                            onlineDeviceNowTimeData.TryRemove(query[n], out tempValue1);

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


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
      
        /// <summary>
        /// 解析线程函数
        /// </summary>
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
                    string data=null;
                    if (true == acceptdata.TryDequeue(out data))
                    {
                        dataprocess(data);
                    }
                    else
                    {
                        MessageBox.Show("队列取数失败！ \n  trydequeue failed");
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
            string id ="20"+ data.Substring(dataHead + 9, 10);
            string fc = data.Substring(dataHead + 19, 2);

            string checksum = data.Substring(dataTail - 2, 2);
            int datacheck = stringToInt(checksum, 2);     //数据中的校验和
            string collectData = data.Substring(dataHead + 23, dataLen);
            int check = CheckSum(collectData, dataLen);
            if (check != datacheck) return; //校验失败直接退出
            switch (fc)
            {
                //状态采集信息处理
                case "01":

                    dataStructure.device deviceState = new dataStructure.device(id, collectData);
                    //推入在线设备队列
                    onlineDeviceDataQueue.Enqueue(deviceState);
                   
               
                    break;
                case"02":
                    dataStructure.device FaultdeviceState = new dataStructure.device(id, collectData);
                    //推入在线设备队列
                    //onlineDeviceDataQueue.Enqueue(FaultdeviceState);
                    //deviceStateSaveQueue.Enqueue(deviceState);

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

            if (istoday != DateTime.Now.Day)
            {


                faultDevice.Clear();
                this.Invoke((Action)(
                    () => {
                        FaultWarnList.Items.Clear();
                    })
                        );
               
                istoday = DateTime.Now.Day;
            }
            if ((deviceState.canError&0x0080)>0)
            {
                int faultNumber = deviceState.canError&0x7f;
            //faultData.faultID = deviceState.id;  //报警ID
            //faultData.faultTime = deviceState.time;//报警时间
            //faultData.runningTime = deviceState.runningTime;//运行时间
                if (faultNumber != 0)  
            {
                    if (faultDevice.ContainsKey(new KeyValuePair<string, int>(deviceState.id, faultNumber)))  //如果已经有该ID的该类型报警
                    {
                        dataStructure.WarnInfo save = new dataStructure.WarnInfo();
                        
                        if(faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.LastOrDefault()==2)
                        {
                            save.errorState = 0;
                        }
                        else
                        {
                            save.errorState = 1;
                        }
                        save.id = deviceState.id;
                        save.errorNumber = faultNumber;
                        save.time = deviceState.time;
                        save.errorLevel = 1;
                        int errorCount= ++faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorCount;
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].faultTime.Add(deviceState.time);
                        faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorState.Add(save.errorState);
                        if (faultNumber ==40)
                            {
                                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorCurrent);
                            save.errorValue = deviceState.liftMotorCurrent;
                        }
                            else 
                            if(faultNumber==41)
                            {
                                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(deviceState.liftMotorTemperature);
                            save.errorValue = deviceState.liftMotorTemperature;
                        }
                            else
                            {
                                faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].errorValue.Add(0);
                             save.errorValue = 0;
                            }
                        // faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)].runningTime = deviceState.runningTime;
                        //存入储存队列
                       
                      
                       
                        this.Invoke((Action)(
                            () =>
                        {
                       

                            ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Text == deviceState.id && x.SubItems[2].Text == faultNumber.ToString());
                            if (li != null)
                            {
                                FaultWarnList.BeginUpdate();
                                li.SubItems[1].Text = deviceState.time.ToString("HH:mm:ss");
                                li.SubItems[3].Text = errorCount.ToString();
                                li.BackColor = Color.Red;
                                FaultWarnList.EndUpdate();
                            }
                            if (btnFaultWarn.Enabled == true) { warningTimer.Enabled = true; }
                        }));
                   

                    }
                    else
                    {
                        dataStructure.WarnInfo save = new dataStructure.WarnInfo();
                        save.id = deviceState.id;
                        save.errorLevel = 1;
                        save.errorNumber = faultNumber;
                        save.errorState = 0;
                        save.time = deviceState.time;
                        
                        dataStructure.faultWarnData errorData = new dataStructure.faultWarnData();
                        errorData.faultID = deviceState.id;
                        //errorData.runningTime = deviceState.runningTime;
                        errorData.errorNumber = faultNumber;
                        errorData.faultTime = new List<DateTime>();
                        errorData.errorValue = new List<int>();
                        errorData.errorState = new List<int>();
                        errorData.faultTime.Add(deviceState.time);
                        errorData.errorState.Add(save.errorState);
                            if (faultNumber == 40)
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
                            // errorData.errorArea = dictOfIdAndArea[deviceState.id].Country;
                            //  errorData.errorContent = dataStructure.errorDictionary[errorData.errorNumber];
                            errorData.errorLevel = 1;
                      //  errorData.errorMethod = dataStructure.errorMethod[errorData.errorNumber];
                        errorData.errorCount = 1;
                        faultDevice.TryAdd(new KeyValuePair<string, int>(deviceState.id, faultNumber), errorData);
                       
                        //报警表格中添加信息
                        ListViewItem item = new ListViewItem(errorData.faultID);
                        item.SubItems.Add(deviceState.time.ToString("HH:mm:ss"));
                        item.SubItems.Add(errorData.errorNumber.ToString());
                        //item.SubItems.Add(dictOfIdAndArea[errorData.faultID].Country);
                        item.SubItems.Add(errorData.errorCount.ToString());
                       // item.SubItems.Add(errorData.runningTime.ToString());
                        item.BackColor = Color.Red;
                        this.Invoke((Action)(
                        () => { FaultWarnList.Items.Add(item); if (btnFaultWarn.Enabled == true) { warningTimer.Enabled = true; }
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
               
               //if ( faultDevice.ContainsKey(new KeyValuePair< string, int> (deviceState.id, faultNumber)))
               // {
               //     var temp = faultDevice[new KeyValuePair<string, int>(deviceState.id, faultNumber)];
               //     faultDevice.TryRemove(new KeyValuePair<string, int>(deviceState.id, faultNumber),out temp);
               //  }
                this.Invoke((Action)(
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

                //ListViewItem li = FaultWarnList.Items.Cast<ListViewItem>().First(x => x.Text == deviceState.id && x.SubItems[2].Text == faultNumber.ToString());
                //li.BackColor = Color.Green;

            }
           
        }
        /// <summary>
        /// 实时查询ip数据库
        /// </summary>
        public void Threadip2location()
        {
            while(true)
            {
                if (ip2locationqueue.IsEmpty)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    KeyValuePair<string, string> idandip = new KeyValuePair<string, string>();
                    ip2locationqueue.TryDequeue(out idandip);
                    Location location = new Location();
                    if (location.country_name != null)
                    {
                        if(!dictOfIdAndArea.ContainsKey(idandip.Key))
                        { 
                        if (false == dictOfIdAndArea.TryAdd(idandip.Key, location))
                            { 
                            MessageBox.Show("IPdictionary添加失败  \n IPdictionary update failed ");
                            this.BeginInvoke((Action)(() =>
                            {
                               if(!comboBoxForChooseInOnlineDev.Items.Contains(idandip.Key))
                                {
                                    comboBoxForChooseInOnlineDev.Items.Add(idandip.Key);
                                }
                                //comboBoxForChooseInOnlineDev.Items.Add(idandip.Key);
                               
                               
                                if (!comboBoxForChooseInWarn.Items.Contains(idandip.Key))
                                {
                                    comboBoxForChooseInWarn.Items.Add(idandip.Key);
                                }
                               
                                
                               
                               
                              
                                
                            }));
                            }
                          

                        }
                        //else
                        //{
                        //    if(false==dictOfIdAndArea.TryUpdate(idandip.Key,dictOfIdAndArea[idandip.Key],location))
                        //    {
                        //        MessageBox.Show("IPdictionary更新失败");
                        //    }
                        //}
                        
                        onlineDeviceUpdate(new KeyValuePair<string, Location>(idandip.Key, location));
                    }
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
                
                if (!onlineDeviceDataQueue.IsEmpty)
                {
                    dataStructure.device device;

                    onlineDeviceDataQueue.TryDequeue(out device);
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
                                  
                                  
                                    if (!comboBoxForChooseInWarn.Items.Contains(device.id))
                                    {
                                        comboBoxForChooseInWarn.Items.Add(device.id);
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
                    
                } else if ( onlineDeviceDataQueue.IsEmpty/*&&onlineDeviceDatabaseQueue.IsEmpty*/)
                {
                    Thread.Sleep(500);
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

        /// <summary>
        /// 储存数据线程
        /// </summary>
       
        
    
        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="lpBuffer"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public static int CheckSum(string lpBuffer, int nLen)
        {
            int ucCheckVal = 0;


            for (int i = 0; i < nLen; ++i)
            {
                ucCheckVal ^= lpBuffer[i];
            }
            return ucCheckVal;
        }

        public static string GetHexChar(int value)
        {
            string sReturn = string.Empty;
            switch (value)
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
            int shift_i;
            string Value = null;
            for (shift_i = 0; shift_i <= dataLenth - 1; shift_i++)
            {
              
                 Value =Value+ GetHexChar((ch_double %(16^(dataLenth-1-shift_i))));
            }
            return Value;
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
        /// <summary>
        /// 更新地区分布列表
        /// </summary>
        private void onlineDeviceUpdate(KeyValuePair<string, Location> item)
        {
            ///数据库中数据不规则 ，有的是广西，有的是广西省，故作如下调整
          
           

         
             
                    if (onlineDevicetreeView.Nodes.ContainsKey(item.Value.country_name))  //是否存在该国家的节点
                    {
                        if (!onlineDevicetreeView.Nodes[item.Value.country_name].Nodes.ContainsKey(item.Value.region_name)) //没有该城市的情况下添加该城市
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
                    else  //不存在该省份
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

               
                //else
                //{
                //    if (!onlineDevicetreeView.Nodes.ContainsKey(countryAndRegion[0]))
                //    {
                //        this.Invoke((Action)(() =>
                //    {
                //        TreeNode locationNode = new TreeNode(countryAndRegion[0]);
                //    locationNode.Name = countryAndRegion[0];
                //    onlineDevicetreeView.Nodes.Add(locationNode);
                //    }));
                //    }
                //}
          
            //else
            //{
            //    if (!onlineDevicetreeView.Nodes.ContainsKey("未分类"))
            //    {
            //        onlineDevicetreeView.Nodes.Add("未分类");
            //    }
            //}













        }

      

     

        

 
        private void chooseOnlineDev_Click(object sender, EventArgs e)
        {

            onlineDevicetreeView.SelectedNode = null;
            string textForFind = comboBoxForChooseInOnlineDev.Text;
            
            //IEnumerable<DataGridViewRow> enumerableList = onlineDevListInOnlineDev.Rows.Cast<DataGridViewRow>();
            //var list = (from item in enumerableList
            //            where item.Cells[0].Value.ToString() == fullFindId
            //            select item).ToList();
            if (idChecked.Checked)
            {
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
                string id = textForFind;
                 var selected = (from itemofdevice in dictOfIdAndArea where itemofdevice.Key == id  select itemofdevice.Key).ToList();
                if (selected.Count == 1)
                {
                    onlineDeviceInChooseArea.BeginUpdate();
                    onlineDeviceInChooseArea.Items.Clear();
                    int i = 0, j = 0;
                    foreach (var deviceId in selected)
                    {
                        ListViewItem item = new ListViewItem((j + 1).ToString());
                        item.SubItems.Add(deviceId);
                        item.SubItems.Add(onlineDeviceNowTimeData[ deviceId].runningTime);
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
                    MessageBox.Show("该叉车并不在线  \n the truck isn't online");
                }
            }
            if (areaChecked.Checked)
            {
              var selected = (from item in dictOfIdAndArea where item.Value.country_name.Contains(textForFind)|| item.Value.region_name.Contains(textForFind)|| item.Value.city_name.Contains(textForFind)
                     orderby item.Key   select item.Key).ToList();
                if (selected.Count >= 1)
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





       

        

    

       

      

      

       

   
        /// <summary>
        /// 根据ID查询报警信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
     

        private void onlineDevicetreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
           string selectedArea = e.Node.Text;
            
            var deviceInSelectArea = (from item in dictOfIdAndArea where item.Value.country_name.Contains(selectedArea) || item.Value.region_name.Contains(selectedArea) || item.Value.city_name.Contains(selectedArea) orderby item.Key select item.Key).ToList();
           
            onlineDeviceInChooseArea.BeginUpdate();
            onlineDeviceInChooseArea.Items.Clear();
            int i = 0;
            foreach (var deviceId in deviceInSelectArea)
            {   
                ListViewItem item = new ListViewItem(deviceId);
                item.SubItems.Add(onlineDeviceNowTimeData[deviceId].runningTime);
                item.SubItems.Add(dictOfIdAndArea[deviceId].city_name +" , "+ dictOfIdAndArea[deviceId].region_name + " , "+dictOfIdAndArea[deviceId].country_name);
                item.SubItems.Add(onlineDeviceNowTimeData[deviceId].time.ToString());
                item.SubItems.Add(onlineDeviceNowTimeData[deviceId].canCourse.ToString()+"公里");
                if(i%2==0)
                {
                    item.BackColor = Color.LightSlateGray;
                }
                i++;
                onlineDeviceInChooseArea.Items.Add(item);
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
 
       

        private void chooseError_Click(object sender, EventArgs e)
        {
            string textForFind = comboBoxForChooseInWarn.Text;
           
            if (warnid.Checked)
            {
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
                var li = FaultWarnList.Items.Cast<ListViewItem>().Where(x => x.Text == textForFind).ToList();
                if (li.Count!=0)
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
                    MessageBox.Show("该叉车并未故障报警，请重新检查查询编号！\n the truck doesn't have fault alarm, please check truck ID before query");
                }
            }
            if (warnnumber.Checked)
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

            ListViewHitTestInfo info = FaultWarnList.HitTest(e.X, e.Y);
            var item = info.Item as ListViewItem;
            string id = item.Text;
             int errornumber =int.Parse(item.SubItems[2].Text);
            warnInfo form = new warnInfo(faultDevice[new KeyValuePair<string, int>(id,errornumber)],language);
            form.Show();
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
           

            
            this.BeginInvoke((Action)(
                     () =>
                     {
                         string selectedArea = e.Node.Text;
                         var deviceInSelectArea = (from item in dictOfIdAndArea where item.Value.country_name.Contains(selectedArea)|| item.Value.region_name.Contains(selectedArea) || item.Value.city_name.Contains(selectedArea)orderby item.Key select item.Key).ToList();
                         onlineDeviceInChooseArea.BeginUpdate();
                         onlineDeviceInChooseArea.Items.Clear();
                         int i = 0;
                     for (int j = 0; j<deviceInSelectArea.Count();j++)
                         {
                             if (onlineDeviceNowTimeData.ContainsKey(deviceInSelectArea[j]) && dictOfIdAndArea.ContainsKey(deviceInSelectArea[j]))
                             {

                                 ListViewItem item = new ListViewItem((j + 1).ToString());
                                 item.SubItems.Add(deviceInSelectArea[j]);
                                 item.SubItems.Add(onlineDeviceNowTimeData[deviceInSelectArea[j]].runningTime);
                                                                 item.SubItems.Add(dictOfIdAndArea[deviceInSelectArea[j]].city_name + " ," + dictOfIdAndArea[deviceInSelectArea[j]].region_name + " ," + dictOfIdAndArea[deviceInSelectArea[j]].country_name);
                                 
                                 if (i % 2 == 0)
                                 {
                                     item.BackColor = Color.DimGray;
                                 }
                                 i++;
                                 onlineDeviceInChooseArea.Items.Add(item);
                             }
                         }
                         onlineDeviceInChooseArea.EndUpdate();


                     }));
       
        }

        private void connect_Click(object sender, EventArgs e)
        {
            Thread beginlisten = new Thread(ThreadTcpServer);
           
            beginlisten.IsBackground = true;
           connect.Enabled = false;
          // portnumber .Visible = false;
          if(portnumber.Text!="")
            { 
            int port =int.Parse(portnumber.Text);
            beginlisten.Start(port);
            }
        }

        private void systemtimer_Tick(object sender, EventArgs e)
        {
            timetext.Text = DateTime.Now.ToString();
            
        }
        

    

       
        

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit form = new Exit();
            form.ShowDialog();
            e.Cancel = true;

        }




    
      
      

     
       
      

      
        





    }
}
