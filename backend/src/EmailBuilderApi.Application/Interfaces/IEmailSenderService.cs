using EmailBuilderApi.Domain.Models;

namespace EmailBuilderApi.Application.Interfaces
{
    /// <summary>
    /// Service interface for sending emails.
    /// </summary>
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sends an email with the provided HTML content, subject, and recipients.
        /// </summary>
        /// <param name="htmlContent">The HTML body of the email.</param>
        /// <param name="recipient">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="cc">CC recipients.</param>
        /// <param name="bcc">BCC recipients.</param>
        Task SendEmailAsync(string htmlContent, string recipient, string? subject, List<string>? cc, List<string>? bcc, List<EmailAttachment>? attachments);
    }
}
