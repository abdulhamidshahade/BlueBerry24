using BlueBerry24.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.Coupon
{
    public class UserCoupon
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
