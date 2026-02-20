using BlueBerry24.Application.Services.Concretes.AuthServiceConcretes;

namespace BlueBerry24.Application.Utils
{
    public class SignupPasswordValidator
    {
        public SignupPasswordValidator()
        {
        }

        public PasswordStrength CheckPasswordLength(string password)
        {
            switch (password.Length)
            {
                case < 6:
                    return PasswordStrength.Weak;
                case >= 6 and < 12:
                    return PasswordStrength.Medium;
                default:
                    return PasswordStrength.Strong;
            }
        }

        public bool IsContainSpecialChar(string password)
        {
            return password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
        }

        public bool IsContainUpperCase(string password)
        {
            return password.Any(ch => char.IsUpper(ch));
        }

        public bool IsContainDigit(string password)
        {
            return password.Any(ch => char.IsDigit(ch));
        }
    }
}
