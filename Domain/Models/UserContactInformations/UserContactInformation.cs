using System;
using ExplorJobAPI.Domain.Dto.UserContactInformations;

namespace ExplorJobAPI.Domain.Models.UserContactInformations
{
    public class UserContactInformation
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public UserContactInformation(
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

        public UserContactInformationDto ToDto() {
            return new UserContactInformationDto(
                Id.ToString(),
                Label
            );
        }
    }
}
