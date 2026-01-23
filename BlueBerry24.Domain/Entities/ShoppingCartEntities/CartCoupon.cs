using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.CouponEntities;

namespace BlueBerry24.Domain.Entities.ShoppingCartEntities
{
    public class CartCoupon : IAuditableEntity
    {
        public int Id { get; set; }


        public int CartId { get; set; }
        public int CouponId { get; set; }


        public int? UserId { get; set; }
        public string? SessionId { get; set; }


        public decimal DiscountAmount { get; set; }


        public DateTime AppliedAt { get; set; }


        public virtual Cart Cart { get; set; } = null!;
        public virtual Coupon Coupon { get; set; } = null!;
        public virtual ApplicationUser? User { get; set; }




        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
