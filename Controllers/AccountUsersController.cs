using System.Threading.Tasks;
using ExplorJobAPI.Domain.Models.AccountUsers;
using ExplorJobAPI.Domain.Repositories.AccountUsers;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.AccountUsers;
using Microsoft.AspNetCore.Http;
using ExplorJobAPI.Domain.Services.Account;
using ExplorJobAPI.Domain.Exceptions.Users;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountUsersController : OverrideController
    {
        private readonly IAccountUsersRepository _accountUsersRepository;
        private readonly IAccountUserPhotoService _accountUserPhotoService;

        public AccountUsersController(
            IAccountUsersRepository accountUsersRepository,
            IAccountUserPhotoService accountUserPhotoService
        ) {
            _accountUsersRepository = accountUsersRepository; 
            _accountUserPhotoService = accountUserPhotoService;
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("account/{userId}")]
        public async Task<ActionResult<AccountUser>> Get(
            string userId
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(userId)) {
                return Unauthorized();
            }

            if (userId == null) {
                return BadRequest();
            }

            var account = await _accountUsersRepository.Get(
                userId
            );

            if (account == null) {
                return NotFound();
            }

            return new JsonResult(account);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("is-professional")]
        public async Task<ActionResult> IsProfessional(
            [FromBody] AccountUserIsProfessionalCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.Id)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _accountUsersRepository.IsProfessional(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("upload-photo")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(5242880)]
        public async Task<ActionResult> UploadPhoto() {
            IFormFile file = Request.Form.Files[0];

            if (file == null) {
                return BadRequest();
            }

            var userId = file.FileName;

            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(userId)) {
                return Unauthorized();
            }

            try {
                var isSucceeded = await _accountUserPhotoService.Save(
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

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("update-general-informations")]
        public async Task<ActionResult> UpdateGeneralInformations(
            [FromBody] AccountUserGeneralInformationsUpdateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.Id)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _accountUsersRepository.UpdateGeneralInformations(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("update-contact-informations")]
        public async Task<ActionResult> UpdateContactInformations(
            [FromBody] AccountUserContactInformationsUpdateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.Id)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _accountUsersRepository.UpdateContactInformations(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("update-situation-informations")]
        public async Task<ActionResult> UpdateSituationInformations(
            [FromBody] AccountUserSituationInformationsUpdateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.Id)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _accountUsersRepository.UpdateSituationInformations(
                command
            );

            return new JsonResult(response);
        }
    
        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("delete")]
        public async Task<ActionResult> Delete(
            [FromBody] AccountUserDeleteCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.Id)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _accountUsersRepository.Delete(
                command
            );

            return new JsonResult(response);
        }
    }
}
