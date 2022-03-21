using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Dto.Jobs
{
    public class JobUserDto
    {
        public string Id { get; set; }
        public List<JobDomainDto> Domains { get; set; }
        public string Label { get; set; }
        public string Company { get; set; }
        public string Presentation { get; set; }
        public string UserId { get; set; }

        public JobUserDto(
            string id,
            List<JobDomainDto> domains,
            string label,
            string company,
            string presentation,
            string userId
        ) {
            Id = id;
            Domains = domains;
            Label = label;
            Company = company;
            Presentation = presentation;
            UserId = userId;
        }
    }
}
