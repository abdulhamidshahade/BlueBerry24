namespace BlueBerry24.Services.CouponAPI.Models.DTOs
{
    public class CreateCouponDto
    {
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
