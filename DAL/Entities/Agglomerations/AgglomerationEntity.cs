using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExplorJobAPI.DAL.Entities.Agglomerations
{
    public class AgglomerationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /* IsUnique */
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Label { get; set; }

        public string ZipCodes { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

    }
}
