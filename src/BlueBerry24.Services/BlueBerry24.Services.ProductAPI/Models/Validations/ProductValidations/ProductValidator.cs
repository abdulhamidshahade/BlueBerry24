using FluentValidation;

namespace BlueBerry24.Services.ProductAPI.Models.Validations.ProductValidations
{
    public class ProductValidator : ProductBaseValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("ID is required!");
        }
    }
}
