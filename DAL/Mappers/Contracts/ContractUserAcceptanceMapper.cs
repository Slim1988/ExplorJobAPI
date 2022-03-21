using System;
using ExplorJobAPI.DAL.Entities.Contracts;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Domain.Models.Contracts;

namespace ExplorJobAPI.DAL.Mappers.Contracts
{
    public class ContractUserAcceptanceMapper
    {
        public ContractUserAcceptance EntityToDomainModel(
            ContractUserAcceptanceEntity entity
        ) {
            return new ContractUserAcceptance(
                entity.Id,
                entity.ContractId,
                entity.UserId,
                entity.AcceptedOn
            );
        }

        public ContractUserAcceptanceDto EntityToDomainDto(
            ContractUserAcceptanceEntity entity
        ) {
            return new ContractUserAcceptanceDto(
                entity.Id.ToString(),
                entity.ContractId.ToString(),
                entity.UserId.ToString(),
                entity.AcceptedOn
            );
        }

        public ContractUserAcceptanceEntity CreateCommandToEntity(
            ContractUserAcceptanceCreateCommand command
        ) {
            return new ContractUserAcceptanceEntity {
                ContractId = new Guid(command.ContractId),
                UserId = new Guid(command.UserId),
                AcceptedOn = command.AcceptedOn
            };
        }
    }
}
