using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Domain.Models.Offers;

namespace ExplorJobAPI.Domain.Models.Jobs
{
    public class JobSearchResultRestricted
    {
        public UserRestrictedDto User { get; set; }
        public JobUserDto Job { get; set; }
        public double Relevance { get; set; }
        public Promote Promote { get; set; }

        public JobSearchResultRestricted(
            UserRestrictedDto user,
            JobUserDto job,
            double relevance,
            Promote promote
        ) {
            User = user;
            Job = job;
            Relevance = relevance;
            Promote = promote != null
                ? promote
                : new Promote();
        }
    }
}
