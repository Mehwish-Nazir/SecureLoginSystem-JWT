using MailKit.Net.Smtp;
using MimeKit;

namespace BackeEndAuthentication.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
