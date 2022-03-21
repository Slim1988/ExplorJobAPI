using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorJobAPI.DAL.Entities.Jobs
{
    public class NounEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Head { get; set; }
        [StringLength(500)]
        public string NewHead { get; set; }
        public int Priority { get; set; }
        public bool Exclude { get; set; }
        public bool IsChar { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
