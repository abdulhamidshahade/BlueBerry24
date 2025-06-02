using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.ShopDtos
{
    public abstract class ShopBaseDto
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Currency { get; set; }
        public string Language { get; set; }
    }
}
