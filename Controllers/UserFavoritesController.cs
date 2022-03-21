using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.UserFavorites;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.UserFavorites;
using ExplorJobAPI.Domain.Repositories.UserFavorites;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Domain.Exceptions.UserFavorites;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserFavoritesController : OverrideController
    {
        private readonly IUserFavoritesRepository _userFavoritesRepository;

        public UserFavoritesController(
            IUserFavoritesRepository userFavoritesRepository
        ) {
            _userFavoritesRepository = userFavoritesRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("all-by-ownerId/{ownerId}")]
        public async Task<ActionResult<IEnumerable<UserFavorite>>> GetAllByOwnerId(
            string ownerId
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(ownerId)) {
                return Unauthorized();
            }

            return new JsonResult(
                await _userFavoritesRepository.FindAllByOwnerId(
                    ownerId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] UserFavoriteCreateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.OwnerId)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            try {
                var response = await _userFavoritesRepository.Create(
                    command
                );

                return new JsonResult(response);
            }
            catch(UserFavoriteAlreadyExistException) {
                return Conflict();
            }
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            var response = await _userFavoritesRepository.Delete(
                new UserFavoriteDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
