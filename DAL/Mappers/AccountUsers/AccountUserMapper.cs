using System.Linq;
using System.Collections.Generic;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.UserFavorites;
using ExplorJobAPI.DAL.Entities.UserMeetings;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.DAL.Mappers.Jobs;
using ExplorJobAPI.DAL.Mappers.Users;
using ExplorJobAPI.Domain.Models.AccountUsers;
using System;
using ExplorJobAPI.DAL.Entities.Appointment;

namespace ExplorJobAPI.DAL.Mappers.AccountUsers
{
    public class AccountUserMapper
    {
        private readonly UserMapper _userMapper;
        private readonly JobUserMapper _jobUserMapper;

        public AccountUserMapper(
            UserMapper userMapper,
            JobUserMapper jobUserMapper
        )
        {
            _userMapper = userMapper;
            _jobUserMapper = jobUserMapper;
        }

        public AccountUser ToAccountUser(
            UserEntity userEntity,
            List<JobUserEntity> jobs,
            List<UserFavoriteEntity> favorites,
            List<ConversationEntity> conversations,
            List<UserMeetingEntity> meetings
        )
        {
            var user = _userMapper.EntityToDomainModel(
                userEntity
            );

            return new AccountUser(
                user.Id.ToString(),
                user.PhotoUrl(),
                user.Email,
                user.Phone,
                user.Address,
                user.SkypeId,
                user.ToDto(),
                jobs.Select(
                    _jobUserMapper.EntityToDomainDto
                ).ToList(),
                favorites.Select(
                    (UserFavoriteEntity favorite) => ToAccountUserFavorite(
                        favorite,
                        FindMeetingInListByUserId(
                            meetings,
                            favorite.UserId
                        )
                    )
                ).ToList(),
                conversations.Select(
                    (ConversationEntity conversation) => ToAccountUserConversation(
                        conversation,
                        FindMeetingInListByUserId(
                            meetings,
                            conversation.InterlocutorId
                        )
                    )
                ).ToList(),
                user.HasAlreadyBeenUpdated()
            );
        }

        private AccountUserFavorite ToAccountUserFavorite(
            UserFavoriteEntity userFavoriteEntity,
            UserMeetingEntity userMeetingEntity
        )
        {
            return new AccountUserFavorite(
                userFavoriteEntity.Id.ToString(),
                userFavoriteEntity.OwnerId.ToString(),
                userFavoriteEntity.UserId.ToString(),
                _userMapper.EntityToDomainRestrictedDto(
                    userFavoriteEntity.User
                ),
                userFavoriteEntity.JobUserId != null
                    ? userFavoriteEntity.JobUserId.ToString()
                    : null,
                userFavoriteEntity.JobUser != null
                    ? _jobUserMapper.EntityToDomainDto(
                        userFavoriteEntity.JobUser
                    )
                    : null,
                userMeetingEntity != null
            );
        }

        private AccountUserConversation ToAccountUserConversation(
            ConversationEntity conversationEntity,
            UserMeetingEntity userMeetingEntity
        )
        {
            return new AccountUserConversation(
                conversationEntity.Id.ToString(),
                conversationEntity.OwnerId.ToString(),
                conversationEntity.InterlocutorId.ToString(),
                _userMapper.EntityToDomainRestrictedDto(
                    conversationEntity.Interlocutor
                ),
                userMeetingEntity != null,
                conversationEntity.Messages.Select(
                    (MessageEntity messageEntity) => ToAccountUserMessage(
                        messageEntity,
                        conversationEntity
                    )
                ).ToList(),
                conversationEntity.Proposals.Select(
                    (MessageProposalEntity messageProposalEntity) => ToAccountUserMessageProposal(
                        messageProposalEntity,
                        conversationEntity
                    )
                ).ToList(),
                conversationEntity.UpdatedOn != null
                    ? conversationEntity.UpdatedOn
                    : conversationEntity.CreatedOn,
               conversationEntity.Display
            );
        }

        private AccountUserMessage ToAccountUserMessage(
            MessageEntity messageEntity,
            ConversationEntity conversation
        )
        {
            return new AccountUserMessage(
                messageEntity.Id.ToString(),
                messageEntity.EmitterId.ToString(),
                messageEntity.EmitterId.Equals(
                    conversation.OwnerId
                ),
                messageEntity.EmitterId.Equals(
                    conversation.InterlocutorId
                ),
                messageEntity.Content,
                messageEntity.Read,
                messageEntity.CreatedOn
            );
        }
        private AccountUserMessageProposal ToAccountUserMessageProposal(
            MessageProposalEntity messageProposalEntity,
            ConversationEntity conversation
        )
        {
            return new AccountUserMessageProposal(
                messageProposalEntity.Id.ToString(),
                messageProposalEntity.EmitterId.Equals(
                    conversation.OwnerId
                ),
                messageProposalEntity.EmitterId.Equals(
                    conversation.InterlocutorId
                ),
                messageProposalEntity.Content,
                messageProposalEntity.Read,
                messageProposalEntity.CreatedOn,
                messageProposalEntity.ProposalAppointments.Where(pa=>pa.DateTime>= DateTime.Now || pa.ProposalStaus != ProposalStatus.Pending).ToList(),
                messageProposalEntity.CommonId
            );
        }

        private UserMeetingEntity FindMeetingInListByUserId(
            List<UserMeetingEntity> meetings,
            Guid userId
        )
        {
            return meetings.Find(
                (meeting) => meeting.UserId.Equals(
                    userId
                )
            );
        }
    }
}
