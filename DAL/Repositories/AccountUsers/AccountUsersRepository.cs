using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.UserFavorites;
using ExplorJobAPI.DAL.Entities.UserMeetings;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.DAL.Mappers.AccountUsers;
using ExplorJobAPI.Domain.Commands.AccountUsers;
using ExplorJobAPI.Domain.Models.AccountUsers;
using ExplorJobAPI.Domain.Repositories.AccountUsers;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.AccountUsers
{
    public class AccountUsersRepository : IAccountUsersRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly AccountUserMapper _accountUserMapper;
        private readonly IUsersRepository _usersRepository;

        public AccountUsersRepository(
            ExplorJobDbContext explorJobDbContext,
            AccountUserMapper accountUserMapper,
            IUsersRepository usersRepository
        ) {
            _explorJobDbContext = explorJobDbContext;
            _accountUserMapper = accountUserMapper;
            _usersRepository = usersRepository;  
        }

        public async Task<AccountUser> Get(
            string userId
        ) {
            var user = await FindUserById(
                userId
            );

            var jobs = await FindJobsByOwnerId(
                userId
            );

            var favorites = await FindFavoritesByOwnerId(
                userId
            );

            var conversations = await FindConversationsByOwnerId(
                userId
            );

            var meetings = await FindMeetingsByInstigatorId(
                userId
            );
   
            return _accountUserMapper.ToAccountUser(
                user,
                jobs,
                favorites,
                conversations,
                meetings                
            );
        }

        public async Task<RepositoryCommandResponse> IsProfessional(
            AccountUserIsProfessionalCommand command
        ) {
            return await _usersRepository.IsProfessional(
                command
            );
        }

        public async Task<RepositoryCommandResponse> UpdateGeneralInformations(
            AccountUserGeneralInformationsUpdateCommand command
        ) {
            return await _usersRepository.UpdateGeneralInformations(
                command
            );
        }

        public async Task<RepositoryCommandResponse> UpdateContactInformations(
            AccountUserContactInformationsUpdateCommand command
        ) {
            return await _usersRepository.UpdateContactInformations(
                command
            );
        }

        public async Task<RepositoryCommandResponse> UpdateSituationInformations(
            AccountUserSituationInformationsUpdateCommand command
        ) {
            return await _usersRepository.UpdateSituationInformations(
                command
            );
        }

        public async Task<RepositoryCommandResponse> Delete(
            AccountUserDeleteCommand command
        ) {
            return await _usersRepository.Delete(
                command
            );
        }

        private async Task<UserEntity> FindUserById(
            string userId
        ) {
            try {
                UserEntity entity = await _explorJobDbContext
                    .Users
                    .SingleOrDefaultAsync(
                        (UserEntity user) => user.Guid.Equals(
                            new Guid(userId)
                        )
                    );

                if (entity == null) {
                    return null;
                }

                var userEntry = _explorJobDbContext.Entry(entity);

                userEntry.Reference(
                    (UserEntity user) => user.LastDegree
                ).Load();

                userEntry.Reference(
                    (UserEntity user) => user.ProfessionalSituation
                ).Load();

                userEntry.Collection(
                    (UserEntity user) => user.ContactMethodJoins
                ).Query().Include(
                    "UserContactMethod"
                ).Load();

                userEntry.Collection(
                    (UserEntity user) => user.Favorites
                ).Load();

                return userEntry.Entity;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        private async Task<List<JobUserEntity>> FindJobsByOwnerId(
            string ownerId
        ) {
            try {
                return await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Include(
                        (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .Where(
                        (JobUserEntity jobUser) => jobUser.UserId.Equals(
                            new Guid(ownerId)
                        )
                    )
                    .OrderByDescending(
                        (JobUserEntity jobUser) => jobUser.UpdatedOn
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobUserEntity>();
            }
        }

        private async Task<List<UserFavoriteEntity>> FindFavoritesByOwnerId(
            string ownerId
        ) {
            try {
                return await _explorJobDbContext
                    .UserFavorites
                    .AsNoTracking()
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.User
                    )
                    .ThenInclude(
                        (UserEntity user) => user.LastDegree
                    )
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.User
                    )
                    .ThenInclude(
                        (UserEntity user) => user.ProfessionalSituation
                    )
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.User
                    )
                    .ThenInclude(
                        (UserEntity user) => user.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.User
                    )
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.JobUser
                    )
                    .ThenInclude(
                        (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .Where(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.OwnerId.Equals(
                            new Guid(ownerId)
                        )
                    )
                    .OrderByDescending(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.CreatedOn
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserFavoriteEntity>();
            }
        }
 
        private async Task<List<ConversationEntity>> FindConversationsByOwnerId(
            string ownerId
        ) {
            try {
                return await _explorJobDbContext
                    .Conversations
                    .AsNoTracking()
                    .Include(
                        (ConversationEntity conversationEntity) => conversationEntity.Interlocutor
                    )
                    .ThenInclude(
                        (UserEntity interlocutor) => interlocutor.LastDegree
                    )
                    .Include(
                        (ConversationEntity conversationEntity) => conversationEntity.Interlocutor
                    )
                    .ThenInclude(
                        (UserEntity interlocutor) => interlocutor.ProfessionalSituation
                    )
                    .Include(
                        (ConversationEntity conversationEntity) => conversationEntity.Interlocutor
                    )
                    .ThenInclude(
                        (UserEntity interlocutor) => interlocutor.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .Include(
                        (ConversationEntity conversationEntity) => conversationEntity.Interlocutor
                    )
                    .Include(
                        (ConversationEntity conversationEntity) => conversationEntity.Messages
                    )
                    .Include(
                        (ConversationEntity conversationEntity) => conversationEntity.Proposals
                    )
                    .ThenInclude(
                       (MessageProposalEntity messageProposalEntity) => messageProposalEntity.ProposalAppointments
                    )
                    .Where(
                        (ConversationEntity conversationEntity) =>
                         conversationEntity.OwnerId.Equals(
                            new Guid(ownerId)
                        )
                    )
                    .OrderByDescending(
                        (ConversationEntity conversationEntity) => conversationEntity.CreatedOn
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<ConversationEntity>();
            }
        }

        private async Task<List<UserMeetingEntity>> FindMeetingsByInstigatorId(
            string instigatorId
        ) {
            try {
                return await _explorJobDbContext
                    .UserMeetings
                    .AsNoTracking()
                    .Where(
                        (UserMeetingEntity userMeetingEntity) => userMeetingEntity.InstigatorId.Equals(
                            new Guid(instigatorId)
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserMeetingEntity>();
            }
        }
    }
}
