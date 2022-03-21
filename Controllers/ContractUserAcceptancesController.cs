using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Infrastructure.Controllers;
using ExplorJobAPI.Auth.Roles;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContractUserAcceptancesController : OverrideController
    {
        private readonly IContractUserAcceptancesRepository _contractUserAcceptancesRepository;

        public ContractUserAcceptancesController(
            IContractUserAcceptancesRepository contractUserAcceptancesRepository
        ) {
            _contractUserAcceptancesRepository = contractUserAcceptancesRepository; 
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("many-by-contract-id/{contractId}")]
        public async Task<ActionResult<IEnumerable<ContractUserAcceptanceDto>>> GetManyByContractId(
            string contractId
        ) {
            return new JsonResult(
                await _contractUserAcceptancesRepository.FindManyByContractId(
                    contractId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("many-by-user-id/{userId}")]
        public async Task<ActionResult<IEnumerable<ContractUserAcceptanceDto>>> GetManyByUserId(
            string userId
        ) {
            return new JsonResult(
                await _contractUserAcceptancesRepository.FindManyByUserId(
                    userId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] ContractUserAcceptanceCreateCommand command
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.UserId)) {
                return Unauthorized();
            }

            if (command == null) {
                return BadRequest();
            }

            var response = await _contractUserAcceptancesRepository.Create(
                command
            );

            return new JsonResult(response);
        }
    }
}
