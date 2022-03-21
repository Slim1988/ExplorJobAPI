using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Admin
{
    public class AddUserCommand
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsProfessional { get; set; } = false;
        public bool EmailConfirmed { get; set; } = false;
        public List<string> ContractIds { get; set; } = new List<string>();

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; } 
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
