namespace BlueBerry24.Services.CouponAPI.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
