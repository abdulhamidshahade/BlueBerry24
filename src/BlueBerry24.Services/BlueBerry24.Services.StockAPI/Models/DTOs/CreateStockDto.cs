namespace BlueBerry24.Services.StockAPI.Models.DTOs
{
    public class CreateStockDto
    {
        public string ShopId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
