using System.Collections.Generic;
using ExplorJobAPI.Domain.Models.Offers;
using ExplorJobAPI.Domain.Repositories.Offers;

namespace ExplorJobAPI.DAL.Repositories.Offers
{
    public class OfferTypesRepository : IOfferTypesRepository
    {
        public OfferTypesRepository() { }

        public List<OfferType> FindAll() {
            return Domain.Models.Offers.Offers.AsList();
        }
    }
}
