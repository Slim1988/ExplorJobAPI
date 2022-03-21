using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorJobAPI.DAL.Entities.Appointment
{
    public class ProposalAppointmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid MessageProposalId { get; set; }
        public virtual MessageProposalEntity MessageProposal { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public ProposalStatus ProposalStaus { get; set; }
        public bool Reviewed { get; set; } = false;
    }

}
