using FluentValidation;

namespace Berryfy.Application.Dtos.AuthDtos.AuthValidations
{
    public class EmailConfirmationDtoValidator : AbstractValidator<EmailConfirmationDto>
    {
        public EmailConfirmationDtoValidator()
        {
            RuleFor(em => em.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(em => em.Token)
                .NotNull().WithMessage("Token is required.")
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
