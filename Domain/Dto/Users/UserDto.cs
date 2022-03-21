using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.UserDegrees;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;
using ExplorJobAPI.Domain.Dto.UserContactInformations;
using ExplorJobAPI.Domain.Dto.UserContactMethods;
using ExplorJobAPI.Domain.Models.Users;

namespace ExplorJobAPI.Domain.Dto.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public UserAddress Address { get; set; }
        public string Presentation { get; set; }
        public bool IsProfessional { get; set; }
        public UserDegreeDto LastDegree { get; set; }
        public UserProfessionalSituationDto ProfessionalSituation { get; set; }
        public string CurrentCompany { get; set; }
        public string CurrentSchool { get; set; }
        public List<UserContactMethodDto> ContactMethods { get; set; }

        public UserDto(
            string id,
            string photoUrl,
            string email,
            string firstName,
            string lastName,
            DateTime? birthDate,
            string phone,
            UserAddress address,
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
        }
    }
}
