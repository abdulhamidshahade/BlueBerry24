using BlueBerry24.Application.Services.Concretes.AuthServiceConcretes;
using BlueBerry24.Domain.Entities.AuthEntities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography.X509Certificates;

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

            RuleFor(fn => fn.LastName)
                .NotNull().WithMessage("Last name is required.")
                .NotEmpty().WithMessage("Last name is required.")
                .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("Last name must contain only letters.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters long.");

            RuleFor(fn => fn.UserName)
                .NotNull().WithMessage("Last name is required.")
                .NotEmpty().WithMessage("Last name is required.")
                .Matches(@"^[a-zA-Z0-9\s._'-]+$").WithMessage("Last name must contain only letters.")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Last name must be at most 20 characters long.");

            RuleFor(fn => fn.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(fn => fn.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(2).WithMessage("Password must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Password must be at most 50 characters long.")
                .Must(pass => ContainsUpperCase(pass)).WithMessage("Password should contains at least one uppercase letter.")
                .Must(pass => ContainsDigits(pass)).WithMessage("Password should contains at least one digit.")
                .Must(pass => ContainsSpecial(pass)).WithMessage("Password should contains at least one special character.");
        }

        private bool ContainsUpperCase(string password)
        {
            return password.Any(ch => char.IsUpper(ch));
        }
        private bool ContainsDigits(string password)
        {
            return password.Any(ch => char.IsDigit(ch));
        }
        private bool ContainsSpecial(string password)
        {
            return password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
        }
    }
}
