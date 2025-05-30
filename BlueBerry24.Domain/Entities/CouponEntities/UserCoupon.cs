
using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.CouponEntities
{
    public class UserCoupon : IAuditableEntity
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
