using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Offers;
using ExplorJobAPI.DAL.Mappers.Offers;
using ExplorJobAPI.Domain.Commands.Offers;
using ExplorJobAPI.Domain.Models.Offers;
using ExplorJobAPI.Domain.Repositories.KeywordLists;
using ExplorJobAPI.Domain.Repositories.Offers;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.Offers
{
    public class OfferSubscriptionsRepository : IOfferSubscriptionsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly IKeywordListsRepository _keywordListsRepository;
        private readonly OfferSubscriptionMapper _offerSubscriptionMapper;
        
        public OfferSubscriptionsRepository(
            ExplorJobDbContext explorJobDbContext,
            IKeywordListsRepository keywordListsRepository,
            OfferSubscriptionMapper offerSubscriptionMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _keywordListsRepository = keywordListsRepository;
            _offerSubscriptionMapper = offerSubscriptionMapper;
        }

        public async Task<IEnumerable<OfferSubscription>> FindAll() {
            try {
                var entities = await _explorJobDbContext
                    .OfferSubscriptions
                    .AsNoTracking()
                    .Include(
                        (OfferSubscriptionEntity entity) => entity.Company
                    )
                    .Include(
                        (OfferSubscriptionEntity entity) => entity.Periods
                    )
                    .Select(
                        (OfferSubscriptionEntity offerSubscription) => _offerSubscriptionMapper.EntityToDomainModel(
                            offerSubscription
                        )
                    )
                    .ToListAsync();

                return await Task.WhenAll(
                    entities.Select(
                        (OfferSubscription offerSubscription) => MapKeywordList(
                            offerSubscription
                        )
                    )
                );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<OfferSubscription>();
            }
        }

        public async Task<IEnumerable<Promote>> FindAllPromotesForSearchForm() {
            try {
                var entities = await _explorJobDbContext
                    .OfferSubscriptions
                    .AsNoTracking()
                    .Include(
                        (OfferSubscriptionEntity entity) => entity.Company
                    )
                    .Include(
                        (OfferSubscriptionEntity entity) => entity.Periods
                    )
                    .Select(
                        (OfferSubscriptionEntity offerSubscription) => _offerSubscriptionMapper.EntityToDomainModel(
                            offerSubscription
                        )
                    )
                    .ToListAsync();

                    var offerSubscriptions = entities.Where(
                        (OfferSubscription offerSubscription) => offerSubscription.IsSearchFormActive()
                    ).DistinctBy(
                        (OfferSubscription offerSubscription) => offerSubscription.Company.Id
                    );

                    offerSubscriptions = await Task.WhenAll(
                        offerSubscriptions.Select(
                            (OfferSubscription offerSubscription) => MapKeywordList(
                                offerSubscription
                            )
                        )
                    );

                    return offerSubscriptions.Select(
                        (OfferSubscription offerSubscription) => _offerSubscriptionMapper.OfferSubscriptionToPromote(
                            offerSubscription
                        )
                    );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<Promote>();
            }
        }

        public async Task<IEnumerable<OfferSubscription>> FindManyByCompanyIds(
            List<string> ids
        ) {
            try {
                var entities = await _explorJobDbContext
                    .OfferSubscriptions
                    .AsNoTracking()
                    .Where(
                        (OfferSubscriptionEntity entity) => ids.Contains(
                            entity.CompanyId.ToString()
                        )
                    )
                    .Include(
                        (OfferSubscriptionEntity entity) => entity.Company
                    )
                    .Include(
                        (OfferSubscriptionEntity entity) => entity.Periods
                    )
                    .Select(
                        (OfferSubscriptionEntity offerSubscription) => _offerSubscriptionMapper.EntityToDomainModel(
                            offerSubscription
                        )
                    )
                    .ToListAsync();

                    return await Task.WhenAll(
                        entities.Select(
                            (OfferSubscription offerSubscription) => MapKeywordList(
                                offerSubscription
                            )
                        )
                    );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<OfferSubscription>();
            }
        }

        public async Task<OfferSubscription> FindOneById(
            string id
        ) {
            try {
                OfferSubscriptionEntity entity = await _explorJobDbContext
                    .OfferSubscriptions
                    .SingleOrDefaultAsync(
                        (OfferSubscriptionEntity offerSubscription) => offerSubscription.Id.Equals(
                            new Guid(id)
                        )
                    );

                if (entity == null) {
                    return null;
                }

                var entry = _explorJobDbContext.Entry(entity);

                entry.Reference(
                    (OfferSubscriptionEntity offerSubscription) => offerSubscription.Company
                ).Load();

                entry.Collection(
                    (OfferSubscriptionEntity offerSubscription) => offerSubscription.Periods
                ).Load();

                var offerSubscription = _offerSubscriptionMapper.EntityToDomainModel(
                    entry.Entity
                );

                return await MapKeywordList(
                    offerSubscription
                );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            OfferSubscriptionCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                OfferSubscriptionEntity entity = _offerSubscriptionMapper.CreateCommandToEntity(
                    command
                );

                try {
                    context.Add(entity);
                    numberOfChanges = await context.SaveChangesAsync();

                    await UpdateOfferSubscriptionPeriods(
                        command.Periods,
                        entity
                    );
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "OfferSubscriptionCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            OfferSubscriptionUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     OfferSubscriptionEntity entity = await context
                        .OfferSubscriptions
                        .FirstOrDefaultAsync(
                            (OfferSubscriptionEntity offerSubscription) => offerSubscription.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = await UpdateOfferSubscriptionPeriods(
                            command.Periods,
                            entity
                        );

                        entity = _offerSubscriptionMapper.UpdateCommandToEntity(
                            command,
                            entity
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "OfferSubscriptionUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            OfferSubscriptionDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    OfferSubscriptionEntity entity = await context
                        .OfferSubscriptions
                        .FirstOrDefaultAsync(
                            (OfferSubscriptionEntity offerSubscription) => offerSubscription.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = await DeleteOfferSubscriptionPeriods(
                            entity
                        );

                        context.Remove(
                            _offerSubscriptionMapper.DeleteCommandToEntity(
                                command,
                                entity
                            )
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "OfferSubscriptionDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        private async Task<OfferSubscription> MapKeywordList(
            OfferSubscription offerSubscription
        ) {
            if (offerSubscription != null
            && offerSubscription.IsTypeKeywordList()
            ) {
                var keywordList = await _keywordListsRepository.FindOneById(
                    offerSubscription.ReferenceId.ToString()
                );

                offerSubscription.SetKeywordList(
                    keywordList
                );
            }

            return offerSubscription;
        }

        private async Task<OfferSubscriptionEntity> UpdateOfferSubscriptionPeriods(
            List<OfferSubscriptionPeriodCommand> periods,
            OfferSubscriptionEntity entity
        ) {
            await DeleteOfferSubscriptionPeriods(entity);

            periods.ForEach(
                (OfferSubscriptionPeriodCommand period) => {
                    _explorJobDbContext.Add(
                        new OfferSubscriptionPeriodEntity {
                            OfferSubscriptionEntityId = entity.Id,
                            Start = new DateTime(
                                period.StartYear,
                                period.StartMonth,
                                period.StartDay
                            ),
                            End = new DateTime(
                                period.EndYear,
                                period.EndMonth,
                                period.EndDay
                            )
                        }
                    );
                }
            );

            await _explorJobDbContext.SaveChangesAsync();
            return entity;
        }

        private async Task<OfferSubscriptionEntity> DeleteOfferSubscriptionPeriods(
            OfferSubscriptionEntity entity
        ) {
            var periods = await _explorJobDbContext
                .OfferSubscriptionPeriods
                .Where(
                    (OfferSubscriptionPeriodEntity period) => entity.Id.Equals(
                        period.OfferSubscriptionEntityId
                    )
                ).ToListAsync();

            periods.ForEach(
                (OfferSubscriptionPeriodEntity period) => {
                    _explorJobDbContext.Remove(period);
                }
            );

            await _explorJobDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
