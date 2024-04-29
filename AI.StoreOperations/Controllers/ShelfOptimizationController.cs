using AI.StoreOperations.Model;
using AI.StoreOperations.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AI.StoreOperations.Controllers
{
    public class ShelfOptimizationController : Controller
    {
        public ActionResult Index()
        {
            List<Product> products = Helper.Helper.GetProducts();
            return View(products);
        }

        // Get subcategories based on category ID
        public JsonResult GetSubCategories(string category)
        {
            List<Product> products = Helper.Helper.GetProducts();
            var subcategories = products.Where(p => p.Category == category)
                                .Select(p => p.SubCategory)
                                .Distinct()
                                .ToList();            
            return Json(subcategories);
        }

        public JsonResult GetProducts(string subCategory)
        {
            List<Product> products = Helper.Helper.GetProducts();
            var prods = products.Where(p => p.SubCategory == subCategory).ToList();
            return Json(prods);
        }


        
        public  JsonResult GetRecommendation(List<string> selectedProductIds, string season, string rows, string columns)
        {
            List<Product> products = Helper.Helper.GetProducts();
            var selectedProducts = products.Where(p => selectedProductIds.Contains(p.ProductId)).ToList();
            // Create a prompt to send to Azure OpenAI
            string prompt = Helper.Helper.GetPrompt(selectedProducts, season, rows, columns);

            var recommendation =  AzureOpenAI.GetShelfOptimizationRecommendations(prompt);
            var result = Json(recommendation.Result);

            return Json(new { success = true, message = result });
        }

        //[HttpPost]
        //public ActionResult SubmitProducts(List<int> selectedProductIds)
        //{
        //    var selectedProducts = products.Where(p => selectedProductIds.Contains(p.Id)).ToList();
        //    // Handle the submission (e.g., save to database, send to an API, etc.)
        //    // For this example, we'll just return a simple success message.
        //    return Json(new { success = true, message = "Products submitted successfully." });
        //}
    }
}
