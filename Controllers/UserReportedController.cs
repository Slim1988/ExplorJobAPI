using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;
using ExplorJobAPI.Domain.Repositories.UserReporting;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserReportedController : OverrideController
    {
        private readonly IUserReportedRepository _userReportedRepository;

        public UserReportedController(
            IUserReportedRepository userReportedRepository
        ) {
            _userReportedRepository = userReportedRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserReportedDto>>> GetAll() {
            return new JsonResult(
                await _userReportedRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserReportedDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            UserReported userReported = await _userReportedRepository.FindOneById(
                id
            );

            if (userReported == null) {
                return NotFound();
            }

            return new JsonResult(
                userReported.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserReportedCreateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.ReporterId)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _userReportedRepository.Create(
                command
            );

            return new JsonResult(response);
        }
    }
}
