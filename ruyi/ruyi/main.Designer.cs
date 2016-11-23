using System.Drawing;
using System.Windows.Forms;
namespace ruyi
{
    partial class main
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRepairRcd = new System.Windows.Forms.Button();
            this.btnStateHistory = new System.Windows.Forms.Button();
            this.btnFaultWarn = new System.Windows.Forms.Button();
            this.btnOnlineDev = new System.Windows.Forms.Button();
            this.btnFaultCount = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlOnlineDev = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.portnumber = new System.Windows.Forms.TextBox();
            this.chooseOnlineDev = new System.Windows.Forms.Button();
            this.idChecked = new System.Windows.Forms.RadioButton();
            this.areaChecked = new System.Windows.Forms.RadioButton();
            this.comboBoxForChooseInOnlineDev = new System.Windows.Forms.ComboBox();
            this.connect = new System.Windows.Forms.Button();
            this.onlineDeviceInChooseArea = new System.Windows.Forms.ListView();
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timetext = new System.Windows.Forms.TextBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.onlineDevicetreeView = new System.Windows.Forms.TreeView();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlFaultWarn = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.clearlist = new System.Windows.Forms.Button();
            this.warnid = new System.Windows.Forms.RadioButton();
            this.warnnumber = new System.Windows.Forms.RadioButton();
            this.chooseError = new System.Windows.Forms.Button();
            this.comboBoxForChooseInWarn = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.endTimeInWarnFind = new System.Windows.Forms.DateTimePicker();
            this.beginTimeInWarnFind = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFindInWarnFind = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxForFindInWarnFind = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.warnHistoryList = new System.Windows.Forms.ListView();
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FaultWarnList = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlStatehistory = new System.Windows.Forms.Panel();
            this.theChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel6 = new System.Windows.Forms.Panel();
            this.Times = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.value = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.digitalName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxForFindInStateFind = new System.Windows.Forms.ComboBox();
            this.endMinuteTimeInStateFind = new System.Windows.Forms.DateTimePicker();
            this.beginMinuteTimeInStateFind = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.endMonthTimeInStateFind = new System.Windows.Forms.DateTimePicker();
            this.beginMonthTimeInStateFind = new System.Windows.Forms.DateTimePicker();
            this.label22 = new System.Windows.Forms.Label();
            this.btnStateHistoryFind = new System.Windows.Forms.Button();
            this.timetext2 = new System.Windows.Forms.TextBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.stateHistoryTreeView = new System.Windows.Forms.TreeView();
            this.pnlRepairRecord = new System.Windows.Forms.Panel();
            this.btnRepiarAdd = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.btnRepairFind = new System.Windows.Forms.Button();
            this.repairList = new System.Windows.Forms.ListView();
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBoxofidForRepair = new System.Windows.Forms.ComboBox();
            this.pnlFaultCount = new System.Windows.Forms.Panel();
            this.pnlsinglefind = new System.Windows.Forms.Panel();
            this.btnPrintInSingleCount = new System.Windows.Forms.Button();
            this.btnPrePrintInSingleCount = new System.Windows.Forms.Button();
            this.btnSetPrintInSingleCount = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.comboBoxForChooseInWarnCount = new System.Windows.Forms.ComboBox();
            this.singleCountFind = new System.Windows.Forms.Button();
            this.pnldatefind = new System.Windows.Forms.Panel();
            this.btnPrintInDateCount = new System.Windows.Forms.Button();
            this.btnPrePrintInDateCount = new System.Windows.Forms.Button();
            this.btnSetPrintInDateCount = new System.Windows.Forms.Button();
            this.endTimeInWarnCount = new System.Windows.Forms.DateTimePicker();
            this.beginTimeInWarnCount = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.dateCountFind = new System.Windows.Forms.Button();
            this.pnlallfind = new System.Windows.Forms.Panel();
            this.btnPrintInAllCount = new System.Windows.Forms.Button();
            this.btnPrePrintInAllCount = new System.Windows.Forms.Button();
            this.btnSetPrintInAllCount = new System.Windows.Forms.Button();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.allCountFind = new System.Windows.Forms.Button();
            this.btnCount3 = new System.Windows.Forms.Button();
            this.btnCount2 = new System.Windows.Forms.Button();
            this.btnCount1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog2 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument2 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog3 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument3 = new System.Drawing.Printing.PrintDocument();
            this.warningTimer = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.systemtimer = new System.Windows.Forms.Timer(this.components);
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog2 = new System.Windows.Forms.PrintDialog();
            this.pageSetupDialog2 = new System.Windows.Forms.PageSetupDialog();
            this.pageSetupDialog3 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog3 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlOnlineDev.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlFaultWarn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlStatehistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theChart)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pnlRepairRecord.SuspendLayout();
            this.pnlFaultCount.SuspendLayout();
            this.pnlsinglefind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.pnldatefind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.pnlallfind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(180)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.label11.Name = "label11";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::ruyi.Properties.Resources.logo;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.pnlOnlineDev);
            this.splitContainer2.Panel2.Controls.Add(this.pnlFaultWarn);
            this.splitContainer2.Panel2.Controls.Add(this.pnlStatehistory);
            this.splitContainer2.Panel2.Controls.Add(this.pnlRepairRecord);
            this.splitContainer2.Panel2.Controls.Add(this.pnlFaultCount);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnRepairRcd, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStateHistory, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFaultWarn, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOnlineDev, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFaultCount, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 9, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnRepairRcd
            // 
            resources.ApplyResources(this.btnRepairRcd, "btnRepairRcd");
            this.btnRepairRcd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnRepairRcd.ForeColor = System.Drawing.Color.White;
            this.btnRepairRcd.Name = "btnRepairRcd";
            this.btnRepairRcd.UseVisualStyleBackColor = false;
            this.btnRepairRcd.Click += new System.EventHandler(this.btnRepairRecordAdd_Click);
            // 
            // btnStateHistory
            // 
            resources.ApplyResources(this.btnStateHistory, "btnStateHistory");
            this.btnStateHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnStateHistory.ForeColor = System.Drawing.Color.White;
            this.btnStateHistory.Name = "btnStateHistory";
            this.btnStateHistory.UseVisualStyleBackColor = false;
            this.btnStateHistory.Click += new System.EventHandler(this.btnStatehistory_Click);
            // 
            // btnFaultWarn
            // 
            resources.ApplyResources(this.btnFaultWarn, "btnFaultWarn");
            this.btnFaultWarn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnFaultWarn.ForeColor = System.Drawing.Color.White;
            this.btnFaultWarn.Name = "btnFaultWarn";
            this.btnFaultWarn.UseVisualStyleBackColor = false;
            this.btnFaultWarn.Click += new System.EventHandler(this.faultwarn_Click);
            // 
            // btnOnlineDev
            // 
            this.btnOnlineDev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.btnOnlineDev, "btnOnlineDev");
            this.btnOnlineDev.ForeColor = System.Drawing.Color.White;
            this.btnOnlineDev.Name = "btnOnlineDev";
            this.btnOnlineDev.UseVisualStyleBackColor = false;
            this.btnOnlineDev.Click += new System.EventHandler(this.onlinedev_Click);
            // 
            // btnFaultCount
            // 
            this.btnFaultCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.btnFaultCount, "btnFaultCount");
            this.btnFaultCount.ForeColor = System.Drawing.Color.White;
            this.btnFaultCount.Name = "btnFaultCount";
            this.btnFaultCount.UseVisualStyleBackColor = false;
            this.btnFaultCount.Click += new System.EventHandler(this.faultCount_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.exit_Click);
            // 
            // pnlOnlineDev
            // 
            this.pnlOnlineDev.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlOnlineDev, "pnlOnlineDev");
            this.pnlOnlineDev.Controls.Add(this.panel2);
            this.pnlOnlineDev.Controls.Add(this.onlineDeviceInChooseArea);
            this.pnlOnlineDev.Controls.Add(this.timetext);
            this.pnlOnlineDev.Controls.Add(this.monthCalendar1);
            this.pnlOnlineDev.Controls.Add(this.panel1);
            this.pnlOnlineDev.Controls.Add(this.pictureBox2);
            this.pnlOnlineDev.Controls.Add(this.label5);
            this.pnlOnlineDev.Controls.Add(this.label4);
            this.pnlOnlineDev.Name = "pnlOnlineDev";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.portnumber);
            this.panel2.Controls.Add(this.chooseOnlineDev);
            this.panel2.Controls.Add(this.idChecked);
            this.panel2.Controls.Add(this.areaChecked);
            this.panel2.Controls.Add(this.comboBoxForChooseInOnlineDev);
            this.panel2.Controls.Add(this.connect);
            this.panel2.Name = "panel2";
            // 
            // portnumber
            // 
            this.portnumber.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.portnumber, "portnumber");
            this.portnumber.Name = "portnumber";
            // 
            // chooseOnlineDev
            // 
            resources.ApplyResources(this.chooseOnlineDev, "chooseOnlineDev");
            this.chooseOnlineDev.Name = "chooseOnlineDev";
            this.chooseOnlineDev.UseVisualStyleBackColor = true;
            this.chooseOnlineDev.Click += new System.EventHandler(this.chooseOnlineDev_Click);
            // 
            // idChecked
            // 
            resources.ApplyResources(this.idChecked, "idChecked");
            this.idChecked.Checked = true;
            this.idChecked.Name = "idChecked";
            this.idChecked.TabStop = true;
            this.idChecked.UseVisualStyleBackColor = true;
            // 
            // areaChecked
            // 
            resources.ApplyResources(this.areaChecked, "areaChecked");
            this.areaChecked.Name = "areaChecked";
            this.areaChecked.UseVisualStyleBackColor = true;
            // 
            // comboBoxForChooseInOnlineDev
            // 
            this.comboBoxForChooseInOnlineDev.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.comboBoxForChooseInOnlineDev, "comboBoxForChooseInOnlineDev");
            this.comboBoxForChooseInOnlineDev.FormattingEnabled = true;
            this.comboBoxForChooseInOnlineDev.Name = "comboBoxForChooseInOnlineDev";
            // 
            // connect
            // 
            resources.ApplyResources(this.connect, "connect");
            this.connect.BackColor = System.Drawing.Color.Transparent;
            this.connect.ForeColor = System.Drawing.Color.Black;
            this.connect.Name = "connect";
            this.connect.UseVisualStyleBackColor = false;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // onlineDeviceInChooseArea
            // 
            resources.ApplyResources(this.onlineDeviceInChooseArea, "onlineDeviceInChooseArea");
            this.onlineDeviceInChooseArea.BackColor = System.Drawing.SystemColors.ControlLight;
            this.onlineDeviceInChooseArea.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader8});
            this.onlineDeviceInChooseArea.FullRowSelect = true;
            this.onlineDeviceInChooseArea.GridLines = true;
            this.onlineDeviceInChooseArea.MultiSelect = false;
            this.onlineDeviceInChooseArea.Name = "onlineDeviceInChooseArea";
            this.onlineDeviceInChooseArea.UseCompatibleStateImageBehavior = false;
            this.onlineDeviceInChooseArea.View = System.Windows.Forms.View.Details;
            this.onlineDeviceInChooseArea.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.onlineDeviceInChooseArea_ColumnClick);
            this.onlineDeviceInChooseArea.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.onlineDeviceInChooseArea_MouseDoubleClick);
            // 
            // columnHeader24
            // 
            resources.ApplyResources(this.columnHeader24, "columnHeader24");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // timetext
            // 
            resources.ApplyResources(this.timetext, "timetext");
            this.timetext.BackColor = System.Drawing.SystemColors.ControlLight;
            this.timetext.Name = "timetext";
            // 
            // monthCalendar1
            // 
            resources.ApplyResources(this.monthCalendar1, "monthCalendar1");
            this.monthCalendar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.monthCalendar1.Name = "monthCalendar1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.onlineDevicetreeView);
            this.panel1.Name = "panel1";
            // 
            // onlineDevicetreeView
            // 
            this.onlineDevicetreeView.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.onlineDevicetreeView, "onlineDevicetreeView");
            this.onlineDevicetreeView.Name = "onlineDevicetreeView";
            this.onlineDevicetreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.onlineDevicetreeView_NodeMouseClick);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Image = global::ruyi.Properties.Resources._2012312;
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // pnlFaultWarn
            // 
            this.pnlFaultWarn.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlFaultWarn, "pnlFaultWarn");
            this.pnlFaultWarn.Controls.Add(this.splitContainer3);
            this.pnlFaultWarn.Name = "pnlFaultWarn";
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panel4);
            this.splitContainer3.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.label8);
            this.splitContainer3.Panel2.Controls.Add(this.label7);
            this.splitContainer3.Panel2.Controls.Add(this.warnHistoryList);
            this.splitContainer3.Panel2.Controls.Add(this.FaultWarnList);
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.clearlist);
            this.panel4.Controls.Add(this.warnid);
            this.panel4.Controls.Add(this.warnnumber);
            this.panel4.Controls.Add(this.chooseError);
            this.panel4.Controls.Add(this.comboBoxForChooseInWarn);
            this.panel4.Name = "panel4";
            // 
            // clearlist
            // 
            resources.ApplyResources(this.clearlist, "clearlist");
            this.clearlist.Name = "clearlist";
            this.clearlist.UseVisualStyleBackColor = true;
            this.clearlist.Click += new System.EventHandler(this.clearlist_Click);
            // 
            // warnid
            // 
            resources.ApplyResources(this.warnid, "warnid");
            this.warnid.Checked = true;
            this.warnid.Name = "warnid";
            this.warnid.TabStop = true;
            this.warnid.UseVisualStyleBackColor = true;
            // 
            // warnnumber
            // 
            resources.ApplyResources(this.warnnumber, "warnnumber");
            this.warnnumber.Name = "warnnumber";
            this.warnnumber.UseVisualStyleBackColor = true;
            // 
            // chooseError
            // 
            resources.ApplyResources(this.chooseError, "chooseError");
            this.chooseError.Name = "chooseError";
            this.chooseError.UseVisualStyleBackColor = true;
            this.chooseError.Click += new System.EventHandler(this.chooseError_Click);
            // 
            // comboBoxForChooseInWarn
            // 
            this.comboBoxForChooseInWarn.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.comboBoxForChooseInWarn, "comboBoxForChooseInWarn");
            this.comboBoxForChooseInWarn.FormattingEnabled = true;
            this.comboBoxForChooseInWarn.Name = "comboBoxForChooseInWarn";
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.endTimeInWarnFind);
            this.panel3.Controls.Add(this.beginTimeInWarnFind);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.btnFindInWarnFind);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.comboBoxForFindInWarnFind);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Name = "panel3";
            // 
            // endTimeInWarnFind
            // 
            resources.ApplyResources(this.endTimeInWarnFind, "endTimeInWarnFind");
            this.endTimeInWarnFind.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimeInWarnFind.Name = "endTimeInWarnFind";
            // 
            // beginTimeInWarnFind
            // 
            resources.ApplyResources(this.beginTimeInWarnFind, "beginTimeInWarnFind");
            this.beginTimeInWarnFind.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginTimeInWarnFind.Name = "beginTimeInWarnFind";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnFindInWarnFind
            // 
            resources.ApplyResources(this.btnFindInWarnFind, "btnFindInWarnFind");
            this.btnFindInWarnFind.Name = "btnFindInWarnFind";
            this.btnFindInWarnFind.UseVisualStyleBackColor = true;
            this.btnFindInWarnFind.Click += new System.EventHandler(this.btnFindInWarnFind_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // comboBoxForFindInWarnFind
            // 
            this.comboBoxForFindInWarnFind.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.comboBoxForFindInWarnFind, "comboBoxForFindInWarnFind");
            this.comboBoxForFindInWarnFind.FormattingEnabled = true;
            this.comboBoxForFindInWarnFind.Name = "comboBoxForFindInWarnFind";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // warnHistoryList
            // 
            resources.ApplyResources(this.warnHistoryList, "warnHistoryList");
            this.warnHistoryList.BackColor = System.Drawing.SystemColors.ControlLight;
            this.warnHistoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader22});
            this.warnHistoryList.FullRowSelect = true;
            this.warnHistoryList.GridLines = true;
            this.warnHistoryList.MultiSelect = false;
            this.warnHistoryList.Name = "warnHistoryList";
            this.warnHistoryList.ShowItemToolTips = true;
            this.warnHistoryList.UseCompatibleStateImageBehavior = false;
            this.warnHistoryList.View = System.Windows.Forms.View.Details;
            this.warnHistoryList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.warnHistoryList_ColumnClick);
            this.warnHistoryList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.warnHistoryList_MouseDoubleClick);
            // 
            // columnHeader10
            // 
            resources.ApplyResources(this.columnHeader10, "columnHeader10");
            // 
            // columnHeader11
            // 
            resources.ApplyResources(this.columnHeader11, "columnHeader11");
            // 
            // columnHeader12
            // 
            resources.ApplyResources(this.columnHeader12, "columnHeader12");
            // 
            // columnHeader22
            // 
            resources.ApplyResources(this.columnHeader22, "columnHeader22");
            // 
            // FaultWarnList
            // 
            resources.ApplyResources(this.FaultWarnList, "FaultWarnList");
            this.FaultWarnList.BackColor = System.Drawing.SystemColors.ControlLight;
            this.FaultWarnList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader21});
            this.FaultWarnList.FullRowSelect = true;
            this.FaultWarnList.GridLines = true;
            this.FaultWarnList.MultiSelect = false;
            this.FaultWarnList.Name = "FaultWarnList";
            this.FaultWarnList.UseCompatibleStateImageBehavior = false;
            this.FaultWarnList.View = System.Windows.Forms.View.Details;
            this.FaultWarnList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.FaultWarnList_ColumnClick);
            this.FaultWarnList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FaultWarnList_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader21
            // 
            resources.ApplyResources(this.columnHeader21, "columnHeader21");
            // 
            // pnlStatehistory
            // 
            this.pnlStatehistory.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlStatehistory, "pnlStatehistory");
            this.pnlStatehistory.Controls.Add(this.theChart);
            this.pnlStatehistory.Controls.Add(this.panel6);
            this.pnlStatehistory.Controls.Add(this.panel5);
            this.pnlStatehistory.Controls.Add(this.timetext2);
            this.pnlStatehistory.Controls.Add(this.pictureBox3);
            this.pnlStatehistory.Controls.Add(this.stateHistoryTreeView);
            this.pnlStatehistory.Name = "pnlStatehistory";
            // 
            // theChart
            // 
            resources.ApplyResources(this.theChart, "theChart");
            this.theChart.BackColor = System.Drawing.SystemColors.ControlLight;
            this.theChart.BorderlineColor = System.Drawing.SystemColors.ControlLight;
            this.theChart.BorderSkin.BackColor = System.Drawing.SystemColors.ControlLight;
            this.theChart.BorderSkin.PageColor = System.Drawing.SystemColors.ControlLight;
            chartArea1.BackColor = System.Drawing.SystemColors.ControlLight;
            chartArea1.Name = "ChartArea1";
            chartArea1.ShadowColor = System.Drawing.SystemColors.ControlLight;
            this.theChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.theChart.Legends.Add(legend1);
            this.theChart.Name = "theChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.LabelBackColor = System.Drawing.SystemColors.ControlLight;
            series1.LabelForeColor = System.Drawing.SystemColors.ControlLight;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.SmartLabelStyle.CalloutBackColor = System.Drawing.SystemColors.ControlLight;
            this.theChart.Series.Add(series1);
            this.theChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.theChart_MouseMove);
            // 
            // panel6
            // 
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.Controls.Add(this.Times);
            this.panel6.Controls.Add(this.label24);
            this.panel6.Controls.Add(this.value);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.digitalName);
            this.panel6.Controls.Add(this.label1);
            this.panel6.ForeColor = System.Drawing.Color.Black;
            this.panel6.Name = "panel6";
            // 
            // Times
            // 
            this.Times.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.Times, "Times");
            this.Times.Name = "Times";
            this.Times.ReadOnly = true;
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // value
            // 
            this.value.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.value, "value");
            this.value.Name = "value";
            this.value.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // digitalName
            // 
            this.digitalName.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.digitalName, "digitalName");
            this.digitalName.Name = "digitalName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.comboBoxForFindInStateFind);
            this.panel5.Controls.Add(this.endMinuteTimeInStateFind);
            this.panel5.Controls.Add(this.beginMinuteTimeInStateFind);
            this.panel5.Controls.Add(this.label21);
            this.panel5.Controls.Add(this.endMonthTimeInStateFind);
            this.panel5.Controls.Add(this.beginMonthTimeInStateFind);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.btnStateHistoryFind);
            this.panel5.ForeColor = System.Drawing.Color.Black;
            this.panel5.Name = "panel5";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // comboBoxForFindInStateFind
            // 
            this.comboBoxForFindInStateFind.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.comboBoxForFindInStateFind, "comboBoxForFindInStateFind");
            this.comboBoxForFindInStateFind.FormattingEnabled = true;
            this.comboBoxForFindInStateFind.Name = "comboBoxForFindInStateFind";
            // 
            // endMinuteTimeInStateFind
            // 
            resources.ApplyResources(this.endMinuteTimeInStateFind, "endMinuteTimeInStateFind");
            this.endMinuteTimeInStateFind.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endMinuteTimeInStateFind.Name = "endMinuteTimeInStateFind";
            this.endMinuteTimeInStateFind.ShowUpDown = true;
            // 
            // beginMinuteTimeInStateFind
            // 
            resources.ApplyResources(this.beginMinuteTimeInStateFind, "beginMinuteTimeInStateFind");
            this.beginMinuteTimeInStateFind.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginMinuteTimeInStateFind.Name = "beginMinuteTimeInStateFind";
            this.beginMinuteTimeInStateFind.ShowUpDown = true;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // endMonthTimeInStateFind
            // 
            resources.ApplyResources(this.endMonthTimeInStateFind, "endMonthTimeInStateFind");
            this.endMonthTimeInStateFind.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endMonthTimeInStateFind.Name = "endMonthTimeInStateFind";
            // 
            // beginMonthTimeInStateFind
            // 
            resources.ApplyResources(this.beginMonthTimeInStateFind, "beginMonthTimeInStateFind");
            this.beginMonthTimeInStateFind.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginMonthTimeInStateFind.Name = "beginMonthTimeInStateFind";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // btnStateHistoryFind
            // 
            resources.ApplyResources(this.btnStateHistoryFind, "btnStateHistoryFind");
            this.btnStateHistoryFind.Name = "btnStateHistoryFind";
            this.btnStateHistoryFind.UseVisualStyleBackColor = true;
            this.btnStateHistoryFind.Click += new System.EventHandler(this.btnStateHistoryFind_Click);
            // 
            // timetext2
            // 
            resources.ApplyResources(this.timetext2, "timetext2");
            this.timetext2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.timetext2.Name = "timetext2";
            // 
            // pictureBox3
            // 
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Image = global::ruyi.Properties.Resources._2012312;
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // stateHistoryTreeView
            // 
            resources.ApplyResources(this.stateHistoryTreeView, "stateHistoryTreeView");
            this.stateHistoryTreeView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.stateHistoryTreeView.ForeColor = System.Drawing.Color.Black;
            this.stateHistoryTreeView.Name = "stateHistoryTreeView";
            this.stateHistoryTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes2"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes3"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes4"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes5"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes6"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("stateHistoryTreeView.Nodes7")))});
            this.stateHistoryTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.stateTreeView_AfterSelect);
            this.stateHistoryTreeView.DoubleClick += new System.EventHandler(this.stateTreeView_DoubleClick);
            // 
            // pnlRepairRecord
            // 
            this.pnlRepairRecord.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlRepairRecord, "pnlRepairRecord");
            this.pnlRepairRecord.Controls.Add(this.btnRepiarAdd);
            this.pnlRepairRecord.Controls.Add(this.label16);
            this.pnlRepairRecord.Controls.Add(this.btnRepairFind);
            this.pnlRepairRecord.Controls.Add(this.repairList);
            this.pnlRepairRecord.Controls.Add(this.comboBoxofidForRepair);
            this.pnlRepairRecord.Name = "pnlRepairRecord";
            // 
            // btnRepiarAdd
            // 
            resources.ApplyResources(this.btnRepiarAdd, "btnRepiarAdd");
            this.btnRepiarAdd.Name = "btnRepiarAdd";
            this.btnRepiarAdd.UseVisualStyleBackColor = true;
            this.btnRepiarAdd.Click += new System.EventHandler(this.btnRepiarAdd_Click);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // btnRepairFind
            // 
            resources.ApplyResources(this.btnRepairFind, "btnRepairFind");
            this.btnRepairFind.Name = "btnRepairFind";
            this.btnRepairFind.UseVisualStyleBackColor = true;
            this.btnRepairFind.Click += new System.EventHandler(this.btnRepairFind_Click);
            // 
            // repairList
            // 
            resources.ApplyResources(this.repairList, "repairList");
            this.repairList.BackColor = System.Drawing.SystemColors.ControlLight;
            this.repairList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader7,
            this.columnHeader1});
            this.repairList.GridLines = true;
            this.repairList.LabelEdit = true;
            this.repairList.Name = "repairList";
            this.repairList.UseCompatibleStateImageBehavior = false;
            this.repairList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader15
            // 
            resources.ApplyResources(this.columnHeader15, "columnHeader15");
            // 
            // columnHeader16
            // 
            resources.ApplyResources(this.columnHeader16, "columnHeader16");
            // 
            // columnHeader17
            // 
            resources.ApplyResources(this.columnHeader17, "columnHeader17");
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // comboBoxofidForRepair
            // 
            resources.ApplyResources(this.comboBoxofidForRepair, "comboBoxofidForRepair");
            this.comboBoxofidForRepair.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxofidForRepair.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBoxofidForRepair.FormattingEnabled = true;
            this.comboBoxofidForRepair.Name = "comboBoxofidForRepair";
            // 
            // pnlFaultCount
            // 
            this.pnlFaultCount.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlFaultCount, "pnlFaultCount");
            this.pnlFaultCount.Controls.Add(this.pnlsinglefind);
            this.pnlFaultCount.Controls.Add(this.pnldatefind);
            this.pnlFaultCount.Controls.Add(this.pnlallfind);
            this.pnlFaultCount.Controls.Add(this.btnCount3);
            this.pnlFaultCount.Controls.Add(this.btnCount2);
            this.pnlFaultCount.Controls.Add(this.btnCount1);
            this.pnlFaultCount.Name = "pnlFaultCount";
            // 
            // pnlsinglefind
            // 
            resources.ApplyResources(this.pnlsinglefind, "pnlsinglefind");
            this.pnlsinglefind.Controls.Add(this.btnPrintInSingleCount);
            this.pnlsinglefind.Controls.Add(this.btnPrePrintInSingleCount);
            this.pnlsinglefind.Controls.Add(this.btnSetPrintInSingleCount);
            this.pnlsinglefind.Controls.Add(this.label12);
            this.pnlsinglefind.Controls.Add(this.pictureBox4);
            this.pnlsinglefind.Controls.Add(this.pictureBox5);
            this.pnlsinglefind.Controls.Add(this.comboBoxForChooseInWarnCount);
            this.pnlsinglefind.Controls.Add(this.singleCountFind);
            this.pnlsinglefind.Name = "pnlsinglefind";
            // 
            // btnPrintInSingleCount
            // 
            resources.ApplyResources(this.btnPrintInSingleCount, "btnPrintInSingleCount");
            this.btnPrintInSingleCount.Name = "btnPrintInSingleCount";
            this.btnPrintInSingleCount.UseVisualStyleBackColor = true;
            this.btnPrintInSingleCount.Click += new System.EventHandler(this.btnPrintInSingleCount_Click);
            // 
            // btnPrePrintInSingleCount
            // 
            resources.ApplyResources(this.btnPrePrintInSingleCount, "btnPrePrintInSingleCount");
            this.btnPrePrintInSingleCount.Name = "btnPrePrintInSingleCount";
            this.btnPrePrintInSingleCount.UseVisualStyleBackColor = true;
            this.btnPrePrintInSingleCount.Click += new System.EventHandler(this.btnPrePrintInSingleCount_Click);
            // 
            // btnSetPrintInSingleCount
            // 
            resources.ApplyResources(this.btnSetPrintInSingleCount, "btnSetPrintInSingleCount");
            this.btnSetPrintInSingleCount.Name = "btnSetPrintInSingleCount";
            this.btnSetPrintInSingleCount.UseVisualStyleBackColor = true;
            this.btnSetPrintInSingleCount.Click += new System.EventHandler(this.btnSetPrintInSingleCount_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // pictureBox4
            // 
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            resources.ApplyResources(this.pictureBox5, "pictureBox5");
            this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.TabStop = false;
            // 
            // comboBoxForChooseInWarnCount
            // 
            resources.ApplyResources(this.comboBoxForChooseInWarnCount, "comboBoxForChooseInWarnCount");
            this.comboBoxForChooseInWarnCount.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBoxForChooseInWarnCount.FormattingEnabled = true;
            this.comboBoxForChooseInWarnCount.Name = "comboBoxForChooseInWarnCount";
            // 
            // singleCountFind
            // 
            resources.ApplyResources(this.singleCountFind, "singleCountFind");
            this.singleCountFind.Name = "singleCountFind";
            this.singleCountFind.UseVisualStyleBackColor = true;
            this.singleCountFind.Click += new System.EventHandler(this.CountFind_Click);
            // 
            // pnldatefind
            // 
            resources.ApplyResources(this.pnldatefind, "pnldatefind");
            this.pnldatefind.Controls.Add(this.btnPrintInDateCount);
            this.pnldatefind.Controls.Add(this.btnPrePrintInDateCount);
            this.pnldatefind.Controls.Add(this.btnSetPrintInDateCount);
            this.pnldatefind.Controls.Add(this.endTimeInWarnCount);
            this.pnldatefind.Controls.Add(this.beginTimeInWarnCount);
            this.pnldatefind.Controls.Add(this.label13);
            this.pnldatefind.Controls.Add(this.label14);
            this.pnldatefind.Controls.Add(this.pictureBox8);
            this.pnldatefind.Controls.Add(this.pictureBox9);
            this.pnldatefind.Controls.Add(this.dateCountFind);
            this.pnldatefind.Name = "pnldatefind";
            // 
            // btnPrintInDateCount
            // 
            resources.ApplyResources(this.btnPrintInDateCount, "btnPrintInDateCount");
            this.btnPrintInDateCount.Name = "btnPrintInDateCount";
            this.btnPrintInDateCount.UseVisualStyleBackColor = true;
            this.btnPrintInDateCount.Click += new System.EventHandler(this.btnPrintInDateCount_Click);
            // 
            // btnPrePrintInDateCount
            // 
            resources.ApplyResources(this.btnPrePrintInDateCount, "btnPrePrintInDateCount");
            this.btnPrePrintInDateCount.Name = "btnPrePrintInDateCount";
            this.btnPrePrintInDateCount.UseVisualStyleBackColor = true;
            this.btnPrePrintInDateCount.Click += new System.EventHandler(this.btnPrePrintInDateCount_Click);
            // 
            // btnSetPrintInDateCount
            // 
            resources.ApplyResources(this.btnSetPrintInDateCount, "btnSetPrintInDateCount");
            this.btnSetPrintInDateCount.Name = "btnSetPrintInDateCount";
            this.btnSetPrintInDateCount.UseVisualStyleBackColor = true;
            this.btnSetPrintInDateCount.Click += new System.EventHandler(this.btnSetPrintInDateCount_Click);
            // 
            // endTimeInWarnCount
            // 
            resources.ApplyResources(this.endTimeInWarnCount, "endTimeInWarnCount");
            this.endTimeInWarnCount.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimeInWarnCount.Name = "endTimeInWarnCount";
            // 
            // beginTimeInWarnCount
            // 
            resources.ApplyResources(this.beginTimeInWarnCount, "beginTimeInWarnCount");
            this.beginTimeInWarnCount.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginTimeInWarnCount.Name = "beginTimeInWarnCount";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // pictureBox8
            // 
            this.pictureBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox8, "pictureBox8");
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox9, "pictureBox9");
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.TabStop = false;
            // 
            // dateCountFind
            // 
            resources.ApplyResources(this.dateCountFind, "dateCountFind");
            this.dateCountFind.Name = "dateCountFind";
            this.dateCountFind.UseVisualStyleBackColor = true;
            this.dateCountFind.Click += new System.EventHandler(this.dateCountFind_Click);
            // 
            // pnlallfind
            // 
            resources.ApplyResources(this.pnlallfind, "pnlallfind");
            this.pnlallfind.Controls.Add(this.btnPrintInAllCount);
            this.pnlallfind.Controls.Add(this.btnPrePrintInAllCount);
            this.pnlallfind.Controls.Add(this.btnSetPrintInAllCount);
            this.pnlallfind.Controls.Add(this.pictureBox6);
            this.pnlallfind.Controls.Add(this.pictureBox7);
            this.pnlallfind.Controls.Add(this.allCountFind);
            this.pnlallfind.Name = "pnlallfind";
            // 
            // btnPrintInAllCount
            // 
            resources.ApplyResources(this.btnPrintInAllCount, "btnPrintInAllCount");
            this.btnPrintInAllCount.Name = "btnPrintInAllCount";
            this.btnPrintInAllCount.UseVisualStyleBackColor = true;
            this.btnPrintInAllCount.Click += new System.EventHandler(this.btnPrintInAllCount_Click);
            // 
            // btnPrePrintInAllCount
            // 
            resources.ApplyResources(this.btnPrePrintInAllCount, "btnPrePrintInAllCount");
            this.btnPrePrintInAllCount.Name = "btnPrePrintInAllCount";
            this.btnPrePrintInAllCount.UseVisualStyleBackColor = true;
            this.btnPrePrintInAllCount.Click += new System.EventHandler(this.btnPrePrintInAllCount_Click);
            // 
            // btnSetPrintInAllCount
            // 
            resources.ApplyResources(this.btnSetPrintInAllCount, "btnSetPrintInAllCount");
            this.btnSetPrintInAllCount.Name = "btnSetPrintInAllCount";
            this.btnSetPrintInAllCount.UseVisualStyleBackColor = true;
            this.btnSetPrintInAllCount.Click += new System.EventHandler(this.btnSetPrintInAllCount_Click);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox6, "pictureBox6");
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox7, "pictureBox7");
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.TabStop = false;
            // 
            // allCountFind
            // 
            resources.ApplyResources(this.allCountFind, "allCountFind");
            this.allCountFind.Name = "allCountFind";
            this.allCountFind.UseVisualStyleBackColor = true;
            this.allCountFind.Click += new System.EventHandler(this.allCountFind_Click);
            // 
            // btnCount3
            // 
            resources.ApplyResources(this.btnCount3, "btnCount3");
            this.btnCount3.Name = "btnCount3";
            this.btnCount3.UseVisualStyleBackColor = true;
            this.btnCount3.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnCount2
            // 
            resources.ApplyResources(this.btnCount2, "btnCount2");
            this.btnCount2.Name = "btnCount2";
            this.btnCount2.UseVisualStyleBackColor = true;
            this.btnCount2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnCount1
            // 
            resources.ApplyResources(this.btnCount1, "btnCount1");
            this.btnCount1.Name = "btnCount1";
            this.btnCount1.UseVisualStyleBackColor = true;
            this.btnCount1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // printPreviewDialog1
            // 
            resources.ApplyResources(this.printPreviewDialog1, "printPreviewDialog1");
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog2
            // 
            resources.ApplyResources(this.printPreviewDialog2, "printPreviewDialog2");
            this.printPreviewDialog2.Document = this.printDocument2;
            this.printPreviewDialog2.Name = "printPreviewDialog1";
            // 
            // printDocument2
            // 
            this.printDocument2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument2_PrintPage);
            // 
            // printPreviewDialog3
            // 
            resources.ApplyResources(this.printPreviewDialog3, "printPreviewDialog3");
            this.printPreviewDialog3.Document = this.printDocument3;
            this.printPreviewDialog3.Name = "printPreviewDialog1";
            // 
            // printDocument3
            // 
            this.printDocument3.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument3_PrintPage);
            // 
            // warningTimer
            // 
            this.warningTimer.Interval = 1000;
            this.warningTimer.Tick += new System.EventHandler(this.warningTimer_Tick);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // systemtimer
            // 
            this.systemtimer.Enabled = true;
            this.systemtimer.Interval = 1000;
            this.systemtimer.Tick += new System.EventHandler(this.systemtimer_Tick);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.printDocument1;
            this.pageSetupDialog1.EnableMetric = true;
            // 
            // printDialog2
            // 
            this.printDialog2.Document = this.printDocument2;
            this.printDialog2.UseEXDialog = true;
            // 
            // pageSetupDialog2
            // 
            this.pageSetupDialog2.Document = this.printDocument2;
            this.pageSetupDialog2.EnableMetric = true;
            // 
            // pageSetupDialog3
            // 
            this.pageSetupDialog3.Document = this.printDocument3;
            this.pageSetupDialog3.EnableMetric = true;
            // 
            // printDialog3
            // 
            this.printDialog3.Document = this.printDocument3;
            this.printDialog3.UseEXDialog = true;
            // 
            // main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlOnlineDev.ResumeLayout(false);
            this.pnlOnlineDev.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlFaultWarn.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnlStatehistory.ResumeLayout(false);
            this.pnlStatehistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theChart)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pnlRepairRecord.ResumeLayout(false);
            this.pnlRepairRecord.PerformLayout();
            this.pnlFaultCount.ResumeLayout(false);
            this.pnlsinglefind.ResumeLayout(false);
            this.pnlsinglefind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.pnldatefind.ResumeLayout(false);
            this.pnldatefind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.pnlallfind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Panel pnlOnlineDev;
        private Panel pnlFaultWarn;
        private Panel pnlStatehistory;
        private Panel pnlRepairRecord;
        private Button chooseOnlineDev;
        private ListView warnHistoryList;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private Label label16;
        private Button btnRepiarAdd;
        private ToolTip toolTip1;
        private Timer warningTimer;
        private Button btnRepairFind;
        private DateTimePicker beginMonthTimeInStateFind;
        private Label label22;
        private Button btnStateHistoryFind;
        private DateTimePicker endMonthTimeInStateFind;
        private Label label21;
        private Label label2;
        private DateTimePicker endTimeInWarnFind;
        private DateTimePicker beginTimeInWarnFind;
        private Label label3;
        private Button btnFindInWarnFind;
        private ListView repairList;
        private ColumnHeader columnHeader15;
        private ColumnHeader columnHeader16;
        private ColumnHeader columnHeader17;
        private TreeView onlineDevicetreeView;
        private ListView onlineDeviceInChooseArea;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader8;
        private RadioButton areaChecked;
        private RadioButton idChecked;
        private PictureBox pictureBox2;
        private ListView FaultWarnList;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader21;
        private RadioButton warnnumber;
        private RadioButton warnid;
        private Button chooseError;
        private ColumnHeader columnHeader22;
        private TextBox Times;
        private Label label24;
        private TextBox value;
        private Label label6;
        private TextBox digitalName;
        private Label label1;
        private TreeView stateHistoryTreeView;
        private Timer timer1;
        private TextBox portnumber;
        private Button connect;
        private MonthCalendar monthCalendar1;
        private Timer systemtimer;
        private ColumnHeader columnHeader24;
        private DateTimePicker beginMinuteTimeInStateFind;
        private DateTimePicker endMinuteTimeInStateFind;
        private TextBox timetext;
        private TextBox timetext2;
        private PictureBox pictureBox3;
        private SplitContainer splitContainer3;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnOnlineDev;
        private Button btnStateHistory;
        private ComboBox comboBoxForChooseInOnlineDev;
        private ComboBox comboBoxForFindInStateFind;
        private ComboBox comboBoxofidForRepair;
        private ComboBox comboBoxForFindInWarnFind;
        private ComboBox comboBoxForChooseInWarn;
        private Panel panel1;
        private Panel panel2;
        private Label label5;
        private Label label4;
        private Panel panel3;
        private Label label9;
        private Panel panel4;
        private Label label8;
        private Label label7;
        private Panel panel6;
        private Panel panel5;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader7;
        private System.Windows.Forms.DataVisualization.Charting.Chart theChart;
        private Label label10;
        private Label label11;
        private PictureBox pictureBox1;
        private Button btnExit;
        private Button btnRepairRcd;
        private Button btnFaultWarn;
        private Button btnFaultCount;
        private Panel pnlFaultCount;
        private PictureBox pictureBox5;
        private PictureBox pictureBox4;
        private Button singleCountFind;
        private ComboBox comboBoxForChooseInWarnCount;
        private Button btnCount3;
        private Button btnCount2;
        private Button btnCount1;
        private Panel pnlsinglefind;
        private Panel pnldatefind;
        private PictureBox pictureBox8;
        private PictureBox pictureBox9;
        private Button dateCountFind;
        private Panel pnlallfind;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private Button allCountFind;
        private Label label12;
        private DateTimePicker endTimeInWarnCount;
        private DateTimePicker beginTimeInWarnCount;
        private Label label13;
        private Label label14;
        private Button btnPrintInDateCount;
        private Button btnPrePrintInDateCount;
        private Button btnSetPrintInDateCount;
        private PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private PrintDialog printDialog1;
        private PageSetupDialog pageSetupDialog1;
        private Button btnPrintInSingleCount;
        private Button btnPrePrintInSingleCount;
        private Button btnSetPrintInSingleCount;
        private PrintPreviewDialog printPreviewDialog2;
        private System.Drawing.Printing.PrintDocument printDocument2;
        private PrintDialog printDialog2;
        private PageSetupDialog pageSetupDialog2;
        private PageSetupDialog pageSetupDialog3;
        private System.Drawing.Printing.PrintDocument printDocument3;
        private PrintDialog printDialog3;
        private PrintPreviewDialog printPreviewDialog3;
        private Button btnPrintInAllCount;
        private Button btnPrePrintInAllCount;
        private Button btnSetPrintInAllCount;
        private Button clearlist;
    }
}

