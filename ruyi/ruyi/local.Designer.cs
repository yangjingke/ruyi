using System.Drawing;
using System.Windows.Forms;
namespace ruyi
{
    partial class local
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(local));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOnlineDev = new System.Windows.Forms.Button();
            this.btnFaultWarn = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlFaultWarn = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.warnid = new System.Windows.Forms.RadioButton();
            this.warnnumber = new System.Windows.Forms.RadioButton();
            this.chooseError = new System.Windows.Forms.Button();
            this.comboBoxForChooseInWarn = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.FaultWarnList = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlOnlineDev = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.warningTimer = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.systemtimer = new System.Windows.Forms.Timer(this.components);
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
            this.pnlFaultWarn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlOnlineDev.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
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
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnOnlineDev, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFaultWarn, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 8, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
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
            // btnFaultWarn
            // 
            resources.ApplyResources(this.btnFaultWarn, "btnFaultWarn");
            this.btnFaultWarn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.btnFaultWarn.ForeColor = System.Drawing.Color.White;
            this.btnFaultWarn.Name = "btnFaultWarn";
            this.btnFaultWarn.UseVisualStyleBackColor = false;
            this.btnFaultWarn.Click += new System.EventHandler(this.faultwarn_Click);
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
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.label7);
            this.splitContainer3.Panel2.Controls.Add(this.FaultWarnList);
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.warnid);
            this.panel4.Controls.Add(this.warnnumber);
            this.panel4.Controls.Add(this.chooseError);
            this.panel4.Controls.Add(this.comboBoxForChooseInWarn);
            this.panel4.Name = "panel4";
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
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
            // pnlOnlineDev
            // 
            this.pnlOnlineDev.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pnlOnlineDev, "pnlOnlineDev");
            this.pnlOnlineDev.Controls.Add(this.label5);
            this.pnlOnlineDev.Controls.Add(this.label4);
            this.pnlOnlineDev.Controls.Add(this.panel2);
            this.pnlOnlineDev.Controls.Add(this.onlineDeviceInChooseArea);
            this.pnlOnlineDev.Controls.Add(this.timetext);
            this.pnlOnlineDev.Controls.Add(this.monthCalendar1);
            this.pnlOnlineDev.Controls.Add(this.panel1);
            this.pnlOnlineDev.Controls.Add(this.pictureBox2);
            this.pnlOnlineDev.Name = "pnlOnlineDev";
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
            // warningTimer
            // 
            this.warningTimer.Interval = 1000;
            this.warningTimer.Tick += new System.EventHandler(this.warningTimer_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // systemtimer
            // 
            this.systemtimer.Enabled = true;
            this.systemtimer.Interval = 1000;
            this.systemtimer.Tick += new System.EventHandler(this.systemtimer_Tick);
            // 
            // local
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "local";
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
            this.pnlFaultWarn.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlOnlineDev.ResumeLayout(false);
            this.pnlOnlineDev.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Panel pnlOnlineDev;
        private Panel pnlFaultWarn;
        private Button chooseOnlineDev;
        private ToolTip toolTip1;
        private Timer warningTimer;
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
        private Timer timer1;
        private TextBox portnumber;
        private Button connect;
        private MonthCalendar monthCalendar1;
        private Timer systemtimer;
        private ColumnHeader columnHeader24;
        private TextBox timetext;
        private SplitContainer splitContainer3;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnOnlineDev;
        private ComboBox comboBoxForChooseInOnlineDev;
        private ComboBox comboBoxForChooseInWarn;
        private Panel panel1;
        private Panel panel2;
        private Label label5;
        private Label label4;
        private Panel panel4;
        private Label label7;
        private Label label11;
        private PictureBox pictureBox1;
        private Button btnFaultWarn;
        private Button btnExit;
    }
}

