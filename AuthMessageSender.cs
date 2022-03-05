using System.Threading.Tasks;

namespace SySIntegral
{
    internal class AuthMessageSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}