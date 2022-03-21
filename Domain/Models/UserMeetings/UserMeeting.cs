using System;

namespace ExplorJobAPI.Domain.Models.UserMeetings
{
    public class UserMeeting
    {
        public Guid Id { get; set; }
        public Guid InstigatorId { get; set; }
        public Guid UserId { get; set; }
        public bool Met { get; set; }
        public DateTime CreatedOn { get; set; }
       
        public UserMeeting(
            Guid id,
            Guid instigatorId,
            Guid userId,
            bool met,
            DateTime createdOn
        ) {
            Id = id;
            InstigatorId = instigatorId;
            UserId = userId;
            Met = met;
            CreatedOn = createdOn;
        }
    }
}
