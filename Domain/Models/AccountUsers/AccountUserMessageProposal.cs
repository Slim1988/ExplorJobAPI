using ExplorJobAPI.DAL.Entities.Appointment;
using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Models.AccountUsers
{
    public class AccountUserMessageProposal
    {
        public string Id { get; set; }
        public bool FromOwner { get; set; }
        public bool FromInterlocutor { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Date { get; set; }
        public List<ProposalAppointmentEntity> ProposalAppointments { get; set; }
        public Guid CommonId { get; set; }
        public AccountUserMessageProposal(
            string id,
            bool fromOwner,
            bool fromInterlocutor,
            string content,
            bool read,
            DateTime date,
            List<ProposalAppointmentEntity> proposalAppointments,
            Guid commonId
        )
        {
            Id = id;
            FromOwner = fromOwner;
            FromInterlocutor = fromInterlocutor;
            Content = content;
            Read = read;
            Date = date;
            ProposalAppointments = proposalAppointments;
            CommonId = commonId;
        }
    }
}
