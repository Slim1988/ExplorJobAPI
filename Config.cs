using System.Collections.Generic;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Infrastructure.Models.Emails;

namespace ExplorJobAPI
{
    public static class Config
    {
        public static string Name = "ExplorJobAPI";
        public static string Version = "2.1.0";
        public static string Port = "9000";
        public static string AdminSecretKey { get; set; }
        public static string Authority { get; set; }
        public static string AuthAudience { get; set; }
        public static string AuthIssuer { get; set; }
        public static string AuthSigningSecureKey { get; set; }
        public static string CertPassword { get; set; }
        public static string ConnectionString { get; set; }
        public static string ApiPublicRootUrl { get; set; }
        public static bool ActiveDevScheduler { get; set; } = false;
        public static EmailsAddresses Emails {  get; set; } = new EmailsAddresses();
        public static FoldersPaths Folders { get; set; } = new FoldersPaths();
        public static UserPhotoConfig UserPhoto = new UserPhotoConfig();
        public static EmailsSenderConfig EmailsSender { get; set; }
        public static UrlsConfig Urls { get; set; }

        public class EmailsAddresses {
            public List<string> Admins = new List<string>() {
                "marion@explorjob.com"
            };

            public string Dev = "dev@eteon.io";
        }

        public class FoldersPaths {
            public string Resources = "Resources";
            public string Images = "images";
            public string Users = "users";
            public string Photos = "photos";
        }

        public class UrlsConfig {
            public string ExplorJobHost { get; set; }
            public string ExplorJobLogo { get; set; }
            public string AccountMessaging { get; set; }
            public string MailingFooter { get; set; }
        }
    }
}
