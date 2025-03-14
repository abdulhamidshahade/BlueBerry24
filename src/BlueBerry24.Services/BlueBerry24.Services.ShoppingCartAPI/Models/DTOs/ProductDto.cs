using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Services.ShoppingCartAPI.Models.DTOs.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
