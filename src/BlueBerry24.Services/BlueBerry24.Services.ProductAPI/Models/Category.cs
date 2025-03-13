namespace BlueBerry24.Services.ProductAPI.Models
{
    public class Category : CategoryBase
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
