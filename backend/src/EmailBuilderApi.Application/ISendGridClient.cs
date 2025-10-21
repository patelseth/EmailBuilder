namespace EmailBuilderApi.Application
{
    /// <summary>
    /// Abstraction for sending emails via any provider (e.g., SendGrid).
    /// Follows Dependency Inversion Principle (DIP) and Interface Segregation Principle (ISP).
    /// </summary>
    public interface IEmailSenderClient
    {
        /// <summary>
        /// Sends an email with the given HTML content to the specified recipient.
        /// </summary>
        /// <param name="htmlContent">The HTML body of the email.</param>
        /// <param name="recipient">The recipient's email address.</param>
        Task SendEmailAsync(string htmlContent, string recipient);
    }
}