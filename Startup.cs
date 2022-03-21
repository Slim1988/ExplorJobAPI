using System.IO;
using System;
using System.Text;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.DAL.Mappers.Contracts;
using ExplorJobAPI.DAL.Mappers.Jobs;
using ExplorJobAPI.DAL.Mappers.Messaging;
using ExplorJobAPI.DAL.Mappers.Notifications;
using ExplorJobAPI.DAL.Mappers.UserContactInformations;
using ExplorJobAPI.DAL.Mappers.UserContactMethods;
using ExplorJobAPI.DAL.Mappers.UserDegrees;
using ExplorJobAPI.DAL.Mappers.UserFavorites;
using ExplorJobAPI.DAL.Mappers.UserMeetings;
using ExplorJobAPI.DAL.Mappers.UserProfessionalSituations;
using ExplorJobAPI.DAL.Mappers.UserReporting;
using ExplorJobAPI.DAL.Mappers.Users;
using ExplorJobAPI.DAL.Repositories.AccountUsers;
using ExplorJobAPI.DAL.Repositories.Contracts;
using ExplorJobAPI.DAL.Repositories.Jobs;
using ExplorJobAPI.DAL.Repositories.Messaging;
using ExplorJobAPI.DAL.Repositories.Notifications;
using ExplorJobAPI.DAL.Repositories.UserContactInformations;
using ExplorJobAPI.DAL.Repositories.UserContactMethods;
using ExplorJobAPI.DAL.Repositories.UserDegrees;
using ExplorJobAPI.DAL.Repositories.UserFavorites;
using ExplorJobAPI.DAL.Repositories.UserMeetings;
using ExplorJobAPI.DAL.Repositories.UserProfessionalSituations;
using ExplorJobAPI.DAL.Repositories.UserReporting;
using ExplorJobAPI.DAL.Repositories.Users;
using ExplorJobAPI.DAL.Services.Admin;
using ExplorJobAPI.DAL.Services.AuthUsers;
using ExplorJobAPI.DAL.Services.Messaging;
using ExplorJobAPI.Domain.Repositories.AccountUsers;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Domain.Repositories.Jobs;
using ExplorJobAPI.Domain.Repositories.Messaging;
using ExplorJobAPI.Domain.Repositories.Notifications;
using ExplorJobAPI.Domain.Repositories.UserContactInformations;
using ExplorJobAPI.Domain.Repositories.UserContactMethods;
using ExplorJobAPI.Domain.Repositories.UserDegrees;
using ExplorJobAPI.Domain.Repositories.UserFavorites;
using ExplorJobAPI.Domain.Repositories.UserMeetings;
using ExplorJobAPI.Domain.Repositories.UserProfessionalSituations;
using ExplorJobAPI.Domain.Repositories.UserReporting;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Domain.Services.Account;
using ExplorJobAPI.Domain.Services.Admin;
using ExplorJobAPI.Domain.Services.AuthUsers;
using ExplorJobAPI.Domain.Services.Emails;
using ExplorJobAPI.Domain.Services.Messaging;
using ExplorJobAPI.Domain.Services.Users;
using ExplorJobAPI.Infrastructure.Models.Emails;
using ExplorJobAPI.Infrastructure.Services.Emails;
using ExplorJobAPI.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ExplorJobAPI.Infrastructure.Images.Services;
using ExplorJobAPI.Infrastructure.Strings.Services;
using ExplorJobAPI.Infrastructure.Http.Services;
using ExplorJobAPI.DAL.Mappers.AccountUsers;
using Hangfire;
using Hangfire.Pro.Redis;
using ExplorJobAPI.Middlewares.JobsScheduler.Services;
using ExplorJobAPI.Middlewares.JobsScheduler.Jobs;
using ExplorJobAPI.Auth.Filters;
using static ExplorJobAPI.Config;
using Newtonsoft.Json;
using ExplorJobAPI.Domain.Repositories.Agglomerations;
using ExplorJobAPI.DAL.Repositories.Agglomerations;
using ExplorJobAPI.DAL.Mappers.Agglomerations;
using ExplorJobAPI.DAL.Mappers.Companies;
using ExplorJobAPI.DAL.Mappers.Offers;
using ExplorJobAPI.DAL.Mappers.KeywordLists;
using ExplorJobAPI.Domain.Repositories.Companies;
using ExplorJobAPI.Domain.Repositories.Offers;
using ExplorJobAPI.Domain.Repositories.KeywordLists;
using ExplorJobAPI.DAL.Repositories.Companies;
using ExplorJobAPI.DAL.Repositories.Offers;
using ExplorJobAPI.DAL.Repositories.KeywordLists;

