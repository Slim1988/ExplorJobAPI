using System.Linq;
using System;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.DAL.Mappers.UserDegrees;
using ExplorJobAPI.DAL.Mappers.UserProfessionalSituations;
using ExplorJobAPI.DAL.Mappers.UserContactInformations;
using ExplorJobAPI.DAL.Mappers.UserContactMethods;
using ExplorJobAPI.Domain.Commands.Users;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Domain.Commands.AuthUsers;

namespace ExplorJobAPI.DAL.Mappers.Users
{
    public class UserMapper
    {
        private readonly UserDegreeMapper _degreeMapper;
        private readonly UserProfessionalSituationMapper _professionalSituationMapper;
        private readonly UserContactMethodMapper _userContactMethodMapper;
        private readonly UserContactInformationMapper _userContactInformationMapper;

        public UserMapper(
            UserDegreeMapper degreeMapper,
            UserProfessionalSituationMapper professionalSituationMapper,
            UserContactMethodMapper userContactMethodMapper,
            UserContactInformationMapper userContactInformationMapper
        )
        {
            _degreeMapper = degreeMapper;
            _professionalSituationMapper = professionalSituationMapper;
            _userContactMethodMapper = userContactMethodMapper;
            _userContactInformationMapper = userContactInformationMapper;
        }

        public User EntityToDomainModel(
            UserEntity entity
        )
        {
            return new User(
                entity.Guid,
                entity.Email,
                entity.FirstName,
                entity.LastName,
                entity.BirthDate,
                entity.Phone,
                new UserAddress(
                    entity.AddressStreet,
                    entity.AddressComplement,
                    entity.AddressZipCode,
                    entity.AddressCity
                ),
                entity.Presentation,
                entity.IsProfessional,
                entity.LastDegree != null
                    ? _degreeMapper.EntityToDomainModel(
                        entity.LastDegree
                    )
                    : null,
                entity.ProfessionalSituation != null
                    ? _professionalSituationMapper.EntityToDomainModel(
                        entity.ProfessionalSituation
                    )
                    : null,
                entity.CurrentCompany,
                entity.CurrentSchool,
                entity.ContactMethodJoins.Select(
                    (join) => _userContactMethodMapper.EntityToDomainModel(
                        join.UserContactMethod
                    )
                ).ToList(),
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public UserPublicDto EntityToDomainPublicDto(
            UserEntity entity
        )
        {
            return new UserPublicDto(
                entity.Guid.ToString(),
                User.PhotoUrl(entity.Guid),
                entity.FirstName,
                entity.LastName,
                entity.AddressCity
            );
        }

        public UserRestrictedDto EntityToDomainRestrictedDto(
            UserEntity entity
        )
        {
            return new UserRestrictedDto(
                entity.Guid.ToString(),
                User.PhotoUrl(entity.Guid),
                entity.FirstName,
                entity.LastName,
                entity.BirthDate,
                entity.AddressCity,
                entity.Presentation,
                entity.IsProfessional,
                entity.LastDegree != null
                    ? _degreeMapper.EntityToDomainDto(
                        entity.LastDegree
                    )
                    : null,
                entity.ProfessionalSituation != null
                    ? _professionalSituationMapper.EntityToDomainDto(
                        entity.ProfessionalSituation
                    )
                    : null,
                entity.CurrentCompany,
                entity.CurrentSchool,
                entity.ContactMethodJoins.Select(
                    (join) => _userContactMethodMapper.EntityToDomainDto(
                        join.UserContactMethod
                    )
                ).ToList()
            );
        }

        public UserDto EntityToDomainDto(
            UserEntity entity
        )
        {
            return new UserDto(
                entity.Guid.ToString(),
                User.PhotoUrl(entity.Guid),
                entity.Email,
                entity.FirstName,
                entity.LastName,
                entity.BirthDate,
                entity.Phone,
                new UserAddress(
                    entity.AddressStreet,
                    entity.AddressComplement,
                    entity.AddressZipCode,
                    entity.AddressCity
                ),
                entity.Presentation,
                entity.IsProfessional,
                entity.LastDegree != null
                    ? _degreeMapper.EntityToDomainDto(
                        entity.LastDegree
                    )
                    : null,
                entity.ProfessionalSituation != null
                    ? _professionalSituationMapper.EntityToDomainDto(
                        entity.ProfessionalSituation
                    )
                    : null,
                entity.CurrentCompany,
                entity.CurrentSchool,
                entity.ContactMethodJoins.Select(
                    (join) => _userContactMethodMapper.EntityToDomainDto(
                        join.UserContactMethod
                    )
                ).ToList()
            );
        }

        public UserEntity RegisterCommandToEntity(
            UserRegisterCommand command
        )
        {
            return new UserEntity
            {
                UserName = command.Email,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                BirthDate = command.BirthDate,
                IsProfessional = command.IsProfessional,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn,
                AddressZipCode = command.ZipCode
            };
        }

        public UserEntity IsProfessionalCommandToEntity(
            UserIsProfessionalCommand command,
            UserEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Guid))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.IsProfessional = command.IsProfessional;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserEntity UpdateGeneralInformationsCommandToEntity(
            UserGeneralInformationsUpdateCommand command,
            UserEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Guid))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.UserName = command.Email;
            entity.Email = command.Email;
            entity.NormalizedEmail = command.Email.ToUpperInvariant();
            entity.FirstName = command.FirstName;
            entity.LastName = command.LastName;
            entity.BirthDate = command.BirthDate;
            entity.Presentation = command.Presentation;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserEntity UpdateContactInformationsCommandToEntity(
            UserContactInformationsUpdateCommand command,
            UserEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Guid))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Phone = command.Phone;
            entity.AddressStreet = command.Address.Street;
            entity.AddressComplement = command.Address.Complement;
            entity.AddressZipCode = command.Address.ZipCode;
            entity.AddressCity = command.Address.City;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserEntity UpdateSituationInformationsCommandToEntity(
            UserSituationInformationsUpdateCommand command,
            UserEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Guid))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.LastDegreeId = command.LastDegreeId != null
                ? new Guid(command.LastDegreeId)
                : (Guid?)null;
            entity.ProfessionalSituationId = command.ProfessionalSituationId != null
                ? new Guid(command.ProfessionalSituationId)
                : (Guid?)null;
            entity.CurrentCompany = command.CurrentCompany;
            entity.CurrentSchool = command.CurrentSchool;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserEntity UpdateCommandToEntity(
            UserUpdateCommand command,
            UserEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Guid))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.UserName = command.Email;
            entity.Email = command.Email;
            entity.NormalizedEmail = command.Email.ToUpperInvariant();
            entity.FirstName = command.FirstName;
            entity.LastName = command.LastName;
            entity.BirthDate = command.BirthDate;
            entity.Phone = command.Phone;
            entity.AddressStreet = command.Address.Street;
            entity.AddressComplement = command.Address.Complement;
            entity.AddressZipCode = command.Address.ZipCode;
            entity.AddressCity = command.Address.City;
            entity.Presentation = command.Presentation;
            entity.IsProfessional = command.IsProfessional;
            entity.LastDegreeId = command.LastDegreeId != null
                ? new Guid(command.LastDegreeId)
                : (Guid?)null;
            entity.ProfessionalSituationId = command.ProfessionalSituationId != null
                ? new Guid(command.ProfessionalSituationId)
                : (Guid?)null;
            entity.CurrentCompany = command.CurrentCompany;
            entity.CurrentSchool = command.CurrentSchool;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserEntity DeleteCommandToEntity(
            UserDeleteCommand command,
            UserEntity entity
        )
        {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Guid))
            {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
