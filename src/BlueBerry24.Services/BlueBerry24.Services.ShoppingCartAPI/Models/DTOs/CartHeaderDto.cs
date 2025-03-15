namespace BlueBerry24.Services.ShoppingCartAPI.Models.DTOs
{
    public class CartHeaderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Discount { get; set; }
        public string CouponCode { get; set; }
        public decimal CartTotal { get; set; }
        public bool IsActive { get; set; }
    }
}
