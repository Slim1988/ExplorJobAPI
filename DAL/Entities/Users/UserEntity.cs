using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ExplorJobAPI.DAL.Entities.UserDegrees;
using ExplorJobAPI.DAL.Entities.UserProfessionalSituations;
using ExplorJobAPI.DAL.Entities.UserFavorites;

namespace ExplorJobAPI.DAL.Entities.Users
{
    public class UserEntity : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Phone { get; set; }

        [StringLength(250, MinimumLength = 1)]
        public string AddressStreet { get; set; }

        [StringLength(250, MinimumLength = 1)]
        public string AddressComplement { get; set; }

        [RegularExpression(@"^[0-9]{5}$")]
        public string AddressZipCode { get; set; }

        [StringLength(150, MinimumLength = 1)]
        public string AddressCity { get; set; }

        [StringLength(500)]
        public string Presentation { get; set; }

        public bool IsProfessional { get; set; }

        [ForeignKey("LastDegree")]
        public Guid? LastDegreeId { get; set; }

        [ForeignKey("ProfessionalSituation")]
        public Guid? ProfessionalSituationId { get; set; }

        [StringLength(175)]
        public string CurrentCompany { get; set; }

        [StringLength(175)]
        public string CurrentSchool { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public virtual UserDegreeEntity LastDegree { get; set; }
        public virtual UserProfessionalSituationEntity ProfessionalSituation { get; set; }
        public virtual List<UserContactMethodJoin> ContactMethodJoins { get; } = new List<UserContactMethodJoin>();
        public virtual List<UserFavoriteEntity> Favorites { get; } = new List<UserFavoriteEntity>();
    }
}
