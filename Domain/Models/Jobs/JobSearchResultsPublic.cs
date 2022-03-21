using System.Collections.Generic;
using ExplorJobAPI.Domain.Models.Offers;

namespace ExplorJobAPI.Domain.Models.Jobs
{
    public class JobSearchResultsPublic
    {
        public List<JobSearchResultPublic> Results { get; set; }
        public List<Promote> Promotes { get; set; }


        public JobSearchResultsPublic(
            List<JobSearchResultPublic> results,
            List<Promote> promotes
        ) {
            Results = results;
            Promotes = promotes;
        }
    }
}
