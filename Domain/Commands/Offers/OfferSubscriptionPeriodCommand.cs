namespace ExplorJobAPI.Domain.Commands.Offers
{
    public class OfferSubscriptionPeriodCommand
    {
        public int StartDay { get; set; }
        public int StartMonth { get; set; }
        public int StartYear { get; set; }
        public int EndDay { get; set; }
        public int EndMonth { get; set; }
        public int EndYear { get; set; }
    }
}
