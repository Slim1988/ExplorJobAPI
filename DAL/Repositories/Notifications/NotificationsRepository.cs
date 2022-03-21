
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Notifications;
using ExplorJobAPI.DAL.Mappers.Notifications;
using ExplorJobAPI.Domain.Commands.Notifications;
using ExplorJobAPI.Domain.Dto.Notifications;
using ExplorJobAPI.Domain.Models.Notifications;
using ExplorJobAPI.Domain.Repositories.Notifications;
using ExplorJobAPI.Infrastructure.Repositories;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.Notifications
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly NotificationMapper _notificationMapper;

        public NotificationsRepository(
            ExplorJobDbContext explorJobDbContext,
            NotificationMapper notificationMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _notificationMapper = notificationMapper;
        }

        public async Task<IEnumerable<Notification>> FindAllByRecipientId(
            string recipientId
        ) {
            try {
                return await _explorJobDbContext
                    .Notifications
                    .AsNoTracking()
                    .Where(
                        (NotificationEntity notification) => notification.RecipientId.Equals(
                            new Guid(recipientId)
                        )
                    )
                    .OrderByDescending(
                        (NotificationEntity notification) => notification.CreatedOn
                    )
                    .Select(
                        (NotificationEntity notification) => _notificationMapper.EntityToDomainModel(
                            notification
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<Notification>();
            }
        }

        public async Task<IEnumerable<NotificationDto>> FindAllDtoByRecipientId(
            string recipientId
        ) {
            try {
                return await _explorJobDbContext
                    .Notifications
                    .AsNoTracking()
                    .Where(
                        (NotificationEntity notification) => notification.RecipientId.Equals(
                            new Guid(recipientId)
                        )
                    )
                    .OrderByDescending(
                        (NotificationEntity notification) => notification.CreatedOn
                    )
                    .Select(
                        (NotificationEntity notification) => _notificationMapper.EntityToDomainDto(
                            notification
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<NotificationDto>();
            }
        }

        public async Task<IEnumerable<Notification>> FindLastsByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        ) {
            try {
                return await _explorJobDbContext
                    .Notifications
                    .AsNoTracking()
                    .Where(
                        (NotificationEntity notification) => notification.RecipientId.Equals(
                            new Guid(recipientId)
                        )
                    )
                    .OrderByDescending(
                        (NotificationEntity notification) => notification.CreatedOn
                    )
                    .Take(numberOfNotifications)
                    .Select(
                        (NotificationEntity notification) => _notificationMapper.EntityToDomainModel(
                            notification
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<Notification>();
            }
        }

        public async  Task<IEnumerable<NotificationDto>> FindLastsDtoByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        ) {
            try {
                return await _explorJobDbContext
                    .Notifications
                    .AsNoTracking()
                    .Where(
                        (NotificationEntity notification) => notification.RecipientId.Equals(
                            new Guid(recipientId)
                        )
                    )
                    .OrderByDescending(
                        (NotificationEntity notification) => notification.CreatedOn
                    )
                    .Take(numberOfNotifications)
                    .Select(
                        (NotificationEntity notification) => _notificationMapper.EntityToDomainDto(
                            notification
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<NotificationDto>();
            }
        }

        public async Task<IEnumerable<Notification>> FindAllAfterLastsByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        ) {
            try {
                return await _explorJobDbContext
                    .Notifications
                    .AsNoTracking()
                    .Where(
                        (NotificationEntity notification) => notification.RecipientId.Equals(
                            new Guid(recipientId)
                        )
                    )
                    .OrderByDescending(
                        (NotificationEntity notification) => notification.CreatedOn
                    )
                    .Skip(numberOfNotifications)
                    .Select(
                        (NotificationEntity notification) => _notificationMapper.EntityToDomainModel(
                            notification
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<Notification>();
            }
        }

        public async Task<IEnumerable<NotificationDto>> FindAllAfterLastsDtoByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        ) {
            try {
                return await _explorJobDbContext
                    .Notifications
                    .AsNoTracking()
                    .Where(
                        (NotificationEntity notification) => notification.RecipientId.Equals(
                            new Guid(recipientId)
                        )
                    )
                    .OrderByDescending(
                        (NotificationEntity notification) => notification.CreatedOn
                    )
                    .Skip(numberOfNotifications)
                    .Select(
                        (NotificationEntity notification) => _notificationMapper.EntityToDomainDto(
                            notification
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<NotificationDto>();
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            NotificationCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                NotificationEntity entity = _notificationMapper.CreateCommandToEntity(
                    command
                );

                try {
                    context.Add(entity);
                    numberOfChanges = await context.SaveChangesAsync();
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "NotificationCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            NotificationDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    NotificationEntity entity = await context
                        .Notifications
                        .FirstOrDefaultAsync(
                            (NotificationEntity notification) => notification.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _notificationMapper.DeleteCommandToEntity(
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
                    "NotificationDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
