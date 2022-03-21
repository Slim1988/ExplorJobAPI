using System.Threading.Tasks;
using ExplorJobAPI.Domain.Services.Users;
using Microsoft.AspNetCore.Http;

namespace ExplorJobAPI.Domain.Services.Account
{
    public class AccountUserPhotoService : IAccountUserPhotoService {
        private readonly IUserPhotoService _userPhotoService;

        public AccountUserPhotoService(
            IUserPhotoService userPhotoService
        ) {
            _userPhotoService = userPhotoService;
        }

        public async Task<bool> Save(
            string userId,
            IFormFile file
        ) {
            return await _userPhotoService.Save(
                userId,
                file
            );
        }

        public bool Delete(
            string userId
        ) {
            return _userPhotoService.Delete(
                userId
            );
        }
    }
}
