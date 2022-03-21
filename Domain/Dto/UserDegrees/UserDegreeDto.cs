namespace ExplorJobAPI.Domain.Dto.UserDegrees
{
    public class UserDegreeDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public UserDegreeDto(
            string id,
            string label
        ) {
            Id = id;
            Label = label;
        }
    }
}
