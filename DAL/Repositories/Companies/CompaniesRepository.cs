using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Companies;
using ExplorJobAPI.DAL.Mappers.Companies;
using ExplorJobAPI.Domain.Commands.Companies;
using ExplorJobAPI.Domain.Dto.Companies;
using ExplorJobAPI.Domain.Repositories.Companies;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.Companies
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly CompanyMapper _companyMapper;
        
        public CompaniesRepository(
            ExplorJobDbContext explorJobDbContext,
            CompanyMapper companyMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _companyMapper = companyMapper;
        }

        public async Task<IEnumerable<CompanyDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .Companies
                    .AsNoTracking()
                    .Select(
                        (CompanyEntity company) => _companyMapper.EntityToDomainDto(
                            company
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<CompanyDto>();
            }
        }

        public async Task<IEnumerable<CompanyDto>> FindManyDtoByIds(
            List<string> ids
        ) {
            try {
                return await _explorJobDbContext
                    .Companies
                    .AsNoTracking()
                    .Where(
                        (CompanyEntity company) => ids.Contains(
                            company.Id.ToString()
                        )
                    )
                    .Select(
                        (CompanyEntity company) => _companyMapper.EntityToDomainDto(
                            company
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<CompanyDto>();
            }
        }

        public async Task<CompanyDto> FindOneDtoBySlug(
            string slug
        ) {
            try {
                CompanyEntity entity = await _explorJobDbContext
                    .Companies
                    .SingleOrDefaultAsync(
                        (CompanyEntity company) => company.Slug.ToLower().Equals(
                            slug.ToLower()
                        )
                    );

                return entity != null ?
                    _companyMapper.EntityToDomainDto(
                        entity
                    )
                    : null;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<CompanyDto> FindOneDtoById(
            string id
        ) {
            try {
                CompanyEntity entity = await _explorJobDbContext
                    .Companies
                    .SingleOrDefaultAsync(
                        (CompanyEntity company) => company.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _companyMapper.EntityToDomainDto(
                        entity
                    )
                    : null;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            CompanyCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                CompanyEntity entity = _companyMapper.CreateCommandToEntity(
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
                    "CompanyCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            CompanyUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     CompanyEntity entity = await context
                        .Companies
                        .FirstOrDefaultAsync(
                            (CompanyEntity company) => company.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _companyMapper.UpdateCommandToEntity(
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
                    "CompanyUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            CompanyDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    CompanyEntity entity = await context
                        .Companies
                        .FirstOrDefaultAsync(
                            (CompanyEntity company) => company.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _companyMapper.DeleteCommandToEntity(
                                command,
                                entity
                            )
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "CompanyDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
