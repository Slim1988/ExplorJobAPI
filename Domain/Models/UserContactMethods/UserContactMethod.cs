using System;
using ExplorJobAPI.Domain.Dto.UserContactMethods;

namespace ExplorJobAPI.Domain.Models.UserContactMethods
{
    public class UserContactMethod
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public UserContactMethod(
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

        public UserContactMethodDto ToDto() {
            return new UserContactMethodDto(
                Id.ToString(),
                Label
            );
        }
    }
}
