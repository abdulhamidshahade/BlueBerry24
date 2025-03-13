using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos
{
    public class ProductDto : ProductBase
    {
        public string Id { get; set; }
    }
}
