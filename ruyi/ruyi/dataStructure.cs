using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ruyi;
using System.Collections.Concurrent;
namespace forkliftDataStruct
{
    public class BlockingQueue<T> : BlockingCollection<T>
    {
        #region ctor(s)  
        public BlockingQueue() : base(new ConcurrentQueue<T>())
        {
        }

        public BlockingQueue(int maxSize) : base(new ConcurrentQueue<T>(), maxSize)
        {
        }
        #endregion ctor(s)  

        #region Methods  
        /// <summary>  
        /// Enqueue an Item  
        /// </summary>  
        /// <param name="item">Item to enqueue</param>  
        /// <remarks>blocks if the blocking queue is full</remarks>  
        public void Enqueue(T item)
        {
            Add(item);
        }

        /// <summary>  
        /// Dequeue an item  
        /// </summary>  
        /// <param name="Item"></param>  
        /// <returns>Item dequeued</returns>  
        /// <remarks>blocks if the blocking queue is empty</remarks>  
        public T Dequeue()
        {
            return Take();
        }
        #endregion Methods  
    }

    /// <summary>
    /// 在此定义各种数据结构类型
    /// </summary>
    public class dataStructure
    {



        public class WarnInfo
        {
            public string id { get; set; }
            public DateTime time { get; set; }
            public int errorValue { get; set; }
            public int errorNumber { get; set; }
            public int errorLevel { get; set; }
            public int errorState { get; set; } //故障状态为0时，故障开始报警，故障状态为1时，故障正在进行中，为2时故障警报解除
        }


        public class faultWarnData
        {
            public string faultID { get; set; }
            public  List<DateTime> faultTime { get; set; }
            public List<int> errorValue { get; set; }
            public List<int> errorState { get; set; }
            public int errorNumber { get; set; }
            
          //  public string errorContent { get; set; }
        //    public string errorMethod { get; set; }
            //public string runningTime { get; set; }      
           // public string errorArea { get; set; }   
            public int errorLevel { get; set; }
            public int errorCount { get; set; }

        }
        public class repairData
        {
            public string repairID { get; set; }
            public DateTime repairTime { get; set; }

            public string repairMan { get; set; }
            
            public string remark { get; set; }
            public string repairRecord { get; set; }
            


        }

