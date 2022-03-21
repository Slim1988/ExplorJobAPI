using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.Agglomeration;
using ExplorJobAPI.Domain.Commands.Agglomerations;
using ExplorJobAPI.Domain.Dto.Agglomerations;
using ExplorJobAPI.Domain.Models.Agglomerations;
using ExplorJobAPI.Domain.Repositories.Agglomerations;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AgglomerationsController : OverrideController
    {
        private readonly IAgglomerationsRepository _agglomerationsRepository;

        public AgglomerationsController(
            IAgglomerationsRepository agglomerationsRepository
        ) {
            _agglomerationsRepository = agglomerationsRepository;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AgglomerationDto>>> GetAll() {
            return new JsonResult(
                await _agglomerationsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<AgglomerationDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            Agglomeration agglomeration = await _agglomerationsRepository.FindOneById(
                id
            );

            if (agglomeration == null) {
                return NotFound();
            }

            return new JsonResult(
                agglomeration.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] AgglomerationCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _agglomerationsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] AgglomerationUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _agglomerationsRepository.Update(
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

            var response = await _agglomerationsRepository.Delete(
                new AgglomerationDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
