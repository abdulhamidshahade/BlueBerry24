using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.CouponDtos
{
    public class CouponDto : CouponBaseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
