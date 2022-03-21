using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserMeetings;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.UserMeetings;
using ExplorJobAPI.Domain.Repositories.UserMeetings;
using ExplorJobAPI.Infrastructure.Controllers;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserMeetingsController : OverrideController
    {
        private readonly IUserMeetingsRepository _userMeetingsRepository;

        public UserMeetingsController(
            IUserMeetingsRepository userMeetingsRepository
        ) {
            _userMeetingsRepository = userMeetingsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("all-by-instigatorId/{instigatorId}")]
        public async Task<ActionResult<IEnumerable<UserMeeting>>> GetAllByInstigatorId(
            string instigatorId
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(instigatorId)) {
                return Unauthorized();
            }

            return new JsonResult(
                await _userMeetingsRepository.FindAllByInstigatorId(
                    instigatorId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserMeetingCreateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.InstigatorId)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _userMeetingsRepository.Create(
                command
            );

            return new JsonResult(response);
        }
    }
}
