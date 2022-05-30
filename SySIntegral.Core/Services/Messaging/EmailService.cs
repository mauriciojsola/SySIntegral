using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SySIntegral.Core.Services.Messaging.Providers.SendGrid;

namespace SySIntegral.Core.Services.Messaging
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return SendEmail(email, subject, message);
        }

        private async Task SendEmail(string toEmailAddress, string emailSubject, string emailMessage)
        {
            if (_configuration["AppSettings:Environment"] == "DEV")
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(toEmailAddress);
                    message.From = new MailAddress(_configuration["MailClientSettings:SenderEmail"],_configuration["MailClientSettings:SenderName"]);

                    message.Subject = emailSubject;
                    message.Body = emailMessage;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(_configuration["MailClientSettings:Host"], Convert.ToInt32(_configuration["MailClientSettings:Port"])))
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                }
            }
            else
            {
                var options = new SendGridEmailSenderOptions
                {
                    ApiKey = _configuration["ExternalProviders:SendGrid:ApiKey"],
                    SenderEmail = _configuration["ExternalProviders:SendGrid:SenderEmail"],
                    SenderName = _configuration["ExternalProviders:SendGrid:SenderName"]
                };
                var sender = new SendGridEmailSender(options);
                await sender.SendEmailAsync(toEmailAddress, emailSubject, emailMessage);
            }
        }
    }
}
