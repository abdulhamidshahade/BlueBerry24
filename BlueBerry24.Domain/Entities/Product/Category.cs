namespace BlueBerry24.Domain.Entities.Product
{
    public class Category : CategoryBase
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<ProductCategory> ProductCategories { get; set; }

        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
