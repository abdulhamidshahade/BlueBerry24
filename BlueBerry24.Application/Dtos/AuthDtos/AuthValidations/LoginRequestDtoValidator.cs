using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.AuthDtos.AuthValidations
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password is required.");
        }
    }
}
