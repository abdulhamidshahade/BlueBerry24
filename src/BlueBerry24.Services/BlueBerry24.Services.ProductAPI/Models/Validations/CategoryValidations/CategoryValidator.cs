using FluentValidation;

namespace BlueBerry24.Services.ProductAPI.Models.Validations.CategoryValidations
{
    public class CategoryValidator : CategoryBaseValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("ID is required!");
        }
    }
}
