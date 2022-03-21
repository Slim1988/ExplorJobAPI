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
    public class JobDomainsController : OverrideController
    {
        private readonly IJobDomainsRepository _jobDomainsRepository;

        public JobDomainsController(
            IJobDomainsRepository jobDomainsRepository
        ) {
            _jobDomainsRepository = jobDomainsRepository;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<JobDomainDto>>> GetAll() {
            return new JsonResult(
                await _jobDomainsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<JobDomainDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            JobDomain jobDomain = await _jobDomainsRepository.FindOneById(
                id
            );

            if (jobDomain == null) {
                return NotFound();
            }

            return new JsonResult(
                jobDomain.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] JobDomainCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _jobDomainsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] JobDomainUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _jobDomainsRepository.Update(
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

            var response = await _jobDomainsRepository.Delete(
                new JobDomainDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
