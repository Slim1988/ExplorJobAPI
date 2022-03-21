using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Domain.Commands.Emails;
using Microsoft.AspNetCore.Authorization;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Services.Emails;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EmailsController : OverrideController
    {
        private ISendEmailService _sendEmailService;

        public EmailsController(
            ISendEmailService sendEmailService
        ) {
            _sendEmailService = sendEmailService;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("send-many")]
        public async Task<ActionResult> SendMany(
            [FromBody] SendEmailsCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var isSucceeded = await _sendEmailService.SendMany(
                command
            );

            return isSucceeded
                ? StatusCode(200)
                : StatusCode(500);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("send-one")]
        public async Task<ActionResult> SendOne(
            [FromBody] SendEmailCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var isSucceeded = await _sendEmailService.SendOne(
                command
            );

            return isSucceeded
                ? StatusCode(200)
                : StatusCode(500);
        }
    }
}
