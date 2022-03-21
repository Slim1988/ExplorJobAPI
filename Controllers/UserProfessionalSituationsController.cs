using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserProfessionalSituations;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.UserProfessionalSituations;
using ExplorJobAPI.Domain.Repositories.UserProfessionalSituations;
using ExplorJobAPI.Infrastructure.Controllers;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserProfessionalSituationsController : OverrideController
    {
        private readonly IUserProfessionalSituationsRepository _userProfessionalSituationsRepository;

        public UserProfessionalSituationsController(
            IUserProfessionalSituationsRepository userProfessionalSituationsRepository
        ) {
            _userProfessionalSituationsRepository = userProfessionalSituationsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserProfessionalSituationDto>>> GetAll() {
            return new JsonResult(
                await _userProfessionalSituationsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserProfessionalSituationDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            UserProfessionalSituation userProfessionalSituation = await _userProfessionalSituationsRepository.FindOneById(
                id
            );

            if (userProfessionalSituation == null) {
                return NotFound();
            }

            return new JsonResult(
                userProfessionalSituation.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserProfessionalSituationCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userProfessionalSituationsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserProfessionalSituationUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userProfessionalSituationsRepository.Update(
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

            var response = await _userProfessionalSituationsRepository.Delete(
                new UserProfessionalSituationDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
