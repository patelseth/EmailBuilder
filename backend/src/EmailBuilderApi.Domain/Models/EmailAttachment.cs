namespace EmailBuilderApi.Domain.Models
{
    /// <summary>
    /// Represents a file attachment for an email message.
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// The name of the file as it will appear in the email.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// The binary content of the file.
        /// </summary>
        public byte[] Content { get; set; } = [];

        /// <summary>
        /// The MIME type of the file (e.g., "application/pdf", "image/png").
        /// </summary>
        public string MimeType { get; set; } = "application/octet-stream";
    }
}
