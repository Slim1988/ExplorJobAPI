namespace ExplorJobAPI.Domain.Models.Offers
{
    public class OfferType
    {
        public string Id { get; private set; }
        public string Label { get; private set; }

        public OfferType(
            string id,
            string label
        ) {
            Id = id;
            Label = label;  
        }
    }
}
