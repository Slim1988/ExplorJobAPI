using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Contracts;
using ExplorJobAPI.DAL.Mappers.Contracts;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.Contracts
{
    public class ContractUserAcceptancesRepository : IContractUserAcceptancesRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly ContractUserAcceptanceMapper _contractUserAcceptanceMapper;

        public ContractUserAcceptancesRepository(
            ExplorJobDbContext explorJobDbContext,
            ContractUserAcceptanceMapper contractUserAcceptanceMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _contractUserAcceptanceMapper = contractUserAcceptanceMapper;  
        }

        public async Task<IEnumerable<ContractUserAcceptanceDto>> FindManyByContractId(
            string contractId
        ) {
            try {
                return await _explorJobDbContext
                    .ContractUserAcceptances
                    .AsNoTracking()
                    .Where(
                        (ContractUserAcceptanceEntity contractUserAcceptance) => contractUserAcceptance.ContractId.ToString().Equals(
                            contractId
                        )
                    )
                    .OrderByDescending(
                        (ContractUserAcceptanceEntity contractUserAcceptance) => contractUserAcceptance.AcceptedOn
                    )
                    .Select(
                        (ContractUserAcceptanceEntity contractUserAcceptance) => _contractUserAcceptanceMapper.EntityToDomainDto(
                            contractUserAcceptance
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<ContractUserAcceptanceDto>();
            }
        }

        public async Task<IEnumerable<ContractUserAcceptanceDto>> FindManyByUserId(
            string userId
        ) {
            try {
                return await _explorJobDbContext
                    .ContractUserAcceptances
                    .AsNoTracking()
                    .Where(
                        (ContractUserAcceptanceEntity contractUserAcceptance) => contractUserAcceptance.UserId.ToString().Equals(
                            userId
                        )
                    )
                    .OrderByDescending(
                        (ContractUserAcceptanceEntity contractUserAcceptance) => contractUserAcceptance.AcceptedOn
                    )
                    .Select(
                        (ContractUserAcceptanceEntity contractUserAcceptance) => _contractUserAcceptanceMapper.EntityToDomainDto(
                            contractUserAcceptance
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<ContractUserAcceptanceDto>();
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            ContractUserAcceptanceCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                ContractUserAcceptanceEntity entity = _contractUserAcceptanceMapper.CreateCommandToEntity(
                    command
                );

                try {
                    context.Add(entity);
                    numberOfChanges = await context.SaveChangesAsync();
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ContractUserAcceptanceCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }
    }
}
