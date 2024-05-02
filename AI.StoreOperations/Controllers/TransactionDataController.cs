using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AI.StoreOperations.Model;

namespace AI.StoreOperations.Controllers
{
    public class TransactionDataController : Controller
    {
        public IActionResult Index()
        {
            List<Transaction> transactions = Helper.Helper.GetTransactions();
            return View(transactions);
        }

        public JsonResult GetSeleactedTransactions(string channel)
        {
            List<Transaction> transactions = Helper.Helper.GetTransactions();
            // Create a prompt to send to Azure OpenAI
            string prompt = Helper.Helper.GetTransactionPrompt(transactions, channel);

            var recommendation = AzureOpenAI.GetShelfOptimizationRecommendations(prompt);
            var result = Json(recommendation.Result);

            return Json(new { success = true, message = result });
        }

    }
}
