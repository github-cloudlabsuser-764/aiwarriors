using AI.StoreOperations.Model;
using AI.StoreOperations.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI.StoreOperations.Controllers
{
    public class PackPricingController : Controller
    {
        public ActionResult Index()
        {
            List<PackPricing> packpricing = Helper.Helper.GetPackPricing();
            return View(packpricing);
        }

        public JsonResult GetSubCategories(string category)
        {
            List<PackPricing> products = Helper.Helper.GetPackPricing();
            var subcategories = products.Where(p => p.Category == category)
                                .Select(p => p.SubCategory)
                                .Distinct()
                                .ToList();
            return Json(subcategories);
        }

        public JsonResult GetProducts(string subCategory)
        {
            List<PackPricing> products = Helper.Helper.GetPackPricing();
            var prods = products.Where(p => p.SubCategory == subCategory).ToList();
            return Json(prods);
        }



        public JsonResult GetPackPricingButton(List<string> selectedProductIds)
        {
            List<PackPricing> products = Helper.Helper.GetPackPricing();
            var selectedProducts = products.Where(p => selectedProductIds.Contains(p.ProductId)).ToList();
            // Create a prompt to send to Azure OpenAI
            string prompt = Helper.Helper.GetPackPricingPrompt(selectedProducts);

            var recommendation = AzureOpenAI.GetShelfOptimizationRecommendations(prompt);
            var result = Json(recommendation.Result);

            return Json(new { success = true, message = result });

        }
    }
}
