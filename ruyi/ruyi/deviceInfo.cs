using System;
using System.Drawing;
using System.Windows.Forms;
using forkliftDataStruct;
using System.Threading;
using System.Globalization;
using System.Net.Sockets;

namespace ruyi
{
    public partial class deviceInfo : Form
    {
        private main mainformhandle;
        private string stateid="";
        private string language ="";
        public deviceInfo(dataStructure.device device,main mainform,string lang)
        {
            language = lang;
            if (lang == "en")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                //对当前窗体应用更改后的资源
            }
            InitializeComponent();
          
            mainformhandle = mainform;
            stateid = device.id;
            //this.Text = "编号" + device.id + "叉车信息";
            id.Text = device.id;
            time.Text = device.time.ToString();
            runningtime.Text = device.runningTime;
        if(device.isCanConnection == true)
         { 
            if (dataStructure.errorDictionary.ContainsKey(device.canError))
            {
                    if(language=="en")
                    {

                        error.Text = dataStructure.EnglishErrorDictionary[device.canError];
                        errormethod.Text = dataStructure.EnglishErrorMethod[device.canError];
                    }
                    else
                    {
                        error.Text = dataStructure.errorDictionary[device.canError];
                        errormethod.Text = dataStructure.errorMethod[device.canError];
                    }
              
                this.error.ForeColor = Color.Red;
                this.errormethod.ForeColor = Color.Red;
        

              
            }
            else
            {
                    
                        error.Text = " #### ";
                        errormethod.Text = " #### ";
                  
               
            }
                int canReverse = device.canDirectionandSpeedMode & 0x0004;
                int canInterlock = device.canDirectionandSpeedMode & 0x0010;
                reverse.Text = canReverse.ToString();
                interlock.Text = canInterlock.ToString();
                int Direction = device.canDirectionandSpeedMode & 0x0003;
            int SpeedMode = device.canDirectionandSpeedMode & 0x0008;
          
            switch (Direction)
            {
                case 0:
                    direction.Text = (language == "en") ? "Invalid" : "无效";
                    break;
                case 1:
                    direction.Text = (language == "en") ? "Forward" : "前进";
                    break;
                case 2:
                    direction.Text = (language == "en") ? "Backward" : "后退";
                    break;
                case 3:
                    direction.Text = (language == "en")? "Invalid" : "无效";
                    break;
                default:
                    break;

            }
            if (SpeedMode == 0)
            {
                    speedmode.Text = (language == "en")? "High speed" : "高速";
            }
            else
            {
                speedmode.Text = (language == "en") ? "Low speed" : "低速";
            }
         
            if (device.canLowPowerMode == 0xaa)
            {
               lowpowermode.Text = (language == "en") ? "yes" : "低功耗";
            }
            else
            {
                lowpowermode.Text = (language == "en") ? "no" : "其他";
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
                error.Text ="##";
                errormethod.Text = "##";
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
            horn.Text=device.hornCount.ToString() + "次";
            hornGround.Text=device.hornGround ? "不正常" : "正常";
            upBtnSwitch.Text=device.upBtnSwitch ? "正常" : "不正常";
            upBtnContactorCoilUpper.Text=device.upBtnCount.ToString() + "次";
            upBtnContactorCoilDown.Text=device.upBtnContactorCoilDown ? "不正常" : "正常";
            upBtnContactUpper.Text=device.upBtnContactUpper ? "正常" : "不正常";
            upBtnContactDown .Text=device.upBtnContactorCount.ToString() + "次";
            downBtnSwitch.Text=device.downBtnSwitch ? "正常" : "不正常";
            downBtnsolenoidvalveUpper .Text=device.downBtnCount.ToString() + "次";
            downBtnsolenoidvalveDown.Text=device.downBtnsolenoidvalveDown ? "不正常" : "正常";
            masterContactorCoilUpper.Text=device.masterContactorCoilUpper ? "正常" : "不正常";
            masterContactorCoilDown.Text=device.masterContactorCoilDown ? "不正常" : "正常";
            masterContactUpper.Text=device.masterContactUpper ? "正常" : "不正常";
            masterContactDown.Text=device.masterContactorCount.ToString() + "次";
            arresterUpper.Text=device.arresterUpper ? "正常" : "不正常";
            arresterDown.Text= device.arresterCount.ToString() + "次";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           dataStructure.device device = mainformhandle.getNewlyDataState(stateid);
            if (device == null) return;
           
            id.Text = device.id;
            time.Text = device.time.ToString();
            runningtime.Text = device.runningTime;
            if (device.isCanConnection == true)
            {
                if (dataStructure.errorDictionary.ContainsKey(device.canError))
                {
                    if (language == "en")
                    {

                        error.Text = dataStructure.EnglishErrorDictionary[device.canError];
                        errormethod.Text = dataStructure.EnglishErrorMethod[device.canError];
                    }
                    else
                    {
                        error.Text = dataStructure.errorDictionary[device.canError];
                        errormethod.Text = dataStructure.errorMethod[device.canError];
                    }
                    this.error.ForeColor = Color.Red;
                    this.errormethod.ForeColor = Color.Red;



                }
                else
                {
                    error.Text = " #### ";
                    errormethod.Text = " #### ";
                }

                int canReverse= device.canDirectionandSpeedMode & 0x0004;
                int canInterlock = device.canDirectionandSpeedMode & 0x0010;
                reverse.Text = canReverse.ToString();
                interlock.Text = canInterlock.ToString();
                int Direction = device.canDirectionandSpeedMode & 0x0003;
                int SpeedMode = device.canDirectionandSpeedMode & 0x0008;

                switch (Direction)
                {
                    case 0:
                        direction.Text = (language == "en") ? "Invalid" : "无效";
                        break;
                    case 1:
                        direction.Text = (language == "en") ? "Forward" : "前进";
                        break;
                    case 2:
                        direction.Text = (language == "en") ? "Backward" : "后退";
                        break;
                    case 3:
                        direction.Text = (language == "en") ? "Invalid" : "无效";
                        break;
                    default:
                        break;

                }
                if (SpeedMode == 0)
                {
                    speedmode.Text = (language == "en") ? "High speed" : "高速";
                }
                else
                {
                    speedmode.Text = (language == "en") ? "Low speed" : "低速";
                }

                if (device.canLowPowerMode == 0xaa)
                {
                    lowpowermode.Text = (language == "en") ? "yes" : "低功耗";
                }
                else
                {
                    lowpowermode.Text = (language == "en") ? "no" : "其他";
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
                error.Text = "##";
                errormethod.Text = "##";
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

        private void refresh_Click(object sender, EventArgs e)
        {
            try { 
            Socket client = mainformhandle.getSocketById(stateid);
            if (client == null)
            {
                if (language == "en")
                {

                    MessageBox.Show("The truck is offline!");
                }
                else
                {
                    MessageBox.Show("该叉车不在线！");
                }
                return;
            }
                else
            {
                    string commandtext = "@00000001" + stateid.Substring(2, 10) + "030000$";
                    byte[] commandchar = System.Text.Encoding.UTF8.GetBytes(commandtext);
                   client.Send(commandchar);
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              
            }
        }
    }
}
