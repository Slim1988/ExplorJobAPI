using System;

namespace ExplorJobAPI.Domain.Commands.UserProfessionalSituations
{
    public class UserProfessionalSituationCreateCommand
    {
        public string Label { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; } 
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
