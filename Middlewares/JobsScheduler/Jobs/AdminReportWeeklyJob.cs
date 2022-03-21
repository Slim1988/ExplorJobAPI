using System.Linq;
using System;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.Domain.Commands.Emails;
using ExplorJobAPI.Domain.Services.Emails;
using Hangfire;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ExplorJobAPI.DAL.Entities.Users;

namespace ExplorJobAPI.Middlewares.JobsScheduler.Jobs
{
    public interface IAdminReportWeeklyJob {
        Task RunAtTimeOf(
            DateTime date
        );
    }
    public class AdminReportWeeklyJob : IAdminReportWeeklyJob
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly ISendEmailService _sendEmailService;

        public AdminReportWeeklyJob(
            ExplorJobDbContext explorJobDbContext,
            ISendEmailService sendEmailService
        ) {
            _explorJobDbContext = explorJobDbContext;
            _sendEmailService = sendEmailService;
        }

        public async Task Run(
            IJobCancellationToken token
        ) {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.Now);
        }

        public async Task RunAtTimeOf(
            DateTime date
        ) {
            Log.Information("'Admin report weekly' Job starts...");

            var report = new AdminReport(
                await NumberOfUsers(),
                await NumberOfProfessionals(),
                await NumberOfExplorers(),
                await NumberOfJobs(),
                await NumberOfConversations(),
                await NumberOfMessages()
            );

            await _sendEmailService.SendMany(
                report.ToEmails()
            );

            Log.Information("'Admin report weekly' Job completed.");
        }

        private class AdminReport {
            public int NumberOfUsers { get; set; }
            public int NumberOfProfessionals { get; set; }
            public int NumberOfExplorers { get; set; }
            public int NumberOfJobs { get; set; }
            public int NumberOfConversations { get; set; }
            public int NumberOfMessages { get; set; }

            public AdminReport(
                int numberOfUsers,
                int numberOfProfessionals,
                int numberOfExplorers,
                int numberOfJobs,
                int numberOfConversations,
                int numberOfMessages
            ) {
                NumberOfUsers = numberOfUsers;
                NumberOfProfessionals = numberOfProfessionals;
                NumberOfExplorers = numberOfExplorers;
                NumberOfJobs = numberOfJobs;
                NumberOfConversations = numberOfConversations;
                NumberOfMessages = numberOfMessages;
            }

            public SendEmailsCommand ToEmails() {
                return new SendEmailsCommand {
                    emails = Config.Emails.Admins.Select(
                        (string email) => new SendEmailCommand {
                            Email = email,
                            Subject = "ExplorJob - Compte rendu hebdomadaire",
                            Message = buildEmailBody(),
                            IsHtml = true
                        }
                    ).ToList()
                };
            }

            private string buildEmailBody() {
                var message = "<div>";

                message += $"<div>Nombre d'utilisateurs <strong>{ NumberOfUsers }</strong></div>";
                message += $"<div>Nombre de professionnels <strong>{ NumberOfProfessionals }</strong></div>";
                message += $"<div>Nombre d'explorateurs <strong>{ NumberOfExplorers }</strong></div>";
                message += $"<div>Nombre de métiers <strong>{ NumberOfJobs }</strong></div>";
                message += $"<div>Nombre de conversations <strong>{ NumberOfConversations }</strong></div>";
                message += $"<div>Nombre de messages <strong>{ NumberOfMessages }</strong></div>";
                
                message += "</div>";
                return message;
            }
        }

        public async Task<int> NumberOfUsers() {
            return (await _explorJobDbContext
                .Users
                .AsNoTracking()
                .ToListAsync()
                ).Count();
        }

        public async Task<int> NumberOfProfessionals() {
            return (await _explorJobDbContext
                .Users
                .AsNoTracking()
                .Where(
                    (UserEntity user) => user.IsProfessional
                )
                .ToListAsync()
                ).Count();
        }

        public async Task<int> NumberOfExplorers() {
            return (await _explorJobDbContext
                .Users
                .AsNoTracking()
                .Where(
                    (UserEntity user) => !user.IsProfessional
                )
                .ToListAsync()
                ).Count();
        }

        public async Task<int> NumberOfJobs() {
            return (await _explorJobDbContext
                .JobUsers
                .AsNoTracking()
                .ToListAsync()
                ).Count();
        }

        public async Task<int> NumberOfConversations() {
            return (await _explorJobDbContext
                .Conversations
                .AsNoTracking()
                .ToListAsync()
                ).Count() / 2;
        }

        public async Task<int> NumberOfMessages() {
            return (await _explorJobDbContext
                .Messages
                .AsNoTracking()
                .ToListAsync()
                ).Count() / 2;
        }
    }
}