namespace ExplorJobAPI
{
    public class Startup
    {
        private readonly string _corsPolicy = "CorsPolicy";

        public Startup(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
            Config.AdminSecretKey = Configuration["Admin:SecretKey"];
            Config.Authority = Configuration["Auth:Authority"];
            Config.AuthAudience = Configuration["Auth:Audience"];
            Config.AuthIssuer = Configuration["Auth:Issuer"];
            Config.AuthSigningSecureKey = Configuration["Auth:Signing:SecureKey"];
            Config.CertPassword = Configuration["Cert:Password"];
            Config.ConnectionString = Configuration["Persistence:Pgsql:ConnectionString"];
            Config.ApiPublicRootUrl = Configuration["Api:Public:RootUrl"];
            Config.EmailsSender = new EmailsSenderConfig(
                Configuration["SMTP:Username"],
                Configuration["SMTP:Password"]
            );
            Config.Urls = new UrlsConfig
            {
                ExplorJobHost = Configuration["App:Urls:ExplorJob:Host"],
                ExplorJobLogo = Configuration["App:Urls:ExplorJob:Logo"],
                AccountMessaging = Configuration["App:Urls:Account:Messaging"],
                MailingFooter = Configuration["App:Urls:Mailing:Footer"]
            };
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(
            IServiceCollection services
        )
        {
            #region Init

            #region Persistence

            services.AddDbContext<ExplorJobDbContext>((options) =>
            {
                options.UseNpgsql(
                    Config.ConnectionString,
                    (builder) =>
                    {
                        builder.EnableRetryOnFailure(
                            5,
                            TimeSpan.FromSeconds(10),
                            null
                        );
                    }
                );
            });

            #endregion

            #region API

            services.AddCors((options) =>
            {
                options.AddPolicy(
                    _corsPolicy,
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });

            services.AddHealthChecks();

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddMvc((options) =>
            {
                options.RequireHttpsPermanent = true;
                options.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc(
                    Config.Version,
                    new OpenApiInfo
                    {
                        Title = Config.Name,
                        Version = Config.Version
                    }
                );
            });

            #endregion

            #region Resources

            services.Configure<FormOptions>((options) =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            #endregion

            #region Auth

            services
                .AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<ExplorJobDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication((options) =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer((options) =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidIssuer = Config.AuthIssuer,
                        ValidAudience = Config.AuthAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                Config.AuthSigningSecureKey
                            )
                        )
                    };
                });

            services.Configure<IdentityOptions>((options) =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddAuthorization((options) =>
            {
                options.AddPolicy("Administrators", policy => policy.RequireRole(Roles.Administrator));
                options.AddPolicy("Employees", policy => policy.RequireRole(Roles.Employee));
                options.AddPolicy("Users", policy => policy.RequireRole(Roles.User));
                options.AddPolicy("UserId", policy => policy.RequireClaim("UserId"));
                options.AddPolicy("UserEmail", policy => policy.RequireClaim("UserEmail"));
            });

            #endregion

            #region Scheduler

            services.AddHangfire((config) =>
            {
                config
                    .SetDataCompatibilityLevel(
                        CompatibilityLevel.Version_170
                    )
                    .UseColouredConsoleLogProvider()
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseRedisStorage(
                        "localhost:6379",
                        new RedisStorageOptions
                        {
                            Prefix = "explorjob:scheduler:",
                            InvisibilityTimeout = TimeSpan.FromHours(3)
                        }
                    );
            });

            #endregion

            #endregion

            #region Injection

            #region Repositories

            services.AddScoped<IUserDegreesRepository, UserDegreesRepository>();
            services.AddScoped<IUserProfessionalSituationsRepository, UserProfessionalSituationsRepository>();
            services.AddScoped<IUserReportingReasonsRepository, UserReportingReasonsRepository>();
            services.AddScoped<IUserReportedRepository, UserReportedRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserContactMethodsRepository, UserContactMethodsRepository>();
            services.AddScoped<IUserContactInformationsRepository, UserContactInformationsRepository>();
            services.AddScoped<IUserFavoritesRepository, UserFavoritesRepository>();
            services.AddScoped<IUserMeetingsRepository, UserMeetingsRepository>();
            services.AddScoped<IContractsRepository, ContractsRepository>();
            services.AddScoped<IContractUserAcceptancesRepository, ContractUserAcceptancesRepository>();
            services.AddScoped<IConversationsRepository, ConversationsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<INotificationsRepository, NotificationsRepository>();
            services.AddScoped<IJobDomainsRepository, JobDomainsRepository>();
            services.AddScoped<IJobUsersRepository, JobUsersRepository>();
            services.AddScoped<IJobSearchesRepository, JobSearchesRepository>();
            services.AddScoped<IAccountUsersRepository, AccountUsersRepository>();
            services.AddScoped<IAgglomerationsRepository, AgglomerationsRepository>();
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            services.AddScoped<IOfferTypesRepository, OfferTypesRepository>();
            services.AddScoped<IOfferSubscriptionsRepository, OfferSubscriptionsRepository>();
            services.AddScoped<IKeywordListsRepository, KeywordListsRepository>();

            #endregion

            #region Mappers

            services.AddScoped<AccountUserMapper, AccountUserMapper>();
            services.AddScoped<UserDegreeMapper, UserDegreeMapper>();
            services.AddScoped<UserProfessionalSituationMapper, UserProfessionalSituationMapper>();
            services.AddScoped<UserReportingReasonMapper, UserReportingReasonMapper>();
            services.AddScoped<UserReportedMapper, UserReportedMapper>();
            services.AddScoped<UserMapper, UserMapper>();
            services.AddScoped<UserContactMethodMapper, UserContactMethodMapper>();
            services.AddScoped<UserContactInformationMapper, UserContactInformationMapper>();
            services.AddScoped<UserFavoriteMapper, UserFavoriteMapper>();
            services.AddScoped<UserMeetingMapper, UserMeetingMapper>();
            services.AddScoped<ContractMapper, ContractMapper>();
            services.AddScoped<ContractUserAcceptanceMapper, ContractUserAcceptanceMapper>();
            services.AddScoped<ConversationMapper, ConversationMapper>();
            services.AddScoped<MessageMapper, MessageMapper>();
            services.AddScoped<NotificationMapper, NotificationMapper>();
            services.AddScoped<JobDomainMapper, JobDomainMapper>();
            services.AddScoped<JobUserMapper, JobUserMapper>();
            services.AddScoped<AgglomerationMapper, AgglomerationMapper>();
            services.AddScoped<CompanyMapper, CompanyMapper>();
            services.AddScoped<OfferSubscriptionMapper, OfferSubscriptionMapper>();
            services.AddScoped<KeywordListMapper, KeywordListMapper>();

            #endregion

            #region Services

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAuthUsersService, AuthUsersService>();
            services.AddScoped<IAccountUserPhotoService, AccountUserPhotoService>();
            services.AddScoped<IUserPhotoService, UserPhotoService>();
            services.AddScoped<ISendEmailService, EmailsSenderService>();
            services.AddScoped<ISendMessageService, SendMessageService>();

            #endregion

            #region Infrastructure

            services.AddScoped<UrlEncoder, UrlEncoder>();
            services.AddScoped<ImagesManipulator, ImagesManipulator>();
            services.AddScoped<StringsManipulator, StringsManipulator>();

            #endregion

            #region Jobs Scheduled

            services.AddScoped<IUserHaveNewConversationsOrUnreadMessagesTodayJob, UserHaveNewConversationsOrUnreadMessagesTodayJob>();

            #endregion

            #endregion
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI((options) =>
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{ Config.Version }/swagger.json",
                        Config.Name
                    );
                    options.RoutePrefix = "swagger";
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHangfireDashboard(
                "/hangfire",
                env.IsDevelopment()
                ? new DashboardOptions { }
                : new DashboardOptions
                {
                    Authorization = new[] {
                        new HangfireDashboardAuthorizationFilter()
                    }
                }
            );

            app.UseMiddleware<IpSafeListMiddleware>(
                Configuration["IpSafeList"]
            );

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        @Config.Folders.Resources
                    )
                ),
                RequestPath = new PathString(
                    $"/{ Config.Folders.Resources }"
                )
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(
                _corsPolicy
            );

            app.UseEndpoints((endpoints) =>
            {
                endpoints.MapHealthChecks(
                    "/health",
                    new HealthCheckOptions { }
                );
                endpoints.MapControllers();
            });

            app.UseHangfireServer(
                new BackgroundJobServerOptions
                {
                    ServerName = "JobsScheduler",
                    WorkerCount = 4
                }
            );

            GlobalJobFilters.Filters.Add(
                new AutomaticRetryAttribute { Attempts = 0 }
            );

            if (env.IsProduction()
            || Config.ActiveDevScheduler
            )
            {
                JobsScheduler.ScheduleRecurringJobs();
            }
        }
    }
}
