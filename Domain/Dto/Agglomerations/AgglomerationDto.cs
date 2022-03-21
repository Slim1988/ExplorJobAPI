using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Dto.Agglomerations
{
    public class AgglomerationDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public List<string> ZipCodes { get; set; }

        public AgglomerationDto(
            string id,
            string label,
            List<string> zipCodes
        ) {
            Id = id;
            Label = label;
            ZipCodes = zipCodes;
        }
    }
}
