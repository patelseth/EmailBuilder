namespace EmailBuilderApi.Application;

/// <summary>
/// Service interface for sending emails.
/// </summary>
public interface IEmailSenderService
{
    /// <summary>
    /// Sends an email with the provided HTML content to the recipient.
    /// </summary>
    /// <param name="htmlContent">The HTML body of the email.</param>
    /// <param name="recipient">The recipient's email address.</param>
    Task SendEmailAsync(string htmlContent, string recipient);
}
