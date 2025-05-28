using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.ProductDtos
{
    public class ProductDto : ProductBaseDto
    {
        public int Id { get; set; }

        public List<string> ProductCategoryNames { get; set; }
    }
}
