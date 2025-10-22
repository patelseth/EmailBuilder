using Moq;
using Microsoft.AspNetCore.Mvc;
using EmailBuilderApi.Application;

namespace EmailBuilderApi.UnitTests
{
    public class EmailControllerTests
    {
        [Fact]
        public async Task SendEmail_WithValidInput_CallsEmailSenderService()
        {
            // Arrange
            var mockEmailSenderService = new Mock<IEmailSenderService>();
            var controller = new EmailController(mockEmailSenderService.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = "test@example.com";
            var request = new SendEmailRequest { HtmlContent = htmlContent, Recipient = recipient };

            // Act
            var result = await controller.SendEmail(request);

            // Assert
            mockEmailSenderService.Verify(x => x.SendEmailAsync(htmlContent, recipient, null, null, null, null), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}
