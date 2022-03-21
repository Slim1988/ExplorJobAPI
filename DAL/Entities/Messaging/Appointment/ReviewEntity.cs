using ExplorJobAPI.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorJobAPI.DAL.Entities.Messaging.Appointment
{
    public class ReviewEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public bool HasMet { get; set; }
        public Role? WhatWereYou { get; set; }
        public int? MeetingQuality { get; set; }
        public MeetingPlateform? MeetingPlateform { get; set; }
        public MeetingDuration? MeetingDuration { get; set; }
        public MeetingCanellationReason? MeetingCanellationReason { get; set; }
        public string MeetingCanellationReasonOther { get; set; }
        public int? Recommendation { get; set; }
        public int? DoTheSame { get; set; }
        public int? SameCompany { get; set; }
        public IsExplorerGood? IsExplorerGood { get;set; }
        public IsExplorerGood? IsExplorerInterestingForCompany { get;set; }
        [ForeignKey("WhichUser")]
        public Guid? JobUserEntityId { get; set; }
        public string OtherComment { get; set; }
        [Required]
        public Guid CommonId { get; set; }
        [Required]
        [ForeignKey("Emitter")]
        public Guid EmitterId { get; set; }
        [Required]
        [ForeignKey("Receiver")]
        public Guid ReceiverId { get; set; }
        public virtual UserEntity Emitter { get; set; }
        public virtual UserEntity Receiver { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }


    }
}