        static   public Dictionary<int, string> errorDictionary = new Dictionary<int, string> {
            { 1, "高踏板故障" }, {2, "预充电故障"},{ 3,"过流"},{4, "控制器过热"},{5,"主回路断电" }, 
            { 6,"电流采样电路故障" }, {8,"BMS故障" }, {9,"电池组欠压" }, {10,"电池组过压" }
            , {11,"电机过热" }, {13,"加速器故障 " },{38,"电流传感器硬件故障" },{39,"温度传感器硬件故障" },
            { 40,"提升电机过流" },{41,"提升电机过热" },
            {42,"CAN通讯故障" },{43,"喇叭开关正极断路" },{44,"喇叭地线断路" },{45,"提升开关正极断路" },
            {46,"提升接触器线圈地线断路" },{47,"提升接触器硬件故障" },{48,"下降开关正极断路" },{49,"下降电磁阀地线断路" },
            {50,"主接触器线圈正极断路" },{51,"主接触器触点正极断路" },{52,"主接触器硬件故障" },{53,"制动器线圈正极断路" },{54,"制动器离线或线圈硬件故障"}
            , {55, "开关量输入检测电路故障"}, {56, "制动器控制电路故障"}, {57, "电流采样电路故障"}, {58, "温度测量电路故障"}, {59, "CAN总线通信故障"}
            , {60, "EEPROM读写故障"}, {61, "Flash读写故障"}, {62, "时钟芯片读写故障"}, {63, "GPRS模块通信故障"}, {64, "WIFI模块通信故障"}
          };
        static public Dictionary<int, string> EnglishErrorDictionary = new Dictionary<int, string> {
            { 1, "High pedal fault" }, {2, "Pre charge fault"},{ 3,"Overcurrent"},{4, "Controller overheating"},{5,"Main circuit power off" },
            { 6,"Current sampling circuit fault" }, {8,"BMS fault" }, {9,"Battery pack undervoltage" }, {10,"Battery pack overvoltage" }
            , {11,"Motor overheating" }, {13,"Accelerator fault " },{38,"Current sensor hardware fault" },{39,"Temperature sensor hardware fault" },
            { 40,"Lifting motor overcurrent" },{41,"Lifting motor overheating" },
            {42,"Can communication fault" },{43,"Horn switch positive disconnect" },{44,"Horn ground disconnect" },{45,"Lift switch positive disconnect" },
            {46,"Lifting contactor coil ground disconnect" },{47,"Lifting contactor hardware fault" },{48,"Down switch positive disconnect" },{49,"Drop solenoid valve ground disconnect" },
            {50,"Master contactor coil positive disconnect" },{51,"Master contactor positive disconnect" },{52,"Master contactor hareware fault" },{53,"Brake coil positive disconnect" },{54,"Brake offline or brake coil hardware fault" }, {55, "Switch input detection circuit fault"}, {56, "Brake control circuit fault"}, {57, "Current Sampling circuit fault"}, {58, "Temperature measurement circuit fault"}, {59, "CAN bus communication fault"}, {60, "EEPROM reads and writes fault"}, {61, "Flash reads and writes fault"}, {62, "The clock chip reads and writes fault"}, {63, "GPRS communication module fault"}, {64, "WIFI GPRS communication module fault"}
          };
        static  public Dictionary<int, string> EnglishErrorMethod = new Dictionary<int, string> {
            { 1, "Don't run, check the pedal and homing" }, {2, "Do not run, check the power supply board has no obvious damage, check the power cable between the power board and the control board is a reliable connection"},{ 3,"Shutdown, the first step , adjust the control parameters, the second step , adjust the output torque, if can not solve the problem , return it to the plant maintenance"},
            { 4, "Shutdown, check the fan is working properly, the duct is smooth"},{5,"Shutdown, check the main circuit insurance, contacts, emergency stop switch" },
            { 6,"Shutdown, Depot Repair" }, {8,"Shutdown, BMS faults or battery pack exception" }, {9,"Shutdown, need to be recharged" },
                { 10,"Shutdown, check the battery is normal, properly reduced energy feedback" }
            , {11,"Shutdown, stop cooling or increase the motor cooling mode" }, {13,"Shutdown, check the accelerator line is properly connected. If it is damaged, required Depot Repair" }
              , {38,"Shutdown, check the current sensor line is properly connected. If it is damaged，depot Repair or replace a new one" },{39,"Shutdown, check the current sensor line is properly connected.If it is damaged，epot Repair or replace a new one" }    , {40,"Shutdown and check the load" },{41,"Shutdown or increase the motor cooling ability" },{42,"Shutdown,check the CAN communication line is properly connected. If it is damaged, required depot repair" },{43,"Check the horn switch" },{44,"Check the horn switch" }
        ,{45,"Check the lift switch" },{46,"Check the lifting contactor coil " },{47,"Check the lifting contactor or replace a new one" },{48,"Check the down switch " },{49,"Check the drop solenoid valve " }
        ,{50,"Check the master contactor coil" },{51,"Check the master contactor" },{52,"Check or replace the master contactor" },{53,"Check the brake coil" },{54,"Check the brake coil.If it is damaged, required depot repair" },{55,"Check the Switch input detection circuit" },{56,"Check the Brake control circuit、Control relay、Brake or Control signal detection circuit" },{57,"Check Current Sampling circuit and Current sensor input signals" },{58,"Check the constant-current source circuit and signal amplification circuit of Temperature measurement" },{59,"Check the CAN bus line and the  send or receive control chip of CAN" },{60,"Check the EEPROM memory chip" },{61,"Check the Flash memory chip" },{62,"Check the clock memory chip" },{63,"Check the GPRS communication module" },{64,"Check the WIFI GPRS communication module" }
          };
        static public Dictionary<int, string> errorMethod = new Dictionary<int, string> {
            { 1, "不运行，检查踏板并归位" }, {2, "不运行，检查电源板有无明显损坏，检查电源板与控制板之间的排线是否可靠连接"},{ 3,"停机，第一步调整控制参数，第二步调整输出力矩，如不能解决问题则返厂维修"},
            { 4, "停机，检查风扇是否正常工作，风道是否顺畅"},{5,"停机，检查主回路保险、接触器、急停开关等" },
            { 6,"停机，返厂维修" }, {8,"停机，BMS 故障或者电池组异常" }, {9,"停机，需充电" },
                { 10,"停机，检查电池是否正常，适当减小能量回馈" }
            , {11,"停机，停机冷却或者增加电机散热方式" }, {13,"停机，检查加速器线路是否正常连接。如果已损坏，需返厂维修" }
              , {38,"停机，检查电流传感器线路是否正常连接，如果已损坏，需维修或更换电流传感器" },{39,"停机，检查温度传感器线路是否正常连接，如果已损坏，需维修或更换温度传感器" }    , {40,"停止运行，检查是否超载" },{41,"停机冷却，检查是否过载" },{42,"停机，检查CAN通讯链路。如果已损坏，需返厂维修" },{43,"停机，检查喇叭开关电路" },{44,"检查喇叭开关电路" }
        ,{45,"检查提升开关电路" },{46,"检查提升接触器线圈电路" },{47,"检查或者更换提升接触器" },{48,"检查下降开关电路" },{49,"检查下降电磁阀电路" }
        ,{50,"检查主接触器线圈电路" },{51,"检查主接触器电路" },{52,"检查或更换主接触器" },{53,"检查制动器线圈电路" },{54,"检查制动器电路。如果已损坏，需返厂维修" },{55,"检查开关量检测电路" },{56,"检查制动器控制电路、控制继电器、制动器或者控制信号检测电路" },{57,"检查电流采样电路和电流传感器输入信号" },{58,"检查温度测量的恒流源电路和信号放大电路" },{59,"检查CAN总线通信线路和CAN收发控制芯片" },{60,"检查EEPROM存储芯片" },{61,"检查Flash存储芯片" },{62,"检查时钟芯片" },{63,"检查GPRS通信模块" },{64,"检查WIFI通信模块" }
          };








