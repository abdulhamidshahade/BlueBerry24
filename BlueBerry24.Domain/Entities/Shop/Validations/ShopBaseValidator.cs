using FluentValidation;

namespace BlueBerry24.Domain.Entities.Shop.Validations
{
    public class ShopBaseValidator<T> : AbstractValidator<T> where T : ShopBase
    {
        public ShopBaseValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(d => d.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(500);

            RuleFor(oi => oi.OwnerId)
                .NotEmpty()
                .NotNull();

            RuleFor(e => e.Email)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.Phone)
                .NotEmpty()
                .NotNull();

            RuleFor(a => a.Address)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Country)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.City)
                .NotEmpty()
                .NotNull();

            RuleFor(lu => lu.LogoUrl)
                .NotEmpty()
                .NotNull();
        }
    }
}
