using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.WishlistEntities
{
    public class Wishlist : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string? Name { get; set; } = "My Wishlist";

        public bool IsDefault { get; set; } = true;

        public bool IsPublic { get; set; } = false;

        public ApplicationUser User { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

        public int ItemCount => WishlistItems?.Count ?? 0;
        public decimal TotalValue => WishlistItems?.Sum(x => x.Product?.Price ?? 0) ?? 0;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
