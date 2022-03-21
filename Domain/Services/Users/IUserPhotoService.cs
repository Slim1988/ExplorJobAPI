using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExplorJobAPI.Domain.Services.Users
{
    public interface IUserPhotoService
    {
        Task<bool> Save(
            string userId,
            IFormFile file
        );

        bool Delete(
            string userId
        );
    }
}
