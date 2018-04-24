using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.ShowDialog();
        }

        private void btnJobs_Click(object sender, EventArgs e)
        {
            Main childForm = new Main();
            childForm.TopLevel = true;
            childForm.Owner = this;
            childForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Finance childForm = new Finance();
            childForm.TopLevel = true;
            childForm.Owner = this;
            childForm.Show();
        }
    }
}
