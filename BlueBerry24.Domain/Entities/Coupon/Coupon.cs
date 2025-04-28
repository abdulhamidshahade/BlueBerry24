namespace BlueBerry24.Domain.Entities.Coupon
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
