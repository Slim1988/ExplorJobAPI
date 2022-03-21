using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Mappers.Jobs;
using ExplorJobAPI.DAL.Mappers.Users;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Domain.Queries.Jobs;
using ExplorJobAPI.Domain.Repositories.Jobs;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.Infrastructure.Strings.Services;
using ExplorJobAPI.Domain.Models.Agglomerations;
using ExplorJobAPI.Domain.Repositories.Agglomerations;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.Domain.Models.Offers;
using ExplorJobAPI.Domain.Repositories.Offers;
using ExplorJobAPI.DAL.Mappers.Offers;
using MoreLinq;

namespace ExplorJobAPI.DAL.Repositories.Jobs
{
    public class JobSearchesRepository : IJobSearchesRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly IAgglomerationsRepository _agglomerationsRepository;
        private readonly IOfferSubscriptionsRepository _offerSubscriptionsRepository;
        private readonly UserMapper _userMapper;
        private readonly JobUserMapper _jobUserMapper;
        private readonly JobDomainMapper _jobDomainMapper;
        private readonly OfferSubscriptionMapper _offerSubscriptionMapper;

        public JobSearchesRepository(
            ExplorJobDbContext explorJobDbContext,
            IAgglomerationsRepository agglomerationsRepository,
            IOfferSubscriptionsRepository offerSubscriptionsRepository,
            UserMapper userMapper,
            JobUserMapper jobUserMapper,
            JobDomainMapper jobDomainMapper,
            OfferSubscriptionMapper offerSubscriptionMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _agglomerationsRepository = agglomerationsRepository;
            _offerSubscriptionsRepository = offerSubscriptionsRepository;
            _userMapper = userMapper;
            _jobUserMapper = jobUserMapper;
            _jobDomainMapper = jobDomainMapper;
            _offerSubscriptionMapper = offerSubscriptionMapper;
        }

        public async Task<JobSearchResultsPublic> SearchPublic(
            JobSearchQuery query
        ) {
            var offerSubscriptions = (await _offerSubscriptionsRepository.FindAll()).ToList();

            var filtered = await Filter(
                query
            );

            var dominantJobDomain = GetDominantJobDomainEntity(
                filtered
            );

            var nouns = await GetNouns();

            var results = filtered.Select(
                (KeyValuePair<JobUserEntity, double> entry) => {
                    var jobUser = entry.Key as JobUserEntity;
                    var relevance = entry.Value;

                    return MapJobSearchResultPublic(
                        query,
                        jobUser.User,
                        jobUser,
                        relevance,
                        offerSubscriptions,
                        dominantJobDomain,
                        nouns
                    );
                }
            ).ToList();

            var promotes = BuildPromotes(
                query,
                offerSubscriptions,
                dominantJobDomain,
                nouns
            );

            return new JobSearchResultsPublic(
                results,
                promotes
            );
        }

        public async Task<JobSearchResultsRestricted> SearchRestricted(
            JobSearchQuery query
        ) {
            var offerSubscriptions = (await _offerSubscriptionsRepository.FindAll()).ToList();

            var filtered = await Filter(
                query
            );

            var dominantJobDomain = GetDominantJobDomainEntity(
                filtered
            );

            var nouns = await GetNouns();

            var results = filtered.Select(
                (KeyValuePair<JobUserEntity, double> entry) => {
                    var jobUser = entry.Key as JobUserEntity;
                    var relevance = entry.Value;

                    return MapJobSearchResultRestricted(
                        query,
                        jobUser.User,
                        jobUser,
                        relevance,
                        offerSubscriptions,
                        dominantJobDomain,
                        nouns
                    );
                }
            ).ToList();

            var promotes = BuildPromotes(
                query,
                offerSubscriptions,
                dominantJobDomain,
                nouns
            );

            return new JobSearchResultsRestricted(
                results,
                promotes
            );
        }

