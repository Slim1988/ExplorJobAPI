using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.Agglomerations;

namespace ExplorJobAPI.Domain.Models.Agglomerations
{
    public class Agglomeration
    {   
        public Guid Id { get; set; }
        public string Label { get; set; }
        public List<string> ZipCodes { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Agglomeration(
            Guid id,
            string label,
            List<string> zipCodes,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Label = label;
            ZipCodes = zipCodes;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public AgglomerationDto ToDto() {
            return new AgglomerationDto(
                Id.ToString(),
                Label,
                ZipCodes
            );
        }
    }
}
