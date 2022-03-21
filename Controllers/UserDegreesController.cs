using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserDegrees;
using ExplorJobAPI.Domain.Dto.UserDegrees;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.UserDegrees;
using ExplorJobAPI.Domain.Repositories.UserDegrees;
using ExplorJobAPI.Infrastructure.Controllers;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserDegreesController : OverrideController
    {
        private readonly IUserDegreesRepository _userDegreesRepository;

        public UserDegreesController(
            IUserDegreesRepository userDegreesRepository
        ) {
            _userDegreesRepository = userDegreesRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDegreeDto>>> GetAll() {
            return new JsonResult(
                await _userDegreesRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserDegreeDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            UserDegree userDegree = await _userDegreesRepository.FindOneById(
                id
            );

            if (userDegree == null) {
                return NotFound();
            }

            return new JsonResult(
                userDegree.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserDegreeCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userDegreesRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserDegreeUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userDegreesRepository.Update(
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

            var response = await _userDegreesRepository.Delete(
                new UserDegreeDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
