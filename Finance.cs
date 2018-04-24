using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using CRUD.Models;
using System.Data;
using System.Globalization;

namespace CRUD
{
    public partial class Finance : Form
    {
        string liteDBPath = ConfigurationManager.AppSettings["DbPath"].ToString();
        Guid selectedIssueItem = Guid.Empty;
        DataGridViewRow selectedRow = null;

        public Finance()
        {
            InitializeComponent();

            var financeHelper = new FinanceHelper(liteDBPath);

            RefreshGridView(financeHelper.GetAll());

            dataGridView1.RowEnter += dataGridView1_RowEnter;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        public void RefreshGridView(IList<FinanceModel> finances)
        {
            var bindingList = new BindingList<FinanceModel>(finances);
            var source = new BindingSource(bindingList, null);

            /*
            var dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
                new DataColumn("id"),
                new DataColumn("firstName",typeof(String)),
                new DataColumn("lastName",typeof(String)),
                new DataColumn("middleName",typeof(String)),
                new DataColumn("address",typeof(String)),
                new DataColumn("phone",typeof(String)),
                new DataColumn("appointmentDate",typeof(DateTime)),
                new DataColumn("model",typeof(String)),
                new DataColumn("make",typeof(String)),
                new DataColumn("engine",typeof(String)),
                new DataColumn("appointment",typeof(String))
            });           

            foreach (var item in customers)
            {
                dt.Rows.Add(item);   
            }
            */

            dataGridView1.DataSource = source;


            for (int i = 0; i <= 5; i++)
            {
                dataGridView1.Columns[i].ReadOnly = true;
                dataGridView1.Columns[i].Width = 200;
            }

            dataGridView1.Columns[2].HeaderText = "Date";
            dataGridView1.Columns[3].HeaderText = "Money In";
            dataGridView1.Columns[4].HeaderText = "Money Out";
            dataGridView1.Columns[5].HeaderText = "Cash On Hand";

            for (int i = 3; i <= 5; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.Format = "c2";
                dataGridView1.Columns[i].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-PH");
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count > 0)
            {
                var row = ((DataGridView)sender).SelectedRows[0];
                selectedRow = row;
                selectedIssueItem = Guid.Parse(row.Cells["id"].Value.ToString());
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddFinance frm = new AddFinance(this);
            frm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            showFinance();
        }

        private void showFinance()
        {
            var financeModel = new FinanceModel()
            {
                id = Guid.Parse(selectedRow.Cells[0].Value.ToString()),
                cashOnHand =double.Parse(selectedRow.Cells[5].Value.ToString()),
                moneyIn = double.Parse(selectedRow.Cells[3].Value.ToString()),
                moneyOut = double.Parse(selectedRow.Cells[4].Value.ToString()),
                jobDate = DateTime.Parse(selectedRow.Cells[2].Value.ToString()),
            };

            AddFinance frm = new AddFinance(this, financeModel);
            frm.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count > 0)
            {
                var row = ((DataGridView)sender).SelectedRows[0];
                selectedRow = row;
                selectedIssueItem = Guid.Parse(row.Cells["id"].Value.ToString());

                showFinance();

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete?";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, "Delete Customer", buttons);

            if (result == DialogResult.Yes)
            {
                var helper = new FinanceHelper(liteDBPath);
                helper.Delete(selectedIssueItem);
                RefreshGridView(helper.GetAll());
            }
            else
            {

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var helper = new FinanceHelper(liteDBPath);
            RefreshGridView(helper.Filter(dpFrom.Value, dpTo.Value));
        }
    }
}
