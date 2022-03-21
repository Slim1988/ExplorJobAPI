using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Domain.Models.UserDegrees;
using ExplorJobAPI.Domain.Models.UserProfessionalSituations;
using ExplorJobAPI.Domain.Models.UserContactInformations;
using ExplorJobAPI.Domain.Models.UserContactMethods;
using System.IO;

namespace ExplorJobAPI.Domain.Models.Users
{
    public class User
    {   
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public UserAddress Address { get; set; }
        public string SkypeId { get; set; }
        public string Presentation { get; set; }
        public bool IsProfessional { get; private set; }
        public UserDegree LastDegree { get; set; }
        public UserProfessionalSituation ProfessionalSituation { get; set; }
        public string CurrentCompany { get; set; }
        public string CurrentSchool { get; set; }
        public List<UserContactMethod> ContactMethods { get; set; }
        public List<UserContactInformation> ContactInformations { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public User(
            Guid id,
            string email,
            string firstName,
            string lastName,
            DateTime? birthDate,
            string phone,
            UserAddress address,
            string presentation,
            bool isProfessional,
            UserDegree lastDegree,
            UserProfessionalSituation professionalSituation,
            string currentCompany,
            string currentSchool,
            List<UserContactMethod> contactMethods,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Phone = phone;
            Address = address;
            Presentation = presentation;
            IsProfessional = isProfessional;
            LastDegree = lastDegree;
            ProfessionalSituation = professionalSituation;
            CurrentCompany = currentCompany;
            CurrentSchool = currentSchool;
            ContactMethods = contactMethods;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public string PhotoUrl() {
            return User.PhotoUrl(
                this.Id.ToString()
            );
        }

        public static string PhotoUrl(
            Guid userId
        ) {
            return User.PhotoUrl(
                userId.ToString()
            );
        }

        public static string PhotoUrl(
            string userId
        ) {
            var filePath = Directory.Exists(Config.UserPhoto.Folder)
                ? Directory.GetFiles(
                    Config.UserPhoto.Folder
                ).ToList().Find(
                    (string directoryFilePath) => Regex.IsMatch(
                        directoryFilePath,
                        userId,
                        RegexOptions.IgnoreCase
                    ) && Regex.IsMatch(
                        directoryFilePath,
                        Config.UserPhoto.Size125x125.Label,
                        RegexOptions.IgnoreCase
                    )
                )
                : null;

            return filePath != null
                ? $"{ Config.ApiPublicRootUrl }/{ filePath }"
                : null;
        }

        public bool HasAlreadyBeenUpdated() {
            return UpdatedOn > CreatedOn;
        }

        public UserPublicDto ToPublicDto() {
            return new UserPublicDto(
                Id.ToString(),
                PhotoUrl(),
                FirstName,
                LastName,
                Address != null
                    ? Address.City
                    : null
            );
        }

        public UserRestrictedDto ToRestrictedDto() {
            return new UserRestrictedDto(
                Id.ToString(),
                PhotoUrl(),
                FirstName,
                LastName,
                BirthDate,
                Address != null
                    ? Address.City
                    : null,
                Presentation,
                IsProfessional,
                LastDegree != null
                    ? LastDegree.ToDto()
                    : null,
                ProfessionalSituation != null
                    ? ProfessionalSituation.ToDto()
                    : null,
                CurrentCompany,
                CurrentSchool,
                ContactMethods.Select(
                    (contactMethod) => contactMethod.ToDto()
                ).ToList()
            );
        }

        public UserDto ToDto() {
            return new UserDto(
                Id.ToString(),
                PhotoUrl(),
                Email,
                FirstName,
                LastName,
                BirthDate,
                Phone,
                Address,
                Presentation,
                IsProfessional,
                LastDegree != null
                    ? LastDegree.ToDto()
                    : null,
                ProfessionalSituation != null
                    ? ProfessionalSituation.ToDto()
                    : null,
                CurrentCompany,
                CurrentSchool,
                ContactMethods.Select(
                    (contactMethod) => contactMethod.ToDto()
                ).ToList()
            );
        }
    }
}
