using System;

namespace ExplorJobAPI.Domain.Commands.UserContactMethods
{
    public class UserContactMethodUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
