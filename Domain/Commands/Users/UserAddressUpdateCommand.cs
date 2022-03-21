namespace ExplorJobAPI.Domain.Commands.Users
{
    public class UserAddressUpdateCommand
    {
        public string Street { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}
