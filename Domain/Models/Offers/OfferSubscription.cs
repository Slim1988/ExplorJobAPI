using System;
using System.Collections.Generic;
using System.Linq;
using ExplorJobAPI.Domain.Models.Companies;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Domain.Models.KeywordLists;
using Slugify;

namespace ExplorJobAPI.Domain.Models.Offers
{
    public class OfferSubscription
    {
        public Guid Id { get; set; }
        public Company Company { get; set; }
        public OfferType Type { get; set; }
        public string LogoUrl { get; set; }
        public string HighlightMessage { get; set; }
        public string Message { get; set; }
        public Guid ReferenceId { get; set; }
        public List<OfferSubscriptionPeriod> Periods { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        private KeywordList _keywordList { get; set; } = null;

        public OfferSubscription(
            Guid id,
            Company company,
            OfferType type,
            string logoUrl,
            string highlightMessage,
            string message,
            Guid referenceId,
            List<OfferSubscriptionPeriod> periods,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Company = company;
            Type = type;
            LogoUrl = logoUrl;
            HighlightMessage = highlightMessage;
            Message = message;
            ReferenceId = referenceId;
            Periods = periods;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public bool SetKeywordList(
            KeywordList keywordList
        ) {
            if (keywordList == null) {
                return false;
            }

            _keywordList = keywordList.Id.Equals(
                    ReferenceId
                )
                ? keywordList
                : null;

            return _keywordList != null;
        }

        public bool IsTypeSearchForm() {
            return Type.Id.Equals(
                Offers.SearchForm.Id
            );
        }

        public bool IsTypeKeywordListSearchResults() {
            return Type.Id.Equals(
                Offers.KeywordListSearchResults.Id
            );
        }

        public bool IsTypeKeywordListContactModal() {
            return Type.Id.Equals(
                Offers.KeywordListContactModal.Id
            );
        }

        public bool IsTypeKeywordList() {
            return Type.Id.Equals(
                Offers.KeywordListSearchResults.Id
            ) || Type.Id.Equals(
                Offers.KeywordListContactModal.Id
            );
        }

        public bool IsTypeJobDomainSearchResults() {
            return Type.Id.Equals(
                Offers.JobDomainSearchResults.Id
            );
        }

        public bool IsTypeJobDomainContactModal() {
            return Type.Id.Equals(
                Offers.JobDomainContactModal.Id
            );
        }

        public bool IsTypeJobDomain() {
            return Type.Id.Equals(
                Offers.JobDomainSearchResults.Id
            ) || Type.Id.Equals(
                Offers.JobDomainContactModal.Id
            );
        }

        public bool IsActive() {
            return Periods.Find(
                (OfferSubscriptionPeriod period) => period.Start <= DateTime.Today
                    && period.End >= DateTime.Today
            ) != null;
        }

        public bool IsSearchFormActive() {
            return IsTypeSearchForm()
                && IsActive();
        }

        public bool IsKeywordListActive() {
            return IsTypeKeywordList()
                && IsActive();
        }

        public bool IsJobDomainActive() {
            return IsTypeJobDomain()
                && IsActive();
        }

        public bool HasReferenceId() {
            return !ReferenceId.Equals(
                Guid.Empty
            );
        }

        public bool MustMatchWording() {
            return IsKeywordListActive()
                || IsJobDomainActive();
        }

        public bool IsKeywordListMatching(
            string query
        ) {
            SlugHelper helper = new SlugHelper();
            return IsKeywordListActive()
                && _keywordList != null
                && _keywordList.Keywords
                    .Select(
                        (string keyword) => helper.GenerateSlug(keyword)
                    ).Contains(
                        query.ToLower()
                    );
        }

        public bool IsJobDomainMatching(
            JobDomain jobDomain
        ) {
            return IsJobDomainActive()
                && jobDomain.Id.Equals(
                    ReferenceId
                );
        }

        public bool IsJobDomainMatching(
            List<string> jobDomainIds
        ) {
            return IsJobDomainActive()
                && jobDomainIds.Contains(
                    ReferenceId.ToString()
                );
        }
    }
}
