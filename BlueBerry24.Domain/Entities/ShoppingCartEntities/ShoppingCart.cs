using BlueBerry24.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.ShoppingCartEntities
{
    public class ShoppingCart : IAuditableEntity
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        public List<CartItem> CartItems { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
