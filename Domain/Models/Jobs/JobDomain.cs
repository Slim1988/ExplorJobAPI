using System;
using ExplorJobAPI.Domain.Dto.Jobs;

namespace ExplorJobAPI.Domain.Models.Jobs
{
    public class JobDomain
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public JobDomain(
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

        public JobDomainDto ToDto() {
            return new JobDomainDto(
                Id.ToString(),
                Label
            );
        }
    }
}
