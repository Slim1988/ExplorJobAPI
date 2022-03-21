using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.Messaging
{
    public class ConversationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public Guid OwnerId { get; set; }

        [Required]
        [ForeignKey("Interlocutor")]
        public Guid InterlocutorId { get; set; }

        public bool Display { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public virtual UserEntity Owner { get; set; }
        public virtual UserEntity Interlocutor { get; set; }
        public virtual List<MessageEntity> Messages { get; } = new List<MessageEntity>();
        public virtual List<MessageProposalEntity> Proposals { get; } = new List<MessageProposalEntity>();
    }
}
