using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.UserMeetings
{
    public class UserMeetingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Instigator")]
        public Guid InstigatorId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public bool Met { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual UserEntity Instigator { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
