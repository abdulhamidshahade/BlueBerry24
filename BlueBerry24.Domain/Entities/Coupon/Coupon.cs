namespace BlueBerry24.Domain.Entities.Coupon
{
    public class Coupon
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
        public bool IsActive { get; set; }

        public Coupon()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
