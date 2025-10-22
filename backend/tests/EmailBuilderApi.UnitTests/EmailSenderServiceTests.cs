using Moq;
using EmailBuilderApi.Application;

namespace EmailBuilderApi.UnitTests
{

    public class EmailSenderServiceTests
    {
        /// <summary>
        /// Should call the email provider when valid input is provided.
        /// </summary>
        [Fact]
        public async Task SendEmailAsync_WithValidInput_CallsSendGridApi()
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = "test@example.com";

            // Act
            await service.SendEmailAsync(htmlContent, recipient);

            // Assert
            mockEmailSenderClient.Verify(x => x.SendEmailAsync(htmlContent, recipient), Times.Once);
        }

        /// <summary>
        /// Should throw an exception if recipient is empty.
        /// </summary>
        [Fact]
        public async Task SendEmailAsync_WithEmptyRecipient_ThrowsArgumentException()
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = string.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.SendEmailAsync(htmlContent, recipient));
        }

        /// <summary>
        /// Should throw an exception if HTML content is empty.
        /// </summary>
        [Fact]
        public async Task SendEmailAsync_WithEmptyHtmlContent_ThrowsArgumentException()
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = string.Empty;
            var recipient = "test@example.com";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.SendEmailAsync(htmlContent, recipient));
        }

        /// <summary>
        /// Should propagate exceptions thrown by the email provider.
        /// </summary>
        [Fact]
        public async Task SendEmailAsync_WhenProviderThrows_PropagatesException()
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = "test@example.com";
            var expectedException = new InvalidOperationException("Provider failure");

            mockEmailSenderClient
                .Setup(x => x.SendEmailAsync(htmlContent, recipient))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.SendEmailAsync(htmlContent, recipient));
            Assert.Equal(expectedException, ex);
        }

        /// <summary>
        /// Should throw an exception if recipient email format is invalid.
        /// </summary>
        [Theory]
        [InlineData("plainaddress")]
        [InlineData("missingatsign.com")]
        [InlineData("@missingusername.com")]
        [InlineData("username@.com")]
        [InlineData("username@com")]
        public async Task SendEmailAsync_WithInvalidRecipientFormat_ThrowsArgumentException(string invalidRecipient)
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = "<h1>Hello</h1>";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.SendEmailAsync(htmlContent, invalidRecipient));
        }

        /// <summary>
        /// Should trim whitespace from recipient email address before sending.
        /// </summary>
        [Fact]
        public async Task SendEmailAsync_TrimsWhitespaceFromRecipient_BeforeSending()
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = "   test@example.com   ";

            // Act
            await service.SendEmailAsync(htmlContent, recipient);

            // Assert
            mockEmailSenderClient.Verify(x => x.SendEmailAsync(htmlContent, "test@example.com"), Times.Once);
        }

        /// <summary>
        /// Should pass subject, CC, and BCC to the email provider.
        /// </summary>
        [Fact]
        public async Task SendEmailAsync_PassesSubjectCcBccToProvider()
        {
            // Arrange
            var mockEmailSenderClient = new Mock<IEmailSenderClient>();
            var service = new EmailSenderService(mockEmailSenderClient.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = "test@example.com";
            var subject = "Test Subject";
            var cc = new[] { "cc1@example.com", "cc2@example.com" };
            var bcc = new[] { "bcc1@example.com" };

            // Act
            await service.SendEmailAsync(htmlContent, recipient, subject, cc, bcc);

            // Assert
            mockEmailSenderClient.Verify(x => x.SendEmailAsync(htmlContent, recipient, subject, cc, bcc), Times.Once);
        }
    }
}