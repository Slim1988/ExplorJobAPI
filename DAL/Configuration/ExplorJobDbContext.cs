using Microsoft.EntityFrameworkCore;
using ExplorJobAPI.DAL.Entities.Contracts;
using ExplorJobAPI.DAL.Entities.UserDegrees;
using ExplorJobAPI.DAL.Entities.UserProfessionalSituations;
using ExplorJobAPI.DAL.Entities.UserReporting;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.DAL.Entities.UserMeetings;
using ExplorJobAPI.DAL.Entities.UserFavorites;
using ExplorJobAPI.DAL.Entities.UserContactInformations;
using ExplorJobAPI.DAL.Entities.UserContactMethods;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.Notifications;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.DAL.Entities.Messaging.Appointment;
using ExplorJobAPI.DAL.Entities.Agglomerations;
using ExplorJobAPI.DAL.Entities.KeywordLists;
using ExplorJobAPI.DAL.Entities.Companies;
using ExplorJobAPI.DAL.Entities.Offers;

namespace ExplorJobAPI.DAL.Configuration
{
    public class ExplorJobDbContext : DbContext
    { 
        public ExplorJobDbContext(
            DbContextOptions<ExplorJobDbContext> options
        ) : base(options) { }

        public static ExplorJobDbContext NewContext() {
            return new ExplorJobDbContext(
                new DbContextOptionsBuilder<ExplorJobDbContext>()
                .UseNpgsql(Config.ConnectionString)
                .Options
            );
        }

