using EmailBuilderApi.Application.Interfaces;
using EmailBuilderApi.Domain.Models;

namespace EmailBuilderApi.Application
{
    /// <summary>
    /// Stub implementation of IEmailSenderClient for SendGrid integration.
    /// </summary>
    public class SendGridClient : IEmailSenderClient
    {
        public Task SendEmailAsync(string htmlContent, string recipient, string? subject, List<string>? cc, List<string>? bcc, List<EmailAttachment>? attachments)
        {
            // TODO: Implement actual SendGrid API integration
            return Task.CompletedTask;
        }
    }
}
