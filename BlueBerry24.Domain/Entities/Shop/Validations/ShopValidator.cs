using FluentValidation;

namespace BlueBerry24.Domain.Entities.Shop.Validations
{
    public class ShopValidator : ShopBaseValidator<Shop>
    {
        public ShopValidator()
        {
            RuleFor(i => i.Id)
                .NotEmpty()
                .NotNull();
        }
    }
}
