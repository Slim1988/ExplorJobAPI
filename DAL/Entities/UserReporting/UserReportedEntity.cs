using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.UserReporting
{
    public class UserReportedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Reporter")]
        public Guid ReporterId { get; set; }

        [Required]
        [ForeignKey("Reported")]
        public Guid ReportedId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string ReportReason { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual UserEntity Reporter { get; set; }
        public virtual UserEntity Reported { get; set; }
    }
}
