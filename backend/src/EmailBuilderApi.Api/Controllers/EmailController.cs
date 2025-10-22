using Microsoft.AspNetCore.Mvc;
using EmailBuilderApi.Application;
using EmailBuilderApi.Api.Requests;

namespace EmailBuilderApi.Api.Controllers
{
    /// <summary>
    /// Controller for sending emails via the API.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EmailController"/> class.
    /// </remarks>
    /// <param name="emailSenderService">The email sender service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController(IEmailSenderService emailSenderService) : ControllerBase
    {
        /// <summary>
        /// Sends an email using the provided HTML content and recipient address.
        /// </summary>
        /// <param name="request">The email request containing HTML content and recipient.</param>
        /// <returns>HTTP 200 OK if sent successfully.</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(request.Recipient))
                return BadRequest("Recipient is required.");
            if (string.IsNullOrWhiteSpace(request.HtmlContent))
                return BadRequest("HTML content is required.");

            try
            {
                await emailSenderService.SendEmailAsync(
                    request.HtmlContent,
                    request.Recipient,
                    request.Subject,
                    request.Cc,
                    request.Bcc,
                    request.Attachments);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
