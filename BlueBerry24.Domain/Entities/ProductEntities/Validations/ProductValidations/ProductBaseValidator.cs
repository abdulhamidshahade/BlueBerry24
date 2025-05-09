using FluentValidation;

namespace BlueBerry24.Domain.Entities.ProductEntities.Validations.ProductValidations
{
    public class ProductBaseValidator<T> : AbstractValidator<T> where T : ProductBase
    {
        public ProductBaseValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be a non-negative number.");

            RuleFor(p => p.ImageUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(p => !string.IsNullOrEmpty(p.ImageUrl))
                .WithMessage("Invalid URL format.");
        }
    }
}
