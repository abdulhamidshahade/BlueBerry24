using Berryfy.Domain.Constants;
using Berryfy.Domain.Entities.AuthEntities;
using Berryfy.Domain.Entities.Base;
using Berryfy.Domain.Entities.OrderEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berryfy.Domain.Entities.ShoppingCartEntities
{
    public class Cart : IAuditableEntity
    {
        public int Id { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public int? UserId { get; set; }
        public string? SessionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Order? Order { get; set; }

        public DateTime? ExpiresAt { get; set; }

        [ConcurrencyCheck]
        public int Version { get; set; } = 1;

        public CartStatus Status { get; set; } = CartStatus.Active;

        public string? Note { get; set; }

        [NotMapped]
        public decimal SubTotal => CartItems?.Sum(i => i.Quantity * i.UnitPrice) ?? 0;

        [NotMapped]
        public decimal DiscountTotal => CartCoupons?.Sum(c => c.DiscountAmount) ?? 0;

        [NotMapped]
        public decimal TaxAmount => CalculateTax();

        [NotMapped]
        public decimal Total => SubTotal - DiscountTotal + TaxAmount;


        public List<CartCoupon> CartCoupons { get; set; } = new List<CartCoupon>();
        public ApplicationUser User { get; set; }


        //TODO fix CalculateTax method to use actual tax rules
        private decimal CalculateTax()
        {
            var taxableAmount = SubTotal - DiscountTotal;
            return taxableAmount > 0 ? taxableAmount * 0.1m : 0;
        }

    }

}
