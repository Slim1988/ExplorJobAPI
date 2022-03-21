using System;

namespace ExplorJobAPI.Domain.Commands.Contracts
{
    public class ContractUserAcceptanceCreateCommand
    {
        public string ContractId { get; set; }
        public string UserId { get; set; }

        public DateTime AcceptedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
