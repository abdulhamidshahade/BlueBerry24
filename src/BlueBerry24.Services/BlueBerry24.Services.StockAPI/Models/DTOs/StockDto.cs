namespace BlueBerry24.Services.StockAPI.Models.DTOs
{
    public class StockDto
    {
        public string Id { get; set; }

        public string ShopId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
