namespace ExplorJobAPI.Domain.Dto.Users
{
    public class UserPublicDto
    {
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LocalisationCity { get; set; }

        public UserPublicDto(
            string id,
            string photoUrl,
            string firstName,
            string lastName,
            string localisationCity
        ) {
            Id = id;
            PhotoUrl = photoUrl;
            FirstName = firstName;
            LastName = lastName;
            LocalisationCity = localisationCity;
        }
    }
}
