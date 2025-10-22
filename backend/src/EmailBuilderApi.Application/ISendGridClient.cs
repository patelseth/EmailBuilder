namespace EmailBuilderApi.Application
{
    /// <summary>
    /// Abstraction for sending emails via any provider (e.g., SendGrid).
    /// Follows Dependency Inversion Principle (DIP) and Interface Segregation Principle (ISP).
    /// </summary>
    public interface IEmailSenderClient
    {
        /// <summary>
        /// Sends an email with the given HTML content, subject, and recipients.
        /// </summary>
        /// <param name="htmlContent">The HTML body of the email.</param>
        /// <param name="recipient">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="cc">CC recipients.</param>
        /// <param name="bcc">BCC recipients.</param>
        Task SendEmailAsync(string htmlContent, string recipient, string? subject, List<string>? cc, List<string>? bcc, List<EmailAttachment>? attachments);
    }
}