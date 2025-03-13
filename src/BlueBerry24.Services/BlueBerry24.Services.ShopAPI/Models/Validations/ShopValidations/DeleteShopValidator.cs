using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;
using FluentValidation;

namespace BlueBerry24.Services.ShopAPI.Models.Validations.ShopValidations
{
    public class DeleteShopValidator : ShopBaseValidator<DeleteShopDto>
    {
        public DeleteShopValidator()
        {
            RuleFor(i => i.Id)
                .NotEmpty()
                .NotNull();
        }
    }
}
