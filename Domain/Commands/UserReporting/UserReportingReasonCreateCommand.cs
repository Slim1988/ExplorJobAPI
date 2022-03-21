using System;

namespace ExplorJobAPI.Domain.Commands.UserReporting
{
    public class UserReportingReasonCreateCommand
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
