using FluentValidation;

namespace BlueBerry24.Application.Dtos.AuthDtos.AuthValidations
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(fn => fn.FirstName)
                .NotNull().WithMessage("First name is required.")
                .NotEmpty().WithMessage("First name is required.")
                .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("First name must contain only letters.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters long.");
        }
    }
}
