using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.CouponDtos
{
    public abstract class CouponBaseDto
    {
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
