
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.DAL.Entities.Contracts
{
    public class ContractUserAcceptanceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Contract")]
        public Guid ContractId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public DateTime AcceptedOn { get; set; }

        public virtual ContractEntity Contract { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
