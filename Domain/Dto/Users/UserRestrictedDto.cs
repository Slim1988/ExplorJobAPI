using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.UserDegrees;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;
using ExplorJobAPI.Domain.Dto.UserContactMethods;
using ExplorJobAPI.Domain.Dto.UserContactInformations;

namespace ExplorJobAPI.Domain.Dto.Users
{
    public class UserRestrictedDto
    {
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string LocalisationCity { get; set; }
        public string Presentation { get; set; }
        public bool IsProfessional { get; set; }
        public UserDegreeDto LastDegree { get; set; }
        public UserProfessionalSituationDto ProfessionalSituation { get; set; }
        public string CurrentCompany { get; set; }
        public string CurrentSchool { get; set; }
        public List<UserContactMethodDto> ContactMethods { get; set; }

        public UserRestrictedDto(
            string id,
            string photoUrl,
            string firstName,
            string lastName,
            DateTime? birthDate,
            string localisationCity,
            string presentation,
            bool isProfessional,
            UserDegreeDto lastDegree,
            UserProfessionalSituationDto professionalSituation,
            string currentCompany,
            string currentSchool,
            List<UserContactMethodDto> contactMethods
        ) {
            Id = id;
            PhotoUrl = photoUrl;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            LocalisationCity = localisationCity;
            Presentation = presentation;
            IsProfessional = isProfessional;
            LastDegree = lastDegree;
            ProfessionalSituation = professionalSituation;
            CurrentCompany = currentCompany;
            CurrentSchool = currentSchool;
            ContactMethods = contactMethods;
        }
    }
}
