using System;

namespace ExplorJobAPI.Domain.Commands.UserMeetings
{
    public class UserMeetingCreateCommand
    {
        public string InstigatorId { get; set; }
        public string UserId { get; set; }
        public bool Met { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
