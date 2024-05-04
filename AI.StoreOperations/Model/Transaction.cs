using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AI.StoreOperations.Model
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGender { get; set; }
        public int CustomerAge { get; set; }
        public string Country { get; set; }
        public string TransactionDate { get; set; }
        public string Region { get; set; }
        public string Channel { get; set; }
        public string Category { get; set; }
        public string Product { get; set; }
        public double Amount { get; set; }
        public double SalesVolume { get; set; }
    }
}
