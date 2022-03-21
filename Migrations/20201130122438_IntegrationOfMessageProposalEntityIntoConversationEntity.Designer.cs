﻿// <auto-generated />
using System;
using ExplorJobAPI.DAL.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ExplorJobAPI.Migrations
{
    [DbContext(typeof(ExplorJobDbContext))]
    [Migration("20201130122438_IntegrationOfMessageProposalEntityIntoConversationEntity")]
    partial class IntegrationOfMessageProposalEntityIntoConversationEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.MessageProposalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("EmitterId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Read")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("EmitterId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.ProposalAppointmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("MessageProposalId")
                        .HasColumnType("uuid");

                    b.Property<int>("ProposalStaus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MessageProposalId");

                    b.ToTable("ProposalAppointmentEntity");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Contracts.ContractEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(50000)")
                        .HasMaxLength(50000);

                    b.Property<string>("ContentHTML")
                        .HasColumnType("character varying(50000)")
                        .HasMaxLength(50000);

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("PublishedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("character varying(9)")
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Contracts.ContractUserAcceptanceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AcceptedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("UserId");

                    b.ToTable("ContractUserAcceptances");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobDomainEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("JobDomains");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Company")
                        .HasColumnType("character varying(175)")
                        .HasMaxLength(175);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Presentation")
                        .IsRequired()
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("JobUsers");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobUserJobDomainJoin", b =>
                {
                    b.Property<Guid>("JobUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobDomainId")
                        .HasColumnType("uuid");

                    b.HasKey("JobUserId", "JobDomainId");

                    b.HasIndex("JobDomainId");

                    b.ToTable("JobUserJobDomainJoins");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.ConversationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Display")
                        .HasColumnType("boolean");

                    b.Property<Guid>("InterlocutorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("InterlocutorId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.MessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("EmitterId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Read")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("EmitterId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Notifications.NotificationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserContactInformations.UserContactInformationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("UserContactInformations");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserContactMethods.UserContactMethodEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("UserContactMethods");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserDegrees.UserDegreeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("UserDegrees");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserFavorites.UserFavoriteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("JobUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserEntityGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("JobUserId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("UserEntityGuid");

                    b.HasIndex("UserId");

                    b.ToTable("UserFavorites");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserMeetings.UserMeetingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("InstigatorId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Met")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InstigatorId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMeetings");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserProfessionalSituations.UserProfessionalSituationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("UserProfessionalSituations");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserReporting.UserReportedEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReportReason")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<Guid>("ReportedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReporterId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReportedId");

                    b.HasIndex("ReporterId");

                    b.ToTable("UserReporteds");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserReporting.UserReportingReasonEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("UserReportingReasons");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserContactInformationJoin", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserContactInformationId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "UserContactInformationId");

                    b.HasIndex("UserContactInformationId");

                    b.ToTable("UserContactInformationJoins");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserContactMethodJoin", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserContactMethodId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "UserContactMethodId");

                    b.HasIndex("UserContactMethodId");

                    b.ToTable("UserContactMethodJoins");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserEntity", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("AddressCity")
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<string>("AddressComplement")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("AddressStreet")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("AddressZipCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CurrentCompany")
                        .HasColumnType("character varying(175)")
                        .HasMaxLength(175);

                    b.Property<string>("CurrentSchool")
                        .HasColumnType("character varying(175)")
                        .HasMaxLength(175);

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsProfessional")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LastDegreeId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Presentation")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<Guid?>("ProfessionalSituationId")
                        .HasColumnType("uuid");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<string>("SkypeId")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.HasIndex("LastDegreeId");

                    b.HasIndex("ProfessionalSituationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.MessageProposalEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Messaging.ConversationEntity", "Conversation")
                        .WithMany("Proposals")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Emitter")
                        .WithMany()
                        .HasForeignKey("EmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.ProposalAppointmentEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Appointment.MessageProposalEntity", "MessageProposal")
                        .WithMany("ProposalAppointments")
                        .HasForeignKey("MessageProposalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Contracts.ContractUserAcceptanceEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Contracts.ContractEntity", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobUserEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobUserJobDomainJoin", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Jobs.JobDomainEntity", "JobDomain")
                        .WithMany("JobUserJobDomainJoins")
                        .HasForeignKey("JobDomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Jobs.JobUserEntity", "JobUser")
                        .WithMany("JobUserJobDomainJoins")
                        .HasForeignKey("JobUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.ConversationEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Interlocutor")
                        .WithMany()
                        .HasForeignKey("InterlocutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.MessageEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Messaging.ConversationEntity", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Emitter")
                        .WithMany()
                        .HasForeignKey("EmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Notifications.NotificationEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserFavorites.UserFavoriteEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Jobs.JobUserEntity", "JobUser")
                        .WithMany()
                        .HasForeignKey("JobUserId");

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", null)
                        .WithMany("Favorites")
                        .HasForeignKey("UserEntityGuid");

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserMeetings.UserMeetingEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Instigator")
                        .WithMany()
                        .HasForeignKey("InstigatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.UserReporting.UserReportedEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Reported")
                        .WithMany()
                        .HasForeignKey("ReportedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Reporter")
                        .WithMany()
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserContactInformationJoin", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.UserContactInformations.UserContactInformationEntity", "UserContactInformation")
                        .WithMany()
                        .HasForeignKey("UserContactInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany("ContactInformationJoins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserContactMethodJoin", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.UserContactMethods.UserContactMethodEntity", "UserContactMethod")
                        .WithMany()
                        .HasForeignKey("UserContactMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany("ContactMethodJoins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.UserDegrees.UserDegreeEntity", "LastDegree")
                        .WithMany()
                        .HasForeignKey("LastDegreeId");

                    b.HasOne("ExplorJobAPI.DAL.Entities.UserProfessionalSituations.UserProfessionalSituationEntity", "ProfessionalSituation")
                        .WithMany()
                        .HasForeignKey("ProfessionalSituationId");
                });
#pragma warning restore 612, 618
        }
    }
}
