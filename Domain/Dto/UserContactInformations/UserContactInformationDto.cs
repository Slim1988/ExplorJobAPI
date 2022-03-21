namespace ExplorJobAPI.Domain.Dto.UserContactInformations
{
    public class UserContactInformationDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public UserContactInformationDto(
            string id,
            string label
        ) {
            Id = id;
            Label = label;
        }
    }
}
