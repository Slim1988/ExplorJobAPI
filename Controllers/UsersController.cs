using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.Users;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Domain.Services.Users;
using Microsoft.AspNetCore.Http;
using ExplorJobAPI.Domain.Exceptions.Users;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : OverrideController
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserPhotoService _userPhotoService;


        public UsersController(
            IUsersRepository usersRepository,
            IUserPhotoService userPhotoService
        ) {
            _usersRepository = usersRepository;
            _userPhotoService = userPhotoService;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll() {
            return new JsonResult(
                await _usersRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("many-by-ids/{ids}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetManyByIds(
            string ids
        ) {
            return new JsonResult(
                await _usersRepository.FindManyByIds(
                    ids.Split(',').ToList()
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("public/one-by-id/{id}")]
        public async Task<ActionResult<UserPublicDto>> GetPublicOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            User user = await _usersRepository.FindOneById(
                id
            );

            if (user == null) {
                return NotFound();
            }

            return new JsonResult(
                user.ToPublicDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("restricted/one-by-id/{id}")]
        public async Task<ActionResult<UserRestrictedDto>> GetRestrictedOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            User user = await _usersRepository.FindOneById(
                id
            );

            if (user == null) {
                return NotFound();
            }

            return new JsonResult(
                user.ToRestrictedDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<UserDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            User user = await _usersRepository.FindOneById(
                id
            );

            if (user == null) {
                return NotFound();
            }

            return new JsonResult(
                user.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-email/{email}")]
        public async Task<ActionResult<UserDto>> GetOneByEmail(
            string email
        ) {
            if (email == null) {
                return BadRequest();
            }

            User user = await _usersRepository.FindOneByEmail(
                email
            );

            if (user == null) {
                return NotFound();
            }

            return new JsonResult(
                user.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("is-professional")]
        public async Task<ActionResult> IsProfessional(
            [FromBody] UserIsProfessionalCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _usersRepository.IsProfessional(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("upload-photo")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(5242880)]
        public async Task<ActionResult> UploadPhoto() {
            IFormFile file = Request.Form.Files[0];

            if (file == null) {
                return BadRequest();
            }

            var userId = file.FileName;

            try {
                var isSucceeded = await _userPhotoService.Save(
                    userId,
                    file
                );

                return isSucceeded
                    ? StatusCode(200)
                    : StatusCode(500);
            }
            catch (UserPhotoTypeNotAllowedException) {
                return BadRequest();
            }
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update-general-informations")]
        public async Task<ActionResult> UpdateGeneralInformations(
            [FromBody] UserGeneralInformationsUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _usersRepository.UpdateGeneralInformations(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update-contact-informations")]
        public async Task<ActionResult> UpdateContactInformations(
            [FromBody] UserContactInformationsUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _usersRepository.UpdateContactInformations(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update-situation-informations")]
        public async Task<ActionResult> UpdateSituationInformations(
            [FromBody] UserSituationInformationsUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _usersRepository.UpdateSituationInformations(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _usersRepository.Update(
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

            var response = await _usersRepository.Delete(
                new UserDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
