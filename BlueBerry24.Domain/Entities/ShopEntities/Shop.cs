using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.ProductEntities;


namespace BlueBerry24.Domain.Entities.ShopEntities
{
    public class Shop : ShopBase, IAuditableEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Product> Product { get; set; }
    }
}