using System.Collections.Generic;
using ExplorJobAPI.Domain.Models.Companies;
using ExplorJobAPI.Domain.Models.Localization;

namespace ExplorJobAPI.Domain.Dto.Companies
{
    public class CompanyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ActivityArea { get; set; }
        public List<CompanyDynamicField> DynamicFields { get; set; }
        public string WebsiteUrl { get; set; }
        public CompanySocialNetworks SocialNetworks { get; set; }
        public CompanyMedias Medias { get; set; }
        public Coordinates Coordinates { get; set; }
        public CompanyTexts Texts { get; set; }

        public CompanyDto(
            string id,
            string name,
            string activityArea,
            List<CompanyDynamicField> dynamicFields,
            string websiteUrl,
            CompanySocialNetworks socialNetworks,
            CompanyMedias medias,
            Coordinates coordinates,
            CompanyTexts texts
        ) {
            Id = id;
            Name = name;
            ActivityArea = activityArea;
            DynamicFields = dynamicFields;
            WebsiteUrl = websiteUrl;
            SocialNetworks = socialNetworks;
            Medias = medias;
            Coordinates = coordinates;
            Texts = texts;
        }
    }
}
