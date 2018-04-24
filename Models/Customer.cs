using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace CRUD.Models
{
    public class Customer
    {
        [BsonId]
        public Guid id { get; set; }

        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }

        public string phone { get; set; }
        public string address { get; set; }        
        
        public DateTime appointmentDate { get; set; }
        public string model { get; set; }
        public string make { get; set; }
        public string engine { get; set; }       
        public string appointmentDetails { get; set; }

    }

    public class FinanceModel
    {
        [BsonId]
        public Guid id { get; set; }

        [BsonId]
        public Guid jobId { get; set; }

        public DateTime jobDate { get; set; }
        public double moneyIn { get; set; }
        public double moneyOut { get; set; }
        public double cashOnHand { get; set; }
    }
}
