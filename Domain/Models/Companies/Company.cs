using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.Companies;
using ExplorJobAPI.Domain.Models.Localization;

namespace ExplorJobAPI.Domain.Models.Companies
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ActivityArea { get; set; }
        public List<CompanyDynamicField> DynamicFields { get; set; }
        public string WebsiteUrl { get; set; }
        public CompanySocialNetworks SocialNetworks { get; set; }
        public CompanyMedias Medias { get; set; }
        public Coordinates Coordinates { get; set; }
        public CompanyTexts Texts { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Company(
            Guid id,
            string name,
            string slug,
            string activityArea,
            List<CompanyDynamicField> dynamicFields,
            string websiteUrl,
            CompanySocialNetworks socialNetworks,
            CompanyMedias medias,
            Coordinates coordinates,
            CompanyTexts texts,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Name = name;
            Slug = slug;
            ActivityArea = activityArea;
            DynamicFields = dynamicFields;
            WebsiteUrl = websiteUrl;
            SocialNetworks = socialNetworks;
            Medias = medias;
            Coordinates = coordinates;
            Texts = texts;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public CompanyDto ToDto() {
            return new CompanyDto(
                Id.ToString(),
                Name,
                ActivityArea,
                DynamicFields,
                WebsiteUrl,
                SocialNetworks,
                Medias,
                Coordinates,
                Texts
            );
        }
    }
}
