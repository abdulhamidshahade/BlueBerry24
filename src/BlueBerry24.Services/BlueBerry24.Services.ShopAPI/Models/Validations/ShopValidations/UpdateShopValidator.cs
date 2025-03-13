using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;
using FluentValidation;

namespace BlueBerry24.Services.ShopAPI.Models.Validations.ShopValidations
{
    public class UpdateShopValidator : ShopBaseValidator<UpdateShopDto>
    {
        public UpdateShopValidator()
        {
            RuleFor(i => i.Id)
                .NotEmpty()
                .NotNull();
        }
    }
}
