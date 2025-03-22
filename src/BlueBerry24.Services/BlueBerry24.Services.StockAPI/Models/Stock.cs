namespace BlueBerry24.Services.StockAPI.Models
{
    public class Stock
    {
        public string Id { get; set; }

        public string ShopId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
