using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public CartStatus Status { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public List<CartCouponDto> CartCoupons { get; set; } = new List<CartCouponDto>();

        public string? Note { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
    }
}
