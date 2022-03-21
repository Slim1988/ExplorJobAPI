using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Domain.Queries.Jobs;

namespace ExplorJobAPI.Domain.Repositories.Jobs
{
    public interface IJobSearchesRepository
    {
        Task<JobSearchResultsPublic> SearchPublic(
            JobSearchQuery query
        );

        Task<JobSearchResultsRestricted> SearchRestricted(
            JobSearchQuery query
        );
    }
}
