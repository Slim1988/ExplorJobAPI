using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Companies;

namespace ExplorJobAPI.DAL.Entities.Offers
{
    public class OfferSubscriptionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }

        [Required]
        public string TypeId { get; set; }

        [Required]
        public string TypeLabel { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string LogoUrl { get; set; }

        [StringLength(300, MinimumLength = 3)]
        public string HighlightMessage { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Message { get; set; }

        public Guid ReferenceId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public virtual CompanyEntity Company { get; set; }
        public virtual List<OfferSubscriptionPeriodEntity> Periods { get; set; } = new List<OfferSubscriptionPeriodEntity>();
    }
}
