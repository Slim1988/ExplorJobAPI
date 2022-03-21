using System;
using System.Collections.Generic;
using ExplorJobAPI.DAL.Entities.Companies;
using ExplorJobAPI.Domain.Commands.Companies;
using ExplorJobAPI.Domain.Dto.Companies;
using ExplorJobAPI.Domain.Models.Companies;
using ExplorJobAPI.Domain.Models.Localization;

namespace ExplorJobAPI.DAL.Mappers.Companies
{
    public class CompanyMapper
    {
        public Company EntityToDomainModel(
            CompanyEntity entity
        ) {
            return new Company(
                entity.Id,
                entity.Name,
                entity.Slug,
                entity.ActivityArea,
                MapDynamicFields(entity),
                entity.WebsiteUrl,
                new CompanySocialNetworks(
                    entity.SocialNetworkFacebookUrl,
                    entity.SocialNetworkTwitterUrl,
                    entity.SocialNetworkInstagramUrl,
                    entity.SocialNetworkLinkedInUrl,
                    entity.SocialNetworkYoutubeUrl
                ),
                new CompanyMedias(
                    entity.MediaLogoUrl,
                    entity.MediaBannerUrl,
                    MapPhotoUrls(entity),
                    entity.MediaYoutubeVideoEmbedUrl,
                    entity.MediaFooterPhotoUrl
                ),
                new Coordinates(
                    entity.CoordinatesMapEmbedUrl
                ),
                new CompanyTexts(
                    entity.TextCompanyCatchPhrase,
                    entity.TextPresentation,
                    entity.TextProfilesSought,
                    entity.TextJobsCatchPhrase,
                    entity.DynamicText1Title,
                    entity.DynamicText1Content,
                    entity.DynamicText2Title,
                    entity.DynamicText2Content,
                    entity.DynamicText3Title,
                    entity.DynamicText3Content,
                    entity.TextFooterPhrase,
                    entity.TextCareerWebsiteWording
                ),
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public CompanyDto EntityToDomainDto(
            CompanyEntity entity
        ) {
            return new CompanyDto(
                entity.Id.ToString(),
                entity.Name,
                entity.ActivityArea,
                MapDynamicFields(entity),
                entity.WebsiteUrl,
                new CompanySocialNetworks(
                    entity.SocialNetworkFacebookUrl,
                    entity.SocialNetworkTwitterUrl,
                    entity.SocialNetworkInstagramUrl,
                    entity.SocialNetworkLinkedInUrl,
                    entity.SocialNetworkYoutubeUrl
                ),
                new CompanyMedias(
                    entity.MediaLogoUrl,
                    entity.MediaBannerUrl,
                    MapPhotoUrls(entity),
                    entity.MediaYoutubeVideoEmbedUrl,
                    entity.MediaFooterPhotoUrl
                ),
                new Coordinates(
                    entity.CoordinatesMapEmbedUrl
                ),
                new CompanyTexts(
                    entity.TextCompanyCatchPhrase,
                    entity.TextPresentation,
                    entity.TextProfilesSought,
                    entity.TextJobsCatchPhrase,
                    entity.DynamicText1Title,
                    entity.DynamicText1Content,
                    entity.DynamicText2Title,
                    entity.DynamicText2Content,
                    entity.DynamicText3Title,
                    entity.DynamicText3Content,
                    entity.TextFooterPhrase,
                    entity.TextCareerWebsiteWording
                )
            );
        }

        public CompanyEntity CreateCommandToEntity(
            CompanyCreateCommand command
        ) {
            return new CompanyEntity {
                Name = command.Name,
                Slug = command.Slug,
                ActivityArea = command.ActivityArea,
                DynamicField1Title = command.DynamicField1Title,
                DynamicField1Content = command.DynamicField1Content,
                DynamicField2Title = command.DynamicField2Title,
                DynamicField2Content = command.DynamicField2Content,
                WebsiteUrl = command.WebsiteUrl,
                SocialNetworkFacebookUrl = command.SocialNetworkFacebookUrl,
                SocialNetworkTwitterUrl = command.SocialNetworkTwitterUrl,
                SocialNetworkInstagramUrl = command.SocialNetworkInstagramUrl,
                SocialNetworkLinkedInUrl = command.SocialNetworkLinkedInUrl,
                SocialNetworkYoutubeUrl = command.SocialNetworkYoutubeUrl,
                MediaLogoUrl = command.MediaLogoUrl,
                MediaBannerUrl = command.MediaBannerUrl,
                MediaPhotoUrl1 = command.MediaPhotoUrls[0],
                MediaPhotoUrl2 = command.MediaPhotoUrls[1],
                MediaPhotoUrl3 = command.MediaPhotoUrls.Count > 2
                    ? command.MediaPhotoUrls[2]
                    : null,
                MediaPhotoUrl4 = command.MediaPhotoUrls.Count > 3
                    ? command.MediaPhotoUrls[3]
                    : null,
                MediaPhotoUrl5 = command.MediaPhotoUrls.Count > 4
                    ? command.MediaPhotoUrls[4]
                    : null,
                MediaPhotoUrl6 = command.MediaPhotoUrls.Count > 5
                    ? command.MediaPhotoUrls[5]
                    : null,
                MediaYoutubeVideoEmbedUrl = command.MediaYoutubeVideoEmbedUrl,
                MediaFooterPhotoUrl = command.MediaFooterPhotoUrl,
                CoordinatesMapEmbedUrl = command.CoordinatesMapEmbedUrl,
                TextCompanyCatchPhrase = command.TextCompanyCatchPhrase,
                TextPresentation = command.TextPresentation,
                TextProfilesSought = command.TextProfilesSought,
                TextJobsCatchPhrase = command.TextJobsCatchPhrase,
                DynamicText1Title = command.DynamicText1Title,
                DynamicText1Content = command.DynamicText1Content,
                DynamicText2Title = command.DynamicText2Title,
                DynamicText2Content = command.DynamicText2Content,
                DynamicText3Title = command.DynamicText3Title,
                DynamicText3Content = command.DynamicText3Content,
                TextFooterPhrase = command.TextFooterPhrase,
                TextCareerWebsiteWording = command.TextCareerWebsiteWording,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public CompanyEntity UpdateCommandToEntity(
            CompanyUpdateCommand command,
            CompanyEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Name = command.Name;
            entity.Slug = command.Slug;
            entity.ActivityArea = command.ActivityArea;
            entity.DynamicField1Title = command.DynamicField1Title;
            entity.DynamicField1Content = command.DynamicField1Content;
            entity.DynamicField2Title = command.DynamicField2Title;
            entity.DynamicField2Content = command.DynamicField2Content;
            entity.WebsiteUrl = command.WebsiteUrl;
            entity.SocialNetworkFacebookUrl = command.SocialNetworkFacebookUrl;
            entity.SocialNetworkTwitterUrl = command.SocialNetworkTwitterUrl;
            entity.SocialNetworkInstagramUrl = command.SocialNetworkInstagramUrl;
            entity.SocialNetworkLinkedInUrl = command.SocialNetworkLinkedInUrl;
            entity.SocialNetworkYoutubeUrl = command.SocialNetworkYoutubeUrl;
            entity.MediaLogoUrl = command.MediaLogoUrl;
            entity.MediaBannerUrl = command.MediaBannerUrl;
            entity.MediaPhotoUrl1 = command.MediaPhotoUrls[0];
            entity.MediaPhotoUrl2 = command.MediaPhotoUrls[1];
            entity.MediaPhotoUrl3 = command.MediaPhotoUrls.Count > 2
                ? command.MediaPhotoUrls[2]
                : null;
            entity.MediaPhotoUrl4 = command.MediaPhotoUrls.Count > 3
                ? command.MediaPhotoUrls[3]
                : null;
            entity.MediaPhotoUrl5 = command.MediaPhotoUrls.Count > 4
                ? command.MediaPhotoUrls[4]
                : null;
            entity.MediaPhotoUrl6 = command.MediaPhotoUrls.Count > 5
                ? command.MediaPhotoUrls[5]
                : null;
            entity.MediaYoutubeVideoEmbedUrl = command.MediaYoutubeVideoEmbedUrl;
            entity.MediaFooterPhotoUrl = command.MediaFooterPhotoUrl;
            entity.CoordinatesMapEmbedUrl = command.CoordinatesMapEmbedUrl;
            entity.TextCompanyCatchPhrase = command.TextCompanyCatchPhrase;
            entity.TextPresentation = command.TextPresentation;
            entity.TextProfilesSought = command.TextProfilesSought;
            entity.TextJobsCatchPhrase = command.TextJobsCatchPhrase;
            entity.DynamicText1Title = command.DynamicText1Title;
            entity.DynamicText1Content = command.DynamicText1Content;
            entity.DynamicText2Title = command.DynamicText2Title;
            entity.DynamicText2Content = command.DynamicText2Content;
            entity.DynamicText3Title = command.DynamicText3Title;
            entity.DynamicText3Content = command.DynamicText3Content;
            entity.TextFooterPhrase = command.TextFooterPhrase;
            entity.TextCareerWebsiteWording = command.TextCareerWebsiteWording;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public CompanyEntity DeleteCommandToEntity(
            CompanyDeleteCommand command,
            CompanyEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }

        private List<CompanyDynamicField> MapDynamicFields(
            CompanyEntity entity
        ) {
            var dynamicFields = new List<CompanyDynamicField>();

            dynamicFields.Add(
                new CompanyDynamicField(
                    entity.DynamicField1Title,
                    entity.DynamicField1Content
                )
            );

            dynamicFields.Add(
                new CompanyDynamicField(
                    entity.DynamicField2Title,
                    entity.DynamicField2Content
                )
            );

            return dynamicFields;
        }

        private List<string> MapPhotoUrls(
            CompanyEntity entity
        ) {
            var mediaPhotoUrls = new List<string>();

            mediaPhotoUrls.Add(
                entity.MediaPhotoUrl1
            );

            mediaPhotoUrls.Add(
                entity.MediaPhotoUrl2
            );

            if (!string.IsNullOrEmpty(entity.MediaPhotoUrl3)) {
                mediaPhotoUrls.Add(
                    entity.MediaPhotoUrl3
                );
            }

            if (!string.IsNullOrEmpty(entity.MediaPhotoUrl4)) {
                mediaPhotoUrls.Add(
                    entity.MediaPhotoUrl4
                );
            }

            if (!string.IsNullOrEmpty(entity.MediaPhotoUrl5)) {
                mediaPhotoUrls.Add(
                    entity.MediaPhotoUrl5
                );
            }

            if (!string.IsNullOrEmpty(entity.MediaPhotoUrl6)) {
                mediaPhotoUrls.Add(
                    entity.MediaPhotoUrl6
                );
            }

            return mediaPhotoUrls;
        }
    }
}
