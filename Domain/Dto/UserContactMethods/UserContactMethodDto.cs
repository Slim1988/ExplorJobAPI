namespace ExplorJobAPI.Domain.Dto.UserContactMethods
{
    public class UserContactMethodDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public UserContactMethodDto(
            string id,
            string label
        ) {
            Id = id;
            Label = label;
        }
    }
}
