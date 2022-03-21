using System;

namespace ExplorJobAPI.Domain.Dto.UserReporting
{
    public class UserReportedDto
    {
        public string Id { get; set; }
        public string ReporterId { get; set; }
        public string ReportedId { get; set; }
        public string ReportReason { get; set; }
        public DateTime Date { get; set; }

        public UserReportedDto(
            string id,
            string reporterId,
            string reportedId,
            string reportReason,
            DateTime date
        ) {
            Id = id;
            ReporterId = reporterId;
            ReportedId = reportedId;
            ReportReason = reportReason;
            Date = date;
        }
    }
}
