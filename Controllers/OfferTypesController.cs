using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Auth.Roles;
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
    public class OfferTypesController : OverrideController
    {
        private readonly IOfferTypesRepository _offerTypesRepository;

        public OfferTypesController(
            IOfferTypesRepository offerTypesRepository
        ) {
            _offerTypesRepository = offerTypesRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpGet("all")]
        public ActionResult<IEnumerable<OfferType>> GetAll() {
            return new JsonResult(
                _offerTypesRepository.FindAll()
            );
        }
    }
}
