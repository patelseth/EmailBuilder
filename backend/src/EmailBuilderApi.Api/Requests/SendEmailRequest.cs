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
    }
}
