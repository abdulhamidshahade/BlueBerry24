using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.ShoppingCartDtos
{
    public class CartCouponDto
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
