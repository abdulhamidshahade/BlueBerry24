using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;
using FluentValidation;

namespace BlueBerry24.Services.ShopAPI.Models.Validations.ShopValidations
{
    public class ShopDtoValidator : ShopBaseValidator<ShopDto>
    {
        public ShopDtoValidator()
        {
            RuleFor(i => i.Id)
                .NotEmpty()
                .NotNull();
        }
    }
}
