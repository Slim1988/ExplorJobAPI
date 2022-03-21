using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.Messaging
{
    public class MessageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Conversation")]
        public Guid ConversationId { get; set; }

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

        public virtual ConversationEntity Conversation { get; set; }
        public virtual UserEntity Emitter { get; set; }
        public virtual UserEntity Receiver { get; set; }
    }
}
