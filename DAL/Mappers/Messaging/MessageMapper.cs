using System;
using System.Collections.Generic;
using System.Linq;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.Messaging.Appointment;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Dto.Messaging;
using ExplorJobAPI.Domain.Models.Messaging;

namespace ExplorJobAPI.DAL.Mappers.Messaging
{
    public class MessageMapper
    {
        public Message EntityToDomainModel(
            MessageEntity entity
        )
        {
            return new Message(
                entity.Id,
                entity.ConversationId,
                entity.EmitterId,
                entity.ReceiverId,
                entity.Content,
                entity.Read,
                entity.CreatedOn
            );
        }

        public MessageDto EntityToDomainDto(
            MessageEntity entity
        )
        {
            return new MessageDto(
                entity.Id.ToString(),
                entity.EmitterId.ToString(),
                entity.ReceiverId.ToString(),
                entity.Content,
                entity.Read,
                entity.CreatedOn
            );
        }
        public MessageProposalEntity UpdateMessageProposal(
            SendMessageProposalAcceptanceCommand command,
            DateTime date,
            MessageProposalEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.CommonId))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }


            if (command.ProposalStatus == ProposalStatus.Approuved)
            {
                entity.ProposalAppointments.ForEach(pa =>
                {
                    if (pa.DateTime != date)
                        pa.ProposalStaus = ProposalStatus.Declined;
                    else
                        pa.ProposalStaus = command.ProposalStatus;
                });
            }
            else
            {
                entity.ProposalAppointments.ForEach(pa =>
                {
                    if (pa.DateTime == date)
                        pa.ProposalStaus = command.ProposalStatus;
                });
            }

            return entity;
        }
        public MessageEntity MarkAsReadCommandToEntity(
            MessageMarkAsReadCommand command,
            MessageEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Read = command.Read;

            return entity;
        }
        public MessageProposalEntity MarkProposalAsReadCommandToEntity(
            MessageMarkAsReadCommand command,
            MessageProposalEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Read = command.Read;

            return entity;
        }
        public MessageEntity MarkAsUnreadCommandToEntity(
            MessageMarkAsUnreadCommand command,
            MessageEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Read = command.Read;

            return entity;
        }
        public MessageProposalEntity MarkProposalAsUnreadCommandToEntity(
            MessageMarkAsUnreadCommand command,
            MessageProposalEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Read = command.Read;

            return entity;
        }
        public MessageEntity CreateCommandToEntity(
            MessageCreateCommand command
        )
        {
            return new MessageEntity
            {
                ConversationId = new Guid(command.ConversationId),
                EmitterId = new Guid(command.EmitterId),
                ReceiverId = new Guid(command.ReceiverId),
                Content = command.Content,
                Read = command.Read,
                CreatedOn = command.CreatedOn
            };
        }
        public ReviewEntity CreateReviewCommandToEntity(
            SendReviewCommand command
        )
        {
            return new ReviewEntity
            {
                CommonId = command.CommonId,
                HasMet = command.HasMet,
                WhatWereYou = command.WhatWereYou,
                MeetingCanellationReason = command.MeetingCanellationReason,
                MeetingCanellationReasonOther = command.MeetingCanellationReasonOther,
                MeetingDuration = command.MeetingDuration,
                MeetingPlateform = command.MeetingPlateform,
                MeetingQuality = command.MeetingQuality,
                DoTheSame = command.DoTheSame,
                SameCompany = command.SameCompany,
                IsExplorerGood = command.IsExplorerGood,
                IsExplorerInterestingForCompany = command.IsExplorerInterestingForCompany,
                JobUserEntityId = command.whichJob,
                OtherComment = command.OtherComment,
                Recommendation = command.Recommendation,
                EmitterId = new Guid(command.EmitterId),
                ReceiverId = new Guid(command.ReceiverId),
                CreatedOn = DateTime.Now
            };
        }
        public MessageProposalEntity CreateProposalCommandToEntity(
            MessageProposalCreateCommand command
        )
        {
            var listProposals = new List<ProposalAppointmentEntity>();

            var message = new MessageProposalEntity
            {
                CommonId = command.CommonId,
                ConversationId = new Guid(command.ConversationId),
                EmitterId = new Guid(command.EmitterId),
                ReceiverId = new Guid(command.ReceiverId),
                Content = command.Content,
                Read = command.Read,
                CreatedOn = command.CreatedOn,

            };
            foreach (var dateTime in command.DateTimeList)
            {
                listProposals.Add(new ProposalAppointmentEntity()
                {
                    DateTime = dateTime,
                    ProposalStaus = ProposalStatus.Pending,
                    MessageProposal = message
                });
            }
            message.ProposalAppointments = listProposals;
            return message;
        }
    }
}

