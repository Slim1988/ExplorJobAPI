using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserContactMethods;
using ExplorJobAPI.Domain.Dto.UserContactMethods;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.UserContactMethods;
using ExplorJobAPI.Domain.Repositories.UserContactMethods;
using ExplorJobAPI.Infrastructure.Controllers;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserContactMethodsController : OverrideController
    {
        private readonly IUserContactMethodsRepository _userContactMethodsRepository;

        public UserContactMethodsController(
            IUserContactMethodsRepository userContactMethodsRepository
        ) {
            _userContactMethodsRepository = userContactMethodsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserContactMethodDto>>> GetAll() {
            return new JsonResult(
                await _userContactMethodsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserContactMethodDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            UserContactMethod userContactMethod = await _userContactMethodsRepository.FindOneById(
                id
            );

            if (userContactMethod == null) {
                return NotFound();
            }

            return new JsonResult(
                userContactMethod.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserContactMethodCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userContactMethodsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserContactMethodUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _userContactMethodsRepository.Update(
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

            var response = await _userContactMethodsRepository.Delete(
                new UserContactMethodDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
