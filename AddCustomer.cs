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
using CRUD.Models;

namespace CRUD
{
    public partial class AddCustomer : Form
    {
        string liteDBPath = ConfigurationManager.AppSettings["DbPath"].ToString();
        private Main pForm;
        private Customer cust;
        private bool isEdit = false;


        public AddCustomer(Main prntForm,Customer c = null)
        {
            InitializeComponent();
            pForm = prntForm;
            cust = c;

            if (c != null) initForm();
        }

        private void initForm()
        {
            firstName.Text = cust.firstName;
            middleName.Text = cust.middleName;
            lastName.Text = cust.lastName;
            address.Text = cust.address;
            phone.Text = cust.phone;
            appointmentDate.Text = cust.appointmentDate.ToShortDateString();
            carEngine.Text = cust.engine;
            appointmentDetails.Text = cust.appointmentDetails;
            carMake.Text = cust.make;
            carModel.Text = cust.model;

            isEdit = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var helper = new CustomerHelper(liteDBPath);
            var customer = new Customer() {
                firstName = firstName.Text,
                middleName = middleName.Text,
                lastName = lastName.Text,
                address= address.Text,
                phone = phone.Text,
                appointmentDate = DateTime.Parse(appointmentDate.Text),
                engine = carEngine.Text,
                appointmentDetails= appointmentDetails.Text,
                make= carMake.Text,
                model= carModel.Text
            };

            if (isEdit)
            {
                customer.id = cust.id;
                helper.Update(customer);
            }
            else
            {
                customer.id = Guid.NewGuid();
                helper.Add(customer);
            }  
            

            pForm.RefreshGridView(helper.GetAll());
            
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
