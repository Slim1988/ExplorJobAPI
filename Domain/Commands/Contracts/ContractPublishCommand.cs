using System;

namespace ExplorJobAPI.Domain.Commands.Contracts
{
    public class ContractPublishCommand
    {
        public string Id { get; set; }

        public DateTime PublishedOn {
            get { return DateTime.UtcNow; } 
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
