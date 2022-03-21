using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExplorJobAPI.DAL.Entities.Companies
{
    public class CompanyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /* IsUnique */
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string ActivityArea { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicField1Title { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicField1Content { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicField2Title { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicField2Content { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string WebsiteUrl { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string SocialNetworkFacebookUrl { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string SocialNetworkTwitterUrl { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string SocialNetworkInstagramUrl { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string SocialNetworkLinkedInUrl { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string SocialNetworkYoutubeUrl { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string MediaLogoUrl { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string MediaBannerUrl { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string MediaPhotoUrl1 { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string MediaPhotoUrl2 { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string MediaPhotoUrl3 { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string MediaPhotoUrl4 { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string MediaPhotoUrl5 { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string MediaPhotoUrl6 { get; set; }

        [StringLength(400, MinimumLength = 3)]
        public string MediaYoutubeVideoEmbedUrl { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string MediaFooterPhotoUrl { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3)]
        public string CoordinatesMapEmbedUrl { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string TextCompanyCatchPhrase { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string TextPresentation { get; set; }

        [Required]
        [StringLength(900, MinimumLength = 3)]
        public string TextProfilesSought { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string TextJobsCatchPhrase { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicText1Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string DynamicText1Content { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicText2Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string DynamicText2Content { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string DynamicText3Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string DynamicText3Content { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string TextFooterPhrase { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string TextCareerWebsiteWording { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
