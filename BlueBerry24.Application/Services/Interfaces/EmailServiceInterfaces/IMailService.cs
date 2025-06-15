using BlueBerry24.Application.Dtos.EmailDtos;

namespace BlueBerry24.Application.Services.Interfaces.EmailServiceInterfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(SendEmailRequest request);
        Task SendPasswordResetEmailAsync(string email, string resetToken, string resetUrl);
        Task SendEmailConfirmationAsync(string email, string confirmationToken, string confirmationUrl, string userName);
    }
}