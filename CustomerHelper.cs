using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiteDB;
using CRUD.Models;

namespace CRUD
{
    public class CustomerHelper
    {
        private string liteDBPath = "";

        public CustomerHelper(string databasePath)
        {
            this.liteDBPath = databasePath;
        }

        public IList<Customer> Get(Guid id)
        {
            var toReturn = new List<Customer>();

            using(var db = new LiteDatabase(liteDBPath))
            {
                var customers = db.GetCollection<Customer>("customers");
                IEnumerable<Customer> filteredCustomers;

                filteredCustomers = customers.Find(x => x.id.Equals(id));

                foreach(Customer c in filteredCustomers)
                {
                    toReturn.Add(c);
                }

                return toReturn.FindAll(x => x.id.Equals(id));
            }
        }

        public IList<Customer> GetAll()
        {
            var toReturn = new List<Customer>();
            using (var db= new LiteDatabase(liteDBPath))
            {
                var customers = db.GetCollection<Customer>("customers");
                var results = customers.FindAll();
                foreach(Customer c in results)
                {
                    toReturn.Add(c);
                }

                return toReturn;
            }
        }

        public IList<Customer> Filter(string field, DateTime from, DateTime to)
        {
            var toReturn = new List<Customer>();

            using (var db = new LiteDatabase(liteDBPath))
            {
                var customers = db.GetCollection<Customer>("customers");
                IEnumerable<Customer> filteredCustomers;

                filteredCustomers = customers.Find(x => x.appointmentDate >= from && x.appointmentDate <= to);
                toReturn = SetupCustomers(filteredCustomers);              

                return toReturn;
            }


        }

        public IList<Customer> Filter(string field, string value)
        {
            var toReturn = new List<Customer>();

            using (var db = new LiteDatabase(liteDBPath))
            {
                var customers = db.GetCollection<Customer>("customers");
                IEnumerable<Customer> filteredCustomers;

                switch (field)
                {
                    case "First Name":
                        filteredCustomers = customers.Find(x => x.firstName.ToLower().Contains(value.ToLower()));
                        toReturn = SetupCustomers(filteredCustomers);
                        break;
                    case "Middle Name":
                        filteredCustomers = customers.Find(x => x.middleName.ToLower().Contains(value.ToLower()));
                        toReturn = SetupCustomers(filteredCustomers);
                        break;
                    case "Last Name":
                        filteredCustomers = customers.Find(x => x.lastName.ToLower().Contains(value.ToLower()));
                        toReturn = SetupCustomers(filteredCustomers);
                        break;
                    case "Job Details":
                        filteredCustomers = customers.Find(x => x.appointmentDetails.ToLower().Contains(value.ToLower()));
                        toReturn = SetupCustomers(filteredCustomers);
                        break;
                }
                return toReturn;
            }
        }

        private List<Customer> SetupCustomers(IEnumerable<Customer> filteredCustomers)
        {
            var toReturn = new List<Customer>();

            foreach (Customer c in filteredCustomers)
            {
                toReturn.Add(c);
            }

            return toReturn;
        }


        public void Add(Customer c)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(liteDBPath))
            {
                var collection = db.GetCollection<Customer>("customers");
                // Insert a new issue document
                collection.Insert(c);
                collection.EnsureIndex(x => x.id);
            }

            
        }

        public void Update(Customer customer)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issueCollection = db.GetCollection<Customer>("customers");
                // Update an existing issue document
                issueCollection.Update(customer);
            }
        }

        public void Delete(Guid id)
        {
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issues = db.GetCollection<Customer>("customers");
                issues.Delete(i => i.id == id);
            }
        }

    }
}
