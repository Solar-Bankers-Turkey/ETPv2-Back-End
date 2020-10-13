using Microsoft.AspNetCore.Mvc;
using NotificationService.Email;
using NotificationService.Models;

namespace NotificationService.Controllers {
    [Route("api/v1/email")]
    [ApiController]
    public class EmailController : ControllerBase {

        private readonly IEmailSender _emailSender;
        private const string NOREPLY_ADDRESS = "noreply@solarbankers.org";

        public EmailController(IEmailSender emailSender) {
            _emailSender = emailSender;
        }

        /// <summary>
        /// email send
        /// </summary>
        /// <returns>void</returns>
        /// <response code="200">Email sent to recipient</response>
        [HttpPost]
        [Route("send")]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public void sendEmail([FromBody] EmailModel emailModel) {
            _emailSender.Send(NOREPLY_ADDRESS, emailModel.to, emailModel.subject, emailModel.body, emailModel.isBodyHtml);
        }
    }
}