        private async Task<List<KeyValuePair<JobUserEntity, double>>> Filter(
            JobSearchQuery query
        ) {
            var dictionary = new Dictionary<JobUserEntity, double>();

            List<JobUserEntity> jobs = await GetJobs();
            List<NounEntity> nouns = await GetNouns();

            List<NounEntity> specialCharacters = nouns.Where(n => n.IsChar).ToList();
            List<NounEntity> excludedNouns = nouns.Where(n => n.Exclude && !n.IsChar).ToList();
            List<string> lowPriorityNouns = nouns.Where(n => n.Priority >= JobSearchQueryRelevance.LowPriorityMinimum).Select(n => n.Head).ToList().ReplaceSpecialCharacters(specialCharacters);
            List<Agglomeration> agglomerations = (await _agglomerationsRepository.FindAll()).ToList();
            Agglomeration queryLocalisationAgglomeration = null;
            List<string> searchedZipCodes = new List<string>();

            if (!string.IsNullOrEmpty(query.Localisation) || !string.IsNullOrEmpty(query.Query)) {

                var searchedAgglomerations = !string.IsNullOrEmpty(query.Localisation) 
                ? query.Localisation
                : query.Query;

                foreach (string searchedAgglomeration in searchedAgglomerations.Split(' '))
                {
                    queryLocalisationAgglomeration = agglomerations.FirstOrDefault(
                        (Agglomeration agglomeration) => searchedAgglomeration
                        .ReplaceSpecialCharacters(specialCharacters)
                        .ToLowerInvariant()
                        .Contains(
                            agglomeration.Label
                                .ToLowerInvariant()
                                .ReplaceSpecialCharacters(specialCharacters)
                        )
                    );

                    if (queryLocalisationAgglomeration != null) {
                        searchedZipCodes.AddRange(queryLocalisationAgglomeration.ZipCodes);
                    }
                }
             
            }

            List<string> queryParts = query.Query != null
                && query.Query.Length > 0
                ? query.Query?.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select
                (
                    (string queryPart) =>
                        queryPart.ToLowerInvariant()
                ).ToList()
                : null;


            if (queryParts != null)
            {
                queryParts.RemoveAll(qp => qp == string.Empty);
                queryParts = queryParts.IgnoreExcludedWords(excludedNouns);
                queryParts = queryParts.ReplaceSpecialCharacters(specialCharacters);
                queryParts.ForEach(qp => qp.ToLowerInvariant());
                queryParts.RemoveAll(qp => qp == "");
            }

            var normalizedJobs = jobs.ReplaceJobsSpecialCharacters(specialCharacters);

            _ = normalizedJobs.FindAll(
                (JobUserEntity job) =>
                {
                    double relevance = 0;
                    var searchStatus = JobSearchQueryStatus.INITIAL;
                    var rechercheLibreResults = JobSearchQueryStatus.INITIAL;
                    double relevanceCheck = 0;

                    if (job.Label != null
                    && job.Label.Length > 0
                    )
                    {
                        List<string> jobLabelParts = job.Label.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select(
                            (string jobLabelPart) => jobLabelPart.ToLowerInvariant()
                        ).ToList();

                        if (jobLabelParts.Count > 0)
                            jobLabelParts.RemoveAll(qp => qp == string.Empty);

                        #region RechercheLibreOnIntitule
                        if (queryParts != null)
                        {
                            List<string> matches = jobLabelParts.FindAll(
                                (string jobLabelPart) => queryParts.Contains(
                                    jobLabelPart
                                )
                            );

                            List<string> lowPriorityMatches = lowPriorityNouns.FindAll(
                                (string lowPriorityNoun) => matches.Contains(
                                    lowPriorityNoun)
                                );

                            foreach (var lpMatch in lowPriorityMatches)
                            {
                                matches.RemoveAll(m => m == lpMatch);
                            }

                            relevance += JobSearchQueryRelevance.QueryOnJobLabel * matches.Count + JobSearchQueryRelevance.QueryOnJobLabel * lowPriorityMatches.Count * JobSearchQueryRelevance.LowPriorityNounRelevanceRate;

                            if (queryParts != null && queryParts.Count > 0)
                            {
                                foreach (var part in queryParts)
                                {
                                    foreach (var jobPart in jobLabelParts)
                                    {
                                        relevance += part.GetSimilarity(jobPart, JobSearchQueryRelevance.SimilarityOnJobLabel) * jobPart.GetNounPriority(nouns);
                                        if (part.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)jobPart.Length / part.Length < 1.5)
                                        {
                                            relevance += part.GetRootSimilarity(jobPart, JobSearchQueryRelevance.SimilarityOnJobLabel) * jobPart.GetNounPriority(nouns);
                                        }
                                    }
                                }
                            }

                            if (relevance > 0 && searchStatus == JobSearchQueryStatus.INITIAL)
                            {
                                searchStatus = JobSearchQueryStatus.STARTED;
                                rechercheLibreResults = JobSearchQueryStatus.SUCCESS;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }
                        #endregion

                        #region IntituleOnIntitule
                        if (query.JobLabel != null
                        && query.JobLabel.Length > 0
                        )
                        {
                            query.JobLabel = query.JobLabel.IgnoreExcludedWords(excludedNouns);

                            List<string> matches = jobLabelParts.FindAll(
                                (string jobLabelPart) => query.JobLabel.ToLowerInvariant().ReplaceSpecialCharacters(specialCharacters).Split(' ').Contains(
                                    jobLabelPart
                                )
                            );

                            List<string> lowPriorityMatches = lowPriorityNouns.FindAll(
                                (string lowPriorityNoun) => matches.Contains(
                                    lowPriorityNoun)
                                );

                            foreach (var lpMatch in lowPriorityMatches)
                            {
                                matches.RemoveAll(m => m == lpMatch);
                            }

                            relevance += JobSearchQueryRelevance.JobLabel * matches.Count + JobSearchQueryRelevance.QueryOnJobLabel * lowPriorityMatches.Count * JobSearchQueryRelevance.LowPriorityNounRelevanceRate;

                            if (query.JobLabel.Length > 0)
                            {
                                foreach (var jobPart in jobLabelParts)
                                {
                                    relevance += query.JobLabel.GetSimilarity(jobPart, JobSearchQueryRelevance.SimilarityOnJobLabel);
                                    if (query.JobLabel.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)jobPart.Length / query.JobLabel.Length < 1.5)
                                    {
                                        relevance += query.JobLabel.GetRootSimilarity(jobPart, JobSearchQueryRelevance.SimilarityOnJobLabel);
                                    }
                                }
                            }
                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.STARTED && relevance == relevanceCheck)
                                {
                                    searchStatus = JobSearchQueryStatus.FAILED;
                                }
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }

                        if (job.Label == null && query.JobLabel != string.Empty && (searchStatus == JobSearchQueryStatus.STARTED || searchStatus == JobSearchQueryStatus.INITIAL))
                        {
                            searchStatus = JobSearchQueryStatus.FAILED;
                        }
                        if (relevance == 0 && query.JobLabel != string.Empty && (searchStatus == JobSearchQueryStatus.STARTED || searchStatus == JobSearchQueryStatus.INITIAL))
                        {
                            searchStatus = JobSearchQueryStatus.FAILED;
                        }

                        #endregion
                    }

                    #region Domain
                    if (job.JobUserJobDomainJoins != null
                    && query.JobDomainIds?.Count > 0)
                    {
                        List<JobDomainEntity> matches = job.JobUserJobDomainJoins.Where(
                            (JobUserJobDomainJoin join) => query.JobDomainIds.Contains(
                                join.JobDomainId.ToString()
                            )
                        ).Select(
                            (JobUserJobDomainJoin join) => join.JobDomain
                        ).ToList();

                        if (matches.Count > 0)
                        {
                            relevance += JobSearchQueryRelevance.JobDomain * matches.Count;
                        }
                        if (relevance > 0)
                        {
                            if (searchStatus == JobSearchQueryStatus.STARTED && relevance == relevanceCheck)
                            {
                                searchStatus = JobSearchQueryStatus.FAILED;
                            }
                            if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                            relevanceCheck = relevance;
                        }
                        if (relevance == 0 && relevanceCheck != relevance)
                            searchStatus = JobSearchQueryStatus.FAILED;
                    }

                    if (job.JobUserJobDomainJoins == null && query.JobDomainIds != null && (searchStatus == JobSearchQueryStatus.STARTED || searchStatus == JobSearchQueryStatus.INITIAL))
                    {
                        searchStatus = JobSearchQueryStatus.FAILED;
                    }
                    #endregion

                    if (job.Company != null
                    && job.Company.Length > 0
                    )
                    {
                        List<string> companyParts = job.Company.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select(
                            (string companyPart) => companyPart.ToLowerInvariant()
                        ).ToList();

                        if (companyParts.Count > 0)
                            companyParts.RemoveAll(qp => qp == string.Empty);
                        var normalisedCompanyParts = companyParts.ReplaceSpecialCharacters(specialCharacters);

                        #region RechercheLibreOnCompany

                        if (queryParts != null)
                        {
                            List<string> matches = normalisedCompanyParts.FindAll(
                                (string companyPart) => queryParts.Contains(
                                    companyPart
                                )
                            );

                            relevance += JobSearchQueryRelevance.QueryOnCompany * matches.Count;
                        }

                        if (queryParts != null && queryParts.Count > 0)
                        {
                            foreach (var part in queryParts)
                            {
                                foreach (var companyPart in companyParts)
                                {
                                    relevance += part.GetSimilarity(companyPart, JobSearchQueryRelevance.SimilarityOnCompany);
                                    if (part.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)companyPart.Length / part.Length < 1.5)
                                    {
                                        relevance += part.GetRootSimilarity(companyPart, JobSearchQueryRelevance.SimilarityOnCompany);
                                    }
                                }
                            }

                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.STARTED && relevance == relevanceCheck && rechercheLibreResults != JobSearchQueryStatus.SUCCESS && query.Company != string.Empty)
                                {
                                    searchStatus = JobSearchQueryStatus.FAILED;
                                }
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                rechercheLibreResults = JobSearchQueryStatus.SUCCESS;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }
                        #endregion

