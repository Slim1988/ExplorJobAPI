using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Queries.Jobs
{
    public class JobSearchQuery
    {
        public string Query { get; set; }
        public string JobLabel { get; set; }
        public List<string> JobDomainIds { get; set; }
        public string Company { get; set; }
        public string Localisation { get; set; }
    }
}
