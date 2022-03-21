using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Users
{
    public class UserUpdateCommand
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public UserAddressUpdateCommand Address { get; set; }
        public string SkypeId { get; set; }
        public string Presentation { get; set; }
        public bool IsProfessional { get; private set; }
        public string LastDegreeId { get; set; }
        public string ProfessionalSituationId { get; set; }
        public string CurrentCompany { get; set; }
        public string CurrentSchool { get; set; }
        public List<string> ContactMethodIds { get; set; }
        public List<string> ContactInformationIds { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
