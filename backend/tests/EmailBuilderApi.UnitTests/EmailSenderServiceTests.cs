using Moq;
using EmailBuilderApi.Application;

namespace EmailBuilderApi.UnitTests
{

    public class EmailSenderServiceTests
    {
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
    }
}