using System;

namespace ExplorJobAPI.Domain.Models.Offers
{
    public class OfferSubscriptionPeriod
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public OfferSubscriptionPeriod(
            DateTime start,
            DateTime end
        ) {
            Start = start;
            End = end;
        }

        public static DateTime CalculateEndFromStart(
            DateTime start
        ) {
            return start.AddDays(365);
        }
    }
}
