using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExplorJobAPI.DAL.Entities.Jobs
{
    public class JobUserJobDomainJoin
    {
        [Required]
        [ForeignKey("JobUser")]
        public Guid JobUserId { get; set; }

        [Required]
        [ForeignKey("JobDomain")]
        public Guid JobDomainId { get; set; }

        public virtual JobUserEntity JobUser { get; set; }
        public virtual JobDomainEntity JobDomain { get; set; }
    }
}
