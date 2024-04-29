using AI.StoreOperations.Model;

namespace AI.StoreOperations.Helper
{
    public static class Helper
    {
        public static List<Product> GetProducts()
        {
            var products = new List<Product>();
            string filePath = "C:\\PersonalProjects\\TEst\\AIHackthon\\Products.csv";
            using (var reader = new StreamReader(filePath))
            {
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (i != 0)
                    {
                        var prod = new Product()
                        {
                            ProductId = values[0],
                            ProductName = values[1],
                            SalesVolume = Convert.ToInt32(values[2]),
                            Seasonality = values[3],
                            CustomerType = values[4],
                            Category = values[5],
                            SubCategory = values[6],
                            ActualPrice = Convert.ToDouble(values[7]),
                            DiscountedPrice = Convert.ToDouble(values[8])

                        };
                        products.Add(prod);
                    }
                    i++;

                }
            }
            return products;
        }

        public static string GetPrompt(List<Product> prdData, string season, string rows, string columns)
        {
            
            var prompt = "Based on the following product data, suggest an optimal shelf placement. Consider seasonality, sales volumes and customer type: ";

            foreach (var product in prdData)
            {
                prompt += $"\n- {product.ProductName} (Season: {product.Seasonality}, Customer: {product.CustomerType}, Sales Volume: {product.SalesVolume})";
            }
            prompt += $"\n Consider current season is {season}.";
            prompt += $"\n Consider the shelf has {rows} rows and {columns} columns. It means that shelf has  {int.Parse(rows) * int.Parse(columns)} (rows * columns) boxes to arrange the product . Row[0][0] means first box of row 1 and column 1 and so on. Response Format - row[0][0]:product 1; row[0][1]:product 1 and product 2;row[n][n]:product 1,2,3...n;";
            prompt += $"\n Use this format for response ```{{\"Row 1\": {{\"Box 1\": [\"Shampoo\", \"Conditioner\", \"Hair Color\"]}},\"Row 2\": {{\"Box 1\": [\"Shaving Cream\", \"Shaving Razor\", \"Shaving Blades\"]}}```";
            return prompt;
        }
    }    
}
