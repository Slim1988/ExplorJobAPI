using System;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.Domain.Commands.Admin;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Models.Admin;
using ExplorJobAPI.Domain.Queries.Admin;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Domain.Repositories.Jobs;
using ExplorJobAPI.Domain.Services.Admin;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IContractUserAcceptancesRepository _contractUserAcceptancesRepository;
        private readonly IJobUsersRepository _jobUsersRepository;

        public AdminService(
            ExplorJobDbContext explorJobDbContext,
            UserManager<UserEntity> userManager,
            IContractUserAcceptancesRepository contractUserAcceptancesRepository,
            IJobUsersRepository jobUsersRepository
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userManager = userManager;
            _contractUserAcceptancesRepository = contractUserAcceptancesRepository;
            _jobUsersRepository = jobUsersRepository;
        }

        public AuthTokenAdmin GetAdminAuthToken(
            AdminAuthTokenQuery query
        ) {
            if (Config.AdminSecretKey.Equals(
                query.SecretKey.Sha256()
            )) {
                var token = new AuthTokenAdmin();
                Log.Information($"Admin Token generated at { DateTime.Now }");

                return token;
            }
            
            return null;
        }

        public async Task<bool> AddUser(
            AddUserCommand command
        ) {
            UserEntity user = new UserEntity {
                UserName = command.Email,
                Email = command.Email,
                EmailConfirmed = command.EmailConfirmed,
                FirstName = command.FirstName,
                LastName = command.LastName,
                IsProfessional = command.IsProfessional,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };

            IdentityResult result = await _userManager.CreateAsync(
                user,
                command.Password
            );

            if (result.Succeeded) {
                command.ContractIds.Select(
                    async (contractId) => await _contractUserAcceptancesRepository.Create(
                        new ContractUserAcceptanceCreateCommand {
                            ContractId = contractId,
                            UserId = user.Guid.ToString()
                        }
                    )
                );

                return true;
            }
            else {
                return false;
            }
        }

        public async Task<bool> AddJobUser(
            AddJobUserCommand command
        ) {
            try {
                UserEntity user = await _explorJobDbContext
                    .Users
                    .SingleOrDefaultAsync(
                        (UserEntity user) => user.Email.ToLower().Equals(
                            command.Email.ToLower()
                        )
                    );

                if (user == null) {
                    return false;
                }

                JobUserEntity jobUser = await _explorJobDbContext
                    .JobUsers
                    .SingleOrDefaultAsync(
                        (JobUserEntity jobUser) => jobUser.UserId.Equals(
                            user.Guid
                        ) && jobUser.Label.ToLower().Equals(
                            command.Label.ToLower()
                        )
                    );

                if (jobUser != null) {
                    return false;
                }

                var response = await _jobUsersRepository.Create(
                    new JobUserCreateCommand {
                        DomainIds = command.DomainIds,
                        Label = command.Label,
                        Company = command.Company,
                        Presentation = command.Presentation,
                        UserId = user.Guid.ToString()
                    }
                );

                return response.IsSuccess;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return false;
            }
        }
    }
}
