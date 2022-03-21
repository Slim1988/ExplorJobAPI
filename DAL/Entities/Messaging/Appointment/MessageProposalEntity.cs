using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ExplorJobAPI.DAL.Entities.Appointment
{
    public class MessageProposalEntity 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CommonId { get; set; }
        [ForeignKey("Conversation")]
        public Guid? ConversationId { get; set; }

        [Required]
        [ForeignKey("Emitter")]
        public Guid EmitterId { get; set; }

        [Required]
        [ForeignKey("Receiver")]
        public Guid ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Read { get; set; }

        public DateTime CreatedOn { get; set; }
        [JsonIgnore]
        public virtual ConversationEntity Conversation { get; set; }
        public virtual UserEntity Emitter { get; set; }
        public virtual UserEntity Receiver { get; set; }

        [MaxLength(3)]
        public virtual List<ProposalAppointmentEntity> ProposalAppointments { get; set; } = new List<ProposalAppointmentEntity>();
    }
}
