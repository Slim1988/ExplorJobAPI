using System;

namespace ExplorJobAPI.Domain.Commands.UserReporting
{
    public class UserReportedCreateCommand
    {
        public string ReporterId { get; set; }
        public string ReportedId { get; set; }
        public string ReportReason { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
