using System;
using ExplorJobAPI.Domain.Dto.UserReporting;

namespace ExplorJobAPI.Domain.Models.UserReporting
{
    public class UserReported
    {
        public Guid Id { get; set; }
        public Guid ReporterId { get; set; }
        public Guid ReportedId { get; set; }
        public string ReportReason { get; set; }
        public DateTime CreatedOn { get; set; }

        public UserReported(
            Guid id,
            Guid reporterId,
            Guid reportedId,
            string reportReason,
            DateTime createdOn
        ) {
            Id = id;
            ReporterId = reporterId;
            ReportedId = reportedId;
            ReportReason = reportReason;
            CreatedOn = createdOn;
        }

        public UserReportedDto ToDto() {
            return new UserReportedDto(
                Id.ToString(),
                ReporterId.ToString(),
                ReportedId.ToString(),
                ReportReason,
                CreatedOn
            );
        }
    }
}
