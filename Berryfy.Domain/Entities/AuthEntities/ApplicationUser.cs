
using Berryfy.Domain.Entities.CouponEntities;
using Berryfy.Domain.Entities.OrderEntities;
using Berryfy.Domain.Entities.PaymentEntities;
using Berryfy.Domain.Entities.ShoppingCartEntities;
using Berryfy.Domain.Entities.WishlistEntities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berryfy.Domain.Entities.AuthEntities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiry { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }

        public List<Cart> Cart { get; set; }

        public List<Order> Orders { get; set; }

        [NotMapped]
        public IList<string> roles { get; set; }

        public List<Wishlist> Wishlists { get; set; }

        public List<Payment> Payments { get; set; }
    }
}
