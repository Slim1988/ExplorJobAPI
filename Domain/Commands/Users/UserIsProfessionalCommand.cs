using System;

namespace ExplorJobAPI.Domain.Commands.Users
{
    public class UserIsProfessionalCommand
    {
        public string Id { get; set; }

        public bool IsProfessional {
            get { return true; }
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