                        #region CompanyOnCompany
                        if (query.Company != null
                        && query.Company.Length > 0
                        )
                        {
                            query.Company = query.Company.IgnoreExcludedWords(excludedNouns);
                            query.Company = query.Company.ReplaceSpecialCharacters(specialCharacters);

                            List<string> matches = companyParts.FindAll(
                                (string companyPart) => query.Company.ToLowerInvariant().ReplaceSpecialCharacters(specialCharacters).Split(' ').Contains(
                                    companyPart
                                )
                            );

                            relevance += JobSearchQueryRelevance.Company * matches.Count;

                            if (query.Company.Length > 0)
                            {
                                foreach (var companyPart in companyParts)
                                {
                                    relevance += query.Company.GetSimilarity(companyPart, JobSearchQueryRelevance.SimilarityOnCompany);
                                    if (query.Company.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)companyPart.Length / query.Company.Length < 1.5)
                                    {
                                        relevance += query.Company.GetRootSimilarity(companyPart, JobSearchQueryRelevance.SimilarityOnCompany);
                                    }
                                }
                            }

                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.STARTED && relevance == relevanceCheck)
                                {
                                    searchStatus = JobSearchQueryStatus.FAILED;
                                }
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                            if (relevance == 0 && query.Company != string.Empty && query.Company.Length > 0)
                            {
                                searchStatus = JobSearchQueryStatus.FAILED;
                            }
                        }
                        #endregion
                    }

                    if (job.Company == null && query.Company != string.Empty && (searchStatus == JobSearchQueryStatus.STARTED || searchStatus == JobSearchQueryStatus.INITIAL))
                    {
                        searchStatus = JobSearchQueryStatus.FAILED;
                    }

                    if (!string.IsNullOrEmpty(job.User?.AddressCity)
                    ||  !string.IsNullOrEmpty(job.User?.AddressZipCode)
                    )
                    {
                        List<string> localisationAndZipCodeParts = 
                        !string.IsNullOrEmpty(job.User?.AddressCity) ?                        
                        job.User.AddressCity.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select(
                            (string localisationPart) => localisationPart.ToLowerInvariant()
                        ).ToList()
                        : new List<string>();

                        var normalizedJobUserAddressZipCode = job.User.AddressZipCode != null
                            ? job.User.AddressZipCode.Replace(" ", string.Empty)
                            : null;

                        if (!string.IsNullOrEmpty(normalizedJobUserAddressZipCode))
                        {
                            localisationAndZipCodeParts.Add(
                                normalizedJobUserAddressZipCode
                                .ReplaceSpecialCharacters(specialCharacters)
                            );
                        }

                        if (localisationAndZipCodeParts.Count > 0)
                            localisationAndZipCodeParts.RemoveAll(qp => qp == string.Empty);

                        var normalizedLocalisationParts = localisationAndZipCodeParts.IgnoreExcludedWords(excludedNouns);
                        normalizedLocalisationParts = localisationAndZipCodeParts.ReplaceLocalisationsSpecialCharacters(specialCharacters);


                        #region RechercheLibreOnLocalisation

                        if (queryParts != null)
                        {

                            var searchByZipCode = string.Join("", queryParts)
                            .ToLowerInvariant()
                            .Replace(" ", string.Empty).All(char.IsDigit);

                            List<string> matches = normalizedLocalisationParts.FindAll(
                                (string localisationPart) => queryParts.Contains(
                                    localisationPart
                                )
                                ||
                                searchedZipCodes.Contains(localisationPart)
                            );

                            relevance += JobSearchQueryRelevance.QueryOnLocalisation * matches.Count;

                            if (queryParts != null && queryParts.Count > 0 && !searchByZipCode)
                            {
                                foreach (var part in queryParts)
                                {
                                    foreach (var localisationPart in normalizedLocalisationParts)
                                    {
                                        relevance += part.GetSimilarity(localisationPart, JobSearchQueryRelevance.SimilarityOnLocalisation);
                                        if (part.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)localisationPart.Length / part.Length < 1.5)
                                        {
                                            relevance += part.GetRootSimilarity(localisationPart, JobSearchQueryRelevance.SimilarityOnLocalisation);
                                        }
                                    }
                                }
                            }
                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.STARTED && relevance == relevanceCheck && rechercheLibreResults != JobSearchQueryStatus.SUCCESS && query.Localisation != string.Empty)
                                {
                                    searchStatus = JobSearchQueryStatus.FAILED;
                                }
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                relevanceCheck = relevance;
                                rechercheLibreResults = JobSearchQueryStatus.SUCCESS;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }
                        #endregion

                        #region LocalisationOnLocalisation
                        if (query.Localisation != null
                        && query.Localisation.Length > 0
                        )
                        {
                            query.Localisation = query.Localisation.IgnoreExcludedWords(excludedNouns);
                            query.Localisation = query.Localisation.ReplaceSpecialCharacters(specialCharacters);

                            var searchByZipCode = query.Localisation
                            .ToLowerInvariant()
                            .Replace(" ", string.Empty).All(char.IsDigit);

                            List<string> matches = normalizedLocalisationParts.FindAll(
                                (string localisationPart) => query.Localisation.ToLowerInvariant().ReplaceSpecialCharacters(specialCharacters).Split(' ').Contains(
                                    localisationPart
                                )
                                ||
                                searchedZipCodes.Contains(localisationPart)
                            );

                            relevance += JobSearchQueryRelevance.Localisation * matches.Count;

                            if (query.Localisation.Length > 0 && !searchByZipCode)
                            {
                                foreach (var localisationPart in normalizedLocalisationParts)
                                {
                                    relevance += query.Localisation.GetSimilarity(localisationPart, JobSearchQueryRelevance.SimilarityOnLocalisation);
                                    if (query.Localisation.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)localisationPart.Length / query.Localisation.Length < 1.5)
                                    {
                                        relevance += query.Localisation.GetRootSimilarity(localisationPart, JobSearchQueryRelevance.SimilarityOnLocalisation);
                                    }
                                }
                            }

                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.STARTED && relevance == relevanceCheck)
                                {
                                    searchStatus = JobSearchQueryStatus.FAILED;
                                }
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }
                        #endregion
                    }

                    if (!string.IsNullOrEmpty(query.Localisation)
                    && string.IsNullOrEmpty(job.User?.AddressCity)
                    && string.IsNullOrEmpty(job.User?.AddressZipCode)
                    && (searchStatus == JobSearchQueryStatus.STARTED || searchStatus == JobSearchQueryStatus.INITIAL))
                    {
                        searchStatus = JobSearchQueryStatus.FAILED;
                    }

                    if (job.Presentation != null
                    && job.Presentation.Length > 0
                    )
                    {
                        List<string> presentationParts = job.Presentation.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select(
                            (string presentationPart) => presentationPart.ToLowerInvariant()
                        ).ToList();

                        if (presentationParts.Count > 0)
                            presentationParts.RemoveAll(qp => qp == string.Empty);

                        var normalizedPresentationParts = presentationParts.IgnoreExcludedWords(excludedNouns);
                        normalizedPresentationParts = presentationParts.ReplaceLocalisationsSpecialCharacters(specialCharacters);

                        #region RechercheLibreOnJobPresentation

                        if (queryParts != null)
                        {
                            List<string> matches = normalizedPresentationParts.FindAll(
                                (string presentationPart) => queryParts.Contains(
                                    presentationPart
                                )
                            );

                            List<string> lowPriorityMatches = lowPriorityNouns.FindAll(
                                (string lowPriorityNoun) => matches.Contains(
                                    lowPriorityNoun)
                                );

                            foreach (var lpMatch in lowPriorityMatches)
                            {
                                matches.RemoveAll(m => m == lpMatch);
                            }

                            relevance += JobSearchQueryRelevance.QueryOnJobPresentation * matches.Count + JobSearchQueryRelevance.QueryOnJobPresentation * lowPriorityMatches.Count * JobSearchQueryRelevance.LowPriorityNounRelevanceRate;

                            if (queryParts != null && queryParts.Count > 0)
                            {
                                foreach (var part in queryParts)
                                {
                                    foreach (var presentationPart in presentationParts)
                                    {
                                        relevance += part.GetSimilarity(presentationPart, JobSearchQueryRelevance.SimilarityOnJobPresentation);
                                        if (part.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)presentationPart.Length / part.Length < 1.5)
                                        {
                                            relevance += part.GetRootSimilarity(presentationPart, JobSearchQueryRelevance.SimilarityOnJobPresentation);
                                        }
                                    }
                                }
                            }

                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                rechercheLibreResults = JobSearchQueryStatus.SUCCESS;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }

                        #endregion
                    }


                    if (job.User?.Presentation != null
                    && job.User?.Presentation.Length > 0
                    )
                    {
                        #region  RechercheLibreOnUserPresentation
                        List<string> presentationParts = job.User.Presentation.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select(
                            (string presentationPart) => presentationPart.ToLowerInvariant()
                        ).ToList();

                        if (presentationParts.Count > 0)
                            presentationParts.RemoveAll(qp => qp == string.Empty);

                        var normalizedPresentationParts = presentationParts.IgnoreExcludedWords(excludedNouns);
                        normalizedPresentationParts = presentationParts.ReplaceLocalisationsSpecialCharacters(specialCharacters);

                        if (queryParts != null)
                        {
                            List<string> matches = normalizedPresentationParts.FindAll(
                                (string presentationPart) => queryParts.Contains(
                                    presentationPart
                                )
                            );

                            List<string> lowPriorityMatches = lowPriorityNouns.FindAll(
                                (string lowPriorityNoun) => matches.Contains(
                                    lowPriorityNoun)
                                );

                            foreach (var lpMatch in lowPriorityMatches)
                            {
                                matches.RemoveAll(m => m == lpMatch);
                            }

                            relevance += JobSearchQueryRelevance.QueryOnUserPresentation * matches.Count + JobSearchQueryRelevance.QueryOnJobPresentation * lowPriorityMatches.Count * JobSearchQueryRelevance.LowPriorityNounRelevanceRate;

                            if (queryParts != null && queryParts.Count > 0)
                            {
                                foreach (var part in queryParts)
                                {
                                    foreach (var presentationPart in normalizedPresentationParts)
                                    {
                                        relevance += part.GetSimilarity(presentationPart, JobSearchQueryRelevance.SimilarityOnUserPresentation);
                                        if (part.Length > JobSearchQueryRelevance.MinimumRootThreshold && (double)presentationPart.Length / part.Length < 1.5)
                                        {
                                            relevance += part.GetRootSimilarity(presentationPart, JobSearchQueryRelevance.SimilarityOnUserPresentation);
                                        }
                                    }
                                }
                            }

                            if (relevance > 0)
                            {
                                if (searchStatus == JobSearchQueryStatus.INITIAL) searchStatus = JobSearchQueryStatus.STARTED;
                                rechercheLibreResults = JobSearchQueryStatus.SUCCESS;
                                relevanceCheck = relevance;
                            }
                            if (relevance == 0 && relevanceCheck != relevance)
                                searchStatus = JobSearchQueryStatus.FAILED;
                        }
                    }

                    #endregion

                    if (searchStatus == JobSearchQueryStatus.FAILED || searchStatus == JobSearchQueryStatus.INITIAL) relevance = 0;

                    if (relevance > 0)
                    {
                        dictionary.Add(
                            job,
                            relevance
                        );
                    }

                    return relevance > 0;
                }
            );

            var keys = dictionary.Keys.ToList();
            keys.ForEach(k => {
                k.Label = jobs.Find(j => j.Id == k.Id).Label;
                k.Company = jobs.Find(j => j.Id == k.Id).Company;
                k.Presentation = jobs.Find(j => j.Id == k.Id).Presentation;
            });

            var values = dictionary.Values.ToList();

            var dict = new Dictionary<JobUserEntity, double>();
            for (int i = 0; i < dictionary.Count; i++)
            {
                if (query.JobLabel != null && keys[i].Label != null)
                {
                    if (keys[i].Label.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters) == query.JobLabel.ToLower()) { values[i] *= 10; };
                    var similarity = keys[i].Label.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters).GetSimilarity(query.JobLabel.ToLower(), JobSearchQueryRelevance.SimilarityOnJobLabel);
                    if (similarity > 0) { values[i] *= 9; }
                }
                if (queryParts != null && keys[i].Label != null)
                {
                    var normalizedJobLabel = keys[i].Label.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters);
                    foreach (var item in queryParts)
                    {
                        if (normalizedJobLabel.Contains(item)) values[i] *= 9;
                        var similarity = normalizedJobLabel.GetSimilarity(item, JobSearchQueryRelevance.SimilarityOnJobLabel);
                        if (similarity > 0) { values[i] *= 9; }
                    }                         
                }
                if (query.Company != null && keys[i].Company != null) 
                {
                    if (keys[i].Company.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters) == query.Company.ToLower()) { values[i] *= 10; };
                    var similarity = keys[i].Company.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters).GetSimilarity(query.Company.ToLower(), JobSearchQueryRelevance.SimilarityOnCompany);
                    if (similarity > 0) { values[i] *= 9; }
                }
                if (queryParts != null && keys[i].Company != null)
                {
                    var normalizedCompany = keys[i].Company.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters);
                    foreach (var item in queryParts)
                    {
                        if (normalizedCompany.ToLower().Contains(item)) values[i] *= 9;
                        var similarity = normalizedCompany.GetSimilarity(item, JobSearchQueryRelevance.SimilarityOnCompany);
                        if (similarity > 0) { values[i] *= 9; }
                    }
                }
                if (query.Localisation != null && keys[i].User.AddressCity != null)
                {
                    if (keys[i].User.AddressCity.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters) == query.Localisation.ToLower()) { values[i] *= 10; };
                    var similarity = keys[i].User.AddressCity.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters).GetSimilarity(query.Localisation.ToLower(), JobSearchQueryRelevance.SimilarityOnLocalisation);
                    if (similarity > 0) { values[i] *= 9; }
                }
                if (queryParts != null && keys[i].User.AddressCity != null)
                {
                    var normalizedLocalisation = keys[i].User.AddressCity.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters);
                    foreach (var item in queryParts)
                    {
                        if (normalizedLocalisation.ToLower().Contains(item)) values[i] *= 9;
                        var similarity = normalizedLocalisation.GetSimilarity(item, JobSearchQueryRelevance.SimilarityOnCompany);
                        if (similarity > 0) { values[i] *= 9; }
                    }
                }
                if (queryParts != null && keys[i].Presentation != null)
                {
                    var normalizedPresentation = keys[i].Presentation.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters);
                    foreach (var item in queryParts)
                    {
                        if (normalizedPresentation.ToLower().Contains(item)) values[i] *= 9;
                        var similarity = normalizedPresentation.GetSimilarity(item, JobSearchQueryRelevance.SimilarityOnCompany);
                        if (similarity > 0) { values[i] *= 9; }
                    }
                }

                if (queryParts != null && keys[i].User.Presentation != null)
                {
                    var normalizedPresentation = keys[i].User.Presentation.ToLower().IgnoreExcludedWords(excludedNouns).ReplaceSpecialCharacters(specialCharacters);
                    foreach (var item in queryParts)
                    {
                        if (normalizedPresentation.ToLower().Contains(item)) values[i] *= 9;
                        var similarity = normalizedPresentation.GetSimilarity(item, JobSearchQueryRelevance.SimilarityOnCompany);
                        if (similarity > 0) { values[i] *= 9; }
                    }
                }

                dict.Add(keys[i], values[i]);
            }

            var sortedList = dict.ToList();

            sortedList.Sort(delegate (KeyValuePair<JobUserEntity, double> firstPair,
                KeyValuePair<JobUserEntity, double> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );

            return sortedList;
        }

        private async Task<List<NounEntity>> GetNouns() {
            try {
                return await _explorJobDbContext
                    .Nouns
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<NounEntity>();
            }
        }

        private async Task<List<JobUserEntity>> GetJobs() {
            try {
                return await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Include(
                        (JobUserEntity jobUserEntity) => jobUserEntity.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .Include(
                        (JobUserEntity jobUserEntity) => jobUserEntity.User
                    )
                    .ThenInclude(
                        (UserEntity userEntity) => userEntity.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobUserEntity>();
            }
        }

        private JobDomainEntity GetDominantJobDomainEntity(
            List<KeyValuePair<JobUserEntity, double>> results
        ) {
            if (results.Count() < 1) {
                return null;
            }

            var jobUsers = results.Select(
                (KeyValuePair<JobUserEntity, double> pair) => pair.Key
            ).ToList();

            var jobDomains = new List<JobDomainEntity>();

            jobUsers.ForEach(
                (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins.ForEach(
                    (JobUserJobDomainJoin join) => jobDomains.Add(
                        join.JobDomain
                    )
                )
            );

            var groupedJobDomains = jobDomains.GroupBy(
                (JobDomainEntity jobDomain) => jobDomain.Id,
                (id, jobDomains) => new {
                    Id = id,
                    JobDomain = jobDomains.First(),
                    Count = jobDomains.Count()
                }
            ).ToList();

            int mostPresentJobDomainMaxCount = groupedJobDomains.Count() > 0
                ? groupedJobDomains.Max(
                    (group) => group.Count
                )
                : 0;

            if (mostPresentJobDomainMaxCount == 0) {
                return null;
            }

            JobDomainEntity mostPresentJobDomain = groupedJobDomains.Find(
                (group) => group.Count.Equals(
                    mostPresentJobDomainMaxCount
                )
            )?.JobDomain;

            int dominantPercentage = 70;
            int mostPresentJobDomainPercentage = mostPresentJobDomainMaxCount * 100 / results.Count();

            JobDomainEntity dominantJobDomain = mostPresentJobDomainPercentage >= dominantPercentage
                ? mostPresentJobDomain
                : null;

            return dominantJobDomain;
        }

        private JobSearchResultRestricted MapJobSearchResultRestricted(
            JobSearchQuery query,
            UserEntity userEntity,
            JobUserEntity jobUserEntity,
            double relevance,
            List<OfferSubscription> offerSubscriptions,
            JobDomainEntity dominantJobDomainEntity,
            List<NounEntity> nouns
        ) {
            var promote = MapResultPromote(
                query,
                jobUserEntity,
                offerSubscriptions,
                dominantJobDomainEntity,
                nouns
            );

            return new JobSearchResultRestricted(
                _userMapper.EntityToDomainRestrictedDto(
                    userEntity
                ),
                _jobUserMapper.EntityToDomainDto(
                    jobUserEntity
                ),
                relevance,
                promote
            );
        }

        private JobSearchResultPublic MapJobSearchResultPublic(
            JobSearchQuery query,
            UserEntity userEntity,
            JobUserEntity jobUserEntity,
            double relevance,
            List<OfferSubscription> offerSubscriptions,
            JobDomainEntity dominantJobDomainEntity,
            List<NounEntity> nouns
        ) {
            var promote = MapResultPromote(
                query,
                jobUserEntity,
                offerSubscriptions,
                dominantJobDomainEntity,
                nouns
            );

            return new JobSearchResultPublic(
                _userMapper.EntityToDomainPublicDto(
                    userEntity
                ),
                _jobUserMapper.EntityToDomainDto(
                    jobUserEntity
                ),
                relevance,
                promote
            );
        }

        private Promote MapResultPromote(
            JobSearchQuery query,
            JobUserEntity jobUserEntity,
            List<OfferSubscription> offerSubscriptions,
            JobDomainEntity dominantJobDomainEntity,
            List<NounEntity> nouns
        ) {
            if (string.IsNullOrEmpty(jobUserEntity.Company)) {
                return new Promote();
            }

            var correspondingOfferSubscriptions = new List<OfferSubscription>();

            offerSubscriptions.ForEach(
                (OfferSubscription offerSubscription) => {
                    if (offerSubscription.Company.Name.ToLower().Equals(
                            jobUserEntity.Company.ToLower()
                        ) && (
                            offerSubscription.IsSearchFormActive()
                            || offerSubscription.IsKeywordListActive()
                            || offerSubscription.IsJobDomainActive()
                        )
                    ) {
                        correspondingOfferSubscriptions.Add(
                            offerSubscription
                        );
                    }
                }
            );

            if (correspondingOfferSubscriptions != null
            && correspondingOfferSubscriptions.Count() > 0
            ) {
                var mustBePutForwardInSearchResults = false;
                var mustBePutForwardInContactModal = false;

                correspondingOfferSubscriptions.ForEach(
                    (OfferSubscription offerSubscription) => {
                        if (offerSubscription.IsKeywordListActive()) {
                            var queryParts = GetQueryParts(query.Query, nouns)
                                .Concat(
                                    GetQueryParts(query.JobLabel, nouns)
                                ).Where(
                                    (string queryPart) => !string.IsNullOrEmpty(queryPart)
                                );

                            mustBePutForwardInSearchResults = mustBePutForwardInSearchResults
                                || (
                                    offerSubscription.IsTypeKeywordListSearchResults()
                                    && queryParts.Any(
                                        (string queryPart) => offerSubscription.IsKeywordListMatching(
                                            queryPart
                                        )
                                    )
                                );

                            mustBePutForwardInContactModal = mustBePutForwardInContactModal
                                || (
                                    offerSubscription.IsTypeKeywordListContactModal()
                                    && queryParts.Any(
                                        (string queryPart) => offerSubscription.IsKeywordListMatching(
                                            queryPart
                                        )
                                    )
                                );
                        }
                        else if (offerSubscription.IsJobDomainActive()) {
                            mustBePutForwardInSearchResults = mustBePutForwardInSearchResults
                                || (
                                    offerSubscription.IsTypeJobDomainSearchResults()
                                    && (
                                        offerSubscription.IsJobDomainMatching(
                                            query.JobDomainIds
                                        )
                                        || (dominantJobDomainEntity != null
                                            && offerSubscription.IsJobDomainMatching(
                                                _jobDomainMapper.EntityToDomainModel(
                                                    dominantJobDomainEntity
                                                )
                                            )
                                        )
                                    )
                                );

                            mustBePutForwardInContactModal = mustBePutForwardInContactModal
                                || (
                                    offerSubscription.IsTypeJobDomainContactModal()
                                    && (
                                        offerSubscription.IsJobDomainMatching(
                                            query.JobDomainIds
                                        )
                                        || (dominantJobDomainEntity != null
                                            && offerSubscription.IsJobDomainMatching(
                                                _jobDomainMapper.EntityToDomainModel(
                                                    dominantJobDomainEntity
                                                )
                                            )
                                        )
                                    )
                                );
                        }
                    }
                );

                return _offerSubscriptionMapper.OfferSubscriptionToPromote(
                    offerSubscription: correspondingOfferSubscriptions.First(),
                    isResultHeaderLogoActive: mustBePutForwardInSearchResults,
                    isContactModalLogoActive: mustBePutForwardInContactModal
                );
            }
            else {
                var offerSubscription = offerSubscriptions.Find(
                    (OfferSubscription offerSubscription) => jobUserEntity.Company.ToLower().Equals(
                        offerSubscription.Company.Name.ToLower()
                    )
                );

                return offerSubscription != null 
                    ? new Promote(
                        offerSubscription.Company.Name,
                        offerSubscription.Company.Slug,
                        offerSubscription.LogoUrl
                    )
                    : new Promote();
            }
        }

        private List<Promote> BuildPromotes(
            JobSearchQuery query,
            List<OfferSubscription> offerSubscriptions,
            JobDomainEntity dominantJobDomainEntity,
            List<NounEntity> nouns
        ) {
            var promotes = new List<Promote>();

            offerSubscriptions.ForEach(
                (OfferSubscription offerSubscription) => {
                    if (offerSubscription.IsKeywordListActive()
                    || offerSubscription.IsJobDomainActive()
                    ) {
                        var mustBePutForwardInSearchResults = false;
                        var mustBePutForwardInContactModal = false;

                        if (offerSubscription.IsKeywordListActive()) {
                            var queryParts = GetQueryParts(query.Query, nouns)
                                .Concat(
                                    GetQueryParts(query.JobLabel, nouns)
                                ).Where(
                                    (string queryPart) => !string.IsNullOrEmpty(queryPart)
                                );

                            mustBePutForwardInSearchResults = mustBePutForwardInSearchResults
                                || (
                                    offerSubscription.IsTypeKeywordListSearchResults()
                                    && queryParts.Any(
                                        (string queryPart) => offerSubscription.IsKeywordListMatching(
                                            queryPart
                                        )
                                    )
                                );

                            mustBePutForwardInContactModal = mustBePutForwardInContactModal
                                || (
                                    offerSubscription.IsTypeKeywordListContactModal()
                                    && queryParts.Any(
                                        (string queryPart) => offerSubscription.IsKeywordListMatching(
                                            queryPart
                                        )
                                    )
                                );
                        }
                        else if (offerSubscription.IsJobDomainActive()) {
                            mustBePutForwardInSearchResults = mustBePutForwardInSearchResults
                                || (
                                    offerSubscription.IsTypeJobDomainSearchResults()
                                    && (
                                        offerSubscription.IsJobDomainMatching(
                                            query.JobDomainIds
                                        )
                                        || (dominantJobDomainEntity != null
                                            && offerSubscription.IsJobDomainMatching(
                                                _jobDomainMapper.EntityToDomainModel(
                                                    dominantJobDomainEntity
                                                )
                                            )
                                        )
                                    )
                                );

                            mustBePutForwardInContactModal = mustBePutForwardInContactModal
                                || (
                                    offerSubscription.IsTypeJobDomainContactModal()
                                    && (
                                        offerSubscription.IsJobDomainMatching(
                                            query.JobDomainIds
                                        )
                                        || (dominantJobDomainEntity != null
                                            && offerSubscription.IsJobDomainMatching(
                                                _jobDomainMapper.EntityToDomainModel(
                                                    dominantJobDomainEntity
                                                )
                                            )
                                        )
                                    )
                                );
                        }

                        if (mustBePutForwardInSearchResults
                        || mustBePutForwardInContactModal
                        ) {
                            promotes.Add(
                                _offerSubscriptionMapper.OfferSubscriptionToPromote(
                                    offerSubscription: offerSubscription,
                                    isResultHeaderLogoActive: mustBePutForwardInSearchResults,
                                    isContactModalLogoActive: mustBePutForwardInContactModal
                                )
                            );
                        }
                    }
                }
            );

            return promotes;
        }

        private List<string> GetQueryParts(
            string query,
            List<NounEntity> nouns
        ) {
            List<NounEntity> specialCharacters = nouns.Where(n => n.IsChar).ToList();
            List<NounEntity> excludedNouns = nouns.Where(n => n.Exclude && !n.IsChar).ToList();

            List<string> queryParts = !string.IsNullOrEmpty(query)
                ? query.ReplaceSpecialCharacters(specialCharacters).Split(' ').Select
                (
                    (string queryPart) =>
                        queryPart.ToLowerInvariant()
                ).ToList()
                : null;


            if (queryParts != null) {
                queryParts.RemoveAll(qp => qp == string.Empty);
                queryParts = queryParts.IgnoreExcludedWords(excludedNouns);
                queryParts = queryParts.ReplaceSpecialCharacters(specialCharacters);
                queryParts.ForEach(qp => qp.ToLowerInvariant());
                queryParts.RemoveAll(qp => qp == "");

                return queryParts;
            }
            else {
                return new List<string>() {
                    query
                };
            }
        }
    }

    public static class JobSearchesRepositoryExtensions
    {
        public static string ReplaceSpecialCharacters(this string queryParts, List<NounEntity> specialChars)
        {
            if (queryParts != null)
            {
                for (var j = 0; j < specialChars.Count; j++)
                {
                    if (queryParts.Contains(specialChars[j].Head))
                    {
                        queryParts = queryParts.Replace(specialChars[j].Head, specialChars[j].NewHead);
                        if (specialChars[j].Exclude)
                        {
                            queryParts = queryParts.Replace(specialChars[j].Head, string.Empty);
                        }
                    }
                }
            }
            return queryParts;
        }

        public static List<string> ReplaceSpecialCharacters(this List<string> queryParts, List<NounEntity> specialChars)
        {     
            for (var i=0; i < queryParts.Count; i++)
            {
                for (var j=0; j < specialChars.Count; j++)
                {
                    if (queryParts[i].Contains(specialChars[j].Head))
                    {
                        queryParts[i] = queryParts[i].Replace(specialChars[j].Head, specialChars[j].NewHead);
                        if (specialChars[j].Exclude)
                        {
                            queryParts[i] = queryParts[i].Replace(specialChars[j].Head, string.Empty);
                        }
                    }
                }
            }

        return queryParts;
        }

        public static bool HasSpecialCharacters(this JobUserEntity job, List<NounEntity> specialCharacters)
        {
            return specialCharacters.Where(ch => job.Label.Contains(ch.Head)).Any();
        }

        public static bool HasSpecialCharacters(this string localisation, List<NounEntity> specialCharacters)
        {
            return specialCharacters.Where(ch => localisation.Contains(ch.Head)).Any();
        }

        public static List<string> ReplaceLocalisationsSpecialCharacters(this List<string> localisations, List<NounEntity> specialCharacters)
        {
            var updatedList = new List<string>();
            foreach (var item in localisations)
            {
                updatedList.Add(item.HasSpecialCharacters(specialCharacters) ? item.ReplaceSpecialCharacters(specialCharacters) : item);                
            }
            return updatedList;
        }

        public static List<JobUserEntity> ReplaceJobsSpecialCharacters (this List<JobUserEntity> jobs, List<NounEntity> specialCharacters)
        {
            var updatedDictionary = new List<JobUserEntity>();
            foreach (var item in jobs)
            {
                var jobUserEntity = new JobUserEntity
                {
                    Company = item.HasSpecialCharacters(specialCharacters) ? item.Company.ReplaceSpecialCharacters(specialCharacters) : item.Company,
                    CreatedOn = item.CreatedOn,
                    Id = item.Id,
                    Label = item.HasSpecialCharacters(specialCharacters) ? item.Label.ReplaceSpecialCharacters(specialCharacters) : item.Label,
                    Presentation = item.Presentation,
                    UpdatedOn = item.UpdatedOn,
                    User = item.User,
                    UserId = item.UserId,
                    JobUserJobDomainJoins = item.JobUserJobDomainJoins
                };

                updatedDictionary.Add(jobUserEntity);
            }
            return updatedDictionary;
        }

        public static List<string> IgnoreExcludedWords(this List<string> queryParts, List<NounEntity> excludedNouns)
        {
            var updatedList = new List<string>();
            var excludedWords = excludedNouns.Select(n => n.Head).ToList();
            foreach (var part in queryParts)
            {
                if (excludedWords.Contains(part)) continue;
                updatedList.Add(part);
            }
            return updatedList;
        }

        public static string IgnoreExcludedWords(this string query, List<NounEntity> excludedNouns)
        {
            var excludedWords = excludedNouns.Select(n => n.Head).ToList();
            var queryParts = query.Split(' ');
            var exclusions = queryParts.Except(excludedWords);
            
            return string.Join(' ', exclusions);
        }

        public static double GetSimilarity(this string source, string target, double similarityScore)
        {
            var distance = StringsComparer.Compare(source, target);
            double similarity = 0;
            if (distance > 0) similarity = similarityScore * (Convert.ToDouble(1) / Convert.ToDouble(distance));
            return (similarity >= 0.75) ? similarity : 0;
        }

        public static double GetRootSimilarity(this string source, string target, double similarityScore)
        {
            var rootLength = Convert.ToInt32(source.Length * 0.8);
            var rootSource = source.Substring(0, rootLength);
            var rootTarget = target;
            if (target.Length > rootLength)
            {
                rootTarget = target.Substring(0, rootLength);
            }            
            var distance = StringsComparer.Compare(rootSource, rootTarget);
            if (distance == 0) return 1;
            double similarity = 0;
            if (distance > 0) similarity = similarityScore * (Convert.ToDouble(1) / Convert.ToDouble(distance));
            return (similarity > 0.8) ? similarity : 0;
        }

        public static double GetNounPriority(this string noun, List<NounEntity> nouns)
        {
            if (noun.IsLowCategory(nouns)) return JobSearchQueryRelevance.LowPriorityNounIndex;
            return JobSearchQueryRelevance.NormalPriorityNounIndex;
        }

        public static bool IsLowCategory(this string noun, List<NounEntity> nouns)
        {
            if (nouns.Select(n => n.Head).Contains(noun))
            {
                if (nouns.FirstOrDefault(n => n.Head == noun).Priority >= JobSearchQueryRelevance.LowPriorityMinimum) return true;
            }
            return false;
        }
    }
}
