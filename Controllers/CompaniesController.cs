using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.Companies;
using ExplorJobAPI.Domain.Dto.Companies;
using ExplorJobAPI.Domain.Repositories.Companies;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CompaniesController : OverrideController
    {  
        private readonly ICompaniesRepository _companiesRepository;

        public CompaniesController(
            ICompaniesRepository companiesRepository
        ) {
            _companiesRepository = companiesRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll() {
            return new JsonResult(
                await _companiesRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("many-by-ids/{ids}")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetManyByIds(
            string ids
        ) {
            return new JsonResult(
                await _companiesRepository.FindManyDtoByIds(
                    ids.Split(',').ToList()
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("one-by-slug/{slug}")]
        public async Task<ActionResult<CompanyDto>> GetOneBySlug(
            string slug
        ) {
            if (slug == null) {
                return BadRequest();
            }

            CompanyDto company = await _companiesRepository.FindOneDtoBySlug(
                slug
            );

            if (company == null) {
                return NotFound();
            }

            return new JsonResult(
                company
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<CompanyDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            CompanyDto company = await _companiesRepository.FindOneDtoById(
                id
            );

            if (company == null) {
                return NotFound();
            }

            return new JsonResult(
                company
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] CompanyCreateCommand command
        ) {
            if (command == null
            || !command.IsValid()
            ) {
                return BadRequest();
            }

            var response = await _companiesRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] CompanyUpdateCommand command
        ) {
            if (command == null
            || !command.IsValid()
            ) {
                return BadRequest();
            }

            var response = await _companiesRepository.Update(
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

            var response = await _companiesRepository.Delete(
                new CompanyDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
