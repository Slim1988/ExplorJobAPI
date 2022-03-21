using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserFavorites;
using ExplorJobAPI.Domain.Dto.UserFavorites;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserFavorites
{
    public interface IUserFavoritesRepository
    {
        Task<IEnumerable<UserFavoriteDto>> FindAllByOwnerId(
            string ownerId
        );
        
        Task<RepositoryCommandResponse> Create(
            UserFavoriteCreateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserFavoriteDeleteCommand command
        );
    }
}
