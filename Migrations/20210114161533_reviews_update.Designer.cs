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
    [Migration("20210114161533_reviews_update")]
    partial class reviews_update
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.MessageProposalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

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

                    b.ToTable("MessageProposal");
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

                    b.Property<bool>("Reviewed")
                        .HasColumnType("boolean");

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
                        .HasMaxLength(50000)
                        .HasColumnType("character varying(50000)");

                    b.Property<string>("ContentHTML")
                        .HasMaxLength(50000)
                        .HasColumnType("character varying(50000)");

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<DateTime>("PublishedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

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
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

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
                        .HasMaxLength(175)
                        .HasColumnType("character varying(175)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Presentation")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

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

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.Appointment.ReviewEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommonId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("EmitterId")
                        .HasColumnType("uuid");

                    b.Property<bool>("HasMet")
                        .HasColumnType("boolean");

                    b.Property<int?>("MeetingCanellationReason")
                        .HasColumnType("integer");

                    b.Property<string>("MeetingCanellationReasonOther")
                        .HasColumnType("text");

                    b.Property<int?>("MeetingDuration")
                        .HasColumnType("integer");

                    b.Property<int?>("MeetingPlateform")
                        .HasColumnType("integer");

                    b.Property<int?>("MeetingQuality")
                        .HasColumnType("integer");

                    b.Property<string>("OtherComment")
                        .HasColumnType("text");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<int?>("Recommendation")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmitterId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Reviews");
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
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

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
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("AddressComplement")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("AddressStreet")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("AddressZipCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CurrentCompany")
                        .HasMaxLength(175)
                        .HasColumnType("character varying(175)");

                    b.Property<string>("CurrentSchool")
                        .HasMaxLength(175)
                        .HasColumnType("character varying(175)");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("IsProfessional")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LastDegreeId")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

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
                        .OnDelete(DeleteBehavior.SetNull)
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

                    b.Navigation("Conversation");

                    b.Navigation("Emitter");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.ProposalAppointmentEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Appointment.MessageProposalEntity", "MessageProposal")
                        .WithMany("ProposalAppointments")
                        .HasForeignKey("MessageProposalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MessageProposal");
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

                    b.Navigation("Contract");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobUserEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
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

                    b.Navigation("JobDomain");

                    b.Navigation("JobUser");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.Appointment.ReviewEntity", b =>
                {
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

                    b.Navigation("Emitter");

                    b.Navigation("Receiver");
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

                    b.Navigation("Interlocutor");

                    b.Navigation("Owner");
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

                    b.Navigation("Conversation");

                    b.Navigation("Emitter");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Notifications.NotificationEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.Users.UserEntity", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipient");
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

                    b.Navigation("JobUser");

                    b.Navigation("Owner");

                    b.Navigation("User");
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

                    b.Navigation("Instigator");

                    b.Navigation("User");
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

                    b.Navigation("Reported");

                    b.Navigation("Reporter");
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

                    b.Navigation("User");

                    b.Navigation("UserContactInformation");
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

                    b.Navigation("User");

                    b.Navigation("UserContactMethod");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserEntity", b =>
                {
                    b.HasOne("ExplorJobAPI.DAL.Entities.UserDegrees.UserDegreeEntity", "LastDegree")
                        .WithMany()
                        .HasForeignKey("LastDegreeId");

                    b.HasOne("ExplorJobAPI.DAL.Entities.UserProfessionalSituations.UserProfessionalSituationEntity", "ProfessionalSituation")
                        .WithMany()
                        .HasForeignKey("ProfessionalSituationId");

                    b.Navigation("LastDegree");

                    b.Navigation("ProfessionalSituation");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Appointment.MessageProposalEntity", b =>
                {
                    b.Navigation("ProposalAppointments");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobDomainEntity", b =>
                {
                    b.Navigation("JobUserJobDomainJoins");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Jobs.JobUserEntity", b =>
                {
                    b.Navigation("JobUserJobDomainJoins");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Messaging.ConversationEntity", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Proposals");
                });

            modelBuilder.Entity("ExplorJobAPI.DAL.Entities.Users.UserEntity", b =>
                {
                    b.Navigation("ContactInformationJoins");

                    b.Navigation("ContactMethodJoins");

                    b.Navigation("Favorites");
                });
#pragma warning restore 612, 618
        }
    }
}
