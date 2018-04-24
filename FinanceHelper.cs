using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiteDB;
using CRUD.Models;

namespace CRUD
{
    public class FinanceHelper
    {
        private string liteDBPath = "";

        public FinanceHelper(string databasePath)
        {
            this.liteDBPath = databasePath;
        }

        public IList<FinanceModel> Get(Guid id)
        {
            var toReturn = new List<FinanceModel>();

            using (var db = new LiteDatabase(liteDBPath))
            {
                var finances = db.GetCollection<FinanceModel>("finance");
                IEnumerable<FinanceModel> filteredFinances;

                filteredFinances = finances.Find(x => x.id.Equals(id));

                foreach (FinanceModel c in filteredFinances)
                {
                    toReturn.Add(c);
                }

                return toReturn.FindAll(x =>x.id.Equals(id));
            }
        }

        public IList<FinanceModel> GetAll()
        {
            var toReturn = new List<FinanceModel>();
            using (var db = new LiteDatabase(liteDBPath))
            {
                var customers = db.GetCollection<FinanceModel>("finance");
                var results = customers.FindAll();
                foreach (FinanceModel c in results)
                {
                    toReturn.Add(c);
                }

                return toReturn;
            }
        }

        public IList<FinanceModel> Filter(DateTime from, DateTime to)
        {
            var toReturn = new List<FinanceModel>();

            using (var db = new LiteDatabase(liteDBPath))
            {
                var finances = db.GetCollection<FinanceModel>("finance");
                IEnumerable<FinanceModel> filteredCustomers;

                filteredCustomers = finances.Find(x => x.jobDate >= from && x.jobDate <= to);
                toReturn = SetupFinances(filteredCustomers);

                return toReturn;
            }


        }

        private List<FinanceModel> SetupFinances(IEnumerable<FinanceModel> filteredFinances)
        {
            var toReturn = new List<FinanceModel>();

            foreach (FinanceModel c in filteredFinances)
            {
                toReturn.Add(c);
            }

            return toReturn;
        }


        public void Add(FinanceModel c)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(liteDBPath))
            {
                var collection = db.GetCollection<FinanceModel>("finance");
                // Insert a new issue document
                collection.Insert(c);
                collection.EnsureIndex(x => x.id);
            }
        }

        public void Update(FinanceModel customer)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issueCollection = db.GetCollection<FinanceModel>("finance");
                // Update an existing issue document
                issueCollection.Update(customer);
            }
        }

        public void Delete(Guid id)
        {
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issues = db.GetCollection<FinanceModel>("finance");
                issues.Delete(i => i.id == id);
            }
        }
    }
}
