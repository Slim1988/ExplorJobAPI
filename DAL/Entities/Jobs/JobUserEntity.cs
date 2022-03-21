using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Messaging.Appointment;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.Jobs
{
    public class JobUserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Label { get; set; }

        [StringLength(175)]
        public string Company { get; set; }

        [Required]
        [StringLength(500)]
        public string Presentation { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public virtual List<JobUserJobDomainJoin> JobUserJobDomainJoins { get; set; } = new List<JobUserJobDomainJoin>();
        public virtual List<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
        public virtual UserEntity User { get; set; }
    }
}
