using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;
using FluentValidation;

namespace BlueBerry24.Services.ProductAPI.Models.Validations.ProductValidations
{
    public class DeleteProductValidator : ProductBaseValidator<DeleteProductDto>
    {
        public DeleteProductValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("ID is required!");
        }
    }
}