        public class device
        {
           
            //获取各个值
          //  public string ID { get { return id; } }
           // public string RunningTime { get { return runningTime; } }
            //public DateTime gettime { get { return time; } }
            //public bool gethornSwitch { get { return hornSwitch; } }
            //public bool gethorn { get { return horn; } }
            //public bool gethornGround { get { return hornGround; } }
            //public bool getupBtnSwitch { get { return upBtnSwitch; } }
            //public bool getupBtnContactorCoilUpper { get { return upBtnContactorCoilUpper; } }
            //public bool getupBtnContactorCoilDown { get { return upBtnContactorCoilDown; } }
            //public bool getupBtnContactUpper { get { return upBtnContactUpper; } }
            //public bool getupBtnContactDown { get { return upBtnContactDown; } }
            //public bool getdownBtnSwitch { get { return downBtnSwitch; } }
            //public bool getdownBtnsolenoidvalveUpper { get { return downBtnsolenoidvalveUpper; } }
            //public bool getdownBtnsolenoidvalveDown { get { return downBtnsolenoidvalveDown; } }
            //public bool getmasterContactorCoilUpper { get { return masterContactorCoilUpper; } }
            //public bool getmasterContactorCoilDown { get { return masterContactorCoilDown; } }
            //public bool getmasterContactUpper { get { return masterContactUpper; } }
            //public bool getmasterContactDown { get { return masterContactDown; } }
            //public bool getarresterUpper { get { return arresterUpper; } }
            //public bool getarresterDown { get { return arresterDown; } }
            //public int getliftMotorCurrent { get { return liftMotorCurrent; } }
            //public int getliftMotorTemperature { get {return liftMotorTemperature; } }
            //public int getcanDirectionandSpeedMode { get { return canDirectionandSpeedMode; } }
            //public int getcanSpeed { get { return canSpeed; } }
            //public int getcanError { get { return canError; } }
            //public int getcanLowPowerMode { get { return canLowPowerMode; } }
            //public int getcanCourse { get { return canCourse; } }
            //public int getcanDirectVoltage { get { return canDirectVoltage; } }
            //public int getcanMotorCurrent { get { return canMotorCurrent; } }
            //public int getcanMotorTemperature { get { return canMotorTemperature; } }
             public string id { get; set; }  //叉车ID
            public string runningTime { get; set; }
            public  DateTime time { get; set; }
            public  bool hornSwitch { get; set; }//喇叭开关
            public bool horn { get; set; }//喇叭
            public int hornCount { get; set; }//喇叭开关动作次数
            public   bool hornGround { get; set; }//喇叭地线
            public  bool upBtnSwitch { get; set; }//上升开关
            public int upBtnCount { get; set; } //上升开关动作次数
            public  bool upBtnContactorCoilUpper { get; set; }//上升接触器线圈上端
           public bool upBtnContactorCoilDown { get; set; }//上升接触器线圈下端
           public int upBtnContactorCount { get; set; } //提升接触器动作次数
           public  bool upBtnContactUpper { get; set; }//上升触点上端
            public bool upBtnContactDown { get; set; }//上升触点下端
          
