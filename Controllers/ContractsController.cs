using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Domain.Models.Contracts;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Auth.Roles;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContractsController : OverrideController
    {
        private readonly IContractsRepository _contractsRepository;

        public ContractsController(
            IContractsRepository contractsRepository
        ) {
            _contractsRepository = contractsRepository; 
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetAll() {
            return new JsonResult(
                await _contractsRepository.FindAllDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployeeOrUser)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<ContractDto>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            Contract contract = await _contractsRepository.FindOneById(
                id
            );

            if (contract == null) {
                return NotFound();
            }

            return new JsonResult(
                contract.ToDto()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("publish")]
        public async Task<ActionResult> Publish(
            [FromBody] ContractPublishCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _contractsRepository.Publish(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] ContractCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _contractsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] ContractUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _contractsRepository.Update(
                command
            );

            return new JsonResult(response);
        }
    }
}
