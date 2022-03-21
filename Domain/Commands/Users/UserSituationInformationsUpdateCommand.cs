using System;

namespace ExplorJobAPI.Domain.Commands.Users
{
    public class UserSituationInformationsUpdateCommand
    {
        public string Id { get; set; }

        public string LastDegreeId { get; set; }
        public string ProfessionalSituationId { get; set; }
        public string CurrentCompany { get; set; }
        public string CurrentSchool { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
