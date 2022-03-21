using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExplorJobAPI.DAL.Entities.Offers
{
    public class OfferSubscriptionPeriodEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("OfferSubscriptionEntity")]
        public Guid OfferSubscriptionEntityId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
