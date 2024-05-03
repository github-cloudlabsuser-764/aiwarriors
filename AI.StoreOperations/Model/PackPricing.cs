namespace AI.StoreOperations.Model
{
    public class PackPricing
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int SalesVolume { get; set; }
        public string Seasonality { get; set; }
        public string CustomerType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public double ActualPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public int CustomerAge { get; set; }
        public string CustomerGender { get; set; }
        public string TransactionDate { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

    }
}
