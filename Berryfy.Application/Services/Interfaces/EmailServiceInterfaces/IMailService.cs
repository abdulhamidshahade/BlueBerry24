using Berryfy.Application.Dtos.EmailDtos;

namespace Berryfy.Application.Services.Interfaces.EmailServiceInterfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(SendEmailRequest request);
        Task SendPasswordResetEmailAsync(string email, string resetToken, string resetUrl);
        Task SendEmailConfirmationAsync(string email, string confirmationCode, string userName);
    }
}
