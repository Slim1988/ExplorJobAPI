using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserContactInformations;
using ExplorJobAPI.Domain.Dto.UserContactInformations;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.UserContactInformations;
using ExplorJobAPI.Domain.Repositories.UserContactInformations;
using ExplorJobAPI.Infrastructure.Controllers;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserContactInformationsController : OverrideController
    {
        private readonly IUserContactInformationsRepository _userContactInformationsRepository;

        public UserContactInformationsController(
            IUserContactInformationsRepository userContactInformationsRepository
        ) {
            _userContactInformationsRepository = userContactInformationsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserContactInformationDto>>> GetAll() {
            return new JsonResult(
                await _userContactInformationsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserContactInformationDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            UserContactInformation userContactInformation = await _userContactInformationsRepository.FindOneById(
                id
            );

            if (userContactInformation == null) {
                return NotFound();
            }

            return new JsonResult(
                userContactInformation.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserContactInformationCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userContactInformationsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserContactInformationUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userContactInformationsRepository.Update(
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

            var response = await _userContactInformationsRepository.Delete(
                new UserContactInformationDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
