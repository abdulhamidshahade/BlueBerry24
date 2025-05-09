using BlueBerry24.Domain.Entities.Base;

namespace BlueBerry24.Domain.Entities.StockEntities
{
    public class Stock : IAuditableEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
