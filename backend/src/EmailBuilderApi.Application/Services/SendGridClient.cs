
using EmailBuilderApi.Application.Interfaces;
using EmailBuilderApi.Domain.Models;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;

namespace EmailBuilderApi.Application.Services
{
    /// <summary>
    /// Stub implementation of IEmailSenderClient for SendGrid integration.
    /// </summary>
    public class SendGridClient : IEmailSenderClient
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SendGridClient(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"] ?? throw new InvalidOperationException("SendGrid API key not configured");
            _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@example.com";
            _fromName = configuration["SendGrid:FromName"] ?? "Email Builder";
        }

        public async Task SendEmailAsync(string htmlContent, string recipient, string? subject, List<string>? cc, List<string>? bcc, List<EmailAttachment>? attachments)
        {
            var client = new SendGrid.SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(recipient);
            var msg = MailHelper.CreateSingleEmail(from, to, subject ?? string.Empty, plainTextContent: null, htmlContent: htmlContent);

            // Add CC
            if (cc != null)
            {
                foreach (var ccEmail in cc)
                {
                    msg.AddCc(new EmailAddress(ccEmail));
                }
            }

            // Add BCC
            if (bcc != null)
            {
                foreach (var bccEmail in bcc)
                {
                    msg.AddBcc(new EmailAddress(bccEmail));
                }
            }

            // Add attachments
            if (attachments != null)
            {
                foreach (var att in attachments)
                {
                    msg.AddAttachment(att.FileName, Convert.ToBase64String(att.Content), att.MimeType);
                }
            }

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new InvalidOperationException($"SendGrid failed: {response.StatusCode} {body}");
            }
        }
    }
}
