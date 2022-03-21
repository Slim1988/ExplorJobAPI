using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Domain.Models.Users;

namespace ExplorJobAPI.Domain.Models.AccountUsers
{
    public class AccountUser
    {
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserAddress Address { get; set; }
        public string SkypeId { get; set; }
        public UserDto User { get; set; }
        public List<JobUserDto> Jobs { get; set; }
        public List<AccountUserFavorite> Favorites { get; set; }
        public List<AccountUserConversation> Conversations { get; set; }
        public bool HasAlreadyBeenUpdated { get; set; }

        public AccountUser(
            string id,
            string photoUrl,
            string email,
            string phone,
            UserAddress address,
            string skypeId,
            UserDto user,
            List<JobUserDto> jobs,
            List<AccountUserFavorite> favorites,
            List<AccountUserConversation> conversations,
            bool hasAlreadyBeenUpdated
        ) {
            Id = id;
            PhotoUrl = photoUrl;
            Email = email;
            Phone = phone;
            Address = address;
            SkypeId = skypeId;
            User = user;
            Jobs = jobs;
            Favorites = favorites;
            Conversations = conversations;
            HasAlreadyBeenUpdated = hasAlreadyBeenUpdated;
        }
    }
}
