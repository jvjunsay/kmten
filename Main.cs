using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using CRUD.Models;
using System.Data;

namespace CRUD
{
    public partial class Main : Form
    {

        string liteDBPath = ConfigurationManager.AppSettings["DbPath"].ToString();
        Guid selectedIssueItem = Guid.Empty;
        DataGridViewRow selectedRow = null;

        public Main()
        {
            InitializeComponent();

            var customerHelper = new CustomerHelper(liteDBPath);

            RefreshGridView(customerHelper.GetAll());

            dataGridView1.RowEnter += dataGridView1_RowEnter;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }

        public void RefreshGridView(IList<Customer> customers)
        {
            var bindingList = new BindingList<Customer>(customers);
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
            

            for (int i = 0; i <= 10; i++)
            {
                dataGridView1.Columns[i].ReadOnly = true;
                dataGridView1.Columns[i].Width = 200;
            }
            
            dataGridView1.Columns[1].HeaderText = "Last Name";
            dataGridView1.Columns[2].HeaderText = "First Name";
            dataGridView1.Columns[3].HeaderText = "Middle Name";
            dataGridView1.Columns[4].HeaderText = "Address";
            dataGridView1.Columns[5].HeaderText = "Phone";
            dataGridView1.Columns[6].HeaderText = "Job Date";
            dataGridView1.Columns[7].HeaderText = "Car Model";
            dataGridView1.Columns[8].HeaderText = "Car Make";
            dataGridView1.Columns[9].HeaderText = "Plate Number";
            dataGridView1.Columns[10].HeaderText = "Job Details";


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

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddCustomer frm = new AddCustomer(this);
            frm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            showCustomer();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete?";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, "Delete Customer", buttons);

            if(result == DialogResult.Yes)
            {
                var helper = new CustomerHelper(liteDBPath);
                helper.Delete(selectedIssueItem);
                RefreshGridView(helper.GetAll());
            }
            else
            {
                
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count > 0)
            {
                var row = ((DataGridView)sender).SelectedRows[0];
                selectedRow = row;
                selectedIssueItem = Guid.Parse(row.Cells["id"].Value.ToString());

                showCustomer();

            }
        }

        private void showCustomer()
        {
            var customer = new Customer()
            {
                id = Guid.Parse(selectedRow.Cells[0].Value.ToString()),
                address =(selectedRow.Cells[5].Value!=null)?selectedRow.Cells[5].Value.ToString():"",
                appointmentDate = DateTime.Parse(selectedRow.Cells[6].Value.ToString()),
                appointmentDetails = (selectedRow.Cells[10].Value!=null) ?selectedRow.Cells[10].Value.ToString():"",
                engine = (selectedRow.Cells[9].Value!=null) ?selectedRow.Cells[9].Value.ToString():"",
                firstName =(selectedRow.Cells[2].Value!=null) ?selectedRow.Cells[2].Value.ToString():"",
                lastName = (selectedRow.Cells[1].Value != null) ? selectedRow.Cells[1].Value.ToString() : "",
                make = (selectedRow.Cells[8].Value != null) ? selectedRow.Cells[8].Value.ToString() : "",
                middleName = (selectedRow.Cells[3].Value != null) ? selectedRow.Cells[3].Value.ToString() : "",
                model = (selectedRow.Cells[7].Value != null) ? selectedRow.Cells[7].Value.ToString() : "",
                phone = (selectedRow.Cells[4].Value != null) ? selectedRow.Cells[4].Value.ToString() : "",
            };

            AddCustomer frm = new AddCustomer(this, customer);
            frm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var helper = new CustomerHelper(liteDBPath);
            if(cmbSearch.Text == "Job Date")
                RefreshGridView(helper.Filter(cmbSearch.Text,dpFrom.Value,dpTo.Value));
            else
                RefreshGridView(helper.Filter(cmbSearch.Text, txtSearch.Text));
        }

        private void cmbSearch_TextChanged(object sender, EventArgs e)
        {
            if(cmbSearch.Text == "Job Date")
            {
                txtSearch.Visible = false;
                dpFrom.Visible = true;
                dpTo.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
                dpFrom.Visible = false;
                dpTo.Visible = false;
            }
        }
    }
}
