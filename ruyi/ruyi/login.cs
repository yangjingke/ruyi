using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace ruyi
{
    public partial class login : Form
    {


        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (password.Text == "12345" && userName.Text == "ruyi")
            {
                this.Hide();
                if (english.Checked == true)
                {
                    main serverForm = new main("en");
                    serverForm.Show();
                }
                else
                {
                    main serverForm = new main("default");
                    serverForm.Show();
                }
            }
            else if ((password.Text == "12345" && userName.Text == "ruyidanji"))
            {
                this.Hide();
                standalone standaloneForm = new standalone();
                standaloneForm.Show();
            }
            else if (password.Text == "12345" && userName.Text == "ruyijuyu")
            {
                this.Hide();
                if (english.Checked == true)
                {
                    local localForm = new local("en");
                    localForm.Show();
                }
                else
                {
                    local localForm = new local("default");
                    localForm.Show();
                }
            }
            else
            {
                MessageBox.Show(" 请正确输入账号密码 \n Login failed,please check your account and password");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ApplyResource()
        {
            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(login));
            foreach (Control ctl in Controls)
            {
                res.ApplyResources(ctl, ctl.Name);
            }
            ////菜单
            //foreach (ToolStripMenuItem item in this.menuStrip1.Items)
            //{
            //    res.ApplyResources(item, item.Name);
            //    foreach (ToolStripMenuItem subItem in item.DropDownItems)
            //    {
            //        res.ApplyResources(subItem, subItem.Name);
            //    }
            //}

            //Caption
            res.ApplyResources(this, "$this");
        }

        private void chinese_CheckedChanged(object sender, EventArgs e)
        {

        }


    }





 


}
