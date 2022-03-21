using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Jobs
{
    public class JobUserUpdateCommand
    {
        public string Id { get; set; }
        public List<string> DomainIds { get; set; }
        public string Label { get; set; }
        public string Company { get; set; }
        public string Presentation { get; set; }
        public string UserId { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }

        public bool IsValid() {
            return DomainIds.Count > 0;
        }
    }
}
