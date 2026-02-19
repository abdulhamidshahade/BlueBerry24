using BlueBerry24.Application.Services.Concretes.AuthServiceConcretes;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Utils
{
    public class SignupPasswordValidator
    {
        private readonly string _password;

        public SignupPasswordValidator(string password)
        {
            _password = password;
        }

        public PasswordStrength CheckPasswordLength()
        {
            switch (_password.Length)
            {
                case < 6:
                    return PasswordStrength.Weak;
                case >= 6 and < 12:
                    return PasswordStrength.Medium;
                default:
                    return PasswordStrength.Strong;
            }
        }

        public bool IsContainSpecialChar()
        {
            return _password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
        }

        public bool IsContainUpperCase()
        {
            return _password.Any(ch => char.IsUpper(ch));
        }

        public bool IsContainDigit()
        {
            return _password.Any(ch => char.IsDigit(ch));
        }
    }
}
