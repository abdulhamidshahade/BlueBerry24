using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;
using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos
{
    public class ProductDto : ProductBase
    {
        public string Id { get; set; }


        public List<CategoryDto> Categories { get; set; }
    }
}
