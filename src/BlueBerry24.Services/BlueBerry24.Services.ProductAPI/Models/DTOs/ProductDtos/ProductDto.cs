using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos
{
    public class ProductDto : ProductBase
    {
        public int Id { get; set; }
    }
}
