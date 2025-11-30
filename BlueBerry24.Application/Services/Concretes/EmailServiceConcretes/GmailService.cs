using BlueBerry24.Application.Config;
using BlueBerry24.Application.Dtos.EmailDtos;
using BlueBerry24.Application.Services.Interfaces.EmailServiceInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BlueBerry24.Application.Services.Concretes.EmailServiceConcretes
{
    public class GmailService : IMailService
    {
        private readonly GmailSettings _gmailOption;
        private readonly ILogger<GmailService> _logger;

        public GmailService(IOptions<GmailSettings> gmailOptions, ILogger<GmailService> logger)
        {
            _gmailOption = gmailOptions.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(SendEmailRequest request)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(_gmailOption.Email, _gmailOption.DisplayName),
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = request.IsBodyHtml
                };

                message.To.Add(request.Recipient);

                using var smtpClient = new SmtpClient();
                smtpClient.Host = _gmailOption.Host;
                smtpClient.Port = _gmailOption.Port;
                smtpClient.Credentials = new NetworkCredential(
                    _gmailOption.Email, _gmailOption.Password);

                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);

                _logger.LogInformation("Email sent successfully to {Recipient}", request.Recipient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}", request.Recipient);
                throw;
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken, string resetUrl)
        {
            var encodedToken = HttpUtility.UrlEncode(resetToken);
            var encodedEmail = HttpUtility.UrlEncode(email);
            var resetLink = $"{resetUrl}?email={encodedEmail}&token={encodedToken}";

            var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Password Reset - BlueBerry24</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                        .content {{ background-color: #f8f9fa; padding: 30px; border-radius: 0 0 5px 5px; }}
                        .button {{ display: inline-block; background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                        .warning {{ background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 10px; border-radius: 4px; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🫐 BlueBerry24</h1>
                            <h2>Password Reset Request</h2>
                        </div>
                        <div class='content'>
                            <p>Hello,</p>
                            <p>We received a request to reset your password for your BlueBerry24 account. If you didn't make this request, you can safely ignore this email.</p>
                            
                            <p>To reset your password, click the button below:</p>
                            
                            <div style='text-align: center;'>
                                <a href='{resetLink}' class='button'>Reset My Password</a>
                            </div>
                            
                            <p>Or copy and paste this link into your browser:</p>
                            <p style='word-break: break-all; background-color: #e9ecef; padding: 10px; border-radius: 4px;'>{resetLink}</p>
                            
                            <div class='warning'>
                                <strong>⚠️ Security Notice:</strong>
                                <ul>
                                    <li>This link will expire in 24 hours for security reasons</li>
                                    <li>If you didn't request this reset, please contact our support team</li>
                                    <li>Never share this link with anyone</li>
                                </ul>
                            </div>
                            
                            <p>If you're having trouble clicking the button, copy and paste the URL above into your web browser.</p>
                            
                            <p>Best regards,<br>The BlueBerry24 Team</p>
                        </div>
                        <div class='footer'>
                            <p>This email was sent from BlueBerry24. If you have any questions, please contact our support team.</p>
                            <p>&copy; 2024 BlueBerry24. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            var request = new SendEmailRequest
            {
                Recipient = email,
                Subject = "Reset Your BlueBerry24 Password",
                Body = emailBody,
                IsBodyHtml = true
            };

            await SendEmailAsync(request);
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationToken, string confirmationUrl, string userName)
        {
            var encodedToken = HttpUtility.UrlEncode(confirmationToken);
            var encodedEmail = HttpUtility.UrlEncode(email);
            var confirmationLink = $"{confirmationUrl}?email={encodedEmail}&token={encodedToken}";

            var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Welcome to BlueBerry24 - Confirm Your Email</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                        .content {{ background-color: #f8f9fa; padding: 30px; border-radius: 0 0 5px 5px; }}
                        .button {{ display: inline-block; background-color: #28a745; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                        .welcome {{ background-color: #d4edda; border: 1px solid #c3e6cb; padding: 15px; border-radius: 4px; margin: 15px 0; }}
                        .features {{ background-color: #e9ecef; padding: 15px; border-radius: 4px; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🫐 Welcome to BlueBerry24!</h1>
                            <h2>Confirm Your Email Address</h2>
                        </div>
                        <div class='content'>
                            <div class='welcome'>
                                <h3>🎉 Welcome {userName}!</h3>
                                <p>Thank you for joining BlueBerry24! We're excited to have you as part of our community.</p>
                            </div>
                            
                            <p>To complete your registration and start shopping, please confirm your email address by clicking the button below:</p>
                            
                            <div style='text-align: center;'>
                                <a href='{confirmationLink}' class='button'>✅ Confirm My Email</a>
                            </div>
                            
                            <p>Or copy and paste this link into your browser:</p>
                            <p style='word-break: break-all; background-color: #e9ecef; padding: 10px; border-radius: 4px;'>{confirmationLink}</p>
                            
                            <div class='features'>
                                <h4>🛍️ What's waiting for you:</h4>
                                <ul>
                                    <li>Access to exclusive deals and discounts</li>
                                    <li>Track your orders and delivery status</li>
                                    <li>Create wishlists for your favorite items</li>
                                    <li>Personalized product recommendations</li>
                                    <li>Priority customer support</li>
                                </ul>
                            </div>
                            
                            <p><strong>Important:</strong> This confirmation link will expire in 24 hours. If you didn't create an account with us, you can safely ignore this email.</p>
                            
                            <p>If you're having trouble clicking the button, copy and paste the URL above into your web browser.</p>
                            
                            <p>Welcome aboard!<br>
                            <strong>The BlueBerry24 Team</strong></p>
                        </div>
                        <div class='footer'>
                            <p>This email was sent because you created an account at BlueBerry24.</p>
                            <p>If you have any questions, please contact our support team.</p>
                            <p>&copy; 2024 BlueBerry24. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            var request = new SendEmailRequest
            {
                Recipient = email,
                Subject = "Welcome to BlueBerry24 - Confirm Your Email Address",
                Body = emailBody,
                IsBodyHtml = true
            };

            await SendEmailAsync(request);
        }
    }
}