using System;
using System.Collections.Generic;
using ExplorJobAPI.Infrastructure.Slug;

namespace ExplorJobAPI.Domain.Commands.Companies
{
    public class CompanyUpdateCommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ActivityArea { get; set; }
        public string DynamicField1Title { get; set; }
        public string DynamicField1Content { get; set; }
        public string DynamicField2Title { get; set; }
        public string DynamicField2Content { get; set; }
        public string WebsiteUrl { get; set; }
        public string SocialNetworkFacebookUrl { get; set; }
        public string SocialNetworkTwitterUrl { get; set; }
        public string SocialNetworkInstagramUrl { get; set; }
        public string SocialNetworkLinkedInUrl { get; set; }
        public string SocialNetworkYoutubeUrl { get; set; }
        public string MediaLogoUrl { get; set; }
        public string MediaBannerUrl { get; set; }
        public List<string> MediaPhotoUrls { get; set; }
        public string MediaYoutubeVideoEmbedUrl { get; set; }
        public string MediaFooterPhotoUrl { get; set; }
        public string CoordinatesMapEmbedUrl { get; set; }
        public string TextCompanyCatchPhrase { get; set; }
        public string TextPresentation { get; set; }
        public string TextProfilesSought { get; set; }
        public string TextJobsCatchPhrase { get; set; }
        public string DynamicText1Title { get; set; }
        public string DynamicText1Content { get; set; }
        public string DynamicText2Title { get; set; }
        public string DynamicText2Content { get; set; }
        public string DynamicText3Title { get; set; }
        public string DynamicText3Content { get; set; }
        public string TextFooterPhrase { get; set; }
        public string TextCareerWebsiteWording { get; set; }

        public string Slug {
            get { return SlugBuilder.Build(Name); }
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }

        public bool IsValid() {
            return !string.IsNullOrEmpty(Id)
                && !string.IsNullOrEmpty(Name)
                && !string.IsNullOrEmpty(ActivityArea)
                && !string.IsNullOrEmpty(DynamicField1Title)
                && !string.IsNullOrEmpty(DynamicField1Content)
                && !string.IsNullOrEmpty(DynamicField2Title)
                && !string.IsNullOrEmpty(DynamicField2Content)
                && !string.IsNullOrEmpty(WebsiteUrl)
                && !string.IsNullOrEmpty(MediaLogoUrl)
                && !string.IsNullOrEmpty(MediaBannerUrl)
                && MediaPhotoUrls.Count >= 2
                && MediaPhotoUrls.Count <= 6
                && !string.IsNullOrEmpty(MediaFooterPhotoUrl)
                && !string.IsNullOrEmpty(CoordinatesMapEmbedUrl)
                && !string.IsNullOrEmpty(TextCompanyCatchPhrase)
                && !string.IsNullOrEmpty(TextPresentation)
                && !string.IsNullOrEmpty(TextProfilesSought)
                && !string.IsNullOrEmpty(TextJobsCatchPhrase)
                && !string.IsNullOrEmpty(DynamicText1Title)
                && !string.IsNullOrEmpty(DynamicText1Content)
                && !string.IsNullOrEmpty(DynamicText2Title)
                && !string.IsNullOrEmpty(DynamicText2Content)
                && !string.IsNullOrEmpty(DynamicText3Title)
                && !string.IsNullOrEmpty(DynamicText3Content)
                && !string.IsNullOrEmpty(TextFooterPhrase)
                && !string.IsNullOrEmpty(TextCareerWebsiteWording);
        }
    }
}
