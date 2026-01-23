using BlueBerry24.Domain.Entities.Base;

using BlueBerry24.Domain.Entities.ProductEntities;

namespace BlueBerry24.Domain.Entities.OrderEntities
{
    public class OrderItem : IAuditableEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }


        public Order Order { get; set; }
        public Product Product { get; set; }




        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
