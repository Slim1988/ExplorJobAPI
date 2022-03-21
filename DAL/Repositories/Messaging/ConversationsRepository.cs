using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Mappers.Messaging;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Dto.Messaging;
using ExplorJobAPI.Domain.Models.Messaging;
using ExplorJobAPI.Domain.Repositories.Messaging;
using ExplorJobAPI.Infrastructure.Repositories;
using Serilog;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.Domain.Services.Messaging;

namespace ExplorJobAPI.DAL.Repositories.Messaging
{
    public class ConversationsRepository : IConversationsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly IMessagesRepository _messagesRepository;
        private readonly ConversationMapper _conversationMapper;

        public ConversationsRepository(
            ExplorJobDbContext explorJobDbContext,
            IMessagesRepository messagesRepository,
            ConversationMapper conversationMapper

        )
        {
            _explorJobDbContext = explorJobDbContext;
            _messagesRepository = messagesRepository;
            _conversationMapper = conversationMapper;
        }

        public async Task<IEnumerable<ConversationDto>> FindAllDtoByOwnerId(
            string ownerId
        )
        {
            try
            {
                return await _explorJobDbContext
                    .Conversations
                    .AsNoTracking()
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
                        (ConversationEntity conversationEntity) => conversationEntity.Display
                            && conversationEntity.OwnerId.Equals(
                                new Guid(ownerId)
                            )
                    )
                    .OrderByDescending(
                        (ConversationEntity conversationEntity) => conversationEntity.CreatedOn
                    )
                    .Select(
                        (ConversationEntity conversationEntity) => _conversationMapper.EntityToDomainDto(
                            conversationEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return new List<ConversationDto>();
            }
        }

        public async Task<Conversation> FindOneByOwnerAndInterlocutorId(
            string ownerId,
            string interlocutorId
        )
        {
            try
            {
                ConversationEntity entity = await _explorJobDbContext
                    .Conversations
                    .SingleOrDefaultAsync(
                        (ConversationEntity conversation) => conversation.OwnerId.Equals(
                            new Guid(ownerId)
                        ) && conversation.InterlocutorId.Equals(
                            new Guid(interlocutorId)
                        )
                    );

                if (entity == null)
                {
                    return null;
                }

                _explorJobDbContext
                    .Entry(entity)
                    .Collection(conversation => conversation.Messages)
                    .Load();

                return _conversationMapper.EntityToDomainModel(
                    entity
                );
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<Conversation> FindOneById(
            string id
        )
        {
            try
            {
                ConversationEntity entity = await _explorJobDbContext
                    .Conversations
                    .SingleOrDefaultAsync(
                        (ConversationEntity conversation) => conversation.Id.Equals(
                            new Guid(id)
                        )
                    );

                if (entity == null)
                {
                    return null;
                }

                _explorJobDbContext
                    .Entry(entity)
                    .Collection(conversation => conversation.Messages)
                    .Load();

                return _conversationMapper.EntityToDomainModel(
                    entity
                );
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<RepositoryCommandResponse> MarkAsRead(
            ConversationMarkAsReadCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    ConversationEntity entity = await _explorJobDbContext
                        .Conversations
                        .SingleOrDefaultAsync(
                            (ConversationEntity conversation) => conversation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        _explorJobDbContext
                            .Entry(entity)
                            .Collection(conversation => conversation.Messages)
                            .Load();
                        _explorJobDbContext
                            .Entry(entity)
                            .Collection(conversation => conversation.Proposals)
                            .Load();
                        List<MessageEntity> unreadMessages = entity.Messages.Where(
                            (MessageEntity message) => !message.Read
                        ).ToList();
                        List<MessageProposalEntity> unreadProposals = entity.Proposals.Where(
                           (MessageProposalEntity message) => !message.Read
                       ).ToList();
                        unreadMessages.Select(
                            async (MessageEntity message) => await _messagesRepository.MarkAsRead(
                                new MessageMarkAsReadCommand()
                                {
                                    Id = message.Id.ToString(),
                                    OwnerId = command.OwnerId
                                }
                            )
                        ).ToList();
                        unreadProposals.Select(
                            async (MessageProposalEntity message) => await _messagesRepository.MarkProposalAsRead(
                                new MessageMarkAsReadCommand()
                                {
                                    Id = message.Id.ToString(),
                                    OwnerId = command.OwnerId
                                }
                            )
                        ).ToList();
                        numberOfChanges = 2;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ConversationMarkAsRead",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> MarkAsUnread(
            ConversationMarkAsUnreadCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    ConversationEntity entity = await _explorJobDbContext
                        .Conversations
                        .SingleOrDefaultAsync(
                            (ConversationEntity conversation) => conversation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        _explorJobDbContext
                            .Entry(entity)
                            .Collection(conversation => conversation.Messages)
                            .Load();
                        _explorJobDbContext
                           .Entry(entity)
                           .Collection(conversation => conversation.Proposals)
                           .Load();
                        List<MessageEntity> readMessages = entity.Messages.Where(
                            (MessageEntity message) => message.Read
                        ).ToList();
                        List<MessageProposalEntity> readProposalMessages = entity.Proposals.Where(
                            (MessageProposalEntity message) => message.Read
                        ).ToList();
                        readMessages.Select(
                            async (MessageEntity message) => await _messagesRepository.MarkAsUnread(
                                new MessageMarkAsUnreadCommand()
                                {
                                    Id = message.Id.ToString(),
                                    OwnerId = command.OwnerId
                                }
                            )
                        ).ToList();
                        readProposalMessages.Select(
                            async (MessageProposalEntity message) => await _messagesRepository.MarkProposalAsUnread(
                                new MessageMarkAsUnreadCommand()
                                {
                                    Id = message.Id.ToString(),
                                    OwnerId = command.OwnerId
                                }
                            )
                        ).ToList();
                        numberOfChanges = 2;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ConversationMarkAsUnread",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> NewMessage(
            ConversationNewMessageCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    ConversationEntity entity = await context
                        .Conversations
                        .FirstOrDefaultAsync(
                            (ConversationEntity conversation) => conversation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        if (!entity.Display)
                            entity.Display = true;
                        entity = _conversationMapper.NewMessageCommandToEntity(
                            command,
                            entity
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ConversationNewMessage",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            ConversationCreateCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                ConversationEntity entity = _conversationMapper.CreateCommandToEntity(
                    command
                );

                try
                {
                    context.Add(entity);
                    numberOfChanges = await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ConversationCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            ConversationDeleteCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {

                    ConversationEntity entity = await context
                       .Conversations
                       .FirstOrDefaultAsync(
                           (ConversationEntity conversation) => conversation.Id.Equals(
                               new Guid(command.Id)
                           )
                       );
                    entity.Messages.Clear();

                    ConversationEntity interlocutorConversation = await context
                       .Conversations
                       .FirstOrDefaultAsync(
                           (ConversationEntity conversation) => conversation.InterlocutorId.Equals(
                               entity.OwnerId
                           )
                           &&
                           conversation.OwnerId.Equals(
                               entity.InterlocutorId
                           )
                       );
                    await UpdateProposals(new ConversationNewMessageCommand(id: command.Id), ProposalStatus.Declined, true);
                    await UpdateProposals(new ConversationNewMessageCommand(id: interlocutorConversation.Id.ToString()), ProposalStatus.Declined, true);
                    string message;

                    message = $"Votre interlocuteur a effac� tous les rendez-vous";


                    MessageCreateCommand messageCommandEmitter = new MessageCreateCommand()
                    {
                        Content = message,
                        ConversationId = interlocutorConversation.Id.ToString(),
                        EmitterId = entity.OwnerId.ToString(),
                        ReceiverId = entity.InterlocutorId.ToString(),
                        Read = false
                    };
                    await _messagesRepository.Create(messageCommandEmitter);
                    if (entity != null)
                    {
                        context.Update(
                            _conversationMapper.DeleteCommandToEntity(
                                command,
                                entity
                            )
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }

                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "ConversationDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> UpdateProposals(
            ConversationNewMessageCommand command,
            ProposalStatus proposalStatus,
            bool isForDelete = false
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    ConversationEntity entity = await context
                       .Conversations
                       .Include(prop => prop.Proposals)
                       .ThenInclude(pa => pa.ProposalAppointments)
                       .FirstOrDefaultAsync(
                           (ConversationEntity conversation) => conversation.Id.Equals(
                               new Guid(command.Id)
                           )
                       );

                    if (entity != null)
                    {
                        if (!entity.Display)
                            entity.Display = true;
                        if (!isForDelete)
                        {
                            entity.Proposals.ForEach(proposal => proposal.ProposalAppointments.ForEach(pa => { if (pa.ProposalStaus != ProposalStatus.Approuved) { pa.ProposalStaus = proposalStatus; } }));
                        }
                        else{
                            entity.Proposals.ForEach(proposal => proposal.ProposalAppointments.ForEach(pa => { if (pa.ProposalStaus != ProposalStatus.Approuved || pa.DateTime > DateTime.Now) { pa.ProposalStaus = proposalStatus; } }));
                        }
                        
                        context.Update(entity);
                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "Proposals Updated",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
