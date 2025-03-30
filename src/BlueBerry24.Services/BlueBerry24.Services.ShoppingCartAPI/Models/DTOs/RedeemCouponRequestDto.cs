namespace BlueBerry24.Services.ShoppingCartAPI.Models.DTOs
{
    public class RedeemCouponRequestDto
    {
        public string UserId { get; set; }
        public string HeaderId { get; set; }

        public decimal total { get; set; }
    }
}
