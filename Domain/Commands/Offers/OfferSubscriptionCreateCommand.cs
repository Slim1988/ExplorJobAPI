using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Models.Offers;

namespace ExplorJobAPI.Domain.Commands.Offers
{
    public class OfferSubscriptionCreateCommand
    {
        public string CompanyId { get; set; }
        public string OfferTypeId { get; set; }
        public string LogoUrl { get; set; }
        public string HighlightMessage { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public List<OfferSubscriptionPeriodCommand> Periods { get; set; }

        public string OfferTypeLabel {
            get {
                var offer = Models.Offers.Offers.FindOneById(
                    OfferTypeId
                );

                return offer != null
                    ? offer.Label
                    : string.Empty;
            }
        }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }

        public bool IsValid() {
            return CompanyId != null
                && Models.Offers.Offers.IsValid(
                    new OfferType(
                        OfferTypeId,
                        OfferTypeLabel
                    )
                )
                && LogoUrl != null
                && Periods.Count > 0
                && IsValidMessage();
        }

        private bool IsValidMessage() {
            if (string.IsNullOrEmpty(Message)) {
                return true;
            }
            else if (OfferTypeId.Equals(
                Models.Offers.Offers.SearchForm.Id
            )) {
                return Message.Length <= 75;
            }
            else if (OfferTypeId.Equals(
                Models.Offers.Offers.KeywordListSearchResults.Id
            ) || OfferTypeId.Equals(
                Models.Offers.Offers.KeywordListContactModal.Id
            )) {
                return HighlightMessage.Length <= 100
                    && Message.Length <= 150;
            }
            else if (OfferTypeId.Equals(
                Models.Offers.Offers.JobDomainSearchResults.Id
            ) || OfferTypeId.Equals(
                Models.Offers.Offers.JobDomainContactModal.Id
            )) {
                return HighlightMessage.Length <= 100
                    && Message.Length <= 150;
            }
            else {
                return false;
            }
        }
    }
}
