namespace BlueBerry24.Services.CouponAPI.Models
{
    public class Coupon
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
        public string ShopId { get; set; }
    }
}
