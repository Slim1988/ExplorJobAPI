namespace ExplorJobAPI.Domain.Dto.UserProfessionalSituations
{
    public class UserProfessionalSituationDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public UserProfessionalSituationDto(
            string id,
            string label
        ) {
            Id = id;
            Label = label;
        }
    }
}
