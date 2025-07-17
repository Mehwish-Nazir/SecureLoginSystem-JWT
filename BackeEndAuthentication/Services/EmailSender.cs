using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
namespace BackeEndAuthentication.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            //validate parameters [ToEmail:Recepient Email]]
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new ArgumentException("Recepient Email is required!", nameof(toEmail));
            }
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("Email Subject is required!", nameof(subject));
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("Email body is required!", nameof(body));
            }

            var email = new MimeMessage();
            try
            {
                //set sender address (From :Sender Email)

                var fromAddress = _config["EmailSettings:From"];
                if (string.IsNullOrWhiteSpace(fromAddress))
                {
                    throw new ArgumentException("Sender Email addres is not configured", nameof(fromAddress));
                }

                email.From.Add(MailboxAddress.Parse(fromAddress));

                //set 'To' Address which has verified above  (To: Recepient Email Address)

                /*
                 int.Parse("100") → converts string "100" to integer 100.
                MailboxAddress.Parse("user@example.com") → converts string to email address object with validation.

                 */
                email.To.Add(MailboxAddress.Parse(toEmail));

                //set subject 

                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = body
                };

                //Use SMTP server 
                using var smtp = new SmtpClient();
                var smtpServer = _config["EmailSettings:SmtpServer"];
                if (string.IsNullOrWhiteSpace(smtpServer))
                {
                    throw new ArgumentException("Smtp server is not configured.");
                }

                /*
                 smtp is an instance of SmtpClient from the MailKit library.

                 ConnectAsync initiates an asynchronous connection to the SMTP server.

                The method parameters:

                 smtpServer: The SMTP server hostname or IP address (e.g., "smtp.gmail.com").

                 587: The port number to connect to on the SMTP server.

                 SecureSocketOptions.StartTls: Defines how to secure the connection with TLS/SSL.

                 587: SMTP port for submission with STARTTLS (encrypted connection started after connection).

                 465: SMTP port for implicit SSL/TLS (connection starts encrypted immediately as the connection starts).
                 */

                await smtp.ConnectAsync(smtpServer, 587, MailKit.Security.SecureSocketOptions.StartTls);
                //await smtp.ConnectAsync("smtp.example.com", 465, SecureSocketOptions.SslOnConnect);

                // Authenticate using credentials from config
                var username = _config["EmailSettings:Username"];
                var password = _config["EmailSettings:Password"];

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("SMPT credentials are not configured yet.");
                }
                await smtp.AuthenticateAsync(username, password);

                //send Email
                await smtp.SendAsync(email);
                // Disconnect gracefully from the SMTP server

                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {ToEmail} with subject {Subject}.", toEmail, subject);


            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Failed to send email to {ToEmail}.", toEmail);
            throw; // Optionally rethrow or handle as needed
            }


        }
    }
}
