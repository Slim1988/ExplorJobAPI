using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.AuthUsers
{
    public class UserRegisterCommand
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public bool IsProfessional { get; set; } = false;
        public List<string> ContractIds { get; set; } = new List<string>();
        public string FallbackUrl { get; set; } = String.Empty;
        public string ZipCode { get; set; }
        public DateTime CreatedOn {
            get { return DateTime.UtcNow; } 
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }

        public bool IsValid() {
            return ContractIds != null 
            && ContractIds.Count > 1
            && !string.IsNullOrEmpty(FirstName)
            && !string.IsNullOrEmpty(LastName);
        }

    }
}
