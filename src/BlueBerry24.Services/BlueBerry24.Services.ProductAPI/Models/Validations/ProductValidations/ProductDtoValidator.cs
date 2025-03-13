using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;
using FluentValidation;

namespace BlueBerry24.Services.ProductAPI.Models.Validations.ProductValidations
{
    public class ProductDtoValidator : ProductBaseValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            
        }
    }
}
