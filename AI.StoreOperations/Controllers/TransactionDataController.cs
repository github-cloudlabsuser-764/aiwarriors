using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AI.StoreOperations.Controllers
{
    public class TransactionDataController : Controller
    {
        public IActionResult Index()
        {
            List<Transaction> transactions = Helper.Helper.GetTransactions();
            return View(transactions);
        }
    }
}
