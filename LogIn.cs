using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace CRUD
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var userName = ConfigurationManager.AppSettings["username"].ToString();
            var password = ConfigurationManager.AppSettings["password"].ToString();

            if(txtUserName.Text  == userName && txtPassword.Text == password)
            {
                MDIParent1 mdi = new MDIParent1();
                this.Hide();
                mdi.ShowDialog();
                this.Close();
            }else
            {
                MessageBox.Show("Invalid Login");
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToInt32(e.KeyCode) == 13)
            {
                button1.PerformClick();
            }
        }
    }
}
