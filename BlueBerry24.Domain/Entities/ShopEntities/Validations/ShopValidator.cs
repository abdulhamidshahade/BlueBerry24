using FluentValidation;

namespace BlueBerry24.Domain.Entities.ShopEntities.Validations
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