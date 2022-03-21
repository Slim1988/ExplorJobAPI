using System;
using ExplorJobAPI.Domain.Dto.UserReporting;

namespace ExplorJobAPI.Domain.Models.UserReporting
{
    public class UserReportingReason
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public UserReportingReason(
            Guid id,
            string label,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Label = label;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public UserReportingReasonDto ToDto() {
            return new UserReportingReasonDto(
                Id.ToString(),
                Label
            );
        }
    }
}
