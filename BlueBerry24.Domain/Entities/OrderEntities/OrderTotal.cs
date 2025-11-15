namespace BlueBerry24.Domain.Entities.OrderEntities
{
    public class OrderTotal
    {
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal Total { get; set; }
    }
}
