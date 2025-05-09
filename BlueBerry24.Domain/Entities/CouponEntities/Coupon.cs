using BlueBerry24.Domain.Entities.Base;

namespace BlueBerry24.Domain.Entities.CouponEntities
{
    public class Coupon : IAuditableEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }
    }
}
