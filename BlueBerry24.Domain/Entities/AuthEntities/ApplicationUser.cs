
using BlueBerry24.Domain.Entities.CouponEntities;
using BlueBerry24.Domain.Entities.OrderEntities;
using BlueBerry24.Domain.Entities.ShopEntities;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using BlueBerry24.Domain.Entities.WishlistEntities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueBerry24.Domain.Entities.AuthEntities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }

        public List<Cart> Cart { get; set; }

        public List<Order> Orders { get; set; }

        [NotMapped]
        public IList<string> roles { get; set; }

        public List<Wishlist> Wishlists { get; set; }
    }
}
