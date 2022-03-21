
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExplorJobAPI.DAL.Entities.Contracts
{
    public class ContractEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Context { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 1)]
        public string Version { get; set; }

        [Required]
        [StringLength(50000, MinimumLength = 3)]
        public string Content { get; set; }

        [StringLength(50000, MinimumLength = 3)]
        public string ContentHTML { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
