using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.UserFavorites
{
    public class UserFavoriteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public Guid OwnerId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("JobUser")]
        public Guid? JobUserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual UserEntity Owner { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual JobUserEntity JobUser { get; set; }
    }
}
