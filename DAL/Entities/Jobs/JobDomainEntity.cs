using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExplorJobAPI.DAL.Entities.Jobs
{
    public class JobDomainEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Label { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public virtual List<JobUserJobDomainJoin> JobUserJobDomainJoins { get; } = new List<JobUserJobDomainJoin>();
    }
}
