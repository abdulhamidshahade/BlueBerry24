using Berryfy.Domain.Entities.Base;

namespace Berryfy.Domain.Entities.ProductEntities
{
    public class ProductCategory : IAuditableEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
