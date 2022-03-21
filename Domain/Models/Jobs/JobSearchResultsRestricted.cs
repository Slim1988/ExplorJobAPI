using System.Collections.Generic;
using ExplorJobAPI.Domain.Models.Offers;

namespace ExplorJobAPI.Domain.Models.Jobs
{
    public class JobSearchResultsRestricted
    {
        public List<JobSearchResultRestricted> Results { get; set; }
        public List<Promote> Promotes { get; set; }


        public JobSearchResultsRestricted(
            List<JobSearchResultRestricted> results,
            List<Promote> promotes
        ) {
            Results = results;
            Promotes = promotes;
        }
    }
}
