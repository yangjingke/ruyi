using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ruyi
{
    public partial class Exit : Form
    {
        public Exit()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(password.Text=="12345")
            {
                Application.ExitThread();
            }else
            {
                MessageBox.Show("请正确输入密码   \n Please enter the correct password");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
