using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.KeywordLists;
using ExplorJobAPI.Domain.Models.KeywordLists;
using ExplorJobAPI.Domain.Repositories.KeywordLists;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class KeywordListsController : OverrideController
    {  
        private readonly IKeywordListsRepository _keywordListsRepository;

        public KeywordListsController(
            IKeywordListsRepository keywordListsRepository
        ) {
            _keywordListsRepository = keywordListsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<KeywordList>>> GetAll() {
            return new JsonResult(
                await _keywordListsRepository.FindAll()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<KeywordList>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            KeywordList keywordList = await _keywordListsRepository.FindOneById(
                id
            );

            if (keywordList == null) {
                return NotFound();
            }

            return new JsonResult(
                keywordList
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] KeywordListCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _keywordListsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] KeywordListUpdateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _keywordListsRepository.Update(
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

            var response = await _keywordListsRepository.Delete(
                new KeywordListDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
