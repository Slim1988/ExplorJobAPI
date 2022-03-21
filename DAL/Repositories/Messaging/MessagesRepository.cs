using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Mappers.Messaging;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Repositories.Messaging;
using ExplorJobAPI.Infrastructure.Repositories;
using Serilog;
using ExplorJobAPI.DAL.Entities.Appointment;
using System.Collections.Generic;
using System.Linq;
using ExplorJobAPI.DAL.Entities.Messaging.Appointment;

namespace ExplorJobAPI.DAL.Repositories.Messaging
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly MessageMapper _messageMapper;

        public MessagesRepository(
            ExplorJobDbContext explorJobDbContext,
            MessageMapper messageMapper
        )
        {
            _explorJobDbContext = explorJobDbContext;
            _messageMapper = messageMapper;
        }
        public async Task<ProposalAppointmentEntity> GetMessageProposalById(
            Guid id
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                try
                {

                    MessageProposalEntity entity = await context
                        .Appointments
                        .Include(p => p.ProposalAppointments).FirstOrDefaultAsync((MessageProposalEntity messageProposal) => messageProposal.ProposalAppointments.Any(pa => pa.Id.Equals(
                                  id
                              )));
                    return entity.ProposalAppointments.FirstOrDefault(pa => pa.Id == id);

                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
                return null;
            }
        }
        public async Task<RepositoryCommandResponse> MessageProposalUpdate(
            SendMessageProposalAcceptanceCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {

                    List<MessageProposalEntity> entities = await context
                        .Appointments
                        .Include(p => p.ProposalAppointments).Where((MessageProposalEntity messageProposal) => messageProposal.CommonId.Equals(
                                  new Guid(command.Id)
                              ))
                        .ToListAsync();

                    if (entities.Any())
                    {
                        ProposalAppointmentEntity proposal = entities.First(x => x.ProposalAppointments.FirstOrDefault(
                            pa => pa.Id == command.ProposalId
                            ) != null).ProposalAppointments.FirstOrDefault(
                            pa => pa.Id == command.ProposalId
                            );
                        entities.ForEach(entity => entity = _messageMapper.UpdateMessageProposal(
                                                        command,
                                                        proposal.DateTime,
                                                        entity
                                                        ));
                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    $"Proposal updated with startus: {command.ProposalStatus}",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> MarkAsRead(
            MessageMarkAsReadCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    MessageEntity entity = await context
                        .Messages
                        .FirstOrDefaultAsync(
                            (MessageEntity message) => message.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        entity = _messageMapper.MarkAsReadCommandToEntity(
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
                    "MessageMarkAsRead",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> MarkProposalAsRead(
            MessageMarkAsReadCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    MessageProposalEntity entity = await context
                        .Appointments
                        .FirstOrDefaultAsync(
                            (MessageProposalEntity message) => message.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        entity = _messageMapper.MarkProposalAsReadCommandToEntity(
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
                    "MessageMarkAsRead",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> MarkAsUnread(
            MessageMarkAsUnreadCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    MessageEntity entity = await context
                        .Messages
                        .FirstOrDefaultAsync(
                            (MessageEntity message) => message.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        entity = _messageMapper.MarkAsUnreadCommandToEntity(
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
                    "MessageMarkAsUnread",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> MarkProposalAsUnread(
            MessageMarkAsUnreadCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                try
                {
                    MessageProposalEntity entity = await context
                        .Appointments
                        .FirstOrDefaultAsync(
                            (MessageProposalEntity message) => message.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null)
                    {
                        entity = _messageMapper.MarkProposalAsUnreadCommandToEntity(
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
                    "MessageMarkAsUnread",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> Create(
            SendReviewCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;
                ProposalAppointmentEntity proposal = await GetMessageProposalById(command.ProposalId);
                if (proposal != null)
                {
                    proposal.Reviewed = true;
                    ReviewEntity entity = _messageMapper.CreateReviewCommandToEntity(
                                        command
                                    );

                    try
                    {
                        context.Update(proposal);
                        context.Add(entity);
                        numberOfChanges = await context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        numberOfChanges = 0;
                    }

                    return new RepositoryCommandResponse(
                        "MessageCreate",
                        isSuccess: numberOfChanges > 0,
                        id: entity.Id.ToString()
                    );
                }
                return new RepositoryCommandResponse(
                    "MessageCreate",
                    isSuccess: numberOfChanges > 0,
                    id: null
                );
            }

        }
        public async Task<RepositoryCommandResponse> Create(
            MessageCreateCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                MessageEntity entity = _messageMapper.CreateCommandToEntity(
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
                    "MessageCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }
        public async Task<RepositoryCommandResponse> Create(
            MessageProposalCreateCommand command
        )
        {
            using (var context = ExplorJobDbContext.NewContext())
            {
                int numberOfChanges = 0;

                MessageProposalEntity entity = _messageMapper.CreateProposalCommandToEntity(
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
                    "MessageCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

    }
}
