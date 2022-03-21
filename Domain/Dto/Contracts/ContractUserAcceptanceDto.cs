using System;

namespace ExplorJobAPI.Domain.Dto.Contracts
{
    public class ContractUserAcceptanceDto
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public string UserId { get; set; }
        public DateTime AcceptedOn { get; set; }

        public ContractUserAcceptanceDto(
            string id,
            string contractId,
            string userId,
            DateTime acceptedOn
        ) {
            Id = id;
            ContractId = contractId;
            UserId = UserId;
            AcceptedOn = acceptedOn;
        }
    }
}
