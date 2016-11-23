using forkliftDataStruct;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ruyi
{
    public partial class repair : Form
    {
        static string sqlconstr = "Database=" + Properties.Settings.Default.database + ";Data Source=" + Properties.Settings.Default.source + ";User ID=" + Properties.Settings.Default.user + "; password=" + Properties.Settings.Default.password;
        private main mainformhandle;
        public repair(string id,main mainform,string lang)
        {
            if (lang == "en")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                //对当前窗体应用更改后的资源
            }
            InitializeComponent();
            mainformhandle = mainform;
            idText.Text = id;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRepiarAdd_Click(object sender, EventArgs e)
        {


            dataStructure.repairData repairInfo = new dataStructure.repairData();

          
            //初始化
            repairInfo.repairID = idText.Text;
            repairInfo.repairTime = repairTime.Value;
            repairInfo.repairMan = repairMan.Text;                                     
            repairInfo.repairRecord =repairOrChange.Text+" "+controlName.Text;
            repairInfo.remark = remark.Text;
            MySqlConnection thisConnection = new MySqlConnection(sqlconstr);
            thisConnection.Open();
            try
            {

                string sql1 = "insert  into  " + "repairinfo" + "  (id , repairTime,repairMan,remark,repairRecord)";
                string sql2 = "  values (" + "'" + repairInfo.repairID + "','" + repairInfo.repairTime + "',' " + repairInfo.repairMan + " '" + ",' " + repairInfo.remark + " ',' " + repairInfo.repairRecord  + "')";
                MySqlCommand cmd = new MySqlCommand(sql1 + sql2, thisConnection);
                cmd.ExecuteNonQuery();
                thisConnection.Close();

            }
            catch (MySqlException ex)
            {
                thisConnection.Close();
                MessageBox.Show(ex + ex.Message);

            }
            this.Close();
           mainformhandle.btnRepairFind_Click(null, null);
        }
    }
}
