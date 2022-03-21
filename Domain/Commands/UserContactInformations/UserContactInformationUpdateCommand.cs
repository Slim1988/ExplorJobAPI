using System;

namespace ExplorJobAPI.Domain.Commands.UserContactInformations
{
    public class UserContactInformationUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
