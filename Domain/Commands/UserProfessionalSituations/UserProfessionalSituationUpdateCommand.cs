using System;

namespace ExplorJobAPI.Domain.Commands.UserProfessionalSituations
{
    public class UserProfessionalSituationUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
