
using BlueBerry24.Domain.Entities.CouponEntities;
using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Domain.Entities.AuthEntities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }
    }
}
