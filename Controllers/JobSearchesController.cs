using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Domain.Queries.Jobs;
using ExplorJobAPI.Domain.Repositories.Jobs;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobSearchesController : OverrideController
    {
        private readonly IJobSearchesRepository _jobSearchesRepository;

        public JobSearchesController(
            IJobSearchesRepository jobSearchesRepository
        ) {
            _jobSearchesRepository = jobSearchesRepository;
        }

        [AllowAnonymous]
        [HttpPost("search-public")]
        public async Task<ActionResult<JobSearchResultsPublic>> SearchPublic(
            [FromBody] JobSearchQuery query
        ) {
            if (query == null) {
                return BadRequest();
            }

            return new JsonResult(
                await _jobSearchesRepository.SearchPublic(
                    query
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpPost("search-restricted")]
        public async Task<ActionResult<JobSearchResultsRestricted>> SearchRestricted(
            [FromBody] JobSearchQuery query
        ) {
            if (query == null) {
                return BadRequest();
            }

            return new JsonResult(
                await _jobSearchesRepository.SearchRestricted(
                    query
                )
            );
        }
    }
}
