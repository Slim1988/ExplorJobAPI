using System;

namespace ExplorJobAPI.Domain.Commands.Jobs
{
    public class JobDomainUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
