using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.ShoppingCart
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        public List<CartItem> CartItems { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
    }
}
