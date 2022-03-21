using System;

namespace ExplorJobAPI.Domain.Commands.Contracts
{
    public class ContractCreateCommand
    {
        public string Context { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Content { get; set; }
        public string ContentHTML { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; } 
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