           public  bool downBtnSwitch { get; set; }//下降开关
            public int downBtnCount { get; set; } //下降开关动作次数
            public bool downBtnsolenoidvalveUpper { get; set; }//下降电磁阀上侧
          public  bool downBtnsolenoidvalveDown { get; set; }//下降电磁阀地线
            public bool masterContactorCoilUpper { get; set; }//主接触器线圈上端
            public bool masterContactorCoilDown { get; set; }//主接触器线圈下端
            public bool masterContactUpper { get; set; }//主接触器触点上端
            public bool masterContactDown { get; set; }//主接触器触点下端

            public int masterContactorCount { get; set; } //主接触器动作次数
            public bool arresterUpper { get; set; }//制动器上端
            public bool arresterDown { get; set; } //制动器下端

            public int arresterCount { get; set; } //制动器动作次数
            public int liftMotorCurrent { get; set; }//提升电机电流
            public int liftMotorTemperature { get; set; }//提升电机温度
            public int canDirectionandSpeedMode { get; set; }//can运行方向 高低速模式
            public int canSpeed { get; set; } //can转速
            public int canError { get; set; }  //can故障代码
            public int canLowPowerMode { get; set; }//can低功耗模式
            public int canCourse { get; set; }//can里程数
            public int canDirectVoltage { get; set; }//can直流电压
            public int canMotorCurrent { get; set; }//can电机电流
            public int canMotorTemperature { get; set; }//can电机温度
            public bool isCanConnection { get; set; }//can是否通讯故障
            public device()
            { }