        public DbSet<UserDegreeEntity> UserDegrees { get; set; }
        public DbSet<UserProfessionalSituationEntity> UserProfessionalSituations { get; set; }
        public DbSet<UserReportingReasonEntity> UserReportingReasons { get; set; }
        public DbSet<UserReportedEntity> UserReporteds { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserContactMethodEntity> UserContactMethods { get; set; }
        public DbSet<UserContactInformationEntity> UserContactInformations { get; set; }
        public DbSet<UserFavoriteEntity> UserFavorites { get; set; }
        public DbSet<UserMeetingEntity> UserMeetings { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<ContractUserAcceptanceEntity> ContractUserAcceptances { get; set; }
        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<MessageProposalEntity> Appointments { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<JobDomainEntity> JobDomains { get; set; }
        public DbSet<JobUserEntity> JobUsers { get; set; }
        public DbSet<JobUserJobDomainJoin> JobUserJobDomainJoins { get; set; }
        public DbSet<NounEntity> Nouns { get; set; }
        public DbSet<GenericLabelEntity> GenericLabels { get; set; }
        public DbSet<AgglomerationEntity> Agglomerations { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<KeywordListEntity> KeywordLists { get; set; }
        public DbSet<OfferSubscriptionEntity> OfferSubscriptions { get; set; }
        public DbSet<OfferSubscriptionPeriodEntity> OfferSubscriptionPeriods { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        ) {
            modelBuilder
                .Entity<UserDegreeEntity>()
                .ToTable("UserDegrees");

            modelBuilder
                .Entity<UserProfessionalSituationEntity>()
                .ToTable("UserProfessionalSituations");

            modelBuilder
                .Entity<UserReportingReasonEntity>()
                .ToTable("UserReportingReasons");

            var userReportedsTable = modelBuilder
                .Entity<UserReportedEntity>()
                .ToTable("UserReporteds");
            
            userReportedsTable.HasOne(e => e.Reporter);
            userReportedsTable.HasOne(e => e.Reported);

            var userFavoritesTable = modelBuilder
                .Entity<UserFavoriteEntity>()
                .ToTable("UserFavorites");

            userFavoritesTable.HasOne(e => e.Owner);
            userFavoritesTable.HasOne(e => e.User);
            userFavoritesTable.HasOne(e => e.JobUser);
            
            var usersTable = modelBuilder
                .Entity<UserEntity>()
                .ToTable("Users");

            usersTable.HasOne(e => e.LastDegree);
            usersTable.HasOne(e => e.ProfessionalSituation);

            usersTable
                .HasMany(e => e.ContactMethodJoins)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);


            usersTable.HasMany(e => e.Favorites);

            var userContactMethodJoinsTable = modelBuilder
                .Entity<UserContactMethodJoin>()
                .ToTable("UserContactMethodJoins");

            userContactMethodJoinsTable
                .HasKey(j => new {
                    j.UserId,
                    j.UserContactMethodId
                });

            userContactMethodJoinsTable
                .HasOne(j => j.User)
                .WithMany(e => e.ContactMethodJoins)
                .HasForeignKey(j => j.UserId);


            var userMeetingsTable = modelBuilder
                .Entity<UserMeetingEntity>()
                .ToTable("UserMeetings");

            userMeetingsTable.HasOne(e => e.Instigator);
            userMeetingsTable.HasOne(e => e.User);

            modelBuilder
                .Entity<ContractEntity>()
                .ToTable("Contracts");

            var contractUserAcceptancesTable = modelBuilder
                .Entity<ContractUserAcceptanceEntity>()
                .ToTable("ContractUserAcceptances");

            contractUserAcceptancesTable.HasOne(e => e.Contract);
            contractUserAcceptancesTable.HasOne(e => e.User);

            var conversationsTable = modelBuilder
                .Entity<ConversationEntity>()
                .ToTable("Conversations");

            conversationsTable.HasOne(e => e.Owner);
            conversationsTable.HasOne(e => e.Interlocutor);
            conversationsTable.HasMany(e => e.Messages);
            conversationsTable.HasMany(e => e.Proposals);

            var messagesTable = modelBuilder
                .Entity<MessageEntity>()
                .ToTable("Messages");

            messagesTable.HasOne(e => e.Conversation);
            messagesTable.HasOne(e => e.Emitter);
            messagesTable.HasOne(e => e.Receiver);
            var reviewsTable = modelBuilder
                .Entity<ReviewEntity>()
                .ToTable("Reviews");

            reviewsTable.HasOne(e => e.Emitter);
            reviewsTable.HasOne(e => e.Receiver);
            var appointmentsTable = modelBuilder
                .Entity<MessageProposalEntity>()
                .ToTable("MessageProposal");

            appointmentsTable.HasOne(e => e.Emitter);
            appointmentsTable.HasOne(e => e.Receiver);
            appointmentsTable.Property(b => b.ConversationId).IsRequired(false);
            appointmentsTable.HasOne(b => b.Conversation)
            .WithMany(a => a.Proposals)
            .OnDelete(DeleteBehavior.SetNull);

            var notificationsTable = modelBuilder
                .Entity<NotificationEntity>()
                .ToTable("Notifications");

            notificationsTable.HasOne(e => e.Recipient);

            var jobDomainsTable = modelBuilder
                .Entity<JobDomainEntity>()
                .ToTable("JobDomains");

            jobDomainsTable
                .HasMany(e => e.JobUserJobDomainJoins)
                .WithOne(e => e.JobDomain)
                .HasForeignKey(e => e.JobDomainId);

            var jobUsersTable = modelBuilder
                .Entity<JobUserEntity>()
                .ToTable("JobUsers");

            jobUsersTable.HasOne(e => e.User);

            jobUsersTable
                .HasMany(e => e.JobUserJobDomainJoins)
                .WithOne(e => e.JobUser)
                .HasForeignKey(e => e.JobUserId);

            var jobUserJobDomainJoinsTable = modelBuilder
                .Entity<JobUserJobDomainJoin>()
                .ToTable("JobUserJobDomainJoins");
            
            jobUserJobDomainJoinsTable
                .HasKey(j => new {
                    j.JobUserId,
                    j.JobDomainId
                });

            jobUserJobDomainJoinsTable
                .HasOne(j => j.JobUser)
                .WithMany(e => e.JobUserJobDomainJoins)
                .HasForeignKey(j => j.JobUserId);

            jobUserJobDomainJoinsTable
                .HasOne(j => j.JobDomain)
                .WithMany(e => e.JobUserJobDomainJoins)
                .HasForeignKey(j => j.JobDomainId);

            var agglomerationsTable = modelBuilder
                .Entity<AgglomerationEntity>()
                .ToTable("Agglomerations");

            agglomerationsTable
                .HasIndex(e => e.Label)
                .IsUnique();

            var keywordListsTable = modelBuilder
                .Entity<KeywordListEntity>()
                .ToTable("KeywordLists");

            keywordListsTable
                .HasIndex(e => e.Name)
                .IsUnique();

            var companiesTable = modelBuilder
                .Entity<CompanyEntity>()
                .ToTable("Companies");

            companiesTable
                .HasIndex(e => e.Name)
                .IsUnique();

            var offerSubscriptionsTable = modelBuilder
                .Entity<OfferSubscriptionEntity>()
                .ToTable("OfferSubscriptions");

            var offerSubscriptionPeriodsTable = modelBuilder
                .Entity<OfferSubscriptionPeriodEntity>()
                .ToTable("OfferSubscriptionPeriods");

            base.OnModelCreating(modelBuilder);
        }
    }
}
