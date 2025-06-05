using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Dtos.CouponDtos
{
    public abstract class CouponBaseDto
    {
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public CouponType Type { get; set; }
        public decimal Value { get; set; }
        public bool IsForNewUsersOnly { get; set; }
    }
}
