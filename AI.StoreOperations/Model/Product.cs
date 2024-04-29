namespace AI.StoreOperations.Model
{
    public class Product
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
    }
}
