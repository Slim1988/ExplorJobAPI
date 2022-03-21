using System.Linq;
using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.Jobs;

namespace ExplorJobAPI.Domain.Models.Jobs
{
    public class JobUser
    {
        public Guid Id { get; set; }
        public List<JobDomain> Domains { get; set; }
        public string Label { get; set; }
        public string Company { get; set; }
        public string Presentation { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public JobUser(
            Guid id,
            List<JobDomain> domains,
            string label,
            string company,
            string presentation,
            Guid userId,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Domains = domains;
            Label = label;
            Company = company;
            Presentation = presentation;
            UserId = userId;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public JobUserDto ToDto() {
            return new JobUserDto(
                Id.ToString(),
                Domains.Select(
                    (domain) => domain.ToDto()
                ).ToList(),
                Label,
                Company,
                Presentation,
                UserId.ToString()
            );
        }
    }
}
