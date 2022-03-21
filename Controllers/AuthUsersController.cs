using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Exceptions.AuthUsers;
using ExplorJobAPI.Domain.Commands.AuthUsers;
using ExplorJobAPI.Domain.Queries.AuthUsers;
using ExplorJobAPI.Domain.Services.AuthUsers;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.AuthUsers;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthUsersController : OverrideController
    {
        private readonly IAuthUsersService _authUsersService;

        public AuthUsersController(
            IAuthUsersService authUsersService
        ) {
            _authUsersService = authUsersService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthTokenUser>> Login(
            [FromBody] UserLoginQuery query
        ) {
            if (query == null) {
                return BadRequest();
            }

            try {
                var response = await _authUsersService.Login(
                    query
                );

                return new JsonResult(response);
            }
            catch (UserNotFoundException) {
                return NotFound();
            }
            catch (UserEmailNotConfirmedException) {
                return StatusCode(417);
            }
            catch (UserInvalidPasswordException) {
                return Unauthorized();
            }
            catch (Exception) {
                return StatusCode(500);
            }        
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(
            [FromBody] UserRegisterCommand command
        ) {
            if (command == null 
            || !command.IsValid()) {
                return BadRequest();
            }

            var isSucceeded = await _authUsersService.Register(
                command
            );

            return isSucceeded
                ? StatusCode(201)
                : StatusCode(500);
        }

        [Authorize(Roles = Authorizations.Administrator)]
        [HttpPut("get-new-email-token-confirmation")]
        public async Task<ActionResult> GetNewEmailTokenConfirmation(
            [FromBody] UserGetNewEmailTokenConfirmationCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var isSucceeded = await _authUsersService.GetNewEmailTokenConfirmation(
                command
            );

            return isSucceeded
                ? StatusCode(200)
                : StatusCode(500);
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(
            [FromBody] UserConfirmEmailCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            try {
                var isSucceeded = await _authUsersService.ConfirmEmail(
                    command
                );

                return isSucceeded
                    ? StatusCode(200)
                    : StatusCode(500);
            }
            catch (UserNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500);
            } 
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword(
            [FromBody] UserChangePasswordCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            try {
                var isSucceeded = await _authUsersService.ChangePassword(
                    command
                );

                return isSucceeded
                    ? StatusCode(200)
                    : StatusCode(500);
            }
            catch (UserNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500);
            } 
        }

        [AllowAnonymous]
        [HttpPost("forgotten-password")]
        public async Task<ActionResult> ForgottenPassword(
            [FromBody] UserForgottenPasswordCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            try {
                var isSucceeded = await _authUsersService.ForgottenPassword(
                    command
                );

                return isSucceeded
                    ? StatusCode(200)
                    : StatusCode(500);
            }
            catch (UserNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500);
            } 
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(
            [FromBody] UserResetPasswordCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            try {
                var isSucceeded = await _authUsersService.ResetPassword(
                    command
                );

                return isSucceeded
                    ? StatusCode(200)
                    : StatusCode(500);
            }
            catch (UserNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500);
            }
        }
    }
}
