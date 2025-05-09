using BlueBerry24.Domain.Entities.Base;

namespace BlueBerry24.Domain.Entities.ProductEntities
{
    public class Category : CategoryBase, IAuditableEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
