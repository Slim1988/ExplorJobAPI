using System;

namespace ExplorJobAPI.Domain.Commands.UserReporting
{
    public class UserReportingReasonUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
