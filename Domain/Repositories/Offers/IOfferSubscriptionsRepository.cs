using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Offers;
using ExplorJobAPI.Domain.Models.Offers;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Offers
{
    public interface IOfferSubscriptionsRepository
    {
        Task<IEnumerable<OfferSubscription>> FindAll();

        Task<IEnumerable<Promote>> FindAllPromotesForSearchForm();

        Task<IEnumerable<OfferSubscription>> FindManyByCompanyIds(
            List<string> ids
        );

        Task<OfferSubscription> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            OfferSubscriptionCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            OfferSubscriptionUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            OfferSubscriptionDeleteCommand command
        );
    }
}
