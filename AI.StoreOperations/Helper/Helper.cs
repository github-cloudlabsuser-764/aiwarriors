using AI.StoreOperations.Model;

namespace AI.StoreOperations.Helper
{
    public static class Helper
    {
        public static List<Product> GetProducts()
        {
            var products = new List<Product>();
            string filePath = @"C:\Users\vathota\source\repos\aiwarriors\AI.StoreOperations\products.csv";
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

        public static List<Transaction> GetTransactions()
        {
            var transactions = new List<Transaction>();
            string filePath = @"C:\Users\vathota\source\repos\aiwarriors\AI.StoreOperations\TransactionData.csv";
            using (var reader = new StreamReader(filePath))
            {
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (i != 0)
                    {
                        var tran = new Transaction()
                        {
                            TransactionId = values[0],
                            CustomerName = values[1],
                            CustomerGender = values[2],
                            CustomerAge = Convert.ToInt32(values[3]),
                            Country = values[4],
                            TransactionDate = values[5],
                            Region = values[6],
                            Channel = values[7],
                            Category = values[8],
                            Product = values[9],
                            Amount = Convert.ToDouble(values[10])

                        };
                        transactions.Add(tran);
                    }
                    i++;

                }
            }
            return transactions;
        }
        public static List<PackPricing> GetPackPricing()
        {
            var packPrices = new List<PackPricing>();
            string filePath = @"C:\Users\vathota\source\repos\aiwarriors\AI.StoreOperations\PackPricing.csv";
            using (var reader = new StreamReader(filePath))
            {
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (i != 0)
                    {
                        var prod = new PackPricing()
                        {
                            ProductId = values[10],
                            ProductName = values[11],
                            //SalesVolume = Convert.ToInt32(values[2]),
                            Seasonality = values[14],
                            //CustomerType = values[4],
                            Category = values[12],
                            SubCategory = values[13],
                            ActualPrice = Convert.ToDouble(values[8]),
                            DiscountedPrice = Convert.ToDouble(values[9]),
                            TransactionDate = values[5],
                            CustomerGender = values[2],
                            CustomerAge = Convert.ToInt32(values[3]),
                            Country = values[4],
                            Region = values[6]
                        };
                        packPrices.Add(prod);
                    }
                    i++;

                }
            }
            return packPrices;
        }

        public static string GetPrompt(List<Product> prdData, string season, string rows, string columns)
        {

            var prompt = "Based on the following product data, suggest an optimal shelf placement. Consider seasonality, sales volumes and customer type in suggestion: ";
            prompt += $"\n Consider current season is {season}.";
            prompt += $"\n Consider the shelf has {rows} rows and {columns} columns. It means that shelf has  {int.Parse(rows) * int.Parse(columns)} (rows * columns) boxes to arrange the product . Row[0][0] means first box of row 1 and column 1 and so on. Response Format - row[0][0]:product 1; row[0][1]:product 1 and product 2;row[n][n]:product 1,2,3...n;";
            prompt += $"\n Keep only one product in one box.";
            prompt += $"\n Use this JSON format for response ```{{\"Row 1\": {{\"Box 1\": [\"Shampoo\"], \"Box 2\": [\"Hair Conditioner\"]}},\"Row 2\": {{\"Box 1\": \"Shaving Blades\"], \"Box 2\": [\"Conditioner\"],  \"Box n\": [\"Product n\"]}}```";
            prompt += $"\n Add the response JSON in triple backticks (```)";
            foreach (var product in prdData)
            {
                prompt += $"\n- {product.ProductName} (Season: {product.Seasonality}, Customer: {product.CustomerType}, Sales Volume: {product.SalesVolume})";
            }

            return prompt;
        }

        public static string GetTransactionPrompt(List<Transaction> transData, string channel)
        {
            var prompt = "Based on the following transactional data, suggest the recommended product with category.Consider Channel, Category, Product and Country in suggestion:";
            prompt += $"\n Consider current Channel is {channel}.";
            prompt += $"\n Add the number of transactions done for each product from each country based on current Channel.";
            //Display all the corresponding transactions from the transactional data.
            prompt += $"\n Use this JSON format for response ```{{\"Category 1\":{{\"Product 1\":{{\"Country 1\"}},{{\"Country n\"}}}},{{\"Product 2\":{{\"Country 1\"}},{{\"Country n\"}}}}}},{{\"Category 2\": {{\"Product 1\":{{\"Country 1\"}},{{\"Country n\"}}}},{{\"Product 2\":{{\"Country 1\"}},{{\"Country n\"}}}}}}```";
            prompt += $"\nAdd the response JSON in triple backticks (```)";
            foreach (var transaction in transData)
            {
                prompt += $"\n- {transaction.TransactionId} (Channel:{transaction.Channel} Category: {transaction.Category}, Product: {transaction.Product}, Country: {transaction.Country})";
            }

            return prompt;
        }
        public static string GetPackPricingPrompt(List<PackPricing> prdData)
        {

            var prompt = "Generate a dynamic pricing strategy for a CPDR company that manufactures consumer packaged goods. The goal is to optimize pack pricing based on various factors including customer gender, transaction date, season, country, and region";
            prompt += $"\n Consider the following context and requirements:";
            prompt += $"\n Customer Gender,";
            prompt += $"\n Transaction Date and Season";
            prompt += $"\n Country and Region";
            prompt += $"\n Actual Price ";
            //prompt += $"\n Generate Pricing only for Country as India";
            prompt += $"\n Genearte Pack pricing for pack of 3,6 and 12 items";
            prompt += $"\n consider discount of 5% on pack of 3, 10% on pack of 6 and 15% on pack of 12. using this discount criteria generate the price for 3,6 and 12 items respectively";
            prompt += $"\n Use this JSON format for response ```{{\"Row 1\": {{\"Box 1\": [\"Shampoo\"\"[pack 1] = $*1,[pack 3]= $*3, [pack 6]= $*6,[pack 12]= $*12\"], \"Box 2\": [\"Hair Conditioner\"\"[pack 1] = $*1,[pack 3]= $*3, [pack 6]= $*6,[pack 12]= $*12\"]}},\"Row 2\": {{\"Box 1\": \"Shaving Blades\"\"[pack 1] = $*1,[pack 3]= $*3, [pack 6]= $*6,[pack 12]= $*12\"], \"Box 2\": [\"Conditioner\"\"[pack 1] = $*1,[pack 3]= $*3, [pack 6]= $*6,[pack 12]= $*12\"],  \"Box n\": [\"Product n\"\"[pack n] = $*n,[pack n]= $*n, [pack n]= $*n,[pack n]= $*n\"]}}```";
            prompt += $"\n Add the JSON response in triple backticks (```)";
            foreach (var product in prdData)
            {
                prompt += $"\n- {product.ProductName} (Season: {product.Seasonality}, Customer: {product.CustomerType}, Sales Volume: {product.SalesVolume} , Actual Price :{product.ActualPrice} , country :{product.Country} , region :{product.Region})";
            }

            return prompt;
        }
    }
}
