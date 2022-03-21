using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Models.Offers;

namespace ExplorJobAPI.Domain.Repositories.Offers
{
    public interface IOfferTypesRepository
    {
        List<OfferType> FindAll();
    }
}
