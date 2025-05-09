using BlueBerry24.Domain.Entities.Base;

namespace BlueBerry24.Domain.Entities.ShoppingCartEntities
{
    public class CartHeader : IAuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Discount { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public decimal CartTotal { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
