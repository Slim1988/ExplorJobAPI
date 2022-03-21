using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;
using ExplorJobAPI.Domain.Repositories.UserReporting;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Auth.Roles;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserReportingReasonsController : OverrideController
    {
        private readonly IUserReportingReasonsRepository _userReportingReasonsRepository;

        public UserReportingReasonsController(
            IUserReportingReasonsRepository userReportingReasonsRepository
        ) {
            _userReportingReasonsRepository = userReportingReasonsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserReportingReasonDto>>> GetAll() {
            return new JsonResult(
                await _userReportingReasonsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserReportingReasonDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            UserReportingReason userReportingReason = await _userReportingReasonsRepository.FindOneById(
                id
            );

            if (userReportingReason == null) {
                return NotFound();
            }

            return new JsonResult(
                userReportingReason.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserReportingReasonCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userReportingReasonsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserReportingReasonUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userReportingReasonsRepository.Update(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            var response = await _userReportingReasonsRepository.Delete(
                new UserReportingReasonDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
