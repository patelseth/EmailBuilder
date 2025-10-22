namespace EmailBuilderApi.Application
{

    /// <summary>
    /// Service responsible for sending emails using an injected email client.
    /// Adheres to Single Responsibility Principle (SRP) and Dependency Inversion Principle (DIP).
    /// </summary>
    /// <remarks>
    /// Constructor injects the email client dependency.
    /// </remarks>
    /// <param name="emailSenderClient">An implementation of IEmailSenderClient.</param>
    public class EmailSenderService(IEmailSenderClient emailSenderClient) : IEmailSenderService
    {
        /// <summary>
        /// Sends an email with the provided HTML content, subject, and recipients.
        /// </summary>
        /// <param name="htmlContent">The HTML body of the email.</param>
        /// <param name="recipient">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="cc">CC recipients.</param>
        /// <param name="bcc">BCC recipients.</param>
    public async Task SendEmailAsync(string htmlContent, string recipient, string? subject, string[]? cc, string[]? bcc)
        {
            // Trim whitespace from recipient
            recipient = recipient.Trim();

            // Validate recipient
            if (string.IsNullOrWhiteSpace(recipient))
                throw new ArgumentException("Recipient email address must not be empty.", nameof(recipient));

            // Improved email format validation
            int atIndex = recipient.IndexOf('@');
            if (atIndex <= 0 || atIndex == recipient.Length - 1)
                throw new ArgumentException("Recipient email address format is invalid.", nameof(recipient));

            string domain = recipient[(atIndex + 1)..];
            int dotIndex = domain.IndexOf('.');
            if (dotIndex <= 0 || dotIndex == domain.Length - 1)
                throw new ArgumentException("Recipient email address format is invalid.", nameof(recipient));

            // Validate HTML content
            if (string.IsNullOrWhiteSpace(htmlContent))
                throw new ArgumentException("Email HTML content must not be empty.", nameof(htmlContent));

            // Delegates the actual sending to the injected client, supporting OCP and testability.
            await emailSenderClient.SendEmailAsync(htmlContent, recipient, subject, cc, bcc);
        }
    }
}