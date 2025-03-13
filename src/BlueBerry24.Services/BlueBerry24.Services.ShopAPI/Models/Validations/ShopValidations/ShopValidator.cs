using FluentValidation;

namespace BlueBerry24.Services.ShopAPI.Models.Validations.ShopValidations
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
