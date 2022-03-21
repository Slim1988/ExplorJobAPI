using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.UserContactInformations;

namespace ExplorJobAPI.DAL.Entities.Users
{
    public class UserContactInformationJoin
    {
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey("UserContactInformation")]
        public Guid UserContactInformationId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual UserContactInformationEntity UserContactInformation { get; set; }
    }
}
