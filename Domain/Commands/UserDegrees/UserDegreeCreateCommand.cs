using System;

namespace ExplorJobAPI.Domain.Commands.UserDegrees
{
    public class UserDegreeCreateCommand
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
