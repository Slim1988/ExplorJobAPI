using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Domain.Repositories.Jobs;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobUsersController : OverrideController
    {
        private readonly IJobUsersRepository _jobUsersRepository;

        public JobUsersController(
            IJobUsersRepository jobUsersRepository
        )
        {
            _jobUsersRepository = jobUsersRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<JobUserDto>>> GetAll()
        {
            return new JsonResult(
                await _jobUsersRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("all/by-user-id/{userId}")]
        public async Task<ActionResult<IEnumerable<JobUserDto>>> GetAllByUserId(
            string userId
        )
        {
            return new JsonResult(
                await _jobUsersRepository.FindAllDtoByUserId(
                    userId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<JobUserDto>> GetOneById(
            string id
        )
        {
            if (id == null)
            {
                return BadRequest();
            }

            JobUser jobUser = await _jobUsersRepository.FindOneById(
                id
            );

            if (jobUser == null)
            {
                return NotFound();
            }

            return new JsonResult(
                jobUser.ToDto()
            );
        }

        [AllowAnonymous]
        [HttpGet("companies/all")]
        public async Task<ActionResult<JobUserDto>> GetAllCompanies() 
        {
            return new JsonResult(
               await _jobUsersRepository.FindAllCompanies()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] JobUserCreateCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.UserId))
            {
                return Unauthorized();
            }

            if (command == null || !command.IsValid())
            {
                return BadRequest();
            }

            var response = await _jobUsersRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] JobUserUpdateCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.UserId))
            {
                return Unauthorized();
            }

            if (command == null || !command.IsValid())
            {
                return BadRequest();
            }

            var response = await _jobUsersRepository.Update(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("delete")]
        public async Task<ActionResult> Delete(
            [FromBody] JobUserDeleteCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.UserId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }

            var response = await _jobUsersRepository.Delete(
                command
            );

            return new JsonResult(response);
        }
    }
}
