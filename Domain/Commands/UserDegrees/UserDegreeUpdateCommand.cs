using System;

namespace ExplorJobAPI.Domain.Commands.UserDegrees
{
    public class UserDegreeUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
