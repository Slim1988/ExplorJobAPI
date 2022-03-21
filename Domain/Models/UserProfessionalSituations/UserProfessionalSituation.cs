using System;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;

namespace ExplorJobAPI.Domain.Models.UserProfessionalSituations
{
    public class UserProfessionalSituation
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public UserProfessionalSituation(
            Guid id,
            string label,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Label = label;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public UserProfessionalSituationDto ToDto() {
            return new UserProfessionalSituationDto(
                Id.ToString(),
                Label
            );
        }
    }
}
