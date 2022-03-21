using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.Notifications
{
    public class NotificationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Recipient")]
        public Guid RecipientId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual UserEntity Recipient { get; set; }
    }
}
