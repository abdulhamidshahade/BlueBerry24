using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.CouponDtos
{
    class UserCouponDto
    {
        public string UserId { get; set; }
        public string CouponId { get; set; }
        public bool IsUsed { get; set; }
    }
}
