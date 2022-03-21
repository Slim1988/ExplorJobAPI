using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.Admin;
using ExplorJobAPI.Domain.Models.Admin;
using ExplorJobAPI.Domain.Queries.Admin;
using ExplorJobAPI.Domain.Services.Admin;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AdminController : OverrideController
    {
        private readonly IAdminService _adminService;

        public AdminController(
            IAdminService adminService
        ) {
            _adminService = adminService;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public ActionResult<AuthTokenAdmin> Token(
            [FromBody] AdminAuthTokenQuery query
        ) {
            if (query == null) {
                return BadRequest();
            }

            var token = _adminService.GetAdminAuthToken(
                query
            );

            if (token == null) {
                return Unauthorized();
            }

            return new JsonResult(token);
        }

        [Authorize(Roles = Authorizations.Administrator)]
        [HttpPost("add-user")]
        public async Task<ActionResult> AddUser(
            [FromBody] AddUserCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var isSucceeded = await _adminService.AddUser(
                command
            );

            return isSucceeded
                ? StatusCode(201)
                : StatusCode(500);
        }

        [Authorize(Roles = Authorizations.Administrator)]
        [HttpPost("add-job-user")]
        public async Task<ActionResult> AddJobUser(
            [FromBody] AddJobUserCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var isSucceeded = await _adminService.AddJobUser(
                command
            );

            return isSucceeded
                ? StatusCode(201)
                : StatusCode(500);
        }
    }
}
