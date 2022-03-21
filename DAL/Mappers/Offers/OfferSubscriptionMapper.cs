using System;
using System.Linq;
using ExplorJobAPI.DAL.Entities.Offers;
using ExplorJobAPI.DAL.Mappers.Companies;
using ExplorJobAPI.Domain.Commands.Offers;
using ExplorJobAPI.Domain.Models.Offers;

namespace ExplorJobAPI.DAL.Mappers.Offers
{
    public class OfferSubscriptionMapper
    {
        private readonly CompanyMapper _companyMapper;

        public OfferSubscriptionMapper(
            CompanyMapper companyMapper
        ) {
            _companyMapper = companyMapper;
        }

        public OfferSubscription EntityToDomainModel(
            OfferSubscriptionEntity entity
        ) {
            return new OfferSubscription(
                entity.Id,
                _companyMapper.EntityToDomainModel(
                    entity.Company
                ),
                new OfferType(
                    entity.TypeId,
                    entity.TypeLabel
                ),
                entity.LogoUrl,
                entity.HighlightMessage,
                entity.Message,
                entity.ReferenceId,
                entity.Periods.Select(
                    (OfferSubscriptionPeriodEntity period) => new OfferSubscriptionPeriod(
                        period.Start,
                        period.End
                    )
                ).ToList(),
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public Promote OfferSubscriptionToPromote(
            OfferSubscription offerSubscription,
            bool isResultHeaderLogoActive = false,
            bool isContactModalLogoActive = false
        ) {
            return new Promote(
                offerSubscription.Company.Name,
                offerSubscription.Company.Slug,
                offerSubscription.LogoUrl,
                offerSubscription.HighlightMessage,
                offerSubscription.Message,
                isResultHeaderLogoActive,
                isContactModalLogoActive
            );
        }

        public OfferSubscriptionEntity CreateCommandToEntity(
            OfferSubscriptionCreateCommand command
        ) {
            return new OfferSubscriptionEntity {
                CompanyId = new Guid(command.CompanyId),
                TypeId = command.OfferTypeId,
                TypeLabel = command.OfferTypeLabel,
                LogoUrl = command.LogoUrl,
                HighlightMessage = command.HighlightMessage,
                Message = command.Message,
                ReferenceId = !command.OfferTypeId.Equals(
                    Domain.Models.Offers.Offers.SearchForm.Id
                )
                && !string.IsNullOrEmpty(command.ReferenceId)
                    ? new Guid(command.ReferenceId)
                    : Guid.Empty,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public OfferSubscriptionEntity UpdateCommandToEntity(
            OfferSubscriptionUpdateCommand command,
            OfferSubscriptionEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.CompanyId = new Guid(command.CompanyId);
            entity.TypeId = command.OfferTypeId;
            entity.TypeLabel = command.OfferTypeLabel;
            entity.LogoUrl = command.LogoUrl;
            entity.HighlightMessage = command.HighlightMessage;
            entity.Message = command.Message;
            entity.ReferenceId = !command.OfferTypeId.Equals(
                    Domain.Models.Offers.Offers.SearchForm.Id
                )
                && !string.IsNullOrEmpty(command.ReferenceId)
                    ? new Guid(command.ReferenceId)
                    : Guid.Empty;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public OfferSubscriptionEntity DeleteCommandToEntity(
            OfferSubscriptionDeleteCommand command,
            OfferSubscriptionEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
