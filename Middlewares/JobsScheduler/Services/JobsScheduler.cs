using System;
using ExplorJobAPI.Middlewares.JobsScheduler.Jobs;
using Hangfire;

namespace ExplorJobAPI.Middlewares.JobsScheduler.Services
{
    public class JobsScheduler
    {
        public static void ScheduleRecurringJobs() {
            JobsScheduler.ScheduleAdminReportWeeklyJob();
            JobsScheduler.ScheduleUserHaveUnreadMessagesWeeklyJob();
            JobsScheduler.ScheduleUserHaveNewConversationsTodayOrUnreadMessagesJob();
        }

        private static void ScheduleAdminReportWeeklyJob() {
            RecurringJob.RemoveIfExists(
                nameof(AdminReportWeeklyJob)
            );

            RecurringJob.AddOrUpdate<AdminReportWeeklyJob>(
                nameof(AdminReportWeeklyJob),
                (job) => job.Run(JobCancellationToken.Null),
                Cron.Weekly(DayOfWeek.Monday, 10),
                TimeZoneInfo.Local
            );
        }

        private static void ScheduleUserHaveUnreadMessagesWeeklyJob() {
            RecurringJob.RemoveIfExists(
                nameof(UserHaveUnreadMessagesWeeklyJob)
            );

            RecurringJob.AddOrUpdate<UserHaveUnreadMessagesWeeklyJob>(
                nameof(UserHaveUnreadMessagesWeeklyJob),
                (job) => job.Run(JobCancellationToken.Null),
                Cron.Weekly(DayOfWeek.Sunday, 16, 30),
                TimeZoneInfo.Local
            );
        }

        private static void ScheduleUserHaveNewConversationsTodayOrUnreadMessagesJob() {
            RecurringJob.RemoveIfExists(
                nameof(UserHaveNewConversationsOrUnreadMessagesTodayJob)
            );

            RecurringJob.AddOrUpdate<UserHaveNewConversationsOrUnreadMessagesTodayJob>(
                nameof(UserHaveNewConversationsOrUnreadMessagesTodayJob),
                (job) => job.Run(JobCancellationToken.Null),
                Cron.Daily(23, 15),
                TimeZoneInfo.Local
            );
        }
    }
}
