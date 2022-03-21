namespace ExplorJobAPI.Domain.Models.Users
{
    public class UserAddress
    {
        public string Street { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        
        public UserAddress(
            string street,
            string complement,
            string zipCode,
            string city
        ) {
            Street = street;
            Complement = complement;
            ZipCode = zipCode;
            City = city; 
        }
    }
}
