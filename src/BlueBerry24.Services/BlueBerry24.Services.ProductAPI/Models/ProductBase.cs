namespace BlueBerry24.Services.ProductAPI.Models
{
    public abstract class ProductBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        public string ShopId { get; set; }
    }
}
