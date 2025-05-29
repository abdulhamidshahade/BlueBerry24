using BlueBerry24.Application.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.CategoryDtos
{
    public class CategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}
