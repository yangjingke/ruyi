using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using forkliftDataStruct;
using System.Threading;
using System.Globalization;

namespace ruyi
{
    public partial class warnInfo : Form
    {
        public warnInfo(dataStructure.faultWarnData warnData,string lang)
        {
            if (lang == "en")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                //对当前窗体应用更改后的资源
            }
            InitializeComponent();
            id.Text = warnData.faultID;
           // errorarea.Text = warnData.errorArea;
            errornumber.Text = warnData.errorNumber.ToString();
            errorcount.Text = warnData.errorCount.ToString();
            // runningtime.Text = warnData.runningTime;
            if (dataStructure.errorDictionary.ContainsKey(warnData.errorNumber))
            { 
                if(lang=="en")
                {
                    error.Text = dataStructure.EnglishErrorDictionary[warnData.errorNumber];
                    errormethod.Text = dataStructure.EnglishErrorMethod[warnData.errorNumber];
                }
                else
                {
                    error.Text = dataStructure.errorDictionary[warnData.errorNumber];
                    errormethod.Text = dataStructure.errorMethod[warnData.errorNumber];
                }
                
            }
            errorBeginTime.Text = warnData.faultTime.First().ToString();
            errorLastTime.Text = warnData.faultTime.Last().ToString();
            for (int i=0;i<warnData.faultTime.Count();i++)
            {
                ListViewItem item = new ListViewItem(warnData.faultTime[i].ToString("MM-dd HH:mm:ss"));
                item.SubItems.Add(warnData.errorValue[i].ToString());
                if(warnData.errorState[i]==2)
                {
                    item.BackColor = Color.Green;
                }else if(warnData.errorState[i] == 0)
                {
                    item.BackColor = Color.Red;
                }else
                {
                    item.BackColor = Color.Yellow;
                }
                
                timeView.Items.Insert(0, item);
            }
        }
    }
}
