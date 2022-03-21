using System;
using ExplorJobAPI.Domain.Dto.UserDegrees;

namespace ExplorJobAPI.Domain.Models.UserDegrees
{
    public class UserDegree
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public UserDegree(
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

        public UserDegreeDto ToDto() {
            return new UserDegreeDto(
                Id.ToString(),
                Label
            );
        }
    }
}
