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
    }
}