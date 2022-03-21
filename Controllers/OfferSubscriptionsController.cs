using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Commands.Offers;
using ExplorJobAPI.Domain.Models.Offers;
using ExplorJobAPI.Domain.Repositories.Offers;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OfferSubscriptionsController : OverrideController
    {  
        private readonly IOfferSubscriptionsRepository _offerSubscriptionsRepository;

        public OfferSubscriptionsController(
            IOfferSubscriptionsRepository offerSubscriptionsRepository
        ) {
            _offerSubscriptionsRepository = offerSubscriptionsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<OfferSubscription>>> GetAll() {
            return new JsonResult(
                await _offerSubscriptionsRepository.FindAll()
            );
        }

        [AllowAnonymous]
        [HttpGet("all-promotes-for-search-form")]
        public async Task<ActionResult<IEnumerable<Promote>>> GetAllPromotesForSearchForm() {
            return new JsonResult(
                await _offerSubscriptionsRepository.FindAllPromotesForSearchForm()
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("one-by-id/{id}")]
        public async Task<ActionResult<OfferSubscription>> GetOneById(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            OfferSubscription offerSubscription = await _offerSubscriptionsRepository.FindOneById(
                id
            );

            if (offerSubscription == null) {
                return NotFound();
            }

            return new JsonResult(
                offerSubscription
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("subscribe")]
        public async Task<ActionResult> Subscribe(
            [FromBody] OfferSubscriptionCreateCommand command
        ) {
            if (command == null
            && command.IsValid()
            ) {
                return BadRequest();
            }

            var response = await _offerSubscriptionsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPut("edit-subscription")]
        public async Task<ActionResult> EditSubscription(
            [FromBody] OfferSubscriptionUpdateCommand command
        ) {
            if (command == null
            && command.IsValid()
            ) {
                return BadRequest();
            }

            var response = await _offerSubscriptionsRepository.Update(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpDelete("cancel-subscription/{id}")]
        public async Task<ActionResult> CancelSubscription(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            var response = await _offerSubscriptionsRepository.Delete(
                new OfferSubscriptionDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