            public device(string dataid,string data)
            {
                id = dataid;  //叉车id
                int year=0,month=0,day=0,hour=0,minute=0,second=0;
                bool yearresult = Int32.TryParse(data.Substring(73, 2), out year);
                bool monthresult= Int32.TryParse(data.Substring(75, 2), out month);
                bool dayresult = Int32.TryParse(data.Substring(73, 2), out day);
                bool hourresult = Int32.TryParse(data.Substring(75, 2), out hour);
                bool minuteresult = Int32.TryParse(data.Substring(73, 2), out minute);
                bool secondresult = Int32.TryParse(data.Substring(75, 2), out second);
                if (yearresult && monthresult && dayresult && hourresult && minuteresult && secondresult &&month>=1&&month <= 12 &&day>=1&& day <= 31&&hour>=0 && hour <= 24 &&minute>=0&& minute <= 60 &&second>=0&& second <= 60)
                {
                    time = DateTime.ParseExact("20" + data.Substring(73, 12), "yyyyMMddHHmmss", null);
                }
                else
                {
                    time = DateTime.Now;
                }
                
                //叉车运行时间
                //year = main.stringToInt(data.Substring(0,2), 2);
                //month = main.stringToInt(data.Substring(2, 2), 2);
                //day = main.stringToInt(data.Substring(4, 2), 2);
                //hour = main.stringToInt(data.Substring(6, 2), 2);
                //minute = main.stringToInt(data.Substring(8, 2), 2);
                runningTime = (main.stringToInt(data.Substring(0, 2), 2)*365*24 + main.stringToInt(data.Substring(2, 2), 2)*30*24+main.stringToInt(data.Substring(4, 2), 2)*24+ main.stringToInt(data.Substring(6, 2), 2)).ToString()+"hour"+ main.stringToInt(data.Substring(8, 2), 2)+"minute";

                //叉车开关量
                hornSwitch =Convert.ToBoolean( int.Parse(data.Substring(10,1)));
                horn =((0x80& main.stringToInt(data.Substring(11, 2),2))==0)?false:true;
                hornCount = (0x7f & main.stringToInt(data.Substring(11, 2), 2));
                hornGround = Convert.ToBoolean(int.Parse(data.Substring(27, 1)));
                upBtnSwitch = Convert.ToBoolean(int.Parse(data.Substring(13, 1)));
                upBtnContactorCoilUpper = ((0x80 & main.stringToInt(data.Substring(14, 2), 2)) == 0) ? false : true;
                upBtnCount = (0x7f & main.stringToInt(data.Substring(14, 2), 2));
                upBtnContactorCoilDown = Convert.ToBoolean(int.Parse(data.Substring(28, 1)));
                upBtnContactUpper = Convert.ToBoolean(int.Parse(data.Substring(16, 1)));
                upBtnContactDown = ((0x80 & main.stringToInt(data.Substring(17, 2), 2)) == 0) ? false : true;
                upBtnContactorCount = (0x7f & main.stringToInt(data.Substring(17, 2), 2));
                downBtnSwitch = Convert.ToBoolean(int.Parse(data.Substring(19, 1)));
                downBtnsolenoidvalveUpper = ((0x80 & main.stringToInt(data.Substring(20, 2), 2)) == 0) ? false : true;
                downBtnCount = (0x7f & main.stringToInt(data.Substring(20, 2), 2));
                downBtnsolenoidvalveDown = Convert.ToBoolean(int.Parse(data.Substring(29, 1)));
                masterContactorCoilUpper = Convert.ToBoolean(int.Parse(data.Substring(22, 1)));
                masterContactorCoilDown = Convert.ToBoolean(int.Parse(data.Substring(30, 1)));
                masterContactUpper = Convert.ToBoolean(int.Parse(data.Substring(23, 1)));
                masterContactDown = ((0x80 & main.stringToInt(data.Substring(24, 2), 2)) == 0) ? false : true;
                masterContactorCount = (0x7f & main.stringToInt(data.Substring(24, 2), 2));
                arresterUpper = Convert.ToBoolean(int.Parse(data.Substring(26, 1)));
                arresterDown = ((0x80 & main.stringToInt(data.Substring(31, 2), 2)) == 0) ? false : true;
                arresterCount = (0x7f & main.stringToInt(data.Substring(31, 2), 2));
                //叉车模拟量
                liftMotorCurrent = main.stringToInt(data.Substring(33, 2), 2);
                liftMotorTemperature = main.stringToInt(data.Substring(35, 2), 2);
                canDirectionandSpeedMode = main.stringToInt(data.Substring(37, 2), 2);
                canSpeed = main.stringToInt(data.Substring(39, 2), 2)+256* main.stringToInt(data.Substring(41, 2), 2);
                canError = main.stringToInt(data.Substring(43, 2), 2);
                canLowPowerMode = main.stringToInt(data.Substring(45, 2), 2);
                canCourse = (main.stringToInt(data.Substring(47, 2), 2) + 256 * main.stringToInt(data.Substring(49, 2), 2))/10;
                if(data.Substring(53, 2)=="ff"&& data.Substring(71, 2) == "ff")
                {
                    isCanConnection = false;
                }
                else
                {
                    isCanConnection = true;
                }
                canDirectVoltage = (main.stringToInt(data.Substring(55, 2), 2) + 256 * main.stringToInt(data.Substring(57, 2), 2)) / 10;
                canMotorCurrent = (main.stringToInt(data.Substring(59, 2), 2) + 256 * main.stringToInt(data.Substring(61, 2), 2)) / 100;
                canMotorTemperature = (main.stringToInt(data.Substring(63, 2), 2) + 256 * main.stringToInt(data.Substring(65, 2), 2)) / 10; 
            }
                

           



           }




      


    }
}
