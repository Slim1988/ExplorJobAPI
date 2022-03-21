using ExplorJobAPI.DAL.Entities.Messaging.Appointment;
using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class SendReviewCommand
    {
        public Guid ProposalId { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public bool HasMet { get; set; }
        public Role? WhatWereYou { get; set; }
        public int? MeetingQuality { get; set; }
        public int? DoTheSame { get; set; }
        public int? SameCompany { get; set; }
        public IsExplorerGood? IsExplorerGood { get; set; }
        public IsExplorerGood? IsExplorerInterestingForCompany { get; set; }
        public Guid? whichJob { get; set; }
        public MeetingPlateform? MeetingPlateform { get; set; }
        public MeetingDuration? MeetingDuration { get; set; }
        public MeetingCanellationReason? MeetingCanellationReason { get; set; }
        public string MeetingCanellationReasonOther { get; set; }
        public int? Recommendation { get; set; }
        public string OtherComment { get; set; }
        public Guid CommonId { get; set; }

        public bool Valid() {
            return EmitterIsNotReceiver();
        }

        private bool EmitterIsNotReceiver() {
            return !EmitterId.Equals(
                ReceiverId
            );
        }
    }
}
