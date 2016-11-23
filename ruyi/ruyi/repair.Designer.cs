namespace ruyi
{
    partial class repair
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(repair));
            this.repairMan = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.idText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.repairTime = new System.Windows.Forms.DateTimePicker();
            this.remark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRepiarAdd = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.repairOrChange = new System.Windows.Forms.ComboBox();
            this.controlName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // repairMan
            // 
            resources.ApplyResources(this.repairMan, "repairMan");
            this.repairMan.Name = "repairMan";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // idText
            // 
            resources.ApplyResources(this.idText, "idText");
            this.idText.Name = "idText";
            this.idText.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // repairTime
            // 
            resources.ApplyResources(this.repairTime, "repairTime");
            this.repairTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.repairTime.Name = "repairTime";
            // 
            // remark
            // 
            this.remark.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.remark, "remark");
            this.remark.ForeColor = System.Drawing.SystemColors.WindowText;
            this.remark.Name = "remark";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnRepiarAdd
            // 
            resources.ApplyResources(this.btnRepiarAdd, "btnRepiarAdd");
            this.btnRepiarAdd.Name = "btnRepiarAdd";
            this.btnRepiarAdd.UseVisualStyleBackColor = true;
            this.btnRepiarAdd.Click += new System.EventHandler(this.btnRepiarAdd_Click);
            // 
            // btncancel
            // 
            resources.ApplyResources(this.btncancel, "btncancel");
            this.btncancel.Name = "btncancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // repairOrChange
            // 
            this.repairOrChange.FormattingEnabled = true;
            this.repairOrChange.Items.AddRange(new object[] {
            resources.GetString("repairOrChange.Items"),
            resources.GetString("repairOrChange.Items1")});
            resources.ApplyResources(this.repairOrChange, "repairOrChange");
            this.repairOrChange.Name = "repairOrChange";
            // 
            // controlName
            // 
            this.controlName.FormattingEnabled = true;
            this.controlName.Items.AddRange(new object[] {
            resources.GetString("controlName.Items"),
            resources.GetString("controlName.Items1"),
            resources.GetString("controlName.Items2"),
            resources.GetString("controlName.Items3"),
            resources.GetString("controlName.Items4"),
            resources.GetString("controlName.Items5"),
            resources.GetString("controlName.Items6"),
            resources.GetString("controlName.Items7")});
            resources.ApplyResources(this.controlName, "controlName");
            this.controlName.Name = "controlName";
            // 
            // repair
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.controlName);
            this.Controls.Add(this.repairOrChange);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnRepiarAdd);
            this.Controls.Add(this.remark);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.repairTime);
            this.Controls.Add(this.repairMan);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.idText);
            this.Controls.Add(this.label1);
            this.Name = "repair";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox repairMan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox idText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker repairTime;
        private System.Windows.Forms.TextBox remark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRepiarAdd;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.ComboBox repairOrChange;
        private System.Windows.Forms.ComboBox controlName;
    }
}