using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.CouponDtos
{
    public class UserCouponDto
    {
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
