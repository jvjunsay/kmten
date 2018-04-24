using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using CRUD.Models;
using System;
using System.Globalization;

namespace CRUD
{
    public partial class AddFinance : Form
    {
        string liteDBPath = ConfigurationManager.AppSettings["DbPath"].ToString();
        private Finance pForm;
        private FinanceModel finance;
        private bool isEdit = false;
        private CultureInfo culture = CultureInfo.GetCultureInfo("en-PH");

        public AddFinance()
        {
            InitializeComponent();
        }

        public AddFinance(Finance prntForm, FinanceModel c = null)
        {
            InitializeComponent();
            pForm = prntForm;
            finance = c;

            if (c != null) initForm();
        }

        private void initForm()
        {
            textBox1.Text = finance.moneyIn.ToString();
            txtNoneyOut.Text = finance.moneyOut.ToString();
            txtCashOnHand.Text = finance.cashOnHand.ToString();
            jobDate.Text = finance.jobDate.ToShortDateString();

            isEdit = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != '.';
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
           
            textBox1.Text = string.Format(culture,"{0:N}", double.Parse(textBox1.Text));
        }

        private void txtNoneyOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != '.';
        }

        private void txtNoneyOut_Leave(object sender, EventArgs e)
        {
            txtNoneyOut.Text = string.Format(culture, "{0:N}", double.Parse(txtNoneyOut.Text));
        }

        private void txtCashOnHand_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != '.';
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var helper = new FinanceHelper(liteDBPath);
            var financeModel = new FinanceModel()
            {
                jobDate = DateTime.Parse(jobDate.Text),
                cashOnHand =double.Parse(String.IsNullOrEmpty(txtCashOnHand.Text)?"0": txtCashOnHand.Text),
                moneyIn = double.Parse(String.IsNullOrEmpty(textBox1.Text) ? "0" : textBox1.Text),
                moneyOut = double.Parse(String.IsNullOrEmpty(txtNoneyOut.Text) ? "0" : txtNoneyOut.Text)

            };

            if (isEdit)
            {
                financeModel.id = finance.id;
                helper.Update(financeModel);
            }
            else
            {
                financeModel.id = Guid.NewGuid();
                helper.Add(financeModel);
            }


            pForm.RefreshGridView(helper.GetAll());

            this.Close();
        }

        
        private void txtCashOnHand_Leave_1(object sender, EventArgs e)
        {
            txtCashOnHand.Text = string.Format(culture, "{0:N}", double.Parse(txtCashOnHand.Text));
        }
    }
}
