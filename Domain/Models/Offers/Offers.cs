using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Models.Offers
{
    public static class Offers
    {
        public static OfferType SearchForm = new OfferType(
            "SearchForm",
            "Formulaire de recherche"
        );

        public static OfferType KeywordListSearchResults = new OfferType(
            "KeywordList - SearchResults",
            "Liste de mots-clés - Résultats de recherche"
        );

        public static OfferType KeywordListContactModal = new OfferType(
            "KeywordList - ContactModal",
            "Liste de mots-clés - Modale de contact"
        );

        public static OfferType JobDomainSearchResults = new OfferType(
            "JobDomain - SearchResults",
            "Domaine métier - Résultats de recherche"
        );

        public static OfferType JobDomainContactModal = new OfferType(
            "JobDomain - ContactModal",
            "Domaine métier - Modale de contact"
        );

        public static List<OfferType> AsList() {
            return new List<OfferType>() {
                Offers.SearchForm,
                Offers.KeywordListSearchResults,
                Offers.KeywordListContactModal,
                Offers.JobDomainSearchResults,
                Offers.JobDomainContactModal
            };
        }

        public static OfferType FindOneById(
            string id
        ) {
            return AsList().Find(
                (OfferType offer) => offer.Id.Equals(id)
            );
        }

        public static bool IsValid(
            OfferType offer
        ) {
            return AsList().Contains(
                offer
            );
        }
    }
}
