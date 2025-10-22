using Moq;
using Microsoft.AspNetCore.Mvc;
using EmailBuilderApi.Api.Controllers;
using EmailBuilderApi.Api.Requests;
using EmailBuilderApi.Domain.Models;
using EmailBuilderApi.Application.Interfaces;

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

        [Fact]
        public async Task SendEmail_WithAllFields_CallsEmailSenderServiceWithAllParameters()
        {
            // Arrange
            var mockEmailSenderService = new Mock<IEmailSenderService>();
            var controller = new EmailController(mockEmailSenderService.Object);
            var htmlContent = "<h1>Hello</h1>";
            var recipient = "test@example.com";
            var subject = "Test Subject";
            var cc = new List<string> { "cc1@example.com", "cc2@example.com" };
            var bcc = new List<string> { "bcc1@example.com" };
            var attachments = new List<EmailAttachment>
            {
                new() { FileName = "file.txt", Content = [1, 2, 3], MimeType = "text/plain" }
            };
            var request = new SendEmailRequest
            {
                HtmlContent = htmlContent,
                Recipient = recipient,
                Subject = subject,
                Cc = cc,
                Bcc = bcc,
                Attachments = attachments
            };

            // Act
            var result = await controller.SendEmail(request);

            // Assert
            mockEmailSenderService.Verify(x => x.SendEmailAsync(htmlContent, recipient, subject, cc, bcc, attachments), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SendEmail_MissingOrEmptyRecipient_ReturnsBadRequest(string? recipient)
        {
            // Arrange
            var mockEmailSenderService = new Mock<IEmailSenderService>();
            var controller = new EmailController(mockEmailSenderService.Object);
            var request = new SendEmailRequest { HtmlContent = "<h1>Hello</h1>", Recipient = recipient! };

            // Act
            var result = await controller.SendEmail(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SendEmail_MissingOrEmptyHtmlContent_ReturnsBadRequest(string? htmlContent)
        {
            // Arrange
            var mockEmailSenderService = new Mock<IEmailSenderService>();
            var controller = new EmailController(mockEmailSenderService.Object);
            var request = new SendEmailRequest { HtmlContent = htmlContent!, Recipient = "test@example.com" };

            // Act
            var result = await controller.SendEmail(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SendEmail_WhenServiceThrows_ReturnsInternalServerError()
        {
            // Arrange
            var mockEmailSenderService = new Mock<IEmailSenderService>();
            mockEmailSenderService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>(), It.IsAny<List<EmailAttachment>>()))
                .ThrowsAsync(new Exception("Unexpected error"));
            var controller = new EmailController(mockEmailSenderService.Object);
            var request = new SendEmailRequest { HtmlContent = "<h1>Hello</h1>", Recipient = "test@example.com" };

            // Act
            var result = await controller.SendEmail(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
