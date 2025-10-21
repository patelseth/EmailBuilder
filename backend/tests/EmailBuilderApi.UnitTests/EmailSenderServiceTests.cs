using Moq;

namespace EmailBuilderApi.UnitTests;

public class EmailSenderServiceTests
{
    [Fact]
    public async Task SendEmailAsync_WithValidInput_CallsSendGridApi()
    {
        // Arrange
        var mockSendGridClient = new Mock<ISendGridClient>();
        var service = new EmailSenderService(mockSendGridClient.Object);
        var htmlContent = "<h1>Hello</h1>";
        var recipient = "test@example.com";

        // Act
        var result = await service.SendEmailAsync(htmlContent, recipient);

        // Assert
        mockSendGridClient.Verify(x => x.SendEmailAsync(htmlContent, recipient), Times.Once);
    }
}
