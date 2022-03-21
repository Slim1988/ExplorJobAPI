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
using ExplorJobAPI.Domain.Models.Contracts;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.Contracts
{
    public class ContractsRepository : IContractsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly ContractMapper _contractMapper;

        public ContractsRepository(
            ExplorJobDbContext explorJobDbContext,
            ContractMapper contractMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _contractMapper = contractMapper;  
        }

        public async Task<IEnumerable<Contract>> FindAll() {
            try {
                return await _explorJobDbContext
                    .Contracts
                    .AsNoTracking()
                    .OrderBy(
                        (ContractEntity contract) => contract.PublishedOn
                    )
                    .Select(
                        (ContractEntity contract) => _contractMapper.EntityToDomainModel(
                            contract
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<Contract>();
            }
        }

        public async Task<IEnumerable<ContractDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .Contracts
                    .AsNoTracking()
                    .OrderBy(
                        (ContractEntity contract) => contract.PublishedOn
                    )
                    .Select(
                        (ContractEntity contract) => _contractMapper.EntityToDomainDto(
                            contract
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<ContractDto>();
            }
        }

        public async Task<Contract> FindOneById(
            string id
        ) {
            try {
                ContractEntity entity = await _explorJobDbContext
                    .Contracts
                    .SingleOrDefaultAsync(
                        (ContractEntity contract) => contract.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _contractMapper.EntityToDomainModel(
                        entity
                    )
                    : null;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<RepositoryCommandResponse> Publish(
            ContractPublishCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     ContractEntity entity = await context
                        .Contracts
                        .FirstOrDefaultAsync(
                            (ContractEntity contract) => contract.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _contractMapper.PublishCommandToEntity(
                            command,
                            entity
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ContractPublish",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            ContractCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                ContractEntity entity = _contractMapper.CreateCommandToEntity(
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
                    "ContractCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            ContractUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     ContractEntity entity = await context
                        .Contracts
                        .FirstOrDefaultAsync(
                            (contract) => contract.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _contractMapper.UpdateCommandToEntity(
                            command,
                            entity
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ContractUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
