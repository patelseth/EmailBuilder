using EmailBuilderApi.Domain.Models;

namespace EmailBuilderApi.Api.Requests
{
    /// <summary>
    /// Request model for sending an email.
    /// </summary>
    public class SendEmailRequest
    {
        /// <summary>
        /// The HTML content of the email.
        /// </summary>
        public required string HtmlContent { get; set; }

        /// <summary>
        /// The recipient's email address.
        /// </summary>
        public required string Recipient { get; set; }

        /// <summary>
        /// The subject of the email.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// The CC (carbon copy) recipients.
        /// </summary>
        public List<string>? Cc { get; set; }

        /// <summary>
        /// The BCC (blind carbon copy) recipients.
        /// </summary>
        public List<string>? Bcc { get; set; }

        /// <summary>
        /// The attachments to include in the email.
        /// </summary>
        public List<EmailAttachment>? Attachments { get; set; }
    }
}
