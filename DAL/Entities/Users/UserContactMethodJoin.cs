using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.UserContactMethods;

namespace ExplorJobAPI.DAL.Entities.Users
{
    public class UserContactMethodJoin
    {
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey("UserContactMethod")]
        public Guid UserContactMethodId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual UserContactMethodEntity UserContactMethod { get; set; }
    }
}
