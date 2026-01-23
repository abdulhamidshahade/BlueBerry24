using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.ProductEntities;

namespace BlueBerry24.Domain.Entities.ShoppingCartEntities
{
    public class CartItem : IAuditableEntity
    {
        public int Id { get; set; }
        public int? ShoppingCartId { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public Cart ShoppingCart { get; set; }
        public int ProductId { get; set; }  
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public decimal UnitPrice { get; set; }

    }
}
