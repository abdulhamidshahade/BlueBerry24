using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.InventoryEntities;
using BlueBerry24.Domain.Entities.OrderEntities;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using System.ComponentModel.DataAnnotations;


namespace BlueBerry24.Domain.Entities.ProductEntities
{
    public class Product : IAuditableEntity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int ReservedStock { get; set; }
        public int LowStockThreshold { get; set; } = 5;
        public bool IsActive { get; set; } = true;
        public string SKU { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public List<InventoryLog> InventoryLogs { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
