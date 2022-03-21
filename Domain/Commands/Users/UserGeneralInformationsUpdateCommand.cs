using System;

namespace ExplorJobAPI.Domain.Commands.Users
{
    public class UserGeneralInformationsUpdateCommand
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Presentation { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
