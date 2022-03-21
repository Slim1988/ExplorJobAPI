using System;
using ExplorJobAPI.Domain.Dto.Contracts;

namespace ExplorJobAPI.Domain.Models.Contracts
{
    public class ContractUserAcceptance
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid UserId { get; set; }
        public DateTime AcceptedOn { get; set; }

        public ContractUserAcceptance(
            Guid id,
            Guid contractId,
            Guid userId,
            DateTime acceptedOn
        ) {
            Id = id;
            ContractId = contractId;
            UserId = userId;
            AcceptedOn = acceptedOn;
        }

        public ContractUserAcceptanceDto ToDto() {
            return new ContractUserAcceptanceDto(
                Id.ToString(),
                ContractId.ToString(),
                UserId.ToString(),
                AcceptedOn
            );
        }
    }
}
