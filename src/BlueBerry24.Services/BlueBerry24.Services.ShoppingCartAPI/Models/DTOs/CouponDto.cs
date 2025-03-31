namespace BlueBerry24.Services.ShoppingCartAPI.Models.DTOs
{
    public class CouponDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
