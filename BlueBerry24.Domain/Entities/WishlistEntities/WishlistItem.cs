using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations;
namespace BlueBerry24.Domain.Entities.WishlistEntities
{
    public class WishlistItem : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public int WishlistId { get; set; }


        public int ProductId { get; set; }

        public string? Notes { get; set; } = null;

        public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High

        public Wishlist Wishlist { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
